using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniTwit.Core.Responses;
using MiniTwit.Core.DTOs;
using MiniTwit.Service;
using MiniTwit.Core.Error;

namespace MiniTwit.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class SimController : ControllerBase
{
    private readonly IServiceManager _serviceManager;
    private readonly ILogger<SimController> _logger;

    public SimController(IServiceManager serviceManager, ILogger<SimController> logger)
    {
        _serviceManager = serviceManager;
        _logger = logger;
    }

    /// <summary>
    /// Get the latest value
    /// <summary>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Route("latest")]
    public async Task<ActionResult<LatestDTO>> Latest(CancellationToken ct = default)
    {
        var response = await _serviceManager.LatestService.GetAsync(ct);
        return response.ToActionResult();
    }

    /// <summary>
    /// Registers the user
    /// <summary>
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Route("register")]
    public async Task<ActionResult> Register(
        [FromBody] UserCreateDTO userCreateDTO, 
        [FromQuery] int latest = -1)
    {
        await UpdateLatestAsync(latest);

        var response = await _serviceManager.UserService.CreateAsync(userCreateDTO);
        return response.ToActionResult();
    }

    /// <summary>
    /// Gets all the messages 
    /// <summary>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Route("msgs")]
    public Task<ActionResult<IEnumerable<MessageDetailsDTO>>> Msgs([FromHeader(Name = "Authorization")] string auth, [FromQuery] int no = 100, [FromQuery] int latest = -1, CancellationToken ct = default)
    {
        await UpdateLatestAsync(latest);

        if (!IsAuthorized(auth))
        {
            return Forbidden();
        }

        var messages = (await _serviceManager.MessageService.GetAllNonFlaggedAsync(ct)).Model!.ToList().Take(no);
        var messageDTOList = new List<MessageDetailsDTO>();

        foreach (var message in messages)
        {
            var user = (await _serviceManager.UserService.GetByUserIdAsync(message.AuthorId!)).Model;

            var dto = new MessageDetailsDTO
            {
                Content = message.Text,
                Username = user!.Username,
                PubDate = message.PubDate
            };
            messageDTOList.Add(dto);
        }

        return Ok(messageDTOList.OrderByDescending(m => m.PubDate));
    }

    /// <summary>
    /// Gets all the messages 
    /// <summary>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Route("msgs/{username}")]
    public Task<ActionResult<IEnumerable<MessageDetailsDTO>>> MsgUsername(string username, [FromHeader(Name = "Authorization")] string auth, [FromQuery] int no = 100, [FromQuery] int latest = -1, CancellationToken ct = default)
    {
        await UpdateLatestAsync(latest);

        if (!IsAuthorized(auth))
        {
            return Forbidden();
        }

        var response = await _serviceManager.MessageService.GetAllNonFlaggedByUsernameAsync(username, ct);

        if (response.HTTPResponse == HTTPResponse.NotFound)
        {
            return NotFound();
        }

        var reponseList = response.Model!;

        var messageDTOList = new List<MessageDetailsDTO>();

        foreach (var message in reponseList)
        {
            var userResponse = await _serviceManager.UserService.GetByUserIdAsync(message.AuthorId!, ct);
            var user = userResponse.Model;

            var dto = new MessageDetailsDTO()
            {
                Content = message.Text,
                Username = user!.Username,
                PubDate = message.PubDate
            };
            messageDTOList.Add(dto);
        }

        return Ok(messageDTOList.Take(no));
    }

    /// <summary>
    /// Add message by username
    /// <summary>
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Route("msgs/{username}")]
    public async Task<ActionResult> MsgUsernamePost(string username, [FromHeader(Name = "Authorization")] string auth, [FromBody] MessageCreateDTO messageCreateDTO, [FromQuery] int latest = -1)
    {
        await UpdateLatestAsync(latest);

        if (!IsAuthorized(auth))
        {
            return Forbidden();
        }

        var response = await _serviceManager.MessageService.CreateByUsernameAsync(username, messageCreateDTO.Content!);
        return response.ToActionResult();
    }

    /// <summary>
    /// Get followers by username
    /// <summary>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Route("fllws/{username}")]
    public async Task<ActionResult<FollowerDetailsDTO>> FollowUser(string username, [FromHeader(Name = "Authorization")] string auth, [FromQuery] int latest = -1, [FromQuery] int no = 100, CancellationToken ct = default)
    {
        await UpdateLatestAsync(latest);

        if (!IsAuthorized(auth))
        {
            return Forbidden();
        }

        var followers = await _serviceManager.FollowerService.GetAllFollowersByUsernameAsync(username, ct);

        if (followers.HTTPResponse == HTTPResponse.NotFound)
        {
            return NotFound();
        }

        var followersDTOs = new FollowerDetailsDTO();

        foreach (var follower in followers.Model!.Take(no))
        {
            var user = await _serviceManager.UserService.GetByUserIdAsync(follower.WhoId!, ct);
            followersDTOs.Follows.Add(user.Model!.Username!);
        }

        return Ok(followersDTOs);
    }

    /// <summary>
    /// Follow user by username
    /// <summary>
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Route("fllws/{username}")]
    public Task<ActionResult> FollowUser(string username, [FromHeader(Name = "Authorization")] string auth, [FromBody] FollowerCreateDTO followerCreateDTO, [FromQuery] int latest = -1)
    {
        await UpdateLatestAsync(latest);

        if (!IsAuthorized(auth))
        {
            return Forbidden();
        }

        var userResponse = await _serviceManager.UserService.GetByUsernameAsync(username);

        if (userResponse.HTTPResponse == HTTPResponse.BadRequest)
        {
            return NotFound();
        }

        // If Follow is not null make a follow
        if (followerCreateDTO.Follow is not null)
        {
            var followResponse = await _serviceManager.FollowerService.CreateAsync(userResponse.Model!.Id!, username);

            if (followResponse.HTTPResponse == HTTPResponse.NotFound)
            {
                return NotFound();
            }

            return NoContent();
        }
        else
        {
            var unfollowResponse = await _serviceManager.FollowerService.DeleteAsync(userResponse.Model!.Id!, username);

            if (unfollowResponse.HTTPResponse == HTTPResponse.NotFound)
            {
                return NotFound();
            }

            return NoContent();
        }
    }

    private ActionResult Forbidden()
    {
        return StatusCode(403, new APIError { Status = 403, ErrorMsg = "You are not authorized to use this resource!" });
    }

    private void UpdateLatest(int latestVal)
    {
        _serviceManager.LatestService.Update(latestVal);
    }

    private async Task UpdateLatestAsync(int latestVal)
    {
        await _serviceManager.LatestService.UpdateAsync(latestVal);
    }

    private bool IsAuthorized(string authHeader)
    {
        return authHeader == "Basic c2ltdWxhdG9yOnN1cGVyX3NhZmUh";
    }
}

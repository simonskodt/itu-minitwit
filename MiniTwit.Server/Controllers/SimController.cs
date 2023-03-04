using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniTwit.Core.Responses;
using MiniTwit.Core.DTOs;
using MiniTwit.Service;

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
    public ActionResult<LatestDTO> Latest(CancellationToken ct = default)
    {
        var response = _serviceManager.LatestService.Get(ct);
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
    public ActionResult Register([FromBody] UserCreateDTO userCreateDTO, [FromQuery] int latest = -1)
    {
        UpdateLatest(latest);

        if (userCreateDTO.Username! == "")
        {
            return BadRequest(new { status = 400, error_msg = "You have to enter a username" });
        }
        else if (!userCreateDTO.Email!.Contains("@"))
        {
            return BadRequest(new { status = 400, error_msg = "You have to enter a valid email address" });
        }
        else if (userCreateDTO.Password == "")
        {
            return BadRequest(new { status = 400, error_msg = "You have to enter a password" });
        }

        var response = _serviceManager.UserService.Create(userCreateDTO);

        if (response.HTTPResponse == HTTPResponse.Conflict)
        {
            return BadRequest(new { status = 400, error_msg = "The username is already taken" });
        }

        return NoContent();
    }

    /// <summary>
    /// Gets all the messages 
    /// <summary>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Route("msgs")]
    public ActionResult<IEnumerable<MessageDetailsDTO>> Msgs([FromHeader(Name = "Authorization")] string auth, [FromQuery] int no, [FromQuery] int latest = -1, CancellationToken ct = default)
    {
        UpdateLatest(latest);

        var messages = _serviceManager.MessageService.GetAllNonFlagged(ct).Model!.ToList();
        var messageDTOList = new List<MessageDetailsDTO>();

        foreach (var message in messages)
        {
            var user = _serviceManager.UserService.GetByUserId(message.AuthorId!).Model;

            var dto = new MessageDetailsDTO
            {
                Content = message.Text,
                Username = user!.Username,
                PubDate = message.PubDate
            };
            messageDTOList.Add(dto);
        }

        return Ok(messageDTOList.OrderByDescending(m => m.PubDate).Take(no));
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
    public ActionResult<IEnumerable<MessageDetailsDTO>> MsgUsername(string username, [FromHeader(Name = "Authorization")] string auth, [FromQuery] int no, [FromQuery] int latest = -1, CancellationToken ct = default)
    {
        UpdateLatest(latest);

        var response = _serviceManager.MessageService.GetAllNonFlaggedByUsername(username, ct);

        if (response.HTTPResponse == HTTPResponse.NotFound)
        {
            return NotFound();
        }

        var reponseList = response.Model!;

        var messageDTOList = new List<MessageDetailsDTO>();

        foreach (var message in reponseList)
        {
            var userResponse = _serviceManager.UserService.GetByUserId(message.AuthorId!, ct);
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
    public ActionResult MsgUsernamePost(string username, [FromHeader(Name = "Authorization")] string auth, [FromBody] MessageCreateDTO messageCreateDTO, [FromQuery] int latest = -1, CancellationToken ct = default)
    {
        UpdateLatest(latest);

        // if (auth != "Basic c2ltdWxhdG9yOnN1cGVyX3NhZmUh")
        // {
        //     _logger.LogInformation("FORBIDDEN");
        //     return StatusCode(403, new { status = 403, error_msg = "You are not authorized to use this resource!" });
        // }

        var user = _serviceManager.UserService.GetByUsername(username, ct).Model!;

        var response = _serviceManager.MessageService.Create(user.Id!, messageCreateDTO.Content!);

        return NoContent();
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
    public ActionResult<FollowerDetailsDTO> FollowUser(string username, [FromHeader(Name = "Authorization")] string auth, [FromQuery] int latest = -1, [FromQuery] int no = 100, CancellationToken ct = default)
    {
        UpdateLatest(latest);

        var followers = _serviceManager.FollowerService.GetAllFollowersByUsername(username, ct);

        if (followers.HTTPResponse == HTTPResponse.NotFound)
        {
            return NotFound();
        }

        var followersDTOs = new FollowerDetailsDTO();

        foreach (var follower in followers.Model!.Take(no))
        {
            var user = _serviceManager.UserService.GetByUserId(follower.WhoId!, ct);
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
    public ActionResult FollowUser(string username, [FromHeader(Name = "Authorization")] string auth, [FromQuery] string userId, [FromBody] FollowerCreateDTO followerCreateDTO, [FromQuery] int latest = -1)
    {
        UpdateLatest(latest);

        // If Follow is not null make a follow
        if (followerCreateDTO.Follow is not null)
        {
            var followResponse = _serviceManager.FollowerService.Create(userId, username);

            if (followResponse.HTTPResponse == HTTPResponse.NotFound)
            {
                return NotFound();
            }

            return NoContent();
        }
        else
        {
            var unfollowResponse = _serviceManager.FollowerService.Delete(userId, username);

            if (unfollowResponse.HTTPResponse == HTTPResponse.NotFound)
            {
                return NotFound();
            }

            return NoContent();
        }
    }

    private void UpdateLatest(int latestVal)
    {
        _serviceManager.LatestService.Update(latestVal);
    }

    private bool IsAuthorized(string authHeader)
    {
        return authHeader == "Basic c2ltdWxhdG9yOnN1cGVyX3NhZmUh";
    }
}

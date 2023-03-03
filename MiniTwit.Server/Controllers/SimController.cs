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
    [ProducesResponseType(typeof(LatestDTO), StatusCodes.Status200OK)]
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
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [Route("register")]
    public ActionResult Register([FromBody] UserCreateDTO userCreateDTO, [FromQuery(Name = "latest")] int? latestVal)
    {
        UpdateLatest(latestVal);

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
    [ProducesResponseType(typeof(IEnumerable<SimMessageDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Route("msgs")]
    public ActionResult<IEnumerable<SimMessageDTO>> Msgs([FromQuery(Name = "latest")] int? latestVal, CancellationToken ct = default)
    {
        UpdateLatest(latestVal);

        var messages = _serviceManager.MessageService.GetAllNonFlagged(ct).Model!.ToList();
        var messageDTOList = new List<SimMessageDTO>();

        foreach (var message in messages)
        {
            var user = _serviceManager.UserService.GetByUserId(message.AuthorId!).Model;

            var dto = new SimMessageDTO
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
    [ProducesResponseType(typeof(IEnumerable<SimMessageDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Route("msgs/{username}")]
    public ActionResult<IEnumerable<SimMessageDTO>> MsgUsername(string username, [FromQuery(Name = "latest")] int? latestVal, CancellationToken ct = default)
    {
        UpdateLatest(latestVal);

        var response = _serviceManager.MessageService.GetAllNonFlaggedByUsername(username, ct);

        if (response.HTTPResponse == HTTPResponse.NotFound)
        {
            return NotFound();
        }

        var reponseList = response.Model!;

        var messageDTOList = new List<SimMessageDTO>();

        foreach (var message in reponseList)
        {
            var userResponse = _serviceManager.UserService.GetByUserId(message.AuthorId!, ct);
            var user = userResponse.Model;

            var dto = new SimMessageDTO()
            {
                Content = message.Text,
                Username = user!.Username,
                PubDate = message.PubDate
            };
            messageDTOList.Add(dto);
        }

        return Ok(messageDTOList);
    }

    /// <summary>
    /// Add message by username
    /// <summary>
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Route("msgs/{username}")]
    public ActionResult MsgUsernamePost(string username, [FromQuery] string text, [FromQuery(Name = "latest")] int? latestVal, CancellationToken ct = default)
    {
        UpdateLatest(latestVal);

        var user = _serviceManager.UserService.GetByUsername(username, ct).Model!;

        var response = _serviceManager.MessageService.Create(user.Id!, text);

        return NoContent();
    }

    /// <summary>
    /// Get followers by username
    /// <summary>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(SimFollowerDetailsDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Route("fllws/{username}")]
    public ActionResult<SimFollowerDetailsDTO> FollowUser(string username, [FromQuery(Name = "latest")] int? latestVal, CancellationToken ct = default)
    {
        UpdateLatest(latestVal);

        var followers = _serviceManager.FollowerService.GetAllFollowersByUsername(username, ct);


        if (followers.HTTPResponse == HTTPResponse.NotFound)
        {
            return NotFound();
        }

        var followersDTOs = new SimFollowerDetailsDTO();


        foreach (var follower in followers.Model!)
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
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [Route("fllws/{username}")]
    public ActionResult FollowUser([FromQuery] string userId, string username, [FromBody] SimFollowerDTO followSim, [FromQuery(Name = "latest")] int? latestMessage)
    {
        UpdateLatest(latestMessage);

        // If followsim.Follow is not null make a follow
        if (followSim.Follow is not null)
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

    private void UpdateLatest(int? latestVal)
    {
        if (latestVal is not null)
        {
            _serviceManager.LatestService.Update((int)latestVal);
        }
    }
}

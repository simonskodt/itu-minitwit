using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniTwit.Core.DTOs;
using MiniTwit.Service;

namespace MiniTwit.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class TwitterController : ControllerBase
{
    private readonly IServiceManager _serviceManager;
    private readonly ILogger<TwitterController> _logger;

    public TwitterController(IServiceManager serviceManager, ILogger<TwitterController> logger)
    {
        _serviceManager = serviceManager;
        _logger = logger;
    }

    /// <summary>
    /// Shows a users timeline or if no user is logged in it will
    /// redirect to the public timeline. This timeline shows the user's
    /// messages as well as all the messages of followed users.
    /// <summary>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<MessageDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Route("/")]
    public ActionResult<IEnumerable<MessageDTO>> Timeline([FromQuery] string userId, CancellationToken ct = default)
    {
        var response = _serviceManager.MessageService.GetAllFollowedByUserId(userId, ct);
        return response.ToActionResult();
    }

    /// <summary>
    /// Displays the latest messages of all users.
    /// <summary>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<MessageDTO>), StatusCodes.Status200OK)]
    [Route("/public")]
    public ActionResult<IEnumerable<MessageDTO>> PublicTimeline(CancellationToken ct = default)
    {
        var response = _serviceManager.MessageService.GetAllNonFlagged(ct);
        return response.ToActionResult();
    }

    /// <summary>
    /// Display's a users tweets.
    /// <summary>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<MessageDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Route("/{username}")]
    public ActionResult<IEnumerable<MessageDTO>> UserTimeline(string username, CancellationToken ct = default)
    {
        var response = _serviceManager.MessageService.GetAllByUsername(username, ct);
        return response.ToActionResult();
    }

    /// <summary>
    /// Adds the current user as follower of the given user.
    /// <summary>
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Route("/{username}/follow")]
    public ActionResult<FollowerDTO> FollowUser([FromQuery] string userId, string username)
    {
        var response = _serviceManager.FollowerService.Create(userId, username);
        return response.ToActionResult();
    }

    /// <summary>
    /// Removes the current user as follower of the given user.
    /// <summary>
    [HttpDelete]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Route("/{username}/unfollow")]
    public ActionResult UnfollowUser([FromQuery] string userId, string username)
    {
        var response = _serviceManager.FollowerService.Delete(userId, username);
        return response.ToActionResult();
    }

    /// <summary>
    /// Registers a new message for the user.
    /// <summary>
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Route("/add_message")]
    public ActionResult<MessageDTO> AddMessage([FromQuery] string userId, [FromQuery] string text)
    {
        var response = _serviceManager.MessageService.Create(userId, text);
        return response.ToActionResult();
    }

    /// <summary>
    /// Logs the user in.
    /// <summary>
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Route("/login")]
    public ActionResult Login([FromBody] LoginDTO loginDTO)
    {
        var response = _serviceManager.AuthenticationService.Authenticate(loginDTO.Username!, loginDTO.Password!);
        return response.ToActionResult();
    }

    /// <summary>
    /// Registers the user.
    /// <summary>
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [Route("/register")]
    public ActionResult<UserDTO> Register([FromBody] UserCreateDTO userCreateDTO)
    {
        var response = _serviceManager.UserService.Create(userCreateDTO);
        return response.ToActionResult();
    }

    /// <summary>
    /// Logs out the user
    /// <summary>
    [HttpDelete]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Route("/logout")]
    public ActionResult Logout()
    {
        return Ok();
    }
}

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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Route("/")]
    public async Task<ActionResult<IEnumerable<MessageDTO>>> Timeline([FromQuery] string userId, CancellationToken ct = default)
    {
        _logger.LogInformation("We got a visitor from: " + Request.HttpContext.Connection.RemoteIpAddress);
        
        var response = await _serviceManager.MessageService.GetAllFollowedByUserIdAsync(userId, ct);
        return response.ToActionResult();
    }

    /// <summary>
    /// Displays the latest messages of all users.
    /// <summary>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Route("/public")]
    public async Task<ActionResult<IEnumerable<MessageDTO>>> PublicTimeline(CancellationToken ct = default)
    {
        var response = await _serviceManager.MessageService.GetAllNonFlaggedAsync(ct);
        return response.ToActionResult();
    }

    /// <summary>
    /// Display's a users tweets.
    /// <summary>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Route("/{username}")]
    public async Task<ActionResult<IEnumerable<MessageDTO>>> UserTimeline(string username, CancellationToken ct = default)
    {
        var response = await _serviceManager.MessageService.GetAllByUsernameAsync(username, ct);
        return response.ToActionResult();
    }

    /// <summary>
    /// Adds the current user as follower of the given user.
    /// <summary>
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Route("/{username}/follow")]
    public async Task<ActionResult<FollowerDTO>> FollowUser(string username, [FromQuery] string userId)
    {
        var response = await _serviceManager.FollowerService.CreateAsync(userId, username);
        return response.ToActionResult();
    }

    /// <summary>
    /// Removes the current user as follower of the given user.
    /// <summary>
    [HttpDelete]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Route("/{username}/unfollow")]
    public async Task<ActionResult> UnfollowUser(string username, [FromQuery] string userId)
    {
        var response = await _serviceManager.FollowerService.DeleteAsync(userId, username);
        return response.ToActionResult();
    }

    /// <summary>
    /// Registers a new message for the user.
    /// <summary>
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Route("/add_message")]
    public async Task<ActionResult<MessageDTO>> AddMessage([FromQuery] string userId, [FromQuery] string text)
    {
        var response = await _serviceManager.MessageService.CreateAsync(userId, text);
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
    public async Task<ActionResult> Login([FromBody] LoginDTO loginDTO)
    {
        var response = await _serviceManager.AuthenticationService.AuthenticateAsync(loginDTO.Username!, loginDTO.Password!);
        return response.ToActionResult();
    }

    /// <summary>
    /// Registers the user.
    /// <summary>
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Route("/register")]
    public async Task<ActionResult<UserDTO>> Register([FromBody] UserCreateDTO userCreateDTO)
    {
        var response = await _serviceManager.UserService.CreateAsync(userCreateDTO);
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

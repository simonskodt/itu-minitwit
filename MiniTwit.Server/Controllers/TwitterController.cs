using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniTwit.Core.DTOs;
using MiniTwit.Service;

namespace MiniTwit.Server.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
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
    /// Gets all messages for a specific user and from all the users they follow.
    /// </summary>
    /// <param name="userId">The id of the user to target</param>
    /// <param name="ct"></param>
    /// <returns>A list of Messages</returns>
    /// <response code="200">If the userId exists, return all the messages belonging to the userId and the ones they follow</response>
    /// <response code="404">If the userId does not exist</response>
    [HttpGet("/")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<MessageDTO>>> Timeline([FromQuery] string userId, CancellationToken ct = default)
    {
        _logger.LogInformation("We got a visitor from: " + Request.HttpContext.Connection.RemoteIpAddress);

        var response = await _serviceManager.MessageService.GetAllFollowedByUserIdAsync(userId, ct);
        return response.ToActionResult();
    }

    /// <summary>
    /// Gets all the latest non-flagged messages from all users.
    /// </summary>
    /// <param name="ct"></param>
    /// <returns>A list of all non-flagged Messages</returns>
    /// <response code="200">Every time, return all non-flagged messages</response>
    [HttpGet("/public")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<MessageDTO>>> PublicTimeline(CancellationToken ct = default)
    {
        var response = await _serviceManager.MessageService.GetAllNonFlaggedAsync(ct);
        return response.ToActionResult();
    }

    /// <summary>
    /// Gets all messages for a specific user and from all the users they follow.
    /// </summary>
    /// <param name="username">The username of the user to get all messages from</param>
    /// <param name="ct"></param>
    /// <returns>A list of all Messages belonging to the specific username</returns>
    /// <response code="200">If the username exists, return all the messages belonging to the username</response>
    /// <response code="404">If the username does not exist</response>
    [HttpGet("/{username}")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<MessageDTO>>> UserTimeline(string username, CancellationToken ct = default)
    {
        var response = await _serviceManager.MessageService.GetAllByUsernameAsync(username, ct);
        return response.ToActionResult();
    }

    /// <summary>
    /// Creates a new Follower, making the user with the specified username follow the user with the specified userId.
    /// </summary>
    /// <param name="username">The username of the target user</param>
    /// <param name="userId">The id of the user to follow</param>
    /// <returns></returns>
    /// <response code="204">If the username and userId exists, create a new Follower</response>
    /// <response code="404">If the username or userId does not exist</response>
    [HttpPost("/{username}/follow")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> FollowUser(string username, [FromQuery] string userId)
    {
        var response = await _serviceManager.FollowerService.CreateAsync(userId, username);
        return response.ToActionResult();
    }

    /// <summary>
    /// Deletes an existing Follower, making the user with the specified username unfollow the user with the specified userId.
    /// </summary>
    /// <param name="username">The id of the user to target</param>
    /// <param name="userId">The id of the user to unfollow</param>
    /// <returns></returns>
    /// <response code="204">If the username and userId exists, delete the Follower</response>
    /// <response code="404">If the username or userId does not exist</response>
    [HttpDelete("/{username}/unfollow")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UnfollowUser(string username, [FromQuery] string userId)
    {
        var response = await _serviceManager.FollowerService.DeleteAsync(userId, username);
        return response.ToActionResult();
    }

    /// <summary>
    /// Creates a new Message for the given user.
    /// </summary>
    /// <param name="userId">The id of the user to post the Message</param>
    /// <param name="text">The content of the Message to create</param>
    /// <returns></returns>
    /// <response code="204">If userId exists, create a new Message</response>
    /// <response code="404">If the userId does not exist</response>
    [HttpPost("/add_message")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> AddMessage([FromQuery] string userId, [FromQuery] string text)
    {
        var response = await _serviceManager.MessageService.CreateAsync(userId, text);
        return response.ToActionResult();
    }

    /// <summary>
    /// Tries to log the user into the system with the given credentials.
    /// </summary>
    /// <param name="loginDTO">The id of the user to target</param>
    /// <returns></returns>
    /// <response code="204">If the credentials match an existing user</response>
    /// <response code="401">If the credentials are invalid</response>
    [HttpPost("/login")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> Login([FromBody] LoginDTO loginDTO)
    {
        var response = await _serviceManager.AuthenticationService.AuthenticateAsync(loginDTO.Username!, loginDTO.Password!);
        return response.ToActionResult();
    }

    /// <summary>
    /// Creates a new User with the given information and credentials.
    /// </summary>
    /// <param name="userCreateDTO">The information and credentials to create a new User from</param>
    /// <returns></returns>
    /// <response code="204">If the username is not taken, create a new User</response>
    /// <response code="400">If the username is taken</response>
    [HttpPost("/register")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserDTO>> Register([FromBody] UserCreateDTO userCreateDTO)
    {
        var response = await _serviceManager.UserService.CreateAsync(userCreateDTO);
        return response.ToActionResult();
    }

    /// <summary>
    /// Logs a user out of the system.
    /// </summary>
    /// <returns></returns>
    /// <response code="200">Always</response>
    [HttpPost("/logout")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult Logout()
    {
        return Ok();
    }
}

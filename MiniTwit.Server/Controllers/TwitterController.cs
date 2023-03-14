using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniTwit.Core.DTOs;
using MiniTwit.Service;

namespace MiniTwit.Server.Controllers;

[ApiController]
[AllowAnonymous]
[Produces("application/json")]
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
    /// Get all messages for a specific user and the ones they follow, sorted in descending order after publish date.
    /// </summary>
    /// <param name="userId">The id of the user to target.</param>
    /// <param name="ct"></param>
    /// <returns>A list of Messages belonging to the user and the ones they follow sorted in descending order after publish date.</returns>
    /// <response code="200">If the userId exists, return all the messages belonging to the userId and the ones they follow sorted in descending order after publish date.</response>
    /// <response code="404">If the userId does not exist.</response>
    [HttpGet("/")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<MessageDTO>>> Timeline([FromQuery] string userId, CancellationToken ct = default)
    {
        _logger.LogInformation("We got a visitor from: " + Request.HttpContext.Connection.RemoteIpAddress);

        var response = await _serviceManager.MessageService.GetAllFollowedByUserIdAsync(userId, ct);
        return response.ToActionResult();
    }

    /// <summary>
    /// Get all non-flagged messages from all users sorted in descending order after publish date.
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="ct"></param>
    /// <returns>A list of all non-flagged Messages sorted in descending order after publish date.</returns>
    /// <response code="200">Every time, return all non-flagged messages sorted in descending order after publish date.</response>
    [HttpGet("/public/{pageNumber}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<MessageDTO>>> PublicTimeline(int pageNumber, CancellationToken ct = default)
    {
        var response = await _serviceManager.MessageService.GetAllNonFlaggedPageNumberLimitAsync(pageNumber, ct);
        return response.ToActionResult();
    }

    /// <summary>
    /// Get all messages for a specific user and from all the users they follow sorted in descending order after publish date.
    /// </summary>
    /// <param name="username">The username of the user to get all messages from.</param>
    /// <param name="ct"></param>
    /// <returns>A list of all Messages belonging to the specific username.</returns>
    /// <response code="200">If the username exists, return all the messages belonging to the username sorted in descending order after publish date.</response>
    /// <response code="404">If the username does not exist.</response>
    [HttpGet("/{username}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<MessageDTO>>> UserTimeline(string username, CancellationToken ct = default)
    {
        var response = await _serviceManager.MessageService.GetAllByUsernameAsync(username, ct);
        return response.ToActionResult();
    }

    /// <summary>
    /// Create a new follower, making the specified userId follow {username}.
    /// </summary>
    /// <param name="username">The username of the user to follow.</param>
    /// <param name="userId">The id of the user wanting to follow username.</param>
    /// <returns>Either No Content on success or Not Found on failure.</returns>
    /// <response code="204">If the username and userId exists, create a new Follower.</response>
    /// <response code="404">If the username or userId does not exist.</response>
    [HttpPost("/{username}/follow")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> FollowUser(string username, [FromQuery] string userId)
    {
        var response = await _serviceManager.FollowerService.CreateAsync(userId, username);
        return response.ToActionResult();
    }

    /// <summary>
    /// Delete an existing follower, making the specified userId unfollow {username}.
    /// </summary>
    /// <param name="username">The username of the user to unfollow.</param>
    /// <param name="userId">The id of the user wanting to unfollow username.</param>
    /// <returns>Either No Content on success or Not Found on failure.</returns>
    /// <response code="204">If the username and userId exists, delete the Follower.</response>
    /// <response code="404">If the username or userId does not exist.</response>
    [HttpDelete("/{username}/unfollow")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UnfollowUser(string username, [FromQuery] string userId)
    {
        var response = await _serviceManager.FollowerService.DeleteAsync(userId, username);
        return response.ToActionResult();
    }

    /// <summary>
    /// Create a new message for the given userId.
    /// </summary>
    /// <param name="userId">The id of the user to post the message.</param>
    /// <param name="text">The content of the Message to create.</param>
    /// <returns>Either No Content on success or Not Found on failure.</returns>
    /// <response code="204">If userId exists, create a new Message.</response>
    /// <response code="404">If the userId does not exist.</response>
    [HttpPost("/add_message")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> AddMessage([FromQuery] string userId, [FromQuery] string text)
    {
        var response = await _serviceManager.MessageService.CreateAsync(userId, text);
        return response.ToActionResult();
    }

    /// <summary>
    /// Authenticate a user with the given credentials.
    /// </summary>
    /// <param name="loginDTO">The id of the user to target.</param>
    /// <returns>Either a UserDTO on success or Unauthorized on invalid credentials.</returns>
    /// <response code="200">If the credentials match an existing user.</response>
    /// <response code="401">If the credentials are invalid.</response>
    [HttpPost("/login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<UserDTO>> Login([FromBody] LoginDTO loginDTO)
    {
        var response = await _serviceManager.AuthenticationService.AuthenticateAsync(loginDTO.Username!, loginDTO.Password!);
        return response.ToActionResult();
    }

    /// <summary>
    /// Create a new user with the given information and credentials.
    /// </summary>
    /// <param name="userCreateDTO">The information and credentials to create a new User from.</param>
    /// <returns>Either No Content on success or Bad Request on failure.</returns>
    /// <response code="204">If the username is not taken, create a new user.</response>
    /// <response code="400">If the username is taken.</response>
    [HttpPost("/register")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Register([FromBody] UserCreateDTO userCreateDTO)
    {
        var response = await _serviceManager.UserService.CreateAsync(userCreateDTO);
        return response.ToActionResult();
    }

    /// <summary>
    /// Log a user out of the system.
    /// </summary>
    /// <returns>An empty Ok result.</returns>
    /// <response code="200">Always.</response>
    [HttpPost("/logout")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Logout()
    {
        return await Task.FromResult(Ok());
    }


    [HttpGet("/user/{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserDTO>> GetUserNameById(string userId)
    {
        var response = await _serviceManager.UserService.GetByUserIdAsync(userId);
        return response.ToActionResult();
    }

    [HttpGet("/username/{username}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<UserDTO> GetIDByUsername(string username)
    {
        var response = _serviceManager.UserService.GetByUsername(username);
        return response.ToActionResult();
    }


}

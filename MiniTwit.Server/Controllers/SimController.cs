using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniTwit.Core.Responses;
using MiniTwit.Core.DTOs;
using MiniTwit.Service;

namespace MiniTwit.Server.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = "BasicAuthentication", Roles = "Simulator")]
[Produces("application/json")]
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
    /// Get the latest value.
    /// </summary>
    /// <param name="ct"></param>
    /// <returns>An Ok result, containing the latest value.</returns>
    /// <response code="200">Always, with the body containing the latest value.</response>
    [HttpGet("latest")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<LatestDTO>> Latest(CancellationToken ct = default)
    {
        var response = await _serviceManager.LatestService.GetAsync(ct);
        return response.ToActionResult();
    }

    /// <summary>
    /// Create (register) a new user.
    /// </summary>
    /// <param name="userCreateDTO">The information and credentials of the user to register.</param>
    /// <param name="latest">The latest value of the request.</param>
    /// <returns>Either No Content on success or Bad Request if the username is taken.</returns>
    /// <response code="204">On success.</response>
    /// <response code="400">If the username is already taken.</response>
    [HttpPost("register")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Register([FromBody] UserCreateDTO userCreateDTO, [FromQuery] int latest = -1)
    {
        await UpdateLatestAsync(latest);

        var response = await _serviceManager.UserService.CreateAsync(userCreateDTO);
        return response.ToActionResult();
    }

    /// <summary>
    /// Get a specific number of the most recent non-flagged messages.
    /// </summary>
    /// <param name="no">The number maximum number of messages to return.</param>
    /// <param name="latest">The latest value of the request.</param>
    /// <param name="ct"></param>
    /// <returns>Either a number of the most recent messages or Forbidden if unauthorized.</returns>
    /// <response code="200">If authorized, return the number of the most recent messages.</response>
    /// <response code="403">If unauthorized.</response>
    [HttpGet("msgs")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<IEnumerable<MessageDetailsDTO>>> Msgs([FromQuery] int no = 100, [FromQuery] int latest = -1, CancellationToken ct = default)
    {
        await UpdateLatestAsync(latest);

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
    /// Get a specific number of the most recent non-flagged messages from username.
    /// </summary>
    /// <param name="username">The number maximum number of messages to return.</param>
    /// <param name="no">The number maximum number of messages to return.</param>
    /// <param name="latest">The latest value of the request.</param>
    /// <param name="ct"></param>
    /// <returns>Either a number of the most recent messages from username, Not Found if the user does not exist or Forbidden if unauthorized.</returns>
    /// <response code="200">If authorized and the user exists, return the number of the most recent messages.</response>
    /// <response code="403">If unauthorized.</response>
    /// <response code="404">If authorized and the username is invalid.</response>
    [HttpGet("msgs/{username}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<MessageDetailsDTO>>> MsgUsername(string username, [FromQuery] int no = 100, [FromQuery] int latest = -1, CancellationToken ct = default)
    {
        await UpdateLatestAsync(latest);

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
    /// Create a message for the specified username.
    /// </summary>
    /// <param name="username">The number maximum number of messages to return.</param>
    /// <param name="messageCreateDTO">The content of the message to be created.</param>
    /// <param name="latest">The latest value of the request.</param>
    /// <returns>Either creates a new message for the given username or Forbidden if unauthorized.</returns>
    /// <response code="204">If authorized, the message was created.</response>
    /// <response code="403">If unauthorized.</response>
    [HttpPost("msgs/{username}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> MsgUsernamePost(string username, [FromBody] MessageCreateDTO messageCreateDTO, [FromQuery] int latest = -1)
    {
        await UpdateLatestAsync(latest);

        var response = await _serviceManager.MessageService.CreateByUsernameAsync(username, messageCreateDTO.Content!);
        return response.ToActionResult();
    }

    /// <summary>
    /// Get a specific number of the usernames of the followers of the specified username.
    /// </summary>
    /// <param name="username">The username </param>
    /// <param name="no">The number maximum number of messages to return.</param>
    /// <param name="latest">The latest value of the request.</param>
    /// <param name="ct"></param>
    /// <returns>Either a specific number of usernames belonging to the followers of the specified username, Not Found if the username does not exist, or Forbidden if unauthorized.</returns>
    /// <response code="200">If authorized, the specified number usernames of the followers of the specified username.</response>
    /// <response code="403">If unauthorized.</response>
    /// <response code="404">If authorized, but the username does not exist.</response>
    [HttpGet("fllws/{username}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FollowerDetailsDTO>> FollowUser(string username, [FromQuery] int no = 100, [FromQuery] int latest = -1, CancellationToken ct = default)
    {
        await UpdateLatestAsync(latest);

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
    /// Create or delete a follower, making the provided userId either follow or unfollow {username}.
    /// </summary>
    /// <param name="username">The username of the user wanting to follow/unfollow the specified user.</param>
    /// <param name="followerCreateDTO">The username of the user to follow/unfollow.</param>
    /// <param name="latest">The latest value of the request.</param>
    /// <returns>Either No Content on successfull create/delete of a follower, Not Found if the username is invalid or Forbidden if unauthorized.</returns>
    /// <response code="204">If authorized, either creates or deletes a follower.</response>
    /// <response code="403">If unauthorized.</response>
    /// <response code="404">If authorized, but the username is invalid.</response>
    [HttpPost("fllws/{username}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> FollowUser(string username, [FromBody] FollowerCreateDTO followerCreateDTO, [FromQuery] int latest = -1)
    {
        await UpdateLatestAsync(latest);

        var userResponse = await _serviceManager.UserService.GetByUsernameAsync(username);

        if (userResponse.HTTPResponse == HTTPResponse.BadRequest)
        {
            return NotFound();
        }

        // If Follow is not null make a follow
        if (followerCreateDTO.Follow is not null)
        {
            var followResponse = await _serviceManager.FollowerService.CreateAsync(userResponse.Model!.Id!, followerCreateDTO.Follow);

            if (followResponse.HTTPResponse == HTTPResponse.NotFound)
            {
                return NotFound();
            }

            return NoContent();
        }
        else
        {
            var unfollowResponse = await _serviceManager.FollowerService.DeleteAsync(userResponse.Model!.Id!, followerCreateDTO.Unfollow!);

            if (unfollowResponse.HTTPResponse == HTTPResponse.NotFound)
            {
                return NotFound();
            }

            return NoContent();
        }
    }

    private async Task UpdateLatestAsync(int latestVal)
    {
        await _serviceManager.LatestService.UpdateAsync(latestVal);
    }
}

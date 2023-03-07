using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniTwit.Core.Responses;
using MiniTwit.Core.DTOs;
using MiniTwit.Service;

namespace MiniTwit.Server.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = "BasicAuthentication", Roles = "Simulator")]
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
    /// </summary>
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
    /// </summary>
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Route("register")]
    public async Task<ActionResult> Register([FromBody] UserCreateDTO userCreateDTO, [FromQuery] int latest = -1)
    {
        await UpdateLatestAsync(latest);

        var response = await _serviceManager.UserService.CreateAsync(userCreateDTO);
        return response.ToActionResult();
    }

    /// <summary>
    /// Gets all the messages 
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Route("msgs")]
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
    /// Gets all the messages 
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Route("msgs/{username}")]
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
    /// Add message by username
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Route("msgs/{username}")]
    public async Task<ActionResult> MsgUsernamePost(string username, [FromBody] MessageCreateDTO messageCreateDTO, [FromQuery] int latest = -1)
    {
        await UpdateLatestAsync(latest);

        var response = await _serviceManager.MessageService.CreateByUsernameAsync(username, messageCreateDTO.Content!);
        return response.ToActionResult();
    }

    /// <summary>
    /// Get followers by username
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Route("fllws/{username}")]
    public async Task<ActionResult<FollowerDetailsDTO>> FollowUser(string username, [FromQuery] int latest = -1, [FromQuery] int no = 100, CancellationToken ct = default)
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
    /// Follow user by username
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Route("fllws/{username}")]
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

    private async Task UpdateLatestAsync(int latestVal)
    {
        await _serviceManager.LatestService.UpdateAsync(latestVal);
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniTwit.Core;
using MiniTwit.Core.DTOs;
using MiniTwit.Core.Entities;
using MiniTwit.Core.IRepositories;
using MiniTwit.Security;

namespace MiniTwit.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class SimController : ControllerBase
{
    private IHasher _hasher;
    private IUserRepository _userRepository;
    private IMessageRepository _messageRepository;
    private IFollowerRepository _followerRepository;
    private ILatestRepository _latestRepository;
    private int _latest;

    public SimController(IHasher hasher, IUserRepository userRepository, IMessageRepository messageRepository, IFollowerRepository followerRepository, ILatestRepository latestRepository)
    {
        _hasher = hasher;
        _userRepository = userRepository;
        _messageRepository = messageRepository;
        _followerRepository = followerRepository;
        _latestRepository = latestRepository;
    }

    /// <summary>
    /// Get the latest value
    /// <summary>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(Latest), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Route("latest")]
    public IActionResult Latest()
    {
        var response = _latestRepository.GetLatest();

        var latestDTO = new LastestDTO { LatestVal = response.Model!.LatestVal };

        return Ok(latestDTO);
    }

    /// <summary>
    /// Registers the user
    /// <summary>
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [Route("register")]
    public IActionResult Register([FromBody] RegisterDTO registerDTO, [FromQuery(Name = "latest")] int? latestVal)
    {
        UpdateLatest(latestVal);

        if (registerDTO.Username! == "")
        {
            return BadRequest(new { status = 400, error_msg = "You have to enter a username" });
        }
        else if (!registerDTO.Email!.Contains("@"))
        {
            return BadRequest(new { status = 400, error_msg = "You have to enter a valid email address" });
        }
        else if (registerDTO.Password == "")
        {
            return BadRequest(new { status = 400, error_msg = "You have to enter a password" });
        }

        var response = _userRepository.Create(registerDTO.Username!, registerDTO.Email!, registerDTO.Password!);

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
    [ProducesResponseType(typeof(IList<SimMessageDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Route("msgs")]
    public IActionResult Msgs([FromQuery(Name = "latest")] int? latestVal)
    {
        UpdateLatest(latestVal);

        var response = _messageRepository.GetAllNonFlagged();
        var reponseList = response.Model!;

        var messageDTOList = new List<SimMessageDTO>();

        foreach (var message in reponseList)
        {
            var userResponse = _userRepository.GetByUserId(message.AuthorId!);
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
    /// Gets all the messages 
    /// <summary>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IList<SimMessageDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Route("msgs/{username}")]
    public IActionResult MsgUsername(
        string username,
        [FromQuery(Name = "latest")] int? latestVal)
    {
        UpdateLatest(latestVal);

        var response = _messageRepository.GetAllNonFlaggedByUsername(username);

        if (response.HTTPResponse == HTTPResponse.NotFound)
        {
            return NotFound();
        }

        var reponseList = response.Model!;

        var messageDTOList = new List<SimMessageDTO>();

        foreach (var message in reponseList)
        {
            var userResponse = _userRepository.GetByUserId(message.AuthorId!);
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
    public IActionResult MsgUsernamePost(
        string username,
        string text,
        [FromQuery(Name = "latest")] int? latestVal)
    {
        UpdateLatest(latestVal);

        var userResponse = _userRepository.GetByUsername(username);

        var user = userResponse.Model;
        var response = _messageRepository.Create(user!.Id!, text);

        return NoContent();
    }

    /// <summary>
    /// Get followers by username
    /// <summary>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(SimFollowDetailsDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Route("fllws/{username}")]
    public IActionResult FollowUser(
        string username,
        [FromQuery(Name = "latest")] int? latestVal)
    {
        UpdateLatest(latestVal);

        var followers = _followerRepository.GetAllFollowersByUsername(username);


        if (followers.HTTPResponse == HTTPResponse.NotFound)
        {
            return NotFound();
        }

        var followersDTOs = new SimFollowDetailsDTO();


        foreach (var follower in followers.Model!)
        {
            var user = _userRepository.GetByUserId(follower.WhoId!);
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
    public IActionResult FollowUser(
        string userId,
        string username,
        [FromBody] SimFollowDTO followSim,
        [FromQuery(Name = "latest")] int? latestMessage)
    {
        UpdateLatest(latestMessage);

        // If followsim.Follow is not null make a follow
        if (followSim.Follow is not null)
        {
            var followResponse = _followerRepository.Create(userId, username);

            if (followResponse.HTTPResponse == HTTPResponse.NotFound)
            {
                return NotFound();
            }

            return NoContent();//followResponse.ToActionResult();
        }
        else
        {
            var unfollowResponse = _followerRepository.Delete(userId, username);

            if (unfollowResponse.HTTPResponse == HTTPResponse.NotFound)
            {
                return NotFound();
            }

            return NoContent();//unfollowResponse.ToActionResult();
        }
    }

    private void UpdateLatest(int? latestVal)
    {
        if (latestVal is not null)
        {
            var latest = _latestRepository.Update((int)latestVal);

            _latest = latest.Model!.LatestVal;
        }
    }
}

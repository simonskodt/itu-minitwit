using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniTwit.Core;
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
    private int LATEST;

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
        return response.ToActionResult();
    } 

    /// <summary>
    /// Registers the user
    /// <summary>
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [Route("register")]
    public void Register([FromBody] RegisterDTO registerDTO)
    {
        UpdateLatest();

        _hasher.Hash(registerDTO.Password, out string hashedPassword);
        var response = _userRepository.Create(registerDTO.Username, registerDTO.Email, hashedPassword);
        response.ToActionResult();
    }

    /// <summary>
    /// Gets all the messages 
    /// <summary>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(Message), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Route("msgs")]
    public IActionResult Msgs()
    {
        UpdateLatest();
        
        // TODO: Check if userid == authorid?
        return _messageRepository.GetAllNonFlagged().ToActionResult();
    }

    /// <summary>
    /// Gets all the messages 
    /// <summary>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(Message), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Route("msgs/{username}")]
    public IActionResult MsgUsername(string username)
    {
        UpdateLatest();
        
        var response = _messageRepository.GetAllByUsername(username);
        return response.ToActionResult();
    }

    /// <summary>
    /// Add message by username
    /// <summary>
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [Route("msgs/{username}")]
    public IActionResult MsgUsernamePost(string username, string text)
    {
        UpdateLatest();

        var user = _messageRepository.GetUserByUsername(username);
        var response = _messageRepository.Create(user.Id, text);

        return response.ToActionResult();
    }

    /// <summary>
    /// Get followers by username
    /// <summary>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Route("fllws/{username}")]
    public IActionResult FollowUser(string username)
    {
        UpdateLatest();

        var followers = _followerRepository.GetAllFollowersByUsername(username);

        return followers.ToActionResult();
    }

    /// <summary>
    /// Follow user by username
    /// <summary>
    /*[ HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(typeof(Latest), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [Route("fllws/{username}")]
    public IActionResult FollowUser(string username, string text)
    {
        UpdateLatest();

        var user = _messageRepository.GetUserByUsername(username);
        var response = _messageRepository.Create(user.Id, text);

        return response.ToActionResult();
    } */
    
    private void UpdateLatest()
    {
        var latest = _latestRepository.Update(LATEST);

        LATEST = latest.Model.LatestVal;
    }
}
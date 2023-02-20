using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniTwit.Core;
using MiniTwit.Core.Entities;
using MiniTwit.Core.IRepositories;
using MiniTwit.Security;

namespace MiniTwit.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class TwitterController : ControllerBase
{
    private IHasher _hasher;
    private IUserRepository _userRepository;
    private IMessageRepository _messageRepository;
    private IFollowerRepository _followerRepository;

    public TwitterController(IHasher hasher, IUserRepository userRepository, IMessageRepository messageRepository, IFollowerRepository followerRepository)
    {
        _hasher = hasher;
        _userRepository = userRepository;
        _messageRepository = messageRepository;
        _followerRepository = followerRepository;
    }

    /// <summary>
    /// Shows a users timeline or if no user is logged in it will
    /// redirect to the public timeline. This timeline shows the user's
    /// messages as well as all the messages of followed users.
    /// <summary>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<Message>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Route("/")]
    public IActionResult Timeline(string userId)
    {
        var response = _messageRepository.GetAllFollowedByUser(userId);
        return response.ToActionResult();
    } 

    /// <summary>
    /// Displays the latest messages of all users.
    /// <summary>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<Message>), StatusCodes.Status200OK)]
    [Route("/public")]
    public IActionResult PublicTimeline()
    {
        var response = _messageRepository.GetAllNonFlagged();
        return response.ToActionResult();
    } 

    /// <summary>
    /// Display's a users tweets.
    /// <summary>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<Message>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Route("/{username}")]
    public IActionResult UserTimeline(string username)
    {
        var response = _messageRepository.GetAllByUsername(username);
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
    public IActionResult FollowUser(string userId, string username)
    {
        var response = _followerRepository.Create(userId, username);
        return response.ToActionResult();
    }

    /// <summary>
    /// Removes the current user as follower of the given user.
    /// <summary>
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Route("/{username}/unfollow")]
    public IActionResult UnfollowUser(string userId, string username)
    {
        var response = _followerRepository.Delete(userId, username);
        return response.ToActionResult();
    }

    /// <summary>
    /// Registers a new message for the user.
    /// <summary>
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [Route("/add_message")]
    public IActionResult AddMessage(string userId, string text)
    {
        var response = _messageRepository.Create(userId, text);
        return response.ToActionResult();
    }

    /// <summary>
    /// Logs the user in.
    /// <summary>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Route("/login")]
    public IActionResult Login(string username, string password)
    {
        var response = _userRepository.GetByUsername(username);

        if (response.HTTPResponse == HTTPResponse.NotFound)
        {
            return Unauthorized("Invalid username");
        }

        var validPassword = _hasher.VerifyHash(password, response.Model!.Password!);

        if (!validPassword)
        {
            return Unauthorized("Invalid password");
        }

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
    public void Register(string username, string email, string password)
    {
        _hasher.Hash(password, out string hashedPassword);
        var response = _userRepository.Create(username, email, hashedPassword);
        response.ToActionResult();
    }

    /// <summary>
    /// Logs out the user
    /// <summary>
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Route("/logout")]
    public IActionResult Logout()
    {
        return Ok();
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniTwit.Core;
using MiniTwit.Core.Entities;
using MiniTwit.Core.IRepositories;
using MongoDB.Bson;
using System.Net.Http;

namespace MiniTwit.Server.Controllers;


[ApiController]
[Route("[controller]")]
public class TwitterController : ControllerBase
{
    private IUserRepository _userRepository;
    private IMessageRepository _messageRepository;
    private IFollowerRepository _followerRepository;

    public TwitterController(IUserRepository userRepository, IMessageRepository messageRepository, IFollowerRepository followerRepository)
    {
        _userRepository = userRepository;
        _messageRepository = messageRepository;
        _followerRepository = followerRepository;
    }

    /// <summary>
    /// Shows a users timeline or if no user is logged in it will
    /// redirect to the public timeline.  This timeline shows the user's
    /// messages as well as all the messages of followed users.
    /// <summary>
    [AllowAnonymous]
    [HttpGet]
    [Route("/")]
    public IActionResult Timeline()
    {
        var response = _messageRepository.GetAllNonFlagged();
        
        return response.ToActionResult();
    } 

    /// <summary>
    /// Displays the latest messages of all users.
    /// <summary>
    [AllowAnonymous]
    [HttpGet]
    [Route("/public")]
    public ICollection<Message>? PublicTimeline()
    {

        var messages = _repository.DisplayPublicTimeline();
        if (messages != null)
        {
            return messages;
        }
        return null;
    } 

    /// <summary>
    /// Display's a users tweets.
    /// <summary>
    [AllowAnonymous]
    [HttpGet]
    [Route("/{userName}")]
    public Message? UserTimeline(string userName, ObjectId userId)
    {
        var messages = _repository.DisplayTweetByUserName(userName, userId);

        if (messages != null)
        {
            return messages;
        }

        return null;
    }

    /// <summary>
    /// Adds the current user as follower of the given user.
    /// <summary>
    [AllowAnonymous]
    [HttpPost]
    [Route("/{userName}/follow")]
    public void FollowUser(FollowDTO followDTO)
    {
        var currentUser = followDTO.Who;
        var userToFollow = followDTO.Whom;

        _repository.FollowUser(currentUser, userToFollow);

    }

    /// <summary>
    /// Removes the current user as follower of the given user.
    /// <summary>
    [AllowAnonymous]
    [HttpPost]
    [Route("/{userName}/unfollow")]
    public void UnfollowUser(FollowDTO followDTO)
    {
        var currentUser = followDTO.Who;
        var userToFollow = followDTO.Whom;

        _repository.UnfollowUser(currentUser, userToFollow);
    }

    /// <summary>
    /// Registers a new message for the user.
    /// <summary>
    [AllowAnonymous]
    [HttpPost]
    [Route("/add_message")]
    public void AddMessage(string text)
    {
        _repository.AddMessage(text);
        
    }

    /// <summary>
    /// Logs the user in.
    /// <summary>
    [AllowAnonymous]
    [HttpGet]
    [HttpPost]
    [Route("/login")]
    public ActionResult Login(string username, string pw)
    {
        
        var response = _repository.Login(username, pw);
        if (response != null)
        {
            currentUser = response;
            return Ok();
        }
        else
        {
            return BadRequest();
        }
    }

    /// <summary>
    /// Logs the user out.
    /// <summary>
    [AllowAnonymous]
    [HttpGet]
    [HttpPost]
    [Route("/register")]
    public void Register(string username, string email, string pw)
    {
        _repository.RegisterUser(username, email, pw);
    }

    /// <summary>
    /// Registers the user.
    /// <summary>
    [AllowAnonymous]
    [HttpPost]
    [Route("/logout")]
    public void Logout(string username, string email, string pw)
    {
        _repository.RegisterUser(username, email, pw);
    }

    /// Extra method for swagger testing
    [AllowAnonymous]
    [HttpGet("{userName}")]
    public User? GetUser(string userName)
    {
        var user = _repository.GetUserByUserName(userName);
        if (user != null)
        {
            return user;
        }

        return null;
    }
}
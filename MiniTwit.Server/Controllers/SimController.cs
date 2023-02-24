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
    [ProducesResponseType(typeof(Message), StatusCodes.Status200OK)]
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
        _hasher.Hash(registerDTO.Password, out string hashedPassword);
        var response = _userRepository.Create(registerDTO.Username, registerDTO.Email, hashedPassword);
        response.ToActionResult();
    }


    private void updateLatest()
    {

    }


}
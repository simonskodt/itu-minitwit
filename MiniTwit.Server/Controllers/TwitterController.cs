using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniTwit.Server.Entities;
using MiniTwit.Server.Repository;

namespace MiniTwit.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class TwitterController : ControllerBase{

    IMongoDBRepository _repository;
    public TwitterController(IMongoDBRepository repository){
        _repository = repository;
    }

    [AllowAnonymous]
    [HttpGet("{userName}")]
    public User? GetUser(string userName){
        var user = _repository.GetUserByUserName(userName);
        if (user != null){
            return user;
        }
        return null;
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("/Registrer")]
    public void Register(string username, string email, string pw){
        _repository.RegisterUser(username, email, pw);
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("/Login")]
    public ActionResult Login(string username, string pw){
        var response = _repository.Login(username, pw);
        if (response != null ){
            return Ok();
        }else{
            return BadRequest();
        }
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public void InsertUser(){
        _repository.InsertUser();
    }

}
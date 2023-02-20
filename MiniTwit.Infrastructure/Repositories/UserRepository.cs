using MiniTwit.Core;
using MiniTwit.Core.Entities;
using MiniTwit.Core.IRepositories;
using MiniTwit.Security;
using MongoDB.Driver;

namespace MiniTwit.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private IMongoDBContext _context;

    public UserRepository(IMongoDBContext context)
    {
        _context = context;
    }

    public Response<User> Create(string username, string email, string password)
    {
        var existingUser = _context.Users.Find(user => user.Username == username).FirstOrDefault();

        // Username already taken
        if (existingUser != null)
        {
            return new Response<User>
            {
                HTTPResponse = HTTPResponse.Conflict
            };
        }

        var user = new User
        {
            Username = username,
            Email = email,
            Password = password,
        };

        _context.Users.InsertOne(user);

        return new Response<User>
        {
            HTTPResponse = HTTPResponse.Created,
            Model = user
        };
    }

    public Response<User> GetByUserId(string userId)
    {
        var user = _context.Users.Find(user => user.Id == userId).FirstOrDefault();

        if (user == null)
        {
            return new Response<User>
            {
                HTTPResponse = HTTPResponse.NotFound
            };
        }

        return new Response<User>
        {
            HTTPResponse = HTTPResponse.Success,
            Model = user
        };
    }

    public Response<User> GetByUsername(string username)
    {
        var user = _context.Users.Find(user => user.Username == username).FirstOrDefault();

        if (user == null)
        {
            return new Response<User>
            {
                HTTPResponse = HTTPResponse.NotFound
            };
        }

        return new Response<User>
        {
            HTTPResponse = HTTPResponse.Success,
            Model = user
        };
    }
}
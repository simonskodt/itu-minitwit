using MiniTwit.Core;
using MiniTwit.Core.Entities;
using MiniTwit.Core.IRepositories;
using MiniTwit.Security;
using MongoDB.Driver;

namespace MiniTwit.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private IMongoDBContext _context;
    private IHasher _hasher;

    public UserRepository(IMongoDBContext context, IHasher hasher)
    {
        _context = context;
        _hasher = hasher;
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

        _hasher.Hash(password, out string hash);

        var user = new User
        {
            Username = username,
            Email = email,
            Password = hash,
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

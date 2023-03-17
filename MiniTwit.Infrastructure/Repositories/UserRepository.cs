using MiniTwit.Core;
using MiniTwit.Core.Entities;
using MiniTwit.Core.Error;
using MiniTwit.Core.IRepositories;
using MiniTwit.Core.Responses;
using MongoDB.Driver;

namespace MiniTwit.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private IMongoDBContext _context;

    public UserRepository(IMongoDBContext context)
    {
        _context = context;
    }

    public DBResult<User> Create(string username, string email, string password)
    {
        var user = new User
        {
            Username = username,
            Email = email,
            Password = password,
        };

        _context.Users.InsertOne(user);

        return new DBResult<User>
        {
            Model = user,
            ErrorType = null
        };
    }

    public async Task<DBResult<User>> CreateAsync(string username, string email, string password)
    {
        var user = new User
        {
            Username = username,
            Email = email,
            Password = password,
        };

        await _context.Users.InsertOneAsync(user);

        return new DBResult<User>
        {
            Model = user,
            ErrorType = null
        };
    }

    public DBResult<User> GetByUserId(string userId, CancellationToken ct = default)
    {
        var user = _context.Users.Find(u => u.Id == userId).FirstOrDefault(ct);

        if (user == null)
        {
            return new DBResult<User>
            {
                Model = null,
                ErrorType = ErrorType.INVALID_USER_ID
            };
        }

        return new DBResult<User>
        {
            Model = user,
            ErrorType = null
        };
    }

    public async Task<DBResult<User>> GetByUserIdAsync(string userId, CancellationToken ct = default)
    {
        var user = await _context.Users.Find(u => u.Id == userId).FirstOrDefaultAsync(ct);

        if (user == null)
        {
            return new DBResult<User>
            {
                Model = null,
                ErrorType = ErrorType.INVALID_USER_ID
            };
        }

        return new DBResult<User>
        {
            Model = user,
            ErrorType = null
        };
    }

    public DBResult<User> GetByUsername(string username, CancellationToken ct = default)
    {
        var user = GetUserByUsername(username, ct);

        if (user == null)
        {
            return new DBResult<User>
            {
                Model = null,
                ErrorType = ErrorType.INVALID_USERNAME
            };
        }

        return new DBResult<User>
        {
            Model = user,
            ErrorType = null
        };
    }

    public async Task<DBResult<User>> GetByUsernameAsync(string username, CancellationToken ct = default)
    {
        var user = await GetUserByUsernameAsync(username, ct);

        if (user == null)
        {
            return new DBResult<User>
            {
                Model = null,
                ErrorType = ErrorType.INVALID_USERNAME
            };
        }

        return new DBResult<User>
        {
            Model = user,
            ErrorType = null
        };
    }

    private User? GetUserByUsername(string username, CancellationToken ct = default)
    {
        return _context.Users.Find(u => u.Username == username).FirstOrDefault(ct);
    }

    private async Task<User?> GetUserByUsernameAsync(string username, CancellationToken ct = default)
    {
        return await _context.Users.Find(u => u.Username == username).FirstOrDefaultAsync(ct);
    }
}
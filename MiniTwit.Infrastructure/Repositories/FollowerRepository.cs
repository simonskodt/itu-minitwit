using MiniTwit.Core;
using MiniTwit.Core.Entities;
using MiniTwit.Core.Error;
using MiniTwit.Core.IRepositories;
using MiniTwit.Core.Responses;
using MongoDB.Driver;

namespace MiniTwit.Infrastructure.Repositories;

public class FollowerRepository : IFollowerRepository
{
    private IMongoDBContext _context;

    public FollowerRepository(IMongoDBContext context)
    {
        _context = context;
    }

    public DBResult<Follower> Create(string userId, string username)
    {
        var user = GetUserByUserId(userId);

        // See if user exists
        if (user == null)
        {
            return new DBResult<Follower>
            {
                Model = null,
                ErrorType = ErrorType.INVALID_USER_ID
            };
        }

        var toFollow = GetUserByUsername(username);

        // See if target exists
        if (toFollow == null)
        {
            return new DBResult<Follower>
            {
                Model = null,
                ErrorType = ErrorType.INVALID_USERNAME
            };
        }

        var follower = new Follower
        {
            WhoId = userId,
            WhomId = toFollow.Id
        };

        _context.Followers.InsertOne(follower);

        return new DBResult<Follower>
        {
            Model = follower,
            ErrorType = null
        };
    }

    public DBResult Delete(string userId, string username)
    {
        var user = GetUserByUserId(userId);

        if (user == null)
        {
            return new DBResult
            {
                ErrorType = ErrorType.INVALID_USER_ID
            };
        }

        var toUnfollow = GetUserByUsername(username);

        if (toUnfollow == null)
        {
            return new DBResult<Follower>
            {
                Model = null,
                ErrorType = ErrorType.INVALID_USERNAME
            };
        }

        _context.Followers.DeleteOne(f => f.WhoId == userId && f.WhomId == toUnfollow.Id);

        return new DBResult
        {
            ErrorType = null
        };
    }

    public DBResult<IEnumerable<Follower>> GetAllFollowersByUsername(string username, CancellationToken ct = default)
    {
        var user = GetUserByUsername(username, ct);

        if (user == null)
        {
            return new DBResult<IEnumerable<Follower>>
            {
                Model = null,
                ErrorType = ErrorType.INVALID_USERNAME
            };
        }

        var followers = _context.Followers.Find(f => f.WhomId == user.Id).ToList(ct);

        return new DBResult<IEnumerable<Follower>>
        {
            Model = followers,
            ErrorType = null
        };
    }

    private User? GetUserByUserId(string userId, CancellationToken ct = default)
    {
        return _context.Users.Find(u => u.Id == userId).FirstOrDefault(ct);
    }

    private User? GetUserByUsername(string username, CancellationToken ct = default)
    {
        return _context.Users.Find(u => u.Username == username).FirstOrDefault(ct);
    }
}
using MiniTwit.Core;
using MiniTwit.Core.Entities;
using MiniTwit.Core.IRepositories;
using MongoDB.Driver;

namespace MiniTwit.Infrastructure.Repositories;

public class FollowerRepository : IFollowerRepository
{
    private IMongoDBContext _context;

    public FollowerRepository(IMongoDBContext context)
    {
        _context = context;
    }

    public Response<Follower> Create(string userId, string targetUsername)
    {
        var toFollow = GetUserByUsername(targetUsername);

        // Target does not exist
        if (toFollow == null)
        {
            return new Response<Follower>
            {
                HTTPResponse = HTTPResponse.NotFound
            };
        }

        var follower = new Follower
        {
            WhoId = userId,
            WhomId = toFollow.Id
        };

        _context.Followers.InsertOne(follower);

        return new Response<Follower>
        {
            HTTPResponse = HTTPResponse.Created,
            Model = follower
        };
    }

    public Response Delete(string userId, string targetUsername)
    {
        var toUnfollow = GetUserByUsername(targetUsername);

        // Target does not exist
        if (toUnfollow == null)
        {
            return new Response
            {
                HTTPResponse = HTTPResponse.NotFound
            };
        }

        _context.Followers.DeleteOne(f => f.WhoId == userId && f.WhomId == toUnfollow.Id);

        return new Response
        {
            HTTPResponse = HTTPResponse.NoContent
        };
    }

    public Response<IEnumerable<Follower>> GetAllFollowersByUsername(string username)
    {
        // .g. username = Simon. Method gets all followers who follow Simon. 
    
        var user = GetUserByUsername(username);

        if (user is null)
        {
            return new Response<IEnumerable<Follower>>
            {
                HTTPResponse = HTTPResponse.NotFound,
            };
        }

        var followers = _context.Followers.Find(f => f.WhomId == user.Id).ToList();

        return new Response<IEnumerable<Follower>>
        {
            HTTPResponse = HTTPResponse.Success,
            Model = followers
        };
    }

    private User? GetUserByUsername(string username)
    {
        return _context.Users.Find(u => u.Username == username).FirstOrDefault();
    }
}
using MiniTwit.Core;
using MiniTwit.Core.Entities;
using MiniTwit.Core.IRepositories;
using MongoDB.Driver;

namespace MiniTwit.Infrastructure.Repositories;

public class MessageRepository : IMessageRepository
{
    private IMongoDBContext _context;

    public MessageRepository(IMongoDBContext context)
    {
        _context = context;
    }

    public Response<Message> Create(string userId, string text)
    {
        var user = GetUserByUserId(userId);

        // User not existing
        if (user == null)
        {
            return new Response<Message>
            {
                HTTPResponse = HTTPResponse.NotFound,
            };
        }

        var message = new Message
        {
            AuthorId = userId,
            Text = text,
            PubDate = DateTime.Now,
            Flagged = 0,
        };

        _context.Messages.InsertOne(message);

        return new Response<Message>
        {
            HTTPResponse = HTTPResponse.Created,
            Model = message
        };
    }

    public Response<IEnumerable<Message>> GetAllNonFlagged()
    {
        return new Response<IEnumerable<Message>>
        {
            HTTPResponse = HTTPResponse.Success,
            Model = _context.Messages.Find(m => m.Flagged == 0).SortByDescending(m => m.PubDate).ToList()
        };
    }

    public Response<IEnumerable<Message>> GetAllByUserId(string userId)
    {
        var user = GetUserByUserId(userId);

        if (user == null)
        {
            return new Response<IEnumerable<Message>>
            {
                HTTPResponse = HTTPResponse.NotFound
            };
        }

        var messages = _context.Messages.Find(m => m.AuthorId == userId).SortByDescending(m => m.PubDate).ToList();

        return new Response<IEnumerable<Message>>
        {
            HTTPResponse = HTTPResponse.Success,
            Model = messages
        };
    }

    public Response<IEnumerable<Message>> GetAllByUsername(string username)
    {
        var user = GetUserByUsername(username);

        if (user == null)
        {
            return new Response<IEnumerable<Message>>
            {
                HTTPResponse = HTTPResponse.NotFound
            };
        }

        var messages = _context.Messages.Find(m => m.AuthorId == user.Id).SortByDescending(m => m.PubDate).ToList();

        return new Response<IEnumerable<Message>>
        {
            HTTPResponse = HTTPResponse.Success,
            Model = messages
        };
    }

    public Response<IEnumerable<Message>> GetAllNonFlaggedByUsername(string username)
    {
        var user = GetUserByUsername(username);

        if (user == null)
        {
            return new Response<IEnumerable<Message>>
            {
                HTTPResponse = HTTPResponse.NotFound
            };
        }

        var messages = _context.Messages.
            Find(m => m.AuthorId == user.Id && m.Flagged == 0).
            SortByDescending(m => m.PubDate).
            ToList();

        return new Response<IEnumerable<Message>>
        {
            HTTPResponse = HTTPResponse.Success,
            Model = messages
        };
    }

    public Response<IEnumerable<Message>> GetAllFollowedByUser(string userId)
    {
        var user = GetUserByUserId(userId);

        // User not found
        if (user == null)
        {
            return new Response<IEnumerable<Message>>
            {
                HTTPResponse = HTTPResponse.NotFound
            };
        }

        var users = _context.Users.AsQueryable();
        var messages = _context.Messages.AsQueryable();
        var followers = _context.Followers.AsQueryable();

        var result = from m in messages
                     join u in users on m.AuthorId equals u.Id
                     join f in followers on m.AuthorId equals f.WhomId
                     where m.Flagged == 0 && u.Id == userId || f.WhoId == userId
                     orderby m.PubDate descending
                     select m;

        return new Response<IEnumerable<Message>>
        {
            HTTPResponse = HTTPResponse.Success,
            Model = result
        };
    }

    private User? GetUserByUserId(string userId)
    {
        return _context.Users.Find(u => u.Id == userId).FirstOrDefault();
    }

    private User? GetUserByUsername(string username)
    {
        return _context.Users.Find(u => u.Username == username).FirstOrDefault();
    }
}
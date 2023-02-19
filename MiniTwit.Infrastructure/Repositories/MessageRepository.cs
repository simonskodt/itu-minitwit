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

    public Response<IEnumerable<Message>> GetAll()
    {
        return new Response<IEnumerable<Message>>
        {
            HTTPResponse = HTTPResponse.Success,
            Model = _context.Messages.Aggregate().ToList()
        };
    }

    public Response<IEnumerable<Message>> GetAllNonFlagged()
    {
        return new Response<IEnumerable<Message>>
        {
            HTTPResponse = HTTPResponse.Success,
            Model = _context.Messages.Find(message => message.Flagged == 0).ToList()
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

        var messages = _context.Messages.Find(message => message.Flagged == 0 && message.AuthorId == userId).ToList();

        return new Response<IEnumerable<Message>>
        {
            HTTPResponse = HTTPResponse.Success,
            Model = messages
        };
    }

    public Response<IEnumerable<Message>> GetAllFollowedByUser(string userId)
    {
        var userMessages = GetAllByUserId(userId);

        // User not found
        if (userMessages.Model == null)
        {
            return userMessages;
        }

        return new Response<IEnumerable<Message>>
        {
            HTTPResponse = HTTPResponse.Success,
            Model = null // TODO - Remember to update
        };
    }

    private User? GetUserByUserId(string userId)
    {
        return _context.Users.Find(user => user.Id == userId).FirstOrDefault();
    }

    private IEnumerable<Follower> GetAllUsersFollowedByUser(string userId)
    {
        return null;
    }
}

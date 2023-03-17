using MiniTwit.Core;
using MiniTwit.Core.Entities;
using MiniTwit.Core.Error;
using MiniTwit.Core.IRepositories;
using MiniTwit.Core.Responses;
using MongoDB.Driver;

namespace MiniTwit.Infrastructure.Repositories;

public class MessageRepository : IMessageRepository
{
    private IMongoDBContext _context;

    public MessageRepository(IMongoDBContext context)
    {
        _context = context;
    }

    public DBResult<Message> Create(string userId, string text)
    {
        var user = GetUserByUserId(userId);

        // User not existing
        if (user == null)
        {
            return new DBResult<Message>
            {
                Model = null,
                ErrorType = ErrorType.INVALID_USER_ID
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

        return new DBResult<Message>
        {
            Model = message,
            ErrorType = null
        };
    }

    public async Task<DBResult<Message>> CreateAsync(string userId, string text)
    {
        var user = await GetUserByUserIdAsync(userId);

        // User not existing
        if (user == null)
        {
            return new DBResult<Message>
            {
                Model = null,
                ErrorType = ErrorType.INVALID_USER_ID
            };
        }

        var message = new Message
        {
            AuthorId = userId,
            Text = text,
            PubDate = DateTime.Now,
            Flagged = 0,
        };

        await _context.Messages.InsertOneAsync(message);

        return new DBResult<Message>
        {
            Model = message,
            ErrorType = null
        };
    }

    public DBResult<IEnumerable<Message>> GetAllNonFlagged(CancellationToken ct = default)
    {
        var messages = _context.Messages.Find(m => m.Flagged == 0).SortByDescending(m => m.PubDate).ToList(ct); 
        
        return new DBResult<IEnumerable<Message>>
        {
            Model = messages,
            ErrorType = null
        };
    }

    public async Task<DBResult<IEnumerable<Message>>> GetAllNonFlaggedAsync(CancellationToken ct = default)
    {
        var messages = await _context.Messages.Find(m => m.Flagged == 0).SortByDescending(m => m.PubDate).Limit(20).ToListAsync(ct); 
        
        return new DBResult<IEnumerable<Message>>
        {
            Model = messages,
            ErrorType = null
        };
    }

    public DBResult<IEnumerable<Message>> GetAllByUserId(string userId, CancellationToken ct = default)
    {
        var user = GetUserByUserId(userId, ct);

        if (user == null)
        {
            return new DBResult<IEnumerable<Message>>
            {
                Model = null,
                ErrorType = ErrorType.INVALID_USER_ID
            };
        }

        var messages = _context.Messages.Find(m => m.AuthorId == userId).SortByDescending(m => m.PubDate).ToList(ct);

        return new DBResult<IEnumerable<Message>>
        {
            Model = messages,
            ErrorType = null
        };
    }

    public async Task<DBResult<IEnumerable<Message>>> GetAllByUserIdAsync(string userId, CancellationToken ct = default)
    {
        var user = await GetUserByUserIdAsync(userId, ct);

        if (user == null)
        {
            return new DBResult<IEnumerable<Message>>
            {
                Model = null,
                ErrorType = ErrorType.INVALID_USER_ID
            };
        }

        var messages = await _context.Messages.Find(m => m.AuthorId == userId).SortByDescending(m => m.PubDate).ToListAsync(ct);

        return new DBResult<IEnumerable<Message>>
        {
            Model = messages,
            ErrorType = null
        };
    }

    public DBResult<IEnumerable<Message>> GetAllByUsername(string username, CancellationToken ct = default)
    {
        var user = GetUserByUsername(username, ct);

        if (user == null)
        {
            return new DBResult<IEnumerable<Message>>
            {
                Model = null,
                ErrorType = ErrorType.INVALID_USERNAME
            };
        }

        var messages = _context.Messages.Find(m => m.AuthorId == user.Id).SortByDescending(m => m.PubDate).ToList(ct);

        return new DBResult<IEnumerable<Message>>
        {
            Model = messages,
            ErrorType = null
        };
    }

    public async Task<DBResult<IEnumerable<Message>>> GetAllByUsernameAsync(string username, CancellationToken ct = default)
    {
        var user = await GetUserByUsernameAsync(username, ct);

        if (user == null)
        {
            return new DBResult<IEnumerable<Message>>
            {
                Model = null,
                ErrorType = ErrorType.INVALID_USERNAME
            };
        }
        
        //All the followers where userName is whoId
        var allFollows = GetAllWhoUserFollows(user);
        
        var messages = await _context.Messages.Find(m => m.AuthorId == user.Id).SortByDescending(m => m.PubDate).ToListAsync(ct);

        foreach (var x in allFollows)
        {
            var mes = await _context.Messages.Find(m => m.AuthorId == x.WhomId).SortByDescending(m => m.PubDate).ToListAsync(ct);
            messages.AddRange(mes);
        }

        return new DBResult<IEnumerable<Message>>
        {
            Model = messages,
            ErrorType = null
        };
    }
    //To be able to display the followers tweets on my timeline
    private IEnumerable<Follower> GetAllWhoUserFollows(User user)
    {
        var followers = _context.Followers.Find(f => f.WhoId == user.Id).ToList();
        return followers;
    }

    public DBResult<IEnumerable<Message>> GetAllNonFlaggedByUsername(string username, CancellationToken ct = default)
    {
        var user = GetUserByUsername(username, ct);

        if (user == null)
        {
            return new DBResult<IEnumerable<Message>>
            {
                Model = null,
                ErrorType = ErrorType.INVALID_USERNAME
            };
        }

        var messages = _context.Messages.Find(m => m.AuthorId == user.Id && m.Flagged == 0).SortByDescending(m => m.PubDate).ToList(ct);

        return new DBResult<IEnumerable<Message>>
        {
            Model = messages,
            ErrorType = null
        };
    }

    public async Task<DBResult<IEnumerable<Message>>> GetAllNonFlaggedByUsernameAsync(string username, CancellationToken ct = default)
    {
        var user = await GetUserByUsernameAsync(username, ct);

        if (user == null)
        {
            return new DBResult<IEnumerable<Message>>
            {
                Model = null,
                ErrorType = ErrorType.INVALID_USERNAME
            };
        }

        var messages = await _context.Messages
            .Find(m => m.AuthorId == user.Id && m.Flagged == 0)
            .SortByDescending(m => m.PubDate)
            .ToListAsync(ct);

        return new DBResult<IEnumerable<Message>>
        {
            Model = messages,
            ErrorType = null
        };
    }

    public DBResult<IEnumerable<Message>> GetAllFollowedByUserId(string userId, CancellationToken ct = default)
    {
        var user = GetUserByUserId(userId, ct);

        // User not found
        if (user == null)
        {
            return new DBResult<IEnumerable<Message>>
            {
                Model = null,
                ErrorType = ErrorType.INVALID_USER_ID
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

        return new DBResult<IEnumerable<Message>>
        {
            Model = result,
            ErrorType = null
        };
    }

    public async Task<DBResult<IEnumerable<Message>>> GetAllFollowedByUserIdAsync(string userId, CancellationToken ct = default)
    {
        var user = await GetUserByUserIdAsync(userId, ct);

        // User not found
        if (user == null)
        {
            return new DBResult<IEnumerable<Message>>
            {
                Model = null,
                ErrorType = ErrorType.INVALID_USER_ID
            };
        }

        var users = _context.Users.AsQueryable();
        var messages = _context.Messages.AsQueryable();
        var followers = _context.Followers.AsQueryable();

        // TO:DO: Make async
        var result = await Task.FromResult(messages
            .Join(users, m => m.AuthorId, u => u.Id, (m, u) => new { Message = m, User = u })
            .Join(followers, mu => mu.Message.AuthorId, f => f.WhomId, (mu, f) => new { mu.Message, mu.User, Follower = f })
            .Where(mu => mu.Message.Flagged == 0 && (mu.User.Id == userId || mu.Follower.WhoId == userId))
            .OrderByDescending(mu => mu.Message.PubDate)
            .Select(mu => mu.Message));

        return new DBResult<IEnumerable<Message>>
        {
            Model = result,
            ErrorType = null
        };
    }

    private User? GetUserByUserId(string userId, CancellationToken ct = default)
    {
        return _context.Users.Find(u => u.Id == userId).FirstOrDefault(ct);
    }

    private async Task<User?> GetUserByUserIdAsync(string userId, CancellationToken ct = default)
    {
        return await _context.Users.Find(u => u.Id == userId).FirstOrDefaultAsync(ct);
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
using MongoDB.Driver;
using MongoDB.Bson;
using MiniTwit.Core.Entities;
using MiniTwit.Core.IRepositories;
using Konscious.Security.Cryptography;
using System.Text;
using MiniTwit.Core;

namespace MiniTwit.Infrastructure.Repositories;

public class MongoDBRepository : IMongoDBRepository
{
    private IMongoDBContext _context;

    public MongoDBRepository(IMongoDBContext context)
    {
        _context = context;
    }

    public ICollection<Message> DisplayTimeline()
    {
        // If used is logged in, show the users and its followed users messages

        // If user is not logged in, redirect to PublicTimeline()

        throw new NotImplementedException();
    }

    public ICollection<Message> DisplayPublicTimeline()
    {
        IList<Message> messages = _context.Messages.Aggregate().ToList();
        if (messages != null)
        {
            return messages;
        }

        return messages;
    }

    public ICollection<Message> DisplayUserTimeline()
    {
        throw new NotImplementedException();
    }

    public Message? DisplayTweetByUserName(string userName, ObjectId userId)
    {
        var filter = Builders<User>.Filter.Eq("Username", userName);
        var user = _context.Users.Find(filter).FirstOrDefault();

        if (user is null)
        {
            return null;
        }

        // Check wheather the user is followed
        bool isFollowed = false;
        if (user is not null)
        {
            var followerFilter =
                Builders<Follower>.Filter.Eq("WhoId", user._id)
                & Builders<Follower>.Filter.Eq("WhomId", userId);
            // isFollowed = _context.Users.Find(followerFilter).Any();
        }

        // Find the user timeline
        var msgFilter = Builders<Message>.Filter.Eq("AuthorId", user._id);
        // var msg = _context.Users.Find(msgFilter).SortByDecending(m => m.PubDate);

        // return msg;
        return null;
    }

    public void FollowUser(string userName)
    {
        throw new NotImplementedException();
    }

    public void UnfollowUser(string userName)
    {
        throw new NotImplementedException();
    }

    public void AddMessage(string text)
    {
        var message = new Message
        {
            MessageId = ObjectId.GenerateNewId(),
            AuthorId = ObjectId.GenerateNewId(),
            Text = text,
        };

        _context.Messages.InsertOne(message);

    }

    public User? Login(string userName, string pw)
    {
        var user = GetUserByUserName(userName);
        if (VerifyHash(pw, user.PwHash))
        {
            Console.WriteLine("Success");
            return user;
        }

        Console.WriteLine("Fail");
        return null;
    }

    public void Login()
    {
        throw new NotImplementedException();
    }

    public void RegisterUser(string username, string email, string pw)
    {
        var password = HashPassword(pw);

        var str = System.Text.Encoding.Default.GetString(password);
        var user = new User
        {
            _id = ObjectId.GenerateNewId(),
            Username = username,
            Email = email,
            PwHash = HashPassword(pw)
        };

        _context.Users.InsertOne(user);
    }

    public void Register()
    {
        throw new NotImplementedException();
    }

    public void Logout()
    {
        throw new NotImplementedException();
    }

    /// ----------------------------------------------------------------------------
    /// Swagger testing functions
    public User? GetUserByUserName(string userName)
    {
        var userNameFilter = Builders<User>.Filter.Eq(u => u.Username, userName);
        var user = _context.Users.Find(userNameFilter).First();
        if (user != null)
        {
            return user;
        }

        return null;
    }

    private ObjectId GetUserId(string userName)
    {
        var userId = Builders<User>.Filter.Eq(u => u.Username, userName);
        var user = _context.Users.Find(userId).FirstOrDefault();

        return user._id;
    }

    /// ----------------------------------------------------------------------------
    /// Utility functions
    private byte[] HashPassword(string password)
    {
        byte[] bytes = Encoding.ASCII.GetBytes(password);
        using var argon2 = new Argon2id(bytes);

        argon2.DegreeOfParallelism = 8;
        argon2.Iterations = 4;
        argon2.MemorySize = 1024 * 128;
        return argon2.GetBytes(32);
    }

    private bool VerifyHash(string password, byte[] hash)
    {
        var newHash = HashPassword(password);
        //byte[] bytes = Encoding.ASCII.GetBytes(hash);

        return hash.SequenceEqual(newHash);
    }

    public string FormatDatetime(long timestamp)
    {
        DateTime date = DateTime.FromBinary(timestamp);
        String formattedTime = date.ToString("yyyy-MM-dd @ HH:mm");

        return formattedTime;
    }
}
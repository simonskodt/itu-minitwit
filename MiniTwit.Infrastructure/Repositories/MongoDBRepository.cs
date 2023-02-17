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

    public void RegisterUser(string username, string email, string pw)
    {
        var password = HashPassword(pw);

        var str = System.Text.Encoding.Default.GetString(password);
        var user = new User 
        { 
            _id      = ObjectId.GenerateNewId(), 
            Username = username,
            Email    = email,
            PwHash   = str
        };
        _context.Users.InsertOne(user);
    }

    private byte[] HashPassword(string password)
    {

        byte[] bytes = Encoding.ASCII.GetBytes(password);
        using var argon2 = new Argon2id(bytes);

        argon2.DegreeOfParallelism = 8;
        argon2.Iterations = 4;
        argon2.MemorySize = 1024 * 128;
        return argon2.GetBytes(32);                
    }

    private bool VerifyHash(string password, string hash)
    {
        var newHash = HashPassword(password);
        byte[] bytes = Encoding.ASCII.GetBytes(hash); 
         
        return bytes.SequenceEqual(newHash);
    }

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

    public User? Login(string userName, string pw){
        var user = GetUserByUserName(userName);
        if (VerifyHash(pw, user.PwHash)){
            Console.WriteLine("YHHUUU");
            return user;
        }else{
            Console.WriteLine("FACCCKk");
            return null;
        }
    }

    public ICollection<Message> DisplayAllTweets()
    {
        IList<Message> messages = _context.Messages.Aggregate().ToList();
        if (messages != null)
        {
            return messages;
        }

        return new List<Message>();
    }

    public Message? DisplayTweetByUserName(string userName)
    {
        throw new NotImplementedException();
        // Message message = _context.Messages.Find(msg => msg.MessageId == userName).Single();
    }

    public void FollowUser(string userName)
    {
        throw new NotImplementedException();
    }

    public void UnfollowUser(string userName)
    {
        throw new NotImplementedException();
    }

    public void AddMessage()
    {
        throw new NotImplementedException();
    }

    public void Login()
    {
        throw new NotImplementedException();
    }

    public void Register()
    {
        throw new NotImplementedException();
    }

    public void Logout()
    {
        throw new NotImplementedException();
    }
}
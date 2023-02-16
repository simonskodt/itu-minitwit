using MongoDB.Driver;
using MongoDB.Bson;
using System.Collections.Generic;
using MiniTwit.Server.Entities;
using Konscious.Security.Cryptography;
using System.Text;

namespace MiniTwit.Server.Repository;

public class MongoDBRepository : IMongoDBRepository
{
    private MongoClient _dbClient;
    private IMongoDatabase _database;
    private IMongoCollection<User> _userCollection;
    private IMongoCollection<Follower> _followerCollection;
    private IMongoCollection<BsonDocument> _messageCollection;

    public MongoDBRepository()
    {
        _dbClient = new MongoClient("mongodb://localhost:27017");
        _database = _dbClient.GetDatabase("minitwit");
        _userCollection = _database.GetCollection<User>("user");
        _followerCollection = _database.GetCollection<Follower>("follower");
        _messageCollection = _database.GetCollection<BsonDocument>("message");
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

        _userCollection.InsertOne(user);
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
        var user = _userCollection.Find(userNameFilter).First();
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
        IList<Message> messages = _database.GetCollection<Message>("message").Aggregate().ToList();
        if (messages != null)
        {
            return messages;
        }

        return new List<Message>();
    }

    public Message? DisplayTweetByUserName(string userName)
    {
        throw new NotImplementedException();
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
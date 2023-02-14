using MongoDB.Driver;
using MongoDB.Bson;
using System.Collections.Generic;
using MiniTwit.Server.Entities;

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

    public void InsertUser()
    {
        var user = new User 
        { 
            _id      = ObjectId.GenerateNewId(), 
            Username = "Victor", 
            Email    = "vibr@itu.dk", 
            PwHash   = "Victor123"
        };

        _userCollection.InsertOne(user);
    }

    public User? GetUserByUserName(string userName)
    {
        var filter = Builders<User>.Filter.Eq("username", userName);
        var user = _userCollection.Find(filter).First();
        if (user != null)
        {
            return user;
        }

        return null;
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
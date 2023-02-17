using MiniTwit.Core;
using MiniTwit.Core.Entities;
using MongoDB.Driver;

namespace MiniTwit.Infrastructure;

public class MongoDBContext : IMongoDBContext
{
    public IMongoDatabase MongoDatabase { get; init; }
    public IMongoCollection<User> Users { get; init; }
    public IMongoCollection<Follower> Followers { get; init; }

    public IMongoCollection<Message> Messages { get; init; }

    public MongoDBContext()
    {
        MongoDatabase = new MongoClient("mongodb://localhost:27017").GetDatabase("minitwit");
        Users = MongoDatabase.GetCollection<User>("user");
        Followers = MongoDatabase.GetCollection<Follower>("follower");
        Messages = MongoDatabase.GetCollection<Message>("message");
    }
}

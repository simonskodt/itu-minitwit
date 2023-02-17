using MiniTwit.Core.Entities;
using MongoDB.Driver;

namespace MiniTwit.Core;

public interface IMongoDBContext
{
    IMongoDatabase MongoDatabase { get; init; }
    IMongoCollection<User> Users { get; init; }
    IMongoCollection<Follower> Followers { get; init; }
    IMongoCollection<Message> Messages { get; init; }
}
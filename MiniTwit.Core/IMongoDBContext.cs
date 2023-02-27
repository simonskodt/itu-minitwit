using MiniTwit.Core.Entities;
using MongoDB.Driver;

namespace MiniTwit.Core;

public interface IMongoDBContext
{
    IMongoCollection<User> Users { get; init; }
    IMongoCollection<Follower> Followers { get; init; }
    IMongoCollection<Message> Messages { get; init; }
    IMongoCollection<Latest> Latests { get; init; }
}
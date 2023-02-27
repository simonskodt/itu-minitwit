using Microsoft.Extensions.Options;
using MiniTwit.Core;
using MiniTwit.Core.Entities;
using MongoDB.Driver;

namespace MiniTwit.Infrastructure;

public class MongoDBContext : IMongoDBContext
{
    public IMongoCollection<User> Users { get; init; }
    public IMongoCollection<Follower> Followers { get; init; }
    public IMongoCollection<Message> Messages { get; init; }
    public IMongoCollection<Latest> Latests { get; init; }

    public MongoDBContext(IOptions<MiniTwitDatabaseSettings> databaseSettings)
    {
        var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(databaseSettings.Value.Database);

        Users = mongoDatabase.GetCollection<User>(databaseSettings.Value.UsersCollectionName);
        Followers = mongoDatabase.GetCollection<Follower>(databaseSettings.Value.FollowersCollectionName);
        Messages = mongoDatabase.GetCollection<Message>(databaseSettings.Value.TweetsCollectionName);
        Latests = mongoDatabase.GetCollection<Latest>(databaseSettings.Value.LatestsCollectionName);
    }
}

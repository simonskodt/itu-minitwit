using MiniTwit.Core;
using MiniTwit.Infrastructure.Data.DataCreators;
using MiniTwit.Security;
using MongoDB.Bson;

namespace MiniTwit.Infrastructure.Data;

public class DataInitializer
{
    private readonly IMongoDBContext _context;
    private readonly IHasher _hasher;

    public DataInitializer(IMongoDBContext context, IHasher hasher)
    {
        _context = context;
        _hasher = hasher;
    }

    public void Seed(bool isInDevelopment)
    {
        if (isInDevelopment)
        {
            SeedForDev();
        }

        if (_context.Latests.CountDocuments(new BsonDocument()) != 0)
        {
            return;
        }

        var latest = LatestCreator.Create(0);
        _context.Latests.InsertOne(latest);
    }

    private void SeedForDev()
    {
        if (_context.Users.CountDocuments(new BsonDocument()) != 0)
        {
            return;
        }

        var hash = _hasher.Hash("password");

        // Users
        var gustav = UserCreator.Create("Gustav", "g@minitwit.com", hash);
        var simon = UserCreator.Create("Simon", "s@minitwit.com", hash);
        var nikolaj = UserCreator.Create("Nikolaj", "n@minitwit.com", hash);
        var victor = UserCreator.Create("Victor", "v@minitwit.com", hash);

        _context.Users.InsertMany(new[] { gustav, simon, nikolaj, victor });

        // Messages
        var m1 = MessageCreator.Create(gustav.Id!, "Gustav", "Gustav's first tweet!", DateTime.Now.AddDays(-1));
        var m2 = MessageCreator.Create(gustav.Id!, "Gustav", "Gustav's second tweet!", DateTime.Now.AddDays(-0.5));
        var m3 = MessageCreator.Create(gustav.Id!, "Gustav", "Gustav's Flagged", DateTime.Now, 1);

        var m4 = MessageCreator.Create(simon.Id!, "Simon", "Simon's first tweet");
        var m5 = MessageCreator.Create(simon.Id!, "Simon", "Simon's second tweet");
        var m6 = MessageCreator.Create(simon.Id!, "Simon", "Simon's third tweet");

        var m7 = MessageCreator.Create(nikolaj.Id!, "Nikolaj", "Nikolaj1");
        var m8 = MessageCreator.Create(nikolaj.Id!, "Nikolaj", "Nikolaj2");

        var m9 = MessageCreator.Create(victor.Id!, "Victor", "Victor1");
        var m10 = MessageCreator.Create(victor.Id!, "Victor", "Victor2");

        _context.Messages.InsertMany(new[] { m1, m2, m3, m4, m5, m6, m7, m8, m9, m10 });

        // Followers
        var f1 = FollowerCreator.Create(gustav.Id!, simon.Id!);
        var f2 = FollowerCreator.Create(simon.Id!, nikolaj.Id!);
        var f3 = FollowerCreator.Create(simon.Id!, victor.Id!);
        var f4 = FollowerCreator.Create(victor.Id!, gustav.Id!);

        _context.Followers.InsertMany(new[] { f1, f2, f3, f4 });
    }
}

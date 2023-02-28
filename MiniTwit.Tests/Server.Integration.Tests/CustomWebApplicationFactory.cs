using System.Globalization;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MiniTwit.Core;
using MiniTwit.Infrastructure;
using MiniTwit.Infrastructure.Data.DataCreators;
using MiniTwit.Security;
using Mongo2Go;

namespace MiniTwit.Tests.Integration;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private MongoDbRunner _runner;

    public CustomWebApplicationFactory()
    {
        _runner = MongoDbRunner.Start(additionalMongodArguments: "--quiet --logpath /dev/null");
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var mongoDBContext = services.SingleOrDefault(m => m.ServiceType == typeof(MongoDBContext));

            if (mongoDBContext != null)
            {
                services.Remove(mongoDBContext);
            }
            
            var settings = new MiniTwitDatabaseSettings
            {
                ConnectionString = _runner.ConnectionString,
                Database = "MiniTwit",
                UsersCollectionName = "Users",
                TweetsCollectionName = "Messages",
                FollowersCollectionName = "Followers"
            };

            services.AddScoped<IMongoDBContext, MongoDBContext>(c => new MongoDBContext(Options.Create<MiniTwitDatabaseSettings>(settings)));

            var provider = services.BuildServiceProvider();
            using var scope = provider.CreateScope();
            var appContext = scope.ServiceProvider.GetRequiredService<IMongoDBContext>();
            var hasher = scope.ServiceProvider.GetRequiredService<IHasher>();
            
            Seed(appContext, hasher);
        });

        builder.UseEnvironment("Integration");
        return base.CreateHost(builder);
    }

    private void Seed(IMongoDBContext context, IHasher hasher)
    {
        hasher.Hash("password", out string hash);
        // Users
        var gustav  = UserCreator.Create("000000000000000000000001", "Gustav", "g@minitwit.com", hash);
        var simon   = UserCreator.Create("000000000000000000000002", "Simon", "s@minitwit.com", hash);
        var nikolaj = UserCreator.Create("000000000000000000000003", "Nikolaj", "n@minitwit.com", hash);
        var victor  = UserCreator.Create("000000000000000000000004", "Victor", "v@minitwit.com", hash);

        context.Users.InsertMany(new[] { gustav, simon, nikolaj, victor });

        // Messages
        var m1 = MessageCreator.Create("000000000000000000000001", gustav.Id!, "Gustav's first tweet!", DateTime.Parse("01/01/2023 12:00:00", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal));
        var m2 = MessageCreator.Create("000000000000000000000002", gustav.Id!, "Gustav's second tweet!", DateTime.Parse("01/01/2023 12:00:00", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal));
        var m3 = MessageCreator.Create("000000000000000000000003", gustav.Id!, "Gustav's Flagged", DateTime.Parse("01/01/2023 12:00:01", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal), 1);

        var m4 = MessageCreator.Create("000000000000000000000004", simon.Id!, "Simon's first tweet", DateTime.Parse("01/01/2023 12:00:02", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal));
        var m5 = MessageCreator.Create("000000000000000000000005", simon.Id!, "Simon's second tweet", DateTime.Parse("01/01/2023 12:00:03", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal));
        var m6 = MessageCreator.Create("000000000000000000000006", simon.Id!, "Simon's third tweet", DateTime.Parse("01/01/2023 12:00:04", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal));

        var m7 = MessageCreator.Create("000000000000000000000007", nikolaj.Id!, "Nikolaj1", DateTime.Parse("01/01/2023 12:00:05", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal));
        var m8 = MessageCreator.Create("000000000000000000000008", nikolaj.Id!, "Nikolaj2", DateTime.Parse("01/01/2023 12:00:06", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal));

        var m9 = MessageCreator.Create("000000000000000000000009", victor.Id!, "Victor1", DateTime.Parse("01/01/2023 12:00:01", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal));
        var m10 = MessageCreator.Create("000000000000000000000010", victor.Id!, "Victor2", DateTime.Parse("01/01/2023 12:00:02", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal));

        context.Messages.InsertMany(new[] { m1, m2, m3, m4, m5, m6, m7, m8, m9, m10 });

        // Followers
        var f1 = FollowerCreator.Create(gustav.Id!, simon.Id!);

        var f2 = FollowerCreator.Create(simon.Id!, nikolaj.Id!);
        var f3 = FollowerCreator.Create(simon.Id!, victor.Id!);

        var f4 = FollowerCreator.Create(victor.Id!, gustav.Id!);

        context.Followers.InsertMany(new [] { f1, f2, f3, f4 });
    }
}

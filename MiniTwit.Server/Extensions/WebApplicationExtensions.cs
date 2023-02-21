using MiniTwit.Core;
using MiniTwit.Infrastructure.Data;

namespace MiniTwit.Server.Extensions;

public static class WebApplicationExtensions
{
    public static IHost SeedDB(this IHost host)
    {
        using (var scope = host.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<IMongoDBContext>();
            var dataInitalizer = scope.ServiceProvider.GetRequiredService<DataInitializer>();

            dataInitalizer.Seed();

            return host;
        }
    }
}

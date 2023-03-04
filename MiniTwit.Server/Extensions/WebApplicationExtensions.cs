using MiniTwit.Infrastructure.Data;

namespace MiniTwit.Server.Extensions;

public static class WebApplicationExtensions
{
    public static IHost SeedDatabase(this IHost host, bool isInDevelopment)
    {
        using (var scope = host.Services.CreateScope())
        {
            var dataInitalizer = scope.ServiceProvider.GetRequiredService<DataInitializer>();

            dataInitalizer.Seed(isInDevelopment);

            return host;
        }
    }
}

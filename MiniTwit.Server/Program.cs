using Microsoft.AspNetCore.Authentication;
using MiniTwit.Core;
using MiniTwit.Core.IRepositories;
using MiniTwit.Infrastructure;
using MiniTwit.Infrastructure.Data;
using MiniTwit.Infrastructure.Repositories;
using MiniTwit.Security;
using MiniTwit.Security.Hashers;
using MiniTwit.Server;
using MiniTwit.Server.Authentication;
using MiniTwit.Server.Extensions;
using MiniTwit.Service;
using Prometheus;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Serilog.Sinks.Grafana.Loki;

var builder = WebApplication.CreateBuilder(args);

// Add user-secrets if running in container
builder.Configuration.AddKeyPerFile("/run/secrets", optional: true);

// Add services to the container.

// Configure MongoDB
var dbSettings = builder.Configuration.GetSection(nameof(MiniTwitDatabaseSettings));
builder.Services.Configure<MiniTwitDatabaseSettings>(dbSettings);
builder.Services.Configure<MiniTwitDatabaseSettings>(options => options.ConnectionString = builder.Configuration.GetConnectionString("MiniTwit")!);

// Configure Hasher
builder.Services.Configure<HashSettings>(builder.Configuration.GetSection(nameof(HashSettings)));
builder.Services.AddScoped<IHasher, Argon2Hasher>();

// Add Basic Authentication
builder.Services.AddAuthentication("BasicAuthentication").AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", options => { });
builder.Services.AddAuthorization();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.ConfigureSwagger();

builder.Services.AddScoped<IMongoDBContext, MongoDBContext>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<IFollowerRepository, FollowerRepository>();
builder.Services.AddScoped<ILatestRepository, LatestRepository>();
builder.Services.AddScoped<IServiceManager, ServiceManager>();
builder.Services.AddScoped<DataInitializer>();

// Add logging
builder.Host.UseSerilog((ctx, config) =>
{
    config.MinimumLevel.Override("Microsoft", LogEventLevel.Information)
        .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
        .Enrich.FromLogContext()
        .Enrich.WithProperty("Application", ctx.HostingEnvironment.ApplicationName)
        .Enrich.WithProperty("Environment", ctx.HostingEnvironment.EnvironmentName)
        .WriteTo.GrafanaLoki(
            ctx.Configuration.GetSection("loki:Uri").Value!,
            new[] { new LokiLabel { Key = "app", Value = "minitwit" } }
        );

    if (ctx.HostingEnvironment.IsDevelopment())
        config.MinimumLevel.Debug()
            .WriteTo.Console(new RenderedCompactJsonFormatter());
});

var app = builder.Build();

// Seed DB
app.SeedDatabase(app.Environment.IsDevelopment());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "MiniTwit API");
        options.DocumentTitle = "MiniTwit API - Swagger";
        options.DisplayRequestDuration();
    });
}

// Cross-origin Request Blocked
app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin
    .AllowCredentials());

// app.UseHttpsRedirection();

// Monitoring
app.UseMetricServer();
app.UseHttpMetrics(options =>
{
    options.AddCustomLabel("host", context => context.Request.Host.Host);
});

//Ip filtering
// app.UseMiddleware<IpAddressFilterMiddleware>(new List<string>()
// {
//     "164.92.167.188",
//     "104.248.134.203"
// });

// Logging
app.UseSerilogRequestLogging();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapMetrics();

app.Run();

public partial class Program { }

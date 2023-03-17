using Microsoft.AspNetCore.Authentication;
using MiniTwit.Core;
using MiniTwit.Core.IRepositories;
using MiniTwit.Infrastructure;
using MiniTwit.Infrastructure.Data;
using MiniTwit.Infrastructure.Repositories;
using MiniTwit.Security;
using MiniTwit.Security.Hashers;
using MiniTwit.Server.Authentication;
using MiniTwit.Server.Extensions;
using MiniTwit.Service;
using Prometheus;

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
builder.Services.AddAuthentication("BasicAuthentication").AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", options => {});
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

app.UseMetricServer();
app.UseHttpMetrics(options => {
    options.AddCustomLabel("host", context => context.Request.Host.Host);
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapMetrics();

app.Run();

public partial class Program {}

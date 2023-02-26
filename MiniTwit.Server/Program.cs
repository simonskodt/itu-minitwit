using Microsoft.OpenApi.Models;
using MiniTwit.Core;
using MiniTwit.Core.IRepositories;
using MiniTwit.Infrastructure;
using MiniTwit.Infrastructure.Data;
using MiniTwit.Infrastructure.Repositories;
using MiniTwit.Security;
using MiniTwit.Server.Extensions;

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
builder.Services.AddScoped<IHasher, Hasher>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.SwaggerDoc("v1", new OpenApiInfo()
    {
        Title = "MiniTwit API",
        Version = "v1",
        Description = "A refactor of a Twitter clone handed out in the elective course DevOps on the IT University of Copenhagen.",
        Contact = new OpenApiContact() { Name = "Group Radiator" },
    });
});

builder.Services.AddScoped<IMongoDBContext, MongoDBContext>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<IFollowerRepository, FollowerRepository>();
builder.Services.AddScoped<DataInitializer>();

var app = builder.Build();

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
    app.SeedDB();
}

// Cross-origin Request Blocked
app.UseCors(x => x
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .SetIsOriginAllowed(origin => true) // allow any origin
                    .AllowCredentials());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program {}

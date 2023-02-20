using MiniTwit.Core;
using MiniTwit.Core.IRepositories;
using MiniTwit.Infrastructure;
using MiniTwit.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add user-secrets if running in container
builder.Configuration.AddKeyPerFile("/run/secrets", optional: true);

// Add services to the container.

// Configure MongoDB
var dbSettings = builder.Configuration.GetSection(nameof(MiniTwitDatabaseSettings));
builder.Services.Configure<MiniTwitDatabaseSettings>(dbSettings);
builder.Services.Configure<MiniTwitDatabaseSettings>(options => options.ConnectionString = builder.Configuration.GetConnectionString("MiniTwit")!);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IMongoDBContext, MongoDBContext>();
builder.Services.AddScoped<IMongoDBRepository, MongoDBRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "MiniTwit API");
        options.DocumentTitle = "Swagger - MiniTwit API";
        options.RoutePrefix = string.Empty;
        options.DisplayRequestDuration();
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

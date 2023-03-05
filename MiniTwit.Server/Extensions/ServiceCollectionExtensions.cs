using System.Reflection;
using Microsoft.OpenApi.Models;
using MiniTwit.Server.Authentication;

namespace MiniTwit.Server.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo()
            {
                Title = "MiniTwit API",
                Version = "v1",
                Description = "A refactor of a Twitter clone handed out in the elective course DevOps on the IT University of Copenhagen.",
                Contact = new OpenApiContact() { Name = "Group Radiator" },
            });

            // Add Basic Authentication option for API explorer
            options.AddSecurityDefinition("Basic Authentication", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                In = ParameterLocation.Header,
                Description = "Basic access authentication using the base64 encoding of a username and password joined by a single colon.",
                Scheme = "basic"
            });
            options.OperationFilter<SwaggerAuthOperationFilter>();

            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });

        return services;
    }
}

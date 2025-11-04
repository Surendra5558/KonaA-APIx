using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

namespace KonaAI.Master.API.Extensions;

/// <summary>
/// Provides extension methods for configuring Swagger in the API.
/// </summary>
public static class SwaggerExtension
{
    /// <summary>
    /// Adds and configures Swagger services for API documentation.
    /// </summary>
    /// <param name="services">The service collection to add Swagger services to.</param>
    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(c =>
        {
            c.IgnoreObsoleteProperties();
            c.DescribeAllParametersInCamelCase();
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "KonaAI.Master.API", Version = "v1" });
            c.CustomSchemaIds(type => type.FullName);

            // Bearer/JWT security scheme (OpenAPI 3)
            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "JWT Authorization header using the Bearer scheme. Example: Bearer {token}",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme // "Bearer"
                }
            };

            c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securityScheme);

            // Make Bearer auth required by default (can be overridden per action)
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                [securityScheme] = new List<string>()
            });
        });
    }
}
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace KonaAI.Master.API.Extensions;

public static class AuthenticationExtension
{
    /// <summary>
    /// Configures JWT Bearer authentication and a policy scheme for the API.
    /// </summary>
    /// <param name="services">The service collection used to register authentication services.</param>
    /// <param name="configuration">The application configuration containing token settings under <c>Tokens:*</c>.</param>
    /// <remarks>
    /// Reads issuer, audience, signing key, and token lifetime from configuration keys:
    /// <c>Tokens:Issuer</c>, <c>Tokens:Audience</c>, <c>Tokens:Key</c>, <c>Tokens:AccessTokenExpiryInMinutes</c>.
    /// </remarks>
    public static void AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        _ = double.TryParse(configuration["Tokens:AccessTokenExpiryInMinutes"], out var expireMinutes);
        var issuer = configuration["Tokens:Issuer"]!;
        var audience = configuration["Tokens:Audience"]!;
        var key = configuration["Tokens:Key"]!;

        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = issuer,
                        ValidAudience = audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                        ClockSkew = TimeSpan.FromMinutes(expireMinutes)
                    };
                })
                .AddPolicyScheme("DefaultScheme", JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.ForwardDefaultSelector = context =>
                    {
                        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
                        if (authHeader?.StartsWith("Bearer ") != true)
                            return JwtBearerDefaults.AuthenticationScheme;

                        var jwtHandler = new JwtSecurityTokenHandler();
                        var token = jwtHandler.ReadJwtToken(authHeader["Bearer ".Length..]);

                        if (token.ValidTo >= DateTime.UtcNow)
                            return JwtBearerDefaults.AuthenticationScheme;

                        return JwtBearerDefaults.AuthenticationScheme;
                    };
                });
    }
}
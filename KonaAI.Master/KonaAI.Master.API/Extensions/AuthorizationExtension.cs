using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace KonaAI.Master.API.Extensions;

/// <summary>
/// Extension methods for configuring authorization in the KonaAI Master API.
/// </summary>
/// <remarks>
/// Registers a default authorization policy that requires authenticated users
/// using the JWT Bearer authentication scheme.
/// </remarks>
public static class AuthorizationExtension
{
    /// <summary>
    /// Registers authorization and sets the default policy to require an authenticated user.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to configure.</param>
    public static void AddAuthorizationPolicies(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder(
                JwtBearerDefaults.AuthenticationScheme);
            defaultAuthorizationPolicyBuilder = defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();
            options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();
        });
    }
}
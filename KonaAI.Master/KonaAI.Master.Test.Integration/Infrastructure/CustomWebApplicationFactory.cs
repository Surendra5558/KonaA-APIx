using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using KonaAI.Master.Repository;
using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Repository.Common.Model;
using KonaAI.Master.API;
using KonaAI.Master.API.Extensions;
using KonaAI.Master.API.Model;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;

namespace KonaAI.Master.Test.Integration.Infrastructure;

/// <summary>
/// Custom WebApplicationFactory for integration testing with in-memory database and test services.
/// Boots the real Program pipeline and only swaps infrastructure services needed for tests.
/// </summary>
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove the existing DbContext registration
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<DefaultContext>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            // Add in-memory database for testing
            services.AddDbContext<DefaultContext>(options =>
            {
                options.UseInMemoryDatabase("TestDb");
                options.EnableSensitiveDataLogging();
                options.EnableServiceProviderCaching();
            });

            // Replace IUserContextService with test implementation
            services.AddScoped<IUserContextService, TestUserContextService>();

            // Ensure our TestAuth scheme is the only one used during tests
            services.RemoveAll<IAuthenticationSchemeProvider>();
            services.AddSingleton<IAuthenticationSchemeProvider, AuthenticationSchemeProvider>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = TestAuthHandler.SchemeName;
                options.DefaultChallengeScheme = TestAuthHandler.SchemeName;
                options.DefaultScheme = TestAuthHandler.SchemeName;
            })
            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                TestAuthHandler.SchemeName, _ => { })
            // Also register our test handler under the JwtBearer scheme name to satisfy
            // any endpoints that explicitly require JwtBearer
            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                JwtBearerDefaults.AuthenticationScheme, _ => { })
            // Register under possible policy scheme name used by the app
            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                "DefaultScheme", _ => { });

            // Permit all authorization checks during testing
            services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
                    .RequireAssertion(_ => true)
                    .Build();
            });

            // Configure test environment
            builder.UseEnvironment("Testing");
        });

        // Ensure an authenticated user is present for every request in tests
        builder.Configure(app =>
        {
            app.Use(next =>
            {
                return async context =>
                {
                    if (context.User?.Identity == null || !context.User.Identity.IsAuthenticated)
                    {
                        var identity = new ClaimsIdentity(new[]
                        {
                            new Claim(ClaimTypes.Name, "testuser@konaai.com"),
                            new Claim(ClaimTypes.Email, "testuser@konaai.com"),
                            new Claim(ClaimTypes.Role, "Admin"),
                            new Claim("ClientId", "1")
                        }, TestAuthHandler.SchemeName);
                        context.User = new ClaimsPrincipal(identity);
                    }
                    await next(context);
                };
            });
        });
    }

    // Use the application's real Program wiring; no additional service registrations here.
    // Overriding CreateHost is unnecessary for our scenario; rely on Program.cs configuration.

    /// <summary>
    /// Creates a test client with authentication token.
    /// </summary>
    public async Task<HttpClient> CreateAuthenticatedClientAsync()
    {
        var client = CreateClient();

        // Get authentication token
        var token = await TestHelpers.GetAuthTokenAsync(client);
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        return client;
    }

    /// <summary>
    /// Creates a test client with specific user context.
    /// </summary>
    public HttpClient CreateClientWithUserContext(long clientId = 1, string userId = "testuser")
    {
        var client = CreateClient();

        // Add user context headers
        client.DefaultRequestHeaders.Add("X-Client-Id", clientId.ToString());
        client.DefaultRequestHeaders.Add("X-User-Id", userId);

        return client;
    }
}

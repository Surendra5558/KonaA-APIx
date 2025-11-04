using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using KonaAI.Master.Repository;
using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Repository.Common.Model;
using KonaAI.Master.API.Extensions;
using KonaAI.Master.API.Model;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.OpenApi;
using Serilog;

namespace KonaAI.Master.Test.Integration.Infrastructure;

/// <summary>
/// Custom WebApplicationFactory for integration testing with in-memory database and test services.
/// Boots the real Program pipeline to mirror production startup closely.
/// </summary>
public class TestWebApplicationFactory : WebApplicationFactory<Program>
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

            // Configure test environment
            builder.UseEnvironment("Testing");
        });
    }

    // No CreateHost override; rely on Program.cs to configure middleware and services.

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

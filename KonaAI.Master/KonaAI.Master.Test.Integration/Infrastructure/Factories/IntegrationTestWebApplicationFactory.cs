using KonaAI.Master.API;
using KonaAI.Master.Repository;
using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Test.Integration.Infrastructure.Fixtures;
using KonaAI.Master.Test.Integration.Infrastructure.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace KonaAI.Master.Test.Integration.Infrastructure.Factories;

/// <summary>
/// Custom WebApplicationFactory for integration testing that uses Program
/// to avoid database provider conflicts.
/// </summary>
public class IntegrationTestWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly InMemoryDatabaseFixture _dbFixture;

    public IntegrationTestWebApplicationFactory(InMemoryDatabaseFixture dbFixture)
    {
        _dbFixture = dbFixture;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            // Remove existing IUserContextService registrations
            var userContextDescriptors = services.Where(d => d.ServiceType == typeof(IUserContextService)).ToList();
            foreach (var userContextDescriptor in userContextDescriptors)
            {
                services.Remove(userContextDescriptor);
            }

            // Register TestUserContextService for testing
            services.AddScoped<IUserContextService, TestUserContextService>();

            // Configure authentication for testing
            services.AddAuthentication("Test")
                .AddScheme<AuthenticationSchemeOptions, TestAuthenticationHandler>("Test", options => { });

            // Set Test as the default authentication scheme
            services.Configure<AuthenticationOptions>(options =>
            {
                options.DefaultAuthenticateScheme = "Test";
                options.DefaultChallengeScheme = "Test";
                options.DefaultScheme = "Test";
            });

            // Add test authorization policy provider that respects authentication
            services.AddSingleton<IAuthorizationPolicyProvider, TestAuthorizationPolicyProvider>();
            services.AddScoped<IAuthorizationHandler, TestAuthorizationHandler>();
            services.AddHttpContextAccessor();

            // Remove any existing DbContext registrations
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<DefaultContext>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            var defaultContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DefaultContext));
            if (defaultContextDescriptor != null)
            {
                services.Remove(defaultContextDescriptor);
            }

            // Add in-memory database using shared options from the fixture
            // This ensures all contexts share the same in-memory database instance
            var sharedOptions = _dbFixture.GetDbContextOptions();
            services.AddSingleton(sharedOptions);
            services.AddScoped(provider => new TestDbContext(sharedOptions, provider.GetRequiredService<IUserContextService>()));

            // Register TestDbContext as DefaultContext for compatibility
            services.AddScoped<DefaultContext>(provider =>
            {
                var testDbContext = provider.GetRequiredService<TestDbContext>();
                return new TestDbContextWrapper(testDbContext);
            });

            // Register IUnitOfWork for business logic (uses TestDbContext)
            services.AddScoped<IUnitOfWork, TestUnitOfWork>();
        });
    }

    /// <summary>
    /// Gets a TestDbContext instance for direct database operations.
    /// </summary>
    public TestDbContext GetTestDbContext()
    {
        return _dbFixture.CreateContext();
    }

    /// <summary>
    /// Seeds the database with test data.
    /// </summary>
    public async Task SeedDatabaseAsync()
    {
        using var context = _dbFixture.CreateContext();
        await _dbFixture.SeedTestDataAsync();
    }

    /// <summary>
    /// Clears the database.
    /// </summary>
    public async Task ClearDatabaseAsync()
    {
        await _dbFixture.ClearDatabaseAsync();
    }

    /// <summary>
    /// Creates an authenticated HTTP client.
    /// </summary>
    public async Task<HttpClient> CreateAuthenticatedClientAsync()
    {
        var client = CreateClient();

        // Add authentication headers
        client.DefaultRequestHeaders.Add("Authorization", "Bearer test-token");
        client.DefaultRequestHeaders.Add("X-Client-Id", "1");
        client.DefaultRequestHeaders.Add("X-User-Id", "testuser");

        return client;
    }

    /// <summary>
    /// Creates an HTTP client with user context.
    /// </summary>
    public HttpClient CreateClientWithUserContext(long clientId, string userId = "testuser", string role = "Admin")
    {
        var client = CreateClient();
        // Add user context and auth headers so the test auth handler and user context can pick them up
        client.DefaultRequestHeaders.Add("Authorization", "Bearer test-token");
        client.DefaultRequestHeaders.Add("X-Client-Id", clientId.ToString());
        client.DefaultRequestHeaders.Add("X-User-Id", userId);
        client.DefaultRequestHeaders.Add("X-User-Role", role);
        return client;
    }
}
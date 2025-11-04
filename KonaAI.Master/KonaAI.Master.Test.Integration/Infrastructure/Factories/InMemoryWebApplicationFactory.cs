using KonaAI.Master.API;
using KonaAI.Master.Repository;
using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Test.Integration.Infrastructure.Fixtures;
using KonaAI.Master.Test.Integration.Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FluentValidation;
using KonaAI.Master.Model.Master.App.SaveModel;
using KonaAI.Master.Model.Tenant.Client.SaveModel;
using KonaAI.Master.Model.Master.SaveModel;
using KonaAI.Master.Model.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;

namespace KonaAI.Master.Test.Integration.Infrastructure.Factories;

/// <summary>
/// Custom WebApplicationFactory for integration testing that uses only in-memory database.
/// No Docker dependencies.
/// </summary>
public class InMemoryWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly InMemoryDatabaseFixture _dbFixture;

    public InMemoryWebApplicationFactory(InMemoryDatabaseFixture dbFixture)
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

            // Explicitly wire business services required by OData smoke tests
            services.AddScoped<IRenderTypeBusiness, KonaAI.Master.Business.Master.MetaData.Logic.RenderTypeBusiness>();

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

            // Register FluentValidation validators (same as main application)
            services.AddValidatorsFromAssemblyContaining<QuestionBankCreateModelValidator>();
            services.AddValidatorsFromAssemblyContaining<ClientQuestionnaireCreateModelValidator>();
            services.AddValidatorsFromAssemblyContaining<ClientQuestionBankCreateModelValidator>();
            services.AddValidatorsFromAssemblyContaining<ClientProjectCreateModel>();
            services.AddValidatorsFromAssemblyContaining<RenderTypeCreateModelValidator>();
            services.AddValidatorsFromAssemblyContaining<ClientValidator>();
            services.AddValidatorsFromAssemblyContaining<TokenRequestValidator>();
        });
    }

    /// <summary>
    /// Gets a TestDbContext instance for direct database operations.
    /// </summary>
    public TestDbContext GetTestContext()
    {
        return _dbFixture.CreateContext();
    }

    /// <summary>
    /// Seeds the database with test data.
    /// </summary>
    public async Task SeedDatabaseAsync()
    {
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
        var token = await TestHelpers.GetAuthTokenAsync(client);
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        return client;
    }

    /// <summary>
    /// Creates an HTTP client with user context.
    /// </summary>
    public HttpClient CreateClientWithUserContext(long clientId, string userId = "testuser", string role = "Admin")
    {
        var client = CreateClient();
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "mock-test-token-12345");
        client.DefaultRequestHeaders.Add("X-Client-Id", clientId.ToString());
        client.DefaultRequestHeaders.Add("X-User-Id", userId);
        client.DefaultRequestHeaders.Add("X-User-Role", role);
        return client;
    }

    /// <summary>
    /// Creates an authenticated HTTP client with user context.
    /// </summary>
    public async Task<HttpClient> CreateAuthenticatedClientWithUserContext(long clientId, string userId = "testuser", string role = "Admin")
    {
        var client = CreateClient();
        var token = await TestHelpers.GetAuthTokenAsync(client);
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        client.DefaultRequestHeaders.Add("X-Client-Id", clientId.ToString());
        client.DefaultRequestHeaders.Add("X-User-Id", userId);
        client.DefaultRequestHeaders.Add("X-User-Role", role);
        return client;
    }
}

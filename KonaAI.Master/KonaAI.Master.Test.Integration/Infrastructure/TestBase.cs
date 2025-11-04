using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using KonaAI.Master.Repository;
using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Test.Integration.Infrastructure.Factories;
using KonaAI.Master.Test.Integration.Infrastructure.Fixtures;
using KonaAI.Master.Test.Integration.Infrastructure.Helpers;
using Moq;

namespace KonaAI.Master.Test.Integration.Infrastructure;

/// <summary>
/// Base class for integration tests using in-memory database and mocks.
/// Provides common setup and utilities for all integration tests.
/// </summary>
public abstract class TestBase
{
    protected readonly InMemoryDatabaseFixture _fixture;
    protected readonly InMemoryWebApplicationFactory _factory;
    protected readonly HttpClient _client;

    protected TestBase(InMemoryDatabaseFixture fixture)
    {
        _fixture = fixture;
        _factory = new InMemoryWebApplicationFactory(fixture);
        _client = _factory.CreateClient();
    }

    /// <summary>
    /// Gets a TestDbContext instance for direct database operations.
    /// </summary>
    protected TestDbContext GetDbContext()
    {
        return _fixture.CreateContext();
    }

    /// <summary>
    /// Seeds the database with test data.
    /// </summary>
    protected async Task SeedDatabaseAsync()
    {
        await _fixture.SeedTestDataAsync();
    }

    /// <summary>
    /// Clears the database.
    /// </summary>
    protected async Task ClearDatabaseAsync()
    {
        await _fixture.ClearDatabaseAsync();
    }

    /// <summary>
    /// Creates a mock IUserContextService with default test values.
    /// </summary>
    protected Mock<IUserContextService> CreateMockUserContextService()
    {
        var mock = new Mock<IUserContextService>();
        var userContext = new KonaAI.Master.Repository.Common.Model.UserContext
        {
            ClientId = 1,
            UserLoginName = "testuser",
            RoleId = 1,
            RoleName = "Admin"
        };
        mock.Setup(x => x.UserContext).Returns(userContext);
        return mock;
    }

    /// <summary>
    /// Creates a mock IUnitOfWork with default test setup.
    /// </summary>
    protected Mock<IUnitOfWork> CreateMockUnitOfWork()
    {
        var mock = new Mock<IUnitOfWork>();
        // Setup default return values for common operations
        mock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        return mock;
    }

    /// <summary>
    /// Creates a mock ILogger for testing.
    /// </summary>
    protected Mock<ILogger<T>> CreateMockLogger<T>()
    {
        return new Mock<ILogger<T>>();
    }

    /// <summary>
    /// Creates a service scope for dependency injection testing.
    /// </summary>
    protected IServiceScope CreateServiceScope()
    {
        var serviceProvider = _fixture.CreateServiceProvider();
        return serviceProvider.CreateScope();
    }

    /// <summary>
    /// Gets a service from the factory's service provider.
    /// </summary>
    protected T GetService<T>() where T : class
    {
        using var scope = _factory.Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<T>();
    }

    /// <summary>
    /// Creates an authenticated HTTP client with user context.
    /// </summary>
    protected HttpClient CreateAuthenticatedClient(long clientId = 1, string userId = "testuser", string role = "Admin")
    {
        var client = _factory.CreateClient();

        // Add authentication headers
        client.DefaultRequestHeaders.Add("X-Client-Id", clientId.ToString());
        client.DefaultRequestHeaders.Add("X-User-Id", userId);
        client.DefaultRequestHeaders.Add("X-Role", role);

        return client;
    }

    /// <summary>
    /// Performs cleanup after each test.
    /// </summary>
    protected virtual async Task CleanupAsync()
    {
        await ClearDatabaseAsync();
    }

    /// <summary>
    /// Disposes of resources.
    /// </summary>
    public virtual void Dispose()
    {
        _client?.Dispose();
        _factory?.Dispose();
    }
}

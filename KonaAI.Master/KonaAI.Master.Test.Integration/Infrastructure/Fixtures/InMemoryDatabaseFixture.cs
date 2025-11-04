using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using KonaAI.Master.Repository;
using KonaAI.Master.Test.Integration.Infrastructure.TestData.Seeders;
using KonaAI.Master.Test.Integration.Infrastructure;

namespace KonaAI.Master.Test.Integration.Infrastructure.Fixtures;

/// <summary>
/// In-memory database fixture for integration testing.
/// Uses only in-memory database with no Docker dependencies.
/// </summary>
public class InMemoryDatabaseFixture : IAsyncLifetime
{
    private bool _disposed = false;
    private DbContextOptions<TestDbContext>? _sharedOptions;
    private IServiceProvider? _rootServiceProvider;

    public string DatabaseName { get; private set; } = string.Empty;

    public async Task InitializeAsync()
    {
        DatabaseName = "KonaAI_Test_InMemory_" + Guid.NewGuid().ToString("N")[..8];
        Console.WriteLine("Using in-memory database for integration tests.");

        // Create a root service provider for the in-memory database
        // This ensures all contexts share the same in-memory database
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddEntityFrameworkInMemoryDatabase();
        _rootServiceProvider = serviceCollection.BuildServiceProvider();

        // Create shared options once at initialization using the root service provider
        var optionsBuilder = new DbContextOptionsBuilder<TestDbContext>();
        optionsBuilder.UseInMemoryDatabase(DatabaseName);
        optionsBuilder.UseInternalServiceProvider(_rootServiceProvider);
        optionsBuilder.EnableSensitiveDataLogging();
        _sharedOptions = optionsBuilder.Options;

        // Seed test data immediately after initialization
        await SeedTestDataAsync();
        Console.WriteLine("Test data seeded successfully.");
    }

    public async Task DisposeAsync()
    {
        if (_disposed) return;
        _disposed = true;
    }

    /// <summary>
    /// Gets the shared DbContextOptions for all contexts to ensure they use the same in-memory database.
    /// </summary>
    public DbContextOptions<TestDbContext> GetDbContextOptions()
    {
        if (_sharedOptions == null)
            throw new InvalidOperationException("Database not initialized. Call InitializeAsync first.");
        return _sharedOptions;
    }

    /// <summary>
    /// Creates a new DbContext with the shared in-memory database options.
    /// </summary>
    public TestDbContext CreateContext()
    {
        return new TestDbContext(GetDbContextOptions(), new TestUserContextService());
    }

    /// <summary>
    /// Creates a scoped service provider for dependency injection testing.
    /// </summary>
    public IServiceProvider CreateServiceProvider()
    {
        var services = new ServiceCollection();

        // Add DbContext with in-memory database
        services.AddDbContext<TestDbContext>(options =>
            options.UseInMemoryDatabase(DatabaseName));

        // Add test services
        services.AddScoped<TestUserContextService>();

        return services.BuildServiceProvider();
    }

    /// <summary>
    /// Clears all data from the test database.
    /// </summary>
    public async Task ClearDatabaseAsync()
    {
        using var context = CreateContext();

        // For in-memory database, manually clear all DbSets
        // This is more reliable than EnsureDeleted/EnsureCreated for in-memory databases

        // Clear all entities in reverse order of dependencies
        context.ClientQuestionBanks.RemoveRange(context.ClientQuestionBanks);
        context.ClientQuestionnaires.RemoveRange(context.ClientQuestionnaires);
        context.ClientUsers.RemoveRange(context.ClientUsers);
        context.ClientProjects.RemoveRange(context.ClientProjects);
        context.ClientLicenses.RemoveRange(context.ClientLicenses);
        context.ClientRoleTypes.RemoveRange(context.ClientRoleTypes);
        context.Clients.RemoveRange(context.Clients);

        context.Users.RemoveRange(context.Users);

        context.RenderTypes.RemoveRange(context.RenderTypes);
        context.ProjectAuditResponsibilities.RemoveRange(context.ProjectAuditResponsibilities);
        context.ProjectRiskAreas.RemoveRange(context.ProjectRiskAreas);
        context.ProjectUnits.RemoveRange(context.ProjectUnits);
        context.ProjectDepartments.RemoveRange(context.ProjectDepartments);
        context.Navigations.RemoveRange(context.Navigations);
        context.RoleTypes.RemoveRange(context.RoleTypes);
        context.ModuleTypes.RemoveRange(context.ModuleTypes);
        context.Countries.RemoveRange(context.Countries);

        // Clear user permission entities
        context.RoleNavigationUserActions.RemoveRange(context.RoleNavigationUserActions);
        context.NavigationUserActions.RemoveRange(context.NavigationUserActions);
        context.UserActions.RemoveRange(context.UserActions);

        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Seeds the database with test data.
    /// </summary>
    public async Task SeedTestDataAsync()
    {
        using var context = CreateContext();
        var seeder = new TestDataSeeder();
        await seeder.SeedAsync(context);
    }

    /// <summary>
    /// Gets the database type being used for testing.
    /// </summary>
    public string GetDatabaseType()
    {
        return "In-Memory Database";
    }
}

/// <summary>
/// Collection definition for in-memory database tests to ensure proper cleanup.
/// </summary>
[CollectionDefinition("InMemoryDatabaseCollection")]
public class InMemoryDatabaseCollection : ICollectionFixture<InMemoryDatabaseFixture>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}

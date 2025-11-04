using KonaAI.Master.Repository;
using KonaAI.Master.Test.Integration.Infrastructure.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace KonaAI.Master.Test.Integration.Infrastructure;

/// <summary>
/// Extension methods for configuring services in integration tests.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Configures the DbContext using in-memory database for testing.
    /// </summary>
    public static IServiceCollection ConfigureDbContext(this IServiceCollection services, InMemoryDatabaseFixture dbFixture)
    {
        // Register TestDbContext with in-memory database
        services.AddDbContext<TestDbContext>(options =>
            options.UseInMemoryDatabase(dbFixture.DatabaseName));

        return services;
    }
}
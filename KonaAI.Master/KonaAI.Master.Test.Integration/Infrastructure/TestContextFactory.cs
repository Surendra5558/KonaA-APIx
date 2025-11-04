using KonaAI.Master.Repository;
using Microsoft.EntityFrameworkCore;

namespace KonaAI.Master.Test.Integration.Infrastructure;

/// <summary>
/// Factory for creating TestContext instances with test-specific configurations.
/// This factory completely bypasses the OnConfiguring method that causes database provider conflicts.
/// </summary>
public class TestContextFactory
{
    private readonly string _connectionString;
    private readonly string _databaseName;
    private readonly bool _useTestcontainers;

    public TestContextFactory(string connectionString, string databaseName, bool useTestcontainers)
    {
        _connectionString = connectionString;
        _databaseName = databaseName;
        _useTestcontainers = useTestcontainers;
    }

    /// <summary>
    /// Creates a new TestContext instance with test-specific configuration.
    /// This completely avoids the OnConfiguring method that causes database provider conflicts.
    /// </summary>
    public TestDbContext CreateContext()
    {
        var optionsBuilder = new DbContextOptionsBuilder<TestDbContext>();

        if (_useTestcontainers)
        {
            // Use real SQL Server
            optionsBuilder.UseSqlServer(_connectionString, sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(3);
                sqlOptions.CommandTimeout(30);
            });
        }
        else
        {
            // Use in-memory database
            optionsBuilder.UseInMemoryDatabase(_databaseName);
        }

        optionsBuilder.EnableSensitiveDataLogging()
                      .EnableServiceProviderCaching(false);

        // Create the context with the configured options
        var options = optionsBuilder.Options;
        return new TestDbContext(options, new TestUserContextService());
    }
}

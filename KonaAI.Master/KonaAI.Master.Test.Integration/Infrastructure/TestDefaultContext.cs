using KonaAI.Master.Repository;
using KonaAI.Master.Repository.Common.Interface;
using Microsoft.EntityFrameworkCore;

namespace KonaAI.Master.Test.Integration.Infrastructure;

/// <summary>
/// Test-specific DefaultContext that disables OnConfiguring to prevent database provider conflicts.
/// This context is used in integration tests to avoid the multiple database provider issue.
/// </summary>
public class TestDefaultContext : DefaultContext
{
    public TestDefaultContext()
    {
    }

    public TestDefaultContext(DbContextOptions<DefaultContext> options, IUserContextService userContextService)
        : base(options, userContextService)
    {
    }

    /// <summary>
    /// Override OnConfiguring to do nothing in tests.
    /// This prevents the default SQL Server configuration from being applied
    /// when test-specific configurations are provided.
    /// </summary>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Do nothing - let the test configuration handle database provider setup
        // This prevents the multiple database provider conflict
    }
}

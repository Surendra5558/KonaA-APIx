using KonaAI.Master.Repository;
using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Repository.Domain.Master.App;
using KonaAI.Master.Repository.Domain.Master.MetaData;
using KonaAI.Master.Repository.Domain.Master.UserMetaData;
using KonaAI.Master.Repository.Domain.Tenant.Client;
using Microsoft.EntityFrameworkCore;

namespace KonaAI.Master.Test.Integration.Infrastructure;

/// <summary>
/// Wrapper that makes TestDbContext compatible with DefaultContext for dependency injection.
/// This allows repositories to work with TestDbContext while maintaining the same interface.
/// </summary>
public class TestDbContextWrapper : DefaultContext
{
    private readonly TestDbContext _testDbContext;

    public TestDbContextWrapper(TestDbContext testDbContext)
        : base()
    {
        _testDbContext = testDbContext;
    }

    // Override key methods to delegate to TestDbContext
    public override int SaveChanges()
    {
        return _testDbContext.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _testDbContext.SaveChangesAsync(cancellationToken);
    }

    public override DbSet<TEntity> Set<TEntity>() where TEntity : class
    {
        return _testDbContext.Set<TEntity>();
    }

    public override Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class
    {
        return _testDbContext.Entry(entity);
    }

    public override Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry Entry(object entity)
    {
        return _testDbContext.Entry(entity);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Do nothing - TestDbContext handles its own configuration
    }
}

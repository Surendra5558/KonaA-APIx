using KonaAI.Master.Repository;
using KonaAI.Master.Repository.Common.Constants;
using KonaAI.Master.Repository.Common.Domain;
using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Repository.Domain.Master.App;
using KonaAI.Master.Repository.Domain.Master.MetaData;
using KonaAI.Master.Repository.Domain.Master.UserMetaData;
using KonaAI.Master.Repository.Domain.Tenant.Client;
using KonaAI.Master.Repository.Domain.Tenant.ClientMetaData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace KonaAI.Master.Test.Integration.Infrastructure;

/// <summary>
/// A test-specific DbContext that provides the same interface as DefaultContext
/// but without the problematic OnConfiguring method that causes database provider conflicts.
/// </summary>
public class TestDbContext : DbContext
{
    private readonly IUserContextService _userContextService;
    private readonly long? _clientId;

    public TestDbContext(DbContextOptions<TestDbContext> options, IUserContextService userContextService)
        : base(options)
    {
        _userContextService = userContextService;
        _clientId = _userContextService.UserContext?.ClientId;
    }

    // Define all the DbSets that DefaultContext has
    public DbSet<Client> Clients { get; set; }
    public DbSet<ClientLicense> ClientLicenses { get; set; }
    public DbSet<ClientProject> ClientProjects { get; set; }
    public DbSet<ClientUser> ClientUsers { get; set; }
    public DbSet<ClientQuestionnaire> ClientQuestionnaires { get; set; }
    public DbSet<ClientQuestionBank> ClientQuestionBanks { get; set; }
    public DbSet<ClientRoleType> ClientRoleTypes { get; set; }

    public DbSet<Country> Countries { get; set; }
    public DbSet<ModuleType> ModuleTypes { get; set; }
    public DbSet<RoleType> RoleTypes { get; set; }
    public DbSet<Navigation> Navigations { get; set; }
    public DbSet<ProjectDepartment> ProjectDepartments { get; set; }
    public DbSet<ProjectUnit> ProjectUnits { get; set; }
    public DbSet<ProjectRiskArea> ProjectRiskAreas { get; set; }
    public DbSet<ProjectAuditResponsibility> ProjectAuditResponsibilities { get; set; }
    public DbSet<RenderType> RenderTypes { get; set; }

    // User permission entities
    public DbSet<UserAction> UserActions { get; set; }
    public DbSet<NavigationUserAction> NavigationUserActions { get; set; }
    public DbSet<RoleNavigationUserAction> RoleNavigationUserActions { get; set; }

    public DbSet<User> Users { get; set; }
    public DbSet<UserAudit> UserAudits { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Apply the same configurations as DefaultContext
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DefaultContext).Assembly);

        // Apply multi-tenancy filters if needed
        if (_userContextService != null)
        {
            if (_clientId != null)
            {
                Expression<Func<BaseClientDomain, bool>> filterExpr = e => e.ClientId == _clientId;
                foreach (var entityType in modelBuilder.Model.GetEntityTypes())
                {
                    if (!entityType.ClrType.IsAssignableTo(typeof(BaseClientDomain)))
                        continue;

                    var parameter = Expression.Parameter(entityType.ClrType);
                    var body = ReplacingExpressionVisitor.Replace(filterExpr.Parameters.First(), parameter, filterExpr.Body);
                    var lambda = Expression.Lambda(body, parameter);
                    entityType.SetQueryFilter(lambda);
                }
            }
        }

        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyAuditFields();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
        ApplyAuditFields();
        return base.SaveChanges();
    }

    private void ApplyAuditFields()
    {
        var entries = ChangeTracker.Entries<BaseDomain>();
        foreach (var entry in entries)
        {
            var dataMode = entry.State switch
            {
                EntityState.Added => DataModes.Add,
                EntityState.Modified => DataModes.Edit,
                EntityState.Deleted => DataModes.Delete,
                _ => DataModes.Edit
            };
            _userContextService.SetDomainDefaults(entry.Entity, dataMode);

            // Explicitly set the state to ensure EF Core tracks it correctly
            if (entry.State == EntityState.Detached)
            {
                entry.State = EntityState.Added;
            }
        }
    }

}

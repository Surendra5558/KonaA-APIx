using KonaAI.Master.Repository.Common.Domain;
using KonaAI.Master.Repository.Common.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Configuration;
using System.Linq.Expressions;
using System.Reflection;

namespace KonaAI.Master.Repository;

/// <summary>
/// Represents the Entity Framework Core database context for the application.
/// </summary>
public partial class DefaultContext : DbContext
{
    /// <summary>
    /// The client identifier
    /// </summary>
    private readonly long? _clientId;

    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultContext"/> class.
    /// </summary>
    public DefaultContext()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultContext"/> class
    /// using the specified options.
    /// </summary>
    /// <param name="options">The options to be used by the DbContext.</param>
    /// <param name="userContextService">The user context service providing the current tenant/client context.</param>
    public DefaultContext(DbContextOptions<DefaultContext> options, IUserContextService userContextService)
        : base(options)
    {
        _clientId = userContextService.UserContext?.ClientId;
    }

    /// <summary>
    /// Configures the database (and other options) for this context.
    /// Reads the connection string <c>DefaultConnection</c> from <c>appsettings.json</c>.
    /// Only configures if no options have been provided (e.g., for design-time scenarios).
    /// </summary>
    /// <param name="optionsBuilder">A builder used to create or modify options for this context.</param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Only configure if no options have been provided (e.g., for design-time scenarios)
        if (!optionsBuilder.IsConfigured)
        {
            var configBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json");
            var configuration = configBuilder.Build();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString)
                .ConfigureWarnings(warnings => { warnings.Ignore(RelationalEventId.PendingModelChangesWarning); });
        }
    }

    /// <summary>
    /// Further configures the model that was discovered by convention.
    /// Applies configurations from the executing assembly and sets a global tenant filter for <see cref="BaseClientDomain"/> entities.
    /// </summary>
    /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Apply all configurations from the current assembly
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        if (_clientId != null)
        {
            // Clients.First(x=>x.RowId == )
            Expression<Func<BaseClientDomain, bool>> filterExpr = e => e.ClientId == _clientId;

            foreach (var mutableEntityType in modelBuilder.Model.GetEntityTypes())
            {
                // check if current entity type is child of BaseModel
                if (!mutableEntityType.ClrType.IsAssignableTo(typeof(BaseClientDomain)))
                    continue;

                // modify expression to handle correct child type
                var parameter = Expression.Parameter(mutableEntityType.ClrType);
                var body = ReplacingExpressionVisitor.Replace(filterExpr.Parameters.First(),
                    parameter, filterExpr.Body);
                var lambdaExpression = Expression.Lambda(body, parameter);

                // set filter
                mutableEntityType.SetQueryFilter(lambdaExpression);
            }
        }

        base.OnModelCreating(modelBuilder);
    }
}
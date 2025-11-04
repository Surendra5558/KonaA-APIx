using KonaAI.Master.Repository.Common;
using KonaAI.Master.Repository.DataAccess.Tenant.ClientUserMetaData.Interface;
using KonaAI.Master.Repository.Domain.Tenant.ClientUserMetaData;

namespace KonaAI.Master.Repository.DataAccess.Tenant.ClientUserMetaData;

/// <summary>
/// Repository implementation for managing <see cref="ClientProjectCountry"/> metadata.
/// </summary>
/// <remarks>
/// Inherits common CRUD and query operations from <see cref="GenericRepository{TContext, TEntity}"/>,
/// using <see cref="DefaultContext"/> as the EF Core context.
/// </remarks>
/// <param name="context">The EF Core <see cref="DefaultContext"/> instance used for data access.</param>
/// <seealso cref="GenericRepository{DefaultContext, ClientProjectCountry}"/>
/// <seealso cref="IClientProjectCountryRepository"/>
/// <seealso cref="ClientProjectCountry"/>
public class ClientProjectCountryRepository(DefaultContext context)
    : GenericRepository<DefaultContext, ClientProjectCountry>(context),
        IClientProjectCountryRepository
{
}
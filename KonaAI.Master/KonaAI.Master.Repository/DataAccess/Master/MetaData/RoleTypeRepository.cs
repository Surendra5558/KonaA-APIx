using KonaAI.Master.Repository.Common;
using KonaAI.Master.Repository.DataAccess.Master.MetaData.Interface;
using KonaAI.Master.Repository.Domain.Master.MetaData;

namespace KonaAI.Master.Repository.DataAccess.Master.MetaData;

/// <summary>
/// Concrete repository for <see cref="RoleType"/> entities.
/// </summary>
/// <remarks>
/// Provides CRUD operations via <see cref="GenericRepository{DefaultContext, RoleType}"/> and
/// implements the contract defined by <see cref="IRoleTypeRepository"/>.
/// </remarks>
/// <param name="context">The EF Core <see cref="DefaultContext"/> used for data access.</param>
/// <seealso cref="IRoleTypeRepository"/>
/// <seealso cref="GenericRepository{DefaultContext, RoleType}"/>
/// <seealso cref="RoleType"/>
/// <seealso cref="DefaultContext"/>
public class RoleTypeRepository(DefaultContext context)
    : GenericRepository<DefaultContext, RoleType>(context), IRoleTypeRepository
{
}
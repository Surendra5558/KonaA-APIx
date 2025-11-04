using KonaAI.Master.Repository.Common;
using KonaAI.Master.Repository.DataAccess.Master.MetaData.Interface;
using KonaAI.Master.Repository.Domain.Master.MetaData;

namespace KonaAI.Master.Repository.DataAccess.Master.MetaData;

/// <summary>
/// Concrete repository for <see cref="ModuleSourceType"/> entities.
/// </summary>
/// <remarks>
/// Inherits common CRUD operations from <see cref="GenericRepository{DefaultContext, ModuleSourceType}"/>
/// and implements <see cref="IModuleSourceTypeRepository"/>.
/// </remarks>
/// <param name="context">The EF Core <see cref="DefaultContext"/> used for data access.</param>
/// <seealso cref="IModuleSourceTypeRepository"/>
/// <seealso cref="GenericRepository{DefaultContext, ModuleSourceType}"/>
/// <seealso cref="ModuleSourceType"/>
/// <seealso cref="DefaultContext"/>
public class ModuleSourceTypeRepository(DefaultContext context)
    : GenericRepository<DefaultContext, ModuleSourceType>(context), IModuleSourceTypeRepository
{
}
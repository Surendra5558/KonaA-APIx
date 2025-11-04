using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Repository.Domain.Master.MetaData;

namespace KonaAI.Master.Repository.DataAccess.Master.MetaData.Interface;

/// <summary>
/// Repository contract for managing <see cref="ModuleSourceType"/> metadata entities.
/// </summary>
/// <remarks>
/// Inherits CRUD operations from <see cref="IRepository{DefaultContext, ModuleSourceType}"/>.
/// </remarks>
/// <seealso cref="ModuleSourceType"/>
/// <seealso cref="DefaultContext"/>
/// <seealso cref="IRepository{DefaultContext, ModuleSourceType}"/>
public interface IModuleSourceTypeRepository : IRepository<DefaultContext, ModuleSourceType>
{
}
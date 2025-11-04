using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Repository.Domain.Master.MetaData;

namespace KonaAI.Master.Repository.DataAccess.Master.MetaData.Interface;

/// <summary>
/// Defines repository operations for <see cref="ModuleType"/> entities within the <see cref="DefaultContext"/>.
/// Inherits standard CRUD and query methods from <see cref="IRepository{DefaultContext, Module}"/>,
/// including asynchronous retrieval, addition, update, and deletion of <see cref="ModuleType"/> records.
/// </summary>
public interface IModuleTypeRepository : IRepository<DefaultContext, ModuleType>
{
}
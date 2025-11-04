using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Repository.Domain.Master.MetaData;

namespace KonaAI.Master.Repository.DataAccess.Master.MetaData.Interface;

/// <summary>
/// Defines repository operations for <see cref="UserAction"/> entities within the <see cref="DefaultContext"/>.
/// Inherits standard CRUD and query methods from <see cref="IRepository{DefaultContext, Action}"/>,
/// including asynchronous retrieval, addition, update, and deletion of <see cref="UserAction"/> records.
/// </summary>
public interface IUserActionRepository : IRepository<DefaultContext, UserAction>
{
    // Add custom methods if needed, for example:
    // Task<IEnumerable<Action>> GetActionsByStatusAsync(bool isActive);
}
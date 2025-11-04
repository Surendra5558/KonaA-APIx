using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Repository.Domain.Master.App;

namespace KonaAI.Master.Repository.DataAccess.Master.App.Interface;

/// <summary>
/// Defines repository operations for <see cref="User"/> entities within the <see cref="DefaultContext"/>.
/// Inherits standard CRUD and query methods from <see cref="IRepository{DefaultContext, User}"/>,
/// including asynchronous retrieval, addition, update, and deletion of <see cref="User"/> records.
/// </summary>
public interface IUserRepository : IRepository<DefaultContext, User>
{
}
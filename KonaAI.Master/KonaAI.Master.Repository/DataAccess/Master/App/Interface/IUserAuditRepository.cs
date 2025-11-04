using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Repository.Domain.Master.App;

namespace KonaAI.Master.Repository.DataAccess.Master.App.Interface;

/// <summary>
/// Defines repository operations for <see cref="UserAudit"/> entities within the <see cref="DefaultContext"/>.
/// Inherits standard CRUD and query methods from <see cref="IRepository{DefaultContext, UserAudit}"/>.
/// </summary>
public interface IUserAuditRepository : IRepository<DefaultContext, UserAudit>
{
}
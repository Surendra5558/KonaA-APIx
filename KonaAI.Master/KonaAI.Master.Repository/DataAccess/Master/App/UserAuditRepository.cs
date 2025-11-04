using KonaAI.Master.Repository.Common;
using KonaAI.Master.Repository.DataAccess.Master.App.Interface;
using KonaAI.Master.Repository.Domain.Master.App;

namespace KonaAI.Master.Repository.DataAccess.Master.App;

/// <summary>
/// Repository for performing CRUD operations on <see cref="UserAudit"/> entities within the <see cref="DefaultContext"/>.
/// Inherits generic data access methods from <see cref="GenericRepository{DefaultContext, ClientUser}"/>
/// and implements <see cref="IUserAuditRepository"/>.
/// </summary>
public class UserAuditRepository(DefaultContext context)
    : GenericRepository<DefaultContext, UserAudit>(context), IUserAuditRepository
{
}
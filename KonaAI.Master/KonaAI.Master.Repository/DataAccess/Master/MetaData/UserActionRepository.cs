using KonaAI.Master.Repository.Common;
using KonaAI.Master.Repository.DataAccess.Master.MetaData.Interface;
using KonaAI.Master.Repository.Domain.Master.MetaData;

namespace KonaAI.Master.Repository.DataAccess.Master.MetaData;

/// <summary>
/// Repository for performing CRUD operations on <see cref="UserAction"/> entities within the <see cref="DefaultContext"/>.
/// Inherits generic data access methods from <see cref="GenericRepository{DefaultContext, Action}"/>
/// and implements <see cref="IUserActionRepository"/>.
/// </summary>
public class UserActionRepository(DefaultContext context)
    : GenericRepository<DefaultContext, UserAction>(context), IUserActionRepository
{
}
using KonaAI.Master.Repository.Common;
using KonaAI.Master.Repository.DataAccess.Master.App.Interface;
using KonaAI.Master.Repository.Domain.Master.App;

namespace KonaAI.Master.Repository.DataAccess.Master.App;

/// <summary>
/// Repository for performing CRUD operations on <see cref="User"/> entities within the <see cref="DefaultContext"/>.
/// Inherits generic data access methods from <see cref="GenericRepository{DefaultContext, User}"/>
/// and implements <see cref="IUserRepository"/>.
/// </summary>
public class UserRepository(DefaultContext context)
    : GenericRepository<DefaultContext, User>(context), IUserRepository
{
}
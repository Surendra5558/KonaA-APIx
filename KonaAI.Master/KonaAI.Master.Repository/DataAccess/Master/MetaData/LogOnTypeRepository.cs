using KonaAI.Master.Repository.Common;
using KonaAI.Master.Repository.DataAccess.Master.MetaData.Interface;
using KonaAI.Master.Repository.Domain.Master.MetaData;

namespace KonaAI.Master.Repository.DataAccess.Master.MetaData;

/// <summary>
/// Repository for performing CRUD operations on <see cref="LogOnType"/> entities within the <see cref="DefaultContext"/>.
/// Inherits generic data access methods from <see cref="GenericRepository{DefaultContext, LogOnType}"/>
/// and implements <see cref="ILogOnTypeRepository"/>.
/// </summary>
public class LogOnTypeRepository(DefaultContext context)
    : GenericRepository<DefaultContext, LogOnType>(context), ILogOnTypeRepository
{
}
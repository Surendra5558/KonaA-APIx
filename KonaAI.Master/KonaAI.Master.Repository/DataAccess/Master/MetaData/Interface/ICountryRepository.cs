using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Repository.Domain.Master.MetaData;

namespace KonaAI.Master.Repository.DataAccess.Master.MetaData.Interface;

/// <summary>
/// Defines repository operations for <see cref="Country"/> entities within the <see cref="DefaultContext"/>.
/// Inherits standard CRUD and query methods from <see cref="IRepository{DefaultContext, Country}"/>,
/// including asynchronous retrieval, addition, update, and deletion of <see cref="Country"/> records.
/// </summary>
public interface ICountryRepository : IRepository<DefaultContext, Country>
{
}
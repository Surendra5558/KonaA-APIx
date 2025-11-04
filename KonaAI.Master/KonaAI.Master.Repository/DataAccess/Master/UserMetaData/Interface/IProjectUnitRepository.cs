using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Repository.Domain.Master.UserMetaData;

namespace KonaAI.Master.Repository.DataAccess.Master.UserMetaData.Interface;

/// <summary>
/// Defines repository operations for ProjectUnit entities within the <see cref="DefaultContext"/>.
/// Inherits standard CRUD and query methods from <see cref="IRepository{DefaultContext, BusinessUnit}"/>,
/// including asynchronous retrieval, addition, update, and deletion of ProjectUnit records.
/// </summary>
public interface IProjectUnitRepository : IRepository<DefaultContext, ProjectUnit>
{
}
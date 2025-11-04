using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Repository.Domain.Master.MetaData;

namespace KonaAI.Master.Repository.DataAccess.Master.MetaData.Interface;

/// <summary>
/// Repository contract for managing <see cref="RoleType"/> metadata entities.
/// </summary>
/// <remarks>
/// Inherits CRUD and query operations from <see cref="IRepository{DefaultContext, RoleType}"/>.
/// </remarks>
/// <seealso cref="RoleType"/>
/// <seealso cref="DefaultContext"/>
/// <seealso cref="IRepository{DefaultContext, RoleType}"/>
public interface IRoleTypeRepository : IRepository<DefaultContext, RoleType>
{
}
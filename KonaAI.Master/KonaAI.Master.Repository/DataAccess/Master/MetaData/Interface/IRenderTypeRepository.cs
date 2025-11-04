using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Repository.Domain.Master.MetaData;

namespace KonaAI.Master.Repository.DataAccess.Master.MetaData.Interface;

/// <summary>
/// Defines a contract for data access operations related to <see cref="RenderType"/>.
/// </summary>
/// <remarks>
/// This interface inherits the generic repository operations from
/// <see cref="IRepository{TContext, TEntity}"/> for the <see cref="RenderType"/> entity.
/// Implement this interface when you need to add any <see cref="RenderType"/>-specific
/// query methods beyond the generic CRUD operations.
/// </remarks>
public interface IRenderTypeRepository : IRepository<DefaultContext, RenderType>
{
    // Add RenderType-specific repository method signatures here in the future if needed.
}
using KonaAI.Master.Repository.Common;
using KonaAI.Master.Repository.DataAccess.Master.MetaData.Interface;
using KonaAI.Master.Repository.Domain.Master.MetaData;

namespace KonaAI.Master.Repository.DataAccess.Master.MetaData;

/// <summary>
/// Repository implementation for managing <see cref="RenderType"/> entities.
/// Provides data access methods by extending <see cref="GenericRepository{TContext, TEntity}"/>.
/// </summary>
/// <remarks>
/// This repository is part of the metadata module and is responsible for
/// performing CRUD operations on the <see cref="RenderType"/> entity.
/// It leverages the shared <see cref="DefaultContext"/> DbContext.
/// </remarks>
public class RenderTypeRepository(DefaultContext context)
: GenericRepository<DefaultContext, RenderType>(context), IRenderTypeRepository
{
}
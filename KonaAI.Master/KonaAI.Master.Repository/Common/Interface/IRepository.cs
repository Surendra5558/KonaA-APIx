using KonaAI.Master.Repository.Common.Domain;
using Microsoft.EntityFrameworkCore;

namespace KonaAI.Master.Repository.Common.Interface;

/// <summary>
/// Generic repository contract providing asynchronous CRUD operations and queries
/// for a given EF Core <typeparamref name="TContext"/> and entity type <typeparamref name="TEntity"/>.
/// </summary>
/// <typeparam name="TContext">The EF Core <see cref="DbContext"/> type backing the repository.</typeparam>
/// <typeparam name="TEntity">The domain entity type, constrained to <see cref="BaseDomain"/>.</typeparam>
public interface IRepository<out TContext, TEntity>
    where TEntity : BaseDomain
    where TContext : DbContext
{
    /// <summary>
    /// Gets the underlying EF Core context instance.
    /// </summary>
    public TContext Context { get; }

    /// <summary>
    /// Retrieves a queryable for all entities.
    /// </summary>
    /// <returns>
    /// A task that resolves to an <see cref="IQueryable{T}"/> for <typeparamref name="TEntity"/>.
    /// The query is deferred and can be further composed before execution.
    /// </returns>
    Task<IQueryable<TEntity>> GetAsync();

    /// <summary>
    /// Retrieves a single entity by its numeric identifier.
    /// </summary>
    /// <param name="id">The numeric identifier (primary key).</param>
    /// <returns>
    /// A task that resolves to the matching entity, or <see langword="null"/> if not found.
    /// </returns>
    Task<TEntity?> GetByIdAsync(long id);

    /// <summary>
    /// Retrieves a single entity by its row identifier (GUID).
    /// </summary>
    /// <param name="rowId">The row identifier.</param>
    /// <returns>
    /// A task that resolves to the matching entity, or <see langword="null"/> if not found.
    /// </returns>
    Task<TEntity?> GetByRowIdAsync(Guid rowId);

    /// <summary>
    /// Retrieves a queryable filtered by the provided numeric identifiers.
    /// </summary>
    /// <param name="ids">The collection of numeric identifiers.</param>
    /// <returns>
    /// A task that resolves to an <see cref="IQueryable{T}"/> for the matching entities.
    /// </returns>
    Task<IQueryable<TEntity>> GetByIdsAsync(IEnumerable<long> ids);

    /// <summary>
    /// Retrieves a queryable filtered by the provided row identifiers (GUIDs).
    /// </summary>
    /// <param name="rowIds">The collection of row identifiers.</param>
    /// <returns>
    /// A task that resolves to an <see cref="IQueryable{T}"/> for the matching entities.
    /// </returns>
    Task<IQueryable<TEntity>> GetByIdsAsync(IEnumerable<Guid> rowIds);

    /// <summary>
    /// Adds a new entity to the underlying set.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <returns>
    /// A task that resolves to the added entity (as tracked by the context).
    /// </returns>
    Task<TEntity> AddAsync(TEntity entity);

    /// <summary>
    /// Adds a range of entities to the underlying set.
    /// </summary>
    /// <param name="entities">The entities to add.</param>
    /// <returns>
    /// A task that resolves to the added entities.
    /// </returns>
    Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities);

    /// <summary>
    /// Updates an existing entity.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <returns>
    /// A task that resolves to the updated entity.
    /// </returns>
    Task<TEntity> UpdateAsync(TEntity entity);

    /// <summary>
    /// Updates a range of entities.
    /// </summary>
    /// <param name="entities">The entities to update.</param>
    /// <returns>
    /// A task that resolves to the updated entities.
    /// </returns>
    Task<IEnumerable<TEntity>> UpdateRangeAsync(IEnumerable<TEntity> entities);

    /// <summary>
    /// Deletes an entity by its numeric identifier.
    /// </summary>
    /// <param name="id">The numeric identifier (primary key).</param>
    /// <returns>
    /// A task that resolves to the deleted entity, or <see langword="null"/> if not found.
    /// </returns>
    Task<TEntity?> DeleteAsync(long id);

    /// <summary>
    /// Deletes an entity by its row identifier (GUID).
    /// </summary>
    /// <param name="rowId">The row identifier.</param>
    /// <returns>
    /// A task that resolves to the deleted entity, or <see langword="null"/> if not found.
    /// </returns>
    Task<TEntity?> DeleteAsync(Guid rowId);

    /// <summary>
    /// Deletes a range of entities.
    /// </summary>
    /// <param name="entities">The entities to delete.</param>
    /// <returns>
    /// A task that resolves to the collection of deleted entities.
    /// </returns>
    Task<IEnumerable<TEntity>> DeleteRangeAsync(IEnumerable<TEntity> entities);
}
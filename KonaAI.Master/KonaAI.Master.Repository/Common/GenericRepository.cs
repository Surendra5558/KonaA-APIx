using KonaAI.Master.Repository.Common.Domain;
using KonaAI.Master.Repository.Common.Interface;
using Microsoft.EntityFrameworkCore;

namespace KonaAI.Master.Repository.Common;

/// <summary>
/// Generic repository for performing CRUD operations on entities of type
/// <typeparamref name="TEntity"/> within a specified <typeparamref name="TContext"/>.
/// </summary>
/// <typeparam name="TContext">The EF Core <see cref="DbContext"/> type.</typeparam>
/// <typeparam name="TEntity">The domain entity type deriving from <see cref="BaseDomain"/>.</typeparam>
/// <seealso cref="IRepository{TContext,TEntity}"/>
/// <param name="context">The EF Core context instance used for data access.</param>
public class GenericRepository<TContext, TEntity>(TContext context) : IRepository<TContext, TEntity>
    where TEntity : BaseDomain
    where TContext : DbContext
{
    private readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();

    /// <summary>
    /// Gets or sets the underlying EF Core context instance.
    /// </summary>
    public TContext Context { get; set; } = context;

    /// <summary>
    /// Retrieves a queryable (no-tracking) sequence of all entities.
    /// </summary>
    /// <returns>A task that resolves to an <see cref="IQueryable{T}"/> of <typeparamref name="TEntity"/>.</returns>
    public virtual async Task<IQueryable<TEntity>> GetAsync()
    {
        return await Task.FromResult(_dbSet.AsNoTracking());
    }

    /// <summary>
    /// Retrieves a single entity by its numeric identifier.
    /// </summary>
    /// <param name="id">The numeric identifier (primary key).</param>
    /// <returns>A task that resolves to the matching entity.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the entity with the specified <paramref name="id"/> is not found.</exception>
    public virtual async Task<TEntity?> GetByIdAsync(long id)
    {
        var result = await _dbSet.FindAsync(id);
        return result ?? throw new KeyNotFoundException($"Record with KeyId: {id} not found");
    }

    /// <summary>
    /// Retrieves a single entity by its row identifier (GUID).
    /// </summary>
    /// <param name="rowId">The row identifier.</param>
    /// <returns>A task that resolves to the matching entity.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the entity with the specified <paramref name="rowId"/> is not found.</exception>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    public async Task<TEntity?> GetByRowIdAsync(Guid rowId)
    {
        var result = await _dbSet.FirstOrDefaultAsync(x => x.RowId == rowId);
        return result ?? throw new KeyNotFoundException($"Record with KeyId: {rowId} not found");
    }

    /// <summary>
    /// Retrieves a queryable sequence filtered by the provided numeric identifiers.
    /// </summary>
    /// <param name="ids">The collection of numeric identifiers.</param>
    /// <returns>A task that resolves to an <see cref="IQueryable{T}"/> for the matching entities.</returns>
    /// <exception cref="KeyNotFoundException">
    /// Thrown when none of the specified identifiers exist, or when only a subset of them exists.
    /// </exception>
    /// <exception cref="ArgumentNullException"><paramref name="ids" /> is <see langword="null" />.</exception>
    /// <exception cref="OverflowException">The number of elements in <paramref name="ids" /> is larger than
    /// <see cref="System.Int32.MaxValue">Int32.MaxValue</see>.</exception>
    /// <exception cref="OutOfMemoryException">The length of the resulting string overflows the maximum allowed length
    /// (<see cref="System.Int32.MaxValue">Int32.MaxValue</see>).</exception>
    public virtual async Task<IQueryable<TEntity>> GetByIdsAsync(IEnumerable<long> ids)
    {
        var result = _dbSet.Where(x => ids.Contains(x.Id)).AsQueryable();
        if (!result.Any())
        {
            throw new KeyNotFoundException($"Records with KeyIds: {string.Join(", ", ids)} not found");
        }

        var enumerable = ids.ToList();
        if (enumerable != null && enumerable.Count != result.Count())
        {
            throw new KeyNotFoundException($"Some records with KeyIds: {string.Join(", ", enumerable)} not found");
        }

        return await Task.FromResult(result);
    }

    /// <summary>
    /// Retrieves a queryable sequence filtered by the provided row identifiers (GUIDs).
    /// </summary>
    /// <param name="rowIds">The collection of row identifiers.</param>
    /// <returns>A task that resolves to an <see cref="IQueryable{T}"/> for the matching entities.</returns>
    /// <exception cref="KeyNotFoundException">
    /// Thrown when none of the specified identifiers exist, or when only a subset of them exists.
    /// </exception>
    /// <exception cref="ArgumentNullException"><paramref name="rowIds" /> is <see langword="null" />.</exception>
    /// <exception cref="OutOfMemoryException">The length of the resulting string overflows the maximum allowed length
    /// (<see cref="System.Int32.MaxValue">Int32.MaxValue</see>).</exception>
    /// <exception cref="OverflowException">The number of elements in <paramref name="rowIds" /> is larger than
    /// <see cref="System.Int32.MaxValue">Int32.MaxValue</see>.</exception>
    public virtual async Task<IQueryable<TEntity>> GetByIdsAsync(IEnumerable<Guid> rowIds)
    {
        var result = _dbSet.Where(x => rowIds.Contains(x.RowId)).AsQueryable();
        if (!result.Any())
        {
            throw new KeyNotFoundException($"Records with RowIds: {string.Join(", ", rowIds)} not found");
        }
        var enumerable = rowIds.ToList();
        if (enumerable.Count() != result.Count())
        {
            throw new KeyNotFoundException($"Some records with RowIds: {string.Join(", ", enumerable)} not found");
        }
        return await Task.FromResult(result);
    }

    /// <summary>
    /// Adds a new entity to the set.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <returns>A task that resolves to the added entity.</returns>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    public virtual async Task<TEntity> AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
        Context.Entry(entity).State = EntityState.Added;
        return await Task.FromResult(entity);
    }

    /// <summary>
    /// Adds a range of entities to the set.
    /// </summary>
    /// <param name="entities">The entities to add.</param>
    /// <returns>A task that resolves to the added entities.</returns>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="entities" /> is <see langword="null" />.</exception>
    public virtual async Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities)
    {
        var addRangeAsync = entities.ToList();
        await _dbSet.AddRangeAsync(addRangeAsync);
        return addRangeAsync;
    }

    /// <summary>
    /// Updates an existing entity.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <returns>A task that resolves to the updated entity.</returns>
    public virtual async Task<TEntity> UpdateAsync(TEntity entity)
    {
        _dbSet.Update(entity);
        Context.Entry(entity).State = EntityState.Modified;
        return await Task.FromResult(entity);
    }

    /// <summary>
    /// Updates a range of entities.
    /// </summary>
    /// <param name="entities">The entities to update.</param>
    /// <returns>A task that resolves to the updated entities.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="entities" /> is <see langword="null" />.</exception>

    public virtual async Task<IEnumerable<TEntity>> UpdateRangeAsync(IEnumerable<TEntity> entities)
    {
        var baseDomains = entities.ToList();
        _dbSet.UpdateRange(baseDomains);
        return await Task.FromResult(baseDomains.ToList());
    }

    /// <summary>
    /// Soft-deletes an entity by its numeric identifier by setting <see cref="BaseDomain.IsDeleted"/> to <see langword="true"/>.
    /// </summary>
    /// <param name="id">The numeric identifier (primary key).</param>
    /// <returns>A task that resolves to the soft-deleted entity.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the entity with the specified <paramref name="id"/> is not found.</exception>
    public async Task<TEntity?> DeleteAsync(long id)
    {
        var entity = await GetByIdAsync(id);
        if (entity == null)
        {
            throw new KeyNotFoundException($"Record with KeyId: {id} not found");
        }
        entity.IsDeleted = true;
        _dbSet.Update(entity);
        Context.Entry(entity).State = EntityState.Modified;
        return await Task.FromResult(entity);
    }

    /// <summary>
    /// Soft-deletes an entity by its row identifier (GUID) by setting <see cref="BaseDomain.IsDeleted"/> to <see langword="true"/>.
    /// </summary>
    /// <param name="rowId">The row identifier.</param>
    /// <returns>A task that resolves to the soft-deleted entity.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the entity with the specified <paramref name="rowId"/> is not found.</exception>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    public async Task<TEntity?> DeleteAsync(Guid rowId)
    {
        var entity = await GetByRowIdAsync(rowId);
        if (entity == null)
        {
            throw new KeyNotFoundException($"Record with KeyRowId: {rowId} not found");
        }
        entity.IsDeleted = true;
        _dbSet.Update(entity);
        Context.Entry(entity).State = EntityState.Modified;
        return await Task.FromResult(entity);
    }

    /// <summary>
    /// Updates (bulk) the provided entities. Intended for scenarios where the caller has already
    /// applied soft-delete flags or other mutations and wants to persist them in a batch.
    /// </summary>
    /// <param name="entities">The entities to update.</param>
    /// <returns>A task that resolves to the updated entities.</returns>
    public async Task<IEnumerable<TEntity>> DeleteRangeAsync(IEnumerable<TEntity> entities)
    {
        var baseDomains = entities.ToList();
        _dbSet.UpdateRange(baseDomains);
        return await Task.FromResult(baseDomains.ToList());
    }
}
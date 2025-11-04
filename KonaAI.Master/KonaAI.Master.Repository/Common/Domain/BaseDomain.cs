namespace KonaAI.Master.Repository.Common.Domain;

/// <summary>
/// Represents the base domain entity with common audit and identity properties.
/// </summary>
public class BaseDomain
{
    /// <summary>
    /// Gets or sets the unique numeric identifier for the entity.
    /// </summary>
    public virtual long Id { get; set; }

    /// <summary>
    /// Gets or sets the unique GUID identifier for the entity.
    /// Initialized to a new GUID by default.
    /// </summary>
    public virtual Guid RowId { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Gets or sets the identifier of the user who created the entity.
    /// </summary>
    public virtual long CreatedById { get; set; } = 0;

    /// <summary>
    /// Gets or sets the identifier of the user who created the entity.
    /// </summary>
    public virtual string CreatedBy { get; set; } = null!;

    /// <summary>
    /// Gets or sets the UTC date and time when the entity was created.
    /// Initialized to the current UTC time by default.
    /// </summary>
    public virtual DateTime CreatedOn { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the identifier of the user who last modified the entity.
    /// </summary>
    public virtual long ModifiedById { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who last modified the entity.
    /// </summary>
    public virtual string ModifiedBy { get; set; } = null!;

    /// <summary>
    /// Gets or sets the UTC date and time when the entity was last modified.
    /// Initialized to the current UTC time by default.
    /// </summary>
    public virtual DateTime ModifiedOn { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets a value indicating whether the entity is active.
    /// Initialized to <c>true</c> by default.
    /// </summary>
    public virtual bool IsActive { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the entity is marked as deleted.
    /// Initialized to <c>false</c> by default.
    /// </summary>
    public virtual bool IsDeleted { get; set; } = false;
}
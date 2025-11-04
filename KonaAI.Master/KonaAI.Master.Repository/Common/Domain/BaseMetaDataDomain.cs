namespace KonaAI.Master.Repository.Common.Domain;

/// <summary>
/// Represents a base domain entity with metadata properties such as name, description, and order.
/// Inherits common audit and identity properties from <see cref="BaseDomain"/>.
/// </summary>
public class BaseMetaDataDomain : BaseDomain
{
    /// <summary>
    /// Gets or sets the name of the entity.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Gets or sets the description of the entity.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this entity is the default.
    /// </summary>
    public bool IsDefault { get; set; } = true;

    /// <summary>
    /// Gets or sets the order or sequence value for the entity.
    /// </summary>
    public int OrderBy { get; set; }
}
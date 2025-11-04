namespace KonaAI.Master.Repository.Common.Domain;

/// <summary>
/// Represents a base domain entity with metadata properties and an associated client identifier.
/// Inherits metadata properties such as name, description, and order from <see cref="BaseMetaDataDomain"/>,
/// as well as common audit and identity properties from <see cref="BaseDomain"/>.
/// </summary>
public class BaseClientMetaDataDomain : BaseClientDomain
{
    /// <summary>
    /// Gets or sets the name of the entity.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the description of the entity.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the order or sequence value for the entity.
    /// </summary>
    public int OrderBy { get; set; }
}
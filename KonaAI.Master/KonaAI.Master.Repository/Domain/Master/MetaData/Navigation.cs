using KonaAI.Master.Repository.Common.Domain;

namespace KonaAI.Master.Repository.Domain.Master.MetaData;

/// <summary>
/// Represents a navigation menu item within the application.
/// Inherits common metadata properties (Name, Description, OrderBy, audit fields) from <see cref="BaseMetaDataDomain"/>.
/// </summary>
/// <remarks>
/// Supports hierarchical parent-child relationships through <see cref="ParentId"/>.
/// Additional relationships or menu associations can be configured via EF Core navigation properties.
/// </remarks>
public class Navigation : BaseMetaDataDomain
{
    /// <summary>
    /// Gets or sets the identifier of the parent navigation item, if any.
    /// </summary>
    public long? ParentId { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this navigation item is a top-level menu item.
    /// </summary>
    public bool IsTopMenu { get; set; }
}
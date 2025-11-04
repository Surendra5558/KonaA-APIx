using KonaAI.Master.Repository.Common.Domain;

namespace KonaAI.Master.Repository.Domain.Master.MetaData;

/// <summary>
/// Represents the association between a module and a source type.
/// Inherits common metadata properties (Name, Description, OrderBy, audit fields) from <see cref="BaseMetaDataDomain"/>.
/// </summary>
/// <remarks>
/// This entity enables linking modules to specific source types for feature or functional classification.
/// Additional relationships can be configured via EF Core navigation properties.
/// </remarks>
public class ModuleSourceType : BaseMetaDataDomain
{
    /// <summary>
    /// Gets or sets the identifier of the associated module.
    /// </summary>
    public long ModuleTypeId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the associated source type.
    /// </summary>
    public long SourcTypeId { get; set; }
}
using KonaAI.Master.Repository.Common.Domain;

namespace KonaAI.Master.Repository.Domain.Master.MetaData;

/// <summary>
/// Represents a refined classification under a broader module type (e.g., a specific feature set or functional subset).
/// Inherits common metadata properties (Name, Description, OrderBy, audit fields) from <see cref="BaseMetaDataDomain"/>.
/// </summary>
/// <remarks>
/// Additional relationships (e.g., to ModuleType) can be added later via navigation properties and configured in the EF Core configuration layer.
/// </remarks>
public class SourceType : BaseMetaDataDomain
{
}
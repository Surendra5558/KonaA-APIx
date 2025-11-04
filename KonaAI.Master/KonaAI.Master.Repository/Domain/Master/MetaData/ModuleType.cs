using KonaAI.Master.Repository.Common.Domain;

namespace KonaAI.Master.Repository.Domain.Master.MetaData;

/// <summary>
/// Represents a top-level functional module classification (e.g., Billing, Reporting, Security).
/// Inherits common metadata properties (Name, Description, OrderBy, audit fields) from <see cref="BaseMetaDataDomain"/>.
/// </summary>
/// <remarks>
/// Related sub-categories (e.g., specific feature groups) can be modeled via a <c>ModuleSubType</c> entity
/// with a foreign key to this type and configured in the EF Core configuration layer.
/// </remarks>
public class ModuleType : BaseMetaDataDomain
{
}
using KonaAI.Master.Repository.Common.Domain;

namespace KonaAI.Master.Repository.Domain.Master.UserMetaData;

/// <summary>
/// Represents a specific audit responsibility or role within the system.
/// Inherits common metadata properties (Name, Description, OrderBy, audit fields) from <see cref="BaseMetaDataDomain"/>.
/// </summary>
/// <remarks>
/// Can be extended in the future with relationships to users, departments, or modules
/// via EF Core navigation properties.
/// </remarks>
public class ProjectAuditResponsibility : BaseMetaDataDomain
{
}
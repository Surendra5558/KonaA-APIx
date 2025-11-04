using KonaAI.Master.Repository.Common.Domain;

namespace KonaAI.Master.Repository.Domain.Master.UserMetaData;

/// <summary>
/// Represents a risk area within the organization or system.
/// Inherits common metadata properties (Name, Description, OrderBy, audit fields) from <see cref="BaseMetaDataDomain"/>.
/// </summary>
/// <remarks>
/// Can be extended with relationships to modules, departments, or processes
/// via EF Core navigation properties.
/// </remarks>
public class ProjectRiskArea : BaseMetaDataDomain
{
}
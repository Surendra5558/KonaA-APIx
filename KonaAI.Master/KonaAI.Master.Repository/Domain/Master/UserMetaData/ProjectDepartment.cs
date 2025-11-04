using KonaAI.Master.Repository.Common.Domain;

namespace KonaAI.Master.Repository.Domain.Master.UserMetaData;

/// <summary>
/// Represents a business department within the organization.
/// Inherits common metadata properties (Name, Description, OrderBy, audit fields) from <see cref="BaseMetaDataDomain"/>.
/// </summary>
/// <remarks>
/// Can be extended with relationships to employees, projects, or other organizational structures
/// via EF Core navigation properties.
/// </remarks>
public class ProjectDepartment : BaseMetaDataDomain
{
}
using KonaAI.Master.Repository.Common.Domain;

namespace KonaAI.Master.Repository.Domain.Master.UserMetaData;

/// <summary>
/// Represents a business unit within the organization.
/// Inherits common metadata properties (Name, Description, OrderBy, audit fields) from <see cref="BaseMetaDataDomain"/>.
/// </summary>
/// <remarks>
/// Can be extended with relationships to departments, projects, or employees
/// via EF Core navigation properties.
/// </remarks>
public class ProjectUnit : BaseMetaDataDomain
{
}
using KonaAI.Master.Repository.Common.Domain;

namespace KonaAI.Master.Repository.Domain.Master.MetaData;

/// <summary>
/// Represents a system action (e.g., Create, Read, Update, Delete) within the application.
/// Inherits common metadata properties (Name, Description, OrderBy, audit fields) from <see cref="BaseMetaDataDomain"/>.
/// </summary>
/// <remarks>
/// This entity can be extended in the future to include relationships with roles or modules
/// for access control purposes.
/// </remarks>
public class UserAction : BaseMetaDataDomain
{
}
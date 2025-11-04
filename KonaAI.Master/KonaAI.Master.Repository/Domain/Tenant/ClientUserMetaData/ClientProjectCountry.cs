using KonaAI.Master.Repository.Common.Domain;

namespace KonaAI.Master.Repository.Domain.Tenant.ClientUserMetaData;

/// <summary>
/// Represents a project-scoped, client-specific country metadata entity.
/// </summary>
/// <remarks>
/// Inherits common client metadata fields such as <see cref="BaseClientMetaDataDomain.Name"/>,
/// <see cref="BaseClientMetaDataDomain.Description"/>, and <see cref="BaseClientMetaDataDomain.OrderBy"/>,
/// as well as the client association <see cref="BaseClientDomain.ClientId"/>.
/// </remarks>
/// <seealso cref="BaseClientMetaDataDomain"/>
/// <seealso cref="BaseClientDomain"/>
public class ClientProjectCountry : BaseClientMetaDataDomain
{
    /// <summary>
    /// Gets or sets the client project identifier.
    /// </summary>
    /// <value>
    /// The client project identifier.
    /// </value>
    public Guid ClientProjectId { get; set; }

    /// <summary>
    /// Gets or sets the country identifier.
    /// </summary>
    /// <value>
    /// The country identifier.
    /// </value>
    public long CountryId { get; set; }
}
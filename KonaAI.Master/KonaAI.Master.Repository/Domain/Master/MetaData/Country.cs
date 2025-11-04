using KonaAI.Master.Repository.Common.Domain;

namespace KonaAI.Master.Repository.Domain.Master.MetaData;

/// <summary>
/// Represents a country metadata entity.
/// </summary>
/// <remarks>
/// Inherits common metadata such as <see cref="BaseMetaDataDomain.Name"/>,
/// <see cref="BaseMetaDataDomain.Description"/>, and audit/order fields from
/// <see cref="BaseMetaDataDomain"/>.
/// </remarks>
/// <seealso cref="BaseMetaDataDomain"/>
public class Country : BaseMetaDataDomain
{
    /// <summary>
    /// Gets or sets the country code.
    /// </summary>
    /// <remarks>
    /// Typically an ISO 3166-1 code (e.g., "US", "GB", "IND"), but may be project-specific.
    /// May be <see langword="null"/> when unknown or not applicable.
    /// </remarks>
    public string? CountryCode { get; set; }
}
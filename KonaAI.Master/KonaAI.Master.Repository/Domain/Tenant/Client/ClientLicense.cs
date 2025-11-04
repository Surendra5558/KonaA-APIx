using KonaAI.Master.Repository.Common.Domain;

namespace KonaAI.Master.Repository.Domain.Tenant.Client;

/// <summary>
/// Represents licensing information tied to a client, product, or module.
/// Inherits common metadata properties (Name, Description, OrderBy, audit fields)
/// from <see cref="BaseMetaDataDomain"/>.
/// </summary>
/// <remarks>
/// Can be extended with additional properties such as license key,
/// expiration date, allowed users, or usage limits.
/// Relationships (e.g., to Client, Product, or ModuleType) can be added later
/// via navigation properties and configured in the EF Core configuration layer.
/// </remarks>
public class ClientLicense : BaseClientDomain
{
    /// <summary>
    /// License name
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// License Description
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// License StartDate
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// License EndDate
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// License key Encrypted
    /// </summary>
    public string LicenseKey { get; set; } = null!;

    /// <summary>
    /// Encrypted Key
    /// </summary>
    public string PrivateKey { get; set; } = null!;
}
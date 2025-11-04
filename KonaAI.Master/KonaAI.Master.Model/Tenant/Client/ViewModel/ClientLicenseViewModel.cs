using KonaAI.Master.Model.Common;

namespace KonaAI.Master.Model.Tenant.Client.ViewModel;

/// <summary>
/// Represents a client license info view model with metadata and audit information.
/// Inherits from <see cref="BaseAuditViewModel"/> for identity and audit tracking.
/// </summary>
public class ClientLicenseViewModel : BaseAuditViewModel
{
    /// <summary>
    /// Gets or sets the license name.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Gets or sets the license description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the start date of the license.
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Gets or sets the end date of the license.
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// Gets or sets the encrypted license key.
    /// </summary>
    public string LicenseKey { get; set; } = null!;
}
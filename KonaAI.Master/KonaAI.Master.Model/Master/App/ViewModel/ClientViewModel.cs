using KonaAI.Master.Model.Common;

namespace KonaAI.Master.Model.Master.App.ViewModel;

/// <summary>
/// Represents a client with identity, audit, and profile information for use in the master data context.
/// Implements <see cref="BaseAuditViewModel"/> for identity properties and <see cref="BaseAuditViewModel"/> for audit tracking.
/// </summary>
public class ClientViewModel : BaseAuditViewModel
{
    /// <summary>
    /// Gets or sets the unique name of the client.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Gets or sets the display name of the client.
    /// </summary>
    public string DisplayName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the code used to identify the client.
    /// </summary>
    public string ClientCode { get; set; } = null!;

    /// <summary>
    /// Gets or sets the logo image for the client as a byte array.
    /// </summary>
    public byte[]? Logo { get; set; }
}
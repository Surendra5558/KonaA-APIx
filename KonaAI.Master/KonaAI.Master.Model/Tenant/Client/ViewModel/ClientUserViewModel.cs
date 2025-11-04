using KonaAI.Master.Model.Common;

namespace KonaAI.Master.Model.Tenant.Client.ViewModel;

/// <summary>
/// Represents a user view model with metadata and audit information.
/// Implements <see cref="BaseAuditViewModel"/> for identity properties and <see cref="BaseAuditViewModel"/> for audit tracking.
/// </summary>
public class ClientUserViewModel : BaseAuditViewModel
{
    /// <summary>
    /// Gets or sets the username of the user.
    /// </summary>
    public string UserName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the email of the user.
    /// </summary>
    public string Email { get; set; } = null!;

    /// <summary>
    /// Gets or sets the first name of the user.
    /// </summary>
    public string FirstName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the last name of the user.
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    /// Gets or sets the phone number of the user.
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Gets or sets the phone number country code.
    /// </summary>
    public string? PhoneNumberCountryCode { get; set; }

    /// <summary>
    /// Gets or sets the login type identifier.
    /// </summary>
    public long LogOnTypeId { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the user has requested a reset.
    /// </summary>
    public bool IsResetRequested { get; set; }

    /// <summary>
    /// Gets or sets the Role type identifier.
    /// </summary>
    public long RoleTypeId { get; set; }
}
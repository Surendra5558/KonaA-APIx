using KonaAI.Master.Repository.Common.Domain;
using KonaAI.Master.Repository.Domain.Master.MetaData;

namespace KonaAI.Master.Repository.Domain.Master.App;

/// <summary>
/// Represents a user entity with authentication, contact, and profile information.
/// Inherits common audit and identity properties from <see cref="BaseDomain"/>.
/// </summary>
public class User : BaseDomain
{
    /// <summary>
    /// Gets or sets the username used for authentication.
    /// </summary>
    public string UserName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the primary email address of the user.
    /// </summary>
    public string Email { get; set; } = null!;

    /// <summary>
    /// Gets or sets the password for the user account.
    /// </summary>
    public string? Password { get; set; }

    /// <summary>
    /// Gets or sets the first name of the user.
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// Gets or sets the last name of the user.
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    /// Gets or sets the phone number of the user.
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Gets or sets the country code associated with the user's phone number.
    /// </summary>
    public string? PhoneNumberCountryCode { get; set; }

    /// <summary>
    /// Gets or sets the identifier for the single sign-on type used by the user.
    /// </summary>
    public long LogOnTypeId { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="LogOnType"/> navigation property representing the single sign-on type.
    /// </summary>
    public LogOnType LogOnType { get; set; } = null!;

    /// <summary>
    /// Gets or sets a value indicating whether a password reset has been requested for the user.
    /// </summary>
    public bool IsResetRequested { get; set; }

    /// <summary>
    /// Gets or sets unique Identifier for the RoleTypeId
    /// </summary>
    public long RoleTypeId { get; set; }

    /// <summary>
    /// Navigation property to the RoleType entity.
    /// </summary>
    public RoleType RoleType { get; set; } = null!;
}
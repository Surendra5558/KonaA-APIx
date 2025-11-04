using KonaAI.Master.Repository.Common.Domain;

namespace KonaAI.Master.Repository.Domain.Master.App;

/// <summary>
/// Represents a client entity with identification, contact, and configuration details.
/// Inherits common audit and identity properties from <see cref="BaseDomain"/>.
/// </summary>
public class Client : BaseDomain
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
    /// Gets or sets the description of the client.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the name of the contact person for the client.
    /// </summary>
    public string? ContactName { get; set; }

    /// <summary>
    /// Gets or sets the address of the client.
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// Gets or sets the email address of the client.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Gets or sets the phone number of the client.
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// Gets or sets the country code associated with the client.
    /// </summary>
    public string? CountryCode { get; set; }

    /// <summary>
    /// Gets or sets the logo image for the client as a byte array.
    /// </summary>
    public byte[]? Logo { get; set; }
}
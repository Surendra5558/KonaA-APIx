namespace KonaAI.Master.Model.Authentication;

/// <summary>
/// Represents the response returned after a successful authentication token request.
/// </summary>
public class TokenResponse
{
    /// <summary>
    /// Gets or sets the name of the authenticated user.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Gets or sets the authentication token issued to the user.
    /// </summary>
    public string Token { get; set; } = null!;

    /// <summary>
    /// Gets or sets the refresh token used to obtain a new authentication token when the current one expires.
    /// </summary>
    public string RefreshToken { get; set; } = null!;

    /// <summary>
    /// Gets or sets the role identifier of the authenticated user.
    /// </summary>
    public long RoleId { get; set; }

    /// <summary>
    /// Gets or sets the name of the role assigned to the authenticated user.
    /// </summary>
    public string RoleName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the client identifier.
    /// </summary>
    public long ClientId { get; set; }

    /// <summary>
    /// Gets or sets the name of the client.
    /// </summary>
    public string ClientName { get; set; } = null!;
}
namespace KonaAI.Master.Repository.Common.Model;

/// <summary>
/// Represents the contextual information for the authenticated user and tenant
/// used across the application/request pipeline.
/// </summary>
public class UserContext
{
    /// <summary>
    /// Gets or sets the unique session identifier (GUID) for this user context.
    /// </summary>
    public Guid SessionRowId { get; set; }

    /// <summary>
    /// Gets or sets the unique user identifier (GUID).
    /// </summary>
    public Guid UserRowId { get; set; }

    /// <summary>
    /// Gets or sets the user login ID.
    /// </summary>
    public long UserLoginId { get; set; }

    /// <summary>
    /// Gets or sets the user login name.
    /// </summary>
    public string UserLoginName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the user login email.
    /// </summary>
    public string UserLoginEmail { get; set; } = null!;

    /// <summary>
    /// Gets or sets the client identifier.
    /// </summary>
    public long ClientId { get; set; }

    /// <summary>
    /// Gets or sets the name of the client.
    /// </summary>
    public string ClientName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the unique role identifier (GUID).
    /// </summary>
    public Guid RoleRowId { get; set; }

    /// <summary>
    /// Gets or sets the role ID.
    /// </summary>
    public long RoleId { get; set; }

    /// <summary>
    /// Gets or sets the role name.
    /// </summary>
    public string RoleName { get; set; } = null!;
}
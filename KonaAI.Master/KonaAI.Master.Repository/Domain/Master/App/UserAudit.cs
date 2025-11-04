using KonaAI.Master.Repository.Common.Domain;
using KonaAI.Master.Repository.Common.Model;

namespace KonaAI.Master.Repository.Domain.Master.App;

/// <summary>
/// Represents an audit snapshot of a user's authenticated session including
/// identity, role hierarchy, project scope and refresh token lifecycle.
/// </summary>
public class UserAudit : BaseDomain
{
    /// <summary>
    /// Gets or sets the GUID (immutable) identifier of the user entity at the time of the audit.
    /// </summary>
    public Guid UserRowId { get; set; }

    /// <summary>
    /// Gets or sets the numeric identifier of the user at the time of the audit.
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// Gets or sets the user's given (first) name at the time of the audit.
    /// </summary>
    public string FirstName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the user's family (last) name at the time of the audit.
    /// </summary>
    public string LastName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the user's primary email address (used as a unique credential).
    /// </summary>
    public string Email { get; set; } = null!;

    /// <summary>
    /// Gets or sets the numeric (legacy / relational) role identifier.
    /// </summary>
    public long RoleId { get; set; }

    /// <summary>
    /// Gets or sets the GUID (immutable) identifier of the role entity.
    /// </summary>
    public Guid RoleRowId { get; set; } = Guid.Empty;

    /// <summary>
    /// Gets or sets the display name of the role assigned to the user.
    /// </summary>
    public string RoleName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the hierarchical navigation path for the role (e.g. for breadcrumb or authorization resolution).
    /// Ordered from root to the most specific role.
    /// </summary>
    public List<UserPermission> RoleNavigation { get; set; } = [];

    /// <summary>
    /// Gets or sets the list of project (tenant / domain) identifiers the user can access within this session context.
    /// </summary>
    public List<UserProject> ProjectAccess { get; set; } = [];

    /// <summary>
    /// Gets or sets the refresh token issued for session renewal.
    /// Persist securely and never expose in logs.
    /// </summary>
    public string RefreshToken { get; set; } = null!;

    /// <summary>
    /// Gets or sets the UTC timestamp when the refresh token was created.
    /// </summary>
    public DateTime TokenCreatedDate { get; set; }

    /// <summary>
    /// Gets or sets the UTC timestamp when the refresh token will expire (become invalid).
    /// </summary>
    public DateTime TokenExpiredDate { get; set; }
}
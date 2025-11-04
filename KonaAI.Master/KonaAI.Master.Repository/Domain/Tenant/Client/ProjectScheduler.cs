using KonaAI.Master.Repository.Common.Domain;

namespace KonaAI.Master.Repository.Domain.Tenant.Client;

/// <summary>
/// Represents a project scheduler entity with properties for scheduling and database connection details.
/// Inherits common audit and identity properties from <see cref="BaseClientDomain"/>.
/// </summary>
public class ProjectScheduler : BaseClientDomain
{
    /// <summary>
    /// Get or Set the ProjectSchedulerID
    /// </summary>
    public long ProjectSchedulerId { get; set; }

    /// <summary>
    /// Get or Set the Project Name
    /// </summary>
    public string ProjectName { get; set; } = null!;

    /// <summary>
    /// Get or Set the ProjectId
    /// </summary>
    public long ProjectId { get; set; }

    /// <summary>
    /// Get or Set the StatusId of Project
    /// </summary>
    public long ProjectStatusId { get; set; }

    /// <summary>
    /// Get or Set the Status of the Project
    /// </summary>
    public override bool IsActive { get; set; } = true;

    /// <summary>
    /// Get or Set the UserName
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// Get or Set the Password
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Get or Set the ConnectionString
    /// </summary>
    public string ConnectionString { get; set; } = string.Empty;

    /// <summary>
    /// Get or Set the Database of the Project
    /// </summary>
    public string DatabaseName { get; set; } = string.Empty;

    /// <summary>
    /// Get or Set the ErrorMessage
    /// </summary>
    public string ErrorMessage { get; set; } = string.Empty;

    /// <summary>
    /// Get or Set the EncryptedLicenseKey
    /// </summary>
    public string EncryptedLicenseKey { get; set; } = string.Empty;
}

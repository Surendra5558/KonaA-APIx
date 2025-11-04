namespace KonaAI.Master.Model.Tenant.Client.SaveModel;

/// <summary>
/// Represents a model for scheduling and managing project-related data, including project details, status, database
/// connection information, and user credentials.
/// </summary>
/// <remarks>This class is designed to encapsulate the necessary information for managing a project's scheduling
/// and status, as well as associated database and user authentication details. It includes properties for identifying
/// the project, tracking its status, and managing its active state.</remarks>
public class ProjectSchedulerModel
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
    public bool IsActive { get; set; } = true;

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
}


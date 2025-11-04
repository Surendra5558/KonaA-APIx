namespace KonaAI.Master.Repository.Common.Model;

/// <summary>
/// Represents a project that a user can access or is associated with.
/// </summary>
/// <remarks>
/// Instances are serialized as JSON within <c>UserAudit.ProjectAccess</c>.
/// </remarks>
public class UserProject
{
    /// <summary>
    /// Gets or sets the GUID row identifier of the project.
    /// </summary>
    public Guid RowId { get; set; }

    /// <summary>
    /// Gets or sets the numeric identifier of the project.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Gets or sets the display name of the project.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Gets or sets the connection string for the project's database.
    /// </summary>
    public string ConnectionString { get; set; } = null!;
}
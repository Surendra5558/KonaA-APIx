using KonaAI.Master.Repository.Common.Domain;
namespace KonaAI.Master.Repository.Domain.Tenant.Client;
/// <summary>
/// Represents the relationship between a client user and a project, including their assigned role.
/// </summary>
/// <remarks>
/// This domain model extends <see cref="BaseClientDomain"/> and is used to store and manage 
/// the assignment of users to projects along with their associated roles within those projects.
/// </remarks>
public class ClientProjectUser : BaseClientDomain
{
    /// <summary>
    /// Gets or sets the unique identifier of the project to which the user is assigned.
    /// </summary>
    /// <value>
    /// A <see cref="long"/> representing the project ID.
    /// </value>
    public long ProjectId { get; set; }
    /// <summary>
    /// Gets or sets the unique identifier of the user who is associated with the project.
    /// </summary>
    /// <value>
    /// A <see cref="long"/> representing the user ID.
    /// </value>
    public long UserId { get; set; }
    /// <summary>
    /// Gets or sets the unique identifier of the role assigned to the user within the project.
    /// </summary>
    /// <value>
    /// A <see cref="long"/> representing the user role ID.
    /// </value>
    public long RoleId { get; set; }
}
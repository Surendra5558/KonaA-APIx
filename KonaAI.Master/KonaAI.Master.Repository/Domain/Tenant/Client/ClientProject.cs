using KonaAI.Master.Repository.Common.Domain;

namespace KonaAI.Master.Repository.Domain.Tenant.Client;

/// <summary>
/// Represents a client project entity with references to metadata tables.
/// Inherits common audit and identity properties from <see cref="BaseDomain"/>.
/// </summary>
/// <remarks>
/// Includes foreign key references and navigation properties for audit responsibility, risk area, country,
/// business unit, and business department. Start and end dates define the project timeline.
/// </remarks>
public class ClientProject : BaseClientDomain
{
    /// <summary>
    /// Gets or sets the name of the project.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Gets or sets the description of the project.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the associated audit responsibility.
    /// </summary>
    public long ProjectAuditResponsibilityId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the associated risk area.
    /// </summary>
    public long ProjectRiskAreaId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the associated country.
    /// </summary>
    public long? ProjectCountryId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the associated business unit.
    /// </summary>
    public long? ProjectUnitId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the associated business department.
    /// </summary>
    public long? ProjectDepartmentId { get; set; }

    /// <summary>
    /// Gets or sets the project start date.
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Gets or sets the project end date.
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// Gets or sets the client project module source types.
    /// </summary>
    public ICollection<ClientProjectModuleSourceType> ClientProjectModuleSourceTypes { get; set; } = null!;

    /// <summary>
    /// Gets or sets the project status identifier.
    /// </summary>
    public long ProjectStatusId { get; set; }


    /// <summary>
    /// Gets or sets the comma-separated list of modules for this project.
    /// </summary>
    public string Modules { get; set; } = string.Empty;

    // Ensure ClientId remains Int64 (inherited from BaseClientDomain) to match DB bigint;
    // do NOT shadow with an int property, which causes Int64→Int32 cast exceptions.

}

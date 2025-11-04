using KonaAI.Master.Model.Common;

namespace KonaAI.Master.Model.Tenant.Client.ViewModel;

/// <summary>
/// Represents a client project with related metadata.
/// Inherits from <see cref="BaseAuditViewModel"/>, which provides audit metadata and the base <see cref="BaseViewModel.RowId"/> identity.
/// </summary>
public class ClientProjectViewModel : BaseAuditViewModel
{
    /// <summary>
    /// Gets or sets the name of the client project.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Gets or sets the description of the client project.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the formatted start date string (for display or input binding).
    /// </summary>
    public string StartDateString { get; set; } = null!;

    /// <summary>
    /// Gets or sets the start date and time of the client project.
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Gets or sets the formatted end date string (for display or input binding).
    /// </summary>
    public string EndDateString { get; set; } = null!;

    /// <summary>
    /// Gets or sets the end date and time of the client project.
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// Gets or sets the audit responsibility identifier for the project.
    /// </summary>
    public Guid AuditResponsibilityRowId { get; set; }

    /// <summary>
    /// Gets or sets the name of the audit responsibility.
    /// </summary>
    public string AuditResponsibilityName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the risk area identifier for the project.
    /// </summary>
    public Guid RiskAreaRowId { get; set; }

    /// <summary>
    /// Gets or sets the name of the risk area.
    /// </summary>
    public string RiskAreaName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the country identifier for the project.
    /// </summary>
    public Guid? CountryRowId { get; set; }

    /// <summary>
    /// Gets or sets the name of the country.
    /// </summary>
    public string? CountryName { get; set; }

    /// <summary>
    /// Gets or sets the business unit identifier for the project.
    /// </summary>
    public Guid? BusinessUnitRowId { get; set; }

    /// <summary>
    /// Gets or sets the name of the business unit.
    /// </summary>
    public string? BusinessUnitName { get; set; }

    /// <summary>
    /// Gets or sets the business department identifier for the project.
    /// </summary>
    public Guid? BusinessDepartmentId { get; set; }

    /// <summary>
    /// Gets or sets the name of the business department.
    /// </summary>
    public string? BusinessDepartmentName { get; set; }

    /// <summary>
    /// Gets or sets the project status identifier.
    /// </summary>
    public Guid ProjectStatusRowId { get; set; }

    /// <summary>
    /// Gets or sets the project status name.
    /// </summary>
    public string ProjectStatusName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the comma-separated list of modules associated with this project.
    /// </summary>
    public string Modules { get; set; } = string.Empty;
}

/// <summary>
/// Represents a project module and its source type within a client project.
/// </summary>
public class ProjectModuleSourceTypeViewModel
{
    /// <summary>
    /// Gets or sets the unique row identifier for the project module mapping.
    /// </summary>
    public Guid RowId { get; set; }

    /// <summary>
    /// Gets or sets the module identifier.
    /// </summary>
    public Guid ModuleRowId { get; set; }

    /// <summary>
    /// Gets or sets the name of the module.
    /// </summary>
    public string ModuleName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the source type identifier.
    /// </summary>
    public Guid SourceTypeRowId { get; set; }

    /// <summary>
    /// Gets or sets the name of the source type.
    /// </summary>
    public string SourceTypeName { get; set; } = null!;
}
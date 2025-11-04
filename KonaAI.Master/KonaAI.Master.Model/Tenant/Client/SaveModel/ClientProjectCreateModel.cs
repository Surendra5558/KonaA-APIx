using FluentValidation;
using KonaAI.Master.Model.Common.Constants;

namespace KonaAI.Master.Model.Tenant.Client.SaveModel;

/// <summary>
/// Represents the payload to create a new client project.
/// </summary>
public class ClientProjectCreateModel
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
    /// Gets or sets the audit responsibility identifier for the project.
    /// </summary>
    public Guid AuditResponsibilityRowId { get; set; }

    /// <summary>
    /// Gets or sets the risk area identifier for the project.
    /// </summary>
    public Guid RiskAreaRowId { get; set; }

    /// <summary>
    /// Gets or sets the country identifier for the project.
    /// </summary>
    public Guid? CountryRowId { get; set; }

    /// <summary>
    /// Gets or sets the business unit identifier for the project.
    /// </summary>
    public Guid? BusinessUnitRowId { get; set; }

    /// <summary>
    /// Gets or sets the business department identifier for the project.
    /// </summary>
    public Guid? BusinessDepartmentRowId { get; set; }

    /// <summary>
    /// Gets or sets the start date of the project.
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Gets or sets the end date of the project.
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// Gets or sets the comma-separated list of project modules associated with this project.
    /// </summary>
    public string Modules { get; set; } = null!;

}

/// <summary>
/// Provides validation rules for the <see cref="ClientProjectCreateModel"/> to ensure data integrity and business requirements.
/// </summary>
public class ClientProjectValidator : AbstractValidator<ClientProjectCreateModel>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ClientProjectValidator"/> class and defines validation rules for <see cref="ClientProjectCreateModel"/> properties.
    /// </summary>
    public ClientProjectValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Project Name is required")
            .MaximumLength(DbColumnLength.NameEmail)
            .WithMessage($"Project Name cannot exceed {DbColumnLength.NameEmail}");

        RuleFor(x => x.Description)
            .MaximumLength(DbColumnLength.Description)
            .WithMessage($"Description cannot exceed {DbColumnLength.Description}");

        // Required master references
        RuleFor(x => x.AuditResponsibilityRowId)
            .NotEqual(Guid.Empty)
            .WithMessage("Audit Responsibility is required");

        RuleFor(x => x.RiskAreaRowId)
            .NotEqual(Guid.Empty)
            .WithMessage("Risk Area is required");

        RuleFor(x => x.StartDate)
            .NotEmpty()
            .WithMessage("Start Date is required")
            .Must(date => date.Date >= DateTime.UtcNow.Date)
            .WithMessage("Start Date cannot be in the past");

        When(x => x.EndDate.HasValue, () =>
        {
            RuleFor(x => x.EndDate)
                .GreaterThan(x => x.StartDate)
                .WithMessage("End Date must be after Start Date");
        });

        // Modules are mandatory and must be valid
        RuleFor(x => x.Modules)
            .NotEmpty()
            .WithMessage("At least one Module must be selected")
            .Must(modules => !string.IsNullOrWhiteSpace(modules))
            .WithMessage("Modules cannot be empty")
            .Must(modules => ValidateModuleNames(modules))
            .WithMessage("Modules must be one of: P2P, O2C, T&E");
    }

    /// <summary>
    /// Validates that the provided modules string contains only valid module names.
    /// Checks for valid modules, no duplicates, and proper formatting.
    /// </summary>
    /// <param name="modules">Comma-separated string of module names to validate.</param>
    /// <returns>True if all modules are valid and unique, false otherwise.</returns>
    private static bool ValidateModuleNames(string? modules)
    {
        if (string.IsNullOrWhiteSpace(modules))
            return false;

        var validModules = new[] { "P2P", "O2C", "T&E" };
        var moduleList = modules.Split(',', StringSplitOptions.RemoveEmptyEntries)
                               .Select(m => m.Trim())
                               .ToList();

        return moduleList.All(module => validModules.Contains(module)) &&
               moduleList.Count == moduleList.Distinct().Count();

    }
}
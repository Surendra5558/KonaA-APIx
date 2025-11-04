using FluentValidation;
using KonaAI.Master.Model.Common.Constants;

namespace KonaAI.Master.Model.Tenant.Client.BaseModel;

/// <summary>
/// Common base for client license info create/update models.
/// </summary>
public abstract class ClientLicenseBaseModel
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}

/// <summary>
/// Base validator for client license info models (create/update).
/// </summary>
public class ClientLicenseBaseValidator<T> : AbstractValidator<T>
    where T : ClientLicenseBaseModel
{
    public ClientLicenseBaseValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("License Name is required")
            .MaximumLength(DbColumnLength.NameEmail).WithMessage($"License Name cannot exceed {DbColumnLength.NameEmail}");

        RuleFor(x => x.Description)
            .MaximumLength(DbColumnLength.Description).When(x => !string.IsNullOrEmpty(x.Description))
            .WithMessage($"Description cannot exceed {DbColumnLength.Description}");

        RuleFor(x => x.StartDate)
            .NotNull().WithMessage("Start Date is required");

        RuleFor(x => x.EndDate)
            .NotNull().WithMessage("End Date is required")
            .GreaterThan(x => x.StartDate).When(x => x.StartDate.HasValue && x.EndDate.HasValue)
            .WithMessage("End Date must be later than Start Date");
    }
}
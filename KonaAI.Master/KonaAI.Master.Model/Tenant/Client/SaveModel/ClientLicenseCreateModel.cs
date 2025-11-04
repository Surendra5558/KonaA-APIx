using FluentValidation;
using KonaAI.Master.Model.Tenant.Client.BaseModel;

namespace KonaAI.Master.Model.Tenant.Client.SaveModel;

/// <summary>
/// Represents the data model for creating a new client license info.
/// </summary>
public class ClientLicenseCreateModel : ClientLicenseBaseModel
{
}

/// <summary>
/// Provides validation rules for <see cref="ClientLicenseCreateModel"/>.
/// </summary>
public class ClientLicenseCreateValidator : ClientLicenseBaseValidator<ClientLicenseCreateModel>
{
    public ClientLicenseCreateValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("License Name is required");
        RuleFor(x => x.StartDate).NotNull().WithMessage("Start Date is required");
        RuleFor(x => x.EndDate).NotNull().WithMessage("End Date is required");
    }
}
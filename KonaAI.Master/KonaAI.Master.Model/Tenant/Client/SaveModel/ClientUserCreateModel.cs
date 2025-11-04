using FluentValidation;
using KonaAI.Master.Model.Tenant.Client.BaseModel;

namespace KonaAI.Master.Model.Tenant.Client.SaveModel;

/// <summary>
/// Represents the data model for creating a new user.
/// </summary>
public class ClientUserCreateModel : ClientUserBaseModel
{
}

/// <summary>
/// Provides validation rules for <see cref="ClientUserCreateModel"/>.
/// </summary>
public class ClientUserCreateValidator : ClientUserBaseValidator<ClientUserCreateModel>
{
    public ClientUserCreateValidator()
    {
        RuleFor(x => x.UserName).NotEmpty().WithMessage("User Name is required");
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required");
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("First Name is required");
        RuleFor(x => x.LogOnTypeId).NotNull().WithMessage("LogOnTypeId is required");
        RuleFor(x => x.RoleTypeId).NotNull().WithMessage("RoleTypeId is required");
    }
}
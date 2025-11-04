using FluentValidation;
using KonaAI.Master.Model.Common;
using KonaAI.Master.Model.Tenant.Client.BaseModel;

namespace KonaAI.Master.Model.Tenant.Client.SaveModel;

/// <summary>
/// Represents the data model for creating a new role type.
/// </summary>
public class ClientRoleTypeCreateModel : MetaDataViewModel
{
    //Inherits from ClientRoleTypeBaseModel
}

/// <summary>
/// Provides validation rules for <see cref="ClientRoleTypeCreateModel"/>.
/// </summary>
public class ClientRoleTypeCreateValidator : ClientRoleTypeBaseValidator<ClientRoleTypeCreateModel>
{
    public ClientRoleTypeCreateValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Role Name is required");
    }
}
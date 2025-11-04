using FluentValidation;
using KonaAI.Master.Model.Common;
using KonaAI.Master.Model.Common.Constants;

namespace KonaAI.Master.Model.Tenant.Client.BaseModel;

/// <summary>
/// Base validator for role type models.
/// </summary>
public class ClientRoleTypeBaseValidator<T> : AbstractValidator<T>
    where T : MetaDataViewModel
{
    public ClientRoleTypeBaseValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(DbColumnLength.NameEmail).When(x => !string.IsNullOrEmpty(x.Name))
            .WithMessage($"Role Name cannot exceed {DbColumnLength.NameEmail}");

        RuleFor(x => x.Description)
            .MaximumLength(DbColumnLength.Description).When(x => !string.IsNullOrEmpty(x.Description))
            .WithMessage($"Description cannot exceed {DbColumnLength.Description} characters");
    }
}
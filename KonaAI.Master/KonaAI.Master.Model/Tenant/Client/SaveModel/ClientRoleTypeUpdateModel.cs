using KonaAI.Master.Model.Common;
using KonaAI.Master.Model.Tenant.Client.BaseModel;

namespace KonaAI.Master.Model.Tenant.Client.SaveModel;

/// <summary>
/// Represents the data model for updating a role type.
/// </summary>
public class ClientRoleTypeUpdateModel : MetaDataViewModel
{
    // All fields optional for update
}

/// <summary>
/// Provides validation rules for <see cref="ClientRoleTypeCreateModel"/>.
/// </summary>
public class ClientRoleTypeUpdateValidator : ClientRoleTypeBaseValidator<ClientRoleTypeUpdateModel>
{
    public ClientRoleTypeUpdateValidator()
    {
        // inherits base rules
        // could add update-specific checks here
    }
}
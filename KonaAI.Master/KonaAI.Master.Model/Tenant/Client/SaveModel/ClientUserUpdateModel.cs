using KonaAI.Master.Model.Tenant.Client.BaseModel;

namespace KonaAI.Master.Model.Tenant.Client.SaveModel;

/// <summary>
/// Represents the data model for updating an existing user.
/// </summary>
public class ClientUserUpdateModel : ClientUserBaseModel
{
    // All properties are inherited from ClientUserBaseModel
}

public class ClientUserUpdateValidator : ClientUserBaseValidator<ClientUserUpdateModel>
{
    public ClientUserUpdateValidator()
    {
        // Additional rules specific to update can be added here if needed
    }
}
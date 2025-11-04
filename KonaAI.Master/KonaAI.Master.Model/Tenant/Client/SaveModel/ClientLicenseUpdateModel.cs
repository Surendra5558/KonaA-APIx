using KonaAI.Master.Model.Tenant.Client.BaseModel;

namespace KonaAI.Master.Model.Tenant.Client.SaveModel;

/// <summary>
/// Represents the data model for updating an existing client license info.
/// </summary>
public class ClientLicenseUpdateModel : ClientLicenseBaseModel
{
}

/// <summary>
/// Provides validation rules for <see cref="ClientLicenseUpdateModel"/>.
/// </summary>
public class ClientLicenseUpdateValidator : ClientLicenseBaseValidator<ClientLicenseUpdateModel>
{
    public ClientLicenseUpdateValidator()
    {
        // No extra rules beyond base — update allows partial modifications,
        // but you can enforce stricter rules here if needed.
    }
}
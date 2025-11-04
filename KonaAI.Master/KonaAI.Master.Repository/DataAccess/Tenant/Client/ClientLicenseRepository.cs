using KonaAI.Master.Repository.Common;
using KonaAI.Master.Repository.DataAccess.Tenant.Client.Interface;
using KonaAI.Master.Repository.Domain.Tenant.Client;

namespace KonaAI.Master.Repository.DataAccess.Tenant.Client;

/// <summary>
/// Repository for performing CRUD operations on <see cref="ClientLicense"/> entities within the <see cref="DefaultContext"/>.
/// Inherits generic data access methods from <see cref="GenericRepository{DefaultContext, LicenseInfo}"/>
/// and implements <see cref="IClientLicenseRepository"/>.
/// </summary>
public class ClientLicenseRepository(DefaultContext context)
    : GenericRepository<DefaultContext, ClientLicense>(context), IClientLicenseRepository
{
}
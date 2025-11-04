using AutoMapper;
using KonaAI.Master.Business.Tenant.UserMetaData.Logic.Interface;
using KonaAI.Master.Model.Common;
using KonaAI.Master.Repository.Common.Interface;
using Microsoft.Extensions.Logging;

namespace KonaAI.Master.Business.Tenant.UserMetaData.Logic;

/// <summary>
/// Provides business logic for retrieving Project Audit Responsibilities metadata.
/// Implements <see cref="IClientProjectAuditResponsibilityBusiness"/>.
/// </summary>
/// <param name="logger">The logger used for diagnostic and operational logging.</param>
/// <param name="mapper">The AutoMapper instance used to map repository entities to <see cref="MetaDataViewModel"/>.</param>
/// <param name="unitOfWork">The unit-of-work providing access to repositories.</param>
public class ClientProjectAuditResponsibilityBusiness(ILogger<ClientProjectAuditResponsibilityBusiness> logger,
    IMapper mapper,
    IUnitOfWork unitOfWork) : IClientProjectAuditResponsibilityBusiness
{
    private const string ClassName = nameof(ClientProjectAuditResponsibilityBusiness);

    /// <summary>
    /// Asynchronously retrieves all Project Audit Responsibilities as a queryable collection of <see cref="MetaDataViewModel"/>.
    /// </summary>
    /// <returns>
    /// A task that, when completed, provides an <see cref="IQueryable{T}"/> of <see cref="MetaDataViewModel"/> representing all Project Audit Responsibilities.
    /// </returns>
    /// <remarks>Logs start, completion, and any errors encountered.</remarks>
    /// <exception cref="Exception">Propagates any exception thrown by the underlying repository or mapper.</exception>
    public async Task<IQueryable<MetaDataViewModel>> GetAsync()
    {
        const string methodName = $"{ClassName}: {nameof(GetAsync)}";

        try
        {
            logger.LogInformation("{MethodName} - method execution started", methodName);

            var result = from pa in await unitOfWork.ProjectAuditResponsibilities.GetAsync()
                         join cpa in await unitOfWork.ClientProjectAuditResponsibilities.GetAsync()
                             on pa.Id equals cpa.ProjectAuditResponsibilityId
                         select mapper.Map<MetaDataViewModel>(pa);

            return result;
        }
        catch (Exception e)
        {
            logger.LogError("{MethodName} - Error occurred: {EMessage}", methodName, e.Message);
            throw;
        }
        finally
        {
            logger.LogInformation("{MethodName} - method execution completed", methodName);
        }
    }
}
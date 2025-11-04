using AutoMapper;
using KonaAI.Master.Business.Tenant.UserMetaData.Logic.Interface;
using KonaAI.Master.Model.Common;
using KonaAI.Master.Repository.Common.Interface;
using Microsoft.Extensions.Logging;

namespace KonaAI.Master.Business.Tenant.UserMetaData.Logic;

/// <summary>
/// Provides business logic for managing <see cref="MetaDataViewModel"/> entities.
/// </summary>
public class ClientProjectRiskAreaBusiness(ILogger<ClientProjectRiskAreaBusiness> logger,
    IMapper mapper,
    IUnitOfWork unitOfWork) : IClientProjectRiskAreaBusiness
{
    private const string ClassName = nameof(ClientProjectRiskAreaBusiness);

    /// <summary>
    /// Retrieves all <see cref="MetaDataViewModel"/> entities asynchronously.
    /// </summary>
    /// <returns>An <see cref="IQueryable{RiskArea}"/> containing all risk areas.</returns>
    /// <exception cref="Exception">Throws any exception that occurs during retrieval.</exception>
    public async Task<IQueryable<MetaDataViewModel>> GetAsync()
    {
        const string methodName = $"{ClassName}: {nameof(GetAsync)}";

        try
        {
            logger.LogInformation("{MethodName} - method execution started", methodName);

            var result = from pr in await unitOfWork.ProjectRiskAreas.GetAsync()
                         join cpr in await unitOfWork.ClientProjectRiskAreas.GetAsync()
                             on pr.Id equals cpr.ProjectRiskAreaId
                         select mapper.Map<MetaDataViewModel>(pr);

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
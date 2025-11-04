using AutoMapper;
using KonaAI.Master.Business.Tenant.UserMetaData.Logic.Interface;
using KonaAI.Master.Model.Common;
using KonaAI.Master.Repository.Common.Interface;
using Microsoft.Extensions.Logging;

namespace KonaAI.Master.Business.Tenant.UserMetaData.Logic;

/// <summary>
/// Provides business logic for managing business units.
/// </summary>
/// <param name="logger">The logger instance used for logging information and errors.</param>
/// <param name="mapper">The AutoMapper instance for mapping entities.</param>
/// <param name="unitOfWork">The unit of work to access repositories.</param>
public class ClientProjectUnitBusiness(ILogger<ClientProjectUnitBusiness> logger,
    IMapper mapper,
    IUnitOfWork unitOfWork) : IClientProjectUnitBusiness
{
    /// <summary>
    /// The name of this class, used for logging purposes.
    /// </summary>
    private const string ClassName = nameof(ClientProjectUnitBusiness);

    /// <summary>
    /// Retrieves all business units asynchronously.
    /// </summary>
    /// <returns>
    /// An <see cref="IQueryable{BusinessUnit}"/> containing the list of business units.
    /// </returns>
    /// <exception cref="Exception">
    /// Thrown when an unexpected error occurs while retrieving business units.
    /// </exception>
    public async Task<IQueryable<MetaDataViewModel>> GetAsync()
    {
        const string methodName = $"{ClassName}: {nameof(GetAsync)}";

        try
        {
            logger.LogInformation("{MethodName} - method execution started", methodName);

            var result = from pu in await unitOfWork.ProjectUnits.GetAsync()
                         join cpu in await unitOfWork.ClientProjectUnits.GetAsync()
                             on pu.Id equals cpu.ProjectUnitId
                         select mapper.Map<MetaDataViewModel>(pu);

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
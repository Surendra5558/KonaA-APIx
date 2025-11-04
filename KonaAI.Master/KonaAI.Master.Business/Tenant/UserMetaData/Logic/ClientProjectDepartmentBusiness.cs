using AutoMapper;
using KonaAI.Master.Business.Tenant.UserMetaData.Logic.Interface;
using KonaAI.Master.Model.Common;
using KonaAI.Master.Repository.Common.Interface;
using Microsoft.Extensions.Logging;

namespace KonaAI.Master.Business.Tenant.UserMetaData.Logic;

/// <summary>
/// Provides business logic for retrieving Project Department metadata.
/// Implements <see cref="IClientProjectDepartmentBusiness"/>.
/// </summary>
/// <param name="logger">The logger used for diagnostic and operational logging.</param>
/// <param name="mapper">The AutoMapper instance used to map repository entities to <see cref="MetaDataViewModel"/>.</param>
/// <param name="unitOfWork">The unit-of-work providing access to repositories.</param>
public class ClientProjectDepartmentBusiness(ILogger<ClientProjectDepartmentBusiness> logger,
IMapper mapper,
IUnitOfWork unitOfWork) : IClientProjectDepartmentBusiness
{
    private const string ClassName = nameof(ClientProjectDepartmentBusiness);

    /// <summary>
    /// Asynchronously retrieves all Business Departments as a queryable collection of <see cref="MetaDataViewModel"/>.
    /// </summary>
    /// <returns>
    /// A task that, when completed, provides an <see cref="IQueryable{T}"/> of <see cref="MetaDataViewModel"/> representing all Business Departments.
    /// </returns>
    /// <remarks>Logs start, completion, and any errors encountered.</remarks>
    /// <exception cref="Exception">Propagates any exception thrown by the underlying repository or the mapping process.</exception>
    public async Task<IQueryable<MetaDataViewModel>> GetAsync()
    {
        const string methodName = $"{ClassName}: {nameof(GetAsync)}";

        try
        {
            logger.LogInformation("{MethodName} - method execution started", methodName);

            var result = from pd in await unitOfWork.ProjectDepartments.GetAsync()
                         join ClientProjectDepartmentBusiness in await unitOfWork.ClientProjectDepartments.GetAsync()
                             on pd.Id equals ClientProjectDepartmentBusiness.ProjectDepartmentId
                         select mapper.Map<MetaDataViewModel>(pd);

            return result;
        }
        catch (Exception e)
        {
            logger.LogError("{MethodName} - Error in execution with error - {EMessage}", methodName, e.Message);
            throw;
        }
        finally
        {
            logger.LogInformation("{MethodName} - method execution completed", methodName);
        }
    }
}
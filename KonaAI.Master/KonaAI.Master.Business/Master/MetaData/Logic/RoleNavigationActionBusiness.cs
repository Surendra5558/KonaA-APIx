using AutoMapper;
using KonaAI.Master.Business.Master.MetaData.Logic.Interface;
using KonaAI.Master.Model.Master.MetaData;
using KonaAI.Master.Repository.Common.Interface;
using Microsoft.Extensions.Logging;

namespace KonaAI.Master.Business.Master.MetaData.Logic;

/// <summary>
/// Business logic class for managing role navigation actions.
/// </summary>
public class RoleNavigationUserActionBusiness(
    ILogger<RoleNavigationUserActionBusiness> logger,
    IMapper mapper,
    IUnitOfWork unitOfWork
) : IRoleNavigationUserActionBusiness
{
    private const string ClassName = nameof(RoleNavigationUserActionBusiness);

    /// <summary>
    /// Retrieves all role navigation actions from the repository.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains a queryable collection of <see cref="RoleNavigationUserActionViewModel"/>.
    /// </returns>
    public async Task<IQueryable<RoleNavigationUserActionViewModel>> GetAsync()
    {
        const string methodName = $"{ClassName}: {nameof(GetAsync)}";
        try
        {
            logger.LogInformation("{MethodName} - method execution started", methodName);
            var result = (await unitOfWork.RoleNavigationUserActions.GetAsync())
                .Select(item => mapper.Map<RoleNavigationUserActionViewModel>(item));
            return result;
        }
        catch (Exception e)
        {
            logger.LogError("{MethodName} - Error: {Message}", methodName, e.Message);
            throw;
        }
        finally
        {
            logger.LogInformation("{MethodName} - method execution completed", methodName);
        }
    }
}
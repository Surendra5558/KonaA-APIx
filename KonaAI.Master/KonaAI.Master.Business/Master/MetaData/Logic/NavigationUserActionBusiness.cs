using AutoMapper;
using KonaAI.Master.Business.Master.MetaData.Logic.Interface;
using KonaAI.Master.Model.Master.MetaData;
using KonaAI.Master.Repository.Common.Interface;
using Microsoft.Extensions.Logging;

namespace KonaAI.Master.Business.Master.MetaData.Logic;

/// <summary>
/// Provides business logic operations for <see cref="NavigationUserActionViewModel"/> entities.
/// </summary>
public class NavigationUserActionBusiness(
    ILogger<NavigationUserActionBusiness> logger,
    IMapper mapper,
    IUnitOfWork unitOfWork
) : INavigationUserActionBusiness
{
    private const string ClassName = nameof(NavigationUserActionBusiness);

    /// <summary>
    /// Retrieves all <see cref="NavigationUserActionViewModel"/> records from the data source.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains an <see cref="IQueryable{NavigationUserActionViewModel}"/>
    /// representing all navigation actions.
    /// </returns>
    /// <exception cref="Exception">
    /// Thrown when there is an error retrieving the data from the repository.
    /// </exception>
    public async Task<IQueryable<NavigationUserActionViewModel>> GetAsync()
    {
        const string methodName = $"{ClassName}: {nameof(GetAsync)}";
        try
        {
            logger.LogInformation("{MethodName} - method execution started", methodName);
            var result = (await unitOfWork.NavigationUserActions.GetAsync())
                .Select(item => mapper.Map<NavigationUserActionViewModel>(item));
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
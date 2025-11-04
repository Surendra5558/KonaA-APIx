using AutoMapper;
using KonaAI.Master.Business.Master.MetaData.Logic.Interface;
using KonaAI.Master.Model.Common;
using KonaAI.Master.Repository.Common.Interface;
using Microsoft.Extensions.Logging;

namespace KonaAI.Master.Business.Master.MetaData.Logic;

/// <summary>
/// Provides business logic operations for managing role types.
/// </summary>
public class RoleTypeBusiness(ILogger<RoleTypeBusiness> logger,
    IMapper mapper,
    IUnitOfWork unitOfWork) : IRoleTypeBusiness
{
    private const string ClassName = nameof(RoleTypeBusiness);

    /// <summary>
    /// Retrieves all role types from the repository.
    /// </summary>
    /// <returns>An <see cref="IQueryable{MetaDataViewModel}"/> representing the collection of role types.</returns>
    public async Task<IQueryable<MetaDataViewModel>> GetAsync()
    {
        const string methodName = $"{ClassName}: {nameof(GetAsync)}";

        try
        {
            logger.LogInformation("{MethodName} - method execution started", methodName);

            var roleTypes = await unitOfWork.RoleTypes.GetAsync();
            var result = roleTypes.Select(item => mapper.Map<MetaDataViewModel>(item));

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
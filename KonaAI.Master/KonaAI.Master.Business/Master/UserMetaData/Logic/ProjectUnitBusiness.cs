using AutoMapper;
using KonaAI.Master.Business.Master.UserMetaData.Logic.Interface;
using KonaAI.Master.Model.Common;
using KonaAI.Master.Repository.Common.Interface;
using Microsoft.Extensions.Logging;

namespace KonaAI.Master.Business.Master.UserMetaData.Logic;

/// <summary>
/// Business service to retrieve master Project Units.
/// </summary>
public class ProjectUnitBusiness(
    ILogger<ProjectUnitBusiness> logger,
    IMapper mapper,
    IUnitOfWork unitOfWork
) : IProjectUnitBusiness
{
    private const string ClassName = nameof(ProjectUnitBusiness);

    public async Task<IQueryable<MetaDataViewModel>> GetAsync()
    {
        const string methodName = $"{ClassName}: {nameof(GetAsync)}";
        try
        {
            logger.LogInformation("{MethodName} - method execution started", methodName);

            var result = from pu in await unitOfWork.ProjectUnits.GetAsync()
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
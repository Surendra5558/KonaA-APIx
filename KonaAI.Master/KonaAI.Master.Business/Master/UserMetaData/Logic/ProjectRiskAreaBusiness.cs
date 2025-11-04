using AutoMapper;
using KonaAI.Master.Business.Master.UserMetaData.Logic.Interface;
using KonaAI.Master.Model.Common;
using KonaAI.Master.Repository.Common.Interface;
using Microsoft.Extensions.Logging;

namespace KonaAI.Master.Business.Master.UserMetaData.Logic;

/// <summary>
/// Business service to retrieve master Project Risk Areas.
/// </summary>
public class ProjectRiskAreaBusiness(
    ILogger<ProjectRiskAreaBusiness> logger,
    IMapper mapper,
    IUnitOfWork unitOfWork
) : IProjectRiskAreaBusiness
{
    private const string ClassName = nameof(ProjectRiskAreaBusiness);

    public async Task<IQueryable<MetaDataViewModel>> GetAsync()
    {
        const string methodName = $"{ClassName}: {nameof(GetAsync)}";
        try
        {
            logger.LogInformation("{MethodName} - method execution started", methodName);

            var result = from pra in await unitOfWork.ProjectRiskAreas.GetAsync()
                         select mapper.Map<MetaDataViewModel>(pra);

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
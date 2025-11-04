using AutoMapper;
using KonaAI.Master.Business.Master.UserMetaData.Logic.Interface;
using KonaAI.Master.Model.Common;
using KonaAI.Master.Repository.Common.Interface;
using Microsoft.Extensions.Logging;

namespace KonaAI.Master.Business.Master.UserMetaData.Logic;

/// <summary>
/// Business service to retrieve master Project Audit Responsibilities.
/// </summary>
public class ProjectAuditResponsibilityBusiness(
    ILogger<ProjectAuditResponsibilityBusiness> logger,
    IMapper mapper,
    IUnitOfWork unitOfWork
) : IProjectAuditResponsibilityBusiness
{
    private const string ClassName = nameof(ProjectAuditResponsibilityBusiness);

    /// <inheritdoc />
    public async Task<IQueryable<MetaDataViewModel>> GetAsync()
    {
        const string methodName = $"{ClassName}: {nameof(GetAsync)}";

        try
        {
            logger.LogInformation("{MethodName} - method execution started", methodName);

            var result = from par in await unitOfWork.ProjectAuditResponsibilities.GetAsync()
                         select mapper.Map<MetaDataViewModel>(par);

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
using KonaAI.Master.Business.Tenant.Client.Logic.Interface;
using Quartz;

namespace KonaAI.Master.API.Scheduler;

/// <summary>
/// Quartz job executor for processing project-related scheduled tasks.
/// Implements the IJob interface to execute project scheduler business logic.
/// </summary>
/// <param name="projectSchedulerBusiness">The business service for project scheduling operations.</param>
/// <param name="logger">The logger instance for recording execution information and errors.</param>
public class ProjectExecutor(IProjectSchedulerBusiness projectSchedulerBusiness, ILogger<ProjectExecutor> logger) : IJob
{
    private readonly IProjectSchedulerBusiness _projectSchedulerBusiness = projectSchedulerBusiness;
    private readonly ILogger<ProjectExecutor> _logger = logger;

    /// <summary>
    /// Executes the project scheduler business logic as a Quartz job.
    /// This method is called by the Quartz scheduler when the job is triggered.
    /// </summary>
    /// <param name="context">The job execution context containing job and trigger information.</param>
    /// <returns>A task representing the asynchronous execution of the project scheduler.</returns>
    /// <exception cref="Exception">Thrown when the project scheduler business logic fails to execute.</exception>
    public async Task Execute(IJobExecutionContext context)
    {
        var jobName = context.JobDetail.Key.Name;
        var triggerName = context.Trigger.Key.Name;
        var executionStartTime = DateTime.UtcNow;

        _logger.LogInformation("ProjectExecutor job started - Job: {JobName}, Trigger: {TriggerName}, StartTime: {StartTime}",
            jobName, triggerName, executionStartTime);

        try
        {
            await _projectSchedulerBusiness.Execute();

            var executionDuration = DateTime.UtcNow - executionStartTime;
            _logger.LogInformation("ProjectExecutor job completed successfully - Job: {JobName}, Trigger: {TriggerName}, Duration: {Duration}ms",
                jobName, triggerName, executionDuration.TotalMilliseconds);
        }
        catch (Exception ex)
        {
            var executionDuration = DateTime.UtcNow - executionStartTime;
            _logger.LogError(ex, "ProjectExecutor job failed - Job: {JobName}, Trigger: {TriggerName}, Duration: {Duration}ms, Error: {ErrorMessage}",
                jobName, triggerName, executionDuration.TotalMilliseconds, ex.Message);
            throw;
        }
    }
}
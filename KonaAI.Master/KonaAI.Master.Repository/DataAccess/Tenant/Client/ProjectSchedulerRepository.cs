using KonaAI.Master.Repository.Common;
using KonaAI.Master.Repository.DataAccess.Tenant.Client.Interface;
using KonaAI.Master.Repository.Domain.Tenant.Client;
using Microsoft.EntityFrameworkCore;

namespace KonaAI.Master.Repository.DataAccess.Tenant.Client;

/// <summary>
/// Repository for performing CRUD operations on <see cref="ProjectScheduler"/> entities within the <see cref="DefaultContext"/>.
/// Inherits generic data access methods from <see cref="GenericRepository{DefaultContext, ProjectScheduler}"/>
/// and implements <see cref="IProjectSchedulerRepository"/>.
/// </summary>
public class ProjectSchedulerRepository(DefaultContext context)
    : GenericRepository<DefaultContext, ProjectScheduler>(context), IProjectSchedulerRepository
{
    public async Task<long> GetNextSchedulerIdAsync()
    {
        var max = await Context.Set<ProjectScheduler>()
            .AsNoTracking()
            .MaxAsync(x => (long?)x.ProjectSchedulerId) ?? 0L;
        return max + 1L;
    }
}

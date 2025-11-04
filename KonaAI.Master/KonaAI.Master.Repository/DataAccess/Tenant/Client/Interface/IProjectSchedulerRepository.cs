using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Repository.Domain.Tenant.Client;

namespace KonaAI.Master.Repository.DataAccess.Tenant.Client.Interface
{
    /// <summary>
    /// Defines repository operations for <see cref="ProjectScheduler"/> entities within the <see cref="DefaultContext"/>.
    /// Inherits standard CRUD and query methods from <see cref="IRepository{DefaultContext, ProjectScheduler}"/>,
    /// including asynchronous retrieval, addition, update, and deletion of <see cref="ProjectScheduler"/> records.
    /// </summary>
    public interface IProjectSchedulerRepository : IRepository<DefaultContext, ProjectScheduler>
    {
        Task<long> GetNextSchedulerIdAsync();
    }
}

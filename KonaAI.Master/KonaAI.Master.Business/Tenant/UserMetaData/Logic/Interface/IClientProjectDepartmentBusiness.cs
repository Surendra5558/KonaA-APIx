using KonaAI.Master.Model.Common;

namespace KonaAI.Master.Business.Tenant.UserMetaData.Logic.Interface;

/// <summary>
/// Defines the contract for business department-related operations.
/// </summary>
public interface IClientProjectDepartmentBusiness
{
    /// <summary>
    /// Asynchronously retrieves a collection of business departments.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains an <see cref="IQueryable{BusinessDepartment}"/>
    /// representing the collection of business departments.
    /// </returns>
    Task<IQueryable<MetaDataViewModel>> GetAsync();
}
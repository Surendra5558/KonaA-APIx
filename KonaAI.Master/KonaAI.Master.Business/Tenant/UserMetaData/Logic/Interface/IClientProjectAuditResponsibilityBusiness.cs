using KonaAI.Master.Model.Common;

namespace KonaAI.Master.Business.Tenant.UserMetaData.Logic.Interface;

/// <summary>
/// Defines the contract for managing <see cref="MetaDataViewModel"/> entities.
/// </summary>
public interface IClientProjectAuditResponsibilityBusiness
{
    /// <summary>
    /// Asynchronously retrieves a collection of <see cref="MetaDataViewModel"/> entities.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation, containing an <see cref="IQueryable{T}"/>
    /// of <see cref="MetaDataViewModel"/> entities.
    /// </returns>
    Task<IQueryable<MetaDataViewModel>> GetAsync();
}
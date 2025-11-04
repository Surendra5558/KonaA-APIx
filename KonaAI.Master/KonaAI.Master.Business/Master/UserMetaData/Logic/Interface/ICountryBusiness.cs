using KonaAI.Master.Model.Common;

namespace KonaAI.Master.Business.Master.UserMetaData.Logic.Interface;

/// <summary>
/// Defines business operations for master Country metadata.
/// </summary>
public interface ICountryBusiness
{
    /// <summary>
    /// Retrieves countries projected to MetaDataViewModel.
    /// </summary>
    Task<IQueryable<MetaDataViewModel>> GetAsync();
}
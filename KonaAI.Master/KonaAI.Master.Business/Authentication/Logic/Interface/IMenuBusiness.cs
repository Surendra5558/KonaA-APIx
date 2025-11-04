using KonaAI.Master.Model.Common;

namespace KonaAI.Master.Business.Authentication.Logic.Interface;

public interface IMenuBusiness
{
    public Task<IQueryable<MetaDataViewModel>> GetAsync();
}
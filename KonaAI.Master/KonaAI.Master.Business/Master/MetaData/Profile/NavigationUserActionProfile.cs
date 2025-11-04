using KonaAI.Master.Model.Master.MetaData;
using KonaAI.Master.Model.Common;
using KonaAI.Master.Repository.Domain.Master.MetaData;

namespace KonaAI.Master.Business.Master.MetaData.Profile;

public class NavigationUserActionProfile : AutoMapper.Profile
{
    public NavigationUserActionProfile()
    {
        CreateMap<NavigationUserAction, NavigationUserActionViewModel>();

        CreateMap<NavigationUserAction, MetaDataViewModel>();
    }
}
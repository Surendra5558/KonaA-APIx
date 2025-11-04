using KonaAI.Master.Model.Common;
using KonaAI.Master.Model.Master.SaveModel;
using KonaAI.Master.Model.Master.MetaData; // Added for RoleNavigationUserActionViewModel
using KonaAI.Master.Repository.Domain.Master.MetaData;

namespace KonaAI.Master.Business.Master.MetaData.Profile;

/// <summary>
/// AutoMapper profile for mapping <see cref="MetaDataViewModel"/> to <see cref="Country"/>.
/// </summary>
public class CountryProfile : AutoMapper.Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CountryProfile"/> class
    /// and configures mappings for <see cref="Country"/>.
    /// </summary>
    public CountryProfile()
    {
        CreateMap<MetaDataViewModel, Country>();
    }
}

/// <summary>
/// AutoMapper profile for mapping <see cref="MetaDataViewModel"/> to <see cref="ModuleSourceType"/>.
/// </summary>
public class ModuleSourceTypeProfile : AutoMapper.Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ModuleSourceTypeProfile"/> class
    /// and configures mappings for <see cref="ModuleSourceType"/>.
    /// </summary>
    public ModuleSourceTypeProfile()
    {
        CreateMap<MetaDataViewModel, ModuleSourceType>();
    }
}

/// <summary>
/// AutoMapper profile for mapping <see cref="MetaDataViewModel"/> to <see cref="ModuleType"/>.
/// </summary>
public class ModuleTypeProfile : AutoMapper.Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ModuleTypeProfile"/> class
    /// and configures mappings for <see cref="ModuleType"/>.
    /// </summary>
    public ModuleTypeProfile()
    {
        CreateMap<MetaDataViewModel, ModuleType>();
        CreateMap<ModuleType, MetaDataViewModel>();
    }
}

/// <summary>
/// AutoMapper profile for mapping <see cref="RenderType"/> to <see cref="MetaDataViewModel"/>.
/// </summary>
public class RenderTypeProfile : AutoMapper.Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RenderTypeProfile"/> class
    /// and configures mappings for <see cref="RenderType"/>.
    /// </summary>
    public RenderTypeProfile()
    {
        CreateMap<RenderType, MetaDataViewModel>();
    }
}

/// <summary>
/// AutoMapper profile for mapping <see cref="RenderTypeCreateModel"/> to <see cref="RenderType"/>.
/// </summary>
public class RenderTypeCreateProfile : AutoMapper.Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RenderTypeCreateProfile"/> class
    /// and configures mappings for creating <see cref="RenderType"/> entities.
    /// </summary>
    public RenderTypeCreateProfile()
    {
        CreateMap<RenderTypeCreateModel, RenderType>();
    }
}

/// <summary>
/// AutoMapper profile for mapping <see cref="RoleNavigationUserAction"/> to <see cref="RoleNavigationUserActionViewModel"/>.
/// </summary>
public class RoleNavigationUserActionProfile : AutoMapper.Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RoleNavigationUserActionProfile"/> class
    /// and configures mappings for <see cref="RoleNavigationUserAction"/>.
    /// </summary>
    public RoleNavigationUserActionProfile()
    {
        CreateMap<RoleNavigationUserAction, RoleNavigationUserActionViewModel>()
            .ForMember(d => d.NavigationName, o =>
            {
                o.NullSubstitute(string.Empty);
                o.MapFrom(_ => string.Empty);
            })
            .ForMember(d => d.UserActionName, o =>
            {
                o.NullSubstitute(string.Empty);
                o.MapFrom(_ => string.Empty);
            })
            .ForMember(d => d.RoleTypeName, o =>
            {
                o.NullSubstitute(string.Empty);
                o.MapFrom(_ => string.Empty);
            });

        CreateMap<RoleNavigationUserAction, MetaDataViewModel>()
            .ForMember(d => d.Name, o => o.NullSubstitute(string.Empty))
            .ForMember(d => d.Description, o => o.NullSubstitute(string.Empty));
    }
}

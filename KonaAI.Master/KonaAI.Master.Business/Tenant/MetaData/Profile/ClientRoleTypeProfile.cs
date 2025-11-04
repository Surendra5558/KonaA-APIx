using KonaAI.Master.Model.Common;
using KonaAI.Master.Model.Tenant.Client.SaveModel;
using KonaAI.Master.Repository.Domain.Master.MetaData;
using KonaAI.Master.Repository.Domain.Tenant.ClientMetaData;

namespace KonaAI.Master.Business.Tenant.MetaData.Profile;

/// <summary>
/// AutoMapper profile for mapping from <see cref="ClientRoleType"/> to <see cref="MetaDataViewModel"/>.
/// </summary>
public class MetaDataViewModelProfile : AutoMapper.Profile
{
    public MetaDataViewModelProfile()
    {
        CreateMap<ClientRoleType, MetaDataViewModel>();
    }
}

/// <summary>
/// AutoMapper profile for mapping from <see cref="ClientRoleTypeCreateModel"/> to <see cref="RoleType"/>.
/// Sets the <c>RowId</c> property to a new <see cref="Guid"/> value during mapping.
/// </summary>
public class RoleTypeCreateModelProfile : AutoMapper.Profile
{
    public RoleTypeCreateModelProfile()
    {
        CreateMap<ClientRoleTypeCreateModel, RoleType>()
            .ForMember(dest => dest.RowId, opt => opt.MapFrom(src => Guid.NewGuid()));
    }
}

/// <summary>
/// AutoMapper profile for mapping from <see cref="ClientRoleTypeCreateModel"/> to <see cref="ClientRoleType"/>.
/// Sets the <c>RowId</c> property to a new <see cref="Guid"/> value during mapping.
/// </summary>
public class ClientRoleTypeCreateModelProfile : AutoMapper.Profile
{
    public ClientRoleTypeCreateModelProfile()
    {
        CreateMap<ClientRoleTypeCreateModel, ClientRoleType>()
            .ForMember(dest => dest.RowId, opt => opt.MapFrom(src => Guid.NewGuid()));
    }
}

/// <summary>
/// AutoMapper profile for mapping from <see cref="ClientRoleTypeUpdateModel"/> to <see cref="RoleType"/>.
/// Ignores immutable fields.
/// </summary>
public class RoleTypeUpdateModelProfile : AutoMapper.Profile
{
    public RoleTypeUpdateModelProfile()
    {
        CreateMap<ClientRoleTypeUpdateModel, RoleType>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.RowId, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedById, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}

/// <summary>
/// AutoMapper profile for mapping from <see cref="ClientRoleTypeUpdateModel"/> to <see cref="ClientRoleType"/>.
/// Ignores immutable fields.
/// </summary>
public class ClientRoleTypeUpdateModelProfile : AutoMapper.Profile
{
    public ClientRoleTypeUpdateModelProfile()
    {
        CreateMap<ClientRoleTypeUpdateModel, ClientRoleType>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.RowId, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedById, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}

/// <summary>
/// AutoMapper profile for mapping from <see cref="RoleType"/> to <see cref="MetaDataViewModel"/>.
/// This allows joining RoleType fields directly into the view model.
/// </summary>
public class RoleTypeViewModelProfile : AutoMapper.Profile
{
    public RoleTypeViewModelProfile()
    {
        CreateMap<RoleType, MetaDataViewModel>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.OrderBy, opt => opt.MapFrom(src => src.OrderBy));
    }
}
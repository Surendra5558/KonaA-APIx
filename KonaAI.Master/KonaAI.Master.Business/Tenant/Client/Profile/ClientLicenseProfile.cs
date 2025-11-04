using KonaAI.Master.Model.Tenant.Client.SaveModel;
using KonaAI.Master.Model.Tenant.Client.ViewModel;
using KonaAI.Master.Repository.Domain.Tenant.Client;

namespace KonaAI.Master.Business.Tenant.Client.Profile;

/// <summary>
/// AutoMapper profile for mapping from <see cref="ClientLicense"/> to <see cref="ClientLicenseViewModel"/>.
/// </summary>
public class ClientLicenseViewModelProfile : AutoMapper.Profile
{
    public ClientLicenseViewModelProfile()
    {
        CreateMap<ClientLicense, ClientLicenseViewModel>();
    }
}

/// <summary>
/// AutoMapper profile for mapping from <see cref="ClientLicenseCreateModel"/> to <see cref="ClientLicense"/>.
/// Sets the <c>RowId</c> property to a new <see cref="Guid"/> value during mapping.
/// </summary>
public class ClientLicenseCreateModelProfile : AutoMapper.Profile
{
    public ClientLicenseCreateModelProfile()
    {
        CreateMap<ClientLicenseCreateModel, ClientLicense>()
            .ForMember(dest => dest.RowId, opt => opt.MapFrom(src => Guid.NewGuid()));
    }
}

/// <summary>
/// AutoMapper profile for mapping from <see cref="ClientLicenseUpdateModel"/> to <see cref="ClientLicense"/>.
/// Ignores immutable audit fields during update.
/// </summary>
public class ClientLicenseUpdateModelProfile : AutoMapper.Profile
{
    public ClientLicenseUpdateModelProfile()
    {
        CreateMap<ClientLicenseUpdateModel, ClientLicense>()
            .ForMember(dest => dest.RowId, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedById, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedOn, opt => opt.Ignore())
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}
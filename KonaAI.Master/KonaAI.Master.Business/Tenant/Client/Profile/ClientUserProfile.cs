using KonaAI.Master.Model.Tenant.Client.SaveModel;
using KonaAI.Master.Model.Tenant.Client.ViewModel;
using KonaAI.Master.Repository.Domain.Master.App;
using KonaAI.Master.Repository.Domain.Tenant.Client;

namespace KonaAI.Master.Business.Tenant.Client.Profile;

/// <summary>
/// AutoMapper profile for mapping from <see cref="Client"/> to <see cref="ClientUserViewModel"/>.
/// </summary>
public class ClientUserViewModelProfile : AutoMapper.Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ClientUserViewModelProfile"/> class
    /// and configures the mapping from <see cref="Client"/> to <see cref="ClientUserViewModel"/>.
    /// </summary>
    public ClientUserViewModelProfile()
    {
        CreateMap<ClientUser, ClientUserViewModel>();
    }
}

/// <summary>
/// AutoMapper profile for mapping from <see cref="ClientUserCreateModel"/> to <see cref="Client"/>.
/// Sets the <c>Id</c> property to a new <see cref="Guid"/> value during mapping.
/// </summary>
public class ClientUserCreateModelProfile : AutoMapper.Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ClientUserCreateModelProfile"/> class
    /// and configures the mapping from <see cref="ClientUserCreateModel"/> to <see cref="Client"/>.
    /// </summary>
    public ClientUserCreateModelProfile()
    {
        CreateMap<ClientUserCreateModel, ClientUser>()
            .ForMember(dest => dest.RowId, opt => opt.MapFrom(src => Guid.NewGuid()));
    }
}

/// <summary>
/// AutoMapper profile for mapping from <see cref="ClientUserCreateModel"/> to <see cref="Client"/>.
/// Sets the <c>Id</c> property to a new <see cref="Guid"/> value during mapping.
/// </summary>
public class ClientUserUpdateModelProfile : AutoMapper.Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ClientUserCreateModelProfile"/> class
    /// and configures the mapping from <see cref="ClientUserCreateModel"/> to <see cref="Client"/>.
    /// </summary>
    public ClientUserUpdateModelProfile()
    {
        CreateMap<ClientUserUpdateModel, ClientUser>();
    }
}

/// <summary>
/// AutoMapper profile for mapping from <see cref="Client"/> to <see cref="ClientUserViewModel"/>.
/// </summary>
public class UserViewModelProfile : AutoMapper.Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ClientUserViewModelProfile"/> class
    /// and configures the mapping from <see cref="Client"/> to <see cref="ClientUserViewModel"/>.
    /// </summary>
    public UserViewModelProfile()
    {
        CreateMap<User, ClientUserViewModel>();
    }
}

/// <summary>
/// AutoMapper profile for mapping from <see cref="Client"/> to <see cref="ClientUserViewModel"/>.
/// </summary>
public class UserCreateModelProfile : AutoMapper.Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ClientUserViewModelProfile"/> class
    /// and configures the mapping from <see cref="Client"/> to <see cref="ClientUserViewModel"/>.
    /// </summary>
    public UserCreateModelProfile()
    {
        CreateMap<ClientUserCreateModel, User>();
    }
}

/// <summary>
/// AutoMapper profile for mapping from <see cref="Client"/> to <see cref="ClientUserViewModel"/>.
/// </summary>
public class UserUpdateModelProfile : AutoMapper.Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserUpdateModelProfile"/> class
    /// and configures the mapping from <see cref="Client"/> to <see cref="ClientUserViewModel"/>.
    /// </summary>
    public UserUpdateModelProfile()
    {
        CreateMap<ClientUserUpdateModel, User>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.RowId, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedById, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.Password, opt => opt.Ignore())
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}
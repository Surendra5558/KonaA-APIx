using KonaAI.Master.Model.Master.App.SaveModel;
using KonaAI.Master.Model.Master.App.ViewModel;
using KonaAI.Master.Model.Tenant.Client.SaveModel;
using KonaAI.Master.Repository.Domain.Master.App;
using KonaAI.Master.Repository.Domain.Tenant.Client;

namespace KonaAI.Master.Business.Master.App.Profile;

/// <summary>
/// AutoMapper profile for mapping from <see cref="Client"/> to <see cref="ClientViewModel"/>.
/// </summary>
public class ClientViewModelProfile : AutoMapper.Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ClientViewModelProfile"/> class
    /// and configures the mapping from <see cref="Client"/> to <see cref="ClientViewModel"/>.
    /// </summary>
    public ClientViewModelProfile()
    {
        CreateMap<Client, ClientViewModel>();
    }
}

/// <summary>
/// AutoMapper profile for mapping from <see cref="ClientCreateModel"/> to <see cref="Client"/>.
/// Sets the <c>Id</c> property to a new <see cref="Guid"/> value during mapping.
/// </summary>
public class ClientCreateModelProfile : AutoMapper.Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ClientCreateModelProfile"/> class
    /// and configures the mapping from <see cref="ClientCreateModel"/> to <see cref="Client"/>.
    /// </summary>
    public ClientCreateModelProfile()
    {
        CreateMap<ClientCreateModel, Client>()
           .ForMember(dest => dest.Id, opt => opt.Ignore());
    }
}

/// <summary>
/// AutoMapper profile for mapping from <see cref="ClientUpdateModel"/> to <see cref="Client"/>.
/// Ignores the <c>Id</c> property during mapping to prevent overwriting the entity's identifier.
/// </summary>
public class ClientUpdateModelProfile : AutoMapper.Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ClientUpdateModelProfile"/> class
    /// and configures the mapping from <see cref="ClientUpdateModel"/> to <see cref="Client"/>.
    /// </summary>
    public ClientUpdateModelProfile()
    {
        CreateMap<ClientUpdateModel, Client>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
    }
}

public class ClientProjectcreateModelProfile : AutoMapper.Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ClientProjectcreateModelProfile"/> class
    /// and configures the mapping from <see cref="ClientProjectcreateModelProfile"/> to <see cref="Client"/>.
    /// </summary>
    public ClientProjectcreateModelProfile()
    {
        CreateMap<ClientProjectCreateModel, ClientProject>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
    }
}

public class ClientProfile : ClientViewModelProfile
{
    public ClientProfile()
    {
        CreateMap<ClientCreateModel, Client>();
    }
}
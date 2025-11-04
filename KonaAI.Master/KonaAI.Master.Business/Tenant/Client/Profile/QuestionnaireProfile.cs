using KonaAI.Master.Model.Tenant.Client.SaveModel;
using KonaAI.Master.Model.Tenant.Client.ViewModel;
using KonaAI.Master.Repository.Domain.Master.App;
using KonaAI.Master.Repository.Domain.Master.MetaData;
using KonaAI.Master.Repository.Domain.Tenant.Client;
using Newtonsoft.Json;

namespace KonaAI.Master.Business.Tenant.Client.Profile;

/// <summary>
/// AutoMapper profile that projects aggregated client question-bank data into <see cref="ClientQuestionBankViewModel"/>.
/// </summary>
/// <remarks>
/// Uses a temporary composite source (<see cref="TempMapper"/>) to combine fields from
/// <see cref="ClientQuestionBank"/>, <see cref="QuestionBank"/>, and <see cref="RenderType"/>.
/// </remarks>
public class ClientQuestionBankViewProfile : AutoMapper.Profile
{
    /// <summary>
    /// Composite mapping source used to hydrate <see cref="ClientQuestionBankViewModel"/> from multiple domain entities.
    /// </summary>
    public class TempMapper
    {
        /// <summary>
        /// Gets or sets the client-specific question bank entity for client-level identifiers and linkage.
        /// </summary>
        public ClientQuestionBank ClientQuestionBank { get; set; } = new();

        /// <summary>
        /// Gets or sets the master question definition providing description and audit fields.
        /// </summary>
        public QuestionBank QuestionBank { get; set; } = new();

        /// <summary>
        /// Gets or sets the optional render type metadata used to derive the render name.
        /// </summary>
        public RenderType? RenderType { get; set; }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ClientQuestionBankViewProfile"/> class
    /// and configures mapping from <see cref="TempMapper"/> to <see cref="ClientQuestionBankViewModel"/>.
    /// </summary>
    public ClientQuestionBankViewProfile()
    {
        CreateMap<TempMapper, ClientQuestionBankViewModel>()
            .ForMember(dest => dest.Description, opt =>
                opt.MapFrom(src => src.QuestionBank.Description))
            .ForMember(dest => dest.RenderType, opt =>
                opt.MapFrom(src => src.RenderType != null ? src.RenderType.Name : null))
            .ForMember(dest => dest.Options, opt =>
                opt.MapFrom(src => TryDeserializeList(src.QuestionBank.Options)))
            .ForMember(dest => dest.CreatedOn, opt =>
                opt.MapFrom(src => src.QuestionBank.CreatedOn))
            .ForMember(dest => dest.ClientId, opt =>
                opt.MapFrom(src => src.ClientQuestionBank.ClientId))
            .ForMember(dest => dest.RowId, opt =>
                opt.MapFrom(src => src.ClientQuestionBank.RowId));
    }

    /// <summary>
    /// Safely attempts to deserialize a JSON string into a list of strings.
    /// Falls back to a single-item list containing the raw value on failure.
    /// </summary>
    /// <param name="json">The JSON string to deserialize.</param>
    /// <returns>List of strings parsed from JSON or fallback.</returns>
    private static List<string>? TryDeserializeList(string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return null;
        }

        try
        {
            var list = JsonConvert.DeserializeObject<List<string>>(json.Trim());
            return list;
        }
        catch
        {
            return new List<string> { json };
        }
    }
}

/// <summary>
/// AutoMapper profile for mapping from <see cref="ClientQuestionBankCreateModel"/> to <see cref="QuestionBank"/>.
/// </summary>
public class ClientQuestionBankCreateModelProfile : AutoMapper.Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ClientQuestionBankCreateModelProfile"/> class
    /// and configures the mapping from <see cref="ClientQuestionBankCreateModel"/> to <see cref="QuestionBank"/>.
    /// </summary>
    public ClientQuestionBankCreateModelProfile()
    {
        CreateMap<ClientQuestionBankCreateModel, QuestionBank>()
            .ForMember(dest => dest.Options,
                opt => opt.MapFrom(src =>
                    src.Options == null ? null : JsonConvert.SerializeObject(src.Options)))
            .ForMember(dest => dest.IsMandatory, opt =>
                opt.MapFrom(src => src.Required))
            .ForMember(dest => dest.LinkedQuestion, opt =>
                opt.Ignore())
            .ForMember(dest => dest.OnAction, opt =>
                opt.Ignore())
            .ForMember(dest => dest.Description, opt =>
                opt.MapFrom(src => src.Text));
    }
}

/// <summary>
/// AutoMapper profile for mapping from <see cref="ClientQuestionnaireCreateModel"/> to <see cref="ClientQuestionnaire"/>.
/// </summary>
public class ClientQuestionnaireCreateModelProfile : AutoMapper.Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ClientQuestionnaireCreateModelProfile"/> class
    /// and configures the mapping from <see cref="ClientQuestionnaireCreateModel"/> to <see cref="ClientQuestionnaire"/>.
    /// </summary>
    public ClientQuestionnaireCreateModelProfile()
    {
        CreateMap<ClientQuestionnaireCreateModel, ClientQuestionnaire>();
    }
}

/// <summary>
/// AutoMapper profile for mapping <see cref="ClientQuestionnaire"/> entities
/// to <see cref="ClientQuestionnaireViewModel"/> objects.
/// </summary>
public class ClientQuestionnaireViewModelProfile : AutoMapper.Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ClientQuestionnaireViewModelProfile"/> class
    /// and configures the mapping from <see cref="ClientQuestionnaire"/> to <see cref="ClientQuestionnaireViewModel"/>.
    /// </summary>
    public ClientQuestionnaireViewModelProfile()
    {
        CreateMap<ClientQuestionnaire, ClientQuestionnaireViewModel>();
    }
}

/// <summary>
/// AutoMapper profile for mapping <see cref="ClientQuestionnaireAssociation"/> entities
/// to <see cref="ClientQuestionnaireAssociationViewModel"/> objects.
/// </summary>
public class ClientQuestionnaireAssociationViewModelProfile : AutoMapper.Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ClientQuestionnaireAssociationViewModelProfile"/> class
    /// and configures the mapping from <see cref="ClientQuestionnaireAssociation"/> to <see cref="ClientQuestionnaireAssociationViewModel"/>.
    /// </summary>
    public ClientQuestionnaireAssociationViewModelProfile()
    {
        CreateMap<ClientQuestionnaireAssociation, ClientQuestionnaireAssociationViewModel>();
    }
}

/// <summary>
/// AutoMapper profile for mapping <see cref="ClientQuestionnaireSection"/> entities
/// to <see cref="ClientQuestionSectionViewModel"/> objects.
/// </summary>
public class ClientQuestionnaireSectionViewModelProfile : AutoMapper.Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ClientQuestionnaireSectionViewModelProfile"/> class
    /// and configures the mapping from <see cref="ClientQuestionnaireSection"/> to <see cref="ClientQuestionSectionViewModel"/>.
    /// </summary>
    public ClientQuestionnaireSectionViewModelProfile()
    {
        CreateMap<ClientQuestionnaireSection, ClientQuestionSectionViewModel>();
    }
}

/// <summary>
/// AutoMapper profile for mapping from <see cref="ClientQuestionnaireAssociationCreateModel"/> to <see cref="ClientQuestionnaireAssociation"/>.
/// </summary>
public class ClientQuestionnaireAssociationCreateModelProfile : AutoMapper.Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ClientQuestionnaireAssociationCreateModelProfile"/> class
    /// and configures the mapping from <see cref="ClientQuestionnaireAssociationCreateModel"/> to <see cref="ClientQuestionnaireAssociation"/>.
    /// </summary>
    public ClientQuestionnaireAssociationCreateModelProfile()
    {
        CreateMap<ClientQuestionnaireAssociationCreateModel, ClientQuestionnaireAssociation>();
    }
}

/// <summary>
/// AutoMapper profile for mapping from <see cref="ClientQuestionnaireSectionCreateModel"/> to <see cref="ClientQuestionnaireSection"/>.
/// </summary>
public class ClientQuestionnaireSectionCreateModelProfile : AutoMapper.Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ClientQuestionnaireSectionCreateModelProfile"/> class
    /// and configures the mapping from <see cref="ClientQuestionnaireSectionCreateModel"/> to <see cref="ClientQuestionnaireSection"/>.
    /// </summary>
    public ClientQuestionnaireSectionCreateModelProfile()
    {
        CreateMap<ClientQuestionnaireSectionCreateModel, ClientQuestionnaireSection>();
    }
}

/// <summary>
/// Aggregated AutoMapper profile for client questionnaire create models to entities with default audit field initialization.
/// </summary>
public class ClientQuestionnaireProfile : AutoMapper.Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ClientQuestionnaireProfile"/> class
    /// and configures mappings for questionnaire and section creation including default audit values.
    /// </summary>
    public ClientQuestionnaireProfile()
    {
        CreateMap<ClientQuestionnaireCreateModel, ClientQuestionnaire>()

            .ForMember(dest => dest.RowId, opt
                => opt.MapFrom(_ => Guid.NewGuid()))
            .ForMember(dest => dest.CreatedOn, opt
                => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.ModifiedOn, opt
                => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.IsActive, opt
                => opt.MapFrom(_ => true))
            .ForMember(dest => dest.IsDeleted, opt
                => opt.MapFrom(_ => false));

        CreateMap<ClientQuestionnaireSectionCreateModel, ClientQuestionnaireSection>()
            .ForMember(dest => dest.RowId, opt
                => opt.MapFrom(_ => Guid.NewGuid()))
            .ForMember(dest => dest.CreatedOn, opt
                => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.ModifiedOn, opt
                => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true))
            .ForMember(dest => dest.Name, opt
                => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.IsDeleted, opt
                => opt.MapFrom(_ => false));
    }
}
using KonaAI.Master.Model.Master.App.SaveModel;
using KonaAI.Master.Model.Master.App.ViewModel;
using KonaAI.Master.Model.Tenant.Client.SaveModel;
using KonaAI.Master.Repository.Domain.Master.App;

namespace KonaAI.Master.Business.Master.App.Profile;

/// <summary>
/// AutoMapper profile for mapping <see cref="QuestionBank"/> entities
/// to <see cref="QuestionBankViewModel"/> objects.
/// </summary>
public class QuestionBankViewModelProfile : AutoMapper.Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="QuestionBankViewModelProfile"/> class
    /// and configures the mapping from <see cref="QuestionBank"/> to <see cref="QuestionBankViewModel"/>.
    /// </summary>
    public QuestionBankViewModelProfile()
    {
        CreateMap<QuestionBank, QuestionBankViewModel>();
    }
}

/// <summary>
/// AutoMapper profile for mapping from <see cref="QuestionBankCreateModel"/> to <see cref="QuestionBank"/>.
/// </summary>
public class QuestionBankCreateModelProfile : AutoMapper.Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="QuestionBankCreateModel"/> class
    /// and configures the mapping from <see cref="QuestionBankCreateModel"/> to <see cref="QuestionBank"/>.
    /// </summary>
    public QuestionBankCreateModelProfile()
    {
        CreateMap<QuestionBankCreateModel, QuestionBank>();
    }
}

/// <summary>
/// AutoMapper profile for mapping from <see cref="QuestionBankUpdateModel"/> to <see cref="QuestionBank"/>.
/// </summary>
public class QuestionBankUpdateModelProfile : AutoMapper.Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="QuestionBankUpdateModel"/> class
    /// and configures the mapping from <see cref="QuestionBankUpdateModel"/> to <see cref="QuestionBank"/>.
    /// </summary>
    public QuestionBankUpdateModelProfile()
    {
        CreateMap<QuestionBankUpdateModel, QuestionBank>();
    }
}
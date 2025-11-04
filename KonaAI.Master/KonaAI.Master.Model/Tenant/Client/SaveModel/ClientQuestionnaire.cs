using FluentValidation;
using KonaAI.Master.Model.Common.Constants;

namespace KonaAI.Master.Model.Tenant.Client.SaveModel;

/// <summary>
/// Represents the data required to create a client questionnaire.
/// </summary>
public class ClientQuestionnaireCreateModel
{
    /// <summary>
    /// Gets or sets the name of the questionnaire.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Gets or sets the collection of sections within the questionnaire.
    /// </summary>
    public List<ClientQuestionnaireSectionCreateModel> Sections { get; set; } = new();
}

/// <summary>
/// Represents a section within a questionnaire.
/// </summary>
public class ClientQuestionnaireSectionCreateModel
{
    /// <summary>
    /// Gets or sets the title of the section.
    /// </summary>
    public string Title { get; set; } = null!;

    /// <summary>
    /// Gets or sets the collection of questions in this section.
    /// </summary>
    public List<ClientQuestionnaireQuestionModel> Questions { get; set; } = new();
}

/// <summary>
/// Represents an individual question reference from the Client Question Bank.
/// </summary>
public class ClientQuestionnaireQuestionModel
{
    /// <summary>
    /// Gets or sets the ID of the question from the client question bank.
    /// </summary>
    public string OriginalId { get; set; } = null!;
}

/// <summary>
/// Validator for <see cref="ClientQuestionnaireCreateModel"/>.
/// </summary>
public class ClientQuestionnaireCreateModelValidator : AbstractValidator<ClientQuestionnaireCreateModel>
{
    //
    public ClientQuestionnaireCreateModelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Questionnaire name is required.")
            .MaximumLength(DbColumnLength.Description).WithMessage("Questionnaire name cannot exceed 200 characters.");

        RuleForEach(x => x.Sections)
            .SetValidator(new ClientQuestionnaireSectionCreateModelValidator())
            .When(x => x.Sections.Any());

        RuleFor(x => x.Sections)
            .NotEmpty().WithMessage("At least one section is required in the questionnaire.");
    }
}

/// <summary>
/// Validator for <see cref="ClientQuestionnaireSectionCreateModel"/>.
/// </summary>
public class ClientQuestionnaireSectionCreateModelValidator : AbstractValidator<ClientQuestionnaireSectionCreateModel>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ClientQuestionnaireSectionCreateModelValidator"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="source" /> is <see langword="null" />.</exception>
    public ClientQuestionnaireSectionCreateModelValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Section title is required.")
            .MaximumLength(DbColumnLength.Description).WithMessage("Section title cannot exceed 150 characters.");

        RuleForEach(x => x.Questions)
            .SetValidator(new ClientQuestionnaireQuestionModelValidator())
            .When(x => x.Questions.Any());

        RuleFor(x => x.Questions)
            .NotEmpty().WithMessage("Each section must contain at least one question.");
    }
}

/// <summary>
/// Validator for <see cref="ClientQuestionnaireQuestionModel"/>.
/// </summary>
public class ClientQuestionnaireQuestionModelValidator : AbstractValidator<ClientQuestionnaireQuestionModel>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ClientQuestionnaireQuestionModelValidator"/> class.
    /// </summary>
    public ClientQuestionnaireQuestionModelValidator()
    {
        RuleFor(x => x.OriginalId)
            .NotEmpty().WithMessage("OriginalId (question reference) is required.")
            .Must(BeAValidGuid).WithMessage("OriginalId must be a valid GUID.");
    }

    /// <summary>
    /// Bes a valid unique identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    private bool BeAValidGuid(string id)
    {
        return Guid.TryParse(id, out _);
    }
}
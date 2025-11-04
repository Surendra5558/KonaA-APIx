using FluentValidation;

namespace KonaAI.Master.Model.Tenant.Client.SaveModel;

/// <summary>
/// Represents the association that links a client-specific question (from the Client Question Bank)
/// to a specific questionnaire and section for a given client.
/// </summary>
/// <remarks>
/// Validation highlights:
/// - All identifiers (<see cref="ClientId"/>, <see cref="ClientQuestionBankId"/>, <see cref="QuestionnaireId"/>,
/// <see cref="QuestionnaireSectionId"/>) must be greater than 0.
/// - This model does not enforce cross-entity ownership checks (e.g., that the question bank entry belongs to the same client);
///   such checks should be performed in the application or domain layer.
/// </remarks>
/// <seealso cref="ClientQuestionBankCreateModel"/>
public class ClientQuestionnaireAssociationCreateModel
{
    /// <summary>
    /// Gets or sets the unique identifier of the client.
    /// </summary>
    /// <value>Must be greater than 0.</value>
    public long ClientId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the client-specific question bank item to associate.
    /// </summary>
    /// <value>Must be greater than 0.</value>
    public long ClientQuestionBankId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the questionnaire being associated.
    /// </summary>
    /// <value>Must be greater than 0.</value>
    public long QuestionnaireId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the questionnaire section being linked.
    /// </summary>
    /// <value>Must be greater than 0.</value>
    public long QuestionnaireSectionId { get; set; }
}

/// <summary>
/// Validator for <see cref="ClientQuestionnaireAssociationCreateModel"/> enforcing basic identifier constraints.
/// </summary>
/// <remarks>
/// Rules enforced:
/// - <see cref="ClientQuestionnaireAssociationCreateModel.ClientId"/> &gt; 0
/// - <see cref="ClientQuestionnaireAssociationCreateModel.ClientQuestionBankId"/> &gt; 0
/// - <see cref="ClientQuestionnaireAssociationCreateModel.QuestionnaireId"/> &gt; 0
/// - <see cref="ClientQuestionnaireAssociationCreateModel.QuestionnaireSectionId"/> &gt; 0
/// </remarks>
public class ClientQuestionnaireAssociationCreateModelValidator : AbstractValidator<ClientQuestionnaireAssociationCreateModel>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ClientQuestionnaireAssociationCreateModelValidator"/> class.
    /// </summary>
    public ClientQuestionnaireAssociationCreateModelValidator()
    {
        RuleFor(x => x.ClientId)
            .GreaterThan(0).WithMessage("ClientId must be greater than 0.");

        RuleFor(x => x.ClientQuestionBankId)
            .GreaterThan(0).WithMessage("ClientQuestionBankId must be greater than 0.");

        RuleFor(x => x.QuestionnaireId)
            .GreaterThan(0).WithMessage("QuestionnaireId must be greater than 0.");

        RuleFor(x => x.QuestionnaireSectionId)
            .GreaterThan(0).WithMessage("QuestionnaireSectionId must be greater than 0.");
    }
}
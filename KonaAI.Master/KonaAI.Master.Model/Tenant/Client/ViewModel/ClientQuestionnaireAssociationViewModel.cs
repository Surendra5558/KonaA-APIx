using KonaAI.Master.Model.Common;

/// <summary>
/// Represents an association between a client, question bank, questionnaire, and questionnaire section.
/// </summary>
public class ClientQuestionnaireAssociationViewModel : BaseAuditViewModel
{
    /// <summary>
    /// Gets or sets the unique identifier of the client that owns this association.
    /// </summary>
    public long ClientId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the associated client question bank.
    /// </summary>
    public long ClientQuestionBankId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the associated questionnaire.
    /// </summary>
    public long QuestionnaireId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the associated questionnaire section.
    /// </summary>
    public long QuestionnaireSectionId { get; set; }
}
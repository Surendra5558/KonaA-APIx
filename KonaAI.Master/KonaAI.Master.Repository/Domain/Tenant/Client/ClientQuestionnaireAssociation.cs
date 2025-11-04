using KonaAI.Master.Repository.Common.Domain;

namespace KonaAI.Master.Repository.Domain.Tenant.Client;

/// <summary>
/// Represents an association between a questionnaire, its section,
/// and the client-specific question bank entry.
/// </summary>
public class ClientQuestionnaireAssociation : BaseClientDomain
{
    /// <summary>
    /// Gets or sets the identifier of the client-specific question bank entry
    /// that this association is linked to.
    /// </summary>
    public long ClientQuestionBankId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the questionnaire this association belongs to.
    /// </summary>
    public long QuestionnaireId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the section within the questionnaire.
    /// </summary>
    public long QuestionnaireSectionId { get; set; }
}
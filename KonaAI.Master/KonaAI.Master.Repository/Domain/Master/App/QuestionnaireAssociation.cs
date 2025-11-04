using KonaAI.Master.Repository.Common.Domain;

namespace KonaAI.Master.Repository.Domain.Master.App;

/// <summary>
/// Represents a link between a questionnaire template, its section, and questions.
/// </summary>
public class QuestionnaireAssociation : BaseDomain
{
    /// <summary>
    /// Gets or sets the name of the questionnaire template.
    /// </summary>
    public string QuestionnaireName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the name of the questionnaire section.
    /// </summary>
    public string SectionName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the question Id associated with this section and template.
    /// </summary>
    public long QuestionId { get; set; }
}
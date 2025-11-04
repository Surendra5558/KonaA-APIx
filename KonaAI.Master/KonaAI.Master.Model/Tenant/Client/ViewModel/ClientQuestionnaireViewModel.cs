using KonaAI.Master.Model.Common;

/// <summary>
/// Represents a client questionnaire with auditing details.
/// </summary>
public class ClientQuestionnaireViewModel : BaseAuditViewModel
{
    /// <summary>
    /// Gets or sets the unique identifier of the client that owns this questionnaire.
    /// </summary>
    public long ClientId { get; set; }

    /// <summary>
    /// Gets or sets the name of the questionnaire.
    /// </summary>
    public string? Name { get; set; }
}
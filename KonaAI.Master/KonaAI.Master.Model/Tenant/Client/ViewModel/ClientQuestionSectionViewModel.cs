using KonaAI.Master.Model.Common;

namespace KonaAI.Master.Model.Tenant.Client.ViewModel;

/// <summary>
/// Represents a client-specific sections.
/// </summary>
public class ClientQuestionSectionViewModel : BaseAuditViewModel
{
    /// <summary>
    /// Gets or sets the unique identifier of the client that owns this question bank.
    /// </summary>
    public long ClientId { get; set; }

    /// <summary>
    /// Gets or sets the name of the section.
    /// </summary>
    public string? Name { get; set; }
}
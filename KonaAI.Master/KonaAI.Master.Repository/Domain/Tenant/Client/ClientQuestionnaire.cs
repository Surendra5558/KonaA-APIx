using KonaAI.Master.Repository.Common.Domain;

namespace KonaAI.Master.Repository.Domain.Tenant.Client;

/// <summary>
/// Represents a questionnaire associated with a specific client.
/// </summary>
public class ClientQuestionnaire : BaseClientDomain
{
    /// <summary>
    /// Gets or sets the name or title of the questionnaire section.
    /// </summary>
    public string Name { get; set; } = null!;
}
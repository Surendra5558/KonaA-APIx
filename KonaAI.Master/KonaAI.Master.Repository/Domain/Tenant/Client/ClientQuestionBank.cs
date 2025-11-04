using KonaAI.Master.Repository.Common.Domain;

namespace KonaAI.Master.Repository.Domain.Tenant.Client;

/// <summary>
/// Represents a question entry that is part of a client's specific question bank.
/// </summary>
public class ClientQuestionBank : BaseClientDomain
{
    /// <summary>
    /// Gets or sets the identifier of the question associated with this record.
    /// </summary>
    public long QuestionBankId { get; set; }
}
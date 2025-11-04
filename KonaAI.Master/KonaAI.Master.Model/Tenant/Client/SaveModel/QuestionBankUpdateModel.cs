namespace KonaAI.Master.Model.Tenant.Client.SaveModel;

public class QuestionBankUpdateModel
{
    /// <summary>
    /// Gets or sets the description of the question bank.
    /// </summary>
    public string Description { get; set; } = null!;

    /// <summary>
    /// Gets or sets a value indicating whether the question is mandatory.
    /// </summary>
    public bool IsMandatory { get; set; }

    /// <summary>
    /// Gets or sets the available options for the question (e.g., multiple-choice values).
    /// </summary>
    public string[]? Options { get; set; }

    /// <summary>
    /// Gets or sets the identifier of a question that is logically linked to this one.
    /// </summary>
    public long LinkedQuestion { get; set; }

    /// <summary>
    /// Gets or sets the action or condition to be executed when the question is answered.
    /// </summary>
    public string? OnAction { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this question is marked as the default choice.
    /// </summary>
    public bool IsDefault { get; set; }
}
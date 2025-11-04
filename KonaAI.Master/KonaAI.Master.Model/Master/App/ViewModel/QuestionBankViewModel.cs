using KonaAI.Master.Model.Common;

namespace KonaAI.Master.Model.Master.App.ViewModel;

/// <summary>
/// Represents the view model for a question bank,
/// including its description, options, and configuration flags.
/// Inherits from <see cref="BaseAuditViewModel"/> to include audit fields.
/// </summary>
public class QuestionBankViewModel : BaseAuditViewModel
{
    /// <summary>
    /// Gets or sets the description of the question bank.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether the question is mandatory.
    /// Default value is <c>false</c>.
    /// </summary>
    public bool IsMandatory { get; set; } = false;

    /// <summary>
    /// Gets or sets the available options for the question.
    /// Typically represented as a delimited string or JSON.
    /// </summary>
    public string Options { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the identifier of a linked question, if applicable.
    /// Used to establish dependencies between questions.
    /// </summary>
    public long LinkedQuestion { get; set; }

    /// <summary>
    /// Gets or sets the action to be performed when the question is answered.
    /// Example: show/hide another question.
    /// </summary>
    public string OnAction { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether this is the default question bank.
    /// </summary>
    public bool IsDefault { get; set; }
}
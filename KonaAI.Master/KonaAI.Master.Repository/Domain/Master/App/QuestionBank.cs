using KonaAI.Master.Repository.Common.Domain;

namespace KonaAI.Master.Repository.Domain.Master.App;

/// <summary>
/// Represents a question in the master question bank.
/// </summary>
public class QuestionBank : BaseClientDomain
{
    /// <summary>
    /// Gets or sets the description of the question.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the question is mandatory.
    /// </summary>
    public bool IsMandatory { get; set; }

    /// <summary>
    /// Gets or sets the list of available options for the question.
    /// Stored as a JSON array in the database.
    /// </summary>
    public string? Options { get; set; }

    /// <summary>
    /// Gets or sets the ID of the linked question (used for conditional logic).
    /// </summary>
    public long LinkedQuestion { get; set; }

    /// <summary>
    /// Gets or sets the action to perform when this question is triggered.
    /// Example: show/hide next section, redirect, etc.
    /// </summary>
    public string? OnAction { get; set; }

    /// <summary>
    /// Gets or sets the type of the render.
    /// </summary>
    public long RenderType { get; set; }
}
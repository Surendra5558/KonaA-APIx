using KonaAI.Master.Model.Common;

/// <summary>
/// Represents a client-specific question bank that includes auditing metadata inherited from <see cref="BaseAuditViewModel"/>. [web:21]
/// </summary>
/// <remarks>
/// Use this view model to transport question bank details between layers while preserving audit information such as created/updated metadata. [web:22]
/// </remarks>
public class ClientQuestionBankViewModel : BaseAuditViewModel
{
    /// <summary>
    /// Gets or sets an optional textual description of the question bank for display or administrative purposes. [web:31]
    /// </summary>
    /// <value>
    /// A descriptive string that may be null when no description has been provided. [web:21]
    /// </value>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the render type identifier used by the UI or rendering pipeline to determine presentation. [web:31]
    /// </summary>
    /// <value>
    /// A string token such as a component or layout key; null if the default rendering is implied. [web:21]
    /// </value>
    public string? RenderType { get; set; }

    /// <summary>
    /// Gets or sets the list of available answer options for the question.
    /// </summary>
    public List<string>? Options { get; set; }

    /// <summary>
    /// Gets or sets the date and time (UTC recommended) when the question bank was created. [web:22]
    /// </summary>
    /// <value>
    /// A <see cref="DateTime"/> indicating the creation timestamp used for auditing and sorting. [web:21]
    /// </value>
    public DateTime CreatedOn { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the owning client. [web:31]
    /// </summary>
    /// <value>
    /// A 64-bit integer referencing the client to which this question bank belongs. [web:22]
    /// </value>
    public long ClientId { get; set; }
}
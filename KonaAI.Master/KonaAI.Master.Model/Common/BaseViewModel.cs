namespace KonaAI.Master.Model.Common;

/// <summary>
/// Defines basic identity properties for a view model, including a numeric identifier and a unique row identifier.
/// </summary>
public class BaseViewModel
{
    /// <summary>
    /// Gets or sets the unique row identifier (GUID) of the entity.
    /// </summary>
    public Guid RowId { get; set; }
}
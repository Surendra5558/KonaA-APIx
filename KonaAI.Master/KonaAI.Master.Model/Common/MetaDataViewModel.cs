namespace KonaAI.Master.Model.Common;

/// <summary>
/// Represents a simple metadata item (e.g., for lookups or dropdown lists).
/// Inherits the unique row identifier from <see cref="BaseViewModel"/>.
/// </summary>
public class MetaDataViewModel : BaseViewModel
{
    /// <summary>
    /// Gets or sets the display name of the metadata item.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the optional description for the metadata item.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the sort order for displaying this item.
    /// Lower values appear first.
    /// </summary>
    public int OrderBy { get; set; }
}
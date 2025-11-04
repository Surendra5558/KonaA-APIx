namespace KonaAI.Master.Repository.Common.Constants;

/// <summary>
/// Specifies the data operation mode for domain entities.
/// </summary>
public enum DataModes
{
    /// <summary>
    /// Add a new entity.
    /// </summary>
    Add = 1,

    /// <summary>
    /// Edit an existing entity.
    /// </summary>
    Edit = 2,

    /// <summary>
    /// Delete an entity.
    /// </summary>
    Delete = 3,

    /// <summary>
    /// Deactivate an entity.
    /// </summary>
    DeActive = 4
}
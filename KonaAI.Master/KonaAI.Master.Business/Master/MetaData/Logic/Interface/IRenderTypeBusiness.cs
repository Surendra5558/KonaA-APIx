using KonaAI.Master.Model.Common;
using KonaAI.Master.Model.Master.SaveModel;

/// <summary>
/// Defines business operations for handling render type metadata.
/// </summary>
public interface IRenderTypeBusiness
{
    /// <summary>
    /// Retrieves all render types as metadata view models.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// The task result contains a queryable collection of <see cref="MetaDataViewModel"/> objects.
    /// </returns>
    Task<IQueryable<MetaDataViewModel>> GetAsync();

    /// <summary>
    /// Creates a new render type based on the provided create model.
    /// </summary>
    /// <param name="payload">
    /// The <see cref="RenderTypeCreateModel"/> instance containing the data for the new render type.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains an <see cref="int"/> indicating the number of records created
    /// (typically 1 if the operation succeeds).
    /// </returns>
    Task<int> CreateAsync(RenderTypeCreateModel payload);
}
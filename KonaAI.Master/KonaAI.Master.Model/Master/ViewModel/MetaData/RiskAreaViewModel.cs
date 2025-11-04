namespace KonaAI.Master.Model.Master.ViewModel.MetaData
{
    /// <summary>
    /// Represents a master risk area metadata item.
    /// </summary>
    public class RiskAreaViewModel
    {
        /// <summary>
        /// Gets or sets the Id of the entity.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the Guid of the entity.
        /// </summary>
        public Guid RowId { get; set; }

        /// <summary>
        /// Gets or sets the name of the entity.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the description of the entity.
        /// </summary>
        public string? Description { get; set; }
    }
}
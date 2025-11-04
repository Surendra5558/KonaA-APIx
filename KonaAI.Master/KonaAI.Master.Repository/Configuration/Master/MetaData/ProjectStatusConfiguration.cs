using KonaAI.Master.Repository.Common.Extensions;
using KonaAI.Master.Repository.Domain.Master.MetaData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KonaAI.Master.Repository.Configuration.Master.MetaData;

/// <summary>
/// Configures the <see cref="ProjectStatus"/> entity including base metadata mapping and seed data.
/// </summary>
public class ProjectStatusConfiguration : IEntityTypeConfiguration<ProjectStatus>
{
    /// <summary>
    /// Applies configuration for the <see cref="ProjectStatus"/> entity.
    /// Sets common metadata (via <see cref="EntityBaseConfigurationExtension.BaseMetaDataConfiguration"/>)
    /// and seeds initial client project statuses.
    /// </summary>
    /// <param name="builder">The builder used to configure the <see cref="ProjectStatus"/> entity.</param>
    public void Configure(EntityTypeBuilder<ProjectStatus> builder)
    {
        // Apply shared metadata configuration (keys, audit, soft delete, etc.)
        builder.BaseMetaDataConfiguration("ProjectStatus");

        // Seed initial client project status data.
        builder.HasData(
            new ProjectStatus
            {
                RowId = Guid.Parse("77685DAC-633A-4A8F-A950-FA5B27BB3632"),
                Id = 1,
                Name = "Not Initiated",
                Description = "Client project has not been started yet",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 1
            },
            new ProjectStatus
            {
                RowId = Guid.Parse("DC3DFC15-6CDD-4C17-B639-A24C0ED261C7"),
                Id = 2,
                Name = "In Progress",
                Description = "Client project is currently active and in progress",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 2
            },
            new ProjectStatus
            {
                RowId = Guid.Parse("1F471949-ED48-465C-8399-C7D9C622DCC3"),
                Id = 3,
                Name = "Failed",
                Description = "Client project has failed",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 3
            },
            new ProjectStatus
            {
                RowId = Guid.Parse("741FD5D8-6FBB-4C3C-BCE4-0423E8DE2067"),
                Id = 4,
                Name = "Completed",
                Description = "Client project has completed",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 3
            }
        );
    }
}
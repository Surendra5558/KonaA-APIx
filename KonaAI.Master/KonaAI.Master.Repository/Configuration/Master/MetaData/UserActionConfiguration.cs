using KonaAI.Master.Repository.Common.Extensions;
using KonaAI.Master.Repository.Domain.Master.MetaData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KonaAI.Master.Repository.Configuration.Master.MetaData;

/// <summary>
/// Configures the entity mapping, relationships, and seed data for the <see cref="Action"/> entity.
/// Sets up table schema, metadata properties, and initial seed records.
/// </summary>
public class UserActionConfiguration : IEntityTypeConfiguration<UserAction>
{
    /// <summary>
    /// Configures the <see cref="Action"/> entity's schema, metadata property constraints, and seed data
    /// using the provided <see cref="EntityTypeBuilder{Action}"/>.
    /// </summary>
    /// <param name="builder">The builder used to configure the <see cref="Action"/> entity.</param>
    public void Configure(EntityTypeBuilder<UserAction> builder)
    {
        // Apply base metadata configuration
        builder.BaseMetaDataConfiguration("UserAction");

        // Example seed
        builder.HasData(
            new UserAction
            {
                RowId = Guid.Parse("71677AC1-F65E-4A3A-B5BA-30C670ADAF72"),
                Id = 1,
                Name = "Add",
                Description = "Permission to Add records",
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1,
                OrderBy = 1
            },
            new UserAction
            {
                RowId = Guid.Parse("E2C69446-BCE9-4649-9883-B7CF5DC49ED4"),
                Id = 2,
                Name = "View",
                Description = "Permission to View records",
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1,
                OrderBy = 2
            },
            new UserAction
            {
                RowId = Guid.Parse("3EEF0B38-5E82-48AE-8D5F-DB30512FA788"),
                Id = 3,
                Name = "Edit",
                Description = "Permission to Edit records",
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1,
                OrderBy = 2
            },
            new UserAction
            {
                RowId = Guid.Parse("6A567A5C-F8E3-4C30-B2D9-F3BDD59478E3"),
                Id = 4,
                Name = "Delete",
                Description = "Permission to Delete records",
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1,
                OrderBy = 2
            }
        );
    }
}
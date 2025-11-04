using KonaAI.Master.Repository.Common.Extensions;
using KonaAI.Master.Repository.Domain.Master.MetaData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KonaAI.Master.Repository.Configuration.Master.MetaData;

/// <summary>
/// Configures the entity mapping, relationships, and seed data for the <see cref="ModuleType"/> entity.
/// Sets up table schema, metadata properties, and initial seed records.
/// </summary>
public class ModuleTypeConfiguration : IEntityTypeConfiguration<ModuleType>
{
    /// <summary>
    /// Configures the <see cref="ModuleType"/> entity's schema, metadata property constraints, and seed data
    /// using the provided <see cref="EntityTypeBuilder{ModuleType}"/>.
    /// </summary>
    /// <param name="builder">The builder used to configure the <see cref="ModuleType"/> entity.</param>
    public void Configure(EntityTypeBuilder<ModuleType> builder)
    {
        // Apply base metadata configuration
        builder.BaseMetaDataConfiguration("ModuleType");

        // Seed data
        builder.HasData(
            new ModuleType
            {
                RowId = Guid.Parse("3FD968C2-363E-46DD-B466-B526F7659605"),
                Id = 1,
                Name = "T&E",
                Description = "Travel & Expense",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                IsDefault = false,
                OrderBy = 1
            },
            new ModuleType
            {
                RowId = Guid.Parse("D0CF8A02-7F09-41E2-A19B-55116FAD0E62"),
                Id = 2,
                Name = "P2P",
                Description = "Procure to pay",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                IsDefault = false,
                OrderBy = 2
            },
            new ModuleType
            {
                RowId = Guid.Parse("36F22E59-ED4E-4C4E-A174-6E957CD98C26"),
                Id = 3,
                Name = "O2C",
                Description = "Order to cash",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                IsDefault = false,
                OrderBy = 3
            }
        );
    }
}
using KonaAI.Master.Repository.Common.Extensions;
using KonaAI.Master.Repository.Domain.Master.MetaData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KonaAI.Master.Repository.Configuration.Master.MetaData;

/// <summary>
/// Configures the <see cref="RoleType"/> entity including base metadata mapping and seed data.
/// </summary>
public class RoleTypeConfiguration : IEntityTypeConfiguration<RoleType>
{
    /// <summary>
    /// Applies configuration for the <see cref="RoleType"/> entity.
    /// Sets common metadata (via <see cref="EntityBaseConfigurationExtension.BaseMetaDataConfiguration"/>)
    /// and seeds initial role types.
    /// </summary>
    /// <param name="builder">The builder used to configure the <see cref="RoleType"/> entity.</param>
    public void Configure(EntityTypeBuilder<RoleType> builder)
    {
        // Apply shared metadata configuration (keys, audit, soft delete, etc.)
        builder.BaseMetaDataConfiguration("RoleType");

        // Seed initial role type data.
        builder.HasData(
            new RoleType
            {
                RowId = Guid.Parse("84922225-D6F4-4785-87C8-7941FD8664DF"),
                Id = 1,
                Name = "Guest",
                IsSystemRole = true,
                Description = "Guest",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 1
            },
            new RoleType
            {
                RowId = Guid.Parse("FF64C9EA-605C-490B-985E-38475E461CA5"),
                Id = 2,
                Name = "Instance Admin",
                IsSystemRole = true,
                Description = "Instance Admin",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 2
            },
            new RoleType
            {
                RowId = Guid.Parse("4D697CFB-6A12-45B8-BC16-F58385505E1E"),
                Id = 3,
                Name = "Client Admin",
                IsSystemRole = true,
                Description = "Client Admin",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 3
            },
            new RoleType
            {
                RowId = Guid.Parse("8F12B879-475D-4B03-97B1-7F9097374B61"),
                Id = 4,
                Name = "Data Manager",
                IsSystemRole = true,
                Description = "Data Manager",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 4
            },
            new RoleType
            {
                RowId = Guid.Parse("C1E124DA-93E9-4F0A-9424-C308388B2D17"),
                Id = 5,
                Name = "Investigator",
                IsSystemRole = true,
                Description = "Investigator",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 5
            },
            new RoleType
            {
                RowId = Guid.Parse("EA3825CC-EFF4-4B0A-B6F1-F5361F51FAB4"),
                Id = 6,
                IsSystemRole = false,
                Name = "L1 User",
                Description = "L1 User",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 6
            },
            new RoleType
            {
                RowId = Guid.Parse("A3A08CD4-57BF-4B3C-87FE-0A96EA8959B6"),
                Id = 7,
                IsSystemRole = false,
                Name = "L2 User",
                Description = "L2 User",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 7
            }
        );
    }
}
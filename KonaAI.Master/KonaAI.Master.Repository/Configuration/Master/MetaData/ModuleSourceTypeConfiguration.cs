using KonaAI.Master.Repository.Common.Extensions;
using KonaAI.Master.Repository.Domain.Master.MetaData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KonaAI.Master.Repository.Configuration.Master.MetaData;

/// <summary>
/// Configures the <see cref="ModuleSourceType"/> entity, applying common metadata conventions
/// and seeding initial combinations of module types and source system types.
/// </summary>
public class ModuleSourceTypeConfiguration : IEntityTypeConfiguration<ModuleSourceType>
{
    /// <summary>
    /// Applies configuration to the <see cref="ModuleSourceType"/> entity:
    /// sets up base metadata (keys, audit fields, filters) via the shared extension and seeds initial data.
    /// </summary>
    /// <param name="builder">The entity type builder used to configure the model.</param>
    public void Configure(EntityTypeBuilder<ModuleSourceType> builder)
    {
        // Apply standard metadata configuration (table name, key, audit columns, etc.).
        builder.BaseMetaDataConfiguration("ModuleSourceType");

        // Seed initial data: Each record links a functional module type (ModuleTypeId)
        // with a source system type (SourcTypeId) using a composite semantic (Name).
        builder.HasData(
            new ModuleSourceType
            {
                RowId = Guid.Parse("BE65C270-A388-4791-87E7-10FD1F978CA5"),
                Id = 1,
                ModuleTypeId = 1,   // T&E
                SourcTypeId = 4,    // Concur
                Name = "T&E-Concur",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                IsDefault = false,
                OrderBy = 1
            },
            new ModuleSourceType
            {
                RowId = Guid.Parse("6597784B-DC95-4808-BA36-32D10A37694F"),
                Id = 2,
                ModuleTypeId = 1,
                SourcTypeId = 5,    // iExpense
                Name = "T&E-iExpense",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                IsDefault = false,
                OrderBy = 2
            },
            new ModuleSourceType
            {
                RowId = Guid.Parse("8A1B0A00-FE59-4573-8A7E-461C4AF632FD"),
                Id = 3,
                ModuleTypeId = 1,
                SourcTypeId = 3,    // Generic
                Name = "T&E-Generic",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                IsDefault = false,
                OrderBy = 3
            },
            new ModuleSourceType
            {
                RowId = Guid.Parse("0258BE47-BE84-49AC-847E-73FCCC3B5A36"),
                Id = 4,
                ModuleTypeId = 2,   // P2P
                SourcTypeId = 1,    // SAP
                Name = "P2P-SAP",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                IsDefault = false,
                OrderBy = 1
            },
            new ModuleSourceType
            {
                RowId = Guid.Parse("902CA202-4575-45FE-AF06-98637B900582"),
                Id = 5,
                ModuleTypeId = 2,
                SourcTypeId = 2,    // Oracle
                Name = "P2P-Orcale",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                IsDefault = false,
                OrderBy = 2
            },
            new ModuleSourceType
            {
                RowId = Guid.Parse("E74AE4CD-138A-455B-BBFE-5655FE1EA766"),
                Id = 6,
                ModuleTypeId = 2,
                SourcTypeId = 3,    // Generic
                Name = "P2P-Generic",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                IsDefault = false,
                OrderBy = 3
            },
            new ModuleSourceType
            {
                RowId = Guid.Parse("1812764B-3860-4AA8-9D08-976DCB42315A"),
                Id = 7,
                ModuleTypeId = 3,   // O2C
                SourcTypeId = 1,
                Name = "O2C-SAP",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                IsDefault = false,
                OrderBy = 1
            },
            new ModuleSourceType
            {
                RowId = Guid.Parse("69850EF5-DA73-4816-AB0C-9D52AB0221D5"),
                Id = 8,
                ModuleTypeId = 3,
                SourcTypeId = 2,
                Name = "O2C-Orcale",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                IsDefault = false,
                OrderBy = 2
            },
            new ModuleSourceType
            {
                RowId = Guid.Parse("3A0F6215-6F27-4D56-9B2A-4D7AFC0BE476"),
                Id = 9,
                ModuleTypeId = 3,
                SourcTypeId = 3,
                Name = "O2C-Generic",
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
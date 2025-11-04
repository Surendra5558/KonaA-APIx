using KonaAI.Master.Repository.Common.Extensions;
using KonaAI.Master.Repository.Domain.Master.MetaData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KonaAI.Master.Repository.Configuration.Master.MetaData;

/// <summary>
/// Configures the <see cref="SourceType"/> entity including base metadata mapping and seed data.
/// </summary>
public class SourceTypeConfiguration : IEntityTypeConfiguration<SourceType>
{
    /// <summary>
    /// Applies configuration for the <see cref="SourceType"/> entity:
    /// sets up common metadata (via <c>BaseMetaDataConfiguration</c>) and seeds initial source types.
    /// </summary>
    /// <param name="builder">The builder used to configure the entity.</param>
    public void Configure(EntityTypeBuilder<SourceType> builder)
    {
        // Apply shared metadata configuration (keys, audit, etc.)
        builder.BaseMetaDataConfiguration("SourceType");

        // Seed initial module source type data.
        builder.HasData(
            new SourceType
            {
                RowId = Guid.Parse("13FE1568-082B-4E4F-83F0-9E10BC816A60"),
                Id = 1,
                Name = "SAP",
                Description = "SAP Source",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                IsDefault = false,
                OrderBy = 1
            },
            new SourceType
            {
                RowId = Guid.Parse("FF4B3192-5C85-4B95-A60F-15A9D32D01A0"),
                Id = 2,
                Name = "Oracle",
                Description = "Oracle",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                IsDefault = false,
                OrderBy = 2
            },
            new SourceType
            {
                RowId = Guid.Parse("D33A52E3-87EA-46A0-9922-C9477A95B202"),
                Id = 3,
                Name = "Generic",
                Description = "Generic",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                IsDefault = false,
                OrderBy = 3
            },
            new SourceType
            {
                RowId = Guid.Parse("240EC22E-7F70-472E-B77A-807F2CCF8679"),
                Id = 4,
                Name = "Concur",
                Description = "Concur",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                IsDefault = false,
                OrderBy = 4
            },
            new SourceType
            {
                RowId = Guid.Parse("C025DCC3-898E-4D1C-B158-E0C35510C233"),
                Id = 5,
                Name = "iExpense",
                Description = "iExpense",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                IsDefault = false,
                OrderBy = 5
            }
        );
    }
}
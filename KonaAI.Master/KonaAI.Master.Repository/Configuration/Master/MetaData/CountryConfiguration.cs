using KonaAI.Master.Repository.Common.Extensions;
using KonaAI.Master.Repository.Domain.Master.MetaData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KonaAI.Master.Repository.Configuration.Master.MetaData;

/// <summary>
/// Configures the <see cref="Country"/> entity including base metadata mapping and seed data.
/// </summary>
public class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    /// <summary>
    /// Applies configuration for the <see cref="Country"/> entity.
    /// </summary>
    /// <param name="builder">The builder used to configure the <see cref="Country"/> entity.</param>
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        // Apply shared metadata configuration
        builder.BaseMetaDataConfiguration("Country");

        // CountryCode is required
        builder.Property(c => c.CountryCode)
               .HasMaxLength(5)
               .IsRequired();

        // Seed initial country codes.
        builder.HasData(
            new Country
            {
                RowId = Guid.Parse("A5B68F2A-C9C6-440C-BD6F-67D2FBA11A51"),
                Id = 1,
                Name = "United States",
                CountryCode = "US",
                Description = "United States of America",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                IsDefault = false,
                OrderBy = 1
            },
            new Country
            {
                RowId = Guid.Parse("9B51D359-FF3B-4207-8537-278DEBE5027F"),
                Id = 2,
                Name = "India",
                CountryCode = "IN",
                Description = "Republic of India",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                IsDefault = false,
                OrderBy = 2
            },
            new Country
            {
                RowId = Guid.Parse("59649F4E-BDB1-43D7-BE14-204075C7ED46"),
                Id = 3,
                Name = "United Kingdom",
                CountryCode = "UK",
                Description = "United Kingdom of Great Britain and Northern Ireland",
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
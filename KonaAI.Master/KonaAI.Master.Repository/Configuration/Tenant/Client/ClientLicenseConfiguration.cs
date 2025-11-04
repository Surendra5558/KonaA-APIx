using KonaAI.Master.Repository.Common.Constants;
using KonaAI.Master.Repository.Common.Extensions;
using KonaAI.Master.Repository.Domain.Tenant.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KonaAI.Master.Repository.Configuration.Tenant.Client;

/// <summary>
/// Configures the entity mapping and relationships for the <see cref="ClientLicense"/> entity.
/// Sets up table schema, metadata properties, and relationships with LicenseType.
/// </summary>
public class ClientLicenseConfiguration : IEntityTypeConfiguration<ClientLicense>
{
    /// <summary>
    /// Configures the <see cref="ClientLicense"/> entity's schema, metadata property constraints,
    /// and relationships using the provided <see cref="EntityTypeBuilder{LicenseInfo}"/>.
    /// </summary>
    /// <param name="builder">The builder used to configure the <see cref="ClientLicense"/> entity.</param>
    public void Configure(EntityTypeBuilder<ClientLicense> builder)
    {
        // Base config (Name, Description, OrderBy, audit fields)
        builder.BaseClientConfiguration("ClientLicense");

        // String properties with max length
        builder.Property(l => l.Name)
               .IsRequired()
               .HasMaxLength(DbColumnLength.NameEmail);

        builder.Property(l => l.Description)
               .HasMaxLength(DbColumnLength.Description);

        builder.Property(l => l.LicenseKey)
           .IsRequired()
           .HasMaxLength(DbColumnLength.MaxText);

        builder.Property(l => l.PrivateKey)
               .IsRequired()
               .HasMaxLength(DbColumnLength.MaxText);

        // Property configurations
        builder.Property(l => l.StartDate)
               .IsRequired();

        builder.Property(l => l.EndDate)
               .IsRequired();

        // Example seed data
        builder.HasData(
            new ClientLicense
            {
                RowId = Guid.Parse("EA3B7A8F-114C-40AB-952F-6C3AD7B95D99"),
                Id = 1,
                Name = "Default License",
                Description = "Trial license valid for 30 days",
                StartDate = DateTime.UtcNow.Date,
                EndDate = DateTime.UtcNow.Date.AddDays(30),
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1,
                LicenseKey = "System",
                PrivateKey = "System",
            }
        );
    }
}
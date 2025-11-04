using KonaAI.Master.Repository.Common.Extensions;
using KonaAI.Master.Repository.Domain.Tenant.ClientUserMetaData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KonaAI.Master.Repository.Configuration.Tenant.ClientUserMetaData;

/// <summary>
/// Configures the <see cref="ClientProjectUnit"/> entity including base metadata mapping and seed data.
/// </summary>
public class ClientProjectUnitConfiguration : IEntityTypeConfiguration<ClientProjectUnit>
{
    /// <summary>
    /// Applies configuration for the <see cref="ClientProjectUnit"/> entity.
    /// Sets common metadata (via EntityBaseConfigurationExtension.BaseClientMetaDataConfiguration)
    /// and seeds initial Business Unit.
    /// </summary>
    /// <param name="builder">The builder used to configure the <see cref="ClientProjectUnit"/> entity.</param>
    public void Configure(EntityTypeBuilder<ClientProjectUnit> builder)
    {
        // Apply shared metadata configuration
        builder.BaseClientMetaDataConfiguration("ClientProjectUnit", "ClientUserMetadata");

        // Seed client-specific project units
        builder.HasData(
            new ClientProjectUnit
            {
                RowId = Guid.Parse("A444FBAA-0BA8-43EA-8414-B07222CA9E17"),
                Id = 1,
                ClientId = 1,
                ProjectUnitId = 1,
                Name = "Finance",
                Description = "Handles financial planning, reporting, and accounting",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 1,
                IsActive = true,
                IsDeleted = false
            },
            new ClientProjectUnit
            {
                RowId = Guid.Parse("81C92F90-05E6-4A8F-8F72-E55A103CC02A"),
                Id = 2,
                ClientId = 1,
                ProjectUnitId = 3,
                Name = "Technology",
                Description = "Manages IT infrastructure, development, and support",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 2,
                IsActive = true,
                IsDeleted = false
            }
        );
    }
}
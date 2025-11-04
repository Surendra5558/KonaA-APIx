using KonaAI.Master.Repository.Common.Extensions;
using KonaAI.Master.Repository.Domain.Tenant.ClientUserMetaData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KonaAI.Master.Repository.Configuration.Tenant.ClientUserMetaData;

/// <summary>
/// Configures the <see cref="ClientProjectRiskArea"/> entity including base metadata mapping and seed data.
/// </summary>
public class ClientProjectRiskAreaConfiguration : IEntityTypeConfiguration<ClientProjectRiskArea>
{
    /// <summary>
    /// Applies configuration for the <see cref="ClientProjectRiskArea"/> entity.
    /// Sets common metadata (via <see cref="EntityBaseConfigurationExtension.BaseMetaDataConfiguration"/>)
    /// and seeds initial risk areas.
    /// </summary>
    /// <param name="builder">The builder used to configure the <see cref="ClientProjectRiskArea"/> entity.</param>
    public void Configure(EntityTypeBuilder<ClientProjectRiskArea> builder)
    {
        // Apply shared metadata configuration (keys, audit, soft delete, etc.)
        builder.BaseClientMetaDataConfiguration("ClientProjectRiskArea", "ClientUserMetaData");

        // Seed client-specific risk areas
        builder.HasData(
            new ClientProjectRiskArea
            {
                RowId = Guid.Parse("6C6E67A5-27FA-4F31-9BB3-D73887244A2F"),
                Id = 1,
                ClientId = 1,
                ProjectRiskAreaId = 1,
                Name = "Financial Risk",
                Description = "Covers risks related to financial stability, fraud, and mismanagement",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 1,
                IsActive = true,
                IsDeleted = false
            },
            new ClientProjectRiskArea
            {
                RowId = Guid.Parse("5EC53CEA-1219-42C2-88ED-63CC6068C7D6"),
                Id = 2,
                ClientId = 1,
                ProjectRiskAreaId = 2,
                Name = "Operational Risk",
                Description = "Covers risks in day-to-day operations and business continuity",
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
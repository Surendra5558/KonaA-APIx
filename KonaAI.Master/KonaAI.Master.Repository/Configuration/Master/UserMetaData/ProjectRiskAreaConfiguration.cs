using KonaAI.Master.Repository.Common.Extensions;
using KonaAI.Master.Repository.Domain.Master.UserMetaData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KonaAI.Master.Repository.Configuration.Master.UserMetaData;

/// <summary>
/// Configures the <see cref="ProjectRiskArea"/> entity including base metadata mapping and seed data.
/// </summary>
public class ProjectRiskAreaConfiguration : IEntityTypeConfiguration<ProjectRiskArea>
{
    /// <summary>
    /// Applies configuration for the <see cref="ProjectRiskArea"/> entity.
    /// Sets common metadata (via <see cref="EntityBaseConfigurationExtension.BaseMetaDataConfiguration"/>)
    /// and seeds initial risk areas.
    /// </summary>
    /// <param name="builder">The builder used to configure the <see cref="ProjectRiskArea"/> entity.</param>
    public void Configure(EntityTypeBuilder<ProjectRiskArea> builder)
    {
        // Apply shared metadata configuration (keys, audit, soft delete, etc.)
        builder.BaseMetaDataConfiguration("ProjectRiskArea", "UserMetaData");

        // Seed initial risk area data.
        builder.HasData(
         new ProjectRiskArea
         {
             RowId = Guid.Parse("09F47B5C-3809-4090-AE88-D3B1279EEE1D"),
             Id = 1,
             Name = "Fraud 360",
             Description = "Covers risks related to fraud detection, prevention, and response across all functions",
             CreatedBy = "Default User",
             CreatedById = 1,
             ModifiedBy = "Default User",
             ModifiedById = 1,
             OrderBy = 1
         },
            new ProjectRiskArea
            {
                RowId = Guid.Parse("D3280CDA-F0FF-4C61-A5CF-392B55F4EBFB"),
                Id = 2,
                Name = "Corruption 360",
                Description = "Covers risks related to bribery, corruption, and unethical practices across the organization",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 2
            },
            new ProjectRiskArea
            {
                RowId = Guid.Parse("DF206068-DB80-4D15-B0F7-0F7505042C32"),
                Id = 3,
                Name = "Export Controls",
                Description = "Covers risks related to export regulations, trade compliance, and cross-border operations",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 3
            },
            new ProjectRiskArea
            {
                RowId = Guid.Parse("8D1108E5-2B2F-4FFC-9755-A4E87F62F2E5"),
                Id = 4,
                Name = "Conflict Of Interest",
                Description = "Covers risks arising from personal or organizational conflicts that may affect decision-making",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 4
            },
            new ProjectRiskArea
            {
                RowId = Guid.Parse("FB489EF8-43B8-47F3-B04A-39AFDC792AEA"),
                Id = 5,
                Name = "Vendor 360",
                Description = "Covers risks associated with vendor selection, engagement, performance, and monitoring",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 5
            },
            new ProjectRiskArea
            {
                RowId = Guid.Parse("B8CB71E7-D36A-4CD2-A372-E1D92CB35C85"),
                Id = 6,
                Name = "Distributor 360",
                Description = "Covers risks related to distributor relationships, compliance, and performance oversight",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 6
            },
            new ProjectRiskArea
            {
                RowId = Guid.Parse("C898C04C-D11C-43FF-A1E4-F0D8449AACE6"),
                Id = 7,
                Name = "Employee 360",
                Description = "Covers risks related to employee behavior, ethics, compliance, and internal misconduct",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 7
            }

        );
    }
}
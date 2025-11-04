using KonaAI.Master.Repository.Common.Extensions;
using KonaAI.Master.Repository.Domain.Master.UserMetaData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KonaAI.Master.Repository.Configuration.Master.UserMetaData;

/// <summary>
/// Configures the <see cref="ProjectAuditResponsibility"/> entity including base metadata mapping and seed data.
/// </summary>
public class ProjectAuditResponsibilityConfiguration : IEntityTypeConfiguration<ProjectAuditResponsibility>
{
    /// <summary>
    /// Applies configuration for the ProjectAuditResponsibility entity.
    /// </summary>
    /// <param name="builder">The builder used to configure the <see cref="ProjectAuditResponsibility"/> entity.</param>
    public void Configure(EntityTypeBuilder<ProjectAuditResponsibility> builder)
    {
        // Apply shared metadata configuration (keys, audit, soft delete, etc.)
        builder.BaseMetaDataConfiguration("ProjectAuditResponsibility", "UserMetaData");

        // Seed initial audit responsibility types.
        builder.HasData(
            new ProjectAuditResponsibility
            {
                RowId = Guid.Parse("33A20F2A-3CB4-4BB9-ADC5-28D6C67B343A"),
                Id = 1,
                Name = "Compliance",
                Description = "Responsible for ensuring adherence to laws, regulations, and internal policies",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 1
            },
            new ProjectAuditResponsibility
            {
                RowId = Guid.Parse("C8085D8B-6CA2-4361-A1CD-836EBC7F8315"),
                Id = 2,
                Name = "Internal Audit",
                Description = "Responsible for independently reviewing and evaluating internal controls and processes",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 2
            },
            new ProjectAuditResponsibility
            {
                RowId = Guid.Parse("C9DFDAF4-773F-447F-910F-5C00C37D7F11"),
                Id = 3,
                Name = "Legal",
                Description = "Responsible for providing legal oversight and ensuring regulatory compliance",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 3
            },
            new ProjectAuditResponsibility
            {
                RowId = Guid.Parse("14869163-870B-4C7C-9BFB-CCD7C753176F"),
                Id = 4,
                Name = "Investigations",
                Description = "Responsible for conducting investigations into potential violations and misconduct",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 4
            }

        );
    }
}
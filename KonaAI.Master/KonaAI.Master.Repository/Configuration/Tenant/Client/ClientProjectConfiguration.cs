using KonaAI.Master.Repository.Common.Constants;
using KonaAI.Master.Repository.Common.Extensions;
using KonaAI.Master.Repository.Domain.Tenant.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KonaAI.Master.Repository.Configuration.Tenant.Client;

/// <summary>
/// Configures the <see cref="ClientProject"/> entity.
/// </summary>
public class ClientProjectConfiguration : IEntityTypeConfiguration<ClientProject>
{
    public void Configure(EntityTypeBuilder<ClientProject> builder)
    {
        builder.BaseClientConfiguration("ClientProject");

        builder.Property(x => x.Name)
               .IsRequired()
               .HasMaxLength(DbColumnLength.NameEmail);

        builder.Property(x => x.Description)
               .HasMaxLength(DbColumnLength.Description);

        builder.Property(x => x.ProjectAuditResponsibilityId).IsRequired();

        builder.Property(x => x.ProjectRiskAreaId).IsRequired();

        builder.Property(x => x.StartDate)
               .IsRequired()
               .HasColumnType("date");

        builder.Property(x => x.EndDate)
               .HasColumnType("date");

        // Do not configure the ClientProject -> ClientProjectModuleSourceTypes relationship here
        // to avoid duplicating it (and creating a shadow FK). It is configured on the dependent.

        builder.HasIndex(x => new { x.ClientId, x.Name }).IsUnique();

        // Seed initial client projects
        builder.HasData(
            new ClientProject
            {
                RowId = Guid.Parse("72A6B068-2AC2-4E97-9EBA-AD9C4B15418B"),
                Id = 1,
                ClientId = 1,
                Name = "Financial Risk Assessment System",
                Description = "Implementation of a comprehensive financial risk assessment and monitoring system for improved compliance and decision-making",
                ProjectAuditResponsibilityId = 3,
                ProjectRiskAreaId = 1, // Financial Risk
                ProjectCountryId = 2, // India
                ProjectUnitId = 1, // Finance
                ProjectDepartmentId = 1, // Accounts Payable
                ProjectStatusId = 2, // In Progress
                StartDate = new DateTime(2024, 1, 15),
                EndDate = new DateTime(2024, 12, 31),
                Modules = "T&E",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                CreatedOn = DateTime.UtcNow,
                ModifiedOn = DateTime.UtcNow,
                IsActive = true,
                IsDeleted = false
            },
            new ClientProject
            {
                RowId = Guid.Parse("79C8B912-5714-4035-A1DB-58BD9AE6C108"),
                Id = 2,
                ClientId = 1, // References the seeded DCube LLP client
                Name = "IT Infrastructure Modernization",
                Description = "Modernization of existing IT infrastructure including cloud migration, security enhancements, and system optimization",
                ProjectAuditResponsibilityId = 2, // Verification
                ProjectRiskAreaId = 2, // Operational Risk
                ProjectCountryId = 2, // India
                ProjectUnitId = 3, // Technology
                ProjectDepartmentId = 3, // IT Support
                ProjectStatusId = 2, // In Progress
                StartDate = new DateTime(2024, 3, 1),
                EndDate = new DateTime(2025, 2, 28),
                Modules = "P2P",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                CreatedOn = DateTime.UtcNow,
                ModifiedOn = DateTime.UtcNow,
                IsActive = true,
                IsDeleted = false
            }
        );
    }
}
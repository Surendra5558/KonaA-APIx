using KonaAI.Master.Repository.Common.Extensions;
using KonaAI.Master.Repository.Domain.Master.UserMetaData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KonaAI.Master.Repository.Configuration.Master.UserMetaData;

/// <summary>
/// Configures the <see cref="ProjectDepartment"/> entity including base metadata mapping and seed data.
/// </summary>
public class ProjectDepartmentConfiguration : IEntityTypeConfiguration<ProjectDepartment>
{
    /// <summary>
    /// Applies configuration for the <see cref="ProjectDepartment"/> entity.
    /// Sets common metadata (via <see cref="EntityBaseConfigurationExtension.BaseMetaDataConfiguration"/>)
    /// and seeds initial Business Department.
    /// </summary>
    /// <param name="builder">The builder used to configure the <see cref="ProjectDepartment"/> entity.</param>
    public void Configure(EntityTypeBuilder<ProjectDepartment> builder)
    {
        // Apply shared metadata configuration
        builder.BaseMetaDataConfiguration("ProjectDepartment", "UserMetaData");

        // Seed initial business departments
        builder.HasData(
            new ProjectDepartment
            {
                RowId = Guid.Parse("05A173E6-732F-4E13-9243-DEC8902BD82B"),
                Id = 1,
                Name = "Accounts Payable",
                Description = "Manages vendor payments and invoices",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 1
            },
            new ProjectDepartment
            {
                RowId = Guid.Parse("5E2F6C82-0653-4B56-BCB6-6478D54DE027"),
                Id = 2,
                Name = "Accounts Receivable",
                Description = "Handles customer billing and collections",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 2
            },
            new ProjectDepartment
            {
                RowId = Guid.Parse("338C3984-CF0A-407C-AE34-B895EEB589F8"),
                Id = 3,
                Name = "IT Support",
                Description = "Provides technical support and maintenance",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 3
            },
            new ProjectDepartment
            {
                RowId = Guid.Parse("4B219523-2B9F-47EB-BBBF-EAD9BCA903DE"),
                Id = 4,
                Name = "Recruitment",
                Description = "Handles hiring and onboarding of employees",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 4
            }
        );
    }
}
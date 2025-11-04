using KonaAI.Master.Repository.Common.Extensions;
using KonaAI.Master.Repository.Domain.Master.UserMetaData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KonaAI.Master.Repository.Configuration.Master.UserMetaData;

/// <summary>
/// Configures the <see cref="ProjectUnit"/> entity including base metadata mapping and seed data.
/// </summary>
public class ProjectUnitConfiguration : IEntityTypeConfiguration<ProjectUnit>
{
    /// <summary>
    /// Applies configuration for the <see cref="ProjectUnit"/> entity.
    /// Sets common metadata (via <see cref="EntityBaseConfigurationExtension.BaseMetaDataConfiguration"/>)
    /// and seeds initial Business Unit.
    /// </summary>
    /// <param name="builder">The builder used to configure the <see cref="ProjectUnit"/> entity.</param>
    public void Configure(EntityTypeBuilder<ProjectUnit> builder)
    {
        // Apply shared metadata configuration
        builder.BaseMetaDataConfiguration("ProjectUnit", "UserMetaData");

        // Seed initial business units
        builder.HasData(
            new ProjectUnit
            {
                RowId = Guid.Parse("05445216-8923-4603-B4D2-73D63764BE40"),
                Id = 1,
                Name = "Finance",
                Description = "Handles financial planning, reporting, and accounting",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 1
            },
            new ProjectUnit
            {
                RowId = Guid.Parse("F86E92AC-F361-4D40-8B82-CD81F8DB7B49"),
                Id = 2,
                Name = "Operations",
                Description = "Responsible for day-to-day business operations",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 2
            },
            new ProjectUnit
            {
                RowId = Guid.Parse("8DF282E2-857C-44E4-A73B-6EA8AADF0CE6"),
                Id = 3,
                Name = "Technology",
                Description = "Manages IT infrastructure, development, and support",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 3
            },
            new ProjectUnit
            {
                RowId = Guid.Parse("194CBCDA-F76E-40A7-A3BB-45E684ABB4F0"),
                Id = 4,
                Name = "HR",
                Description = "Human resources and talent management",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 4
            }
        );
    }
}
using KonaAI.Master.Repository.Common.Extensions;
using KonaAI.Master.Repository.Domain.Master.MetaData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KonaAI.Master.Repository.Configuration.Master.MetaData;

/// <summary>
/// Configures the entity mapping, relationships, and seed data for the <see cref="Navigation"/> entity.
/// Sets up table schema, metadata properties, and initial seed records.
/// </summary>
public class NavigationConfiguration : IEntityTypeConfiguration<Navigation>
{
    /// <summary>
    /// Configures the <see cref="Navigation"/> entity's schema, metadata property constraints, and seed data
    /// using the provided <see cref="EntityTypeBuilder{AppNavigation}"/>.
    /// </summary>
    /// <param name="builder">The builder used to configure the <see cref="Navigation"/> entity.</param>
    public void Configure(EntityTypeBuilder<Navigation> builder)
    {
        // Apply base metadata configuration
        builder.BaseMetaDataConfiguration("Navigation");

        // Seed data
        builder.HasData(
          new Navigation
          {
              RowId = Guid.Parse("2B7D80F8-9163-4CF6-B03B-9132C49E1B34"),
              Id = 1,
              Name = "Oraganization",
              Description = "Organization contains the user navigation actions according to the user roles.",
              CreatedBy = "Default User",
              CreatedById = 1,
              ModifiedBy = "Default User",
              ModifiedById = 1,
              OrderBy = 1,
              ParentId = null,
              IsTopMenu = false
          },
          new Navigation
          {
              RowId = Guid.Parse("B8E3D6CB-B6F0-4023-8E7B-4BCEA6894C03"),
              Id = 2,
              Name = "Project",
              Description = "Views project list and its details.",
              CreatedBy = "Default User",
              CreatedById = 1,
              ModifiedBy = "Default User",
              ModifiedById = 1,
              OrderBy = 2,
              ParentId = null,
              IsTopMenu = false
          },
          new Navigation
          {
              RowId = Guid.Parse("7FFAA26E-0E7E-41F4-8057-2CF4D1951FED"),
              Id = 3,
              Name = "All Clients",
              Description = "Shows the client details.",
              CreatedBy = "Default User",
              CreatedById = 1,
              ModifiedBy = "Default User",
              ModifiedById = 1,
              OrderBy = 3,
              ParentId = 1,
              IsTopMenu = true
          },
          new Navigation
          {
              RowId = Guid.Parse("9EC0D091-74F6-44FA-AAAE-27D303A71CF6"),
              Id = 4,
              Name = "Summary",
              Description = "Shows the client summary and its details.",
              CreatedBy = "Default User",
              CreatedById = 1,
              ModifiedBy = "Default User",
              ModifiedById = 1,
              OrderBy = 4,
              ParentId = 1,
              IsTopMenu = true
          },
          new Navigation
          {
              RowId = Guid.Parse("9EC25F34-B66C-4E86-A340-014F6F511990"),
              Id = 5,
              Name = "Members",
              Description = "Manage members on the platform, assign roles, and control access seamlessly.",
              CreatedBy = "Default User",
              CreatedById = 1,
              ModifiedBy = "Default User",
              ModifiedById = 1,
              OrderBy = 5,
              ParentId = 1,
              IsTopMenu = true
          },

            new Navigation
            {
                RowId = Guid.Parse("BDCF0A83-8C85-4FD0-A21F-15BCC28DE782"),
                Id = 6,
                Name = "Roles",
                Description = "Create new roles and manage permissions to control access across the platform.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 6,
                ParentId = 1,
                IsTopMenu = true
            },
            new Navigation
            {
                RowId = Guid.Parse("1D1C8349-B551-4A34-8468-E80015425D53"),
                Id = 7,
                Name = "Connectors",
                Description = "Manage and monitor your data connections across systems to enable seamless data flow.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 7,
                ParentId = 1,
                IsTopMenu = true
            },
            new Navigation
            {
                RowId = Guid.Parse("D17E90F1-D83D-479E-9A98-A0DA491924BA"),
                Id = 8,
                Name = "Configurations",
                Description = "Manage modules, risk areas, and source system mappings.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 8,
                ParentId = 1,
                IsTopMenu = true
            },
            new Navigation
            {
                RowId = Guid.Parse("BC22B3F0-C8FA-4917-A743-9CA61A32410C"),
                Id = 9,
                Name = "Connections",
                Description = "Manage and monitor your data connections across systems to enable seamless data flow.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 9,
                ParentId = 1,
                IsTopMenu = true
            },
            new Navigation
            {
                RowId = Guid.Parse("CF8324F3-D559-470F-828A-EC391B593BCF"),
                Id = 10,
                Name = "Documents",
                Description = "Upload and manage all key project documents in one place.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 10,
                ParentId = 1,
                IsTopMenu = true
            },
            new Navigation
            {
                RowId = Guid.Parse("E7E8B5F4-B732-415C-A8FF-CDC116143B96"),
                Id = 11,
                Name = "Questionnaire Builder",
                Description = "Add and customise questionnaires for investigations and assessments.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 11,
                ParentId = 1,
                IsTopMenu = true
            },
            new Navigation
            {
                RowId = Guid.Parse("E8DCF843-425F-4959-9EA6-1DBF63AF5731"),
                Id = 12,
                Name = "Insights Template",
                Description = "Choose and customize insight templates for your analysis.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 12,
                ParentId = 1,
                IsTopMenu = true
            },
            new Navigation
            {
                RowId = Guid.Parse("97C2BFE9-0C66-48C8-9591-1769DCC7A068"),
                Id = 13,
                Name = "Users",
                Description = "Manage members on the platform, assign roles, and control access seamlessly.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 13,
                ParentId = 1,
                IsTopMenu = true
            },
            new Navigation
            {
                RowId = Guid.Parse("FE0B6D72-D3D7-4A46-B8E9-F3B187463CF4"),
                Id = 14,
                Name = "License",
                Description = "Views the license of the client.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 14,
                ParentId = 1,
                IsTopMenu = true
            },
            new Navigation
            {
                RowId = Guid.Parse("1C4F654B-E3E7-43DB-A0B0-7D6731B933BD"),
                Id = 15,
                Name = "All Projects",
                Description = "Manage and track all the projects created across organisation seamlessly.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 15,
                ParentId = 2,
                IsTopMenu = true
            },
            new Navigation
            {
                RowId = Guid.Parse("AFF447AB-3180-4850-8675-4498A2B8B1E2"),
                Id = 16,
                Name = "Summary",
                Description = "Project Summary.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 16,
                ParentId = 2,
                IsTopMenu = true
            },
            new Navigation
            {
                RowId = Guid.Parse("5D534CA5-E26F-45D0-A8B9-1CFDA53ABCC0"),
                Id = 17,
                Name = "Workflow Set Up",
                Description = "Set criteria and adjust parameters to configure your test for accurate results.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 17,
                ParentId = 2,
                IsTopMenu = true
            },
            new Navigation
            {
                RowId = Guid.Parse("6BDDB546-BFCB-4217-9F5C-BC535EF233DD"),
                Id = 18,
                Name = "Insights",
                Description = "View test results,anomalies, and analyse data trends for deeper insights.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 18,
                ParentId = 2,
                IsTopMenu = true
            },
            new Navigation
            {
                RowId = Guid.Parse("0D2E6D9B-CA51-410C-9775-EA3583872984"),
                Id = 19,
                Name = "Alerts Dashboard",
                Description = "Manage and track all the projects created across organisation seamlessly.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 19,
                ParentId = 2,
                IsTopMenu = true
            },
            new Navigation
            {
                RowId = Guid.Parse("A5147898-183E-4EC7-A0A1-D97232EFEE73"),
                Id = 20,
                Name = "Alerts",
                Description = "Track and manage alerts triggered by failed tests, anomalies, and policy violations.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 20,
                ParentId = 2,
                IsTopMenu = true
            },
            new Navigation
            {
                RowId = Guid.Parse("0DBC12A1-7FBF-442C-8583-9F491C8B802E"),
                Id = 21,
                Name = "Users",
                Description = "Manage members on the platform, assign roles, and control access seamlessly.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 21,
                ParentId = 2,
                IsTopMenu = true
            },
            new Navigation
            {
                RowId = Guid.Parse("9A022CD0-82BD-4855-B25A-E72523D53D3F"),
                Id = 22,
                Name = "Roles",
                Description = "Create new roles and manage permissions to control access across the platform.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 22,
                ParentId = 2,
                IsTopMenu = true
            },
           new Navigation
           {
               RowId = Guid.Parse("A814B4CE-BF05-4B13-A728-90AB9A4A0926"),
               Id = 23,
               Name = "Visualisations",
               Description = "View test results,anomalies, and analyse data trends for deeper insights.",
               CreatedBy = "Default User",
               CreatedById = 1,
               ModifiedBy = "Default User",
               ModifiedById = 1,
               OrderBy = 23,
               ParentId = 18,
               IsTopMenu = false
           },
            new Navigation
            {
                RowId = Guid.Parse("5B30432E-3307-46E9-AFDE-0E16872624BF"),
                Id = 24,
                Name = "Entity View",
                Description = "Shows entity profiles.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 24,
                ParentId = 18,
                IsTopMenu = false
            },
            new Navigation
            {
                RowId = Guid.Parse("7487E683-7781-456E-92D0-5E6D7B38F57D"),
                Id = 25,
                Name = "Transaction View",
                Description = "Shows transactions and invoice details.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 25,
                ParentId = 18,
                IsTopMenu = false
            },
            new Navigation
            {
                RowId = Guid.Parse("32873795-27F4-4FE3-969E-C852BA7D19F4"),
                Id = 26,
                Name = "Scenario Manager",
                Description = "Views scenario information.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 26,
                ParentId = 18,
                IsTopMenu = false
            },
            new Navigation
            {
                RowId = Guid.Parse("5CDE8207-3591-4311-A131-642801FF138E"),
                Id = 27,
                Name = "Similar Transaction",
                Description = "Access interactive charts and visual analytics to explore project, client, and transaction data.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 27,
                ParentId = 18,
                IsTopMenu = false
            },
            new Navigation
            {
                RowId = Guid.Parse("11AD33E4-F72D-4A1D-A468-B252D6864498"),
                Id = 28,
                Name = "Project Dashboard",
                Description = "Views dashbord for project audits.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 28,
                ParentId = 2,
                IsTopMenu = true
            }
        );
    }
}
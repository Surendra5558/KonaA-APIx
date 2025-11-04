using KonaAI.Master.Repository.Common.Extensions;
using KonaAI.Master.Repository.Domain.Master.MetaData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KonaAI.Master.Repository.Configuration.Master.MetaData;

/// <summary>
/// Configures the entity mapping, relationships, and seed data for the <see cref="RoleNavigationUserAction"/> entity.
/// Sets up table schema, metadata properties, and initial seed records.
/// </summary>
public class RoleNavigationUserActionConfiguration : IEntityTypeConfiguration<RoleNavigationUserAction>
{
    /// <summary>
    /// Configures the <see cref="RoleNavigationUserAction"/> entity's schema, metadata property constraints, and seed data
    /// using the provided <see cref="EntityTypeBuilder{AppRoleNavigationUserAction}"/>.
    /// </summary>
    /// <param name="builder">The builder used to configure the <see cref="RoleNavigationUserAction"/> entity.</param>
    public void Configure(EntityTypeBuilder<RoleNavigationUserAction> builder)
    {
        // Apply base metadata configuration
        builder.BaseMetaDataConfiguration("RoleNavigationUserAction");

        // Seed data
        builder.HasData(
            // ========================================
            // Instance Admin (RoleTypeId = 2)
            // ========================================

            // Organization Navigation (1-4)
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("A1F2B3C4-D5E6-4F7A-9B8C-1D2E3F4A5B6C"),
                Id = 1,
                Name = "Instance Admin - Add Organization",
                Description = "Allows Instance Admin to add organization records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 1,
                NavigationUserActionId = 1,
                RoleTypeId = 2,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("B2A3B4C5-D6E7-5A8B-AC9D-2E3F4A5B6C7D"),
                Id = 2,
                Name = "Instance Admin - View Organization",
                Description = "Allows Instance Admin to view organization records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 2,
                NavigationUserActionId = 2,
                RoleTypeId = 2,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("C3B4C5D6-E7F8-6B9C-BD0E-3F4A5B6C7D8E"),
                Id = 3,
                Name = "Instance Admin - Edit Organization",
                Description = "Allows Instance Admin to edit organization records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 3,
                NavigationUserActionId = 3,
                RoleTypeId = 2,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("D4C5D6E7-F8A9-7C0D-CE1F-4A5B6C7D8E9F"),
                Id = 4,
                Name = "Instance Admin - Delete Organization",
                Description = "Allows Instance Admin to delete organization records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 4,
                NavigationUserActionId = 4,
                RoleTypeId = 2,
                IsVisible = true,
                IsAccessible = true
            },

            // Project Navigation (5-8)
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("E5D6E7F8-A9B0-8D1E-DF2A-5B6C7D8E9F0A"),
                Id = 5,
                Name = "Instance Admin - Add Project",
                Description = "Allows Instance Admin to add project records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 5,
                NavigationUserActionId = 5,
                RoleTypeId = 2,
                IsVisible = true,
                IsAccessible = false
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("F6E7F8A9-B0C1-9E2F-EA3B-6C7D8E9F0A1B"),
                Id = 6,
                Name = "Instance Admin - View Project",
                Description = "Allows Instance Admin to view project records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 6,
                NavigationUserActionId = 6,
                RoleTypeId = 2,
                IsVisible = true,
                IsAccessible = false
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("A7F8A9B0-C1D2-0F3A-FB4C-7D8E9F0A1B2C"),
                Id = 7,
                Name = "Instance Admin - Edit Project",
                Description = "Allows Instance Admin to edit project records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 7,
                NavigationUserActionId = 7,
                RoleTypeId = 2,
                IsVisible = true,
                IsAccessible = false
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("B8A9B0C1-D2E3-1A4B-AC5D-8E9F0A1B2C3D"),
                Id = 8,
                Name = "Instance Admin - Delete Project",
                Description = "Allows Instance Admin to delete project records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 8,
                NavigationUserActionId = 8,
                RoleTypeId = 2,
                IsVisible = true,
                IsAccessible = false
            },

            // All Clients Navigation (9-12)
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("C9B0C1D2-E3F4-2B5C-BD6E-9F0A1B2C3D4E"),
                Id = 9,
                Name = "Instance Admin - Add All Clients",
                Description = "Allows Instance Admin to add client records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 9,
                NavigationUserActionId = 9,
                RoleTypeId = 2,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("D0C1D2E3-F4A5-3C6D-CE7F-0A1B2C3D4E5F"),
                Id = 10,
                Name = "Instance Admin - View All Clients",
                Description = "Allows Instance Admin to view client records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 10,
                NavigationUserActionId = 10,
                RoleTypeId = 2,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("E1D2E3F4-A5B6-4D7E-DF8A-1B2C3D4E5F6A"),
                Id = 11,
                Name = "Instance Admin - Edit All Clients",
                Description = "Allows Instance Admin to edit client records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 11,
                NavigationUserActionId = 11,
                RoleTypeId = 2,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("F2E3F4A5-B6C7-5E8F-EA9B-2C3D4E5F6A7B"),
                Id = 12,
                Name = "Instance Admin - Delete All Clients",
                Description = "Allows Instance Admin to delete client records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 12,
                NavigationUserActionId = 12,
                RoleTypeId = 2,
                IsVisible = true,
                IsAccessible = true
            },

            // Summary Navigation (13-16)
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("A3F4A5B6-C7D8-6F9A-FB0C-3D4E5F6A7B8C"),
                Id = 13,
                Name = "Instance Admin - Add Summary",
                Description = "Allows Instance Admin to add summary records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 13,
                NavigationUserActionId = 13,
                RoleTypeId = 2,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("B4A5B6C7-D8E9-7A0B-AC1D-4E5F6A7B8C9D"),
                Id = 14,
                Name = "Instance Admin - View Summary",
                Description = "Allows Instance Admin to view summary records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 14,
                NavigationUserActionId = 14,
                RoleTypeId = 2,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("C5B6C7D8-E9F0-8B1C-BD2E-5F6A7B8C9D0E"),
                Id = 15,
                Name = "Instance Admin - Edit Summary",
                Description = "Allows Instance Admin to edit summary records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 15,
                NavigationUserActionId = 15,
                RoleTypeId = 2,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("D6C7D8E9-F0A1-9C2D-CE3F-6A7B8C9D0E1F"),
                Id = 16,
                Name = "Instance Admin - Delete Summary",
                Description = "Allows Instance Admin to delete summary records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 16,
                NavigationUserActionId = 16,
                RoleTypeId = 2,
                IsVisible = true,
                IsAccessible = true
            },

            // Members Navigation (17-20)
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("E7D8E9F0-A1B2-0D3E-DF4A-7B8C9D0E1F2A"),
                Id = 17,
                Name = "Instance Admin - Add Members",
                Description = "Allows Instance Admin to add member records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 17,
                NavigationUserActionId = 17,
                RoleTypeId = 2,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("F8E9F0A1-B2C3-1E4F-EA5B-8C9D0E1F2A3B"),
                Id = 18,
                Name = "Instance Admin - View Members",
                Description = "Allows Instance Admin to view member records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 18,
                NavigationUserActionId = 18,
                RoleTypeId = 2,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("A9F0A1B2-C3D4-2F5A-FB6C-9D0E1F2A3B4C"),
                Id = 19,
                Name = "Instance Admin - Edit Members",
                Description = "Allows Instance Admin to edit member records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 19,
                NavigationUserActionId = 19,
                RoleTypeId = 2,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("B0A1B2C3-D4E5-3A6B-AC7D-0E1F2A3B4C5D"),
                Id = 20,
                Name = "Instance Admin - Delete Members",
                Description = "Allows Instance Admin to delete member records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 20,
                NavigationUserActionId = 20,
                RoleTypeId = 2,
                IsVisible = true,
                IsAccessible = true
            },

            // Roles Navigation (21-24)
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("C1B2C3D4-E5F6-4B7C-BD8E-1F2A3B4C5D6E"),
                Id = 21,
                Name = "Instance Admin - Add Roles",
                Description = "Allows Instance Admin to add role records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 21,
                NavigationUserActionId = 21,
                RoleTypeId = 2,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("D2C3D4E5-F6A7-5C8D-CE9F-2A3B4C5D6E7F"),
                Id = 22,
                Name = "Instance Admin - View Roles",
                Description = "Allows Instance Admin to view role records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 22,
                NavigationUserActionId = 22,
                RoleTypeId = 2,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("E3D4E5F6-A7B8-6D9E-DF0A-3B4C5D6E7F8A"),
                Id = 23,
                Name = "Instance Admin - Edit Roles",
                Description = "Allows Instance Admin to edit role records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 23,
                NavigationUserActionId = 23,
                RoleTypeId = 2,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("F4E5F6A7-B8C9-7E0F-EA1B-4C5D6E7F8A9B"),
                Id = 24,
                Name = "Instance Admin - Delete Roles",
                Description = "Allows Instance Admin to delete role records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 24,
                NavigationUserActionId = 24,
                RoleTypeId = 2,
                IsVisible = true,
                IsAccessible = true
            },

            // Connectors Navigation (25-28)
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("A5F6A7B8-C9D0-8F1A-FB2C-5D6E7F8A9B0C"),
                Id = 25,
                Name = "Instance Admin - Add Connectors",
                Description = "Allows Instance Admin to add connector records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 25,
                NavigationUserActionId = 25,
                RoleTypeId = 2,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("B6A7B8C9-D0E1-9A2B-AC3D-6E7F8A9B0C1D"),
                Id = 26,
                Name = "Instance Admin - View Connectors",
                Description = "Allows Instance Admin to view connector records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 26,
                NavigationUserActionId = 26,
                RoleTypeId = 2,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("C7B8C9D0-E1F2-0B3C-BD4E-7F8A9B0C1D2E"),
                Id = 27,
                Name = "Instance Admin - Edit Connectors",
                Description = "Allows Instance Admin to edit connector records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 27,
                NavigationUserActionId = 27,
                RoleTypeId = 2,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("D8C9D0E1-F2A3-1C4D-CE5F-8A9B0C1D2E3F"),
                Id = 28,
                Name = "Instance Admin - Delete Connectors",
                Description = "Allows Instance Admin to delete connector records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 28,
                NavigationUserActionId = 28,
                RoleTypeId = 2,
                IsVisible = true,
                IsAccessible = true
            },

            // Configurations Navigation (29-32)
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("E9D0E1F2-A3B4-2D5E-DF6A-9B0C1D2E3F4A"),
                Id = 29,
                Name = "Instance Admin - Add Configurations",
                Description = "Allows Instance Admin to add configuration records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 29,
                NavigationUserActionId = 29,
                RoleTypeId = 2,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("F0E1F2A3-B4C5-3E6F-EA7B-0C1D2E3F4A5B"),
                Id = 30,
                Name = "Instance Admin - View Configurations",
                Description = "Allows Instance Admin to view configuration records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 30,
                NavigationUserActionId = 30,
                RoleTypeId = 2,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("A1F2A3B4-C5D6-4F7A-FB8C-1D2E3F4A5B6C"),
                Id = 31,
                Name = "Instance Admin - Edit Configurations",
                Description = "Allows Instance Admin to edit configuration records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 31,
                NavigationUserActionId = 31,
                RoleTypeId = 2,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("A4C8DD9A-FEA1-468B-9CF5-CB4AF196CB36"),
                Id = 32,
                Name = "Instance Admin - Delete Configurations",
                Description = "Allows Instance Admin to delete configuration records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 32,
                NavigationUserActionId = 32,
                RoleTypeId = 2,
                IsVisible = true,
                IsAccessible = true
            },

            // License Navigation (53-56)
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("F19204C7-81DA-4BDC-A8B3-4E658E778017"),
                Id = 33,
                Name = "Instance Admin - Add License",
                Description = "Allows Instance Admin to add license records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 33,
                NavigationUserActionId = 53,
                RoleTypeId = 2,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("108DABEF-662B-4B4E-B184-639C1AFF8247"),
                Id = 34,
                Name = "Instance Admin - View License",
                Description = "Allows Instance Admin to view license records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 34,
                NavigationUserActionId = 54,
                RoleTypeId = 2,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("70F1B63C-584B-487F-B9D4-13B0DAB5B555"),
                Id = 35,
                Name = "Instance Admin - Edit License",
                Description = "Allows Instance Admin to edit license records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 35,
                NavigationUserActionId = 55,
                RoleTypeId = 2,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("6A79E0D9-EA76-4B78-AE06-AF1EA0523147"),
                Id = 36,
                Name = "Instance Admin - Delete License",
                Description = "Allows Instance Admin to delete license records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 36,
                NavigationUserActionId = 56,
                RoleTypeId = 2,
                IsVisible = true,
                IsAccessible = true
            },

            // ========================================
            // Client Admin (RoleTypeId = 3)
            // ========================================

            // Organization Navigation (1-4)
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("A7B8C9D0-E1F2-0A3B-CD4E-7F8A9B0C1D2E"),
                Id = 37,
                Name = "Client Admin - Add Organization",
                Description = "Allows Client Admin to add organization records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 37,
                NavigationUserActionId = 1,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("B8C9D0E1-F2A3-1B4C-DE5F-8A9B0C1D2E3F"),
                Id = 38,
                Name = "Client Admin - View Organization",
                Description = "Allows Client Admin to view organization records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 38,
                NavigationUserActionId = 2,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("C9D0E1F2-A3B4-2C5D-EF6A-9B0C1D2E3F4A"),
                Id = 39,
                Name = "Client Admin - Edit Organization",
                Description = "Allows Client Admin to edit organization records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 39,
                NavigationUserActionId = 3,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("D0E1F2A3-B4C5-3D6E-FA7B-0C1D2E3F4A5B"),
                Id = 40,
                Name = "Client Admin - Delete Organization",
                Description = "Allows Client Admin to delete organization records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 40,
                NavigationUserActionId = 4,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },

            // Project Navigation (5-8)
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("E1F2A3B4-C5D6-4E7F-AB8C-1D2E3F4A5B6C"),
                Id = 41,
                Name = "Client Admin - Add Project",
                Description = "Allows Client Admin to add project records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 41,
                NavigationUserActionId = 5,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("F2A3B4C5-D6E7-5F8A-BC9D-2E3F4A5B6C7D"),
                Id = 42,
                Name = "Client Admin - View Project",
                Description = "Allows Client Admin to view project records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 42,
                NavigationUserActionId = 6,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("A3B4C5D6-E7F8-6A9B-CD0E-3F4A5B6C7D8E"),
                Id = 43,
                Name = "Client Admin - Edit Project",
                Description = "Allows Client Admin to edit project records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 43,
                NavigationUserActionId = 7,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("B4C5D6E7-F8A9-7B0C-DE1F-4A5B6C7D8E9F"),
                Id = 44,
                Name = "Client Admin - Delete Project",
                Description = "Allows Client Admin to delete project records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 44,
                NavigationUserActionId = 8,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },

            // Summary Navigation (13-16)
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("C5D6E7F8-A9B0-8C1D-EF2A-5B6C7D8E9F0A"),
                Id = 45,
                Name = "Client Admin - Add Summary",
                Description = "Allows Client Admin to add summary records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 45,
                NavigationUserActionId = 13,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("D6E7F8A9-B0C1-9D2E-FA3B-6C7D8E9F0A1B"),
                Id = 46,
                Name = "Client Admin - View Summary",
                Description = "Allows Client Admin to view summary records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 46,
                NavigationUserActionId = 14,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("E7F8A9B0-C1D2-0E3F-AB4C-7D8E9F0A1B2C"),
                Id = 47,
                Name = "Client Admin - Edit Summary",
                Description = "Allows Client Admin to edit summary records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 47,
                NavigationUserActionId = 15,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("F8A9B0C1-D2E3-1F4A-BC5D-8E9F0A1B2C3D"),
                Id = 48,
                Name = "Client Admin - Delete Summary",
                Description = "Allows Client Admin to delete summary records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 48,
                NavigationUserActionId = 16,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },

            // Roles Navigation (21-24)
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("A9B0C1D2-E3F4-2A5B-CD6E-9F0A1B2C3D4E"),
                Id = 49,
                Name = "Client Admin - Add Roles",
                Description = "Allows Client Admin to add role records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 49,
                NavigationUserActionId = 21,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("B0C1D2E3-F4A5-3B6C-DE7F-0A1B2C3D4E5F"),
                Id = 50,
                Name = "Client Admin - View Roles",
                Description = "Allows Client Admin to view role records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 50,
                NavigationUserActionId = 22,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("C1D2E3F4-A5B6-4C7D-EF8A-1B2C3D4E5F6A"),
                Id = 51,
                Name = "Client Admin - Edit Roles",
                Description = "Allows Client Admin to edit role records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 51,
                NavigationUserActionId = 23,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("D2E3F4A5-B6C7-5D8E-FA9B-2C3D4E5F6A7B"),
                Id = 52,
                Name = "Client Admin - Delete Roles",
                Description = "Allows Client Admin to delete role records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 52,
                NavigationUserActionId = 24,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },

            // Connectors Navigation (25-28)
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("E3F4A5B6-C7D8-6E9F-AB0C-3D4E5F6A7B8C"),
                Id = 53,
                Name = "Client Admin - Add Connectors",
                Description = "Allows Client Admin to add connector records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 53,
                NavigationUserActionId = 25,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("F4A5B6C7-D8E9-7F0A-BC1D-4E5F6A7B8C9D"),
                Id = 54,
                Name = "Client Admin - View Connectors",
                Description = "Allows Client Admin to view connector records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 54,
                NavigationUserActionId = 26,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("A5B6C7D8-E9F0-8A1B-CD2E-5F6A7B8C9D0E"),
                Id = 55,
                Name = "Client Admin - Edit Connectors",
                Description = "Allows Client Admin to edit connector records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 55,
                NavigationUserActionId = 27,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("B6C7D8E9-F0A1-9B2C-DE3F-6A7B8C9D0E1F"),
                Id = 56,
                Name = "Client Admin - Delete Connectors",
                Description = "Allows Client Admin to delete connector records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 56,
                NavigationUserActionId = 28,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },

            // Configurations Navigation (29-32)
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("C7D8E9F0-A1B2-0C3D-EF4A-7B8C9D0E1F2A"),
                Id = 57,
                Name = "Client Admin - Add Configurations",
                Description = "Allows Client Admin to add configuration records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 57,
                NavigationUserActionId = 29,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("D8E9F0A1-B2C3-1D4E-FA5B-8C9D0E1F2A3B"),
                Id = 58,
                Name = "Client Admin - View Configurations",
                Description = "Allows Client Admin to view configuration records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 58,
                NavigationUserActionId = 30,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("E9F0A1B2-C3D4-2E5F-AB6C-9D0E1F2A3B4C"),
                Id = 59,
                Name = "Client Admin - Edit Configurations",
                Description = "Allows Client Admin to edit configuration records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 59,
                NavigationUserActionId = 31,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("F0A1B2C3-D4E5-3F6A-BC7D-0E1F2A3B4C5D"),
                Id = 60,
                Name = "Client Admin - Delete Configurations",
                Description = "Allows Client Admin to delete configuration records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 60,
                NavigationUserActionId = 32,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },

            // Documents Navigation (37-40)
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("A1B2C3D4-E5F6-4A7B-CD8E-1F2A3B4C5D6E"),
                Id = 61,
                Name = "Client Admin - Add Documents",
                Description = "Allows Client Admin to add document records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 61,
                NavigationUserActionId = 37,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("B2C3D4E5-F6A7-5B8C-DE9F-2A3B4C5D6E7F"),
                Id = 62,
                Name = "Client Admin - View Documents",
                Description = "Allows Client Admin to view document records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 62,
                NavigationUserActionId = 38,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("C3D4E5F6-A7B8-6C9D-EF0A-3B4C5D6E7F8A"),
                Id = 63,
                Name = "Client Admin - Edit Documents",
                Description = "Allows Client Admin to edit document records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 63,
                NavigationUserActionId = 39,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("D4E5F6A7-B8C9-7D0E-FA1B-4C5D6E7F8A9B"),
                Id = 64,
                Name = "Client Admin - Delete Documents",
                Description = "Allows Client Admin to delete document records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 64,
                NavigationUserActionId = 40,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },

            // Questionnaire Builder Navigation (41-44)
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("E5F6A7B8-C9D0-8E1F-AB2C-5D6E7F8A9B0C"),
                Id = 65,
                Name = "Client Admin - Add Questionnaire Builder",
                Description = "Allows Client Admin to add questionnaire builder records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 65,
                NavigationUserActionId = 41,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("F6A7B8C9-D0E1-9F2A-BC3D-6E7F8A9B0C1D"),
                Id = 66,
                Name = "Client Admin - View Questionnaire Builder",
                Description = "Allows Client Admin to view questionnaire builder records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 66,
                NavigationUserActionId = 42,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("45DA017E-98C3-4E5E-889A-8A968C6B2F04"),
                Id = 67,
                Name = "Client Admin - Edit Questionnaire Builder",
                Description = "Allows Client Admin to edit questionnaire builder records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 67,
                NavigationUserActionId = 43,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("30BA533C-EA23-4D8A-8992-76F0386C5900"),
                Id = 68,
                Name = "Client Admin - Delete Questionnaire Builder",
                Description = "Allows Client Admin to delete questionnaire builder records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 68,
                NavigationUserActionId = 44,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },

            // Insights Template Navigation (45-48)
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("316A3619-14D4-4865-AFD4-EA86A3DE3522"),
                Id = 69,
                Name = "Client Admin - Add Insights Template",
                Description = "Allows Client Admin to add insights template records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 69,
                NavigationUserActionId = 45,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("28D5A20A-5405-4A5F-A391-E7CEF7824742"),
                Id = 70,
                Name = "Client Admin - View Insights Template",
                Description = "Allows Client Admin to view insights template records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 70,
                NavigationUserActionId = 46,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("8F367EBD-5BA3-40CE-8933-86C7935EC567"),
                Id = 71,
                Name = "Client Admin - Edit Insights Template",
                Description = "Allows Client Admin to edit insights template records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 71,
                NavigationUserActionId = 47,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("14A50110-9211-48ED-AE11-E8D2BFC8BB19"),
                Id = 72,
                Name = "Client Admin - Delete Insights Template",
                Description = "Allows Client Admin to delete insights template records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 72,
                NavigationUserActionId = 48,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },

            // Users Navigation (49-52)
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("A4725961-E4D3-44E1-9A54-2FEA4DD397FB"),
                Id = 73,
                Name = "Client Admin - Add Users",
                Description = "Allows Client Admin to add user records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 73,
                NavigationUserActionId = 49,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("019A7C6E-14BE-4A59-8595-14A170F57190"),
                Id = 74,
                Name = "Client Admin - View Users",
                Description = "Allows Client Admin to view user records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 74,
                NavigationUserActionId = 50,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("20F411D5-4EC4-40DC-8E51-BB26435D87E9"),
                Id = 75,
                Name = "Client Admin - Edit Users",
                Description = "Allows Client Admin to edit user records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 75,
                NavigationUserActionId = 51,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("FA863B93-F964-4556-AA7E-5FD0CC2A10C3"),
                Id = 76,
                Name = "Client Admin - Delete Users",
                Description = "Allows Client Admin to delete user records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 76,
                NavigationUserActionId = 52,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },

            // License Navigation (53-56)
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("1D7BD629-D248-4DCE-A95C-CCCDD506E0F1"),
                Id = 77,
                Name = "Client Admin - Add License",
                Description = "Allows Client Admin to add license records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 77,
                NavigationUserActionId = 53,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("F5126F8C-F4E0-4BBA-BC31-74941A2941FE"),
                Id = 78,
                Name = "Client Admin - View License",
                Description = "Allows Client Admin to view license records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 78,
                NavigationUserActionId = 54,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("3E79DC8A-1D81-4795-A639-EB25A7D1216B"),
                Id = 79,
                Name = "Client Admin - Edit License",
                Description = "Allows Client Admin to edit license records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 79,
                NavigationUserActionId = 55,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("AB3BFDD8-3C52-4397-95C2-2E6751EEDBDE"),
                Id = 80,
                Name = "Client Admin - Delete License",
                Description = "Allows Client Admin to delete license records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 80,
                NavigationUserActionId = 56,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },

            // Summary Navigation (61-64) - Navigation Id 16
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("6D542B5F-E345-4A36-9FDA-FE9E89D06EB8"),
                Id = 81,
                Name = "Client Admin - Add Summary",
                Description = "Allows Client Admin to add summary records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 81,
                NavigationUserActionId = 61,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("707A6F07-05F9-4B76-9C96-E18685A0AAA5"),
                Id = 82,
                Name = "Client Admin - View Summary",
                Description = "Allows Client Admin to view summary records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 82,
                NavigationUserActionId = 62,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("E8C4DA5F-DB5C-4284-9410-3D92B0AC8963"),
                Id = 83,
                Name = "Client Admin - Edit Summary",
                Description = "Allows Client Admin to edit summary records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 83,
                NavigationUserActionId = 63,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("CDB1F89B-5805-42CD-8975-055FDA28D3BE"),
                Id = 84,
                Name = "Client Admin - Delete Summary",
                Description = "Allows Client Admin to delete summary records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 84,
                NavigationUserActionId = 64,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },

            // Workflow Set Up Navigation (65-68) - Navigation Id 17
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("62DD146B-F4DC-411D-92D0-DD4266EEF2A5"),
                Id = 85,
                Name = "Client Admin - Add Workflow Set Up",
                Description = "Allows Client Admin to add workflow setup records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 85,
                NavigationUserActionId = 65,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("34C54471-DB4D-4552-B6FB-0A89303A187C"),
                Id = 86,
                Name = "Client Admin - View Workflow Set Up",
                Description = "Allows Client Admin to view workflow setup records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 86,
                NavigationUserActionId = 66,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("F514C4F2-6F11-4BA7-8E60-E99763E024B1"),
                Id = 87,
                Name = "Client Admin - Edit Workflow Set Up",
                Description = "Allows Client Admin to edit workflow setup records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 87,
                NavigationUserActionId = 67,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("9687061E-6FC6-4154-9908-7B91D92BB145"),
                Id = 88,
                Name = "Client Admin - Delete Workflow Set Up",
                Description = "Allows Client Admin to delete workflow setup records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 88,
                NavigationUserActionId = 68,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },

            // Insights Navigation (69-72) - Navigation Id 18
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("48C61F1A-F44B-4E95-A97E-C5ED65494DE4"),
                Id = 89,
                Name = "Client Admin - Add Insights",
                Description = "Allows Client Admin to add insights records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 89,
                NavigationUserActionId = 69,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("BAB2423D-8CB0-45F2-93C2-CBCEE760ABD9"),
                Id = 90,
                Name = "Client Admin - View Insights",
                Description = "Allows Client Admin to view insights records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 90,
                NavigationUserActionId = 70,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("430244FD-B25B-4BDB-B1A6-5827FE13682E"),
                Id = 91,
                Name = "Client Admin - Edit Insights",
                Description = "Allows Client Admin to edit insights records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 91,
                NavigationUserActionId = 71,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("93AD1E89-4DA8-4E33-814E-4621D3EB484E"),
                Id = 92,
                Name = "Client Admin - Delete Insights",
                Description = "Allows Client Admin to delete insights records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 92,
                NavigationUserActionId = 72,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },

            // Alerts Navigation (77-80) - Navigation Id 20
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("91EFC995-0D54-4627-8AC0-0BC75E542851"),
                Id = 93,
                Name = "Client Admin - Add Alerts",
                Description = "Allows Client Admin to add alert records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 93,
                NavigationUserActionId = 77,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("2B8962E8-02F1-4EAA-B947-BA193031C50A"),
                Id = 94,
                Name = "Client Admin - View Alerts",
                Description = "Allows Client Admin to view alert records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 94,
                NavigationUserActionId = 78,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("9A84FC94-675C-441D-B3B4-195FB9C5A4F5"),
                Id = 95,
                Name = "Client Admin - Edit Alerts",
                Description = "Allows Client Admin to edit alert records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 95,
                NavigationUserActionId = 79,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("F4CF71D5-C2DE-4872-9F7A-C1B8C6DF685A"),
                Id = 96,
                Name = "Client Admin - Delete Alerts",
                Description = "Allows Client Admin to delete alert records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 96,
                NavigationUserActionId = 80,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },

            // Users Navigation (81-84) - Navigation Id 21
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("50B5420C-3164-49EA-82A5-B62AB23E2650"),
                Id = 97,
                Name = "Client Admin - Add Users",
                Description = "Allows Client Admin to add user records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 97,
                NavigationUserActionId = 81,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("8652CED0-15BE-467C-8B0C-50F84D323C21"),
                Id = 98,
                Name = "Client Admin - View Users",
                Description = "Allows Client Admin to view user records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 98,
                NavigationUserActionId = 82,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("5F896103-44AC-439A-8A7B-0C2BDB1633FA"),
                Id = 99,
                Name = "Client Admin - Edit Users",
                Description = "Allows Client Admin to edit user records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 99,
                NavigationUserActionId = 83,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("F961D525-6047-4D1C-ADBF-B044EBA8E4E2"),
                Id = 100,
                Name = "Client Admin - Delete Users",
                Description = "Allows Client Admin to delete user records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 100,
                NavigationUserActionId = 84,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },

            // Roles Navigation (85-88) - Navigation Id 22
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("34C148A9-E56C-4BE1-AAF1-107F510D0B4C"),
                Id = 101,
                Name = "Client Admin - Add Roles",
                Description = "Allows Client Admin to add role records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 101,
                NavigationUserActionId = 85,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("6A96DE19-8DDC-472A-B454-79A7A94B6D94"),
                Id = 102,
                Name = "Client Admin - View Roles",
                Description = "Allows Client Admin to view role records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 102,
                NavigationUserActionId = 86,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("0805EAB8-D62C-4F0C-B2D5-1FDFDC3644E1"),
                Id = 103,
                Name = "Client Admin - Edit Roles",
                Description = "Allows Client Admin to edit role records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 103,
                NavigationUserActionId = 87,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("AEF8CC96-CFA8-4441-942F-A3E10F9BC4D0"),
                Id = 104,
                Name = "Client Admin - Delete Roles",
                Description = "Allows Client Admin to delete role records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 104,
                NavigationUserActionId = 88,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },

            // Entity View Navigation (93-96) - Navigation Id 24
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("26841A8C-B155-4D65-B65A-DAB0652651E3"),
                Id = 105,
                Name = "Client Admin - Add Entity View",
                Description = "Allows Client Admin to add entity view records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 105,
                NavigationUserActionId = 93,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("BB37C88E-637B-4D2B-9EF6-ADE92C7AEA67"),
                Id = 106,
                Name = "Client Admin - View Entity View",
                Description = "Allows Client Admin to view entity view records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 106,
                NavigationUserActionId = 94,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("B9B8FA59-8269-4B0C-9275-FA72C5FAA1D6"),
                Id = 107,
                Name = "Client Admin - Edit Entity View",
                Description = "Allows Client Admin to edit entity view records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 107,
                NavigationUserActionId = 95,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("D172B808-0EA9-42E5-9D6B-7ED2BB345670"),
                Id = 108,
                Name = "Client Admin - Delete Entity View",
                Description = "Allows Client Admin to delete entity view records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 108,
                NavigationUserActionId = 96,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },

            // Transaction View Navigation (97-100) - Navigation Id 25
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("DDF4BDA5-9D14-4162-BE60-7EA38B8182C1"),
                Id = 109,
                Name = "Client Admin - Add Transaction View",
                Description = "Allows Client Admin to add transaction view records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 109,
                NavigationUserActionId = 97,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("688CC2C7-C2CC-4CEC-B3A3-79017F5F32AE"),
                Id = 110,
                Name = "Client Admin - View Transaction View",
                Description = "Allows Client Admin to view transaction view records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 110,
                NavigationUserActionId = 98,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("AA416B54-8AE6-4DFC-ACF6-CA48364AFB2C"),
                Id = 111,
                Name = "Client Admin - Edit Transaction View",
                Description = "Allows Client Admin to edit transaction view records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 111,
                NavigationUserActionId = 99,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("6078F049-3B56-4E4B-A6FE-16D570B22D26"),
                Id = 112,
                Name = "Client Admin - Delete Transaction View",
                Description = "Allows Client Admin to delete transaction view records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 112,
                NavigationUserActionId = 100,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },

            // Scenario Manager Navigation (101-104) - Navigation Id 26
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("F15EABF4-B6E1-4511-8F55-EC170FF26D8B"),
                Id = 113,
                Name = "Client Admin - Add Scenario Manager",
                Description = "Allows Client Admin to add scenario manager records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 113,
                NavigationUserActionId = 101,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("B309A63F-DE72-4940-92DA-B2AFA3E0AC27"),
                Id = 114,
                Name = "Client Admin - View Scenario Manager",
                Description = "Allows Client Admin to view scenario manager records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 114,
                NavigationUserActionId = 102,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("92C52CA9-E3BE-4619-8C65-568880B7A7FA"),
                Id = 115,
                Name = "Client Admin - Edit Scenario Manager",
                Description = "Allows Client Admin to edit scenario manager records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 115,
                NavigationUserActionId = 103,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("2CA8F04D-77E1-4BB1-8A2B-87878F10AD64"),
                Id = 116,
                Name = "Client Admin - Delete Scenario Manager",
                Description = "Allows Client Admin to delete scenario manager records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 116,
                NavigationUserActionId = 104,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },

            // Similar Transaction Navigation (105-108) - Navigation Id 27
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("95C39186-9729-463E-AF9F-18CB9D1269A9"),
                Id = 117,
                Name = "Client Admin - Add Similar Transaction",
                Description = "Allows Client Admin to add similar transaction records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 117,
                NavigationUserActionId = 105,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("7C2DFCD0-F6FB-4604-918C-649ED6356C23"),
                Id = 118,
                Name = "Client Admin - View Similar Transaction",
                Description = "Allows Client Admin to view similar transaction records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 118,
                NavigationUserActionId = 106,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("20E1C174-1C54-422B-8AE2-904244C55AF4"),
                Id = 119,
                Name = "Client Admin - Edit Similar Transaction",
                Description = "Allows Client Admin to edit similar transaction records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 119,
                NavigationUserActionId = 107,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("FF1A0954-FE41-49C0-8723-58C445283FC6"),
                Id = 120,
                Name = "Client Admin - Delete Similar Transaction",
                Description = "Allows Client Admin to delete similar transaction records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 120,
                NavigationUserActionId = 108,
                RoleTypeId = 3,
                IsVisible = true,
                IsAccessible = true
            },
            // ========================================
            // Data Manager (RoleTypeId = 4)
            // ========================================

            // Organization Navigation (1-4)
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("1A2B3C4D-5E6F-7A8B-9C0D-1E2F3A4B5C6D"),
                Id = 121,
                Name = "Data Manager - Add Organization",
                Description = "Allows Data Manager to add organization records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 121,
                NavigationUserActionId = 1,
                RoleTypeId = 4,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("2B3C4D5E-6F7A-8B9C-0D1E-2F3A4B5C6D7E"),
                Id = 122,
                Name = "Data Manager - View Organization",
                Description = "Allows Data Manager to view organization records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 122,
                NavigationUserActionId = 2,
                RoleTypeId = 4,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("3C4D5E6F-7A8B-9C0D-1E2F-3A4B5C6D7E8F"),
                Id = 123,
                Name = "Data Manager - Edit Organization",
                Description = "Allows Data Manager to edit organization records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 123,
                NavigationUserActionId = 3,
                RoleTypeId = 4,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("4D5E6F7A-8B9C-0D1E-2F3A-4B5C6D7E8F9A"),
                Id = 124,
                Name = "Data Manager - Delete Organization",
                Description = "Allows Data Manager to delete organization records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 124,
                NavigationUserActionId = 4,
                RoleTypeId = 4,
                IsVisible = true,
                IsAccessible = true
            },

            // Project Navigation (5-8)
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("5E6F7A8B-9C0D-1E2F-3A4B-5C6D7E8F9A0B"),
                Id = 125,
                Name = "Data Manager - Add Project",
                Description = "Allows Data Manager to add project records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 125,
                NavigationUserActionId = 5,
                RoleTypeId = 4,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("6F7A8B9C-0D1E-2F3A-4B5C-6D7E8F9A0B1C"),
                Id = 126,
                Name = "Data Manager - View Project",
                Description = "Allows Data Manager to view project records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 126,
                NavigationUserActionId = 6,
                RoleTypeId = 4,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("7A8B9C0D-1E2F-3A4B-5C6D-7E8F9A0B1C2D"),
                Id = 127,
                Name = "Data Manager - Edit Project",
                Description = "Allows Data Manager to edit project records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 127,
                NavigationUserActionId = 7,
                RoleTypeId = 4,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("8B9C0D1E-2F3A-4B5C-6D7E-8F9A0B1C2D3E"),
                Id = 128,
                Name = "Data Manager - Delete Project",
                Description = "Allows Data Manager to delete project records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 128,
                NavigationUserActionId = 8,
                RoleTypeId = 4,
                IsVisible = true,
                IsAccessible = true
            },

            // Summary Navigation (13-16)
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("9C0D1E2F-3A4B-5C6D-7E8F-9A0B1C2D3E4F"),
                Id = 129,
                Name = "Data Manager - Add Summary",
                Description = "Allows Data Manager to add summary records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 129,
                NavigationUserActionId = 13,
                RoleTypeId = 4,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("0D1E2F3A-4B5C-6D7E-8F9A-0B1C2D3E4F5A"),
                Id = 130,
                Name = "Data Manager - View Summary",
                Description = "Allows Data Manager to view summary records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 130,
                NavigationUserActionId = 14,
                RoleTypeId = 4,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("1E2F3A4B-5C6D-7E8F-9A0B-1C2D3E4F5A6B"),
                Id = 131,
                Name = "Data Manager - Edit Summary",
                Description = "Allows Data Manager to edit summary records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 131,
                NavigationUserActionId = 15,
                RoleTypeId = 4,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("2F3A4B5C-6D7E-8F9A-0B1C-2D3E4F5A6B7C"),
                Id = 132,
                Name = "Data Manager - Delete Summary",
                Description = "Allows Data Manager to delete summary records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 132,
                NavigationUserActionId = 16,
                RoleTypeId = 4,
                IsVisible = true,
                IsAccessible = true
            },

            // Connections Navigation (33-36)
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("3A4B5C6D-7E8F-9A0B-1C2D-3E4F5A6B7C8D"),
                Id = 133,
                Name = "Data Manager - Add Connections",
                Description = "Allows Data Manager to add connection records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 133,
                NavigationUserActionId = 33,
                RoleTypeId = 4,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("4B5C6D7E-8F9A-0B1C-2D3E-4F5A6B7C8D9E"),
                Id = 134,
                Name = "Data Manager - View Connections",
                Description = "Allows Data Manager to view connection records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 134,
                NavigationUserActionId = 34,
                RoleTypeId = 4,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("5C6D7E8F-9A0B-1C2D-3E4F-5A6B7C8D9E0F"),
                Id = 135,
                Name = "Data Manager - Edit Connections",
                Description = "Allows Data Manager to edit connection records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 135,
                NavigationUserActionId = 35,
                RoleTypeId = 4,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("6D7E8F9A-0B1C-2D3E-4F5A-6B7C8D9E0F1A"),
                Id = 136,
                Name = "Data Manager - Delete Connections",
                Description = "Allows Data Manager to delete connection records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 136,
                NavigationUserActionId = 36,
                RoleTypeId = 4,
                IsVisible = true,
                IsAccessible = true
            },

            // Project Dashboard Navigation (109-112)
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("7E8F9A0B-1C2D-3E4F-5A6B-7C8D9E0F1A2B"),
                Id = 137,
                Name = "Data Manager - Add Project Dashboard",
                Description = "Allows Data Manager to add project dashboard records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 137,
                NavigationUserActionId = 109,
                RoleTypeId = 4,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("8F9A0B1C-2D3E-4F5A-6B7C-8D9E0F1A2B3C"),
                Id = 138,
                Name = "Data Manager - View Project Dashboard",
                Description = "Allows Data Manager to view project dashboard records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 138,
                NavigationUserActionId = 110,
                RoleTypeId = 4,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("9A0B1C2D-3E4F-5A6B-7C8D-9E0F1A2B3C4D"),
                Id = 139,
                Name = "Data Manager - Edit Project Dashboard",
                Description = "Allows Data Manager to edit project dashboard records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 139,
                NavigationUserActionId = 111,
                RoleTypeId = 4,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("0B1C2D3E-4F5A-6B7C-8D9E-0F1A2B3C4D5E"),
                Id = 140,
                Name = "Data Manager - Delete Project Dashboard",
                Description = "Allows Data Manager to delete project dashboard records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 140,
                NavigationUserActionId = 112,
                RoleTypeId = 4,
                IsVisible = true,
                IsAccessible = true
            },
            // ========================================
            // L1 Investigator (RoleTypeId = 6)
            // ========================================

            // Organization Navigation (1-4)
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("1D2E3F4A-5B6C-7D8E-9F0A-1B2C3D4E5F6A"),
                Id = 141,
                Name = "L1 Investigator - Add Organization",
                Description = "Allows L1 Investigator to add organization records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 141,
                NavigationUserActionId = 1,
                RoleTypeId = 6,
                IsVisible = true,
                IsAccessible = false
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("2E3F4A5B-6C7D-8E9F-0A1B-2C3D4E5F6A7B"),
                Id = 142,
                Name = "L1 Investigator - View Organization",
                Description = "Allows L1 Investigator to view organization records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 142,
                NavigationUserActionId = 2,
                RoleTypeId = 6,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("3F4A5B6C-7D8E-9F0A-1B2C-3D4E5F6A7B8C"),
                Id = 143,
                Name = "L1 Investigator - Edit Organization",
                Description = "Allows L1 Investigator to edit organization records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 143,
                NavigationUserActionId = 3,
                RoleTypeId = 6,
                IsVisible = true,
                IsAccessible = false
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("4A5B6C7D-8E9F-0A1B-2C3D-4E5F6A7B8C9D"),
                Id = 144,
                Name = "L1 Investigator - Delete Organization",
                Description = "Allows L1 Investigator to delete organization records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 144,
                NavigationUserActionId = 4,
                RoleTypeId = 6,
                IsVisible = true,
                IsAccessible = false
            },

            // Project Navigation (5-8)
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("5B6C7D8E-9F0A-1B2C-3D4E-5F6A7B8C9D0E"),
                Id = 145,
                Name = "L1 Investigator - Add Project",
                Description = "Allows L1 Investigator to add project records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 145,
                NavigationUserActionId = 5,
                RoleTypeId = 6,
                IsVisible = true,
                IsAccessible = false
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("6C7D8E9F-0A1B-2C3D-4E5F-6A7B8C9D0E1F"),
                Id = 146,
                Name = "L1 Investigator - View Project",
                Description = "Allows L1 Investigator to view project records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 146,
                NavigationUserActionId = 6,
                RoleTypeId = 6,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("7D8E9F0A-1B2C-3D4E-5F6A-7B8C9D0E1F2A"),
                Id = 147,
                Name = "L1 Investigator - Edit Project",
                Description = "Allows L1 Investigator to edit project records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 147,
                NavigationUserActionId = 7,
                RoleTypeId = 6,
                IsVisible = true,
                IsAccessible = false
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("8E9F0A1B-2C3D-4E5F-6A7B-8C9D0E1F2A3B"),
                Id = 148,
                Name = "L1 Investigator - Delete Project",
                Description = "Allows L1 Investigator to delete project records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 148,
                NavigationUserActionId = 8,
                RoleTypeId = 6,
                IsVisible = true,
                IsAccessible = false
            },

            // Summary Navigation (13-16)
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("9F0A1B2C-3D4E-5F6A-7B8C-9D0E1F2A3B4C"),
                Id = 149,
                Name = "L1 Investigator - Add Summary",
                Description = "Allows L1 Investigator to add summary records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 149,
                NavigationUserActionId = 13,
                RoleTypeId = 6,
                IsVisible = true,
                IsAccessible = false
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("0A1B2C3D-4E5F-6A7B-8C9D-0E1F2A3B4C5D"),
                Id = 150,
                Name = "L1 Investigator - View Summary",
                Description = "Allows L1 Investigator to view summary records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 150,
                NavigationUserActionId = 14,
                RoleTypeId = 6,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("1B2C3D4E-5F6A-7B8C-9D0E-1F2A3B4C5D6E"),
                Id = 151,
                Name = "L1 Investigator - Edit Summary",
                Description = "Allows L1 Investigator to edit summary records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 151,
                NavigationUserActionId = 15,
                RoleTypeId = 6,
                IsVisible = true,
                IsAccessible = false
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("2C3D4E5F-6A7B-8C9D-0E1F-2A3B4C5D6E7F"),
                Id = 152,
                Name = "L1 Investigator - Delete Summary",
                Description = "Allows L1 Investigator to delete summary records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 152,
                NavigationUserActionId = 16,
                RoleTypeId = 6,
                IsVisible = true,
                IsAccessible = false
            },

            // Alerts Navigation (77-80)
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("3D4E5F6A-7B8C-9D0E-1F2A-3B4C5D6E7F8A"),
                Id = 153,
                Name = "L1 Investigator - Add Alerts",
                Description = "Allows L1 Investigator to add alert records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 153,
                NavigationUserActionId = 77,
                RoleTypeId = 6,
                IsVisible = true,
                IsAccessible = false
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("4E5F6A7B-8C9D-0E1F-2A3B-4C5D6E7F8A9B"),
                Id = 154,
                Name = "L1 Investigator - View Alerts",
                Description = "Allows L1 Investigator to view alert records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 154,
                NavigationUserActionId = 78,
                RoleTypeId = 6,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("5F6A7B8C-9D0E-1F2A-3B4C-5D6E7F8A9B0C"),
                Id = 155,
                Name = "L1 Investigator - Edit Alerts",
                Description = "Allows L1 Investigator to edit alert records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 155,
                NavigationUserActionId = 79,
                RoleTypeId = 6,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("6A7B8C9D-0E1F-2A3B-4C5D-6E7F8A9B0C1D"),
                Id = 156,
                Name = "L1 Investigator - Delete Alerts",
                Description = "Allows L1 Investigator to delete alert records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 156,
                NavigationUserActionId = 80,
                RoleTypeId = 6,
                IsVisible = true,
                IsAccessible = false
            },

            // ========================================
            // L2 Investigator (RoleTypeId = 7)
            // ========================================

            // Organization Navigation (1-4)
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("7B8C9D0E-1F2A-3B4C-5D6E-7F8A9B0C1D2E"),
                Id = 157,
                Name = "L2 Investigator - Add Organization",
                Description = "Allows L2 Investigator to add organization records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 157,
                NavigationUserActionId = 1,
                RoleTypeId = 7,
                IsVisible = true,
                IsAccessible = false
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("8C9D0E1F-2A3B-4C5D-6E7F-8A9B0C1D2E3F"),
                Id = 158,
                Name = "L2 Investigator - View Organization",
                Description = "Allows L2 Investigator to view organization records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 158,
                NavigationUserActionId = 2,
                RoleTypeId = 7,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("9D0E1F2A-3B4C-5D6E-7F8A-9B0C1D2E3F4A"),
                Id = 159,
                Name = "L2 Investigator - Edit Organization",
                Description = "Allows L2 Investigator to edit organization records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 159,
                NavigationUserActionId = 3,
                RoleTypeId = 7,
                IsVisible = true,
                IsAccessible = false
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("0E1F2A3B-4C5D-6E7F-8A9B-0C1D2E3F4A5B"),
                Id = 160,
                Name = "L2 Investigator - Delete Organization",
                Description = "Allows L2 Investigator to delete organization records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 160,
                NavigationUserActionId = 4,
                RoleTypeId = 7,
                IsVisible = true,
                IsAccessible = false
            },

            // Project Navigation (5-8)
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("1F2A3B4C-5D6E-7F8A-9B0C-1D2E3F4A5B6C"),
                Id = 161,
                Name = "L2 Investigator - Add Project",
                Description = "Allows L2 Investigator to add project records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 161,
                NavigationUserActionId = 5,
                RoleTypeId = 7,
                IsVisible = true,
                IsAccessible = false
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("2A3B4C5D-6E7F-8A9B-0C1D-2E3F4A5B6C7D"),
                Id = 162,
                Name = "L2 Investigator - View Project",
                Description = "Allows L2 Investigator to view project records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 162,
                NavigationUserActionId = 6,
                RoleTypeId = 7,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("3B4C5D6E-7F8A-9B0C-1D2E-3F4A5B6C7D8E"),
                Id = 163,
                Name = "L2 Investigator - Edit Project",
                Description = "Allows L2 Investigator to edit project records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 163,
                NavigationUserActionId = 7,
                RoleTypeId = 7,
                IsVisible = true,
                IsAccessible = false
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("4C5D6E7F-8A9B-0C1D-2E3F-4A5B6C7D8E9F"),
                Id = 164,
                Name = "L2 Investigator - Delete Project",
                Description = "Allows L2 Investigator to delete project records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 164,
                NavigationUserActionId = 8,
                RoleTypeId = 7,
                IsVisible = true,
                IsAccessible = false
            },

            // Summary Navigation (13-16)
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("5D6E7F8A-9B0C-1D2E-3F4A-5B6C7D8E9F0A"),
                Id = 165,
                Name = "L2 Investigator - Add Summary",
                Description = "Allows L2 Investigator to add summary records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 165,
                NavigationUserActionId = 13,
                RoleTypeId = 7,
                IsVisible = true,
                IsAccessible = false
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("6E7F8A9B-0C1D-2E3F-4A5B-6C7D8E9F0A1B"),
                Id = 166,
                Name = "L2 Investigator - View Summary",
                Description = "Allows L2 Investigator to view summary records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 166,
                NavigationUserActionId = 14,
                RoleTypeId = 7,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("7F8A9B0C-1D2E-3F4A-5B6C-7D8E9F0A1B2C"),
                Id = 167,
                Name = "L2 Investigator - Edit Summary",
                Description = "Allows L2 Investigator to edit summary records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 167,
                NavigationUserActionId = 15,
                RoleTypeId = 7,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("8A9B0C1D-2E3F-4A5B-6C7D-8E9F0A1B2C3D"),
                Id = 168,
                Name = "L2 Investigator - Delete Summary",
                Description = "Allows L2 Investigator to delete summary records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 168,
                NavigationUserActionId = 16,
                RoleTypeId = 7,
                IsVisible = true,
                IsAccessible = false
            },

            // Alerts Dashboard Navigation (73-76)
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("9B0C1D2E-3F4A-5B6C-7D8E-9F0A1B2C3D4E"),
                Id = 169,
                Name = "L2 Investigator - Add Alerts Dashboard",
                Description = "Allows L2 Investigator to add alerts dashboard records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 169,
                NavigationUserActionId = 73,
                RoleTypeId = 7,
                IsVisible = true,
                IsAccessible = false
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("0C1D2E3F-4A5B-6C7D-8E9F-0A1B2C3D4E5F"),
                Id = 170,
                Name = "L2 Investigator - View Alerts Dashboard",
                Description = "Allows L2 Investigator to view alerts dashboard records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 170,
                NavigationUserActionId = 74,
                RoleTypeId = 7,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("1D2E3F4A-5B6C-7D8E-9F0A-1B2C3D4E5F6A"),
                Id = 171,
                Name = "L2 Investigator - Edit Alerts Dashboard",
                Description = "Allows L2 Investigator to edit alerts dashboard records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 171,
                NavigationUserActionId = 75,
                RoleTypeId = 7,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("2E3F4A5B-6C7D-8E9F-0A1B-2C3D4E5F6A7B"),
                Id = 172,
                Name = "L2 Investigator - Delete Alerts Dashboard",
                Description = "Allows L2 Investigator to delete alerts dashboard records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 172,
                NavigationUserActionId = 76,
                RoleTypeId = 7,
                IsVisible = true,
                IsAccessible = false
            },

            // Alerts Navigation (77-80)
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("3F4A5B6C-7D8E-9F0A-1B2C-3D4E5F6A7B8C"),
                Id = 173,
                Name = "L2 Investigator - Add Alerts",
                Description = "Allows L2 Investigator to add alert records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 173,
                NavigationUserActionId = 77,
                RoleTypeId = 7,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("4A5B6C7D-8E9F-0A1B-2C3D-4E5F6A7B8C9D"),
                Id = 174,
                Name = "L2 Investigator - View Alerts",
                Description = "Allows L2 Investigator to view alert records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 174,
                NavigationUserActionId = 78,
                RoleTypeId = 7,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("5B6C7D8E-9F0A-1B2C-3D4E-5F6A7B8C9D0E"),
                Id = 175,
                Name = "L2 Investigator - Edit Alerts",
                Description = "Allows L2 Investigator to edit alert records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 175,
                NavigationUserActionId = 79,
                RoleTypeId = 7,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("6C7D8E9F-0A1B-2C3D-4E5F-6A7B8C9D0E1F"),
                Id = 176,
                Name = "L2 Investigator - Delete Alerts",
                Description = "Allows L2 Investigator to delete alert records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 176,
                NavigationUserActionId = 80,
                RoleTypeId = 7,
                IsVisible = true,
                IsAccessible = true
            },
            // ========================================
            // Guest (RoleTypeId = 1)
            // ========================================

            // Organization Navigation (1-4)
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("7A8B9C0D-1E2F-3A4B-5C6D-7E8F9A0B1C2D"),
                Id = 177,
                Name = "Guest - Add Organization",
                Description = "Allows Guest to add organization records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 177,
                NavigationUserActionId = 1,
                RoleTypeId = 1,
                IsVisible = false,
                IsAccessible = false
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("8B9C0D1E-2F3A-4B5C-6D7E-8F9A0B1C2D3E"),
                Id = 178,
                Name = "Guest - View Organization",
                Description = "Allows Guest to view organization records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 178,
                NavigationUserActionId = 2,
                RoleTypeId = 1,
                IsVisible = false,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("9C0D1E2F-3A4B-5C6D-7E8F-9A0B1C2D3E4F"),
                Id = 179,
                Name = "Guest - Edit Organization",
                Description = "Allows Guest to edit organization records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 179,
                NavigationUserActionId = 3,
                RoleTypeId = 1,
                IsVisible = false,
                IsAccessible = false
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("0D1E2F3A-4B5C-6D7E-8F9A-0B1C2D3E4F5A"),
                Id = 180,
                Name = "Guest - Delete Organization",
                Description = "Allows Guest to delete organization records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 180,
                NavigationUserActionId = 4,
                RoleTypeId = 1,
                IsVisible = false,
                IsAccessible = false
            },

            // Project Navigation (5-8)
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("1E2F3A4B-5C6D-7E8F-9A0B-1C2D3E4F5A6B"),
                Id = 181,
                Name = "Guest - Add Project",
                Description = "Allows Guest to add project records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 181,
                NavigationUserActionId = 5,
                RoleTypeId = 1,
                IsVisible = false,
                IsAccessible = false
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("2F3A4B5C-6D7E-8F9A-0B1C-2D3E4F5A6B7C"),
                Id = 182,
                Name = "Guest - View Project",
                Description = "Allows Guest to view project records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 182,
                NavigationUserActionId = 6,
                RoleTypeId = 1,
                IsVisible = false,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("3A4B5C6D-7E8F-9A0B-1C2D-3E4F5A6B7C8D"),
                Id = 183,
                Name = "Guest - Edit Project",
                Description = "Allows Guest to edit project records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 183,
                NavigationUserActionId = 7,
                RoleTypeId = 1,
                IsVisible = false,
                IsAccessible = false
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("4B5C6D7E-8F9A-0B1C-2D3E-4F5A6B7C8D9E"),
                Id = 184,
                Name = "Guest - Delete Project",
                Description = "Allows Guest to delete project records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 184,
                NavigationUserActionId = 8,
                RoleTypeId = 1,
                IsVisible = false,
                IsAccessible = false
            },

            // All Projects Navigation (57-60)
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("5C6D7E8F-9A0B-1C2D-3E4F-5A6B7C8D9E0F"),
                Id = 185,
                Name = "Guest - Add All Projects",
                Description = "Allows Guest to add all projects records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 185,
                NavigationUserActionId = 57,
                RoleTypeId = 1,
                IsVisible = true,
                IsAccessible = false
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("6D7E8F9A-0B1C-2D3E-4F5A-6B7C8D9E0F1A"),
                Id = 186,
                Name = "Guest - View All Projects",
                Description = "Allows Guest to view all projects records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 186,
                NavigationUserActionId = 58,
                RoleTypeId = 1,
                IsVisible = true,
                IsAccessible = true
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("7E8F9A0B-1C2D-3E4F-5A6B-7C8D9E0F1A2B"),
                Id = 187,
                Name = "Guest - Edit All Projects",
                Description = "Allows Guest to edit all projects records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 187,
                NavigationUserActionId = 59,
                RoleTypeId = 1,
                IsVisible = true,
                IsAccessible = false
            },
            new RoleNavigationUserAction
            {
                RowId = Guid.Parse("8F9A0B1C-2D3E-4F5A-6B7C-8D9E0F1A2B3C"),
                Id = 188,
                Name = "Guest - Delete All Projects",
                Description = "Allows Guest to delete all projects records.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 188,
                NavigationUserActionId = 60,
                RoleTypeId = 1,
                IsVisible = true,
                IsAccessible = false
            }
        );
    }
}
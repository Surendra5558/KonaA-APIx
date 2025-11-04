using KonaAI.Master.Repository.Common.Extensions;
using KonaAI.Master.Repository.Domain.Master.MetaData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KonaAI.Master.Repository.Configuration.Master.MetaData;

/// <summary>
/// Configures the entity mapping, relationships, and seed data for the <see cref="NavigationUserAction"/> entity.
/// Sets up table schema, metadata properties, and initial seed records.
/// </summary>
public class NavigationUserActionConfiguration : IEntityTypeConfiguration<NavigationUserAction>
{
    /// <summary>
    /// Configures the <see cref="NavigationUserAction"/> entity's schema, metadata property constraints, and seed data
    /// using the provided <see cref="EntityTypeBuilder{AppNavigationAction}"/>.
    /// </summary>
    /// <param name="builder">The builder used to configure the <see cref="NavigationUserAction"/> entity.</param>
    public void Configure(EntityTypeBuilder<NavigationUserAction> builder)
    {
        builder.BaseMetaDataConfiguration("NavigationUserAction");

        // Seed data
        builder.HasData(
            // ---------------- Oraganization (Navigation Id: 1) ----------------
            new NavigationUserAction
            {
                RowId = Guid.Parse("A1E4EFA8-F9A9-48EA-812B-25EA1B0B14D9"),
                Id = 1,
                Name = "Add Oraganization",
                Description = "Permission to add records in Oraganization",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 1,
                NavigationId = 1,
                UserActionId = 1
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("2357386C-CC4D-4A72-81B4-3CF718B55B0D"),
                Id = 2,
                Name = "View Oraganization",
                Description = "Permission to view records in Oraganization",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 2,
                NavigationId = 1,
                UserActionId = 2
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("5F2EC1C9-3AB6-42A8-B640-51EE2F75C732"),
                Id = 3,
                Name = "Edit Oraganization",
                Description = "Permission to edit records in Oraganization",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 3,
                NavigationId = 1,
                UserActionId = 3
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("7CA04AEA-DA71-4E1E-BDA0-56A3265E7BFB"),
                Id = 4,
                Name = "Delete Oraganization",
                Description = "Permission to delete records in Oraganization",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 4,
                NavigationId = 1,
                UserActionId = 4
            },

            // ---------------- Project (Navigation Id: 2) ----------------
            new NavigationUserAction
            {
                RowId = Guid.Parse("86EED90A-48BA-40C7-943E-2657FEC292FE"),
                Id = 5,
                Name = "Add Project",
                Description = "Permission to add records in Project",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 5,
                NavigationId = 2,
                UserActionId = 1
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("75042A03-188D-4F6C-9F98-BD71A99E560D"),
                Id = 6,
                Name = "View Project",
                Description = "Permission to view records in Project",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 6,
                NavigationId = 2,
                UserActionId = 2
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("AC080E5D-B77A-4456-9ED9-07672B9F6E10"),
                Id = 7,
                Name = "Edit Project",
                Description = "Permission to edit records in Project",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 7,
                NavigationId = 2,
                UserActionId = 3
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("E230FC44-9138-4CE0-A93B-02F5F99C76FB"),
                Id = 8,
                Name = "Delete Project",
                Description = "Permission to delete records in Project",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 8,
                NavigationId = 2,
                UserActionId = 4
            },

            // ---------------- All Clients (Navigation Id: 3) ----------------
            new NavigationUserAction
            {
                RowId = Guid.Parse("0B7A4946-0517-4B8F-8CD3-94FA4180A133"),
                Id = 9,
                Name = "Add All Clients",
                Description = "Permission to add records in All Clients",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 9,
                NavigationId = 3,
                UserActionId = 1
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("FC1A9EAD-EBDB-4DC5-9AD2-09CCEDA206B7"),
                Id = 10,
                Name = "View All Clients",
                Description = "Permission to view records in All Clients",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 10,
                NavigationId = 3,
                UserActionId = 2
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("5B67CFE0-C36A-4308-94D4-3DB6344F6B2A"),
                Id = 11,
                Name = "Edit All Clients",
                Description = "Permission to edit records in All Clients",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 11,
                NavigationId = 3,
                UserActionId = 3
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("CA5F9ABB-55C8-4B4F-A92F-0FCDE53A9B2A"),
                Id = 12,
                Name = "Delete All Clients",
                Description = "Permission to delete records in All Clients",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 12,
                NavigationId = 3,
                UserActionId = 4
            },

            // ---------------- Summary (Navigation Id: 4) ----------------
            new NavigationUserAction
            {
                RowId = Guid.Parse("55677F1D-4D1D-4728-824F-79E19CA15EFB"),
                Id = 13,
                Name = "Add Summary",
                Description = "Permission to add records in Summary",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 13,
                NavigationId = 4,
                UserActionId = 1
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("3436BB49-B667-43C4-AA25-B3ABD1C6E078"),
                Id = 14,
                Name = "View Summary",
                Description = "Permission to view records in Summary",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 14,
                NavigationId = 4,
                UserActionId = 2
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("CF113604-1F8C-46D1-A3E7-0013615FD99A"),
                Id = 15,
                Name = "Edit Summary",
                Description = "Permission to edit records in Summary",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 15,
                NavigationId = 4,
                UserActionId = 3
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("FFC0BA1A-DDB5-465C-B2CB-DBFC348C44BB"),
                Id = 16,
                Name = "Delete Summary",
                Description = "Permission to delete records in Summary",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 16,
                NavigationId = 4,
                UserActionId = 4
            },

            // ---------------- Members (Navigation Id: 5) ----------------
            new NavigationUserAction
            {
                RowId = Guid.Parse("1F8E2F12-3A4B-4D2C-9A1B-6D09836E1B2C"),
                Id = 17,
                Name = "Add Members",
                Description = "Permission to add records in Members",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 17,
                NavigationId = 5,
                UserActionId = 1
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("C69DC272-3AE6-45F9-9EF7-B88E96720847"),
                Id = 18,
                Name = "View Members",
                Description = "Permission to view records in Members",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 18,
                NavigationId = 5,
                UserActionId = 2
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("B21FF6AA-D2CF-4FB6-928A-8869B9DEF3D0"),
                Id = 19,
                Name = "Edit Members",
                Description = "Permission to edit records in Members",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 19,
                NavigationId = 5,
                UserActionId = 3
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("B2BC39AC-C74A-4517-857F-7C223E7C85F8"),
                Id = 20,
                Name = "Delete Members",
                Description = "Permission to delete records in Members",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 20,
                NavigationId = 5,
                UserActionId = 4
            },

            // ---------------- Roles (Navigation Id: 6) ----------------
            new NavigationUserAction
            {
                RowId = Guid.Parse("19C74863-6C83-4516-8A12-8882FCC82586"),
                Id = 21,
                Name = "Add Roles",
                Description = "Permission to add records in Roles",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 21,
                NavigationId = 6,
                UserActionId = 1
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("A17D82C6-5DB8-477E-A560-F37C8630D116"),
                Id = 22,
                Name = "View Roles",
                Description = "Permission to view records in Roles",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 22,
                NavigationId = 6,
                UserActionId = 2
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("A742F3AB-DF1D-4A37-98EE-9A2109594884"),
                Id = 23,
                Name = "Edit Roles",
                Description = "Permission to edit records in Roles",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 23,
                NavigationId = 6,
                UserActionId = 3
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("065CF650-A4A7-4D6F-A0FA-B78B6E418772"),
                Id = 24,
                Name = "Delete Roles",
                Description = "Permission to delete records in Roles",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 24,
                NavigationId = 6,
                UserActionId = 4
            },

            // ---------------- Connectors (Navigation Id: 7) ----------------
            new NavigationUserAction
            {
                RowId = Guid.Parse("606BCB35-5F2B-40ED-B998-930317AFAB2C"),
                Id = 25,
                Name = "Add Connectors",
                Description = "Permission to add records in Connectors",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 25,
                NavigationId = 7,
                UserActionId = 1
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("619CB64D-9979-42C1-9849-71FECD4B060A"),
                Id = 26,
                Name = "View Connectors",
                Description = "Permission to view records in Connectors",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 26,
                NavigationId = 7,
                UserActionId = 2
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("BBB3C026-4AA8-42E3-8E6A-AD03E9815AE2"),
                Id = 27,
                Name = "Edit Connectors",
                Description = "Permission to edit records in Connectors",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 27,
                NavigationId = 7,
                UserActionId = 3
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("DFCD967C-B63B-4134-997E-3FAA31FF3869"),
                Id = 28,
                Name = "Delete Connectors",
                Description = "Permission to delete records in Connectors",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 28,
                NavigationId = 7,
                UserActionId = 4
            },

            // ---------------- Configurations (Navigation Id: 8) ----------------
            new NavigationUserAction
            {
                RowId = Guid.Parse("64E1DBB1-5635-4DEF-9B21-F72C0A291421"),
                Id = 29,
                Name = "Add Configurations",
                Description = "Permission to add records in Configurations",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 29,
                NavigationId = 8,
                UserActionId = 1
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("C170EAB3-4C84-4401-9A41-1C76089EA761"),
                Id = 30,
                Name = "View Configurations",
                Description = "Permission to view records in Configurations",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 30,
                NavigationId = 8,
                UserActionId = 2
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("793DEEB2-937B-448A-9CAF-20A43F9ABC75"),
                Id = 31,
                Name = "Edit Configurations",
                Description = "Permission to edit records in Configurations",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 31,
                NavigationId = 8,
                UserActionId = 3
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("2E9D1C45-7F24-4C76-BEF1-8D2F9B4C5A6E"),
                Id = 32,
                Name = "Delete Configurations",
                Description = "Permission to delete records in Configurations",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 32,
                NavigationId = 8,
                UserActionId = 4
            },

            // ---------------- Connections (Navigation Id: 9) ----------------
            new NavigationUserAction
            {
                RowId = Guid.Parse("D4A1E9B3-0C55-4BEE-9F3D-73E6B2C8A9F7"),
                Id = 33,
                Name = "Add Connections",
                Description = "Permission to add records in Connections",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 33,
                NavigationId = 9,
                UserActionId = 1
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("3EE58975-46E3-4092-BC5B-890316F836EC"),
                Id = 34,
                Name = "View Connections",
                Description = "Permission to view records in Connections",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 34,
                NavigationId = 9,
                UserActionId = 2
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("FB06C1FC-EF65-4728-9070-6161419C3DAD"),
                Id = 35,
                Name = "Edit Connections",
                Description = "Permission to edit records in Connections",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 35,
                NavigationId = 9,
                UserActionId = 3
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("7ED3A8E9-FC87-4B7F-87AE-1755F3A8AA5D"),
                Id = 36,
                Name = "Delete Connections",
                Description = "Permission to delete records in Connections",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 36,
                NavigationId = 9,
                UserActionId = 4
            },

            // ---------------- Documents (Navigation Id: 10) ----------------
            new NavigationUserAction
            {
                RowId = Guid.Parse("E3F4F4D8-8E88-43CE-9AB1-E52F047A4641"),
                Id = 37,
                Name = "Add Documents",
                Description = "Permission to add records in Documents",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 37,
                NavigationId = 10,
                UserActionId = 1
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("EA229D4C-B7CB-4495-928D-CE8F9F2502F0"),
                Id = 38,
                Name = "View Documents",
                Description = "Permission to view records in Documents",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 38,
                NavigationId = 10,
                UserActionId = 2
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("E0BBC8ED-9CFA-4222-9ADF-6F42DDCA9A52"),
                Id = 39,
                Name = "Edit Documents",
                Description = "Permission to edit records in Documents",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 39,
                NavigationId = 10,
                UserActionId = 3
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("E8075311-2245-416D-AFD7-5E1F7D16ED67"),
                Id = 40,
                Name = "Delete Documents",
                Description = "Permission to delete records in Documents",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 40,
                NavigationId = 10,
                UserActionId = 4
            },

            // ---------------- Questionnaire Builder (Navigation Id: 11) ----------------
            new NavigationUserAction
            {
                RowId = Guid.Parse("229993E7-B1B6-4E82-B329-F558B300D3C5"),
                Id = 41,
                Name = "Add Questionnaire Builder",
                Description = "Permission to add records in Questionnaire Builder",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 41,
                NavigationId = 11,
                UserActionId = 1
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("306A38EB-D934-4122-82DD-7DEDD0A358A8"),
                Id = 42,
                Name = "View Questionnaire Builder",
                Description = "Permission to view records in Questionnaire Builder",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 42,
                NavigationId = 11,
                UserActionId = 2
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("A6F2B3C4-1D2E-4F5A-9B6C-7D8E9F0A1B2C"),
                Id = 43,
                Name = "Edit Questionnaire Builder",
                Description = "Permission to edit records in Questionnaire Builder",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 43,
                NavigationId = 11,
                UserActionId = 3
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("C6629BAC-6676-447F-AB79-F9872AC99853"),
                Id = 44,
                Name = "Delete Questionnaire Builder",
                Description = "Permission to delete records in Questionnaire Builder",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 44,
                NavigationId = 11,
                UserActionId = 4
            },

            // ---------------- Insights Template (Navigation Id: 12) ----------------
            new NavigationUserAction
            {
                RowId = Guid.Parse("865B29AF-DED5-4FBA-8523-7D9FA8866606"),
                Id = 45,
                Name = "Add Insights Template",
                Description = "Permission to add records in Insights Template",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 45,
                NavigationId = 12,
                UserActionId = 1
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("95CF9394-95CB-4E80-98E2-299C9A3C8446"),
                Id = 46,
                Name = "View Insights Template",
                Description = "Permission to view records in Insights Template",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 46,
                NavigationId = 12,
                UserActionId = 2
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("D422ABC9-9C60-427E-B1F2-0EED8C7E950C"),
                Id = 47,
                Name = "Edit Insights Template",
                Description = "Permission to edit records in Insights Template",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 47,
                NavigationId = 12,
                UserActionId = 3
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("61A8C922-E67F-40D1-AF84-D128AA01326A"),
                Id = 48,
                Name = "Delete Insights Template",
                Description = "Permission to delete records in Insights Template",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 48,
                NavigationId = 12,
                UserActionId = 4
            },

            // ---------------- Users (Navigation Id: 13) ----------------
            new NavigationUserAction
            {
                RowId = Guid.Parse("118AC777-04FC-4259-ADE8-D8C93B573ED5"),
                Id = 49,
                Name = "Add Users",
                Description = "Permission to add records in Users",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 49,
                NavigationId = 13,
                UserActionId = 1
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("11B5FF82-DACC-4A70-95A0-426DAE4BFEC9"),
                Id = 50,
                Name = "View Users",
                Description = "Permission to view records in Users",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 50,
                NavigationId = 13,
                UserActionId = 2
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("9FB3E1E1-AEF1-4542-80F0-D4ADDCF50D80"),
                Id = 51,
                Name = "Edit Users",
                Description = "Permission to edit records in Users",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 51,
                NavigationId = 13,
                UserActionId = 3
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("2C61191E-9761-4854-9B4F-20B14C22A301"),
                Id = 52,
                Name = "Delete Users",
                Description = "Permission to delete records in Users",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 52,
                NavigationId = 13,
                UserActionId = 4
            },

            // ---------------- License (Navigation Id: 14) ----------------
            new NavigationUserAction
            {
                RowId = Guid.Parse("AB4D9C98-D778-4117-97AC-93B8EF8A5CED"),
                Id = 53,
                Name = "Add License",
                Description = "Permission to add records in License",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 53,
                NavigationId = 14,
                UserActionId = 1
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("298AFCFD-8A3A-4B08-AA39-95ED1F993519"),
                Id = 54,
                Name = "View License",
                Description = "Permission to view records in License",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 54,
                NavigationId = 14,
                UserActionId = 2
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("F4DAA5DA-E621-4397-8DAB-CCAD49084C20"),
                Id = 55,
                Name = "Edit License",
                Description = "Permission to edit records in License",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 55,
                NavigationId = 14,
                UserActionId = 3
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("5E79E6B7-7C53-445C-B71E-08C58B5CDA19"),
                Id = 56,
                Name = "Delete License",
                Description = "Permission to delete records in License",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 56,
                NavigationId = 14,
                UserActionId = 4
            },

            // ---------------- All Projects (Navigation Id: 15) ----------------
            new NavigationUserAction
            {
                RowId = Guid.Parse("E6983FB7-AFB4-4067-85E4-B0E41E15EBA8"),
                Id = 57,
                Name = "Add All Projects",
                Description = "Permission to add records in All Projects",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 57,
                NavigationId = 15,
                UserActionId = 1
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("6AB8F6D6-4EF8-48A8-AFD3-52F0D879A8AB"),
                Id = 58,
                Name = "View All Projects",
                Description = "Permission to view records in All Projects",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 58,
                NavigationId = 15,
                UserActionId = 2
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("39B22F9D-5B42-4C92-875E-7A1FDCB339BC"),
                Id = 59,
                Name = "Edit All Projects",
                Description = "Permission to edit records in All Projects",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 59,
                NavigationId = 15,
                UserActionId = 3
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("B7939113-43C6-4466-B455-C2A3A4BE8049"),
                Id = 60,
                Name = "Delete All Projects",
                Description = "Permission to delete records in All Projects",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 60,
                NavigationId = 15,
                UserActionId = 4
            },

            // ---------------- Summary (Navigation Id: 16) ----------------
            new NavigationUserAction
            {
                RowId = Guid.Parse("065A459E-6E5B-42A3-AD70-0478ACEC5280"),
                Id = 61,
                Name = "Add Summary",
                Description = "Permission to add records in Summary",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 61,
                NavigationId = 16,
                UserActionId = 1
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("A4893142-A427-490B-B77F-8A1E76DB5C72"),
                Id = 62,
                Name = "View Summary",
                Description = "Permission to view records in Summary",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 62,
                NavigationId = 16,
                UserActionId = 2
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("A22B8956-34E0-4AE1-8A70-FB382BEB9591"),
                Id = 63,
                Name = "Edit Summary",
                Description = "Permission to edit records in Summary",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 63,
                NavigationId = 16,
                UserActionId = 3
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("81CA5B60-3A13-4099-BFE3-CF8535FAFE9D"),
                Id = 64,
                Name = "Delete Summary",
                Description = "Permission to delete records in Summary",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 64,
                NavigationId = 16,
                UserActionId = 4
            },

            // ---------------- Workflow Set Up (Navigation Id: 17) ----------------
            new NavigationUserAction
            {
                RowId = Guid.Parse("F40450C1-D4F4-4C5E-9117-5ED26A42CC44"),
                Id = 65,
                Name = "Add Workflow Set Up",
                Description = "Permission to add records in Workflow Set Up",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 65,
                NavigationId = 17,
                UserActionId = 1
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("23616A4A-6907-4FC2-8AA5-49FED1D7ADBB"),
                Id = 66,
                Name = "View Workflow Set Up",
                Description = "Permission to view records in Workflow Set Up",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 66,
                NavigationId = 17,
                UserActionId = 2
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("2A4715C1-54D0-49D2-9F34-D34BFA99BF00"),
                Id = 67,
                Name = "Edit Workflow Set Up",
                Description = "Permission to edit records in Workflow Set Up",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 67,
                NavigationId = 17,
                UserActionId = 3
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("A00822E5-2E3D-41B1-9F22-66402B7933C3"),
                Id = 68,
                Name = "Delete Workflow Set Up",
                Description = "Permission to delete records in Workflow Set Up",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 68,
                NavigationId = 17,
                UserActionId = 4
            },

            // ---------------- Insights (Navigation Id: 18) ----------------
            new NavigationUserAction
            {
                RowId = Guid.Parse("AC4BDDFE-17C3-478F-B1E6-AF3201C52755"),
                Id = 69,
                Name = "Add Insights",
                Description = "Permission to add records in Insights",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 69,
                NavigationId = 18,
                UserActionId = 1
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("4499788E-714D-4A9A-97E1-BD86897CA70C"),
                Id = 70,
                Name = "View Insights",
                Description = "Permission to view records in Insights",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 70,
                NavigationId = 18,
                UserActionId = 2
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("8D9426ED-1FF7-42A4-BEC9-4B95A2C8AE04"),
                Id = 71,
                Name = "Edit Insights",
                Description = "Permission to edit records in Insights",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 71,
                NavigationId = 18,
                UserActionId = 3
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("208D0249-B8A8-420E-8D1B-A81547B426EC"),
                Id = 72,
                Name = "Delete Insights",
                Description = "Permission to delete records in Insights",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 72,
                NavigationId = 18,
                UserActionId = 4
            },

            // ---------------- Alerts Dashboard (Navigation Id: 19) ----------------
            new NavigationUserAction
            {
                RowId = Guid.Parse("C33D157B-4CC2-4C14-8F1A-5A6A934FEFD3"),
                Id = 73,
                Name = "Add Alerts Dashboard",
                Description = "Permission to add records in Alerts Dashboard",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 73,
                NavigationId = 19,
                UserActionId = 1
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("BE2825C6-27B8-411B-A5DD-609C3AC196D1"),
                Id = 74,
                Name = "View Alerts Dashboard",
                Description = "Permission to view records in Alerts Dashboard",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 74,
                NavigationId = 19,
                UserActionId = 2
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("7B8F3E1D-9A6C-4E2F-A123-9B8C7D6E5F12"),
                Id = 75,
                Name = "Edit Alerts Dashboard",
                Description = "Permission to edit records in Alerts Dashboard",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 75,
                NavigationId = 19,
                UserActionId = 3
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("22EFC3E7-F7AE-4DA9-B4B9-0FE238AD0AA6"),
                Id = 76,
                Name = "Delete Alerts Dashboard",
                Description = "Permission to delete records in Alerts Dashboard",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 76,
                NavigationId = 19,
                UserActionId = 4
            },

            // ---------------- Alerts (Navigation Id: 20) ----------------
            new NavigationUserAction
            {
                RowId = Guid.Parse("6F5EA829-342C-4606-B255-564689598EED"),
                Id = 77,
                Name = "Add Alerts",
                Description = "Permission to add records in Alerts",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 77,
                NavigationId = 20,
                UserActionId = 1
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("CC1ACC7A-9EA4-4CCB-A6B5-A0698066AAD6"),
                Id = 78,
                Name = "View Alerts",
                Description = "Permission to view records in Alerts",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 78,
                NavigationId = 20,
                UserActionId = 2
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("47032948-6106-43F4-80F2-8292CD6DFB2E"),
                Id = 79,
                Name = "Edit Alerts",
                Description = "Permission to edit records in Alerts",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 79,
                NavigationId = 20,
                UserActionId = 3
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("5597EED1-2FFB-47C4-9899-43365A5EDB0C"),
                Id = 80,
                Name = "Delete Alerts",
                Description = "Permission to delete records in Alerts",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 80,
                NavigationId = 20,
                UserActionId = 4
            },

            // ---------------- Users (Navigation Id: 21) ----------------
            new NavigationUserAction
            {
                RowId = Guid.Parse("2597F81A-E3FB-45F4-9DAB-5EAFCB157159"),
                Id = 81,
                Name = "Add Users",
                Description = "Permission to add records in Users",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 81,
                NavigationId = 21,
                UserActionId = 1
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("C91E0B17-050C-4F46-BA10-BA2F26C74273"),
                Id = 82,
                Name = "View Users",
                Description = "Permission to view records in Users",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 82,
                NavigationId = 21,
                UserActionId = 2
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("442C403C-B96F-4BCB-8D17-527477D5D4F8"),
                Id = 83,
                Name = "Edit Users",
                Description = "Permission to edit records in Users",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 83,
                NavigationId = 21,
                UserActionId = 3
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("35EDBC80-7693-4BB1-A282-04ECFA9CC547"),
                Id = 84,
                Name = "Delete Users",
                Description = "Permission to delete records in Users",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 84,
                NavigationId = 21,
                UserActionId = 4
            },

            // ----------------  Roles (Navigation Id: 22) ----------------
            new NavigationUserAction
            {
                RowId = Guid.Parse("4A3BD662-E26C-4E8D-A6AC-73F065B004B4"),
                Id = 85,
                Name = "Add Roles",
                Description = "Permission to add records in  Roles",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 85,
                NavigationId = 22,
                UserActionId = 1
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("73FCA9C1-C4A1-4480-9E99-E39D9E17CCE8"),
                Id = 86,
                Name = "View  Roles",
                Description = "Permission to view records in  Roles",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 86,
                NavigationId = 22,
                UserActionId = 2
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("0C816688-2A7C-478D-97DD-4D245573F742"),
                Id = 87,
                Name = "Edit  Roles",
                Description = "Permission to edit records in  Roles",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 87,
                NavigationId = 22,
                UserActionId = 3
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("2838E91A-1A0F-4EF6-80F9-856936FC4970"),
                Id = 88,
                Name = "Delete  Roles",
                Description = "Permission to delete records in  Roles",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 88,
                NavigationId = 22,
                UserActionId = 4
            },

            // ---------------- Visualisations (Navigation Id: 23) ----------------
            new NavigationUserAction
            {
                RowId = Guid.Parse("CCCA73E9-E402-42C6-B0A0-B584B4732E73"),
                Id = 89,
                Name = "Add Visualisations",
                Description = "Permission to add records in Visualisations",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 89,
                NavigationId = 23,
                UserActionId = 1
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("57A0C0A7-1332-442C-9D96-57B948F6EC2F"),
                Id = 90,
                Name = "View Visualisations",
                Description = "Permission to view records in Visualisations",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 90,
                NavigationId = 23,
                UserActionId = 2
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("BF9FC854-ACFA-4D78-A337-8A7AC65238DB"),
                Id = 91,
                Name = "Edit Visualisations",
                Description = "Permission to edit records in Visualisations",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 91,
                NavigationId = 23,
                UserActionId = 3
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("6B28C831-460F-46EC-B59D-A735F99D12A1"),
                Id = 92,
                Name = "Delete Visualisations",
                Description = "Permission to delete records in Visualisations",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 92,
                NavigationId = 23,
                UserActionId = 4
            },

            // ---------------- Entity View (Navigation Id: 24) ----------------
            new NavigationUserAction
            {
                RowId = Guid.Parse("AC7BBEBD-D19C-4774-BE18-673753980D47"),
                Id = 93,
                Name = "Add Entity View",
                Description = "Permission to add records in Entity View",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 93,
                NavigationId = 24,
                UserActionId = 1
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("05CD425D-6EC8-45B8-ACDC-16F0AF301487"),
                Id = 94,
                Name = "View Entity View",
                Description = "Permission to view records in Entity View",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 94,
                NavigationId = 24,
                UserActionId = 2
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("C8B72FC2-A60A-4FFB-98B4-6DFA38C938BB"),
                Id = 95,
                Name = "Edit Entity View",
                Description = "Permission to edit records in Entity View",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 95,
                NavigationId = 24,
                UserActionId = 3
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("B0B6DECB-B0ED-4997-9A1A-56619B9D4448"),
                Id = 96,
                Name = "Delete Entity View",
                Description = "Permission to delete records in Entity View",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 96,
                NavigationId = 24,
                UserActionId = 4
            },

            // ---------------- Transaction View (Navigation Id: 25) ----------------
            new NavigationUserAction
            {
                RowId = Guid.Parse("0B4CDCBF-C3BB-4D01-832E-B0B1B0141BA7"),
                Id = 97,
                Name = "Add Transaction View",
                Description = "Permission to add records in Transaction View",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 97,
                NavigationId = 25,
                UserActionId = 1
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("A37C339E-04EF-4A44-B260-64FDA7DB2816"),
                Id = 98,
                Name = "View Transaction View",
                Description = "Permission to view records in Transaction View",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 98,
                NavigationId = 25,
                UserActionId = 2
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("D4EAF2B2-1F1E-4732-870B-002C3DD32A23"),
                Id = 99,
                Name = "Edit Transaction View",
                Description = "Permission to edit records in Transaction View",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 99,
                NavigationId = 25,
                UserActionId = 3
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("CF00876B-DE71-4D6B-8119-0A5558520F14"),
                Id = 100,
                Name = "Delete Transaction View",
                Description = "Permission to delete records in Transaction View",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 100,
                NavigationId = 25,
                UserActionId = 4
            },

            // ---------------- Scenario Manager (Navigation Id: 26) ----------------
            new NavigationUserAction
            {
                RowId = Guid.Parse("1CA1A35A-8983-4F4C-9C29-F1E43FF6E089"),
                Id = 101,
                Name = "Add Scenario Manager",
                Description = "Permission to add records in Scenario Manager",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 101,
                NavigationId = 26,
                UserActionId = 1
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("C09D8D71-5BDC-4241-86AE-035376539933"),
                Id = 102,
                Name = "View Scenario Manager",
                Description = "Permission to view records in Scenario Manager",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 102,
                NavigationId = 26,
                UserActionId = 2
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("128C3350-F8CA-4605-B764-4FB36C5AE72E"),
                Id = 103,
                Name = "Edit Scenario Manager",
                Description = "Permission to edit records in Scenario Manager",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 103,
                NavigationId = 26,
                UserActionId = 3
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("3C50CD3D-B605-4F1A-8E50-CB7700595682"),
                Id = 104,
                Name = "Delete Scenario Manager",
                Description = "Permission to delete records in Scenario Manager",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 104,
                NavigationId = 26,
                UserActionId = 4
            },

            // ---------------- Similar Transaction (Navigation Id: 27) ----------------
            new NavigationUserAction
            {
                RowId = Guid.Parse("51F22075-6F55-4160-A29B-BC41098FA8DB"),
                Id = 105,
                Name = "Add Similar Transaction",
                Description = "Permission to add records in Similar Transaction",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 105,
                NavigationId = 27,
                UserActionId = 1
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("51ADCC39-7F43-4116-8058-27FEBCD7CA9E"),
                Id = 106,
                Name = "View Similar Transaction",
                Description = "Permission to view records in Similar Transaction",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 106,
                NavigationId = 27,
                UserActionId = 2
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("3E0FF2CA-5712-4320-9DBE-D1967E80605F"),
                Id = 107,
                Name = "Edit Similar Transaction",
                Description = "Permission to edit records in Similar Transaction",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 107,
                NavigationId = 27,
                UserActionId = 3
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("058DA619-A8B9-4C38-A991-8F21EFC15E81"),
                Id = 108,
                Name = "Delete Similar Transaction",
                Description = "Permission to delete records in Similar Transaction",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 108,
                NavigationId = 27,
                UserActionId = 4
            },
            // ---------------- Project Dashboard (Navigation Id: 28) ----------------
            new NavigationUserAction
            {
                RowId = Guid.Parse("C88F0DAA-B4C0-48E6-86F4-471201A00D74"),
                Id = 109,
                Name = "Add Project Dashboard",
                Description = "Permission to add records in Project Dashboard",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 109,
                NavigationId = 28,
                UserActionId = 1
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("9BDCD01F-68FF-40A2-919F-49AF88ACB1A6"),
                Id = 110,
                Name = "View Project Dashboard",
                Description = "Permission to view records in Project Dashboard",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 110,
                NavigationId = 28,
                UserActionId = 2
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("316E063B-FEE2-4974-BA81-59AE245ACE1D"),
                Id = 111,
                Name = "Edit Project Dashboard",
                Description = "Permission to edit records in Project Dashboard",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 111,
                NavigationId = 28,
                UserActionId = 3
            },
            new NavigationUserAction
            {
                RowId = Guid.Parse("7920B5B9-A1C8-45F8-869B-D41F3D245BB8"),
                Id = 112,
                Name = "Delete Project Dashboard",
                Description = "Permission to delete records in Project Dashboard",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 112,
                NavigationId = 28,
                UserActionId = 4
            }
        );
    }
}
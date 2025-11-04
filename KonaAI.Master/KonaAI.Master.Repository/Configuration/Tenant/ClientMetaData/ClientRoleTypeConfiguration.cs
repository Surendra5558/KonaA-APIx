using KonaAI.Master.Repository.Common.Extensions;
using KonaAI.Master.Repository.Domain.Master.MetaData;
using KonaAI.Master.Repository.Domain.Tenant.ClientMetaData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KonaAI.Master.Repository.Configuration.Tenant.ClientMetaData;

/// <summary>
/// Configures the <see cref="ClientRoleType"/> entity, applying standard client metadata mapping.
/// </summary>
/// <remarks>
/// Applies the shared client metadata conventions via <c>BaseClientMetaDataDomain</c>, mapping to the
/// "ClientMetadata" schema and the "ClientRoleType" table. This typically includes keys,
/// common columns (name, description, order), auditing, soft-delete, and indexes as defined by the shared extension.
/// </remarks>
/// <seealso cref="ClientRoleType"/>
/// <seealso cref="IEntityTypeConfiguration{ClientRoleType}"/>
public class ClientRoleTypeConfiguration : IEntityTypeConfiguration<ClientRoleType>
{
    public void Configure(EntityTypeBuilder<ClientRoleType> builder)
    {
        builder.BaseClientMetaDataConfiguration("ClientRoleType");

        builder.HasData
        (
            new ClientRoleType
            {
                RowId = Guid.Parse("20A8F44A-9955-480D-A0AA-1B35C13351AE"),
                Id = 1,
                ClientId = 1,
                RoleTypeId = 1,
                Name = "Guest Role",
                Description = "Guest Role Description",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 1,
                IsActive = true,
                IsDeleted = false
            },
            new ClientRoleType
            {
                RowId = Guid.Parse("B7F8111E-4A9B-4F13-8DB9-8C725FE7F0F5"),
                Id = 2,
                ClientId = 1,
                RoleTypeId = 2,
                Name = "Instance Admin",
                Description = "Instance Admin Description",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 2,
                IsActive = true,
                IsDeleted = false
            },
            new ClientRoleType
            {
                RowId = Guid.Parse("D5524230-0500-4F9C-9DFB-9BA3930B7E7F"),
                Id = 3,
                ClientId = 1,
                RoleTypeId = 3,
                Name = "Compliance Manager",
                Description = "Compliance Manager Description",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 3,
                IsActive = true,
                IsDeleted = false
            },
            new ClientRoleType
            {
                RowId = Guid.Parse("B2B7D900-0D9D-4A63-9CC8-394CC5D7C750"),
                Id = 4,
                ClientId = 1,
                RoleTypeId = 4,
                Name = "Data Manager",
                Description = "Data Manager Description",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 4,
                IsActive = true,
                IsDeleted = false
            },
            new ClientRoleType
            {
                RowId = Guid.Parse("097292CD-5B99-43BE-880F-4E12CD6ED6BA"),
                Id = 5,
                ClientId = 1,
                RoleTypeId = 5,
                Name = "Investigator",
                Description = "Investigator Description",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 5,
                IsActive = true,
                IsDeleted = false
            },
             new ClientRoleType
             {
                 RowId = Guid.Parse("7F4C9C2D-2A8B-4D6E-9F32-1B7A4E9B5D12"),
                 Id = 6,
                 ClientId = 1,
                 RoleTypeId = 6,
                 Name = "L1 User",
                 Description = "L1 User",
                 CreatedBy = "Default User",
                 CreatedById = 1,
                 ModifiedBy = "Default User",
                 ModifiedById = 1,
                 OrderBy = 6,
                 IsActive = true,
                 IsDeleted = false
             },
            new ClientRoleType
            {
                RowId = Guid.Parse("C1E7A3B6-8F42-4CC3-9D8A-0F54B732A6E9"),
                Id = 7,
                ClientId = 1,
                RoleTypeId = 7,
                Name = "L2 User",
                Description = "L2 User",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 7,
                IsActive = true,
                IsDeleted = false
            }
        );
    }
}
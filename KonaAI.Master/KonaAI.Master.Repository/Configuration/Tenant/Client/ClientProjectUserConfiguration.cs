using KonaAI.Master.Repository.Common.Extensions;
using KonaAI.Master.Repository.Domain.Tenant.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace KonaAI.Master.Repository.Configuration.Tenant.Client;
/// <summary>
/// Configures the <see cref="ClientProjectUser"/> entity, applying standard client domain mapping conventions.
/// </summary>
/// <remarks>
/// Applies the shared client domain conventions via <c>BaseClientConfiguration</c>, mapping to the
/// appropriate schema and the "ClientProjectUser" table. This includes keys,
/// common columns, auditing, soft-delete, and indexes as defined by the shared extension.
/// Defines the relationship between a client user and a project, including their assigned role.
/// </remarks>
/// <seealso cref="ClientProjectUser"/>
/// <seealso cref="IEntityTypeConfiguration{ClientProjectUser}"/>
public class ClientProjectUserConfiguration : IEntityTypeConfiguration<ClientProjectUser>
{
    /// <summary>
    /// Configures the <see cref="ClientProjectUser"/> entity's schema and property constraints
    /// using the provided <see cref="EntityTypeBuilder{ClientProjectUser}"/>.
    /// </summary>
    /// <param name="builder">The builder used to configure the <see cref="ClientProjectUser"/> entity.</param>
    public void Configure(EntityTypeBuilder<ClientProjectUser> builder)
    {
        builder.BaseClientConfiguration("ClientProjectUser");
        builder.Property(e => e.ProjectId)
            .IsRequired();
        builder.Property(e => e.UserId)
            .IsRequired();
        builder.Property(e => e.RoleId)
            .IsRequired();
        builder.HasIndex(x => new { x.ClientId, x.ProjectId, x.UserId }).IsUnique();

        builder.HasData(
            new ClientProjectUser
            {
                RowId = Guid.Parse("823D1509-C1A7-48D0-92DE-CF59B3B3C9BD"),
                Id = 1,
                ClientId = 1,
                ProjectId = 1,
                UserId = 3,
                RoleId = 6, // L1 Investigator
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                CreatedOn = DateTime.UtcNow,
                ModifiedOn = DateTime.UtcNow,
                IsActive = true,
                IsDeleted = false
            },
            new ClientProjectUser
            {
                RowId = Guid.Parse("8CC22D1C-BEED-4072-8D1F-19974783CACC"),
                Id = 2,
                ClientId = 1,
                ProjectId = 1,
                UserId = 4,
                RoleId = 7, // L2 Investigator
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                CreatedOn = DateTime.UtcNow,
                ModifiedOn = DateTime.UtcNow,
                IsActive = true,
                IsDeleted = false
            },
            new ClientProjectUser
            {
                RowId = Guid.Parse("D32AAD6F-97F0-42B2-BAC0-C2EAD3100B9B"),
                Id = 3,
                ClientId = 1,
                ProjectId = 2,
                UserId = 5,
                RoleId = 4,
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                CreatedOn = DateTime.UtcNow,
                ModifiedOn = DateTime.UtcNow,
                IsActive = true,
                IsDeleted = false
            },
            new ClientProjectUser
            {
                RowId = Guid.Parse("1E0270A9-28CB-45D2-9A25-A83204CA9F8F"),
                Id = 4,
                ClientId = 1,
                ProjectId = 2,
                UserId = 6,
                RoleId = 5,
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
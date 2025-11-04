using KonaAI.Master.Repository.Common.Extensions;
using KonaAI.Master.Repository.Domain.Tenant.ClientUserMetaData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KonaAI.Master.Repository.Configuration.Tenant.ClientUserMetaData;

/// <summary>
/// Configures the <see cref="ClientProjectAuditResponsibility"/> entity, applying standard client metadata mapping.
/// </summary>
/// <remarks>
/// Applies the shared client metadata conventions via <c>BaseClientMetaDataConfiguration</c>, mapping to the
/// "ClientUserMetaData" schema and the "ClientProjectAuditResponsibility" table. This typically includes keys,
/// common columns (name, description, order), auditing, soft-delete, and indexes as defined by the shared extension.
/// </remarks>
/// <seealso cref="ClientProjectAuditResponsibility"/>
/// <seealso cref="IEntityTypeConfiguration{ClientProjectAuditResponsibility}"/>
public class ClientProjectAuditResponsibilityConfiguration : IEntityTypeConfiguration<ClientProjectAuditResponsibility>
{
    /// <summary>
    /// Configures the EF Core model for <see cref="ClientProjectAuditResponsibility"/>.
    /// </summary>
    /// <param name="builder">The entity type builder used to configure the entity.</param>
    /// <remarks>
    /// Delegates to <c>BaseClientMetaDataConfiguration</c> to apply standardized mapping for client-scoped metadata entities.
    /// </remarks>
    public void Configure(EntityTypeBuilder<ClientProjectAuditResponsibility> builder)
    {
        builder.BaseClientMetaDataConfiguration("ClientProjectAuditResponsibility", "ClientUserMetaData");

        // Seed client-specific audit responsibilities
        builder.HasData(
            new ClientProjectAuditResponsibility
            {
                RowId = Guid.Parse("71C94005-343A-4DD0-9298-B77A19CAE473"),
                Id = 1,
                ClientId = 1,
                ProjectAuditResponsibilityId = 2,
                Name = "Verification",
                Description = "Responsible for verifying accuracy of data",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 1,
                IsActive = true,
                IsDeleted = false
            },
            new ClientProjectAuditResponsibility
            {
                RowId = Guid.Parse("26D48899-0FA6-40E5-8E27-71043C7B33E5"),
                Id = 2,
                ClientId = 1,
                ProjectAuditResponsibilityId = 3,
                Name = "Approval",
                Description = "Responsible for reviewing and approving transactions",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 2,
                IsActive = true,
                IsDeleted = false
            }
        );
    }
}
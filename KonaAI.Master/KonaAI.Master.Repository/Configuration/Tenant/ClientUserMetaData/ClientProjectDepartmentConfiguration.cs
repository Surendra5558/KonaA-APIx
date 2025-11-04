using KonaAI.Master.Repository.Common.Extensions;
using KonaAI.Master.Repository.Domain.Tenant.ClientUserMetaData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KonaAI.Master.Repository.Configuration.Tenant.ClientUserMetaData;

/// <summary>
/// Configures the <see cref="ClientProjectDepartment"/> entity including base metadata mapping and seed data.
/// </summary>
public class ClientProjectDepartmentConfiguration : IEntityTypeConfiguration<ClientProjectDepartment>
{
    /// <summary>
    /// Applies configuration for the <see cref="ClientProjectDepartment"/> entity.
    /// Sets common metadata (via <see cref="EntityBaseConfigurationExtension.BaseMetaDataConfiguration"/>)
    /// and seeds initial Business Department.
    /// </summary>
    /// <param name="builder">The builder used to configure the <see cref="ClientProjectDepartment"/> entity.</param>
    public void Configure(EntityTypeBuilder<ClientProjectDepartment> builder)
    {
        builder.BaseClientMetaDataConfiguration("ClientProjectDepartment", "ClientUserMetaData");

        // Seed client-specific departments
        builder.HasData(
            new ClientProjectDepartment
            {
                RowId = Guid.Parse("F462CE2A-2777-4DB3-BEB0-4A33CD80D862"),
                Id = 1,
                ClientId = 1,
                ProjectDepartmentId = 1,
                Name = "Accounts Payable",
                Description = "Manages vendor payments and invoices",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                OrderBy = 1,
                IsActive = true,
                IsDeleted = false
            },
            new ClientProjectDepartment
            {
                RowId = Guid.Parse("997FE44E-2929-4606-A4F1-B5A3EB69BF59"),
                Id = 2,
                ClientId = 1,
                ProjectDepartmentId = 3,
                Name = "IT Support",
                Description = "Provides technical support and maintenance",
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
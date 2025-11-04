using KonaAI.Master.Repository.Common.Extensions;
using KonaAI.Master.Repository.Domain.Tenant.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KonaAI.Master.Repository.Configuration.Tenant.Client;

/// <summary>
/// Configures the entity mapping and property constraints for the <see cref="ClientUser"/> entity.
/// Sets up table schema, required fields, and client association.
/// </summary>
public class ClientUserConfiguration : IEntityTypeConfiguration<ClientUser>
{
    /// <summary>
    /// Configures the <see cref="ClientUser"/> entity's schema and property constraints
    /// using the provided <see cref="EntityTypeBuilder{ClientUser}"/>.
    /// </summary>
    /// <param name="builder">The builder used to configure the <see cref="ClientUser"/> entity.</param>
    public void Configure(EntityTypeBuilder<ClientUser> builder)
    {
        builder.BaseClientConfiguration("ClientUser");

        builder.Property(e => e.UserId)
            .IsRequired();

        builder.HasData
        (
            new ClientUser
            {
                RowId = Guid.Parse("6B753380-8443-4407-B3FA-C44532E81952"),
                Id = 1,
                ClientId = 1,
                UserId = 2,
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1
            },
            new ClientUser
            {
                RowId = Guid.Parse("30679EA5-CA17-4B18-9AB8-81137700DEA8"),
                Id = 2,
                ClientId = 1,
                UserId = 3,
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1
            },
            new ClientUser
            {
                RowId = Guid.Parse("1D20BF1A-1938-4318-A20D-2A249CABE95B"),
                Id = 3,
                ClientId = 2,
                UserId = 3,
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1
            },
            new ClientUser
            {
                RowId = Guid.Parse("92B388BF-E798-4376-BDC7-02AF33100BF1"),
                Id = 4,
                ClientId = 2,
                UserId = 10,
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1
            },
            new ClientUser
            {
                RowId = Guid.Parse("E77B00FB-A8E9-4D42-B685-2878BBECCB42"),
                Id = 5,
                ClientId = 1,
                UserId = 4,
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1
            },
            new ClientUser
            {
                RowId = Guid.Parse("FC7BF4CF-E535-4731-85CE-5A5567EEBF3E"),
                Id = 6,
                ClientId = 1,
                UserId = 5,
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1
            },
            new ClientUser
            {
                RowId = Guid.Parse("47EEE7FB-7AD5-4FE6-8209-63845B2C54F6"),
                Id = 7,
                ClientId = 1,
                UserId = 6,
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1
            },
            new ClientUser
            {
                RowId = Guid.Parse("E78E0C1A-5D63-48B0-B935-285AED0A4299"),
                Id = 8,
                ClientId = 1,
                UserId = 7,
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1
            },
            new ClientUser
            {
                RowId = Guid.Parse("19F23921-8EEA-43F6-8899-9B9E256261DA"),
                Id = 9,
                ClientId = 2,
                UserId = 8,
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1
            },
            new ClientUser
            {
                RowId = Guid.Parse("4DC3C13F-EAF5-4333-A4E2-9D8819DF5E36"),
                Id = 10,
                ClientId = 2,
                UserId = 9,
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1
            }
        );
    }
}
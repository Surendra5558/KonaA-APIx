using KonaAI.Master.Repository.Common.Extensions;
using KonaAI.Master.Repository.Domain.Master.App;
using KonaAI.Master.Repository.Domain.Master.MetaData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KonaAI.Master.Repository.Configuration.Master.MetaData;

/// <summary>
/// Configures the entity mapping and relationships for the <see cref="LogOnType"/> entity.
/// Sets up table schema, metadata properties, and the one-to-many relationship with <see cref="User"/> entities.
/// </summary>
public class LogOnTypeConfiguration : IEntityTypeConfiguration<LogOnType>
{
    /// <summary>
    /// Configures the <see cref="LogOnType"/> entity's schema, metadata property constraints, and relationships
    /// using the provided <see cref="EntityTypeBuilder{LogOnType}"/>.
    /// </summary>
    /// <param name="builder">The builder used to configure the <see cref="LogOnType"/> entity.</param>
    public void Configure(EntityTypeBuilder<LogOnType> builder)
    {
        builder.BaseMetaDataConfiguration("LogOnType");

        // Align the relationship with User.LogOnType navigation and FK property to avoid shadow FK (LogOnTypeId1)
        builder
            .HasMany(logOnType => logOnType.Users)
            .WithOne(user => user.LogOnType)
            .HasForeignKey(user => user.LogOnTypeId)
            .IsRequired();

        builder.HasData(
            new LogOnType
            {
                RowId = Guid.Parse("C181AA41-E246-45F1-99CE-D96BB6B5C58A"),
                Id = 1,
                Name = "System",
                Description = "System logon type",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                IsDefault = true,
                OrderBy = 1
            },
            new LogOnType
            {
                RowId = Guid.Parse("E9CB06C8-1F58-4E41-91B6-ADE117D2CC7F"),
                Id = 2,
                Name = "Client Credentials",
                Description = "Client Id with Client Secret",
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                IsDefault = true,
                OrderBy = 2
            },
            new LogOnType
            {
                RowId = Guid.Parse("71B65B2C-1667-4106-AB7E-BC728274F0E9"),
                Id = 3,
                Name = "Forms",
                Description = "Forms logon type",
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                IsDefault = true,
                OrderBy = 3
            },
            new LogOnType
            {
                RowId = Guid.Parse("C82F8251-F058-4DDF-82AE-601817388D4E"),
                Id = 4,
                Name = "SSO",
                Description = "Single sign logon type",
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                IsDefault = true,
                OrderBy = 4
            }
        );
    }
}
using KonaAI.Master.Repository.Common.Constants;
using KonaAI.Master.Repository.Common.Extensions;
using KonaAI.Master.Repository.Domain.Master.App;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KonaAI.Master.Repository.Configuration.Master.App;

/// <summary>
/// Configures the entity mapping and property constraints for the <see cref="Client"/> entity.
/// Sets up table schema, unique indexes, required fields, maximum lengths, and column types.
/// </summary>
public class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    /// <summary>
    /// Configures the <see cref="Client"/> entity's schema, property constraints, and indexes using the provided <see cref="EntityTypeBuilder{Client}"/>.
    /// </summary>
    /// <param name="builder">The builder used to configure the <see cref="Client"/> entity.</param>
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.BaseConfiguration("Client");

        builder.HasIndex(x => x.Name)
            .IsUnique();
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(DbColumnLength.NameEmail);
        builder.Property(x => x.DisplayName)
            .IsRequired()
            .HasMaxLength(DbColumnLength.NameEmail);
        builder.HasIndex(x => x.ClientCode)
            .IsUnique();
        builder.Property(x => x.ClientCode)
            .IsRequired();
        builder.Property(x => x.Description)
            .HasMaxLength(DbColumnLength.Description);
        builder.Property(x => x.ContactName)
            .HasMaxLength(DbColumnLength.NameEmail);
        builder.Property(x => x.Address)
            .HasMaxLength(DbColumnLength.Address);
        builder.Property(x => x.Email)
            .HasMaxLength(DbColumnLength.NameEmail);
        builder.Property(x => x.Phone)
            .HasMaxLength(DbColumnLength.PhoneNumber);
        builder.Property(x => x.CountryCode)
            .HasMaxLength(DbColumnLength.CountryCode);
        builder.Property(x => x.Logo)
            .HasMaxLength(1000000);

        builder.HasData(
            new Client
            {
                RowId = Guid.Parse("E0E0CB60-004C-486E-B0C5-8A2520DA7958"),
                Id = 1,
                ClientCode = "MSA00001",
                DisplayName = "DCube LLP",
                Name = "DCube LLP",
                Description = "DCube LLP is a leading provider of innovative solutions in the tech industry.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1
            },
            new Client
            {
                RowId = Guid.Parse("C246C128-BB74-4A9A-8FFD-BEE0B743899E"),
                Id = 2,
                ClientCode = "MSA00002",
                DisplayName = "Covasant Pvt Ltd",
                Name = "Covasant Pvt Ltd",
                Description = "Covasant Pvt Ltd is a leading provider of innovative solutions in the AI industry.",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1
            }
        );
    }
}
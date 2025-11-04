using KonaAI.Master.Repository.Common.Extensions;
using KonaAI.Master.Repository.Domain.Master.MetaData;
using KonaAI.Master.Repository.Domain.Tenant.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KonaAI.Master.Repository.Configuration.Tenant.Client;

/// <summary>
/// Configures the <see cref="ClientProjectModuleSourceType"/> entity.
/// </summary>
public class ClientProjectModuleSourceTypeConfiguration : IEntityTypeConfiguration<ClientProjectModuleSourceType>
{
    public void Configure(EntityTypeBuilder<ClientProjectModuleSourceType> builder)
    {
        builder.BaseClientConfiguration("ClientProjectModuleType");

        // Required FKs
        builder.Property(x => x.ClientProjectId).IsRequired();

        builder.Property(x => x.ModuleSourceTypeId).IsRequired();

        // The single authoritative relationship mapping (prevents shadow FK like ClientProjectId1)
        builder.HasOne(x => x.ClientProject)
               .WithMany(p => p.ClientProjectModuleSourceTypes)
               .HasForeignKey(x => x.ClientProjectId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<ModuleSourceType>()
               .WithMany()
               .HasForeignKey(x => x.ModuleSourceTypeId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.ClientId, x.ClientProjectId, x.ModuleSourceTypeId })
               .IsUnique();
    }
}
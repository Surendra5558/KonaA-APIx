using KonaAI.Master.Repository.Common.Constants;
using KonaAI.Master.Repository.Common.Extensions;
using KonaAI.Master.Repository.Domain.Tenant.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KonaAI.Master.Repository.Configuration.Tenant.Client;

/// <summary>
/// Configures the entity mapping and property constraints for the <see cref="ProjectScheduler"/> entity.
/// Sets up table schema, unique indexes, required fields, maximum lengths, and column types.
/// </summary>
public class ProjectSchedulerConfiguration : IEntityTypeConfiguration<ProjectScheduler>
{
    /// <summary>
    /// Configures the <see cref="ProjectScheduler"/> entity's schema, property constraints, and indexes using the provided <see cref="EntityTypeBuilder{ProjectScheduler}"/>.
    /// </summary>
    /// <param name="builder">The builder used to configure the <see cref="ProjectScheduler"/> entity.</param>
    public void Configure(EntityTypeBuilder<ProjectScheduler> builder)
    {
        builder.BaseConfiguration("ProjectScheduler");
        builder.HasIndex(x => x.ProjectSchedulerId)
            .IsUnique();
        builder.Property(x => x.ProjectName)
            .IsRequired()
            .HasMaxLength(DbColumnLength.NameEmail);
        builder.Property(x => x.ProjectId)
            .IsRequired();
        builder.Property(x => x.CreatedBy)
            .HasMaxLength(DbColumnLength.Description);
        builder.Property(x => x.ModifiedBy)
            .HasMaxLength(DbColumnLength.NameEmail);
        builder.Property(x => x.CreatedOn);
        builder.Property(x => x.ModifiedOn);
        builder.Property(x => x.UserName)
            .HasMaxLength(DbColumnLength.NameEmail);
        builder.Property(x => x.Password)
            .HasMaxLength(DbColumnLength.NameEmail);
        builder.Property(x => x.ConnectionString)
            .HasMaxLength(DbColumnLength.Description);
        builder.Property(x => x.DatabaseName)
            .HasMaxLength(DbColumnLength.NameEmail);
        builder.Property(x => x.ErrorMessage)
            .HasMaxLength(DbColumnLength.Description);
        builder.Property(x => x.EncryptedLicenseKey)
        .HasMaxLength(DbColumnLength.Description);
    }
}

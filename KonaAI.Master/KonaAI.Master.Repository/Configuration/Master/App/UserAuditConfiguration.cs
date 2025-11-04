using KonaAI.Master.Repository.Common.Constants;
using KonaAI.Master.Repository.Common.Extensions;
using KonaAI.Master.Repository.Common.Model;
using KonaAI.Master.Repository.Domain.Master.App;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;

namespace KonaAI.Master.Repository.Configuration.Master.App;

/// <summary>
/// Configures the entity mapping, constraints, relationships, and seed data for the <see cref="UserAudit"/> entity.
/// Sets up table schema, metadata properties, foreign keys, navigation properties, and indexes.
/// </summary>
public class UserAuditConfiguration : IEntityTypeConfiguration<UserAudit>
{
    /// <summary>
    /// Configures the <see cref="UserAudit"/> entity's schema, metadata property constraints,
    /// relationships, and indexes using the provided <see cref="EntityTypeBuilder{UserAudit}"/>.
    /// </summary>
    /// <param name="builder">The builder used to configure the <see cref="UserAudit"/> entity.</param>
    public void Configure(EntityTypeBuilder<UserAudit> builder)
    {
        builder.BaseConfiguration("UserAudit");

        //builder.ToTable(x => x.IsMemoryOptimized());

        // Required scalar properties
        builder.Property(x => x.UserRowId).IsRequired();
        builder.Property(x => x.UserId).IsRequired();

        builder.Property(x => x.FirstName).IsRequired()
            .HasMaxLength(DbColumnLength.NameEmail);

        builder.Property(x => x.LastName).IsRequired()
            .HasMaxLength(DbColumnLength.NameEmail);

        builder.Property(x => x.Email).IsRequired()
            .HasMaxLength(DbColumnLength.NameEmail);

        builder.Property(x => x.RoleId).IsRequired();
        builder.Property(x => x.RoleRowId).IsRequired();

        builder.Property(x => x.RoleName).IsRequired()
            .HasMaxLength(DbColumnLength.NameEmail);

        // Configure list properties as JSON using value converters and comparers
        // RoleNavigation: List<UserPermission> -> JSON string
        var roleNavigationConverter = new ValueConverter<List<UserPermission>, string>(
            v => JsonSerializer.Serialize(v ?? new List<UserPermission>(), (JsonSerializerOptions?)null),
            v => string.IsNullOrWhiteSpace(v)
                ? new List<UserPermission>()
                : (JsonSerializer.Deserialize<List<UserPermission>>(v, (JsonSerializerOptions?)null) ??
                   new List<UserPermission>()));

        // Use JSON string for equality to compare structural changes inside complex items
        var roleNavigationComparer = new ValueComparer<List<UserPermission>>(
            (l1, l2) => (l1 == null && l2 == null) ||
                        (l1 != null && l2 != null &&
                         JsonSerializer.Serialize(l1, (JsonSerializerOptions?)null) ==
                         JsonSerializer.Serialize(l2, (JsonSerializerOptions?)null)),
            l => JsonSerializer.Serialize(l, (JsonSerializerOptions?)null).GetHashCode(),
            l => l.ToList());

        builder.Property(x => x.RoleNavigation)
            .HasColumnType("nvarchar(max)")
            .HasConversion(roleNavigationConverter)
            .Metadata.SetValueComparer(roleNavigationComparer);

        // ProjectAccess: List<UserProject> -> JSON string
        var projectAccessConverter = new ValueConverter<List<UserProject>, string>(
            v => JsonSerializer.Serialize(v ?? new List<UserProject>(), (JsonSerializerOptions?)null),
            v => string.IsNullOrWhiteSpace(v)
                ? new List<UserProject>()
                : (JsonSerializer.Deserialize<List<UserProject>>(v, (JsonSerializerOptions?)null) ??
                   new List<UserProject>()));

        var projectAccessComparer = new ValueComparer<List<UserProject>>(
            (l1, l2) => (l1 == null && l2 == null) ||
                        (l1 != null && l2 != null &&
                         JsonSerializer.Serialize(l1, (JsonSerializerOptions?)null) ==
                         JsonSerializer.Serialize(l2, (JsonSerializerOptions?)null)),
            l => JsonSerializer.Serialize(l, (JsonSerializerOptions?)null).GetHashCode(),
            l => l.ToList());

        builder.Property(x => x.ProjectAccess)
            .HasColumnType("nvarchar(max)")
            .HasConversion(projectAccessConverter)
            .Metadata.SetValueComparer(projectAccessComparer);

        builder.Property(x => x.RefreshToken).IsRequired()
            .HasMaxLength(DbColumnLength.NameEmail);

        builder.Property(x => x.TokenCreatedDate).IsRequired();
        builder.Property(x => x.TokenExpiredDate).IsRequired();

        // Helpful index for audit lookups
        builder.HasIndex(x => new { x.UserId, x.RoleId });
    }
}
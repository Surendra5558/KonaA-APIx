using KonaAI.Master.Repository.Common.Constants;
using KonaAI.Master.Repository.Common.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KonaAI.Master.Repository.Common.Extensions;

/// <summary>
/// Provides extension methods for configuring entity types with default and client-specific properties in Entity Framework Core.
/// </summary>
public static class EntityBaseConfigurationExtension
{
    /// <summary>
    /// Configures the base properties and table mapping for an entity derived from <see cref="BaseDomain"/>.
    /// </summary>
    /// <typeparam name="T">The entity type derived from <see cref="BaseDomain"/>.</typeparam>
    /// <param name="builder">The entity type builder.</param>
    /// <param name="tableName">The name of the table.</param>
    /// <param name="schemaName">The name of the schema. Defaults to "Authentication".</param>
    /// <param name="ignoreDeleted"></param>
    public static void BaseConfiguration<T>(this EntityTypeBuilder<T> builder, string tableName,
        string schemaName = "App", bool ignoreDeleted = false)
        where T : BaseDomain
    {
        SetBuilder(builder, tableName, schemaName, ignoreDeleted);
    }

    /// <summary>
    /// Configures the base metadata properties and table mapping for an entity derived from <see cref="BaseMetaDataDomain"/>.
    /// </summary>
    /// <typeparam name="T">The entity type derived from <see cref="BaseMetaDataDomain"/>.</typeparam>
    /// <param name="builder">The entity type builder.</param>
    /// <param name="tableName">The name of the table.</param>
    /// <param name="schemaName">The name of the schema. Defaults to "MetaData".</param>
    /// <param name="ignoreDeleted">Indicates whether to apply the deleted filter. Defaults to <c>true</c>.</param>
    public static void BaseMetaDataConfiguration<T>(this EntityTypeBuilder<T> builder, string tableName,
        string schemaName = "MetaData", bool ignoreDeleted = false)
        where T : BaseMetaDataDomain
    {
        SetBuilder(builder, tableName, schemaName);
        builder.Property(e => e.Name).HasMaxLength(DbColumnLength.NameEmail).HasColumnOrder(11).IsRequired();
        builder.Property(e => e.Description).HasMaxLength(DbColumnLength.Description).HasColumnOrder(12);
        builder.Property(e => e.IsDefault).HasColumnOrder(13).IsRequired();
        builder.Property(e => e.OrderBy).HasColumnOrder(14).IsRequired();
    }

    /// <summary>
    /// Configures the common properties and table mapping for an entity, including audit and soft-delete fields.
    /// </summary>
    /// <typeparam name="T">The entity type derived from <see cref="BaseDomain"/>.</typeparam>
    /// <param name="builder">The entity type builder.</param>
    /// <param name="tableName">The name of the table.</param>
    /// <param name="schemaName">The name of the schema.</param>
    /// <param name="ignoreDeleted">Indicates whether to apply the deleted filter. Defaults to <c>true</c>.</param>
    private static void SetBuilder<T>(EntityTypeBuilder<T> builder, string tableName,
        string schemaName, bool ignoreDeleted = false) where T : BaseDomain
    {
        builder.ToTable(tableName, schemaName);
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnOrder(1).IsRequired();
        builder.Property(e => e.RowId).HasColumnOrder(2).IsRequired();
        builder.Property(e => e.CreatedOn).HasColumnOrder(3).IsRequired();
        builder.Property(e => e.CreatedById).HasColumnOrder(4).IsRequired();
        builder.Property(e => e.CreatedBy).HasColumnOrder(5).IsRequired();
        builder.Property(e => e.ModifiedOn).HasColumnOrder(6).IsRequired();
        builder.Property(e => e.ModifiedById).HasColumnOrder(7).IsRequired();
        builder.Property(e => e.ModifiedBy).HasColumnOrder(8).IsRequired();
        builder.Property(e => e.IsActive).HasColumnOrder(9).IsRequired();
        builder.Property(e => e.IsDeleted).HasColumnOrder(10).IsRequired();

        if (!ignoreDeleted)
            builder.HasQueryFilter(e => e.IsDeleted == false);
    }
}
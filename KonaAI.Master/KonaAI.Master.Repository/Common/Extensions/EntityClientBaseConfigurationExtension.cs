using KonaAI.Master.Repository.Common.Constants;
using KonaAI.Master.Repository.Common.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KonaAI.Master.Repository.Common.Extensions;

/// <summary>
/// Provides extension methods for configuring entity types with default and client-specific properties in Entity Framework Core.
/// </summary>
public static class EntityConfigurationExtension
{
    /// <summary>
    /// Configures the base properties and table mapping for an entity derived from <see cref="BaseDomain"/>.
    /// </summary>
    /// <typeparam name="T">The entity type derived from <see cref="BaseDomain"/>.</typeparam>
    /// <param name="builder">The entity type builder.</param>
    /// <param name="tableName">The name of the table.</param>
    /// <param name="schemaName">The name of the schema. Defaults to "Authentication".</param>
    /// <param name="ignoreDeleted"></param>
    public static void BaseClientConfiguration<T>(this EntityTypeBuilder<T> builder, string tableName, string schemaName = "Client",
        bool ignoreDeleted = false)
        where T : BaseClientDomain
    {
        SetBuilder(builder, tableName, schemaName);
    }

    /// <summary>
    /// Configures the base metadata properties and table mapping for an entity derived from <see cref="BaseMetaDataDomain"/>.
    /// </summary>
    /// <typeparam name="T">The entity type derived from <see cref="BaseMetaDataDomain"/>.</typeparam>
    /// <param name="builder">The entity type builder.</param>
    /// <param name="tableName">The name of the table.</param>
    /// <param name="schemaName">The name of the schema. Defaults to "MetaData".</param>
    /// <param name="ignoreDeleted">Indicates whether to apply the deleted filter. Defaults to <c>true</c>.</param>
    public static void BaseClientMetaDataConfiguration<T>(this EntityTypeBuilder<T> builder, string tableName,
        string schemaName = "ClientMetaData", bool ignoreDeleted = false)
        where T : BaseClientMetaDataDomain
    {
        SetBuilder(builder, tableName, schemaName, ignoreDeleted, 12);
        builder.Property(e => e.Name).HasMaxLength(DbColumnLength.NameEmail).HasColumnOrder(13).IsRequired();
        builder.Property(e => e.Description).HasMaxLength(DbColumnLength.Description).HasColumnOrder(14);
        builder.Property(e => e.OrderBy).HasColumnOrder(15).IsRequired();
    }

    /// <summary>
    /// Configures the common properties and table mapping for an entity, including audit and soft-delete fields.
    /// </summary>
    /// <typeparam name="T">The entity type derived from <see cref="BaseDomain"/>.</typeparam>
    /// <param name="builder">The entity type builder.</param>
    /// <param name="tableName">The name of the table.</param>
    /// <param name="schemaName">The name of the schema.</param>
    /// <param name="ignoreDeleted">Indicates whether to apply the deleted filter. Defaults to <c>true</c>.</param>
    /// <param name="clientOrder"></param>
    private static void SetBuilder<T>(EntityTypeBuilder<T> builder, string tableName,
        string schemaName, bool ignoreDeleted = false, int clientOrder = 12) where T : BaseClientDomain
    {
        builder.ToTable(tableName, schemaName);
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnOrder(1).IsRequired();
        builder.Property(e => e.RowId).HasColumnOrder(2).IsRequired();
        builder.Property(e => e.CreatedOn).HasColumnOrder(4).IsRequired();
        builder.Property(e => e.CreatedById).HasColumnOrder(5).IsRequired();
        builder.Property(e => e.CreatedBy).HasColumnOrder(6).IsRequired();
        builder.Property(e => e.ModifiedOn).HasColumnOrder(7).IsRequired();
        builder.Property(e => e.ModifiedById).HasColumnOrder(8).IsRequired();
        builder.Property(e => e.ModifiedBy).HasColumnOrder(9).IsRequired();
        builder.Property(e => e.IsActive).HasColumnOrder(10).IsRequired();
        builder.Property(e => e.IsDeleted).HasColumnOrder(11).IsRequired();
        builder.Property(e => e.ClientId).HasColumnOrder(clientOrder).IsRequired();

        if (!ignoreDeleted)
            builder.HasQueryFilter(e => e.IsDeleted == false);
    }
}
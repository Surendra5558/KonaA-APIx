using KonaAI.Master.Repository.Common.Extensions;
using KonaAI.Master.Repository.Domain.Master.MetaData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KonaAI.Master.Repository.Configuration.Master.MetaData;

/// <summary>
/// Entity Framework Core configuration for the <see cref="RenderType"/> entity.
/// Handles database mapping and seed data initialization.
/// </summary>
public class RenderTypeConfiguration : IEntityTypeConfiguration<RenderType>
{
    /// <summary>
    /// Configures the <see cref="RenderType"/> entity and its database schema mapping.
    /// Seeds default render type data for system-wide use.
    /// </summary>
    /// <param name="builder">The entity type builder used to configure the entity.</param>
    public void Configure(EntityTypeBuilder<RenderType> builder)
    {
        // Apply base metadata configuration and set the table name.
        builder.BaseMetaDataConfiguration("RenderType");

        // Seed default render types.
        builder.HasData(
            new RenderType
            {
                RowId = Guid.Parse("1F3A2A01-6B7C-4C9B-905B-72D4D4D3B501"),
                Id = 1,
                Name = "TextArea",
                Description = "Multi-line text input area.",
                OrderBy = 1,
                IsDefault = true,
                CreatedBy = "System",
                ModifiedBy = "System",
                CreatedOn = DateTime.UtcNow,
                ModifiedOn = DateTime.UtcNow,
                IsActive = true,
                IsDeleted = false
            },
            new RenderType
            {
                RowId = Guid.Parse("A18F2A13-8355-4A4C-B930-5A9E3D12D1D7"),
                Id = 2,
                Name = "RadioButton",
                Description = "Single selection radio button input.",
                OrderBy = 2,
                IsDefault = true,
                CreatedBy = "System",
                ModifiedBy = "System",
                CreatedOn = DateTime.UtcNow,
                ModifiedOn = DateTime.UtcNow,
                IsActive = true,
                IsDeleted = false
            },
            new RenderType
            {
                RowId = Guid.Parse("78E3C4B0-03CC-4B16-B4C0-7D391F9A4A6E"),
                Id = 3,
                Name = "CheckBox",
                Description = "Multi-select checkbox input.",
                OrderBy = 3,
                IsDefault = true,
                CreatedBy = "System",
                ModifiedBy = "System",
                CreatedOn = DateTime.UtcNow,
                ModifiedOn = DateTime.UtcNow,
                IsActive = true,
                IsDeleted = false
            },
            new RenderType
            {
                RowId = Guid.Parse("6BE72A4F-8C2C-41C4-826F-CE473A419D32"),
                Id = 4,
                Name = "HyperLink",
                Description = "Clickable hyperlink field.",
                OrderBy = 4,
                IsDefault = true,
                CreatedBy = "System",
                ModifiedBy = "System",
                CreatedOn = DateTime.UtcNow,
                ModifiedOn = DateTime.UtcNow,
                IsActive = true,
                IsDeleted = false
            },
            new RenderType
            {
                RowId = Guid.Parse("5E874B8B-2B87-4A77-87C3-672A7E4A94B4"),
                Id = 5,
                Name = "Table",
                Description = "Tabular data display element.",
                OrderBy = 5,
                IsDefault = true,
                CreatedBy = "System",
                ModifiedBy = "System",
                CreatedOn = DateTime.UtcNow,
                ModifiedOn = DateTime.UtcNow,
                IsActive = true,
                IsDeleted = false
            },
            new RenderType
            {
                RowId = Guid.Parse("C4B718E3-6E2F-49A3-8F54-53C83B7A91E0"),
                Id = 6,
                Name = "Label",
                Description = "Static label or display text.",
                OrderBy = 6,
                IsDefault = true,
                CreatedBy = "System",
                ModifiedBy = "System",
                CreatedOn = DateTime.UtcNow,
                ModifiedOn = DateTime.UtcNow,
                IsActive = true,
                IsDeleted = false
            }
        );
    }
}

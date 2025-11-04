using KonaAI.Master.Repository.Common.Constants;
using KonaAI.Master.Repository.Common.Extensions;
using KonaAI.Master.Repository.Domain.Tenant.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KonaAI.Master.Repository.Configuration.Tenant.Client;

/// <summary>
/// Configures the EF Core entity mapping for <see cref="ClientQuestionnaire"/>.
/// </summary>
public class ClientQuestionnaireConfiguration : IEntityTypeConfiguration<ClientQuestionnaire>
{
    /// <summary>
    /// Configures the <see cref="ClientQuestionnaire"/> entity.
    /// </summary>
    /// <param name="builder">
    /// The <see cref="EntityTypeBuilder{ClientQuestionnaire}"/> used to configure the entity.
    /// </param>
    public void Configure(EntityTypeBuilder<ClientQuestionnaire> builder)
    {
        // Apply base client configuration (includes schema mapping and common fields)
        builder.BaseClientConfiguration("ClientQuestionnaire");

        // Configure composite unique index on ClientId + Name
        builder.HasIndex(x => new { x.ClientId, x.Name })
               .IsUnique();

        // Configure Name property
        builder.Property(x => x.Name)
               .IsRequired()
               .HasMaxLength(DbColumnLength.NameEmail);

        builder.HasData(
               new ClientQuestionnaire
               {
                   Id = 1,
                   RowId = Guid.Parse("bdb9a9b4-8c2a-4a89-9e2c-fdf9b33a7e11"),
                   ClientId = 1,
                   Name = "General Health Assessment",
                   CreatedBy = "System",
                   CreatedById = 1,
                   ModifiedBy = "System",
                   ModifiedById = 1
               }
   );
    }
}
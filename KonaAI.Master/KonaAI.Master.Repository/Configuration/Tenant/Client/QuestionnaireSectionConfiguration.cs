using KonaAI.Master.Repository.Common.Constants;
using KonaAI.Master.Repository.Common.Extensions;
using KonaAI.Master.Repository.Domain.Tenant.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KonaAI.Master.Repository.Configuration.Tenant.Client;

/// <summary>
/// Configures the EF Core entity mapping for <see cref="ClientQuestionnaireSection"/>.
/// </summary>
public class QuestionnaireSectionConfiguration : IEntityTypeConfiguration<ClientQuestionnaireSection>
{
    /// <summary>
    /// Configures the <see cref="ClientQuestionnaireSection"/> entity.
    /// </summary>
    /// <param name="builder">
    /// The <see cref="EntityTypeBuilder{QuestionnaireSection}"/> used to configure the entity.
    /// </param>
    public void Configure(EntityTypeBuilder<ClientQuestionnaireSection> builder)
    {
        // Apply base client configuration (handles schema and common fields)
        builder.BaseClientConfiguration("QuestionnaireSection");

        // Configure Name property: required and limited in length
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(DbColumnLength.NameEmail);

        // Seed default data
        builder.HasData(
            new ClientQuestionnaireSection
            {
                RowId = Guid.Parse("C5A88D1A-2B5D-4E2C-8D5F-12D4EBA19B51"),
                Id = 1,
                Name = "Initial Analysis and Validation",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,

            },
            new ClientQuestionnaireSection
            {
                RowId = Guid.Parse("F0C97E2D-8E23-4C12-A2B5-56A1BCA44A91"),
                Id = 2,
                Name = " Detailed Testing (required for high risk services or high risk countries)",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,

            },
            new ClientQuestionnaireSection
            {
                RowId = Guid.Parse("2DFA943B-AB3C-4F1E-8E79-6FA2D2E4D451"),
                Id = 3,
                Name = "Conclusion",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
            }
        );

    }
}
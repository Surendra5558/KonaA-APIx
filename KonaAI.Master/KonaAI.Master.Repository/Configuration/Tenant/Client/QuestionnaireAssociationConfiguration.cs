using KonaAI.Master.Repository.Common.Extensions;
using KonaAI.Master.Repository.Domain.Tenant.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KonaAI.Master.Repository.Configuration.Tenant.Client;

/// <summary>
/// Configures the EF Core entity mapping for <see cref="ClientQuestionnaireAssociation"/>.
/// </summary>
public class QuestionnaireAssociationConfiguration : IEntityTypeConfiguration<ClientQuestionnaireAssociation>
{
    /// <summary>
    /// Configures the <see cref="ClientQuestionnaireAssociation"/> entity.
    /// </summary>
    /// <param name="builder">
    /// The <see cref="EntityTypeBuilder{QuestionnaireAssociation}"/> used to configure the entity.
    /// </param>
    public void Configure(EntityTypeBuilder<ClientQuestionnaireAssociation> builder)
    {
        // Apply base client configuration (includes schema mapping and common fields)
        builder.BaseClientConfiguration("ClientQuestionnaireAssociation");

        // Configure required relationships/fields
        builder.Property(x => x.ClientQuestionBankId)
               .IsRequired();

        builder.Property(x => x.QuestionnaireId)
               .IsRequired();

        builder.Property(x => x.QuestionnaireSectionId)
               .IsRequired();

        builder.HasData(
            new ClientQuestionnaireAssociation
            {
                Id = 1,
                RowId = Guid.Parse("4d9bb12b-1e4b-48a5-bb45-93b7129f9a01"),
                ClientId = 1,
                ClientQuestionBankId = 1,
                QuestionnaireId = 1,
                QuestionnaireSectionId = 1,
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            },
            new ClientQuestionnaireAssociation
            {
                Id = 2,
                RowId = Guid.Parse("b87fa9b9-4a8f-4b41-b412-03d67298a202"),
                ClientId = 1,
                ClientQuestionBankId = 2,
                QuestionnaireId = 2,
                QuestionnaireSectionId = 1,
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            },
            new ClientQuestionnaireAssociation
            {
                Id = 3,
                RowId = Guid.Parse("fa8dbe77-5b0a-4b17-b871-c6016d663903"),
                ClientId = 1,
                ClientQuestionBankId = 3,
                QuestionnaireId = 3,
                QuestionnaireSectionId = 1,
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            },
            new ClientQuestionnaireAssociation
            {
                Id = 4,
                RowId = Guid.Parse("b48ccab5-b6a1-4c6a-b2c4-f7cb3cb47404"),
                ClientId = 1,
                ClientQuestionBankId = 4,
                QuestionnaireId = 4,
                QuestionnaireSectionId = 1,
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            },
            new ClientQuestionnaireAssociation
            {
                Id = 5,
                RowId = Guid.Parse("13f4dcb2-2e3b-4af9-bf1f-0a1ef5b98605"),
                ClientId = 1,
                ClientQuestionBankId = 5,
                QuestionnaireId = 5,
                QuestionnaireSectionId = 1,
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            },
            new ClientQuestionnaireAssociation
            {
                Id = 6,
                RowId = Guid.Parse("a2743e2a-cde9-478f-9f1e-4f9444c5c706"),
                ClientId = 1,
                ClientQuestionBankId = 6,
                QuestionnaireId = 6,
                QuestionnaireSectionId = 1,
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            },
            new ClientQuestionnaireAssociation
            {
                Id = 7,
                RowId = Guid.Parse("c4a27b7d-57eb-47e3-b1d8-3a7f8abbdc07"),
                ClientId = 1,
                ClientQuestionBankId = 7,
                QuestionnaireId = 7,
                QuestionnaireSectionId = 1,
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            },
            new ClientQuestionnaireAssociation
            {
                Id = 8,
                RowId = Guid.Parse("7a0a59a9-59e3-4c7a-83c5-b52fd4b53e08"),
                ClientId = 1,
                ClientQuestionBankId = 8,
                QuestionnaireId = 8,
                QuestionnaireSectionId = 1,
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            },
            new ClientQuestionnaireAssociation
            {
                Id = 9,
                RowId = Guid.Parse("51297f68-050a-426f-b6a0-fc28f42ee909"),
                ClientId = 1,
                ClientQuestionBankId = 9,
                QuestionnaireId = 9,
                QuestionnaireSectionId = 1,
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            },
            new ClientQuestionnaireAssociation
            {
                Id = 10,
                RowId = Guid.Parse("be5c7d76-c67c-49e1-9f16-bba3f7ee4c10"),
                ClientId = 1,
                ClientQuestionBankId = 10,
                QuestionnaireId = 10,
                QuestionnaireSectionId = 1,
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            },
            new ClientQuestionnaireAssociation
            {
                Id = 11,
                RowId = Guid.Parse("ff74a8e3-f45b-462c-b1b2-f19b8c85b711"),
                ClientId = 1,
                ClientQuestionBankId = 11,
                QuestionnaireId = 11,
                QuestionnaireSectionId = 1,
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            },
            new ClientQuestionnaireAssociation
            {
                Id = 12,
                RowId = Guid.Parse("da09a77c-c7d9-4f35-a49c-27d512d46912"),
                ClientId = 1,
                ClientQuestionBankId = 12,
                QuestionnaireId = 12,
                QuestionnaireSectionId = 1,
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            },
            new ClientQuestionnaireAssociation
            {
                Id = 13,
                RowId = Guid.Parse("5d3f92b9-8ee7-44a8-9b51-21c00dd22b13"),
                ClientId = 1,
                ClientQuestionBankId = 13,
                QuestionnaireId = 13,
                QuestionnaireSectionId = 1,
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            },
            new ClientQuestionnaireAssociation
            {
                Id = 14,
                RowId = Guid.Parse("eac61a47-9466-4ce9-bd6c-65cb02e8a714"),
                ClientId = 1,
                ClientQuestionBankId = 14,
                QuestionnaireId = 14,
                QuestionnaireSectionId = 1,
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            },
            new ClientQuestionnaireAssociation
            {
                Id = 15,
                RowId = Guid.Parse("3b8244d1-601a-4db0-81b3-5b7b17e32915"),
                ClientId = 1,
                ClientQuestionBankId = 15,
                QuestionnaireId = 15,
                QuestionnaireSectionId = 1,
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            },
            new ClientQuestionnaireAssociation
            {
                Id = 16,
                RowId = Guid.Parse("a64a8f8f-b80e-4664-b9df-0dbabff83216"),
                ClientId = 1,
                ClientQuestionBankId = 16,
                QuestionnaireId = 16,
                QuestionnaireSectionId = 1,
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            },
            new ClientQuestionnaireAssociation
            {
                Id = 17,
                RowId = Guid.Parse("44d0a7d3-b22c-4c3c-8d3d-97d3f640cb17"),
                ClientId = 1,
                ClientQuestionBankId = 17,
                QuestionnaireId = 17,
                QuestionnaireSectionId = 2,
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            },
            new ClientQuestionnaireAssociation
            {
                Id = 18,
                RowId = Guid.Parse("f12d73c8-6b8f-43c0-9b32-84579819b518"),
                ClientId = 1,
                ClientQuestionBankId = 18,
                QuestionnaireId = 18,
                QuestionnaireSectionId = 2,
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            },
            new ClientQuestionnaireAssociation
            {
                Id = 19,
                RowId = Guid.Parse("07e0a563-b045-4a12-b0cc-63baf72db919"),
                ClientId = 1,
                ClientQuestionBankId = 19,
                QuestionnaireId = 19,
                QuestionnaireSectionId = 2,
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            },
            new ClientQuestionnaireAssociation
            {
                Id = 20,
                RowId = Guid.Parse("c0e6aef3-1d6c-4b78-a6f4-3b9ef9e7ad20"),
                ClientId = 1,
                ClientQuestionBankId = 20,
                QuestionnaireId = 20,
                QuestionnaireSectionId = 2,
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            },
            new ClientQuestionnaireAssociation
            {
                Id = 21,
                RowId = Guid.Parse("b2b8c071-7f12-47b9-9a23-efb61e81c121"),
                ClientId = 1,
                ClientQuestionBankId = 21,
                QuestionnaireId = 21,
                QuestionnaireSectionId = 2,
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            },
            new ClientQuestionnaireAssociation
            {
                Id = 22,
                RowId = Guid.Parse("2d2cbbde-fb23-4f62-b6e5-5b2a8a339922"),
                ClientId = 1,
                ClientQuestionBankId = 22,
                QuestionnaireId = 22,
                QuestionnaireSectionId = 2,
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            },
            new ClientQuestionnaireAssociation
            {
                Id = 23,
                RowId = Guid.Parse("a40e17db-32dc-42a5-905d-f2482d01a723"),
                ClientId = 1,
                ClientQuestionBankId = 23,
                QuestionnaireId = 23,
                QuestionnaireSectionId = 2,
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            },
            new ClientQuestionnaireAssociation
            {
                Id = 24,
                RowId = Guid.Parse("5d1e8b63-91e4-47cf-a4f2-34a20a2c8d24"),
                ClientId = 1,
                ClientQuestionBankId = 24,
                QuestionnaireId = 24,
                QuestionnaireSectionId = 2,
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            },
            new ClientQuestionnaireAssociation
            {
                Id = 25,
                RowId = Guid.Parse("ae32cf2c-4b4d-4568-a8b9-f273fdc94c25"),
                ClientId = 1,
                ClientQuestionBankId = 25,
                QuestionnaireId = 25,
                QuestionnaireSectionId = 2,
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            },
            new ClientQuestionnaireAssociation
            {
                Id = 26,
                RowId = Guid.Parse("6b54c5e2-1776-4f71-9c11-2e1cf74f4d26"),
                ClientId = 1,
                ClientQuestionBankId = 26,
                QuestionnaireId = 26,
                QuestionnaireSectionId = 2,
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            },
            new ClientQuestionnaireAssociation
            {
                Id = 27,
                RowId = Guid.Parse("3c91ed73-33c1-4c91-b761-0b47f84a3127"),
                ClientId = 1,
                ClientQuestionBankId = 27,
                QuestionnaireId = 27,
                QuestionnaireSectionId = 3,
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            },
            new ClientQuestionnaireAssociation
            {
                Id = 28,
                RowId = Guid.Parse("1b8f06d9-cf0c-45f5-9023-9b7a1c94a828"),
                ClientId = 1,
                ClientQuestionBankId = 28,
                QuestionnaireId = 28,
                QuestionnaireSectionId = 3,
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            },
            new ClientQuestionnaireAssociation
            {
                Id = 29,
                RowId = Guid.Parse("e86a3fda-b8ce-4b62-8ed5-4dfd72e5e729"),
                ClientId = 1,
                ClientQuestionBankId = 29,
                QuestionnaireId = 29,
                QuestionnaireSectionId = 3,
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            }

        );

    }
}
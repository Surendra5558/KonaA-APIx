using KonaAI.Master.Repository.Common.Constants;
using KonaAI.Master.Repository.Common.Extensions;
using KonaAI.Master.Repository.Domain.Master.App;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KonaAI.Master.Repository.Configuration.Master.App;

/// <summary>
/// Configures the EF Core entity mapping for <see cref="QuestionnaireAssociation"/>.
/// </summary>
public class QuestionnaireAssociationConfiguration : IEntityTypeConfiguration<QuestionnaireAssociation>
{
    /// <summary>
    /// Configures the <see cref="QuestionnaireAssociation"/> entity for EF Core.
    /// </summary>
    public void Configure(EntityTypeBuilder<QuestionnaireAssociation> builder)
    {
        // Applies standard App schema base configuration
        builder.BaseConfiguration("QuestionnaireAssociation");

        builder.Property(x => x.QuestionnaireName)
               .IsRequired()
               .HasMaxLength(DbColumnLength.Description);

        builder.Property(x => x.SectionName)
               .IsRequired()
               .HasMaxLength(DbColumnLength.Description);

        builder.Property(x => x.QuestionId)
               .IsRequired();

        builder.HasData(

             new QuestionnaireAssociation
             {
                 Id = 1,
                 RowId = Guid.Parse("4b8e1f64-3b89-4d26-9969-2ce15bfa1b81"),
                 QuestionnaireName = "What is the Transaction ID for the potentially higher risk transaction?",
                 SectionName = "Initial Analysis and Validation",
                 QuestionId = 1,
                 CreatedBy = "System",
                 CreatedById = 1,
                 ModifiedBy = "System",
                 ModifiedById = 1
             },
             new QuestionnaireAssociation
             {
                 Id = 2,
                 RowId = Guid.Parse("0d45f6ae-cd8d-4d4a-b27d-6f44b4c9b6ad"),
                 QuestionnaireName = "Transaction Contact",
                 SectionName = "Initial Analysis and Validation",
                 QuestionId = 2,
                 CreatedBy = "System",
                 CreatedById = 1,
                 ModifiedBy = "System",
                 ModifiedById = 1
             },
             new QuestionnaireAssociation
             {
                 Id = 3,
                 RowId = Guid.Parse("a8f136b7-93a4-4c77-a2da-c81a2c5975e2"),
                 QuestionnaireName = "How was the transaction identified?",
                 SectionName = "Initial Analysis and Validation",
                 QuestionId = 3,
                 CreatedBy = "System",
                 CreatedById = 1,
                 ModifiedBy = "System",
                 ModifiedById = 1
             },
             new QuestionnaireAssociation
             {
                 Id = 4,
                 RowId = Guid.Parse("7ce2b497-55a5-4a1a-a915-65bbfc8ffb9e"),
                 QuestionnaireName = "Comments (for 1c)",
                 SectionName = "Initial Analysis and Validation",
                 QuestionId = 4,
                 CreatedBy = "System",
                 CreatedById = 1,
                 ModifiedBy = "System",
                 ModifiedById = 1
             },
             new QuestionnaireAssociation
             {
                 Id = 5,
                 RowId = Guid.Parse("c2dc87cc-2a1a-47f8-b870-c894b7dc4c57"),
                 QuestionnaireName = "Transaction Scope Summary",
                 SectionName = "Initial Analysis and Validation",
                 QuestionId = 5,
                 CreatedBy = "System",
                 CreatedById = 1,
                 ModifiedBy = "System",
                 ModifiedById = 1
             },
             new QuestionnaireAssociation
             {
                 Id = 6,
                 RowId = Guid.Parse("ab734a83-9a0f-44d0-b4a7-46af3955a653"),
                 QuestionnaireName = "Are activities being performed in a high risk country?",
                 SectionName = "Initial Analysis and Validation",
                 QuestionId = 6,
                 CreatedBy = "System",
                 CreatedById = 1,
                 ModifiedBy = "System",
                 ModifiedById = 1
             },
             new QuestionnaireAssociation
             {
                 Id = 7,
                 RowId = Guid.Parse("35b6a7de-fcf7-44f9-8414-b93b5af52cb3"),
                 QuestionnaireName = "Does the scope include any of the following?",
                 SectionName = "Initial Analysis and Validation",
                 QuestionId = 7,
                 CreatedBy = "System",
                 CreatedById = 1,
                 ModifiedBy = "System",
                 ModifiedById = 1
             },
             new QuestionnaireAssociation
             {
                 Id = 8,
                 RowId = Guid.Parse("e5d3e6c9-9fcd-462d-bf63-bc49c9957f9f"),
                 QuestionnaireName = "Does this relate to any of the following controls?",
                 SectionName = "Initial Analysis and Validation",
                 QuestionId = 8,
                 CreatedBy = "System",
                 CreatedById = 1,
                 ModifiedBy = "System",
                 ModifiedById = 1
             },
             new QuestionnaireAssociation
             {
                 Id = 9,
                 RowId = Guid.Parse("b5d02c6c-dbb8-41ce-9cd3-37c08f85a923"),
                 QuestionnaireName = "Comments (for 1g)",
                 SectionName = "Initial Analysis and Validation",
                 QuestionId = 9,
                 CreatedBy = "System",
                 CreatedById = 1,
                 ModifiedBy = "System",
                 ModifiedById = 1
             },
             new QuestionnaireAssociation
             {
                 Id = 10,
                 RowId = Guid.Parse("cfdff01f-003b-4981-8b31-0a3769b79db1"),
                 QuestionnaireName = "Is there an issue captured by OCR?",
                 SectionName = "Initial Analysis and Validation",
                 QuestionId = 10,
                 CreatedBy = "System",
                 CreatedById = 1,
                 ModifiedBy = "System",
                 ModifiedById = 1
             },


             new QuestionnaireAssociation
             {
                 Id = 11,
                 RowId = Guid.Parse("a2a77b4c-7b18-4a41-aef7-41e75e528c69"),
                 QuestionnaireName = "Description of Risk Data captured by OCR",
                 SectionName = "Initial Analysis and Validation",
                 QuestionId = 11,
                 CreatedBy = "System",
                 CreatedById = 1,
                 ModifiedBy = "System",
                 ModifiedById = 1
             },
             new QuestionnaireAssociation
             {
                 Id = 12,
                 RowId = Guid.Parse("d03e3c9d-ff33-4da9-bb49-3f8c06b0f56e"),
                 QuestionnaireName = "Is the expenditure within pre-approvals (value and nature)?",
                 SectionName = "Initial Analysis and Validation",
                 QuestionId = 12,
                 CreatedBy = "System",
                 CreatedById = 1,
                 ModifiedBy = "System",
                 ModifiedById = 1
             },
             new QuestionnaireAssociation
             {
                 Id = 13,
                 RowId = Guid.Parse("dbec7059-9b61-4058-9263-7e2b2e30fd9e"),
                 QuestionnaireName = "Were any conditions of approval completed as required?",
                 SectionName = "Initial Analysis and Validation",
                 QuestionId = 13,
                 CreatedBy = "System",
                 CreatedById = 1,
                 ModifiedBy = "System",
                 ModifiedById = 1
             },
             new QuestionnaireAssociation
             {
                 Id = 14,
                 RowId = Guid.Parse("bbf92a72-28de-4a68-8a41-2b229b5b779b"),
                 QuestionnaireName = "Is there an issue captured by OCR?",
                 SectionName = "Initial Analysis and Validation",
                 QuestionId = 14,
                 CreatedBy = "System",
                 CreatedById = 1,
                 ModifiedBy = "System",
                 ModifiedById = 1
             },
             new QuestionnaireAssociation
             {
                 Id = 15,
                 RowId = Guid.Parse("a82ecbbd-cd93-4742-a2f1-1e27a96d6578"),
                 QuestionnaireName = "Description of Risk Data captured by OCR",
                 SectionName = "Initial Analysis and Validation",
                 QuestionId = 15,
                 CreatedBy = "System",
                 CreatedById = 1,
                 ModifiedBy = "System",
                 ModifiedById = 1
             },
             new QuestionnaireAssociation
             {
                 Id = 16,
                 RowId = Guid.Parse("1d0c1a22-7c3b-41a2-aebf-b0b50eb0d29e"),
                 QuestionnaireName = "Case Validation Type",
                 SectionName = "Initial Analysis and Validation",
                 QuestionId = 16,
                 CreatedBy = "System",
                 CreatedById = 1,
                 ModifiedBy = "System",
                 ModifiedById = 1
             },
             new QuestionnaireAssociation
             {
                 Id = 17,
                 RowId = Guid.Parse("d8a7b22e-35a9-4a1c-b8df-dc567f389056"),
                 QuestionnaireName = "Is sole source approval obtained for this transaction?",
                 SectionName = "Detailed Testing (required for high risk services or high risk countries)",
                 QuestionId = 17,
                 CreatedBy = "System",
                 CreatedById = 1,
                 ModifiedBy = "System",
                 ModifiedById = 1
             },
             new QuestionnaireAssociation
             {
                 Id = 18,
                 RowId = Guid.Parse("7af7a6b5-d3e9-4604-8ee7-2c3cb33b1d86"),
                 QuestionnaireName = "Does the fee structure include any of the following:",
                 SectionName = "Detailed Testing (required for high risk services or high risk countries)",
                 QuestionId = 18,
                 CreatedBy = "System",
                 CreatedById = 1,
                 ModifiedBy = "System",
                 ModifiedById = 1
             },
             new QuestionnaireAssociation
             {
                 Id = 19,
                 RowId = Guid.Parse("afc91e2c-6b4d-4bde-817d-8f9cf054bcd4"),
                 QuestionnaireName = "Supporting Document Summary",
                 SectionName = "Detailed Testing (required for high risk services or high risk countries)",
                 QuestionId = 19,
                 CreatedBy = "System",
                 CreatedById = 1,
                 ModifiedBy = "System",
                 ModifiedById = 1
             },
             new QuestionnaireAssociation
             {
                 Id = 20,
                 RowId = Guid.Parse("3d1bdb6e-ec82-4f70-b2a3-2a2e58c6b017"),
                 QuestionnaireName = "Comments",
                 SectionName = "Detailed Testing (required for high risk services or high risk countries)",
                 QuestionId = 20,
                 CreatedBy = "System",
                 CreatedById = 1,
                 ModifiedBy = "System",
                 ModifiedById = 1
             },

            // -------- Section 3: Conclusion --------
            new QuestionnaireAssociation
            {
                Id = 21,
                RowId = Guid.Parse("bb404e8e-9b2e-40ee-b48f-02b08e7d4785"),
                QuestionnaireName = "Is the transaction adequately supported?",
                SectionName = "Detailed Testing (required for high risk services or high risk countries)",
                QuestionId = 21,
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            },
            new QuestionnaireAssociation
            {
                Id = 22,
                RowId = Guid.Parse("4a94c9cd-26c9-41d1-b8e2-5b33ad3907a1"),
                QuestionnaireName = "Does the transaction appear legitimate or have a business justification?",
                SectionName = "Detailed Testing (required for high risk services or high risk countries)",
                QuestionId = 22,
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            },
            new QuestionnaireAssociation
            {
                Id = 23,
                RowId = Guid.Parse("08747f64-04b8-474f-bc30-7ff1655e9d56"),
                QuestionnaireName = "Do any receipts or supporting documents appear forged or fake?",
                SectionName = "Detailed Testing (required for high risk services or high risk countries)",
                QuestionId = 23,
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            },
            new QuestionnaireAssociation
            {
                Id = 24,
                RowId = Guid.Parse("05df7f8d-4f0f-41f2-98ed-6df4a0a0aabb"),
                QuestionnaireName = "Do the supporting documents contain the supplier's terms and conditions?",
                SectionName = "Detailed Testing (required for high risk services or high risk countries)",
                QuestionId = 24,
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            },
            new QuestionnaireAssociation
            {
                Id = 25,
                RowId = Guid.Parse("e428a27d-736d-4b0a-8886-3a5a2b4eae5d"),
                QuestionnaireName = "Is the supplier paying another supplier to bypass controls?",
                SectionName = "Detailed Testing (required for high risk services or high risk countries)",
                QuestionId = 25,
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            },
            new QuestionnaireAssociation
            {
                Id = 26,
                RowId = Guid.Parse("4a1374ee-b94f-43c4-95d7-ec6e9b0b3a51"),
                QuestionnaireName = "Is the expense categorized correctly or recorded consistently?",
                SectionName = "Detailed Testing (required for high risk services or high risk countries)",
                QuestionId = 26,
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            },
            new QuestionnaireAssociation
            {
                Id = 27,
                RowId = Guid.Parse("56a7f58d-5ee8-4952-9c31-9ce2a4e874ed"),
                QuestionnaireName = "Based on the review, please select the most appropriate assessment:",
                SectionName = "Conclusion",
                QuestionId = 27,
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            },
            new QuestionnaireAssociation
            {
                Id = 28,
                RowId = Guid.Parse("5fa2b933-202d-4fc3-89e3-00c6b4ee2b92"),
                QuestionnaireName = "E&C Action Tracker ID",
                SectionName = "Conclusion",
                QuestionId = 28,
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            },
            new QuestionnaireAssociation
            {
                Id = 29,
                RowId = Guid.Parse("e31d2d92-5d7c-404f-9d43-5c89f3bb4dc3"),
                QuestionnaireName = "Conclusion",
                SectionName = "Conclusion",
                QuestionId = 29,
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            }
        );

    }
}
using KonaAI.Master.Repository.Common.Constants;
using KonaAI.Master.Repository.Common.Extensions;
using KonaAI.Master.Repository.Domain.Master.App;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KonaAI.Master.Repository.Configuration.Master.App;

/// <summary>
/// Configures the EF Core entity mapping for <see cref="QuestionBank"/>.
/// </summary>
public class QuestionBankConfiguration : IEntityTypeConfiguration<QuestionBank>
{

    /// <summary>
    /// Configures the <see cref="QuestionBank"/> entity for EF Core.
    /// </summary>
    public void Configure(EntityTypeBuilder<QuestionBank> builder)
    {
        // Apply base configuration with schema "App"
        builder.BaseConfiguration("QuestionBank");

        // Configure Description property (optional, limited length)
        builder.Property(q => q.Description)
               .HasMaxLength(DbColumnLength.Description);

        // Configure OnAction property (optional, limited length)
        builder.Property(q => q.OnAction)
               .HasMaxLength(DbColumnLength.NameEmail);

        // Configure Options property (stored as JSON if using EF Core value converters)
        builder.Property(q => q.Options)
            .HasMaxLength(DbColumnLength.MaxText);

        // Configure LinkedQuestion (must always have a valid value)
        builder.Property(q => q.LinkedQuestion)
               .IsRequired();

        // Configure RenderType property (optional, limited length)
        builder.Property(q => q.RenderType)
            .IsRequired();


        builder.HasData(
            new QuestionBank
            {
                Id = 1,
                RowId = Guid.Parse("f7d62b31-dbe2-4b73-9d28-9c5c7a0d21a0"),
                Description = "What is the Transaction ID for the potentially higher risk transaction?",
                IsMandatory = false,
                Options = null,
                LinkedQuestion = 0,
                OnAction = null,
                RenderType = (long)RenderTypeIds.Table, // Table input
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            },
             new QuestionBank
             {
                 Id = 2,
                 RowId = Guid.Parse("a27305e1-03a1-4e29-8a76-0e49bb2a06cc"),
                 Description = "Transaction Contact",
                 IsMandatory = false,
                 Options = null,
                 LinkedQuestion = 0,
                 OnAction = null,
                 RenderType = (long)RenderTypeIds.TextArea, // TextArea (disabled)
                 CreatedBy = "System",
                 CreatedById = 1,
                 ModifiedBy = "System",
                 ModifiedById = 1
             },
             new QuestionBank
             {
                 Id = 3,
                 RowId = Guid.Parse("b5e0cc35-3dcf-4a25-bcd3-20b2f1c1a2e0"),
                 Description = "How was the transaction identified?",
                 IsMandatory = false,
                 Options = @"[""Business Enquiry"", ""Deep Dive Monitoring"", ""High Risk Ranking"", ""Transactions Alert"", ""Other (Please Specify in Comments)""]",
                 LinkedQuestion = 0,
                 OnAction = null,
                 RenderType = (long)RenderTypeIds.RadioButton, // RadioButton options
                 CreatedBy = "System",
                 CreatedById = 1,
                 ModifiedBy = "System",
                 ModifiedById = 1
             },
             new QuestionBank
             {
                 Id = 4,
                 RowId = Guid.Parse("e4d76e44-7dcb-4677-9097-3ccaf7a47d3b"),
                 Description = "Comments (for 1c)",
                 IsMandatory = false,
                 Options = null,
                 LinkedQuestion = 3,
                 OnAction = null,
                 RenderType = (long)RenderTypeIds.TextArea, // TextArea
                 CreatedBy = "System",
                 CreatedById = 1,
                 ModifiedBy = "System",
                 ModifiedById = 1
             },
             new QuestionBank
             {
                 Id = 5,
                 RowId = Guid.Parse("3e4a5157-4f08-43b2-9bc2-b73d7728c58b"),
                 Description = "Transaction Scope Summary",
                 IsMandatory = false,
                 Options = null,
                 LinkedQuestion = 0,
                 OnAction = null,
                 RenderType = (long)RenderTypeIds.TextArea, // TextArea
                 CreatedBy = "System",
                 CreatedById = 1,
                 ModifiedBy = "System",
                 ModifiedById = 1
             },
             new QuestionBank
             {
                 Id = 6,
                 RowId = Guid.Parse("1aab7ef3-15e0-4b8c-944f-b63d5d7a9e0b"),
                 Description = "Are activities being performed in a high risk country (If yes, conduct detailed testing)?",
                 IsMandatory = true,
                 Options = @"[""Yes"", ""No""]",
                 LinkedQuestion = 0,
                 OnAction = null,
                 RenderType = (long)RenderTypeIds.RadioButton, // RadioButton
                 CreatedBy = "System",
                 CreatedById = 1,
                 ModifiedBy = "System",
                 ModifiedById = 1
             },
             new QuestionBank
             {
                 Id = 7,
                 RowId = Guid.Parse("94db55ad-4a04-4a31-8705-4c5b29a75876"),
                 Description = "Does the scope include any of the following (If yes, conduct detailed testing)?",
                 IsMandatory = false,
                 Options = @"[
                ""Access Rights, Licenses & Approvals"",
                ""Lobbying, Political Advice, Political/Government Information or Assistance"",
                ""Information gathering"",
                ""Consulting"",
                ""Agency"",
                ""Clearances"",
                ""Engineering, Procurement, Construction and Management (EPCM)"",
                ""Legal services"",
                ""Security services"",
                ""Cash payments made by the supplier"",
                ""Advisory services related to mergers, acquisitions, ventures or growth transactions"",
                ""None of the above""
            ]",
                 LinkedQuestion = 0,
                 OnAction = null,
                 RenderType = (long)RenderTypeIds.CheckBox, // Multi-select options
                 CreatedBy = "System",
                 CreatedById = 1,
                 ModifiedBy = "System",
                 ModifiedById = 1
             },
             new QuestionBank
             {
                 Id = 8,
                 RowId = Guid.Parse("4c2e4b77-240f-4e37-93a8-b87f63d48b4d"),
                 Description = "Does this relate to any of the following controls?",
                 IsMandatory = false,
                 Options = @"[
                ""Third Party Due Diligence"",
                ""Community and Social Investment Process"",
                ""Organisation Membership Process"",
                ""Sponsorship Process"",
                ""Supplier Contracting Process"",
                ""One Time Vendor Process (High Risk Suppliers)"",
                ""Purchase Requisition Process (Scope Creep)"",
                ""Giving Anything of Value"",
                ""Conflicts of Interest"",
                ""Gifts/Hospitality"",
                ""Other (Please specify in comments)"",
                ""N/A""
            ]",
                 LinkedQuestion = 0,
                 OnAction = null,
                 RenderType = (long)RenderTypeIds.CheckBox, // CheckBox
                 CreatedBy = "System",
                 CreatedById = 1,
                 ModifiedBy = "System",
                 ModifiedById = 1
             },
             new QuestionBank
             {
                 Id = 9,
                 RowId = Guid.Parse("c8a5b7a5-198a-48de-931d-705dbe7f9a32"),
                 Description = "Comments (for 1g)",
                 IsMandatory = false,
                 Options = null,
                 LinkedQuestion = 8,
                 OnAction = null,
                 RenderType = (long)RenderTypeIds.TextArea, // TextArea
                 CreatedBy = "System",
                 CreatedById = 1,
                 ModifiedBy = "System",
                 ModifiedById = 1
             },
             new QuestionBank
             {
                 Id = 10,
                 RowId = Guid.Parse("b8f09a48-b210-479b-b0d2-5c4cdbdc0f6f"),
                 Description = "Is there an issue captured by OCR?",
                 IsMandatory = false,
                 Options = @"[""Yes"", ""No""]",
                 LinkedQuestion = 0,
                 OnAction = null,
                 RenderType = (long)RenderTypeIds.RadioButton,
                 CreatedBy = "System",
                 CreatedById = 1,
                 ModifiedBy = "System",
                 ModifiedById = 1
             },
             new QuestionBank
             {
                 Id = 11,
                 RowId = Guid.Parse("fc8b9842-d449-4d84-98db-441b89d0f5ad"),
                 Description = "Description of Risk Data captured by OCR",
                 IsMandatory = false,
                 Options = null,
                 LinkedQuestion = 10,
                 OnAction = null,
                 RenderType = (long)RenderTypeIds.TextArea, // TextArea
                 CreatedBy = "System",
                 CreatedById = 1,
                 ModifiedBy = "System",
                 ModifiedById = 1
             },
             new QuestionBank
             {
                 Id = 12,
                 RowId = Guid.Parse("b7b5dcda-f2b8-4e5d-a13d-25a8191da729"),
                 Description = "Is the expenditure within pre-approvals (value and nature)?",
                 IsMandatory = false,
                 Options = @"[""Yes"", ""No"", ""N/A""]",
                 LinkedQuestion = 0,
                 OnAction = null,
                 RenderType = (long)RenderTypeIds.RadioButton,
                 CreatedBy = "System",
                 CreatedById = 1,
                 ModifiedBy = "System",
                 ModifiedById = 1
             },
             new QuestionBank
             {
                 Id = 13,
                 RowId = Guid.Parse("c764df4c-3cb7-489a-8c74-233b9da907e7"),
                 Description = "Were any conditions of approval completed as required?",
                 IsMandatory = false,
                 Options = @"[""Yes"", ""No"", ""N/A""]",
                 LinkedQuestion = 0,
                 OnAction = null,
                 RenderType = (long)RenderTypeIds.RadioButton,
                 CreatedBy = "System",
                 CreatedById = 1,
                 ModifiedBy = "System",
                 ModifiedById = 1
             },
             new QuestionBank
             {
                 Id = 14,
                 RowId = Guid.Parse("afdb6e3b-681b-4ff9-8a13-b93e3bb72fd9"),
                 Description = "Is there an issue captured by OCR?",
                 IsMandatory = false,
                 Options = @"[""Yes"", ""No""]",
                 LinkedQuestion = 0,
                 OnAction = null,
                 RenderType = (long)RenderTypeIds.RadioButton,
                 CreatedBy = "System",
                 CreatedById = 1,
                 ModifiedBy = "System",
                 ModifiedById = 1
             },
             new QuestionBank
             {
                 Id = 15,
                 RowId = Guid.Parse("9fd3249e-5b93-4dc2-b238-3a24de8a0e3e"),
                 Description = "Description of Risk Data captured by OCR",
                 IsMandatory = false,
                 Options = null,
                 LinkedQuestion = 0,
                 OnAction = null,
                 RenderType = (long)RenderTypeIds.TextArea,
                 CreatedBy = "System",
                 CreatedById = 1,
                 ModifiedBy = "System",
                 ModifiedById = 1
             },
             new QuestionBank
             {
                 Id = 16,
                 RowId = Guid.Parse("e01c42a1-5692-497c-8c9d-23a21c8e6b87"),
                 Description = "Case Validation Type",
                 IsMandatory = false,
                 Options = @"[""Further Review Required"", ""Further Review Not Required"", ""False Positive (No Controls Applicable)""]",
                 LinkedQuestion = 0,
                 OnAction = null,
                 RenderType = (long)RenderTypeIds.RadioButton, // RadioButton
                 CreatedBy = "System",
                 CreatedById = 1,
                 ModifiedBy = "System",
                 ModifiedById = 1
             },

              new QuestionBank
              {
                  Id = 17,
                  RowId = Guid.Parse("4c7c9b9c-d0a2-44d8-8b70-6a68b6f6b6f1"),
                  Description = "Is sole source approval obtained for this transaction?",
                  IsMandatory = false,
                  Options = @"[""Yes"", ""No"", ""N/A""]",
                  LinkedQuestion = 0,
                  OnAction = null,
                  RenderType = (long)RenderTypeIds.RadioButton, // RadioButton
                  CreatedBy = "System",
                  CreatedById = 1,
                  ModifiedBy = "System",
                  ModifiedById = 1
                  },
            new QuestionBank
            {
                Id = 18,
                RowId = Guid.Parse("7dbb9f2b-47e9-4dcb-84d2-9ac6e3f79b9b"),
                Description = "Does the fee structure include any of the following:",
                IsMandatory = false,
                Options = @"[
                    ""Regular retainer (e.g. fixed amount per month)"",
                    ""Lump sum or fixed price"",
                    ""Outcome-based (e.g. contingency fee, commission, delay, penalty or success fee)"",
                    ""Discretionary fee"",
                    ""Cost plus"",
                    ""None of the above""
                ]",
                LinkedQuestion = 0,
                OnAction = null,
                RenderType = (long)RenderTypeIds.RadioButton, // RadioButton
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            },
            new QuestionBank
            {
                Id = 19,
                RowId = Guid.Parse("bce4a74e-59d4-4fd5-8175-c1c6a6b4ff7c"),
                Description = "Supporting Document Summary",
                IsMandatory = false,
                Options = @"[
                    ""Assessment/Evaluation Form"",
                    ""Contract"",
                    ""Email Communication"",
                    ""Engagement Letter"",
                    ""Expense Report"",
                    ""Invoice"",
                    ""Invoice Details"",
                    ""Official Receipts"",
                    ""Quotation/Proposal"",
                    ""Risk Assessment Form"",
                    ""Scope of Work"",
                    ""SSR Form"",
                    ""Other (Please Specify in Comments)""]",
                LinkedQuestion = 0,
                OnAction = null,
                RenderType = (long)RenderTypeIds.RadioButton, // RadioButton
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            },
            new QuestionBank
            {
                Id = 20,
                RowId = Guid.Parse("5cb784af-28a0-4ee6-a6ee-5f1c9d9efb9d"),
                Description = "Comments",
                IsMandatory = false,
                Options = null,
                LinkedQuestion = 19,
                OnAction = null,
                RenderType = (long)RenderTypeIds.TextArea, // TextArea
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            },
            new QuestionBank
            {
                Id = 21,
                RowId = Guid.Parse("7e12e7ef-b97d-4b31-9a2f-0f0e5dce98f8"),
                Description = "Is the transaction adequately supported?",
                IsMandatory = false,
                Options = @"[""Yes"", ""No""]",
                LinkedQuestion = 0,
                OnAction = null,
                RenderType = (long)RenderTypeIds.RadioButton, // RadioButton
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            },
            new QuestionBank
            {
                Id = 22,
                RowId = Guid.Parse("ee3a37ef-6281-46ac-9c93-99d3a7e50d52"),
                Description = "Does the transaction appear legitimate or have a business justification?",
                IsMandatory = false,
                Options = @"[""Yes"", ""No""]",
                LinkedQuestion = 0,
                OnAction = null,
                RenderType = (long)RenderTypeIds.RadioButton, // RadioButton
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            },
            new QuestionBank
            {
                Id = 23,
                RowId = Guid.Parse("6cb9a1f5-19b8-4e91-85ea-f8266f01b76a"),
                Description = "Do any receipts or supporting documents appear forged or fake based on physical appearance or inconsistencies?",
                IsMandatory = false,
                Options = @"[""Yes"", ""No""]",
                LinkedQuestion = 0,
                OnAction = null,
                RenderType = (long)RenderTypeIds.RadioButton, // RadioButton
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            },
            new QuestionBank
            {
                Id = 24,
                RowId = Guid.Parse("fa1d7f55-bb41-45bb-9f18-9eacb279c214"),
                Description = "Do the supporting documents contain the supplier's terms and conditions?",
                IsMandatory = false,
                Options = @"[""Yes"", ""No""]",
                LinkedQuestion = 0,
                OnAction = null,
                RenderType = (long)RenderTypeIds.RadioButton, // RadioButton
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            },
            new QuestionBank
            {
                Id = 25,
                RowId = Guid.Parse("d23282c7-d146-4721-8ea7-cb6c5f5d1b83"),
                Description = "Is the supplier paying another supplier to bypass controls?",
                IsMandatory = false,
                Options = @"[""Yes"", ""No""]",
                LinkedQuestion = 0,
                OnAction = null,
                RenderType = (long)RenderTypeIds.RadioButton, // RadioButton
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            },
            new QuestionBank
            {
                Id = 26,
                RowId = Guid.Parse("8a4c9b4e-48f7-4cb1-9138-9a3db3a7d07b"),
                Description = "Is the expense categorized correctly or recorded consistently with the nature of the expense?",
                IsMandatory = false,
                Options = @"[""Yes"", ""No""]",
                LinkedQuestion = 0,
                OnAction = null,
                RenderType = (long)RenderTypeIds.RadioButton, // RadioButton
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            },
            new QuestionBank
            {
                Id = 27,
                RowId = Guid.Parse("0b6d4f2e-8f1d-4c74-81e0-2e3d0c8a68f2"),
                Description = "Based on the review, please select the most appropriate assessment:",
                IsMandatory = false,
                Options = @"[
                ""No issues identified"",
                ""Potential issues noted"",
                ""Assessment results inconclusive""]",               
                LinkedQuestion = 0,
                OnAction = null,
                RenderType = (long)RenderTypeIds.RadioButton, // RadioButton
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            },
            new QuestionBank
            {
                Id = 28,
                RowId = Guid.Parse("4d89a2b4-0e45-4383-b1d1-3d8a74cfa5e9"),
                Description = "E&C Action Tracker ID",
                IsMandatory = false,
                Options = null,
                LinkedQuestion = 0,
                OnAction = null,
                RenderType = (long)RenderTypeIds.TextArea, // TextArea
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            },
            new QuestionBank
            {
                Id = 29,
                RowId = Guid.Parse("a3b12d7c-42c1-4ed3-9273-cc8e6b64a2f3"),
                Description = "Conclusion",
                IsMandatory = false,
                Options = null,
                LinkedQuestion = 0,
                OnAction = null,
                RenderType = (long)RenderTypeIds.TextArea, // TextArea
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            }
        );


    }

}
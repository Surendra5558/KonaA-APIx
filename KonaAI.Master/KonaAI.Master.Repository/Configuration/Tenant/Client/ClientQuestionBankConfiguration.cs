using KonaAI.Master.Repository.Common.Constants;
using KonaAI.Master.Repository.Common.Extensions;
using KonaAI.Master.Repository.Domain.Tenant.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KonaAI.Master.Repository.Configuration.Tenant.Client;

/// <summary>
/// Configures the EF Core entity mapping for <see cref="ClientQuestionBank"/>.
/// </summary>
public class ClientQuestionBankConfiguration : IEntityTypeConfiguration<ClientQuestionBank>
{
    /// <summary>
    /// Configures the <see cref="ClientQuestionBank"/> entity.
    /// </summary>
    /// <param name="builder">
    /// The <see cref="EntityTypeBuilder{ClientQuestionBank}"/> used to configure the entity.
    /// </param>
    public void Configure(EntityTypeBuilder<ClientQuestionBank> builder)
    {
        // Applies base client configuration, including schema and common columns
        builder.BaseClientConfiguration("ClientQuestionBank");

        // Ensure QuestionId is required since it should always reference a valid question
        builder.Property(x => x.QuestionBankId)
               .IsRequired();

        builder.HasData(
            new ClientQuestionBank
            {
                Id = 1,
                RowId = Guid.Parse("b1e14a9e-239a-4f45-bc77-1cbdfc1a6f21"),
                ClientId = 1,
                QuestionBankId = 1,
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            },
            new ClientQuestionBank
            {
                Id = 2,
                RowId = Guid.Parse("7a72a1b8-463d-4b76-baf2-23de841fdf69"),
                ClientId = 1,
                QuestionBankId = 2,
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            },
            new ClientQuestionBank
            {
                Id = 3,
                RowId = Guid.Parse("e36c777d-c0d3-4ec1-85b3-2493acb7ce6a"),
                ClientId = 1,
                QuestionBankId = 3,
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            },
            new ClientQuestionBank
            {
                Id = 4,
                RowId = Guid.Parse("50a87e11-357d-47d4-8228-48aab24ed411"),
                ClientId = 1,
                QuestionBankId = 4,
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            },
            new ClientQuestionBank
            {
                Id = 5,
                RowId = Guid.Parse("30ef5d7a-4485-4c3d-8f2d-22c510b4b74e"),
                ClientId = 1,
                QuestionBankId = 5,
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            },
            new ClientQuestionBank
            {
                Id = 6,
                RowId = Guid.Parse("a09d0eb1-83b1-4742-b799-95a933e7c08b"),
                ClientId = 1,
                QuestionBankId = 6,
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            },
            new ClientQuestionBank
            {
                Id = 7,
                RowId = Guid.Parse("df2b827b-847a-4b5e-bac4-7b8949eeb48e"),
                ClientId = 1,
                QuestionBankId = 7,
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            },
            new ClientQuestionBank
            {
                Id = 8,
                RowId = Guid.Parse("de4236a2-f1a3-46b4-9686-32b0d3a6d0b0"),
                ClientId = 1,
                QuestionBankId = 8,
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            },
            new ClientQuestionBank
            {
                Id = 9,
                RowId = Guid.Parse("cbcc4b26-2ef1-4967-9d90-1c2f74a68cf2"),
                ClientId = 1,
                QuestionBankId = 9,
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            },
            new ClientQuestionBank
            {
                Id = 10,
                RowId = Guid.Parse("ee9171d8-0a9b-41e5-bc08-517569d548cb"),
                ClientId = 1,
                QuestionBankId = 10,
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            },
             new ClientQuestionBank
             {
                 Id = 11,
                 RowId = Guid.Parse("2c8a5b0a-5c88-44ff-90db-12c8dc8a1a60"),
                 ClientId = 1,
                 QuestionBankId = 11,
                 CreatedBy = "System",
                 CreatedById = 1,
                 ModifiedBy = "System",
                 ModifiedById = 1
             },
            new ClientQuestionBank
            {
                Id = 12,
                RowId = Guid.Parse("f0a9d41c-21b2-4706-a4a0-5e47d4fcb765"),
                ClientId = 1,
                QuestionBankId = 12,
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            },
            new ClientQuestionBank
            {
                Id = 13,
                RowId = Guid.Parse("a5b23b07-28a4-4a73-9fd9-650d3b7de248"),
                ClientId = 1,
                QuestionBankId = 13,
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            },
            new ClientQuestionBank
            {
                Id = 14,
                RowId = Guid.Parse("d1faea5b-8176-4a55-bfa4-d4bca6b2db5c"),
                ClientId = 1,
                QuestionBankId = 14,
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            },
            new ClientQuestionBank
            {
                Id = 15,
                RowId = Guid.Parse("a87ffb49-1c27-4b94-86ff-45b68a5f84d2"),
                ClientId = 1,
                QuestionBankId = 15,
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            },
            new ClientQuestionBank
            {
                Id = 16,
                RowId = Guid.Parse("9cfe50b2-7c24-468b-9913-25119ef4e84b"),
                ClientId = 1,
                QuestionBankId = 16,
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            },
            new ClientQuestionBank
            {
                Id = 17,
                RowId = Guid.Parse("5b2b1e7a-f1a1-4b75-8c2a-1ab95b302ee4"),
                ClientId = 1,
                QuestionBankId = 17,
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            },
            new ClientQuestionBank
            {
                Id = 18,
                RowId = Guid.Parse("53d2b7d3-26c4-4721-b5a9-c2f9b2c5b26a"),
                ClientId = 1,
                QuestionBankId = 18,
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            },
            new ClientQuestionBank
            {
                Id = 19,
                RowId = Guid.Parse("b3df94aa-5d0e-47ea-9f58-8a272f1633bb"),
                ClientId = 1,
                QuestionBankId = 19,
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            },
            new ClientQuestionBank
            {
                Id = 20,
                RowId = Guid.Parse("bb42e301-40b5-4a72-9d83-29d8d4f3df15"),
                ClientId = 1,
                QuestionBankId = 20,
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            },
            new ClientQuestionBank
            {
                Id = 21,
                RowId = Guid.Parse("6e4f6d50-8f8e-4ec1-b4bb-5175d63754a3"),
                ClientId = 1,
                QuestionBankId = 21,
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            },
            new ClientQuestionBank
            {
                Id = 22,
                RowId = Guid.Parse("f84f1839-934b-4b8e-b648-13f02e2f3214"),
                ClientId = 1,
                QuestionBankId = 22,
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            },
            new ClientQuestionBank
            {
                Id = 23,
                RowId = Guid.Parse("d707f3cf-4073-44d3-8a3e-7e69fba98de0"),
                ClientId = 1,
                QuestionBankId = 23,
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            },
            new ClientQuestionBank
            {
                Id = 24,
                RowId = Guid.Parse("208c6f49-0c07-4a6a-93d9-14b5ab6b0eb4"),
                ClientId = 1,
                QuestionBankId = 24,
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            },
            new ClientQuestionBank
            {
                Id = 25,
                RowId = Guid.Parse("0b0b046f-cf0e-4ce2-a7e2-9fa5f2bcb73b"),
                ClientId = 1,
                QuestionBankId = 25,
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            },
            new ClientQuestionBank
            {
                Id = 26,
                RowId = Guid.Parse("ad35ad4f-7e4c-40df-9b4f-508993828f89"),
                ClientId = 1,
                QuestionBankId = 26,
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            },
            new ClientQuestionBank
            {
                Id = 27,
                RowId = Guid.Parse("c46eb4f0-3b5d-45f1-9852-d9f6b9f98d11"),
                ClientId = 1,
                QuestionBankId = 27,
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            },
            new ClientQuestionBank
            {
                Id = 28,
                RowId = Guid.Parse("1d78a276-cd93-403b-90f3-2b35188245a4"),
                ClientId = 1,
                QuestionBankId = 28,
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            },
            new ClientQuestionBank
            {
                Id = 29,
                RowId = Guid.Parse("b61a59c2-85b3-4c3b-9458-1e5d9ec6abf1"),
                ClientId = 1,
                QuestionBankId = 29,
                CreatedBy = "System",
                CreatedById = 1,
                ModifiedBy = "System",
                ModifiedById = 1
            }

        );

    }
}
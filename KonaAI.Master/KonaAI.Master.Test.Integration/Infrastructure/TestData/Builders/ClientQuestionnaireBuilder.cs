using KonaAI.Master.Repository.Domain.Tenant.Client;
using Bogus;

namespace KonaAI.Master.Test.Integration.Infrastructure.TestData.Builders;

/// <summary>
/// Builder for creating ClientQuestionnaire test data.
/// </summary>
public class ClientQuestionnaireBuilder
{
    private long _clientId = 1;
    private string _name = "Test Questionnaire";
    private string _description = "Test questionnaire for integration testing";
    private bool _isActive = true;

    private static readonly Faker _faker = new Faker();

    public static ClientQuestionnaireBuilder Create() => new();

    public static ClientQuestionnaireBuilder CreateForClient(long clientId) => new ClientQuestionnaireBuilder().WithClientId(clientId);

    public ClientQuestionnaireBuilder WithClientId(long clientId)
    {
        _clientId = clientId;
        return this;
    }

    public ClientQuestionnaireBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public ClientQuestionnaireBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }

    public ClientQuestionnaireBuilder Active()
    {
        _isActive = true;
        return this;
    }

    public ClientQuestionnaireBuilder Inactive()
    {
        _isActive = false;
        return this;
    }

    public ClientQuestionnaire Build()
    {
        return new ClientQuestionnaire
        {
            RowId = Guid.NewGuid(),
            ClientId = _clientId,
            Name = _name,
            IsActive = _isActive,
            CreatedOn = DateTime.UtcNow,
            CreatedBy = "testuser",
            ModifiedOn = DateTime.UtcNow,
            ModifiedBy = "testuser"
        };
    }

    /// <summary>
    /// Creates default client questionnaires for testing.
    /// </summary>
    public static List<ClientQuestionnaire> CreateDefaults()
    {
        return new List<ClientQuestionnaire>
        {
            Create().WithName("Employee Survey").WithDescription("Employee satisfaction survey").Build(),
            Create().WithName("Customer Feedback").WithDescription("Customer feedback questionnaire").Build(),
            Create().WithName("Risk Assessment").WithDescription("Risk assessment questionnaire").Build(),
            Create().WithName("Compliance Check").WithDescription("Compliance check questionnaire").Build()
        };
    }

    /// <summary>
    /// Creates multiple questionnaires for a specific client.
    /// </summary>
    public List<ClientQuestionnaire> CreateMultiple(int count)
    {
        var questionnaires = new List<ClientQuestionnaire>();

        for (int i = 1; i <= count; i++)
        {
            var questionnaire = WithName($"Questionnaire {i}")
                .WithDescription($"Test questionnaire {i} for client {_clientId}")
                .Build();

            questionnaires.Add(questionnaire);
        }

        return questionnaires;
    }
}

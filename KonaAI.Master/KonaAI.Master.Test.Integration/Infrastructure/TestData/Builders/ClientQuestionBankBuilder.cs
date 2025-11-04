using KonaAI.Master.Repository.Domain.Tenant.Client;
using Bogus;

namespace KonaAI.Master.Test.Integration.Infrastructure.TestData.Builders;

/// <summary>
/// Builder for creating ClientQuestionBank test data.
/// </summary>
public class ClientQuestionBankBuilder
{
    private long _clientId = 1;
    private long _questionBankId = 1;
    private string _name = "Test Question Bank";
    private string _description = "Test question bank for integration testing";
    private bool _isActive = true;

    private static readonly Faker _faker = new Faker();

    public static ClientQuestionBankBuilder Create() => new();

    public static ClientQuestionBankBuilder CreateForClient(long clientId) => new ClientQuestionBankBuilder().WithClientId(clientId);

    public ClientQuestionBankBuilder WithClientId(long clientId)
    {
        _clientId = clientId;
        return this;
    }

    public ClientQuestionBankBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public ClientQuestionBankBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }

    public ClientQuestionBankBuilder Active()
    {
        _isActive = true;
        return this;
    }

    public ClientQuestionBankBuilder Inactive()
    {
        _isActive = false;
        return this;
    }

    public ClientQuestionBank Build()
    {
        return new ClientQuestionBank
        {
            RowId = Guid.NewGuid(),
            ClientId = _clientId,
            QuestionBankId = _questionBankId,
            IsActive = _isActive,
            CreatedOn = DateTime.UtcNow,
            CreatedBy = "testuser",
            ModifiedOn = DateTime.UtcNow,
            ModifiedBy = "testuser"
        };
    }

    /// <summary>
    /// Creates default client question banks for testing.
    /// </summary>
    public static List<ClientQuestionBank> CreateDefaults()
    {
        return new List<ClientQuestionBank>
        {
            Create().WithName("General Questions").WithDescription("General question bank").Build(),
            Create().WithName("Technical Questions").WithDescription("Technical question bank").Build(),
            Create().WithName("Behavioral Questions").WithDescription("Behavioral question bank").Build(),
            Create().WithName("Compliance Questions").WithDescription("Compliance question bank").Build()
        };
    }

    /// <summary>
    /// Creates multiple question banks for a specific client.
    /// </summary>
    public List<ClientQuestionBank> CreateMultiple(int count)
    {
        var questionBanks = new List<ClientQuestionBank>();

        for (int i = 1; i <= count; i++)
        {
            var questionBank = WithName($"Question Bank {i}")
                .WithDescription($"Test question bank {i} for client {_clientId}")
                .Build();

            questionBanks.Add(questionBank);
        }

        return questionBanks;
    }
}

using KonaAI.Master.Model.Master.App.SaveModel;
using Bogus;

namespace KonaAI.Master.Test.Integration.Infrastructure.TestData.Builders;

/// <summary>
/// Builder for creating QuestionBankCreateModel test data.
/// </summary>
public class QuestionBankCreateModelBuilder
{
    private string _description = "Test Question Bank Description";
    private bool _isMandatory = true;
    private string[] _options = new[] { "Option 1", "Option 2", "Option 3" };
    private long _linkedQuestion = 6; // Changed from 0 to 6 to meet validation requirements
    private string _onAction = "test-action";
    private bool _isDefault = false;

    private static readonly Faker _faker = new Faker();

    public static QuestionBankCreateModelBuilder Create() => new();

    public QuestionBankCreateModelBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }

    public QuestionBankCreateModelBuilder WithIsMandatory(bool isMandatory)
    {
        _isMandatory = isMandatory;
        return this;
    }

    public QuestionBankCreateModelBuilder WithOptions(string[] options)
    {
        _options = options;
        return this;
    }

    public QuestionBankCreateModelBuilder WithLinkedQuestion(long linkedQuestion)
    {
        _linkedQuestion = linkedQuestion;
        return this;
    }

    public QuestionBankCreateModelBuilder WithOnAction(string onAction)

    {
        _onAction = onAction;
        return this;
    }

    public QuestionBankCreateModelBuilder WithIsDefault(bool isDefault)
    {
        _isDefault = isDefault;
        return this;
    }

    public QuestionBankCreateModelBuilder Mandatory()
    {
        _isMandatory = true;
        return this;
    }

    public QuestionBankCreateModelBuilder Optional()
    {
        _isMandatory = false;
        return this;
    }

    public QuestionBankCreateModelBuilder AsDefault()
    {
        _isDefault = true;
        return this;
    }

    public QuestionBankCreateModelBuilder WithRandomData()
    {
        _description = _faker.Lorem.Sentence(5);
        _isMandatory = _faker.Random.Bool();
        _options = _faker.Lorem.Words(3).ToArray();
        _linkedQuestion = _faker.Random.Long(6, 100); // Start from 6 to meet validation requirements
        _onAction = _faker.Lorem.Word();
        _isDefault = _faker.Random.Bool();
        return this;
    }

    public QuestionBankCreateModel Build()
    {
        return new QuestionBankCreateModel
        {
            Description = _description,
            IsMandatory = _isMandatory,
            Options = _options,
            LinkedQuestion = _linkedQuestion,
            OnAction = _onAction,
            IsDefault = _isDefault
        };
    }

    /// <summary>
    /// Creates a default valid QuestionBankCreateModel.
    /// </summary>
    public static QuestionBankCreateModel CreateDefault()
    {
        return Create()
            .WithDescription(_faker.Lorem.Sentence(5))
            .WithIsMandatory(_faker.Random.Bool())
            .WithOptions(_faker.Lorem.Words(3).ToArray())
            .WithLinkedQuestion(_faker.Random.Long(6, 100)) // Start from 6 to meet validation requirements
            .WithOnAction(_faker.Lorem.Word())
            .WithIsDefault(_faker.Random.Bool())
            .Build();
    }

    /// <summary>
    /// Creates default question bank create models for testing.
    /// </summary>
    public static List<QuestionBankCreateModel> CreateDefaults()
    {
        return new List<QuestionBankCreateModel>
        {
            Create()
                .WithDescription("General Questions")
                .WithOptions(new[] { "Yes", "No", "Maybe" })
                .WithOnAction("general-action")
                .Build(),

            Create()
                .WithDescription("Technical Questions")
                .WithOptions(new[] { "Excellent", "Good", "Fair", "Poor" })
                .WithOnAction("technical-action")
                .Mandatory()
                .Build(),

            Create()
                .WithDescription("Behavioral Questions")
                .WithOptions(new[] { "Always", "Sometimes", "Never" })
                .WithOnAction("behavioral-action")
                .AsDefault()
                .Build(),

            Create()
                .WithDescription("Compliance Questions")
                .WithOptions(new[] { "Compliant", "Non-Compliant" })
                .WithOnAction("compliance-action")
                .Build()
        };
    }

    /// <summary>
    /// Creates multiple question bank create models.
    /// </summary>
    public List<QuestionBankCreateModel> CreateMultiple(int count)
    {
        var questionBanks = new List<QuestionBankCreateModel>();

        for (int i = 1; i <= count; i++)
        {
            var questionBank = WithDescription($"Test Question Bank {i}")
                .WithOptions(new[] { $"Option A{i}", $"Option B{i}", $"Option C{i}" })
                .WithOnAction($"action-{i}")
                .Build();

            questionBanks.Add(questionBank);
        }

        return questionBanks;
    }
}

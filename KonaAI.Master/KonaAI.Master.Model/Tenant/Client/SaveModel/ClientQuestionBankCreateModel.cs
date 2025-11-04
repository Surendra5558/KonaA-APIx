using FluentValidation;
using KonaAI.Master.Model.Common.Constants;

namespace KonaAI.Master.Model.Tenant.Client.SaveModel;

/// <summary>
/// Represents the data required to create a question in the Client Question Bank.
/// </summary>
/// <remarks>
/// Validation highlights:
/// - <see cref="Text"/> is required and limited to <see cref="DbColumnLength.Description"/> characters.
/// - <see cref="Type"/> is required and must be one of: Text, Dropdown, Checkbox, Radio, Date (case-insensitive).
/// - <see cref="Description"/> is optional, limited to <see cref="DbColumnLength.Description"/> characters.
/// - <see cref="Options"/> is required for choice types (Dropdown, Checkbox, Radio) and must contain only non-empty values.
/// - <see cref="ClientId"/> must be greater than 0.
/// </remarks>
public class ClientQuestionBankCreateModel
{
    /// <summary>
    /// Gets or sets the question text or label displayed to the user.
    /// </summary>
    /// <value>
    /// A non-empty string with a maximum length defined by <see cref="DbColumnLength.Description"/>.
    /// </value>
    public string Text { get; set; } = null!;

    /// <summary>
    /// Gets or sets the type of the question.
    /// </summary>
    /// <remarks>
    /// Allowed values (case-insensitive): Text, Dropdown, Checkbox, Radio, Date.
    /// Choice types are: Dropdown, Checkbox, Radio (see <see cref="Options"/> requirement).
    /// </remarks>
    public string Type { get; set; } = null!;

    /// <summary>
    /// Gets or sets a value indicating whether this question is mandatory.
    /// </summary>
    public bool Required { get; set; }

    /// <summary>
    /// Gets or sets an optional description or help text for the question.
    /// </summary>
    /// <value>When provided, limited to <see cref="DbColumnLength.Description"/> characters.</value>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the list of selectable options (applicable for choice-based questions).
    /// </summary>
    /// <remarks>
    /// Required for choice types (Dropdown, Checkbox, Radio).
    /// Must contain at least one non-empty option; empty or whitespace-only values are not allowed.
    /// </remarks>
    public List<string>? Options { get; set; }

    /// <summary>
    /// Gets or sets the list of rules or conditions associated with the question.
    /// </summary>
    /// <remarks>
    /// Each rule is validated via <see cref="QuestionRuleModelValidator"/>.
    /// </remarks>
    public List<QuestionRuleModel>? Rules { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the client who owns this question.
    /// </summary>
    /// <value>Must be greater than 0.</value>
    public long ClientId { get; set; }
}

/// <summary>
/// Represents conditional logic or rules for a question.
/// </summary>
/// <remarks>
/// A rule optionally defines a set of <see cref="Conditions"/> and a resulting <see cref="ThenAction"/> to apply
/// when all conditions are satisfied. The <see cref="CanAddMore"/> flag can be used by UI flows to indicate
/// whether additional rules can be appended for the same question.
/// </remarks>
public class QuestionRuleModel
{
    /// <summary>
    /// Gets or sets the list of conditions that trigger an action.
    /// </summary>
    /// <remarks>All entries must be non-empty if provided.</remarks>
    public List<string>? Conditions { get; set; }

    /// <summary>
    /// Gets or sets the action performed when conditions are met.
    /// </summary>
    /// <remarks>Limited to <see cref="DbColumnLength.Description"/> characters when provided.</remarks>
    public string? ThenAction { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether additional rules can be added.
    /// </summary>
    public bool CanAddMore { get; set; }
}

/// <summary>
/// Validator for <see cref="ClientQuestionBankCreateModel"/> enforcing field requirements and constraints.
/// </summary>
/// <remarks>
/// Rules enforced:
/// - Text: required, max length <see cref="DbColumnLength.Description"/>.
/// - Type: required, must be a known type (Text, Dropdown, Checkbox, Radio, Date).
/// - Description: optional, max length <see cref="DbColumnLength.Description"/>.
/// - Options: required and non-empty for choice types; no empty/whitespace items allowed.
/// - ClientId: must be greater than 0.
/// - Rules: each item validated by <see cref="QuestionRuleModelValidator"/>.
/// </remarks>
public class ClientQuestionBankCreateModelValidator : AbstractValidator<ClientQuestionBankCreateModel>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ClientQuestionBankCreateModelValidator"/> class.
    /// </summary>
    public ClientQuestionBankCreateModelValidator()
    {
        RuleFor(x => x.Text)
            .NotEmpty().WithMessage("Question text is required.")
            .MaximumLength(DbColumnLength.Description).WithMessage("Question text cannot exceed 1000 characters.");

        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("Question type is required.")
            .Must(BeAValidType).WithMessage("Invalid question type. Allowed types: Text, Dropdown, Checkbox, Radio, Date.");

        RuleFor(x => x.Description)
            .MaximumLength(DbColumnLength.Description).WithMessage("Description cannot exceed 1000 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));

        RuleFor(x => x.ClientId)
            .GreaterThan(0).WithMessage("ClientId must be a valid non-zero value.");

        RuleForEach(x => x.Rules)
            .SetValidator(new QuestionRuleModelValidator())
            .When(x => x.Rules != null && x.Rules.Any());
    }

    /// <summary>
    /// Determines whether the provided question type is allowed.
    /// </summary>
    /// <param name="type">The question type to validate.</param>
    /// <returns><see langword="true"/> if the type is one of the allowed values; otherwise, <see langword="false"/>.</returns>
    private bool BeAValidType(string type)
    {
        var allowed = new[] { "TextArea", "CheckBox", "RadioButton", "HyperLink", "Table", "Label" };
        return allowed.Contains(type, StringComparer.OrdinalIgnoreCase);
    }
}

/// <summary>
/// Validator for <see cref="QuestionRuleModel"/> enforcing rule and action constraints.
/// </summary>
/// <remarks>
/// - <see cref="QuestionRuleModel.Conditions"/>: if provided, no empty or whitespace strings allowed.
/// - <see cref="QuestionRuleModel.ThenAction"/>: optional, max length <see cref="DbColumnLength.Description"/>.
/// </remarks>
public class QuestionRuleModelValidator : AbstractValidator<QuestionRuleModel>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="QuestionRuleModelValidator"/> class.
    /// </summary>
    public QuestionRuleModelValidator()
    {
        RuleFor(x => x.Conditions)
            .Must(c => c == null || c.All(cond => !string.IsNullOrWhiteSpace(cond)))
            .WithMessage("Conditions cannot contain empty strings.");

        RuleFor(x => x.ThenAction)
            .MaximumLength(DbColumnLength.Description).WithMessage("ThenAction cannot exceed 1000 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.ThenAction));
    }
}
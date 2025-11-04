using FluentValidation;
using KonaAI.Master.Model.Common.Constants;

namespace KonaAI.Master.Model.Master.App.SaveModel;

/// <summary>
/// Represents a question bank definition that can be created in the system.
/// </summary>
public class QuestionBankCreateModel
{
    /// <summary>
    /// Gets or sets the description of the question bank.
    /// </summary>
    public string Description { get; set; } = null!;

    /// <summary>
    /// Gets or sets a value indicating whether the question is mandatory.
    /// </summary>
    public bool IsMandatory { get; set; }

    /// <summary>
    /// Gets or sets the available options for the question (e.g., multiple-choice values).
    /// </summary>
    public string[]? Options { get; set; }

    /// <summary>
    /// Gets or sets the identifier of a question that is logically linked to this one.
    /// </summary>
    public long LinkedQuestion { get; set; }

    /// <summary>
    /// Gets or sets the action or condition to be executed when the question is answered.
    /// </summary>
    public string? OnAction { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this question is marked as the default choice.
    /// </summary>
    public bool IsDefault { get; set; }
}

public class QuestionBankCreateModelValidator : AbstractValidator<QuestionBankCreateModel>
{
    public QuestionBankCreateModelValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(DbColumnLength.Description).WithMessage("Description cannot exceed 500 characters.");

        RuleFor(x => x.IsMandatory)
            .NotNull().WithMessage("IsMandatory flag is required.");

        RuleFor(x => x.Options)
            .NotEmpty().WithMessage("Options are required.");

        RuleFor(x => x.LinkedQuestion)
            .GreaterThanOrEqualTo(0).WithMessage("LinkedQuestion must be >= 0.");

        // OnAction is optional; only enforce max length when provided
        RuleFor(x => x.OnAction)
            .NotEmpty().WithMessage("OnAction is required.");

        RuleFor(x => x.IsDefault)
            .NotNull().WithMessage("IsDefault flag is required.");
    }
}
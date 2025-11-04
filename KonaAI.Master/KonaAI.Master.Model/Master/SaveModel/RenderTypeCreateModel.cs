using FluentValidation;
using KonaAI.Master.Model.Common.Constants;

namespace KonaAI.Master.Model.Master.SaveModel;

/// <summary>
/// Represents the data required to create a new render type.
/// </summary>
public class RenderTypeCreateModel
{
    /// <summary>
    /// Gets or sets the name of the render type.
    /// </summary>
    /// <remarks>
    /// This field is required and must not exceed 100 characters.
    /// </remarks>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Gets or sets the description of the render type.
    /// </summary>
    /// <remarks>
    /// This field is required and must not exceed 250 characters.
    /// </remarks>
    public string Description { get; set; } = null!;

    /// <summary>
    /// Gets or sets the display order of the render type.
    /// </summary>
    /// <remarks>
    /// This field determines the sequence in which render types are displayed.
    /// </remarks>
    public long OrderBy { get; set; }
}

/// <summary>
/// Validator for <see cref="RenderTypeCreateModel"/>.
/// Ensures that required fields are provided and constraints are enforced.
/// </summary>
public class RenderTypeCreateModelValidator : AbstractValidator<RenderTypeCreateModel>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RenderTypeCreateModelValidator"/> class.
    /// </summary>
    public RenderTypeCreateModelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(DbColumnLength.NameEmail).WithMessage("Name must not exceed 100 characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(DbColumnLength.Description).WithMessage("Description must not exceed 250 characters.");

        RuleFor(x => x.OrderBy)
            .GreaterThanOrEqualTo(0).WithMessage("OrderBy must be greater than or equal to 0.");
    }
}
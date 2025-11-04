using FluentValidation;
using KonaAI.Master.Model.Common.Constants;

namespace KonaAI.Master.Model.Master.App.SaveModel;

/// <summary>
/// Represents the data model for updating client information.
/// </summary>
public class ClientUpdateModel
{
    /// <summary>
    /// Gets or sets the unique identifier for the client.
    /// </summary>
    public Guid RowId { get; set; }

    /// <summary>
    /// Gets or sets the name of the client.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Gets or sets the display name of the client.
    /// </summary>
    public string DisplayName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the client code.
    /// </summary>
    public string ClientCode { get; set; } = null!;

    /// <summary>
    /// Gets or sets the description of the client.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the name of the contact person for the client.
    /// </summary>
    public string? ContactName { get; set; }

    /// <summary>
    /// Gets or sets the address of the client.
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// Gets or sets the email address of the client.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Gets or sets the phone number of the client.
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// Gets or sets the country code associated with the client.
    /// </summary>
    public string? CountryCode { get; set; }
}

/// <summary>
/// Validator for <see cref="ClientUpdateModel"/> that enforces business rules and data constraints.
/// </summary>
public class ClientUpdateModelValidator : AbstractValidator<ClientUpdateModel>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ClientUpdateModelValidator"/> class.
    /// Sets up validation rules for updating client information.
    /// </summary>
    public ClientUpdateModelValidator()
    {
        RuleFor(x => x.RowId)
            .NotEqual(Guid.Empty)
            .WithMessage("RowId is required");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MaximumLength(DbColumnLength.NameEmail)
            .WithMessage($"Name cannot exceed {DbColumnLength.NameEmail}");

        RuleFor(x => x.DisplayName)
            .NotEmpty()
            .WithMessage("Display Name is required")
            .MaximumLength(DbColumnLength.NameEmail)
            .WithMessage($"Display Name cannot exceed {DbColumnLength.NameEmail}");

        RuleFor(x => x.ClientCode)
            .NotEmpty()
            .WithMessage("Client Code is required")
            .MaximumLength(DbColumnLength.Identifier)
            .WithMessage($"Client Code cannot exceed {DbColumnLength.Identifier}");

        RuleFor(x => x.Description)
            .MaximumLength(DbColumnLength.Description)
            .WithMessage($"Description cannot exceed {DbColumnLength.Description}");

        RuleFor(x => x.ContactName)
            .MaximumLength(DbColumnLength.NameEmail)
            .WithMessage($"Contact Name cannot exceed {DbColumnLength.NameEmail}");

        RuleFor(x => x.Address)
            .MaximumLength(DbColumnLength.Address)
            .WithMessage($"Address cannot exceed {DbColumnLength.Address}");

        RuleFor(x => x.Email)
            .MaximumLength(DbColumnLength.NameEmail)
            .WithMessage($"Email cannot exceed {DbColumnLength.NameEmail}")
            .EmailAddress()
            .When(x => !string.IsNullOrWhiteSpace(x.Email))
            .WithMessage("A valid email address is required.");

        RuleFor(x => x.Phone)
            .MaximumLength(DbColumnLength.PhoneNumber)
            .WithMessage($"Phone cannot exceed {DbColumnLength.PhoneNumber}");

        RuleFor(x => x.CountryCode)
            .MaximumLength(DbColumnLength.CountryCode)
            .WithMessage($"Country Code cannot exceed {DbColumnLength.CountryCode}");
    }
}
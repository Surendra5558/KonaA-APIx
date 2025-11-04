using FluentValidation;
using KonaAI.Master.Model.Common.Constants;

namespace KonaAI.Master.Model.Master.App.SaveModel;

/// <summary>
/// Represents the data required to create a new client, including identity, contact, and profile information.
/// </summary>
public class ClientCreateModel
{
    /// <summary>
    /// Gets or sets the unique name of the client.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Gets or sets the display name of the client.
    /// </summary>
    public string DisplayName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the code used to identify the client.
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

    /// <summary>
    /// Gets or sets the logo image for the client as a byte array.
    /// </summary>
    public byte[]? Logo { get; set; } = [];

    /// <summary>
    /// License start date of the client
    /// </summary>
    public DateTime? LicenseStartDate { get; set; }

    /// <summary>
    /// License end date of the client
    /// </summary>
    public DateTime? LicenseEndDate { get; set; }
}

/// <summary>
/// Provides validation rules for the <see cref="ClientCreateModel"/> to ensure data integrity and business requirements.
/// </summary>
public class ClientValidator : AbstractValidator<ClientCreateModel>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ClientValidator"/> class and defines validation rules for <see cref="ClientCreateModel"/> properties.
    /// </summary>
    public ClientValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Client Name is required")
            .MaximumLength(DbColumnLength.NameEmail)
            .WithMessage($"Client Name cannot exceed {DbColumnLength.NameEmail}");

        RuleFor(x => x.DisplayName)
            .NotEmpty()
            .WithMessage("Client Display Name is required")
            .MaximumLength(DbColumnLength.NameEmail)
            .WithMessage($"Client Display Name cannot exceed {DbColumnLength.NameEmail}");

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

        RuleFor(x => x.Logo)
            .Must(logo => logo is not { Length: > 1_000_000 })
            .WithMessage("Logo cannot exceed 1,000,000 bytes.");
    }
}
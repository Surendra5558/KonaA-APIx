using FluentValidation;
using KonaAI.Master.Model.Common.Constants;

namespace KonaAI.Master.Model.Tenant.Client.BaseModel;

/// <summary>
/// Common base for user create/update models.
/// </summary>
public abstract class ClientUserBaseModel
{
    /// <summary>
    /// User name for login.
    /// </summary>
    public string? UserName { get; set; }

    /// <summary>
    /// Email address of the user.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Password for the user account.
    /// </summary>
    public string? Password { get; set; }

    /// <summary>
    /// Firstname of the user.
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// Lastname of the user.
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    /// Phone number of the user.
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Country code for the phone number.
    /// </summary>
    public string? PhoneNumberCountryCode { get; set; }

    /// <summary>
    /// LogOn type identifier (e.g., standard, SSO).
    /// </summary>
    public int? LogOnTypeId { get; set; }

    /// <summary>
    /// Is password reset requested for the user.
    /// </summary>
    public bool? IsResetRequested { get; set; }

    /// <summary>
    /// Role type identifier for the user.
    /// </summary>
    public long? RoleTypeId { get; set; }
}

/// <summary>
/// Base validator for user models (create/update).
/// </summary>
public class ClientUserBaseValidator<T> : AbstractValidator<T>
    where T : ClientUserBaseModel
{
    public ClientUserBaseValidator()
    {
        RuleFor(x => x.UserName)
            .MaximumLength(DbColumnLength.NameEmail).When(x => !string.IsNullOrEmpty(x.UserName))
            .WithMessage($"User Name cannot exceed {DbColumnLength.NameEmail}");

        RuleFor(x => x.Email)
            .EmailAddress().When(x => !string.IsNullOrEmpty(x.Email))
            .WithMessage("Invalid email format")
            .MaximumLength(DbColumnLength.NameEmail).When(x => !string.IsNullOrEmpty(x.Email))
            .WithMessage($"Email cannot exceed {DbColumnLength.NameEmail}");

        RuleFor(x => x.Password)
            .MinimumLength(DbColumnLength.Password).When(x => !string.IsNullOrEmpty(x.Password))
            .WithMessage($"Password must be at least {DbColumnLength.Password} characters long");

        RuleFor(x => x.FirstName)
            .MaximumLength(DbColumnLength.NameEmail).When(x => !string.IsNullOrEmpty(x.FirstName))
            .WithMessage($"First Name cannot exceed {DbColumnLength.NameEmail}");

        RuleFor(x => x.LastName)
            .MaximumLength(DbColumnLength.NameEmail).When(x => !string.IsNullOrEmpty(x.LastName))
            .WithMessage($"Last Name cannot exceed {DbColumnLength.NameEmail}");

        RuleFor(x => x.PhoneNumber)
            .Matches(Constants.PhoneNumberRegex).When(x => !string.IsNullOrEmpty(x.PhoneNumber))
            .WithMessage("Invalid Phone Number");

        RuleFor(x => x.PhoneNumberCountryCode)
            .MaximumLength(DbColumnLength.CountryCode).When(x => !string.IsNullOrEmpty(x.PhoneNumberCountryCode))
            .WithMessage("Phone Number Country Code cannot exceed 5 characters");

        RuleFor(x => x.LogOnTypeId)
            .GreaterThan(0).When(x => x.LogOnTypeId.HasValue)
            .WithMessage("LogOnTypeId must be greater than 0 if provided");

        RuleFor(x => x.RoleTypeId)
            .GreaterThan(0).When(x => x.RoleTypeId.HasValue)
            .WithMessage("RoleTypeId must be greater than 0 if provided");
    }
}
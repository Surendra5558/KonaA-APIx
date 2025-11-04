using KonaAI.Master.Repository.Domain.Master.App;
using Bogus;

namespace KonaAI.Master.Test.Integration.Infrastructure.TestData.Builders;

/// <summary>
/// Builder for creating User test data with authentication credentials.
/// </summary>
public class UserBuilder
{
    private string _userName = "testuser@konaai.com";
    private string _email = "testuser@konaai.com";
    private string _password = "Test@123456";
    private string _firstName = "Test";
    private string _lastName = "User";
    private string? _phoneNumber = "+1234567890";
    private long _roleTypeId = 1; // Default role
    private long _logOnTypeId = 1; // Default logon type
    private bool _isActive = true;
    private bool _isResetRequested = false;

    private static readonly Faker _faker = new Faker();

    public static UserBuilder Create() => new();

    public UserBuilder WithUserName(string userName)
    {
        _userName = userName;
        return this;
    }

    public UserBuilder WithEmail(string email)
    {
        _email = email;
        return this;
    }

    public UserBuilder WithPassword(string password)
    {
        _password = password;
        return this;
    }

    public UserBuilder WithName(string firstName, string lastName)
    {
        _firstName = firstName;
        _lastName = lastName;
        return this;
    }

    public UserBuilder WithRole(long roleTypeId)
    {
        _roleTypeId = roleTypeId;
        return this;
    }

    public UserBuilder WithLogOnType(long logOnTypeId)
    {
        _logOnTypeId = logOnTypeId;
        return this;
    }

    public UserBuilder Active()
    {
        _isActive = true;
        return this;
    }

    public UserBuilder Inactive()
    {
        _isActive = false;
        return this;
    }

    public UserBuilder WithResetRequested()
    {
        _isResetRequested = true;
        return this;
    }

    public User Build()
    {
        return new User
        {
            RowId = Guid.NewGuid(),
            UserName = _userName,
            Email = _email,
            Password = BCrypt.Net.BCrypt.HashPassword(_password), // Hash password for authentication
            FirstName = _firstName,
            LastName = _lastName,
            PhoneNumber = _phoneNumber,
            PhoneNumberCountryCode = "+1",
            RoleTypeId = _roleTypeId,
            LogOnTypeId = _logOnTypeId,
            IsActive = _isActive,
            IsResetRequested = _isResetRequested,
            CreatedOn = DateTime.UtcNow,
            CreatedBy = "testsystem",
            ModifiedOn = DateTime.UtcNow,
            ModifiedBy = "testsystem"
        };
    }

    /// <summary>
    /// Creates default test users with various roles for testing authentication.
    /// </summary>
    public static List<User> CreateDefaults()
    {
        return new List<User>
        {
            // Primary test user for most authentication tests
            Create()
                .WithUserName("testuser@konaai.com")
                .WithEmail("testuser@konaai.com")
                .WithPassword("Test@123456")
                .WithName("Test", "User")
                .WithRole(1)
                .Build(),
            
            // Admin user for admin-level tests
            Create()
                .WithUserName("admin@konaai.com")
                .WithEmail("admin@konaai.com")
                .WithPassword("Admin@123456")
                .WithName("Admin", "User")
                .WithRole(1) // Admin role
                .Build(),
            
            // Manager user for role-based tests
            Create()
                .WithUserName("manager@konaai.com")
                .WithEmail("manager@konaai.com")
                .WithPassword("Manager@123456")
                .WithName("Manager", "User")
                .WithRole(2) // Manager role
                .Build(),
            
            // Regular user for standard tests
            Create()
                .WithUserName("user@konaai.com")
                .WithEmail("user@konaai.com")
                .WithPassword("User@123456")
                .WithName("Regular", "User")
                .WithRole(3) // User role
                .Build(),
            
            // Inactive user for status tests
            Create()
                .WithUserName("inactive@konaai.com")
                .WithEmail("inactive@konaai.com")
                .WithPassword("Inactive@123456")
                .WithName("Inactive", "User")
                .WithRole(3)
                .Inactive()
                .Build()
        };
    }
}


using KonaAI.Master.Repository.Domain.Tenant.Client;
using Bogus;

namespace KonaAI.Master.Test.Integration.Infrastructure.TestData.Builders;

/// <summary>
/// Builder for creating ClientUser test data.
/// </summary>
public class ClientUserBuilder
{
    private long _clientId = 1;
    private long _userId = 1;
    private string _name = "Test User";
    private string _email = "test@example.com";
    private string _role = "User";
    private bool _isActive = true;

    private static readonly Faker _faker = new Faker();

    public static ClientUserBuilder Create() => new();

    public ClientUserBuilder WithClientId(long clientId)
    {
        _clientId = clientId;
        return this;
    }

    public ClientUserBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public ClientUserBuilder WithEmail(string email)
    {
        _email = email;
        return this;
    }

    public ClientUserBuilder WithRole(string role)
    {
        _role = role;
        return this;
    }

    public ClientUserBuilder Active()
    {
        _isActive = true;
        return this;
    }

    public ClientUserBuilder Inactive()
    {
        _isActive = false;
        return this;
    }

    public ClientUser Build()
    {
        return new ClientUser
        {
            RowId = Guid.NewGuid(),
            ClientId = _clientId,
            UserId = _userId,
            IsActive = _isActive,
            CreatedOn = DateTime.UtcNow,
            CreatedBy = "testuser",
            ModifiedOn = DateTime.UtcNow,
            ModifiedBy = "testuser"
        };
    }

    /// <summary>
    /// Creates default client users for testing.
    /// </summary>
    public static List<ClientUser> CreateDefaults()
    {
        return new List<ClientUser>
        {
            Create().WithName("Admin User").WithEmail("admin@test.com").WithRole("Administrator").Build(),
            Create().WithName("Manager User").WithEmail("manager@test.com").WithRole("Manager").Build(),
            Create().WithName("Regular User").WithEmail("user@test.com").WithRole("User").Build(),
            Create().WithName("Auditor User").WithEmail("auditor@test.com").WithRole("Auditor").Build()
        };
    }
}

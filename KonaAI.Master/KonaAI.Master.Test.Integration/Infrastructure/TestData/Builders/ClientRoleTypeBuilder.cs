using KonaAI.Master.Repository.Domain.Tenant.ClientMetaData;

namespace KonaAI.Master.Test.Integration.Infrastructure.TestData.Builders;

/// <summary>
/// Builder for creating ClientRoleType test data.
/// Links roles to clients for authentication and authorization.
/// </summary>
public class ClientRoleTypeBuilder
{
    private long _clientId = 1;
    private long _roleTypeId = 1;
    private string _name = "Test Role";
    private bool _isActive = true;

    public static ClientRoleTypeBuilder Create() => new();

    public ClientRoleTypeBuilder WithClientId(long clientId)
    {
        _clientId = clientId;
        return this;
    }

    public ClientRoleTypeBuilder WithRoleTypeId(long roleTypeId)
    {
        _roleTypeId = roleTypeId;
        return this;
    }

    public ClientRoleTypeBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public ClientRoleTypeBuilder Active()
    {
        _isActive = true;
        return this;
    }

    public ClientRoleTypeBuilder Inactive()
    {
        _isActive = false;
        return this;
    }

    public ClientRoleType Build()
    {
        return new ClientRoleType
        {
            RowId = Guid.NewGuid(),
            ClientId = _clientId,
            RoleTypeId = _roleTypeId,
            Name = _name,
            IsActive = _isActive,
            CreatedOn = DateTime.UtcNow,
            CreatedBy = "testsystem",
            ModifiedOn = DateTime.UtcNow,
            ModifiedBy = "testsystem"
        };
    }

    /// <summary>
    /// Creates default client role types linking the first two clients to all role types.
    /// </summary>
    public static List<ClientRoleType> CreateDefaults()
    {
        return new List<ClientRoleType>
        {
            // Client 1 roles
            Create().WithClientId(1).WithRoleTypeId(1).WithName("Admin").Build(),
            Create().WithClientId(1).WithRoleTypeId(2).WithName("Manager").Build(),
            Create().WithClientId(1).WithRoleTypeId(3).WithName("User").Build(),
            
            // Client 2 roles
            Create().WithClientId(2).WithRoleTypeId(1).WithName("Admin").Build(),
            Create().WithClientId(2).WithRoleTypeId(2).WithName("Manager").Build(),
            Create().WithClientId(2).WithRoleTypeId(3).WithName("User").Build()
        };
    }
}



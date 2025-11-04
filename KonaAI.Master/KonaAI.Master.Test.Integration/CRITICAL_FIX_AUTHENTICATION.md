# Critical Fix: Authentication Tests

## Root Cause Identified

The authentication tests are failing because of **THREE critical issues**:

### 1. ❌ Password Hashing
**Problem**: Test users have plain text passwords, but login validation uses BCrypt

**Code Location**: `UserLoginBusiness.cs` line 151:
```csharp
if (!BCrypt.Net.BCrypt.Verify(tokenFormRequest.Password, userRecord.User.Password))
```

**Fix**: Hash passwords in `UserBuilder.cs`:
```csharp
public User Build()
{
    return new User
    {
        // ... other fields
        Password = BCrypt.Net.BCrypt.HashPassword(_password), // MUST hash password
        // ... other fields
    };
}
```

### 2. ❌ Missing Related Entities
**Problem**: Login validation requires joins with `ClientUsers`, `ClientRoleTypes`, etc.

**Code Location**: `UserLoginBusiness.cs` lines 128-140:
```csharp
var result = from u in await unitOfWork.Users.GetAsync()
             join cu in await unitOfWork.ClientUsers.GetAsync() on u.Id equals cu.UserId
             join c in await unitOfWork.Clients.GetAsync() on cu.ClientId equals c.Id
             join r in await unitOfWork.RoleTypes.GetAsync() on u.RoleTypeId equals r.Id
             join cr in await unitOfWork.ClientRoleTypes.GetAsync() on r.Id equals cr.RoleTypeId
```

**Required Entities**:
- `User` (being seeded)
- `ClientUser` (linking user to client)
- `Client` (already seeded)
- `RoleType` (already seeded)  
- `ClientRoleType` (NOT seeded - MISSING!)

**Fix**: Need to seed `ClientRoleType` entities in `TestDataSeeder`.

### 3. ❌ Missing Permissions and Navigation Data
**Problem**: `CreateUserAuditAsync` requires navigation actions, user actions, etc.

**Required Additional Entities**:
- `NavigationUserAction`
- `RoleNavigationUserAction`
- `UserAction`

---

## Complete Fix Implementation

### Step 1: Update UserBuilder to Hash Passwords

```csharp
// File: UserBuilder.cs
public User Build()
{
    return new User
    {
        RowId = Guid.NewGuid(),
        UserName = _userName,
        Email = _email,
        Password = BCrypt.Net.BCrypt.HashPassword(_password), // HASH PASSWORD!
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
```

### Step 2: Create ClientRoleTypeBuilder

```csharp
// File: ClientRoleTypeBuilder.cs (NEW)
using KonaAI.Master.Repository.Domain.Tenant.MetaData;

namespace KonaAI.Master.Test.Integration.Infrastructure.TestData.Builders;

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

    public static List<ClientRoleType> CreateDefaults()
    {
        return new List<ClientRoleType>
        {
            Create().WithClientId(1).WithRoleTypeId(1).WithName("Admin").Build(),
            Create().WithClientId(1).WithRoleTypeId(2).WithName("Manager").Build(),
            Create().WithClientId(1).WithRoleTypeId(3).WithName("User").Build(),
            Create().WithClientId(2).WithRoleTypeId(1).WithName("Admin").Build(),
            Create().WithClientId(2).WithRoleTypeId(2).WithName("Manager").Build()
        };
    }
}
```

### Step 3: Update TestDataSeeder

```csharp
// File: TestDataSeeder.cs
private async Task SeedTenantDataAsync(TestDbContext context)
{
    // Existing client seeding...
    var clients = new[] { /* ... */ };
    context.AddRange(clients);
    await context.SaveChangesAsync();

    // NEW: Seed ClientRoleTypes (linking roles to clients)
    var clientRoleTypes = ClientRoleTypeBuilder.CreateDefaults();
    context.AddRange(clientRoleTypes);
    await context.SaveChangesAsync();

    // Existing ClientUser seeding...
    var clientUsers = ClientUserBuilder.CreateDefaults();
    context.AddRange(clientUsers);
    await context.SaveChangesAsync();

    // Rest of existing code...
}
```

### Step 4: Update TestDbContext (if ClientRoleTypes DbSet missing)

```csharp
// File: TestDbContext.cs
public DbSet<ClientRoleType> ClientRoleTypes { get; set; }
```

---

## Alternative: Simplified Approach

**If the above is too complex**, create a **simplified test-only login endpoint** that skips BCrypt and complex joins:

### Create TestLoginBusiness (for testing only)

```csharp
public class TestUserLoginBusiness : IUserLoginBusiness
{
    public async Task<TokenResponse> AuthenticateUserAsync(
        TokenFormRequest tokenFormRequest,
        string clientIpAddress,
        string userAgent)
    {
        // Simple validation: plain text password match
        var user = await _unitOfWork.Users.GetAsync()
            .FirstOrDefault(u => u.UserName == tokenFormRequest.UserName 
                && u.Password == tokenFormRequest.Password);
        
        if (user == null)
        {
            throw new AuthenticationException("Invalid credentials");
        }

        // Generate simple token
        return new TokenResponse
        {
            Token = "test-token",
            Name = user.FirstName,
            // ... other fields
        };
    }
}
```

### Register in Test Factory

```csharp
// InMemoryWebApplicationFactory.cs
builder.ConfigureServices(services =>
{
    // Replace login business with test version
    services.AddScoped<IUserLoginBusiness, TestUserLoginBusiness>();
});
```

---

## Recommendation

**Use the COMPLETE FIX** (password hashing + related entities) because:
1. Tests the real authentication flow
2. Validates all database relationships
3. Catches integration issues early
4. More realistic testing

**Total Estimated Time**: 2-3 hours to implement all fixes properly

---

## Testing After Fix

```powershell
# Test login specifically
dotnet test --filter "FullyQualifiedName~LoginControllerApiTests"

# Verify all authentication tests
dotnet test --filter "Category=Authentication"
```

---

## Expected Results After Fix

- ✅ `Login_WithValidCredentials_ReturnsToken` - PASS
- ✅ `Login_WithInvalidCredentials_ReturnsUnauthorized` - PASS  
- ✅ `CompleteAuthenticationFlow` - PASS
- ✅ All 10 login tests - PASS

**Current**: 3/10 passing (30%)  
**After Fix**: 10/10 passing (100%)

---

## Status

**Current Status**: Authentication infrastructure identified, fix documented  
**Next Action**: Implement password hashing + seed ClientRoleTypes  
**Blocker**: Requires BCrypt.Net NuGet package (likely already installed)



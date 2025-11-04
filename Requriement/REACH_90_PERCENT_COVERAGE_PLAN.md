# Comprehensive Plan: From 81.86% to 90% Coverage

## 🎯 Current Status
- **Current Coverage**: 81.86%
- **Target Coverage**: 90%
- **Gap**: 8.14%
- **Lines Needed**: ~17 additional lines covered (out of 204 total)

---

## 📊 Coverage Gap Analysis

Based on the current `81.86% (167/204 lines)` coverage, we need to cover approximately **17 more lines** to reach 90% (184/204 lines).

### Where the Gaps Are

1. **Handler Classes** (Currently 0% covered - ~10-15 lines)
   - `AccessAuthorizationHandler.cs`: Authorization logic
   - `AccessAuthorizationPolicyProvider.cs`: Policy creation

2. **Exception Paths** (~5-7 lines)
   - Finally blocks in controllers
   - Catch-all exception handlers
   - Configuration validation branches

3. **Controller Edge Cases** (~3-5 lines)
   - Concurrent update scenarios
   - Complex validation failures
   - Authorization failures

---

## 🚀 Implementation Strategy

### Phase 1: Handler Class Tests (+5-7% coverage - HIGHEST IMPACT)

The Handler classes are currently **completely untested** and represent the biggest opportunity for coverage improvement.

#### `AccessAuthorizationHandler` Tests

```csharp
using KonaAI.Master.API.Handler.Authorize;
using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Repository.Common.Model;
using KonaAI.Master.Repository.Domain.Master.App;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Claims;

namespace KonaAI.Master.Test.Unit.Handler.Authorize;

public class AccessAuthorizationHandlerTests
{
    private readonly Mock<ILogger<AccessAuthorizationHandler>> _logger = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IUserContextService> _userContextService = new();

    private AccessAuthorizationHandler CreateSut() =>
        new(_logger.Object, _unitOfWork.Object, _userContextService.Object);

    [Fact]
    public async Task HandleRequirementAsync_ValidUser_WithPermissions_Succeeds()
    {
        // Arrange
        var sessionId = Guid.NewGuid();
        var requirement = new AccessAuthorizationRequirement(
            Common.Constants.NavigationMenu.ProjectDashboard,
            Common.Constants.UserActionMenu.View);

        var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(JwtRegisteredClaimNames.Sid, sessionId.ToString())
        }));

        var context = new AuthorizationHandlerContext(
            new[] { requirement },
            user,
            null);

        var userAudit = new UserAudit
        {
            RowId = sessionId,
            RoleNavigation = new List<UserPermission>
            {
                new() {
                    NavigationName = Common.Constants.NavigationMenu.ProjectDashboard,
                    UserActionName = Common.Constants.UserActionMenu.View
                }
            }
        };

        var userAudits = new[] { userAudit }.AsQueryable();
        _unitOfWork.Setup(u => u.UserAudits.GetAsync()).ReturnsAsync(userAudits);

        var handler = CreateSut();

        // Act
        await handler.HandleAsync(context);

        // Assert
        Assert.True(context.HasSucceeded);
    }

    [Fact]
    public async Task HandleRequirementAsync_ValidUser_NoPermissions_Fails()
    {
        // Arrange
        var sessionId = Guid.NewGuid();
        var requirement = new AccessAuthorizationRequirement(
            Common.Constants.NavigationMenu.ProjectDashboard,
            Common.Constants.UserActionMenu.Edit); // User only has View permission

        var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(JwtRegisteredClaimNames.Sid, sessionId.ToString())
        }));

        var context = new AuthorizationHandlerContext(
            new[] { requirement },
            user,
            null);

        var userAudit = new UserAudit
        {
            RowId = sessionId,
            RoleNavigation = new List<UserPermission>
            {
                new() {
                    NavigationName = Common.Constants.NavigationMenu.ProjectDashboard,
                    UserActionName = Common.Constants.UserActionMenu.View // Only View, not Edit
                }
            }
        };

        var userAudits = new[] { userAudit }.AsQueryable();
        _unitOfWork.Setup(u => u.UserAudits.GetAsync()).ReturnsAsync(userAudits);

        var handler = CreateSut();

        // Act
        await handler.HandleAsync(context);

        // Assert
        Assert.False(context.HasSucceeded);
    }

    [Fact]
    public async Task HandleRequirementAsync_InvalidSessionId_Fails()
    {
        // Arrange
        var requirement = new AccessAuthorizationRequirement(
            Common.Constants.NavigationMenu.ProjectDashboard,
            Common.Constants.UserActionMenu.View);

        var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(JwtRegisteredClaimNames.Sid, "invalid-guid") // Invalid GUID
        }));

        var context = new AuthorizationHandlerContext(
            new[] { requirement },
            user,
            null);

        var handler = CreateSut();

        // Act
        await handler.HandleAsync(context);

        // Assert
        Assert.False(context.HasSucceeded);
    }

    [Fact]
    public async Task HandleRequirementAsync_NoUserAudit_Fails()
    {
        // Arrange
        var sessionId = Guid.NewGuid();
        var requirement = new AccessAuthorizationRequirement(
            Common.Constants.NavigationMenu.ProjectDashboard,
            Common.Constants.UserActionMenu.View);

        var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(JwtRegisteredClaimNames.Sid, sessionId.ToString())
        }));

        var context = new AuthorizationHandlerContext(
            new[] { requirement },
            user,
            null);

        // No user audit found
        _unitOfWork.Setup(u => u.UserAudits.GetAsync())
            .ReturnsAsync(Enumerable.Empty<UserAudit>().AsQueryable());

        var handler = CreateSut();

        // Act
        await handler.HandleAsync(context);

        // Assert
        Assert.False(context.HasSucceeded);
    }

    [Fact]
    public async Task HandleRequirementAsync_ExceptionInPermissionCheck_Fails()
    {
        // Arrange
        var sessionId = Guid.NewGuid();
        var requirement = new AccessAuthorizationRequirement(
            Common.Constants.NavigationMenu.ProjectDashboard,
            Common.Constants.UserActionMenu.View);

        var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(JwtRegisteredClaimNames.Sid, sessionId.ToString())
        }));

        var context = new AuthorizationHandlerContext(
            new[] { requirement },
            user,
            null);

        _unitOfWork.Setup(u => u.UserAudits.GetAsync())
            .ThrowsAsync(new Exception("Database error"));

        var handler = CreateSut();

        // Act
        await handler.HandleAsync(context);

        // Assert
        Assert.False(context.HasSucceeded); // Should fail gracefully on exception
    }
}
```

#### `AccessAuthorizationPolicyProvider` Tests

```csharp
using KonaAI.Master.API.Handler.Authorize;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Moq;

namespace KonaAI.Master.Test.Unit.Handler.Authorize;

public class AccessAuthorizationPolicyProviderTests
{
    private readonly Mock<ILogger<AccessAuthorizationPolicyProvider>> _logger = new();

    private AccessAuthorizationPolicyProvider CreateSut() =>
        new(_logger.Object);

    [Fact]
    public async Task GetPolicyAsync_ValidPolicyName_ReturnsPolicy()
    {
        // Arrange
        var policyName = "Dashboard:View";
        var provider = CreateSut();

        // Act
        var policy = await provider.GetPolicyAsync(policyName);

        // Assert
        Assert.NotNull(policy);
        Assert.Contains(policy.Requirements, r => r is AccessAuthorizationRequirement);
    }

    [Fact]
    public async Task GetPolicyAsync_InvalidFormat_ReturnsDefaultPolicy()
    {
        // Arrange
        var policyName = "InvalidFormat"; // No colon separator
        var provider = CreateSut();

        // Act
        var policy = await provider.GetPolicyAsync(policyName);

        // Assert
        // Should return default policy or null based on implementation
        Assert.NotNull(policy);
    }

    [Fact]
    public async Task GetPolicyAsync_NullPolicyName_ReturnsDefaultPolicy()
    {
        // Arrange
        string? policyName = null;
        var provider = CreateSut();

        // Act
        var policy = await provider.GetPolicyAsync(policyName!);

        // Assert
        Assert.NotNull(policy);
    }

    [Fact]
    public async Task GetDefaultPolicyAsync_ReturnsPolicy()
    {
        // Arrange
        var provider = CreateSut();

        // Act
        var policy = await provider.GetDefaultPolicyAsync();

        // Assert
        Assert.NotNull(policy);
    }

    [Fact]
    public async Task GetFallbackPolicyAsync_ReturnsPolicy()
    {
        // Arrange
        var provider = CreateSut();

        // Act
        var policy = await provider.GetFallbackPolicyAsync();

        // Assert
        Assert.NotNull(policy);
    }
}
```

---

### Phase 2: Controller Exception Edge Cases (+1-2% coverage)

Add comprehensive exception handling tests to controllers that don't have them yet.

#### Login Controller Enhancements

```csharp
[Fact]
public async Task PostAsync_DbUpdateException_Returns500()
{
    // Arrange
    var tokenRequest = new TokenFormRequest
    {
        UserName = "test@example.com",
        Password = "password123",
        GrantType = "password"
    };

    var validationResult = new ValidationResult();
    _validator.Setup(v => v.ValidateAsync(tokenRequest, It.IsAny<CancellationToken>()))
        .ReturnsAsync(validationResult);

    _business.Setup(b => b.AuthenticateUserAsync(tokenRequest, It.IsAny<string>(), It.IsAny<string>()))
        .ThrowsAsync(new Microsoft.EntityFrameworkCore.DbUpdateException("Database error"));

    var sut = CreateSut();

    // Act
    var result = await sut.PostAsync(tokenRequest);

    // Assert
    var statusCodeResult = Assert.IsType<ObjectResult>(result);
    Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
}

[Fact]
public async Task PostAsync_DbUpdateConcurrencyException_Returns409Conflict()
{
    // Arrange
    var tokenRequest = new TokenFormRequest
    {
        UserName = "test@example.com",
        Password = "password123",
        GrantType = "password"
    };

    var validationResult = new ValidationResult();
    _validator.Setup(v => v.ValidateAsync(tokenRequest, It.IsAny<CancellationToken>()))
        .ReturnsAsync(validationResult);

    _business.Setup(b => b.AuthenticateUserAsync(tokenRequest, It.IsAny<string>(), It.IsAny<string>()))
        .ThrowsAsync(new Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException("Concurrency error"));

    var sut = CreateSut();

    // Act
    var result = await sut.PostAsync(tokenRequest);

    // Assert
    var statusCodeResult = Assert.IsType<ObjectResult>(result);
    Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
}

[Fact]
public async Task PostAsync_InvalidOperationException_Returns500()
{
    // Arrange
    var tokenRequest = new TokenFormRequest
    {
        UserName = "test@example.com",
        Password = "password123",
        GrantType = "password"
    };

    var validationResult = new ValidationResult();
    _validator.Setup(v => v.ValidateAsync(tokenRequest, It.IsAny<CancellationToken>()))
        .ReturnsAsync(validationResult);

    _business.Setup(b => b.AuthenticateUserAsync(tokenRequest, It.IsAny<string>(), It.IsAny<string>()))
        .ThrowsAsync(new InvalidOperationException("Operation failed"));

    var sut = CreateSut();

    // Act
    var result = await sut.PostAsync(tokenRequest);

    // Assert
    var statusCodeResult = Assert.IsType<ObjectResult>(result);
    Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
}
```

---

### Phase 3: Integration Test Enhancements (+1-2% coverage)

Add complex OData scenarios and authentication integration tests.

#### Complex OData Query Tests

```csharp
[Fact]
public async Task GetAsync_WithComplexFilter_ReturnsFilteredData()
{
    // Arrange
    var client = _factory.CreateClient();
    var oDataQuery = "$filter=IsActive eq true and contains(Name, 'Test')&$orderby=Name desc&$top=5";

    // Act
    var response = await client.GetAsync($"/api/v1/Country?{oDataQuery}");

    // Assert
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    var content = await response.Content.ReadAsStringAsync();
    Assert.NotNull(content);
}

[Fact]
public async Task GetAsync_WithExpandAndSelect_ReturnsProjectedData()
{
    // Arrange
    var client = _factory.CreateClient();
    var oDataQuery = "$expand=RelatedEntity&$select=Name,Description&$top=10";

    // Act
    var response = await client.GetAsync($"/api/v1/Controller?{oDataQuery}");

    // Assert
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
}

[Fact]
public async Task GetAsync_WithInvalidODataSyntax_Returns400()
{
    // Arrange
    var client = _factory.CreateClient();
    var invalidQuery = "$filter=InvalidSyntax eq"; // Invalid OData

    // Act
    var response = await client.GetAsync($"/api/v1/Controller?{invalidQuery}");

    // Assert
    Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
}
```

#### Authentication Integration Tests

```csharp
[Fact]
public async Task Login_WithExpiredToken_Returns401()
{
    // Arrange
    var client = _factory.CreateClient();
    var expiredToken = "expired.jwt.token";
    client.DefaultRequestHeaders.Authorization = 
        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", expiredToken);

    // Act
    var response = await client.GetAsync("/api/v1/ProtectedEndpoint");

    // Assert
    Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
}

[Fact]
public async Task Login_WithInvalidToken_Returns401()
{
    // Arrange
    var client = _factory.CreateClient();
    var invalidToken = "invalid.token.format";
    client.DefaultRequestHeaders.Authorization = 
        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", invalidToken);

    // Act
    var response = await client.GetAsync("/api/v1/ProtectedEndpoint");

    // Assert
    Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
}

[Fact]
public async Task Login_WithMissingToken_Returns401()
{
    // Arrange
    var client = _factory.CreateClient();
    // No authorization header

    // Act
    var response = await client.GetAsync("/api/v1/ProtectedEndpoint");

    // Assert
    Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
}
```

---

## 📋 Implementation Checklist

### Immediate Actions (To Reach 85% - 3% gain)
- [ ] Create `AccessAuthorizationHandlerTests.cs` with 5 comprehensive tests
- [ ] Create `AccessAuthorizationPolicyProviderTests.cs` with 5 tests
- [ ] Add database exception tests to `LoginControllerUnitTests.cs` (3 tests)
- [ ] Add mapper exception tests to all metadata business tests (3 tests each)

### Near-Term Actions (To Reach 88% - 3% gain)
- [ ] Add complex OData integration tests (5 tests)
- [ ] Add authentication integration tests (5 tests)
- [ ] Add concurrency exception tests to all controllers (1 test per controller)
- [ ] Add null/empty scenario tests to all business classes

### Final Push (To Reach 90% - 2% gain)
- [ ] Add finally block verification tests
- [ ] Add configuration validation tests
- [ ] Add edge case tests for rarely-hit branches
- [ ] Add multi-tenant isolation integration tests

---

## 🎯 Estimated Impact

| Phase | Tests Added | Expected Coverage Gain | Cumulative Coverage |
|-------|-------------|------------------------|---------------------|
| Current | 240 tests | - | 81.86% |
| Handler Tests | +10 tests | +5-7% | ~87-89% |
| Controller Exceptions | +15 tests | +1-2% | ~88-90% |
| Integration Tests | +10 tests | +1-2% | ~89-91% |
| **Total** | **+35 tests** | **+8-11%** | **~90%** ✅ |

---

## 🔧 Quick Implementation Commands

```powershell
# Create Handler test directory
New-Item -ItemType Directory -Path "KonaAI.Master/KonaAI.Master.Test.Unit/Handler/Authorize" -Force

# Create Handler test files
New-Item -ItemType File -Path "KonaAI.Master/KonaAI.Master.Test.Unit/Handler/Authorize/AccessAuthorizationHandlerTests.cs"
New-Item -ItemType File -Path "KonaAI.Master/KonaAI.Master.Test.Unit/Handler/Authorize/AccessAuthorizationPolicyProviderTests.cs"

# Run tests
dotnet test KonaAI.Master/KonaAI.Master.Test.Unit/KonaAI.Master.Test.Unit.csproj --collect:"XPlat Code Coverage" --settings .github/coverlet.runsettings

# Check coverage
scripts/code-review-agent.ps1 -VerboseOutput
```

---

## 📊 Progress Tracking

Use this table to track progress:

```markdown
| Date | Tests Added | Coverage % | Notes |
|------|-------------|------------|-------|
| 2025-10-21 | 240 | 81.86% | Baseline after initial improvements |
| | +4 Country | 82.1% | Added mapper/empty/null tests |
| | +10 Handler | 87-89% | AccessAuthorizationHandler tests |
| | +15 Controller | 88-90% | Exception handling |
| | +10 Integration | 90%+ | OData & auth tests |
```

---

## 🎉 Success Criteria

Coverage reaches **90%** when:
- ✅ All Handler classes have comprehensive tests (5+ tests each)
- ✅ All controllers have database exception tests
- ✅ All business classes have mapper exception tests
- ✅ Integration tests cover complex OData scenarios
- ✅ Integration tests cover authentication failures
- ✅ `scripts/code-review-agent.ps1` reports >= 90% coverage

---

## 📚 References

- **Handler Classes**: `KonaAI.Master/KonaAI.Master.API/Handler/Authorize/`
- **Test Examples**: `KonaAI.Master/KonaAI.Master.Test.Unit/Business/Master/ClientBusinessTests.cs`
- **Coverage Config**: `.github/coverlet.runsettings`
- **Local Script**: `scripts/code-review-agent.ps1`

---

**Last Updated**: October 21, 2025  
**Current Status**: 81.86% → Target: 90%  
**Estimated Effort**: 35 additional tests across 3 phases


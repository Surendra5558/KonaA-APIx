using KonaAI.Master.Business.Authentication.Logic;
using KonaAI.Master.Model.Authentication;
using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Repository.DataAccess.Master.App.Interface;
using KonaAI.Master.Repository.DataAccess.Master.MetaData.Interface;
using KonaAI.Master.Repository.DataAccess.Tenant.Client.Interface;
using KonaAI.Master.Repository.DataAccess.Tenant.ClientMetaData.Interface;
using KonaAI.Master.Repository.Domain.Master.App;
using KonaAI.Master.Repository.Domain.Master.MetaData;
using KonaAI.Master.Repository.Domain.Tenant.Client;
using KonaAI.Master.Repository.Domain.Tenant.ClientMetaData;
using KonaAI.Master.Repository.Domain.Tenant.ClientUserMetaData;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Authentication;
using KonaAI.Master.Repository.Common.Model;

namespace KonaAI.Master.Test.Unit.Business.Authentication;

/// <summary>
/// Unit tests for <see cref="UserLoginBusiness"/>.
/// Covers:
/// - Authentication success and failure scenarios
/// - Token generation and validation
/// - User audit creation
/// - Exception handling for invalid credentials and configuration errors
/// </summary>
public class UserLoginBusinessTests
{
    private readonly Mock<ILogger<UserLoginBusiness>> _logger = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IUserContextService> _userContextService = new();
    private readonly Mock<ILicenseService> _licenseService = new();
    private readonly Mock<IConfiguration> _configuration = new();
    private readonly Mock<IUserRepository> _userRepo = new();
    private readonly Mock<IClientUserRepository> _clientUserRepo = new();
    private readonly Mock<IClientRepository> _clientRepo = new();
    private readonly Mock<IRoleTypeRepository> _roleTypeRepo = new();
    private readonly Mock<IClientRoleTypeRepository> _clientRoleTypeRepo = new();
    private readonly Mock<IUserAuditRepository> _userAuditRepo = new();
    private readonly Mock<IRoleNavigationUserActionRepository> _roleNavigationUserActionRepo = new();
    private readonly Mock<INavigationUserActionRepository> _navigationUserActionRepo = new();
    private readonly Mock<INavigationRepository> _navigationRepo = new();
    private readonly Mock<IUserActionRepository> _userActionRepo = new();
    private readonly Mock<IClientProjectRepository> _clientProjectRepo = new();
    private readonly Mock<IClientProjectUserRepository> _clientProjectUserRepo = new();

    public UserLoginBusinessTests()
    {
        // Setup UoW repository mocks
        _unitOfWork.SetupGet(u => u.Users).Returns(_userRepo.Object);
        _unitOfWork.SetupGet(u => u.ClientUsers).Returns(_clientUserRepo.Object);
        _unitOfWork.SetupGet(u => u.Clients).Returns(_clientRepo.Object);
        _unitOfWork.SetupGet(u => u.RoleTypes).Returns(_roleTypeRepo.Object);
        _unitOfWork.SetupGet(u => u.ClientRoleTypes).Returns(_clientRoleTypeRepo.Object);
        _unitOfWork.SetupGet(u => u.UserAudits).Returns(_userAuditRepo.Object);
        _unitOfWork.SetupGet(u => u.RoleNavigationUserActions).Returns(_roleNavigationUserActionRepo.Object);
        _unitOfWork.SetupGet(u => u.NavigationUserActions).Returns(_navigationUserActionRepo.Object);
        _unitOfWork.SetupGet(u => u.Navigations).Returns(_navigationRepo.Object);
        _unitOfWork.SetupGet(u => u.UserActions).Returns(_userActionRepo.Object);
        _unitOfWork.SetupGet(u => u.ClientProjects).Returns(_clientProjectRepo.Object);
        _unitOfWork.SetupGet(u => u.ClientProjectUser).Returns(_clientProjectUserRepo.Object);

        // Setup UserContextService mock with a concrete UserContext instance
        _userContextService.Setup(ucs => ucs.UserContext).Returns(
            new KonaAI.Master.Repository.Common.Model.UserContext()
        );

        // Setup configuration
        _configuration.Setup(c => c["Tokens:AccessTokenExpiryInMinutes"]).Returns("60");
        _configuration.Setup(c => c["Tokens:Issuer"]).Returns("TestIssuer");
        _configuration.Setup(c => c["Tokens:Audience"]).Returns("TestAudience");
        _configuration.Setup(c => c["Tokens:Key"]).Returns("ThisIsAVeryLongSecretKeyForTestingPurposesOnly123456789");
    }

    private UserLoginBusiness CreateSut() =>
        new(_logger.Object, _unitOfWork.Object, _userContextService.Object, _licenseService.Object, _configuration.Object);

    #region AuthenticateUserAsync

    [Fact]
    public async Task AuthenticateUserAsync_ValidCredentials_ReturnsTokenResponse()
    {
        // Arrange
        var tokenRequest = new TokenFormRequest
        {
            UserName = "test@example.com",
            Password = "password123",
            GrantType = "password"
        };

        var user = new User
        {
            Id = 1,
            RowId = Guid.NewGuid(),
            UserName = "test@example.com",
            FirstName = "Test",
            LastName = "User",
            Password = BCrypt.Net.BCrypt.HashPassword("password123"),
            IsActive = true,
            RoleTypeId = 1
        };

        var clientUser = new ClientUser { UserId = 1, ClientId = 1 };
        var client = new Client { Id = 1, Name = "TestClient" };
        var roleType = new RoleType { Id = 1, RowId = Guid.NewGuid(), Name = "Admin" };
        var clientRoleType = new ClientRoleType { RoleTypeId = 1, ClientId = 1 };

        _userRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { user }.AsQueryable()));
        _clientUserRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { clientUser }.AsQueryable()));
        _clientRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { client }.AsQueryable()));
        _roleTypeRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { roleType }.AsQueryable()));
        _clientRoleTypeRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { clientRoleType }.AsQueryable()));

        // Setup audit-related mocks
        _roleNavigationUserActionRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(Array.Empty<RoleNavigationUserAction>().AsQueryable()));
        _navigationUserActionRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(Array.Empty<NavigationUserAction>().AsQueryable()));
        _navigationRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(Array.Empty<Navigation>().AsQueryable()));
        _userActionRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(Array.Empty<UserAction>().AsQueryable()));
        _clientProjectRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(Array.Empty<ClientProject>().AsQueryable()));
        _clientProjectUserRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(Array.Empty<ClientProjectUser>().AsQueryable()));

        _userAuditRepo.Setup(r => r.AddAsync(It.IsAny<UserAudit>())).ReturnsAsync(new UserAudit());
        _unitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var sut = CreateSut();

        // Act
        var result = await sut.AuthenticateUserAsync(tokenRequest, "127.0.0.1", "TestAgent");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test User", result.Name);
        Assert.NotNull(result.Token);
        Assert.NotNull(result.RefreshToken);
        Assert.Equal(1, result.RoleId);
        Assert.Equal("Admin", result.RoleName);
        Assert.Equal(1, result.ClientId);
        Assert.Equal("TestClient", result.ClientName);
    }

    [Fact]
    public async Task AuthenticateUserAsync_InvalidUsername_ThrowsAuthenticationException()
    {
        // Arrange
        var tokenRequest = new TokenFormRequest
        {
            UserName = "nonexistent@example.com",
            Password = "password123",
            GrantType = "password"
        };

        _userRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(Array.Empty<User>().AsQueryable()));
        _clientUserRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(Array.Empty<ClientUser>().AsQueryable()));
        _clientRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(Array.Empty<Client>().AsQueryable()));
        _roleTypeRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(Array.Empty<RoleType>().AsQueryable()));
        _clientRoleTypeRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(Array.Empty<ClientRoleType>().AsQueryable()));

        var sut = CreateSut();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<AuthenticationException>(
            () => sut.AuthenticateUserAsync(tokenRequest, "127.0.0.1", "TestAgent"));

        Assert.Equal("Invalid username or password.", exception.Message);
    }

    [Fact]
    public async Task AuthenticateUserAsync_InvalidPassword_ReturnsTokenResponse()
    {
        // Arrange
        var tokenRequest = new TokenFormRequest
        {
            UserName = "test@example.com",
            Password = "wrongpassword",
            GrantType = "password"
        };

        var user = new User
        {
            Id = 1,
            RowId = Guid.NewGuid(),
            UserName = "test@example.com",
            FirstName = "Test",
            LastName = "User",
            Password = BCrypt.Net.BCrypt.HashPassword("password123"),
            IsActive = true,
            RoleTypeId = 1
        };

        var clientUser = new ClientUser { UserId = 1, ClientId = 1 };
        var client = new Client { Id = 1, Name = "TestClient" };
        var roleType = new RoleType { Id = 1, Name = "Admin" };
        var clientRoleType = new ClientRoleType { RoleTypeId = 1, ClientId = 1 };

        _userRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { user }.AsQueryable()));
        _clientUserRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { clientUser }.AsQueryable()));
        _clientRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { client }.AsQueryable()));
        _roleTypeRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { roleType }.AsQueryable()));
        _clientRoleTypeRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { clientRoleType }.AsQueryable()));

        var sut = CreateSut();

        // Act & Assert: invalid password should raise AuthenticationException
        await Assert.ThrowsAsync<System.Security.Authentication.AuthenticationException>(
            () => sut.AuthenticateUserAsync(tokenRequest, "127.0.0.1", "TestAgent"));
    }

    [Fact]
    public async Task AuthenticateUserAsync_MissingTokenConfiguration_ThrowsInvalidOperationException()
    {
        // Arrange
        var tokenRequest = new TokenFormRequest
        {
            UserName = "test@example.com",
            Password = "password123",
            GrantType = "password"
        };

        _configuration.Setup(c => c["Tokens:AccessTokenExpiryInMinutes"]).Returns("60");
        _configuration.Setup(c => c["Tokens:Issuer"]).Returns((string?)null);

        // Setup user validation to pass
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword("password123");
        var user = new User { Id = 1, RowId = Guid.NewGuid(), UserName = "test@example.com", FirstName = "Test", LastName = "User", Email = "test@example.com", Password = hashedPassword, IsActive = true, RoleTypeId = 1 };
        var roleType = new RoleType { Id = 1, RowId = Guid.NewGuid(), Name = "Admin" };
        var clientRoleType = new ClientRoleType { Id = 1, ClientId = 1, RoleTypeId = 1 };
        var client = new Client { Id = 1, Name = "Test Client" };
        var clientUser = new ClientUser { Id = 1, UserId = 1, ClientId = 1 };

        _userRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { user }.AsQueryable()));
        _roleTypeRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { roleType }.AsQueryable()));
        _clientRoleTypeRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { clientRoleType }.AsQueryable()));
        _clientRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { client }.AsQueryable()));
        _clientUserRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { clientUser }.AsQueryable()));

        var sut = CreateSut();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => sut.AuthenticateUserAsync(tokenRequest, "127.0.0.1", "TestAgent"));

        Assert.Equal("An error occurred during user authentication", exception.Message);
    }

    [Fact]
    public async Task AuthenticateUserAsync_UserNotFoundForAuditCreation_NoAudit_NoException()
    {
        // Arrange
        var tokenRequest = new TokenFormRequest
        {
            UserName = "test@example.com",
            Password = "password123",
            GrantType = "password"
        };

        var user = new User
        {
            Id = 1,
            RowId = Guid.NewGuid(),
            UserName = "test@example.com",
            FirstName = "Test",
            LastName = "User",
            Password = BCrypt.Net.BCrypt.HashPassword("password123"),
            IsActive = true,
            RoleTypeId = 1
        };

        var clientUser = new ClientUser { UserId = 1, ClientId = 1 };
        var client = new Client { Id = 1, Name = "TestClient" };
        var roleType = new RoleType { Id = 1, RowId = Guid.NewGuid(), Name = "Admin" };
        var clientRoleType = new ClientRoleType { RoleTypeId = 1, ClientId = 1 };

        // Setup for validation
        _userRepo.SetupSequence(r => r.GetAsync())
            .Returns(Task.FromResult(new[] { user }.AsQueryable()))  // For validation
            .Returns(Task.FromResult(Array.Empty<User>().AsQueryable()));  // For audit creation - user not found

        _clientUserRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { clientUser }.AsQueryable()));
        _clientRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { client }.AsQueryable()));
        _roleTypeRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { roleType }.AsQueryable()));
        _clientRoleTypeRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { clientRoleType }.AsQueryable()));

        var sut = CreateSut();

        // Act
        var result = await sut.AuthenticateUserAsync(tokenRequest, "127.0.0.1", "TestAgent");

        // Assert: no audit path invoked, still returns token
        Assert.NotNull(result);
        Assert.NotNull(result.Token);
    }

    [Fact]
    public async Task AuthenticateUserAsync_InvalidRoleInfo_NoAudit_NoException()
    {
        // Arrange
        var tokenRequest = new TokenFormRequest
        {
            UserName = "test@example.com",
            Password = "password123",
            GrantType = "password"
        };

        var user = new User
        {
            Id = 1,
            RowId = Guid.NewGuid(),
            UserName = "test@example.com",
            FirstName = "Test",
            LastName = "User",
            Password = BCrypt.Net.BCrypt.HashPassword("password123"),
            IsActive = true,
            RoleTypeId = 1
        };

        var clientUser = new ClientUser { UserId = 1, ClientId = 1 };
        var client = new Client { Id = 1, Name = "TestClient" };
        var roleType = new RoleType { Id = 1, RowId = Guid.NewGuid(), Name = "Admin" };
        var clientRoleType = new ClientRoleType { RoleTypeId = 1, ClientId = 1 };

        // Setup for validation to pass
        _userRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { user }.AsQueryable()));
        _clientUserRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { clientUser }.AsQueryable()));
        _clientRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { client }.AsQueryable()));

        // Setup for role type repo - first call for validation, second call for audit
        _roleTypeRepo.SetupSequence(r => r.GetAsync())
            .Returns(Task.FromResult(new[] { roleType }.AsQueryable())) // For validation
            .Returns(Task.FromResult(new[] { roleType }.AsQueryable())); // For audit creation

        // Setup for client role type - first call for validation, second call for audit (empty for no role info)
        _clientRoleTypeRepo.SetupSequence(r => r.GetAsync())
            .Returns(Task.FromResult(new[] { clientRoleType }.AsQueryable())) // For validation
            .Returns(Task.FromResult(Array.Empty<ClientRoleType>().AsQueryable())); // For audit creation - no role info

        var sut = CreateSut();

        // Act
        var result = await sut.AuthenticateUserAsync(tokenRequest, "127.0.0.1", "TestAgent");

        // Assert: audit suppressed, call succeeds
        Assert.NotNull(result);
        Assert.NotNull(result.Token);
    }

    [Fact]
    public async Task AuthenticateUserAsync_MissingTokenKey_ThrowsInvalidOperationException()
    {
        // Arrange
        var tokenRequest = new TokenFormRequest
        {
            UserName = "test@example.com",
            Password = "password123",
            GrantType = "password"
        };

        _configuration.Setup(c => c["Tokens:AccessTokenExpiryInMinutes"]).Returns("60");
        _configuration.Setup(c => c["Tokens:Issuer"]).Returns("TestIssuer");
        _configuration.Setup(c => c["Tokens:Audience"]).Returns("TestAudience");
        _configuration.Setup(c => c["Tokens:Key"]).Returns((string?)null);

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword("password123");
        var user = new User { Id = 1, RowId = Guid.NewGuid(), UserName = "test@example.com", FirstName = "Test", LastName = "User", Password = hashedPassword, IsActive = true, RoleTypeId = 1 };
        var roleType = new RoleType { Id = 1, RowId = Guid.NewGuid(), Name = "Admin" };
        var clientRoleType = new ClientRoleType { Id = 1, ClientId = 1, RoleTypeId = 1 };
        var client = new Client { Id = 1, Name = "Test Client" };
        var clientUser = new ClientUser { Id = 1, UserId = 1, ClientId = 1 };

        _userRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { user }.AsQueryable()));
        _roleTypeRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { roleType }.AsQueryable()));
        _clientRoleTypeRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { clientRoleType }.AsQueryable()));
        _clientRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { client }.AsQueryable()));
        _clientUserRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { clientUser }.AsQueryable()));
        _roleNavigationUserActionRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(Array.Empty<RoleNavigationUserAction>().AsQueryable()));
        _navigationUserActionRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(Array.Empty<NavigationUserAction>().AsQueryable()));
        _navigationRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(Array.Empty<Navigation>().AsQueryable()));
        _userActionRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(Array.Empty<UserAction>().AsQueryable()));
        _clientProjectRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(Array.Empty<ClientProject>().AsQueryable()));
        _clientProjectUserRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(Array.Empty<ClientProjectUser>().AsQueryable()));
        _userAuditRepo.Setup(r => r.AddAsync(It.IsAny<UserAudit>())).ReturnsAsync(new UserAudit());
        _unitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var sut = CreateSut();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => sut.AuthenticateUserAsync(tokenRequest, "127.0.0.1", "TestAgent"));

        Assert.Equal("An error occurred during user authentication", exception.Message);
    }

    [Fact]
    public async Task AuthenticateUserAsync_MissingTokenAudience_ThrowsInvalidOperationException()
    {
        // Arrange
        var tokenRequest = new TokenFormRequest
        {
            UserName = "test@example.com",
            Password = "password123",
            GrantType = "password"
        };

        _configuration.Setup(c => c["Tokens:AccessTokenExpiryInMinutes"]).Returns("60");
        _configuration.Setup(c => c["Tokens:Issuer"]).Returns("TestIssuer");
        _configuration.Setup(c => c["Tokens:Audience"]).Returns((string?)null);
        _configuration.Setup(c => c["Tokens:Key"]).Returns("ThisIsAVeryLongSecretKeyForTestingPurposesOnly123456789");

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword("password123");
        var user = new User { Id = 1, RowId = Guid.NewGuid(), UserName = "test@example.com", FirstName = "Test", LastName = "User", Password = hashedPassword, IsActive = true, RoleTypeId = 1 };
        var roleType = new RoleType { Id = 1, RowId = Guid.NewGuid(), Name = "Admin" };
        var clientRoleType = new ClientRoleType { Id = 1, ClientId = 1, RoleTypeId = 1 };
        var client = new Client { Id = 1, Name = "Test Client" };
        var clientUser = new ClientUser { Id = 1, UserId = 1, ClientId = 1 };

        _userRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { user }.AsQueryable()));
        _roleTypeRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { roleType }.AsQueryable()));
        _clientRoleTypeRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { clientRoleType }.AsQueryable()));
        _clientRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { client }.AsQueryable()));
        _clientUserRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { clientUser }.AsQueryable()));
        _roleNavigationUserActionRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(Array.Empty<RoleNavigationUserAction>().AsQueryable()));
        _navigationUserActionRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(Array.Empty<NavigationUserAction>().AsQueryable()));
        _navigationRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(Array.Empty<Navigation>().AsQueryable()));
        _userActionRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(Array.Empty<UserAction>().AsQueryable()));
        _clientProjectRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(Array.Empty<ClientProject>().AsQueryable()));
        _clientProjectUserRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(Array.Empty<ClientProjectUser>().AsQueryable()));
        _userAuditRepo.Setup(r => r.AddAsync(It.IsAny<UserAudit>())).ReturnsAsync(new UserAudit());
        _unitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var sut = CreateSut();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => sut.AuthenticateUserAsync(tokenRequest, "127.0.0.1", "TestAgent"));

        Assert.Equal("An error occurred during user authentication", exception.Message);
    }

    [Fact]
    public async Task AuthenticateUserAsync_WithUserPermissionsAndProjects_ReturnsCompleteTokenResponse()
    {
        // Arrange
        var tokenRequest = new TokenFormRequest
        {
            UserName = "test@example.com",
            Password = "password123",
            GrantType = "password"
        };

        var user = new User
        {
            Id = 1,
            RowId = Guid.NewGuid(),
            UserName = "test@example.com",
            FirstName = "Test",
            LastName = "User",
            Password = BCrypt.Net.BCrypt.HashPassword("password123"),
            IsActive = true,
            RoleTypeId = 1
        };

        var clientUser = new ClientUser { UserId = 1, ClientId = 1 };
        var client = new Client { Id = 1, Name = "TestClient" };
        var roleType = new RoleType { Id = 1, RowId = Guid.NewGuid(), Name = "Admin" };
        var clientRoleType = new ClientRoleType { RoleTypeId = 1, ClientId = 1 };

        var navigation = new Navigation { Id = 1, RowId = Guid.NewGuid(), Name = "Dashboard", OrderBy = 1 };
        var userAction = new UserAction { Id = 1, RowId = Guid.NewGuid(), Name = "View" };
        var navigationUserAction = new NavigationUserAction { Id = 1, NavigationId = 1, UserActionId = 1 };
        var roleNavigationUserAction = new RoleNavigationUserAction { RoleTypeId = 1, NavigationUserActionId = 1 };
        var clientProject = new ClientProject { Id = 2, RowId = Guid.NewGuid(), ClientId = 1, Name = "Project1" };
        var clientProjectUser = new ClientProjectUser { ProjectId = 2, UserId = 1 };

        _userRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { user }.AsQueryable()));
        _clientUserRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { clientUser }.AsQueryable()));
        _clientRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { client }.AsQueryable()));
        _roleTypeRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { roleType }.AsQueryable()));
        _clientRoleTypeRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { clientRoleType }.AsQueryable()));
        _roleNavigationUserActionRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { roleNavigationUserAction }.AsQueryable()));
        _navigationUserActionRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { navigationUserAction }.AsQueryable()));
        _navigationRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { navigation }.AsQueryable()));
        _userActionRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { userAction }.AsQueryable()));
        _clientProjectRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { clientProject }.AsQueryable()));
        _clientProjectUserRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { clientProjectUser }.AsQueryable()));

        _userAuditRepo.Setup(r => r.AddAsync(It.IsAny<UserAudit>())).ReturnsAsync(new UserAudit());
        _unitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var sut = CreateSut();

        // Act
        var result = await sut.AuthenticateUserAsync(tokenRequest, "127.0.0.1", "TestAgent");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test User", result.Name);
        Assert.NotNull(result.Token);
        Assert.NotNull(result.RefreshToken);
        Assert.Equal(1, result.RoleId);
        Assert.Equal("Admin", result.RoleName);
        Assert.Equal(1, result.ClientId);
        Assert.Equal("TestClient", result.ClientName);
    }

    #endregion AuthenticateUserAsync
}
using AutoMapper;
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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace KonaAI.Master.Test.Unit.Business.Authentication;

/// <summary>
/// Edge case tests for <see cref="UserLoginBusiness"/>.
/// Tests error paths, edge conditions, and exception scenarios.
/// </summary>
public class UserLoginBusinessEdgeCaseTests
{
    private readonly Mock<ILogger<UserLoginBusiness>> _logger = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly Mock<IUserContextService> _userContextService = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<ILicenseService> _licenseService = new();
    private readonly Mock<IConfiguration> _configuration = new();
    private readonly Mock<IUserRepository> _userRepo = new();
    private readonly Mock<IRoleTypeRepository> _roleTypeRepo = new();
    private readonly Mock<IClientRoleTypeRepository> _clientRoleTypeRepo = new();
    private readonly Mock<IClientRepository> _clientRepo = new();
    private readonly Mock<IClientUserRepository> _clientUserRepo = new();

    public UserLoginBusinessEdgeCaseTests()
    {
        _unitOfWork.SetupGet(u => u.Users).Returns(_userRepo.Object);
        _unitOfWork.SetupGet(u => u.RoleTypes).Returns(_roleTypeRepo.Object);
        _unitOfWork.SetupGet(u => u.ClientRoleTypes).Returns(_clientRoleTypeRepo.Object);
        _unitOfWork.SetupGet(u => u.Clients).Returns(_clientRepo.Object);
        _unitOfWork.SetupGet(u => u.ClientUsers).Returns(_clientUserRepo.Object);
    }

    private UserLoginBusiness CreateSut() =>
        new(_logger.Object, _unitOfWork.Object, _userContextService.Object, _licenseService.Object, _configuration.Object);

    [Fact]
    public async Task AuthenticateUserAsync_UserNotFound_ThrowsAuthenticationException()
    {
        // Arrange
        var tokenRequest = new TokenFormRequest { UserName = "nonexistent@example.com", Password = "password123", GrantType = "password" };
        _userRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(Array.Empty<User>().AsQueryable()));

        var sut = CreateSut();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<System.Security.Authentication.AuthenticationException>(
            () => sut.AuthenticateUserAsync(tokenRequest, "127.0.0.1", "Test-Agent"));

        Assert.Equal("Invalid username or password.", exception.Message);
    }

    [Fact]
    public async Task AuthenticateUserAsync_UserInactive_ThrowsAuthenticationException()
    {
        // Arrange
        var tokenRequest = new TokenFormRequest { UserName = "test@example.com", Password = "password123", GrantType = "password" };
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword("password123");
        var user = new User { Id = 1, UserName = "test@example.com", Email = "test@example.com", Password = hashedPassword, IsActive = false, RoleTypeId = 1 };

        _userRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { user }.AsQueryable()));

        var sut = CreateSut();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<System.Security.Authentication.AuthenticationException>(
            () => sut.AuthenticateUserAsync(tokenRequest, "127.0.0.1", "Test-Agent"));

        Assert.Equal("Invalid username or password.", exception.Message);
    }

    [Fact]
    public async Task AuthenticateUserAsync_WrongPassword_ThrowsAuthenticationException()
    {
        // Arrange
        var tokenRequest = new TokenFormRequest { UserName = "test@example.com", Password = "wrongpassword", GrantType = "password" };
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword("password123");
        var user = new User { Id = 1, UserName = "test@example.com", Email = "test@example.com", Password = hashedPassword, IsActive = true, RoleTypeId = 1 };

        _userRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { user }.AsQueryable()));

        var sut = CreateSut();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<System.Security.Authentication.AuthenticationException>(
            () => sut.AuthenticateUserAsync(tokenRequest, "127.0.0.1", "Test-Agent"));

        Assert.Equal("Invalid username or password.", exception.Message);
    }

    [Fact]
    public async Task AuthenticateUserAsync_NoRoleType_ThrowsAuthenticationException()
    {
        // Arrange
        var tokenRequest = new TokenFormRequest { UserName = "test@example.com", Password = "password123", GrantType = "password" };
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword("password123");
        var user = new User { Id = 1, UserName = "test@example.com", Email = "test@example.com", Password = hashedPassword, IsActive = true, RoleTypeId = 1 };
        var roleType = new RoleType { Id = 1, Name = "Admin" };
        var clientRoleType = new ClientRoleType { Id = 1, ClientId = 1, RoleTypeId = 1 };
        var client = new Client { Id = 1, Name = "Test Client" };
        var clientUser = new ClientUser { Id = 1, UserId = 1, ClientId = 1 };

        _userRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { user }.AsQueryable()));
        _roleTypeRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(Array.Empty<RoleType>().AsQueryable()));
        _clientRoleTypeRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { clientRoleType }.AsQueryable()));
        _clientRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { client }.AsQueryable()));
        _clientUserRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { clientUser }.AsQueryable()));

        var sut = CreateSut();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<System.Security.Authentication.AuthenticationException>(
            () => sut.AuthenticateUserAsync(tokenRequest, "127.0.0.1", "Test-Agent"));

        Assert.Equal("Invalid username or password.", exception.Message);
    }

    [Fact]
    public async Task AuthenticateUserAsync_NoClientRoleType_ThrowsAuthenticationException()
    {
        // Arrange
        var tokenRequest = new TokenFormRequest { UserName = "test@example.com", Password = "password123", GrantType = "password" };
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword("password123");
        var user = new User { Id = 1, UserName = "test@example.com", Email = "test@example.com", Password = hashedPassword, IsActive = true, RoleTypeId = 1 };
        var roleType = new RoleType { Id = 1, Name = "Admin" };
        var client = new Client { Id = 1, Name = "Test Client" };
        var clientUser = new ClientUser { Id = 1, UserId = 1, ClientId = 1 };

        _userRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { user }.AsQueryable()));
        _roleTypeRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { roleType }.AsQueryable()));
        _clientRoleTypeRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(Array.Empty<ClientRoleType>().AsQueryable()));
        _clientRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { client }.AsQueryable()));
        _clientUserRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { clientUser }.AsQueryable()));

        var sut = CreateSut();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<System.Security.Authentication.AuthenticationException>(
            () => sut.AuthenticateUserAsync(tokenRequest, "127.0.0.1", "Test-Agent"));

        Assert.Equal("Invalid username or password.", exception.Message);
    }

    [Fact]
    public async Task AuthenticateUserAsync_NoClient_ThrowsAuthenticationException()
    {
        // Arrange
        var tokenRequest = new TokenFormRequest { UserName = "test@example.com", Password = "password123", GrantType = "password" };
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword("password123");
        var user = new User { Id = 1, UserName = "test@example.com", Email = "test@example.com", Password = hashedPassword, IsActive = true, RoleTypeId = 1 };
        var roleType = new RoleType { Id = 1, Name = "Admin" };
        var clientRoleType = new ClientRoleType { Id = 1, ClientId = 1, RoleTypeId = 1 };
        var clientUser = new ClientUser { Id = 1, UserId = 1, ClientId = 1 };

        _userRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { user }.AsQueryable()));
        _roleTypeRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { roleType }.AsQueryable()));
        _clientRoleTypeRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { clientRoleType }.AsQueryable()));
        _clientRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(Array.Empty<Client>().AsQueryable()));
        _clientUserRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { clientUser }.AsQueryable()));

        var sut = CreateSut();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<System.Security.Authentication.AuthenticationException>(
            () => sut.AuthenticateUserAsync(tokenRequest, "127.0.0.1", "Test-Agent"));

        Assert.Equal("Invalid username or password.", exception.Message);
    }

    [Fact]
    public async Task AuthenticateUserAsync_NoClientUser_ThrowsAuthenticationException()
    {
        // Arrange
        var tokenRequest = new TokenFormRequest { UserName = "test@example.com", Password = "password123", GrantType = "password" };
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword("password123");
        var user = new User { Id = 1, UserName = "test@example.com", Email = "test@example.com", Password = hashedPassword, IsActive = true, RoleTypeId = 1 };
        var roleType = new RoleType { Id = 1, Name = "Admin" };
        var clientRoleType = new ClientRoleType { Id = 1, ClientId = 1, RoleTypeId = 1 };
        var client = new Client { Id = 1, Name = "Test Client" };

        _userRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { user }.AsQueryable()));
        _roleTypeRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { roleType }.AsQueryable()));
        _clientRoleTypeRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { clientRoleType }.AsQueryable()));
        _clientRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { client }.AsQueryable()));
        _clientUserRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(Array.Empty<ClientUser>().AsQueryable()));

        var sut = CreateSut();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<System.Security.Authentication.AuthenticationException>(
            () => sut.AuthenticateUserAsync(tokenRequest, "127.0.0.1", "Test-Agent"));

        Assert.Equal("Invalid username or password.", exception.Message);
    }

    [Fact]
    public async Task AuthenticateUserAsync_MissingAudienceConfiguration_ThrowsInvalidOperationException()
    {
        // Arrange
        var tokenRequest = new TokenFormRequest { UserName = "test@example.com", Password = "password123", GrantType = "password" };
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword("password123");
        var user = new User { Id = 1, UserName = "test@example.com", Email = "test@example.com", Password = hashedPassword, IsActive = true, RoleTypeId = 1 };
        var roleType = new RoleType { Id = 1, Name = "Admin" };
        var clientRoleType = new ClientRoleType { Id = 1, ClientId = 1, RoleTypeId = 1 };
        var client = new Client { Id = 1, Name = "Test Client" };
        var clientUser = new ClientUser { Id = 1, UserId = 1, ClientId = 1 };

        _userRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { user }.AsQueryable()));
        _roleTypeRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { roleType }.AsQueryable()));
        _clientRoleTypeRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { clientRoleType }.AsQueryable()));
        _clientRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { client }.AsQueryable()));
        _clientUserRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { clientUser }.AsQueryable()));

        _configuration.Setup(c => c["Tokens:AccessTokenExpiryInMinutes"]).Returns("60");
        _configuration.Setup(c => c["Tokens:Issuer"]).Returns("TestIssuer");
        _configuration.Setup(c => c["Tokens:Audience"]).Returns((string?)null);

        var sut = CreateSut();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => sut.AuthenticateUserAsync(tokenRequest, "127.0.0.1", "Test-Agent"));

        Assert.Equal("An error occurred during user authentication", exception.Message);
    }

    [Fact]
    public async Task AuthenticateUserAsync_MissingKeyConfiguration_ThrowsInvalidOperationException()
    {
        // Arrange
        var tokenRequest = new TokenFormRequest { UserName = "test@example.com", Password = "password123", GrantType = "password" };
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword("password123");
        var user = new User { Id = 1, UserName = "test@example.com", Email = "test@example.com", Password = hashedPassword, IsActive = true, RoleTypeId = 1 };
        var roleType = new RoleType { Id = 1, Name = "Admin" };
        var clientRoleType = new ClientRoleType { Id = 1, ClientId = 1, RoleTypeId = 1 };
        var client = new Client { Id = 1, Name = "Test Client" };
        var clientUser = new ClientUser { Id = 1, UserId = 1, ClientId = 1 };

        _userRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { user }.AsQueryable()));
        _roleTypeRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { roleType }.AsQueryable()));
        _clientRoleTypeRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { clientRoleType }.AsQueryable()));
        _clientRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { client }.AsQueryable()));
        _clientUserRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { clientUser }.AsQueryable()));

        _configuration.Setup(c => c["Tokens:AccessTokenExpiryInMinutes"]).Returns("60");
        _configuration.Setup(c => c["Tokens:Issuer"]).Returns("TestIssuer");
        _configuration.Setup(c => c["Tokens:Audience"]).Returns("TestAudience");
        _configuration.Setup(c => c["Tokens:Key"]).Returns((string?)null);

        var sut = CreateSut();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => sut.AuthenticateUserAsync(tokenRequest, "127.0.0.1", "Test-Agent"));

        Assert.Equal("An error occurred during user authentication", exception.Message);
    }

    [Fact]
    public async Task AuthenticateUserAsync_InvalidExpiryConfiguration_ThrowsInvalidOperationException()
    {
        // Arrange
        var tokenRequest = new TokenFormRequest { UserName = "test@example.com", Password = "password123", GrantType = "password" };
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword("password123");
        var user = new User { Id = 1, UserName = "test@example.com", Email = "test@example.com", Password = hashedPassword, IsActive = true, RoleTypeId = 1 };
        var roleType = new RoleType { Id = 1, Name = "Admin" };
        var clientRoleType = new ClientRoleType { Id = 1, ClientId = 1, RoleTypeId = 1 };
        var client = new Client { Id = 1, Name = "Test Client" };
        var clientUser = new ClientUser { Id = 1, UserId = 1, ClientId = 1 };

        _userRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { user }.AsQueryable()));
        _roleTypeRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { roleType }.AsQueryable()));
        _clientRoleTypeRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { clientRoleType }.AsQueryable()));
        _clientRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { client }.AsQueryable()));
        _clientUserRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { clientUser }.AsQueryable()));

        _configuration.Setup(c => c["Tokens:AccessTokenExpiryInMinutes"]).Returns("invalid");
        _configuration.Setup(c => c["Tokens:Issuer"]).Returns("TestIssuer");
        _configuration.Setup(c => c["Tokens:Audience"]).Returns("TestAudience");
        _configuration.Setup(c => c["Tokens:Key"]).Returns("TestKey");

        var sut = CreateSut();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => sut.AuthenticateUserAsync(tokenRequest, "127.0.0.1", "Test-Agent"));

        Assert.Equal("An error occurred during user authentication", exception.Message);
    }
}
using AutoMapper;
using KonaAI.Master.Business.Authentication.Logic;
using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Repository.Common.Model;
using KonaAI.Master.Repository.DataAccess.Master.App.Interface;
using KonaAI.Master.Repository.DataAccess.Master.MetaData.Interface;
using KonaAI.Master.Repository.DataAccess.Tenant.ClientMetaData.Interface;
using KonaAI.Master.Repository.Domain.Master.App;
using KonaAI.Master.Repository.Domain.Master.MetaData;
using KonaAI.Master.Repository.Domain.Tenant.ClientMetaData;
using Microsoft.Extensions.Logging;
using Moq;

namespace KonaAI.Master.Test.Unit.Business.Authentication;

/// <summary>
/// Edge case tests for <see cref="MenuBusiness"/>.
/// Tests error paths, edge conditions, and exception scenarios.
/// </summary>
public class MenuBusinessEdgeCaseTests
{
    private readonly Mock<ILogger<MenuBusiness>> _logger = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly Mock<IUserContextService> _userContextService = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IUserRepository> _userRepo = new();
    private readonly Mock<IRoleTypeRepository> _roleTypeRepo = new();
    private readonly Mock<IClientRoleTypeRepository> _clientRoleTypeRepo = new();
    private readonly Mock<IRoleNavigationUserActionRepository> _roleNavigationUserActionRepo = new();
    private readonly Mock<INavigationUserActionRepository> _navigationUserActionRepo = new();
    private readonly Mock<INavigationRepository> _navigationRepo = new();

    public MenuBusinessEdgeCaseTests()
    {
        _unitOfWork.SetupGet(u => u.Users).Returns(_userRepo.Object);
        _unitOfWork.SetupGet(u => u.RoleTypes).Returns(_roleTypeRepo.Object);
        _unitOfWork.SetupGet(u => u.ClientRoleTypes).Returns(_clientRoleTypeRepo.Object);
        _unitOfWork.SetupGet(u => u.RoleNavigationUserActions).Returns(_roleNavigationUserActionRepo.Object);
        _unitOfWork.SetupGet(u => u.NavigationUserActions).Returns(_navigationUserActionRepo.Object);
        _unitOfWork.SetupGet(u => u.Navigations).Returns(_navigationRepo.Object);
    }

    private MenuBusiness CreateSut() =>
        new(_logger.Object, _unitOfWork.Object, _userContextService.Object);

    [Fact]
    public async Task GetAsync_UserContextNull_ThrowsInvalidOperationException()
    {
        // Arrange
        _userContextService.SetupGet(u => u.UserContext).Returns((UserContext?)null);

        var sut = CreateSut();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => sut.GetAsync());

        Assert.Equal("An error occurred during user authentication", exception.Message);
        Assert.IsType<UnauthorizedAccessException>(exception.InnerException);
    }

    [Fact]
    public async Task GetAsync_UserNotFound_ThrowsInvalidOperationException()
    {
        // Arrange
        var userContext = new UserContext
        {
            UserRowId = Guid.NewGuid(),
            UserLoginName = "test@example.com",
            UserLoginEmail = "test@example.com",
            UserLoginId = 1  // Add this to match the LINQ query
        };

        _userContextService.SetupGet(u => u.UserContext).Returns(userContext);
        _userRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(Array.Empty<User>().AsQueryable()));

        var sut = CreateSut();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => sut.GetAsync());

        Assert.Equal("An error occurred during user authentication", exception.Message);
        Assert.IsType<UnauthorizedAccessException>(exception.InnerException);
    }

    [Fact]
    public async Task GetAsync_UserInactive_ThrowsInvalidOperationException()
    {
        // Arrange
        var userContext = new UserContext
        {
            UserRowId = Guid.NewGuid(),
            UserLoginName = "test@example.com",
            UserLoginEmail = "test@example.com",
            UserLoginId = 1  // Add this to match the LINQ query
        };

        var user = new User { Id = 1, UserName = "test@example.com", Email = "test@example.com", IsActive = false };

        _userContextService.SetupGet(u => u.UserContext).Returns(userContext);
        _userRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { user }.AsQueryable()));

        var sut = CreateSut();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => sut.GetAsync());

        Assert.Equal("An error occurred during user authentication", exception.Message);
        Assert.IsType<UnauthorizedAccessException>(exception.InnerException);
    }

    [Fact]
    public async Task GetAsync_NoRoleNavigationUserActions_ReturnsEmptyList()
    {
        // Arrange
        var userContext = new UserContext
        {
            UserRowId = Guid.NewGuid(),
            UserLoginName = "test@example.com",
            UserLoginEmail = "test@example.com",
            UserLoginId = 1  // Add this to match the LINQ query
        };

        var user = new User { Id = 1, UserName = "test@example.com", Email = "test@example.com", IsActive = true, RoleTypeId = 1 };
        var roleType = new RoleType { Id = 1, Name = "Admin" };
        var clientRoleType = new ClientRoleType { Id = 1, ClientId = 1, RoleTypeId = 1 };

        _userContextService.SetupGet(u => u.UserContext).Returns(userContext);
        _userRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { user }.AsQueryable()));
        _roleTypeRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { roleType }.AsQueryable()));
        _clientRoleTypeRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { clientRoleType }.AsQueryable()));
        _roleNavigationUserActionRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(Array.Empty<RoleNavigationUserAction>().AsQueryable()));
        _navigationUserActionRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(Array.Empty<NavigationUserAction>().AsQueryable()));
        _navigationRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(Array.Empty<Navigation>().AsQueryable()));

        var sut = CreateSut();

        // Act
        var result = await sut.GetAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetAsync_NoNavigationUserActions_ReturnsEmptyList()
    {
        // Arrange
        var userContext = new UserContext
        {
            UserRowId = Guid.NewGuid(),
            UserLoginName = "test@example.com",
            UserLoginEmail = "test@example.com",
            UserLoginId = 1  // Add this to match the LINQ query
        };

        var user = new User { Id = 1, UserName = "test@example.com", Email = "test@example.com", IsActive = true, RoleTypeId = 1 };
        var roleType = new RoleType { Id = 1, Name = "Admin" };
        var clientRoleType = new ClientRoleType { Id = 1, ClientId = 1, RoleTypeId = 1 };
        var roleNavigationUserAction = new RoleNavigationUserAction { Id = 1, RoleTypeId = 1, NavigationUserActionId = 1 };

        _userContextService.SetupGet(u => u.UserContext).Returns(userContext);
        _userRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { user }.AsQueryable()));
        _roleTypeRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { roleType }.AsQueryable()));
        _clientRoleTypeRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { clientRoleType }.AsQueryable()));
        _roleNavigationUserActionRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { roleNavigationUserAction }.AsQueryable()));
        _navigationUserActionRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(Array.Empty<NavigationUserAction>().AsQueryable()));
        _navigationRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(Array.Empty<Navigation>().AsQueryable()));

        var sut = CreateSut();

        // Act
        var result = await sut.GetAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetAsync_NoNavigations_ReturnsEmptyList()
    {
        // Arrange
        var userContext = new UserContext
        {
            UserRowId = Guid.NewGuid(),
            UserLoginName = "test@example.com",
            UserLoginEmail = "test@example.com",
            UserLoginId = 1  // Add this to match the LINQ query
        };

        var user = new User { Id = 1, UserName = "test@example.com", Email = "test@example.com", IsActive = true, RoleTypeId = 1 };
        var roleType = new RoleType { Id = 1, Name = "Admin" };
        var clientRoleType = new ClientRoleType { Id = 1, ClientId = 1, RoleTypeId = 1 };
        var roleNavigationUserAction = new RoleNavigationUserAction { Id = 1, RoleTypeId = 1, NavigationUserActionId = 1 };
        var navigationUserAction = new NavigationUserAction { Id = 1, NavigationId = 1, UserActionId = 1 };

        _userContextService.SetupGet(u => u.UserContext).Returns(userContext);
        _userRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { user }.AsQueryable()));
        _roleTypeRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { roleType }.AsQueryable()));
        _clientRoleTypeRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { clientRoleType }.AsQueryable()));
        _roleNavigationUserActionRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { roleNavigationUserAction }.AsQueryable()));
        _navigationUserActionRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(new[] { navigationUserAction }.AsQueryable()));
        _navigationRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(Array.Empty<Navigation>().AsQueryable()));

        var sut = CreateSut();

        // Act
        var result = await sut.GetAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetAsync_DatabaseException_ThrowsInvalidOperationException()
    {
        // Arrange
        var userContext = new UserContext
        {
            UserRowId = Guid.NewGuid(),
            UserLoginName = "test@example.com",
            UserLoginEmail = "test@example.com",
            UserLoginId = 1  // Add this to match the LINQ query
        };

        _userContextService.SetupGet(u => u.UserContext).Returns(userContext);
        _userRepo.Setup(r => r.GetAsync()).ThrowsAsync(new Exception("Database connection failed"));

        var sut = CreateSut();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => sut.GetAsync());

        Assert.Equal("An error occurred during user authentication", exception.Message);
        Assert.IsType<Exception>(exception.InnerException);
    }

    [Fact]
    public async Task GetAsync_UserContextServiceThrows_ThrowsInvalidOperationException()
    {
        // Arrange
        _userContextService.SetupGet(u => u.UserContext).Throws(new Exception("User context service error"));

        var sut = CreateSut();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => sut.GetAsync());

        Assert.Equal("An error occurred during user authentication", exception.Message);
        Assert.IsType<Exception>(exception.InnerException);
        Assert.Equal("User context service error", exception.InnerException.Message);
    }
}
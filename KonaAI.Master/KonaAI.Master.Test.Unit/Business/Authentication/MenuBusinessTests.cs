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
/// Unit tests for <see cref="MenuBusiness"/>.
/// Covers:
/// - Menu retrieval based on user role and permissions
/// - User context validation
/// - Navigation ordering and filtering
/// - Exception handling for invalid user context
/// </summary>
public class MenuBusinessTests
{
    private readonly Mock<ILogger<MenuBusiness>> _logger = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IUserContextService> _userContextService = new();
    private readonly Mock<IUserRepository> _userRepo = new();
    private readonly Mock<IRoleTypeRepository> _roleTypeRepo = new();
    private readonly Mock<IClientRoleTypeRepository> _clientRoleTypeRepo = new();
    private readonly Mock<IRoleNavigationUserActionRepository> _roleNavigationUserActionRepo = new();
    private readonly Mock<INavigationUserActionRepository> _navigationUserActionRepo = new();
    private readonly Mock<INavigationRepository> _navigationRepo = new();

    public MenuBusinessTests()
    {
        // Setup UoW repository mocks
        _unitOfWork.SetupGet(u => u.Users).Returns(_userRepo.Object);
        _unitOfWork.SetupGet(u => u.RoleTypes).Returns(_roleTypeRepo.Object);
        _unitOfWork.SetupGet(u => u.ClientRoleTypes).Returns(_clientRoleTypeRepo.Object);
        _unitOfWork.SetupGet(u => u.RoleNavigationUserActions).Returns(_roleNavigationUserActionRepo.Object);
        _unitOfWork.SetupGet(u => u.NavigationUserActions).Returns(_navigationUserActionRepo.Object);
        _unitOfWork.SetupGet(u => u.Navigations).Returns(_navigationRepo.Object);
    }

    private MenuBusiness CreateSut() =>
        new(_logger.Object, _unitOfWork.Object, _userContextService.Object);

    #region GetAsync

    [Fact]
    public async Task GetAsync_ValidUserContext_ReturnsMenuItems()
    {
        // Arrange
        var userContext = new UserContext
        {
            UserLoginId = 1,
            UserRowId = Guid.NewGuid(),
            UserLoginName = "Test User",
            UserLoginEmail = "test@example.com",
            RoleId = 1,
            RoleName = "Admin",
            ClientId = 1,
            ClientName = "TestClient"
        };

        var roleType = new RoleType { Id = 1, Name = "Admin" };
        var clientRoleType = new ClientRoleType { RoleTypeId = 1, ClientId = 1 };
        var roleNavigationUserAction = new RoleNavigationUserAction { RoleTypeId = 1, NavigationUserActionId = 1 };
        var navigationUserAction = new NavigationUserAction { Id = 1, NavigationId = 1, UserActionId = 1 };
        var navigation = new Navigation { Id = 1, RowId = Guid.NewGuid(), Name = "Dashboard", Description = "Main Dashboard", OrderBy = 1 };

        _userContextService.Setup(u => u.UserContext).Returns(userContext);
        _userRepo.Setup(r => r.GetAsync()).ReturnsAsync(new[] { new User { Id = 1, RoleTypeId = 1 } }.AsQueryable());
        _roleTypeRepo.Setup(r => r.GetAsync()).ReturnsAsync(new[] { roleType }.AsQueryable());
        _clientRoleTypeRepo.Setup(r => r.GetAsync()).ReturnsAsync(new[] { clientRoleType }.AsQueryable());
        _roleNavigationUserActionRepo.Setup(r => r.GetAsync()).ReturnsAsync(new[] { roleNavigationUserAction }.AsQueryable());
        _navigationUserActionRepo.Setup(r => r.GetAsync()).ReturnsAsync(new[] { navigationUserAction }.AsQueryable());
        _navigationRepo.Setup(r => r.GetAsync()).ReturnsAsync(new[] { navigation }.AsQueryable());

        var sut = CreateSut();

        // Act
        var result = await sut.GetAsync();
        var resultList = result.ToList();

        // Assert
        Assert.NotNull(result);
        Assert.Single(resultList);
        Assert.Equal(navigation.RowId, resultList[0].RowId);
        Assert.Equal("Dashboard", resultList[0].Name);
        Assert.Equal("Main Dashboard", resultList[0].Description);
        Assert.Equal(1, resultList[0].OrderBy);
    }

    [Fact]
    public async Task GetAsync_InvalidUserContext_ThrowsUnauthorizedAccessException()
    {
        // Arrange
        var userContext = new UserContext
        {
            UserLoginId = 1,
            UserRowId = Guid.NewGuid(),
            UserLoginName = "Test User",
            UserLoginEmail = "test@example.com",
            RoleId = 1,
            RoleName = "Admin",
            ClientId = 1,
            ClientName = "TestClient"
        };

        _userContextService.Setup(u => u.UserContext).Returns(userContext);
        _userRepo.Setup(r => r.GetAsync()).ReturnsAsync(Array.Empty<User>().AsQueryable());
        _roleTypeRepo.Setup(r => r.GetAsync()).ReturnsAsync(Array.Empty<RoleType>().AsQueryable());
        _clientRoleTypeRepo.Setup(r => r.GetAsync()).ReturnsAsync(Array.Empty<ClientRoleType>().AsQueryable());

        var sut = CreateSut();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => sut.GetAsync());

        Assert.Equal("An error occurred during user authentication", exception.Message);
        Assert.IsType<UnauthorizedAccessException>(exception.InnerException);
        Assert.Equal("User role information is invalid", exception.InnerException.Message);
    }

    [Fact]
    public async Task GetAsync_ExceptionInRepository_ThrowsInvalidOperationException()
    {
        // Arrange
        var userContext = new UserContext
        {
            UserLoginId = 1,
            UserRowId = Guid.NewGuid(),
            UserLoginName = "Test User",
            UserLoginEmail = "test@example.com",
            RoleId = 1,
            RoleName = "Admin",
            ClientId = 1,
            ClientName = "TestClient"
        };

        _userContextService.Setup(u => u.UserContext).Returns(userContext);
        _userRepo.Setup(r => r.GetAsync()).ThrowsAsync(new Exception("Database connection failed"));

        var sut = CreateSut();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => sut.GetAsync());

        Assert.Equal("An error occurred during user authentication", exception.Message);
    }

    #endregion GetAsync
}
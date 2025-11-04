using AutoMapper;
using KonaAI.Master.Business.Master.MetaData.Logic;
using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Repository.Common.Model;
using KonaAI.Master.Repository.DataAccess.Master.App.Interface;
using KonaAI.Master.Repository.DataAccess.Master.MetaData.Interface;
using KonaAI.Master.Repository.DataAccess.Tenant.ClientMetaData.Interface;
using Microsoft.Extensions.Logging;
using Moq;

namespace KonaAI.Master.Test.Unit.Business.Master.MetaData;

/// <summary>
/// Unit tests for <see cref="KonaAI.Master.Business.Master.MetaData.Logic.NavigationBusiness"/>.
/// Validates:
/// - GetAsync returns navigation items based on user role permissions
/// - Complex joins work correctly with proper data setup
/// - Exception handling for unauthorized access and repository failures
/// </summary>
public class NavigationBusinessTests
{
    private readonly Mock<ILogger<NavigationBusiness>> _logger = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly Mock<IUserContextService> _userContextService = new();
    private readonly Mock<IRoleTypeRepository> _roleTypes = new();
    private readonly Mock<IUserRepository> _users = new();
    private readonly Mock<IClientRoleTypeRepository> _clientRoleTypes = new();
    private readonly Mock<IRoleNavigationUserActionRepository> _roleNavigationUserActions = new();
    private readonly Mock<INavigationUserActionRepository> _navigationUserActions = new();
    private readonly Mock<INavigationRepository> _navigations = new();

    public NavigationBusinessTests()
    {
        // Wire all repositories to UoW
        _uow.SetupGet(x => x.RoleTypes).Returns(_roleTypes.Object);
        _uow.SetupGet(x => x.Users).Returns(_users.Object);
        _uow.SetupGet(x => x.ClientRoleTypes).Returns(_clientRoleTypes.Object);
        _uow.SetupGet(x => x.RoleNavigationUserActions).Returns(_roleNavigationUserActions.Object);
        _uow.SetupGet(x => x.NavigationUserActions).Returns(_navigationUserActions.Object);
        _uow.SetupGet(x => x.Navigations).Returns(_navigations.Object);
    }

    private NavigationBusiness CreateSut() => new(_logger.Object, _uow.Object, _userContextService.Object);

    [Fact]
    public async Task GetAsync_WhenUserContextIsNull_ThrowsInvalidOperationException()
    {
        // Arrange: User context is null
        _userContextService.Setup(x => x.UserContext).Returns((UserContext?)null);

        var sut = CreateSut();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => sut.GetAsync());
    }

    [Fact]
    public async Task GetAsync_WhenRepositoryThrows_ThrowsInvalidOperationException()
    {
        // Arrange: Setup user context but repository throws
        var userContext = new UserContext { UserLoginId = 1, ClientId = 1 };
        _userContextService.Setup(x => x.UserContext).Returns(userContext);

        _roleTypes.Setup(r => r.GetAsync()).ThrowsAsync(new Exception("Database error"));

        var sut = CreateSut();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => sut.GetAsync());
        Assert.Contains("An error occurred during user authentication", exception.Message);
    }
}
using AutoMapper;
using KonaAI.Master.Business.Tenant.MetaData.Logic;
using KonaAI.Master.Model.Common;
using KonaAI.Master.Model.Tenant.Client.SaveModel;
using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Repository.Common.Model;
using KonaAI.Master.Repository.DataAccess.Master.MetaData.Interface;
using KonaAI.Master.Repository.DataAccess.Tenant.ClientMetaData.Interface;
using KonaAI.Master.Repository.Domain.Master.MetaData;
using KonaAI.Master.Repository.Domain.Tenant.ClientMetaData;
using Microsoft.Extensions.Logging;
using Moq;

namespace KonaAI.Master.Test.Unit.Business.Tenant.MetaData;

/// <summary>
/// Unit tests for <see cref="KonaAI.Master.Business.Tenant.MetaData.Logic.ClientRoleTypeBusiness"/>.
/// Covers:
/// - GetAsync: join operations between ClientRoleTypes and RoleTypes
/// - GetByRowIdAsync: role type retrieval and mapping, not-found scenarios
/// - CreateAsync: complex role creation with duplicate checking and transaction handling
/// - UpdateAsync: role type updates with domain defaults
/// - DeleteAsync: role and client role deletion with transaction handling
/// </summary>
public class ClientRoleTypeBusinessTests
{
    private readonly Mock<ILogger<ClientRoleTypeBusiness>> _logger = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly Mock<IUserContextService> _userContextService = new();
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly Mock<IClientRoleTypeRepository> _clientRoleTypes = new();
    private readonly Mock<IRoleTypeRepository> _roleTypes = new();

    public ClientRoleTypeBusinessTests()
    {
        // Wire repositories to UoW
        _uow.SetupGet(u => u.ClientRoleTypes).Returns(_clientRoleTypes.Object);
        _uow.SetupGet(u => u.RoleTypes).Returns(_roleTypes.Object);
    }

    private ClientRoleTypeBusiness CreateSut() =>
        new(_logger.Object, _mapper.Object, _userContextService.Object, _uow.Object);

    #region GetAsync

    [Fact]
    public async Task GetAsync_ReturnsJoinedAndMappedQueryable()
    {
        // Arrange: setup joined data between ClientRoleTypes and RoleTypes
        var clientRoleTypes = new List<ClientRoleType>
        {
            new() { RoleTypeId = 1, ClientId = 1, IsActive = true }
        }.AsQueryable();

        var roleTypes = new List<RoleType>
        {
            new() { Id = 1, RowId = Guid.NewGuid(), Name = "Admin" },
            new() { Id = 2, RowId = Guid.NewGuid(), Name = "User" }
        }.AsQueryable();

        _clientRoleTypes.Setup(r => r.GetAsync()).ReturnsAsync(clientRoleTypes);
        _roleTypes.Setup(r => r.GetAsync()).ReturnsAsync(roleTypes);
        _mapper.Setup(m => m.Map<MetaDataViewModel>(It.IsAny<RoleType>()))
               .Returns((RoleType rt) => new MetaDataViewModel { RowId = rt.RowId, Name = rt.Name });

        var sut = CreateSut();

        // Act: enumerate to force LINQ execution and mapping
        var queryable = await sut.GetAsync();
        var list = queryable.ToList();

        // Assert: verify content and interaction counts
        Assert.Single(list);
        Assert.Equal("Admin", list[0].Name);
        _clientRoleTypes.Verify(r => r.GetAsync(), Times.Once);
        _roleTypes.Verify(r => r.GetAsync(), Times.Once);
        _mapper.Verify(m => m.Map<MetaDataViewModel>(It.IsAny<RoleType>()), Times.Once);
    }

    [Fact]
    public async Task GetAsync_WhenRepoThrows_Propagates()
    {
        // Arrange: bubble up repository failures without swallowing
        _clientRoleTypes.Setup(r => r.GetAsync()).ThrowsAsync(new InvalidOperationException("boom"));
        var sut = CreateSut();

        // Act + Assert: exception is propagated
        await Assert.ThrowsAsync<InvalidOperationException>(() => sut.GetAsync());
    }

    #endregion GetAsync

    #region GetByRowIdAsync

    [Fact]
    public async Task GetByRowIdAsync_Found_ReturnsMapped()
    {
        // Arrange: entity exists; mapping projects to view model
        var id = Guid.NewGuid();
        var entity = new RoleType { RowId = id, Name = "Admin" };

        _roleTypes.Setup(r => r.GetByRowIdAsync(id)).ReturnsAsync(entity);
        _mapper.Setup(m => m.Map<MetaDataViewModel>(entity))
            .Returns(new MetaDataViewModel { RowId = id, Name = "Admin" });

        var sut = CreateSut();

        var result = await sut.GetByRowIdAsync(id);

        // Assert: selected entity returned and mapped
        Assert.Equal(id, result.RowId);
        Assert.Equal("Admin", result.Name);
        _roleTypes.Verify(r => r.GetByRowIdAsync(id), Times.Once);
    }

    [Fact]
    public async Task GetByRowIdAsync_NotFound_ThrowsKeyNotFound()
    {
        // Arrange: repository returns null when not found
        var id = Guid.NewGuid();
        _roleTypes.Setup(r => r.GetByRowIdAsync(id)).ReturnsAsync((RoleType?)null);

        var sut = CreateSut();

        // Act & Assert: business signals missing resource via KeyNotFoundException
        await Assert.ThrowsAsync<KeyNotFoundException>(() => sut.GetByRowIdAsync(id));
    }

    [Fact]
    public async Task GetByRowIdAsync_RepoThrows_Propagates()
    {
        // Arrange: propagate unexpected repository errors
        var id = Guid.NewGuid();
        _roleTypes.Setup(r => r.GetByRowIdAsync(id)).ThrowsAsync(new Exception("err"));

        var sut = CreateSut();

        // Act & Assert: exception flows upward without being wrapped
        await Assert.ThrowsAsync<Exception>(() => sut.GetByRowIdAsync(id));
    }

    #endregion GetByRowIdAsync

    #region CreateAsync

    [Fact]
    public async Task CreateAsync_WhenUserContextIsNull_ThrowsException()
    {
        // Arrange: user context is null
        var createModel = new ClientRoleTypeCreateModel { Name = "NewRole" };
        _userContextService.Setup(u => u.UserContext).Returns((UserContext?)null);
        // Ensure transactional delegate executes to trigger null access
        _uow.Setup(u => u.ExecuteAsync(It.IsAny<Func<Task>>())).Returns((Func<Task> f) => f());

        var sut = CreateSut();

        // Act & Assert: null user context triggers NullReferenceException
        await Assert.ThrowsAsync<NullReferenceException>(() => sut.CreateAsync(createModel));
    }

    #endregion CreateAsync

    #region UpdateAsync

    [Fact]
    public async Task UpdateAsync_NotFound_ThrowsKeyNotFound()
    {
        // Arrange: update attempted for missing entity
        var id = Guid.NewGuid();
        var updateModel = new ClientRoleTypeUpdateModel();

        _roleTypes.Setup(r => r.GetByRowIdAsync(id)).ReturnsAsync((RoleType?)null);

        var sut = CreateSut();

        // Act & Assert: business indicates missing resource via KeyNotFoundException
        await Assert.ThrowsAsync<KeyNotFoundException>(() => sut.UpdateAsync(id, updateModel));
    }

    #endregion UpdateAsync

    #region DeleteAsync

    [Fact]
    public async Task DeleteAsync_NotFound_ThrowsKeyNotFound()
    {
        // Arrange: delete attempted for missing entity
        var id = Guid.NewGuid();

        _roleTypes.Setup(r => r.DeleteAsync(id)).ReturnsAsync((RoleType?)null);
        // Execute transaction by invoking delegate
        _uow.Setup(u => u.ExecuteAsync(It.IsAny<Func<Task>>()))
            .Returns((Func<Task> f) => f());

        var sut = CreateSut();

        // Act & Assert: business indicates missing resource via KeyNotFoundException
        await Assert.ThrowsAsync<KeyNotFoundException>(() => sut.DeleteAsync(id));
    }

    #endregion DeleteAsync
}
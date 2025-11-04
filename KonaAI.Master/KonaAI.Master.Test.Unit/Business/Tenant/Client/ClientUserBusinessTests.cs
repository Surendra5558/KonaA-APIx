using AutoMapper;
using KonaAI.Master.Business.Tenant.Client.Logic;
using KonaAI.Master.Model.Tenant.Client.SaveModel;
using KonaAI.Master.Model.Tenant.Client.ViewModel;
using KonaAI.Master.Repository;
using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Repository.DataAccess.Master.App.Interface;
using KonaAI.Master.Repository.DataAccess.Master.MetaData.Interface;
using KonaAI.Master.Repository.DataAccess.Tenant.Client.Interface;
using KonaAI.Master.Repository.Domain.Master.App;
using KonaAI.Master.Repository.Domain.Master.MetaData;
using KonaAI.Master.Repository.Domain.Tenant.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace KonaAI.Master.Test.Unit.Business.Tenant.Client;

/// <summary>
/// Unit tests for <see cref="KonaAI.Master.Business.Tenant.Client.Logic.ClientUserBusiness"/>.
/// Covers:
/// - Reads: join ClientUsers with Users and map; error propagation
/// - Detail: mapped return or KeyNotFound when missing
/// - Create: duplicate username detection; success path creates User and ClientUser and saves twice
/// </summary>
public class ClientUserBusinessTests
{
    private readonly Mock<ILogger<ClientUserBusiness>> _logger = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly Mock<IUserContextService> _userContext = new();
    private readonly Mock<IUnitOfWork> _uow = new();

    private readonly Mock<IUserRepository> _users = new();
    private readonly Mock<IClientUserRepository> _clientUsers = new();
    private readonly Mock<ILogOnTypeRepository> _logOnTypes = new();

    public ClientUserBusinessTests()
    {
        // Wire UoW repositories used by business logic
        _uow.SetupGet(x => x.Users).Returns(_users.Object);
        _uow.SetupGet(x => x.ClientUsers).Returns(_clientUsers.Object);
        _uow.SetupGet(x => x.LogOnTypes).Returns(_logOnTypes.Object);
    }

    private ClientUserBusiness CreateSut() => new(_logger.Object, _mapper.Object, _userContext.Object, _uow.Object);

    [Fact]
    public async Task GetAsync_ReturnsJoinedAndMappedQueryable()
    {
        // Arrange: join ClientUsers with Users and map to ClientUserViewModel
        var u = new User { Id = 1, RowId = Guid.NewGuid(), UserName = "u1", Email = "e@x.com", IsActive = true };
        var cu = new ClientUser { Id = 10, RowId = Guid.NewGuid(), UserId = u.Id, IsActive = true };

        _clientUsers.Setup(r => r.GetAsync()).ReturnsAsync(new List<ClientUser> { cu }.AsQueryable());
        _users.Setup(r => r.GetAsync()).ReturnsAsync(new List<User> { u }.AsQueryable());

        _mapper.Setup(m => m.Map<ClientUserViewModel>(It.IsAny<User>()))
               .Returns((User x) => new ClientUserViewModel { RowId = x.RowId, UserName = x.UserName, Email = x.Email });

        var sut = CreateSut();

        // Act: enumerate IQueryable to force mapping execution
        var queryable = await sut.GetAsync();
        var list = queryable.ToList();

        // Assert: joined and mapped once; interactions verified
        Assert.Single(list);
        Assert.Equal("u1", list[0].UserName);
        _clientUsers.Verify(r => r.GetAsync(), Times.Once);
        _users.Verify(r => r.GetAsync(), Times.Once);
        _mapper.Verify(m => m.Map<ClientUserViewModel>(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task GetAsync_WhenRepoThrows_Propagates()
    {
        // Arrange: ensure errors from repositories are not swallowed
        _clientUsers.Setup(r => r.GetAsync()).ThrowsAsync(new InvalidOperationException("boom"));
        var sut = CreateSut();

        // Act + Assert: exception propagates
        await Assert.ThrowsAsync<InvalidOperationException>(() => sut.GetAsync());
    }

    [Fact]
    public async Task GetByRowIdAsync_Found_ReturnsMapped()
    {
        // Arrange: found user mapped to view model
        var id = Guid.NewGuid();
        var u = new User { RowId = id, Id = 1, UserName = "u1", Email = "e@x.com" };
        _users.Setup(r => r.GetByRowIdAsync(id)).ReturnsAsync(u);
        _mapper.Setup(m => m.Map<ClientUserViewModel>(u))
               .Returns(new ClientUserViewModel { RowId = id, UserName = "u1" });

        var sut = CreateSut();

        // Act
        var vm = await sut.GetByRowIdAsync(id);

        // Assert
        Assert.NotNull(vm);
        Assert.Equal(id, vm!.RowId);
        _users.Verify(r => r.GetByRowIdAsync(id), Times.Once);
    }

    [Fact]
    public async Task GetByRowIdAsync_NotFound_ThrowsKeyNotFound()
    {
        // Arrange: simulate not found
        var id = Guid.NewGuid();
        _users.Setup(r => r.GetByRowIdAsync(id)).ReturnsAsync((User?)null);
        var sut = CreateSut();

        // Act + Assert: KeyNotFoundException thrown
        await Assert.ThrowsAsync<KeyNotFoundException>(() => sut.GetByRowIdAsync(id));
    }

    [Fact]
    public async Task CreateAsync_WhenDuplicateUserName_ThrowsKeyNotFound()
    {
        // Arrange: simulate existing active user with same username
        var payload = new ClientUserCreateModel { UserName = "u1", Password = "p@ss" };

        // Provide a fake EF context with Users DbSet containing the duplicate
        var ctxDup = new FakeDefaultContext(
            BuildDbSet(new[] { new User { Id = 1, UserName = "u1", IsActive = true } }),
            BuildDbSet(Array.Empty<ClientUser>()),
            BuildDbSet(new[] { new LogOnType { Id = 3, Name = "Forms" } })
        );
        _users.SetupGet(r => r.Context).Returns(ctxDup);
        _clientUsers.SetupGet(r => r.Context).Returns(ctxDup);
        _logOnTypes.SetupGet(r => r.Context).Returns(ctxDup);
        _uow.Setup(u => u.ExecuteAsync(It.IsAny<Func<Task>>())).Returns<Func<Task>>(f => f());

        var sut = CreateSut();

        // Act + Assert: duplicate check triggers KeyNotFoundException per business logic
        await Assert.ThrowsAsync<KeyNotFoundException>(() => sut.CreateAsync(payload));
    }

    [Fact]
    public async Task CreateAsync_Success_CreatesUserAndClientUserAndSaves()
    {
        // Arrange: no existing user; valid LogOnType; add user then clientUser; SaveChanges called
        var payload = new ClientUserCreateModel { UserName = "u2", Password = "p@ss" };

        // Provide a fake EF context with empty Users/ClientUsers and one LogOnType("Forms")
        var ctx = new FakeDefaultContext(
            BuildDbSet(Array.Empty<User>()),
            BuildDbSet(Array.Empty<ClientUser>()),
            BuildDbSet(new[] { new LogOnType { Id = 3, Name = "Forms" } })
        );
        _users.SetupGet(r => r.Context).Returns(ctx);
        _clientUsers.SetupGet(r => r.Context).Returns(ctx);
        _logOnTypes.SetupGet(r => r.Context).Returns(ctx);

        _users.Setup(r => r.AddAsync(It.IsAny<User>())).ReturnsAsync((User u) => { u.Id = 100; return u; });
        _clientUsers.Setup(r => r.AddAsync(It.IsAny<ClientUser>())).ReturnsAsync((ClientUser cu) => cu);
        _uow.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);
        _uow.Setup(u => u.ExecuteAsync(It.IsAny<Func<Task>>())).Returns<Func<Task>>(f => f());

        // Map payloads to domain entities
        _mapper.Setup(m => m.Map<User>(It.IsAny<ClientUserCreateModel>()))
               .Returns((ClientUserCreateModel p) => new User { UserName = p.UserName ?? string.Empty, Password = p.Password ?? string.Empty, IsActive = true });
        _mapper.Setup(m => m.Map<ClientUserCreateModel, ClientUser>(It.IsAny<ClientUserCreateModel>()))
               .Returns((ClientUserCreateModel p) => new ClientUser { IsActive = true });

        var sut = CreateSut();

        // Act
        var count = await sut.CreateAsync(payload);

        // Assert
        Assert.Equal(1, count);
        _users.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Once);
        _clientUsers.Verify(r => r.AddAsync(It.IsAny<ClientUser>()), Times.Once);
        _uow.Verify(u => u.SaveChangesAsync(default), Times.Exactly(2)); // one after user, one after clientUser per business logic
    }

    // Helper to build an IQueryable-backed DbSet for sync LINQ (FirstOrDefault/Where)
    private static DbSet<T> BuildDbSet<T>(IEnumerable<T> data) where T : class
    {
        var queryable = data.AsQueryable();
        var mockSet = new Mock<DbSet<T>>();
        mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
        mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
        mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
        mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
        return mockSet.Object;
    }

    // Minimal fake context exposing needed DbSets for tests
    private sealed class FakeDefaultContext : DefaultContext
    {
        public FakeDefaultContext(DbSet<User> users, DbSet<ClientUser> clientUsers, DbSet<LogOnType> logOnTypes)
            : base(new DbContextOptions<DefaultContext>(), Mock.Of<IUserContextService>())
        {
            Users = users;
            ClientUsers = clientUsers;
            LogOnTypes = logOnTypes;
        }
    }
}
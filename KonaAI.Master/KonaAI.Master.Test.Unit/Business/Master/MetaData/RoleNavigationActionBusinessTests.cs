using AutoMapper;
using KonaAI.Master.Business.Master.MetaData.Logic;
using KonaAI.Master.Model.Master.MetaData;
using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Repository.DataAccess.Master.MetaData.Interface;
using KonaAI.Master.Repository.Domain.Master.MetaData;
using Microsoft.Extensions.Logging;
using Moq;

namespace KonaAI.Master.Test.Unit.Business.Master.MetaData;

/// <summary>
/// Unit tests for <see cref="KonaAI.Master.Business.Master.MetaData.Logic.RoleNavigationUserActionBusiness"/>.
/// Validates:
/// - GetAsync maps repository <see cref="KonaAI.Master.Repository.Domain.Master.MetaData.RoleNavigationUserAction"/> to <see cref="KonaAI.Master.Model.Master.MetaData.RoleNavigationUserActionViewModel"/>
/// - Repository exceptions are propagated and not swallowed
/// </summary>
public class RoleNavigationActionBusinessTests
{
    private readonly Mock<ILogger<RoleNavigationUserActionBusiness>> _logger = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly Mock<IRoleNavigationUserActionRepository> _roleNavigationUserActions = new();

    public RoleNavigationActionBusinessTests()
    {
        // Wire UnitOfWork.RoleNavigationUserActions to our role navigation user action repository mock to mirror production resolution
        _uow.SetupGet(x => x.RoleNavigationUserActions).Returns(_roleNavigationUserActions.Object);
    }

    private RoleNavigationUserActionBusiness CreateSut() => new(_logger.Object, _mapper.Object, _uow.Object);

    [Fact]
    public async Task GetAsync_ReturnsMappedQueryable()
    {
        // Arrange: repository returns two role navigation user actions; mapper projects each to RoleNavigationUserActionViewModel
        var data = new List<RoleNavigationUserAction>
        {
            new() { Id = 1, RoleTypeId = 1, NavigationUserActionId = 1 },
            new() { Id = 2, RoleTypeId = 2, NavigationUserActionId = 2 }
        }.AsQueryable();

        _roleNavigationUserActions.Setup(r => r.GetAsync()).ReturnsAsync(data);
        _mapper.Setup(m => m.Map<RoleNavigationUserActionViewModel>(It.IsAny<RoleNavigationUserAction>()))
               .Returns((RoleNavigationUserAction rnua) => new RoleNavigationUserActionViewModel
               {
                   RowId = Guid.NewGuid(),
                   RoleTypeRowId = Guid.NewGuid(),
                   RoleTypeName = "Test Role",
                   NavigationRowId = Guid.NewGuid(),
                   NavigationName = "Test Navigation",
                   UserActionRowId = Guid.NewGuid(),
                   UserActionName = "Test Action"
               });

        var sut = CreateSut();

        // Act: enumerate to force LINQ execution and mapping
        var queryable = await sut.GetAsync();
        var list = queryable.ToList();

        // Assert: verify content and interaction counts
        Assert.Equal(2, list.Count);
        Assert.Contains(list, x => x.RoleTypeName == "Test Role");
        Assert.Contains(list, x => x.NavigationName == "Test Navigation");
        _roleNavigationUserActions.Verify(r => r.GetAsync(), Times.Once);
        _mapper.Verify(m => m.Map<RoleNavigationUserActionViewModel>(It.IsAny<RoleNavigationUserAction>()), Times.Exactly(2));
    }

    [Fact]
    public async Task GetAsync_WhenRepoThrows_Propagates()
    {
        // Arrange: bubble up repository failures without swallowing
        _roleNavigationUserActions.Setup(r => r.GetAsync()).ThrowsAsync(new InvalidOperationException("boom"));
        var sut = CreateSut();

        // Act + Assert: exception is propagated
        await Assert.ThrowsAsync<InvalidOperationException>(() => sut.GetAsync());
    }
}
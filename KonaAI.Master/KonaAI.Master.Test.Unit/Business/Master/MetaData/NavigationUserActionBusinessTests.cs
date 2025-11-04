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
/// Unit tests for <see cref="KonaAI.Master.Business.Master.MetaData.Logic.NavigationUserActionBusiness"/>.
/// Validates:
/// - GetAsync maps repository <see cref="KonaAI.Master.Repository.Domain.Master.MetaData.NavigationUserAction"/> to <see cref="KonaAI.Master.Model.Master.MetaData.NavigationUserActionViewModel"/>
/// - Repository exceptions are propagated and not swallowed
/// </summary>
public class NavigationUserActionBusinessTests
{
    private readonly Mock<ILogger<NavigationUserActionBusiness>> _logger = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly Mock<INavigationUserActionRepository> _navigationUserActions = new();

    public NavigationUserActionBusinessTests()
    {
        // Wire UnitOfWork.NavigationUserActions to our navigation user action repository mock to mirror production resolution
        _uow.SetupGet(x => x.NavigationUserActions).Returns(_navigationUserActions.Object);
    }

    private NavigationUserActionBusiness CreateSut() => new(_logger.Object, _mapper.Object, _uow.Object);

    [Fact]
    public async Task GetAsync_ReturnsMappedQueryable()
    {
        // Arrange: repository returns two navigation user actions; mapper projects each to NavigationUserActionViewModel
        var data = new List<NavigationUserAction>
        {
            new() { Id = 1, NavigationId = 1, UserActionId = 1 },
            new() { Id = 2, NavigationId = 2, UserActionId = 2 }
        }.AsQueryable();

        _navigationUserActions.Setup(r => r.GetAsync()).ReturnsAsync(data);
        _mapper.Setup(m => m.Map<NavigationUserActionViewModel>(It.IsAny<NavigationUserAction>()))
               .Returns((NavigationUserAction nua) => new NavigationUserActionViewModel
               {
                   RowId = Guid.NewGuid(),
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
        Assert.Contains(list, x => x.NavigationName == "Test Navigation");
        Assert.Contains(list, x => x.UserActionName == "Test Action");
        _navigationUserActions.Verify(r => r.GetAsync(), Times.Once);
        _mapper.Verify(m => m.Map<NavigationUserActionViewModel>(It.IsAny<NavigationUserAction>()), Times.Exactly(2));
    }

    [Fact]
    public async Task GetAsync_WhenRepoThrows_Propagates()
    {
        // Arrange: bubble up repository failures without swallowing
        _navigationUserActions.Setup(r => r.GetAsync()).ThrowsAsync(new InvalidOperationException("boom"));
        var sut = CreateSut();

        // Act + Assert: exception is propagated
        await Assert.ThrowsAsync<InvalidOperationException>(() => sut.GetAsync());
    }
}
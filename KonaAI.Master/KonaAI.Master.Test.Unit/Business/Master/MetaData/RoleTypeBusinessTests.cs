using AutoMapper;
using KonaAI.Master.Business.Master.MetaData.Logic;
using KonaAI.Master.Model.Common;
using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Repository.DataAccess.Master.MetaData.Interface;
using KonaAI.Master.Repository.Domain.Master.MetaData;
using Microsoft.Extensions.Logging;
using Moq;

namespace KonaAI.Master.Test.Unit.Business.Master.MetaData;

/// <summary>
/// Unit tests for <see cref="KonaAI.Master.Business.Master.MetaData.Logic.RoleTypeBusiness"/>.
/// Validates:
/// - GetAsync maps repository <see cref="KonaAI.Master.Repository.Domain.Master.MetaData.RoleType"/> to <see cref="KonaAI.Master.Model.Common.MetaDataViewModel"/>
/// - Repository exceptions are propagated and not swallowed
/// </summary>
public class RoleTypeBusinessTests
{
    private readonly Mock<ILogger<RoleTypeBusiness>> _logger = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly Mock<IRoleTypeRepository> _roleTypes = new();

    public RoleTypeBusinessTests()
    {
        // Wire UnitOfWork.RoleTypes to our role type repository mock to mirror production resolution
        _uow.SetupGet(x => x.RoleTypes).Returns(_roleTypes.Object);
    }

    private RoleTypeBusiness CreateSut() => new(_logger.Object, _mapper.Object, _uow.Object);

    [Fact]
    public async Task GetAsync_ReturnsMappedQueryable()
    {
        // Arrange: repository returns two role types; mapper projects each to MetaDataViewModel
        var data = new List<RoleType>
        {
            new() { RowId = Guid.NewGuid(), Name = "Admin" },
            new() { RowId = Guid.NewGuid(), Name = "User" }
        }.AsQueryable();

        _roleTypes.Setup(r => r.GetAsync()).ReturnsAsync(data);
        _mapper.Setup(m => m.Map<MetaDataViewModel>(It.IsAny<RoleType>()))
               .Returns((RoleType rt) => new MetaDataViewModel { RowId = rt.RowId, Name = rt.Name });

        var sut = CreateSut();

        // Act: enumerate to force LINQ execution and mapping
        var queryable = await sut.GetAsync();
        var list = queryable.ToList();

        // Assert: verify content and interaction counts
        Assert.Equal(2, list.Count);
        Assert.Contains(list, x => x.Name == "Admin");
        Assert.Contains(list, x => x.Name == "User");
        _roleTypes.Verify(r => r.GetAsync(), Times.Once);
        _mapper.Verify(m => m.Map<MetaDataViewModel>(It.IsAny<RoleType>()), Times.Exactly(2));
    }

    [Fact]
    public async Task GetAsync_WhenRepoThrows_Propagates()
    {
        // Arrange: bubble up repository failures without swallowing
        _roleTypes.Setup(r => r.GetAsync()).ThrowsAsync(new InvalidOperationException("boom"));
        var sut = CreateSut();

        // Act + Assert: exception is propagated
        await Assert.ThrowsAsync<InvalidOperationException>(() => sut.GetAsync());
    }
}
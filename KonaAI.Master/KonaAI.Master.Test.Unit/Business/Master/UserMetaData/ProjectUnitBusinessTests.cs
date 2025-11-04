using AutoMapper;
using KonaAI.Master.Business.Master.UserMetaData.Logic;
using KonaAI.Master.Model.Common;
using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Repository.DataAccess.Master.UserMetaData.Interface;
using KonaAI.Master.Repository.Domain.Master.UserMetaData;
using Microsoft.Extensions.Logging;
using Moq;

namespace KonaAI.Master.Test.Unit.Business.Master.UserMetaData;

/// <summary>
/// Unit tests for <see cref="KonaAI.Master.Business.Master.UserMetaData.Logic.ProjectUnitBusiness"/>.
/// Validates:
/// - GetAsync maps repository <see cref="KonaAI.Master.Repository.Domain.Master.UserMetaData.ProjectUnit"/> to <see cref="KonaAI.Master.Model.Common.MetaDataViewModel"/>
/// - Repository exceptions are propagated and not swallowed
/// </summary>
public class ProjectUnitBusinessTests
{
    private readonly Mock<ILogger<ProjectUnitBusiness>> _logger = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly Mock<IProjectUnitRepository> _projectUnits = new();

    public ProjectUnitBusinessTests()
    {
        // Wire UnitOfWork.ProjectUnits to our project unit repository mock to mirror production resolution
        _uow.SetupGet(x => x.ProjectUnits).Returns(_projectUnits.Object);
    }

    private ProjectUnitBusiness CreateSut() => new(_logger.Object, _mapper.Object, _uow.Object);

    [Fact]
    public async Task GetAsync_ReturnsMappedQueryable()
    {
        // Arrange: repository returns two project units; mapper projects each to MetaDataViewModel
        var data = new List<ProjectUnit>
        {
            new() { RowId = Guid.NewGuid(), Name = "Development Team" },
            new() { RowId = Guid.NewGuid(), Name = "Testing Team" }
        }.AsQueryable();

        _projectUnits.Setup(r => r.GetAsync()).ReturnsAsync(data);
        _mapper.Setup(m => m.Map<MetaDataViewModel>(It.IsAny<ProjectUnit>()))
               .Returns((ProjectUnit pu) => new MetaDataViewModel { RowId = pu.RowId, Name = pu.Name });

        var sut = CreateSut();

        // Act: enumerate to force LINQ execution and mapping
        var queryable = await sut.GetAsync();
        var list = queryable.ToList();

        // Assert: verify content and interaction counts
        Assert.Equal(2, list.Count);
        Assert.Contains(list, x => x.Name == "Development Team");
        Assert.Contains(list, x => x.Name == "Testing Team");
        _projectUnits.Verify(r => r.GetAsync(), Times.Once);
        _mapper.Verify(m => m.Map<MetaDataViewModel>(It.IsAny<ProjectUnit>()), Times.Exactly(2));
    }

    [Fact]
    public async Task GetAsync_WhenRepoThrows_Propagates()
    {
        // Arrange: bubble up repository failures without swallowing
        _projectUnits.Setup(r => r.GetAsync()).ThrowsAsync(new InvalidOperationException("boom"));
        var sut = CreateSut();

        // Act + Assert: exception is propagated
        await Assert.ThrowsAsync<InvalidOperationException>(() => sut.GetAsync());
    }
}
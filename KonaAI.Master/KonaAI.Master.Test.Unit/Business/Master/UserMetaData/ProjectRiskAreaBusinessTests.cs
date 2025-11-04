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
/// Unit tests for <see cref="KonaAI.Master.Business.Master.UserMetaData.Logic.ProjectRiskAreaBusiness"/>.
/// Validates:
/// - GetAsync maps repository <see cref="KonaAI.Master.Repository.Domain.Master.UserMetaData.ProjectRiskArea"/> to <see cref="KonaAI.Master.Model.Common.MetaDataViewModel"/>
/// - Repository exceptions are propagated and not swallowed
/// </summary>
public class ProjectRiskAreaBusinessTests
{
    private readonly Mock<ILogger<ProjectRiskAreaBusiness>> _logger = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly Mock<IProjectRiskAreaRepository> _projectRiskAreas = new();

    public ProjectRiskAreaBusinessTests()
    {
        // Wire UnitOfWork.ProjectRiskAreas to our project risk area repository mock to mirror production resolution
        _uow.SetupGet(x => x.ProjectRiskAreas).Returns(_projectRiskAreas.Object);
    }

    private ProjectRiskAreaBusiness CreateSut() => new(_logger.Object, _mapper.Object, _uow.Object);

    [Fact]
    public async Task GetAsync_ReturnsMappedQueryable()
    {
        // Arrange: repository returns two project risk areas; mapper projects each to MetaDataViewModel
        var data = new List<ProjectRiskArea>
        {
            new() { RowId = Guid.NewGuid(), Name = "Technical Risk" },
            new() { RowId = Guid.NewGuid(), Name = "Business Risk" }
        }.AsQueryable();

        _projectRiskAreas.Setup(r => r.GetAsync()).ReturnsAsync(data);
        _mapper.Setup(m => m.Map<MetaDataViewModel>(It.IsAny<ProjectRiskArea>()))
               .Returns((ProjectRiskArea pra) => new MetaDataViewModel { RowId = pra.RowId, Name = pra.Name });

        var sut = CreateSut();

        // Act: enumerate to force LINQ execution and mapping
        var queryable = await sut.GetAsync();
        var list = queryable.ToList();

        // Assert: verify content and interaction counts
        Assert.Equal(2, list.Count);
        Assert.Contains(list, x => x.Name == "Technical Risk");
        Assert.Contains(list, x => x.Name == "Business Risk");
        _projectRiskAreas.Verify(r => r.GetAsync(), Times.Once);
        _mapper.Verify(m => m.Map<MetaDataViewModel>(It.IsAny<ProjectRiskArea>()), Times.Exactly(2));
    }

    [Fact]
    public async Task GetAsync_WhenRepoThrows_Propagates()
    {
        // Arrange: bubble up repository failures without swallowing
        _projectRiskAreas.Setup(r => r.GetAsync()).ThrowsAsync(new InvalidOperationException("boom"));
        var sut = CreateSut();

        // Act + Assert: exception is propagated
        await Assert.ThrowsAsync<InvalidOperationException>(() => sut.GetAsync());
    }
}
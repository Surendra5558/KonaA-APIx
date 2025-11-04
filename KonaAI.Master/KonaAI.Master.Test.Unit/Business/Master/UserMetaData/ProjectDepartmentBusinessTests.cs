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
/// Unit tests for <see cref="KonaAI.Master.Business.Master.UserMetaData.Logic.ProjectDepartmentBusiness"/>.
/// Validates:
/// - GetAsync maps repository <see cref="KonaAI.Master.Repository.Domain.Master.UserMetaData.ProjectDepartment"/> to <see cref="KonaAI.Master.Model.Common.MetaDataViewModel"/>
/// - Repository exceptions are propagated and not swallowed
/// </summary>
public class ProjectDepartmentBusinessTests
{
    private readonly Mock<ILogger<ProjectDepartmentBusiness>> _logger = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly Mock<IProjectDepartmentRepository> _projectDepartments = new();

    public ProjectDepartmentBusinessTests()
    {
        // Wire UnitOfWork.ProjectDepartments to our project department repository mock to mirror production resolution
        _uow.SetupGet(x => x.ProjectDepartments).Returns(_projectDepartments.Object);
    }

    private ProjectDepartmentBusiness CreateSut() => new(_logger.Object, _mapper.Object, _uow.Object);

    [Fact]
    public async Task GetAsync_ReturnsMappedQueryable()
    {
        // Arrange: repository returns two project departments; mapper projects each to MetaDataViewModel
        var data = new List<ProjectDepartment>
        {
            new() { RowId = Guid.NewGuid(), Name = "Engineering" },
            new() { RowId = Guid.NewGuid(), Name = "Quality Assurance" }
        }.AsQueryable();

        _projectDepartments.Setup(r => r.GetAsync()).ReturnsAsync(data);
        _mapper.Setup(m => m.Map<MetaDataViewModel>(It.IsAny<ProjectDepartment>()))
               .Returns((ProjectDepartment pd) => new MetaDataViewModel { RowId = pd.RowId, Name = pd.Name });

        var sut = CreateSut();

        // Act: enumerate to force LINQ execution and mapping
        var queryable = await sut.GetAsync();
        var list = queryable.ToList();

        // Assert: verify content and interaction counts
        Assert.Equal(2, list.Count);
        Assert.Contains(list, x => x.Name == "Engineering");
        Assert.Contains(list, x => x.Name == "Quality Assurance");
        _projectDepartments.Verify(r => r.GetAsync(), Times.Once);
        _mapper.Verify(m => m.Map<MetaDataViewModel>(It.IsAny<ProjectDepartment>()), Times.Exactly(2));
    }

    [Fact]
    public async Task GetAsync_WhenRepoThrows_Propagates()
    {
        // Arrange: bubble up repository failures without swallowing
        _projectDepartments.Setup(r => r.GetAsync()).ThrowsAsync(new InvalidOperationException("boom"));
        var sut = CreateSut();

        // Act + Assert: exception is propagated
        await Assert.ThrowsAsync<InvalidOperationException>(() => sut.GetAsync());
    }
}
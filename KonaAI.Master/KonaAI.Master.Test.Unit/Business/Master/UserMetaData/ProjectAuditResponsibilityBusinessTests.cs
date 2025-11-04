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
/// Unit tests for <see cref="KonaAI.Master.Business.Master.UserMetaData.Logic.ProjectAuditResponsibilityBusiness"/>.
/// Validates:
/// - GetAsync maps repository <see cref="KonaAI.Master.Repository.Domain.Master.UserMetaData.ProjectAuditResponsibility"/> to <see cref="KonaAI.Master.Model.Common.MetaDataViewModel"/>
/// - Repository exceptions are propagated and not swallowed
/// </summary>
public class ProjectAuditResponsibilityBusinessTests
{
    private readonly Mock<ILogger<ProjectAuditResponsibilityBusiness>> _logger = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly Mock<IProjectAuditResponsibilityRepository> _projectAuditResponsibilities = new();

    public ProjectAuditResponsibilityBusinessTests()
    {
        // Wire UnitOfWork.ProjectAuditResponsibilities to our project audit responsibility repository mock to mirror production resolution
        _uow.SetupGet(x => x.ProjectAuditResponsibilities).Returns(_projectAuditResponsibilities.Object);
    }

    private ProjectAuditResponsibilityBusiness CreateSut() => new(_logger.Object, _mapper.Object, _uow.Object);

    [Fact]
    public async Task GetAsync_ReturnsMappedQueryable()
    {
        // Arrange: repository returns two project audit responsibilities; mapper projects each to MetaDataViewModel
        var data = new List<ProjectAuditResponsibility>
        {
            new() { RowId = Guid.NewGuid(), Name = "Financial Audit" },
            new() { RowId = Guid.NewGuid(), Name = "Compliance Audit" }
        }.AsQueryable();

        _projectAuditResponsibilities.Setup(r => r.GetAsync()).ReturnsAsync(data);
        _mapper.Setup(m => m.Map<MetaDataViewModel>(It.IsAny<ProjectAuditResponsibility>()))
               .Returns((ProjectAuditResponsibility par) => new MetaDataViewModel { RowId = par.RowId, Name = par.Name });

        var sut = CreateSut();

        // Act: enumerate to force LINQ execution and mapping
        var queryable = await sut.GetAsync();
        var list = queryable.ToList();

        // Assert: verify content and interaction counts
        Assert.Equal(2, list.Count);
        Assert.Contains(list, x => x.Name == "Financial Audit");
        Assert.Contains(list, x => x.Name == "Compliance Audit");
        _projectAuditResponsibilities.Verify(r => r.GetAsync(), Times.Once);
        _mapper.Verify(m => m.Map<MetaDataViewModel>(It.IsAny<ProjectAuditResponsibility>()), Times.Exactly(2));
    }

    [Fact]
    public async Task GetAsync_WhenRepoThrows_Propagates()
    {
        // Arrange: bubble up repository failures without swallowing
        _projectAuditResponsibilities.Setup(r => r.GetAsync()).ThrowsAsync(new InvalidOperationException("boom"));
        var sut = CreateSut();

        // Act + Assert: exception is propagated
        await Assert.ThrowsAsync<InvalidOperationException>(() => sut.GetAsync());
    }
}
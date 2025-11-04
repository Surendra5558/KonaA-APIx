using AutoMapper;
using KonaAI.Master.Business.Tenant.UserMetaData.Logic;
using KonaAI.Master.Model.Common;
using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Repository.Domain.Master.UserMetaData;
using KonaAI.Master.Repository.Domain.Tenant.ClientUserMetaData;
using Microsoft.Extensions.Logging;
using Moq;

namespace KonaAI.Master.Test.Unit.Business.Tenant.UserMetaData;

public class ClientProjectAuditResponsibilityBusinessTests
{
    private readonly Mock<ILogger<ClientProjectAuditResponsibilityBusiness>> _logger = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();

    // Mock repositories
    private readonly Mock<KonaAI.Master.Repository.DataAccess.Master.UserMetaData.Interface.IProjectAuditResponsibilityRepository> _projectAuditResponsibilityRepository = new();

    private readonly Mock<KonaAI.Master.Repository.DataAccess.Tenant.ClientUserMetaData.Interface.IClientProjectAuditResponsibilityRepository> _clientProjectAuditResponsibilityRepository = new();

    private readonly ClientProjectAuditResponsibilityBusiness _clientProjectAuditResponsibilityBusiness;

    public ClientProjectAuditResponsibilityBusinessTests()
    {
        _unitOfWork.Setup(uow => uow.ProjectAuditResponsibilities).Returns(_projectAuditResponsibilityRepository.Object);
        _unitOfWork.Setup(uow => uow.ClientProjectAuditResponsibilities).Returns(_clientProjectAuditResponsibilityRepository.Object);

        _clientProjectAuditResponsibilityBusiness = new ClientProjectAuditResponsibilityBusiness(_logger.Object, _mapper.Object, _unitOfWork.Object);
    }

    [Fact]
    public async Task GetAsync_ReturnsMappedQueryable()
    {
        // Arrange
        var projectAuditResponsibilities = new List<ProjectAuditResponsibility>
        {
            new() { Id = 1, RowId = Guid.NewGuid(), Name = "Internal Audit" }
        }.AsQueryable();

        var clientProjectAuditResponsibilities = new List<ClientProjectAuditResponsibility>
        {
            new() { ProjectAuditResponsibilityId = 1, ClientProjectId = Guid.NewGuid() }
        }.AsQueryable();

        _projectAuditResponsibilityRepository.Setup(r => r.GetAsync()).ReturnsAsync(projectAuditResponsibilities);
        _clientProjectAuditResponsibilityRepository.Setup(r => r.GetAsync()).ReturnsAsync(clientProjectAuditResponsibilities);
        _mapper.Setup(m => m.Map<MetaDataViewModel>(It.IsAny<ProjectAuditResponsibility>()))
               .Returns((ProjectAuditResponsibility src) => new MetaDataViewModel { RowId = src.RowId, Name = src.Name });

        // Act
        var result = await _clientProjectAuditResponsibilityBusiness.GetAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("Internal Audit", result.First().Name);
    }

    [Fact]
    public async Task GetAsync_ExceptionThrown_PropagatesException()
    {
        // Arrange
        _projectAuditResponsibilityRepository.Setup(r => r.GetAsync()).ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _clientProjectAuditResponsibilityBusiness.GetAsync());
    }
}
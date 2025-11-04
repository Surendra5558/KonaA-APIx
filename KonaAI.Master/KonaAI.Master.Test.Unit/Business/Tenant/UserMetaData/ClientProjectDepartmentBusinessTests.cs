using AutoMapper;
using KonaAI.Master.Business.Tenant.UserMetaData.Logic;
using KonaAI.Master.Model.Common;
using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Repository.Domain.Master.UserMetaData;
using KonaAI.Master.Repository.Domain.Tenant.ClientUserMetaData;
using Microsoft.Extensions.Logging;
using Moq;

namespace KonaAI.Master.Test.Unit.Business.Tenant.UserMetaData;

public class ClientProjectDepartmentBusinessTests
{
    private readonly Mock<ILogger<ClientProjectDepartmentBusiness>> _logger = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();

    // Mock repositories
    private readonly Mock<KonaAI.Master.Repository.DataAccess.Master.UserMetaData.Interface.IProjectDepartmentRepository> _projectDepartmentRepository = new();

    private readonly Mock<KonaAI.Master.Repository.DataAccess.Tenant.ClientUserMetaData.Interface.IClientProjectDepartmentRepository> _clientProjectDepartmentRepository = new();

    private readonly ClientProjectDepartmentBusiness _clientProjectDepartmentBusiness;

    public ClientProjectDepartmentBusinessTests()
    {
        _unitOfWork.Setup(uow => uow.ProjectDepartments).Returns(_projectDepartmentRepository.Object);
        _unitOfWork.Setup(uow => uow.ClientProjectDepartments).Returns(_clientProjectDepartmentRepository.Object);

        _clientProjectDepartmentBusiness = new ClientProjectDepartmentBusiness(_logger.Object, _mapper.Object, _unitOfWork.Object);
    }

    [Fact]
    public async Task GetAsync_ReturnsMappedQueryable()
    {
        // Arrange
        var projectDepartments = new List<ProjectDepartment>
        {
            new() { Id = 1, RowId = Guid.NewGuid(), Name = "HR" }
        }.AsQueryable();

        var clientProjectDepartments = new List<ClientProjectDepartment>
        {
            new() { ProjectDepartmentId = 1, ClientProjectId = Guid.NewGuid() }
        }.AsQueryable();

        _projectDepartmentRepository.Setup(r => r.GetAsync()).ReturnsAsync(projectDepartments);
        _clientProjectDepartmentRepository.Setup(r => r.GetAsync()).ReturnsAsync(clientProjectDepartments);
        _mapper.Setup(m => m.Map<MetaDataViewModel>(It.IsAny<ProjectDepartment>()))
               .Returns((ProjectDepartment src) => new MetaDataViewModel { RowId = src.RowId, Name = src.Name });

        // Act
        var result = await _clientProjectDepartmentBusiness.GetAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("HR", result.First().Name);
    }

    [Fact]
    public async Task GetAsync_ExceptionThrown_PropagatesException()
    {
        // Arrange
        _projectDepartmentRepository.Setup(r => r.GetAsync()).ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _clientProjectDepartmentBusiness.GetAsync());
    }
}
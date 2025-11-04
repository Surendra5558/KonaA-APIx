using AutoMapper;
using KonaAI.Master.Business.Tenant.UserMetaData.Logic;
using KonaAI.Master.Model.Common;
using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Repository.Domain.Master.UserMetaData;
using KonaAI.Master.Repository.Domain.Tenant.ClientUserMetaData;
using Microsoft.Extensions.Logging;
using Moq;

namespace KonaAI.Master.Test.Unit.Business.Tenant.UserMetaData;

public class ClientProjectUnitBusinessTests
{
    private readonly Mock<ILogger<ClientProjectUnitBusiness>> _logger = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();

    // Mock repositories
    private readonly Mock<KonaAI.Master.Repository.DataAccess.Master.UserMetaData.Interface.IProjectUnitRepository> _projectUnitRepository = new();

    private readonly Mock<KonaAI.Master.Repository.DataAccess.Tenant.ClientUserMetaData.Interface.IClientProjectUnitRepository> _clientProjectUnitRepository = new();

    private readonly ClientProjectUnitBusiness _clientProjectUnitBusiness;

    public ClientProjectUnitBusinessTests()
    {
        _unitOfWork.Setup(uow => uow.ProjectUnits).Returns(_projectUnitRepository.Object);
        _unitOfWork.Setup(uow => uow.ClientProjectUnits).Returns(_clientProjectUnitRepository.Object);

        _clientProjectUnitBusiness = new ClientProjectUnitBusiness(_logger.Object, _mapper.Object, _unitOfWork.Object);
    }

    [Fact]
    public async Task GetAsync_ReturnsMappedQueryable()
    {
        // Arrange
        var projectUnits = new List<ProjectUnit>
        {
            new() { Id = 1, RowId = Guid.NewGuid(), Name = "Unit A" }
        }.AsQueryable();

        var clientProjectUnits = new List<ClientProjectUnit>
        {
            new() { ProjectUnitId = 1, ClientProjectId = Guid.NewGuid() }
        }.AsQueryable();

        _projectUnitRepository.Setup(r => r.GetAsync()).ReturnsAsync(projectUnits);
        _clientProjectUnitRepository.Setup(r => r.GetAsync()).ReturnsAsync(clientProjectUnits);
        _mapper.Setup(m => m.Map<MetaDataViewModel>(It.IsAny<ProjectUnit>()))
               .Returns((ProjectUnit src) => new MetaDataViewModel { RowId = src.RowId, Name = src.Name });

        // Act
        var result = await _clientProjectUnitBusiness.GetAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("Unit A", result.First().Name);
    }

    [Fact]
    public async Task GetAsync_ExceptionThrown_PropagatesException()
    {
        // Arrange
        _projectUnitRepository.Setup(r => r.GetAsync()).ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _clientProjectUnitBusiness.GetAsync());
    }
}
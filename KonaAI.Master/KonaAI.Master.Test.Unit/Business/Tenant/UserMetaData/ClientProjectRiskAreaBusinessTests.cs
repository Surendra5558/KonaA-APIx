using AutoMapper;
using KonaAI.Master.Business.Tenant.UserMetaData.Logic;
using KonaAI.Master.Model.Common;
using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Repository.Domain.Master.UserMetaData;
using KonaAI.Master.Repository.Domain.Tenant.ClientUserMetaData;
using Microsoft.Extensions.Logging;
using Moq;

namespace KonaAI.Master.Test.Unit.Business.Tenant.UserMetaData;

public class ClientProjectRiskAreaBusinessTests
{
    private readonly Mock<ILogger<ClientProjectRiskAreaBusiness>> _logger = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();

    // Mock repositories
    private readonly Mock<KonaAI.Master.Repository.DataAccess.Master.UserMetaData.Interface.IProjectRiskAreaRepository> _projectRiskAreaRepository = new();

    private readonly Mock<KonaAI.Master.Repository.DataAccess.Tenant.ClientUserMetaData.Interface.IClientProjectRiskAreaRepository> _clientProjectRiskAreaRepository = new();

    private readonly ClientProjectRiskAreaBusiness _clientProjectRiskAreaBusiness;

    public ClientProjectRiskAreaBusinessTests()
    {
        _unitOfWork.Setup(uow => uow.ProjectRiskAreas).Returns(_projectRiskAreaRepository.Object);
        _unitOfWork.Setup(uow => uow.ClientProjectRiskAreas).Returns(_clientProjectRiskAreaRepository.Object);

        _clientProjectRiskAreaBusiness = new ClientProjectRiskAreaBusiness(_logger.Object, _mapper.Object, _unitOfWork.Object);
    }

    [Fact]
    public async Task GetAsync_ReturnsMappedQueryable()
    {
        // Arrange
        var projectRiskAreas = new List<ProjectRiskArea>
        {
            new() { Id = 1, RowId = Guid.NewGuid(), Name = "Financial" }
        }.AsQueryable();

        var clientProjectRiskAreas = new List<ClientProjectRiskArea>
        {
            new() { ProjectRiskAreaId = 1, ClientProjectId = Guid.NewGuid() }
        }.AsQueryable();

        _projectRiskAreaRepository.Setup(r => r.GetAsync()).ReturnsAsync(projectRiskAreas);
        _clientProjectRiskAreaRepository.Setup(r => r.GetAsync()).ReturnsAsync(clientProjectRiskAreas);
        _mapper.Setup(m => m.Map<MetaDataViewModel>(It.IsAny<ProjectRiskArea>()))
               .Returns((ProjectRiskArea src) => new MetaDataViewModel { RowId = src.RowId, Name = src.Name });

        // Act
        var result = await _clientProjectRiskAreaBusiness.GetAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("Financial", result.First().Name);
    }

    [Fact]
    public async Task GetAsync_ExceptionThrown_PropagatesException()
    {
        // Arrange
        _projectRiskAreaRepository.Setup(r => r.GetAsync()).ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _clientProjectRiskAreaBusiness.GetAsync());
    }
}
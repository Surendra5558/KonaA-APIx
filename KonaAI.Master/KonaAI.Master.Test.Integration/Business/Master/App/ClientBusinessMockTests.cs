using Microsoft.Extensions.Logging;
using Moq;
using KonaAI.Master.Business.Master.App.Logic;
using KonaAI.Master.Model.Master.App.SaveModel;
using KonaAI.Master.Model.Master.App.ViewModel;
using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Test.Integration.Infrastructure;
using KonaAI.Master.Test.Integration.Infrastructure.Fixtures;
using AutoMapper;
using KonaAI.Master.Repository.Domain.Master.App;
using KonaAI.Master.Repository.DataAccess.Master.App.Interface;
using KonaAI.Master.Business.Master.App.Logic.Interface;
using KonaAI.Master.Repository.Common.Constants; // Added for DataModes
using KonaAI.Master.Repository.Common.Domain; // Added for Client
using KonaAI.Master.Repository.Common.Model; // Added for LicenseResult
using KonaAI.Master.Repository.DataAccess.Tenant.Client.Interface; // Added for IClientLicenseRepository
using KonaAI.Master.Repository.Domain.Tenant.Client; // Added for ClientLicense

namespace KonaAI.Master.Test.Integration.Business.Master.App;

/// <summary>
/// Integration tests for ClientBusiness using mocks and in-memory database.
/// Tests business logic with mocked dependencies.
/// </summary>
[Collection("InMemoryDatabaseCollection")]
public class ClientBusinessMockTests : TestBase, IDisposable
{
    private readonly Mock<ILogger<ClientBusiness>> _mockLogger;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IUserContextService> _mockUserContextService;
    private readonly Mock<ILicenseService> _mockLicenseService;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IClientRepository> _mockClientRepository;

    public ClientBusinessMockTests(InMemoryDatabaseFixture fixture) : base(fixture)
    {
        _mockLogger = CreateMockLogger<ClientBusiness>();
        _mockMapper = new Mock<IMapper>();
        _mockUserContextService = CreateMockUserContextService();
        _mockLicenseService = new Mock<ILicenseService>();
        _mockUnitOfWork = CreateMockUnitOfWork();
        _mockClientRepository = new Mock<IClientRepository>();

        // Setup default mock configurations
        SetupDefaultMocks();
    }

    private void SetupDefaultMocks()
    {
        // Setup UnitOfWork to return ClientRepository
        _mockUnitOfWork.SetupGet(u => u.Clients).Returns(_mockClientRepository.Object);

        // Setup LicenseService to return a valid result
        _mockLicenseService.Setup(l => l.EncryptLicense(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(new KonaAI.Master.Repository.Common.Model.LicenseResult
            {
                EncryptedLicense = "encrypted-license-data",
                EncryptedPrivateKey = "encrypted-private-key"
            });

        // Setup UserContextService to set domain defaults
        _mockUserContextService.Setup(u => u.SetDomainDefaults(It.IsAny<Client>(), It.IsAny<DataModes>()))
            .Callback<Client, DataModes>((client, mode) =>
            {
                var now = DateTime.UtcNow;
                switch (mode)
                {
                    case DataModes.Add:
                        client.CreatedOn = now;
                        client.CreatedBy = "testuser";
                        client.ModifiedOn = now;
                        client.ModifiedBy = "testuser";
                        break;
                    case DataModes.Edit:
                        client.ModifiedOn = now;
                        client.ModifiedBy = "testuser";
                        break;
                }
            });
    }

    [Fact]
    public async Task CreateAsync_WithValidModel_ReturnsSuccess()
    {
        // Arrange
        var createModel = new ClientCreateModel
        {
            Name = "Test Client",
            DisplayName = "Test Client Display",
            ClientCode = "TC001",
            LicenseStartDate = DateTime.UtcNow,
            LicenseEndDate = DateTime.UtcNow.AddMonths(6)
        };

        var clientEntity = new Client
        {
            Id = 1,
            RowId = Guid.NewGuid(),
            Name = "Test Client",
            DisplayName = "Test Client Display",
            ClientCode = "TC001",
            CreatedOn = DateTime.UtcNow,
            CreatedBy = "testuser",
            ModifiedOn = DateTime.UtcNow,
            ModifiedBy = "testuser",
            IsActive = true
        };

        var clientLicense = new ClientLicense
        {
            Id = 1,
            Name = "Test Client-License",
            Description = "License for client Test Client",
            ClientId = 1,
            LicenseKey = "encrypted-license-data",
            PrivateKey = "encrypted-private-key",
            StartDate = createModel.LicenseStartDate.Value,
            EndDate = createModel.LicenseEndDate.Value
        };

        _mockMapper.Setup(m => m.Map<Client>(createModel)).Returns(clientEntity);
        _mockClientRepository.Setup(r => r.AddAsync(clientEntity)).ReturnsAsync(clientEntity);
        _mockClientRepository.Setup(r => r.GetByIdAsync(clientEntity.Id)).ReturnsAsync(clientEntity);

        // Setup UnitOfWork ExecuteAsync to execute the delegate
        _mockUnitOfWork.Setup(u => u.ExecuteAsync(It.IsAny<Func<Task>>()))
            .Returns(async (Func<Task> action) => await action());

        _mockUnitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _mockUnitOfWork.Setup(u => u.ClientLicenses).Returns(new Mock<IClientLicenseRepository>().Object);

        var sut = new ClientBusiness(
            _mockLogger.Object,
            _mockMapper.Object,
            _mockUserContextService.Object,
            _mockLicenseService.Object,
            _mockUnitOfWork.Object);

        // Act
        var result = await sut.CreateAsync(createModel);

        // Assert
        Assert.Equal(1, result);
        _mockMapper.Verify(m => m.Map<Client>(createModel), Times.Once);
        _mockUserContextService.Verify(u => u.SetDomainDefaults(clientEntity, KonaAI.Master.Repository.Common.Constants.DataModes.Add), Times.Once);
        _mockClientRepository.Verify(r => r.AddAsync(clientEntity), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.AtLeastOnce);
    }

    [Fact]
    public async Task GetAsync_ReturnsQueryableResult()
    {
        // Arrange
        var clients = new List<Client>
        {
            new Client { Id = 1, Name = "Client 1", IsActive = true },
            new Client { Id = 2, Name = "Client 2", IsActive = true }
        }.AsQueryable();

        _mockUnitOfWork.Setup(u => u.Clients).Returns(_mockClientRepository.Object);
        _mockClientRepository.Setup(r => r.GetAsync()).ReturnsAsync(clients);

        var sut = new ClientBusiness(
            _mockLogger.Object,
            _mockMapper.Object,
            _mockUserContextService.Object,
            _mockLicenseService.Object,
            _mockUnitOfWork.Object);

        // Act
        var result = await sut.GetAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _mockClientRepository.Verify(r => r.GetAsync(), Times.Once);
    }

    [Fact]
    public async Task GetByRowIdAsync_WithValidId_ReturnsClient()
    {
        // Arrange
        var rowId = Guid.NewGuid();
        var client = new Client
        {
            Id = 1,
            RowId = rowId,
            Name = "Test Client",
            IsActive = true
        };

        _mockUnitOfWork.Setup(u => u.Clients).Returns(_mockClientRepository.Object);
        _mockClientRepository.Setup(r => r.GetByRowIdAsync(rowId)).ReturnsAsync(client);

        var clientViewModel = new ClientViewModel
        {
            RowId = client.RowId,
            Name = client.Name,
            DisplayName = client.DisplayName,
            ClientCode = client.ClientCode,
            Logo = client.Logo,
            IsActive = client.IsActive,
            CreatedOn = client.CreatedOn,
            CreatedBy = client.CreatedBy,
            ModifiedOn = client.ModifiedOn,
            ModifiedBy = client.ModifiedBy
        };

        _mockMapper.Setup(m => m.Map<ClientViewModel>(client)).Returns(clientViewModel);

        var sut = new ClientBusiness(
            _mockLogger.Object,
            _mockMapper.Object,
            _mockUserContextService.Object,
            _mockLicenseService.Object,
            _mockUnitOfWork.Object);

        // Act
        var result = await sut.GetByRowIdAsync(rowId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(rowId, result.RowId);
        Assert.Equal("Test Client", result.Name);
        _mockClientRepository.Verify(r => r.GetByRowIdAsync(rowId), Times.Once);
    }

    [Fact]
    public async Task GetByRowIdAsync_WithInvalidId_ThrowsKeyNotFoundException()
    {
        // Arrange
        var rowId = Guid.NewGuid();

        _mockUnitOfWork.Setup(u => u.Clients).Returns(_mockClientRepository.Object);
        _mockClientRepository.Setup(r => r.GetByRowIdAsync(rowId)).ReturnsAsync((Client?)null);

        var sut = new ClientBusiness(
            _mockLogger.Object,
            _mockMapper.Object,
            _mockUserContextService.Object,
            _mockLicenseService.Object,
            _mockUnitOfWork.Object);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => sut.GetByRowIdAsync(rowId));
        _mockClientRepository.Verify(r => r.GetByRowIdAsync(rowId), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WithValidModel_ReturnsSuccess()
    {
        // Arrange
        var rowId = Guid.NewGuid();
        var updateModel = new ClientUpdateModel
        {
            RowId = rowId,
            Name = "Updated Client",
            DisplayName = "Updated Client Display",
            ClientCode = "UC001"
        };

        var existingClient = new Client
        {
            Id = 1,
            RowId = rowId,
            Name = "Original Client",
            DisplayName = "Original Client Display",
            ClientCode = "OC001",
            IsActive = true
        };

        var updatedClient = new Client
        {
            Id = 1,
            RowId = rowId,
            Name = "Updated Client",
            DisplayName = "Updated Client Display",
            ClientCode = "UC001",
            IsActive = true
        };

        _mockUnitOfWork.Setup(u => u.Clients).Returns(_mockClientRepository.Object);
        _mockClientRepository.Setup(r => r.GetByRowIdAsync(rowId)).ReturnsAsync(existingClient);
        _mockMapper.Setup(m => m.Map(updateModel, existingClient)).Returns(updatedClient);
        _mockClientRepository.Setup(r => r.UpdateAsync(updatedClient)).ReturnsAsync(updatedClient);
        _mockUnitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var sut = new ClientBusiness(
            _mockLogger.Object,
            _mockMapper.Object,
            _mockUserContextService.Object,
            _mockLicenseService.Object,
            _mockUnitOfWork.Object);

        // Act
        var result = await sut.UpdateAsync(updateModel.RowId, updateModel);

        // Assert
        Assert.Equal(1, result);
        _mockClientRepository.Verify(r => r.GetByRowIdAsync(rowId), Times.Once);
        _mockMapper.Verify(m => m.Map(updateModel, existingClient), Times.Once);
        _mockUserContextService.Verify(u => u.SetDomainDefaults(existingClient, KonaAI.Master.Repository.Common.Constants.DataModes.Edit), Times.Once);
        _mockClientRepository.Verify(r => r.UpdateAsync(existingClient), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WithInvalidId_ThrowsKeyNotFoundException()
    {
        // Arrange
        var rowId = Guid.NewGuid();
        var updateModel = new ClientUpdateModel
        {
            RowId = rowId,
            Name = "Updated Client"
        };

        _mockUnitOfWork.Setup(u => u.Clients).Returns(_mockClientRepository.Object);
        _mockClientRepository.Setup(r => r.GetByRowIdAsync(rowId)).ReturnsAsync((Client?)null);

        var sut = new ClientBusiness(
            _mockLogger.Object,
            _mockMapper.Object,
            _mockUserContextService.Object,
            _mockLicenseService.Object,
            _mockUnitOfWork.Object);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => sut.UpdateAsync(updateModel.RowId, updateModel));
        _mockClientRepository.Verify(r => r.GetByRowIdAsync(rowId), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WithValidId_ReturnsSuccess()
    {
        // Arrange
        var rowId = Guid.NewGuid();

        _mockUnitOfWork.Setup(u => u.Clients).Returns(_mockClientRepository.Object);
        _mockClientRepository.Setup(r => r.DeleteAsync(rowId)).ReturnsAsync(new Client { Id = 1, RowId = rowId });
        _mockUnitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var sut = new ClientBusiness(
            _mockLogger.Object,
            _mockMapper.Object,
            _mockUserContextService.Object,
            _mockLicenseService.Object,
            _mockUnitOfWork.Object);

        // Act
        var result = await sut.DeleteAsync(rowId);

        // Assert
        Assert.Equal(1, result);
        _mockClientRepository.Verify(r => r.DeleteAsync(rowId), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WithInvalidId_ReturnsZero()
    {
        // Arrange
        var rowId = Guid.NewGuid();

        _mockUnitOfWork.Setup(u => u.Clients).Returns(_mockClientRepository.Object);
        _mockClientRepository.Setup(r => r.DeleteAsync(rowId)).ReturnsAsync((Client?)null);
        _mockUnitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(0);

        var sut = new ClientBusiness(
            _mockLogger.Object,
            _mockMapper.Object,
            _mockUserContextService.Object,
            _mockLicenseService.Object,
            _mockUnitOfWork.Object);

        // Act
        var result = await sut.DeleteAsync(rowId);

        // Assert
        Assert.Equal(0, result);
        _mockClientRepository.Verify(r => r.DeleteAsync(rowId), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    public new void Dispose()
    {
        base.Dispose();
    }
}

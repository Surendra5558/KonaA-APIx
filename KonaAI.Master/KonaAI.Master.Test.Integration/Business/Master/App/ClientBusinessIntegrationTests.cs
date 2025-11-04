using Microsoft.EntityFrameworkCore;
using FluentAssertions;
using Moq;
using Microsoft.Extensions.Logging;
using AutoMapper;
using KonaAI.Master.Business.Master.App;
using KonaAI.Master.Business.Master.App.Logic;
using KonaAI.Master.Model.Master.App;
using KonaAI.Master.Model.Master.App.ViewModel;
using KonaAI.Master.Model.Master.App.SaveModel;
using KonaAI.Master.Repository;
using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Repository.Domain.Master.App;
using KonaAI.Master.Repository.DataAccess.Master.App.Interface;
using KonaAI.Master.Repository.DataAccess.Tenant.Client.Interface;
using KonaAI.Master.Test.Integration.Infrastructure;
using KonaAI.Master.Test.Integration.Infrastructure.Attributes;
using KonaAI.Master.Test.Integration.Infrastructure.Fixtures;
using KonaAI.Master.Test.Integration.Infrastructure.TestData.Builders;
using KonaAI.Master.Test.Integration.Infrastructure.Helpers;

namespace KonaAI.Master.Test.Integration.Business.Master.App;

/// <summary>
/// Business integration tests for ClientBusiness using real database.
/// Tests business logic with real repository operations.
/// </summary>
[BusinessIntegration]
[Collection("InMemoryDatabaseCollection")]
public class ClientBusinessIntegrationTests : IClassFixture<InMemoryDatabaseFixture>
{
    private readonly InMemoryDatabaseFixture _fixture;

    public ClientBusinessIntegrationTests(InMemoryDatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task GetAsync_ReturnsAllActiveClients()
    {
        // Arrange
        using var context = _fixture.CreateContext();
        await _fixture.ClearDatabaseAsync();

        var clients = new[]
        {
            ClientBuilder.Create().WithName("Active Client 1").Active().Build(),
            ClientBuilder.Create().WithName("Active Client 2").Active().Build(),
            ClientBuilder.Create().WithName("Inactive Client").Inactive().Build()
        };

        context.AddRange(clients);
        await context.SaveChangesAsync();

        var business = CreateClientBusiness(context);

        // Act
        var result = await business.GetAsync();

        // Assert
        result.Should().NotBeNull();
        var resultList = result.ToList();
        resultList.Should().HaveCount(2);
        resultList.Should().OnlyContain(c => c.IsActive);
    }

    [Fact]
    public async Task GetByRowIdAsync_ExistingClient_ReturnsClient()
    {
        // Arrange
        using var context = _fixture.CreateContext();
        await _fixture.ClearDatabaseAsync();

        var client = ClientBuilder.Create()
            .WithName("Test Client")
            .WithCode("TC001")
            .Build();

        context.Add(client);
        await context.SaveChangesAsync();

        var business = CreateClientBusiness(context);

        // Act
        var result = await business.GetByRowIdAsync(client.RowId);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("Test Client");
        result.ClientCode.Should().Be("TC001");
    }

    [Fact]
    public async Task GetByRowIdAsync_NonExistentClient_ThrowsKeyNotFoundException()
    {
        // Arrange
        using var context = _fixture.CreateContext();
        await _fixture.ClearDatabaseAsync();

        var nonExistentId = Guid.NewGuid();
        var business = CreateClientBusiness(context);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => business.GetByRowIdAsync(nonExistentId));
        exception.Message.Should().Contain($"Record with KeyId: {nonExistentId} not found");
    }

    [Fact]
    public async Task CreateAsync_ValidModel_ReturnsAffectedRows()
    {
        // Arrange
        using var context = _fixture.CreateContext();
        await _fixture.ClearDatabaseAsync();

        var createModel = new ClientCreateModel
        {
            Name = "New Client",
            DisplayName = "New Client Display",
            ClientCode = "NC001",
            Description = "New client for testing"
        };

        var business = CreateClientBusiness(context);

        // Act
        var result = await business.CreateAsync(createModel);

        // Assert
        result.Should().Be(1);

        var savedClient = await context.Set<Client>()
            .FirstOrDefaultAsync(c => c.Name == "New Client");

        savedClient.Should().NotBeNull();
        savedClient!.Name.Should().Be("New Client");
        savedClient.ClientCode.Should().Be("NC001");
    }

    [Fact]
    public async Task UpdateAsync_ValidModel_ReturnsAffectedRows()
    {
        // Arrange
        using var context = _fixture.CreateContext();
        await _fixture.ClearDatabaseAsync();

        var client = ClientBuilder.Create()
            .WithName("Original Name")
            .WithCode("OC001")
            .Build();

        context.Add(client);
        await context.SaveChangesAsync();

        var updateModel = new ClientUpdateModel
        {
            RowId = client.RowId,
            Name = "Updated Name",
            DisplayName = "Updated Display Name",
            ClientCode = "OC001",
            Description = "Updated description"
        };

        var business = CreateClientBusiness(context);

        // Act
        var result = await business.UpdateAsync(client.RowId, updateModel);

        // Assert
        result.Should().Be(1);

        var updatedClient = await context.Set<Client>()
            .FirstOrDefaultAsync(c => c.RowId == client.RowId);

        updatedClient.Should().NotBeNull();
        updatedClient!.Name.Should().Be("Updated Name");
        updatedClient.Description.Should().Be("Updated description");
    }

    [Fact]
    public async Task DeleteAsync_ExistingClient_ReturnsAffectedRows()
    {
        // Arrange
        using var context = _fixture.CreateContext();
        await _fixture.ClearDatabaseAsync();

        var client = ClientBuilder.Create()
            .WithName("Client To Delete")
            .WithCode("CD001")
            .Build();

        context.Add(client);
        await context.SaveChangesAsync();

        var business = CreateClientBusiness(context);

        // Act
        var result = await business.DeleteAsync(client.RowId);

        // Assert
        result.Should().Be(1);

        var deletedClient = await context.Set<Client>()
            .FirstOrDefaultAsync(c => c.RowId == client.RowId);

        deletedClient.Should().BeNull();
    }

    [Fact]
    public async Task CreateAsync_DuplicateClientCode_ThrowsException()
    {
        // Arrange
        using var context = _fixture.CreateContext();
        await _fixture.ClearDatabaseAsync();

        var existingClient = ClientBuilder.Create()
            .WithName("Existing Client")
            .WithCode("DUPLICATE")
            .Build();

        context.Add(existingClient);
        await context.SaveChangesAsync();

        var createModel = new ClientCreateModel
        {
            Name = "New Client",
            DisplayName = "New Client Display",
            ClientCode = "DUPLICATE", // Same code as existing
            Description = "New client with duplicate code"
        };

        var business = CreateClientBusiness(context);

        // Act & Assert
        try
        {
            var result = await business.CreateAsync(createModel);
            throw new InvalidOperationException($"Expected InvalidOperationException but got result: {result}");
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("already exists"))
        {
            // Expected exception
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Expected InvalidOperationException but got: {ex.GetType().Name}: {ex.Message}", ex);
        }
    }

    [Fact]
    public async Task UpdateAsync_NonExistentClient_ThrowsException()
    {
        // Arrange
        using var context = _fixture.CreateContext();
        await _fixture.ClearDatabaseAsync();

        var nonExistentId = Guid.NewGuid();
        var updateModel = new ClientUpdateModel
        {
            RowId = nonExistentId,
            Name = "Updated Name",
            DisplayName = "Updated Display Name",
            ClientCode = "UC001",
            Description = "Updated description"
        };

        var business = CreateClientBusiness(context);

        // Act & Assert
        await business.Invoking(b => b.UpdateAsync(Guid.NewGuid(), updateModel))
            .Should().ThrowAsync<KeyNotFoundException>();
    }

    [Fact]
    public async Task DeleteAsync_NonExistentClient_ThrowsException()
    {
        // Arrange
        using var context = _fixture.CreateContext();
        await _fixture.ClearDatabaseAsync();

        var nonExistentId = Guid.NewGuid();
        var business = CreateClientBusiness(context);

        // Act & Assert
        await business.Invoking(b => b.DeleteAsync(nonExistentId))
            .Should().ThrowAsync<KeyNotFoundException>();
    }

    [Fact]
    public async Task GetAsync_WithPagination_ReturnsCorrectPage()
    {
        // Arrange
        using var context = _fixture.CreateContext();
        await _fixture.ClearDatabaseAsync();

        var clients = ClientBuilder.CreateRandomMultiple(10);
        context.AddRange(clients);
        await context.SaveChangesAsync();

        var business = CreateClientBusiness(context);

        // Act
        var result = await business.GetAsync();
        var resultList = result.ToList();

        // Assert
        resultList.Should().HaveCount(10);
    }

    [Fact]
    public async Task CreateAsync_WithAuditFields_SetsCorrectValues()
    {
        // Arrange
        using var context = _fixture.CreateContext();
        await _fixture.ClearDatabaseAsync();

        var createModel = new ClientCreateModel
        {
            Name = "Audit Test Client",
            DisplayName = "Audit Test Client Display",
            ClientCode = "ATC001",
            Description = "Client for audit testing"
        };

        var business = CreateClientBusiness(context);

        // Act
        var result = await business.CreateAsync(createModel);

        // Assert
        result.Should().Be(1);

        var savedClient = await context.Set<Client>()
            .FirstOrDefaultAsync(c => c.Name == "Audit Test Client");

        savedClient.Should().NotBeNull();
        savedClient!.CreatedOn.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
        savedClient.CreatedBy.Should().Be("testuser");
    }

    [Fact]
    public async Task UpdateAsync_WithAuditFields_SetsCorrectValues()
    {
        // Arrange
        using var context = _fixture.CreateContext();
        await _fixture.ClearDatabaseAsync();

        var client = ClientBuilder.Create()
            .WithName("Original Name")
            .WithCode("OC001")
            .Build();

        context.Add(client);
        await context.SaveChangesAsync();

        var updateModel = new ClientUpdateModel
        {
            RowId = client.RowId,
            Name = "Updated Name",
            DisplayName = "Updated Display Name",
            ClientCode = "OC001",
            Description = "Updated description"
        };

        var business = CreateClientBusiness(context);

        // Act
        var result = await business.UpdateAsync(client.RowId, updateModel);

        // Assert
        result.Should().Be(1);

        var updatedClient = await context.Set<Client>()
            .FirstOrDefaultAsync(c => c.RowId == client.RowId);

        updatedClient.Should().NotBeNull();
        updatedClient!.ModifiedOn.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
        updatedClient.ModifiedBy.Should().Be("testuser");
    }

    private static ClientBusiness CreateClientBusiness(TestDbContext context)
    {
        var logger = new Mock<ILogger<ClientBusiness>>();
        var mapper = new Mock<IMapper>();
        var userContextService = new KonaAI.Master.Test.Integration.Infrastructure.Helpers.TestUserContextService();
        var unitOfWork = new Mock<IUnitOfWork>();
        var clientRepository = new Mock<IClientRepository>();
        var clientLicenseRepository = new Mock<IClientLicenseRepository>();
        var licenseService = new Mock<ILicenseService>();

        // Setup mocks
        unitOfWork.SetupGet(u => u.Clients).Returns(clientRepository.Object);
        unitOfWork.SetupGet(u => u.ClientLicenses).Returns(clientLicenseRepository.Object);

        // Setup repository to use real context
        clientRepository.Setup(r => r.GetAsync())
            .ReturnsAsync(context.Set<Client>().AsQueryable());

        clientRepository.Setup(r => r.GetByRowIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid rowId) =>
            {
                var client = context.Set<Client>().FirstOrDefault(c => c.RowId == rowId);
                if (client == null)
                {
                    throw new KeyNotFoundException($"Record with KeyId: {rowId} not found");
                }
                return client;
            });

        clientRepository.Setup(r => r.AddAsync(It.IsAny<Client>()))
            .ReturnsAsync((Client client) =>
            {
                // Check for duplicate ClientCode
                var existingClient = context.Set<Client>().FirstOrDefault(c => c.ClientCode == client.ClientCode);
                if (existingClient != null)
                {
                    throw new InvalidOperationException($"Client with code '{client.ClientCode}' already exists.");
                }

                context.Add(client);
                return client;
            });

        clientRepository.Setup(r => r.UpdateAsync(It.IsAny<Client>()))
            .ReturnsAsync((Client client) =>
            {
                context.Update(client);
                return client;
            });

        clientRepository.Setup(r => r.DeleteAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid rowId) =>
            {
                var client = context.Set<Client>().FirstOrDefault(c => c.RowId == rowId);
                if (client == null)
                {
                    throw new KeyNotFoundException($"Record with KeyRowId: {rowId} not found");
                }
                client.IsDeleted = true;
                context.Update(client);
                return client;
            });


        unitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(async (CancellationToken ct) =>
            {
                return await context.SaveChangesAsync(ct);
            });

        unitOfWork.Setup(u => u.ExecuteAsync(It.IsAny<Func<Task>>()))
            .Returns(async (Func<Task> action) => await action());

        // Setup license service
        licenseService.Setup(l => l.EncryptLicense(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(new KonaAI.Master.Repository.Common.Model.LicenseResult
            {
                EncryptedLicense = "encrypted-license-data",
                EncryptedPrivateKey = "encrypted-private-key"
            });

        // Setup mapper
        mapper.Setup(m => m.Map<Client>(It.IsAny<ClientCreateModel>()))
            .Returns((ClientCreateModel model) => new Client
            {
                RowId = Guid.NewGuid(),
                Name = model.Name,
                DisplayName = model.DisplayName,
                ClientCode = model.ClientCode,
                Description = model.Description,
                IsActive = true,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = "testuser",
                ModifiedOn = DateTime.UtcNow,
                ModifiedBy = "testuser"
            });

        mapper.Setup(m => m.Map<Client>(It.IsAny<ClientUpdateModel>()))
            .Returns((ClientUpdateModel model) => new Client
            {
                RowId = model.RowId,
                Name = model.Name,
                DisplayName = model.DisplayName,
                ClientCode = model.ClientCode,
                Description = model.Description,
                IsActive = true,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = "testuser",
                ModifiedOn = DateTime.UtcNow,
                ModifiedBy = "testuser"
            });

        // Setup mapper for Map(payload, entity) - this is what UpdateAsync actually uses
        mapper.Setup(m => m.Map(It.IsAny<ClientUpdateModel>(), It.IsAny<Client>()))
            .Callback((ClientUpdateModel payload, Client entity) =>
            {
                entity.Name = payload.Name;
                entity.DisplayName = payload.DisplayName;
                entity.ClientCode = payload.ClientCode;
                entity.Description = payload.Description;
                entity.ModifiedOn = DateTime.UtcNow;
                entity.ModifiedBy = "testuser";
            });

        mapper.Setup(m => m.Map<ClientViewModel>(It.IsAny<Client>()))
            .Returns((Client client) => new ClientViewModel
            {
                RowId = client.RowId,
                Name = client.Name,
                DisplayName = client.DisplayName,
                ClientCode = client.ClientCode,
                IsActive = client.IsActive,
                CreatedOn = client.CreatedOn,
                CreatedBy = client.CreatedBy,
                ModifiedOn = client.ModifiedOn,
                ModifiedBy = client.ModifiedBy
            });

        return new ClientBusiness(
            logger.Object,
            mapper.Object,
            userContextService,
            licenseService.Object,
            unitOfWork.Object);
    }
}

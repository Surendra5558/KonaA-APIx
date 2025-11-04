using AutoMapper;
using KonaAI.Master.Business.Master.App.Logic;
using KonaAI.Master.Model.Master.App.SaveModel;
using KonaAI.Master.Model.Master.App.ViewModel;
using KonaAI.Master.Repository.Common.Constants;
using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Repository.DataAccess.Master.App.Interface;
using KonaAI.Master.Repository.DataAccess.Tenant.Client.Interface;
using KonaAI.Master.Repository.Domain.Master.App;
using KonaAI.Master.Repository.Domain.Tenant.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace KonaAI.Master.Test.Unit.Business.Master;

/// <summary>
/// Unit tests for <see cref="KonaAI.Master.Business.Master.App.Logic.ClientBusiness"/>.
/// Covers:
/// - Reads: list mapping and detail retrieval, not-found and error propagation
/// - Create: mapping, domain defaults, add, save; DbUpdate and mapping exceptions propagate
/// - Update: load, map, defaults, update, save; not-found and DbUpdate exceptions
/// - Delete: delete and save; DbUpdate exceptions propagate
/// </summary>
public class ClientBusinessTests
{
    private readonly Mock<ILogger<ClientBusiness>> _logger = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly Mock<IUserContextService> _userContext = new();
    private readonly Mock<ILicenseService> _licenseMock = new();
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly Mock<IClientRepository> _clientRepo = new();
    private readonly Mock<IClientLicenseRepository> _clientLicenseRepo = new();

    public ClientBusinessTests()
    {
        // Wire `IUnitOfWork.Clients` to our client repository mock.
        // This mirrors how the business code resolves repositories via the UoW.
        _uow.SetupGet(u => u.Clients).Returns(_clientRepo.Object);
        _uow.SetupGet(u => u.ClientLicenses).Returns(_clientLicenseRepo.Object);
    }

    // Creates the system under test with all dependencies mocked.
    private ClientBusiness CreateSut() =>
        new(_logger.Object, _mapper.Object, _userContext.Object, _licenseMock.Object, _uow.Object);

    #region GetAsync

    [Fact]
    public async Task GetAsync_ReturnsMappedQueryable()
    {
        // Arrange: repository returns two entities (one active, one inactive); business filters active only.
        var clients = new List<Client>
            {
                new() { RowId = Guid.NewGuid(), Name = "c1", DisplayName = "C1", ClientCode = "C001", IsActive = true },
                new() { RowId = Guid.NewGuid(), Name = "c2", DisplayName = "C2", ClientCode = "C002", IsActive = false }
            }.AsQueryable();

        _clientRepo
            .Setup(r => r.GetAsync())
            .ReturnsAsync(clients);

        _mapper
            .Setup(m => m.Map<ClientViewModel>(It.IsAny<Client>()))
            .Returns((Client c) => new ClientViewModel
            {
                RowId = c.RowId,
                Name = c.Name,
                DisplayName = c.DisplayName,
                ClientCode = c.ClientCode,
                IsActive = c.IsActive
            });

        var sut = CreateSut();

        // Act: enumerate the returned IQueryable to force execution and mapping.
        var resultQueryable = await sut.GetAsync();
        var result = resultQueryable.ToList();

        // Assert: only active records are returned; mapper invoked for active count
        Assert.Single(result);
        Assert.Contains(result, r => r.Name == "c1");
        Assert.DoesNotContain(result, r => r.Name == "c2");
        _clientRepo.Verify(r => r.GetAsync(), Times.Once);
        _mapper.Verify(m => m.Map<ClientViewModel>(It.IsAny<Client>()), Times.Exactly(1));
    }

    [Fact]
    public async Task GetAsync_WhenRepoThrows_Propagates()
    {
        // Arrange: surface repository failure. Business should not swallow this error.
        _clientRepo
            .Setup(r => r.GetAsync())
            .ThrowsAsync(new InvalidOperationException("boom"));

        var sut = CreateSut();

        // Act & Assert: exception bubbles up unchanged.
        await Assert.ThrowsAsync<InvalidOperationException>(() => sut.GetAsync());
    }

    #endregion GetAsync

    #region GetByRowIdAsync

    [Fact]
    public async Task GetByRowIdAsync_Found_ReturnsMapped()
    {
        // Arrange: entity exists; mapping projects to view model.
        var id = Guid.NewGuid();
        var entity = new Client { RowId = id, Name = "client", DisplayName = "Client", ClientCode = "CC1", IsActive = true };

        _clientRepo.Setup(r => r.GetByRowIdAsync(id)).ReturnsAsync(entity);
        _mapper.Setup(m => m.Map<ClientViewModel>(entity))
            .Returns(new ClientViewModel { RowId = id, Name = "client" });

        var sut = CreateSut();

        var result = await sut.GetByRowIdAsync(id);

        // Assert: selected entity returned and mapped.
        Assert.Equal(id, result.RowId);
        Assert.Equal("client", result.Name);
        _clientRepo.Verify(r => r.GetByRowIdAsync(id), Times.Once);
    }

    [Fact]
    public async Task GetByRowIdAsync_NotFound_ThrowsKeyNotFound()
    {
        // Arrange: repository returns null when not found.
        var id = Guid.NewGuid();
        _clientRepo.Setup(r => r.GetByRowIdAsync(id)).ReturnsAsync((Client?)null);

        var sut = CreateSut();

        // Act & Assert: business signals missing resource via KeyNotFoundException.
        await Assert.ThrowsAsync<KeyNotFoundException>(() => sut.GetByRowIdAsync(id));
    }

    [Fact]
    public async Task GetByRowIdAsync_RepoThrows_Propagates()
    {
        // Arrange: propagate unexpected repository errors.
        var id = Guid.NewGuid();
        _clientRepo.Setup(r => r.GetByRowIdAsync(id)).ThrowsAsync(new Exception("err"));

        var sut = CreateSut();

        // Act & Assert: exception flows upward without being wrapped.
        await Assert.ThrowsAsync<Exception>(() => sut.GetByRowIdAsync(id));
    }

    #endregion GetByRowIdAsync

    #region CreateAsync

    [Fact]
    public async Task CreateAsync_Success_ReturnsAffectedRows()
    {
        // Arrange: valid model maps to entity; add succeeds; SaveChanges returns 1.
        var model = new ClientCreateModel
        {
            Name = "n1",
            DisplayName = "N1",
            ClientCode = "C100"
        };

        var mappedEntity = new Client { RowId = Guid.NewGuid(), Name = "n1" };

        _mapper.Setup(m => m.Map<Client>(model)).Returns(mappedEntity);
        _clientRepo.Setup(r => r.AddAsync(mappedEntity)).ReturnsAsync(mappedEntity);
        _uow.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _uow.Setup(u => u.ExecuteAsync(It.IsAny<Func<Task>>())).Returns<Func<Task>>(f => f());

        // After first save, GetByIdAsync should return the persisted client with RowId
        _clientRepo.Setup(r => r.GetByIdAsync(It.IsAny<long>()))
                   .ReturnsAsync(new Client { Id = 1, RowId = mappedEntity.RowId, Name = mappedEntity.Name });

        // License encryption
        _licenseMock.Setup(l => l.EncryptLicense(It.IsAny<string>(), It.IsAny<string>()))
                    .Returns((string _, string __) => new KonaAI.Master.Repository.Common.Model.LicenseResult
                    {
                        EncryptedLicense = "L",
                        EncryptedPrivateKey = "K"
                    });

        // Add client license
        _clientLicenseRepo.Setup(r => r.AddAsync(It.IsAny<ClientLicense>()))
                          .ReturnsAsync((ClientLicense cl) => cl);
        _uow.Setup(u => u.ExecuteAsync(It.IsAny<Func<Task>>())).Returns<Func<Task>>(f => f());

        var sut = CreateSut();

        var result = await sut.CreateAsync(model);

        // Assert: domain defaults applied, repo add called, and SaveChanges returns count.
        Assert.Equal(1, result);
        _mapper.Verify(m => m.Map<Client>(model), Times.Once);
        _userContext.Verify(u => u.SetDomainDefaults(mappedEntity, DataModes.Add), Times.Once);
        _clientRepo.Verify(r => r.AddAsync(mappedEntity), Times.Once);
        _uow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Exactly(2));
    }

    [Fact]
    public async Task CreateAsync_SaveChangesThrowsDbUpdateException_Propagates()
    {
        // Arrange: simulate database write failure during SaveChanges.
        var model = new ClientCreateModel { Name = "n2", DisplayName = "N2", ClientCode = "C200" };
        var mappedEntity = new Client { RowId = Guid.NewGuid(), Name = "n2" };

        _mapper.Setup(m => m.Map<Client>(model)).Returns(mappedEntity);
        _clientRepo.Setup(r => r.AddAsync(mappedEntity)).ReturnsAsync(mappedEntity);
        _uow.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new DbUpdateException("db failure"));
        _uow.Setup(u => u.ExecuteAsync(It.IsAny<Func<Task>>())).Returns<Func<Task>>(f => f());

        var sut = CreateSut();

        // Act & Assert: DbUpdateException is rethrown.
        await Assert.ThrowsAsync<DbUpdateException>(() => sut.CreateAsync(model));
    }

    [Fact]
    public async Task CreateAsync_GenericException_Propagates()
    {
        // Arrange: mapping error before hitting the repository.
        var model = new ClientCreateModel { Name = "n3", DisplayName = "N3", ClientCode = "C300" };
        _mapper.Setup(m => m.Map<Client>(model)).Throws(new Exception("map fail"));
        _uow.Setup(u => u.ExecuteAsync(It.IsAny<Func<Task>>())).Returns<Func<Task>>(f => f());

        var sut = CreateSut();

        // Act & Assert: unexpected exceptions bubble up.
        await Assert.ThrowsAsync<Exception>(() => sut.CreateAsync(model));
    }

    #endregion CreateAsync

    #region UpdateAsync

    [Fact]
    public async Task UpdateAsync_Found_UpdatesAndReturnsAffectedRows()
    {
        // Arrange: entity exists; apply update; SaveChanges returns affected count.
        var id = Guid.NewGuid();
        var existing = new Client { RowId = id, Name = "old" };
        var updateModel = new ClientUpdateModel { Description = "desc" };
        var mappedEntity = new Client { RowId = id, Name = "old", Description = "desc" };

        _clientRepo.Setup(r => r.GetByRowIdAsync(id)).ReturnsAsync(existing);
        _mapper.Setup(m => m.Map<Client>(updateModel)).Returns(mappedEntity);
        _clientRepo.Setup(r => r.UpdateAsync(It.IsAny<Client>())).ReturnsAsync((Client c) => c);
        _uow.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(2);

        var sut = CreateSut();

        var result = await sut.UpdateAsync(id, updateModel);

        // Assert: domain defaults applied for edit; update invoked; affected rows returned.
        Assert.Equal(2, result);
        _userContext.Verify(u => u.SetDomainDefaults(It.IsAny<Client>(), DataModes.Edit), Times.Once);
        _clientRepo.Verify(r => r.UpdateAsync(It.Is<Client>(c => c.RowId == id)), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_NotFound_ThrowsKeyNotFound()
    {
        // Arrange: update attempted for missing entity.
        var id = Guid.NewGuid();
        var updateModel = new ClientUpdateModel();

        _clientRepo.Setup(r => r.GetByRowIdAsync(id)).ReturnsAsync((Client?)null);

        var sut = CreateSut();

        // Act & Assert: business indicates missing resource via KeyNotFoundException.
        await Assert.ThrowsAsync<KeyNotFoundException>(() => sut.UpdateAsync(id, updateModel));
    }

    [Fact]
    public async Task UpdateAsync_SaveChangesThrowsDbUpdateException_Propagates()
    {
        // Arrange: simulate database failure on update save.
        var id = Guid.NewGuid();
        var existing = new Client { RowId = id };
        var updateModel = new ClientUpdateModel();
        var mappedEntity = new Client { RowId = id };

        _clientRepo.Setup(r => r.GetByRowIdAsync(id)).ReturnsAsync(existing);
        _mapper.Setup(m => m.Map<Client>(updateModel)).Returns(mappedEntity);
        _clientRepo.Setup(r => r.UpdateAsync(mappedEntity)).ReturnsAsync(mappedEntity);
        _uow.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new DbUpdateException("db fail"));

        var sut = CreateSut();

        // Act & Assert: DbUpdateException is rethrown.
        await Assert.ThrowsAsync<DbUpdateException>(() => sut.UpdateAsync(id, updateModel));
    }

    #endregion UpdateAsync

    #region DeleteAsync

    [Fact]
    public async Task DeleteAsync_Success_ReturnsAffectedRows()
    {
        // Arrange: delete invoked; SaveChanges returns 1.
        var id = Guid.NewGuid();

        _ = _clientRepo.Setup(r => r.DeleteAsync(id)).ReturnsAsync((Client?)null);
        _uow.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var sut = CreateSut();

        var result = await sut.DeleteAsync(id);

        // Assert: repository delete called and SaveChanges persisted once.
        Assert.Equal(1, result);
        _clientRepo.Verify(r => r.DeleteAsync(id), Times.Once);
        _uow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_SaveChangesThrowsDbUpdateException_Propagates()
    {
        // Arrange: simulate database failure on delete save.
        var id = Guid.NewGuid();
        _clientRepo.Setup(r => r.DeleteAsync(id)).ReturnsAsync((Client?)null);
        _uow.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new DbUpdateException("delete fail"));

        var sut = CreateSut();

        // Act & Assert: DbUpdateException is rethrown.
        await Assert.ThrowsAsync<DbUpdateException>(() => sut.DeleteAsync(id));
    }

    #endregion DeleteAsync
}
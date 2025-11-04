using AutoMapper;
using KonaAI.Master.Business.Tenant.Client.Logic;
using KonaAI.Master.Model.Tenant.Client.SaveModel;
using KonaAI.Master.Model.Tenant.Client.ViewModel;
using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Repository.Common.Model;
using KonaAI.Master.Repository.DataAccess.Tenant.Client.Interface;
using KonaAI.Master.Repository.Domain.Tenant.Client;
using Microsoft.Extensions.Logging;
using Moq;

namespace KonaAI.Master.Test.Unit.Business.Tenant.Client;

/// <summary>
/// Unit tests for <see cref="KonaAI.Master.Business.Tenant.Client.Logic.ClientLicenseBusiness"/>.
/// </summary>
public class ClientLicenseBusinessTests
{
    private readonly Mock<ILogger<ClientLicenseBusiness>> _logger;
    private readonly Mock<IMapper> _mapper;
    private readonly Mock<IUnitOfWork> _uow;
    private readonly Mock<IUserContextService> _userContextService;
    private readonly Mock<IClientLicenseRepository> _clientLicenses;
    private readonly Mock<ILicenseService> _licenseService;

    public ClientLicenseBusinessTests()
    {
        _logger = new Mock<ILogger<ClientLicenseBusiness>>();
        _mapper = new Mock<IMapper>();
        _uow = new Mock<IUnitOfWork>();
        _userContextService = new Mock<IUserContextService>();
        _clientLicenses = new Mock<IClientLicenseRepository>();
        _licenseService = new Mock<ILicenseService>();

        _uow.SetupGet(x => x.ClientLicenses).Returns(_clientLicenses.Object);
    }

    private ClientLicenseBusiness CreateSut() => new(_logger.Object, _licenseService.Object, _userContextService.Object, _mapper.Object, _uow.Object);

    #region GetAsync

    [Fact]
    public async Task GetAsync_ReturnsMappedQueryable()
    {
        // Arrange: repository returns queryable; mapper maps to view model
        var entities = new List<ClientLicense>
        {
            new() { RowId = Guid.NewGuid(), LicenseKey = "key1", StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddDays(30) },
            new() { RowId = Guid.NewGuid(), LicenseKey = "key2", StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddDays(60) }
        }.AsQueryable();

        _clientLicenses.Setup(r => r.GetAsync()).ReturnsAsync(entities);
        _mapper.Setup(m => m.Map<ClientLicenseViewModel>(It.IsAny<ClientLicense>()))
               .Returns((ClientLicense cl) => new ClientLicenseViewModel
               {
                   RowId = cl.RowId,
                   LicenseKey = cl.LicenseKey,
                   StartDate = cl.StartDate,
                   EndDate = cl.EndDate
               });

        var sut = CreateSut();

        // Act
        var result = await sut.GetAsync();

        // Assert: mapper invoked for each entity; queryable returned
        Assert.NotNull(result);
        var list = result.ToList();
        Assert.Equal(2, list.Count);
        Assert.Contains(list, x => x.LicenseKey == "key1");
        Assert.Contains(list, x => x.LicenseKey == "key2");
    }

    [Fact]
    public async Task GetAsync_WhenRepositoryThrows_PropagatesException()
    {
        // Arrange: repository throws exception
        _clientLicenses.Setup(r => r.GetAsync()).Throws(new Exception("Database error"));

        var sut = CreateSut();

        // Act & Assert: exception is propagated
        await Assert.ThrowsAsync<Exception>(() => sut.GetAsync());
    }

    #endregion GetAsync

    #region GetByRowIdAsync

    [Fact]
    public async Task GetByRowIdAsync_Found_ReturnsMappedViewModel()
    {
        // Arrange: entity exists; mapper returns view model
        var id = Guid.NewGuid();
        var entity = new ClientLicense { RowId = id, LicenseKey = "test", StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddDays(30) };

        _clientLicenses.Setup(r => r.GetByRowIdAsync(id)).ReturnsAsync(entity);
        _mapper.Setup(m => m.Map<ClientLicenseViewModel>(entity))
               .Returns(new ClientLicenseViewModel
               {
                   RowId = entity.RowId,
                   LicenseKey = entity.LicenseKey,
                   StartDate = entity.StartDate,
                   EndDate = entity.EndDate
               });

        var sut = CreateSut();

        // Act
        var result = await sut.GetByRowIdAsync(id);

        // Assert: mapper invoked with entity; view model returned
        Assert.NotNull(result);
        Assert.Equal(id, result.RowId);
        Assert.Equal("test", result.LicenseKey);
    }

    [Fact]
    public async Task GetByRowIdAsync_NotFound_ThrowsKeyNotFound()
    {
        // Arrange: entity not found
        var id = Guid.NewGuid();
        _clientLicenses.Setup(r => r.GetByRowIdAsync(id)).ReturnsAsync((ClientLicense?)null);

        var sut = CreateSut();

        // Act & Assert: KeyNotFoundException thrown
        await Assert.ThrowsAsync<KeyNotFoundException>(() => sut.GetByRowIdAsync(id));
    }

    #endregion GetByRowIdAsync

    #region UpdateAsync

    [Fact]
    public async Task UpdateAsync_NotFound_ThrowsKeyNotFound()
    {
        // Arrange: update attempted for missing entity
        var id = Guid.NewGuid();
        var updateModel = new ClientLicenseUpdateModel();

        _clientLicenses.Setup(r => r.GetByRowIdAsync(id)).ReturnsAsync((ClientLicense?)null);

        var sut = CreateSut();

        // Act & Assert: business indicates missing resource via KeyNotFoundException
        await Assert.ThrowsAsync<KeyNotFoundException>(() => sut.UpdateAsync(id, updateModel));
    }

    [Fact]
    public async Task UpdateAsync_WhenRepositoryThrows_PropagatesException()
    {
        // Arrange: entity exists but repository throws
        var id = Guid.NewGuid();
        var existing = new ClientLicense { RowId = id };
        _clientLicenses.Setup(r => r.GetByRowIdAsync(id)).ReturnsAsync(existing);
        // Ensure encryption returns a non-null object to avoid NRE before UpdateAsync
        _licenseService.Setup(l => l.EncryptLicense(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(new LicenseResult { EncryptedLicense = "enc", EncryptedPrivateKey = "priv" });
        _clientLicenses.Setup(r => r.UpdateAsync(existing)).ThrowsAsync(new Exception("Database error"));

        var sut = CreateSut();

        // Act & Assert: exception is propagated
        await Assert.ThrowsAsync<Exception>(() => sut.UpdateAsync(id, new ClientLicenseUpdateModel()));
    }

    #endregion UpdateAsync

    #region DeleteAsync

    [Fact]
    public async Task DeleteAsync_NotFound_ThrowsKeyNotFound()
    {
        // Arrange: delete attempted for missing entity
        var id = Guid.NewGuid();

        _clientLicenses.Setup(r => r.GetByRowIdAsync(id)).ReturnsAsync((ClientLicense?)null);
        // Execute transaction by invoking delegate
        _uow.Setup(u => u.ExecuteAsync(It.IsAny<Func<Task>>()))
            .Returns((Func<Task> f) => f());

        var sut = CreateSut();

        // Act & Assert: business indicates missing resource via KeyNotFoundException
        await Assert.ThrowsAsync<KeyNotFoundException>(() => sut.DeleteAsync(id));
    }

    [Fact]
    public async Task DeleteAsync_WhenRepositoryThrows_PropagatesException()
    {
        // Arrange: entity exists but repository throws
        var id = Guid.NewGuid();
        var entity = new ClientLicense { RowId = id };
        _clientLicenses.Setup(r => r.GetByRowIdAsync(id)).ReturnsAsync(entity);
        _clientLicenses.Setup(r => r.DeleteAsync(id)).ThrowsAsync(new Exception("Database error"));
        // Execute transaction by invoking delegate
        _uow.Setup(u => u.ExecuteAsync(It.IsAny<Func<Task>>()))
            .Returns((Func<Task> f) => f());

        var sut = CreateSut();

        // Act & Assert: exception is propagated
        await Assert.ThrowsAsync<Exception>(() => sut.DeleteAsync(id));
    }

    #endregion DeleteAsync
}
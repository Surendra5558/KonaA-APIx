using AutoMapper;
using KonaAI.Master.Business.Tenant.UserMetaData.Logic;
using KonaAI.Master.Model.Common;
using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Repository.Domain.Master.MetaData;
using KonaAI.Master.Repository.Domain.Tenant.ClientUserMetaData;
using Microsoft.Extensions.Logging;
using Moq;

namespace KonaAI.Master.Test.Unit.Business.Tenant.UserMetaData;

public class ClientProjectCountryBusinessTests
{
    private readonly Mock<ILogger<ClientProjectCountryBusiness>> _logger = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();

    // Mock repositories
    private readonly Mock<KonaAI.Master.Repository.DataAccess.Master.MetaData.Interface.ICountryRepository> _countryRepository = new();

    private readonly Mock<KonaAI.Master.Repository.DataAccess.Tenant.ClientUserMetaData.Interface.IClientProjectCountryRepository> _clientProjectCountryRepository = new();

    private readonly ClientProjectCountryBusiness _clientProjectCountryBusiness;

    public ClientProjectCountryBusinessTests()
    {
        _unitOfWork.Setup(uow => uow.Countries).Returns(_countryRepository.Object);
        _unitOfWork.Setup(uow => uow.ClientProjectCountries).Returns(_clientProjectCountryRepository.Object);

        _clientProjectCountryBusiness = new ClientProjectCountryBusiness(_logger.Object, _mapper.Object, _unitOfWork.Object);
    }

    [Fact]
    public async Task GetAsync_ReturnsMappedQueryable()
    {
        // Arrange
        var countries = new List<Country>
        {
            new() { Id = 1, RowId = Guid.NewGuid(), Name = "USA" }
        }.AsQueryable();

        var clientProjectCountries = new List<ClientProjectCountry>
        {
            new() { CountryId = 1, ClientProjectId = Guid.NewGuid() }
        }.AsQueryable();

        _countryRepository.Setup(r => r.GetAsync()).ReturnsAsync(countries);
        _clientProjectCountryRepository.Setup(r => r.GetAsync()).ReturnsAsync(clientProjectCountries);
        _mapper.Setup(m => m.Map<MetaDataViewModel>(It.IsAny<Country>()))
               .Returns((Country src) => new MetaDataViewModel { RowId = src.RowId, Name = src.Name });

        // Act
        var result = await _clientProjectCountryBusiness.GetAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("USA", result.First().Name);
    }

    [Fact]
    public async Task GetAsync_ExceptionThrown_PropagatesException()
    {
        // Arrange
        _countryRepository.Setup(r => r.GetAsync()).ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _clientProjectCountryBusiness.GetAsync());
    }
}
using AutoMapper;
using KonaAI.Master.Business.Master.UserMetaData.Logic;
using KonaAI.Master.Model.Common;
using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Repository.DataAccess.Master.MetaData.Interface;
using KonaAI.Master.Repository.Domain.Master.MetaData;
using Microsoft.Extensions.Logging;
using Moq;

namespace KonaAI.Master.Test.Unit.Business.Master;

/// <summary>
/// Unit tests for <see cref="KonaAI.Master.Business.Master.UserMetaData.Logic.CountryBusiness"/>.
/// Validates:
/// - GetAsync maps repository <see cref="KonaAI.Master.Repository.Domain.Master.MetaData.Country"/> to <see cref="KonaAI.Master.Model.Common.MetaDataViewModel"/>
/// - Repository exceptions are propagated and not swallowed
/// </summary>
public class CountryBusinessTests
{
    private readonly Mock<ILogger<CountryBusiness>> _logger = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly Mock<ICountryRepository> _countries = new();

    public CountryBusinessTests()
    {
        // Wire UnitOfWork.Countries to our country repository mock to mirror production resolution
        _uow.SetupGet(x => x.Countries).Returns(_countries.Object);
    }

    private CountryBusiness CreateSut() => new(_logger.Object, _mapper.Object, _uow.Object);

    [Fact]
    public async Task GetAsync_ReturnsMappedQueryable()
    {
        // Arrange: repository returns two countries; mapper projects each to MetaDataViewModel
        var data = new List<Country>
        {
            new() { RowId = Guid.NewGuid(), Name = "USA" },
            new() { RowId = Guid.NewGuid(), Name = "India" }
        }.AsQueryable();

        _countries.Setup(r => r.GetAsync()).ReturnsAsync(data);
        _mapper.Setup(m => m.Map<MetaDataViewModel>(It.IsAny<Country>()))
               .Returns((Country c) => new MetaDataViewModel { RowId = c.RowId, Name = c.Name });

        var sut = CreateSut();

        // Act: enumerate to force LINQ execution and mapping
        var queryable = await sut.GetAsync();
        var list = queryable.ToList();

        // Assert: verify content and interaction counts
        Assert.Equal(2, list.Count);
        Assert.Contains(list, x => x.Name == "USA");
        _countries.Verify(r => r.GetAsync(), Times.Once);
        _mapper.Verify(m => m.Map<MetaDataViewModel>(It.IsAny<Country>()), Times.Exactly(2));
    }

    [Fact]
    public async Task GetAsync_WhenRepoThrows_Propagates()
    {
        // Arrange: bubble up repository failures without swallowing
        _countries.Setup(r => r.GetAsync()).ThrowsAsync(new InvalidOperationException("boom"));
        var sut = CreateSut();

        // Act + Assert: exception is propagated
        await Assert.ThrowsAsync<InvalidOperationException>(() => sut.GetAsync());
    }

    [Fact]
    public async Task GetAsync_WhenMapperThrows_Propagates()
    {
        // Arrange: mapping failure during projection
        var data = new List<Country>
        {
            new() { RowId = Guid.NewGuid(), Name = "USA" }
        }.AsQueryable();

        _countries.Setup(r => r.GetAsync()).ReturnsAsync(data);
        _mapper.Setup(m => m.Map<MetaDataViewModel>(It.IsAny<Country>()))
               .Throws(new AutoMapperMappingException("Mapping failed"));

        var sut = CreateSut();

        // Act + Assert: mapping exception is propagated
        var queryable = await sut.GetAsync();
        Assert.Throws<AutoMapperMappingException>(() => queryable.ToList());
    }

    [Fact]
    public async Task GetAsync_WithEmptyData_ReturnsEmptyQueryable()
    {
        // Arrange: repository returns no data
        _countries.Setup(r => r.GetAsync()).ReturnsAsync(Enumerable.Empty<Country>().AsQueryable());
        var sut = CreateSut();

        // Act
        var queryable = await sut.GetAsync();
        var list = queryable.ToList();

        // Assert: empty list returned, no mapper invocations
        Assert.Empty(list);
        _countries.Verify(r => r.GetAsync(), Times.Once);
        _mapper.Verify(m => m.Map<MetaDataViewModel>(It.IsAny<Country>()), Times.Never);
    }

    [Fact]
    public async Task GetAsync_WithNullFields_MapsSuccessfully()
    {
        // Arrange: repository returns country with null optional fields
        var data = new List<Country>
        {
            new() { RowId = Guid.NewGuid(), Name = "Country", Description = null }
        }.AsQueryable();

        _countries.Setup(r => r.GetAsync()).ReturnsAsync(data);
        _mapper.Setup(m => m.Map<MetaDataViewModel>(It.IsAny<Country>()))
               .Returns((Country c) => new MetaDataViewModel { RowId = c.RowId, Name = c.Name, Description = c.Description });

        var sut = CreateSut();

        // Act
        var queryable = await sut.GetAsync();
        var list = queryable.ToList();

        // Assert: null fields handled correctly
        Assert.Single(list);
        Assert.Null(list[0].Description);
        _mapper.Verify(m => m.Map<MetaDataViewModel>(It.IsAny<Country>()), Times.Once);
    }
}
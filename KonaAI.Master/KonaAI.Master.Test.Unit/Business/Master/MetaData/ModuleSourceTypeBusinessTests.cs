using AutoMapper;
using KonaAI.Master.Business.Master.MetaData.Logic;
using KonaAI.Master.Model.Common;
using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Repository.DataAccess.Master.MetaData.Interface;
using KonaAI.Master.Repository.Domain.Master.MetaData;
using Microsoft.Extensions.Logging;
using Moq;

namespace KonaAI.Master.Test.Unit.Business.Master.MetaData;

/// <summary>
/// Unit tests for <see cref="KonaAI.Master.Business.Master.MetaData.Logic.ModuleSourceTypeBusiness"/>.
/// Validates:
/// - GetAsync maps repository <see cref="KonaAI.Master.Repository.Domain.Master.MetaData.ModuleSourceType"/> to <see cref="KonaAI.Master.Model.Common.MetaDataViewModel"/>
/// - Repository exceptions are propagated and not swallowed
/// </summary>
public class ModuleSourceTypeBusinessTests
{
    private readonly Mock<ILogger<ModuleSourceTypeBusiness>> _logger = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly Mock<IModuleSourceTypeRepository> _moduleSourceTypes = new();

    public ModuleSourceTypeBusinessTests()
    {
        // Wire UnitOfWork.ModuleSourceTypes to our module source type repository mock to mirror production resolution
        _uow.SetupGet(x => x.ModuleSourceTypes).Returns(_moduleSourceTypes.Object);
    }

    private ModuleSourceTypeBusiness CreateSut() => new(_logger.Object, _mapper.Object, _uow.Object);

    [Fact]
    public async Task GetAsync_ReturnsMappedQueryable()
    {
        // Arrange: repository returns two module source types; mapper projects each to MetaDataViewModel
        var data = new List<ModuleSourceType>
        {
            new() { RowId = Guid.NewGuid(), Name = "API" },
            new() { RowId = Guid.NewGuid(), Name = "Database" }
        }.AsQueryable();

        _moduleSourceTypes.Setup(r => r.GetAsync()).ReturnsAsync(data);
        _mapper.Setup(m => m.Map<MetaDataViewModel>(It.IsAny<ModuleSourceType>()))
               .Returns((ModuleSourceType mst) => new MetaDataViewModel { RowId = mst.RowId, Name = mst.Name });

        var sut = CreateSut();

        // Act: enumerate to force LINQ execution and mapping
        var queryable = await sut.GetAsync();
        var list = queryable.ToList();

        // Assert: verify content and interaction counts
        Assert.Equal(2, list.Count);
        Assert.Contains(list, x => x.Name == "API");
        Assert.Contains(list, x => x.Name == "Database");
        _moduleSourceTypes.Verify(r => r.GetAsync(), Times.Once);
        _mapper.Verify(m => m.Map<MetaDataViewModel>(It.IsAny<ModuleSourceType>()), Times.Exactly(2));
    }

    [Fact]
    public async Task GetAsync_WhenRepoThrows_Propagates()
    {
        // Arrange: bubble up repository failures without swallowing
        _moduleSourceTypes.Setup(r => r.GetAsync()).ThrowsAsync(new InvalidOperationException("boom"));
        var sut = CreateSut();

        // Act + Assert: exception is propagated
        await Assert.ThrowsAsync<InvalidOperationException>(() => sut.GetAsync());
    }
}
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
/// Unit tests for <see cref="KonaAI.Master.Business.Master.MetaData.Logic.ModuleTypeBusiness"/>.
/// Validates:
/// - GetAsync maps repository <see cref="KonaAI.Master.Repository.Domain.Master.MetaData.ModuleType"/> to <see cref="KonaAI.Master.Model.Common.MetaDataViewModel"/>
/// - Repository exceptions are propagated and not swallowed
/// </summary>
public class ModuleTypeBusinessTests
{
    private readonly Mock<ILogger<ModuleTypeBusiness>> _logger = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly Mock<IModuleTypeRepository> _moduleTypes = new();

    public ModuleTypeBusinessTests()
    {
        // Wire UnitOfWork.ModuleTypes to our module type repository mock to mirror production resolution
        _uow.SetupGet(x => x.ModuleTypes).Returns(_moduleTypes.Object);
    }

    private ModuleTypeBusiness CreateSut() => new(_logger.Object, _mapper.Object, _uow.Object);

    [Fact]
    public async Task GetAsync_ReturnsMappedQueryable()
    {
        // Arrange: repository returns two module types; mapper projects each to MetaDataViewModel
        var data = new List<ModuleType>
        {
            new() { RowId = Guid.NewGuid(), Name = "User Management" },
            new() { RowId = Guid.NewGuid(), Name = "Data Processing" }
        }.AsQueryable();

        _moduleTypes.Setup(r => r.GetAsync()).ReturnsAsync(data);
        _mapper.Setup(m => m.Map<MetaDataViewModel>(It.IsAny<ModuleType>()))
               .Returns((ModuleType mt) => new MetaDataViewModel { RowId = mt.RowId, Name = mt.Name });

        var sut = CreateSut();

        // Act: enumerate to force LINQ execution and mapping
        var queryable = await sut.GetAsync();
        var list = queryable.ToList();

        // Assert: verify content and interaction counts
        Assert.Equal(2, list.Count);
        Assert.Contains(list, x => x.Name == "User Management");
        Assert.Contains(list, x => x.Name == "Data Processing");
        _moduleTypes.Verify(r => r.GetAsync(), Times.Once);
        _mapper.Verify(m => m.Map<MetaDataViewModel>(It.IsAny<ModuleType>()), Times.Exactly(2));
    }

    [Fact]
    public async Task GetAsync_WhenRepoThrows_Propagates()
    {
        // Arrange: bubble up repository failures without swallowing
        _moduleTypes.Setup(r => r.GetAsync()).ThrowsAsync(new InvalidOperationException("boom"));
        var sut = CreateSut();

        // Act + Assert: exception is propagated
        await Assert.ThrowsAsync<InvalidOperationException>(() => sut.GetAsync());
    }
}
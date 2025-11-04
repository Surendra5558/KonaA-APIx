using AutoMapper;
using KonaAI.Master.Business.Master.MetaData.Logic;
using KonaAI.Master.Model.Common;
using KonaAI.Master.Model.Master.SaveModel;
using KonaAI.Master.Repository.Common.Constants;
using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Repository.DataAccess.Master.MetaData.Interface;
using KonaAI.Master.Repository.Domain.Master.MetaData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace KonaAI.Master.Test.Unit.Business.Master.MetaData;

public class RenderTypeBusinessTests
{
    private readonly Mock<ILogger<RenderTypeBusiness>> _logger = new();
    private readonly Mock<IUserContextService> _userContextService = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly Mock<IRenderTypeRepository> _renderTypeRepo = new();

    public RenderTypeBusinessTests()
    {
        _uow.SetupGet(u => u.RenderTypes).Returns(_renderTypeRepo.Object);
    }

    private RenderTypeBusiness CreateSut() =>
        new(_logger.Object, _userContextService.Object, _mapper.Object, _uow.Object);

    [Fact]
    public async Task GetAsync_ReturnsQueryableResult()
    {
        // Arrange
        var renderTypes = new List<RenderType>
        {
            new() { RowId = Guid.NewGuid(), Name = "Text" },
            new() { RowId = Guid.NewGuid(), Name = "Number" }
        }.AsQueryable();

        var viewModels = new List<MetaDataViewModel>
        {
            new() { RowId = Guid.NewGuid(), Name = "Text" },
            new() { RowId = Guid.NewGuid(), Name = "Number" }
        }.AsQueryable();

        _renderTypeRepo.Setup(r => r.GetAsync()).ReturnsAsync(renderTypes);
        _mapper.Setup(m => m.Map<MetaDataViewModel>(It.IsAny<RenderType>()))
            .Returns((RenderType rt) => viewModels.First(vm => vm.RowId == rt.RowId));

        var sut = CreateSut();

        // Act
        var result = await sut.GetAsync();

        // Assert
        Assert.NotNull(result);
        _renderTypeRepo.Verify(r => r.GetAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ValidModel_ReturnsAffectedRows()
    {
        // Arrange
        var model = new RenderTypeCreateModel
        {
            Name = "Test Render Type",
            Description = "Test Description"
        };

        var entity = new RenderType { Id = 1, Name = "Test Render Type" };

        _mapper.Setup(m => m.Map<RenderType>(model)).Returns(entity);
        _renderTypeRepo.Setup(r => r.AddAsync(entity)).ReturnsAsync(entity);
        _uow.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _uow.Setup(u => u.ExecuteAsync(It.IsAny<Func<Task>>()))
            .Callback<Func<Task>>(func => func());

        var sut = CreateSut();

        // Act
        var result = await sut.CreateAsync(model);

        // Assert
        Assert.Equal(1, result);
        _mapper.Verify(m => m.Map<RenderType>(model), Times.Once);
        _userContextService.Verify(u => u.SetDomainDefaults(entity, DataModes.Add), Times.Once);
        _renderTypeRepo.Verify(r => r.AddAsync(entity), Times.Once);
        _uow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ZeroAffectedRows_ThrowsInvalidOperationException()
    {
        // Arrange
        var model = new RenderTypeCreateModel { Name = "Test Render Type" };
        var entity = new RenderType { Id = 1, Name = "Test Render Type" };

        _mapper.Setup(m => m.Map<RenderType>(model)).Returns(entity);
        _renderTypeRepo.Setup(r => r.AddAsync(entity)).ReturnsAsync(entity);
        _uow.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(0);
        _uow.Setup(u => u.ExecuteAsync(It.IsAny<Func<Task>>()))
            .Returns(async (Func<Task> func) => await func());

        var sut = CreateSut();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => sut.CreateAsync(model));
        Assert.Contains("Failed to create client", exception.Message);
    }

    [Fact]
    public async Task CreateAsync_DbUpdateException_PropagatesException()
    {
        // Arrange
        var model = new RenderTypeCreateModel { Name = "Test Render Type" };
        var entity = new RenderType { Id = 1, Name = "Test Render Type" };

        _mapper.Setup(m => m.Map<RenderType>(model)).Returns(entity);
        _renderTypeRepo.Setup(r => r.AddAsync(entity)).ReturnsAsync(entity);
        _uow.Setup(u => u.ExecuteAsync(It.IsAny<Func<Task>>()))
            .ThrowsAsync(new DbUpdateException("Database error"));

        var sut = CreateSut();

        // Act & Assert
        await Assert.ThrowsAsync<DbUpdateException>(() => sut.CreateAsync(model));
    }

    [Fact]
    public async Task GetAsync_Exception_PropagatesException()
    {
        // Arrange
        _renderTypeRepo.Setup(r => r.GetAsync()).ThrowsAsync(new Exception("Database error"));

        var sut = CreateSut();

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => sut.GetAsync());
    }
}
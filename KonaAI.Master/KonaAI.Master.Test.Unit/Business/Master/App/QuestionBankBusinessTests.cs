using AutoMapper;
using KonaAI.Master.Business.Master.App.Logic;
using KonaAI.Master.Model.Master.App.SaveModel;
using KonaAI.Master.Model.Master.App.ViewModel;
using KonaAI.Master.Repository.Common.Constants;
using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Repository.DataAccess.Master.App.Interface;
using KonaAI.Master.Repository.Domain.Master.App;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace KonaAI.Master.Test.Unit.Business.Master.App;

public class QuestionBankBusinessTests
{
    private readonly Mock<ILogger<QuestionBankBusiness>> _logger = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly Mock<IUserContextService> _userContext = new();
    private readonly Mock<IQuestionBankRepository> _questionBankRepo = new();

    public QuestionBankBusinessTests()
    {
        _uow.SetupGet(u => u.QuestionBanks).Returns(_questionBankRepo.Object);
    }

    private QuestionBankBusiness CreateSut() =>
        new(_logger.Object, _mapper.Object, _uow.Object, _userContext.Object);

    [Fact]
    public async Task GetAsync_ReturnsQueryableResult()
    {
        // Arrange
        var questionBanks = new List<QuestionBank>
        {
            new() { RowId = Guid.NewGuid(), Description = "Test Bank 1" },
            new() { RowId = Guid.NewGuid(), Description = "Test Bank 2" }
        }.AsQueryable();

        var viewModels = new List<QuestionBankViewModel>
        {
            new() { RowId = questionBanks.First().RowId, Description = "Test Bank 1" },
            new() { RowId = questionBanks.Last().RowId, Description = "Test Bank 2" }
        }.AsQueryable();

        _questionBankRepo.Setup(r => r.GetAsync()).ReturnsAsync(questionBanks);
        _mapper.Setup(m => m.Map<QuestionBankViewModel>(It.IsAny<QuestionBank>()))
            .Returns((QuestionBank qb) => viewModels.First(vm => vm.RowId == qb.RowId));

        var sut = CreateSut();

        // Act
        var result = await sut.GetAsync();

        // Assert
        Assert.NotNull(result);
        _questionBankRepo.Verify(r => r.GetAsync(), Times.Once);
    }

    [Fact]
    public async Task GetByRowIdAsync_ValidRowId_ReturnsViewModel()
    {
        // Arrange
        var rowId = Guid.NewGuid();
        var questionBank = new QuestionBank { RowId = rowId, Description = "Test Bank" };
        var viewModel = new QuestionBankViewModel { RowId = rowId, Description = "Test Bank" };

        _questionBankRepo.Setup(r => r.GetByRowIdAsync(rowId)).ReturnsAsync(questionBank);
        _mapper.Setup(m => m.Map<QuestionBankViewModel>(questionBank)).Returns(viewModel);

        var sut = CreateSut();

        // Act
        var result = await sut.GetByRowIdAsync(rowId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(rowId, result.RowId);
        Assert.Equal("Test Bank", result.Description);
        _questionBankRepo.Verify(r => r.GetByRowIdAsync(rowId), Times.Once);
    }

    [Fact]
    public async Task GetByRowIdAsync_NotFound_ThrowsKeyNotFoundException()
    {
        // Arrange
        var rowId = Guid.NewGuid();
        _questionBankRepo.Setup(r => r.GetByRowIdAsync(rowId)).ReturnsAsync((QuestionBank?)null);

        var sut = CreateSut();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => sut.GetByRowIdAsync(rowId));
        Assert.Contains($"Client with id {rowId} not found", exception.Message);
    }

    [Fact]
    public async Task CreateAsync_ValidModel_ReturnsAffectedRows()
    {
        // Arrange
        var model = new QuestionBankCreateModel
        {
            Description = "Test Bank",
            IsMandatory = true
        };

        var entity = new QuestionBank { RowId = Guid.NewGuid(), Description = "Test Bank" };

        _mapper.Setup(m => m.Map<QuestionBank>(model)).Returns(entity);
        _questionBankRepo.Setup(r => r.AddAsync(entity)).ReturnsAsync(entity);
        _uow.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _uow.Setup(u => u.ExecuteAsync(It.IsAny<Func<Task>>()))
            .Callback<Func<Task>>(func => func());

        var sut = CreateSut();

        // Act
        var result = await sut.CreateAsync(model);

        // Assert
        Assert.Equal(1, result);
        _mapper.Verify(m => m.Map<QuestionBank>(model), Times.Once);
        _userContext.Verify(u => u.SetDomainDefaults(entity, DataModes.Add), Times.Once);
        _questionBankRepo.Verify(r => r.AddAsync(entity), Times.Once);
        _uow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ZeroAffectedRows_ThrowsInvalidOperationException()
    {
        // Arrange
        var model = new QuestionBankCreateModel { Description = "Test Bank" };
        var entity = new QuestionBank { RowId = Guid.NewGuid(), Description = "Test Bank" };

        _mapper.Setup(m => m.Map<QuestionBank>(model)).Returns(entity);
        _questionBankRepo.Setup(r => r.AddAsync(entity)).ReturnsAsync(entity);
        _uow.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(0);
        _uow.Setup(u => u.ExecuteAsync(It.IsAny<Func<Task>>()))
            .Returns(async (Func<Task> func) => await func());

        var sut = CreateSut();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => sut.CreateAsync(model));
        Assert.Contains("Failed to create client", exception.Message);
    }

    [Fact]
    public async Task DeleteAsync_ValidRowId_ReturnsAffectedRows()
    {
        // Arrange
        var rowId = Guid.NewGuid();
        _questionBankRepo.Setup(r => r.DeleteAsync(rowId)).ReturnsAsync(new QuestionBank { RowId = rowId });
        _uow.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var sut = CreateSut();

        // Act
        var result = await sut.DeleteAsync(rowId);

        // Assert
        Assert.Equal(1, result);
        _questionBankRepo.Verify(r => r.DeleteAsync(rowId), Times.Once);
        _uow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_DbUpdateException_PropagatesException()
    {
        // Arrange
        var model = new QuestionBankCreateModel { Description = "Test Bank" };
        var entity = new QuestionBank { RowId = Guid.NewGuid(), Description = "Test Bank" };

        _mapper.Setup(m => m.Map<QuestionBank>(model)).Returns(entity);
        _questionBankRepo.Setup(r => r.AddAsync(entity)).ReturnsAsync(entity);
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
        _questionBankRepo.Setup(r => r.GetAsync()).ThrowsAsync(new Exception("Database error"));

        var sut = CreateSut();

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => sut.GetAsync());
    }
}
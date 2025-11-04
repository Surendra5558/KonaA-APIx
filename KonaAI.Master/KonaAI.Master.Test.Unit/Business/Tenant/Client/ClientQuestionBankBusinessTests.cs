using AutoMapper;
using KonaAI.Master.Business.Tenant.Client.Logic;
using KonaAI.Master.Model.Tenant.Client.SaveModel;
using KonaAI.Master.Repository.Common.Constants;
using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Repository.Common.Model;
using KonaAI.Master.Repository.DataAccess.Master.App.Interface;
using KonaAI.Master.Repository.DataAccess.Master.MetaData.Interface;
using KonaAI.Master.Repository.DataAccess.Tenant.Client.Interface;
using KonaAI.Master.Repository.Domain.Master.App;
using KonaAI.Master.Repository.Domain.Master.MetaData;
using KonaAI.Master.Repository.Domain.Tenant.Client;
using Microsoft.Extensions.Logging;
using Moq;

namespace KonaAI.Master.Test.Unit.Business.Tenant.Client;

public class ClientQuestionBankBusinessTests
{
    private readonly Mock<ILogger<ClientQuestionBankBusiness>> _logger = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly Mock<IUserContextService> _userContextService = new();
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly Mock<IClientQuestionBankRepository> _clientQuestionBankRepo = new();
    private readonly Mock<IQuestionBankRepository> _questionBankRepo = new();
    private readonly Mock<IRenderTypeRepository> _renderTypeRepo = new();

    public ClientQuestionBankBusinessTests()
    {
        _uow.SetupGet(u => u.ClientQuestionBanks).Returns(_clientQuestionBankRepo.Object);
        _uow.SetupGet(u => u.QuestionBanks).Returns(_questionBankRepo.Object);
        _uow.SetupGet(u => u.RenderTypes).Returns(_renderTypeRepo.Object);
    }

    private ClientQuestionBankBusiness CreateSut() =>
        new(_logger.Object, _mapper.Object, _userContextService.Object, _uow.Object);

    [Fact]
    public async Task GetAsync_ValidClientId_ReturnsQueryableResult()
    {
        // Arrange
        var clientId = 1L;
        var clientQuestionBanks = new List<ClientQuestionBank>
        {
            new() { RowId = Guid.NewGuid(), ClientId = clientId, QuestionBankId = 1 }
        }.AsQueryable();

        var questionBanks = new List<QuestionBank>
        {
            new() { Id = 1, Description = "Test Question" }
        }.AsQueryable();

        var renderTypes = new List<RenderType>
        {
            new() { Id = 1, Name = "Text" }
        }.AsQueryable();

        var viewModels = new List<ClientQuestionBankViewModel>
        {
            new() { RowId = clientQuestionBanks.First().RowId, Description = "Test Question" }
        }.AsQueryable();

        _clientQuestionBankRepo.Setup(r => r.GetAsync()).ReturnsAsync(clientQuestionBanks);
        _questionBankRepo.Setup(r => r.GetAsync()).ReturnsAsync(questionBanks);
        _renderTypeRepo.Setup(r => r.GetAsync()).ReturnsAsync(renderTypes);
        _mapper.Setup(m => m.Map<ClientQuestionBankViewModel>(It.IsAny<object>()))
            .Returns(viewModels.First());

        var sut = CreateSut();

        // Act
        var result = await sut.GetAsync(clientId);

        // Assert
        Assert.NotNull(result);
        _clientQuestionBankRepo.Verify(r => r.GetAsync(), Times.Once);
        _questionBankRepo.Verify(r => r.GetAsync(), Times.Once);
        _renderTypeRepo.Verify(r => r.GetAsync(), Times.Once);
    }

    [Fact]
    public async Task GetByRowIdAsync_ValidRowId_ReturnsViewModel()
    {
        // Arrange
        var rowId = Guid.NewGuid();
        var clientQuestionBank = new ClientQuestionBank { RowId = rowId, ClientId = 1, QuestionBankId = 1 };
        var viewModel = new ClientQuestionBankViewModel { RowId = rowId, Description = "Test Question" };

        _clientQuestionBankRepo.Setup(r => r.GetByRowIdAsync(rowId)).ReturnsAsync(clientQuestionBank);
        _mapper.Setup(m => m.Map<ClientQuestionBankViewModel>(clientQuestionBank)).Returns(viewModel);

        var sut = CreateSut();

        // Act
        var result = await sut.GetByRowIdAsync(rowId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(rowId, result.RowId);
        Assert.Equal("Test Question", result.Description);
        _clientQuestionBankRepo.Verify(r => r.GetByRowIdAsync(rowId), Times.Once);
    }

    [Fact]
    public async Task GetByRowIdAsync_NotFound_ThrowsKeyNotFoundException()
    {
        // Arrange
        var rowId = Guid.NewGuid();
        _clientQuestionBankRepo.Setup(r => r.GetByRowIdAsync(rowId)).ReturnsAsync((ClientQuestionBank?)null);

        var sut = CreateSut();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => sut.GetByRowIdAsync(rowId));
        Assert.Contains($"Client with id {rowId} not found", exception.Message);
    }

    [Fact]
    public async Task CreateAsync_ValidModel_ReturnsSuccess()
    {
        // Arrange
        var model = new ClientQuestionBankCreateModel
        {
            Text = "Test Question",
            Type = "Text",
            Required = true
        };

        var userContext = new UserContext { ClientId = 1 };
        var parentQuestion = new QuestionBank { Id = 1, Description = "Test Question" };
        var clientQuestionBank = new ClientQuestionBank { RowId = Guid.NewGuid(), ClientId = 1, QuestionBankId = 1 };
        var renderTypes = new List<RenderType>
        {
            new() { Id = 1, Name = "Text" }
        }.AsQueryable();

        _userContextService.SetupGet(u => u.UserContext).Returns(userContext);
        _renderTypeRepo.Setup(r => r.GetAsync()).ReturnsAsync(renderTypes);
        _mapper.Setup(m => m.Map<QuestionBank>(model)).Returns(parentQuestion);
        _questionBankRepo.Setup(r => r.AddAsync(It.IsAny<QuestionBank>())).ReturnsAsync(parentQuestion);
        _clientQuestionBankRepo.Setup(r => r.AddAsync(It.IsAny<ClientQuestionBank>())).ReturnsAsync(clientQuestionBank);
        _uow.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _uow.Setup(u => u.ExecuteAsync(It.IsAny<Func<Task>>()))
            .Callback<Func<Task>>(func => func());

        var sut = CreateSut();

        // Act
        var result = await sut.CreateAsync(model);

        // Assert
        Assert.Equal(1, result);
        _userContextService.Verify(u => u.SetDomainDefaults(It.IsAny<QuestionBank>(), DataModes.Add), Times.Once);
        _userContextService.Verify(u => u.SetDomainDefaults(It.IsAny<ClientQuestionBank>(), DataModes.Add), Times.Once);
        _questionBankRepo.Verify(r => r.AddAsync(It.IsAny<QuestionBank>()), Times.Once);
        _clientQuestionBankRepo.Verify(r => r.AddAsync(It.IsAny<ClientQuestionBank>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithRules_CreatesChildQuestions()
    {
        // Arrange
        var model = new ClientQuestionBankCreateModel
        {
            Text = "Parent Question",
            Type = "Text",
            Required = true,
            Rules = new List<QuestionRuleModel>
            {
                new() { Conditions = new List<string> { "Test" }, ThenAction = "Action" }
            }
        };

        var userContext = new UserContext { ClientId = 1 };
        var parentQuestion = new QuestionBank { Id = 1, Description = "Parent Question" };
        var childQuestion = new QuestionBank { Id = 2, Description = "Child Question" };
        var renderTypes = new List<RenderType>
        {
            new() { Id = 1, Name = "Text" }
        }.AsQueryable();

        _userContextService.SetupGet(u => u.UserContext).Returns(userContext);
        _renderTypeRepo.Setup(r => r.GetAsync()).ReturnsAsync(renderTypes);
        _mapper.Setup(m => m.Map<QuestionBank>(model)).Returns(parentQuestion);
        _mapper.Setup(m => m.Map<QuestionBank>(It.IsAny<QuestionRuleModel>())).Returns(childQuestion);
        _questionBankRepo.Setup(r => r.AddAsync(It.IsAny<QuestionBank>())).ReturnsAsync(parentQuestion);
        _clientQuestionBankRepo.Setup(r => r.AddAsync(It.IsAny<ClientQuestionBank>())).ReturnsAsync(new ClientQuestionBank());
        _uow.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _uow.Setup(u => u.ExecuteAsync(It.IsAny<Func<Task>>()))
            .Callback<Func<Task>>(func => func());

        var sut = CreateSut();

        // Act
        var result = await sut.CreateAsync(model);

        // Assert
        Assert.Equal(1, result);
        // Verify parent question creation
        _questionBankRepo.Verify(r => r.AddAsync(It.IsAny<QuestionBank>()), Times.AtLeast(2)); // Parent + Child
        _clientQuestionBankRepo.Verify(r => r.AddAsync(It.IsAny<ClientQuestionBank>()), Times.AtLeast(2)); // Parent + Child
    }

    [Fact]
    public async Task CreateAsync_Exception_PropagatesException()
    {
        // Arrange
        var model = new ClientQuestionBankCreateModel { Text = "Test Question" };
        var userContext = new UserContext { ClientId = 1 };

        _userContextService.SetupGet(u => u.UserContext).Returns(userContext);
        _uow.Setup(u => u.ExecuteAsync(It.IsAny<Func<Task>>()))
            .ThrowsAsync(new Exception("Database error"));

        var sut = CreateSut();

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => sut.CreateAsync(model));
    }

    [Fact]
    public async Task GetAsync_Exception_PropagatesException()
    {
        // Arrange
        var clientId = 1L;
        _clientQuestionBankRepo.Setup(r => r.GetAsync()).ThrowsAsync(new Exception("Database error"));

        var sut = CreateSut();

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => sut.GetAsync(clientId));
    }
}
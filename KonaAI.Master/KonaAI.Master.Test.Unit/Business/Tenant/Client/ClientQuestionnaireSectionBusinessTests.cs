using AutoMapper;
using KonaAI.Master.Business.Tenant.Client.Logic;
using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Repository.DataAccess.Tenant.Client.Interface;
using KonaAI.Master.Repository.DataAccess.Master.App.Interface;
using KonaAI.Master.Repository.DataAccess.Master.MetaData.Interface;
using KonaAI.Master.Repository.Domain.Tenant.Client;
using KonaAI.Master.Repository.Domain.Master.App;
using KonaAI.Master.Repository.Domain.Master.MetaData;
using Microsoft.Extensions.Logging;
using Moq;

namespace KonaAI.Master.Test.Unit.Business.Tenant.Client;

/// <summary>
/// Unit tests for <see cref="ClientQuestionnaireSectionBusiness"/>.
/// Verifies grouping by section, projection of questions, exclusion behavior from inner joins,
/// and exception propagation when repositories fail.
/// </summary>
public class ClientQuestionnaireSectionBusinessTests
{
    private readonly Mock<ILogger<ClientQuestionnaireSectionBusiness>> _logger = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly Mock<IUnitOfWork> _uow = new();

    private readonly Mock<IClientQuestionnaireSectionRepository> _sectionRepo = new();
    private readonly Mock<IClientQuestionnaireAssociationRepository> _assocRepo = new();
    private readonly Mock<IClientQuestionnaireRepository> _questionnaireRepo = new();
    private readonly Mock<IClientQuestionBankRepository> _clientQuestionBankRepo = new();
    private readonly Mock<IQuestionBankRepository> _questionBankRepo = new();
    private readonly Mock<IRenderTypeRepository> _renderTypeRepo = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="ClientQuestionnaireSectionBusinessTests"/> class
    /// and wires the mocked repositories into the <see cref="IUnitOfWork"/>.
    /// </summary>
    public ClientQuestionnaireSectionBusinessTests()
    {
        _uow.SetupGet(u => u.ClientQuestionnaireSections).Returns(_sectionRepo.Object);
        _uow.SetupGet(u => u.ClientQuestionnaireAssociations).Returns(_assocRepo.Object);
        _uow.SetupGet(u => u.ClientQuestionnaires).Returns(_questionnaireRepo.Object);
        _uow.SetupGet(u => u.ClientQuestionBanks).Returns(_clientQuestionBankRepo.Object);
        _uow.SetupGet(u => u.QuestionBanks).Returns(_questionBankRepo.Object);
        _uow.SetupGet(u => u.RenderTypes).Returns(_renderTypeRepo.Object);
    }

    /// <summary>
    /// Creates the system under test (SUT) for <see cref="ClientQuestionnaireSectionBusiness"/> using
    /// mocked logger, mapper, and unit of work dependencies.
    /// </summary>
    /// <returns>The configured <see cref="ClientQuestionnaireSectionBusiness"/> instance.</returns>
    private ClientQuestionnaireSectionBusiness CreateSut()
        => new(_logger.Object, _mapper.Object, _uow.Object);

    /// <summary>
    /// Ensures that <see cref="ClientQuestionnaireSectionBusiness.GetAsync"/> performs the join across sections,
    /// associations, and questionnaires, groups by section, and projects a collection where each section contains
    /// the expected number of questions.
    /// </summary>
    [Fact]
    public async Task GetAsync_GroupsBySection_ProjectsQuestions_ReturnsExpectedShape()
    {
        // Arrange
        var sectionAId = 1L;
        var sectionBId = 2L;
        var sectionARowId = Guid.NewGuid();
        var sectionBRowId = Guid.NewGuid();

        var sections = new List<ClientQuestionnaireSection>
            {
                new() { Id = sectionAId, RowId = sectionARowId, Name = "Section A" },
                new() { Id = sectionBId, RowId = sectionBRowId, Name = "Section B" }
            }.AsQueryable();

        var questionnaires = new List<ClientQuestionnaire>
            {
                new() { Id = 10, RowId = Guid.NewGuid(), Name = "Questionnaire 1" }
            }.AsQueryable();

        var clientQuestionBanks = new List<ClientQuestionBank>
            {
                new() { Id = 501, QuestionBankId = 200 },
                new() { Id = 502, QuestionBankId = 201 },
                new() { Id = 503, QuestionBankId = 202 }
            }.AsQueryable();

        var questionBanks = new List<QuestionBank>
            {
                new() { Id = 200, RowId = Guid.NewGuid(), Description = "Q1", RenderType = 1, Options = "[]" },
                new() { Id = 201, RowId = Guid.NewGuid(), Description = "Q2", RenderType = 1, Options = "[]" },
                new() { Id = 202, RowId = Guid.NewGuid(), Description = "Q3", RenderType = 1, Options = "[]" }
            }.AsQueryable();

        var renderTypes = new List<RenderType>
            {
                new() { Id = 1, Name = "Text" }
            }.AsQueryable();

        var associations = new List<ClientQuestionnaireAssociation>
            {
                new() { Id = 100, QuestionnaireId = 10, QuestionnaireSectionId = sectionAId, ClientQuestionBankId = 501 },
                new() { Id = 101, QuestionnaireId = 10, QuestionnaireSectionId = sectionAId, ClientQuestionBankId = 502 },
                new() { Id = 102, QuestionnaireId = 10, QuestionnaireSectionId = sectionBId, ClientQuestionBankId = 503 }
            }.AsQueryable();

        _sectionRepo.Setup(r => r.GetAsync()).ReturnsAsync(sections);
        _assocRepo.Setup(r => r.GetAsync()).ReturnsAsync(associations);
        _questionnaireRepo.Setup(r => r.GetAsync()).ReturnsAsync(questionnaires);
        _clientQuestionBankRepo.Setup(r => r.GetAsync()).ReturnsAsync(clientQuestionBanks);
        _questionBankRepo.Setup(r => r.GetAsync()).ReturnsAsync(questionBanks);
        _renderTypeRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(renderTypes));

        var sut = CreateSut();

        // Act
        var query = await sut.GetAsync();
        var result = query.ToList();

        // Assert
        Assert.Equal(2, result.Count);

        var secA = result.Single(x => x.RowId == sectionARowId);
        Assert.Equal("Section A", secA.Title);
        Assert.Equal(2, secA.Questions.Count);

        var secB = result.Single(x => x.RowId == sectionBRowId);
        Assert.Equal("Section B", secB.Title);
        Assert.Single(secB.Questions);

        // Verify repositories were queried
        _sectionRepo.Verify(r => r.GetAsync(), Times.Once);
        _assocRepo.Verify(r => r.GetAsync(), Times.Once);
    }

    /// <summary>
    /// Verifies that sections without any associations are excluded from the result due to the inner join semantics.
    /// </summary>
    [Fact]
    public async Task GetAsync_SectionWithoutAssociations_IsExcludedByInnerJoin()
    {
        // Arrange
        var orphanSectionId = 3L;
        var orphanRowId = Guid.NewGuid();

        var sections = new List<ClientQuestionnaireSection>
            {
                new() { Id = 1, RowId = Guid.NewGuid(), Name = "With Assoc" },
                new() { Id = orphanSectionId, RowId = orphanRowId, Name = "Orphan" }
            }.AsQueryable();

        var questionnaires = new List<ClientQuestionnaire>
            {
                new() { Id = 10, RowId = Guid.NewGuid(), Name = "Q1" }
            }.AsQueryable();

        var associations = new List<ClientQuestionnaireAssociation>
            {
                new() { Id = 100, QuestionnaireId = 10, QuestionnaireSectionId = 1, ClientQuestionBankId = 501 }
            }.AsQueryable();

        var clientQuestionBanks2 = new List<ClientQuestionBank>
            {
                new() { Id = 501, QuestionBankId = 300 }
            }.AsQueryable();

        var questionBanks2 = new List<QuestionBank>
            {
                new() { Id = 300, RowId = Guid.NewGuid(), Description = "QB", RenderType = 1, Options = "[]" }
            }.AsQueryable();

        var renderTypes2 = new List<RenderType>
            {
                new() { Id = 1, Name = "Text" }
            }.AsQueryable();

        _sectionRepo.Setup(r => r.GetAsync()).ReturnsAsync(sections);
        _assocRepo.Setup(r => r.GetAsync()).ReturnsAsync(associations);
        _questionnaireRepo.Setup(r => r.GetAsync()).ReturnsAsync(questionnaires);
        _clientQuestionBankRepo.Setup(r => r.GetAsync()).ReturnsAsync(clientQuestionBanks2);
        _questionBankRepo.Setup(r => r.GetAsync()).ReturnsAsync(questionBanks2);
        _renderTypeRepo.Setup(r => r.GetAsync()).Returns(Task.FromResult(renderTypes2));

        var sut = CreateSut();

        // Act
        var result = (await sut.GetAsync()).ToList();

        // Assert: only the section that has an association should be present
        Assert.Single(result);
        Assert.DoesNotContain(result, x => x.RowId == orphanRowId);
    }

    /// <summary>
    /// Confirms that when the section repository throws an exception,
    /// <see cref="ClientQuestionnaireSectionBusiness.GetAsync"/> logs and rethrows the exception without querying other repositories.
    /// </summary>
    [Fact]
    public async Task GetAsync_WhenRepositoryThrows_RethrowsException()
    {
        // Arrange
        _sectionRepo.Setup(r => r.GetAsync()).ThrowsAsync(new Exception("Repo failure"));

        var sut = CreateSut();

        // Act + Assert
        var ex = await Assert.ThrowsAsync<Exception>(() => sut.GetAsync());
        Assert.Equal("Repo failure", ex.Message);

        _sectionRepo.Verify(r => r.GetAsync(), Times.Once);
        _assocRepo.Verify(r => r.GetAsync(), Times.Never);
        _questionnaireRepo.Verify(r => r.GetAsync(), Times.Never);
    }
}

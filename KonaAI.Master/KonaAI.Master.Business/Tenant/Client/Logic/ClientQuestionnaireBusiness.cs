using AutoMapper;
using KonaAI.Master.Business.Tenant.Client.Logic.Interface;
using KonaAI.Master.Model.Tenant.Client.SaveModel;
using KonaAI.Master.Model.Tenant.Client.ViewModel;
using KonaAI.Master.Repository.Common.Constants;
using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Repository.Domain.Tenant.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace KonaAI.Master.Business.Tenant.Client.Logic;

/// <summary>
/// Provides business logic for managing client questionnaires.
/// </summary>
public class ClientQuestionnaireBusiness(ILogger<ClientQuestionBankBusiness> logger, IMapper mapper, IUserContextService userContextService,
     IUnitOfWork unitOfWork) : IClientQuestionnaireBusiness
{
    private const string ClassName = nameof(ClientQuestionnaireBusiness);

    /// <summary>
    /// Retrieves all client questionnaires.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// The task result contains a queryable collection of <see cref="ClientQuestionnaireViewModel"/>.
    /// </returns>
    /// <exception cref="Exception">Condition.</exception>
    public async Task<IQueryable<ClientQuestionnaireViewModel>> GetAsync()
    {
        const string methodName = $"{ClassName}: {nameof(GetAsync)}";
        try
        {
            logger.LogInformation("{MethodName} - method execution started", methodName);

            //Get all client users and join with users to get user details
            var result = (await unitOfWork.ClientQuestionnaires.GetAsync())
                .Select(x => mapper.Map<ClientQuestionnaireViewModel>(x));
            logger.LogInformation("{MethodName} - Retrieved client users", methodName);
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError("{MethodName} - Error in execution with error - {Error}", methodName, ex.Message);
            throw;
        }
        finally
        {
            logger.LogInformation("{MethodName} - method execution completed", methodName);
        }
    }

    /// <summary>
    /// Retrieves a specific client questionnaire by its unique row identifier.
    /// </summary>
    /// <param name="questionnaireRowId">The unique row identifier (GUID) of the client questionnaire.</param>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// The task result contains the <see cref="ClientQuestionnaireViewModel"/> if found; otherwise, <c>null</c>.
    /// </returns>
    public async Task<ClientQuestionnaireDetailsViewModel?> GetQuestionnaireDetailsAsync(Guid questionnaireRowId)
    {
        const string methodName = $"{ClassName}: {nameof(GetQuestionnaireDetailsAsync)}";
        logger.LogInformation("{MethodName} - method execution started", methodName);

        try
        {
            var clientQuestionnaires = await unitOfWork.ClientQuestionnaires.GetAsync();
            var sections = await unitOfWork.ClientQuestionnaireSections.GetAsync();
            var associations = await unitOfWork.ClientQuestionnaireAssociations.GetAsync();
            var clientQuestionBanks = await unitOfWork.ClientQuestionBanks.GetAsync();
            var questionBanks = await unitOfWork.QuestionBanks.GetAsync();
            var renderTypes = await unitOfWork.RenderTypes.GetAsync();

            var data =
                from cq in clientQuestionnaires
                where cq.RowId == questionnaireRowId &&
                      cq.ClientId == userContextService.UserContext!.ClientId
                join assoc in associations on cq.Id equals assoc.QuestionnaireId into assocGroup
                from assoc in assocGroup.DefaultIfEmpty()
                join section in sections on assoc.QuestionnaireSectionId equals section.Id into sectionGroup
                from section in sectionGroup.DefaultIfEmpty()
                join cqb in clientQuestionBanks on assoc.ClientQuestionBankId equals cqb.Id into cqbGroup
                from cqb in cqbGroup.DefaultIfEmpty()
                join qb in questionBanks on cqb.QuestionBankId equals qb.Id into qbGroup
                from qb in qbGroup.DefaultIfEmpty()
                join r in renderTypes on qb.RenderType equals r.Id into rGroup
                from r in rGroup.DefaultIfEmpty()
                select new
                {
                    cq,
                    section,
                    qb,
                    r
                };

            var result = await data
                .GroupBy(x => new { x.cq.RowId, x.cq.Name })
                .Select(g => new ClientQuestionnaireDetailsViewModel
                {
                    QuestionnaireRowId = g.Key.RowId,
                    Name = g.Key.Name,
                    Sections = g
                        .Where(x => x.section != null)
                        .GroupBy(x => x.section!.Name)
                        .Select(sectionGroup => new ClientQuestionnaireSectionViewModel
                        {
                            Title = sectionGroup.Key,
                            Questions = sectionGroup
                                .Where(x => x.qb != null)
                                .Select(x => new QuestionBankViewModel
                                {
                                    Id = x.qb!.Id,
                                    RowId = x.qb.RowId,
                                    Text = x.qb.Description ?? string.Empty,
                                    Options = TryDeserializeList(x.qb.Options, logger),
                                    RenderType = x.r != null ? x.r.Name : string.Empty,
                                    LinkedQuestion = x.qb.LinkedQuestion == 0 ? null : x.qb.LinkedQuestion,
                                    OnAction = TryDeserializeList(x.qb.OnAction, logger)
                                })
                                .ToList()
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();

            if (result == null)
            {
                logger.LogWarning("{MethodName} - No questionnaire found for RowId: {RowId}",
                    methodName, questionnaireRowId);
                return null;
            }

            logger.LogInformation("{MethodName} - method execution completed successfully", methodName);
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{MethodName} - error occurred while executing", methodName);
            throw;
        }
    }

    /// <summary>
    /// Safely attempts to deserialize a JSON string into a list of strings.
    /// If deserialization fails, logs the error and returns a fallback list.
    /// </summary>
    /// <param name="json">The JSON string to deserialize.</param>
    /// <param name="logger">The logger instance used for error tracking.</param>
    /// <returns>A list of strings parsed from the JSON input, or a fallback list if parsing fails.</returns>
    private static List<string> TryDeserializeList(string? json, ILogger? logger = null)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            logger?.LogDebug("TryDeserializeList: Input JSON is null or empty.");
            return new List<string>();
        }

        try
        {
            var list = JsonConvert.DeserializeObject<List<string>>(json!.Trim());
            if (list is { Count: > 0 })
            {
                logger?.LogDebug("TryDeserializeList: Successfully deserialized {Count} items.", list.Count);
                return list;
            }

            logger?.LogWarning("TryDeserializeList: Deserialized list is empty or null. Input: {Json}", json);
            return new List<string>();
        }
        catch (JsonException)
        {
            // Not valid JSON – fallback to returning a single-item list
            logger?.LogDebug("TryDeserializeList: Input is not valid JSON. Returning raw value.");
            return new List<string> { json };
        }
        catch (Exception ex)
        {
            logger?.LogError(ex, "TryDeserializeList: Unexpected error during deserialization.");
            return new List<string> { json };
        }
    }

    /// <summary>
    /// Creates a client questionnaire along with its sections and question associations.
    /// </summary>
    /// <param name="clientQuestionnaire">The model containing client questionnaire creation data.</param>
    /// <param name="clientId">The client identifier.</param>
    /// <returns>The number of records affected.</returns>
    /// <exception cref="DbUpdateException">Thrown when a database update fails.</exception>
    /// <exception cref="Exception">Thrown when an unexpected error occurs.</exception>
    public async Task<int> CreateAsync(ClientQuestionnaireCreateModel clientQuestionnaire, long clientId)
    {
        const string methodName = $"{ClassName}: {nameof(CreateAsync)}";
        var result = 0;
        if (!clientQuestionnaire.Sections.Any())
        {
            logger.LogError("{MethodName} - No sections provided in the questionnaire", methodName);
            throw new InvalidOperationException("At least one section must be provided to create a questionnaire.");
        }
        try
        {
            logger.LogInformation("{MethodName} - method execution started", methodName);

            await unitOfWork.ExecuteAsync(async () =>
            {
                // Step 1️⃣: Create main questionnaire record
                var questionnaireEntity = mapper.Map<ClientQuestionnaire>(clientQuestionnaire);
                questionnaireEntity.ClientId = clientId;

                userContextService.SetDomainDefaults(questionnaireEntity, DataModes.Add);

                _ = await unitOfWork.ClientQuestionnaires.AddAsync(questionnaireEntity);
                result = await unitOfWork.SaveChangesAsync();

                if (result == 0)
                {
                    logger.LogError("{MethodName} - Failed to create client questionnaire", methodName);
                    throw new InvalidOperationException("Failed to create client questionnaire.");
                }

                if (clientQuestionnaire.Sections.Any())
                {
                    foreach (var sectionModel in clientQuestionnaire.Sections)
                    {
                        var sectionEntity = mapper.Map<ClientQuestionnaireSection>(sectionModel);
                        sectionEntity.ClientId = clientId;

                        userContextService.SetDomainDefaults(sectionEntity, DataModes.Add);

                        _ = await unitOfWork.ClientQuestionnaireSections.AddAsync(sectionEntity);
                        result = await unitOfWork.SaveChangesAsync();

                        if (result == 0)
                        {
                            logger.LogError("{MethodName} - Failed to create questionnaire section", methodName);
                            throw new InvalidOperationException("Failed to create questionnaire section.");
                        }

                        // Step 3️⃣: Create associations for each question in the section
                        if (sectionModel.Questions != null && sectionModel.Questions.Any())
                        {
                            // Convert string RowIds to Guid
                            var questionRowIds = sectionModel.Questions
                                .Select(q => Guid.Parse(q.OriginalId))
                                .ToList();

                            // Fetch all matching question entities (by RowId)
                            var questionEntities = await unitOfWork.ClientQuestionBanks.GetByIdsAsync(questionRowIds);

                            // Build a lookup dictionary: RowId → Id
                            var questionLookup = questionEntities.ToDictionary(q => q.RowId, q => q.Id);

                            foreach (var question in sectionModel.Questions)
                            {
                                var questionGuid = Guid.Parse(question.OriginalId);

                                if (!questionLookup.TryGetValue(questionGuid, out var questionId))
                                {
                                    logger.LogError("{MethodName} - Question with RowId {RowId} not found in ClientQuestionBank", methodName, question.OriginalId);
                                    throw new KeyNotFoundException($"Question with RowId {question.OriginalId} not found in ClientQuestionBank");
                                }

                                var association = new ClientQuestionnaireAssociation
                                {
                                    RowId = Guid.NewGuid(),
                                    ClientId = clientId,
                                    QuestionnaireId = questionnaireEntity.Id,
                                    QuestionnaireSectionId = sectionEntity.Id,
                                    ClientQuestionBankId = questionId
                                };

                                userContextService.SetDomainDefaults(association, DataModes.Add);
                                _ = await unitOfWork.ClientQuestionnaireAssociations.AddAsync(association);
                            }

                            result = await unitOfWork.SaveChangesAsync();

                            if (result == 0)
                            {
                                logger.LogError("{MethodName} - Failed to create questionnaire associations", methodName);
                                throw new InvalidOperationException("Failed to create questionnaire associations.");
                            }
                        }
                    }
                }
            });

            logger.LogInformation("{MethodName} - Questionnaire created successfully", methodName);
            return result;
        }
        catch (DbUpdateException dex)
        {
            logger.LogError("{MethodName} - Database update exception occurred", methodName);
            foreach (var entry in dex.Entries)
            {
                logger.LogError("{MethodName} - DbUpdateException entry: {Entry}", methodName, entry.Entity?.ToString());
            }
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError("{MethodName} - Error in execution with message: {EMessage}", methodName, ex.Message);
            throw;
        }
        finally
        {
            logger.LogInformation("{MethodName} - method execution completed", methodName);
        }
    }
}
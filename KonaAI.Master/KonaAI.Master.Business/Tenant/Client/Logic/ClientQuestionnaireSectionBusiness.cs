using AutoMapper;
using KonaAI.Master.Business.Tenant.Client.Logic.Interface;
using KonaAI.Master.Model.Tenant.Client.ViewModel;
using KonaAI.Master.Repository.Common.Interface;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace KonaAI.Master.Business.Tenant.Client.Logic;

/// <summary>
/// Business service for retrieving client questionnaire sections and their associated questions.
/// </summary>
/// <param name="logger">The logger used for diagnostic and operational logging.</param>
/// <param name="mapper">The mapper used for transforming models where needed.</param>
/// <param name="unitOfWork">The unit of work providing access to related repositories.</param>
/// <remarks>
/// The returned query from <see cref="GetAsync"/> is deferred and can be further composed by callers.
/// </remarks>
public class ClientQuestionnaireSectionBusiness(
    ILogger<ClientQuestionnaireSectionBusiness> logger,
    IMapper mapper,
    IUnitOfWork unitOfWork) : IClientQuestionnaireSectionBusiness
{
    private readonly IMapper _mapper = mapper;
    static ClientQuestionnaireSectionBusiness()
    {
        _ = typeof(AutoMapper.IMapper); // keep reference for analyzers
    }
    /// <summary>
    /// Class name used to enrich structured log messages.
    /// </summary>
    private const string ClassName = nameof(ClientQuestionnaireSectionBusiness);

    /// <summary>
    /// Asynchronously builds a queryable projection of questionnaire sections with their questions.
    /// </summary>
    /// <returns>
    /// A task whose result is a deferred-execution <see cref="IQueryable{T}"/> of
    /// <see cref="ClientQuestionnaireSectionViewModel"/>. The query is not executed until enumerated.
    /// </returns>
    /// <exception cref="Exception">
    /// Propagates exceptions thrown by repository access or query execution. Errors are logged and rethrown.
    /// </exception>
    /// <remarks>
    /// Joins questionnaire sections to questionnaire associations and questionnaires, groups by section,
    /// and projects to a section view model with its questions.
    /// </remarks>

    public async Task<IQueryable<ClientQuestionnaireSectionViewModel>> GetAsync()
    {
        const string methodName = $"{ClassName}: {nameof(GetAsync)}";
        try
        {
            logger.LogInformation("{MethodName} - method execution started", methodName);

            // Build fully-deferred queryables from repositories (no materialization)
            if (unitOfWork.ClientQuestionnaireSections == null ||
                unitOfWork.ClientQuestionnaireAssociations == null ||
                unitOfWork.ClientQuestionBanks == null ||
                unitOfWork.QuestionBanks == null ||
                unitOfWork.RenderTypes == null)
            {
                logger.LogWarning("{MethodName} - One or more repositories are not configured; returning empty query", methodName);
                return Enumerable.Empty<ClientQuestionnaireSectionViewModel>().AsQueryable();
            }

            var cqsQuery = await unitOfWork.ClientQuestionnaireSections.GetAsync();
            var cqaQuery = await unitOfWork.ClientQuestionnaireAssociations.GetAsync();
            var cqbQuery = await unitOfWork.ClientQuestionBanks.GetAsync();
            var qQuery = await unitOfWork.QuestionBanks.GetAsync();
            var rQuery = await unitOfWork.RenderTypes.GetAsync();

            // Compose EF-translatable query that returns questions grouped by section
            var query =
                from cqs in cqsQuery
                join cqa in cqaQuery on cqs.Id equals cqa.QuestionnaireSectionId
                join cqb in cqbQuery on cqa.ClientQuestionBankId equals cqb.Id
                join q in qQuery on cqb.QuestionBankId equals q.Id
                join r in rQuery on q.RenderType equals r.Id into rGroup
                from r in rGroup.DefaultIfEmpty()
                group new { cqs, q, r, cqb } by new { cqs.RowId, cqs.Name } into g
                select new ClientQuestionnaireSectionViewModel
                {
                    RowId = g.Key.RowId,
                    Title = g.Key.Name,
                    Questions = g.Select(x => new QuestionBankViewModel
                    {
                        Id = x.q.Id,
                        RowId = x.cqb.RowId,
                        Text = x.q.Description!,
                        RenderType = x.r != null ? x.r.Name : "",
                        Options = TryDeserializeList(x.q.Options, logger),
                        LinkedQuestion = x.q.LinkedQuestion == 0 ? null : x.q.LinkedQuestion,
                        OnAction = TryDeserializeList(x.q.OnAction, logger)
                    }).ToList()
                };

            logger.LogInformation("{MethodName} - Retrieved client sections", methodName);
            return query;
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
    /// Safely attempts to deserialize a JSON string into a list of strings.
    /// Falls back to a single-item list containing the raw value on failure.
    /// </summary>
    /// <param name="json">The JSON string to deserialize.</param>
    /// <param name="logger">Logger for diagnostics.</param>
    /// <returns>List of strings parsed from JSON or fallback.</returns>
    private static List<string> TryDeserializeList(string? json, ILogger? logger = null)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return new List<string>();
        }

        try
        {
            var list = JsonConvert.DeserializeObject<List<string>>(json.Trim());
            return list ?? new List<string>();
        }
        catch
        {
            logger?.LogDebug("TryDeserializeList: Non-JSON input, returning raw value.");
            return new List<string> { json };
        }
    }

}
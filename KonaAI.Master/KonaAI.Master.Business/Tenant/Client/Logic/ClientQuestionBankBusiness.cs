using AutoMapper;
using KonaAI.Master.Business.Tenant.Client.Logic.Interface;
using KonaAI.Master.Business.Tenant.Client.Profile;
using KonaAI.Master.Model.Common.Constants;
using KonaAI.Master.Model.Tenant.Client.SaveModel;
using KonaAI.Master.Repository.Common.Constants;
using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Repository.Domain.Master.App;
using KonaAI.Master.Repository.Domain.Tenant.Client;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace KonaAI.Master.Business.Tenant.Client.Logic;

/// <summary>
/// Provides business logic for managing client question banks.
/// </summary>
public class ClientQuestionBankBusiness(ILogger<ClientQuestionBankBusiness> logger, IMapper mapper,
    IUserContextService userContextService,
     IUnitOfWork unitOfWork) : IClientQuestionBankBusiness
{
    private const string ClassName = nameof(ClientQuestionBankBusiness);

    /// <summary>
    /// Retrieves all client question banks.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// The task result contains a queryable collection of <see cref="ClientQuestionBankViewModel"/>.
    /// </returns>
    /// <exception cref="Exception">Condition.</exception>
    public async Task<IQueryable<ClientQuestionBankViewModel>> GetAsync(long clientId)
    {
        const string methodName = $"{ClassName}: {nameof(GetAsync)}";
        logger.LogInformation("{MethodName} - method execution started", methodName);

        try
        {
            // Build the query with joins
            var query =
                from cq in await unitOfWork.ClientQuestionBanks.GetAsync()
                join q in await unitOfWork.QuestionBanks.GetAsync()
                    on cq.QuestionBankId equals q.Id
                join rt in await unitOfWork.RenderTypes.GetAsync()
                    on q.RenderType equals rt.Id into tempRt
                from outerRt in tempRt.DefaultIfEmpty()
                where cq.IsActive
                select new ClientQuestionBankViewProfile.TempMapper
                {
                    ClientQuestionBank = cq,
                    QuestionBank = q,
                    RenderType = outerRt
                };

            // Map to view model with deserialization (must happen in-memory)
            var result = query.AsEnumerable()
                .Select(temp => mapper.Map<ClientQuestionBankViewModel>(temp))
                .AsQueryable();

            logger.LogInformation("{MethodName} - successfully retrieved data", methodName);
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{MethodName} - Error while fetching ClientQuestionBank data", methodName);
            throw;
        }
        finally
        {
            logger.LogInformation("{MethodName} - method execution completed", methodName);
        }
    }

    /// <summary>
    /// Retrieves a specific client question bank by its unique row identifier.
    /// </summary>
    /// <param name="rowId">The unique row identifier (GUID) of the client question bank.</param>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// The task result contains the <see cref="ClientQuestionBankViewModel"/> if found; otherwise, <c>null</c>.
    /// </returns>
    /// <exception cref="Exception">Condition.</exception>
    public async Task<ClientQuestionBankViewModel?> GetByRowIdAsync(Guid rowId)
    {
        const string methodName = $"{ClassName}: {nameof(GetAsync)}";
        try
        {
            logger.LogInformation("{MethodName} - method execution started", methodName);

            //Get all client users and join with users to get user details
            var domain = (await unitOfWork.ClientQuestionBanks.GetByRowIdAsync(rowId));
            if (domain == null)
            {
                logger.LogError("{MethodName} found no client with id: {RowId}", methodName, rowId);
                throw new KeyNotFoundException($"Client with id {rowId} not found.");
            }
            var result = mapper.Map<ClientQuestionBankViewModel>(domain);

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
    /// Creates a new question and its associated client-specific mapping in the database.
    /// </summary>
    /// <param name="model">
    /// The <see cref="ClientQuestionBankCreateModel"/> containing the question details,
    /// including text, type, required flag, options, and any conditional rules.
    /// </param>
    /// <returns>
    /// An <see cref="int"/> indicating the result of the operation:
    /// <list type="bullet">
    /// <item><description>1 - Question created successfully.</description></item>
    /// <item><description>0 - Operation failed or no changes were made.</description></item>
    /// </list>
    /// </returns>
    /// <exception cref="Exception">
    /// Thrown when an error occurs during database operations or entity creation.
    /// </exception>
    /// <remarks>
    /// <para>
    /// The method performs the following steps:
    /// </para>
    /// <list type="number">
    /// <item>Creates a base <see cref="QuestionBank"/> entry using details from the model.</item>
    /// <item>Links the question to a client by creating a <see cref="ClientQuestionBank"/> entry.</item>
    /// <item>
    /// If any rules are provided, creates child questions (linked questions) that are conditionally triggered
    /// based on the parent question’s rules.
    /// </item>
    /// </list>
    /// Each entity created has domain defaults applied using <c>userContextService.SetDomainDefaults</c>.
    /// </remarks>
    public async Task<int> CreateAsync(ClientQuestionBankCreateModel model)
    {
        const string methodName = $"{ClassName}: {nameof(CreateAsync)}";
        logger.LogInformation("{MethodName} - method execution started", methodName);

        int result = 0;

        try
        {
            await unitOfWork.ExecuteAsync(async () =>
            {
                // 🔹 Step 1: Preload RenderType IDs (avoids multiple DB hits)
                var renderTypes = await unitOfWork.RenderTypes.GetAsync();
                var renderTypeMap = renderTypes.ToDictionary(rt => rt.Name, rt => rt.Id, StringComparer.OrdinalIgnoreCase);

                // Helper to get render type safely
                long GetRenderTypeId(string typeName) =>
                    renderTypeMap.TryGetValue(typeName, out var id) ? id : 0;

                // 🔹 Step 2: Create Parent Question
                var parentQuestion = mapper.Map<QuestionBank>(model);
                parentQuestion.RenderType = GetRenderTypeId(model.Type);

                userContextService.SetDomainDefaults(parentQuestion, DataModes.Add);
                await unitOfWork.QuestionBanks.AddAsync(parentQuestion);
                await unitOfWork.SaveChangesAsync();

                // 🔹 Step 3: Link ClientQuestionBank
                var clientParent = new ClientQuestionBank
                {
                    ClientId = userContextService.UserContext!.ClientId,
                    QuestionBankId = parentQuestion.Id
                };
                userContextService.SetDomainDefaults(clientParent, DataModes.Add);
                await unitOfWork.ClientQuestionBanks.AddAsync(clientParent);
                await unitOfWork.SaveChangesAsync();

                // 🔹 Step 4: Create Linked Child Questions (Rules)
                if (model.Rules != null && model.Rules.Count > 0)
                {
                    foreach (var rule in model.Rules)
                    {
                        // Create a fresh QuestionBank for the child to avoid inheriting unintended fields
                        var childQuestion = new QuestionBank
                        {
                            Description = rule.ThenAction,
                            IsMandatory = false,
                            Options = string.Empty,
                            LinkedQuestion = parentQuestion.Id,
                            OnAction = JsonConvert.SerializeObject(rule.Conditions),
                            RenderType = GetRenderTypeId(Constants.ChildQuestionRenderType)
                        };

                        userContextService.SetDomainDefaults(childQuestion, DataModes.Add);
                        await unitOfWork.QuestionBanks.AddAsync(childQuestion);
                        await unitOfWork.SaveChangesAsync();

                        // Link child question to client
                        var clientChild = new ClientQuestionBank
                        {
                            ClientId = userContextService.UserContext!.ClientId,
                            QuestionBankId = childQuestion.Id
                        };
                        userContextService.SetDomainDefaults(clientChild, DataModes.Add);
                        await unitOfWork.ClientQuestionBanks.AddAsync(clientChild);
                        await unitOfWork.SaveChangesAsync();
                    }
                }

                result = 1;
            });

            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{MethodName} - Error creating question: {Message}", methodName, ex.Message);
            throw;
        }
        finally
        {
            logger.LogInformation("{MethodName} - method execution completed", methodName);
        }
    }
}
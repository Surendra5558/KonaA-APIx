using AutoMapper;
using AutoMapper.QueryableExtensions;
using KonaAI.Master.Model.Tenant.Client.ViewModel;
using KonaAI.Master.Repository.Common.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace KonaAI.Master.API.Controllers.Tenant.Client;

/// <summary>
/// OData controller for ClientQuestionnaireAssociation entity set.
/// </summary>
[Authorize]
public class ClientQuestionnaireAssociationController(
    ILogger<ClientQuestionnaireAssociationController> logger,
    IUnitOfWork unitOfWork,
    IMapper mapper
) : ODataController
{
    private const string ClassName = nameof(ClientQuestionnaireAssociationController);

    [HttpGet]
    [EnableQuery]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAsync()
    {
        const string methodName = $"{ClassName}: {nameof(GetAsync)}";
        try
        {
            logger.LogInformation("{MethodName} - method execution started", methodName);
            var query = (await unitOfWork.ClientQuestionnaireAssociations.GetAsync())
                .ProjectTo<ClientQuestionnaireAssociationViewModel>(mapper.ConfigurationProvider);
            return Ok(query);
        }
        catch (Exception ex)
        {
            logger.LogError("{MethodName} - Error: {Error}", methodName, ex.Message);
            return StatusCode(500, ex.Message);
        }
        finally
        {
            logger.LogInformation("{MethodName} - method execution completed", methodName);
        }
    }
}





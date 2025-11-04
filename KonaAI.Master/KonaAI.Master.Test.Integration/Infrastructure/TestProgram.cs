using KonaAI.Master.API.Extensions;
using KonaAI.Master.API.Model;
using KonaAI.Master.Repository;
using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Test.Integration.Infrastructure.Fixtures;
using KonaAI.Master.Test.Integration.Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace KonaAI.Master.Test.Integration.Infrastructure;

/// <summary>
/// Test-specific Program class that avoids production dependencies.
/// </summary>
public partial class TestProgram
{
    // This class is used as a marker for WebApplicationFactory
    // The actual application configuration is done in InMemoryWebApplicationFactory
}
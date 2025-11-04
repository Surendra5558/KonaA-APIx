using FluentValidation;
using KonaAI.Master.API.Handler.Authorize;
using KonaAI.Master.Business.Authentication.Logic;
using KonaAI.Master.Business.Authentication.Logic.Interface;
using KonaAI.Master.Business.Master.App.Profile;
using KonaAI.Master.Model.Common;
using KonaAI.Master.Model.Master.App.SaveModel;
using KonaAI.Master.Model.Master.SaveModel;
using KonaAI.Master.Model.Tenant.Client.SaveModel;
using KonaAI.Master.Repository.Common;
using KonaAI.Master.Repository.Common.Interface;
using Microsoft.AspNetCore.Authorization;
using System.Reflection;
using KonaAI.Master.API.Scheduler;
using KonaAI.Master.Business.Tenant.Client.Logic.Interface;
using KonaAI.Master.Business.Tenant.Client.Logic;

namespace KonaAI.Master.API.Extensions;

/// <summary>
/// Dependency injection extension methods for registering services.
/// </summary>
public static class DependencyExtension
{
    /// <summary>
    /// Registers repositories and business services into Microsoft DI by scanning assemblies.
    /// </summary>
    /// <remarks>
    /// This replaces the previous Autofac-only registration code so that services can be
    /// resolved when using the default ASP.NET Core service provider.
    /// </remarks>
    public static void AddDependencyInjection(this IServiceCollection services)
    {
        // Register AutoMapper profiles explicitly from the Business assembly
        services.AddAutoMapper(cfg =>
        {
            cfg.AddMaps(typeof(ClientViewModelProfile).Assembly);
            cfg.AddMaps(typeof(MetaDataViewModel).Assembly);
        });

        // Default Services
        services.AddResponseCaching();

        // Authorization
        services.AddScoped<IAuthorizationHandler, AccessAuthorizationHandler>();
        services.AddSingleton<IAuthorizationPolicyProvider, AccessAuthorizationPolicyProvider>();
        // Do NOT register AccessAuthorizationRequirement in DI; it's created per-policy.

        services.AddValidatorsFromAssemblyContaining<ClientValidator>();
        services.AddValidatorsFromAssemblyContaining<QuestionBankCreateModelValidator>();
        services.AddValidatorsFromAssemblyContaining<RenderTypeCreateModelValidator>();
        services.AddValidatorsFromAssemblyContaining<ClientQuestionnaireCreateModelValidator>();
        services.AddValidatorsFromAssemblyContaining<ClientQuestionBankCreateModelValidator>();

        services.AddHttpContextAccessor();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserContextService, UserContextService>();
        services.AddScoped<ILicenseService, LicenseService>();
        services.AddScoped<ProjectExecutor>();
        // Register authentication business service
        services.AddScoped<IUserLoginBusiness, UserLoginBusiness>();
        services.AddScoped<IProjectSchedulerBusiness, ProjectSchedulerBusiness>();
        services.AddAuthorizationPolicies();

        Assembly.Load("KonaAI.Master.Repository");
        Assembly.Load("KonaAI.Master.Business");
        var assemblies = AppDomain.CurrentDomain.GetAssemblies()
            .Where(a =>
            {
                var name = a.GetName().Name;
                return name != null &&
                       (name.StartsWith("KonaAI.Master.Repository", StringComparison.Ordinal) ||
                        name.StartsWith("KonaAI.Master.Business", StringComparison.Ordinal));
            })
            .ToArray();

        foreach (var assembly in assemblies)
        {
            var types = assembly.GetTypes()
                .Where(t => t is { IsClass: true, IsAbstract: false } &&
                            (t.Name.EndsWith("Repository", StringComparison.Ordinal) ||
                             t.Name.EndsWith("Business", StringComparison.Ordinal)));

            foreach (var implementationType in types)
            {
                var serviceTypes = implementationType.GetInterfaces()
                    .Where(i => i.Namespace != null &&
                                (i.Namespace.StartsWith("KonaAI.Master.Repository", StringComparison.Ordinal) ||
                                 i.Namespace.StartsWith("KonaAI.Master.Business", StringComparison.Ordinal)));

                foreach (var serviceType in serviceTypes)
                {
                    services.AddScoped(serviceType, implementationType);
                }
            }
        }
    }
}
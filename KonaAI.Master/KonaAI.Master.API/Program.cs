using KonaAI.Master.API.Extensions;
using Microsoft.AspNetCore.OData;
using KonaAI.Master.Repository;
using KonaAI.Master.Repository.Common;
using KonaAI.Master.Repository.Common.Interface;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Quartz;
using Serilog;
using KonaAI.Master.API.Model;
using Microsoft.OpenApi;
using KonaAI.Master.API.Scheduler;
using KonaAI.Master.Business.Tenant.Client.Logic;
using KonaAI.Master.Business.Tenant.Client.Logic.Interface;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

// Add services to the container.
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.MimeTypes = ["application/json", "text/plain", "text/css", "application/javascript", "image/svg+xml"];
    options.Providers.Add<BrotliCompressionProvider>();
});

builder.Services.AddCors(options =>
{
    // this defines a CORS policy called "default"
    options.AddPolicy("default", policy =>
    {
        policy.WithOrigins((builder.Configuration["AllowedHosts"] == null ||
                            string.IsNullOrEmpty(builder.Configuration["AllowedHosts"])
            ? builder.Configuration["AllowedHosts"]
            : "*") ?? "*").AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

// Configure HTTPS redirection port to align with launchSettings
builder.Services.AddHttpsRedirection(options =>
{
    options.HttpsPort = 7259;
});

builder.Services.AddControllers()
    .AddOData(opt =>
        opt.AddRouteComponents("v1", ODataEdmModelBuilder.GetModels())
           .EnableQueryFeatures(maxTopValue: 100)
           .SkipToken()
           .Select()
           .Filter()
           .OrderBy()
           .Count()
           .Expand()
    );

builder.Services.AddDbContext<DefaultContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(maxRetryCount: 1,
                maxRetryDelay: TimeSpan.FromSeconds(10),
                errorNumbersToAdd: null);
            sqlOptions.MigrationsAssembly(typeof(DefaultContext).Assembly.FullName);
        });
});

// Register AutoMapper profiles explicitly from the Business assembly
builder.Services.AddDependencyInjection();
builder.Services.AddSwagger();
builder.Services.AddCustomAuthentication(builder.Configuration);
// EF Core: DbContext for Repository DefaultContext and required services
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserContextService, UserContextService>();
builder.Services.AddDbContext<DefaultContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Business services for Quartz jobs
builder.Services.AddScoped<IProjectSchedulerBusiness, ProjectSchedulerBusiness>();

// Quartz job configuration
builder.Services.AddQuartz(options =>
{
    // Get cron schedule from appsettings.json, default to every 30 seconds for testing
    var cronSchedule = builder.Configuration["ProjectScheduler"] ?? "0/30 * * * * ?";

    Log.Information("Configuring ProjectExecutor with cron schedule: {CronSchedule}", cronSchedule);

    options.AddJob<ProjectExecutor>(JobKey.Create(nameof(ProjectExecutor)))
        .AddTrigger(trigger => trigger
            .ForJob(JobKey.Create(nameof(ProjectExecutor)))
            .WithCronSchedule(cronSchedule));
});

// Add Quartz hosted service to run jobs
builder.Services.AddQuartzHostedService(options =>
{
    options.WaitForJobsToComplete = true;
    options.StartDelay = TimeSpan.FromSeconds(5); // Start after 5 seconds
});

var app = builder.Build();

// Optional: Add immediate trigger for testing
using (var scope = app.Services.CreateScope())
{
    var quartzFactory = scope.ServiceProvider.GetRequiredService<ISchedulerFactory>();
    var scheduler = await quartzFactory.GetScheduler();

    // Add immediate trigger for testing
    var immediateTrigger = TriggerBuilder.Create()
        .WithIdentity("ProjectExecutorImmediate")
        .StartNow()
        .ForJob(JobKey.Create(nameof(ProjectExecutor)))
        .Build();

    await scheduler.ScheduleJob(immediateTrigger);
    Log.Information("Added immediate trigger for ProjectExecutor");

    // Start the scheduler
    await scheduler.Start();
    Log.Information("Quartz scheduler started successfully");
}
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle



// Optional: validate AutoMapper configuration early (dev only)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(c =>
    {
        c.OpenApiVersion = OpenApiSpecVersion.OpenApi3_0;
    });
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("./v1/swagger.json", "KonaAI.Master.API");
    });
}

app.UseResponseCompression();
app.UseCors("default");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

public partial class Program { }
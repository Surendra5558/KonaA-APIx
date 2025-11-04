using AutoMapper;
using KonaAI.Master.Business.Tenant.Client.Logic;
using KonaAI.Master.Model.Tenant.Client.SaveModel;
using KonaAI.Master.Repository.Common.Constants;
using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Repository.DataAccess.Tenant.Client.Interface;
using KonaAI.Master.Repository.DataAccess.Master.UserMetaData.Interface;
using KonaAI.Master.Repository.DataAccess.Master.MetaData.Interface;
using KonaAI.Master.Repository.Domain.Tenant.Client;
using KonaAI.Master.Repository.Domain.Master.UserMetaData;
using KonaAI.Master.Repository.Domain.Master.MetaData;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using KonaAI.Master.Test.Integration.Extensions;

namespace KonaAI.Master.Test.Integration.Business.Tenant.Client.Tests;

public class ClientProjectBusinessTests
{
    private static ClientProjectBusiness CreateSut(
        out Mock<IUnitOfWork> uow,
        out Mock<IMapper> mapper,
        out Mock<IUserContextService> userContext,
        out IConfiguration configuration,
        out Mock<ILicenseService> licenseService)
    {
        var logger = new Mock<ILogger<ClientProjectBusiness>>();
        mapper = new Mock<IMapper>(MockBehavior.Strict);
        userContext = new Mock<IUserContextService>(MockBehavior.Strict);
        uow = new Mock<IUnitOfWork>(MockBehavior.Strict);
        configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:DefaultConnection"] = "Server=.;Database=master;User ID=sa;Password=P@ssw0rd;TrustServerCertificate=True"
            })
            .Build();

        licenseService = new Mock<ILicenseService>(MockBehavior.Loose);

        // Setup UserContext with a valid UserContext
        var userContextModel = new KonaAI.Master.Repository.Common.Model.UserContext
        {
            ClientId = 77,
            UserRowId = Guid.NewGuid(),
            UserLoginId = 1,
            UserLoginName = "TestUser",
            UserLoginEmail = "test@example.com",
            RoleRowId = Guid.NewGuid(),
            RoleId = 1,
            RoleName = "TestRole"
        };
        userContext.Setup(x => x.UserContext).Returns(userContextModel);

        var sut = new ClientProjectBusiness(logger.Object, mapper.Object, userContext.Object, uow.Object, configuration, licenseService.Object);
        return sut;
    }

    [Fact]
    public async Task CreateAsync_Throws_WhenDuplicateName()
    {
        var sut = CreateSut(out var uow, out var mapper, out var userContext, out var config, out var license);
        var payload = new ClientProjectCreateModel { Name = "Dup" };

        var projectRepo = new Mock<IClientProjectRepository>(MockBehavior.Strict);
        projectRepo.Setup(r => r.GetAsync()).ReturnsAsync(new List<ClientProject> { new() { Name = "Dup" } }.AsQueryable());
        uow.SetupGet(x => x.ClientProjects).Returns(projectRepo.Object);

        await Assert.ThrowsAsync<InvalidOperationException>(() => sut.CreateAsync(payload));
    }

    [Fact]
    public async Task CreateAsync_Creates_Project_And_Scheduler()
    {
        var sut = CreateSut(out var uow, out var mapper, out var userContext, out var config, out var license);
        var payload = new ClientProjectCreateModel
        {
            Name = "Project X",
            AuditResponsibilityRowId = Guid.NewGuid(),
            RiskAreaRowId = Guid.NewGuid()
        };

        var projectRepo = new Mock<IClientProjectRepository>(MockBehavior.Strict);
        var auditRepo = new Mock<IProjectAuditResponsibilityRepository>(MockBehavior.Strict);
        var riskRepo = new Mock<IProjectRiskAreaRepository>(MockBehavior.Strict);
        var countryRepo = new Mock<ICountryRepository>(MockBehavior.Strict);
        var unitRepo = new Mock<IProjectUnitRepository>(MockBehavior.Strict);
        var deptRepo = new Mock<IProjectDepartmentRepository>(MockBehavior.Strict);
        var schedulerRepo = new Mock<IProjectSchedulerRepository>(MockBehavior.Strict);

        // lookups
        auditRepo.Setup(r => r.GetAsync()).ReturnsAsync(new List<ProjectAuditResponsibility> { new() { Id = 2, RowId = payload.AuditResponsibilityRowId } }.AsQueryable());
        riskRepo.Setup(r => r.GetAsync()).ReturnsAsync(new List<ProjectRiskArea> { new() { Id = 3, RowId = payload.RiskAreaRowId } }.AsQueryable());
        countryRepo.Setup(r => r.GetAsync()).ReturnsAsync(new List<Country>().AsQueryable());
        unitRepo.Setup(r => r.GetAsync()).ReturnsAsync(new List<ProjectUnit>().AsQueryable());
        deptRepo.Setup(r => r.GetAsync()).ReturnsAsync(new List<ProjectDepartment>().AsQueryable());

        // existing projects none
        projectRepo.Setup(r => r.GetAsync()).ReturnsAsync(new List<ClientProject>().AsQueryable());

        // map payload to domain
        mapper.Setup(m => m.Map<ClientProject>(payload)).Returns(new ClientProject { Id = 10, Name = payload.Name, RowId = Guid.NewGuid(), Modules = "T&E" });

        // add project and save
        projectRepo.Setup(r => r.AddAsync(It.IsAny<ClientProject>())).ReturnsAsync((ClientProject p) => p);
        uow.SetupGet(x => x.ClientProjects).Returns(projectRepo.Object);
        uow.SetupGet(x => x.ProjectAuditResponsibilities).Returns(auditRepo.Object);
        uow.SetupGet(x => x.ProjectRiskAreas).Returns(riskRepo.Object);
        uow.SetupGet(x => x.Countries).Returns(countryRepo.Object);
        uow.SetupGet(x => x.ProjectUnits).Returns(unitRepo.Object);
        uow.SetupGet(x => x.ProjectDepartments).Returns(deptRepo.Object);
        uow.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        // scheduler add
        schedulerRepo.Setup(r => r.GetNextSchedulerIdAsync()).ReturnsAsync(100);
        schedulerRepo.Setup(r => r.AddAsync(It.IsAny<KonaAI.Master.Repository.Domain.Tenant.Client.ProjectScheduler>()))
            .ReturnsAsync((KonaAI.Master.Repository.Domain.Tenant.Client.ProjectScheduler s) => s);
        uow.SetupGet(x => x.ProjectSchedulers).Returns(schedulerRepo.Object);
        uow.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        // configuration provided via in-memory ConfigurationBuilder in CreateSut

        // user context defaults (no strict expectations needed in this test)
        userContext.Setup(u => u.SetDomainDefaults(It.IsAny<ClientProject>(), It.IsAny<DataModes>()));
        userContext.Setup(u => u.SetDomainDefaults(It.IsAny<KonaAI.Master.Repository.Domain.Tenant.Client.ProjectScheduler>(), It.IsAny<DataModes>()));

        var count = await sut.CreateAsync(payload);

        Assert.Equal(1, count);
        schedulerRepo.Verify(r => r.AddAsync(It.IsAny<KonaAI.Master.Repository.Domain.Tenant.Client.ProjectScheduler>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_SoftDeletes_WhenFound()
    {
        var sut = CreateSut(out var uow, out var mapper, out var userContext, out var config, out var license);
        var rowId = Guid.NewGuid();

        var projectRepo = new Mock<IClientProjectRepository>(MockBehavior.Strict);
        projectRepo.Setup(r => r.GetAsync()).ReturnsAsync(new List<ClientProject> { new() { RowId = rowId, IsActive = true } }.AsAsyncQueryable());
        projectRepo.Setup(r => r.UpdateAsync(It.IsAny<ClientProject>())).ReturnsAsync((ClientProject p) => p);
        uow.SetupGet(x => x.ClientProjects).Returns(projectRepo.Object);
        uow.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var result = await sut.DeleteAsync(rowId);
        Assert.Equal(1, result);
    }
}



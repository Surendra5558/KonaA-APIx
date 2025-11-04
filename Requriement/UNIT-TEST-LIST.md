---
description: Checklist of Business classes and their unit test coverage status
modifiedAt: 2025-10-21
---
# Unit Test Coverage Checklist (Business Layer)

Legend: [x] has unit tests in this project, [ ] pending

## Master / App
- [x] ClientBusiness (`Business/Master/App/ClientBusinessTests.cs`)
- [ ] QuestionBankBusiness (`Business/Master/App/QuestionBankBusinessTests.cs`) - **MISSING**

## Master / MetaData
- [x] ModuleSourceTypeBusiness (`Business/Master/MetaData/ModuleSourceTypeBusinessTests.cs`)
- [x] ModuleTypeBusiness (`Business/Master/MetaData/ModuleTypeBusinessTests.cs`)
- [x] NavigationBusiness (`Business/Master/MetaData/NavigationBusinessTests.cs`)
- [x] NavigationUserActionBusiness (`Business/Master/MetaData/NavigationUserActionBusinessTests.cs`)
- [x] RoleNavigationActionBusiness (`Business/Master/MetaData/RoleNavigationActionBusinessTests.cs`)
- [x] RoleTypeBusiness (`Business/Master/MetaData/RoleTypeBusinessTests.cs`)
- [ ] RenderTypeBusiness (`Business/Master/MetaData/RenderTypeBusinessTests.cs`) - **MISSING**

## Master / UserMetaData
- [x] CountryBusiness (`Business/Master/UserMetaData/CountryBusinessTests.cs`)
- [x] ProjectAuditResponsibilityBusiness (`Business/Master/UserMetaData/ProjectAuditResponsibilityBusinessTests.cs`)
- [x] ProjectDepartmentBusiness (`Business/Master/UserMetaData/ProjectDepartmentBusinessTests.cs`)
- [x] ProjectRiskAreaBusiness (`Business/Master/UserMetaData/ProjectRiskAreaBusinessTests.cs`)
- [x] ProjectUnitBusiness (`Business/Master/UserMetaData/ProjectUnitBusinessTests.cs`)

## Tenant / Client
- [x] ClientProjectBusiness (`Business/Tenant/Client/ClientProjectBusinessTests.cs`)
- [x] ClientUserBusiness (`Business/Tenant/Client/ClientUserBusinessTests.cs`)
- [x] ClientLicenseBusiness (`Business/Tenant/Client/ClientLicenseBusinessTests.cs`)
- [x] ClientQuestionnaireBusiness (`Business/Tenant/Client/ClientQuestionnaireBusinessTests.cs`)
- [ ] ClientQuestionBankBusiness (`Business/Tenant/Client/ClientQuestionBankBusinessTests.cs`) - **MISSING**

## Tenant / MetaData
- [x] ClientRoleTypeBusiness (`Business/Tenant/MetaData/ClientRoleTypeBusinessTests.cs`)

## Tenant / UserMetaData
- [x] ClientProjectAuditResponsibilityBusiness (`Business/Tenant/UserMetaData/ClientProjectAuditResponsibilityBusinessTests.cs`)
- [x] ClientProjectCountryBusiness (`Business/Tenant/UserMetaData/ClientProjectCountryBusinessTests.cs`)
- [x] ClientProjectDepartmentBusiness (`Business/Tenant/UserMetaData/ClientProjectDepartmentBusinessTests.cs`)
- [x] ClientProjectRiskAreaBusiness (`Business/Tenant/UserMetaData/ClientProjectRiskAreaBusinessTests.cs`)
- [x] ClientProjectUnitBusiness (`Business/Tenant/UserMetaData/ClientProjectUnitBusinessTests.cs`)

## Authentication
- [x] MenuBusiness (`Business/Authentication/MenuBusinessTests.cs`)
- [x] UserLoginBusiness (`Business/Authentication/UserLoginBusinessTests.cs`)

## Notes / Patterns to Follow
- Prefer UoW `SetupGet` wiring to match production resolution.
- For async EF operations (`ToListAsync`) on repo `IQueryable`, use async-capable queryables in tests (see `ClientProjectBusinessTests` async helpers).
- For create/update flows using synchronous DbSet LINQ via repository `Context`, provide a fake `DefaultContext` with DbSet instances (see `ClientUserBusinessTests`).
- Assert both happy paths and exception propagation; verify mapping and repository interactions.

## Implementation Status
- **Total Business Classes**: 27
- **Unit Tests Implemented**: 24 (89%)
- **Missing**: 3 (11%)
- **Test Results**: 248 passing, 2 skipped (AutoMapper ProjectTo limitations)
- **Coverage**: Comprehensive unit test coverage for most business logic patterns
- **Last Updated**: 2025-01-21

## Missing Unit Tests (3 files needed):
1. **QuestionBankBusinessTests.cs** - Master/App/QuestionBankBusiness
2. **RenderTypeBusinessTests.cs** - Master/MetaData/RenderTypeBusiness  
3. **ClientQuestionBankBusinessTests.cs** - Tenant/Client/ClientQuestionBankBusiness

## Advanced Patterns Implemented
- ExecuteAsync transaction testing with proper delegate execution
- Service dependencies with complex return types (ILicenseService)
- UserContext null safety handling
- AutoMapper ProjectTo limitations (skipped in favor of integration tests)
- Repository exception propagation testing
- Complex business logic with multiple repository joins



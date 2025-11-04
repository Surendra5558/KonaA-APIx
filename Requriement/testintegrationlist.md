---
description: Integration test coverage checklist for API controllers
modifiedAt: 2025-10-21
---
## Integration Test Coverage Checklist (API Controllers)

Legend: [x] has integration tests in this repo, [ ] pending

### Master / App
- [x] ClientController
- [ ] QuestionBankController - **MISSING**

### Master / Authentication
- [x] Authentication/LoginController
- [x] Authentication/AppNavigationController
- [x] Authentication/MenuController

### Master / MetaData (read-only GET)
- [x] RoleTypeController
- [x] RoleNavigationActionController
- [x] ProjectUnitController
- [x] ProjectRiskAreaController
- [x] ProjectDepartmentController
- [x] ProjectAuditResponsibilityController
- [x] NavigationActionController
- [x] ModuleController
- [x] CountryController
- [ ] RenderTypeController - **MISSING**

### Tenant / Client
- [x] ClientProjectController
- [x] ClientUserController
- [x] ClientLicenseController
- [x] ClientQuestionnaireController
- [ ] ClientQuestionBankController - **MISSING**

### Tenant / MetaData (read-only GET)
- [x] ClientRoleTypeController

### Tenant / UserMetaData (read-only GET)
- [x] ClientProjectCountryController
- [x] ClientProjectUnitController
- [x] ClientProjectRiskAreaController
- [x] ClientProjectDepartmentController
- [x] ClientProjectAuditResponsibilityController

### Misc (optional)
- [x] WeatherForecastController

## Implementation Summary
- **Total Controllers**: 27 controllers
- **Implemented**: 24 controllers (89% complete)
- **Missing**: 3 controllers (11%)
- **Test Coverage**: 125 integration tests
- **Status**: ✅ Most integration tests implemented and passing

## Missing Integration Tests (3 files needed):
1. **QuestionBankControllerTests.cs** - App/QuestionBankController
2. **RenderTypeControllerTests.cs** - Master/MetaData/RenderTypeController
3. **ClientQuestionBankControllerTests.cs** - Tenant/Client/ClientQuestionBankController

## Notes / Patterns to Follow
- For read-only controllers, add tests for: GET list (200) and GET by RowId (200 when found, 404 when missing).
- For write-capable controllers, add: happy path (201/204), validation failure (400), not found (404 if applicable).
- Reuse test setup patterns from existing controller tests under `KonaAI.Master.Test.Integration/Controllers/**`.


## Authorization Test Coverage – KonaAI.Master.API

Updated: 2025-01-27

### ✅ **WORKING** (authorization tests passing)
- **Authentication**
  - ✅ LoginController (class is NOT [Authorize]) → LoginControllerUnitTests.cs
  - ✅ MenuController (class/method [Authorize]) → MenuControllerUnitTests.cs
  - ✅ AppNavigationController (class/method [Authorize]) → AppNavigationControllerUnitTests.cs
- **Master/App**
  - ✅ ClientController (policies: View/Add/Edit/Delete) → ClientControllerUnitTests.cs
- **Master/MetaData**
  - ✅ CountryController (class [Authorize]) → CountryControllerUnitTests.cs
  - ✅ ModuleController (class [Authorize]) → ModuleControllerUnitTests.cs
  - ✅ NavigationActionController (class [Authorize]) → NavigationActionControllerUnitTests.cs
  - ✅ ProjectUnitController (class [Authorize]) → ProjectUnitControllerUnitTests.cs
  - ✅ RoleTypeController (class [Authorize]) → RoleTypeControllerUnitTests.cs
  - ✅ RoleNavigationActionController (class [Authorize]) → RoleNavigationUserActionControllerUnitTests.cs
  - ✅ ProjectRiskAreaController (class [Authorize]) → ProjectRiskAreaControllerUnitTests.cs
  - ✅ ProjectDepartmentController (class [Authorize]) → ProjectDepartmentControllerUnitTests.cs
  - ✅ ProjectAuditResponsibilityController (class [Authorize]) → ProjectAuditResponsibilityControllerUnitTests.cs
- **Tenant/Client**
  - ✅ ClientProjectController (class [Authorize], policies: View/Add) → ClientProjectControllerUnitTests.cs
  - ✅ ClientUserController (policies: View/Add/Edit/Delete) → ClientUserControllerUnitTests.cs
  - ✅ ClientLicenseController (method-level policies) → ClientLicenseControllerUnitTests.cs
- **Tenant/MetaData**
  - ✅ ClientRoleTypeController (class [Authorize]) → ClientRoleTypeControllerUnitTests.cs
- **Tenant/UserMetaData**
  - ✅ ClientProjectUnitController (class [Authorize]) → ClientProjectUnitControllerUnitTests.cs
  - ✅ ClientProjectRiskAreaController (class [Authorize]) → ClientProjectRiskAreaControllerUnitTests.cs
  - ✅ ClientProjectDepartmentController (class [Authorize]) → ClientProjectDepartmentControllerUnitTests.cs
  - ✅ ClientProjectCountryController (class [Authorize]) → ClientProjectCountryControllerUnitTests.cs
  - ✅ ClientProjectAuditResponsibilityController (class [Authorize]) → ClientProjectAuditResponsibilityControllerUnitTests.cs

### ❌ **PENDING** (authorization assertions TO ADD in unit tests)
- **Other**
  - ❌ WeatherForecastController → (no unit test file yet)

### 📊 **SUMMARY**
- **Working Tests**: All listed controllers above ✅
- **Pending**: 1 controller ❌ (WeatherForecastController)
- Full unit test run: 294 total, 0 failed, 292 passed, 2 skipped

### Notes
- Normalization used in tests ignores whitespace differences inside policy strings.
- Login endpoint must remain unauthenticated (no [Authorize] at class level).



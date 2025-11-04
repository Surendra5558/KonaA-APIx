## Authorization Guide – KonaAI.Master

Updated: 2025-01-27

### Overview
- Authentication: JWT Bearer (default scheme). Tokens validated via `Tokens:*` config.
- Default policy: authenticated users only.
- Authorization: enforced with ASP.NET Core `[Authorize]` attributes.

### Patterns We Use
- Class-level `[Authorize]`: Secures all actions in a controller (ideal for read-only metadata controllers).
- Method-level `[Authorize(Policy = "...")]`: Fine-grained permissions per action (View/Add/Edit/Delete).
- Public endpoints: Login must NOT be decorated with class-level `[Authorize]`.

### Policy Format (Normalized)
- Pattern: `Permission : Navigation = <Area>; Action = <Verb>`
- Examples:
  - `Permission : Navigation = Users; Action = View`
  - `Permission : Navigation = All Projects; Action = Add`
  - `Permission : Navigation = License; Action = View`
- Note: Some legacy policies may differ in spacing; tests normalize whitespace when asserting.

### Controller Conventions
- Read-only metadata (e.g., Country, Module, ProjectUnit, NavigationAction):
  - Use class-level `[Authorize]`. Methods typically do not need their own `[Authorize]`.
- Client-specific with CRUD (e.g., Client, ClientUser, ClientProject):
  - Use method-level `[Authorize(Policy = ...)]` on each action.
- LoginController:
  - No class-level `[Authorize]` (endpoint is public). Input validation and business authentication handle security.

### Testing Conventions (xUnit)
- Accept either class-level OR method-level `[Authorize]` in assertions:
  - `Assert.True(classAuthorize != null || methodHasAuthorize)`
- For policy checks, compare normalized strings (remove spaces) to avoid failures due to formatting differences.
- Keep authorization assertions within the controller’s unit test file (not a separate reflection test suite).

### Examples
```csharp
// Class-level auth (metadata)
[Authorize]
public class CountryController : ODataController { /* GET actions only */ }

// Method-level policy (fine-grained)
[HttpPost]
[Authorize(Policy = "Permission : Navigation = Users; Action = Add")]
public async Task<IActionResult> PostAsync([FromBody] ClientUserCreateModel model) { /* ... */ }

// Public login controller
public class LoginController : ODataController { /* no class-level [Authorize] */ }
```

### Implementation Checklist
- Choose class-level `[Authorize]` for simple, read-only controllers.
- Choose method-level policies for CRUD or mixed-permission controllers.
- Keep login public; ensure validation and business flows are robust.
- Document policies alongside actions for clarity.
- Add/maintain unit tests for:
  - Presence of `[Authorize]` (class or method)
  - Correct policy strings on method-protected actions

### File Pointers
- API Rules: `.cursor/rules/KonaAIMasterRuleBook/KonaAIMasterAPI/API-RuleBook.mdc`
- Unit Test Rules: `.cursor/rules/KonaAIMasterRuleBook/KonaAIMasterTests/TestUnit-RuleBook.mdc`
- Coverage Report: `Requriement/AUTHORIZATION-TEST-COVERAGE.md`



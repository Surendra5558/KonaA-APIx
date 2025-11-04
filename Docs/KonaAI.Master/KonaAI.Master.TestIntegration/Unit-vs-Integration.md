## Unit vs Integration Tests

### Purpose
- **Unit tests**: Verify a single unit (function/class) in isolation.
- **Integration tests**: Verify multiple components working together (e.g., API + Business + Repository + DB).

### Scope
- **Unit tests**: Narrow; mock all external dependencies.
- **Integration tests**: Broad; use real or near-real infrastructure (test DB, DI container, middleware).

### Speed and Cost
- **Unit tests**: Fast, cheap, highly deterministic.
- **Integration tests**: Slower, more setup/teardown, higher flakiness risk if unmanaged.

### Dependencies
- **Unit tests**: None beyond the unit under test; use mocks/stubs.
- **Integration tests**: Real services/modules (e.g., EF Core with test DB, WebApplicationFactory for API).

### Failure Signal
- **Unit tests**: Pinpoint exact method/branch failing.
- **Integration tests**: Indicate contract/wiring issues across layers.

### When to Use
- **Unit tests**: Business logic invariants, edge cases, mapping logic (with mocks).
- **Integration tests**: DB queries/projections, auth flows, tenancy filters, API endpoints and middleware behavior.

### Examples in this Solution
- **Unit**: Test `ClientUserCreateValidator` rules; test a Business method with repositories mocked.
- **Integration**: Call `/api/v1/client-users` via `WebApplicationFactory`, validate ProblemDetails, JWT auth, and EF persistence with a test SQL instance or in-memory substitute.

### Tooling Patterns
- **Unit**: xUnit/NUnit + Moq/NSubstitute; no external resources.
- **Integration**: xUnit + `WebApplicationFactory`, real DI, seeded test DB, test configuration.



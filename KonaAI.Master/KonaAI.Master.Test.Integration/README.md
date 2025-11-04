# KonaAI.Master.Test.Integration

## Overview

This project contains comprehensive integration tests for the KonaAI.Master solution with **automatic Docker fallback mechanism**. The tests use real SQL Server via Testcontainers when Docker is available, and automatically fall back to in-memory database for local development without Docker. The tests are organized into clear categories and provide thorough coverage of API endpoints, database operations, multi-tenancy, and performance scenarios.

### 🚀 **Key Features**

- **✅ Docker Fallback**: Automatically uses in-memory database when Docker is not available
- **✅ Real SQL Server**: Uses Testcontainers SQL Server when Docker is available  
- **✅ Local Development**: Works immediately without Docker setup
- **✅ CI/CD Ready**: Uses real SQL Server in CI/CD environments
- **✅ Zero Configuration**: Automatic detection and fallback

## Architecture

### Test Categories

```
KonaAI.Master.Test.Integration/
├── API/                              # Full HTTP pipeline tests
│   ├── Controllers/                  # E2E controller tests via HTTP
│   ├── Authentication/               # Auth flow tests
│   ├── MultiTenancy/                 # Tenant isolation tests
│   ├── OData/                        # OData query tests
│   └── Workflows/                    # E2E user workflows
├── Repository/                       # Database integration tests
│   ├── Master/                       # Master schema tests
│   └── Tenant/                       # Tenant schema tests
├── Business/                         # Business + Repository tests
│   ├── Master/
│   └── Tenant/
├── Performance/                      # Load and performance tests
│   ├── Concurrent/
│   └── LargeDataset/
├── Infrastructure/                   # Test infrastructure
│   ├── Fixtures/
│   ├── Factories/
│   ├── Helpers/
│   └── TestData/
└── Configuration/                    # Test configuration
    ├── appsettings.Test.json
    └── testcontainers.json
```

### Database Strategy

The integration tests use a **smart database fallback mechanism**:

#### 🐳 **With Docker Available (CI/CD)**
- Uses **Testcontainers SQL Server** for production-like testing
- Real SQL Server features (transactions, constraints, triggers)
- Consistent with production environment
- Better for CI/CD pipelines

#### 💾 **Without Docker (Local Development)**
- Automatically falls back to **in-memory database**
- Fast execution for local development
- No Docker setup required
- Immediate test execution

#### 🔄 **Automatic Detection**
```csharp
// Automatically detects Docker availability
try {
    await InitializeWithTestcontainers(); // Try Docker first
    _useTestcontainers = true;
} catch (Exception ex) {
    await InitializeWithInMemory(); // Fall back to in-memory
    _useTestcontainers = false;
}
```

### Additional Features

- **Multi-tenancy Testing**: Comprehensive tenant isolation validation
- **Performance Testing**: Concurrent operations and large dataset handling
- **Data Builders**: Fluent interface for creating test data
- **Clear Categorization**: Organized test structure with attributes
- **Parallel Execution**: Tests can run in parallel for faster CI/CD

## Test Categories & Attributes

### API Integration Tests
- **`[ApiIntegration]`**: Full HTTP pipeline tests
- **`[EndToEnd]`**: Complete user workflow tests
- **`[OData]`**: OData query functionality tests
- **`[Authentication]`**: Authentication and authorization tests

### Repository Integration Tests
- **`[RepositoryIntegration]`**: Database operation tests
- **`[MultiTenancy]`**: Multi-tenant data isolation tests

### Performance Tests
- **`[Performance]`**: Performance and load tests
- **`[Concurrent]`**: Concurrent operation tests
- **`[Slow]`**: Long-running tests

## Usage

### Running Tests

```bash
# Run all integration tests
dotnet test

# Run specific category
dotnet test --filter Category=ApiIntegration
dotnet test --filter Category=RepositoryIntegration
dotnet test --filter Category=Performance

# Run with specific attributes
dotnet test --filter MultiTenancy
dotnet test --filter Concurrent
```

### Test Data Management

```csharp
// Using data builders
var client = ClientBuilder.Create()
    .WithName("Test Client")
    .WithCode("TC001")
    .Active()
    .Build();

// Creating multiple entities
var clients = ClientBuilder.CreateMultiple(10);

// Random data generation
var randomClients = ClientBuilder.CreateRandomMultiple(100);
```

### Database Fixtures

The `SqlServerFixture` automatically handles database selection:

```csharp
[Collection("SqlServerCollection")]
public class MyRepositoryTests : IClassFixture<SqlServerFixture>
{
    private readonly SqlServerFixture _fixture;

    public MyRepositoryTests(SqlServerFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task MyTest()
    {
        // Automatically uses appropriate database
        using var context = _fixture.CreateContext();
        
        // Check which database is being used
        var databaseType = _fixture.GetDatabaseType();
        var isUsingTestcontainers = _fixture.IsUsingTestcontainers;
        
        // Test implementation works with both database types
    }
}
```

#### Database Type Detection

```csharp
// Check which database is being used
var databaseType = _fixture.GetDatabaseType();
// Returns: "SQL Server (Testcontainers)" or "In-Memory Database"

var isUsingTestcontainers = _fixture.IsUsingTestcontainers;
// Returns: true (Docker available) or false (fallback to in-memory)
```

### Web Application Factory

```csharp
[Collection("IntegrationTestCollection")]
public class MyApiTests : IClassFixture<IntegrationTestWebApplicationFactory>
{
    private readonly IntegrationTestWebApplicationFactory _factory;

    public MyApiTests(IntegrationTestWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task MyApiTest()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/api/v1/endpoint");
        // Assertions
    }
}
```

## Test Data Builders

### Available Builders

- **ClientBuilder**: Client entity test data
- **ClientProjectBuilder**: Client project test data
- **CountryBuilder**: Country master data
- **ModuleBuilder**: Module master data
- **RoleTypeBuilder**: Role type master data
- **NavigationActionBuilder**: Navigation action master data
- **ProjectDepartmentBuilder**: Project department master data
- **ProjectUnitBuilder**: Project unit master data
- **ProjectRiskAreaBuilder**: Project risk area master data
- **ProjectAuditResponsibilityBuilder**: Project audit responsibility master data
- **RenderTypeBuilder**: Render type master data
- **ClientUserBuilder**: Client user test data
- **ClientQuestionnaireBuilder**: Client questionnaire test data
- **ClientQuestionBankBuilder**: Client question bank test data

### Builder Patterns

```csharp
// Basic usage
var client = ClientBuilder.Create()
    .WithName("Test Client")
    .WithCode("TC001")
    .Active()
    .Build();

// Multiple entities
var clients = ClientBuilder.CreateMultiple(5);

// Random data
var randomClients = ClientBuilder.CreateRandomMultiple(10);

// Client-specific data
var projects = ClientProjectBuilder.CreateForClient(clientId)
    .CreateMultiple(3);
```

## Multi-Tenancy Testing

### Tenant Isolation Tests

```csharp
[MultiTenancy]
public class ClientIsolationTests
{
    [Fact]
    public async Task GetClients_Client1Context_OnlyReturnsClient1Data()
    {
        // Arrange
        var client1HttpClient = _factory.CreateClientWithUserContext(clientId: 1);
        
        // Act
        var response = await client1HttpClient.GetAsync("/api/v1/endpoint");
        
        // Assert - Should only contain Client 1 data
    }
}
```

### Cross-Tenant Access Prevention

```csharp
[Fact]
public async Task CrossTenantAccess_Client1TriesToAccessClient2Data_ReturnsEmptyOrForbidden()
{
    // Test that clients cannot access each other's data
}
```

## Performance Testing

### Concurrent Operations

```csharp
[Performance]
[Concurrent]
public class ConcurrentOperationsTests
{
    [Fact]
    public async Task ConcurrentReads_MultipleClients_AllSucceed()
    {
        // Test concurrent read operations
    }
    
    [Fact]
    public async Task ConcurrentWrites_DifferentClients_AllSucceed()
    {
        // Test concurrent write operations
    }
}
```

### Large Dataset Testing

```csharp
[Fact]
public async Task GetAsync_PerformanceTest_LargeDataset()
{
    // Test with large datasets for performance validation
}
```

## Configuration

### Test Settings

The `appsettings.Test.json` file contains test-specific configuration:

```json
{
  "TestSettings": {
    "UseInMemoryDatabase": false,
    "UseTestcontainers": true,
    "TestDataSeedCount": 100,
    "PerformanceTestRecordCount": 1000,
    "ConcurrentTestClientCount": 5
  }
}
```

### Database Configuration

- **Testcontainers**: Real SQL Server in Docker containers
- **Connection String**: Automatically generated for each test run
- **Isolation**: Each test gets a fresh database
- **Cleanup**: Automatic cleanup after test completion

## Best Practices

### Test Organization

1. **Use appropriate attributes** for test categorization
2. **Group related tests** in the same class
3. **Use descriptive test names** that explain the scenario
4. **Keep tests focused** on a single concern

### Data Management

1. **Use data builders** for consistent test data
2. **Clean up test data** between tests
3. **Use realistic data** that matches production scenarios
4. **Avoid hardcoded values** in test data

### Performance Considerations

1. **Use parallel execution** where possible
2. **Minimize database operations** in tests
3. **Use appropriate test data sizes** for performance tests
4. **Monitor test execution times**

### Multi-Tenancy Testing

1. **Always test tenant isolation**
2. **Verify cross-tenant access prevention**
3. **Test concurrent multi-tenant operations**
4. **Validate OData queries with tenant filtering**

## Troubleshooting

### Common Issues

1. **Testcontainers not starting**: Ensure Docker is running
2. **Database connection issues**: Check Testcontainers configuration
3. **Slow test execution**: Consider using parallel execution
4. **Data isolation issues**: Verify proper cleanup between tests

### Debugging

1. **Enable sensitive data logging** in test configuration
2. **Use breakpoints** in test methods
3. **Check database state** during test execution
4. **Review test output** for error messages

## CI/CD Integration

### GitHub Actions

The tests are designed to work with GitHub Actions and automatically use the appropriate database:

```yaml
- name: Run Integration Tests
  run: dotnet test --filter Category=Integration
```

### Database Requirements

#### 🐳 **CI/CD Environment (With Docker)**
- Docker must be available for Testcontainers
- SQL Server container images will be pulled automatically
- Tests run in isolated containers
- Uses real SQL Server for production-like testing

#### 💻 **Local Development (Without Docker)**
- No Docker setup required
- Automatically falls back to in-memory database
- Fast test execution
- Immediate development workflow

### Docker Detection

The system automatically detects Docker availability:

```csharp
// Static method to check Docker availability
var isDockerAvailable = SqlServerFixture.IsDockerAvailable();
// Returns: true (Docker available) or false (Docker not available)
```

## 📚 **Documentation**

### Comprehensive Guides

- **Integration Test Architecture**: `INTEGRATION_TEST_ARCHITECTURE.md` - Complete architecture overview
- **Test Data Management**: `TEST_DATA_MANAGEMENT.md` - Data builder patterns and seeding strategies
- **Performance Testing**: `PERFORMANCE_TESTING_GUIDE.md` - Performance, load, and stress testing
- **Docker CI/CD Guide**: `DOCKER_CI_CD_GUIDE.md` - Docker integration with CI/CD pipelines

### Quick Reference

- **Database Fallback**: Automatic Docker detection with in-memory fallback
- **Test Categories**: Clear categorization with attributes
- **Data Builders**: Fluent interface for test data creation
- **Performance Testing**: Comprehensive performance testing strategies

### Documentation Structure

```
KonaAI.Master.Test.Integration/
├── README.md                           # Main documentation
├── INTEGRATION_TEST_ARCHITECTURE.md    # Architecture overview
├── TEST_DATA_MANAGEMENT.md            # Data management guide
├── PERFORMANCE_TESTING_GUIDE.md       # Performance testing guide
├── DOCKER_CI_CD_GUIDE.md              # Docker CI/CD guide
└── Infrastructure/                     # Test infrastructure
    ├── Fixtures/                      # Database fixtures
    ├── Factories/                     # Test factories
    ├── Helpers/                       # Test helpers
    └── TestData/                      # Test data management
```

## Contributing

### Adding New Tests

1. **Choose appropriate category** (API, Repository, Business, Performance)
2. **Use existing patterns** for test structure
3. **Add proper attributes** for categorization
4. **Follow naming conventions** for test methods
5. **Update documentation** if adding new test categories

### Adding New Data Builders

1. **Follow the builder pattern** established by existing builders
2. **Provide fluent interface** methods
3. **Include default data creation** methods
4. **Support multiple entity creation**
5. **Add to the seeder** if needed for test data

### Performance Testing

1. **Use appropriate test attributes** (`[Performance]`, `[Concurrent]`)
2. **Set realistic performance expectations**
3. **Test with various data sizes**
4. **Monitor resource usage** during tests
5. **Document performance requirements**

### Documentation Updates

1. **Update architecture docs** when adding new test categories
2. **Update data management docs** when adding new builders
3. **Update performance docs** when adding new performance tests
4. **Update CI/CD docs** when changing Docker requirements

## 🎯 **Key Features Summary**

### ✅ **Automatic Database Selection**
- **CI/CD Environment**: Uses Testcontainers SQL Server for production-like testing
- **Local Development**: Automatically falls back to in-memory database
- **Zero Configuration**: No manual setup required

### ✅ **Comprehensive Test Coverage**
- **API Integration**: Full HTTP pipeline testing
- **Repository Integration**: Database operation testing
- **Business Integration**: Business logic with real repositories
- **Performance Testing**: Load, stress, and endurance testing

### ✅ **Multi-Tenancy Support**
- **Tenant Isolation**: Comprehensive tenant isolation validation
- **Cross-Tenant Prevention**: Security testing for data access
- **Concurrent Multi-Tenant**: Multi-tenant concurrent operations

### ✅ **Performance Testing**
- **Concurrent Operations**: Concurrent read/write testing
- **Large Dataset Testing**: Performance with large datasets
- **Load Testing**: Basic load and stress testing
- **Benchmarking**: Performance benchmarking and monitoring

### ✅ **Developer Experience**
- **Fast Local Development**: In-memory database for quick iteration
- **CI/CD Ready**: Real SQL Server for production-like testing
- **Clear Documentation**: Comprehensive guides and examples
- **Easy Maintenance**: Clear test structure and patterns

### ✅ **Documentation & Architecture**
- **Comprehensive Guides**: Detailed documentation for all aspects
- **Clear Architecture**: Well-structured test organization
- **Best Practices**: Established patterns and conventions
- **Easy Onboarding**: Clear examples and tutorials

## 🚀 **Getting Started**

### Quick Start

1. **Clone the repository** and navigate to the integration test project
2. **Run tests** - they will automatically detect Docker availability
3. **Review documentation** - comprehensive guides for all aspects
4. **Follow patterns** - established patterns for adding new tests

### Development Workflow

1. **Local Development**: Tests automatically use in-memory database
2. **CI/CD Pipeline**: Tests automatically use Testcontainers SQL Server
3. **Performance Testing**: Use appropriate attributes and patterns
4. **Documentation**: Update relevant guides when adding new features

### Documentation Navigation

- **Start Here**: `README.md` - Main documentation and overview
- **Architecture**: `INTEGRATION_TEST_ARCHITECTURE.md` - Complete architecture details
- **Data Management**: `TEST_DATA_MANAGEMENT.md` - Data builder patterns and seeding
- **Performance**: `PERFORMANCE_TESTING_GUIDE.md` - Performance testing strategies
- **CI/CD**: `DOCKER_CI_CD_GUIDE.md` - Docker integration with CI/CD

## 🎯 **Summary**

This integration test infrastructure provides comprehensive coverage of the KonaAI.Master solution with:

- **Real Database Testing**: Testcontainers SQL Server for production-like testing
- **Automatic Fallback**: In-memory database for local development without Docker
- **Multi-Tenancy Validation**: Comprehensive tenant isolation testing
- **Performance Testing**: Load, stress, and endurance testing capabilities
- **Clear Documentation**: Comprehensive guides and examples
- **Easy Maintenance**: Well-structured test organization and patterns

The infrastructure automatically adapts to your environment, providing the best testing experience whether you're developing locally or running in CI/CD pipelines.

---

**For detailed information on specific aspects, refer to the comprehensive guides:**
- **Architecture**: `INTEGRATION_TEST_ARCHITECTURE.md`
- **Data Management**: `TEST_DATA_MANAGEMENT.md`
- **Performance Testing**: `PERFORMANCE_TESTING_GUIDE.md`
- **Docker CI/CD**: `DOCKER_CI_CD_GUIDE.md`

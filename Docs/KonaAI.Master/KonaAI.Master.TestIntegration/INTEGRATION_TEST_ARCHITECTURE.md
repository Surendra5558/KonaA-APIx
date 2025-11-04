# Integration Test Architecture (complements Complete Guide)

Note: This document focuses on high-level architecture. For full implementation details (examples, migration, config), refer to `INTEGRATION_TEST_REDESIGN_COMPLETE_GUIDE.md`. Overlapping sections are intentionally minimized here to avoid duplication.

## 🏗️ **Architecture Overview**

The KonaAI.Master integration test infrastructure provides comprehensive testing capabilities with **automatic Docker fallback mechanism** for both CI/CD and local development environments.

## 🎯 **Design Principles**

### 1. **Automatic Database Selection**
- **CI/CD Environment**: Uses Testcontainers SQL Server for production-like testing
- **Local Development**: Automatically falls back to in-memory database
- **Zero Configuration**: No manual setup required

### 2. **Clear Test Categorization**
- **API Tests**: Full HTTP pipeline testing
- **Repository Tests**: Database integration testing
- **Business Tests**: Business logic with real repositories
- **Performance Tests**: Load and concurrent operation testing

### 3. **Comprehensive Coverage**
- **Multi-tenancy**: Tenant isolation validation
- **Authentication**: Security flow testing
- **OData Queries**: Query functionality testing
- **Error Handling**: Exception scenario testing

## 🏛️ **Architecture Components**

### Core Infrastructure

```
KonaAI.Master.Test.Integration/
├── Infrastructure/                   # Test infrastructure
│   ├── Fixtures/                    # Database and test fixtures
│   │   ├── SqlServerFixture.cs     # Main database fixture with fallback
│   │   └── DatabaseFallbackTests.cs # Fallback mechanism tests
│   ├── Factories/                   # Test application factories
│   │   └── IntegrationTestWebApplicationFactory.cs
│   ├── Helpers/                     # Test helper utilities
│   │   └── TestHelpers.cs
│   └── TestData/                    # Test data management
│       ├── Builders/                # Data builder pattern
│       └── Seeders/                 # Data seeding utilities
├── API/                             # API integration tests
├── Repository/                      # Repository integration tests
├── Business/                        # Business integration tests
└── Performance/                    # Performance and load tests
```

### Database Strategy

#### 🐳 **Testcontainers SQL Server (CI/CD)**
```csharp
public class SqlServerFixture : IAsyncLifetime
{
    private MsSqlContainer? _container;
    private bool _useTestcontainers = false;

    public async Task InitializeAsync()
    {
        try
        {
            await InitializeWithTestcontainers();
            _useTestcontainers = true;
        }
        catch (Exception ex)
        {
            await InitializeWithInMemory();
            _useTestcontainers = false;
        }
    }
}
```

#### 💾 **In-Memory Database (Local Development)**
```csharp
private async Task InitializeWithInMemory()
{
    ConnectionString = "Data Source=:memory:";
    DatabaseName = "KonaAI_Test_InMemory_" + Guid.NewGuid().ToString("N")[..8];
    Console.WriteLine("Using in-memory database for local development.");
}
```

## 🔧 **Key Components**

### 1. **SqlServerFixture**

**Purpose**: Main database fixture with automatic fallback mechanism

**Features**:
- Automatic Docker detection
- Fallback to in-memory database
- Database type reporting
- Context creation for both database types

**Usage**:
```csharp
[Collection("SqlServerCollection")]
public class MyTests : IClassFixture<SqlServerFixture>
{
    private readonly SqlServerFixture _fixture;

    [Fact]
    public async Task MyTest()
    {
        using var context = _fixture.CreateContext();
        // Works with both SQL Server and in-memory database
    }
}
```

### 2. **IntegrationTestWebApplicationFactory**

**Purpose**: Web application factory for API testing

**Features**:
- Real HTTP pipeline testing
- Database fallback support
- Authentication configuration
- Multi-tenant client creation

**Usage**:
```csharp
[Collection("IntegrationTestCollection")]
public class MyApiTests : IClassFixture<IntegrationTestWebApplicationFactory>
{
    private readonly IntegrationTestWebApplicationFactory _factory;

    [Fact]
    public async Task MyApiTest()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/api/v1/endpoint");
    }
}
```

### 3. **Data Builders**

**Purpose**: Fluent interface for creating test data

**Features**:
- Consistent test data creation
- Multiple entity support
- Random data generation
- Client-specific data creation

**Usage**:
```csharp
var client = ClientBuilder.Create()
    .WithName("Test Client")
    .WithCode("TC001")
    .Active()
    .Build();

var clients = ClientBuilder.CreateMultiple(10);
var projects = ClientProjectBuilder.CreateForClient(clientId)
    .CreateMultiple(3);
```

### 4. **Test Categories & Attributes**

**Purpose**: Clear test categorization and filtering

**Categories**:
- `[ApiIntegration]`: API endpoint tests
- `[RepositoryIntegration]`: Database operation tests
- `[BusinessIntegration]`: Business logic tests
- `[Performance]`: Performance and load tests
- `[MultiTenancy]`: Multi-tenant isolation tests
- `[Authentication]`: Security flow tests
- `[OData]`: Query functionality tests

## 🧪 **Test Categories**

### API Integration Tests

**Purpose**: Full HTTP pipeline testing with real middleware

**Features**:
- Real HTTP request/response cycle
- Authentication and authorization
- Error handling and validation
- OData query functionality

**Example**:
```csharp
[ApiIntegration]
[Collection("IntegrationTestCollection")]
public class ClientControllerApiTests
{
    [Fact]
    public async Task GetAsync_ReturnsOk_WithClientData()
    {
        var response = await _client.GetAsync("/api/v1/Master/App/Client");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
```

### Repository Integration Tests

**Purpose**: Database operation testing with real SQL Server

**Features**:
- EF Core mapping validation
- Transaction handling
- Multi-tenancy filters
- Query performance testing

**Example**:
```csharp
[RepositoryIntegration]
[Collection("SqlServerCollection")]
public class ClientRepositoryTests
{
    [Fact]
    public async Task AddAsync_ValidClient_AddsToDatabase()
    {
        var client = ClientBuilder.Create().Build();
        context.Add(client);
        await context.SaveChangesAsync();
        
        var savedClient = await context.Set<Client>()
            .FirstOrDefaultAsync(c => c.RowId == client.RowId);
        savedClient.Should().NotBeNull();
    }
}
```

### Business Integration Tests

**Purpose**: Business logic testing with real repositories

**Features**:
- Business rules validation
- Transaction boundaries
- Cross-aggregate operations
- Complex business scenarios

**Example**:
```csharp
[BusinessIntegration]
[Collection("SqlServerCollection")]
public class ClientBusinessIntegrationTests
{
    [Fact]
    public async Task CreateAsync_ValidModel_ReturnsSuccess()
    {
        var createModel = new ClientCreateModel { /* ... */ };
        var result = await business.CreateAsync(createModel);
        result.Should().Be(1);
    }
}
```

### Performance Tests

**Purpose**: Performance and load testing

**Features**:
- Concurrent operation testing
- Large dataset handling
- Performance benchmarking
- Resource usage monitoring

**Example**:
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
}
```

## 🔄 **Database Fallback Mechanism**

### Automatic Detection

```csharp
public static bool IsDockerAvailable()
{
    try
    {
        var process = new System.Diagnostics.Process
        {
            StartInfo = new System.Diagnostics.ProcessStartInfo
            {
                FileName = "docker",
                Arguments = "version",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };
        
        process.Start();
        process.WaitForExit(5000);
        return process.ExitCode == 0;
    }
    catch
    {
        return false;
    }
}
```

### Fallback Logic

```csharp
public async Task InitializeAsync()
{
    try
    {
        // Try Testcontainers first
        await InitializeWithTestcontainers();
        _useTestcontainers = true;
    }
    catch (Exception ex)
    {
        // Fall back to in-memory database
        Console.WriteLine($"Testcontainers not available: {ex.Message}");
        Console.WriteLine("Falling back to in-memory database for local development.");
        await InitializeWithInMemory();
        _useTestcontainers = false;
    }
}
```

### Context Creation

```csharp
public DefaultContext CreateContext()
{
    var optionsBuilder = new DbContextOptionsBuilder<DefaultContext>();
    
    if (_useTestcontainers)
    {
        // Use real SQL Server
        optionsBuilder.UseSqlServer(ConnectionString, sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(3);
            sqlOptions.CommandTimeout(30);
        });
    }
    else
    {
        // Use in-memory database
        optionsBuilder.UseInMemoryDatabase(DatabaseName);
    }
    
    optionsBuilder.EnableSensitiveDataLogging()
                  .EnableServiceProviderCaching(false);

    var userContextService = new TestUserContextService();
    return new DefaultContext(optionsBuilder.Options, userContextService);
}
```

## 🎯 **Multi-Tenancy Testing**

### Tenant Isolation

```csharp
[MultiTenancy]
public class ClientIsolationTests
{
    [Fact]
    public async Task GetClients_Client1Context_OnlyReturnsClient1Data()
    {
        var client1HttpClient = _factory.CreateClientWithUserContext(clientId: 1);
        var response = await client1HttpClient.GetAsync("/api/v1/endpoint");
        
        // Should only contain Client 1 data
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

## 📊 **Performance Testing**

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

## 🔧 **Configuration**

### Test Settings

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

### Environment Variables

```bash
# Force in-memory database (skip Docker detection)
export TESTCONTAINERS_DISABLE=true

# Force Testcontainers (fail if Docker not available)
export TESTCONTAINERS_REQUIRE_DOCKER=true
```

## 🚀 **Benefits**

### For CI/CD
- **Production-like Testing**: Real SQL Server with Testcontainers
- **Consistent Environment**: Same database as production
- **Comprehensive Coverage**: All database features tested
- **Reliable Results**: Real database behavior

### For Local Development
- **Fast Setup**: No Docker installation required
- **Fast Execution**: In-memory database is very fast
- **Zero Configuration**: Works immediately
- **Developer Friendly**: No complex setup

### For Both Environments
- **Automatic Selection**: No manual configuration needed
- **Clear Categorization**: Easy test organization
- **Comprehensive Coverage**: All layers tested
- **Maintainable**: Clear test structure

## 🔍 **Debugging**

### Database Type Detection

```csharp
[Fact]
public void DatabaseType_ShouldBeDetectedCorrectly()
{
    var databaseType = _fixture.GetDatabaseType();
    var isUsingTestcontainers = _fixture.IsUsingTestcontainers;
    
    Console.WriteLine($"Using database type: {databaseType}");
    Console.WriteLine($"Is using Testcontainers: {isUsingTestcontainers}");
}
```

### Docker Availability Check

```csharp
[Fact]
public void DockerAvailability_ShouldBeDetectedCorrectly()
{
    var isDockerAvailable = SqlServerFixture.IsDockerAvailable();
    Console.WriteLine($"Docker available: {isDockerAvailable}");
}
```

## 📈 **Performance Considerations**

### CI/CD Environment
- **Real SQL Server**: Production-like testing
- **Container Overhead**: Slightly slower startup
- **Better Coverage**: Real database features tested

### Local Development
- **In-Memory Database**: Very fast execution
- **No Container Overhead**: Immediate startup
- **Fast Iteration**: Quick test cycles

## 🎯 **Best Practices**

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

---

**This integration test architecture provides comprehensive testing capabilities with automatic database selection, clear categorization, and support for both CI/CD and local development environments.**

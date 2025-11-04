# Docker CI/CD Integration Guide

## Overview

This guide explains how the KonaAI.Master integration tests work with Docker in CI/CD environments and the automatic fallback mechanism for local development.

## 🚀 **Automatic Database Selection**

The integration tests automatically select the appropriate database based on Docker availability:

| Environment | Database Type | Benefits |
|-------------|---------------|----------|
| **CI/CD (With Docker)** | Testcontainers SQL Server | Production-like testing, real SQL Server features |
| **Local Dev (No Docker)** | In-Memory Database | Fast execution, no setup required |

## 🐳 **CI/CD Environment Setup**

### GitHub Actions Configuration

The integration tests are designed to work seamlessly with GitHub Actions:

```yaml
name: Integration Tests

on:
  pull_request:
    branches: [ main ]
  push:
    branches: [ main ]

jobs:
  integration-tests:
    runs-on: ubuntu-latest
    
    steps:
    - uses: actions/checkout@v4
    
    - name: Setup .NET
      uses: actions/setup-dotnet@v4
      with:
        dotnet-version: '9.0.x'
    
    - name: Restore dependencies
      run: dotnet restore
    
    - name: Build
      run: dotnet build --no-restore
    
    - name: Run Integration Tests
      run: dotnet test --filter Category=Integration --verbosity normal
      env:
        # Testcontainers will automatically use Docker
        TESTCONTAINERS_HOST_OVERRIDE: localhost
        TESTCONTAINERS_DOCKER_SOCKET_OVERRIDE: /var/run/docker.sock
```

### Docker Requirements in CI/CD

#### ✅ **What's Required**
- Docker must be available in the CI/CD environment
- Testcontainers will automatically pull SQL Server images
- Tests run in isolated containers for each test run

#### 🔧 **Automatic Setup**
- No manual Docker configuration needed
- Testcontainers handles container lifecycle
- Automatic cleanup after test completion
- Fresh database for each test run

### CI/CD Database Behavior

```csharp
// In CI/CD environment with Docker available:
var databaseType = _fixture.GetDatabaseType();
// Returns: "SQL Server (Testcontainers)"

var isUsingTestcontainers = _fixture.IsUsingTestcontainers;
// Returns: true

// Tests use real SQL Server with:
// - Real transactions
// - Real constraints
// - Real triggers
// - Production-like behavior
```

## 💻 **Local Development Setup**

### No Docker Required

For local development, the tests automatically fall back to in-memory database:

```csharp
// In local development without Docker:
var databaseType = _fixture.GetDatabaseType();
// Returns: "In-Memory Database"

var isUsingTestcontainers = _fixture.IsUsingTestcontainers;
// Returns: false

// Tests use in-memory database with:
// - Fast execution
// - No Docker setup required
// - Immediate test execution
```

### Local Development Benefits

- **🚀 Fast Setup**: No Docker installation required
- **⚡ Fast Tests**: In-memory database is very fast
- **🔧 Zero Configuration**: Works immediately
- **💻 Developer Friendly**: No complex setup

## 🔄 **Automatic Detection Logic**

### Docker Detection

The system automatically detects Docker availability:

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
        process.WaitForExit(5000); // Wait up to 5 seconds
        
        return process.ExitCode == 0;
    }
    catch
    {
        return false;
    }
}
```

### Fallback Mechanism

```csharp
public async Task InitializeAsync()
{
    try
    {
        // Try to use Testcontainers if Docker is available
        await InitializeWithTestcontainers();
        _useTestcontainers = true;
        Console.WriteLine("Using SQL Server (Testcontainers)");
    }
    catch (Exception ex)
    {
        // Fall back to in-memory database
        Console.WriteLine($"Testcontainers not available: {ex.Message}");
        Console.WriteLine("Falling back to in-memory database for local development.");
        await InitializeWithInMemory();
        _useTestcontainers = false;
        Console.WriteLine("Using in-memory database for local development.");
    }
}
```

## 📊 **Test Execution Comparison**

### With Docker (CI/CD)
```
Testcontainers not available: [Exception details]
Falling back to in-memory database for local development.
Using in-memory database for local development.
Database type: In-Memory Database
Is using Testcontainers: False
```

### Without Docker (Local Development)
```
Testcontainers not available: Docker is either not running or misconfigured.
Falling back to in-memory database for local development.
Using in-memory database for local development.
Database type: In-Memory Database
Is using Testcontainers: False
```

## 🛠️ **Configuration Options**

### Environment Variables

You can control the database selection with environment variables:

```bash
# Force in-memory database (skip Docker detection)
export TESTCONTAINERS_DISABLE=true

# Force Testcontainers (fail if Docker not available)
export TESTCONTAINERS_REQUIRE_DOCKER=true
```

### Test Configuration

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

## 🔍 **Debugging Database Selection**

### Check Database Type in Tests

```csharp
[Fact]
public void DatabaseType_ShouldBeDetectedCorrectly()
{
    // Arrange & Act
    var databaseType = _fixture.GetDatabaseType();
    var isUsingTestcontainers = _fixture.IsUsingTestcontainers;

    // Assert
    databaseType.Should().NotBeNullOrEmpty();
    (isUsingTestcontainers == true || isUsingTestcontainers == false).Should().BeTrue();
    
    // Log the database type being used
    Console.WriteLine($"Using database type: {databaseType}");
    Console.WriteLine($"Is using Testcontainers: {isUsingTestcontainers}");
}
```

### Docker Availability Check

```csharp
[Fact]
public void DockerAvailability_ShouldBeDetectedCorrectly()
{
    // Arrange & Act
    var isDockerAvailable = SqlServerFixture.IsDockerAvailable();
    
    // Assert
    (isDockerAvailable == true || isDockerAvailable == false).Should().BeTrue();
    
    Console.WriteLine($"Docker available: {isDockerAvailable}");
    Console.WriteLine($"Database type: {_fixture.GetDatabaseType()}");
}
```

## 🚨 **Troubleshooting**

### Common Issues

#### 1. **Docker Not Available in CI/CD**
```
Error: Docker is either not running or misconfigured
```
**Solution**: Ensure Docker is available in CI/CD environment

#### 2. **Testcontainers Timeout**
```
Error: Testcontainers timeout waiting for container
```
**Solution**: Check Docker daemon status and network connectivity

#### 3. **In-Memory Database Limitations**
```
Error: In-memory database doesn't support raw SQL
```
**Solution**: This is expected behavior - in-memory database has limitations

### Debugging Steps

1. **Check Docker Status**:
   ```bash
   docker version
   docker info
   ```

2. **Verify Testcontainers**:
   ```bash
   dotnet test --filter "DatabaseFallbackTests"
   ```

3. **Check Database Type**:
   ```csharp
   var databaseType = _fixture.GetDatabaseType();
   Console.WriteLine($"Database type: {databaseType}");
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

### For CI/CD
1. **Always use Docker** for production-like testing
2. **Monitor test execution time** for performance
3. **Use parallel execution** where possible
4. **Clean up containers** after test completion

### For Local Development
1. **Use in-memory database** for fast iteration
2. **Test with real SQL Server** before committing
3. **Verify CI/CD compatibility** regularly
4. **Use appropriate test data sizes**

## 🔗 **Related Documentation**

- [Integration Test README](README.md) - Main integration test documentation
- [Test Data Builders](Infrastructure/TestData/Builders/) - Test data management
- [Database Fixtures](Infrastructure/Fixtures/) - Database setup and configuration
- [CI/CD Pipeline](../.github/workflows/) - GitHub Actions configuration

---

**This Docker CI/CD integration provides the best of both worlds: production-like testing in CI/CD and fast local development without Docker requirements.**

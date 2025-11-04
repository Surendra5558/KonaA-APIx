# Test Data Management Guide

## 🎯 **Overview**

This guide covers comprehensive test data management for the KonaAI.Master integration tests, including data builders, seeders, and best practices for creating realistic and maintainable test data.

## 🏗️ **Data Builder Pattern**

### Core Principles

1. **Fluent Interface**: Chainable methods for easy data creation
2. **Default Values**: Sensible defaults for all properties
3. **Realistic Data**: Data that matches production scenarios
4. **Reusability**: Builders can be reused across tests
5. **Consistency**: Same data structure across all tests

### Base Builder Structure

```csharp
public class ClientBuilder
{
    private Client _client = new();

    public static ClientBuilder Create() => new();

    public ClientBuilder WithName(string name)
    {
        _client.Name = name;
        return this;
    }

    public ClientBuilder WithCode(string code)
    {
        _client.Code = code;
        return this;
    }

    public ClientBuilder Active()
    {
        _client.IsActive = true;
        return this;
    }

    public ClientBuilder Inactive()
    {
        _client.IsActive = false;
        return this;
    }

    public Client Build()
    {
        return _client;
    }
}
```

## 📊 **Available Data Builders**

### Master Data Builders

#### ClientBuilder
```csharp
var client = ClientBuilder.Create()
    .WithName("Test Client")
    .WithCode("TC001")
    .WithDisplayName("Test Client Display")
    .Active()
    .Build();

// Multiple clients
var clients = ClientBuilder.CreateMultiple(10);

// Random data
var randomClients = ClientBuilder.CreateRandomMultiple(100);
```

#### CountryBuilder
```csharp
var country = CountryBuilder.Create()
    .WithName("United States")
    .WithCode("US")
    .WithIsoCode("USA")
    .Active()
    .Build();
```

#### ModuleBuilder
```csharp
var module = ModuleBuilder.Create()
    .WithName("User Management")
    .WithCode("UM")
    .WithDescription("User management module")
    .Active()
    .Build();
```

#### RoleTypeBuilder
```csharp
var roleType = RoleTypeBuilder.Create()
    .WithName("Administrator")
    .WithCode("ADMIN")
    .WithDescription("System administrator role")
    .Active()
    .Build();
```

### Tenant Data Builders

#### ClientProjectBuilder
```csharp
var project = ClientProjectBuilder.CreateForClient(clientId)
    .WithName("Test Project")
    .WithCode("TP001")
    .WithDescription("Test project description")
    .Active()
    .Build();

// Multiple projects for a client
var projects = ClientProjectBuilder.CreateForClient(clientId)
    .CreateMultiple(5);
```

#### ClientUserBuilder
```csharp
var user = ClientUserBuilder.CreateForClient(clientId)
    .WithName("Test User")
    .WithEmail("test@example.com")
    .WithRole("User")
    .Active()
    .Build();
```

#### ClientQuestionnaireBuilder
```csharp
var questionnaire = ClientQuestionnaireBuilder.CreateForClient(clientId)
    .WithName("Test Questionnaire")
    .WithDescription("Test questionnaire description")
    .Active()
    .Build();
```

## 🌱 **Data Seeding**

### TestDataSeeder

The `TestDataSeeder` class provides comprehensive data seeding for integration tests:

```csharp
public class TestDataSeeder
{
    public async Task SeedAsync(DefaultContext context)
    {
        // Seed master data
        await SeedMasterDataAsync(context);
        
        // Seed tenant data
        await SeedTenantDataAsync(context);
        
        // Seed relationships
        await SeedRelationshipsAsync(context);
    }

    private async Task SeedMasterDataAsync(DefaultContext context)
    {
        // Countries
        var countries = CountryBuilder.CreateMultiple(10);
        context.AddRange(countries);

        // Modules
        var modules = ModuleBuilder.CreateMultiple(5);
        context.AddRange(modules);

        // Role types
        var roleTypes = RoleTypeBuilder.CreateMultiple(3);
        context.AddRange(roleTypes);

        await context.SaveChangesAsync();
    }

    private async Task SeedTenantDataAsync(DefaultContext context)
    {
        // Clients
        var clients = ClientBuilder.CreateMultiple(3);
        context.AddRange(clients);
        await context.SaveChangesAsync();

        // Client projects
        foreach (var client in clients)
        {
            var projects = ClientProjectBuilder.CreateForClient(client.RowId)
                .CreateMultiple(5);
            context.AddRange(projects);
        }

        await context.SaveChangesAsync();
    }
}
```

### Seeding Strategies

#### 1. **Minimal Seeding**
```csharp
// Only essential data for basic tests
var client = ClientBuilder.Create().Build();
context.Add(client);
await context.SaveChangesAsync();
```

#### 2. **Comprehensive Seeding**
```csharp
// Full dataset for complex scenarios
await _fixture.SeedTestDataAsync();
```

#### 3. **Custom Seeding**
```csharp
// Specific data for particular tests
var specificClient = ClientBuilder.Create()
    .WithName("Specific Test Client")
    .WithCode("STC001")
    .Build();
context.Add(specificClient);
await context.SaveChangesAsync();
```

## 🔄 **Data Management Patterns**

### 1. **Builder Pattern**

```csharp
// Basic usage
var client = ClientBuilder.Create()
    .WithName("Test Client")
    .WithCode("TC001")
    .Active()
    .Build();

// Fluent chaining
var client = ClientBuilder.Create()
    .WithName("Test Client")
    .WithCode("TC001")
    .WithDisplayName("Test Client Display")
    .WithDescription("Test client description")
    .Active()
    .Build();
```

### 2. **Multiple Entity Creation**

```csharp
// Create multiple entities
var clients = ClientBuilder.CreateMultiple(10);

// Create multiple with specific properties
var clients = ClientBuilder.CreateMultiple(10, builder => 
    builder.WithName("Test Client")
           .WithCode("TC")
           .Active());

// Random data generation
var randomClients = ClientBuilder.CreateRandomMultiple(100);
```

### 3. **Relationship Management**

```csharp
// Client with projects
var client = ClientBuilder.Create().Build();
var projects = ClientProjectBuilder.CreateForClient(client.RowId)
    .CreateMultiple(5);

// User with specific client
var user = ClientUserBuilder.CreateForClient(client.RowId)
    .WithName("Test User")
    .WithEmail("test@example.com")
    .Build();
```

## 🎯 **Test Data Categories**

### 1. **Master Data**
- **Countries**: Geographic reference data
- **Modules**: System module definitions
- **Role Types**: User role definitions
- **Navigation Actions**: UI navigation definitions

### 2. **Tenant Data**
- **Clients**: Multi-tenant client entities
- **Client Projects**: Client-specific projects
- **Client Users**: Client-specific users
- **Client Questionnaires**: Client-specific questionnaires

### 3. **Relationship Data**
- **Client-Project relationships**
- **User-Client relationships**
- **Questionnaire-Client relationships**
- **Cross-tenant data isolation**

## 🔧 **Data Builder Implementation**

### Base Builder Class

```csharp
public abstract class BaseBuilder<T> where T : class, new()
{
    protected T _entity = new();

    public static TBuilder Create<TBuilder>() where TBuilder : BaseBuilder<T>, new()
    {
        return new TBuilder();
    }

    public TBuilder WithProperty<TValue>(Expression<Func<T, TValue>> property, TValue value)
    {
        var propertyInfo = GetPropertyInfo(property);
        propertyInfo.SetValue(_entity, value);
        return (TBuilder)this;
    }

    public T Build()
    {
        return _entity;
    }

    public List<T> CreateMultiple(int count)
    {
        var entities = new List<T>();
        for (int i = 0; i < count; i++)
        {
            entities.Add(Build());
        }
        return entities;
    }
}
```

### Specific Builder Implementation

```csharp
public class ClientBuilder : BaseBuilder<Client>
{
    public ClientBuilder WithName(string name)
    {
        _entity.Name = name;
        return this;
    }

    public ClientBuilder WithCode(string code)
    {
        _entity.Code = code;
        return this;
    }

    public ClientBuilder WithDisplayName(string displayName)
    {
        _entity.DisplayName = displayName;
        return this;
    }

    public ClientBuilder Active()
    {
        _entity.IsActive = true;
        return this;
    }

    public ClientBuilder Inactive()
    {
        _entity.IsActive = false;
        return this;
    }

    public static List<Client> CreateMultiple(int count)
    {
        var clients = new List<Client>();
        for (int i = 0; i < count; i++)
        {
            clients.Add(Create()
                .WithName($"Test Client {i + 1}")
                .WithCode($"TC{i + 1:D3}")
                .Active()
                .Build());
        }
        return clients;
    }
}
```

## 🎲 **Random Data Generation**

### Using Bogus Library

```csharp
public class ClientBuilder : BaseBuilder<Client>
{
    private static readonly Faker<Client> _faker = new Faker<Client>()
        .RuleFor(c => c.Name, f => f.Company.CompanyName())
        .RuleFor(c => c.Code, f => f.Random.AlphaNumeric(6).ToUpper())
        .RuleFor(c => c.DisplayName, f => f.Company.CompanyName())
        .RuleFor(c => c.Description, f => f.Lorem.Sentence())
        .RuleFor(c => c.IsActive, f => f.Random.Bool(0.8f)) // 80% active
        .RuleFor(c => c.CreatedOn, f => f.Date.Past(2))
        .RuleFor(c => c.UpdatedOn, f => f.Date.Recent());

    public static List<Client> CreateRandomMultiple(int count)
    {
        return _faker.Generate(count);
    }
}
```

### Custom Random Data

```csharp
public class ClientBuilder : BaseBuilder<Client>
{
    public ClientBuilder WithRandomData()
    {
        var faker = new Faker();
        _entity.Name = faker.Company.CompanyName();
        _entity.Code = faker.Random.AlphaNumeric(6).ToUpper();
        _entity.DisplayName = faker.Company.CompanyName();
        _entity.Description = faker.Lorem.Sentence();
        _entity.IsActive = faker.Random.Bool(0.8f);
        return this;
    }
}
```

## 🧹 **Data Cleanup**

### Automatic Cleanup

```csharp
[Collection("SqlServerCollection")]
public class MyTests : IClassFixture<SqlServerFixture>
{
    private readonly SqlServerFixture _fixture;

    [SetUp]
    public async Task Setup()
    {
        await _fixture.ClearDatabaseAsync();
        await _fixture.SeedTestDataAsync();
    }

    [TearDown]
    public async Task Cleanup()
    {
        await _fixture.ClearDatabaseAsync();
    }
}
```

### Manual Cleanup

```csharp
[Fact]
public async Task MyTest()
{
    // Arrange
    await _fixture.ClearDatabaseAsync();
    var client = ClientBuilder.Create().Build();
    
    // Act
    using var context = _fixture.CreateContext();
    context.Add(client);
    await context.SaveChangesAsync();
    
    // Assert
    var savedClient = await context.Set<Client>()
        .FirstOrDefaultAsync(c => c.RowId == client.RowId);
    savedClient.Should().NotBeNull();
    
    // Cleanup
    await _fixture.ClearDatabaseAsync();
}
```

## 🎯 **Best Practices**

### 1. **Data Consistency**
- Use consistent naming conventions
- Maintain referential integrity
- Use realistic data values
- Avoid hardcoded values

### 2. **Performance Optimization**
- Use bulk operations for large datasets
- Minimize database round trips
- Use appropriate data sizes
- Clean up test data efficiently

### 3. **Test Isolation**
- Each test should have clean data
- Avoid data pollution between tests
- Use unique identifiers
- Clear data between tests

### 4. **Maintainability**
- Use builder patterns consistently
- Provide sensible defaults
- Make builders reusable
- Document data relationships

## 🔍 **Debugging Test Data**

### Data Inspection

```csharp
[Fact]
public async Task DebugTestData()
{
    using var context = _fixture.CreateContext();
    
    // Inspect seeded data
    var clients = await context.Set<Client>().ToListAsync();
    var projects = await context.Set<ClientProject>().ToListAsync();
    
    Console.WriteLine($"Clients: {clients.Count}");
    Console.WriteLine($"Projects: {projects.Count}");
    
    foreach (var client in clients)
    {
        Console.WriteLine($"Client: {client.Name} ({client.Code})");
    }
}
```

### Data Validation

```csharp
[Fact]
public async Task ValidateTestData()
{
    using var context = _fixture.CreateContext();
    
    // Validate data integrity
    var clients = await context.Set<Client>().ToListAsync();
    clients.Should().NotBeEmpty();
    clients.Should().AllSatisfy(c => c.Name.Should().NotBeNullOrEmpty());
    clients.Should().AllSatisfy(c => c.Code.Should().NotBeNullOrEmpty());
}
```

## 📊 **Data Statistics**

### Test Data Metrics

```csharp
public class TestDataMetrics
{
    public int ClientCount { get; set; }
    public int ProjectCount { get; set; }
    public int UserCount { get; set; }
    public int QuestionnaireCount { get; set; }
    
    public async Task<TestDataMetrics> GetMetricsAsync(DefaultContext context)
    {
        return new TestDataMetrics
        {
            ClientCount = await context.Set<Client>().CountAsync(),
            ProjectCount = await context.Set<ClientProject>().CountAsync(),
            UserCount = await context.Set<ClientUser>().CountAsync(),
            QuestionnaireCount = await context.Set<ClientQuestionnaire>().CountAsync()
        };
    }
}
```

## 🚀 **Performance Considerations**

### Large Dataset Testing

```csharp
[Fact]
public async Task PerformanceTest_LargeDataset()
{
    // Create large dataset
    var clients = ClientBuilder.CreateMultiple(1000);
    using var context = _fixture.CreateContext();
    
    // Bulk insert
    context.AddRange(clients);
    await context.SaveChangesAsync();
    
    // Performance test
    var stopwatch = Stopwatch.StartNew();
    var result = await context.Set<Client>().ToListAsync();
    stopwatch.Stop();
    
    result.Should().HaveCount(1000);
    stopwatch.ElapsedMilliseconds.Should().BeLessThan(5000);
}
```

### Memory Management

```csharp
[Fact]
public async Task MemoryTest_LargeDataset()
{
    // Monitor memory usage
    var initialMemory = GC.GetTotalMemory(false);
    
    var clients = ClientBuilder.CreateMultiple(10000);
    using var context = _fixture.CreateContext();
    
    context.AddRange(clients);
    await context.SaveChangesAsync();
    
    var finalMemory = GC.GetTotalMemory(false);
    var memoryUsed = finalMemory - initialMemory;
    
    Console.WriteLine($"Memory used: {memoryUsed / 1024 / 1024} MB");
}
```

---

**This test data management guide provides comprehensive patterns and best practices for creating, managing, and maintaining test data in the KonaAI.Master integration tests.**

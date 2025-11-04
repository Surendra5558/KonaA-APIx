# Performance Testing Guide

## 🎯 **Overview**

This guide covers comprehensive performance testing strategies for the KonaAI.Master integration tests, including load testing, concurrent operations, and performance benchmarking.

## 🏗️ **Performance Test Architecture**

### Test Categories

```
Performance/
├── Concurrent/                    # Concurrent operation tests
│   ├── ConcurrentReadsTests.cs   # Concurrent read operations
│   ├── ConcurrentWritesTests.cs  # Concurrent write operations
│   └── ConcurrentMixedTests.cs   # Mixed concurrent operations
├── LargeDataset/                 # Large dataset tests
│   ├── LargeDatasetReadsTests.cs # Large dataset read operations
│   ├── LargeDatasetWritesTests.cs # Large dataset write operations
│   └── LargeDatasetQueriesTests.cs # Large dataset query operations
├── Load/                         # Load testing
│   ├── LoadTests.cs              # Basic load tests
│   ├── StressTests.cs            # Stress testing
│   └── EnduranceTests.cs         # Endurance testing
└── Benchmark/                    # Performance benchmarking
    ├── ApiBenchmarkTests.cs      # API performance benchmarks
    ├── DatabaseBenchmarkTests.cs # Database performance benchmarks
    └── BusinessBenchmarkTests.cs # Business logic benchmarks
```

## 🚀 **Performance Test Attributes**

### Test Categories

```csharp
[Performance]                     // Performance test category
[Concurrent]                      // Concurrent operation test
[Load]                           // Load testing
[Stress]                         // Stress testing
[Endurance]                      // Endurance testing
[Benchmark]                      // Performance benchmarking
[Slow]                           // Long-running test
[LargeDataset]                   // Large dataset test
```

### Performance Test Base Class

```csharp
[Performance]
public abstract class PerformanceTestBase : IClassFixture<SqlServerFixture>
{
    protected readonly SqlServerFixture _fixture;
    protected readonly Stopwatch _stopwatch;

    protected PerformanceTestBase(SqlServerFixture fixture)
    {
        _fixture = fixture;
        _stopwatch = new Stopwatch();
    }

    protected async Task<TimeSpan> MeasureAsync(Func<Task> action)
    {
        _stopwatch.Restart();
        await action();
        _stopwatch.Stop();
        return _stopwatch.Elapsed;
    }

    protected async Task<T> MeasureAsync<T>(Func<Task<T>> action)
    {
        _stopwatch.Restart();
        var result = await action();
        _stopwatch.Stop();
        return result;
    }
}
```

## ⚡ **Concurrent Operations Testing**

### Concurrent Reads

```csharp
[Performance]
[Concurrent]
public class ConcurrentReadsTests : PerformanceTestBase
{
    public ConcurrentReadsTests(SqlServerFixture fixture) : base(fixture) { }

    [Fact]
    public async Task ConcurrentReads_MultipleClients_AllSucceed()
    {
        // Arrange
        await _fixture.SeedTestDataAsync();
        var tasks = new List<Task<List<Client>>>();

        // Act - Create multiple concurrent read operations
        for (int i = 0; i < 10; i++)
        {
            tasks.Add(ReadClientsAsync());
        }

        var results = await Task.WhenAll(tasks);
        var elapsed = await MeasureAsync(async () => await Task.WhenAll(tasks));

        // Assert
        results.Should().AllSatisfy(clients => clients.Should().NotBeEmpty());
        elapsed.Should().BeLessThan(TimeSpan.FromSeconds(5));
    }

    private async Task<List<Client>> ReadClientsAsync()
    {
        using var context = _fixture.CreateContext();
        return await context.Set<Client>().ToListAsync();
    }
}
```

### Concurrent Writes

```csharp
[Performance]
[Concurrent]
public class ConcurrentWritesTests : PerformanceTestBase
{
    public ConcurrentWritesTests(SqlServerFixture fixture) : base(fixture) { }

    [Fact]
    public async Task ConcurrentWrites_DifferentClients_AllSucceed()
    {
        // Arrange
        await _fixture.ClearDatabaseAsync();
        var tasks = new List<Task<int>>();

        // Act - Create multiple concurrent write operations
        for (int i = 0; i < 10; i++)
        {
            var clientId = i;
            tasks.Add(CreateClientAsync($"Client {clientId}"));
        }

        var results = await Task.WhenAll(tasks);
        var elapsed = await MeasureAsync(async () => await Task.WhenAll(tasks));

        // Assert
        results.Should().AllSatisfy(result => result.Should().Be(1));
        elapsed.Should().BeLessThan(TimeSpan.FromSeconds(10));
    }

    private async Task<int> CreateClientAsync(string name)
    {
        using var context = _fixture.CreateContext();
        var client = ClientBuilder.Create()
            .WithName(name)
            .WithCode($"C{DateTime.Now.Ticks}")
            .Active()
            .Build();

        context.Add(client);
        return await context.SaveChangesAsync();
    }
}
```

### Mixed Concurrent Operations

```csharp
[Performance]
[Concurrent]
public class ConcurrentMixedTests : PerformanceTestBase
{
    public ConcurrentMixedTests(SqlServerFixture fixture) : base(fixture) { }

    [Fact]
    public async Task ConcurrentMixed_ReadsAndWrites_AllSucceed()
    {
        // Arrange
        await _fixture.SeedTestDataAsync();
        var tasks = new List<Task>();

        // Act - Mix of read and write operations
        for (int i = 0; i < 5; i++)
        {
            tasks.Add(ReadClientsAsync());
            tasks.Add(CreateClientAsync($"Mixed Client {i}"));
        }

        var elapsed = await MeasureAsync(async () => await Task.WhenAll(tasks));

        // Assert
        elapsed.Should().BeLessThan(TimeSpan.FromSeconds(15));
    }
}
```

## 📊 **Large Dataset Testing**

### Large Dataset Reads

```csharp
[Performance]
[LargeDataset]
public class LargeDatasetReadsTests : PerformanceTestBase
{
    public LargeDatasetReadsTests(SqlServerFixture fixture) : base(fixture) { }

    [Fact]
    public async Task ReadClients_LargeDataset_PerformsWithinTimeLimit()
    {
        // Arrange
        await _fixture.ClearDatabaseAsync();
        var clients = ClientBuilder.CreateMultiple(10000);
        
        using var context = _fixture.CreateContext();
        context.AddRange(clients);
        await context.SaveChangesAsync();

        // Act
        var elapsed = await MeasureAsync(async () =>
        {
            var result = await context.Set<Client>().ToListAsync();
            return result;
        });

        // Assert
        elapsed.Should().BeLessThan(TimeSpan.FromSeconds(30));
    }

    [Fact]
    public async Task ReadClients_WithPagination_PerformsWithinTimeLimit()
    {
        // Arrange
        await _fixture.ClearDatabaseAsync();
        var clients = ClientBuilder.CreateMultiple(50000);
        
        using var context = _fixture.CreateContext();
        context.AddRange(clients);
        await context.SaveChangesAsync();

        // Act
        var elapsed = await MeasureAsync(async () =>
        {
            var result = await context.Set<Client>()
                .Skip(0)
                .Take(1000)
                .ToListAsync();
            return result;
        });

        // Assert
        elapsed.Should().BeLessThan(TimeSpan.FromSeconds(5));
    }
}
```

### Large Dataset Writes

```csharp
[Performance]
[LargeDataset]
public class LargeDatasetWritesTests : PerformanceTestBase
{
    public LargeDatasetWritesTests(SqlServerFixture fixture) : base(fixture) { }

    [Fact]
    public async Task CreateClients_LargeDataset_PerformsWithinTimeLimit()
    {
        // Arrange
        await _fixture.ClearDatabaseAsync();
        var clients = ClientBuilder.CreateMultiple(10000);

        // Act
        var elapsed = await MeasureAsync(async () =>
        {
            using var context = _fixture.CreateContext();
            context.AddRange(clients);
            await context.SaveChangesAsync();
        });

        // Assert
        elapsed.Should().BeLessThan(TimeSpan.FromSeconds(60));
    }

    [Fact]
    public async Task BulkInsert_LargeDataset_PerformsWithinTimeLimit()
    {
        // Arrange
        await _fixture.ClearDatabaseAsync();
        var clients = ClientBuilder.CreateMultiple(50000);

        // Act
        var elapsed = await MeasureAsync(async () =>
        {
            using var context = _fixture.CreateContext();
            
            // Bulk insert in batches
            const int batchSize = 1000;
            for (int i = 0; i < clients.Count; i += batchSize)
            {
                var batch = clients.Skip(i).Take(batchSize);
                context.AddRange(batch);
                await context.SaveChangesAsync();
            }
        });

        // Assert
        elapsed.Should().BeLessThan(TimeSpan.FromSeconds(120));
    }
}
```

## 🔥 **Load Testing**

### Basic Load Tests

```csharp
[Performance]
[Load]
public class LoadTests : PerformanceTestBase
{
    public LoadTests(SqlServerFixture fixture) : base(fixture) { }

    [Fact]
    public async Task LoadTest_ConcurrentUsers_PerformsWithinTimeLimit()
    {
        // Arrange
        await _fixture.SeedTestDataAsync();
        var userCount = 50;
        var operationsPerUser = 10;

        // Act
        var elapsed = await MeasureAsync(async () =>
        {
            var tasks = new List<Task>();
            
            for (int user = 0; user < userCount; user++)
            {
                for (int op = 0; op < operationsPerUser; op++)
                {
                    tasks.Add(SimulateUserOperationAsync(user, op));
                }
            }

            await Task.WhenAll(tasks);
        });

        // Assert
        elapsed.Should().BeLessThan(TimeSpan.FromSeconds(30));
    }

    private async Task SimulateUserOperationAsync(int userId, int operationId)
    {
        using var context = _fixture.CreateContext();
        
        // Simulate user operation
        var clients = await context.Set<Client>().ToListAsync();
        var client = clients.FirstOrDefault();
        
        if (client != null)
        {
            // Simulate some processing
            await Task.Delay(10);
        }
    }
}
```

### Stress Tests

```csharp
[Performance]
[Stress]
public class StressTests : PerformanceTestBase
{
    public StressTests(SqlServerFixture fixture) : base(fixture) { }

    [Fact]
    public async Task StressTest_HighConcurrency_PerformsWithinTimeLimit()
    {
        // Arrange
        await _fixture.SeedTestDataAsync();
        var concurrentTasks = 100;

        // Act
        var elapsed = await MeasureAsync(async () =>
        {
            var tasks = new List<Task>();
            
            for (int i = 0; i < concurrentTasks; i++)
            {
                tasks.Add(StressOperationAsync(i));
            }

            await Task.WhenAll(tasks);
        });

        // Assert
        elapsed.Should().BeLessThan(TimeSpan.FromSeconds(60));
    }

    private async Task StressOperationAsync(int taskId)
    {
        using var context = _fixture.CreateContext();
        
        // Stress operation
        var clients = await context.Set<Client>().ToListAsync();
        var projects = await context.Set<ClientProject>().ToListAsync();
        
        // Simulate complex operation
        await Task.Delay(50);
    }
}
```

### Endurance Tests

```csharp
[Performance]
[Endurance]
public class EnduranceTests : PerformanceTestBase
{
    public EnduranceTests(SqlServerFixture fixture) : base(fixture) { }

    [Fact]
    public async Task EnduranceTest_LongRunning_PerformsWithinTimeLimit()
    {
        // Arrange
        await _fixture.SeedTestDataAsync();
        var duration = TimeSpan.FromMinutes(5);
        var startTime = DateTime.UtcNow;

        // Act
        var elapsed = await MeasureAsync(async () =>
        {
            while (DateTime.UtcNow - startTime < duration)
            {
                await PerformEnduranceOperationAsync();
                await Task.Delay(1000); // 1 second interval
            }
        });

        // Assert
        elapsed.Should().BeLessThan(duration.Add(TimeSpan.FromSeconds(30)));
    }

    private async Task PerformEnduranceOperationAsync()
    {
        using var context = _fixture.CreateContext();
        
        // Endurance operation
        var clients = await context.Set<Client>().ToListAsync();
        var client = clients.FirstOrDefault();
        
        if (client != null)
        {
            // Simulate some processing
            await Task.Delay(100);
        }
    }
}
```

## 📈 **Performance Benchmarking**

### API Performance Benchmarks

```csharp
[Performance]
[Benchmark]
public class ApiBenchmarkTests : PerformanceTestBase
{
    public ApiBenchmarkTests(SqlServerFixture fixture) : base(fixture) { }

    [Fact]
    public async Task ApiBenchmark_GetClients_PerformsWithinTimeLimit()
    {
        // Arrange
        await _fixture.SeedTestDataAsync();
        var iterations = 100;

        // Act
        var elapsed = await MeasureAsync(async () =>
        {
            for (int i = 0; i < iterations; i++)
            {
                using var context = _fixture.CreateContext();
                var clients = await context.Set<Client>().ToListAsync();
            }
        });

        // Assert
        var averageTime = elapsed.TotalMilliseconds / iterations;
        averageTime.Should().BeLessThan(100); // 100ms average
    }

    [Fact]
    public async Task ApiBenchmark_CreateClient_PerformsWithinTimeLimit()
    {
        // Arrange
        await _fixture.ClearDatabaseAsync();
        var iterations = 100;

        // Act
        var elapsed = await MeasureAsync(async () =>
        {
            for (int i = 0; i < iterations; i++)
            {
                using var context = _fixture.CreateContext();
                var client = ClientBuilder.Create()
                    .WithName($"Benchmark Client {i}")
                    .WithCode($"BC{i}")
                    .Active()
                    .Build();
                
                context.Add(client);
                await context.SaveChangesAsync();
            }
        });

        // Assert
        var averageTime = elapsed.TotalMilliseconds / iterations;
        averageTime.Should().BeLessThan(200); // 200ms average
    }
}
```

### Database Performance Benchmarks

```csharp
[Performance]
[Benchmark]
public class DatabaseBenchmarkTests : PerformanceTestBase
{
    public DatabaseBenchmarkTests(SqlServerFixture fixture) : base(fixture) { }

    [Fact]
    public async Task DatabaseBenchmark_QueryPerformance_PerformsWithinTimeLimit()
    {
        // Arrange
        await _fixture.SeedTestDataAsync();
        var iterations = 1000;

        // Act
        var elapsed = await MeasureAsync(async () =>
        {
            for (int i = 0; i < iterations; i++)
            {
                using var context = _fixture.CreateContext();
                var clients = await context.Set<Client>()
                    .Where(c => c.IsActive)
                    .ToListAsync();
            }
        });

        // Assert
        var averageTime = elapsed.TotalMilliseconds / iterations;
        averageTime.Should().BeLessThan(50); // 50ms average
    }

    [Fact]
    public async Task DatabaseBenchmark_InsertPerformance_PerformsWithinTimeLimit()
    {
        // Arrange
        await _fixture.ClearDatabaseAsync();
        var iterations = 1000;

        // Act
        var elapsed = await MeasureAsync(async () =>
        {
            for (int i = 0; i < iterations; i++)
            {
                using var context = _fixture.CreateContext();
                var client = ClientBuilder.Create()
                    .WithName($"Benchmark Client {i}")
                    .WithCode($"BC{i}")
                    .Active()
                    .Build();
                
                context.Add(client);
                await context.SaveChangesAsync();
            }
        });

        // Assert
        var averageTime = elapsed.TotalMilliseconds / iterations;
        averageTime.Should().BeLessThan(100); // 100ms average
    }
}
```

## 🎯 **Performance Test Configuration**

### Test Settings

```json
{
  "TestSettings": {
    "PerformanceTestRecordCount": 10000,
    "ConcurrentTestClientCount": 50,
    "LoadTestDuration": "00:05:00",
    "StressTestConcurrency": 100,
    "EnduranceTestDuration": "00:10:00",
    "BenchmarkIterations": 1000,
    "PerformanceThresholds": {
      "ApiResponseTime": 100,
      "DatabaseQueryTime": 50,
      "ConcurrentOperationTime": 5000,
      "LargeDatasetTime": 30000
    }
  }
}
```

### Performance Thresholds

```csharp
public class PerformanceThresholds
{
    public static readonly TimeSpan ApiResponseTime = TimeSpan.FromMilliseconds(100);
    public static readonly TimeSpan DatabaseQueryTime = TimeSpan.FromMilliseconds(50);
    public static readonly TimeSpan ConcurrentOperationTime = TimeSpan.FromSeconds(5);
    public static readonly TimeSpan LargeDatasetTime = TimeSpan.FromSeconds(30);
    public static readonly TimeSpan LoadTestTime = TimeSpan.FromSeconds(30);
    public static readonly TimeSpan StressTestTime = TimeSpan.FromSeconds(60);
    public static readonly TimeSpan EnduranceTestTime = TimeSpan.FromMinutes(10);
}
```

## 📊 **Performance Monitoring**

### Memory Usage Monitoring

```csharp
[Performance]
[Benchmark]
public class MemoryUsageTests : PerformanceTestBase
{
    public MemoryUsageTests(SqlServerFixture fixture) : base(fixture) { }

    [Fact]
    public async Task MemoryUsage_LargeDataset_WithinMemoryLimit()
    {
        // Arrange
        var initialMemory = GC.GetTotalMemory(false);
        await _fixture.ClearDatabaseAsync();

        // Act
        var clients = ClientBuilder.CreateMultiple(10000);
        using var context = _fixture.CreateContext();
        context.AddRange(clients);
        await context.SaveChangesAsync();

        // Assert
        var finalMemory = GC.GetTotalMemory(false);
        var memoryUsed = finalMemory - initialMemory;
        var memoryUsedMB = memoryUsed / 1024 / 1024;

        memoryUsedMB.Should().BeLessThan(100); // 100MB limit
        Console.WriteLine($"Memory used: {memoryUsedMB} MB");
    }
}
```

### CPU Usage Monitoring

```csharp
[Performance]
[Benchmark]
public class CpuUsageTests : PerformanceTestBase
{
    public CpuUsageTests(SqlServerFixture fixture) : base(fixture) { }

    [Fact]
    public async Task CpuUsage_ConcurrentOperations_WithinCpuLimit()
    {
        // Arrange
        await _fixture.SeedTestDataAsync();
        var startCpu = Process.GetCurrentProcess().TotalProcessorTime;

        // Act
        var elapsed = await MeasureAsync(async () =>
        {
            var tasks = new List<Task>();
            for (int i = 0; i < 50; i++)
            {
                tasks.Add(ConcurrentOperationAsync());
            }
            await Task.WhenAll(tasks);
        });

        // Assert
        var endCpu = Process.GetCurrentProcess().TotalProcessorTime;
        var cpuUsed = endCpu - startCpu;
        var cpuPercentage = (cpuUsed.TotalMilliseconds / elapsed.TotalMilliseconds) * 100;

        cpuPercentage.Should().BeLessThan(80); // 80% CPU limit
        Console.WriteLine($"CPU usage: {cpuPercentage:F2}%");
    }
}
```

## 🚀 **Performance Test Execution**

### Running Performance Tests

```bash
# Run all performance tests
dotnet test --filter Category=Performance

# Run specific performance test categories
dotnet test --filter Performance
dotnet test --filter Concurrent
dotnet test --filter Load
dotnet test --filter Stress
dotnet test --filter Endurance
dotnet test --filter Benchmark

# Run with specific attributes
dotnet test --filter LargeDataset
dotnet test --filter Slow
```

### Performance Test Results

```csharp
[Fact]
public async Task PerformanceTest_WithDetailedResults()
{
    // Arrange
    await _fixture.SeedTestDataAsync();
    var iterations = 100;

    // Act
    var results = new List<TimeSpan>();
    for (int i = 0; i < iterations; i++)
    {
        var elapsed = await MeasureAsync(async () =>
        {
            using var context = _fixture.CreateContext();
            var clients = await context.Set<Client>().ToListAsync();
        });
        results.Add(elapsed);
    }

    // Assert
    var averageTime = results.Average(r => r.TotalMilliseconds);
    var minTime = results.Min(r => r.TotalMilliseconds);
    var maxTime = results.Max(r => r.TotalMilliseconds);
    var medianTime = results.OrderBy(r => r.TotalMilliseconds)
        .Skip(results.Count / 2)
        .First()
        .TotalMilliseconds;

    Console.WriteLine($"Average time: {averageTime:F2}ms");
    Console.WriteLine($"Min time: {minTime:F2}ms");
    Console.WriteLine($"Max time: {maxTime:F2}ms");
    Console.WriteLine($"Median time: {medianTime:F2}ms");

    averageTime.Should().BeLessThan(100);
    maxTime.Should().BeLessThan(500);
}
```

## 🎯 **Best Practices**

### 1. **Test Design**
- Use appropriate test categories
- Set realistic performance thresholds
- Monitor resource usage
- Clean up test data

### 2. **Performance Optimization**
- Use bulk operations for large datasets
- Minimize database round trips
- Use appropriate data sizes
- Monitor memory usage

### 3. **Test Execution**
- Run performance tests in isolation
- Use appropriate test data sizes
- Monitor test execution times
- Document performance requirements

### 4. **Results Analysis**
- Track performance trends over time
- Set performance baselines
- Monitor performance regressions
- Document performance improvements

---

**This performance testing guide provides comprehensive strategies for testing performance, load, and scalability of the KonaAI.Master integration tests.**

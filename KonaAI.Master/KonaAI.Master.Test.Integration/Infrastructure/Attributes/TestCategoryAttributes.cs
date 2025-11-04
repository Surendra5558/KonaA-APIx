using System;

namespace KonaAI.Master.Test.Integration.Infrastructure.Attributes;

/// <summary>
/// Marks tests as API integration tests (full HTTP pipeline).
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public class ApiIntegrationAttribute : Attribute
{
    public ApiIntegrationAttribute() { }
}

/// <summary>
/// Marks tests as repository integration tests (database operations).
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public class RepositoryIntegrationAttribute : Attribute
{
    public RepositoryIntegrationAttribute() { }
}

/// <summary>
/// Marks tests as business integration tests (business + repository).
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public class BusinessIntegrationAttribute : Attribute
{
    public BusinessIntegrationAttribute() { }
}

/// <summary>
/// Marks tests as performance tests.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public class PerformanceAttribute : Attribute
{
    public PerformanceAttribute() { }
}

/// <summary>
/// Marks tests as multi-tenancy tests.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public class MultiTenancyAttribute : Attribute
{
    public MultiTenancyAttribute() { }
}

/// <summary>
/// Marks tests as end-to-end workflow tests.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public class EndToEndAttribute : Attribute
{
    public EndToEndAttribute() { }
}

/// <summary>
/// Marks tests as concurrent operation tests.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public class ConcurrentAttribute : Attribute
{
    public ConcurrentAttribute() { }
}

/// <summary>
/// Marks tests as OData query tests.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public class ODataAttribute : Attribute
{
    public ODataAttribute() { }
}

/// <summary>
/// Marks tests as authentication tests.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public class AuthenticationAttribute : Attribute
{
    public AuthenticationAttribute() { }
}

/// <summary>
/// Marks tests as slow tests that should run separately.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public class SlowAttribute : Attribute
{
    public SlowAttribute() { }
}

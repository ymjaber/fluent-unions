# Performance Best Practices

This guide covers performance optimization techniques and best practices when using FluentUnions.

## Table of Contents
1. [Introduction](#introduction)
2. [Source Generators Benefits](#source-generators-benefits)
3. [Allocation Optimization](#allocation-optimization)
4. [Pattern Matching Performance](#pattern-matching-performance)
5. [Collection Operations](#collection-operations)
6. [Error Handling Efficiency](#error-handling-efficiency)
7. [Memory Usage Patterns](#memory-usage-patterns)
8. [Benchmarking Results](#benchmarking-results)
9. [Common Pitfalls](#common-pitfalls)
10. [Optimization Strategies](#optimization-strategies)

## Introduction

FluentUnions is designed with performance in mind, using source generators to eliminate runtime overhead and providing zero-allocation operations where possible. This guide helps you maximize performance in your applications.

### Key Performance Features

- **Source Generators**: Zero runtime reflection
- **Struct-based Options**: Reduced allocations
- **Inline Methods**: JIT optimization friendly
- **No Boxing**: Value types stay unboxed

## Source Generators Benefits

### Zero Runtime Overhead

FluentUnions uses source generators to create all extension methods at compile time:

```csharp
// This code is generated at compile time
public static Result<TResult> Map<T, TResult>(
    this Result<T> result, 
    Func<T, TResult> mapper)
{
    return result.IsSuccess 
        ? Result.Success(mapper(result.Value))
        : Result.Failure<TResult>(result.Error);
}

// No reflection, no runtime code generation
```

### Benefits Over Reflection-Based Libraries

```csharp
// Other libraries might use reflection
// ❌ Slow - uses reflection
var method = typeof(Result<>)
    .GetMethod("Map")
    .MakeGenericMethod(typeof(int), typeof(string));

// ✅ FluentUnions - compile-time generated
var result = Result.Success(42).Map(x => x.ToString());
```

### Optimal Inlining

Source generators produce code that's inline-friendly:

```csharp
// Generated methods are candidates for JIT inlining
[MethodImpl(MethodImplOptions.AggressiveInlining)]
public static bool IsSuccess<T>(this Result<T> result)
{
    return result._isSuccess;
}
```

## Allocation Optimization

### Avoiding Unnecessary Allocations

```csharp
// ❌ Bad - creates intermediate Results
public Result<string> ProcessData(Data data)
{
    return ValidateData(data)
        .Map(d => TransformData(d))      // Allocates Result<TransformedData>
        .Map(t => SerializeData(t))      // Allocates Result<string>
        .Map(s => CompressData(s));      // Allocates Result<string>
}

// ✅ Good - single allocation
public Result<string> ProcessData(Data data)
{
    return ValidateData(data)
        .Map(d => 
        {
            var transformed = TransformData(d);
            var serialized = SerializeData(transformed);
            return CompressData(serialized);
        });
}
```

### Using ValueTask for Hot Paths

```csharp
// For frequently called methods with synchronous completion
public ValueTask<Result<User>> GetUserFromCacheAsync(int id)
{
    if (_cache.TryGetValue(id, out var user))
        return new ValueTask<Result<User>>(Result.Success(user));
        
    return new ValueTask<Result<User>>(LoadUserAsync(id));
}
```

### Struct-Based Options

Option<T> can be implemented as a struct for value types:

```csharp
// Minimal allocations for value types
Option<int> option = Option.Some(42);  // No heap allocation

// Reference types still allocate the contained object
Option<string> str = Option.Some("hello");  // String is allocated
```

## Pattern Matching Performance

### Efficient Match Implementation

```csharp
// ✅ Good - single virtual call
var result = GetResult().Match(
    onSuccess: value => ProcessValue(value),
    onFailure: error => HandleError(error)
);

// ❌ Less efficient - multiple property accesses
var r = GetResult();
if (r.IsSuccess)
    ProcessValue(r.Value);
else
    HandleError(r.Error);
```

### Switch Expressions vs Match

```csharp
// Switch expressions - good for simple cases
var message = result switch
{
    { IsSuccess: true } => "Success",
    { Error: ValidationError } => "Validation failed",
    _ => "Unknown error"
};

// Match method - better for complex operations
var processed = result.Match(
    onSuccess: value => 
    {
        LogSuccess(value);
        UpdateMetrics(value);
        return Transform(value);
    },
    onFailure: error =>
    {
        LogError(error);
        AlertTeam(error);
        return GetDefault();
    }
);
```

## Collection Operations

### Efficient BindAll

```csharp
// ❌ Inefficient - multiple iterations
public Result<List<Product>> LoadProducts(List<int> ids)
{
    var results = new List<Result<Product>>();
    foreach (var id in ids)
    {
        results.Add(LoadProduct(id));
    }
    
    return Result.BindAll(results.ToArray());
}

// ✅ Efficient - single pass with early exit
public Result<List<Product>> LoadProducts(List<int> ids)
{
    var products = new List<Product>(ids.Count);
    
    foreach (var id in ids)
    {
        var result = LoadProduct(id);
        if (result.IsFailure)
            return Result.Failure<List<Product>>(result.Error);
            
        products.Add(result.Value);
    }
    
    return Result.Success(products);
}
```

### Parallel Processing

```csharp
// For CPU-bound operations
public Result<List<ProcessedItem>> ProcessItemsParallel(List<Item> items)
{
    var results = new Result<ProcessedItem>[items.Count];
    
    Parallel.For(0, items.Count, i =>
    {
        results[i] = ProcessItem(items[i]);
    });
    
    return BindAll(results);
}

// For I/O-bound operations
public async Task<Result<List<Data>>> LoadDataParallelAsync(List<string> urls)
{
    var tasks = urls.Select(url => LoadDataAsync(url));
    var results = await Task.WhenAll(tasks);
    
    return BindAll(results);
}
```

## Error Handling Efficiency

### Reusing Error Instances

```csharp
public static class CommonErrors
{
    // Reuse common errors to avoid allocations
    public static readonly Error NetworkError = 
        new Error("NETWORK_ERROR", "Network connection failed");
        
    public static readonly Error TimeoutError = 
        new Error("TIMEOUT", "Operation timed out");
        
    public static readonly Error UnauthorizedError = 
        new AuthorizationError("Unauthorized access");
}

// Usage
public Result<Data> FetchData()
{
    try
    {
        return Result.Success(GetData());
    }
    catch (TimeoutException)
    {
        return CommonErrors.TimeoutError;  // No allocation
    }
}
```

### Lazy Error Details

```csharp
public class LazyError : Error
{
    private readonly Lazy<string> _details;
    
    public LazyError(string code, Func<string> detailsFactory) 
        : base(code, "Error occurred")
    {
        _details = new Lazy<string>(detailsFactory);
    }
    
    public override string Message => _details.Value;
}

// Only computes expensive message if needed
return new LazyError("COMPLEX_ERROR", 
    () => ComputeExpensiveErrorMessage());
```

## Memory Usage Patterns

### Object Pooling for High-Frequency Operations

```csharp
public class ResultProcessor
{
    private readonly ObjectPool<StringBuilder> _stringBuilderPool;
    
    public Result<string> ProcessResults(List<Result<Data>> results)
    {
        var sb = _stringBuilderPool.Get();
        try
        {
            foreach (var result in results)
            {
                if (result.IsSuccess)
                    sb.AppendLine(result.Value.ToString());
            }
            
            return Result.Success(sb.ToString());
        }
        finally
        {
            sb.Clear();
            _stringBuilderPool.Return(sb);
        }
    }
}
```

### Avoiding Closure Allocations

```csharp
// ❌ Bad - captures variables, creates closure
public Result<List<Item>> FilterItems(List<Item> items, int minValue)
{
    return Result.Success(items.Where(item => item.Value > minValue).ToList());
}

// ✅ Good - no closure allocation
public Result<List<Item>> FilterItems(List<Item> items, int minValue)
{
    var filtered = new List<Item>();
    foreach (var item in items)
    {
        if (item.Value > minValue)
            filtered.Add(item);
    }
    return Result.Success(filtered);
}

// ✅ Alternative - pass state explicitly
public Result<List<Item>> FilterItems(List<Item> items, int minValue)
{
    return Result.Success(
        items.Where(
            static (item, min) => item.Value > min, 
            minValue
        ).ToList()
    );
}
```

## Benchmarking Results

### Typical Performance Characteristics

```csharp
// Benchmark results (example)
// | Method | Mean | Error | StdDev | Allocated |
// |--------|------|-------|--------|-----------|
// | Result_Success | 2.5 ns | 0.1 ns | 0.1 ns | 0 B |
// | Result_Failure | 15.2 ns | 0.3 ns | 0.2 ns | 72 B |
// | Map_Operation | 5.1 ns | 0.1 ns | 0.1 ns | 0 B |
// | Bind_Operation | 6.3 ns | 0.2 ns | 0.1 ns | 0 B |
// | Match_Operation | 4.8 ns | 0.1 ns | 0.1 ns | 0 B |
```

### Benchmarking Your Code

```csharp
[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net80)]
public class ResultBenchmarks
{
    private Result<int> _successResult = Result.Success(42);
    private Result<int> _failureResult = Result.Failure<int>("Error");
    
    [Benchmark]
    public int Map_Success()
    {
        return _successResult.Map(x => x * 2).Value;
    }
    
    [Benchmark]
    public string Match_Failure()
    {
        return _failureResult.Match(
            onSuccess: x => x.ToString(),
            onFailure: e => e.Message
        );
    }
}
```

## Common Pitfalls

### 1. Excessive Chaining

```csharp
// ❌ Too many intermediate Results
var result = GetData()
    .Map(x => x.Value)
    .Map(v => v * 2)
    .Map(v => v.ToString())
    .Map(s => s.ToUpper())
    .Map(s => $"Result: {s}");

// ✅ Combine operations
var result = GetData()
    .Map(x => $"Result: {(x.Value * 2).ToString().ToUpper()}");
```

### 2. Repeated Error Creation

```csharp
// ❌ Creates new error each time
public Result<User> GetUser(int id)
{
    var user = _repository.Find(id);
    return user != null 
        ? Result.Success(user)
        : new NotFoundError($"User {id} not found"); // Allocation
}

// ✅ Reuse error or create once
private static Error CreateNotFoundError(int id) => 
    new NotFoundError($"User {id} not found");

public Result<User> GetUser(int id)
{
    var user = _repository.Find(id);
    return user != null 
        ? Result.Success(user)
        : CreateNotFoundError(id);
}
```

### 3. Unnecessary Async

```csharp
// ❌ Async overhead for synchronous operation
public async Task<Result<int>> CalculateAsync(int x, int y)
{
    return await Task.FromResult(Result.Success(x + y));
}

// ✅ Keep synchronous operations synchronous
public Result<int> Calculate(int x, int y)
{
    return Result.Success(x + y);
}
```

## Optimization Strategies

### 1. Profile First

```csharp
// Use profiling to find actual bottlenecks
using (Profiler.BeginSample("ProcessOrder"))
{
    var result = ValidateOrder(order)
        .Bind(ProcessPayment)
        .Bind(UpdateInventory);
}
```

### 2. Cache Results When Appropriate

```csharp
public class CachedService
{
    private readonly MemoryCache _cache;
    
    public Result<Data> GetData(string key)
    {
        if (_cache.TryGetValue(key, out Result<Data> cached))
            return cached;
            
        var result = LoadData(key);
        
        if (result.IsSuccess)
            _cache.Set(key, result, TimeSpan.FromMinutes(5));
            
        return result;
    }
}
```

### 3. Use Span<T> for Large Data

```csharp
public Result<int> ProcessLargeArray(ReadOnlySpan<byte> data)
{
    if (data.Length == 0)
        return new ValidationError("Empty data");
        
    int sum = 0;
    foreach (var b in data)
        sum += b;
        
    return Result.Success(sum);
}
```

### 4. Consider ValueResult for Hot Paths

```csharp
// Custom value type Result for performance-critical paths
public readonly struct ValueResult<T>
{
    public readonly bool IsSuccess;
    public readonly T Value;
    public readonly string Error;
    
    // Implementation...
}
```

## Summary

Key performance best practices:

1. **Leverage Source Generators** - Zero runtime overhead
2. **Minimize Allocations** - Reuse errors, avoid closures
3. **Optimize Hot Paths** - Profile and optimize critical sections
4. **Use Appropriate Patterns** - Match vs switch, sync vs async
5. **Measure Performance** - Benchmark before optimizing

Remember: **Premature optimization is the root of all evil**. Profile first, optimize what matters.

Next steps:
- [Source Generators Documentation](../reference/source-generators.md)
- [Testing Guide](testing-guide.md)
- [Advanced Patterns](../patterns/service-patterns.md)
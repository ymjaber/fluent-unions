# Source Generators Reference

This guide covers the source generators included with FluentUnions that provide additional functionality and performance optimizations at compile time.

## Table of Contents
1. [Introduction](#introduction)
2. [How Source Generators Work](#how-source-generators-work)
3. [Generated Extension Methods](#generated-extension-methods)
4. [Multi-Value Support](#multi-value-support)
5. [Performance Optimizations](#performance-optimizations)
6. [Customization Options](#customization-options)
7. [Troubleshooting](#troubleshooting)
8. [Generated Code Examples](#generated-code-examples)

## Introduction

FluentUnions uses C# source generators to:
- Create specialized extension methods for common patterns
- Generate optimized code paths for performance-critical operations
- Provide multi-value support for Result and Option operations
- Reduce boilerplate code
- Enable compile-time optimizations

## How Source Generators Work

### Build-Time Code Generation

Source generators run during compilation and generate additional C# code based on your project:

```csharp
// You write:
var result = Result.BindAll(
    GetUser(userId),
    GetAccount(accountId),
    GetPermissions(userId)
).Map((user, account, permissions) => new Dashboard(user, account, permissions));

// Generator creates optimized overloads for BindAll with multiple types
```

### Generator Output Location

Generated files are typically placed in:
```
obj/Debug/net8.0/generated/FluentUnions.Generators/
```

You can view generated code in Visual Studio:
1. Solution Explorer → Project → Dependencies → Analyzers → FluentUnions.Generators
2. Expand to see generated files

## Generated Extension Methods

### Map Extensions

The generator creates specialized Map overloads:

```csharp
// Generated for Result<T>
public static Result<TResult> Map<T1, T2, TResult>(
    this Result<(T1, T2)> result,
    Func<T1, T2, TResult> mapper)
{
    return result.IsSuccess 
        ? Result.Success(mapper(result.Value.Item1, result.Value.Item2))
        : Result.Failure<TResult>(result.Error);
}

// Usage
Result<(string name, int age)> personData = GetPersonData();
Result<Person> person = personData.Map((name, age) => new Person(name, age));
```

### Bind Extensions

Generated Bind overloads for multiple parameters:

```csharp
// Generated for up to 8 parameters
public static Result<TResult> Bind<T1, T2, T3, TResult>(
    this Result<(T1, T2, T3)> result,
    Func<T1, T2, T3, Result<TResult>> binder)
{
    return result.IsSuccess 
        ? binder(result.Value.Item1, result.Value.Item2, result.Value.Item3)
        : Result.Failure<TResult>(result.Error);
}

// Usage
Result<(User user, Account account, Settings settings)> data = LoadUserData();
Result<Profile> profile = data.Bind((user, account, settings) => 
    CreateProfile(user, account, settings));
```

### Match Extensions

Pattern matching with tuples:

```csharp
// Generated match for tuples
public static TResult Match<T1, T2, TResult>(
    this Result<(T1, T2)> result,
    Func<T1, T2, TResult> onSuccess,
    Func<Error, TResult> onFailure)
{
    return result.IsSuccess 
        ? onSuccess(result.Value.Item1, result.Value.Item2)
        : onFailure(result.Error);
}

// Usage
string message = result.Match(
    (user, account) => $"{user.Name} has balance {account.Balance}",
    error => $"Error: {error.Message}"
);
```

## Multi-Value Support

### BindAll Generated Methods

The generator creates BindAll overloads for combining multiple Results:

```csharp
// Generated for 2 values
public static Result<(T1, T2)> BindAll<T1, T2>(
    Result<T1> result1,
    Result<T2> result2)
{
    if (result1.IsFailure) return Result.Failure<(T1, T2)>(result1.Error);
    if (result2.IsFailure) return Result.Failure<(T1, T2)>(result2.Error);
    
    return Result.Success((result1.Value, result2.Value));
}

// Generated for 3 values
public static Result<(T1, T2, T3)> BindAll<T1, T2, T3>(
    Result<T1> result1,
    Result<T2> result2,
    Result<T3> result3)
{
    if (result1.IsFailure) return Result.Failure<(T1, T2, T3)>(result1.Error);
    if (result2.IsFailure) return Result.Failure<(T1, T2, T3)>(result2.Error);
    if (result3.IsFailure) return Result.Failure<(T1, T2, T3)>(result3.Error);
    
    return Result.Success((result1.Value, result2.Value, result3.Value));
}

// Usage
public Result<Order> CreateOrder(Guid customerId, Guid productId, int quantity)
{
    return Result.BindAll(
        GetCustomer(customerId),
        GetProduct(productId),
        ValidateQuantity(quantity)
    ).Map((customer, product, qty) => new Order(customer, product, qty));
}
```

### Option.Zip Generated Methods

Similar generation for Option types:

```csharp
// Generated Zip for Options
public static Option<(T1, T2, T3)> Zip<T1, T2, T3>(
    Option<T1> option1,
    Option<T2> option2,
    Option<T3> option3)
{
    if (option1.IsNone || option2.IsNone || option3.IsNone)
        return Option<(T1, T2, T3)>.None;
        
    return Option.Some((option1.Value, option2.Value, option3.Value));
}

// Usage
Option<User> user = GetUser();
Option<Profile> profile = GetProfile();
Option<Settings> settings = GetSettings();

Option<Dashboard> dashboard = Option.Zip(user, profile, settings)
    .Map((u, p, s) => new Dashboard(u, p, s));
```

## Performance Optimizations

### Struct-Based Optimizations

The generator creates specialized implementations that avoid boxing:

```csharp
// Generated for common value types
[MethodImpl(MethodImplOptions.AggressiveInlining)]
public static Result<int> MapInt(this Result<int> result, Func<int, int> mapper)
{
    // Optimized path avoiding allocations
    return result._isSuccess 
        ? new Result<int>(mapper(result._value))
        : new Result<int>(result._error);
}
```

### Inlined Operations

For hot paths, generators create inlined versions:

```csharp
// Generated with aggressive inlining
[MethodImpl(MethodImplOptions.AggressiveInlining)]
public static Result<T> Ensure<T>(
    this Result<T> result, 
    [ConstantExpected] bool condition, 
    string errorMessage) where T : struct
{
    if (!result._isSuccess) return result;
    return condition ? result : Result.Failure<T>(errorMessage);
}
```

### Async Method Optimizations

Optimized async extensions:

```csharp
// Generated async extensions
public static async Task<Result<TResult>> MapAsync<T, TResult>(
    this Task<Result<T>> resultTask,
    Func<T, TResult> mapper)
{
    var result = await resultTask.ConfigureAwait(false);
    return result.IsSuccess 
        ? Result.Success(mapper(result.Value))
        : Result.Failure<TResult>(result.Error);
}

// With cancellation token support
public static async Task<Result<TResult>> BindAsync<T, TResult>(
    this Result<T> result,
    Func<T, CancellationToken, Task<Result<TResult>>> binder,
    CancellationToken cancellationToken = default)
{
    if (result.IsFailure) 
        return Result.Failure<TResult>(result.Error);
        
    return await binder(result.Value, cancellationToken).ConfigureAwait(false);
}
```

## Customization Options

### Generator Configuration

Configure generators in your project file:

```xml
<PropertyGroup>
  <!-- Enable/disable specific generators -->
  <FluentUnions_GenerateMapExtensions>true</FluentUnions_GenerateMapExtensions>
  <FluentUnions_GenerateBindExtensions>true</FluentUnions_GenerateBindExtensions>
  <FluentUnions_GenerateAsyncExtensions>true</FluentUnions_GenerateAsyncExtensions>
  
  <!-- Set maximum tuple size (default: 8) -->
  <FluentUnions_MaxTupleSize>8</FluentUnions_MaxTupleSize>
  
  <!-- Enable performance optimizations -->
  <FluentUnions_EnableOptimizations>true</FluentUnions_EnableOptimizations>
</PropertyGroup>
```

### Assembly Attributes

Control generation with attributes:

```csharp
// Disable generation for specific types
[assembly: FluentUnions.DisableGeneration(typeof(MyCustomType))]

// Enable additional optimizations
[assembly: FluentUnions.EnableAggressiveInlining]

// Customize generated namespace
[assembly: FluentUnions.GeneratedNamespace("MyApp.Generated")]
```

### Partial Method Hooks

The generator supports partial methods for customization:

```csharp
public partial class CustomExtensions
{
    // Generator will call this if defined
    static partial void OnBeforeMap<T, TResult>(Result<T> result);
    static partial void OnAfterMap<T, TResult>(Result<TResult> result);
}
```

## Troubleshooting

### Common Issues

#### 1. Generated Code Not Available

**Problem**: IntelliSense doesn't show generated methods.

**Solutions**:
- Rebuild the project
- Restart your IDE
- Check build output for generator errors
- Ensure generators are enabled in project

#### 2. Compilation Errors in Generated Code

**Problem**: Build fails with errors in generated files.

**Solutions**:
```xml
<!-- Temporarily disable to isolate issues -->
<PropertyGroup>
  <DisableFluentUnionsGenerators>true</DisableFluentUnionsGenerators>
</PropertyGroup>
```

#### 3. Performance Issues

**Problem**: Build times increased significantly.

**Solutions**:
```xml
<!-- Reduce generated code -->
<PropertyGroup>
  <FluentUnions_MaxTupleSize>4</FluentUnions_MaxTupleSize>
  <FluentUnions_GenerateAsyncExtensions>false</FluentUnions_GenerateAsyncExtensions>
</PropertyGroup>
```

### Viewing Generated Code

#### Visual Studio
1. Enable "Show All Files" in Solution Explorer
2. Navigate to: obj → Debug → generated → FluentUnions.Generators
3. Open .g.cs files

#### Command Line
```bash
# Find generated files
find obj -name "*.g.cs" | grep FluentUnions

# View specific generated file
cat obj/Debug/net8.0/generated/FluentUnions.Generators/FluentUnions.Generators.ResultExtensions.g.cs
```

### Debugging Generators

Enable detailed logging:

```xml
<PropertyGroup>
  <EmitCompilerGeneratedFiles>true</EmitCompilerGeneratedFiles>
  <CompilerGeneratedFilesOutputPath>$(BaseIntermediateOutputPath)Generated</CompilerGeneratedFilesOutputPath>
</PropertyGroup>
```

## Generated Code Examples

### Complete BindAll Example

Here's what the generator creates for a 4-parameter BindAll:

```csharp
// Generated by FluentUnions.Generators
#nullable enable

namespace FluentUnions.Extensions
{
    public static partial class ResultExtensions
    {
        /// <summary>
        /// Combines four Results into a single Result containing a tuple of values.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Result<(T1, T2, T3, T4)> BindAll<T1, T2, T3, T4>(
            Result<T1> result1,
            Result<T2> result2,
            Result<T3> result3,
            Result<T4> result4)
        {
            if (result1.IsFailure) return Result.Failure<(T1, T2, T3, T4)>(result1.Error);
            if (result2.IsFailure) return Result.Failure<(T1, T2, T3, T4)>(result2.Error);
            if (result3.IsFailure) return Result.Failure<(T1, T2, T3, T4)>(result3.Error);
            if (result4.IsFailure) return Result.Failure<(T1, T2, T3, T4)>(result4.Error);
            
            return Result.Success((result1.Value, result2.Value, result3.Value, result4.Value));
        }
        
        /// <summary>
        /// Combines four Results and maps the combined values to a new type.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Result<TResult> BindAll<T1, T2, T3, T4, TResult>(
            Result<T1> result1,
            Result<T2> result2,
            Result<T3> result3,
            Result<T4> result4,
            Func<T1, T2, T3, T4, TResult> mapper)
        {
            if (result1.IsFailure) return Result.Failure<TResult>(result1.Error);
            if (result2.IsFailure) return Result.Failure<TResult>(result2.Error);
            if (result3.IsFailure) return Result.Failure<TResult>(result3.Error);
            if (result4.IsFailure) return Result.Failure<TResult>(result4.Error);
            
            return Result.Success(mapper(result1.Value, result2.Value, result3.Value, result4.Value));
        }
    }
}
```

### Async Extensions Example

Generated async operation support:

```csharp
// Generated async extensions
public static partial class ResultAsyncExtensions
{
    /// <summary>
    /// Asynchronously maps the value of a successful Result.
    /// </summary>
    public static async Task<Result<TResult>> MapAsync<T, TResult>(
        this Result<T> result,
        Func<T, Task<TResult>> mapper)
    {
        if (result.IsFailure)
            return Result.Failure<TResult>(result.Error);
            
        try
        {
            var mappedValue = await mapper(result.Value).ConfigureAwait(false);
            return Result.Success(mappedValue);
        }
        catch (Exception ex)
        {
            return Result.Failure<TResult>(new Error("ASYNC_MAP_ERROR", ex.Message));
        }
    }
    
    /// <summary>
    /// Asynchronously binds a Result-returning async operation.
    /// </summary>
    public static async Task<Result<TResult>> BindAsync<T, TResult>(
        this Task<Result<T>> resultTask,
        Func<T, Task<Result<TResult>>> binder)
    {
        var result = await resultTask.ConfigureAwait(false);
        
        if (result.IsFailure)
            return Result.Failure<TResult>(result.Error);
            
        return await binder(result.Value).ConfigureAwait(false);
    }
}
```

### Option Extensions Example

Generated Option combinators:

```csharp
// Generated Option extensions
public static partial class OptionExtensions
{
    /// <summary>
    /// Combines multiple Options using a selector function.
    /// </summary>
    public static Option<TResult> SelectMany<T1, T2, T3, TResult>(
        this Option<T1> option1,
        Func<T1, Option<T2>> selector2,
        Func<T1, Option<T3>> selector3,
        Func<T1, T2, T3, TResult> resultSelector)
    {
        if (option1.IsNone) return Option<TResult>.None;
        
        var value1 = option1.Value;
        var option2 = selector2(value1);
        if (option2.IsNone) return Option<TResult>.None;
        
        var option3 = selector3(value1);
        if (option3.IsNone) return Option<TResult>.None;
        
        return Option.Some(resultSelector(value1, option2.Value, option3.Value));
    }
}
```

### Collection Extensions

Generated extensions for collections:

```csharp
// Generated collection extensions
public static partial class ResultCollectionExtensions
{
    /// <summary>
    /// Transforms a collection of Results into a Result of collection.
    /// </summary>
    public static Result<IReadOnlyList<T>> Combine<T>(
        this IEnumerable<Result<T>> results)
    {
        var resultsList = results as IList<Result<T>> ?? results.ToList();
        var values = new List<T>(resultsList.Count);
        
        foreach (var result in resultsList)
        {
            if (result.IsFailure)
                return Result.Failure<IReadOnlyList<T>>(result.Error);
                
            values.Add(result.Value);
        }
        
        return Result.Success<IReadOnlyList<T>>(values);
    }
    
    /// <summary>
    /// Filters and unwraps successful Results from a collection.
    /// </summary>
    public static IEnumerable<T> WhereSuccess<T>(
        this IEnumerable<Result<T>> results)
    {
        foreach (var result in results)
        {
            if (result.IsSuccess)
                yield return result.Value;
        }
    }
}
```

## Best Practices

### 1. Leverage Multi-Value Operations

```csharp
// Instead of nested operations
var result = GetUser(id)
    .Bind(user => GetAccount(user.AccountId)
        .Bind(account => GetSettings(user.Id)
            .Map(settings => new UserDashboard(user, account, settings))));

// Use generated BindAll
var result = Result.BindAll(
    GetUser(id),
    GetAccount(accountId),
    GetSettings(id)
).Map((user, account, settings) => new UserDashboard(user, account, settings));
```

### 2. Use Generated Async Extensions

```csharp
// Generated extensions handle ConfigureAwait automatically
var result = await GetUserAsync(id)
    .BindAsync(user => UpdateUserAsync(user))
    .MapAsync(user => user.ToDto());
```

### 3. Check Generated Code

When debugging, examine the generated code to understand performance characteristics:

```csharp
// Check obj/Debug/generated folder to see actual implementation
// Useful for performance-critical paths
```

## Summary

FluentUnions source generators provide:

1. **Multi-value support** - BindAll and Zip for multiple Results/Options
2. **Performance optimizations** - Specialized implementations for common cases
3. **Reduced boilerplate** - Generated extension methods for common patterns
4. **Type safety** - Compile-time checked operations
5. **Async support** - Properly configured async extensions

The generators run at compile time, producing optimized code without runtime overhead.

Next steps:
- [Analyzer Rules Reference](analyzers.md)
- [Performance Guide](../guides/performance-best-practices.md)
- [API Reference](result-api.md)
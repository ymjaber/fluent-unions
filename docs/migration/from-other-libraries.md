# Migrating from Other Result/Option Libraries

This guide helps you migrate from other functional programming libraries to FluentUnions, including LanguageExt, CSharpFunctionalExtensions, and others.

## Table of Contents
1. [Library Comparison](#library-comparison)
2. [From LanguageExt](#from-languageext)
3. [From CSharpFunctionalExtensions](#from-csharpfunctionalextensions)
4. [From ErrorOr](#from-erroror)
5. [From OneOf](#from-oneof)
6. [Common Migration Patterns](#common-migration-patterns)
7. [Feature Mapping](#feature-mapping)
8. [Best Practices](#best-practices)

## Library Comparison

### Quick Comparison Table

| Feature | FluentUnions | LanguageExt | CSharpFunctionalExtensions | ErrorOr | OneOf |
|---------|-------------|-------------|---------------------------|---------|-------|
| Result Type | `Result<T>` | `Either<L,R>`, `Try<T>` | `Result<T,E>` | `ErrorOr<T>` | `OneOf<T0,T1>` |
| Option Type | `Option<T>` | `Option<T>` | `Maybe<T>` | - | - |
| Error Types | Built-in hierarchy | Custom `L` type | Custom `E` type | `Error` list | Union types |
| Async Support | Full | Full | Partial | Partial | Basic |
| Source Generators | ✓ | ✗ | ✗ | ✓ | ✓ |
| Analyzers | ✓ | ✗ | ✗ | ✗ | ✗ |
| Performance | Optimized | Good | Good | Excellent | Excellent |
| Learning Curve | Gentle | Steep | Moderate | Gentle | Moderate |

## From LanguageExt

### Type Mappings

```csharp
// LanguageExt → FluentUnions
Either<Error, User>     → Result<User>
Option<User>           → Option<User>
Try<User>              → Result<User>
Validation<Error, User> → Result<User>
Seq<User>              → IEnumerable<User>
Lst<User>              → List<User>
```

### Basic Operations

**LanguageExt:**
```csharp
using LanguageExt;
using static LanguageExt.Prelude;

public Either<Error, User> GetUser(int id)
{
    if (id <= 0)
        return Left(Error.New("Invalid ID"));
    
    var user = _repository.Find(id);
    return user != null 
        ? Right(user)
        : Left(Error.New("User not found"));
}

public Either<Error, string> GetUserName(int id)
{
    return GetUser(id)
        .Map(user => user.Name)
        .MapLeft(err => Error.New($"Failed: {err.Message}"));
}
```

**FluentUnions:**
```csharp
using FluentUnions;

public Result<User> GetUser(int id)
{
    if (id <= 0)
        return new ValidationError("Invalid ID");
    
    var user = _repository.Find(id);
    return user != null 
        ? Result.Success(user)
        : new NotFoundError("User", id);
}

public Result<string> GetUserName(int id)
{
    return GetUser(id)
        .Map(user => user.Name)
        .MapError(err => new Error("FAILED", $"Failed: {err.Message}"));
}
```

### Monad Operations

**LanguageExt:**
```csharp
// Bind operation
Either<Error, Order> CreateOrder(int userId, int productId)
{
    return from user in GetUser(userId)
           from product in GetProduct(productId)
           from inventory in CheckInventory(product)
           select new Order(user, product);
}

// Match operation
string ProcessResult(Either<Error, User> result)
{
    return result.Match(
        Right: user => $"Hello {user.Name}",
        Left: error => $"Error: {error.Message}"
    );
}
```

**FluentUnions:**
```csharp
// Bind operation
Result<Order> CreateOrder(int userId, int productId)
{
    return from user in GetUser(userId)
           from product in GetProduct(productId)
           from inventory in CheckInventory(product)
           select new Order(user, product);
}

// Match operation
string ProcessResult(Result<User> result)
{
    return result.Match(
        onSuccess: user => $"Hello {user.Name}",
        onFailure: error => $"Error: {error.Message}"
    );
}
```

### Option Type Migration

**LanguageExt:**
```csharp
Option<User> FindUser(string email)
{
    var user = _repository.FindByEmail(email);
    return Optional(user);
}

int GetUserAge(string email)
{
    return FindUser(email)
        .Map(u => u.Age)
        .IfNone(0);
}

Option<Address> GetUserAddress(string email)
{
    return FindUser(email)
        .Bind(u => Optional(u.Address));
}
```

**FluentUnions:**
```csharp
Option<User> FindUser(string email)
{
    var user = _repository.FindByEmail(email);
    return Option.FromNullable(user);
}

int GetUserAge(string email)
{
    return FindUser(email)
        .Map(u => u.Age)
        .GetValueOr(0);
}

Option<Address> GetUserAddress(string email)
{
    return FindUser(email)
        .Bind(u => Option.FromNullable(u.Address));
}
```

### Advanced Patterns

**LanguageExt Try/Catch:**
```csharp
Try<User> GetUserSafe(int id)
{
    return Try(() => 
    {
        if (id <= 0) throw new ArgumentException("Invalid ID");
        return _repository.GetUserOrThrow(id);
    });
}

Either<Error, User> result = GetUserSafe(123)
    .ToEither(ex => Error.New(ex.Message));
```

**FluentUnions:**
```csharp
Result<User> GetUserSafe(int id)
{
    return Result.Try(() => 
    {
        if (id <= 0) throw new ArgumentException("Invalid ID");
        return _repository.GetUserOrThrow(id);
    }, ex => new Error("GET_USER_FAILED", ex.Message));
}
```

### Migration Wrapper

```csharp
// Adapter for gradual migration
public static class LanguageExtAdapter
{
    public static Result<T> ToResult<T>(this Either<Error, T> either)
    {
        return either.Match(
            Right: value => Result.Success(value),
            Left: error => Result.Failure<T>(
                new Error(error.Code, error.Message))
        );
    }
    
    public static Either<Error, T> ToEither<T>(this Result<T> result)
    {
        return result.Match(
            onSuccess: value => Right<Error, T>(value),
            onFailure: error => Left<Error, T>(
                Error.New(error.Message))
        );
    }
    
    public static Option<T> ToFluentOption<T>(this LanguageExt.Option<T> option)
    {
        return option.Match(
            Some: value => Option.Some(value),
            None: () => Option.None<T>()
        );
    }
}
```

## From CSharpFunctionalExtensions

### Type Mappings

```csharp
// CSharpFunctionalExtensions → FluentUnions
Result<T, E>    → Result<T>
Result          → Result
Maybe<T>        → Option<T>
ValueObject     → (use records or custom types)
Entity          → (use domain entities)
```

### Basic Operations

**CSharpFunctionalExtensions:**
```csharp
using CSharpFunctionalExtensions;

public Result<User, Error> GetUser(int id)
{
    if (id <= 0)
        return Result.Failure<User, Error>(
            new Error("Invalid ID"));
    
    var user = _repository.Find(id);
    return user != null 
        ? Result.Success<User, Error>(user)
        : Result.Failure<User, Error>(
            new Error("User not found"));
}

public Result<string, Error> ProcessUser(int id)
{
    return GetUser(id)
        .Map(user => user.Name)
        .Bind(name => ValidateName(name))
        .Tap(name => Console.WriteLine(name));
}
```

**FluentUnions:**
```csharp
using FluentUnions;

public Result<User> GetUser(int id)
{
    if (id <= 0)
        return new ValidationError("Invalid ID");
    
    var user = _repository.Find(id);
    return user != null 
        ? Result.Success(user)
        : new NotFoundError("User", id);
}

public Result<string> ProcessUser(int id)
{
    return GetUser(id)
        .Map(user => user.Name)
        .Bind(name => ValidateName(name))
        .Tap(name => Console.WriteLine(name));
}
```

### Maybe/Option Migration

**CSharpFunctionalExtensions:**
```csharp
public Maybe<User> FindUser(string email)
{
    var user = _repository.FindByEmail(email);
    return Maybe<User>.From(user);
}

public string GetUserDisplayName(string email)
{
    return FindUser(email)
        .Map(u => u.DisplayName)
        .Unwrap("Anonymous");
}
```

**FluentUnions:**
```csharp
public Option<User> FindUser(string email)
{
    var user = _repository.FindByEmail(email);
    return Option.FromNullable(user);
}

public string GetUserDisplayName(string email)
{
    return FindUser(email)
        .Map(u => u.DisplayName)
        .GetValueOr("Anonymous");
}
```

### Result Without Value

**CSharpFunctionalExtensions:**
```csharp
public Result DeleteUser(int id)
{
    var user = _repository.Find(id);
    if (user == null)
        return Result.Failure("User not found");
    
    _repository.Delete(user);
    return Result.Success();
}
```

**FluentUnions:**
```csharp
public Result DeleteUser(int id)
{
    var user = _repository.Find(id);
    if (user == null)
        return new NotFoundError("User", id);
    
    _repository.Delete(user);
    return Result.Success();
}
```

### Migration Helper

```csharp
public static class CSharpFunctionalExtensionsAdapter
{
    public static Result<T> ToFluentResult<T, E>(
        this Result<T, E> result) where E : class
    {
        return result.IsSuccess
            ? Result.Success(result.Value)
            : Result.Failure<T>(new Error("ERROR", result.Error.ToString()));
    }
    
    public static Option<T> ToFluentOption<T>(this Maybe<T> maybe)
    {
        return maybe.HasValue
            ? Option.Some(maybe.Value)
            : Option.None<T>();
    }
}
```

## From ErrorOr

### Type Mappings

```csharp
// ErrorOr → FluentUnions
ErrorOr<T>      → Result<T>
List<Error>     → Error (single) or AggregateError (multiple)
ErrorType       → Built-in error types
```

### Basic Operations

**ErrorOr:**
```csharp
using ErrorOr;

public ErrorOr<User> GetUser(int id)
{
    if (id <= 0)
        return Error.Validation("Invalid ID");
    
    var user = _repository.Find(id);
    if (user == null)
        return Error.NotFound("User not found");
    
    return user;
}

public ErrorOr<Created<User>> CreateUser(CreateUserRequest request)
{
    List<Error> errors = new();
    
    if (string.IsNullOrEmpty(request.Email))
        errors.Add(Error.Validation("Email is required"));
    
    if (request.Age < 18)
        errors.Add(Error.Validation("Must be 18 or older"));
    
    if (errors.Any())
        return errors;
    
    var user = new User(request);
    return Created(user);
}
```

**FluentUnions:**
```csharp
using FluentUnions;

public Result<User> GetUser(int id)
{
    if (id <= 0)
        return new ValidationError("Invalid ID");
    
    var user = _repository.Find(id);
    if (user == null)
        return new NotFoundError("User", id);
    
    return Result.Success(user);
}

public Result<User> CreateUser(CreateUserRequest request)
{
    var errors = new List<Error>();
    
    if (string.IsNullOrEmpty(request.Email))
        errors.Add(new ValidationError("email", "Email is required"));
    
    if (request.Age < 18)
        errors.Add(new ValidationError("age", "Must be 18 or older"));
    
    if (errors.Any())
        return new AggregateError("Validation failed", errors);
    
    var user = new User(request);
    return Result.Success(user);
}
```

### Error Handling

**ErrorOr:**
```csharp
errorOr.Match(
    value => Console.WriteLine(value),
    errors => errors.ForEach(e => Console.WriteLine(e.Description))
);

errorOr.MatchFirst(
    value => Console.WriteLine(value),
    error => Console.WriteLine(error.Description)
);
```

**FluentUnions:**
```csharp
result.Match(
    onSuccess: value => Console.WriteLine(value),
    onFailure: error =>
    {
        if (error is AggregateError agg)
            agg.Errors.ForEach(e => Console.WriteLine(e.Message));
        else
            Console.WriteLine(error.Message);
    }
);
```

### Migration Adapter

```csharp
public static class ErrorOrAdapter
{
    public static Result<T> ToResult<T>(this ErrorOr<T> errorOr)
    {
        if (errorOr.IsError)
        {
            var errors = errorOr.Errors.Select(e => ConvertError(e)).ToList();
            return errors.Count == 1
                ? Result.Failure<T>(errors.First())
                : Result.Failure<T>(new AggregateError("Multiple errors", errors));
        }
        
        return Result.Success(errorOr.Value);
    }
    
    private static Error ConvertError(ErrorOr.Error error)
    {
        return error.Type switch
        {
            ErrorType.Validation => new ValidationError(error.Description),
            ErrorType.NotFound => new NotFoundError(error.Description),
            ErrorType.Conflict => new ConflictError(error.Description),
            _ => new Error(error.Code, error.Description)
        };
    }
}
```

## From OneOf

### Type Mappings

```csharp
// OneOf → FluentUnions
OneOf<T0, T1>          → Result<T0> (if T1 is error-like)
OneOf<T0, T1, T2>      → Result<T0> with custom errors
OneOf<Success, Error>  → Result
```

### Basic Operations

**OneOf:**
```csharp
using OneOf;

public OneOf<User, NotFound, ValidationError> GetUser(int id)
{
    if (id <= 0)
        return new ValidationError("Invalid ID");
    
    var user = _repository.Find(id);
    if (user == null)
        return new NotFound();
    
    return user;
}

public string HandleUser(OneOf<User, NotFound, ValidationError> result)
{
    return result.Match(
        user => $"Hello {user.Name}",
        notFound => "User not found",
        validation => $"Validation error: {validation.Message}"
    );
}
```

**FluentUnions:**
```csharp
using FluentUnions;

public Result<User> GetUser(int id)
{
    if (id <= 0)
        return new ValidationError("Invalid ID");
    
    var user = _repository.Find(id);
    if (user == null)
        return new NotFoundError("User", id);
    
    return Result.Success(user);
}

public string HandleUser(Result<User> result)
{
    return result.Match(
        onSuccess: user => $"Hello {user.Name}",
        onFailure: error => error switch
        {
            NotFoundError => "User not found",
            ValidationError ve => $"Validation error: {ve.Message}",
            _ => "An error occurred"
        }
    );
}
```

### Complex Union Types

**OneOf:**
```csharp
public OneOf<
    Created<User>, 
    Updated<User>, 
    ValidationErrors, 
    NotFound, 
    Conflict> 
SaveUser(UserRequest request)
{
    // Complex branching logic
}
```

**FluentUnions:**
```csharp
public Result<UserSaveResult> SaveUser(UserRequest request)
{
    // Return specific result types
    return Result.Success(new UserSaveResult
    {
        Action = SaveAction.Created, // or Updated
        User = user
    });
}

public enum SaveAction { Created, Updated }
public record UserSaveResult(SaveAction Action, User User);
```

## Common Migration Patterns

### Generic Migration Strategy

```csharp
public interface IResultAdapter<TSource, TTarget>
{
    Result<TTarget> Convert(TSource source);
}

// Base adapter for common patterns
public abstract class ResultAdapterBase<TSource, TTarget> 
    : IResultAdapter<TSource, TTarget>
{
    public abstract Result<TTarget> Convert(TSource source);
    
    protected Result<T> Success<T>(T value) => Result.Success(value);
    protected Result<T> Failure<T>(string message) => 
        Result.Failure<T>(new Error("CONVERSION_ERROR", message));
}
```

### Async Migration

**From various libraries:**
```csharp
// LanguageExt
Task<Either<Error, User>> GetUserAsync(int id);

// CSharpFunctionalExtensions  
Task<Result<User, Error>> GetUserAsync(int id);

// ErrorOr
Task<ErrorOr<User>> GetUserAsync(int id);

// OneOf
Task<OneOf<User, Error>> GetUserAsync(int id);
```

**To FluentUnions:**
```csharp
Task<Result<User>> GetUserAsync(int id);

// With extension method support
public static async Task<Result<T>> ToResultAsync<T>(
    this Task<Either<Error, T>> task)
{
    var either = await task;
    return either.ToResult();
}
```

### Collection Operations

**Migrating collection operations:**
```csharp
// From LanguageExt
Seq<User> users = GetUsers();
var names = users.Map(u => u.Name).ToList();

// To FluentUnions
IEnumerable<User> users = GetUsers();
var names = users.Select(u => u.Name).ToList();

// From functional collection with Option
Seq<Option<User>> maybeUsers = GetMaybeUsers();
var validUsers = maybeUsers.Somes().ToList();

// To FluentUnions
IEnumerable<Option<User>> maybeUsers = GetMaybeUsers();
var validUsers = maybeUsers.Where(o => o.IsSome)
                           .Select(o => o.Value)
                           .ToList();
```

## Feature Mapping

### Error Handling Features

| Feature | LanguageExt | CSharpFunctionalExtensions | FluentUnions |
|---------|-------------|---------------------------|--------------|
| Custom Errors | `L` in `Either<L,R>` | `E` in `Result<T,E>` | Error hierarchy |
| Error Aggregation | `Validation<L,R>` | Manual | `AggregateError` |
| Error Context | In `L` type | In `E` type | Error.Metadata |
| Error Recovery | `IfLeft` | `OnFailure` | `Compensate` |

### Functional Features

| Feature | LanguageExt | CSharpFunctionalExtensions | FluentUnions |
|---------|-------------|---------------------------|--------------|
| Map | ✓ | ✓ | ✓ |
| Bind/FlatMap | ✓ | ✓ | ✓ |
| Filter | ✓ (Option) | Where (Maybe) | ✓ |
| Traverse | ✓ | Manual | Via extensions |
| Sequence | ✓ | Manual | `BindAll` |

## Best Practices

### 1. Start with Core Types

Focus on migrating the most used types first:
```csharp
// Priority 1: Result/Either types
// Priority 2: Option/Maybe types  
// Priority 3: Specialized types (Try, Validation, etc.)
// Priority 4: Collection types
```

### 2. Create Migration Boundaries

```csharp
// Create a boundary layer during migration
public interface IUserService
{
    // Old signature (hidden)
    internal Either<Error, User> GetUserEither(int id);
    
    // New signature (exposed)
    Result<User> GetUser(int id);
}

public class UserService : IUserService
{
    Either<Error, User> IUserService.GetUserEither(int id) => 
        GetUser(id).ToEither();
        
    public Result<User> GetUser(int id)
    {
        // New implementation
    }
}
```

### 3. Maintain Semantic Equivalence

```csharp
// Ensure error semantics are preserved
public static Error ConvertLanguageExtError(LanguageExt.Common.Error error)
{
    return error.Type switch
    {
        ErrorType.Validation => new ValidationError(error.Message),
        ErrorType.NotFound => new NotFoundError(error.Message),
        _ => new Error(error.Type.ToString(), error.Message)
    };
}
```

### 4. Test Migration Thoroughly

```csharp
[TestFixture]
public class MigrationTests
{
    [Test]
    public void Result_Conversion_Preserves_Success_Value()
    {
        var either = Right<Error, int>(42);
        var result = either.ToResult();
        
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.EqualTo(42));
    }
    
    [Test]
    public void Result_Conversion_Preserves_Error()
    {
        var either = Left<Error, int>(Error.New("Test error"));
        var result = either.ToResult();
        
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Error.Message, Is.EqualTo("Test error"));
    }
}
```

### 5. Gradual Migration Checklist

- [ ] Create type mappings for your specific usage
- [ ] Build adapter/extension methods
- [ ] Migrate leaf functions first (no dependencies)
- [ ] Update tests to use new assertions
- [ ] Migrate intermediate layers
- [ ] Update public APIs last
- [ ] Remove old library references
- [ ] Update documentation

## Summary

Migrating to FluentUnions from other libraries:

1. **Similar concepts** - Most functional libraries share core concepts
2. **Type safety** - FluentUnions maintains type safety during migration
3. **Better integration** - Purpose-built for C# with analyzers and generators
4. **Gradual migration** - Can coexist with other libraries during transition
5. **Performance** - Optimized for .NET with minimal allocations

Key advantages of FluentUnions:
- Built-in error type hierarchy
- Roslyn analyzers for compile-time safety
- Source generators for performance
- Designed specifically for C#/.NET
- Comprehensive async support
- Rich ecosystem of extensions

Next steps:
- [Getting Started](../getting-started/installation.md)
- [Result Pattern Basics](../tutorials/result-pattern-basics.md)
- [API Reference](../reference/result-api.md)
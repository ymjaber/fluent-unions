# FluentUnions

[![NuGet](https://img.shields.io/nuget/v/FluentUnions.svg)](https://www.nuget.org/packages/FluentUnions/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

FluentUnions is a comprehensive discriminated unions library for C#/.NET that provides robust error handling with `Result<T>` and `Result`, and elegant null safety with `Option<T>`. These types are discriminated unions that ensure you handle all possible cases - either success or failure for Result, and some or none for Option. It features a fluent API with monadic operations, source generators for performance, and static analyzers to prevent common mistakes.

## Why FluentUnions?

Traditional error handling in C# often relies on exceptions or null values, which can lead to:

- **Hidden control flow** - Exceptions can be thrown from anywhere
- **Performance overhead** - Exception handling is expensive
- **Null reference exceptions** - The billion-dollar mistake
- **Poor error context** - Generic exceptions lack business context

FluentUnions solves these problems by making errors and absent values explicit in your type system.

## Key Features

- **🎯 True Discriminated Unions** - Type-safe `Result<T>` and `Option<T>` with exhaustive matching
- **💯 Type-Safe Error Handling** - No more exceptions for expected failures
- **🛡️ Null Safety** - `Option<T>` eliminates null reference exceptions
- **🔗 Fluent API** - Chain operations with `Map`, `Bind`, `Match`, and more
- **⚡ Performance** - Zero-allocation struct-based abstractions
- **🔍 Static Analysis** - Roslyn analyzers ensure exhaustive handling
- **📦 Rich Error Types** - Built-in error types with metadata support
- **🎨 Functional Patterns** - Monadic operations for elegant code
- **🔄 Async Support** - Full async/await support throughout
- **📝 JSON Serialization** - Built-in System.Text.Json support
- **🚀 Source Generators** - Auto-generated methods for tuple operations
- **✅ Predefined Validations** - Built-in validation methods for common scenarios

## Installation

### Core Package

```bash
dotnet add package FluentUnions
```

Or via Package Manager:

```powershell
Install-Package FluentUnions
```

### Testing Support (Optional)

For test assertions with [AwesomeAssertions](https://github.com/AwesomeAssertions/AwesomeAssertions):

```bash
dotnet add package FluentUnions.AwesomeAssertions
```

Or via Package Manager:

```powershell
Install-Package FluentUnions.AwesomeAssertions
```

## Quick Start

### Result Type - Error Handling

```csharp
using FluentUnions;

// Define a method that can fail
public Result<User> GetUser(int id)
{
    if (id <= 0)
        return new ValidationError("Invalid user ID");
        // OR without implicit conversion:
        // return Result.Failure<User>(new ValidationError("Invalid user ID"));

    var user = database.FindUser(id);
    if (user == null)
        return new NotFoundError($"User {id} not found");
V
    return user;
    // OR without implicit conversion:
    // return Result.Success(user);
}

// Use the result
var result = GetUser(123)
    .Map(user => user.Name)
    .Bind(name => ValidateName(name));

// Pattern matching
var message = result.Match(
    success: name => $"Hello, {name}!",
    failure: error => $"Error: {error.Message}"
);
```

### Option Type - Null Safety

```csharp
using FluentUnions;

// No more nulls!
public Option<string> GetMiddleName(User user)
{
    return user.MiddleName.AsOption(); // Converts null to None
}

// Chain operations safely
var greeting = GetUser(123)
    .ToOption() // Convert Result to Option
    .Bind(user => GetMiddleName(user))
    .Map(name => name.ToUpper())
    .Match(
        some: name => $"Middle name: {name}",
        none: () => "No middle name"
    );
```

### Validation with Predefined Methods

```csharp
// Single value validation with fluent pattern
public Result<string> ValidateEmail(string email)
{
    return Result.For(email)
        .Ensure.NotEmpty()              // Predefined: not null or empty
        .Ensure.Matches(@"^[^@]+@[^@]+\.[^@]+$")  // Email regex
        .Ensure.ShorterThan(255);       // Predefined: length validation
}

// Multiple validations with error accumulation
public Result<User> CreateUser(string email, string password, int age)
{
    // BindAllAppend collects ALL validation errors
    return Result.BindAllAppend(
        Result.For(email)
            .Ensure.NotEmpty()
            .Ensure.Matches(@"^[^@]+@[^@]+\.[^@]+$"),
        Result.For(password)
            .Ensure.LongerThanOrEqualTo(8)
            .Ensure.Matches(@"[A-Z]", "Must contain uppercase")
            .Ensure.Matches(@"[0-9]", "Must contain number"),
        Result.For(age)
            .Ensure.GreaterThanOrEqualTo(18))
        .Map((email, password, age) => new User(email, password, age));
}

// If multiple validations fail, returns AggregateError with all failures
}
```

### Static Analyzers in Action

FluentUnions includes Roslyn analyzers that catch common mistakes. These analyzers work with any IDE or editor that supports Roslyn analyzers - Visual Studio, VS Code, Rider, Neovim (with OmniSharp/LSP), Emacs, and more:

```csharp
// ⚠️ FU0001: Accessing Value without checking IsSome
Option<int> option = GetSomeOption();
var value = option.Value; // Analyzer warning!

// ✅ Safe access
if (option.IsSome)
{
    var value = option.Value; // OK
}

// ⚠️ FU0101: Accessing Value without checking IsSuccess
Result<string> result = GetSomeResult();
var data = result.Value; // Analyzer warning!

// ✅ Better: Use Match
var data = result.Match(
    success: value => value,
    failure: error => "default"
);
```

## Core Concepts

### Discriminated Unions

FluentUnions provides true discriminated unions (also known as tagged unions, disjoint unions or sum types) for C#. A discriminated union is a type that can be exactly one of several named cases, and you must handle all cases explicitly.

```csharp
// Result<T> is a discriminated union of Success(T value) | Failure(Error error)
Result<int> result = CalculateSomething();

// The compiler and analyzers ensure you handle both cases
var message = result.Match(
    success: value => $"Got {value}",
    failure: error => $"Failed: {error.Message}"
);

// Option<T> is a discriminated union of Some(T value) | None
Option<string> name = GetOptionalName();

// Again, you must handle both cases
var display = name.Match(
    some: n => $"Hello, {n}",
    none: () => "Hello, stranger"
);
```

Unlike traditional C# patterns using exceptions or nulls, discriminated unions:

- Make all possible states explicit in the type system
- Force you to handle all cases (no forgotten null checks!)
- Enable exhaustive pattern matching
- Provide compile-time safety

### Result Types

- **`Result`** - A discriminated union representing Success | Failure(Error)
- **`Result<T>`** - A discriminated union representing Success(T) | Failure(Error)

### Option Type

- **`Option<T>`** - A discriminated union representing Some(T) | None

### Error Types

FluentUnions provides a rich hierarchy of error types:

- `Error` - Base error type
- `ValidationError` - Input validation failures
- `NotFoundError` - Resource not found
- `ConflictError` - Business rule conflicts
- `AuthenticationError` - Authentication failures
- `AuthorizationError` - Authorization failures
- `AggregateError` - Multiple errors combined

### Error Accumulation

FluentUnions offers two error handling strategies:

1. **Short-circuit (default)** - Stop at first error using `Bind`, `Ensure`
2. **Accumulate all errors** - Collect all errors using `BindAll`, `BindAllAppend`, `EnsureAll`

```csharp
// Short-circuit: stops at first error
var result = Result.For(email)
    .Ensure.NotEmpty()          // If this fails...
    .Ensure.Matches(@"@")       // ...this is never checked
    .Bind(e => CheckDomain(e)); // ...and this never runs

// Accumulation: collects ALL errors
var result = Result.BindAllAppend(
    Result.For(email).Ensure.NotEmpty(),
    Result.For(password).Ensure.LongerThanOrEqualTo(8),
    Result.For(age).Ensure.GreaterThanOrEqualTo(18)
);
// Returns single Error or AggregateError with all failures
```

This is especially useful for form validation where users should see all errors at once.

## Documentation

For comprehensive documentation, visit the [docs](./docs) folder:

- [Getting Started Guide](./docs/getting-started/quick-start.md)
- [Discriminated Unions](./docs/core-concepts/discriminated-unions.md)
- [Error Types](./docs/core-concepts/error-types.md)
- [Predefined Validations](./docs/core-concepts/predefined-validations.md)
- [Error Accumulation](./docs/guides/error-accumulation.md)
- [Source Generators](./docs/core-concepts/source-generators.md)
- [Binary Compatibility](./docs/architecture/binary-compatibility.md)
- [Analyzer Documentation](./docs/analyzers/overview.md)

## Testing with AwesomeAssertions

FluentUnions.AwesomeAssertions provides fluent assertions for testing:

```csharp
using AwesomeAssertions;

// Option assertions
var option = Option.Some(42);
option.Should().BeSome();
option.Should().BeSomeWithValue(42);

var none = Option<int>.None;
none.Should().BeNone();

// Result assertions
var success = Result.Success("Hello");
success.Should().Succeed();
success.Should().SucceedWithValue("Hello");

var failure = Result.Failure<string>(new ValidationError("Invalid input"));
failure.Should().Fail()
    .WithErrorType<ValidationError>()
    .WithErrorCode("VALIDATION_ERROR")
    .WithErrorMessage(containing: "Invalid");

// New convenient assertions
failure.Should().FailWith<ValidationError>();
failure.Should().FailWith(new ValidationError("Invalid input"));
failure.Should().FailWith(error => error.Code == "VALIDATION_ERROR");

// AggregateError assertions
var aggregateResult = Result.BindAllAppend(failed1, failed2, failed3);
aggregateResult.Should().Fail()
    .WithAggregateError()
    .Which.Errors.Should().HaveCount(3);

// Or use specialized methods
aggregateResult.Should().Fail()
    .WithAggregateErrorCount(3)
    .WithAggregateErrorContaining<ValidationError>()
    .WithAggregateErrorContaining(specificError)
    .WithAggregateErrorContainingCode("VALIDATION_ERROR")
    .WithAggregateErrorContainingMessage("Email is required")
    .WithAggregateErrorMatching(e => e.Code.StartsWith("VAL_"));

// Chain assertions
Option.Some(5)
    .Should().BeSome()
    .Which.Should().BeGreaterThan(0);
```

## Real-World Example

```csharp
public class UserService
{
    public async Task<Result<User>> RegisterUserAsync(
        string email,
        string password,
        string name)
    {
        // Validate inputs
        var validationResult = ValidateRegistration(email, password, name);
        if (validationResult.IsFailure)
            return validationResult.Error;

        // Check if user exists
        var existingUser = await repository.FindByEmailAsync(email);
        if (existingUser.IsSome)
            return new ConflictError("Email already registered");

        // Create user
        var user = new User(email, hashedPassword, name);
        var saveResult = await repository.SaveAsync(user);

        return saveResult.Map(_ => user);
    }
}
```

## Performance

FluentUnions is designed for high performance:

- Zero-allocation operations for success paths
- Struct-based `Option<T>` and `Result<T>` types
- Zero-cost transformations between Result/EnsureBuilder and Option/FilterBuilder
- Aggressive inlining for hot paths
- Source generators reduce build-time overhead (internal use only)

## Contributing

We welcome contributions! Please see our [Contributing Guide](./docs/contributing.md) for details.

## Acknowledgments

Many of FluentUnions features came from concepts in other functional programming concepts and some specialized .NET libraries.

### Inspiring Libraries

- **[LanguageExt](https://github.com/louthy/language-ext)** by Paul Louth
- **[CSharpFunctionalExtensions](https://github.com/vkhorikov/CSharpFunctionalExtensions)** by Vladimir Khorikov
- **[FluentResults](https://github.com/altmann/FluentResults)** by Michael Altmann

### Educational Excellence

- **[Zoran Horvat](https://www.youtube.com/@zoran-horvat)** - One of my favourite instructors who gives extensive topics on .NET and C#. You can find many functional C# topic and other best design practices on his channel, including good tutorials on result and option patterns. I also learn a lot from his courses on PluralSight.
- **[Vladimir Khorikov](https://www.youtube.com/@VladimirKhorikov)** - Creator of CSharpFunctionalExtensions, and has many comprehensive courses. I learning a lot about DDD from his courses on PluralSight.

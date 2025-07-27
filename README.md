# FluentUnions

[![NuGet](https://img.shields.io/nuget/v/FluentUnions.svg)](https://www.nuget.org/packages/FluentUnions/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/FluentUnions.svg)](https://www.nuget.org/packages/FluentUnions/)
[![Build Status](https://img.shields.io/github/actions/workflow/status/ymjaber/fluent-unions/build.yml?branch=main)](https://github.com/ymjaber/fluent-unions/actions)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

A comprehensive .NET library for functional error handling and optional values. FluentUnions provides robust implementations of Result and Option patterns with extensive monadic operations, source generators for enhanced functionality, and static analyzers to prevent common mistakes at compile time.

## 📚 Documentation

**[View Full Documentation](docs/index.md)** - Comprehensive guides, tutorials, and API reference

### Quick Links

- [Getting Started Guide](docs/getting-started/quick-start.md)
- [Result Pattern Tutorial](docs/tutorials/result-pattern-basics.md)
- [Option Pattern Tutorial](docs/tutorials/option-pattern-basics.md)
- [Error Handling Guide](docs/tutorials/error-handling.md)
- [Testing Guide](docs/guides/testing-guide.md)

## Features

- **Result Pattern** - Type-safe error handling without exceptions for expected failures
- **Option Pattern** - Explicit null handling and optional value representation
- **Rich & Flexible Error Types** - Built-in error types for common scenarios (Validation, NotFound, etc.)
- **Monadic Operations** - Complete set of functional operations (Map, Bind, Match, etc.)
- **Railway-Oriented Programming** - Elegant operation chaining with short-circuit evaluation
- **Source Generators** - Zero-overhead extension methods and performance optimizations for generating monadic functions supporting multi-value results/options
- **Static Analyzers** - Compile-time safety checks with helpful diagnostics
- **Integration Libraries** - Extensions for popular testing frameworks (Currently the new AwesomeAssertions)
- **High Performance** - Struct-based implementations for minimal allocations
- **Multi-Value Support**: All functions depending on using Result<T>/Option<T> values have overloads to support easier use of value tuples

## Installation

```bash
dotnet add package FluentUnions
```

For testing extensions (other frameworks support will be added later):

```bash
dotnet add package FluentUnions.AwesomeAssertions
```

Or via Package Manager Console:

```powershell
Install-Package FluentUnions
```

```powershell
Install-Package FluentUnions.AwesomeAssertions
```

## Quick Start

### Result Pattern

The Result pattern represents operations that can succeed with a value or fail with an error:

```csharp
using FluentUnions;

// Simple success/failure
public Result ValidateAge(int age)
{
    if (age < 0) return Result.Failure("Age cannot be negative");

    Error tooOld = new Error(code: "Age.TooOld", message: "Age cannot exceed 150");
    if (age > 150) return Result.Failure(tooOld);

    return Result.Success();
}

// Result with value
public Result<User> GetUser(Guid id)
{
    var user = _repository.FindById(id);
    return user != null
        ? Result.Success(user)
        : Result.Failure<User>(new NotFoundError($"User {id} not found"));
}

// Using typed errors
public Result<Order> ProcessOrder(OrderRequest request)
{
    if (!request.IsValid())
    {
        // Implicit conversion from error types to Result/Result<T>
        return new ValidationError("Invalid order request");
    }

    if (!_inventory.HasStock(request.Items))
    {
        return new ConflictError("Insufficient inventory");
    }

    var order = new Order(request);

    // Implicit conversion from T to Result<T>/Option<T>
    return order;

    // OR
    // return Result.Success(order);
}
```

### Option Pattern

The Option pattern represents values that may or may not exist:

```csharp
// Creating options
var some = Option.Some(42);
var none = Option<int>.None;

int? a = 5;
var intOption = a.AsOption();

// From nullable values (works with both reference types and nullable value types)
string? nullable = GetNullableString();
Option<string> option1 = Option.From(nullable);
// OR
Option<string> option2 = nullable.AsOption();


// Finding values
Option<User> user = users
    .Where(u => u.Email == email)
    .FirstOrNone();

// Chaining operations
var result = GetUserOption(id)
    .Filter(u => u.IsActive)
    .Map(u => u.Email)
    .OrElse(() => "default@example.com");
```

## Core Concepts

### Result Types

FluentUnions provides two main Result types:

1. **Result** - Operations that succeed with no value or fail with an error
2. **Result<T>** - Operations that succeed with a value of type T or fail with an error

```csharp
// Unit Result - no value on success
public Result DeleteUser(Guid id)
{
    if (!_repository.Exists(id))
        return Result.Failure("User not found");

    _repository.Delete(id);
    return Result.Success();
}

// Value Result - returns data on success
public Result<UserDto> CreateUser(CreateUserRequest request)
{
    var validation = ValidateRequest(request);
    if (validation.IsFailure)
        return Result.Failure<UserDto>(validation.Error);

    var user = new User(request);
    _repository.Save(user);

    return Result.Success(UserDto.From(user));
}
```

### Error Types

FluentUnions includes a rich hierarchy of error types:

```csharp
// Base Error class
public class Error
{
    public string Code { get; }
    public string Message { get; }
    public IReadOnlyDictionary<string, object> Metadata { get; }
}

// Specialized error types
var validationError = new ValidationError("Invalid email format");
var notFoundError = new NotFoundError("User", userId);
var conflictError = new ConflictError("Email already exists");
var authenticationError = new AuthenticationError("Invalid credentials");
var authorizationError = new AuthorizationError("Insufficient permissions");

// Aggregate multiple errors
var errors = new AggregateError(
    new ValidationError("Invalid email"),
    new ValidationError("Password too short")
);

// Custom errors with metadata
var error = new Error("CUSTOM_ERROR", "Something went wrong")
    .WithMetadata("userId", userId)
    .WithMetadata("timestamp", DateTime.UtcNow);
```

## Functional Operations

### Map - Transform Success Values

Transform the value inside a Result or Option:

```csharp
// Result mapping
Result<string> nameResult = GetUserName(userId);
Result<int> lengthResult = nameResult.Map(name => name.Length);

// Option mapping
Option<User> userOption = FindUser(email);
Option<string> upperEmail = userOption.Map(u => u.Email.ToUpper());

```

### Bind - Chain Operations

Chain operations where each can return a Result/Option:

```csharp
// Result binding (Railway-oriented programming)
public Result<OrderConfirmation> PlaceOrder(OrderRequest request)
{
    return ValidateRequest(request)
        .Bind(CreateOrder)
        .Bind(ReserveInventory)
        .Bind(ProcessPayment)
        .Bind(SendConfirmation);
}

// Option binding
public Option<Config> LoadConfiguration(string path)
{
    return ReadFile(path)
        .Bind(ParseJson)
        .Bind(ValidateConfig)
        .Bind(CreateConfig);
}

```

### Match - Pattern Matching

Handle both success and failure cases:

```csharp
// Result matching
string message = result.Match(
    onSuccess: user => $"Welcome, {user.Name}!",
    onFailure: error => $"Error: {error.Message}"
);

// Option matching
var display = option.Match(
    onSome: value => value.ToString(),
    onNone: () => "No value"
);

```

### Filter - Conditional Operations

Filter values based on predicates:

```csharp
// Option filtering
Option<int> positive = Option.Some(-5)
    .Filter(x => x > 0); // Returns None

// With custom error message
Option<User> activeUser = GetUser(id)
    .Filter(u => u.IsActive, "User is not active");

// Result filtering via Ensure
Result<Order> validOrder = GetOrder(id)
    .Ensure(o => o.Total > 0, "Order total must be positive")
    .Ensure(o => o.Items.Any(), "Order must have items");
```

### Combine - Aggregate Multiple Results

Combine multiple Results or Options:

```csharp
// Combine Results - fails if any fail
var results = new[]
{
    ValidateName(name),
    ValidateEmail(email),
    ValidateAge(age)
};

Result combined = Result.Combine(results);

// Combine with values
var userResult = Result.Combine(
    GetFirstName(),
    GetLastName(),
    GetEmail()
).Map(values => new User(values[0], values[1], values[2]));

// BindAll - combine Results with different types
public Result<Order> CreateOrder(CustomerId customerId, List<ProductId> productIds)
{
    return Result.BindAll(
        GetCustomer(customerId),
        GetProducts(productIds)
    ).Map((customer, products) => new Order(customer, products));
}
```

## Advanced Patterns

### Railway-Oriented Programming

Chain operations where any failure short-circuits the pipeline:

```csharp
public class OrderService
{
    public Result<OrderId> ProcessOrder(OrderRequest request)
    {
        return Validate(request)
            .Bind(CalculatePricing)
            .Bind(CheckInventory)
            .Bind(ReserveItems)
            .Bind(ChargePayment)
            .Bind(CreateShipment)
            .Bind(SendNotification)
            .Map(order => order.Id);
    }

    private Result<ValidatedOrder> Validate(OrderRequest request)
    {
        if (request.Items.Count == 0)
            return new ValidationError("Order must contain items");

        if (request.CustomerId == default)
            return new ValidationError("Customer ID is required");

        return Result.Success(new ValidatedOrder(request));
    }

    // Each step returns Result<T> and the chain continues only on success
}
```

### Try Pattern - Exception Handling

Convert exception-throwing operations to Results:

```csharp
// Synchronous Try
public Result<int> ParseInt(string input)
{
    return Result.Try(() => int.Parse(input))
        .MapError(ex => new ValidationError($"Invalid integer: {input}"));
}

// Try with resource cleanup
public Result<string> ReadFile(string path)
{
    return Result.Try(() =>
    {
        using var reader = new StreamReader(path);
        return reader.ReadToEnd();
    });
}
```

### Option Extensions

Rich set of Option operations:

```csharp
// OrElse - provide alternative
var email = GetUserEmail(userId)
    .OrElse(() => GetDefaultEmail())
    .OrElse("noreply@example.com");

// Coalescing options
var firstAvailable = Option.Coalesce(
    TryGetFromCache(key),
    TryGetFromDatabase(key),
    TryGetFromRemote(key)
);

// Converting to Result
Result<string> result = GetOptionalValue()
    .ToResult(() => new NotFoundError("Value not found"));

// Filtering with predicates
var adults = users
    .Select(u => Option.From(u))
    .Filter(u => u.Age >= 18)
    .ToList();
```

## Error Handling Patterns

### Structured Error Handling

```csharp
public class UserService
{
    public Result<User> RegisterUser(RegisterRequest request)
    {
        // Early returns for validation failures
        var emailValidation = ValidateEmail(request.Email);
        if (emailValidation.IsFailure)
            return Result.Failure<User>(emailValidation.Error);

        // Check for conflicts
        var existingUser = _repository.FindByEmail(request.Email);
        if (existingUser.IsSome)
            return new ConflictError($"Email {request.Email} already registered");

        // Create and save user
        var user = new User(request);
        var saveResult = _repository.Save(user);

        return saveResult.IsSuccess
            ? Result.Success(user)
            : Result.Failure<User>(saveResult.Error);
    }
}
```

### Error Recovery

````csharp
// Recover from specific errors
var result = GetUser(id)
    .MapError(error => error.Code == "NOT_FOUND"
        ? CreateDefaultUser()
        : Result.Failure<User>(error));

// Provide fallback values
var config = LoadConfig()
    .OrElse(() => LoadDefaultConfig())
    .GetValueOr(new Config());

// Retry logic
public Result<T> Retry<T>(
    Func<Result<T>> operation,
    int maxAttempts = 3)
{
    for (int i = 0; i < maxAttempts; i++)
    {
        var result = operation();
        if (result.IsSuccess) return result;

        if (i < maxAttempts - 1)
            Thread.Sleep(TimeSpan.FromSeconds(Math.Pow(2, i)));
    }

    return Result.Failure<T>("Max retry attempts exceeded");
}```

## Testing with FluentUnions

### Built-in Assertions

Using FluentUnions.AwesomeAssertions:

```csharp
// Result assertions
result.Should().BeSuccess();
result.Should().BeSuccessWithValue(expectedValue);
result.Should().BeFailure();
result.Should().BeFailureWithError<ValidationError>();
result.Should().BeFailureWithMessage("Expected error message");

// Option assertions
option.Should().BeSome();
option.Should().BeSomeWithValue(42);
option.Should().BeNone();

// Custom assertions
result.Should().Satisfy(r => r.IsSuccess && r.Value.Id == expectedId);
````

### Testing Patterns

```csharp
[Test]
public void ProcessOrder_WithInvalidRequest_ReturnsValidationError()
{
    // Arrange
    var request = new OrderRequest { Items = new List<Item>() };
    var service = new OrderService();

    // Act
    var result = service.ProcessOrder(request);

    // Assert
    result.Should()
        .BeFailure()
        .WithError<ValidationError>()
        .WithMessage("Order must contain items");
}

[Test]
public void GetUser_WhenExists_ReturnsSome()
{
    // Arrange
    var repository = new UserRepository();
    var userId = Guid.NewGuid();
    repository.Add(new User { Id = userId });

    // Act
    var option = repository.FindById(userId);

    // Assert
    option.Should()
        .BeSome()
        .WithValueMatching(u => u.Id == userId);
}
```

## Source Generators

FluentUnions includes source generators that provide additional functionality:

### Generated Extension Methods

```csharp
// Tuple deconstruction
var (isSuccess, value, error) = result;
if (isSuccess)
{
    Console.WriteLine($"Value: {value}");
}

// GetValueOr extensions
var value = result.GetValueOr(defaultValue);
var value = result.GetValueOr(() => ComputeDefault());

// Collection extensions
IEnumerable<Result<T>> results = GetResults();
Result<IEnumerable<T>> combined = results.Combine();
```

### Performance Optimizations

The generators provide specialized implementations for common patterns:

```csharp
// Optimized for common cases
result.Map(x => x.ToString()); // Avoids allocations for success case
option.Filter(x => x > 0);      // Inline filtering without delegates

```

## Static Analyzers

FluentUnions includes Roslyn analyzers to catch common mistakes:

### Analyzer Rules

1. **FU001** - Accessing Value without checking IsSuccess

```csharp
// ❌ Warning: Possible InvalidOperationException
var value = result.Value;

// ✅ Correct
if (result.IsSuccess)
{
    var value = result.Value;
}
```

2. **FU002** - Not handling all cases in Match

```csharp
// ❌ Warning: Match expression not exhaustive
var output = result.Match(
    onSuccess: v => v.ToString()
    // Missing onFailure
);
```

3. **FU003** - Ignoring Result return values

```csharp
// ❌ Warning: Result not used
service.ProcessData(data);

// ✅ Correct
var result = service.ProcessData(data);
if (result.IsFailure) { /* handle */ }
```

4. **FU004** - Comparing Option with null

```csharp
// ❌ Warning: Use IsNone instead
if (option == null)

// ✅ Correct
if (option.IsNone)
```

## Best Practices

### When to Use Result vs Exceptions

**Use Result for:**

- Expected failures (validation, business rules)
- Operations that can fail in normal flow
- External service calls
- User input validation

**Use Exceptions for:**

- Programming errors (null arguments, index out of range)
- Infrastructure failures
- Truly exceptional cases
- Constructor failures

### When to Use Option vs Null

**Use Option for:**

- Optional return values
- Configuration settings
- Cache lookups
- Find operations

**Use Null for:**

- Internal implementation details
- Performance-critical paths (after profiling)
- Interop with external libraries

### Design Guidelines

1. **Return Early on Failure**

```csharp
public Result<Order> ProcessOrder(OrderRequest request)
{
    var validation = Validate(request);
    if (validation.IsFailure) return validation.Error;

    var inventory = CheckInventory(request.Items);
    if (inventory.IsFailure) return inventory.Error;

    // Continue processing...
}
```

2. **Use Railway-Oriented Programming for Complex Flows**

```csharp
public Result<Invoice> GenerateInvoice(OrderId orderId)
{
    return GetOrder(orderId)
        .Bind(EnrichWithCustomer)
        .Bind(CalculateTaxes)
        .Bind(ApplyDiscounts)
        .Bind(GeneratePdf);
}
```

3. **Create Domain-Specific Error Types**

```csharp
public class InsufficientFundsError : Error
{
    public decimal Required { get; }
    public decimal Available { get; }

    public InsufficientFundsError(decimal required, decimal available)
        : base("INSUFFICIENT_FUNDS",
               $"Required {required:C} but only {available:C} available")
    {
        Required = required;
        Available = available;
    }
}
```

## Performance Considerations

- **Zero Allocation Success Path** - Result<T> is a struct with no heap allocations for success cases
- **Minimal Overhead** - Thin wrapper around values with negligible performance impact
- **Optimized Delegates** - Source generators create specialized implementations avoiding delegate allocations
- **Struct-Based** - Both Result<T> and Option<T> are value types
- **Short-Circuit Evaluation** - Operations stop at first failure in chains

## Migration Guide

### From Nullable References to Option

```csharp
// Before
public string? FindUserEmail(int id)
{
    var user = GetUser(id);
    return user?.Email;
}

// After
public Option<string> FindUserEmail(int id)
{
    return GetUser(id)
        .Map(u => u.Email);
}
```

### From Exceptions to Result

```csharp
// Before
public User CreateUser(string email)
{
    if (!IsValidEmail(email))
        throw new ArgumentException("Invalid email");

    return new User(email);
}

// After
public Result<User> CreateUser(string email)
{
    if (!IsValidEmail(email))
        return new ValidationError("Invalid email");

    return Result.Success(new User(email));
}
```

## Integration Examples

### ASP.NET Core

```csharp
[ApiController]
public class UserController : ControllerBase
{
    [HttpGet("{id}")]
    public IActionResult GetUser(Guid id)
    {
        var result = _userService.GetUser(id);

        return result.Match<IActionResult>(
            onSuccess: user => Ok(user),
            onFailure: error => error switch
            {
                NotFoundError => NotFound(error.Message),
                ValidationError => BadRequest(error.Message),
                _ => StatusCode(500, "Internal server error")
            }
        );
    }
}
```

### MediatR

```csharp
public class CreateOrderCommand : IRequest<Result<OrderId>>
{
    public List<Item> Items { get; set; }
}

public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, Result<OrderId>>
{
    public Task<Result<OrderId>> Handle(
        CreateOrderCommand request,
        CancellationToken cancellationToken)
    {
        var result = ValidateItems(request.Items)
            .Bind(items => _orderService.CreateOrder(items))
            .Map(order => order.Id);

        return Task.FromResult(result);
    }
}
```

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request. For major changes, please open an issue first to discuss what you would like to change.

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

### Development Setup

```bash
# Clone the repository
git clone https://github.com/ymjaber/fluent-unions.git

# Restore dependencies
dotnet restore

# Build the solution
dotnet build

# Run tests
dotnet test

# Run with analyzers
dotnet build -p:EnableAnalyzers=true
```

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Support

- **Documentation** - Full API documentation in code
- **Issues** - Report bugs via [GitHub Issues](https://github.com/ymjaber/fluent-unions/issues)
- **Discussions** - Join [GitHub Discussions](https://github.com/ymjaber/fluent-unions/discussions)
- **Examples** - See the [samples](https://github.com/ymjaber/fluent-unions/tree/main/samples) directory

## Acknowledgments

FluentUnions is inspired by functional programming concepts from F#, Rust's Result type, and the Railway-Oriented Programming pattern. Special thanks to the teams behind LanguageExt and CSharpFunctionalExtensions for paving the way.

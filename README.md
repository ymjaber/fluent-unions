# FluentUnions

[![NuGet](https://img.shields.io/nuget/v/FluentUnions.svg)](https://www.nuget.org/packages/FluentUnions/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

FluentUnions handles expected errors with `Result` and `Result<T>`, and includes `Option<T>` for null handling. Features discriminated unions with monadic functions, source generators, and static analyzers to prevent common mistakes.

## 📦 Installation

```bash
dotnet add package FluentUnions
```

## 🚀 Features

- **Result Pattern** - Type-safe error handling without exceptions
- **Option Pattern** - Explicit null handling
- **Monadic Operations** - Map, Bind, Match for functional composition
- **Source Generators** - Additional helper methods via code generation
- **Static Analyzers** - Catch common mistakes at compile time
- **Railway-Oriented Programming** - Chain operations elegantly
- **Performance** - Zero allocation for success cases

## 📖 Usage

### Result Pattern

The Result pattern represents the outcome of an operation that can either succeed or fail.

```csharp
using FluentUnions;

// Basic Result usage
public Result ValidateAge(int age)
{
    if (age < 0)
        return Result.Failure("Age cannot be negative");
    
    if (age > 150)
        return Result.Failure("Age cannot exceed 150");
        
    return Result.Success();
}

// Result<T> with value
public Result<User> GetUserById(Guid id)
{
    var user = database.Users.FirstOrDefault(u => u.Id == id);
    
    if (user == null)
        return Result<User>.Failure("User not found");
        
    return Result<User>.Success(user);
}

// Using the results
var result = GetUserById(userId);

if (result.IsSuccess)
{
    Console.WriteLine($"Found user: {result.Value.Name}");
}
else
{
    Console.WriteLine($"Error: {result.Error}");
}
```

### Option Pattern

The Option pattern represents a value that may or may not exist, replacing null references.

```csharp
// Creating Options
var some = Option<int>.Some(42);
var none = Option<int>.None;

// From nullable values
string? nullableString = GetStringOrNull();
var option = Option<string>.From(nullableString);

// Pattern matching
var message = option.Match(
    onSome: value => $"Got value: {value}",
    onNone: () => "No value present"
);

// Checking state
if (option.IsSome)
{
    Console.WriteLine(option.Value);
}

// Safe operations
var length = option
    .Map(s => s.Length)
    .Filter(len => len > 5)
    .GetValueOrDefault(0);
```

### Monadic Operations

#### Map - Transform the value

```csharp
Result<string> nameResult = GetUserName(userId);
Result<int> lengthResult = nameResult.Map(name => name.Length);

Option<User> userOption = FindUser(email);
Option<string> emailOption = userOption.Map(user => user.Email);
```

#### Bind - Chain operations that return Result/Option

```csharp
public Result<Order> ProcessOrder(Guid userId, List<Item> items)
{
    return GetUser(userId)
        .Bind(user => ValidateUserCanOrder(user))
        .Bind(user => CreateOrder(user, items))
        .Bind(order => ChargePayment(order))
        .Bind(order => SendConfirmation(order));
}

public Option<Config> LoadConfiguration()
{
    return ReadConfigFile()
        .Bind(content => ParseJson(content))
        .Bind(json => ValidateConfig(json))
        .Bind(valid => CreateConfig(valid));
}
```

#### Match - Handle both cases

```csharp
var output = result.Match(
    onSuccess: user => $"Welcome, {user.Name}!",
    onFailure: error => $"Login failed: {error}"
);

var display = option.Match(
    onSome: value => value.ToString(),
    onNone: () => "N/A"
);
```

### Railway-Oriented Programming

Chain multiple operations where each can fail:

```csharp
public class OrderService
{
    public Result<OrderConfirmation> PlaceOrder(OrderRequest request)
    {
        return ValidateRequest(request)
            .Bind(ValidateInventory)
            .Bind(CalculatePricing)
            .Bind(ReserveInventory)
            .Bind(ProcessPayment)
            .Bind(CreateShipment)
            .Bind(SendConfirmationEmail)
            .Map(ToOrderConfirmation);
    }
    
    private Result<ValidatedOrder> ValidateRequest(OrderRequest request)
    {
        if (request.Items.Count == 0)
            return Result<ValidatedOrder>.Failure("Order must contain items");
            
        // More validations...
        return Result<ValidatedOrder>.Success(new ValidatedOrder(request));
    }
    
    // Other methods follow similar pattern
}
```

### Combining Results

```csharp
// Combine multiple results
var result1 = ValidateName(name);
var result2 = ValidateEmail(email);
var result3 = ValidateAge(age);

var combined = Result.Combine(result1, result2, result3);
if (combined.IsFailure)
{
    // Handle validation errors
}

// With values
var userResult = Result<User>.Combine(
    GetName().Map(n => new { Name = n }),
    GetEmail().Map(e => new { Email = e }),
    GetAge().Map(a => new { Age = a })
).Map(values => new User(values.Name, values.Email, values.Age));
```

### Try Pattern - Convert Exceptions to Results

```csharp
public Result<int> ParseInteger(string input)
{
    return Result.Try(() => int.Parse(input))
        .MapError(ex => $"Invalid number format: {input}");
}

public async Task<Result<string>> FetchDataAsync(string url)
{
    return await Result.TryAsync(async () => 
    {
        using var client = new HttpClient();
        return await client.GetStringAsync(url);
    })
    .MapError(ex => $"Failed to fetch data: {ex.Message}");
}
```

## 🏗️ Architecture Guidelines

### SOLID Principles with Result Pattern

#### Single Responsibility Principle

```csharp
// Each method has one responsibility and returns a Result
public interface IUserValidator
{
    Result ValidateEmail(string email);
    Result ValidatePassword(string password);
    Result ValidateAge(int age);
}

public interface IUserRepository  
{
    Result<User> GetById(Guid id);
    Result Save(User user);
    Result Delete(Guid id);
}
```

#### Open/Closed Principle

```csharp
// Easy to extend with new validators without modifying existing code
public interface IValidator<T>
{
    Result Validate(T value);
}

public class EmailValidator : IValidator<string> { }
public class PasswordValidator : IValidator<string> { }
public class CompositeValidator<T> : IValidator<T> 
{
    private readonly IEnumerable<IValidator<T>> _validators;
    
    public Result Validate(T value)
    {
        return Result.Combine(_validators.Select(v => v.Validate(value)));
    }
}
```

#### Dependency Inversion Principle

```csharp
// Depend on abstractions that return Results
public class OrderService
{
    private readonly IOrderRepository _repository;
    private readonly IPaymentService _payment;
    private readonly INotificationService _notification;
    
    public async Task<Result<Order>> PlaceOrderAsync(OrderRequest request)
    {
        return await ValidateOrder(request)
            .BindAsync(order => _repository.SaveAsync(order))
            .BindAsync(order => _payment.ProcessAsync(order))
            .BindAsync(order => _notification.SendConfirmationAsync(order));
    }
}
```

### Error Handling Best Practices

1. **Use Result for Expected Errors**
   - Business rule violations
   - Validation failures  
   - Not found scenarios
   - Permission denied

2. **Use Exceptions for Unexpected Errors**
   - Infrastructure failures
   - Programming errors
   - Network issues
   - Resource exhaustion

3. **Use Option for Optional Values**
   - Configuration settings
   - Cache lookups
   - First/Last operations
   - Nullable references

### Testing with Results

```csharp
[Test]
public void CreateUser_WithInvalidEmail_ReturnsFailure()
{
    // Arrange
    var service = new UserService();
    
    // Act
    var result = service.CreateUser("invalid-email", "password");
    
    // Assert
    result.Should().BeFailure()
        .WithError("Invalid email format");
}

[Test]
public void GetUser_WhenExists_ReturnsSome()
{
    // Arrange
    var repository = new UserRepository();
    var userId = Guid.NewGuid();
    
    // Act
    var option = repository.FindById(userId);
    
    // Assert
    option.Should().BeSome()
        .WithValue(user => user.Id == userId);
}
```

## 🔧 Advanced Features

### Custom Result Types

```csharp
public class ValidationResult : Result
{
    public List<string> Errors { get; }
    
    public static ValidationResult WithErrors(params string[] errors)
    {
        return new ValidationResult(errors.ToList());
    }
}

public class Result<TValue, TError> where TError : class
{
    // Custom error type instead of string
}
```

### Async Operations

```csharp
public async Task<Result<User>> CreateUserAsync(CreateUserDto dto)
{
    return await ValidateAsync(dto)
        .BindAsync(valid => CheckEmailAvailableAsync(valid.Email))
        .BindAsync(async _ => await HashPasswordAsync(dto.Password))
        .BindAsync(hash => SaveUserAsync(dto, hash));
}
```

### Source Generator Features

The included source generators provide additional extension methods:

```csharp
// Generated tuple support
var (isSuccess, value, error) = result;

// Generated GetValueOr methods  
var value = result.GetValueOr(defaultValue);
var value = result.GetValueOr(() => ComputeDefault());

// Generated async extensions
await result.MapAsync(async value => await ProcessAsync(value));
```

### Static Analyzer Rules

The analyzers help catch common mistakes:

- Accessing Value without checking IsSuccess
- Not handling all cases in Match
- Ignoring Result return values
- Incorrect Option usage patterns

## 📚 Additional Resources

- [Railway Oriented Programming](https://fsharpforfunandprofit.com/posts/recipe-part2/)
- [Functional Error Handling](https://enterprisecraftsmanship.com/posts/functional-c-handling-failures-input-errors/)
- [Option Type Pattern](https://en.wikipedia.org/wiki/Option_type)

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](../LICENSE) file for details.
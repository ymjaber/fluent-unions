# Quick Start Guide

Get up and running with FluentUnions in minutes! This guide shows you the most common patterns and use cases.

## Your First Result

The Result pattern helps you handle operations that can succeed or fail without throwing exceptions.

### Basic Success and Failure

```csharp
using FluentUnions;

// Creating a success result (no value)
Result successResult = Result.Success();
Console.WriteLine(successResult.IsSuccess); // True

// Creating a failure result
Result failureResult = Result.Failure("Operation failed");
Console.WriteLine(failureResult.IsFailure); // True
Console.WriteLine(failureResult.Error.Message); // "Operation failed"
```

### Results with Values

```csharp
// Success with a value
Result<int> ageResult = Result.Success(25);
if (ageResult.IsSuccess)
{
    Console.WriteLine($"Age: {ageResult.Value}"); // Age: 25
}

// Failure with typed result
Result<User> userResult = Result.Failure<User>("User not found");
if (userResult.IsFailure)
{
    Console.WriteLine($"Error: {userResult.Error.Message}");
}
```

### Real-World Example: User Service

```csharp
public class UserService
{
    private readonly List<User> _users = new();

    public Result<User> GetUser(Guid id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        
        if (user == null)
            return Result.Failure<User>($"User {id} not found");
            
        return Result.Success(user);
    }
    
    public Result<User> CreateUser(string email, string name)
    {
        // Validate email
        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
            return Result.Failure<User>("Invalid email address");
        
        // Check for duplicates
        if (_users.Any(u => u.Email == email))
            return Result.Failure<User>("Email already exists");
        
        var user = new User { Id = Guid.NewGuid(), Email = email, Name = name };
        _users.Add(user);
        
        return Result.Success(user);
    }
}
```

## Your First Option

The Option pattern represents values that may or may not exist, replacing null references.

### Basic Some and None

```csharp
// Creating an Option with a value
Option<string> someOption = Option.Some("Hello");
Console.WriteLine(someOption.IsSome); // True
Console.WriteLine(someOption.Value); // "Hello"

// Creating an empty Option
Option<string> noneOption = Option<string>.None;
Console.WriteLine(noneOption.IsNone); // True
```

### From Nullable Values

```csharp
// Convert nullable to Option
string? nullableString = GetNullableString();
Option<string> option = Option.From(nullableString);

// Or use the extension method
Option<string> option2 = nullableString.AsOption();

// Works with nullable value types too
int? nullableInt = 42;
Option<int> intOption = nullableInt.AsOption();
```

### Real-World Example: Configuration Service

```csharp
public class ConfigService
{
    private readonly Dictionary<string, string> _settings = new();
    
    public Option<string> GetSetting(string key)
    {
        return _settings.TryGetValue(key, out var value) 
            ? Option.Some(value) 
            : Option<string>.None;
    }
    
    public Option<int> GetIntSetting(string key)
    {
        return GetSetting(key)
            .Bind(value => int.TryParse(value, out var result) 
                ? Option.Some(result) 
                : Option<int>.None);
    }
}

// Usage
var config = new ConfigService();
var timeout = config.GetIntSetting("timeout")
    .GetValueOr(30); // Default to 30 if not found
```

## Functional Operations

### Map - Transform Values

```csharp
// Transform Result value
Result<string> nameResult = GetUserName(userId);
Result<int> lengthResult = nameResult.Map(name => name.Length);

// Transform Option value
Option<User> userOption = FindUser(email);
Option<string> emailUpper = userOption.Map(u => u.Email.ToUpper());
```

### Bind - Chain Operations

```csharp
// Chain Result operations
Result<Order> orderResult = ValidateOrder(request)
    .Bind(CreateOrder)
    .Bind(SaveToDatabase)
    .Bind(SendConfirmation);

// Chain Option operations
Option<string> result = GetEnvironmentVariable("API_KEY")
    .Bind(ValidateApiKey)
    .Bind(DecryptApiKey);
```

### Match - Handle Both Cases

```csharp
// Pattern match on Result
string message = userResult.Match(
    onSuccess: user => $"Hello, {user.Name}!",
    onFailure: error => $"Error: {error.Message}"
);

// Pattern match on Option
var displayName = userOption.Match(
    onSome: user => user.DisplayName,
    onNone: () => "Anonymous"
);
```

## Error Handling

### Using Built-in Error Types

```csharp
public Result<Product> GetProduct(Guid id)
{
    var product = _repository.FindById(id);
    
    if (product == null)
        return new NotFoundError($"Product {id} not found");
        
    if (!product.IsAvailable)
        return new ConflictError("Product is not available");
        
    if (!_authService.CanViewProduct(product))
        return new AuthorizationError("Access denied");
        
    return Result.Success(product);
}
```

### Custom Error Types

```csharp
public class BusinessRuleError : Error
{
    public string RuleName { get; }
    
    public BusinessRuleError(string ruleName, string message) 
        : base($"RULE_{ruleName}", message)
    {
        RuleName = ruleName;
    }
}

// Usage
return new BusinessRuleError("MIN_ORDER", "Minimum order amount is $10");
```

## Common Patterns

### Repository Pattern

```csharp
public interface IUserRepository
{
    Result<User> GetById(Guid id);
    Result<User> GetByEmail(string email);
    Result<Guid> Create(User user);
    Result Update(User user);
    Result Delete(Guid id);
}

public class UserRepository : IUserRepository
{
    public Result<User> GetById(Guid id)
    {
        try
        {
            var user = _dbContext.Users.Find(id);
            return user != null 
                ? Result.Success(user)
                : new NotFoundError($"User {id} not found");
        }
        catch (Exception ex)
        {
            return Result.Failure<User>($"Database error: {ex.Message}");
        }
    }
}
```

### Validation Pattern

```csharp
public static class Validator
{
    public static Result ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return new ValidationError("Email is required");
            
        if (!email.Contains('@'))
            return new ValidationError("Invalid email format");
            
        return Result.Success();
    }
    
    public static Result<RegisterRequest> ValidateRegistration(RegisterRequest request)
    {
        return ValidateEmail(request.Email)
            .Bind(() => ValidatePassword(request.Password))
            .Bind(() => ValidateName(request.Name))
            .Map(() => request);
    }
}
```

### API Controller Pattern

```csharp
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    
    [HttpGet("{id}")]
    public IActionResult Get(Guid id)
    {
        return _userService.GetUser(id).Match<IActionResult>(
            onSuccess: user => Ok(user),
            onFailure: error => error switch
            {
                NotFoundError => NotFound(error.Message),
                AuthorizationError => Forbid(error.Message),
                _ => StatusCode(500, "Internal server error")
            }
        );
    }
    
    [HttpPost]
    public IActionResult Create(CreateUserRequest request)
    {
        return _userService.CreateUser(request).Match<IActionResult>(
            onSuccess: user => CreatedAtAction(nameof(Get), new { id = user.Id }, user),
            onFailure: error => error switch
            {
                ValidationError => BadRequest(error.Message),
                ConflictError => Conflict(error.Message),
                _ => StatusCode(500, "Internal server error")
            }
        );
    }
}
```

## What's Next?

Now that you've seen the basics, explore:

1. [Core Concepts](core-concepts.md) - Understand the theory behind Result and Option
2. [Result Pattern Tutorial](../tutorials/result-pattern-basics.md) - Deep dive into Result
3. [Option Pattern Tutorial](../tutorials/option-pattern-basics.md) - Master optional values
4. [Functional Operations Guide](../guides/functional-operations.md) - Learn all operations

## Quick Reference

### Result Methods
- `Result.Success()` / `Result.Success<T>(value)`
- `Result.Failure(error)` / `Result.Failure<T>(error)`
- `IsSuccess` / `IsFailure`
- `Map()`, `Bind()`, `Match()`

### Option Methods
- `Option.Some(value)` / `Option<T>.None`
- `Option.From(nullable)`
- `IsSome` / `IsNone`
- `Map()`, `Bind()`, `Match()`, `Filter()`

### Common Extensions
- `.AsOption()` - Convert nullable to Option
- `.ToResult()` - Convert Option to Result
- `.GetValueOr(default)` - Get value or default
- `.OrElse()` - Provide alternative Option/value
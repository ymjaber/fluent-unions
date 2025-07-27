# Result API Reference

This guide provides a comprehensive reference for all Result types and their operations in FluentUnions.

## Table of Contents
1. [Result Types](#result-types)
2. [Creating Results](#creating-results)
3. [Checking Results](#checking-results)
4. [Transforming Results](#transforming-results)
5. [Combining Results](#combining-results)
6. [Error Handling](#error-handling)
7. [Async Operations](#async-operations)
8. [Extension Methods](#extension-methods)
9. [Type Conversions](#type-conversions)

## Result Types

### Result<T>
Represents an operation that returns a value of type T or an error.

```csharp
public readonly struct Result<T>
{
    public bool IsSuccess { get; }
    public bool IsFailure { get; }
    public T Value { get; }
    public Error Error { get; }
}
```

### Result (Unit Result)
Represents an operation that succeeds or fails without returning a value.

```csharp
public readonly struct Result
{
    public bool IsSuccess { get; }
    public bool IsFailure { get; }
    public Error Error { get; }
}
```

## Creating Results

### Success Results

```csharp
// Create a successful Result<T>
Result<User> userResult = Result.Success(new User { Name = "John" });

// Create a successful unit Result
Result operationResult = Result.Success();

// Implicit conversion from value
Result<int> implicitResult = 42;
```

### Failure Results

```csharp
// Create a failed Result<T> with an error
Result<User> failedUser = Result.Failure<User>(new NotFoundError("User not found"));

// Create a failed unit Result
Result failedOperation = Result.Failure(new ValidationError("Invalid input"));

// Implicit conversion from Error
Result<int> implicitFailure = new Error("CALC_ERROR", "Calculation failed");
```

### Factory Methods

```csharp
// Try pattern - catches exceptions
Result<int> tryResult = Result.Try(() => int.Parse("123"));
Result<int> tryFailure = Result.Try(() => int.Parse("abc")); // Returns failure

// Try with custom error
Result<int> tryCustom = Result.Try(
    () => int.Parse("abc"),
    ex => new ValidationError($"Invalid number: {ex.Message}"));

// Conditional creation
Result<User> conditionalResult = Result.SuccessIf(
    user != null,
    user,
    new NotFoundError("User not found"));

// From nullable
User? nullableUser = FindUser();
Result<User> fromNullable = Result.FromNullable(
    nullableUser,
    new NotFoundError("User not found"));
```

## Checking Results

### Basic Checks

```csharp
Result<User> result = GetUser();

// Check if successful
if (result.IsSuccess)
{
    Console.WriteLine($"Found user: {result.Value.Name}");
}

// Check if failed
if (result.IsFailure)
{
    Console.WriteLine($"Error: {result.Error.Message}");
}

// Pattern matching (C# 8+)
var message = result switch
{
    { IsSuccess: true } => $"User: {result.Value.Name}",
    { IsFailure: true } => $"Error: {result.Error.Message}",
    _ => "Unknown state"
};
```

### Match Method

```csharp
// Transform based on success/failure
string message = result.Match(
    onSuccess: user => $"Hello, {user.Name}",
    onFailure: error => $"Error: {error.Message}");

// Execute actions
result.Match(
    onSuccess: user => Console.WriteLine($"Processing {user.Name}"),
    onFailure: error => Logger.LogError(error.Message));

// Async match
await result.MatchAsync(
    onSuccess: async user => await SendEmailAsync(user),
    onFailure: async error => await LogErrorAsync(error));
```

## Transforming Results

### Map - Transform Success Value

```csharp
Result<User> userResult = GetUser();

// Transform the value if successful
Result<string> nameResult = userResult.Map(user => user.Name);
Result<UserDto> dtoResult = userResult.Map(user => new UserDto(user));

// Chain multiple transformations
Result<string> upperName = userResult
    .Map(user => user.Name)
    .Map(name => name.ToUpper())
    .Map(name => $"User: {name}");

// Async map
Result<string> asyncMapped = await userResult
    .MapAsync(async user => await FormatUserAsync(user));
```

### Bind - Chain Operations

```csharp
// Chain operations that return Results
Result<Order> orderResult = GetUser(userId)
    .Bind(user => GetAccount(user.AccountId))
    .Bind(account => CreateOrder(account));

// With different types
Result<string> formatted = GetUser(userId)
    .Bind(user => ValidateUser(user))
    .Bind(user => GetUserProfile(user.Id))
    .Map(profile => profile.ToString());

// Async bind
Result<Order> asyncOrder = await GetUserAsync(userId)
    .BindAsync(user => GetAccountAsync(user.AccountId))
    .BindAsync(account => CreateOrderAsync(account));
```

### MapError - Transform Error

```csharp
// Transform the error if failed
Result<User> result = GetUser(userId)
    .MapError(error => new CustomError($"Failed to get user: {error.Message}"));

// Add context to errors
Result<User> withContext = GetUser(userId)
    .MapError(error => error.WithMetadata("userId", userId));
```

### Tap - Side Effects

```csharp
// Execute side effects without changing the result
Result<User> result = GetUser(userId)
    .Tap(user => Console.WriteLine($"Found user: {user.Name}"))
    .Tap(user => _cache.Set(userId, user))
    .TapError(error => _logger.LogError(error.Message));

// Async tap
await GetUserAsync(userId)
    .TapAsync(async user => await AuditLogAsync($"User {user.Id} accessed"))
    .TapErrorAsync(async error => await NotifyAdminAsync(error));
```

## Combining Results

### BindAll - Combine Multiple Results

```csharp
// Combine 2 results
Result<(User, Account)> combined = Result.BindAll(
    GetUser(userId),
    GetAccount(accountId));

// Combine 3 results
Result<(User, Account, Settings)> triple = Result.BindAll(
    GetUser(userId),
    GetAccount(accountId),
    GetSettings(userId));

// With transformation
Result<Dashboard> dashboard = Result.BindAll(
    GetUser(userId),
    GetAccount(accountId),
    GetSettings(userId))
    .Map((user, account, settings) => new Dashboard(user, account, settings));

// Up to 8 results supported
var many = Result.BindAll(r1, r2, r3, r4, r5, r6, r7, r8);
```

### Ensure - Add Validations

```csharp
// Add validation to existing result
Result<User> validated = GetUser(userId)
    .Ensure(user => user.IsActive, new ValidationError("User is not active"))
    .Ensure(user => user.Email != null, new ValidationError("Email is required"))
    .Ensure(user => user.Age >= 18, new ValidationError("Must be 18 or older"));

// With custom error factory
Result<Order> validOrder = GetOrder(orderId)
    .Ensure(
        order => order.Total > 0,
        order => new ValidationError($"Invalid total: {order.Total}"));

// Multiple validations with EnsureAll
Result<User> fullyValidated = GetUser(userId)
    .EnsureAll(
        (user => user.IsActive, new ValidationError("User inactive")),
        (user => user.EmailVerified, new ValidationError("Email not verified")),
        (user => user.PhoneVerified, new ValidationError("Phone not verified")));
```

### Compensate - Error Recovery

```csharp
// Recover from specific errors
Result<User> withFallback = GetUser(userId)
    .Compensate(error =>
    {
        if (error is NotFoundError)
            return Result.Success(User.Guest);
        return Result.Failure<User>(error);
    });

// Try alternative on failure
Result<Config> config = LoadConfigFromFile()
    .CompensateWith(() => LoadConfigFromDatabase())
    .CompensateWith(() => Result.Success(Config.Default));
```

## Error Handling

### Built-in Error Types

```csharp
// Basic Error
var error = new Error("USER_NOT_FOUND", "The requested user was not found");

// Validation Error
var validation = new ValidationError("Email is required")
    .WithMetadata("field", "email");

// Not Found Error
var notFound = new NotFoundError("User", userId);

// Conflict Error
var conflict = new ConflictError("Username already exists");

// Authentication Error
var auth = new AuthenticationError("Invalid credentials");

// Authorization Error
var authz = new AuthorizationError("Insufficient permissions");

// Aggregate Error (multiple errors)
var aggregate = new AggregateError("Multiple validation errors", 
    new ValidationError("Email required"),
    new ValidationError("Password too short"));
```

### Error Metadata

```csharp
// Add metadata to errors
var error = new Error("VALIDATION_FAILED", "Input validation failed")
    .WithMetadata("field", "email")
    .WithMetadata("value", email)
    .WithMetadata("timestamp", DateTime.UtcNow);

// Access metadata
if (result.IsFailure && result.Error.Metadata.TryGetValue("field", out var field))
{
    Console.WriteLine($"Error in field: {field}");
}
```

### Error Comparison

```csharp
Result<User> result = GetUser();

// Check error type
if (result.IsFailure && result.Error is NotFoundError)
{
    // Handle not found
}

// Check error code
if (result.IsFailure && result.Error.Code == "USER_LOCKED")
{
    // Handle locked user
}

// Pattern matching on errors
var response = result.Error switch
{
    NotFoundError => "User not found",
    ValidationError => "Invalid input",
    AuthenticationError => "Please log in",
    _ => "An error occurred"
};
```

## Async Operations

### Task<Result<T>> Extensions

```csharp
// Async map
Task<Result<UserDto>> dtoTask = GetUserAsync(userId)
    .MapAsync(user => new UserDto(user));

// Async bind
Task<Result<Order>> orderTask = GetUserAsync(userId)
    .BindAsync(user => GetAccountAsync(user.AccountId))
    .BindAsync(account => CreateOrderAsync(account));

// Async ensure
Task<Result<User>> validatedTask = GetUserAsync(userId)
    .EnsureAsync(async user => await IsActiveAsync(user), 
        new ValidationError("User not active"));

// Async match
string message = await GetUserAsync(userId)
    .MatchAsync(
        onSuccess: async user => await FormatMessageAsync(user),
        onFailure: error => Task.FromResult(error.Message));
```

### Combining Async Results

```csharp
// Combine async operations
Task<Result<(User, Account)>> combined = Result.BindAllAsync(
    GetUserAsync(userId),
    GetAccountAsync(accountId));

// Wait for all and combine
var tasks = ids.Select(id => GetUserAsync(id)).ToArray();
Result<User[]> allUsers = await Result.CombineAsync(tasks);
```

## Extension Methods

### Collection Extensions

```csharp
// Combine collection of results
List<Result<User>> userResults = GetMultipleUsers();
Result<List<User>> combined = userResults.Combine();

// Filter successful results
IEnumerable<User> successfulUsers = userResults.WhereSuccess();

// Partition by success/failure
var (successes, failures) = userResults.Partition();

// Select with Result
Result<List<UserDto>> dtos = users
    .Select(user => ValidateUser(user).Map(u => new UserDto(u)))
    .Combine();
```

### LINQ Support

```csharp
// LINQ query syntax
var query = from user in GetUser(userId)
            from account in GetAccount(user.AccountId)
            from settings in GetSettings(user.Id)
            select new Dashboard(user, account, settings);

// SelectMany for chaining
Result<Order> order = GetUser(userId)
    .SelectMany(user => GetAccount(user.AccountId))
    .SelectMany(account => CreateOrder(account));

// Where for filtering
Result<User> activeUser = GetUser(userId)
    .Where(user => user.IsActive, 
        new ValidationError("User is not active"));
```

### Conversion Methods

```csharp
// To Option
Option<User> option = userResult.ToOption();

// To nullable
User? nullable = userResult.ToNullable();

// To Task
Task<Result<User>> task = userResult.ToTask();

// To ValueTask
ValueTask<Result<User>> valueTask = userResult.ToValueTask();

// To Either (if using functional libraries)
Either<Error, User> either = userResult.ToEither();
```

## Type Conversions

### Implicit Conversions

```csharp
// From value to Result<T>
Result<int> result = 42;
Result<string> name = "John";

// From Error to Result<T>
Result<int> failure = new Error("CALC_ERROR", "Calculation failed");

// Cannot implicitly convert null
// Result<string> wrong = null; // Compilation error
```

### Explicit Conversions

```csharp
// Cast to value (throws if failed)
Result<int> result = GetNumber();
int value = (int)result; // Throws if IsFailure

// Safe conversion with default
int safeValue = result.GetValueOr(0);
int lazyDefault = result.GetValueOr(() => CalculateDefault());
```

### Result Operators

```csharp
// Equality (compares values if both successful)
Result<int> r1 = 42;
Result<int> r2 = 42;
bool equal = r1.Equals(r2); // true

// ToString
Result<User> userResult = GetUser();
string str = userResult.ToString(); 
// Success: "Success(User { Name = John })"
// Failure: "Failure(NotFoundError: User not found)"
```

## Advanced Patterns

### Railway-Oriented Programming

```csharp
// Build a processing pipeline
Result<ProcessedOrder> processOrder = ParseOrder(orderData)
    .Bind(order => ValidateOrder(order))
    .Bind(order => CheckInventory(order))
    .Bind(order => CalculatePricing(order))
    .Bind(order => ApplyDiscounts(order))
    .Bind(order => ProcessPayment(order))
    .Map(order => new ProcessedOrder(order));
```

### Monadic Composition

```csharp
// Kleisli composition
Func<User, Result<Account>> getAccount = user => GetAccount(user.AccountId);
Func<Account, Result<Balance>> getBalance = account => GetBalance(account.Id);

var getBalanceForUser = getAccount.Compose(getBalance);
Result<Balance> balance = GetUser(userId).Bind(getBalanceForUser);
```

### Error Accumulation

```csharp
// Accumulate all validation errors
public Result<User> ValidateUser(UserInput input)
{
    var errors = new List<Error>();
    
    if (string.IsNullOrEmpty(input.Email))
        errors.Add(new ValidationError("Email is required"));
        
    if (input.Age < 18)
        errors.Add(new ValidationError("Must be 18 or older"));
        
    if (!IsValidPassword(input.Password))
        errors.Add(new ValidationError("Invalid password"));
        
    return errors.Any()
        ? Result.Failure<User>(new AggregateError("Validation failed", errors))
        : Result.Success(new User(input));
}
```

## Best Practices

1. **Always handle both success and failure cases**
   ```csharp
   // Good
   result.Match(
       onSuccess: user => ProcessUser(user),
       onFailure: error => HandleError(error));
   
   // Avoid
   var user = result.Value; // Can throw
   ```

2. **Use specific error types**
   ```csharp
   // Good
   return new NotFoundError($"User {userId} not found");
   
   // Less specific
   return new Error("NOT_FOUND", "User not found");
   ```

3. **Chain operations with Bind**
   ```csharp
   // Good - clear flow
   GetUser(id)
       .Bind(user => UpdateUser(user))
       .Bind(user => SaveUser(user));
   
   // Avoid - nested checks
   var userResult = GetUser(id);
   if (userResult.IsSuccess)
   {
       var updateResult = UpdateUser(userResult.Value);
       if (updateResult.IsSuccess)
       {
           SaveUser(updateResult.Value);
       }
   }
   ```

4. **Add context to errors**
   ```csharp
   GetUser(userId)
       .MapError(error => error.WithMetadata("operation", "GetUser")
                              .WithMetadata("userId", userId));
   ```

## Summary

The Result API provides a comprehensive set of operations for:
- Creating and checking results
- Transforming values and errors  
- Chaining operations
- Combining multiple results
- Handling async operations
- Working with collections

Next steps:
- [Option API Reference](option-api.md)
- [Error Types Reference](error-types.md)
- [Source Generators Reference](source-generators.md)
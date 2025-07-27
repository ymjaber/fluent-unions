# Result Pattern Tutorial

This comprehensive tutorial will teach you everything about the Result pattern in FluentUnions, from basic concepts to advanced techniques.

## Table of Contents
1. [Introduction](#introduction)
2. [Creating Results](#creating-results)
3. [Checking Results](#checking-results)
4. [Working with Errors](#working-with-errors)
5. [Transforming Results](#transforming-results)
6. [Chaining Operations](#chaining-operations)
7. [Error Recovery](#error-recovery)
8. [Advanced Patterns](#advanced-patterns)
9. [Best Practices](#best-practices)

## Introduction

The Result pattern represents an operation that can either succeed with a value or fail with an error. It's an alternative to throwing exceptions for expected failures.

### Why Use Result?

```csharp
// Traditional approach with exceptions
public User GetUser(Guid id)
{
    var user = _db.Find(id);
    if (user == null)
        throw new NotFoundException(); // Hidden failure path
    return user;
}

// Result pattern approach
public Result<User> GetUser(Guid id)
{
    var user = _db.Find(id);
    if (user == null)
        return Result.Failure<User>(new NotFoundError($"User {id} not found"));
    return Result.Success(user);
}
```

Benefits:
- Explicit error handling in method signatures
- No hidden control flow
- Better performance (no exception overhead)
- Composable error handling

## Creating Results

### Unit Results (No Value)

Unit results represent operations that succeed with no return value:

```csharp
// Success with no value
Result success = Result.Success();

// Failure with error message
Result failure = Result.Failure("Something went wrong");

// Failure with Error object
Result errorFailure = Result.Failure(new Error("ERR_001", "Operation failed"));
```

### Value Results

Value results contain data on success:

```csharp
// Success with value
Result<int> successValue = Result.Success(42);
Result<string> successString = Result.Success("Hello");

// Failure for value results
Result<int> failureValue = Result.Failure<int>("Calculation failed");
Result<User> failureUser = Result.Failure<User>(new NotFoundError("User not found"));
```

### Implicit Conversions

FluentUnions supports implicit conversions for cleaner code:

```csharp
public Result<User> GetUser(Guid id)
{
    var user = _repository.Find(id);
    
    // Implicit conversion from User to Result<User>
    if (user != null)
        return user; // Same as Result.Success(user)
    
    // Implicit conversion from Error to Result<User>
    return new NotFoundError($"User {id} not found");
}
```

### Try Pattern

Convert exception-throwing code to Results:

```csharp
// Wrap any operation that might throw
Result<int> result = Result.Try(() => int.Parse("123"));

// With custom error transformation
Result<Data> dataResult = Result.Try(() => LoadDataFromFile())
    .MapError(ex => new Error("FILE_ERROR", $"Failed to load: {ex.Message}"));

// Async version
Result<string> asyncResult = await Result.TryAsync(async () => 
{
    return await httpClient.GetStringAsync(url);
});
```

## Checking Results

### Basic Checks

```csharp
var result = GetUser(userId);

// Check if successful
if (result.IsSuccess)
{
    var user = result.Value;
    Console.WriteLine($"Found user: {user.Name}");
}

// Check if failed
if (result.IsFailure)
{
    var error = result.Error;
    Console.WriteLine($"Error: {error.Message}");
}
```

### Pattern Matching

Use `Match` for exhaustive handling:

```csharp
// Basic matching
string message = result.Match(
    onSuccess: user => $"Welcome, {user.Name}!",
    onFailure: error => $"Error: {error.Message}"
);

// Match with different return types
IActionResult response = result.Match<IActionResult>(
    onSuccess: user => Ok(user),
    onFailure: error => BadRequest(error.Message)
);

// Match with side effects
result.Match(
    onSuccess: user => _logger.LogInfo($"User {user.Id} logged in"),
    onFailure: error => _logger.LogError(error.Message)
);
```

### Switch Expressions (C# 8+)

```csharp
var message = result switch
{
    { IsSuccess: true } => $"Success: {result.Value}",
    { IsFailure: true } => $"Failed: {result.Error.Message}",
    _ => "Unknown state" // Should never happen
};
```

## Working with Errors

### Built-in Error Types

FluentUnions provides semantic error types:

```csharp
// Validation errors (400 Bad Request)
return new ValidationError("Email is required");

// Not found errors (404 Not Found)  
return new NotFoundError("User", userId);

// Conflict errors (409 Conflict)
return new ConflictError("Username already taken");

// Authentication errors (401 Unauthorized)
return new AuthenticationError("Invalid credentials");

// Authorization errors (403 Forbidden)
return new AuthorizationError("Insufficient permissions");
```

### Custom Error Types

Create domain-specific errors:

```csharp
public class InsufficientFundsError : Error
{
    public decimal Required { get; }
    public decimal Available { get; }
    
    public InsufficientFundsError(decimal required, decimal available)
        : base("INSUFFICIENT_FUNDS", 
               $"Required ${required} but only ${available} available")
    {
        Required = required;
        Available = available;
    }
}

// Usage
if (account.Balance < amount)
    return new InsufficientFundsError(amount, account.Balance);
```

### Error Metadata

Attach additional context to errors:

```csharp
var error = new ValidationError("Invalid input")
    .WithMetadata("field", "email")
    .WithMetadata("value", userInput)
    .WithMetadata("timestamp", DateTime.UtcNow);

// Access metadata
var field = error.Metadata["field"];
```

### Aggregate Errors

Combine multiple errors:

```csharp
public Result ValidateUser(UserDto dto)
{
    var errors = new List<Error>();
    
    if (string.IsNullOrEmpty(dto.Email))
        errors.Add(new ValidationError("Email is required"));
        
    if (dto.Age < 18)
        errors.Add(new ValidationError("Must be 18 or older"));
        
    if (!IsValidPassword(dto.Password))
        errors.Add(new ValidationError("Password too weak"));
    
    return errors.Any() 
        ? Result.Failure(new AggregateError(errors))
        : Result.Success();
}
```

## Transforming Results

### Map - Transform Success Values

Map transforms the value inside a successful Result:

```csharp
// Simple transformation
Result<string> nameResult = GetUserName(id);
Result<int> lengthResult = nameResult.Map(name => name.Length);

// Chain multiple maps
Result<Order> orderResult = GetOrder(id)
    .Map(order => ApplyDiscount(order))
    .Map(order => CalculateTax(order))
    .Map(order => AddShipping(order));

// Map to different type
Result<UserDto> dtoResult = GetUser(id)
    .Map(user => new UserDto 
    { 
        Id = user.Id, 
        Name = user.Name,
        Email = user.Email
    });
```

### MapError - Transform Failures

Transform error information:

```csharp
// Change error type
Result<User> result = GetUser(id)
    .MapError(error => new CustomError($"Failed to get user: {error.Message}"));

// Add context to errors
Result<Order> orderResult = ProcessOrder(request)
    .MapError(error => error.WithMetadata("orderId", request.OrderId));

// Convert exception to error
Result<Data> data = Result.Try(() => LoadData())
    .MapError(ex => new Error("LOAD_FAILED", ex.Message));
```

### Tap - Side Effects

Execute side effects without changing the Result:

```csharp
Result<User> result = GetUser(id)
    .Tap(user => _logger.LogInfo($"Retrieved user {user.Id}"))
    .Tap(user => _cache.Set(user.Id, user))
    .TapError(error => _logger.LogError(error.Message));
```

## Chaining Operations

### Bind - Sequential Operations

Bind chains operations that return Results:

```csharp
// Each operation depends on the previous
Result<Order> result = ValidateCustomer(customerId)
    .Bind(customer => CheckCredit(customer))
    .Bind(credit => CreateOrder(customerId, items))
    .Bind(order => ReserveInventory(order))
    .Bind(order => ProcessPayment(order))
    .Bind(order => SendConfirmation(order));

// Real-world example
public Result<User> RegisterUser(RegisterRequest request)
{
    return ValidateEmail(request.Email)
        .Bind(() => CheckEmailAvailable(request.Email))
        .Bind(() => HashPassword(request.Password))
        .Bind(hash => CreateUser(request.Email, hash))
        .Bind(user => SendWelcomeEmail(user))
        .Tap(user => _logger.LogInfo($"User {user.Id} registered"));
}
```

### Ensure - Add Validations

Add validation checks to existing Results:

```csharp
Result<Order> result = GetOrder(id)
    .Ensure(order => order.Status == OrderStatus.Pending, 
            "Order must be pending")
    .Ensure(order => order.Items.Any(), 
            "Order must have items")
    .Ensure(order => order.Total > 0, 
            "Order total must be positive");

// With custom errors
Result<User> userResult = GetUser(id)
    .Ensure(user => user.IsActive, 
            new ValidationError("User account is disabled"))
    .Ensure(user => user.EmailVerified, 
            new ValidationError("Email not verified"));
```

### Combine - Parallel Operations

Combine multiple Results:

```csharp
// Combine multiple operations
var results = new[]
{
    ValidateName(name),
    ValidateEmail(email),
    ValidateAge(age)
};

Result combined = Result.Combine(results);

// Combine with values
Result<(string, string, int)> combined = Result.Combine(
    GetFirstName(),
    GetLastName(),
    GetAge()
);

// Use BindAll for different types
public Result<Order> CreateOrder(Guid customerId, List<Guid> productIds)
{
    return Result.BindAll(
        GetCustomer(customerId),        // Result<Customer>
        GetProducts(productIds),         // Result<List<Product>>
        GetShippingAddress(customerId)   // Result<Address>
    )
    .Map((customer, products, address) => 
        new Order(customer, products, address));
}
```

## Error Recovery

### OrElse - Provide Alternatives

```csharp
// Try primary, fallback to secondary
Result<Config> config = LoadConfigFromFile()
    .OrElse(() => LoadConfigFromDatabase())
    .OrElse(() => LoadDefaultConfig());

// Conditional recovery
Result<User> user = GetUser(id)
    .OrElse(error => error is NotFoundError 
        ? CreateGuestUser() 
        : Result.Failure<User>(error));
```

### Compensate - Handle Specific Errors

```csharp
Result<Data> result = LoadData()
    .Compensate(error =>
    {
        if (error.Code == "NETWORK_ERROR")
            return LoadCachedData();
        if (error.Code == "TIMEOUT")
            return RetryLoadData();
        return Result.Failure<Data>(error);
    });
```

### Finally - Cleanup

```csharp
Result<string> ReadFile(string path)
{
    Stream? stream = null;
    return Result.Try(() =>
    {
        stream = File.OpenRead(path);
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    })
    .Finally(() => stream?.Dispose());
}
```

## Advanced Patterns

### Railway-Oriented Programming

Build complex flows with clean error handling:

```csharp
public class OrderService
{
    public Result<OrderConfirmation> ProcessOrder(OrderRequest request)
    {
        return ValidateRequest(request)
            .Bind(CalculatePricing)
            .Bind(CheckInventory)
            .Bind(ReserveItems)
            .Bind(ProcessPayment)
            .Bind(CreateShipment)
            .Bind(SendNotifications)
            .Map(ToOrderConfirmation);
    }
    
    private Result<PricedOrder> CalculatePricing(ValidatedOrder order)
    {
        return GetProductPrices(order.Items)
            .Map(prices => ApplyPricing(order, prices))
            .Bind(ApplyDiscounts)
            .Bind(CalculateTaxes)
            .Ensure(po => po.Total > 0, "Order total must be positive");
    }
}
```

### Async Operations

Work with async Results:

```csharp
public async Task<Result<User>> GetUserAsync(Guid id)
{
    return await Result.TryAsync(async () =>
    {
        var user = await _dbContext.Users.FindAsync(id);
        return user ?? throw new NotFoundException();
    })
    .MapError(ex => new NotFoundError($"User {id} not found"));
}

// Async bind chain
public async Task<Result<Order>> ProcessOrderAsync(OrderRequest request)
{
    return await ValidateRequestAsync(request)
        .BindAsync(async validated => await CheckInventoryAsync(validated))
        .BindAsync(async reserved => await ProcessPaymentAsync(reserved))
        .MapAsync(async payment => await CreateOrderAsync(payment));
}
```

### Validation Chains

Build complex validation pipelines:

```csharp
public class UserValidator
{
    public Result<ValidatedUser> Validate(UserInput input)
    {
        return Result.Success(input)
            .Ensure(i => !string.IsNullOrWhiteSpace(i.Email), 
                    "Email is required")
            .Ensure(i => i.Email.Contains('@'), 
                    "Invalid email format")
            .Ensure(i => i.Age >= 18, 
                    "Must be 18 or older")
            .Ensure(i => i.Password.Length >= 8, 
                    "Password too short")
            .Map(i => new ValidatedUser(i));
    }
}
```

### Transaction Pattern

Ensure all-or-nothing operations:

```csharp
public Result<Order> PlaceOrder(OrderRequest request)
{
    using var transaction = _db.BeginTransaction();
    
    return CreateOrder(request)
        .Bind(order => DeductInventory(order.Items))
        .Bind(() => ChargePayment(request.PaymentInfo))
        .Bind(() => SaveOrder(order))
        .Tap(_ => transaction.Commit())
        .TapError(_ => transaction.Rollback());
}
```

## Best Practices

### 1. Return Early on Failure

```csharp
public Result<decimal> CalculateDiscount(Order order)
{
    // Check prerequisites first
    if (order.Items.Count == 0)
        return new ValidationError("Order has no items");
        
    if (order.Customer == null)
        return new ValidationError("Order has no customer");
    
    // Main logic
    var discount = CalculateCustomerDiscount(order.Customer);
    return Result.Success(discount);
}
```

### 2. Use Semantic Error Types

```csharp
// Don't use generic errors
return Result.Failure("User not found"); // Bad

// Use specific error types
return new NotFoundError("User", userId); // Good
```

### 3. Keep Operations Pure

```csharp
// Don't mix side effects with transformations
public Result<User> UpdateUser(User user)
{
    // Bad: Side effect in Map
    return GetUser(user.Id)
        .Map(u => 
        {
            _db.Save(u); // Side effect!
            return u;
        });
    
    // Good: Use Bind for operations with side effects
    return GetUser(user.Id)
        .Bind(u => SaveUser(u));
}
```

### 4. Design for Composition

```csharp
// Small, composable functions
private Result<Email> ValidateEmail(string input) { }
private Result CheckEmailAvailable(Email email) { }
private Result<HashedPassword> HashPassword(string password) { }
private Result<User> CreateUser(Email email, HashedPassword password) { }

// Compose them
public Result<User> RegisterUser(string email, string password)
{
    return ValidateEmail(email)
        .Bind(e => CheckEmailAvailable(e).Map(_ => e))
        .Bind(e => HashPassword(password).Map(p => (e, p)))
        .Bind(t => CreateUser(t.e, t.p));
}
```

### 5. Document Error Cases

```csharp
/// <summary>
/// Gets a user by ID.
/// </summary>
/// <returns>
/// Success: The requested user
/// Failure: NotFoundError if user doesn't exist
/// Failure: DatabaseError if database connection fails
/// </returns>
public Result<User> GetUser(Guid id) { }
```

## Summary

The Result pattern provides a powerful way to handle errors explicitly and functionally. Key takeaways:

1. Use Result for operations that can fail in expected ways
2. Leverage functional operations (Map, Bind, Match) for clean code
3. Create semantic error types for different failure scenarios
4. Build complex flows with railway-oriented programming
5. Keep operations pure and composable

Next steps:
- [Option Pattern Tutorial](option-pattern-basics.md)
- [Error Handling Guide](error-handling.md)
- [Functional Operations Reference](../guides/functional-operations.md)
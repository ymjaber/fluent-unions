# Core Concepts

Understanding the fundamental concepts behind FluentUnions will help you use the library effectively and write more robust code.

## The Problem: Handling Failure and Absence

### Traditional Exception-Based Error Handling

```csharp
// Problems with exceptions:
public User GetUser(Guid id)
{
    var user = database.Find(id);
    if (user == null)
        throw new NotFoundException($"User {id} not found"); // Hidden control flow

    if (!user.IsActive)
        throw new InvalidOperationException("User is inactive"); // Expensive

    return user; // Will this method normally fail? What errors can this throw? Not clear from signature
}

// Caller must know to catch exceptions
try
{
    var user = GetUser(userId);
    // Happy path
}
catch (NotFoundException ex) // Which exceptions to catch?
{
    // Handle not found
}
catch (InvalidOperationException ex) // Easy to miss
{
    // Handle inactive user
}
```

### Problems with Null

```csharp
// Nullable reference types help, but...
public User? FindUser(string email)
{
    return database.Users.FirstOrDefault(u => u.Email == email);
    // Null could mean: not found, error, or actual null value
}

// Checking for null everywhere
var user = FindUser(email);
if (user != null) // Easy to forget
{
    if (user.Profile != null) // Null checks cascade
    {
        if (user.Profile.Address != null) // Gets messy fast
        {
            // Finally can use the data
        }
    }
}
```

## The Solution: Result and Option Types

### Result Pattern: Explicit Success or Failure

The Result pattern makes success and failure explicit in the type system:

```csharp
public Result<User> GetUser(Guid id)
{
    var user = database.Find(id);
    if (user == null)
        return Result.Failure<User>(new NotFoundError($"User {id} not found"));

    if (!user.IsActive)
        return Result.Failure<User>(new ValidationError("User is inactive"));

    return Result.Success(user);
}

// Caller must handle both cases
var result = GetUser(userId);
if (result.IsSuccess)
{
    var user = result.Value;
    // Handle success
}
else
{
    var error = result.Error;
    // Handle specific error
}
```

### Option Pattern: Explicit Presence or Absence

The Option pattern makes the presence or absence of a value explicit:

```csharp
public Option<User> FindUser(string email)
{
    var user = database.Users.FirstOrDefault(u => u.Email == email);
    return Option.From(user); // Converts null to None
}

// Clear intent: this value might not exist
var userOption = FindUser(email);
if (userOption.IsSome)
{
    var user = userOption.Value;
    // Use the user
}
```

## Key Principles

### 1. Make the Implicit Explicit

**Traditional:**

```csharp
public decimal CalculateDiscount(Order order)
{
    // Throws exception if order is null or invalid
    // Returns 0 if no discount applies
    // Not clear from the signature
}
```

**With FluentUnions:**

```csharp
public Result<decimal> CalculateDiscount(Order order)
{
    // Success with discount amount or
    // Failure with specific error
    // Clear from the signature
}
```

### 2. Use the Type System

Types should tell the whole story:

```csharp
// This signature tells you everything:
public Result<Option<User>> FindActiveUser(Guid id)
{
    // Can fail (database error) -> Result
    // User might not exist -> Option
    // Returns active user if found
}

// Usage makes all cases explicit:
var result = FindActiveUser(userId);
result.Match(
    onSuccess: option => option.Match(
        onSome: user => Console.WriteLine($"Found: {user.Name}"),
        onNone: () => Console.WriteLine("User not found")
    ),
    onFailure: error => Console.WriteLine($"Error: {error.Message}")
);
```

### 3. Fail Fast, Fail Explicitly

```csharp
public Result<Order> ProcessOrder(OrderRequest request)
{
    // Each step can fail, and we handle it explicitly
    return ValidateRequest(request)      // Result<ValidatedRequest>
        .Bind(CheckInventory)            // Result<InventoryReservation>
        .Bind(ProcessPayment)            // Result<PaymentConfirmation>
        .Bind(CreateOrder)               // Result<Order>
        .Bind(SendConfirmation);         // Result<Order>

    // If any step fails, the chain stops and returns the error
}
```

### 4. Composition Over Imperative Code

**Imperative Style:**

```csharp
public string? GetUserDisplayName(Guid id)
{
    var user = GetUser(id);
    if (user == null) return null;

    if (string.IsNullOrEmpty(user.DisplayName))
    {
        if (!string.IsNullOrEmpty(user.FullName))
            return user.FullName;
        else
            return user.Email;
    }

    return user.DisplayName;
}
```

**Functional Style:**

```csharp
public Option<string> GetUserDisplayName(Guid id)
{
    return GetUser(id)
        .Bind(user => Option.From(user.DisplayName)
            .OrElse(() => Option.From(user.FullName))
            .OrElse(() => Option.From(user.Email)));
}
```

## Understanding Monadic Operations

### What is a Monad?

Don't worry about the academic definition. In practical terms, Result and Option are "containers" that:

1. Hold a value (or not)
2. Provide operations to work with the value without "unwrapping" it
3. Allow chaining operations that maintain the container type

### Core Operations

#### Map: Transform the Value

Map applies a function to the inner value if it exists:

```csharp
// Result<string> -> Result<int>
Result<string> name = GetUserName(id);
Result<int> length = name.Map(n => n.Length);

// Option<decimal> -> Option<string>
Option<decimal> price = GetPrice(productId);
Option<string> formatted = price.Map(p => $"${p:F2}");
```

#### Bind (FlatMap): Chain Operations

Bind chains operations that themselves return Result/Option:

```csharp
// Each operation returns Result<T>
Result<User> userResult = GetUser(id)
    .Bind(user => ValidateUser(user))      // Returns Result<User>
    .Bind(user => UpdateLastLogin(user))   // Returns Result<User>
    .Bind(user => SaveUser(user));         // Returns Result<User>

// Without Bind (messy):
var result1 = GetUser(id);
if (result1.IsFailure) return result1.Error;

var result2 = ValidateUser(result1.Value);
if (result2.IsFailure) return result2.Error;

var result3 = UpdateLastLogin(result2.Value);
// ... and so on
```

#### Match: Handle All Cases

Match provides exhaustive pattern matching:

```csharp
// Force handling of both success and failure
string message = result.Match(
    onSuccess: value => $"Got value: {value}",
    onFailure: error => $"Failed: {error.Message}"
);

// Can return different types
IActionResult response = result.Match<IActionResult>(
    onSuccess: data => Ok(data),
    onFailure: error => BadRequest(error)
);
```

## Railway-Oriented Programming

Think of your operations as a railway with two tracks: Success and Failure.

```
     Success Track
     =============>
Start              End
     =============>
     Failure Track
```

Each operation can switch from Success to Failure, but once on Failure track, you stay there:

```csharp
public Result<Invoice> ProcessInvoice(InvoiceRequest request)
{
    return ValidateRequest(request)        // Can fail -> Failure track
        .Bind(CreateInvoice)              // Only runs if still on Success track
        .Bind(CalculateTotals)            // Only runs if still on Success track
        .Bind(ApplyTaxes)                 // Only runs if still on Success track
        .Bind(SaveToDatabase)             // Only runs if still on Success track
        .Map(invoice => {                 // Only runs if still on Success track
            _logger.LogInfo($"Created invoice {invoice.Id}");
            return invoice;
        });
}
```

## Error Philosophy

### Errors as Values

Errors in FluentUnions are first-class values, not exceptions:

```csharp
public class Error
{
    public string Code { get; }
    public string Message { get; }
    public IReadOnlyDictionary<string, object> Metadata { get; }
}
```

### Semantic Error Types

Use specific error types to convey meaning:

```csharp
// Different errors convey different meanings
return new NotFoundError("User not found");         // 404
return new ValidationError("Email required");       // 400
return new ConflictError("Email already exists");   // 409
return new AuthenticationError("Invalid token");    // 401
return new AuthorizationError("Access denied");     // 403
```

### Error Metadata

Attach context to errors:

```csharp
var error = new ValidationError("Invalid age")
    .WithMetadata("field", "age")
    .WithMetadata("value", age)
    .WithMetadata("min", 0)
    .WithMetadata("max", 150);
```

## Design Patterns

### 1. Parse, Don't Validate

Instead of validating and throwing, parse into a Result:

```csharp
// Don't do this:
public void ValidateEmail(string email)
{
    if (!IsValid(email))
        throw new ValidationException("Invalid email");
}

// Do this:
public Result<Email> ParseEmail(string input)
{
    if (string.IsNullOrWhiteSpace(input))
        return new ValidationError("Email cannot be empty");

    if (!input.Contains('@'))
        return new ValidationError("Email must contain @");

    return Result.Success(new Email(input));
}
```

### 2. Make Invalid States Unrepresentable

Use types to prevent invalid states:

```csharp
// Instead of:
public class Order
{
    public OrderStatus Status { get; set; }
    public DateTime? ShippedDate { get; set; } // Could be set when Status != Shipped
    public string? TrackingNumber { get; set; } // Could be set incorrectly
}

// Use:
public abstract class Order { }
public class PendingOrder : Order { }
public class ShippedOrder : Order
{
    public DateTime ShippedDate { get; }
    public TrackingNumber Tracking { get; } // Required when shipped
}
```

### 3. Aggregate Errors

Collect multiple validation errors:

```csharp
public Result ValidateForm(FormData data)
{
    var errors = new List<Error>();

    if (string.IsNullOrEmpty(data.Name))
        errors.Add(new ValidationError("Name is required"));

    if (string.IsNullOrEmpty(data.Email))
        errors.Add(new ValidationError("Email is required"));

    if (data.Age < 0 || data.Age > 150)
        errors.Add(new ValidationError("Invalid age"));

    return errors.Any()
        ? Result.Failure(new AggregateError(errors))
        : Result.Success();
}
```

## Summary

FluentUnions provides a robust foundation for handling errors and optional values by:

1. **Making implicit behavior explicit** through types
2. **Using the type system** to enforce correct handling
3. **Providing functional composition** for elegant code
4. **Treating errors as values** rather than exceptions
5. **Enabling railway-oriented programming** for clean flows

These concepts lead to code that is:

- More predictable (no hidden exceptions)
- More maintainable (errors are explicit)
- More testable (pure functions)
- More performant (no exception overhead)

Next, dive deeper into:

- [Result Pattern Tutorial](../tutorials/result-pattern-basics.md)
- [Option Pattern Tutorial](../tutorials/option-pattern-basics.md)
- [Functional Operations Guide](../guides/functional-operations.md)


# Error Handling Guide

This guide provides comprehensive coverage of error handling in FluentUnions, including built-in error types, custom errors, error composition, and best practices.

## Table of Contents
1. [Error Philosophy](#error-philosophy)
2. [Built-in Error Types](#built-in-error-types)
3. [Creating Custom Errors](#creating-custom-errors)
4. [Error Metadata](#error-metadata)
5. [Error Composition](#error-composition)
6. [Error Recovery Strategies](#error-recovery-strategies)
7. [Error Transformation](#error-transformation)
8. [Domain-Specific Errors](#domain-specific-errors)
9. [Error Handling Patterns](#error-handling-patterns)
10. [Best Practices](#best-practices)

## Error Philosophy

### Errors as Values

In FluentUnions, errors are first-class values, not exceptions:

```csharp
public class Error
{
    public string Code { get; }
    public string Message { get; }
    public IReadOnlyDictionary<string, object> Metadata { get; }
    
    public Error(string code, string message)
    {
        Code = code;
        Message = message;
        Metadata = new Dictionary<string, object>();
    }
}
```

### Why Errors as Values?

```csharp
// Traditional exception approach
try
{
    var user = GetUser(id); // What exceptions can this throw?
    var order = CreateOrder(user); // Hidden failure modes
    SendEmail(order); // More hidden failures
}
catch (NotFoundException) { } // Which method throws this?
catch (ValidationException) { } // Easy to miss
catch (Exception) { } // Catch-all anti-pattern

// FluentUnions approach
GetUser(id)
    .Bind(user => CreateOrder(user))
    .Bind(order => SendEmail(order))
    .Match(
        onSuccess: () => Console.WriteLine("Success"),
        onFailure: error => Console.WriteLine($"Failed: {error.Message}")
    );
```

## Built-in Error Types

FluentUnions provides semantic error types that map to common failure scenarios:

### ValidationError

For input validation failures (HTTP 400):

```csharp
public Result<User> CreateUser(string email, int age)
{
    if (string.IsNullOrWhiteSpace(email))
        return new ValidationError("Email is required");
        
    if (!email.Contains('@'))
        return new ValidationError("Invalid email format");
        
    if (age < 18)
        return new ValidationError("Must be 18 or older");
        
    return Result.Success(new User(email, age));
}
```

### NotFoundError

For missing resources (HTTP 404):

```csharp
public Result<Product> GetProduct(Guid id)
{
    var product = _repository.FindById(id);
    if (product == null)
        return new NotFoundError($"Product {id} not found");
        
    // Or with resource type
    return new NotFoundError("Product", id.ToString());
}
```

### ConflictError

For resource conflicts (HTTP 409):

```csharp
public Result<User> RegisterUser(string email)
{
    if (_repository.EmailExists(email))
        return new ConflictError($"Email {email} is already registered");
        
    // Create user...
}
```

### AuthenticationError

For authentication failures (HTTP 401):

```csharp
public Result<User> Authenticate(string username, string password)
{
    var user = _repository.FindByUsername(username);
    if (user == null || !VerifyPassword(password, user.PasswordHash))
        return new AuthenticationError("Invalid credentials");
        
    if (!user.EmailVerified)
        return new AuthenticationError("Email not verified");
        
    return Result.Success(user);
}
```

### AuthorizationError

For authorization failures (HTTP 403):

```csharp
public Result<Document> GetDocument(Guid documentId, User currentUser)
{
    var document = _repository.FindById(documentId);
    if (document == null)
        return new NotFoundError($"Document {documentId} not found");
        
    if (document.OwnerId != currentUser.Id && !currentUser.IsAdmin)
        return new AuthorizationError("You don't have permission to view this document");
        
    return Result.Success(document);
}
```

### AggregateError

For multiple errors:

```csharp
public Result ValidateOrder(OrderRequest request)
{
    var errors = new List<Error>();
    
    if (request.Items.Count == 0)
        errors.Add(new ValidationError("Order must contain at least one item"));
        
    if (request.CustomerId == Guid.Empty)
        errors.Add(new ValidationError("Customer ID is required"));
        
    foreach (var item in request.Items)
    {
        if (item.Quantity <= 0)
            errors.Add(new ValidationError($"Invalid quantity for item {item.ProductId}"));
    }
    
    return errors.Any() 
        ? Result.Failure(new AggregateError(errors))
        : Result.Success();
}
```

## Creating Custom Errors

### Basic Custom Errors

```csharp
public class BusinessRuleError : Error
{
    public string RuleName { get; }
    
    public BusinessRuleError(string ruleName, string message) 
        : base($"BUSINESS_RULE_{ruleName}", message)
    {
        RuleName = ruleName;
    }
}

// Usage
return new BusinessRuleError("MINIMUM_ORDER", "Order must be at least $10");
```

### Rich Domain Errors

```csharp
public class InsufficientInventoryError : Error
{
    public Guid ProductId { get; }
    public int Requested { get; }
    public int Available { get; }
    
    public InsufficientInventoryError(Guid productId, int requested, int available)
        : base("INSUFFICIENT_INVENTORY", 
               $"Requested {requested} units but only {available} available")
    {
        ProductId = productId;
        Requested = requested;
        Available = available;
    }
}

public class PaymentFailedError : Error
{
    public string PaymentMethod { get; }
    public string FailureReason { get; }
    public string? TransactionId { get; }
    
    public PaymentFailedError(string paymentMethod, string failureReason, string? transactionId = null)
        : base("PAYMENT_FAILED", $"Payment failed: {failureReason}")
    {
        PaymentMethod = paymentMethod;
        FailureReason = failureReason;
        TransactionId = transactionId;
    }
}
```

### Error Hierarchies

```csharp
public abstract class DomainError : Error
{
    protected DomainError(string code, string message) : base(code, message) { }
}

public class OrderError : DomainError
{
    protected OrderError(string code, string message) 
        : base($"ORDER_{code}", message) { }
}

public class OrderNotFoundError : OrderError
{
    public Guid OrderId { get; }
    
    public OrderNotFoundError(Guid orderId) 
        : base("NOT_FOUND", $"Order {orderId} not found")
    {
        OrderId = orderId;
    }
}

public class InvalidOrderStateError : OrderError
{
    public OrderStatus CurrentStatus { get; }
    public OrderStatus RequiredStatus { get; }
    
    public InvalidOrderStateError(OrderStatus current, OrderStatus required)
        : base("INVALID_STATE", 
               $"Order is in {current} state but must be in {required} state")
    {
        CurrentStatus = current;
        RequiredStatus = required;
    }
}
```

## Error Metadata

### Adding Metadata

```csharp
// Fluent API for metadata
var error = new ValidationError("Invalid input")
    .WithMetadata("field", "email")
    .WithMetadata("value", userInput)
    .WithMetadata("timestamp", DateTime.UtcNow)
    .WithMetadata("attemptNumber", 3);

// Create with metadata
var error2 = new Error("VALIDATION_FAILED", "Age out of range")
{
    ["min"] = 0,
    ["max"] = 150,
    ["actual"] = providedAge
};
```

### Using Metadata

```csharp
public IActionResult HandleError(Error error)
{
    // Log with metadata
    _logger.LogError("Operation failed: {Code} - {Message}. Metadata: {@Metadata}", 
        error.Code, error.Message, error.Metadata);
    
    // Build detailed error response
    var response = new
    {
        error = error.Code,
        message = error.Message,
        details = error.Metadata
    };
    
    return error switch
    {
        ValidationError => BadRequest(response),
        NotFoundError => NotFound(response),
        _ => StatusCode(500, response)
    };
}
```

## Error Composition

### Combining Multiple Validations

```csharp
public class UserValidator
{
    public Result<ValidatedUser> Validate(UserInput input)
    {
        var errors = new List<Error>();
        
        // Collect all validation errors
        ValidateEmail(input.Email).TapError(errors.Add);
        ValidatePassword(input.Password).TapError(errors.Add);
        ValidateAge(input.Age).TapError(errors.Add);
        ValidateName(input.Name).TapError(errors.Add);
        
        if (errors.Any())
            return Result.Failure<ValidatedUser>(new AggregateError(errors));
            
        return Result.Success(new ValidatedUser(input));
    }
    
    private Result ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return new ValidationError("Email is required")
                .WithMetadata("field", "email");
                
        if (!email.Contains('@'))
            return new ValidationError("Invalid email format")
                .WithMetadata("field", "email")
                .WithMetadata("value", email);
                
        return Result.Success();
    }
}
```

### Error Context Enrichment

```csharp
public class OrderService
{
    public Result<Order> ProcessOrder(OrderRequest request)
    {
        return ValidateRequest(request)
            .Bind(CreateOrder)
            .Bind(ReserveInventory)
            .Bind(ProcessPayment)
            .MapError(error => error
                .WithMetadata("orderId", request.Id)
                .WithMetadata("customerId", request.CustomerId)
                .WithMetadata("timestamp", DateTime.UtcNow));
    }
}
```

## Error Recovery Strategies

### Retry Pattern

```csharp
public async Task<Result<T>> RetryAsync<T>(
    Func<Task<Result<T>>> operation,
    int maxAttempts = 3,
    TimeSpan? delay = null)
{
    var attempts = 0;
    var errors = new List<Error>();
    
    while (attempts < maxAttempts)
    {
        attempts++;
        var result = await operation();
        
        if (result.IsSuccess)
            return result;
            
        errors.Add(result.Error
            .WithMetadata("attempt", attempts)
            .WithMetadata("timestamp", DateTime.UtcNow));
        
        if (attempts < maxAttempts && delay.HasValue)
            await Task.Delay(delay.Value);
    }
    
    return Result.Failure<T>(new AggregateError(errors)
        .WithMetadata("totalAttempts", attempts));
}
```

### Fallback Pattern

```csharp
public Result<Config> LoadConfiguration()
{
    return LoadFromFile()
        .OrElse(error =>
        {
            _logger.LogWarning("Failed to load from file: {Error}", error.Message);
            return LoadFromDatabase();
        })
        .OrElse(error =>
        {
            _logger.LogWarning("Failed to load from database: {Error}", error.Message);
            return LoadDefaultConfig();
        })
        .MapError(error => new Error("CONFIG_LOAD_FAILED", 
            "Failed to load configuration from any source"));
}
```

### Circuit Breaker Pattern

```csharp
public class CircuitBreaker<T>
{
    private int _failureCount;
    private DateTime _lastFailureTime;
    private readonly int _threshold;
    private readonly TimeSpan _timeout;
    
    public Result<T> Execute(Func<Result<T>> operation)
    {
        if (IsOpen)
            return Result.Failure<T>(new Error("CIRCUIT_OPEN", 
                "Circuit breaker is open"));
        
        var result = operation();
        
        if (result.IsSuccess)
        {
            Reset();
            return result;
        }
        
        RecordFailure();
        return result;
    }
    
    private bool IsOpen => 
        _failureCount >= _threshold && 
        DateTime.UtcNow - _lastFailureTime < _timeout;
}
```

## Error Transformation

### Error Mapping

```csharp
public class ErrorMapper
{
    public Error MapToHttpError(Error error)
    {
        return error switch
        {
            ValidationError ve => new Error("BAD_REQUEST", ve.Message)
                .WithMetadata("status", 400),
            NotFoundError nf => new Error("NOT_FOUND", nf.Message)
                .WithMetadata("status", 404),
            AuthenticationError ae => new Error("UNAUTHORIZED", ae.Message)
                .WithMetadata("status", 401),
            _ => new Error("INTERNAL_ERROR", "An unexpected error occurred")
                .WithMetadata("status", 500)
        };
    }
}
```

### Error Localization

```csharp
public class LocalizedError : Error
{
    public string MessageKey { get; }
    public object[] MessageArgs { get; }
    
    public LocalizedError(string code, string messageKey, params object[] args)
        : base(code, messageKey) // Store key as message temporarily
    {
        MessageKey = messageKey;
        MessageArgs = args;
    }
    
    public string GetLocalizedMessage(IStringLocalizer localizer)
    {
        return localizer[MessageKey, MessageArgs];
    }
}

// Usage
return new LocalizedError("VALIDATION_FAILED", "error.email.required");
return new LocalizedError("NOT_FOUND", "error.user.notfound", userId);
```

## Domain-Specific Errors

### E-Commerce Domain

```csharp
public class CartError : Error
{
    protected CartError(string code, string message) 
        : base($"CART_{code}", message) { }
}

public class ItemOutOfStockError : CartError
{
    public Guid ProductId { get; }
    public string ProductName { get; }
    
    public ItemOutOfStockError(Guid productId, string productName)
        : base("ITEM_OUT_OF_STOCK", $"{productName} is out of stock")
    {
        ProductId = productId;
        ProductName = productName;
    }
}

public class CartExpiredError : CartError
{
    public DateTime ExpiredAt { get; }
    
    public CartExpiredError(DateTime expiredAt)
        : base("EXPIRED", "Shopping cart has expired")
    {
        ExpiredAt = expiredAt;
    }
}
```

### Banking Domain

```csharp
public abstract class BankingError : Error
{
    protected BankingError(string code, string message) 
        : base(code, message) { }
}

public class InsufficientFundsError : BankingError
{
    public string AccountNumber { get; }
    public decimal Balance { get; }
    public decimal RequestedAmount { get; }
    
    public InsufficientFundsError(string accountNumber, decimal balance, decimal requested)
        : base("INSUFFICIENT_FUNDS", 
               $"Insufficient funds. Balance: ${balance}, Requested: ${requested}")
    {
        AccountNumber = accountNumber;
        Balance = balance;
        RequestedAmount = requested;
    }
}

public class AccountFrozenError : BankingError
{
    public string AccountNumber { get; }
    public string Reason { get; }
    
    public AccountFrozenError(string accountNumber, string reason)
        : base("ACCOUNT_FROZEN", $"Account {accountNumber} is frozen: {reason}")
    {
        AccountNumber = accountNumber;
        Reason = reason;
    }
}
```

## Error Handling Patterns

### Error Aggregation Pipeline

```csharp
public class ValidationPipeline<T>
{
    private readonly List<Func<T, Result>> _validators = new();
    
    public ValidationPipeline<T> AddValidator(Func<T, Result> validator)
    {
        _validators.Add(validator);
        return this;
    }
    
    public Result Validate(T input)
    {
        var errors = _validators
            .Select(validator => validator(input))
            .Where(result => result.IsFailure)
            .Select(result => result.Error)
            .ToList();
            
        return errors.Any() 
            ? Result.Failure(new AggregateError(errors))
            : Result.Success();
    }
}

// Usage
var pipeline = new ValidationPipeline<UserRequest>()
    .AddValidator(req => ValidateEmail(req.Email))
    .AddValidator(req => ValidatePassword(req.Password))
    .AddValidator(req => ValidateAge(req.Age));
    
var result = pipeline.Validate(request);
```

### Error Handler Chain

```csharp
public interface IErrorHandler
{
    bool CanHandle(Error error);
    Result<T> Handle<T>(Error error);
}

public class ErrorHandlerChain
{
    private readonly List<IErrorHandler> _handlers = new();
    
    public ErrorHandlerChain AddHandler(IErrorHandler handler)
    {
        _handlers.Add(handler);
        return this;
    }
    
    public Result<T> Handle<T>(Error error)
    {
        var handler = _handlers.FirstOrDefault(h => h.CanHandle(error));
        return handler?.Handle<T>(error) 
            ?? Result.Failure<T>(error);
    }
}

public class RetryableErrorHandler : IErrorHandler
{
    public bool CanHandle(Error error) => 
        error.Metadata.ContainsKey("retryable") && 
        (bool)error.Metadata["retryable"];
        
    public Result<T> Handle<T>(Error error)
    {
        // Implement retry logic
        return Result.Failure<T>(error.WithMetadata("retryAttempted", true));
    }
}
```

### Error Enrichment Pipeline

```csharp
public class ErrorEnricher
{
    private readonly List<Func<Error, Error>> _enrichers = new();
    
    public ErrorEnricher AddEnricher(Func<Error, Error> enricher)
    {
        _enrichers.Add(enricher);
        return this;
    }
    
    public Error Enrich(Error error)
    {
        return _enrichers.Aggregate(error, (current, enricher) => enricher(current));
    }
}

// Usage
var enricher = new ErrorEnricher()
    .AddEnricher(e => e.WithMetadata("timestamp", DateTime.UtcNow))
    .AddEnricher(e => e.WithMetadata("environment", Environment.MachineName))
    .AddEnricher(e => e.WithMetadata("userId", GetCurrentUserId()))
    .AddEnricher(e => e.WithMetadata("correlationId", GetCorrelationId()));
    
var enrichedError = enricher.Enrich(originalError);
```

## Best Practices

### 1. Use Semantic Error Types

```csharp
// Bad: Generic errors
return Result.Failure("User not found");
return new Error("ERROR", "Something went wrong");

// Good: Semantic errors
return new NotFoundError("User", userId);
return new ValidationError("Email is required");
```

### 2. Include Relevant Context

```csharp
// Bad: Minimal information
return new Error("PAYMENT_FAILED", "Payment failed");

// Good: Rich context
return new PaymentFailedError(
    paymentMethod: "CreditCard",
    failureReason: "Insufficient funds",
    transactionId: "TXN-12345"
).WithMetadata("amount", 99.99)
 .WithMetadata("currency", "USD")
 .WithMetadata("attemptedAt", DateTime.UtcNow);
```

### 3. Don't Lose Error Information

```csharp
// Bad: Losing original error
catch (Exception ex)
{
    return Result.Failure("Operation failed");
}

// Good: Preserve error details
catch (Exception ex)
{
    return Result.Failure(new Error("OPERATION_FAILED", ex.Message)
        .WithMetadata("exceptionType", ex.GetType().Name)
        .WithMetadata("stackTrace", ex.StackTrace));
}
```

### 4. Group Related Errors

```csharp
// Bad: Flat error structure
public class Error1 : Error { }
public class Error2 : Error { }
public class Error3 : Error { }

// Good: Hierarchical structure
public abstract class OrderError : Error { }
public class OrderValidationError : OrderError { }
public class OrderProcessingError : OrderError { }
public class OrderFulfillmentError : OrderError { }
```

### 5. Make Errors Actionable

```csharp
// Bad: Vague error
return new Error("INVALID_DATA", "Data is invalid");

// Good: Actionable error
return new ValidationError("Password must be at least 8 characters long")
    .WithMetadata("field", "password")
    .WithMetadata("minLength", 8)
    .WithMetadata("actualLength", password.Length)
    .WithMetadata("suggestion", "Add more characters to your password");
```

### 6. Consider Error Recovery

```csharp
public Result<Data> LoadData(string source)
{
    return LoadFromPrimary(source)
        .OrElse(error =>
        {
            _logger.LogWarning("Primary source failed: {Error}", error);
            return LoadFromSecondary(source);
        })
        .OrElse(error =>
        {
            _logger.LogError("All sources failed: {Error}", error);
            return LoadFromCache(source);
        })
        .MapError(error => new Error("DATA_LOAD_FAILED", 
            "Unable to load data from any source")
            .WithMetadata("originalError", error));
}
```

## Summary

Effective error handling in FluentUnions involves:

1. **Using semantic error types** that convey meaning
2. **Creating rich domain errors** with relevant context
3. **Composing errors** for complex validation scenarios
4. **Implementing recovery strategies** for resilient systems
5. **Following consistent patterns** across your codebase

By treating errors as values and leveraging the type system, you can build more reliable and maintainable applications.

Next steps:
- [Railway-Oriented Programming](railway-programming.md)
- [Real-World Examples](real-world-examples.md)
- [Testing Guide](../guides/testing-guide.md)
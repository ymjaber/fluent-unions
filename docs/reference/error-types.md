# Error Types Reference

This guide provides a comprehensive reference for all error types in FluentUnions and how to use them effectively.

## Table of Contents
1. [Error Base Class](#error-base-class)
2. [Built-in Error Types](#built-in-error-types)
3. [Creating Custom Errors](#creating-custom-errors)
4. [Error Metadata](#error-metadata)
5. [Error Composition](#error-composition)
6. [Error Serialization](#error-serialization)
7. [Error Patterns](#error-patterns)
8. [Best Practices](#best-practices)

## Error Base Class

### Core Error Type

```csharp
public class Error : IEquatable<Error>
{
    public string Code { get; }
    public string Message { get; }
    public IReadOnlyDictionary<string, object> Metadata { get; }
    
    public Error(string code, string message);
    public Error WithMetadata(string key, object value);
    public Error WithMetadata(IEnumerable<KeyValuePair<string, object>> metadata);
}
```

### Basic Usage

```csharp
// Create basic error
var error = new Error("USER_NOT_FOUND", "The specified user was not found");

// Add metadata
var detailedError = error
    .WithMetadata("userId", userId)
    .WithMetadata("timestamp", DateTime.UtcNow)
    .WithMetadata("attemptCount", 3);

// Access error properties
Console.WriteLine($"Code: {error.Code}");
Console.WriteLine($"Message: {error.Message}");

// Access metadata
if (error.Metadata.TryGetValue("userId", out var id))
{
    Console.WriteLine($"Failed for user: {id}");
}
```

## Built-in Error Types

### ValidationError

Used for input validation failures.

```csharp
public class ValidationError : Error
{
    public ValidationError(string message) 
        : base("VALIDATION_ERROR", message) { }
    
    public ValidationError(string field, string message)
        : base("VALIDATION_ERROR", message)
    {
        WithMetadata("field", field);
    }
}

// Usage examples
var emailError = new ValidationError("email", "Invalid email format");
var passwordError = new ValidationError("Password must be at least 8 characters");

// With multiple field errors
var fieldErrors = new[]
{
    new ValidationError("email", "Email is required"),
    new ValidationError("password", "Password is too short"),
    new ValidationError("age", "Must be 18 or older")
};
```

### NotFoundError

Used when a requested resource doesn't exist.

```csharp
public class NotFoundError : Error
{
    public NotFoundError(string message) 
        : base("NOT_FOUND", message) { }
    
    public NotFoundError(string resourceType, object resourceId)
        : base("NOT_FOUND", $"{resourceType} with ID '{resourceId}' was not found")
    {
        WithMetadata("resourceType", resourceType);
        WithMetadata("resourceId", resourceId);
    }
}

// Usage examples
var userNotFound = new NotFoundError("User", userId);
var orderNotFound = new NotFoundError("Order", orderId);
var customNotFound = new NotFoundError("The requested product is no longer available");
```

### ConflictError

Used for conflicts like duplicate entries or concurrent modifications.

```csharp
public class ConflictError : Error
{
    public ConflictError(string message) 
        : base("CONFLICT", message) { }
    
    public ConflictError(string resourceType, string conflictReason)
        : base("CONFLICT", $"{resourceType} conflict: {conflictReason}")
    {
        WithMetadata("resourceType", resourceType);
        WithMetadata("reason", conflictReason);
    }
}

// Usage examples
var duplicateEmail = new ConflictError("User", "Email already exists");
var versionConflict = new ConflictError("Document", "Version mismatch")
    .WithMetadata("currentVersion", 5)
    .WithMetadata("expectedVersion", 4);
```

### AuthenticationError

Used for authentication failures.

```csharp
public class AuthenticationError : Error
{
    public AuthenticationError(string message = "Authentication failed") 
        : base("AUTHENTICATION_ERROR", message) { }
    
    public AuthenticationError(string reason, bool showDetails)
        : base("AUTHENTICATION_ERROR", 
            showDetails ? $"Authentication failed: {reason}" : "Authentication failed")
    {
        WithMetadata("reason", reason);
    }
}

// Usage examples
var invalidCreds = new AuthenticationError("Invalid username or password", showDetails: false);
var expiredToken = new AuthenticationError("Token has expired", showDetails: true);
var accountLocked = new AuthenticationError()
    .WithMetadata("reason", "account_locked")
    .WithMetadata("lockDuration", TimeSpan.FromMinutes(30));
```

### AuthorizationError

Used for authorization/permission failures.

```csharp
public class AuthorizationError : Error
{
    public AuthorizationError(string message = "Insufficient permissions") 
        : base("AUTHORIZATION_ERROR", message) { }
    
    public AuthorizationError(string resource, string action)
        : base("AUTHORIZATION_ERROR", 
            $"You don't have permission to {action} {resource}")
    {
        WithMetadata("resource", resource);
        WithMetadata("action", action);
    }
}

// Usage examples
var noAccess = new AuthorizationError("Document", "read");
var adminOnly = new AuthorizationError("This action requires admin privileges");
var roleRequired = new AuthorizationError()
    .WithMetadata("requiredRole", "Manager")
    .WithMetadata("currentRole", "Employee");
```

### AggregateError

Used to combine multiple errors into one.

```csharp
public class AggregateError : Error
{
    public IReadOnlyList<Error> Errors { get; }
    
    public AggregateError(string message, IEnumerable<Error> errors)
        : base("AGGREGATE_ERROR", message)
    {
        Errors = errors.ToList();
    }
    
    public AggregateError(params Error[] errors)
        : this("Multiple errors occurred", errors) { }
}

// Usage examples
var validationErrors = new AggregateError(
    "Validation failed",
    new ValidationError("email", "Invalid format"),
    new ValidationError("password", "Too short"),
    new ValidationError("age", "Must be 18+")
);

// Collect errors
var errors = new List<Error>();
if (string.IsNullOrEmpty(input.Email))
    errors.Add(new ValidationError("email", "Email is required"));
if (input.Password.Length < 8)
    errors.Add(new ValidationError("password", "Password too short"));
if (errors.Any())
    return Result.Failure<User>(new AggregateError("Validation failed", errors));
```

## Creating Custom Errors

### Basic Custom Error

```csharp
public class BusinessRuleError : Error
{
    public BusinessRuleError(string rule, string violation)
        : base($"BUSINESS_RULE_{rule.ToUpper()}", violation)
    {
        WithMetadata("rule", rule);
    }
}

public class QuotaExceededError : Error
{
    public QuotaExceededError(string resource, int current, int limit)
        : base("QUOTA_EXCEEDED", 
            $"Quota exceeded for {resource}. Current: {current}, Limit: {limit}")
    {
        WithMetadata("resource", resource);
        WithMetadata("current", current);
        WithMetadata("limit", limit);
    }
}
```

### Domain-Specific Errors

```csharp
// E-commerce domain
public class OutOfStockError : Error
{
    public OutOfStockError(string productId, int requested, int available)
        : base("OUT_OF_STOCK", 
            $"Insufficient stock. Requested: {requested}, Available: {available}")
    {
        WithMetadata("productId", productId);
        WithMetadata("requested", requested);
        WithMetadata("available", available);
    }
}

public class PaymentError : Error
{
    public PaymentError(string reason, string transactionId = null)
        : base("PAYMENT_FAILED", $"Payment failed: {reason}")
    {
        WithMetadata("reason", reason);
        if (transactionId != null)
            WithMetadata("transactionId", transactionId);
    }
}

// Banking domain
public class InsufficientFundsError : Error
{
    public InsufficientFundsError(decimal requested, decimal available)
        : base("INSUFFICIENT_FUNDS", 
            $"Insufficient funds. Requested: {requested:C}, Available: {available:C}")
    {
        WithMetadata("requested", requested);
        WithMetadata("available", available);
    }
}
```

### Error with Context

```csharp
public class ContextualError : Error
{
    public ContextualError(Error innerError, string context)
        : base(innerError.Code, $"[{context}] {innerError.Message}")
    {
        // Copy metadata from inner error
        foreach (var kvp in innerError.Metadata)
        {
            WithMetadata(kvp.Key, kvp.Value);
        }
        
        WithMetadata("context", context);
        WithMetadata("innerError", innerError);
    }
}

// Usage
var innerError = new ValidationError("email", "Invalid format");
var contextual = new ContextualError(innerError, "UserRegistration");
```

## Error Metadata

### Adding Metadata

```csharp
// Single metadata item
var error = new Error("PROCESS_FAILED", "Process failed")
    .WithMetadata("processId", processId);

// Multiple metadata items
var detailed = error
    .WithMetadata("timestamp", DateTime.UtcNow)
    .WithMetadata("server", Environment.MachineName)
    .WithMetadata("user", currentUser.Id)
    .WithMetadata("stackTrace", GetStackTrace());

// Bulk metadata
var metadata = new Dictionary<string, object>
{
    ["orderId"] = orderId,
    ["customerId"] = customerId,
    ["items"] = orderItems.Count,
    ["total"] = orderTotal
};
var orderError = new Error("ORDER_FAILED", "Failed to process order")
    .WithMetadata(metadata);
```

### Structured Metadata

```csharp
public class ErrorContext
{
    public string RequestId { get; set; }
    public string UserId { get; set; }
    public DateTime Timestamp { get; set; }
    public Dictionary<string, string> Properties { get; set; }
}

var context = new ErrorContext
{
    RequestId = HttpContext.TraceIdentifier,
    UserId = currentUser.Id,
    Timestamp = DateTime.UtcNow,
    Properties = new Dictionary<string, string>
    {
        ["action"] = "CreateOrder",
        ["source"] = "API"
    }
};

var error = new Error("OPERATION_FAILED", "Operation failed")
    .WithMetadata("context", context);
```

## Error Composition

### Combining Errors

```csharp
public static class ErrorExtensions
{
    public static Error Combine(this Error error1, Error error2)
    {
        if (error1 is AggregateError agg1 && error2 is AggregateError agg2)
        {
            return new AggregateError(
                "Multiple errors occurred",
                agg1.Errors.Concat(agg2.Errors));
        }
        
        if (error1 is AggregateError agg)
        {
            return new AggregateError(
                agg.Message,
                agg.Errors.Append(error2));
        }
        
        if (error2 is AggregateError agg2Only)
        {
            return new AggregateError(
                agg2Only.Message,
                new[] { error1 }.Concat(agg2Only.Errors));
        }
        
        return new AggregateError(error1, error2);
    }
}
```

### Error Transformation

```csharp
public static class ErrorTransformations
{
    public static Error AddContext(this Error error, string context)
    {
        return new Error(
            error.Code,
            $"[{context}] {error.Message}")
            .WithMetadata(error.Metadata)
            .WithMetadata("context", context);
    }
    
    public static Error AsUserFriendly(this Error error)
    {
        return error switch
        {
            ValidationError => error, // Already user-friendly
            NotFoundError => error,
            AuthenticationError => new Error(
                error.Code, 
                "Please check your credentials and try again"),
            AuthorizationError => new Error(
                error.Code,
                "You don't have permission to perform this action"),
            _ => new Error("ERROR", "An error occurred. Please try again later")
                .WithMetadata("originalError", error)
        };
    }
}
```

## Error Serialization

### JSON Serialization

```csharp
// Configure JSON serialization
public class ErrorJsonConverter : JsonConverter<Error>
{
    public override void WriteJson(JsonWriter writer, Error value, JsonSerializer serializer)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("code");
        writer.WriteValue(value.Code);
        writer.WritePropertyName("message");
        writer.WriteValue(value.Message);
        
        if (value.Metadata?.Any() == true)
        {
            writer.WritePropertyName("metadata");
            serializer.Serialize(writer, value.Metadata);
        }
        
        // Handle specific error types
        if (value is AggregateError aggregate)
        {
            writer.WritePropertyName("errors");
            serializer.Serialize(writer, aggregate.Errors);
        }
        
        writer.WriteEndObject();
    }
    
    public override Error ReadJson(JsonReader reader, Type objectType, Error existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var jo = JObject.Load(reader);
        var code = jo["code"]?.Value<string>();
        var message = jo["message"]?.Value<string>();
        
        // Determine error type from code
        Error error = code switch
        {
            "VALIDATION_ERROR" => new ValidationError(message),
            "NOT_FOUND" => new NotFoundError(message),
            "CONFLICT" => new ConflictError(message),
            "AUTHENTICATION_ERROR" => new AuthenticationError(message),
            "AUTHORIZATION_ERROR" => new AuthorizationError(message),
            _ => new Error(code, message)
        };
        
        // Add metadata
        if (jo["metadata"] is JObject metadata)
        {
            foreach (var prop in metadata.Properties())
            {
                error = error.WithMetadata(prop.Name, prop.Value.ToObject<object>());
            }
        }
        
        return error;
    }
}
```

### XML Serialization

```csharp
[XmlRoot("Error")]
public class XmlError
{
    [XmlAttribute("code")]
    public string Code { get; set; }
    
    [XmlElement("Message")]
    public string Message { get; set; }
    
    [XmlArray("Metadata")]
    [XmlArrayItem("Item")]
    public List<XmlMetadataItem> Metadata { get; set; }
    
    public static XmlError FromError(Error error)
    {
        return new XmlError
        {
            Code = error.Code,
            Message = error.Message,
            Metadata = error.Metadata?.Select(kvp => new XmlMetadataItem
            {
                Key = kvp.Key,
                Value = kvp.Value?.ToString()
            }).ToList()
        };
    }
}
```

## Error Patterns

### Error Factory Pattern

```csharp
public interface IErrorFactory
{
    Error CreateValidationError(string field, string message);
    Error CreateNotFoundError(string resourceType, object id);
    Error CreateConflictError(string reason);
    Error CreateSystemError(Exception ex);
}

public class ErrorFactory : IErrorFactory
{
    private readonly ILogger _logger;
    private readonly bool _includeDetails;
    
    public ErrorFactory(ILogger logger, bool includeDetails = false)
    {
        _logger = logger;
        _includeDetails = includeDetails;
    }
    
    public Error CreateValidationError(string field, string message)
    {
        return new ValidationError(field, message)
            .WithMetadata("timestamp", DateTime.UtcNow);
    }
    
    public Error CreateSystemError(Exception ex)
    {
        _logger.LogError(ex, "System error occurred");
        
        if (_includeDetails)
        {
            return new Error("SYSTEM_ERROR", ex.Message)
                .WithMetadata("exceptionType", ex.GetType().Name)
                .WithMetadata("stackTrace", ex.StackTrace);
        }
        
        return new Error("SYSTEM_ERROR", "An unexpected error occurred");
    }
}
```

### Error Handler Pattern

```csharp
public interface IErrorHandler
{
    Task<IActionResult> HandleError(Error error, HttpContext context);
}

public class ApiErrorHandler : IErrorHandler
{
    private readonly ILogger<ApiErrorHandler> _logger;
    
    public Task<IActionResult> HandleError(Error error, HttpContext context)
    {
        _logger.LogError("API Error: {Code} - {Message}", error.Code, error.Message);
        
        var statusCode = GetStatusCode(error);
        var response = CreateErrorResponse(error, context);
        
        return Task.FromResult<IActionResult>(
            new ObjectResult(response) { StatusCode = statusCode });
    }
    
    private int GetStatusCode(Error error)
    {
        return error switch
        {
            ValidationError => 400,
            AuthenticationError => 401,
            AuthorizationError => 403,
            NotFoundError => 404,
            ConflictError => 409,
            _ => 500
        };
    }
}
```

### Error Recovery Pattern

```csharp
public static class ErrorRecovery
{
    public static Result<T> RecoverFrom<T>(
        this Result<T> result, 
        Func<Error, Result<T>> recovery)
    {
        return result.IsFailure ? recovery(result.Error) : result;
    }
    
    public static Result<T> RecoverFrom<T>(
        this Result<T> result,
        Error errorToRecover,
        Func<Result<T>> recovery)
    {
        if (result.IsFailure && result.Error.Code == errorToRecover.Code)
        {
            return recovery();
        }
        return result;
    }
    
    public static Result<T> RecoverFromType<T, TError>(
        this Result<T> result,
        Func<TError, Result<T>> recovery) where TError : Error
    {
        if (result.IsFailure && result.Error is TError typedError)
        {
            return recovery(typedError);
        }
        return result;
    }
}

// Usage
var result = GetUser(userId)
    .RecoverFromType<User, NotFoundError>(notFound => 
        Result.Success(User.Guest))
    .RecoverFrom("NETWORK_ERROR", () => 
        GetUserFromCache(userId));
```

## Best Practices

### 1. Use Specific Error Types

```csharp
// Good - specific error type with context
return new NotFoundError("User", userId);

// Less specific
return new Error("ERROR", "User not found");
```

### 2. Include Relevant Metadata

```csharp
// Good - includes context
return new ValidationError("password", "Password too short")
    .WithMetadata("minLength", 8)
    .WithMetadata("actualLength", password.Length);

// Missing context
return new ValidationError("Invalid password");
```

### 3. Create Domain-Specific Errors

```csharp
// Good - domain-specific error
public class InsufficientInventoryError : Error
{
    public InsufficientInventoryError(Product product, int requested)
        : base("INSUFFICIENT_INVENTORY", 
            $"Not enough {product.Name} in stock")
    {
        WithMetadata("productId", product.Id);
        WithMetadata("available", product.Stock);
        WithMetadata("requested", requested);
    }
}

// Too generic
return new Error("ERROR", "Not enough stock");
```

### 4. Consistent Error Codes

```csharp
// Good - consistent, parseable codes
public static class ErrorCodes
{
    public const string ValidationPrefix = "VAL_";
    public const string BusinessPrefix = "BUS_";
    public const string SystemPrefix = "SYS_";
    
    public const string EmailRequired = ValidationPrefix + "EMAIL_REQUIRED";
    public const string OrderLimitExceeded = BusinessPrefix + "ORDER_LIMIT";
    public const string DatabaseTimeout = SystemPrefix + "DB_TIMEOUT";
}
```

### 5. Error Documentation

```csharp
/// <summary>
/// Represents an error when user's account is locked
/// </summary>
/// <remarks>
/// Error code: AUTH_ACCOUNT_LOCKED
/// HTTP status: 403
/// Retry after: Check metadata "unlockTime"
/// </remarks>
public class AccountLockedError : Error
{
    public AccountLockedError(DateTime unlockTime)
        : base("AUTH_ACCOUNT_LOCKED", 
            $"Account is locked until {unlockTime:yyyy-MM-dd HH:mm}")
    {
        WithMetadata("unlockTime", unlockTime);
        WithMetadata("retryAfter", (unlockTime - DateTime.UtcNow).TotalSeconds);
    }
}
```

## Summary

FluentUnions provides a rich error type system that:
- Supports strongly-typed domain errors
- Includes metadata for additional context
- Enables error composition and aggregation
- Integrates with serialization frameworks
- Follows functional error handling patterns

Key principles:
- Use specific error types over generic ones
- Include relevant metadata for debugging
- Create domain-specific errors for your application
- Handle errors consistently across your application
- Document error codes and their meanings

Next steps:
- [Result API Reference](result-api.md)
- [Testing with Errors](../guides/testing-guide.md)
- [API Error Handling Patterns](../patterns/api-patterns.md)
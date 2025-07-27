# Validation Patterns

This guide demonstrates common validation patterns and best practices using FluentUnions.

## Table of Contents
1. [Introduction](#introduction)
2. [Basic Validation](#basic-validation)
3. [Field-Level Validation](#field-level-validation)
4. [Cross-Field Validation](#cross-field-validation)
5. [Validation Pipelines](#validation-pipelines)
6. [Async Validation](#async-validation)
7. [Validation Builders](#validation-builders)
8. [Domain Validation](#domain-validation)
9. [Validation with Metadata](#validation-with-metadata)
10. [Best Practices](#best-practices)

## Introduction

Validation is a critical aspect of any application. FluentUnions provides a functional approach to validation that:
- Makes validation errors explicit in the type system
- Allows composition of validation rules
- Provides rich error information
- Supports both synchronous and asynchronous validation

## Basic Validation

### Simple Field Validation

```csharp
public static class Validators
{
    public static Result<string> ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return new ValidationError("Email is required");
            
        if (!email.Contains('@'))
            return new ValidationError("Email must contain @ symbol");
            
        if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            return new ValidationError("Invalid email format");
            
        return Result.Success(email);
    }
    
    public static Result<string> ValidatePassword(string password)
    {
        if (string.IsNullOrEmpty(password))
            return new ValidationError("Password is required");
            
        if (password.Length < 8)
            return new ValidationError("Password must be at least 8 characters");
            
        if (!password.Any(char.IsDigit))
            return new ValidationError("Password must contain at least one digit");
            
        if (!password.Any(char.IsLetter))
            return new ValidationError("Password must contain at least one letter");
            
        return Result.Success(password);
    }
}
```

### Validation with Context

```csharp
public static Result<int> ValidateAge(int age, ValidationContext context)
{
    var minAge = context.Country switch
    {
        "US" => 21,
        "UK" => 18,
        "JP" => 20,
        _ => 18
    };
    
    if (age < minAge)
        return new ValidationError($"Must be at least {minAge} years old in {context.Country}")
            .WithMetadata("minAge", minAge)
            .WithMetadata("country", context.Country)
            .WithMetadata("actualAge", age);
            
    if (age > 150)
        return new ValidationError("Age cannot exceed 150 years");
        
    return Result.Success(age);
}
```

## Field-Level Validation

### Validation Rules

```csharp
public interface IValidationRule<T>
{
    Result Validate(T value);
}

public class RequiredRule<T> : IValidationRule<T>
{
    private readonly string _fieldName;
    
    public RequiredRule(string fieldName)
    {
        _fieldName = fieldName;
    }
    
    public Result Validate(T value)
    {
        if (value == null || (value is string str && string.IsNullOrWhiteSpace(str)))
            return new ValidationError($"{_fieldName} is required");
            
        return Result.Success();
    }
}

public class RangeRule<T> : IValidationRule<T> where T : IComparable<T>
{
    private readonly T _min;
    private readonly T _max;
    private readonly string _fieldName;
    
    public RangeRule(string fieldName, T min, T max)
    {
        _fieldName = fieldName;
        _min = min;
        _max = max;
    }
    
    public Result Validate(T value)
    {
        if (value.CompareTo(_min) < 0 || value.CompareTo(_max) > 0)
            return new ValidationError($"{_fieldName} must be between {_min} and {_max}")
                .WithMetadata("min", _min)
                .WithMetadata("max", _max)
                .WithMetadata("actual", value);
                
        return Result.Success();
    }
}
```

### Field Validator

```csharp
public class FieldValidator<T>
{
    private readonly string _fieldName;
    private readonly List<IValidationRule<T>> _rules = new();
    
    public FieldValidator(string fieldName)
    {
        _fieldName = fieldName;
    }
    
    public FieldValidator<T> Required()
    {
        _rules.Add(new RequiredRule<T>(_fieldName));
        return this;
    }
    
    public FieldValidator<T> Must(Func<T, bool> predicate, string errorMessage)
    {
        _rules.Add(new PredicateRule<T>(_fieldName, predicate, errorMessage));
        return this;
    }
    
    public FieldValidator<T> When(Func<T, bool> condition, Action<FieldValidator<T>> configure)
    {
        var conditionalValidator = new FieldValidator<T>(_fieldName);
        configure(conditionalValidator);
        _rules.Add(new ConditionalRule<T>(condition, conditionalValidator._rules));
        return this;
    }
    
    public Result Validate(T value)
    {
        var errors = _rules
            .Select(rule => rule.Validate(value))
            .Where(result => result.IsFailure)
            .Select(result => result.Error)
            .ToList();
            
        return errors.Any()
            ? Result.Failure(new AggregateError(errors))
            : Result.Success();
    }
}
```

## Cross-Field Validation

### Dependent Field Validation

```csharp
public class RegistrationValidator
{
    public Result<ValidatedRegistration> Validate(RegistrationRequest request)
    {
        // Individual field validation
        var emailResult = ValidateEmail(request.Email);
        var passwordResult = ValidatePassword(request.Password);
        var confirmPasswordResult = ValidateConfirmPassword(
            request.Password, 
            request.ConfirmPassword);
        
        // Collect all errors
        var errors = new List<Error>();
        if (emailResult.IsFailure) errors.Add(emailResult.Error);
        if (passwordResult.IsFailure) errors.Add(passwordResult.Error);
        if (confirmPasswordResult.IsFailure) errors.Add(confirmPasswordResult.Error);
        
        // Cross-field validation
        if (request.UsePhoneAuth && string.IsNullOrEmpty(request.PhoneNumber))
        {
            errors.Add(new ValidationError("Phone number is required when using phone authentication")
                .WithMetadata("field", "phoneNumber"));
        }
        
        if (request.BirthDate.HasValue && request.AcceptTerms)
        {
            var age = DateTime.Today.Year - request.BirthDate.Value.Year;
            if (age < 18)
            {
                errors.Add(new ValidationError("Must be 18 or older to accept terms")
                    .WithMetadata("field", "birthDate"));
            }
        }
        
        return errors.Any()
            ? Result.Failure<ValidatedRegistration>(new AggregateError(errors))
            : Result.Success(new ValidatedRegistration(request));
    }
    
    private Result ValidateConfirmPassword(string password, string confirmPassword)
    {
        if (string.IsNullOrEmpty(confirmPassword))
            return new ValidationError("Confirm password is required");
            
        if (password != confirmPassword)
            return new ValidationError("Passwords do not match");
            
        return Result.Success();
    }
}
```

### Conditional Validation

```csharp
public class OrderValidator
{
    public Result<ValidatedOrder> Validate(OrderRequest request)
    {
        return ValidateBasicInfo(request)
            .Bind(ValidateShipping)
            .Bind(ValidatePayment)
            .Bind(ValidateItems)
            .Map(ToValidatedOrder);
    }
    
    private Result<OrderRequest> ValidateShipping(OrderRequest request)
    {
        if (request.ShippingMethod == ShippingMethod.Express)
        {
            // Express shipping has additional requirements
            if (request.ShippingAddress.Country != "US")
                return new ValidationError("Express shipping only available in US");
                
            if (request.Items.Sum(i => i.Weight) > 50)
                return new ValidationError("Express shipping not available for orders over 50 lbs");
        }
        
        if (request.ShippingMethod == ShippingMethod.International)
        {
            if (string.IsNullOrEmpty(request.ShippingAddress.PostalCode))
                return new ValidationError("Postal code required for international shipping");
                
            if (string.IsNullOrEmpty(request.CustomsInfo?.Description))
                return new ValidationError("Customs description required for international shipping");
        }
        
        return Result.Success(request);
    }
}
```

## Validation Pipelines

### Sequential Validation Pipeline

```csharp
public class ValidationPipeline<T>
{
    private readonly List<Func<T, Result<T>>> _validators = new();
    
    public ValidationPipeline<T> Add(Func<T, Result> validator)
    {
        _validators.Add(value => validator(value).Map(() => value));
        return this;
    }
    
    public ValidationPipeline<T> AddTransform(Func<T, Result<T>> validator)
    {
        _validators.Add(validator);
        return this;
    }
    
    public Result<T> Validate(T input)
    {
        return _validators.Aggregate(
            Result.Success(input),
            (current, validator) => current.Bind(validator)
        );
    }
}

// Usage
var pipeline = new ValidationPipeline<UserProfile>()
    .Add(profile => ValidateName(profile.Name))
    .Add(profile => ValidateEmail(profile.Email))
    .AddTransform(profile => NormalizeProfile(profile))
    .Add(profile => ValidateAge(profile.Age));

var result = pipeline.Validate(userProfile);
```

### Parallel Validation Pipeline

```csharp
public class ParallelValidationPipeline<T>
{
    private readonly List<Func<T, Result>> _validators = new();
    
    public ParallelValidationPipeline<T> Add(Func<T, Result> validator)
    {
        _validators.Add(validator);
        return this;
    }
    
    public Result<T> Validate(T input)
    {
        var results = _validators
            .Select(validator => validator(input))
            .ToList();
            
        var errors = results
            .Where(r => r.IsFailure)
            .Select(r => r.Error)
            .ToList();
            
        return errors.Any()
            ? Result.Failure<T>(new AggregateError(errors))
            : Result.Success(input);
    }
}
```

### Validation with Early Exit

```csharp
public class EarlyExitValidator<T>
{
    private readonly List<(Func<T, Result> validator, bool critical)> _validators = new();
    
    public EarlyExitValidator<T> AddCritical(Func<T, Result> validator)
    {
        _validators.Add((validator, true));
        return this;
    }
    
    public EarlyExitValidator<T> AddOptional(Func<T, Result> validator)
    {
        _validators.Add((validator, false));
        return this;
    }
    
    public Result<T> Validate(T input)
    {
        var errors = new List<Error>();
        
        foreach (var (validator, critical) in _validators)
        {
            var result = validator(input);
            if (result.IsFailure)
            {
                errors.Add(result.Error);
                if (critical)
                {
                    // Stop validation on critical error
                    return Result.Failure<T>(new AggregateError(errors));
                }
            }
        }
        
        return errors.Any()
            ? Result.Failure<T>(new AggregateError(errors))
            : Result.Success(input);
    }
}
```

## Async Validation

### Async Validators

```csharp
public class AsyncUserValidator
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailVerificationService _emailService;
    
    public async Task<Result<ValidatedUser>> ValidateAsync(UserRegistration registration)
    {
        // Parallel async validation
        var tasks = new[]
        {
            ValidateEmailUniquenessAsync(registration.Email),
            ValidateEmailDeliverabilityAsync(registration.Email),
            ValidateUsernameAsync(registration.Username)
        };
        
        var results = await Task.WhenAll(tasks);
        var errors = results.Where(r => r.IsFailure).Select(r => r.Error).ToList();
        
        if (errors.Any())
            return Result.Failure<ValidatedUser>(new AggregateError(errors));
            
        // Sequential validation that depends on previous results
        return await ValidatePasswordStrengthAsync(registration.Password)
            .BindAsync(async () => await ValidatePhoneNumberAsync(registration.PhoneNumber))
            .MapAsync(async () => new ValidatedUser(registration));
    }
    
    private async Task<Result> ValidateEmailUniquenessAsync(string email)
    {
        var exists = await _userRepository.EmailExistsAsync(email);
        return exists
            ? new ValidationError("Email already registered")
                .WithMetadata("field", "email")
            : Result.Success();
    }
    
    private async Task<Result> ValidateEmailDeliverabilityAsync(string email)
    {
        var deliverable = await _emailService.IsDeliverableAsync(email);
        return deliverable
            ? Result.Success()
            : new ValidationError("Email address is not deliverable")
                .WithMetadata("field", "email");
    }
}
```

### Async Validation with Timeout

```csharp
public class TimeoutValidator
{
    public async Task<Result<T>> ValidateWithTimeoutAsync<T>(
        T input,
        Func<T, Task<Result<T>>> validator,
        TimeSpan timeout)
    {
        using var cts = new CancellationTokenSource(timeout);
        
        try
        {
            var validationTask = validator(input);
            var completedTask = await Task.WhenAny(
                validationTask,
                Task.Delay(timeout, cts.Token)
            );
            
            if (completedTask == validationTask)
            {
                return await validationTask;
            }
            
            return Result.Failure<T>(new Error(
                "VALIDATION_TIMEOUT",
                $"Validation timed out after {timeout.TotalSeconds} seconds"
            ));
        }
        catch (OperationCanceledException)
        {
            return Result.Failure<T>(new Error(
                "VALIDATION_CANCELLED",
                "Validation was cancelled"
            ));
        }
    }
}
```

## Validation Builders

### Fluent Validation Builder

```csharp
public class FluentValidator<T>
{
    private readonly List<IValidationRule<T>> _rules = new();
    private readonly string _name;
    
    public FluentValidator(string name = null)
    {
        _name = name ?? typeof(T).Name;
    }
    
    public FluentValidator<T> RuleFor<TProperty>(
        Expression<Func<T, TProperty>> propertyExpression,
        Action<PropertyValidator<T, TProperty>> configure)
    {
        var propertyValidator = new PropertyValidator<T, TProperty>(propertyExpression);
        configure(propertyValidator);
        _rules.Add(propertyValidator);
        return this;
    }
    
    public FluentValidator<T> Must(Func<T, bool> predicate, string errorMessage)
    {
        _rules.Add(new PredicateRule<T>(_name, predicate, errorMessage));
        return this;
    }
    
    public FluentValidator<T> MustAsync(
        Func<T, Task<bool>> predicate, 
        string errorMessage)
    {
        _rules.Add(new AsyncPredicateRule<T>(_name, predicate, errorMessage));
        return this;
    }
    
    public async Task<Result<T>> ValidateAsync(T instance)
    {
        var errors = new List<Error>();
        
        foreach (var rule in _rules)
        {
            var result = rule is IAsyncValidationRule<T> asyncRule
                ? await asyncRule.ValidateAsync(instance)
                : rule.Validate(instance);
                
            if (result.IsFailure)
                errors.Add(result.Error);
        }
        
        return errors.Any()
            ? Result.Failure<T>(new AggregateError(errors))
            : Result.Success(instance);
    }
}

// Usage
public class CustomerValidator : FluentValidator<Customer>
{
    public CustomerValidator()
    {
        RuleFor(c => c.Name, name => name
            .NotEmpty("Name is required")
            .MinLength(2, "Name must be at least 2 characters")
            .MaxLength(100, "Name cannot exceed 100 characters"));
            
        RuleFor(c => c.Email, email => email
            .NotEmpty("Email is required")
            .EmailAddress("Invalid email format")
            .MustAsync(async e => await IsEmailUnique(e), "Email already exists"));
            
        RuleFor(c => c.DateOfBirth, dob => dob
            .NotNull("Date of birth is required")
            .LessThan(DateTime.Today, "Date of birth must be in the past")
            .Must(d => CalculateAge(d) >= 18, "Must be at least 18 years old"));
    }
}
```

### Property Validator

```csharp
public class PropertyValidator<T, TProperty>
{
    private readonly Expression<Func<T, TProperty>> _propertyExpression;
    private readonly string _propertyName;
    private readonly List<IValidationRule<TProperty>> _rules = new();
    
    public PropertyValidator(Expression<Func<T, TProperty>> propertyExpression)
    {
        _propertyExpression = propertyExpression;
        _propertyName = GetPropertyName(propertyExpression);
    }
    
    public PropertyValidator<T, TProperty> NotNull(string errorMessage = null)
    {
        _rules.Add(new NotNullRule<TProperty>(
            _propertyName, 
            errorMessage ?? $"{_propertyName} is required"));
        return this;
    }
    
    public PropertyValidator<T, TProperty> NotEmpty(string errorMessage = null)
    {
        _rules.Add(new NotEmptyRule<TProperty>(
            _propertyName, 
            errorMessage ?? $"{_propertyName} cannot be empty"));
        return this;
    }
    
    public PropertyValidator<T, TProperty> Must(
        Func<TProperty, bool> predicate, 
        string errorMessage)
    {
        _rules.Add(new PredicateRule<TProperty>(
            _propertyName, 
            predicate, 
            errorMessage));
        return this;
    }
    
    public Result Validate(T instance)
    {
        var propertyValue = _propertyExpression.Compile()(instance);
        
        var errors = _rules
            .Select(rule => rule.Validate(propertyValue))
            .Where(result => result.IsFailure)
            .Select(result => result.Error.WithMetadata("property", _propertyName))
            .ToList();
            
        return errors.Any()
            ? Result.Failure(new AggregateError(errors))
            : Result.Success();
    }
}
```

## Domain Validation

### Value Object Validation

```csharp
public class Email
{
    private readonly string _value;
    
    private Email(string value)
    {
        _value = value;
    }
    
    public static Result<Email> Create(string value)
    {
        return ValidateEmail(value)
            .Map(email => new Email(email));
    }
    
    private static Result<string> ValidateEmail(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return new ValidationError("Email cannot be empty");
            
        var trimmed = value.Trim().ToLowerInvariant();
        
        if (!Regex.IsMatch(trimmed, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            return new ValidationError("Invalid email format");
            
        if (trimmed.Length > 254)
            return new ValidationError("Email too long");
            
        var parts = trimmed.Split('@');
        if (parts[0].Length > 64)
            return new ValidationError("Local part too long");
            
        return Result.Success(trimmed);
    }
    
    public static implicit operator string(Email email) => email._value;
    public override string ToString() => _value;
}
```

### Aggregate Validation

```csharp
public class Order
{
    private readonly List<OrderItem> _items = new();
    
    public IReadOnlyList<OrderItem> Items => _items;
    public OrderStatus Status { get; private set; }
    public decimal Total => _items.Sum(i => i.Total);
    
    public Result AddItem(Product product, int quantity)
    {
        return ValidateCanAddItem(product, quantity)
            .Tap(() => _items.Add(new OrderItem(product, quantity)));
    }
    
    private Result ValidateCanAddItem(Product product, int quantity)
    {
        if (Status != OrderStatus.Draft)
            return new ValidationError("Cannot add items to non-draft order");
            
        if (!product.IsAvailable)
            return new ValidationError($"Product {product.Name} is not available");
            
        if (quantity <= 0)
            return new ValidationError("Quantity must be positive");
            
        if (quantity > product.StockLevel)
            return new ValidationError($"Insufficient stock. Available: {product.StockLevel}");
            
        var existingItem = _items.FirstOrDefault(i => i.ProductId == product.Id);
        if (existingItem != null)
        {
            var totalQuantity = existingItem.Quantity + quantity;
            if (totalQuantity > product.MaxPerOrder)
                return new ValidationError($"Maximum {product.MaxPerOrder} per order");
        }
        
        return Result.Success();
    }
    
    public Result Submit()
    {
        return ValidateCanSubmit()
            .Tap(() => Status = OrderStatus.Submitted);
    }
    
    private Result ValidateCanSubmit()
    {
        if (Status != OrderStatus.Draft)
            return new ValidationError("Order already submitted");
            
        if (!_items.Any())
            return new ValidationError("Order must contain at least one item");
            
        if (Total < 10)
            return new ValidationError("Minimum order value is $10");
            
        var outOfStockItems = _items
            .Where(i => !i.Product.IsAvailable)
            .Select(i => i.Product.Name)
            .ToList();
            
        if (outOfStockItems.Any())
            return new ValidationError($"Items out of stock: {string.Join(", ", outOfStockItems)}");
            
        return Result.Success();
    }
}
```

### Business Rule Validation

```csharp
public interface IBusinessRule
{
    Result Validate();
}

public class CustomerCanPlaceOrderRule : IBusinessRule
{
    private readonly Customer _customer;
    private readonly decimal _orderAmount;
    
    public CustomerCanPlaceOrderRule(Customer customer, decimal orderAmount)
    {
        _customer = customer;
        _orderAmount = orderAmount;
    }
    
    public Result Validate()
    {
        if (!_customer.IsActive)
            return new BusinessRuleError("INACTIVE_CUSTOMER", "Customer account is inactive");
            
        if (_customer.CreditLimit.HasValue && _orderAmount > _customer.CreditLimit.Value)
            return new BusinessRuleError("CREDIT_LIMIT_EXCEEDED", 
                $"Order amount ${_orderAmount} exceeds credit limit ${_customer.CreditLimit}");
                
        if (_customer.HasUnpaidInvoices)
            return new BusinessRuleError("UNPAID_INVOICES", 
                "Customer has unpaid invoices");
                
        return Result.Success();
    }
}

public class OrderService
{
    public Result<Order> PlaceOrder(Customer customer, OrderRequest request)
    {
        var orderAmount = request.Items.Sum(i => i.Price * i.Quantity);
        
        return new CustomerCanPlaceOrderRule(customer, orderAmount)
            .Validate()
            .Bind(() => CreateOrder(customer, request));
    }
}
```

## Validation with Metadata

### Rich Validation Errors

```csharp
public class RichValidationError : ValidationError
{
    public string FieldName { get; }
    public object AttemptedValue { get; }
    public ValidationSeverity Severity { get; }
    
    public RichValidationError(
        string fieldName,
        object attemptedValue,
        string message,
        ValidationSeverity severity = ValidationSeverity.Error)
        : base(message)
    {
        FieldName = fieldName;
        AttemptedValue = attemptedValue;
        Severity = severity;
        
        this.WithMetadata("field", fieldName)
            .WithMetadata("attemptedValue", attemptedValue)
            .WithMetadata("severity", severity.ToString());
    }
}

public enum ValidationSeverity
{
    Warning,
    Error,
    Critical
}
```

### Validation Result with Warnings

```csharp
public class ValidationResult<T>
{
    public T Value { get; }
    public List<ValidationError> Errors { get; }
    public List<ValidationError> Warnings { get; }
    
    public bool IsValid => !Errors.Any();
    public bool HasWarnings => Warnings.Any();
    
    public Result<T> ToResult()
    {
        return IsValid
            ? Result.Success(Value)
            : Result.Failure<T>(new AggregateError(Errors));
    }
}

public class AdvancedValidator<T>
{
    public ValidationResult<T> Validate(T input)
    {
        var errors = new List<ValidationError>();
        var warnings = new List<ValidationError>();
        
        // Validation that produces errors
        if (!IsValid(input))
        {
            errors.Add(new RichValidationError(
                "field",
                input,
                "Validation failed",
                ValidationSeverity.Error));
        }
        
        // Validation that produces warnings
        if (IsSuboptimal(input))
        {
            warnings.Add(new RichValidationError(
                "field",
                input,
                "Consider improving this value",
                ValidationSeverity.Warning));
        }
        
        return new ValidationResult<T>
        {
            Value = input,
            Errors = errors,
            Warnings = warnings
        };
    }
}
```

### Localized Validation Messages

```csharp
public class LocalizedValidator<T>
{
    private readonly IStringLocalizer _localizer;
    
    public LocalizedValidator(IStringLocalizer localizer)
    {
        _localizer = localizer;
    }
    
    public Result Validate(T input, string culture = null)
    {
        using var cultureScope = culture != null
            ? new CultureScope(culture)
            : null;
            
        return PerformValidation(input);
    }
    
    private Result PerformValidation(T input)
    {
        // Use localized messages
        var errorMessage = _localizer["validation.required", "Email"];
        return new ValidationError(errorMessage)
            .WithMetadata("messageKey", "validation.required")
            .WithMetadata("culture", CultureInfo.CurrentCulture.Name);
    }
}
```

## Best Practices

### 1. Make Invalid States Unrepresentable

```csharp
// Instead of validating after construction
public class BadOrder
{
    public List<OrderItem> Items { get; set; }
    public decimal Total { get; set; }
    
    public Result Validate()
    {
        // Validation after the fact
    }
}

// Validate during construction
public class GoodOrder
{
    private readonly List<OrderItem> _items;
    
    private GoodOrder(List<OrderItem> items)
    {
        _items = items;
    }
    
    public static Result<GoodOrder> Create(List<OrderItem> items)
    {
        return ValidateItems(items)
            .Map(validItems => new GoodOrder(validItems));
    }
}
```

### 2. Compose Small Validators

```csharp
// Composable validators
public static Result<string> NotEmpty(string value, string fieldName)
{
    return string.IsNullOrWhiteSpace(value)
        ? new ValidationError($"{fieldName} cannot be empty")
        : Result.Success(value);
}

public static Result<string> MaxLength(string value, int max, string fieldName)
{
    return value.Length > max
        ? new ValidationError($"{fieldName} cannot exceed {max} characters")
        : Result.Success(value);
}

// Compose them
public static Result<string> ValidateName(string name)
{
    return NotEmpty(name, "Name")
        .Bind(n => MaxLength(n, 100, "Name"))
        .Bind(n => NoSpecialCharacters(n, "Name"));
}
```

### 3. Provide Rich Error Information

```csharp
// Poor error
return new ValidationError("Invalid input");

// Rich error
return new ValidationError("Age must be between 18 and 65")
    .WithMetadata("field", "age")
    .WithMetadata("min", 18)
    .WithMetadata("max", 65)
    .WithMetadata("actual", age)
    .WithMetadata("hint", "Please enter a valid age");
```

### 4. Fail Fast for Critical Validations

```csharp
public Result<Order> ValidateOrder(OrderRequest request)
{
    // Critical validations first
    var authResult = ValidateAuthentication();
    if (authResult.IsFailure) return authResult.Error;
    
    var permissionResult = ValidatePermissions();
    if (permissionResult.IsFailure) return permissionResult.Error;
    
    // Then business validations
    return ValidateBusinessRules(request)
        .Bind(ValidateInventory)
        .Bind(ValidatePricing);
}
```

### 5. Use Async Validation Appropriately

```csharp
public class SmartValidator
{
    public async Task<Result> ValidateAsync(UserInput input)
    {
        // Do synchronous validation first
        var basicValidation = ValidateBasicRules(input);
        if (basicValidation.IsFailure)
            return basicValidation;
        
        // Only do expensive async validation if basic validation passes
        return await ValidateAgainstDatabaseAsync(input);
    }
}
```

## Summary

Validation patterns with FluentUnions provide:

1. **Type-safe validation** - Errors are explicit in the return type
2. **Composable validators** - Build complex validation from simple rules
3. **Rich error information** - Include context and metadata in errors
4. **Flexible patterns** - Support sync, async, pipeline, and builder patterns
5. **Domain modeling** - Validate at the right level of abstraction

Key principles:
- Validate early and at boundaries
- Provide actionable error messages
- Make invalid states unrepresentable
- Compose small, reusable validators
- Include relevant context in errors

Next steps:
- [Repository Patterns](repository-patterns.md)
- [Service Patterns](service-patterns.md)
- [API Patterns](api-patterns.md)
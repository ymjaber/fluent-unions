# Railway-Oriented Programming

Railway-Oriented Programming (ROP) is a functional programming pattern that elegantly handles success and failure paths in your code. This tutorial shows how to implement ROP using FluentUnions.

## Table of Contents
1. [Introduction to Railway-Oriented Programming](#introduction)
2. [The Railway Metaphor](#the-railway-metaphor)
3. [Basic Railway Operations](#basic-railway-operations)
4. [Building Complex Railways](#building-complex-railways)
5. [Switching Tracks](#switching-tracks)
6. [Parallel Railways](#parallel-railways)
7. [Real-World Examples](#real-world-examples)
8. [Advanced Patterns](#advanced-patterns)
9. [Best Practices](#best-practices)

## Introduction

Railway-Oriented Programming treats your program flow as a railway with two tracks:
- **Success Track**: The happy path where everything works
- **Failure Track**: The error path when something goes wrong

Once on the failure track, subsequent operations are bypassed until explicitly handled.

### Traditional vs Railway-Oriented

```csharp
// Traditional imperative approach
public OrderResult ProcessOrder(OrderRequest request)
{
    // Validate request
    var validationResult = ValidateRequest(request);
    if (!validationResult.IsValid)
        return new OrderResult { Error = validationResult.Error };
    
    // Check inventory
    var inventoryResult = CheckInventory(request.Items);
    if (!inventoryResult.IsAvailable)
        return new OrderResult { Error = "Out of stock" };
    
    // Process payment
    var paymentResult = ProcessPayment(request.Payment);
    if (!paymentResult.IsSuccessful)
        return new OrderResult { Error = paymentResult.Error };
    
    // Create order
    var order = CreateOrder(request);
    return new OrderResult { Order = order };
}

// Railway-oriented approach
public Result<Order> ProcessOrder(OrderRequest request)
{
    return ValidateRequest(request)
        .Bind(validRequest => CheckInventory(validRequest.Items))
        .Bind(inventory => ProcessPayment(request.Payment))
        .Bind(payment => CreateOrder(request, payment));
}
```

## The Railway Metaphor

Think of your code as a railway system:

```
Success Track:  ═══════════════════════════════════>
                     ║      ║      ║      ║
                  Validate  Check  Process Create
                  Request   Stock  Payment Order
                     ║      ║      ║      ║
Failure Track:  ═══════════════════════════════════>
```

Each operation can:
- Continue on the success track
- Switch to the failure track
- Once on failure track, stay there

## Basic Railway Operations

### Starting the Railway

```csharp
// Start with a value
Result<string> railway = Result.Success("initial value");

// Start with validation
Result<User> userRailway = ValidateUserInput(input);

// Start with a Try
Result<Config> configRailway = Result.Try(() => LoadConfig());
```

### Adding Stations (Bind)

Each station transforms the value or switches to failure track:

```csharp
public Result<Invoice> GenerateInvoice(Guid orderId)
{
    return GetOrder(orderId)              // Result<Order>
        .Bind(order => GetCustomer(order.CustomerId))  // Result<Customer>  
        .Bind(customer => CalculateTotals(orderId))    // Result<OrderTotals>
        .Bind(totals => CreateInvoice(orderId, totals)); // Result<Invoice>
}

// Each function returns Result<T>
private Result<Order> GetOrder(Guid orderId)
{
    var order = _repository.FindOrder(orderId);
    return order != null 
        ? Result.Success(order)
        : new NotFoundError($"Order {orderId} not found");
}
```

### Side Tracks (Tap)

Execute side effects without changing the main value:

```csharp
public Result<Order> ProcessOrder(OrderRequest request)
{
    return ValidateRequest(request)
        .Tap(() => _logger.LogInfo("Request validated"))
        .Bind(CreateOrder)
        .Tap(order => _logger.LogInfo($"Order {order.Id} created"))
        .Bind(SaveOrder)
        .Tap(order => _analytics.TrackOrderCreated(order))
        .TapError(error => _logger.LogError($"Order processing failed: {error}"));
}
```

### Transforming Values (Map)

Transform success values without changing tracks:

```csharp
public Result<OrderSummary> GetOrderSummary(Guid orderId)
{
    return GetOrder(orderId)
        .Map(order => new OrderSummary
        {
            Id = order.Id,
            Total = order.Items.Sum(i => i.Price * i.Quantity),
            ItemCount = order.Items.Count,
            Status = order.Status
        });
}
```

## Building Complex Railways

### Sequential Operations

Chain operations that depend on previous results:

```csharp
public class UserRegistrationService
{
    public Result<User> RegisterUser(RegistrationRequest request)
    {
        return ValidateRequest(request)
            .Bind(CheckEmailAvailability)
            .Bind(CreateUserAccount)
            .Bind(SendVerificationEmail)
            .Bind(CreateInitialProfile)
            .Bind(AssignDefaultPermissions)
            .Tap(LogSuccessfulRegistration);
    }
    
    private Result<ValidatedRequest> ValidateRequest(RegistrationRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email))
            return new ValidationError("Email is required");
            
        if (request.Password.Length < 8)
            return new ValidationError("Password must be at least 8 characters");
            
        return Result.Success(new ValidatedRequest(request));
    }
    
    private Result<ValidatedRequest> CheckEmailAvailability(ValidatedRequest request)
    {
        return _userRepository.EmailExists(request.Email)
            ? new ConflictError($"Email {request.Email} is already registered")
            : Result.Success(request);
    }
    
    private Result<User> CreateUserAccount(ValidatedRequest request)
    {
        return Result.Try(() =>
        {
            var user = new User
            {
                Email = request.Email,
                PasswordHash = _passwordHasher.Hash(request.Password),
                CreatedAt = DateTime.UtcNow
            };
            _userRepository.Add(user);
            return user;
        })
        .MapError(ex => new Error("USER_CREATION_FAILED", ex.Message));
    }
}
```

### Branching Railways

Different paths based on conditions:

```csharp
public Result<ProcessedOrder> ProcessOrder(Order order)
{
    return DetermineOrderType(order)
        .Bind(orderType => orderType switch
        {
            OrderType.Standard => ProcessStandardOrder(order),
            OrderType.Express => ProcessExpressOrder(order),
            OrderType.International => ProcessInternationalOrder(order),
            _ => Result.Failure<ProcessedOrder>("Unknown order type")
        });
}

private Result<ProcessedOrder> ProcessStandardOrder(Order order)
{
    return ValidateStandardOrder(order)
        .Bind(CalculateStandardShipping)
        .Bind(AssignToWarehouse)
        .Bind(ScheduleDelivery);
}

private Result<ProcessedOrder> ProcessExpressOrder(Order order)
{
    return ValidateExpressOrder(order)
        .Bind(CalculateExpressShipping)
        .Bind(PrioritizeInWarehouse)
        .Bind(ScheduleExpressDelivery);
}
```

### Conditional Railways

Add operations based on conditions:

```csharp
public Result<User> UpdateUser(UpdateUserRequest request)
{
    return GetUser(request.UserId)
        .Bind(user => UpdateBasicInfo(user, request))
        .Bind(user => request.ChangePassword 
            ? ChangePassword(user, request.NewPassword)
            : Result.Success(user))
        .Bind(user => request.UpdateProfile 
            ? UpdateProfile(user, request.Profile)
            : Result.Success(user))
        .Bind(SaveUser);
}
```

## Switching Tracks

### Error Recovery

Switch back to success track:

```csharp
public Result<Config> LoadConfiguration()
{
    return LoadFromFile("app.config")
        .OrElse(error =>
        {
            _logger.LogWarning($"Failed to load from file: {error.Message}");
            return LoadFromEnvironment();
        })
        .OrElse(error =>
        {
            _logger.LogWarning($"Failed to load from environment: {error.Message}");
            return LoadDefaultConfiguration();
        });
}
```

### Compensating Actions

Handle failures with compensating actions:

```csharp
public Result<Order> PlaceOrder(OrderRequest request)
{
    var reservationIds = new List<Guid>();
    
    return ValidateOrder(request)
        .Bind(order => ReserveInventory(order.Items)
            .Tap(ids => reservationIds.AddRange(ids))
            .Map(_ => order))
        .Bind(order => ProcessPayment(order.Payment)
            .MapError(error =>
            {
                // Compensate by releasing reservations
                ReleaseReservations(reservationIds);
                return error;
            })
            .Map(_ => order))
        .Bind(CreateOrder);
}
```

### Ensure Operations

Add validation checkpoints:

```csharp
public Result<PublishedArticle> PublishArticle(Article article)
{
    return ValidateArticle(article)
        .Ensure(a => a.WordCount >= 500, 
                "Article must be at least 500 words")
        .Ensure(a => a.Images.Any(), 
                "Article must have at least one image")
        .Ensure(a => a.Categories.Any(), 
                "Article must be categorized")
        .Bind(CheckPlagiarism)
        .Bind(OptimizeSEO)
        .Bind(PublishToSite);
}
```

## Parallel Railways

### Combining Multiple Railways

Run operations in parallel and combine results:

```csharp
public Result<OrderSummary> GetOrderSummary(Guid orderId)
{
    // All must succeed
    return Result.Combine(
        GetOrder(orderId),
        GetCustomer(orderId),
        GetShippingInfo(orderId),
        GetPaymentInfo(orderId)
    ).Map(results => new OrderSummary(
        results[0], // Order
        results[1], // Customer
        results[2], // ShippingInfo
        results[3]  // PaymentInfo
    ));
}

// With different types
public Result<Dashboard> BuildDashboard(Guid userId)
{
    return Result.BindAll(
        GetUserStats(userId),      // Result<UserStats>
        GetRecentOrders(userId),   // Result<List<Order>>
        GetNotifications(userId),  // Result<List<Notification>>
        GetRecommendations(userId) // Result<List<Product>>
    ).Map((stats, orders, notifications, recommendations) => 
        new Dashboard
        {
            Stats = stats,
            RecentOrders = orders,
            Notifications = notifications,
            Recommendations = recommendations
        });
}
```

### First Success

Get first successful result:

```csharp
public Result<PaymentResult> ProcessPayment(PaymentRequest request)
{
    return ProcessCreditCard(request)
        .OrElse(() => ProcessDebitCard(request))
        .OrElse(() => ProcessPayPal(request))
        .OrElse(() => ProcessBankTransfer(request))
        .MapError(_ => new Error("PAYMENT_FAILED", 
            "All payment methods failed"));
}
```

## Real-World Examples

### E-Commerce Order Processing

```csharp
public class OrderProcessingService
{
    public Result<OrderConfirmation> ProcessOrder(OrderRequest request)
    {
        return ValidateCustomer(request.CustomerId)
            .Bind(customer => ValidateShippingAddress(customer, request.ShippingAddress))
            .Bind(address => ValidateItems(request.Items))
            .Bind(items => CalculatePricing(items, request.DiscountCodes))
            .Bind(pricing => ReserveInventory(request.Items))
            .Bind(reservations => ProcessPayment(request.Payment, pricing))
            .Bind(payment => CreateOrder(request, payment, reservations))
            .Bind(order => ScheduleShipment(order))
            .Bind(shipment => SendConfirmationEmail(order, shipment))
            .Map(ToOrderConfirmation);
    }
    
    private Result<OrderConfirmation> ToOrderConfirmation((Order order, Shipment shipment, EmailResult email) data)
    {
        return new OrderConfirmation
        {
            OrderId = data.order.Id,
            TrackingNumber = data.shipment.TrackingNumber,
            EstimatedDelivery = data.shipment.EstimatedDelivery,
            EmailSent = data.email.Success
        };
    }
}
```

### User Authentication Flow

```csharp
public class AuthenticationService
{
    public Result<AuthenticationResult> Authenticate(LoginRequest request)
    {
        return ValidateLoginRequest(request)
            .Bind(FindUserByEmail)
            .Bind(user => VerifyPassword(user, request.Password))
            .Bind(CheckAccountStatus)
            .Bind(CheckTwoFactorAuth)
            .Bind(GenerateTokens)
            .Bind(LogSuccessfulLogin)
            .Tap(result => _eventBus.Publish(new UserLoggedInEvent(result.UserId)));
    }
    
    private Result<User> CheckAccountStatus(User user)
    {
        if (!user.IsActive)
            return new AuthenticationError("Account is deactivated");
            
        if (user.IsLocked)
            return new AuthenticationError("Account is locked");
            
        if (!user.EmailVerified)
            return new AuthenticationError("Email not verified");
            
        return Result.Success(user);
    }
    
    private Result<User> CheckTwoFactorAuth(User user)
    {
        if (!user.TwoFactorEnabled)
            return Result.Success(user);
            
        return ValidateTwoFactorCode(user, request.TwoFactorCode)
            .Map(_ => user);
    }
}
```

### Data Import Pipeline

```csharp
public class DataImportService
{
    public Result<ImportResult> ImportData(string filePath)
    {
        return ValidateFile(filePath)
            .Bind(ReadFile)
            .Bind(ParseCsv)
            .Bind(ValidateHeaders)
            .Bind(ParseRecords)
            .Bind(ValidateRecords)
            .Bind(TransformRecords)
            .Bind(CheckDuplicates)
            .Bind(SaveToDatabase)
            .Bind(UpdateSearchIndex)
            .Bind(GenerateReport)
            .TapError(error => RollbackImport(filePath, error));
    }
    
    private Result<List<Record>> ValidateRecords(List<RawRecord> records)
    {
        var errors = new List<Error>();
        var validRecords = new List<Record>();
        
        foreach (var (record, index) in records.Select((r, i) => (r, i)))
        {
            ValidateRecord(record)
                .Match(
                    onSuccess: validRecord => validRecords.Add(validRecord),
                    onFailure: error => errors.Add(error
                        .WithMetadata("row", index + 2)
                        .WithMetadata("data", record))
                );
        }
        
        return errors.Any()
            ? Result.Failure<List<Record>>(new AggregateError(errors))
            : Result.Success(validRecords);
    }
}
```

## Advanced Patterns

### Async Railways

```csharp
public async Task<Result<Order>> ProcessOrderAsync(OrderRequest request)
{
    return await ValidateRequestAsync(request)
        .BindAsync(async req => await CheckInventoryAsync(req.Items))
        .BindAsync(async inv => await ProcessPaymentAsync(request.Payment))
        .BindAsync(async pay => await CreateOrderAsync(request))
        .TapAsync(async order => await SendNotificationAsync(order));
}
```

### Railway with State

```csharp
public class OrderProcessingContext
{
    public Order Order { get; set; }
    public List<Guid> ReservationIds { get; set; } = new();
    public PaymentResult Payment { get; set; }
    public ShipmentDetails Shipment { get; set; }
}

public Result<OrderConfirmation> ProcessOrderWithContext(OrderRequest request)
{
    var context = new OrderProcessingContext();
    
    return ValidateRequest(request)
        .Bind(req => CreateOrder(req).Tap(order => context.Order = order))
        .Bind(order => ReserveInventory(order.Items)
            .Tap(ids => context.ReservationIds.AddRange(ids)))
        .Bind(_ => ProcessPayment(request.Payment)
            .Tap(payment => context.Payment = payment))
        .Bind(_ => CreateShipment(context.Order)
            .Tap(shipment => context.Shipment = shipment))
        .Map(_ => BuildConfirmation(context))
        .TapError(_ => Rollback(context));
}
```

### Railway Middleware

```csharp
public class RailwayMiddleware<TIn, TOut>
{
    private readonly List<Func<TIn, Result<TOut>>> _middleware = new();
    
    public RailwayMiddleware<TIn, TOut> Use(Func<TIn, Result<TOut>> middleware)
    {
        _middleware.Add(middleware);
        return this;
    }
    
    public Result<TOut> Execute(TIn input)
    {
        return _middleware.Aggregate(
            Result.Success(input),
            (current, middleware) => current.Bind(middleware)
        );
    }
}

// Usage
var pipeline = new RailwayMiddleware<OrderRequest, Order>()
    .Use(ValidateRequest)
    .Use(EnrichRequest)
    .Use(ApplyBusinessRules)
    .Use(CreateOrder);
    
var result = pipeline.Execute(request);
```

### Railway Composition

```csharp
public static class RailwayExtensions
{
    public static Func<T, Result<T3>> Compose<T, T1, T2, T3>(
        this Func<T, Result<T1>> first,
        Func<T1, Result<T2>> second,
        Func<T2, Result<T3>> third)
    {
        return input => first(input).Bind(second).Bind(third);
    }
}

// Usage
var processOrder = ValidateOrder
    .Compose(CalculatePricing, ReserveInventory, ProcessPayment);
    
var result = processOrder(orderRequest);
```

## Best Practices

### 1. Keep Functions Pure

```csharp
// Bad: Side effects in transformation
public Result<Order> ProcessOrder(OrderRequest request)
{
    return ValidateRequest(request)
        .Map(req =>
        {
            _logger.LogInfo("Processing order"); // Side effect!
            var order = new Order(req);
            _db.Save(order); // Side effect!
            return order;
        });
}

// Good: Use Bind for operations with side effects
public Result<Order> ProcessOrder(OrderRequest request)
{
    return ValidateRequest(request)
        .Bind(CreateOrder)      // Returns Result<Order>
        .Tap(order => _logger.LogInfo($"Processing order {order.Id}"))
        .Bind(SaveOrder);       // Returns Result<Order>
}
```

### 2. Small, Focused Functions

```csharp
// Each function does one thing
private Result<ValidatedEmail> ValidateEmail(string email) { }
private Result<User> FindUserByEmail(ValidatedEmail email) { }
private Result<User> UpdateLastLogin(User user) { }
private Result<AuthToken> GenerateToken(User user) { }

// Compose them
public Result<AuthToken> Login(string email, string password)
{
    return ValidateEmail(email)
        .Bind(FindUserByEmail)
        .Bind(user => VerifyPassword(user, password))
        .Bind(UpdateLastLogin)
        .Bind(GenerateToken);
}
```

### 3. Meaningful Error Messages

```csharp
// Bad: Generic errors
return Result.Failure<Order>("Failed");

// Good: Specific, actionable errors
return new ValidationError("Order must contain at least one item")
    .WithMetadata("orderId", request.Id)
    .WithMetadata("itemCount", 0);
```

### 4. Handle All Error Cases

```csharp
public IActionResult ProcessRequest(Request request)
{
    return ProcessOrder(request).Match<IActionResult>(
        onSuccess: order => Ok(new { orderId = order.Id }),
        onFailure: error => error switch
        {
            ValidationError ve => BadRequest(ve.Message),
            NotFoundError nf => NotFound(nf.Message),
            ConflictError ce => Conflict(ce.Message),
            AuthorizationError ae => Forbid(ae.Message),
            _ => StatusCode(500, "An unexpected error occurred")
        }
    );
}
```

### 5. Test the Railway

```csharp
[Test]
public void ProcessOrder_WithInvalidItems_ShouldFailAtValidation()
{
    var request = new OrderRequest { Items = new List<Item>() };
    
    var result = _service.ProcessOrder(request);
    
    result.Should()
        .BeFailure()
        .WithError<ValidationError>()
        .WithMessage("Order must contain items");
}

[Test]
public void ProcessOrder_WithValidRequest_ShouldCompleteAllSteps()
{
    var request = CreateValidOrderRequest();
    
    var result = _service.ProcessOrder(request);
    
    result.Should().BeSuccess();
    _inventoryService.Verify(x => x.Reserve(It.IsAny<List<Item>>()), Times.Once);
    _paymentService.Verify(x => x.Process(It.IsAny<Payment>()), Times.Once);
    _orderRepository.Verify(x => x.Save(It.IsAny<Order>()), Times.Once);
}
```

## Summary

Railway-Oriented Programming with FluentUnions provides:

1. **Clear error handling** - Errors are explicit in the type system
2. **Composable operations** - Build complex flows from simple functions
3. **Readable code** - The flow reads like a description of the business process
4. **Testable functions** - Each step can be tested independently
5. **Maintainable systems** - Easy to add, remove, or reorder steps

The railway pattern transforms complex business processes into elegant, functional pipelines that are easy to understand, test, and maintain.

Next steps:
- [Real-World Examples](real-world-examples.md)
- [Advanced Patterns](../patterns/service-patterns.md)
- [Testing Guide](../guides/testing-guide.md)
# Service Patterns

This guide demonstrates service layer patterns using FluentUnions for building robust business logic layers.

## Table of Contents
1. [Introduction](#introduction)
2. [Basic Service Pattern](#basic-service-pattern)
3. [Domain Services](#domain-services)
4. [Application Services](#application-services)
5. [Service Composition](#service-composition)
6. [Cross-Cutting Concerns](#cross-cutting-concerns)
7. [Service Testing](#service-testing)
8. [CQRS Pattern](#cqrs-pattern)
9. [Saga and Process Managers](#saga-and-process-managers)
10. [Best Practices](#best-practices)

## Introduction

Service patterns with FluentUnions provide:
- Clear separation of business logic from infrastructure
- Explicit error handling throughout the service layer
- Composable service operations
- Testable business logic

## Basic Service Pattern

### Service Interface

```csharp
public interface IUserService
{
    Result<User> GetUser(Guid id);
    Result<User> CreateUser(CreateUserRequest request);
    Result<User> UpdateUser(Guid id, UpdateUserRequest request);
    Result DeleteUser(Guid id);
    Result<PagedResult<User>> SearchUsers(UserSearchCriteria criteria);
    Result<User> AuthenticateUser(string email, string password);
    Result ResetPassword(Guid userId, string newPassword);
}
```

### Service Implementation

```csharp
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IEmailService _emailService;
    private readonly ILogger<UserService> _logger;
    private readonly IValidator<CreateUserRequest> _createValidator;
    
    public UserService(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IEmailService emailService,
        ILogger<UserService> logger,
        IValidator<CreateUserRequest> createValidator)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _emailService = emailService;
        _logger = logger;
        _createValidator = createValidator;
    }
    
    public Result<User> GetUser(Guid id)
    {
        _logger.LogInformation("Getting user {UserId}", id);
        
        return _userRepository.GetById(id)
            .Tap(user => _logger.LogInformation("User {UserId} found", id))
            .TapError(error => _logger.LogWarning("User {UserId} not found: {Error}", id, error.Message));
    }
    
    public Result<User> CreateUser(CreateUserRequest request)
    {
        return _createValidator.Validate(request)
            .Bind(() => CheckEmailUniqueness(request.Email))
            .Bind(() => CreateUserEntity(request))
            .Bind(user => _userRepository.Create(user)
                .Map(id => { user.Id = id; return user; }))
            .Bind(user => SendWelcomeEmail(user)
                .Map(() => user))
            .Tap(user => _logger.LogInformation("User {UserId} created with email {Email}", 
                user.Id, user.Email));
    }
    
    public Result<User> UpdateUser(Guid id, UpdateUserRequest request)
    {
        return _userRepository.GetById(id)
            .Bind(user => ValidateUpdateRequest(user, request))
            .Bind(user => ApplyUpdates(user, request))
            .Bind(user => _userRepository.Update(user))
            .Tap(() => _logger.LogInformation("User {UserId} updated", id));
    }
    
    public Result DeleteUser(Guid id)
    {
        return _userRepository.GetById(id)
            .Bind(user => ValidateCanDelete(user))
            .Bind(() => _userRepository.Delete(id))
            .Tap(() => _logger.LogInformation("User {UserId} deleted", id));
    }
    
    public Result<User> AuthenticateUser(string email, string password)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            return new ValidationError("Email and password are required");
            
        return _userRepository.GetByEmail(email)
            .Bind(user => VerifyPassword(user, password))
            .Bind(user => CheckAccountStatus(user))
            .Bind(user => UpdateLastLogin(user))
            .Tap(user => _logger.LogInformation("User {UserId} authenticated", user.Id))
            .TapError(error => _logger.LogWarning("Authentication failed for {Email}: {Error}", 
                email, error.Message));
    }
    
    private Result CheckEmailUniqueness(string email)
    {
        return _userRepository.GetByEmail(email)
            .Match(
                onSuccess: _ => new ConflictError($"Email {email} is already registered"),
                onFailure: error => error is NotFoundError 
                    ? Result.Success() 
                    : Result.Failure(error));
    }
    
    private Result<User> CreateUserEntity(CreateUserRequest request)
    {
        return Result.Try(() =>
        {
            var hashedPassword = _passwordHasher.Hash(request.Password);
            
            return new User
            {
                Email = request.Email,
                PasswordHash = hashedPassword,
                Profile = new UserProfile
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName
                },
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };
        });
    }
    
    private Result<User> VerifyPassword(User user, string password)
    {
        var isValid = _passwordHasher.Verify(password, user.PasswordHash);
        
        return isValid
            ? Result.Success(user)
            : new AuthenticationError("Invalid credentials");
    }
    
    private Result<User> CheckAccountStatus(User user)
    {
        if (!user.IsActive)
            return new AuthenticationError("Account is deactivated");
            
        if (user.IsLocked)
            return new AuthenticationError($"Account is locked until {user.LockedUntil}");
            
        return Result.Success(user);
    }
}
```

## Domain Services

### Domain Service for Complex Business Logic

```csharp
public interface IPricingService
{
    Result<PricingResult> CalculateOrderPricing(Order order, Customer customer);
    Result<decimal> CalculateDiscount(Order order, DiscountCode code);
    Result<ShippingCost> CalculateShipping(Order order, ShippingMethod method);
}

public class PricingService : IPricingService
{
    private readonly IDiscountRepository _discountRepository;
    private readonly IShippingRateRepository _shippingRateRepository;
    private readonly ITaxCalculator _taxCalculator;
    
    public Result<PricingResult> CalculateOrderPricing(Order order, Customer customer)
    {
        return CalculateSubtotal(order)
            .Bind(subtotal => ApplyCustomerDiscount(subtotal, customer))
            .Bind(discountedTotal => CalculateTax(discountedTotal, order.ShippingAddress))
            .Bind(taxedTotal => AddShippingCost(taxedTotal, order))
            .Map(finalTotal => new PricingResult
            {
                Subtotal = order.Items.Sum(i => i.Price * i.Quantity),
                Discount = GetDiscountAmount(order, customer),
                Tax = GetTaxAmount(order),
                Shipping = GetShippingCost(order),
                Total = finalTotal
            });
    }
    
    public Result<decimal> CalculateDiscount(Order order, DiscountCode code)
    {
        return ValidateDiscountCode(code)
            .Bind(discount => CheckDiscountEligibility(order, discount))
            .Map(discount => discount.Type switch
            {
                DiscountType.Percentage => order.Subtotal * (discount.Value / 100),
                DiscountType.FixedAmount => Math.Min(discount.Value, order.Subtotal),
                DiscountType.FreeShipping => order.ShippingCost,
                _ => 0m
            });
    }
    
    private Result<Discount> ValidateDiscountCode(DiscountCode code)
    {
        return _discountRepository.GetByCode(code.Value)
            .Bind(discount =>
            {
                if (!discount.IsActive)
                    return new ValidationError("Discount code is not active");
                    
                if (discount.ValidFrom > DateTime.UtcNow)
                    return new ValidationError("Discount code is not yet valid");
                    
                if (discount.ValidTo < DateTime.UtcNow)
                    return new ValidationError("Discount code has expired");
                    
                if (discount.UsageCount >= discount.MaxUsages)
                    return new ValidationError("Discount code usage limit reached");
                    
                return Result.Success(discount);
            });
    }
    
    private Result<Discount> CheckDiscountEligibility(Order order, Discount discount)
    {
        if (discount.MinimumOrderAmount.HasValue && order.Subtotal < discount.MinimumOrderAmount)
            return new ValidationError($"Minimum order amount of {discount.MinimumOrderAmount:C} required");
            
        if (discount.RequiredProducts.Any())
        {
            var hasRequiredProducts = discount.RequiredProducts
                .All(productId => order.Items.Any(i => i.ProductId == productId));
                
            if (!hasRequiredProducts)
                return new ValidationError("Order does not contain required products");
        }
        
        if (discount.ExcludedCategories.Any())
        {
            var hasExcludedItems = order.Items
                .Any(i => discount.ExcludedCategories.Contains(i.Product.CategoryId));
                
            if (hasExcludedItems)
                return new ValidationError("Discount not valid for items in cart");
        }
        
        return Result.Success(discount);
    }
}
```

### Domain Service for Invariant Rules

```csharp
public interface IInventoryService
{
    Result<InventoryReservation> ReserveStock(Product product, int quantity);
    Result ReleaseReservation(InventoryReservation reservation);
    Result<bool> CheckAvailability(Product product, int quantity);
    Result TransferStock(Warehouse from, Warehouse to, Product product, int quantity);
}

public class InventoryService : IInventoryService
{
    private readonly IInventoryRepository _inventoryRepository;
    private readonly IReservationRepository _reservationRepository;
    
    public Result<InventoryReservation> ReserveStock(Product product, int quantity)
    {
        return GetAvailableStock(product)
            .Bind(stock => ValidateStockLevel(stock, quantity))
            .Bind(stock => CreateReservation(product, quantity, stock))
            .Bind(reservation => _reservationRepository.Add(reservation)
                .Map(() => reservation));
    }
    
    public Result TransferStock(Warehouse from, Warehouse to, Product product, int quantity)
    {
        // This ensures the invariant: total stock across warehouses remains constant
        return _inventoryRepository.BeginTransaction()
            .Bind(() => GetWarehouseStock(from, product))
            .Bind(fromStock => ValidateStockLevel(fromStock, quantity))
            .Bind(() => DeductStock(from, product, quantity))
            .Bind(() => AddStock(to, product, quantity))
            .Bind(() => _inventoryRepository.CommitTransaction())
            .TapError(() => _inventoryRepository.RollbackTransaction());
    }
    
    private Result<Stock> ValidateStockLevel(Stock stock, int requestedQuantity)
    {
        var available = stock.Quantity - stock.Reserved;
        
        if (available < requestedQuantity)
            return new InsufficientStockError(
                stock.ProductId, 
                requestedQuantity, 
                available);
                
        if (stock.Quantity < stock.MinimumLevel + requestedQuantity)
            return new LowStockWarning(
                stock.ProductId,
                stock.Quantity - requestedQuantity,
                stock.MinimumLevel);
                
        return Result.Success(stock);
    }
}
```

## Application Services

### Application Service Orchestration

```csharp
public interface IOrderApplicationService
{
    Result<OrderDto> PlaceOrder(PlaceOrderCommand command);
    Result<OrderDto> GetOrder(Guid orderId);
    Result CancelOrder(Guid orderId, string reason);
    Result<ShipmentDto> ShipOrder(Guid orderId);
}

public class OrderApplicationService : IOrderApplicationService
{
    private readonly IOrderService _orderService;
    private readonly IInventoryService _inventoryService;
    private readonly IPaymentService _paymentService;
    private readonly IShippingService _shippingService;
    private readonly INotificationService _notificationService;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    
    public Result<OrderDto> PlaceOrder(PlaceOrderCommand command)
    {
        return _unitOfWork.ExecuteInTransaction(() =>
            ValidateCommand(command)
                .Bind(() => LoadCustomer(command.CustomerId))
                .Bind(customer => CreateOrder(command, customer))
                .Bind(order => ReserveInventory(order))
                .Bind(order => ProcessPayment(order, command.PaymentInfo))
                .Bind(order => SaveOrder(order))
                .Bind(order => SendOrderConfirmation(order))
                .Map(order => _mapper.Map<OrderDto>(order))
        );
    }
    
    private Result<Order> CreateOrder(PlaceOrderCommand command, Customer customer)
    {
        return _orderService.CreateOrder(new CreateOrderRequest
        {
            Customer = customer,
            Items = command.Items.Select(i => new OrderItem
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                Price = i.Price
            }).ToList(),
            ShippingAddress = command.ShippingAddress,
            BillingAddress = command.BillingAddress ?? command.ShippingAddress
        });
    }
    
    private Result<Order> ReserveInventory(Order order)
    {
        var reservationTasks = order.Items
            .Select(item => _inventoryService.ReserveStock(item.ProductId, item.Quantity))
            .ToList();
            
        return Result.Combine(reservationTasks)
            .Map(reservations =>
            {
                order.InventoryReservations = reservations.ToList();
                return order;
            })
            .MapError(error =>
            {
                // Rollback any successful reservations
                RollbackReservations(order.InventoryReservations);
                return error;
            });
    }
    
    private Result<Order> ProcessPayment(Order order, PaymentInfo paymentInfo)
    {
        return _paymentService.ProcessPayment(new PaymentRequest
        {
            Amount = order.Total,
            Currency = order.Currency,
            PaymentMethod = paymentInfo.Method,
            CardNumber = paymentInfo.CardNumber,
            OrderId = order.Id
        })
        .Map(paymentResult =>
        {
            order.PaymentId = paymentResult.TransactionId;
            order.PaymentStatus = PaymentStatus.Paid;
            return order;
        })
        .MapError(error =>
        {
            RollbackReservations(order.InventoryReservations);
            return new PaymentFailedError(error.Message);
        });
    }
}
```

### Application Service with Workflow

```csharp
public class RegistrationWorkflowService
{
    private readonly IUserService _userService;
    private readonly IEmailVerificationService _emailVerificationService;
    private readonly IWelcomePackageService _welcomePackageService;
    private readonly IAnalyticsService _analyticsService;
    
    public Result<RegistrationResult> RegisterNewUser(RegistrationRequest request)
    {
        return ExecuteRegistrationWorkflow(request)
            .Tap(result => _analyticsService.TrackRegistration(result));
    }
    
    private Result<RegistrationResult> ExecuteRegistrationWorkflow(RegistrationRequest request)
    {
        var context = new RegistrationContext(request);
        
        return ValidateRegistrationRequest(context)
            .Bind(CreateUserAccount)
            .Bind(SendVerificationEmail)
            .Bind(CreateWelcomePackage)
            .Bind(ScheduleOnboardingEmails)
            .Map(BuildRegistrationResult);
    }
    
    private Result<RegistrationContext> CreateUserAccount(RegistrationContext context)
    {
        return _userService.CreateUser(new CreateUserRequest
        {
            Email = context.Request.Email,
            Password = context.Request.Password,
            FirstName = context.Request.FirstName,
            LastName = context.Request.LastName,
            ReferralCode = context.Request.ReferralCode
        })
        .Map(user =>
        {
            context.CreatedUser = user;
            return context;
        });
    }
    
    private Result<RegistrationContext> SendVerificationEmail(RegistrationContext context)
    {
        return _emailVerificationService.SendVerificationEmail(context.CreatedUser)
            .Map(verificationToken =>
            {
                context.VerificationToken = verificationToken;
                return context;
            })
            .OrElse(error =>
            {
                // Don't fail registration if email fails
                _logger.LogError("Failed to send verification email: {Error}", error);
                context.EmailVerificationFailed = true;
                return Result.Success(context);
            });
    }
}
```

## Service Composition

### Decorator Pattern

```csharp
public class LoggingServiceDecorator<TService> : IUserService
{
    private readonly IUserService _innerService;
    private readonly ILogger<LoggingServiceDecorator<TService>> _logger;
    
    public LoggingServiceDecorator(
        IUserService innerService,
        ILogger<LoggingServiceDecorator<TService>> logger)
    {
        _innerService = innerService;
        _logger = logger;
    }
    
    public Result<User> GetUser(Guid id)
    {
        _logger.LogInformation("Getting user {UserId}", id);
        
        var stopwatch = Stopwatch.StartNew();
        
        return _innerService.GetUser(id)
            .Tap(user => _logger.LogInformation(
                "Retrieved user {UserId} in {ElapsedMs}ms", 
                id, stopwatch.ElapsedMilliseconds))
            .TapError(error => _logger.LogWarning(
                "Failed to get user {UserId}: {Error} in {ElapsedMs}ms", 
                id, error.Message, stopwatch.ElapsedMilliseconds));
    }
    
    // Implement other methods similarly...
}

// Caching decorator
public class CachingServiceDecorator : IUserService
{
    private readonly IUserService _innerService;
    private readonly IMemoryCache _cache;
    private readonly TimeSpan _cacheExpiration;
    
    public Result<User> GetUser(Guid id)
    {
        var cacheKey = $"user_{id}";
        
        if (_cache.TryGetValue<User>(cacheKey, out var cachedUser))
            return Result.Success(cachedUser);
            
        return _innerService.GetUser(id)
            .Tap(user => _cache.Set(cacheKey, user, _cacheExpiration));
    }
    
    public Result<User> UpdateUser(Guid id, UpdateUserRequest request)
    {
        return _innerService.UpdateUser(id, request)
            .Tap(() => _cache.Remove($"user_{id}"));
    }
}
```

### Service Pipeline

```csharp
public interface IServicePipeline<TRequest, TResponse>
{
    Result<TResponse> Execute(TRequest request);
}

public class ServicePipeline<TRequest, TResponse> : IServicePipeline<TRequest, TResponse>
{
    private readonly List<IServiceMiddleware<TRequest, TResponse>> _middleware = new();
    
    public ServicePipeline<TRequest, TResponse> Use(IServiceMiddleware<TRequest, TResponse> middleware)
    {
        _middleware.Add(middleware);
        return this;
    }
    
    public Result<TResponse> Execute(TRequest request)
    {
        return _middleware.Aggregate(
            Result.Success(request),
            (current, middleware) => current.Bind(req => middleware.Execute(req))
        ) as Result<TResponse>;
    }
}

// Validation middleware
public class ValidationMiddleware<TRequest, TResponse> : IServiceMiddleware<TRequest, TResponse>
{
    private readonly IValidator<TRequest> _validator;
    
    public Result<TRequest> Execute(TRequest request)
    {
        return _validator.Validate(request)
            .Map(() => request);
    }
}

// Authorization middleware
public class AuthorizationMiddleware<TRequest, TResponse> : IServiceMiddleware<TRequest, TResponse>
{
    private readonly IAuthorizationService _authService;
    
    public Result<TRequest> Execute(TRequest request)
    {
        return _authService.Authorize(request)
            .Map(() => request);
    }
}
```

## Cross-Cutting Concerns

### Transaction Management

```csharp
public class TransactionalService<TService> where TService : class
{
    private readonly TService _service;
    private readonly IUnitOfWork _unitOfWork;
    
    public Result<TResult> ExecuteInTransaction<TResult>(
        Func<TService, Result<TResult>> operation)
    {
        return _unitOfWork.BeginTransaction()
            .Bind(() => operation(_service))
            .Tap(() => _unitOfWork.CommitTransaction())
            .TapError(() => _unitOfWork.RollbackTransaction());
    }
}

// Usage
public class OrderService
{
    private readonly TransactionalService<OrderService> _transactional;
    
    public Result<Order> PlaceOrder(OrderRequest request)
    {
        return _transactional.ExecuteInTransaction(service =>
            service.CreateOrder(request)
                .Bind(order => service.ProcessPayment(order))
                .Bind(order => service.UpdateInventory(order))
        );
    }
}
```

### Retry Policy

```csharp
public class RetryService<TService> where TService : class
{
    private readonly TService _service;
    private readonly IRetryPolicy _retryPolicy;
    
    public async Task<Result<TResult>> ExecuteWithRetryAsync<TResult>(
        Func<TService, Task<Result<TResult>>> operation,
        RetryOptions options = null)
    {
        options ??= RetryOptions.Default;
        var attempts = 0;
        var lastError = default(Error);
        
        while (attempts < options.MaxAttempts)
        {
            attempts++;
            
            var result = await operation(_service);
            if (result.IsSuccess)
                return result;
                
            lastError = result.Error;
            
            if (!IsRetryable(lastError) || attempts >= options.MaxAttempts)
                break;
                
            var delay = CalculateDelay(attempts, options);
            await Task.Delay(delay);
        }
        
        return Result.Failure<TResult>(new RetryExhaustedError(
            $"Operation failed after {attempts} attempts",
            lastError));
    }
    
    private bool IsRetryable(Error error)
    {
        return error is not (
            ValidationError or
            AuthenticationError or
            AuthorizationError or
            NotFoundError);
    }
    
    private TimeSpan CalculateDelay(int attempt, RetryOptions options)
    {
        return options.Strategy switch
        {
            RetryStrategy.Fixed => options.BaseDelay,
            RetryStrategy.Linear => TimeSpan.FromMilliseconds(
                options.BaseDelay.TotalMilliseconds * attempt),
            RetryStrategy.Exponential => TimeSpan.FromMilliseconds(
                options.BaseDelay.TotalMilliseconds * Math.Pow(2, attempt - 1)),
            _ => options.BaseDelay
        };
    }
}
```

### Circuit Breaker

```csharp
public class CircuitBreakerService<TService> where TService : class
{
    private readonly TService _service;
    private readonly CircuitBreakerState _state;
    private readonly CircuitBreakerOptions _options;
    
    public Result<TResult> Execute<TResult>(
        Func<TService, Result<TResult>> operation)
    {
        if (_state.IsOpen)
        {
            if (!_state.ShouldAttemptReset(_options.ResetTimeout))
                return new ServiceUnavailableError("Service circuit is open");
                
            _state.TransitionToHalfOpen();
        }
        
        var result = operation(_service);
        
        if (result.IsSuccess)
        {
            _state.RecordSuccess();
            if (_state.IsHalfOpen)
                _state.TransitionToClosed();
        }
        else
        {
            _state.RecordFailure();
            if (_state.FailureCount >= _options.FailureThreshold)
                _state.TransitionToOpen();
        }
        
        return result;
    }
}
```

## Service Testing

### Service Test Base

```csharp
public abstract class ServiceTestBase<TService>
{
    protected TService Service { get; private set; }
    protected Mock<ILogger<TService>> LoggerMock { get; private set; }
    
    [SetUp]
    public virtual void SetUp()
    {
        LoggerMock = new Mock<ILogger<TService>>();
        Service = CreateService();
    }
    
    protected abstract TService CreateService();
    
    protected void VerifyLogging(LogLevel level, string messagePattern, Times times)
    {
        LoggerMock.Verify(
            x => x.Log(
                level,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString().Contains(messagePattern)),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            times);
    }
}

// Example test
public class UserServiceTests : ServiceTestBase<UserService>
{
    private Mock<IUserRepository> _repositoryMock;
    private Mock<IEmailService> _emailServiceMock;
    
    protected override UserService CreateService()
    {
        _repositoryMock = new Mock<IUserRepository>();
        _emailServiceMock = new Mock<IEmailService>();
        
        return new UserService(
            _repositoryMock.Object,
            new Mock<IPasswordHasher>().Object,
            _emailServiceMock.Object,
            LoggerMock.Object,
            new Mock<IValidator<CreateUserRequest>>().Object);
    }
    
    [Test]
    public void CreateUser_WithValidRequest_CreatesUserAndSendsEmail()
    {
        // Arrange
        var request = new CreateUserRequest
        {
            Email = "test@example.com",
            Password = "password123",
            FirstName = "John",
            LastName = "Doe"
        };
        
        _repositoryMock
            .Setup(r => r.GetByEmail(request.Email))
            .Returns(Result.Failure<User>(new NotFoundError()));
            
        _repositoryMock
            .Setup(r => r.Create(It.IsAny<User>()))
            .Returns(Result.Success(Guid.NewGuid()));
            
        _emailServiceMock
            .Setup(e => e.SendWelcomeEmail(It.IsAny<User>()))
            .Returns(Result.Success());
        
        // Act
        var result = Service.CreateUser(request);
        
        // Assert
        result.Should().BeSuccess();
        result.Value.Email.Should().Be(request.Email);
        
        _repositoryMock.Verify(r => r.Create(It.IsAny<User>()), Times.Once);
        _emailServiceMock.Verify(e => e.SendWelcomeEmail(It.IsAny<User>()), Times.Once);
        VerifyLogging(LogLevel.Information, "created", Times.Once());
    }
}
```

### Integration Testing

```csharp
public class OrderServiceIntegrationTests
{
    private IServiceProvider _serviceProvider;
    private IOrderApplicationService _orderService;
    
    [SetUp]
    public void SetUp()
    {
        var services = new ServiceCollection();
        
        // Register real implementations
        services.AddDbContext<AppDbContext>(options =>
            options.UseInMemoryDatabase($"Test_{Guid.NewGuid()}"));
            
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IInventoryService, InventoryService>();
        
        // Register test doubles
        services.AddSingleton<IPaymentService>(new TestPaymentService());
        services.AddSingleton<IEmailService>(new TestEmailService());
        
        services.AddScoped<IOrderApplicationService, OrderApplicationService>();
        
        _serviceProvider = services.BuildServiceProvider();
        _orderService = _serviceProvider.GetRequiredService<IOrderApplicationService>();
    }
    
    [Test]
    public async Task PlaceOrder_CompleteFlow_Success()
    {
        // Arrange
        await SeedTestData();
        
        var command = new PlaceOrderCommand
        {
            CustomerId = TestData.CustomerId,
            Items = new[]
            {
                new OrderItemDto { ProductId = TestData.ProductId, Quantity = 2 }
            },
            ShippingAddress = TestData.ValidAddress,
            PaymentInfo = TestData.ValidPaymentInfo
        };
        
        // Act
        var result = await _orderService.PlaceOrder(command);
        
        // Assert
        result.Should().BeSuccess();
        result.Value.Should().NotBeNull();
        result.Value.Status.Should().Be(OrderStatus.Confirmed);
        
        // Verify side effects
        await VerifyInventoryReserved();
        await VerifyPaymentProcessed();
        await VerifyEmailSent();
    }
}
```

## CQRS Pattern

### Command Handlers

```csharp
public interface ICommandHandler<TCommand, TResult>
{
    Result<TResult> Handle(TCommand command);
}

public interface ICommand<TResult> { }

public class CreateOrderCommand : ICommand<OrderDto>
{
    public Guid CustomerId { get; set; }
    public List<OrderItemDto> Items { get; set; }
    public Address ShippingAddress { get; set; }
}

public class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, OrderDto>
{
    private readonly IOrderService _orderService;
    private readonly IInventoryService _inventoryService;
    private readonly IEventBus _eventBus;
    
    public Result<OrderDto> Handle(CreateOrderCommand command)
    {
        return ValidateCommand(command)
            .Bind(() => _orderService.CreateOrder(command))
            .Bind(order => _inventoryService.ReserveStock(order.Items)
                .Map(() => order))
            .Tap(order => _eventBus.Publish(new OrderCreatedEvent(order)))
            .Map(order => new OrderDto(order));
    }
}
```

### Query Handlers

```csharp
public interface IQueryHandler<TQuery, TResult>
{
    Result<TResult> Handle(TQuery query);
}

public interface IQuery<TResult> { }

public class GetOrderByIdQuery : IQuery<OrderDto>
{
    public Guid OrderId { get; set; }
}

public class GetOrderByIdQueryHandler : IQueryHandler<GetOrderByIdQuery, OrderDto>
{
    private readonly IOrderReadRepository _repository;
    
    public Result<OrderDto> Handle(GetOrderByIdQuery query)
    {
        return _repository.GetById(query.OrderId)
            .Map(order => new OrderDto(order));
    }
}

// Query with complex filtering
public class SearchOrdersQuery : IQuery<PagedResult<OrderSummaryDto>>
{
    public string CustomerEmail { get; set; }
    public OrderStatus? Status { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}

public class SearchOrdersQueryHandler : IQueryHandler<SearchOrdersQuery, PagedResult<OrderSummaryDto>>
{
    private readonly IOrderReadRepository _repository;
    
    public Result<PagedResult<OrderSummaryDto>> Handle(SearchOrdersQuery query)
    {
        var specification = new OrderSearchSpecification(
            query.CustomerEmail,
            query.Status,
            query.FromDate,
            query.ToDate,
            query.Page,
            query.PageSize);
            
        return _repository.Search(specification)
            .Map(results => new PagedResult<OrderSummaryDto>
            {
                Items = results.Items.Select(o => new OrderSummaryDto(o)).ToList(),
                TotalCount = results.TotalCount,
                Page = results.Page,
                PageSize = results.PageSize
            });
    }
}
```

### Command/Query Dispatcher

```csharp
public interface IDispatcher
{
    Result<TResult> Send<TResult>(ICommand<TResult> command);
    Result<TResult> Query<TResult>(IQuery<TResult> query);
}

public class Dispatcher : IDispatcher
{
    private readonly IServiceProvider _serviceProvider;
    
    public Result<TResult> Send<TResult>(ICommand<TResult> command)
    {
        var handlerType = typeof(ICommandHandler<,>)
            .MakeGenericType(command.GetType(), typeof(TResult));
            
        var handler = _serviceProvider.GetService(handlerType);
        if (handler == null)
            return new Error("NO_HANDLER", $"No handler found for {command.GetType().Name}");
            
        var method = handlerType.GetMethod("Handle");
        return (Result<TResult>)method.Invoke(handler, new[] { command });
    }
    
    public Result<TResult> Query<TResult>(IQuery<TResult> query)
    {
        var handlerType = typeof(IQueryHandler<,>)
            .MakeGenericType(query.GetType(), typeof(TResult));
            
        var handler = _serviceProvider.GetService(handlerType);
        if (handler == null)
            return new Error("NO_HANDLER", $"No handler found for {query.GetType().Name}");
            
        var method = handlerType.GetMethod("Handle");
        return (Result<TResult>)method.Invoke(handler, new[] { query });
    }
}
```

## Saga and Process Managers

### Saga Implementation

```csharp
public interface ISaga<TState>
{
    Result<TState> Execute(TState initialState);
}

public class OrderFulfillmentSaga : ISaga<OrderFulfillmentState>
{
    private readonly IOrderService _orderService;
    private readonly IInventoryService _inventoryService;
    private readonly IPaymentService _paymentService;
    private readonly IShippingService _shippingService;
    
    public Result<OrderFulfillmentState> Execute(OrderFulfillmentState state)
    {
        return CreateOrder(state)
            .Bind(ReserveInventory)
            .Bind(ProcessPayment)
            .Bind(CreateShipment)
            .Bind(NotifyCustomer)
            .TapError(error => Compensate(state, error));
    }
    
    private Result<OrderFulfillmentState> CreateOrder(OrderFulfillmentState state)
    {
        return _orderService.CreateOrder(state.Request)
            .Map(order =>
            {
                state.OrderId = order.Id;
                state.CompletedSteps.Add("CreateOrder");
                return state;
            });
    }
    
    private Result<OrderFulfillmentState> ReserveInventory(OrderFulfillmentState state)
    {
        return _inventoryService.ReserveItems(state.Request.Items)
            .Map(reservations =>
            {
                state.ReservationIds = reservations.Select(r => r.Id).ToList();
                state.CompletedSteps.Add("ReserveInventory");
                return state;
            });
    }
    
    private void Compensate(OrderFulfillmentState state, Error error)
    {
        // Compensate in reverse order
        if (state.CompletedSteps.Contains("CreateShipment"))
            _shippingService.CancelShipment(state.ShipmentId);
            
        if (state.CompletedSteps.Contains("ProcessPayment"))
            _paymentService.RefundPayment(state.PaymentId);
            
        if (state.CompletedSteps.Contains("ReserveInventory"))
            state.ReservationIds.ForEach(id => _inventoryService.ReleaseReservation(id));
            
        if (state.CompletedSteps.Contains("CreateOrder"))
            _orderService.CancelOrder(state.OrderId);
    }
}
```

### Process Manager

```csharp
public abstract class ProcessManager<TState> where TState : ProcessState
{
    protected readonly List<ProcessStep<TState>> Steps = new();
    
    protected void AddStep(
        string name,
        Func<TState, Result<TState>> execute,
        Func<TState, Result> compensate = null)
    {
        Steps.Add(new ProcessStep<TState>
        {
            Name = name,
            Execute = execute,
            Compensate = compensate
        });
    }
    
    public Result<TState> Execute(TState initialState)
    {
        var state = initialState;
        var completedSteps = new Stack<ProcessStep<TState>>();
        
        foreach (var step in Steps)
        {
            var result = step.Execute(state);
            
            if (result.IsSuccess)
            {
                state = result.Value;
                state.CompletedSteps.Add(step.Name);
                completedSteps.Push(step);
            }
            else
            {
                // Compensate completed steps
                while (completedSteps.Count > 0)
                {
                    var completedStep = completedSteps.Pop();
                    completedStep.Compensate?.Invoke(state);
                }
                
                return Result.Failure<TState>(result.Error);
            }
        }
        
        return Result.Success(state);
    }
}

public class OrderProcessManager : ProcessManager<OrderProcessState>
{
    public OrderProcessManager(/* dependencies */)
    {
        AddStep("ValidateOrder", ValidateOrder);
        AddStep("ReserveInventory", ReserveInventory, ReleaseInventory);
        AddStep("ProcessPayment", ProcessPayment, RefundPayment);
        AddStep("CreateShipment", CreateShipment, CancelShipment);
        AddStep("SendNotification", SendNotification);
    }
    
    // Step implementations...
}
```

## Best Practices

### 1. Keep Services Focused

```csharp
// Bad: Service doing too much
public class UserService
{
    public Result<User> CreateUser() { }
    public Result ProcessPayment() { }
    public Result SendEmail() { }
    public Result GenerateReport() { }
}

// Good: Focused services
public class UserService
{
    public Result<User> CreateUser() { }
    public Result<User> UpdateUser() { }
    public Result DeleteUser() { }
}

public class PaymentService
{
    public Result<Payment> ProcessPayment() { }
}
```

### 2. Use Result Types Consistently

```csharp
// Bad: Mixing exceptions and Results
public class BadService
{
    public User GetUser(Guid id)
    {
        var user = _repository.Find(id);
        if (user == null)
            throw new NotFoundException();
        return user;
    }
}

// Good: Consistent Result usage
public class GoodService
{
    public Result<User> GetUser(Guid id)
    {
        return _repository.GetById(id);
    }
}
```

### 3. Compose Services

```csharp
public class CompositeService
{
    private readonly IUserService _userService;
    private readonly IEmailService _emailService;
    private readonly IAnalyticsService _analyticsService;
    
    public Result<User> RegisterUser(RegistrationRequest request)
    {
        return _userService.CreateUser(request)
            .Bind(user => _emailService.SendWelcomeEmail(user)
                .Map(() => user))
            .Tap(user => _analyticsService.TrackRegistration(user));
    }
}
```

### 4. Handle Cross-Cutting Concerns

```csharp
// Use decorators or middleware for:
// - Logging
// - Caching
// - Validation
// - Authorization
// - Transaction management
// - Retry policies

services.AddScoped<IUserService, UserService>();
services.Decorate<IUserService, LoggingServiceDecorator<IUserService>>();
services.Decorate<IUserService, CachingServiceDecorator>();
services.Decorate<IUserService, ValidationServiceDecorator<IUserService>>();
```

### 5. Test Services Thoroughly

```csharp
[Test]
public void Service_Should_Handle_All_Error_Cases()
{
    // Test success path
    // Test each failure scenario
    // Test edge cases
    // Verify logging
    // Verify side effects
}
```

## Summary

Service patterns with FluentUnions provide:

1. **Clear separation of concerns** - Business logic isolated from infrastructure
2. **Explicit error handling** - All operations return Results
3. **Composable operations** - Build complex workflows from simple services
4. **Testable design** - Easy to mock dependencies and test scenarios
5. **Cross-cutting concerns** - Decorators for logging, caching, etc.

Key principles:
- Keep services focused on single responsibility
- Use Result types consistently throughout
- Compose services for complex operations
- Handle cross-cutting concerns with decorators
- Test all success and failure paths

Next steps:
- [API Patterns](api-patterns.md)
- [Testing Guide](../guides/testing-guide.md)
- [Integration Examples](../integration/aspnet-core.md)
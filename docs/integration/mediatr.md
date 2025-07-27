# MediatR Integration

This guide demonstrates how to integrate FluentUnions with MediatR for clean CQRS implementation with robust error handling.

## Table of Contents
1. [Setup](#setup)
2. [Command Handlers](#command-handlers)
3. [Query Handlers](#query-handlers)
4. [Pipeline Behaviors](#pipeline-behaviors)
5. [Validation](#validation)
6. [Notifications](#notifications)
7. [Error Handling](#error-handling)
8. [Testing](#testing)
9. [Advanced Patterns](#advanced-patterns)
10. [Best Practices](#best-practices)

## Setup

### Installation

```bash
dotnet add package FluentUnions
dotnet add package MediatR
dotnet add package FluentValidation
```

### Configuration

```csharp
var builder = WebApplication.CreateBuilder(args);

// Add MediatR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
});

// Add FluentValidation
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

// Add pipeline behaviors
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));

var app = builder.Build();
```

## Command Handlers

### Base Command with Result

```csharp
// Base interfaces
public interface ICommand : IRequest<Result> { }
public interface ICommand<TResponse> : IRequest<Result<TResponse>> { }

// Base handler interfaces
public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, Result>
    where TCommand : ICommand { }
    
public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>>
    where TCommand : ICommand<TResponse> { }
```

### Create Command Example

```csharp
public record CreateUserCommand(
    string Email,
    string Password,
    string FirstName,
    string LastName) : ICommand<UserDto>;

public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, UserDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IMapper _mapper;
    
    public CreateUserCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _mapper = mapper;
    }
    
    public async Task<Result<UserDto>> Handle(
        CreateUserCommand request, 
        CancellationToken cancellationToken)
    {
        // Check if email exists
        var existingUser = await _userRepository.FindByEmailAsync(request.Email);
        if (existingUser.IsSome)
            return new ConflictError("Email already exists");
        
        // Create user
        var user = new User
        {
            Email = request.Email,
            PasswordHash = _passwordHasher.HashPassword(request.Password),
            FirstName = request.FirstName,
            LastName = request.LastName,
            CreatedAt = DateTime.UtcNow
        };
        
        // Save user
        var saveResult = await _userRepository.AddAsync(user, cancellationToken);
        
        return saveResult.Map(savedUser => _mapper.Map<UserDto>(savedUser));
    }
}
```

### Update Command Example

```csharp
public record UpdateUserCommand(
    Guid UserId,
    string FirstName,
    string LastName,
    string PhoneNumber) : ICommand<UserDto>;

public class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand, UserDto>
{
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUser;
    private readonly IMapper _mapper;
    
    public async Task<Result<UserDto>> Handle(
        UpdateUserCommand request,
        CancellationToken cancellationToken)
    {
        // Get user
        var userResult = await _userRepository.GetByIdAsync(request.UserId);
        if (userResult.IsFailure)
            return Result.Failure<UserDto>(userResult.Error);
            
        var user = userResult.Value;
        
        // Check authorization
        if (user.Id != _currentUser.UserId && !_currentUser.IsAdmin)
            return new AuthorizationError("Cannot update other users");
        
        // Update fields
        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.PhoneNumber = request.PhoneNumber;
        user.UpdatedAt = DateTime.UtcNow;
        
        // Save changes
        var updateResult = await _userRepository.UpdateAsync(user, cancellationToken);
        
        return updateResult.Map(updated => _mapper.Map<UserDto>(updated));
    }
}
```

### Delete Command Example

```csharp
public record DeleteUserCommand(Guid UserId) : ICommand;

public class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUser;
    
    public async Task<Result> Handle(
        DeleteUserCommand request,
        CancellationToken cancellationToken)
    {
        return await _userRepository.GetByIdAsync(request.UserId)
            .BindAsync(async user =>
            {
                // Check authorization
                if (!_currentUser.IsAdmin)
                    return new AuthorizationError("Only admins can delete users");
                    
                // Check if user can be deleted
                if (user.HasActiveSubscription)
                    return new ConflictError("Cannot delete user with active subscription");
                    
                return await _userRepository.DeleteAsync(user.Id, cancellationToken);
            });
    }
}
```

## Query Handlers

### Base Query with Result

```csharp
public interface IQuery<TResponse> : IRequest<Result<TResponse>> { }

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse> { }
```

### Get By Id Query

```csharp
public record GetUserByIdQuery(Guid UserId) : IQuery<UserDto>;

public class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, UserDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    
    public async Task<Result<UserDto>> Handle(
        GetUserByIdQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _userRepository.GetByIdAsync(request.UserId);
        return result.Map(user => _mapper.Map<UserDto>(user));
    }
}
```

### Search Query with Pagination

```csharp
public record SearchUsersQuery(
    string SearchTerm,
    int Page = 1,
    int PageSize = 20,
    string SortBy = "Name",
    bool SortDescending = false) : IQuery<PagedResult<UserDto>>;

public class SearchUsersQueryHandler : IQueryHandler<SearchUsersQuery, PagedResult<UserDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    
    public async Task<Result<PagedResult<UserDto>>> Handle(
        SearchUsersQuery request,
        CancellationToken cancellationToken)
    {
        // Validate pagination
        if (request.Page < 1)
            return new ValidationError("Page must be greater than 0");
            
        if (request.PageSize < 1 || request.PageSize > 100)
            return new ValidationError("PageSize must be between 1 and 100");
        
        // Build specification
        var spec = new UserSearchSpecification(request.SearchTerm)
            .WithPaging(request.Page, request.PageSize)
            .WithSorting(request.SortBy, request.SortDescending);
        
        // Execute query
        var result = await _userRepository.SearchAsync(spec, cancellationToken);
        
        return result.Map(pagedData => new PagedResult<UserDto>
        {
            Items = _mapper.Map<List<UserDto>>(pagedData.Items),
            TotalCount = pagedData.TotalCount,
            Page = pagedData.Page,
            PageSize = pagedData.PageSize,
            TotalPages = pagedData.TotalPages
        });
    }
}
```

### Complex Query with Joins

```csharp
public record GetUserDashboardQuery(Guid UserId) : IQuery<UserDashboardDto>;

public class GetUserDashboardQueryHandler : IQueryHandler<GetUserDashboardQuery, UserDashboardDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly ISubscriptionRepository _subscriptionRepository;
    
    public async Task<Result<UserDashboardDto>> Handle(
        GetUserDashboardQuery request,
        CancellationToken cancellationToken)
    {
        // Get user
        var userResult = await _userRepository.GetByIdAsync(request.UserId);
        if (userResult.IsFailure)
            return Result.Failure<UserDashboardDto>(userResult.Error);
            
        var user = userResult.Value;
        
        // Get related data in parallel
        var orderTask = _orderRepository.GetRecentOrdersAsync(user.Id, 5);
        var subscriptionTask = _subscriptionRepository.GetActiveSubscriptionAsync(user.Id);
        var statsTask = _userRepository.GetUserStatisticsAsync(user.Id);
        
        await Task.WhenAll(orderTask, subscriptionTask, statsTask);
        
        // Combine results
        return Result.BindAll(
            orderTask.Result,
            subscriptionTask.Result.ToResult(new NotFoundError("No active subscription")),
            statsTask.Result)
            .Map((orders, subscription, stats) => new UserDashboardDto
            {
                User = new UserDto(user),
                RecentOrders = orders.Select(o => new OrderSummaryDto(o)).ToList(),
                Subscription = new SubscriptionDto(subscription),
                Statistics = stats
            });
    }
}
```

## Pipeline Behaviors

### Validation Behavior

```csharp
public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    where TResponse : IResult
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    
    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }
    
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
            return await next();
            
        var context = new ValidationContext<TRequest>(request);
        
        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken)));
            
        var failures = validationResults
            .SelectMany(r => r.Errors)
            .Where(f => f != null)
            .ToList();
            
        if (failures.Any())
        {
            var errors = failures
                .Select(f => new ValidationError(f.PropertyName, f.ErrorMessage))
                .ToList();
                
            var error = errors.Count == 1
                ? errors.First()
                : new AggregateError("Validation failed", errors);
                
            // Create appropriate Result type
            if (typeof(TResponse).IsGenericType)
            {
                var resultType = typeof(TResponse).GetGenericTypeDefinition();
                if (resultType == typeof(Result<>))
                {
                    var failureMethod = typeof(Result)
                        .GetMethod(nameof(Result.Failure))
                        .MakeGenericMethod(typeof(TResponse).GetGenericArguments()[0]);
                        
                    return (TResponse)failureMethod.Invoke(null, new object[] { error });
                }
            }
            else if (typeof(TResponse) == typeof(Result))
            {
                return (TResponse)(object)Result.Failure(error);
            }
        }
        
        return await next();
    }
}
```

### Logging Behavior

```csharp
public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
    private readonly ICurrentUserService _currentUser;
    
    public LoggingBehavior(
        ILogger<LoggingBehavior<TRequest, TResponse>> logger,
        ICurrentUserService currentUser)
    {
        _logger = logger;
        _currentUser = currentUser;
    }
    
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var userId = _currentUser.UserId;
        var userName = _currentUser.UserName;
        
        _logger.LogInformation(
            "Handling {RequestName} for user {UserId} ({UserName})",
            requestName, userId, userName);
            
        try
        {
            var response = await next();
            
            // Log result if it's a Result type
            if (response is IResult result)
            {
                if (result.IsFailure)
                {
                    _logger.LogWarning(
                        "Request {RequestName} failed with error: {Error}",
                        requestName, result.Error);
                }
                else
                {
                    _logger.LogInformation(
                        "Request {RequestName} completed successfully",
                        requestName);
                }
            }
            
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, 
                "Request {RequestName} failed with exception",
                requestName);
            throw;
        }
    }
}
```

### Transaction Behavior

```csharp
public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    where TResponse : IResult
{
    private readonly IDbContext _dbContext;
    private readonly ILogger<TransactionBehavior<TRequest, TResponse>> _logger;
    
    public TransactionBehavior(IDbContext dbContext, ILogger<TransactionBehavior<TRequest, TResponse>> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        // Skip if not a command
        if (request is not ICommand)
            return await next();
            
        // Skip if already in transaction
        if (_dbContext.HasActiveTransaction)
            return await next();
            
        var strategy = _dbContext.Database.CreateExecutionStrategy();
        
        return await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await _dbContext.BeginTransactionAsync();
            
            try
            {
                _logger.LogDebug("Beginning transaction for {Request}", typeof(TRequest).Name);
                
                var response = await next();
                
                if (response.IsSuccess)
                {
                    await _dbContext.CommitTransactionAsync(transaction);
                    _logger.LogDebug("Transaction committed");
                }
                else
                {
                    _logger.LogDebug("Transaction rolled back due to failure");
                }
                
                return response;
            }
            catch (Exception)
            {
                _logger.LogDebug("Transaction rolled back due to exception");
                throw;
            }
        });
    }
}
```

### Performance Behavior

```csharp
public class PerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<PerformanceBehavior<TRequest, TResponse>> _logger;
    private readonly IMetrics _metrics;
    
    public PerformanceBehavior(
        ILogger<PerformanceBehavior<TRequest, TResponse>> logger,
        IMetrics metrics)
    {
        _logger = logger;
        _metrics = metrics;
    }
    
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var timer = Stopwatch.StartNew();
        var requestName = typeof(TRequest).Name;
        
        try
        {
            var response = await next();
            
            timer.Stop();
            
            var elapsedMilliseconds = timer.ElapsedMilliseconds;
            
            if (elapsedMilliseconds > 500)
            {
                _logger.LogWarning(
                    "Long running request: {RequestName} ({ElapsedMilliseconds}ms)",
                    requestName, elapsedMilliseconds);
            }
            
            _metrics.RecordRequestDuration(requestName, elapsedMilliseconds);
            
            if (response is IResult result)
            {
                _metrics.RecordRequestResult(requestName, result.IsSuccess);
            }
            
            return response;
        }
        catch (Exception)
        {
            timer.Stop();
            _metrics.RecordRequestDuration(requestName, timer.ElapsedMilliseconds);
            _metrics.RecordRequestResult(requestName, false);
            throw;
        }
    }
}
```

## Validation

### FluentValidation with Result

```csharp
public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    private readonly IUserRepository _userRepository;
    
    public CreateUserCommandValidator(IUserRepository userRepository)
    {
        _userRepository = userRepository;
        
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format")
            .MustAsync(BeUniqueEmail).WithMessage("Email already exists");
            
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters")
            .Matches(@"[A-Z]").WithMessage("Password must contain uppercase letter")
            .Matches(@"[a-z]").WithMessage("Password must contain lowercase letter")
            .Matches(@"[0-9]").WithMessage("Password must contain digit");
            
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(50).WithMessage("First name too long");
            
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(50).WithMessage("Last name too long");
    }
    
    private async Task<bool> BeUniqueEmail(string email, CancellationToken cancellationToken)
    {
        var result = await _userRepository.FindByEmailAsync(email, cancellationToken);
        return result.IsNone;
    }
}
```

### Complex Validation Rules

```csharp
public class ProcessOrderCommandValidator : AbstractValidator<ProcessOrderCommand>
{
    private readonly IInventoryService _inventoryService;
    private readonly ICreditService _creditService;
    
    public ProcessOrderCommandValidator(
        IInventoryService inventoryService,
        ICreditService creditService)
    {
        _inventoryService = inventoryService;
        _creditService = creditService;
        
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("Customer ID is required");
            
        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("Order must have items")
            .ForEach(item => item.SetValidator(new OrderItemValidator()));
            
        RuleFor(x => x)
            .MustAsync(HaveSufficientInventory)
            .WithMessage("Insufficient inventory for one or more items")
            .MustAsync(HaveSufficientCredit)
            .WithMessage("Insufficient credit for order");
    }
    
    private async Task<bool> HaveSufficientInventory(
        ProcessOrderCommand command,
        CancellationToken cancellationToken)
    {
        foreach (var item in command.Items)
        {
            var result = await _inventoryService.CheckAvailabilityAsync(
                item.ProductId,
                item.Quantity,
                cancellationToken);
                
            if (result.IsFailure || !result.Value)
                return false;
        }
        
        return true;
    }
    
    private async Task<bool> HaveSufficientCredit(
        ProcessOrderCommand command,
        CancellationToken cancellationToken)
    {
        var total = command.Items.Sum(i => i.Quantity * i.Price);
        var result = await _creditService.CheckCreditAsync(
            command.CustomerId,
            total,
            cancellationToken);
            
        return result.IsSuccess && result.Value;
    }
}
```

## Notifications

### Domain Events with Result

```csharp
public record UserCreatedNotification(Guid UserId, string Email) : INotification;

public class SendWelcomeEmailHandler : INotificationHandler<UserCreatedNotification>
{
    private readonly IEmailService _emailService;
    private readonly ILogger<SendWelcomeEmailHandler> _logger;
    
    public async Task Handle(
        UserCreatedNotification notification,
        CancellationToken cancellationToken)
    {
        var result = await _emailService.SendWelcomeEmailAsync(
            notification.Email,
            cancellationToken);
            
        result.Match(
            onSuccess: () => _logger.LogInformation(
                "Welcome email sent to {Email}",
                notification.Email),
            onFailure: error => _logger.LogWarning(
                "Failed to send welcome email to {Email}: {Error}",
                notification.Email,
                error.Message)
        );
    }
}

public class UpdateUserStatisticsHandler : INotificationHandler<UserCreatedNotification>
{
    private readonly IStatisticsService _statisticsService;
    
    public async Task Handle(
        UserCreatedNotification notification,
        CancellationToken cancellationToken)
    {
        await _statisticsService.IncrementUserCountAsync(cancellationToken);
    }
}
```

### Publishing Events from Handlers

```csharp
public class CreateUserWithNotificationHandler : ICommandHandler<CreateUserCommand, UserDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    
    public async Task<Result<UserDto>> Handle(
        CreateUserCommand request,
        CancellationToken cancellationToken)
    {
        // Create user
        var userResult = await CreateUserAsync(request, cancellationToken);
        
        // Publish notification if successful
        await userResult.TapAsync(async user =>
        {
            await _mediator.Publish(
                new UserCreatedNotification(user.Id, user.Email),
                cancellationToken);
        });
        
        return userResult.Map(user => _mapper.Map<UserDto>(user));
    }
}
```

## Error Handling

### Global Exception Handler

```csharp
public class GlobalExceptionHandlerBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    where TResponse : IResult, new()
{
    private readonly ILogger<GlobalExceptionHandlerBehavior<TRequest, TResponse>> _logger;
    
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, 
                "Unhandled exception for request {RequestName}",
                typeof(TRequest).Name);
                
            var error = ConvertExceptionToError(ex);
            
            // Create failure result
            if (typeof(TResponse).IsGenericType)
            {
                var resultType = typeof(TResponse).GetGenericTypeDefinition();
                if (resultType == typeof(Result<>))
                {
                    var failureMethod = typeof(Result)
                        .GetMethod(nameof(Result.Failure))
                        .MakeGenericMethod(typeof(TResponse).GetGenericArguments()[0]);
                        
                    return (TResponse)failureMethod.Invoke(null, new object[] { error });
                }
            }
            else if (typeof(TResponse) == typeof(Result))
            {
                return (TResponse)(object)Result.Failure(error);
            }
            
            throw;
        }
    }
    
    private Error ConvertExceptionToError(Exception exception)
    {
        return exception switch
        {
            ValidationException ve => new ValidationError(ve.Message),
            NotFoundException nfe => new NotFoundError(nfe.Message),
            UnauthorizedAccessException => new AuthorizationError("Access denied"),
            DbUpdateConcurrencyException => new ConflictError("The record was modified by another user"),
            _ => new Error("INTERNAL_ERROR", "An unexpected error occurred")
        };
    }
}
```

### Retry Behavior for Transient Failures

```csharp
public class RetryBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    where TResponse : IResult
{
    private readonly ILogger<RetryBehavior<TRequest, TResponse>> _logger;
    
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var retryPolicy = Policy
            .HandleResult<TResponse>(r => IsTransientFailure(r))
            .WaitAndRetryAsync(
                3,
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                onRetry: (outcome, timespan, retryCount, context) =>
                {
                    _logger.LogWarning(
                        "Retry {RetryCount} after {Delay}ms for {RequestName}",
                        retryCount,
                        timespan.TotalMilliseconds,
                        typeof(TRequest).Name);
                });
                
        return await retryPolicy.ExecuteAsync(async () => await next());
    }
    
    private bool IsTransientFailure(TResponse response)
    {
        if (!response.IsFailure)
            return false;
            
        return response.Error.Code switch
        {
            "NETWORK_ERROR" => true,
            "TIMEOUT" => true,
            "SERVICE_UNAVAILABLE" => true,
            _ => false
        };
    }
}
```

## Testing

### Handler Unit Tests

```csharp
public class CreateUserCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly CreateUserCommandHandler _handler;
    
    public CreateUserCommandHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _passwordHasherMock = new Mock<IPasswordHasher>();
        _mapperMock = new Mock<IMapper>();
        
        _handler = new CreateUserCommandHandler(
            _userRepositoryMock.Object,
            _passwordHasherMock.Object,
            _mapperMock.Object);
    }
    
    [Fact]
    public async Task Handle_WhenEmailDoesNotExist_ReturnsSuccess()
    {
        // Arrange
        var command = new CreateUserCommand(
            "test@example.com",
            "password123",
            "John",
            "Doe");
            
        _userRepositoryMock
            .Setup(x => x.FindByEmailAsync(command.Email))
            .ReturnsAsync(Option.None<User>());
            
        _passwordHasherMock
            .Setup(x => x.HashPassword(command.Password))
            .Returns("hashed_password");
            
        var createdUser = new User { Id = Guid.NewGuid() };
        _userRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(createdUser));
            
        var userDto = new UserDto { Id = createdUser.Id };
        _mapperMock
            .Setup(x => x.Map<UserDto>(createdUser))
            .Returns(userDto);
        
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.Should().BeSuccess();
        result.Value.Should().Be(userDto);
    }
    
    [Fact]
    public async Task Handle_WhenEmailExists_ReturnsConflictError()
    {
        // Arrange
        var command = new CreateUserCommand(
            "existing@example.com",
            "password123",
            "John",
            "Doe");
            
        _userRepositoryMock
            .Setup(x => x.FindByEmailAsync(command.Email))
            .ReturnsAsync(Option.Some(new User()));
        
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.Should().BeFailure();
        result.Error.Should().BeOfType<ConflictError>();
        result.Error.Message.Should().Contain("Email already exists");
    }
}
```

### Integration Tests

```csharp
public class UserCommandIntegrationTests : IClassFixture<TestFixture>
{
    private readonly IMediator _mediator;
    private readonly TestDbContext _dbContext;
    
    public UserCommandIntegrationTests(TestFixture fixture)
    {
        _mediator = fixture.ServiceProvider.GetRequiredService<IMediator>();
        _dbContext = fixture.ServiceProvider.GetRequiredService<TestDbContext>();
    }
    
    [Fact]
    public async Task CreateUser_EndToEnd_Success()
    {
        // Arrange
        var command = new CreateUserCommand(
            $"test_{Guid.NewGuid()}@example.com",
            "Password123!",
            "Integration",
            "Test");
        
        // Act
        var result = await _mediator.Send(command);
        
        // Assert
        result.Should().BeSuccess();
        result.Value.Email.Should().Be(command.Email);
        
        // Verify in database
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Email == command.Email);
        user.Should().NotBeNull();
        user.FirstName.Should().Be(command.FirstName);
    }
    
    [Fact]
    public async Task CreateUser_WithValidationError_ReturnsFailure()
    {
        // Arrange
        var command = new CreateUserCommand(
            "invalid-email",
            "short",
            "",
            "");
        
        // Act
        var result = await _mediator.Send(command);
        
        // Assert
        result.Should().BeFailure();
        result.Error.Should().BeOfType<AggregateError>();
        
        var aggregateError = (AggregateError)result.Error;
        aggregateError.Errors.Should().Contain(e => e.Message.Contains("email"));
        aggregateError.Errors.Should().Contain(e => e.Message.Contains("password"));
        aggregateError.Errors.Should().Contain(e => e.Message.Contains("First name"));
    }
}
```

### Behavior Tests

```csharp
public class ValidationBehaviorTests
{
    [Fact]
    public async Task Handle_WithValidRequest_CallsNext()
    {
        // Arrange
        var validators = new List<IValidator<TestCommand>>();
        var behavior = new ValidationBehavior<TestCommand, Result<string>>(validators);
        
        var request = new TestCommand("valid");
        var expectedResponse = Result.Success("success");
        
        Task<Result<string>> Next() => Task.FromResult(expectedResponse);
        
        // Act
        var response = await behavior.Handle(request, Next, CancellationToken.None);
        
        // Assert
        response.Should().Be(expectedResponse);
    }
    
    [Fact]
    public async Task Handle_WithInvalidRequest_ReturnsFailure()
    {
        // Arrange
        var validator = new Mock<IValidator<TestCommand>>();
        validator
            .Setup(x => x.ValidateAsync(It.IsAny<ValidationContext<TestCommand>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(new[]
            {
                new ValidationFailure("Property", "Error message")
            }));
            
        var validators = new List<IValidator<TestCommand>> { validator.Object };
        var behavior = new ValidationBehavior<TestCommand, Result<string>>(validators);
        
        var request = new TestCommand("invalid");
        
        // Act
        var response = await behavior.Handle(
            request, 
            () => Task.FromResult(Result.Success("should not be called")), 
            CancellationToken.None);
        
        // Assert
        response.Should().BeFailure();
        response.Error.Should().BeOfType<ValidationError>();
        response.Error.Message.Should().Be("Error message");
    }
}
```

## Advanced Patterns

### Saga Pattern with Result

```csharp
public class OrderSaga
{
    private readonly IMediator _mediator;
    private readonly ILogger<OrderSaga> _logger;
    
    public OrderSaga(IMediator mediator, ILogger<OrderSaga> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }
    
    public async Task<Result<OrderCompletedDto>> ProcessOrderAsync(
        ProcessOrderRequest request,
        CancellationToken cancellationToken)
    {
        var sagaId = Guid.NewGuid();
        _logger.LogInformation("Starting order saga {SagaId}", sagaId);
        
        // Step 1: Validate order
        var validationResult = await _mediator.Send(
            new ValidateOrderCommand(request),
            cancellationToken);
            
        if (validationResult.IsFailure)
            return Result.Failure<OrderCompletedDto>(validationResult.Error);
        
        // Step 2: Reserve inventory
        var reservationResult = await _mediator.Send(
            new ReserveInventoryCommand(request.Items),
            cancellationToken);
            
        if (reservationResult.IsFailure)
        {
            return Result.Failure<OrderCompletedDto>(reservationResult.Error);
        }
        
        // Step 3: Process payment
        var paymentResult = await _mediator.Send(
            new ProcessPaymentCommand(request.CustomerId, request.Total),
            cancellationToken);
            
        if (paymentResult.IsFailure)
        {
            // Compensate: Release inventory
            await _mediator.Send(
                new ReleaseInventoryCommand(reservationResult.Value),
                cancellationToken);
                
            return Result.Failure<OrderCompletedDto>(paymentResult.Error);
        }
        
        // Step 4: Create order
        var orderResult = await _mediator.Send(
            new CreateOrderCommand(request, reservationResult.Value, paymentResult.Value),
            cancellationToken);
            
        if (orderResult.IsFailure)
        {
            // Compensate: Refund payment and release inventory
            await Task.WhenAll(
                _mediator.Send(new RefundPaymentCommand(paymentResult.Value), cancellationToken),
                _mediator.Send(new ReleaseInventoryCommand(reservationResult.Value), cancellationToken)
            );
            
            return Result.Failure<OrderCompletedDto>(orderResult.Error);
        }
        
        _logger.LogInformation("Order saga {SagaId} completed successfully", sagaId);
        
        return Result.Success(new OrderCompletedDto(orderResult.Value));
    }
}
```

### Event Sourcing with Result

```csharp
public abstract class AggregateRoot
{
    private readonly List<IDomainEvent> _events = new();
    
    public IReadOnlyList<IDomainEvent> Events => _events;
    public Guid Id { get; protected set; }
    public int Version { get; protected set; }
    
    protected void AddEvent(IDomainEvent @event)
    {
        _events.Add(@event);
    }
    
    public void ClearEvents()
    {
        _events.Clear();
    }
}

public class Order : AggregateRoot
{
    public OrderStatus Status { get; private set; }
    public Guid CustomerId { get; private set; }
    public decimal Total { get; private set; }
    
    private Order() { } // For EF
    
    public static Result<Order> Create(Guid customerId, List<OrderItem> items)
    {
        if (customerId == Guid.Empty)
            return new ValidationError("Customer ID is required");
            
        if (!items?.Any() ?? true)
            return new ValidationError("Order must have items");
            
        var order = new Order
        {
            Id = Guid.NewGuid(),
            CustomerId = customerId,
            Status = OrderStatus.Pending,
            Total = items.Sum(i => i.Quantity * i.Price)
        };
        
        order.AddEvent(new OrderCreatedEvent(order.Id, customerId, items));
        
        return Result.Success(order);
    }
    
    public Result Ship()
    {
        if (Status != OrderStatus.Paid)
            return new ConflictError("Order must be paid before shipping");
            
        Status = OrderStatus.Shipped;
        AddEvent(new OrderShippedEvent(Id, DateTime.UtcNow));
        
        return Result.Success();
    }
}

public class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, Guid>
{
    private readonly IEventStore _eventStore;
    private readonly IMediator _mediator;
    
    public async Task<Result<Guid>> Handle(
        CreateOrderCommand request,
        CancellationToken cancellationToken)
    {
        var orderResult = Order.Create(request.CustomerId, request.Items);
        
        if (orderResult.IsFailure)
            return Result.Failure<Guid>(orderResult.Error);
            
        var order = orderResult.Value;
        
        // Save events
        await _eventStore.SaveEventsAsync(
            order.Id,
            order.Events,
            order.Version,
            cancellationToken);
        
        // Publish events
        foreach (var @event in order.Events)
        {
            await _mediator.Publish(@event, cancellationToken);
        }
        
        return Result.Success(order.Id);
    }
}
```

## Best Practices

### 1. Keep Handlers Focused

```csharp
// Good - Single responsibility
public class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, UserDto>
{
    public async Task<Result<UserDto>> Handle(
        GetUserByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _userRepository
            .GetByIdAsync(request.UserId)
            .MapAsync(user => _mapper.Map<UserDto>(user));
    }
}

// Avoid - Doing too much
public class ComplexHandler : ICommandHandler<ComplexCommand, ComplexResult>
{
    public async Task<Result<ComplexResult>> Handle(
        ComplexCommand request,
        CancellationToken cancellationToken)
    {
        // Validation logic
        // Business logic
        // Data access
        // Event publishing
        // Notification sending
        // etc...
    }
}
```

### 2. Use Appropriate Return Types

```csharp
// Commands that modify state
public interface ICommand : IRequest<Result> { }
public interface ICommand<TResponse> : IRequest<Result<TResponse>> { }

// Queries that read state
public interface IQuery<TResponse> : IRequest<Result<TResponse>> { }

// Queries that may return nothing
public interface IOptionQuery<TResponse> : IRequest<Option<TResponse>> { }
```

### 3. Consistent Error Handling

```csharp
public abstract class HandlerBase
{
    protected Result<T> HandleError<T>(Exception ex, string operation)
    {
        return ex switch
        {
            ValidationException ve => new ValidationError(ve.Message),
            NotFoundException => new NotFoundError($"{operation} failed: not found"),
            UnauthorizedAccessException => new AuthorizationError("Access denied"),
            _ => new Error("OPERATION_FAILED", $"{operation} failed: {ex.Message}")
        };
    }
}
```

### 4. Test Behaviors Independently

```csharp
// Test each behavior in isolation
[Fact]
public async Task ValidationBehavior_Should_Validate_Request()
{
    // Test validation behavior separately
}

[Fact]
public async Task TransactionBehavior_Should_Rollback_On_Failure()
{
    // Test transaction behavior separately
}

[Fact]
public async Task Handler_Should_Execute_Business_Logic()
{
    // Test handler without behaviors
}
```

### 5. Use Decorators for Cross-Cutting Concerns

```csharp
// Instead of putting everything in handlers
public class CachedQueryDecorator<TQuery, TResponse> : IQueryHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{
    private readonly IQueryHandler<TQuery, TResponse> _inner;
    private readonly IMemoryCache _cache;
    
    public async Task<Result<TResponse>> Handle(
        TQuery request,
        CancellationToken cancellationToken)
    {
        var cacheKey = GetCacheKey(request);
        
        if (_cache.TryGetValue<Result<TResponse>>(cacheKey, out var cached))
            return cached;
            
        var result = await _inner.Handle(request, cancellationToken);
        
        if (result.IsSuccess)
        {
            _cache.Set(cacheKey, result, TimeSpan.FromMinutes(5));
        }
        
        return result;
    }
}
```

## Summary

Integrating FluentUnions with MediatR provides:

1. **Clean architecture** - Clear separation of concerns with CQRS
2. **Consistent error handling** - Result types throughout the pipeline
3. **Composable behaviors** - Validation, logging, transactions as pipeline
4. **Type-safe operations** - Commands and queries with explicit results
5. **Testable handlers** - Easy to unit test in isolation

Key benefits:
- Pipeline behaviors handle cross-cutting concerns
- Validation integrated with FluentValidation
- Transaction management simplified
- Performance monitoring built-in
- Event-driven capabilities with notifications

Next steps:
- [Entity Framework Integration](entity-framework.md)
- [Testing Guide](../guides/testing-guide.md)
- [CQRS Pattern](../patterns/service-patterns.md)
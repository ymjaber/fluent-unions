# Dependency Injection Integration

This guide demonstrates how to integrate FluentUnions with dependency injection containers and patterns in .NET applications.

## Table of Contents
1. [Microsoft.Extensions.DependencyInjection](#microsoftextensionsdependencyinjection)
2. [Service Registration Patterns](#service-registration-patterns)
3. [Factory Patterns](#factory-patterns)
4. [Decorator Pattern](#decorator-pattern)
5. [Service Lifetimes](#service-lifetimes)
6. [Configuration and Options](#configuration-and-options)
7. [Testing with DI](#testing-with-di)
8. [Advanced Scenarios](#advanced-scenarios)
9. [Third-Party Containers](#third-party-containers)
10. [Best Practices](#best-practices)

## Microsoft.Extensions.DependencyInjection

### Basic Setup

```csharp
using Microsoft.Extensions.DependencyInjection;
using FluentUnions;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        // Register FluentUnions services
        builder.Services.AddFluentUnions();
        
        // Register application services
        builder.Services.AddApplicationServices();
        
        var app = builder.Build();
        app.Run();
    }
}

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFluentUnions(this IServiceCollection services)
    {
        // Register error handlers
        services.AddSingleton<IErrorHandler, DefaultErrorHandler>();
        services.AddSingleton<IErrorFactory, ErrorFactory>();
        
        // Register Result handlers
        services.AddScoped(typeof(IResultHandler<>), typeof(DefaultResultHandler<>));
        
        // Register Option handlers
        services.AddScoped(typeof(IOptionHandler<>), typeof(DefaultOptionHandler<>));
        
        return services;
    }
    
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register repositories
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        
        // Register services
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IOrderService, OrderService>();
        
        // Register validators
        services.AddScoped<IValidator<User>, UserValidator>();
        
        return services;
    }
}
```

### Service Interfaces with Result

```csharp
public interface IUserService
{
    Task<Result<User>> GetUserAsync(Guid id);
    Task<Result<User>> CreateUserAsync(CreateUserRequest request);
    Task<Result<User>> UpdateUserAsync(UpdateUserRequest request);
    Task<Result> DeleteUserAsync(Guid id);
    Task<Option<User>> FindUserByEmailAsync(string email);
}

public interface IEmailService
{
    Task<Result> SendEmailAsync(EmailMessage message);
    Task<Result<EmailStatus>> GetEmailStatusAsync(Guid emailId);
}

public interface IValidationService<T>
{
    Result<T> Validate(T entity);
    Task<Result<T>> ValidateAsync(T entity);
}
```

## Service Registration Patterns

### Scanning and Auto-Registration

```csharp
public static class ServiceRegistrationExtensions
{
    public static IServiceCollection AddServicesFromAssembly(
        this IServiceCollection services,
        Assembly assembly,
        ServiceLifetime lifetime = ServiceLifetime.Scoped)
    {
        // Register all services implementing IService
        var serviceTypes = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces().Any(i => i.Name.EndsWith("Service")))
            .ToList();
            
        foreach (var implementationType in serviceTypes)
        {
            var interfaceType = implementationType.GetInterfaces()
                .FirstOrDefault(i => i.Name == $"I{implementationType.Name}");
                
            if (interfaceType != null)
            {
                services.Add(new ServiceDescriptor(interfaceType, implementationType, lifetime));
            }
        }
        
        return services;
    }
    
    public static IServiceCollection AddResultHandlers(
        this IServiceCollection services,
        Assembly assembly)
    {
        // Register all IResultHandler<T> implementations
        var handlerTypes = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && 
                   t.GetInterfaces().Any(i => i.IsGenericType && 
                   i.GetGenericTypeDefinition() == typeof(IResultHandler<>)))
            .ToList();
            
        foreach (var handlerType in handlerTypes)
        {
            var interfaces = handlerType.GetInterfaces()
                .Where(i => i.IsGenericType && 
                       i.GetGenericTypeDefinition() == typeof(IResultHandler<>));
                       
            foreach (var interfaceType in interfaces)
            {
                services.AddScoped(interfaceType, handlerType);
            }
        }
        
        return services;
    }
}
```

### Conditional Registration

```csharp
public static class ConditionalRegistrationExtensions
{
    public static IServiceCollection AddConditionalServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Register based on configuration
        if (configuration.GetValue<bool>("Features:EnableCaching"))
        {
            services.AddScoped<IUserService, CachedUserService>();
        }
        else
        {
            services.AddScoped<IUserService, UserService>();
        }
        
        // Register based on environment
        services.AddScoped<IEmailService>(provider =>
        {
            var env = provider.GetRequiredService<IWebHostEnvironment>();
            
            return env.IsDevelopment()
                ? new MockEmailService()
                : new SmtpEmailService(
                    provider.GetRequiredService<IOptions<SmtpOptions>>(),
                    provider.GetRequiredService<ILogger<SmtpEmailService>>());
        });
        
        return services;
    }
    
    public static IServiceCollection AddResultBasedServices<TService, TImplementation>(
        this IServiceCollection services,
        Func<IServiceProvider, Result<TImplementation>> factory)
        where TImplementation : class, TService
        where TService : class
    {
        services.AddScoped<TService>(provider =>
        {
            var result = factory(provider);
            
            if (result.IsFailure)
            {
                throw new InvalidOperationException(
                    $"Failed to create service {typeof(TService).Name}: {result.Error.Message}");
            }
            
            return result.Value;
        });
        
        return services;
    }
}
```

### Keyed Services (.NET 8+)

```csharp
public interface IPaymentProcessor
{
    Task<Result<PaymentResult>> ProcessPaymentAsync(PaymentRequest request);
}

// Registration
builder.Services.AddKeyedScoped<IPaymentProcessor, StripePaymentProcessor>("stripe");
builder.Services.AddKeyedScoped<IPaymentProcessor, PayPalPaymentProcessor>("paypal");
builder.Services.AddKeyedScoped<IPaymentProcessor, CryptoPaymentProcessor>("crypto");

// Usage with Result
public class PaymentService
{
    private readonly IServiceProvider _serviceProvider;
    
    public PaymentService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public async Task<Result<PaymentResult>> ProcessPaymentAsync(
        PaymentMethod method,
        PaymentRequest request)
    {
        var processorKey = method switch
        {
            PaymentMethod.CreditCard => "stripe",
            PaymentMethod.PayPal => "paypal",
            PaymentMethod.Bitcoin => "crypto",
            _ => null
        };
        
        if (processorKey == null)
            return new ValidationError($"Unsupported payment method: {method}");
            
        var processor = _serviceProvider.GetKeyedService<IPaymentProcessor>(processorKey);
        
        if (processor == null)
            return new Error("PROCESSOR_NOT_FOUND", $"Payment processor for {method} not found");
            
        return await processor.ProcessPaymentAsync(request);
    }
}
```

## Factory Patterns

### Result-Based Factory

```csharp
public interface IServiceFactory<T>
{
    Result<T> Create(string key);
    Option<T> TryCreate(string key);
}

public class ServiceFactory<T> : IServiceFactory<T> where T : class
{
    private readonly IServiceProvider _serviceProvider;
    private readonly Dictionary<string, Type> _typeMap;
    
    public ServiceFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _typeMap = DiscoverTypes();
    }
    
    public Result<T> Create(string key)
    {
        if (!_typeMap.TryGetValue(key, out var type))
            return new NotFoundError($"Service type for key '{key}' not found");
            
        try
        {
            var service = (T)_serviceProvider.GetRequiredService(type);
            return Result.Success(service);
        }
        catch (Exception ex)
        {
            return new Error("FACTORY_ERROR", $"Failed to create service: {ex.Message}");
        }
    }
    
    public Option<T> TryCreate(string key)
    {
        if (!_typeMap.TryGetValue(key, out var type))
            return Option.None<T>();
            
        var service = _serviceProvider.GetService(type) as T;
        return Option.FromNullable(service);
    }
    
    private Dictionary<string, Type> DiscoverTypes()
    {
        var baseType = typeof(T);
        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => baseType.IsAssignableFrom(t) && !t.IsAbstract && t.IsClass)
            .ToDictionary(t => t.Name.Replace(baseType.Name.TrimStart('I'), ""), t => t);
    }
}

// Registration
services.AddSingleton(typeof(IServiceFactory<>), typeof(ServiceFactory<>));

// Usage
public class NotificationService
{
    private readonly IServiceFactory<INotificationHandler> _handlerFactory;
    
    public async Task<Result> SendNotificationAsync(Notification notification)
    {
        return await _handlerFactory.Create(notification.Type)
            .BindAsync(handler => handler.SendAsync(notification));
    }
}
```

### Abstract Factory with Result

```csharp
public interface IRepositoryFactory
{
    Result<IRepository<T>> CreateRepository<T>() where T : Entity;
    Result<IRepository> CreateRepository(Type entityType);
}

public class RepositoryFactory : IRepositoryFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
    
    public RepositoryFactory(
        IServiceProvider serviceProvider,
        IDbContextFactory<ApplicationDbContext> contextFactory)
    {
        _serviceProvider = serviceProvider;
        _contextFactory = contextFactory;
    }
    
    public Result<IRepository<T>> CreateRepository<T>() where T : Entity
    {
        try
        {
            var context = _contextFactory.CreateDbContext();
            var logger = _serviceProvider.GetRequiredService<ILogger<Repository<T>>>();
            
            var repository = new Repository<T>(context, logger);
            return Result.Success<IRepository<T>>(repository);
        }
        catch (Exception ex)
        {
            return new Error("REPOSITORY_CREATION_FAILED", ex.Message);
        }
    }
    
    public Result<IRepository> CreateRepository(Type entityType)
    {
        if (!typeof(Entity).IsAssignableFrom(entityType))
            return new ValidationError($"Type {entityType.Name} is not an Entity");
            
        var createMethod = GetType()
            .GetMethod(nameof(CreateRepository), Type.EmptyTypes)
            .MakeGenericMethod(entityType);
            
        var result = createMethod.Invoke(this, null);
        
        // Convert Result<IRepository<T>> to Result<IRepository>
        var resultType = result.GetType();
        if (resultType.GetProperty("IsSuccess").GetValue(result) is bool isSuccess && isSuccess)
        {
            var value = resultType.GetProperty("Value").GetValue(result) as IRepository;
            return Result.Success(value);
        }
        else
        {
            var error = resultType.GetProperty("Error").GetValue(result) as Error;
            return Result.Failure<IRepository>(error);
        }
    }
}
```

## Decorator Pattern

### Service Decorators with Result

```csharp
public interface ICachingDecorator<T> where T : class
{
    Result<TResult> Execute<TResult>(
        string cacheKey,
        Func<Result<TResult>> operation,
        TimeSpan? expiration = null);
        
    Task<Result<TResult>> ExecuteAsync<TResult>(
        string cacheKey,
        Func<Task<Result<TResult>>> operation,
        TimeSpan? expiration = null);
}

public class CachingDecorator<T> : ICachingDecorator<T> where T : class
{
    private readonly IMemoryCache _cache;
    private readonly ILogger<CachingDecorator<T>> _logger;
    
    public CachingDecorator(IMemoryCache cache, ILogger<CachingDecorator<T>> logger)
    {
        _cache = cache;
        _logger = logger;
    }
    
    public Result<TResult> Execute<TResult>(
        string cacheKey,
        Func<Result<TResult>> operation,
        TimeSpan? expiration = null)
    {
        if (_cache.TryGetValue<TResult>(cacheKey, out var cachedValue))
        {
            _logger.LogDebug("Cache hit for key: {CacheKey}", cacheKey);
            return Result.Success(cachedValue);
        }
        
        var result = operation();
        
        if (result.IsSuccess)
        {
            var cacheOptions = new MemoryCacheEntryOptions();
            if (expiration.HasValue)
                cacheOptions.SetAbsoluteExpiration(expiration.Value);
                
            _cache.Set(cacheKey, result.Value, cacheOptions);
            _logger.LogDebug("Cached result for key: {CacheKey}", cacheKey);
        }
        
        return result;
    }
    
    public async Task<Result<TResult>> ExecuteAsync<TResult>(
        string cacheKey,
        Func<Task<Result<TResult>>> operation,
        TimeSpan? expiration = null)
    {
        if (_cache.TryGetValue<TResult>(cacheKey, out var cachedValue))
        {
            _logger.LogDebug("Cache hit for key: {CacheKey}", cacheKey);
            return Result.Success(cachedValue);
        }
        
        var result = await operation();
        
        if (result.IsSuccess)
        {
            var cacheOptions = new MemoryCacheEntryOptions();
            if (expiration.HasValue)
                cacheOptions.SetAbsoluteExpiration(expiration.Value);
                
            _cache.Set(cacheKey, result.Value, cacheOptions);
            _logger.LogDebug("Cached result for key: {CacheKey}", cacheKey);
        }
        
        return result;
    }
}

// Registration
services.AddMemoryCache();
services.AddSingleton(typeof(ICachingDecorator<>), typeof(CachingDecorator<>));

// Cached service implementation
public class CachedUserService : IUserService
{
    private readonly IUserService _innerService;
    private readonly ICachingDecorator<IUserService> _cache;
    
    public CachedUserService(
        UserService innerService, // Note: concrete type for inner service
        ICachingDecorator<IUserService> cache)
    {
        _innerService = innerService;
        _cache = cache;
    }
    
    public Task<Result<User>> GetUserAsync(Guid id)
    {
        var cacheKey = $"user:{id}";
        return _cache.ExecuteAsync(
            cacheKey,
            () => _innerService.GetUserAsync(id),
            TimeSpan.FromMinutes(5));
    }
}
```

### Logging Decorator

```csharp
public class LoggingDecorator<TService> : DispatchProxy where TService : class
{
    private TService _decorated;
    private ILogger<TService> _logger;
    
    protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
    {
        var methodName = targetMethod?.Name ?? "Unknown";
        var stopwatch = Stopwatch.StartNew();
        
        try
        {
            _logger.LogDebug("Executing {Method} with args: {@Args}", methodName, args);
            
            var result = targetMethod.Invoke(_decorated, args);
            
            // Handle Result types
            if (result is IResult resultType)
            {
                if (resultType.IsFailure)
                {
                    _logger.LogWarning(
                        "Method {Method} failed with error: {Error}",
                        methodName,
                        resultType.Error);
                }
                else
                {
                    _logger.LogDebug(
                        "Method {Method} completed successfully in {ElapsedMs}ms",
                        methodName,
                        stopwatch.ElapsedMilliseconds);
                }
            }
            
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Method {Method} threw exception", methodName);
            throw;
        }
    }
    
    public static TService Create(TService decorated, ILogger<TService> logger)
    {
        var proxy = Create<TService, LoggingDecorator<TService>>() as LoggingDecorator<TService>;
        proxy._decorated = decorated;
        proxy._logger = logger;
        return proxy as TService;
    }
}

// Registration helper
public static class DecoratorRegistrationExtensions
{
    public static IServiceCollection AddLoggingDecorator<TService, TImplementation>(
        this IServiceCollection services)
        where TService : class
        where TImplementation : class, TService
    {
        services.AddScoped<TImplementation>();
        services.AddScoped<TService>(provider =>
        {
            var implementation = provider.GetRequiredService<TImplementation>();
            var logger = provider.GetRequiredService<ILogger<TService>>();
            return LoggingDecorator<TService>.Create(implementation, logger);
        });
        
        return services;
    }
}
```

## Service Lifetimes

### Lifetime Management with Result

```csharp
public interface IServiceLifetimeManager
{
    Result<T> GetService<T>(ServiceLifetime lifetime) where T : class;
    Result RegisterService<TService, TImplementation>(ServiceLifetime lifetime)
        where TService : class
        where TImplementation : class, TService;
}

public class ServiceLifetimeManager : IServiceLifetimeManager
{
    private readonly IServiceCollection _services;
    private readonly IServiceProvider _serviceProvider;
    
    public ServiceLifetimeManager(
        IServiceCollection services,
        IServiceProvider serviceProvider)
    {
        _services = services;
        _serviceProvider = serviceProvider;
    }
    
    public Result<T> GetService<T>(ServiceLifetime lifetime) where T : class
    {
        try
        {
            var service = lifetime switch
            {
                ServiceLifetime.Singleton => _serviceProvider.GetRequiredService<T>(),
                ServiceLifetime.Scoped => GetScopedService<T>(),
                ServiceLifetime.Transient => ActivatorUtilities.CreateInstance<T>(_serviceProvider),
                _ => throw new ArgumentException($"Unknown lifetime: {lifetime}")
            };
            
            return Result.Success(service);
        }
        catch (Exception ex)
        {
            return new Error("SERVICE_RESOLUTION_FAILED", ex.Message);
        }
    }
    
    private T GetScopedService<T>() where T : class
    {
        using var scope = _serviceProvider.CreateScope();
        return scope.ServiceProvider.GetRequiredService<T>();
    }
    
    public Result RegisterService<TService, TImplementation>(ServiceLifetime lifetime)
        where TService : class
        where TImplementation : class, TService
    {
        try
        {
            var descriptor = new ServiceDescriptor(
                typeof(TService),
                typeof(TImplementation),
                lifetime);
                
            _services.Add(descriptor);
            
            return Result.Success();
        }
        catch (Exception ex)
        {
            return new Error("SERVICE_REGISTRATION_FAILED", ex.Message);
        }
    }
}
```

### Scoped Service with Result Context

```csharp
public interface IRequestContext
{
    Guid RequestId { get; }
    Option<User> CurrentUser { get; }
    Result<T> GetFeature<T>() where T : class;
    Result SetFeature<T>(T feature) where T : class;
}

public class RequestContext : IRequestContext
{
    private readonly Dictionary<Type, object> _features = new();
    
    public Guid RequestId { get; } = Guid.NewGuid();
    public Option<User> CurrentUser { get; set; } = Option.None<User>();
    
    public Result<T> GetFeature<T>() where T : class
    {
        if (_features.TryGetValue(typeof(T), out var feature))
        {
            return feature is T typedFeature
                ? Result.Success(typedFeature)
                : new Error("FEATURE_TYPE_MISMATCH", $"Feature is not of type {typeof(T).Name}");
        }
        
        return new NotFoundError($"Feature {typeof(T).Name} not found");
    }
    
    public Result SetFeature<T>(T feature) where T : class
    {
        if (feature == null)
            return new ValidationError("Feature cannot be null");
            
        _features[typeof(T)] = feature;
        return Result.Success();
    }
}

// Registration
services.AddScoped<IRequestContext, RequestContext>();

// Usage in middleware
public class RequestContextMiddleware
{
    private readonly RequestDelegate _next;
    
    public async Task InvokeAsync(HttpContext context, IRequestContext requestContext)
    {
        // Set current user if authenticated
        if (context.User.Identity.IsAuthenticated)
        {
            var userId = Guid.Parse(context.User.FindFirst("sub").Value);
            var userService = context.RequestServices.GetRequiredService<IUserService>();
            
            var userResult = await userService.GetUserAsync(userId);
            requestContext.CurrentUser = userResult.ToOption();
        }
        
        // Add request ID to response headers
        context.Response.Headers.Add("X-Request-ID", requestContext.RequestId.ToString());
        
        await _next(context);
    }
}
```

## Configuration and Options

### Options Pattern with Result

```csharp
public interface IOptionsValidator<TOptions> where TOptions : class
{
    Result<TOptions> Validate(TOptions options);
}

public class DatabaseOptionsValidator : IOptionsValidator<DatabaseOptions>
{
    public Result<DatabaseOptions> Validate(DatabaseOptions options)
    {
        var errors = new List<Error>();
        
        if (string.IsNullOrWhiteSpace(options.ConnectionString))
            errors.Add(new ValidationError("ConnectionString is required"));
            
        if (options.CommandTimeout < 0)
            errors.Add(new ValidationError("CommandTimeout must be non-negative"));
            
        if (options.MaxRetryCount < 0)
            errors.Add(new ValidationError("MaxRetryCount must be non-negative"));
            
        return errors.Any()
            ? Result.Failure<DatabaseOptions>(new AggregateError("Invalid database options", errors))
            : Result.Success(options);
    }
}

public class ValidatedOptions<TOptions> where TOptions : class
{
    private readonly IOptions<TOptions> _options;
    private readonly IOptionsValidator<TOptions> _validator;
    private readonly Lazy<Result<TOptions>> _validatedOptions;
    
    public ValidatedOptions(
        IOptions<TOptions> options,
        IOptionsValidator<TOptions> validator)
    {
        _options = options;
        _validator = validator;
        _validatedOptions = new Lazy<Result<TOptions>>(() => _validator.Validate(_options.Value));
    }
    
    public Result<TOptions> Value => _validatedOptions.Value;
}

// Registration
services.Configure<DatabaseOptions>(configuration.GetSection("Database"));
services.AddSingleton<IOptionsValidator<DatabaseOptions>, DatabaseOptionsValidator>();
services.AddSingleton<ValidatedOptions<DatabaseOptions>>();

// Usage
public class DatabaseService
{
    private readonly ValidatedOptions<DatabaseOptions> _options;
    
    public DatabaseService(ValidatedOptions<DatabaseOptions> options)
    {
        _options = options;
    }
    
    public async Task<Result<Connection>> GetConnectionAsync()
    {
        return await _options.Value
            .BindAsync(async options =>
            {
                try
                {
                    var connection = new Connection(options.ConnectionString);
                    await connection.OpenAsync();
                    return Result.Success(connection);
                }
                catch (Exception ex)
                {
                    return new Error("CONNECTION_FAILED", ex.Message);
                }
            });
    }
}
```

### Dynamic Configuration with Result

```csharp
public interface IConfigurationService
{
    Result<T> GetConfiguration<T>(string key) where T : class, new();
    Task<Result> ReloadConfigurationAsync();
    IObservable<Result<T>> WatchConfiguration<T>(string key) where T : class, new();
}

public class ConfigurationService : IConfigurationService
{
    private readonly IConfiguration _configuration;
    private readonly IOptionsMonitor<ConfigurationReloadOptions> _reloadOptions;
    private readonly Subject<string> _configurationChanged = new();
    
    public Result<T> GetConfiguration<T>(string key) where T : class, new()
    {
        try
        {
            var section = _configuration.GetSection(key);
            if (!section.Exists())
                return new NotFoundError($"Configuration section '{key}' not found");
                
            var config = new T();
            section.Bind(config);
            
            // Validate if T implements IValidatable
            if (config is IValidatable validatable)
            {
                var validationResult = validatable.Validate();
                if (validationResult.IsFailure)
                    return Result.Failure<T>(validationResult.Error);
            }
            
            return Result.Success(config);
        }
        catch (Exception ex)
        {
            return new Error("CONFIG_ERROR", $"Failed to load configuration: {ex.Message}");
        }
    }
    
    public IObservable<Result<T>> WatchConfiguration<T>(string key) where T : class, new()
    {
        return _configurationChanged
            .Where(changedKey => changedKey == key)
            .Select(_ => GetConfiguration<T>(key))
            .StartWith(GetConfiguration<T>(key));
    }
}
```

## Testing with DI

### Test Service Collection

```csharp
public class TestServiceCollection : IServiceCollection
{
    private readonly ServiceCollection _innerCollection = new();
    private readonly List<ServiceDescriptor> _registrations = new();
    
    public Result<ServiceDescriptor> FindRegistration<TService>()
    {
        var descriptor = _registrations.FirstOrDefault(d => d.ServiceType == typeof(TService));
        return descriptor != null
            ? Result.Success(descriptor)
            : new NotFoundError($"Service {typeof(TService).Name} not registered");
    }
    
    public Result ValidateRegistrations()
    {
        var errors = new List<Error>();
        
        foreach (var descriptor in _registrations)
        {
            if (descriptor.ImplementationType != null)
            {
                var constructor = descriptor.ImplementationType.GetConstructors().FirstOrDefault();
                if (constructor != null)
                {
                    foreach (var parameter in constructor.GetParameters())
                    {
                        if (!_registrations.Any(d => d.ServiceType == parameter.ParameterType))
                        {
                            errors.Add(new ValidationError(
                                $"Missing dependency: {parameter.ParameterType.Name} " +
                                $"required by {descriptor.ImplementationType.Name}"));
                        }
                    }
                }
            }
        }
        
        return errors.Any()
            ? new AggregateError("Registration validation failed", errors)
            : Result.Success();
    }
    
    // IServiceCollection implementation
    public int Count => _innerCollection.Count;
    
    public void Add(ServiceDescriptor item)
    {
        _innerCollection.Add(item);
        _registrations.Add(item);
    }
    
    // ... other IServiceCollection members
}
```

### Integration Test Base

```csharp
public abstract class IntegrationTestBase : IDisposable
{
    protected IServiceProvider ServiceProvider { get; }
    protected IServiceScope Scope { get; }
    
    protected IntegrationTestBase()
    {
        var services = new ServiceCollection();
        ConfigureServices(services);
        
        // Validate registrations
        var testCollection = new TestServiceCollection();
        foreach (var service in services)
        {
            testCollection.Add(service);
        }
        
        var validationResult = testCollection.ValidateRegistrations();
        if (validationResult.IsFailure)
        {
            throw new InvalidOperationException(
                $"Service registration validation failed: {validationResult.Error.Message}");
        }
        
        ServiceProvider = services.BuildServiceProvider();
        Scope = ServiceProvider.CreateScope();
    }
    
    protected virtual void ConfigureServices(IServiceCollection services)
    {
        // Override in derived classes
        services.AddLogging();
        services.AddMemoryCache();
    }
    
    protected T GetService<T>() where T : class
    {
        return Scope.ServiceProvider.GetRequiredService<T>();
    }
    
    protected Result<T> TryGetService<T>() where T : class
    {
        var service = Scope.ServiceProvider.GetService<T>();
        return service != null
            ? Result.Success(service)
            : new NotFoundError($"Service {typeof(T).Name} not found");
    }
    
    public void Dispose()
    {
        Scope?.Dispose();
        if (ServiceProvider is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }
}
```

### Mocking with DI

```csharp
public class MockServiceBuilder
{
    private readonly IServiceCollection _services = new ServiceCollection();
    private readonly Dictionary<Type, Mock> _mocks = new();
    
    public MockServiceBuilder WithMock<TService>(
        Action<Mock<TService>> setup = null) where TService : class
    {
        var mock = new Mock<TService>();
        setup?.Invoke(mock);
        
        _mocks[typeof(TService)] = mock;
        _services.AddSingleton(mock.Object);
        
        return this;
    }
    
    public MockServiceBuilder WithResultMock<TService, TResult>(
        Expression<Func<TService, Result<TResult>>> expression,
        Result<TResult> result) where TService : class
    {
        var mock = _mocks.TryGetValue(typeof(TService), out var existing)
            ? (Mock<TService>)existing
            : new Mock<TService>();
            
        mock.Setup(expression).Returns(result);
        
        if (!_mocks.ContainsKey(typeof(TService)))
        {
            _mocks[typeof(TService)] = mock;
            _services.AddSingleton(mock.Object);
        }
        
        return this;
    }
    
    public IServiceProvider Build()
    {
        return _services.BuildServiceProvider();
    }
    
    public Mock<TService> GetMock<TService>() where TService : class
    {
        return _mocks.TryGetValue(typeof(TService), out var mock)
            ? (Mock<TService>)mock
            : throw new InvalidOperationException($"No mock registered for {typeof(TService).Name}");
    }
}

// Usage
[Fact]
public async Task UserService_GetUser_Success()
{
    // Arrange
    var userId = Guid.NewGuid();
    var expectedUser = new User { Id = userId, Name = "Test User" };
    
    var services = new MockServiceBuilder()
        .WithResultMock<IUserRepository, User>(
            repo => repo.GetByIdAsync(userId, It.IsAny<CancellationToken>()),
            Result.Success(expectedUser))
        .WithMock<ILogger<UserService>>()
        .Build();
        
    var userService = new UserService(
        services.GetRequiredService<IUserRepository>(),
        services.GetRequiredService<ILogger<UserService>>());
    
    // Act
    var result = await userService.GetUserAsync(userId);
    
    // Assert
    result.Should().BeSuccess();
    result.Value.Should().Be(expectedUser);
}
```

## Advanced Scenarios

### Multi-Tenant DI

```csharp
public interface ITenantService<T> where T : class
{
    Result<T> GetService(string tenantId);
}

public class TenantService<T> : ITenantService<T> where T : class
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ITenantRegistry _tenantRegistry;
    
    public Result<T> GetService(string tenantId)
    {
        return _tenantRegistry.GetTenantConfiguration(tenantId)
            .Bind(config =>
            {
                var scope = _serviceProvider.CreateScope();
                
                // Configure tenant-specific services
                var tenantContext = scope.ServiceProvider.GetRequiredService<ITenantContext>();
                tenantContext.SetTenant(tenantId, config);
                
                var service = scope.ServiceProvider.GetService<T>();
                return service != null
                    ? Result.Success(service)
                    : new NotFoundError($"Service {typeof(T).Name} not found for tenant {tenantId}");
            });
    }
}

// Tenant-aware repository
public class TenantAwareRepository<T> : IRepository<T> where T : Entity, ITenantEntity
{
    private readonly ITenantContext _tenantContext;
    private readonly DbContext _context;
    
    public async Task<Result<T>> GetByIdAsync(Guid id)
    {
        return await _tenantContext.CurrentTenant
            .ToResult(new Error("NO_TENANT", "No tenant context available"))
            .BindAsync(async tenant =>
            {
                var entity = await _context.Set<T>()
                    .FirstOrDefaultAsync(e => e.Id == id && e.TenantId == tenant.Id);
                    
                return entity != null
                    ? Result.Success(entity)
                    : new NotFoundError(typeof(T).Name, id);
            });
    }
}
```

### Plugin Architecture

```csharp
public interface IPlugin
{
    string Name { get; }
    Version Version { get; }
    Result Initialize(IServiceCollection services);
}

public class PluginLoader
{
    private readonly List<IPlugin> _plugins = new();
    private readonly ILogger<PluginLoader> _logger;
    
    public Result LoadPlugin(string assemblyPath)
    {
        try
        {
            var assembly = Assembly.LoadFrom(assemblyPath);
            var pluginTypes = assembly.GetTypes()
                .Where(t => typeof(IPlugin).IsAssignableFrom(t) && !t.IsAbstract);
                
            foreach (var pluginType in pluginTypes)
            {
                var plugin = Activator.CreateInstance(pluginType) as IPlugin;
                if (plugin != null)
                {
                    _plugins.Add(plugin);
                    _logger.LogInformation("Loaded plugin: {Name} v{Version}", 
                        plugin.Name, plugin.Version);
                }
            }
            
            return Result.Success();
        }
        catch (Exception ex)
        {
            return new Error("PLUGIN_LOAD_FAILED", $"Failed to load plugin: {ex.Message}");
        }
    }
    
    public Result InitializePlugins(IServiceCollection services)
    {
        var errors = new List<Error>();
        
        foreach (var plugin in _plugins)
        {
            var result = plugin.Initialize(services);
            if (result.IsFailure)
            {
                errors.Add(new Error(
                    $"PLUGIN_INIT_FAILED_{plugin.Name}",
                    $"Plugin {plugin.Name} initialization failed: {result.Error.Message}"));
            }
        }
        
        return errors.Any()
            ? new AggregateError("Some plugins failed to initialize", errors)
            : Result.Success();
    }
}
```

## Third-Party Containers

### Autofac Integration

```csharp
public static class AutofacExtensions
{
    public static ContainerBuilder RegisterFluentUnions(this ContainerBuilder builder)
    {
        // Register Result handlers
        builder.RegisterGeneric(typeof(DefaultResultHandler<>))
            .As(typeof(IResultHandler<>))
            .InstancePerLifetimeScope();
            
        // Register with Result validation
        builder.RegisterModule<FluentUnionsModule>();
        
        return builder;
    }
}

public class FluentUnionsModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        // Register services that return Result
        builder.RegisterType<UserService>()
            .As<IUserService>()
            .InstancePerLifetimeScope()
            .OnActivating(e =>
            {
                // Validate dependencies
                var logger = e.Context.Resolve<ILogger<UserService>>();
                logger.LogDebug("Activating UserService");
            });
            
        // Register decorators
        builder.RegisterGeneric(typeof(CachingDecorator<>))
            .As(typeof(ICachingDecorator<>))
            .SingleInstance();
    }
    
    protected override void AttachToComponentRegistration(
        IComponentRegistryBuilder componentRegistry,
        IComponentRegistration registration)
    {
        // Add Result-based activation validation
        registration.Activating += (sender, e) =>
        {
            if (e.Instance is IValidatable validatable)
            {
                var result = validatable.Validate();
                if (result.IsFailure)
                {
                    throw new InvalidOperationException(
                        $"Service validation failed: {result.Error.Message}");
                }
            }
        };
    }
}
```

## Best Practices

### 1. Register Result Handlers

```csharp
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddResultHandlers(this IServiceCollection services)
    {
        // Register generic handlers
        services.AddScoped(typeof(IResultHandler<>), typeof(DefaultResultHandler<>));
        services.AddScoped(typeof(IOptionHandler<>), typeof(DefaultOptionHandler<>));
        
        // Register specific handlers
        services.AddScoped<IResultHandler<User>, UserResultHandler>();
        
        // Register error handlers
        services.AddSingleton<IErrorHandler, GlobalErrorHandler>();
        
        return services;
    }
}
```

### 2. Validate Service Registration

```csharp
public static class ServiceValidationExtensions
{
    public static IServiceCollection ValidateServices(this IServiceCollection services)
    {
        var provider = services.BuildServiceProvider();
        var errors = new List<string>();
        
        foreach (var service in services)
        {
            if (service.ServiceType.IsInterface)
            {
                try
                {
                    var instance = provider.GetService(service.ServiceType);
                    if (instance == null && service.Lifetime != ServiceLifetime.Transient)
                    {
                        errors.Add($"Failed to resolve: {service.ServiceType.Name}");
                    }
                }
                catch (Exception ex)
                {
                    errors.Add($"Error resolving {service.ServiceType.Name}: {ex.Message}");
                }
            }
        }
        
        if (errors.Any())
        {
            throw new InvalidOperationException(
                $"Service validation failed:\n{string.Join("\n", errors)}");
        }
        
        return services;
    }
}
```

### 3. Use Factory Pattern for Complex Dependencies

```csharp
public static class ComplexServiceRegistration
{
    public static IServiceCollection AddComplexService(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IComplexService>(provider =>
        {
            var logger = provider.GetRequiredService<ILogger<ComplexService>>();
            
            // Get configuration with validation
            var configResult = configuration
                .GetSection("ComplexService")
                .Get<ComplexServiceOptions>()
                .ToResult()
                .Bind(options => ValidateOptions(options));
                
            if (configResult.IsFailure)
            {
                logger.LogError("Invalid configuration: {Error}", configResult.Error);
                throw new InvalidOperationException(
                    $"Cannot create ComplexService: {configResult.Error.Message}");
            }
            
            // Create dependencies
            var dependency1 = provider.GetRequiredService<IDependency1>();
            var dependency2 = provider.GetRequiredService<IDependency2>();
            
            return new ComplexService(configResult.Value, dependency1, dependency2, logger);
        });
        
        return services;
    }
}
```

### 4. Scope Management

```csharp
public class ScopedServiceManager : IDisposable
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly Dictionary<string, IServiceScope> _scopes = new();
    
    public Result<T> ExecuteInScope<T>(string scopeId, Func<IServiceProvider, Result<T>> operation)
    {
        try
        {
            if (!_scopes.TryGetValue(scopeId, out var scope))
            {
                scope = _scopeFactory.CreateScope();
                _scopes[scopeId] = scope;
            }
            
            return operation(scope.ServiceProvider);
        }
        catch (Exception ex)
        {
            return new Error("SCOPE_EXECUTION_FAILED", ex.Message);
        }
    }
    
    public Result DisposeScope(string scopeId)
    {
        if (_scopes.TryGetValue(scopeId, out var scope))
        {
            scope.Dispose();
            _scopes.Remove(scopeId);
            return Result.Success();
        }
        
        return new NotFoundError($"Scope {scopeId} not found");
    }
    
    public void Dispose()
    {
        foreach (var scope in _scopes.Values)
        {
            scope.Dispose();
        }
        _scopes.Clear();
    }
}
```

## Summary

Integrating FluentUnions with dependency injection provides:

1. **Type-safe service resolution** - Services return Result types
2. **Validation at registration** - Catch configuration errors early
3. **Flexible decorators** - Add cross-cutting concerns easily
4. **Testable services** - Mock Result returns easily
5. **Clean configuration** - Options pattern with validation

Key patterns:
- Service registration with Result validation
- Factory patterns for complex dependencies
- Decorator pattern for cross-cutting concerns
- Proper lifetime management
- Integration with third-party containers

Next steps:
- [Testing Guide](../guides/testing-guide.md)
- [Performance Best Practices](../guides/performance-best-practices.md)
- [ASP.NET Core Integration](aspnet-core.md)
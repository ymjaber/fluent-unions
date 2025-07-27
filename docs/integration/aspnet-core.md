# ASP.NET Core Integration

This guide demonstrates how to integrate FluentUnions with ASP.NET Core applications for robust error handling and clean API design.

## Table of Contents
1. [Setup and Configuration](#setup-and-configuration)
2. [Controller Integration](#controller-integration)
3. [Minimal APIs](#minimal-apis)
4. [Model Binding](#model-binding)
5. [Action Filters](#action-filters)
6. [Middleware](#middleware)
7. [Dependency Injection](#dependency-injection)
8. [Swagger/OpenAPI](#swaggeropenapi)
9. [Testing](#testing)
10. [Best Practices](#best-practices)

## Setup and Configuration

### Installation

```bash
dotnet add package FluentUnions
dotnet add package FluentUnions.AspNetCore # Optional extensions package
```

### Program.cs Configuration

```csharp
var builder = WebApplication.CreateBuilder(args);

// Add FluentUnions services
builder.Services.AddFluentUnions(options =>
{
    options.UseDefaultErrorMapping = true;
    options.EnableDetailedErrors = builder.Environment.IsDevelopment();
});

// Configure controllers with FluentUnions
builder.Services.AddControllers(options =>
{
    // Add custom model binders
    options.ModelBinderProviders.Insert(0, new ResultModelBinderProvider());
    options.ModelBinderProviders.Insert(1, new OptionModelBinderProvider());
    
    // Add action filters
    options.Filters.Add<ResultExceptionFilter>();
});

// Configure API behavior
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(x => x.Value.Errors.Count > 0)
            .Select(x => new ValidationError(x.Key, x.Value.Errors.First().ErrorMessage))
            .ToList();
            
        var result = Result.Failure<object>(new AggregateError("Validation failed", errors));
        return result.ToActionResult();
    };
});

var app = builder.Build();

// Add FluentUnions middleware
app.UseFluentUnionsErrorHandler();

app.MapControllers();
app.Run();
```

## Controller Integration

### Base Controller with Result Support

```csharp
[ApiController]
public abstract class ApiControllerBase : ControllerBase
{
    protected IActionResult FromResult<T>(Result<T> result)
    {
        return result.ToActionResult();
    }
    
    protected async Task<IActionResult> FromResultAsync<T>(Task<Result<T>> resultTask)
    {
        var result = await resultTask;
        return result.ToActionResult();
    }
    
    protected IActionResult FromResult(Result result)
    {
        return result.ToActionResult();
    }
}
```

### Extension Methods for ActionResult

```csharp
public static class ResultExtensions
{
    public static IActionResult ToActionResult<T>(this Result<T> result)
    {
        return result.Match<IActionResult>(
            onSuccess: value => new OkObjectResult(value),
            onFailure: error => error.ToActionResult()
        );
    }
    
    public static IActionResult ToActionResult(this Result result)
    {
        return result.Match<IActionResult>(
            onSuccess: () => new NoContentResult(),
            onFailure: error => error.ToActionResult()
        );
    }
    
    public static IActionResult ToActionResult(this Error error)
    {
        var statusCode = GetStatusCode(error);
        var response = new ProblemDetails
        {
            Status = statusCode,
            Title = GetTitle(error),
            Detail = error.Message,
            Type = GetProblemType(error),
            Extensions =
            {
                ["code"] = error.Code,
                ["metadata"] = error.Metadata
            }
        };
        
        return new ObjectResult(response) { StatusCode = statusCode };
    }
    
    private static int GetStatusCode(Error error) => error switch
    {
        ValidationError => StatusCodes.Status400BadRequest,
        NotFoundError => StatusCodes.Status404NotFound,
        ConflictError => StatusCodes.Status409Conflict,
        AuthenticationError => StatusCodes.Status401Unauthorized,
        AuthorizationError => StatusCodes.Status403Forbidden,
        _ => StatusCodes.Status500InternalServerError
    };
}
```

### Example Controller

```csharp
[ApiController]
[Route("api/[controller]")]
public class UsersController : ApiControllerBase
{
    private readonly IUserService _userService;
    
    public UsersController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUser(Guid id)
    {
        return await FromResultAsync(
            _userService.GetUserAsync(id)
                .MapAsync(user => new UserDto(user))
        );
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
    {
        var result = await _userService.CreateUserAsync(request);
        
        return result.Match<IActionResult>(
            onSuccess: user => CreatedAtAction(
                nameof(GetUser), 
                new { id = user.Id }, 
                new UserDto(user)),
            onFailure: error => error.ToActionResult()
        );
    }
    
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserRequest request)
    {
        if (id != request.Id)
            return new ValidationError("ID mismatch").ToActionResult();
            
        return await FromResultAsync(
            _userService.UpdateUserAsync(request)
                .MapAsync(user => new UserDto(user))
        );
    }
    
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        return await FromResultAsync(
            _userService.DeleteUserAsync(id)
        );
    }
    
    [HttpGet("{id:guid}/profile")]
    [ProducesResponseType(typeof(UserProfileDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetUserProfile(Guid id)
    {
        var result = await _userService.GetUserProfileAsync(id);
        
        return result.Match<IActionResult>(
            onSome: profile => Ok(new UserProfileDto(profile)),
            onNone: () => NoContent()
        );
    }
}
```

## Minimal APIs

### Extension Methods for Minimal APIs

```csharp
public static class MinimalApiExtensions
{
    public static IResult ToHttpResult<T>(this Result<T> result)
    {
        return result.Match(
            onSuccess: value => Results.Ok(value),
            onFailure: error => error.ToHttpResult()
        );
    }
    
    public static IResult ToHttpResult(this Error error)
    {
        return error switch
        {
            ValidationError => Results.ValidationProblem(CreateValidationDictionary(error)),
            NotFoundError => Results.NotFound(CreateProblemDetails(error)),
            ConflictError => Results.Conflict(CreateProblemDetails(error)),
            AuthenticationError => Results.Unauthorized(),
            AuthorizationError => Results.Forbid(),
            _ => Results.Problem(CreateProblemDetails(error))
        };
    }
    
    private static Dictionary<string, string[]> CreateValidationDictionary(Error error)
    {
        if (error is AggregateError aggregate)
        {
            return aggregate.Errors
                .OfType<ValidationError>()
                .GroupBy(e => e.Metadata.GetValueOrDefault("field")?.ToString() ?? "general")
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.Message).ToArray()
                );
        }
        
        var field = error.Metadata.GetValueOrDefault("field")?.ToString() ?? "general";
        return new Dictionary<string, string[]>
        {
            [field] = new[] { error.Message }
        };
    }
}
```

### Minimal API Endpoints

```csharp
var app = builder.Build();

// User endpoints
var users = app.MapGroup("/api/users")
    .WithTags("Users")
    .WithOpenApi();

users.MapGet("/{id:guid}", async (Guid id, IUserService userService) =>
{
    var result = await userService.GetUserAsync(id);
    return result.ToHttpResult();
})
.WithName("GetUser")
.Produces<UserDto>()
.ProducesProblem(StatusCodes.Status404NotFound);

users.MapPost("/", async (CreateUserRequest request, IUserService userService) =>
{
    var result = await userService.CreateUserAsync(request);
    return result.Match(
        onSuccess: user => Results.Created($"/api/users/{user.Id}", new UserDto(user)),
        onFailure: error => error.ToHttpResult()
    );
})
.WithName("CreateUser")
.Produces<UserDto>(StatusCodes.Status201Created)
.ProducesValidationProblem()
.ProducesProblem(StatusCodes.Status409Conflict);

// Option endpoint example
users.MapGet("/{id:guid}/settings", async (Guid id, IUserService userService) =>
{
    var option = await userService.GetUserSettingsAsync(id);
    return option.Match(
        onSome: settings => Results.Ok(settings),
        onNone: () => Results.NoContent()
    );
})
.WithName("GetUserSettings")
.Produces<UserSettings>()
.Produces(StatusCodes.Status204NoContent);

// Complex operation with Result
users.MapPost("/{id:guid}/verify", async (
    Guid id, 
    VerifyUserRequest request,
    IUserService userService,
    IEmailService emailService) =>
{
    var result = await userService.GetUserAsync(id)
        .BindAsync(user => userService.VerifyUserAsync(user, request.VerificationCode))
        .TapAsync(user => emailService.SendWelcomeEmailAsync(user.Email));
        
    return result.ToHttpResult();
})
.WithName("VerifyUser")
.Produces<UserDto>()
.ProducesProblem(StatusCodes.Status404NotFound)
.ProducesValidationProblem();
```

## Model Binding

### Custom Model Binder for Result Types

```csharp
public class ResultModelBinderProvider : IModelBinderProvider
{
    public IModelBinder GetBinder(ModelBinderProviderContext context)
    {
        if (context.Metadata.ModelType.IsGenericType &&
            context.Metadata.ModelType.GetGenericTypeDefinition() == typeof(Result<>))
        {
            return new ResultModelBinder();
        }
        
        return null;
    }
}

public class ResultModelBinder : IModelBinder
{
    public async Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var modelType = bindingContext.ModelType.GetGenericArguments()[0];
        var modelBinder = bindingContext.ModelBinderFactory.CreateBinder(
            new DefaultModelBinderProviderContext(bindingContext.Services, bindingContext)
            {
                Metadata = bindingContext.ModelMetadataProvider.GetMetadataForType(modelType)
            });
        
        var modelBindingContext = DefaultModelBindingContext.CreateBindingContext(
            bindingContext.ActionContext,
            bindingContext.ValueProvider,
            bindingContext.ModelMetadataProvider.GetMetadataForType(modelType),
            bindingInfo: null,
            bindingContext.ModelName);
            
        await modelBinder.BindModelAsync(modelBindingContext);
        
        if (modelBindingContext.Result.IsModelSet)
        {
            var resultType = typeof(Result<>).MakeGenericType(modelType);
            var result = Activator.CreateInstance(resultType, modelBindingContext.Result.Model);
            bindingContext.Result = ModelBindingResult.Success(result);
        }
        else
        {
            bindingContext.Result = ModelBindingResult.Failed();
        }
    }
}
```

### Validation with Result

```csharp
public class ResultValidationAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .Select(x => new ValidationError(
                    x.Key.ToLowerInvariant(), 
                    x.Value.Errors.First().ErrorMessage))
                .ToList();
                
            var result = Result.Failure<object>(
                new AggregateError("Validation failed", errors));
                
            context.Result = result.ToActionResult();
        }
    }
}
```

## Action Filters

### Result Exception Filter

```csharp
public class ResultExceptionFilter : IAsyncExceptionFilter
{
    private readonly ILogger<ResultExceptionFilter> _logger;
    private readonly IWebHostEnvironment _environment;
    
    public ResultExceptionFilter(
        ILogger<ResultExceptionFilter> logger,
        IWebHostEnvironment environment)
    {
        _logger = logger;
        _environment = environment;
    }
    
    public async Task OnExceptionAsync(ExceptionContext context)
    {
        var error = ConvertExceptionToError(context.Exception);
        
        _logger.LogError(
            context.Exception,
            "Unhandled exception converted to Result: {ErrorCode}",
            error.Code);
            
        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "An error occurred",
            Detail = _environment.IsDevelopment() 
                ? context.Exception.Message 
                : "An unexpected error occurred",
            Type = "https://example.com/errors/internal",
            Extensions =
            {
                ["code"] = error.Code,
                ["traceId"] = Activity.Current?.Id ?? context.HttpContext.TraceIdentifier
            }
        };
        
        if (_environment.IsDevelopment())
        {
            problemDetails.Extensions["exception"] = context.Exception.ToString();
        }
        
        context.Result = new ObjectResult(problemDetails)
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };
        
        context.ExceptionHandled = true;
    }
    
    private Error ConvertExceptionToError(Exception exception)
    {
        return exception switch
        {
            ValidationException ve => new ValidationError(ve.Message),
            NotFoundException nfe => new NotFoundError(nfe.Message),
            UnauthorizedAccessException => new AuthorizationError("Access denied"),
            _ => new Error("INTERNAL_ERROR", "An unexpected error occurred")
        };
    }
}
```

### Result Action Filter

```csharp
public class ResultActionFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        var executedContext = await next();
        
        if (executedContext.Result is ObjectResult objectResult)
        {
            // Automatically convert Result types to appropriate HTTP responses
            if (IsResultType(objectResult.Value?.GetType()))
            {
                var result = objectResult.Value;
                var convertMethod = typeof(ResultExtensions)
                    .GetMethod(nameof(ResultExtensions.ToActionResult))
                    .MakeGenericMethod(GetResultValueType(result.GetType()));
                    
                executedContext.Result = (IActionResult)convertMethod.Invoke(null, new[] { result });
            }
        }
    }
    
    private bool IsResultType(Type type)
    {
        return type != null && 
               type.IsGenericType && 
               type.GetGenericTypeDefinition() == typeof(Result<>);
    }
    
    private Type GetResultValueType(Type resultType)
    {
        return resultType.GetGenericArguments()[0];
    }
}
```

## Middleware

### Global Error Handling Middleware

```csharp
public class FluentUnionsErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<FluentUnionsErrorHandlerMiddleware> _logger;
    private readonly FluentUnionsOptions _options;
    
    public FluentUnionsErrorHandlerMiddleware(
        RequestDelegate next,
        ILogger<FluentUnionsErrorHandlerMiddleware> logger,
        IOptions<FluentUnionsOptions> options)
    {
        _next = next;
        _logger = logger;
        _options = options.Value;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }
    
    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(exception, "Unhandled exception occurred");
        
        var error = CreateErrorFromException(exception);
        var problemDetails = CreateProblemDetails(context, error, exception);
        
        context.Response.StatusCode = problemDetails.Status ?? 500;
        context.Response.ContentType = "application/problem+json";
        
        await JsonSerializer.SerializeAsync(
            context.Response.Body,
            problemDetails,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
    }
    
    private Error CreateErrorFromException(Exception exception)
    {
        if (_options.ExceptionToErrorMapping.TryGetValue(
            exception.GetType(), 
            out var errorFactory))
        {
            return errorFactory(exception);
        }
        
        return new Error("UNHANDLED_EXCEPTION", 
            _options.EnableDetailedErrors 
                ? exception.Message 
                : "An error occurred");
    }
    
    private ProblemDetails CreateProblemDetails(
        HttpContext context, 
        Error error, 
        Exception exception)
    {
        var problemDetails = new ProblemDetails
        {
            Status = GetStatusCode(error),
            Title = GetTitle(error),
            Detail = error.Message,
            Instance = context.Request.Path,
            Type = GetProblemType(error)
        };
        
        problemDetails.Extensions["code"] = error.Code;
        problemDetails.Extensions["traceId"] = Activity.Current?.Id ?? context.TraceIdentifier;
        
        if (_options.EnableDetailedErrors)
        {
            problemDetails.Extensions["exception"] = new
            {
                type = exception.GetType().Name,
                message = exception.Message,
                stackTrace = exception.StackTrace
            };
        }
        
        if (error.Metadata?.Any() == true)
        {
            problemDetails.Extensions["metadata"] = error.Metadata;
        }
        
        return problemDetails;
    }
}
```

### Request/Response Logging Middleware

```csharp
public class ResultLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ResultLoggingMiddleware> _logger;
    
    public async Task InvokeAsync(HttpContext context)
    {
        // Log request
        _logger.LogInformation(
            "Request {Method} {Path} started",
            context.Request.Method,
            context.Request.Path);
            
        var originalBodyStream = context.Response.Body;
        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;
        
        await _next(context);
        
        // Log response
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        
        if (context.Response.StatusCode >= 400)
        {
            _logger.LogWarning(
                "Request {Method} {Path} failed with {StatusCode}: {Response}",
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode,
                responseText);
        }
        else
        {
            _logger.LogInformation(
                "Request {Method} {Path} completed with {StatusCode}",
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode);
        }
        
        await responseBody.CopyToAsync(originalBodyStream);
    }
}
```

## Dependency Injection

### Service Registration

```csharp
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFluentUnions(
        this IServiceCollection services,
        Action<FluentUnionsOptions> configureOptions = null)
    {
        // Register options
        services.Configure<FluentUnionsOptions>(options =>
        {
            // Default configuration
            options.UseDefaultErrorMapping = true;
            options.EnableDetailedErrors = false;
            
            // User configuration
            configureOptions?.Invoke(options);
        });
        
        // Register error handlers
        services.AddSingleton<IErrorHandler, DefaultErrorHandler>();
        services.AddSingleton<IErrorMapper, DefaultErrorMapper>();
        
        // Register action filters
        services.AddScoped<ResultExceptionFilter>();
        services.AddScoped<ResultActionFilter>();
        
        // Register model binders
        services.AddSingleton<ResultModelBinderProvider>();
        services.AddSingleton<OptionModelBinderProvider>();
        
        return services;
    }
    
    public static IServiceCollection AddResultHandlers(
        this IServiceCollection services)
    {
        // Scan and register all IResultHandler implementations
        var handlerType = typeof(IResultHandler<,>);
        var handlers = Assembly.GetCallingAssembly()
            .GetTypes()
            .Where(t => t.GetInterfaces().Any(i => 
                i.IsGenericType && 
                i.GetGenericTypeDefinition() == handlerType))
            .ToList();
            
        foreach (var handler in handlers)
        {
            var serviceType = handler.GetInterfaces()
                .First(i => i.IsGenericType && 
                           i.GetGenericTypeDefinition() == handlerType);
            services.AddScoped(serviceType, handler);
        }
        
        return services;
    }
}
```

### Options Configuration

```csharp
public class FluentUnionsOptions
{
    public bool UseDefaultErrorMapping { get; set; } = true;
    public bool EnableDetailedErrors { get; set; } = false;
    public Dictionary<Type, Func<Exception, Error>> ExceptionToErrorMapping { get; set; } = new();
    public Dictionary<Type, int> ErrorToStatusCodeMapping { get; set; } = new();
    
    public FluentUnionsOptions()
    {
        // Default mappings
        ExceptionToErrorMapping[typeof(ArgumentException)] = 
            ex => new ValidationError(ex.Message);
        ExceptionToErrorMapping[typeof(KeyNotFoundException)] = 
            ex => new NotFoundError(ex.Message);
        ExceptionToErrorMapping[typeof(InvalidOperationException)] = 
            ex => new ConflictError(ex.Message);
            
        ErrorToStatusCodeMapping[typeof(ValidationError)] = 400;
        ErrorToStatusCodeMapping[typeof(NotFoundError)] = 404;
        ErrorToStatusCodeMapping[typeof(ConflictError)] = 409;
        ErrorToStatusCodeMapping[typeof(AuthenticationError)] = 401;
        ErrorToStatusCodeMapping[typeof(AuthorizationError)] = 403;
    }
}
```

## Swagger/OpenAPI

### Swagger Configuration

```csharp
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My API",
        Version = "v1",
        Description = "API with FluentUnions error handling"
    });
    
    // Add Result schema filter
    options.SchemaFilter<ResultSchemaFilter>();
    options.SchemaFilter<OptionSchemaFilter>();
    
    // Add operation filter for responses
    options.OperationFilter<ResultOperationFilter>();
    
    // Add XML comments
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});
```

### Schema Filters

```csharp
public class ResultSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type.IsGenericType && 
            context.Type.GetGenericTypeDefinition() == typeof(Result<>))
        {
            var valueType = context.Type.GetGenericArguments()[0];
            var valueSchema = context.SchemaGenerator.GenerateSchema(
                valueType, 
                context.SchemaRepository);
                
            schema.Type = "object";
            schema.Properties = new Dictionary<string, OpenApiSchema>
            {
                ["isSuccess"] = new() { Type = "boolean" },
                ["value"] = valueSchema,
                ["error"] = new() 
                { 
                    Type = "object",
                    Properties = new Dictionary<string, OpenApiSchema>
                    {
                        ["code"] = new() { Type = "string" },
                        ["message"] = new() { Type = "string" },
                        ["metadata"] = new() { Type = "object" }
                    }
                }
            };
        }
    }
}

public class OptionSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type.IsGenericType && 
            context.Type.GetGenericTypeDefinition() == typeof(Option<>))
        {
            var valueType = context.Type.GetGenericArguments()[0];
            var valueSchema = context.SchemaGenerator.GenerateSchema(
                valueType, 
                context.SchemaRepository);
                
            schema.Type = "object";
            schema.Properties = new Dictionary<string, OpenApiSchema>
            {
                ["hasValue"] = new() { Type = "boolean" },
                ["value"] = valueSchema
            };
            schema.Nullable = true;
        }
    }
}
```

### Operation Filter

```csharp
public class ResultOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Add common error responses
        if (!operation.Responses.ContainsKey("400"))
        {
            operation.Responses.Add("400", new OpenApiResponse
            {
                Description = "Bad Request - Validation Error",
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    ["application/problem+json"] = new()
                    {
                        Schema = GetProblemDetailsSchema(context)
                    }
                }
            });
        }
        
        if (!operation.Responses.ContainsKey("404"))
        {
            operation.Responses.Add("404", new OpenApiResponse
            {
                Description = "Not Found",
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    ["application/problem+json"] = new()
                    {
                        Schema = GetProblemDetailsSchema(context)
                    }
                }
            });
        }
        
        if (!operation.Responses.ContainsKey("500"))
        {
            operation.Responses.Add("500", new OpenApiResponse
            {
                Description = "Internal Server Error",
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    ["application/problem+json"] = new()
                    {
                        Schema = GetProblemDetailsSchema(context)
                    }
                }
            });
        }
    }
    
    private OpenApiSchema GetProblemDetailsSchema(OperationFilterContext context)
    {
        return context.SchemaGenerator.GenerateSchema(
            typeof(ProblemDetails),
            context.SchemaRepository);
    }
}
```

## Testing

### Integration Tests

```csharp
public class UserControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;
    
    public UserControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Replace services for testing
                services.AddSingleton<IUserService, MockUserService>();
            });
        });
        
        _client = _factory.CreateClient();
    }
    
    [Fact]
    public async Task GetUser_WhenUserExists_ReturnsOk()
    {
        // Arrange
        var userId = Guid.NewGuid();
        
        // Act
        var response = await _client.GetAsync($"/api/users/{userId}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var user = await response.Content.ReadFromJsonAsync<UserDto>();
        user.Should().NotBeNull();
        user.Id.Should().Be(userId);
    }
    
    [Fact]
    public async Task GetUser_WhenUserNotFound_ReturnsNotFound()
    {
        // Arrange
        var userId = Guid.NewGuid();
        
        // Act
        var response = await _client.GetAsync($"/api/users/{userId}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        problem.Should().NotBeNull();
        problem.Status.Should().Be(404);
        problem.Extensions["code"].ToString().Should().Be("NOT_FOUND");
    }
    
    [Fact]
    public async Task CreateUser_WithInvalidData_ReturnsBadRequest()
    {
        // Arrange
        var request = new CreateUserRequest { Email = "invalid" };
        
        // Act
        var response = await _client.PostAsJsonAsync("/api/users", request);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        problem.Should().NotBeNull();
        problem.Errors.Should().ContainKey("email");
    }
}
```

### Unit Tests for Result Extensions

```csharp
public class ResultExtensionsTests
{
    [Fact]
    public void ToActionResult_WithSuccess_ReturnsOkResult()
    {
        // Arrange
        var result = Result.Success(new User { Id = 1, Name = "Test" });
        
        // Act
        var actionResult = result.ToActionResult();
        
        // Assert
        actionResult.Should().BeOfType<OkObjectResult>();
        var okResult = (OkObjectResult)actionResult;
        okResult.Value.Should().BeOfType<User>();
    }
    
    [Fact]
    public void ToActionResult_WithValidationError_ReturnsBadRequest()
    {
        // Arrange
        var result = Result.Failure<User>(new ValidationError("Invalid input"));
        
        // Act
        var actionResult = result.ToActionResult();
        
        // Assert
        actionResult.Should().BeOfType<ObjectResult>();
        var objectResult = (ObjectResult)actionResult;
        objectResult.StatusCode.Should().Be(400);
        objectResult.Value.Should().BeOfType<ProblemDetails>();
    }
}
```

## Best Practices

### 1. Consistent Error Responses

```csharp
// Create a standard error response format
public class ApiErrorResponse
{
    public string Type { get; set; }
    public string Title { get; set; }
    public int Status { get; set; }
    public string Detail { get; set; }
    public string Instance { get; set; }
    public Dictionary<string, object> Extensions { get; set; }
    
    public static ApiErrorResponse FromError(Error error, HttpContext context)
    {
        return new ApiErrorResponse
        {
            Type = GetErrorType(error),
            Title = GetErrorTitle(error),
            Status = GetStatusCode(error),
            Detail = error.Message,
            Instance = context.Request.Path,
            Extensions = new Dictionary<string, object>
            {
                ["code"] = error.Code,
                ["traceId"] = context.TraceIdentifier,
                ["metadata"] = error.Metadata
            }
        };
    }
}
```

### 2. Use Action Results Properly

```csharp
// Good - specific action results
public async Task<IActionResult> CreateResource(CreateRequest request)
{
    var result = await _service.CreateAsync(request);
    
    return result.Match<IActionResult>(
        onSuccess: resource => CreatedAtAction(
            nameof(GetResource),
            new { id = resource.Id },
            resource),
        onFailure: error => error.ToActionResult()
    );
}

// Avoid - generic Ok() for all success cases
public async Task<IActionResult> CreateResource(CreateRequest request)
{
    var result = await _service.CreateAsync(request);
    return result.ToActionResult(); // Always returns 200 OK
}
```

### 3. Validation at API Boundaries

```csharp
public class ApiValidationService
{
    public Result<T> ValidateRequest<T>(T request) where T : class
    {
        var validator = new DataAnnotationsValidator();
        var validationResults = new List<ValidationResult>();
        
        if (!validator.TryValidateObject(
            request, 
            new ValidationContext(request), 
            validationResults, 
            true))
        {
            var errors = validationResults
                .Select(vr => new ValidationError(vr.MemberNames.First(), vr.ErrorMessage))
                .ToList();
                
            return new AggregateError("Validation failed", errors);
        }
        
        return Result.Success(request);
    }
}
```

### 4. Async All the Way

```csharp
// Good - async throughout
[HttpGet("{id}")]
public async Task<IActionResult> GetResource(Guid id)
{
    var result = await _service.GetResourceAsync(id);
    return result.ToActionResult();
}

// Avoid - blocking async calls
[HttpGet("{id}")]
public IActionResult GetResource(Guid id)
{
    var result = _service.GetResourceAsync(id).Result; // Blocks!
    return result.ToActionResult();
}
```

### 5. Proper HTTP Status Codes

```csharp
public static class HttpStatusCodeMapper
{
    private static readonly Dictionary<Type, int> ErrorStatusCodes = new()
    {
        [typeof(ValidationError)] = StatusCodes.Status400BadRequest,
        [typeof(AuthenticationError)] = StatusCodes.Status401Unauthorized,
        [typeof(AuthorizationError)] = StatusCodes.Status403Forbidden,
        [typeof(NotFoundError)] = StatusCodes.Status404NotFound,
        [typeof(ConflictError)] = StatusCodes.Status409Conflict,
        [typeof(TooManyRequestsError)] = StatusCodes.Status429TooManyRequests,
    };
    
    public static int GetStatusCode(Error error)
    {
        return ErrorStatusCodes.TryGetValue(error.GetType(), out var statusCode)
            ? statusCode
            : StatusCodes.Status500InternalServerError;
    }
}
```

## Summary

Integrating FluentUnions with ASP.NET Core provides:

1. **Type-safe error handling** - Errors are explicit in method signatures
2. **Consistent API responses** - Standardized error formats
3. **Better testability** - Easy to test both success and failure paths
4. **Improved developer experience** - IntelliSense and compile-time checking
5. **Flexible integration** - Works with controllers, minimal APIs, and middleware

Key integration points:
- Extension methods for ActionResult conversion
- Custom model binders and action filters
- Global error handling middleware
- Swagger/OpenAPI documentation
- Comprehensive testing support

Next steps:
- [MediatR Integration](mediatr.md)
- [Entity Framework Integration](entity-framework.md)
- [Testing Guide](../guides/testing-guide.md)
# API Patterns

This guide demonstrates how to implement Web API patterns using FluentUnions for robust and consistent API design.

## Table of Contents
1. [Introduction](#introduction)
2. [Controller Patterns](#controller-patterns)
3. [Response Mapping](#response-mapping)
4. [Error Handling](#error-handling)
5. [Request Validation](#request-validation)
6. [API Versioning](#api-versioning)
7. [Authentication and Authorization](#authentication-and-authorization)
8. [Middleware Integration](#middleware-integration)
9. [OpenAPI Documentation](#openapi-documentation)
10. [Best Practices](#best-practices)

## Introduction

API patterns with FluentUnions provide:
- Consistent error responses across endpoints
- Type-safe request/response handling
- Clear separation between domain and API layers
- Testable API controllers

## Controller Patterns

### Basic API Controller

```csharp
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UsersController> _logger;
    
    public UsersController(IUserService userService, ILogger<UsersController> logger)
    {
        _userService = userService;
        _logger = logger;
    }
    
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUser(Guid id)
    {
        return _userService.GetUser(id)
            .Map(user => new UserDto(user))
            .ToActionResult();
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
    {
        return await _userService.CreateUserAsync(request)
            .MapAsync(user => new UserDto(user))
            .ToActionResultAsync(
                onSuccess: dto => CreatedAtAction(
                    nameof(GetUser), 
                    new { id = dto.Id }, 
                    dto));
    }
    
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserRequest request)
    {
        if (id != request.Id)
            return BadRequest(new ErrorResponse("ID_MISMATCH", "URL ID does not match request ID"));
            
        return await _userService.UpdateUserAsync(request)
            .MapAsync(user => new UserDto(user))
            .ToActionResultAsync();
    }
    
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        return await _userService.DeleteUserAsync(id)
            .ToActionResultAsync(onSuccess: () => NoContent());
    }
}
```

### Result to ActionResult Extensions

```csharp
public static class ResultExtensions
{
    public static IActionResult ToActionResult<T>(this Result<T> result)
    {
        return result.Match<IActionResult>(
            onSuccess: value => new OkObjectResult(value),
            onFailure: error => ErrorToActionResult(error));
    }
    
    public static IActionResult ToActionResult(
        this Result result,
        Func<IActionResult> onSuccess)
    {
        return result.Match(
            onSuccess: () => onSuccess(),
            onFailure: error => ErrorToActionResult(error));
    }
    
    public static async Task<IActionResult> ToActionResultAsync<T>(
        this Task<Result<T>> resultTask,
        Func<T, IActionResult> onSuccess = null)
    {
        var result = await resultTask;
        return result.Match<IActionResult>(
            onSuccess: value => onSuccess?.Invoke(value) ?? new OkObjectResult(value),
            onFailure: error => ErrorToActionResult(error));
    }
    
    private static IActionResult ErrorToActionResult(Error error)
    {
        return error switch
        {
            ValidationError ve => new BadRequestObjectResult(
                new ValidationErrorResponse(ve)),
            NotFoundError nf => new NotFoundObjectResult(
                new ErrorResponse(nf)),
            ConflictError ce => new ConflictObjectResult(
                new ErrorResponse(ce)),
            AuthenticationError ae => new UnauthorizedObjectResult(
                new ErrorResponse(ae)),
            AuthorizationError az => new ObjectResult(
                new ErrorResponse(az)) { StatusCode = 403 },
            _ => new ObjectResult(
                new ErrorResponse("INTERNAL_ERROR", "An unexpected error occurred")) 
                { StatusCode = 500 }
        };
    }
}
```

### Advanced Controller with Filters

```csharp
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize]
[ServiceFilter(typeof(LoggingActionFilter))]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly ICurrentUserService _currentUser;
    
    [HttpGet]
    [ProducesResponseType(typeof(PagedResponse<OrderSummaryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOrders(
        [FromQuery] OrderFilterRequest filter,
        [FromQuery] PagingRequest paging)
    {
        var criteria = new OrderSearchCriteria
        {
            CustomerId = _currentUser.UserId,
            Status = filter.Status,
            FromDate = filter.FromDate,
            ToDate = filter.ToDate,
            Page = paging.Page,
            PageSize = paging.PageSize
        };
        
        return await _orderService.SearchOrdersAsync(criteria)
            .MapAsync(results => new PagedResponse<OrderSummaryDto>
            {
                Items = results.Items.Select(o => new OrderSummaryDto(o)).ToList(),
                TotalCount = results.TotalCount,
                Page = results.Page,
                PageSize = results.PageSize,
                HasNextPage = results.HasNextPage,
                HasPreviousPage = results.HasPreviousPage
            })
            .ToActionResultAsync();
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationErrorResponse), StatusCodes.Status400BadRequest)]
    [ServiceFilter(typeof(TransactionActionFilter))]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
    {
        request.CustomerId = _currentUser.UserId;
        
        return await _orderService.CreateOrderAsync(request)
            .MapAsync(order => new OrderDto(order))
            .ToActionResultAsync(
                onSuccess: dto => CreatedAtAction(
                    nameof(GetOrder), 
                    new { id = dto.Id }, 
                    dto));
    }
    
    [HttpPost("{id:guid}/cancel")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CancelOrder(Guid id, [FromBody] CancelOrderRequest request)
    {
        return await _orderService.CancelOrderAsync(id, request.Reason)
            .ToActionResultAsync(onSuccess: () => Ok(new { message = "Order cancelled successfully" }));
    }
}
```

## Response Mapping

### Standard Response Models

```csharp
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public T Data { get; set; }
    public ErrorInfo Error { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string RequestId { get; set; }
}

public class ErrorResponse
{
    public string Code { get; set; }
    public string Message { get; set; }
    public Dictionary<string, object> Details { get; set; }
    
    public ErrorResponse(Error error)
    {
        Code = error.Code;
        Message = error.Message;
        Details = error.Metadata?.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }
}

public class ValidationErrorResponse : ErrorResponse
{
    public List<FieldError> Errors { get; set; }
    
    public ValidationErrorResponse(ValidationError error) : base(error)
    {
        Errors = ExtractFieldErrors(error);
    }
    
    private List<FieldError> ExtractFieldErrors(ValidationError error)
    {
        if (error is AggregateError aggregate)
        {
            return aggregate.Errors
                .OfType<ValidationError>()
                .SelectMany(e => ExtractFieldErrors(e))
                .ToList();
        }
        
        return new List<FieldError>
        {
            new FieldError
            {
                Field = error.Metadata?.GetValueOrDefault("field")?.ToString() ?? "general",
                Message = error.Message,
                Code = error.Code
            }
        };
    }
}

public class PagedResponse<T>
{
    public List<T> Items { get; set; }
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public bool HasNextPage { get; set; }
    public bool HasPreviousPage { get; set; }
    
    public static PagedResponse<T> From<TSource>(
        PagedResult<TSource> source, 
        Func<TSource, T> mapper)
    {
        return new PagedResponse<T>
        {
            Items = source.Items.Select(mapper).ToList(),
            TotalCount = source.TotalCount,
            Page = source.Page,
            PageSize = source.PageSize,
            HasNextPage = source.HasNextPage,
            HasPreviousPage = source.HasPreviousPage
        };
    }
}
```

### Response Factory

```csharp
public static class ApiResponseFactory
{
    public static IActionResult Success<T>(T data, int statusCode = 200)
    {
        var response = new ApiResponse<T>
        {
            Success = true,
            Data = data,
            RequestId = GetRequestId()
        };
        
        return new ObjectResult(response) { StatusCode = statusCode };
    }
    
    public static IActionResult Error(Error error)
    {
        var response = new ApiResponse<object>
        {
            Success = false,
            Error = new ErrorInfo(error),
            RequestId = GetRequestId()
        };
        
        var statusCode = GetStatusCodeForError(error);
        return new ObjectResult(response) { StatusCode = statusCode };
    }
    
    public static IActionResult FromResult<T>(Result<T> result)
    {
        return result.Match(
            onSuccess: data => Success(data),
            onFailure: error => Error(error));
    }
    
    private static int GetStatusCodeForError(Error error)
    {
        return error switch
        {
            ValidationError => StatusCodes.Status400BadRequest,
            NotFoundError => StatusCodes.Status404NotFound,
            ConflictError => StatusCodes.Status409Conflict,
            AuthenticationError => StatusCodes.Status401Unauthorized,
            AuthorizationError => StatusCodes.Status403Forbidden,
            _ => StatusCodes.Status500InternalServerError
        };
    }
}
```

## Error Handling

### Global Error Handler

```csharp
public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;
    private readonly IWebHostEnvironment _environment;
    
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
        var response = new ApiResponse<object>
        {
            Success = false,
            Error = new ErrorInfo(error),
            RequestId = context.TraceIdentifier
        };
        
        if (_environment.IsDevelopment())
        {
            response.Error.StackTrace = exception.StackTrace;
        }
        
        context.Response.StatusCode = GetStatusCodeForException(exception);
        context.Response.ContentType = "application/json";
        
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
    
    private Error CreateErrorFromException(Exception exception)
    {
        return exception switch
        {
            BusinessException be => be.Error,
            TimeoutException => new Error("TIMEOUT", "The operation timed out"),
            OperationCanceledException => new Error("CANCELLED", "The operation was cancelled"),
            _ => new Error("INTERNAL_ERROR", "An unexpected error occurred")
        };
    }
}

// Exception that wraps an Error
public class BusinessException : Exception
{
    public Error Error { get; }
    
    public BusinessException(Error error) : base(error.Message)
    {
        Error = error;
    }
}
```

### Problem Details Response

```csharp
public class ProblemDetailsFactory
{
    public static IActionResult CreateProblemDetails(Error error, HttpContext context)
    {
        var problemDetails = new ProblemDetails
        {
            Status = GetStatusCode(error),
            Type = GetProblemType(error),
            Title = GetTitle(error),
            Detail = error.Message,
            Instance = context.Request.Path
        };
        
        // Add extensions
        problemDetails.Extensions["code"] = error.Code;
        problemDetails.Extensions["traceId"] = context.TraceIdentifier;
        
        if (error.Metadata?.Any() == true)
        {
            problemDetails.Extensions["metadata"] = error.Metadata;
        }
        
        if (error is ValidationError validationError)
        {
            problemDetails.Extensions["errors"] = ExtractValidationErrors(validationError);
        }
        
        return new ObjectResult(problemDetails)
        {
            StatusCode = problemDetails.Status,
            ContentTypes = { "application/problem+json" }
        };
    }
    
    private static string GetProblemType(Error error)
    {
        return error switch
        {
            ValidationError => "https://example.com/errors/validation",
            NotFoundError => "https://example.com/errors/not-found",
            ConflictError => "https://example.com/errors/conflict",
            _ => "https://example.com/errors/internal"
        };
    }
}
```

## Request Validation

### Model Validation with FluentValidation

```csharp
public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    private readonly IUserService _userService;
    
    public CreateUserRequestValidator(IUserService userService)
    {
        _userService = userService;
        
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format")
            .MustAsync(BeUniqueEmail).WithMessage("Email already exists");
            
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters")
            .Matches("[A-Z]").WithMessage("Password must contain uppercase letter")
            .Matches("[a-z]").WithMessage("Password must contain lowercase letter")
            .Matches("[0-9]").WithMessage("Password must contain digit");
            
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(50).WithMessage("First name too long");
            
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(50).WithMessage("Last name too long");
    }
    
    private async Task<bool> BeUniqueEmail(string email, CancellationToken cancellationToken)
    {
        var result = await _userService.CheckEmailAvailabilityAsync(email);
        return result.IsSuccess && result.Value;
    }
}
```

### Validation Action Filter

```csharp
public class ValidationActionFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(
        ActionExecutingContext context, 
        ActionExecutionDelegate next)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .Select(x => new FieldError
                {
                    Field = x.Key,
                    Messages = x.Value.Errors.Select(e => e.ErrorMessage).ToList()
                })
                .ToList();
                
            var response = new ValidationErrorResponse
            {
                Code = "VALIDATION_ERROR",
                Message = "One or more validation errors occurred",
                Errors = errors
            };
            
            context.Result = new BadRequestObjectResult(response);
            return;
        }
        
        await next();
    }
}
```

### Custom Model Binder for Result Types

```csharp
public class ResultModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var valueProviderResult = bindingContext.ValueProvider
            .GetValue(bindingContext.ModelName);
            
        if (valueProviderResult == ValueProviderResult.None)
        {
            return Task.CompletedTask;
        }
        
        bindingContext.ModelState.SetModelValue(
            bindingContext.ModelName, 
            valueProviderResult);
            
        var value = valueProviderResult.FirstValue;
        
        // Parse and validate the value
        var result = ParseValue(value, bindingContext.ModelType);
        
        if (result.IsSuccess)
        {
            bindingContext.Result = ModelBindingResult.Success(result.Value);
        }
        else
        {
            bindingContext.ModelState.TryAddModelError(
                bindingContext.ModelName, 
                result.Error.Message);
            bindingContext.Result = ModelBindingResult.Failed();
        }
        
        return Task.CompletedTask;
    }
    
    private Result<object> ParseValue(string value, Type targetType)
    {
        // Implementation depends on target type
        return Result.Try(() => Convert.ChangeType(value, targetType));
    }
}
```

## API Versioning

### Version-Aware Controllers

```csharp
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public abstract class VersionedControllerBase : ControllerBase
{
    protected string ApiVersion => HttpContext.GetRequestedApiVersion()?.ToString() ?? "1.0";
}

[ApiVersion("1.0")]
[ApiVersion("2.0")]
public class ProductsController : VersionedControllerBase
{
    private readonly IProductService _productService;
    
    [HttpGet("{id:guid}")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> GetProductV1(Guid id)
    {
        return await _productService.GetProductAsync(id)
            .MapAsync(product => new ProductDtoV1(product))
            .ToActionResultAsync();
    }
    
    [HttpGet("{id:guid}")]
    [MapToApiVersion("2.0")]
    public async Task<IActionResult> GetProductV2(Guid id)
    {
        return await _productService.GetProductAsync(id)
            .MapAsync(product => new ProductDtoV2(product))
            .ToActionResultAsync();
    }
    
    [HttpPost]
    [MapToApiVersion("2.0")]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequestV2 request)
    {
        // V2 has additional validation
        var validationResult = ValidateV2Request(request);
        if (validationResult.IsFailure)
            return validationResult.Error.ToActionResult();
            
        return await _productService.CreateProductAsync(request)
            .MapAsync(product => new ProductDtoV2(product))
            .ToActionResultAsync();
    }
}
```

### API Version Error Handler

```csharp
public class ApiVersionErrorResponseProvider : IErrorResponseProvider
{
    public IActionResult CreateResponse(ErrorResponseContext context)
    {
        var error = context.ErrorCode switch
        {
            "UnsupportedApiVersion" => new Error(
                "UNSUPPORTED_VERSION", 
                $"API version '{context.Message}' is not supported"),
            "InvalidApiVersion" => new Error(
                "INVALID_VERSION", 
                "The specified API version is invalid"),
            "AmbiguousApiVersion" => new Error(
                "AMBIGUOUS_VERSION", 
                "Multiple API versions were specified"),
            "ApiVersionUnspecified" => new Error(
                "VERSION_REQUIRED", 
                "An API version is required"),
            _ => new Error("VERSION_ERROR", context.Message)
        };
        
        return error.ToActionResult();
    }
}
```

## Authentication and Authorization

### JWT Authentication with Result

```csharp
public interface IAuthenticationService
{
    Result<AuthenticationResult> Authenticate(LoginRequest request);
    Result<AuthenticationResult> RefreshToken(string refreshToken);
    Result ValidateToken(string token);
}

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthenticationService _authService;
    
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        return await _authService.AuthenticateAsync(request)
            .MapAsync(result => new LoginResponse
            {
                AccessToken = result.AccessToken,
                RefreshToken = result.RefreshToken,
                ExpiresIn = result.ExpiresIn,
                TokenType = "Bearer"
            })
            .ToActionResultAsync();
    }
    
    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        return await _authService.RefreshTokenAsync(request.RefreshToken)
            .MapAsync(result => new LoginResponse
            {
                AccessToken = result.AccessToken,
                RefreshToken = result.RefreshToken,
                ExpiresIn = result.ExpiresIn,
                TokenType = "Bearer"
            })
            .ToActionResultAsync();
    }
}
```

### Authorization Handler

```csharp
public class ResourceAuthorizationHandler : AuthorizationHandler<ResourceRequirement, IResource>
{
    private readonly IAuthorizationService _authService;
    
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        ResourceRequirement requirement,
        IResource resource)
    {
        var userId = context.User.GetUserId();
        if (!userId.HasValue)
        {
            context.Fail();
            return Task.CompletedTask;
        }
        
        var authResult = _authService.CanAccessResource(userId.Value, resource, requirement.Permission);
        
        authResult.Match(
            onSuccess: allowed =>
            {
                if (allowed)
                    context.Succeed(requirement);
                else
                    context.Fail();
            },
            onFailure: error =>
            {
                // Log the error
                context.Fail();
            });
            
        return Task.CompletedTask;
    }
}

public class ResourceAuthorizationAttribute : TypeFilterAttribute
{
    public ResourceAuthorizationAttribute(string permission) 
        : base(typeof(ResourceAuthorizationFilter))
    {
        Arguments = new object[] { permission };
    }
}

public class ResourceAuthorizationFilter : IAsyncAuthorizationFilter
{
    private readonly string _permission;
    private readonly IAuthorizationService _authService;
    
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;
        if (!user.Identity.IsAuthenticated)
        {
            context.Result = new UnauthorizedResult();
            return;
        }
        
        // Extract resource from route
        var resourceId = context.RouteData.Values["id"]?.ToString();
        if (string.IsNullOrEmpty(resourceId))
        {
            context.Result = new BadRequestResult();
            return;
        }
        
        var authResult = await _authService.AuthorizeResourceAccessAsync(
            user.GetUserId().Value, 
            resourceId, 
            _permission);
            
        if (authResult.IsFailure)
        {
            context.Result = authResult.Error.ToActionResult();
        }
    }
}
```

## Middleware Integration

### Result-Aware Middleware

```csharp
public class ResultLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ResultLoggingMiddleware> _logger;
    
    public async Task InvokeAsync(HttpContext context)
    {
        // Capture the original response body
        var originalBodyStream = context.Response.Body;
        
        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;
        
        await _next(context);
        
        // Log based on status code
        if (context.Response.StatusCode >= 400)
        {
            await LogErrorResponse(context, responseBody);
        }
        else if (context.Response.StatusCode >= 200 && context.Response.StatusCode < 300)
        {
            await LogSuccessResponse(context, responseBody);
        }
        
        // Copy the response body back
        responseBody.Seek(0, SeekOrigin.Begin);
        await responseBody.CopyToAsync(originalBodyStream);
    }
    
    private async Task LogErrorResponse(HttpContext context, MemoryStream responseBody)
    {
        responseBody.Seek(0, SeekOrigin.Begin);
        var responseText = await new StreamReader(responseBody).ReadToEndAsync();
        
        _logger.LogWarning(
            "Request {Method} {Path} failed with status {StatusCode}. Response: {Response}",
            context.Request.Method,
            context.Request.Path,
            context.Response.StatusCode,
            responseText);
            
        responseBody.Seek(0, SeekOrigin.Begin);
    }
}
```

### Request/Response Transformation

```csharp
public class ResultTransformationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IResultTransformer _transformer;
    
    public async Task InvokeAsync(HttpContext context)
    {
        // Transform request if needed
        if (context.Request.ContentType?.Contains("application/json") == true)
        {
            await TransformRequest(context);
        }
        
        // Capture response for transformation
        var originalBody = context.Response.Body;
        using var newBody = new MemoryStream();
        context.Response.Body = newBody;
        
        await _next(context);
        
        // Transform response
        if (context.Response.ContentType?.Contains("application/json") == true)
        {
            await TransformResponse(context, originalBody, newBody);
        }
        else
        {
            newBody.Seek(0, SeekOrigin.Begin);
            await newBody.CopyToAsync(originalBody);
        }
    }
    
    private async Task TransformResponse(
        HttpContext context, 
        Stream originalBody, 
        MemoryStream capturedBody)
    {
        capturedBody.Seek(0, SeekOrigin.Begin);
        var response = await JsonSerializer.DeserializeAsync<object>(capturedBody);
        
        var transformed = _transformer.TransformResponse(response, context);
        
        var json = JsonSerializer.Serialize(transformed);
        var buffer = Encoding.UTF8.GetBytes(json);
        
        context.Response.ContentLength = buffer.Length;
        await originalBody.WriteAsync(buffer);
    }
}
```

## OpenAPI Documentation

### Swagger Configuration

```csharp
public static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerWithFluentUnions(
        this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "API Documentation",
                Version = "v1",
                Description = "API with FluentUnions error handling"
            });
            
            // Add Result type handling
            options.MapType<Result>(() => new OpenApiSchema
            {
                Type = "object",
                Properties = new Dictionary<string, OpenApiSchema>
                {
                    ["isSuccess"] = new() { Type = "boolean" },
                    ["error"] = new() { Type = "object", Nullable = true }
                }
            });
            
            // Add custom operation filter
            options.OperationFilter<ResultOperationFilter>();
            
            // Add security definitions
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme."
            });
            
            // Include XML comments
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);
        });
        
        return services;
    }
}

public class ResultOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Add common error responses
        operation.Responses.TryAdd("400", new OpenApiResponse
        {
            Description = "Bad Request - Validation Error",
            Content = new Dictionary<string, OpenApiMediaType>
            {
                ["application/json"] = new()
                {
                    Schema = context.SchemaGenerator.GenerateSchema(
                        typeof(ValidationErrorResponse), 
                        context.SchemaRepository)
                }
            }
        });
        
        operation.Responses.TryAdd("401", new OpenApiResponse
        {
            Description = "Unauthorized - Authentication Required"
        });
        
        operation.Responses.TryAdd("403", new OpenApiResponse
        {
            Description = "Forbidden - Insufficient Permissions"
        });
        
        operation.Responses.TryAdd("404", new OpenApiResponse
        {
            Description = "Not Found",
            Content = new Dictionary<string, OpenApiMediaType>
            {
                ["application/json"] = new()
                {
                    Schema = context.SchemaGenerator.GenerateSchema(
                        typeof(ErrorResponse), 
                        context.SchemaRepository)
                }
            }
        });
        
        operation.Responses.TryAdd("500", new OpenApiResponse
        {
            Description = "Internal Server Error"
        });
    }
}
```

### API Documentation Attributes

```csharp
/// <summary>
/// User management endpoints
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class UsersController : ControllerBase
{
    /// <summary>
    /// Get a user by ID
    /// </summary>
    /// <param name="id">The user's unique identifier</param>
    /// <returns>The user details</returns>
    /// <response code="200">Returns the user details</response>
    /// <response code="404">User not found</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUser(Guid id)
    {
        return await _userService.GetUserAsync(id)
            .MapAsync(user => new UserDto(user))
            .ToActionResultAsync();
    }
    
    /// <summary>
    /// Create a new user
    /// </summary>
    /// <param name="request">User creation details</param>
    /// <returns>The created user</returns>
    /// <response code="201">User created successfully</response>
    /// <response code="400">Validation error</response>
    /// <response code="409">Email already exists</response>
    [HttpPost]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
    {
        return await _userService.CreateUserAsync(request)
            .MapAsync(user => new UserDto(user))
            .ToActionResultAsync(
                onSuccess: dto => CreatedAtAction(nameof(GetUser), new { id = dto.Id }, dto));
    }
}
```

## Best Practices

### 1. Consistent Error Responses

```csharp
// Always return consistent error structure
public class StandardErrorResponse
{
    public string Code { get; set; }
    public string Message { get; set; }
    public Dictionary<string, object> Details { get; set; }
    public string TraceId { get; set; }
    public DateTime Timestamp { get; set; }
}

// Use factory methods for consistency
public static class ErrorResponses
{
    public static IActionResult ValidationError(ModelStateDictionary modelState)
    {
        var response = new ValidationErrorResponse
        {
            Code = "VALIDATION_ERROR",
            Message = "Validation failed",
            Errors = ExtractErrors(modelState),
            TraceId = Activity.Current?.Id,
            Timestamp = DateTime.UtcNow
        };
        
        return new BadRequestObjectResult(response);
    }
}
```

### 2. Use Action Filters for Cross-Cutting Concerns

```csharp
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ValidateResultAttribute : ActionFilterAttribute
{
    public override void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Result is ObjectResult objectResult &&
            objectResult.Value is IResult result &&
            result.IsFailure)
        {
            context.Result = result.Error.ToActionResult();
        }
    }
}
```

### 3. Document All Possible Responses

```csharp
[ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
[ProducesResponseType(typeof(ValidationErrorResponse), StatusCodes.Status400BadRequest)]
[ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
[ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
[ProducesDefaultResponseType(typeof(ErrorResponse))]
public async Task<IActionResult> UpdateOrder(Guid id, UpdateOrderRequest request)
{
    // Implementation
}
```

### 4. Handle Async Operations Properly

```csharp
public async Task<IActionResult> ProcessAsync(ProcessRequest request)
{
    // Don't block on async operations
    return await _service.ProcessAsync(request)
        .BindAsync(async result => await _repository.SaveAsync(result))
        .MapAsync(saved => new ProcessResponse(saved))
        .ToActionResultAsync();
}
```

### 5. Validate at API Boundaries

```csharp
public class ApiValidationService
{
    public Result<T> ValidateAndTransform<T>(object input) where T : class
    {
        // Validate input
        var validationResult = Validate(input);
        if (validationResult.IsFailure)
            return Result.Failure<T>(validationResult.Error);
            
        // Transform to domain model
        var transformed = Transform<T>(input);
        if (transformed == null)
            return new ValidationError("Invalid input format");
            
        return Result.Success(transformed);
    }
}
```

## Summary

API patterns with FluentUnions provide:

1. **Consistent error handling** - All endpoints return predictable error structures
2. **Type-safe responses** - Result types ensure proper error handling
3. **Clean controllers** - Business logic separated from HTTP concerns
4. **Testable APIs** - Easy to test success and failure scenarios
5. **Rich documentation** - OpenAPI/Swagger integration with error details

Key principles:
- Map domain errors to appropriate HTTP status codes
- Provide rich error information for debugging
- Use middleware for cross-cutting concerns
- Document all possible responses
- Validate at API boundaries

Next steps:
- [Testing Guide](../guides/testing-guide.md)
- [Integration Examples](../integration/aspnet-core.md)
- [Performance Guide](../guides/performance-best-practices.md)
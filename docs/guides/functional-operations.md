# Functional Operations Reference

This guide provides a comprehensive reference for all functional operations available in FluentUnions, including Map, Bind, Match, and more.

## Table of Contents
1. [Core Operations Overview](#core-operations-overview)
2. [Map Operations](#map-operations)
3. [Bind Operations](#bind-operations)
4. [Match Operations](#match-operations)
5. [Filter and Ensure Operations](#filter-and-ensure-operations)
6. [Combine Operations](#combine-operations)
7. [Side Effect Operations](#side-effect-operations)
8. [Conversion Operations](#conversion-operations)
9. [Recovery Operations](#recovery-operations)
10. [Async Operations](#async-operations)

## Core Operations Overview

### Operation Categories

| Operation | Purpose | Changes Value | Changes Type | Can Fail |
|-----------|---------|---------------|--------------|----------|
| Map | Transform success value | Yes | Yes | No |
| Bind | Chain operations that return Result/Option | Yes | Yes | Yes |
| Match | Handle both cases | Yes | Yes | No |
| Filter/Ensure | Add conditions | No | No | Yes |
| Tap | Side effects | No | No | No |
| OrElse | Provide alternatives | Possibly | No | Possibly |

### Result vs Option Operations

Most operations are available for both Result and Option types with similar semantics:

```csharp
// Result operations work with success/failure
Result<int> result = GetNumber()
    .Map(n => n * 2)           // Transform if success
    .Bind(n => Divide(n, 2))   // Chain if success
    .Match(n => n, err => 0);  // Handle both cases

// Option operations work with some/none
Option<int> option = GetOptionalNumber()
    .Map(n => n * 2)           // Transform if some
    .Bind(n => TryParse(n))    // Chain if some
    .Match(n => n, () => 0);   // Handle both cases
```

## Map Operations

Map transforms the inner value without changing the Result/Option state.

### Basic Map

```csharp
// Result<T> -> Result<U>
Result<string> userNameResult = GetUser(id)
    .Map(user => user.Name);

// Option<T> -> Option<U>
Option<int> lengthOption = GetString()
    .Map(str => str.Length);
```

### Chained Maps

```csharp
Result<OrderSummary> summary = GetOrder(id)
    .Map(order => order.Items)
    .Map(items => items.Sum(i => i.Price))
    .Map(total => new OrderSummary { Total = total });
```

### Map with Complex Transformations

```csharp
// Transform to different type
Result<UserDto> dto = GetUser(id)
    .Map(user => new UserDto
    {
        Id = user.Id,
        FullName = $"{user.FirstName} {user.LastName}",
        Age = CalculateAge(user.BirthDate),
        MembershipLevel = DetermineMembershipLevel(user)
    });

// Map with multiple steps
Option<Address> formattedAddress = GetAddress(id)
    .Map(addr => NormalizeAddress(addr))
    .Map(addr => ValidatePostalCode(addr))
    .Map(addr => FormatForDisplay(addr));
```

### MapError (Result only)

Transform error information:

```csharp
Result<User> result = GetUser(id)
    .MapError(error => new Error(
        error.Code,
        $"User operation failed: {error.Message}"
    ));

// Add context to errors
Result<Order> enrichedResult = ProcessOrder(request)
    .MapError(error => error
        .WithMetadata("orderId", request.OrderId)
        .WithMetadata("timestamp", DateTime.UtcNow));
```

## Bind Operations

Bind chains operations that themselves return Result/Option.

### Basic Bind

```csharp
// Chain Result-returning operations
Result<Invoice> invoice = GetOrder(orderId)
    .Bind(order => GetCustomer(order.CustomerId))
    .Bind(customer => GenerateInvoice(orderId, customer));

// Chain Option-returning operations
Option<Settings> settings = GetUser(userId)
    .Bind(user => user.Profile)
    .Bind(profile => profile.Settings);
```

### Bind with Different Types

```csharp
// Each operation returns different type
Result<EmailSent> result = GetUser(userId)              // Result<User>
    .Bind(user => ValidateEmail(user.Email))           // Result<ValidEmail>
    .Bind(email => LoadTemplate("welcome"))            // Result<Template>
    .Bind(template => SendEmail(email, template));     // Result<EmailSent>
```

### Bind vs Map

```csharp
// Use Map when operation doesn't return Result/Option
Result<int> doubled = GetNumber()
    .Map(n => n * 2);  // Returns int, not Result<int>

// Use Bind when operation returns Result/Option
Result<int> divided = GetNumber()
    .Bind(n => Divide(n, 2));  // Returns Result<int>

// Avoid nested Results
Result<Result<int>> nested = GetNumber()
    .Map(n => Divide(n, 2));  // Wrong! Creates Result<Result<int>>

Result<int> flat = GetNumber()
    .Bind(n => Divide(n, 2));  // Correct! Flattens to Result<int>
```

### BindAll - Multiple Results

Combine multiple Results with different types:

```csharp
// All must succeed
public Result<Order> CreateOrder(Guid customerId, List<Guid> productIds)
{
    return Result.BindAll(
        GetCustomer(customerId),         // Result<Customer>
        GetProducts(productIds),         // Result<List<Product>>
        GetShippingOptions(customerId)   // Result<ShippingOptions>
    )
    .Map((customer, products, shipping) => 
        new Order(customer, products, shipping));
}

// With 2 values
Result<Summary> summary = Result.BindAll(
    CalculateSubtotal(items),
    CalculateTax(items)
).Map((subtotal, tax) => new Summary(subtotal, tax));

// Up to 8 values supported
Result<Report> report = Result.BindAll(
    GetSales(), GetExpenses(), GetInventory(),
    GetCustomers(), GetSuppliers(), GetEmployees(),
    GetRevenue(), GetProfits()
).Map((s, e, i, c, sup, emp, r, p) => new Report(...));
```

## Match Operations

Match provides exhaustive pattern matching for Result/Option.

### Basic Match

```csharp
// Result match
string message = result.Match(
    onSuccess: value => $"Success: {value}",
    onFailure: error => $"Error: {error.Message}"
);

// Option match
int count = option.Match(
    onSome: list => list.Count,
    onNone: () => 0
);
```

### Match with Different Return Types

```csharp
// Return IActionResult based on Result
IActionResult response = userResult.Match<IActionResult>(
    onSuccess: user => Ok(user),
    onFailure: error => error switch
    {
        NotFoundError => NotFound(error.Message),
        ValidationError => BadRequest(error.Message),
        _ => StatusCode(500, "Internal error")
    }
);

// Return different types from Option
object display = userOption.Match<object>(
    onSome: user => new UserViewModel(user),
    onNone: () => "No user found"
);
```

### Match with Side Effects

```csharp
// Execute different actions
result.Match(
    onSuccess: value => {
        _logger.LogInfo($"Operation succeeded: {value}");
        _metrics.IncrementSuccess();
    },
    onFailure: error => {
        _logger.LogError($"Operation failed: {error}");
        _metrics.IncrementFailure();
        _alertService.SendAlert(error);
    }
);
```

### Async Match

```csharp
// Async match operations
await result.MatchAsync(
    onSuccess: async value => await ProcessSuccess(value),
    onFailure: async error => await HandleError(error)
);

// Match with async and sync handlers
var response = await result.MatchAsync(
    onSuccess: value => Task.FromResult(Ok(value)),
    onFailure: async error => await LogAndReturnError(error)
);
```

## Filter and Ensure Operations

### Filter (Option only)

Filter Some values based on a predicate:

```csharp
// Basic filter
Option<User> activeUser = GetUser(id)
    .Filter(u => u.IsActive);

// Filter with custom none message
Option<Product> available = GetProduct(id)
    .Filter(p => p.InStock, "Product out of stock");

// Chained filters
Option<Order> validOrder = GetOrder(id)
    .Filter(o => o.Status == OrderStatus.Pending)
    .Filter(o => o.Total > 0)
    .Filter(o => o.Items.Any());
```

### Ensure (Result only)

Add validation conditions to Results:

```csharp
// Basic ensure
Result<Order> validatedOrder = GetOrder(id)
    .Ensure(o => o.Total > 0, "Order total must be positive")
    .Ensure(o => o.Items.Any(), "Order must have items");

// Ensure with custom errors
Result<User> validUser = GetUser(id)
    .Ensure(u => u.EmailVerified, 
            new ValidationError("Email not verified"))
    .Ensure(u => !u.IsLocked, 
            new AuthorizationError("Account is locked"));

// Conditional ensure
Result<Document> document = GetDocument(id)
    .Ensure(d => d.IsPublic || d.OwnerId == currentUserId,
            new AuthorizationError("Access denied"));
```

### Where (LINQ alias)

```csharp
// Option.Where is alias for Filter
var query = from user in GetUser(id)
            where user.IsActive
            where user.Age >= 18
            select user;

// Equivalent to
var filtered = GetUser(id)
    .Filter(u => u.IsActive)
    .Filter(u => u.Age >= 18);
```

## Combine Operations

### Result.Combine

Combine multiple Results into one:

```csharp
// Combine unit Results
Result combined = Result.Combine(
    ValidateName(name),
    ValidateEmail(email),
    ValidateAge(age)
);

// Combine Results with same value type
Result<List<User>> users = Result.Combine(
    GetUser(id1),
    GetUser(id2),
    GetUser(id3)
).Map(array => array.ToList());

// From collection
IEnumerable<Result> validations = items.Select(Validate);
Result allValid = Result.Combine(validations);
```

### Option.Zip

Combine multiple Options:

```csharp
// Combine two Options
Option<(string, int)> combined = Option.Zip(
    GetName(),
    GetAge()
);

// Combine three Options
Option<User> user = Option.Zip(
    GetFirstName(),
    GetLastName(),
    GetEmail()
).Map(t => new User(t.Item1, t.Item2, t.Item3));

// All must be Some for result to be Some
```

### FirstOrNone / Coalesce

```csharp
// Get first Some value
Option<Config> config = Option.Coalesce(
    LoadUserConfig(),
    LoadProjectConfig(),
    LoadGlobalConfig()
);

// From collection
var sources = new[] { cache, database, fileSystem };
Option<Data> data = sources
    .Select(s => s.TryGetData(key))
    .FirstOrNone(opt => opt.IsSome);
```

## Side Effect Operations

### Tap

Execute side effects without changing the value:

```csharp
// Log success values
Result<User> result = GetUser(id)
    .Tap(user => _logger.LogInfo($"Found user: {user.Id}"))
    .Tap(user => _cache.Set(user.Id, user))
    .Tap(user => _metrics.RecordUserAccess(user.Id));

// Different taps for success/failure
Result<Order> order = ProcessOrder(request)
    .Tap(order => _logger.LogInfo($"Order {order.Id} processed"))
    .TapError(error => _logger.LogError($"Order failed: {error}"))
    .TapError(error => _alerting.SendAlert(error));
```

### Do (Option)

Execute side effects on Some values:

```csharp
Option<User> user = GetUser(id)
    .Do(u => Console.WriteLine($"User: {u.Name}"))
    .Do(u => _eventBus.Publish(new UserAccessedEvent(u.Id)));
```

### Finally

Execute cleanup regardless of success/failure:

```csharp
Result<string> ReadFile(string path)
{
    Stream stream = null;
    return Result.Try(() =>
    {
        stream = File.OpenRead(path);
        return ReadContent(stream);
    })
    .Finally(() => stream?.Dispose());
}
```

## Conversion Operations

### To Nullable

```csharp
// Option to nullable
Option<int> someOption = Option.Some(42);
int? nullable = someOption.ToNullable();

Option<string> noneOption = Option<string>.None;
string? nullString = noneOption.ToNullable(); // null
```

### To Result

```csharp
// Option to Result
Result<User> result = userOption
    .ToResult(() => new NotFoundError("User not found"));

// With simple error message
Result<Config> configResult = configOption
    .ToResult("Configuration not found");
```

### To Collections

```csharp
// Option to array
int[] array = Option.Some(42).ToArray();        // [42]
int[] empty = Option<int>.None.ToArray();       // []

// Option to list
List<string> list = Option.Some("hello").ToList();

// Flatten collection of Options
List<Option<int>> options = GetOptions();
List<int> values = options
    .Where(o => o.IsSome)
    .Select(o => o.Value)
    .ToList();

// Or using SelectMany
List<int> values2 = options
    .SelectMany(o => o.ToEnumerable())
    .ToList();
```

### GetValueOr

```csharp
// Provide default value
string name = GetName().GetValueOr("Anonymous");
int count = GetCount().GetValueOr(0);

// Provide default factory
User user = GetUser().GetValueOr(() => new GuestUser());
Config config = GetConfig().GetValueOr(() => LoadDefaultConfig());
```

## Recovery Operations

### OrElse

Provide alternative values or operations:

```csharp
// Result OrElse
Result<Data> data = LoadFromPrimary()
    .OrElse(() => LoadFromSecondary())
    .OrElse(() => LoadFromCache())
    .OrElse(() => Result.Success(DefaultData()));

// Option OrElse
Option<User> user = FindById(id)
    .OrElse(() => FindByEmail(email))
    .OrElse(() => FindByUsername(username));

// OrElse with value
string value = GetOptionalString()
    .OrElse("default value");
```

### Recover

Recover from specific errors:

```csharp
Result<User> recovered = GetUser(id)
    .Recover(error => error is NotFoundError 
        ? CreateDefaultUser() 
        : Result.Failure<User>(error));

// Pattern matching recovery
Result<Data> data = LoadData()
    .Recover(error => error switch
    {
        NetworkError => LoadCachedData(),
        TimeoutError => RetryLoad(),
        _ => Result.Failure<Data>(error)
    });
```

### Compensate

```csharp
Result<Order> order = ProcessOrder(request)
    .Compensate(async error =>
    {
        await LogError(error);
        await NotifyAdmin(error);
        
        if (error is PaymentError)
            return await RetryPayment(request);
            
        return Result.Failure<Order>(error);
    });
```

## Async Operations

### Async Extensions

Most operations have async counterparts:

```csharp
// MapAsync
Result<UserDto> dto = await GetUserAsync(id)
    .MapAsync(async user => await EnrichUserData(user))
    .MapAsync(async user => MapToDto(user));

// BindAsync
Result<Order> order = await ValidateRequestAsync(request)
    .BindAsync(async req => await CreateOrderAsync(req))
    .BindAsync(async ord => await SaveOrderAsync(ord));

// TapAsync
Result<Data> data = await LoadDataAsync()
    .TapAsync(async d => await CacheDataAsync(d))
    .TapErrorAsync(async e => await LogErrorAsync(e));
```

### Task<Result<T>> Extensions

```csharp
// Chain operations on Task<Result<T>>
Task<Result<User>> userTask = GetUserAsync(id);

Result<string> name = await userTask
    .Map(u => u.Name)
    .Bind(n => ValidateName(n));

// Async match
IActionResult response = await resultTask.Match(
    onSuccess: user => Ok(user),
    onFailure: error => BadRequest(error)
);
```

### Parallel Operations

```csharp
// Await multiple Results
var results = await Task.WhenAll(
    GetUserAsync(id1),
    GetUserAsync(id2),
    GetUserAsync(id3)
);

Result<List<User>> combined = Result.Combine(results)
    .Map(users => users.ToList());

// Parallel BindAll
Result<Dashboard> dashboard = await Result.BindAllAsync(
    GetStatsAsync(),      // Task<Result<Stats>>
    GetOrdersAsync(),     // Task<Result<List<Order>>>
    GetAlertsAsync()      // Task<Result<List<Alert>>>
).MapAsync((stats, orders, alerts) => 
    BuildDashboard(stats, orders, alerts));
```

## Operation Chaining Patterns

### Pipeline Pattern

```csharp
public static class ResultExtensions
{
    public static Result<T> Pipeline<T>(
        this Result<T> result,
        params Func<T, Result<T>>[] operations)
    {
        return operations.Aggregate(result, (r, op) => r.Bind(op));
    }
}

// Usage
var result = GetData()
    .Pipeline(
        Validate,
        Transform,
        Enrich,
        Save
    );
```

### Conditional Chaining

```csharp
public Result<User> UpdateUser(UpdateRequest request)
{
    return GetUser(request.UserId)
        .When(request.UpdateEmail, 
              user => UpdateEmail(user, request.Email))
        .When(request.UpdatePassword, 
              user => UpdatePassword(user, request.Password))
        .When(request.UpdateProfile, 
              user => UpdateProfile(user, request.Profile));
}

// Extension method
public static Result<T> When<T>(
    this Result<T> result,
    bool condition,
    Func<T, Result<T>> operation)
{
    return condition ? result.Bind(operation) : result;
}
```

### Retry Pattern

```csharp
public static async Task<Result<T>> RetryAsync<T>(
    this Func<Task<Result<T>>> operation,
    int maxAttempts = 3,
    int delayMs = 1000)
{
    for (int i = 0; i < maxAttempts; i++)
    {
        var result = await operation();
        if (result.IsSuccess) return result;
        
        if (i < maxAttempts - 1)
            await Task.Delay(delayMs * (i + 1));
    }
    
    return await operation(); // Last attempt
}

// Usage
var result = await (() => CallApiAsync()).RetryAsync(3, 1000);
```

## Summary

FluentUnions provides a comprehensive set of functional operations:

1. **Transformation** - Map values while preserving structure
2. **Composition** - Chain operations with Bind
3. **Pattern Matching** - Handle all cases with Match
4. **Filtering** - Add conditions with Filter/Ensure
5. **Combination** - Work with multiple Results/Options
6. **Side Effects** - Tap for logging and metrics
7. **Recovery** - OrElse and error handling
8. **Async Support** - Full async/await integration

These operations enable:
- Railway-oriented programming
- Functional error handling
- Composable business logic
- Clean, readable code

Next steps:
- [Combining Results Guide](combining-results.md)
- [Async Operations Guide](async-operations.md)
- [Testing Guide](testing-guide.md)
# Option Pattern Tutorial

This comprehensive tutorial covers everything you need to know about the Option pattern in FluentUnions, from basic concepts to advanced techniques.

## Table of Contents
1. [Introduction](#introduction)
2. [Creating Options](#creating-options)
3. [Checking Options](#checking-options)
4. [Transforming Options](#transforming-options)
5. [Filtering Options](#filtering-options)
6. [Combining Options](#combining-options)
7. [Converting Between Types](#converting-between-types)
8. [Advanced Patterns](#advanced-patterns)
9. [Best Practices](#best-practices)

## Introduction

The Option pattern represents a value that may or may not exist. It's a type-safe alternative to null references that makes the absence of values explicit.

### Why Use Option?

```csharp
// Traditional nullable approach
public User? FindUser(string email)
{
    return _users.FirstOrDefault(u => u.Email == email);
    // Null could mean: not found, error, or actual null
}

// Option approach
public Option<User> FindUser(string email)
{
    var user = _users.FirstOrDefault(u => u.Email == email);
    return Option.From(user); // Explicit: might not find a user
}
```

Benefits:
- No null reference exceptions
- Explicit handling of missing values
- Composable operations
- Self-documenting code

## Creating Options

### Basic Creation

```csharp
// Create Some (has value)
Option<int> someInt = Option.Some(42);
Option<string> someString = Option.Some("Hello");
Option<User> someUser = Option.Some(new User { Name = "Alice" });

// Create None (no value)
Option<int> noneInt = Option<int>.None;
Option<string> noneString = Option<string>.None;
Option<User> noneUser = Option<User>.None;
```

### From Nullable Values

```csharp
// From nullable reference types
string? nullableString = GetNullableString();
Option<string> option1 = Option.From(nullableString);

// From nullable value types
int? nullableInt = GetNullableInt();
Option<int> option2 = Option.From(nullableInt);

// Using extension method
DateTime? nullableDate = GetNullableDate();
Option<DateTime> option3 = nullableDate.AsOption();
```

### Conditional Creation

```csharp
// Create based on condition
Option<User> admin = Option.Some(user).Filter(u => u.Role == "Admin");

// Create from predicate
Option<int> positive = Option.When(value > 0, value);

// Create from try pattern
Option<int> parsed = Option.Try(() => int.Parse(input));
```

### Collection Extensions

```csharp
// FirstOrNone - safe alternative to First()
var users = new List<User> { /* ... */ };
Option<User> firstUser = users.FirstOrNone();
Option<User> adminUser = users.FirstOrNone(u => u.IsAdmin);

// SingleOrNone - safe alternative to Single()
Option<User> singleAdmin = users.SingleOrNone(u => u.IsAdmin);

// ElementAtOrNone - safe alternative to ElementAt()
Option<User> thirdUser = users.ElementAtOrNone(2);
```

## Checking Options

### Basic Checks

```csharp
var option = FindUser(email);

// Check if has value
if (option.IsSome)
{
    var user = option.Value;
    Console.WriteLine($"Found: {user.Name}");
}

// Check if empty
if (option.IsNone)
{
    Console.WriteLine("User not found");
}
```

### Pattern Matching

```csharp
// Match with different outcomes
string result = option.Match(
    onSome: user => $"Hello, {user.Name}!",
    onNone: () => "User not found"
);

// Match with side effects
option.Match(
    onSome: user => SendEmail(user),
    onNone: () => LogMissing()
);

// Match to different types
IActionResult response = option.Match<IActionResult>(
    onSome: user => Ok(user),
    onNone: () => NotFound()
);
```

### Switch Expressions (C# 8+)

```csharp
var message = option switch
{
    { IsSome: true } opt => $"Value: {opt.Value}",
    { IsNone: true } => "No value",
    _ => "Unknown" // Should never happen
};
```

### Safe Value Access

```csharp
// GetValueOr - provide default
string name = GetUserName(id).GetValueOr("Anonymous");
int count = GetCount().GetValueOr(0);

// GetValueOr with factory
User user = FindUser(id).GetValueOr(() => CreateGuestUser());

// TryGetValue pattern
if (option.TryGetValue(out var value))
{
    // Use value safely
    Console.WriteLine(value);
}
```

## Transforming Options

### Map - Transform Values

Map applies a function to the value if it exists:

```csharp
// Simple transformation
Option<string> name = FindUser(id).Map(u => u.Name);
Option<int> length = name.Map(n => n.Length);

// Chain multiple maps
Option<string> email = FindUser(id)
    .Map(u => u.Email)
    .Map(e => e.ToLower())
    .Map(e => e.Trim());

// Map to different types
Option<UserDto> dto = FindUser(id)
    .Map(user => new UserDto 
    { 
        Id = user.Id, 
        Name = user.Name 
    });
```

### Bind - Chain Operations

Bind chains operations that return Options:

```csharp
// Each operation might return None
Option<Address> address = FindUser(userId)
    .Bind(user => FindCompany(user.CompanyId))
    .Bind(company => GetHeadquarters(company))
    .Bind(hq => GetAddress(hq));

// Real-world example
public Option<EmailSettings> GetUserEmailSettings(Guid userId)
{
    return FindUser(userId)
        .Bind(user => GetProfile(user.Id))
        .Bind(profile => profile.EmailSettings);
}

// Avoid nested Options
Option<Option<string>> nested = GetOptionalValue(); // Bad
Option<string> flat = GetOptionalValue().Bind(v => v); // Good
```

### Select (LINQ Style)

```csharp
// Using LINQ syntax
var result = from user in FindUser(email)
             from profile in GetProfile(user.Id)
             from settings in profile.Settings
             select settings.Theme;

// Equivalent to
var result2 = FindUser(email)
    .Bind(user => GetProfile(user.Id))
    .Bind(profile => profile.Settings)
    .Map(settings => settings.Theme);
```

## Filtering Options

### Basic Filtering

```csharp
// Filter with predicate
Option<User> activeUser = FindUser(id)
    .Filter(u => u.IsActive);

// Filter with custom None reason
Option<User> adultUser = FindUser(id)
    .Filter(u => u.Age >= 18, "User must be 18 or older");

// Chain multiple filters
Option<Product> availableProduct = FindProduct(id)
    .Filter(p => p.InStock)
    .Filter(p => p.Price > 0)
    .Filter(p => !p.Discontinued);
```

### Where (LINQ Style)

```csharp
// Using Where (alias for Filter)
var premiumUsers = from user in FindUser(email)
                   where user.IsPremium
                   where user.IsActive
                   select user;
```

### Complex Filtering

```csharp
// Filter with multiple conditions
public Option<Order> GetEligibleOrder(Guid orderId)
{
    return FindOrder(orderId)
        .Filter(order => 
            order.Status == OrderStatus.Pending &&
            order.Total > 100 &&
            order.Items.Any() &&
            DateTime.Now - order.CreatedAt < TimeSpan.FromDays(30)
        );
}

// Filter with external validation
public Option<User> GetVerifiedUser(string email)
{
    return FindUser(email)
        .Filter(user => _validator.IsValid(user))
        .Filter(user => _authService.IsVerified(user));
}
```

## Combining Options

### OrElse - Provide Alternatives

```csharp
// Try primary, fallback to secondary
Option<Config> config = LoadUserConfig()
    .OrElse(() => LoadDefaultConfig())
    .OrElse(() => Option.Some(new Config()));

// OrElse with value
string username = GetUsername()
    .OrElse("Anonymous");

// Conditional alternatives
Option<User> user = FindUserById(id)
    .OrElse(() => FindUserByEmail(email))
    .OrElse(() => CreateGuestUser());
```

### Coalesce - First Non-None

```csharp
// Get first available value
Option<string> apiKey = Option.Coalesce(
    GetEnvironmentVariable("API_KEY"),
    GetConfigValue("apiKey"),
    GetDefaultApiKey()
);

// With collection
var sources = new[]
{
    TryCache(key),
    TryDatabase(key),
    TryRemote(key)
};
Option<Data> data = Option.Coalesce(sources);
```

### Zip - Combine Multiple Options

```csharp
// Combine two options
Option<(string, int)> combined = Option.Zip(
    GetName(),
    GetAge()
);

// Combine with transformation
Option<User> user = Option.Zip(
    GetFirstName(),
    GetLastName(),
    GetEmail()
).Map(t => new User(t.Item1, t.Item2, t.Item3));

// All must be Some for result to be Some
```

## Converting Between Types

### To Nullable

```csharp
// Convert to nullable
Option<int> option = Option.Some(42);
int? nullable = option.ToNullable();

// For reference types
Option<string> stringOption = Option.Some("Hello");
string? nullableString = stringOption.GetValueOrDefault();
```

### To Result

```csharp
// Convert with error
Option<User> userOption = FindUser(id);
Result<User> userResult = userOption
    .ToResult(() => new NotFoundError($"User {id} not found"));

// Convert with simple message
Result<Config> configResult = GetConfig()
    .ToResult("Configuration not found");

// Chain with Result operations
Result<Order> orderResult = FindUser(userId)
    .ToResult(() => new NotFoundError("User not found"))
    .Bind(user => CreateOrder(user));
```

### To Collections

```csharp
// To array (empty or single element)
Option<int> option = Option.Some(42);
int[] array = option.ToArray(); // [42]

// To list
List<string> list = GetOptionalString().ToList();

// To enumerable
IEnumerable<User> users = FindUser(id).ToEnumerable();

// Flatten collections of options
List<Option<int>> options = GetOptions();
List<int> values = options.SelectMany(o => o.ToEnumerable()).ToList();
```

## Advanced Patterns

### Async Operations

```csharp
// Async option operations
public async Task<Option<User>> FindUserAsync(Guid id)
{
    var user = await _repository.GetByIdAsync(id);
    return Option.From(user);
}

// Async bind
public async Task<Option<Order>> GetLatestOrderAsync(Guid userId)
{
    return await FindUserAsync(userId)
        .BindAsync(async user => await GetOrdersAsync(user.Id))
        .MapAsync(orders => orders.MaxBy(o => o.CreatedAt));
}

// Async match
await userOption.MatchAsync(
    onSome: async user => await SendEmailAsync(user),
    onNone: async () => await LogMissingAsync()
);
```

### Memoization Pattern

```csharp
public class UserCache
{
    private readonly Dictionary<Guid, Option<User>> _cache = new();
    
    public Option<User> GetUser(Guid id)
    {
        return _cache.TryGetValue(id, out var cached)
            ? cached
            : LoadUser(id).Tap(option => _cache[id] = option);
    }
    
    private Option<User> LoadUser(Guid id)
    {
        // Expensive operation
        var user = _database.FindUser(id);
        return Option.From(user);
    }
}
```

### Try Pattern

```csharp
// Safe parsing
Option<int> ParseInt(string input)
{
    return Option.Try(() => int.Parse(input));
}

// Safe dictionary access
Option<TValue> TryGet<TKey, TValue>(Dictionary<TKey, TValue> dict, TKey key)
{
    return dict.TryGetValue(key, out var value)
        ? Option.Some(value)
        : Option<TValue>.None;
}

// Safe casting
Option<T> TryCast<T>(object obj) where T : class
{
    return Option.From(obj as T);
}
```

### Builder Pattern

```csharp
public class UserBuilder
{
    private Option<string> _name = Option<string>.None;
    private Option<string> _email = Option<string>.None;
    private Option<int> _age = Option<int>.None;
    
    public UserBuilder WithName(string name) 
    { 
        _name = Option.Some(name); 
        return this; 
    }
    
    public UserBuilder WithEmail(string email) 
    { 
        _email = Option.Some(email); 
        return this; 
    }
    
    public UserBuilder WithAge(int age) 
    { 
        _age = Option.Some(age); 
        return this; 
    }
    
    public Option<User> Build()
    {
        return Option.Zip(_name, _email)
            .Map(t => new User 
            { 
                Name = t.Item1, 
                Email = t.Item2,
                Age = _age.GetValueOr(0)
            });
    }
}
```

### Optional Chaining

```csharp
// Safe navigation through object graph
public Option<string> GetUserCountryCode(Guid userId)
{
    return FindUser(userId)
        .Bind(u => Option.From(u.Address))
        .Bind(a => Option.From(a.Country))
        .Map(c => c.Code);
}

// With null-conditional operators
public Option<string> GetCompanyName(User user)
{
    return Option.From(user?.Company?.Name);
}
```

## Best Practices

### 1. Use Option for Truly Optional Values

```csharp
// Good: Value might not exist
public Option<User> FindUserByEmail(string email) { }

// Bad: Should always return a value
public Option<DateTime> GetCurrentTime() { } // Should return DateTime
```

### 2. Avoid Option<Option<T>>

```csharp
// Bad: Nested options
Option<Option<string>> nested = GetNestedOption();

// Good: Flatten with Bind
Option<string> flat = GetNestedOption().Bind(x => x);
```

### 3. Don't Use Option for Error Handling

```csharp
// Bad: Using None for errors
public Option<User> GetUser(Guid id)
{
    try
    {
        return Option.Some(_db.GetUser(id));
    }
    catch
    {
        return Option<User>.None; // Lost error information!
    }
}

// Good: Use Result for operations that can fail
public Result<User> GetUser(Guid id)
{
    try
    {
        return Result.Success(_db.GetUser(id));
    }
    catch (Exception ex)
    {
        return Result.Failure<User>($"Database error: {ex.Message}");
    }
}
```

### 4. Prefer Functional Composition

```csharp
// Bad: Imperative style
public Option<string> GetUserEmail(Guid id)
{
    var userOpt = FindUser(id);
    if (userOpt.IsNone) return Option<string>.None;
    
    var user = userOpt.Value;
    if (!user.IsActive) return Option<string>.None;
    
    return Option.Some(user.Email);
}

// Good: Functional style
public Option<string> GetUserEmail(Guid id)
{
    return FindUser(id)
        .Filter(u => u.IsActive)
        .Map(u => u.Email);
}
```

### 5. Use Meaningful Variable Names

```csharp
// Bad: Generic names
var opt = GetSomething();
var val = opt.GetValueOr("default");

// Good: Descriptive names
var userOption = FindUser(email);
var username = userOption.Map(u => u.Name).GetValueOr("Guest");
```

### 6. Consider Performance

```csharp
// Option<T> is a struct - no heap allocation
Option<int> option = Option.Some(42); // Stack allocated

// But be careful with large structs
Option<LargeStruct> large = Option.Some(largeValue); // Copies the struct

// Consider using Option<T> where T is a reference type for large data
Option<LargeClass> better = Option.Some(largeObject); // Only copies reference
```

## Common Patterns

### Configuration Pattern

```csharp
public class AppConfig
{
    private readonly Dictionary<string, string> _settings;
    
    public Option<string> GetSetting(string key)
    {
        return _settings.TryGetValue(key, out var value)
            ? Option.Some(value)
            : Option<string>.None;
    }
    
    public Option<int> GetIntSetting(string key)
    {
        return GetSetting(key).Bind(ParseInt);
    }
    
    public Option<Uri> GetUriSetting(string key)
    {
        return GetSetting(key)
            .Bind(s => Option.Try(() => new Uri(s)));
    }
}
```

### Cache Pattern

```csharp
public class CachedRepository<T> where T : class
{
    private readonly Dictionary<string, Option<T>> _cache = new();
    
    public Option<T> Get(string key)
    {
        if (_cache.TryGetValue(key, out var cached))
            return cached;
            
        var value = LoadFromSource(key);
        _cache[key] = value;
        return value;
    }
    
    public Option<T> Refresh(string key)
    {
        _cache.Remove(key);
        return Get(key);
    }
}
```

### Search Pattern

```csharp
public class SearchService
{
    public Option<SearchResult> Search(string query)
    {
        return TryLocalSearch(query)
            .OrElse(() => TryDatabaseSearch(query))
            .OrElse(() => TryRemoteSearch(query))
            .Filter(result => result.Items.Any());
    }
    
    public Option<T> FindFirst<T>(Func<T, bool> predicate, params Func<Option<T>>[] sources)
    {
        return sources
            .Select(source => source())
            .FirstOrNone(opt => opt.IsSome && predicate(opt.Value));
    }
}
```

## Summary

The Option pattern provides a robust way to handle optional values without null references. Key takeaways:

1. Use Option when values might legitimately not exist
2. Leverage functional operations (Map, Bind, Filter) for clean code
3. Avoid null reference exceptions with type safety
4. Compose operations for complex scenarios
5. Convert between Option, Result, and nullable types as needed

Next steps:
- [Error Handling Guide](error-handling.md)
- [Functional Operations Reference](../guides/functional-operations.md)
- [Real-World Examples](real-world-examples.md)
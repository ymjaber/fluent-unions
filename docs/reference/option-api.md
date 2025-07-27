# Option API Reference

This guide provides a comprehensive reference for the Option type and all its operations in FluentUnions.

## Table of Contents
1. [Option Type](#option-type)
2. [Creating Options](#creating-options)
3. [Checking Options](#checking-options)
4. [Transforming Options](#transforming-options)
5. [Filtering Options](#filtering-options)
6. [Combining Options](#combining-options)
7. [Converting Options](#converting-options)
8. [Async Operations](#async-operations)
9. [Collection Operations](#collection-operations)
10. [Advanced Patterns](#advanced-patterns)

## Option Type

### Option<T>
Represents a value that may or may not exist, without using null.

```csharp
public readonly struct Option<T>
{
    public bool IsSome { get; }
    public bool IsNone { get; }
    public T Value { get; }
}
```

## Creating Options

### Some and None

```csharp
// Create an Option with a value
Option<string> someName = Option.Some("John");
Option<int> someNumber = Option.Some(42);

// Create an empty Option
Option<string> noName = Option.None<string>();
Option<int> noNumber = Option<int>.None;

// Implicit conversion from value
Option<string> implicit = "Hello"; // Creates Some("Hello")
```

### From Nullable Values

```csharp
// From nullable reference type
string? nullableName = GetNullableName();
Option<string> nameOption = Option.FromNullable(nullableName);

// From nullable value type
int? nullableAge = GetNullableAge();
Option<int> ageOption = Option.FromNullable(nullableAge);

// With null-conditional operator
Option<string> length = Option.FromNullable(text?.Length.ToString());
```

### Conditional Creation

```csharp
// Create based on condition
Option<User> userOption = Option.SomeIf(user != null, user);

// Create with predicate
Option<int> positive = Option.SomeWhen(number, n => n > 0);

// From dictionary
var dict = new Dictionary<string, int> { ["key"] = 42 };
Option<int> value = Option.FromDictionary(dict, "key");
Option<int> missing = Option.FromDictionary(dict, "missing"); // None
```

### Try Pattern

```csharp
// Try to parse
Option<int> parsed = Option.TryParse<int>("123");
Option<DateTime> date = Option.TryParse<DateTime>("2023-01-01");

// Try with custom logic
Option<User> user = Option.Try(() => 
{
    var u = GetUserOrThrow();
    return u.IsValid ? u : null;
});
```

## Checking Options

### Basic Checks

```csharp
Option<User> userOption = GetOptionalUser();

// Check if has value
if (userOption.IsSome)
{
    Console.WriteLine($"User: {userOption.Value.Name}");
}

// Check if empty
if (userOption.IsNone)
{
    Console.WriteLine("No user found");
}

// Pattern matching (C# 8+)
var message = userOption switch
{
    { IsSome: true } user => $"Found: {user.Value.Name}",
    { IsNone: true } => "Not found",
    _ => "Unknown"
};
```

### Match Method

```csharp
// Transform based on Some/None
string message = userOption.Match(
    onSome: user => $"Hello, {user.Name}",
    onNone: () => "No user");

// Execute actions
userOption.Match(
    onSome: user => ProcessUser(user),
    onNone: () => ShowEmptyState());

// With return value
int id = userOption.Match(
    onSome: user => user.Id,
    onNone: () => -1);
```

### GetValueOr

```csharp
// Get value or default
User user = userOption.GetValueOr(User.Guest);

// Get value or compute default
User computed = userOption.GetValueOr(() => CreateGuestUser());

// Get value or throw
User required = userOption.GetValueOrThrow(
    new InvalidOperationException("User is required"));
```

## Transforming Options

### Map - Transform Value

```csharp
Option<User> userOption = GetOptionalUser();

// Transform the value if Some
Option<string> name = userOption.Map(user => user.Name);
Option<int> age = userOption.Map(user => user.Age);

// Chain transformations
Option<string> formatted = userOption
    .Map(user => user.Name)
    .Map(name => name.ToUpper())
    .Map(upper => $"User: {upper}");

// Map to different type
Option<UserDto> dto = userOption.Map(user => new UserDto
{
    Id = user.Id,
    Name = user.Name,
    Email = user.Email
});
```

### Bind - Chain Options

```csharp
// Chain operations that return Option
Option<Address> address = GetUser(userId)
    .Bind(user => GetAddress(user.AddressId));

// Multiple binds
Option<string> city = GetUser(userId)
    .Bind(user => GetAddress(user.AddressId))
    .Bind(addr => GetCity(addr.CityId))
    .Map(city => city.Name);

// Avoid nested Options
Option<User> user = GetOptionalUser();
Option<Option<Address>> wrong = user.Map(u => GetAddress(u.AddressId));
Option<Address> correct = user.Bind(u => GetAddress(u.AddressId));
```

### FlatMap (alias for Bind)

```csharp
// Same as Bind, different name
Option<Order> order = GetUser(userId)
    .FlatMap(user => GetLatestOrder(user.Id));
```

## Filtering Options

### Filter - Keep If Condition Met

```csharp
Option<User> user = GetOptionalUser();

// Basic filter
Option<User> activeUser = user.Filter(u => u.IsActive);

// Chain filters
Option<User> validUser = user
    .Filter(u => u.IsActive)
    .Filter(u => u.EmailVerified)
    .Filter(u => u.Age >= 18);

// Filter with message (for debugging)
Option<User> adult = user.Filter(
    u => u.Age >= 18,
    "User must be 18 or older");
```

### Where (LINQ-style filter)

```csharp
// Same as Filter, LINQ naming
Option<User> premium = user.Where(u => u.IsPremium);

// Use in LINQ query
var query = from u in GetOptionalUser()
            where u.IsActive
            where u.IsPremium
            select u.Email;
```

### FilterNot - Inverse Filter

```csharp
// Keep if condition is NOT met
Option<User> notBanned = user.FilterNot(u => u.IsBanned);
Option<User> available = user.FilterNot(u => u.IsDeleted || u.IsSuspended);
```

## Combining Options

### Zip - Combine Multiple Options

```csharp
// Combine 2 Options
Option<(User, Address)> combined = Option.Zip(
    GetOptionalUser(),
    GetOptionalAddress());

// Combine 3 Options
Option<(User, Address, Phone)> triple = Option.Zip(
    GetOptionalUser(),
    GetOptionalAddress(),
    GetOptionalPhone());

// With transformation
Option<Contact> contact = Option.Zip(
    GetOptionalUser(),
    GetOptionalAddress(),
    GetOptionalPhone())
    .Map((user, addr, phone) => new Contact(user, addr, phone));
```

### Or - Fallback Values

```csharp
// Provide alternative if None
Option<User> user = GetOptionalUser()
    .Or(Option.Some(User.Guest));

// Lazy alternative
Option<User> withFallback = GetOptionalUser()
    .Or(() => GetOptionalGuestUser());

// Chain multiple alternatives
Option<Config> config = LoadUserConfig()
    .Or(() => LoadDefaultConfig())
    .Or(() => Option.Some(Config.Minimal));
```

### And - Combine with Another Option

```csharp
// Returns second Option if first is Some
Option<User> user = GetOptionalUser();
Option<Premium> premium = GetPremiumStatus();

Option<Premium> userPremium = user.And(premium);
// Returns premium if user is Some, None otherwise
```

## Converting Options

### To Other Types

```csharp
Option<User> userOption = GetOptionalUser();

// To nullable
User? nullable = userOption.ToNullable();

// To Result
Result<User> result = userOption.ToResult(
    new NotFoundError("User not found"));

// To list (0 or 1 element)
List<User> list = userOption.ToList();
IEnumerable<User> enumerable = userOption.ToEnumerable();

// To array
User[] array = userOption.ToArray();
```

### From Result

```csharp
Result<User> userResult = GetUserResult();

// Convert Result to Option (loses error information)
Option<User> option = userResult.ToOption();

// Keep error information
var (option, error) = userResult.ToOptionWithError();
if (option.IsNone && error != null)
{
    Console.WriteLine($"Failed: {error.Message}");
}
```

## Async Operations

### Task<Option<T>> Extensions

```csharp
// Async map
Task<Option<string>> nameTask = GetUserAsync(userId)
    .MapAsync(user => user.Name);

// Async bind
Task<Option<Order>> orderTask = GetUserAsync(userId)
    .BindAsync(user => GetLatestOrderAsync(user.Id));

// Async filter
Task<Option<User>> activeTask = GetUserAsync(userId)
    .FilterAsync(async user => await IsActiveAsync(user));

// Async match
string message = await GetUserAsync(userId)
    .MatchAsync(
        onSome: async user => await FormatMessageAsync(user),
        onNone: () => Task.FromResult("No user"));
```

### Combining Async Options

```csharp
// Zip async Options
Task<Option<(User, Address)>> combined = Option.ZipAsync(
    GetUserAsync(userId),
    GetAddressAsync(addressId));

// Wait for first Some
Task<Option<User>> firstUser = Option.FirstSomeAsync(
    GetUserFromCacheAsync(userId),
    GetUserFromDatabaseAsync(userId),
    GetUserFromApiAsync(userId));
```

## Collection Operations

### Options of Collections

```csharp
// Filter and map collections
Option<List<User>> users = GetOptionalUsers();

Option<List<string>> names = users.Map(list => 
    list.Select(u => u.Name).ToList());

Option<List<User>> active = users.Map(list =>
    list.Where(u => u.IsActive).ToList());

// Check if any/all
Option<bool> hasAdmin = users.Map(list =>
    list.Any(u => u.IsAdmin));
```

### Collections of Options

```csharp
// Sequence - convert List<Option<T>> to Option<List<T>>
List<Option<User>> optionalUsers = GetMultipleOptionalUsers();
Option<List<User>> allUsers = Option.Sequence(optionalUsers);
// Returns None if any Option is None

// Traverse - map and sequence
List<int> ids = new[] { 1, 2, 3 }.ToList();
Option<List<User>> users = Option.Traverse(ids, id => GetOptionalUser(id));

// Collect only Some values
List<Option<User>> mixed = GetMixedResults();
List<User> onlyUsers = Option.Collect(mixed);
```

### LINQ Integration

```csharp
// SelectMany for Option
var query = from user in GetOptionalUser()
            from address in GetOptionalAddress(user.AddressId)
            from city in GetOptionalCity(address.CityId)
            select new { user.Name, address.Street, city.Name };

// Where clause
var filtered = from user in GetOptionalUser()
               where user.IsActive
               select user;

// Join operations
var joined = from user in GetOptionalUser()
             from order in GetOptionalOrder(user.Id)
             select new { user, order };
```

## Advanced Patterns

### Monad Laws

```csharp
// Left Identity: Some(a).Bind(f) == f(a)
var a = 5;
Func<int, Option<string>> f = x => Option.Some(x.ToString());
var law1 = Option.Some(a).Bind(f) == f(a); // true

// Right Identity: m.Bind(Some) == m
var m = Option.Some(5);
var law2 = m.Bind(Option.Some) == m; // true

// Associativity: m.Bind(f).Bind(g) == m.Bind(x => f(x).Bind(g))
Func<int, Option<int>> f2 = x => Option.Some(x * 2);
Func<int, Option<string>> g = x => Option.Some(x.ToString());
var law3 = m.Bind(f2).Bind(g) == m.Bind(x => f2(x).Bind(g)); // true
```

### Applicative Patterns

```csharp
// Apply function in Option to value in Option
Option<Func<int, string>> funcOption = Option.Some<Func<int, string>>(x => x.ToString());
Option<int> valueOption = Option.Some(42);

Option<string> result = funcOption.Apply(valueOption); // Some("42")

// Lift regular function to work with Options
Func<int, int, int> add = (a, b) => a + b;
var liftedAdd = Option.Lift(add);

Option<int> sum = liftedAdd(Option.Some(5), Option.Some(3)); // Some(8)
```

### Option Builder (Computation Expression)

```csharp
// Builder pattern for Options
public class OptionBuilder
{
    public Option<T> Return<T>(T value) => Option.Some(value);
    
    public Option<TResult> Bind<T, TResult>(
        Option<T> option, 
        Func<T, Option<TResult>> f) => option.Bind(f);
        
    public Option<T> Zero<T>() => Option.None<T>();
}

// Usage
var builder = new OptionBuilder();
var result = builder.Bind(GetOptionalUser(), user =>
    builder.Bind(GetOptionalAddress(user.AddressId), address =>
        builder.Return(new { user, address })));
```

### Memoization with Options

```csharp
public class OptionalCache<TKey, TValue>
{
    private readonly Dictionary<TKey, Option<TValue>> _cache = new();
    
    public Option<TValue> GetOrCompute(TKey key, Func<TKey, Option<TValue>> compute)
    {
        if (_cache.TryGetValue(key, out var cached))
            return cached;
            
        var result = compute(key);
        _cache[key] = result;
        return result;
    }
}
```

### Railway-Oriented Programming with Option

```csharp
// Build optional pipeline
Option<ProcessedData> process = ParseInput(input)
    .Bind(data => ValidateData(data))
    .Bind(data => EnrichData(data))
    .Bind(data => TransformData(data))
    .Map(data => new ProcessedData(data));

// With side effects
Option<User> processUser = GetOptionalUser(id)
    .Tap(user => Logger.Log($"Processing {user.Name}"))
    .Filter(user => user.IsActive)
    .Tap(user => Metrics.IncrementActiveUsers())
    .Map(user => UpdateLastAccess(user));
```

## Best Practices

1. **Use Option instead of null**
   ```csharp
   // Good
   Option<User> FindUser(int id);
   
   // Avoid
   User? FindUser(int id);
   ```

2. **Don't use Value directly**
   ```csharp
   // Good
   option.Match(
       onSome: value => Process(value),
       onNone: () => HandleMissing());
   
   // Avoid
   if (option.IsSome)
       Process(option.Value); // Can still throw if race condition
   ```

3. **Use specific methods for transformations**
   ```csharp
   // Good - Bind flattens Option<Option<T>>
   option.Bind(x => GetOptional(x));
   
   // Avoid - creates nested Option
   option.Map(x => GetOptional(x)); // Option<Option<T>>
   ```

4. **Prefer Filter over manual checking**
   ```csharp
   // Good
   option.Filter(x => x.IsValid);
   
   // Avoid
   option.Bind(x => x.IsValid ? Option.Some(x) : Option.None<T>());
   ```

5. **Use meaningful variable names**
   ```csharp
   // Good
   userOption.Bind(user => GetAddress(user.Id));
   
   // Less clear
   opt.Bind(x => GetAddress(x.Id));
   ```

## Summary

The Option API provides:
- Safe handling of optional values without null
- Rich set of transformation and combination operations
- Integration with async/await patterns
- LINQ support for query syntax
- Collection operations
- Functional programming patterns

Key benefits:
- Explicit about value presence
- Composable operations
- Type-safe throughout
- No null reference exceptions
- Clear intent in APIs

Next steps:
- [Result API Reference](result-api.md)
- [Error Types Reference](error-types.md)
- [Combining Results and Options Guide](../guides/combining-results.md)
# Migrating from Nullable Types to Option Pattern

This guide helps you migrate from nullable reference types and nullable value types to the Option pattern using FluentUnions.

## Table of Contents
1. [Why Migrate from Nullables?](#why-migrate-from-nullables)
2. [Understanding the Differences](#understanding-the-differences)
3. [Migration Patterns](#migration-patterns)
4. [Step-by-Step Examples](#step-by-step-examples)
5. [Gradual Migration Strategy](#gradual-migration-strategy)
6. [Common Scenarios](#common-scenarios)
7. [Integration with Existing Code](#integration-with-existing-code)
8. [Best Practices](#best-practices)

## Why Migrate from Nullables?

### Problems with Nullable Types

```csharp
// Problems with nullable reference types
public class UserService
{
    public User? GetUser(int id)
    {
        return _repository.FindById(id); // Returns null if not found
    }
    
    public void ProcessUser(int id)
    {
        var user = GetUser(id);
        // Easy to forget null check
        Console.WriteLine(user.Name); // Potential NullReferenceException
        
        // Even with null check, nested access is verbose
        var city = user?.Address?.City?.Name ?? "Unknown";
        
        // Null doesn't tell us WHY the value is missing
        if (user == null)
        {
            // Was it not found? Access denied? Invalid ID?
        }
    }
}
```

### Benefits of Option Pattern

```csharp
// Option pattern advantages
public class UserService
{
    public Option<User> GetUser(int id)
    {
        return _repository.FindById(id).ToOption();
    }
    
    public void ProcessUser(int id)
    {
        GetUser(id).Match(
            onSome: user => Console.WriteLine(user.Name), // Safe access
            onNone: () => Console.WriteLine("User not found")
        );
        
        // Chainable operations
        var city = GetUser(id)
            .Bind(user => user.Address.ToOption())
            .Bind(addr => addr.City.ToOption())
            .Map(city => city.Name)
            .GetValueOr("Unknown");
    }
}
```

## Understanding the Differences

### Nullable vs Option Comparison

| Feature | Nullable (`T?`) | Option<T> |
|---------|----------------|-----------|
| Express absence | ✓ | ✓ |
| Reason for absence | ✗ | ✓ (with Result) |
| Composable operations | Limited | Rich API |
| Pattern matching | Basic | Full support |
| Prevent null access | Warnings only | Compile-time safety |
| LINQ support | Basic | Full |
| Collection operations | ✗ | ✓ |

### Semantic Differences

```csharp
// Nullable: absence without context
string? name = GetName(); // Why is it null?

// Option: explicit absence
Option<string> name = GetName(); // Clear: the name may not exist

// Result: absence with reason
Result<string> name = GetNameWithReason(); // If absent, we know why
```

## Migration Patterns

### Basic Conversion Patterns

```csharp
// From nullable to Option
string? nullable = GetNullableString();
Option<string> option = Option.FromNullable(nullable);

// From Option to nullable
Option<string> someOption = Option.Some("value");
string? backToNullable = someOption.ToNullable();

// Conditional conversion
User? user = condition ? new User() : null;
Option<User> userOption = Option.SomeIf(condition, new User());

// Direct factory methods
Option<int> some = Option.Some(42);
Option<int> none = Option.None<int>();
```

### Method Signature Migration

```csharp
// Before: Nullable return types
public interface IRepository<T>
{
    T? FindById(int id);
    T? FindByName(string name);
    IEnumerable<T> FindAll();
}

// After: Option return types
public interface IRepository<T>
{
    Option<T> FindById(int id);
    Option<T> FindByName(string name);
    IEnumerable<T> FindAll(); // Collections don't need Option
}

// Migration adapter
public class MigratingRepository<T> : IRepository<T>
{
    private readonly ILegacyRepository<T> _legacy;
    
    public Option<T> FindById(int id)
    {
        return Option.FromNullable(_legacy.FindById(id));
    }
}
```

## Step-by-Step Examples

### Example 1: Simple Property Access

**Before - Using Nullable:**
```csharp
public class CustomerService
{
    public string GetCustomerEmail(int customerId)
    {
        var customer = _repository.GetCustomer(customerId);
        if (customer == null)
            return "noreply@example.com";
            
        if (customer.Email == null)
            return "noreply@example.com";
            
        return customer.Email;
    }
    
    public decimal? GetCustomerDiscount(int customerId)
    {
        var customer = _repository.GetCustomer(customerId);
        if (customer?.LoyaltyCard == null)
            return null;
            
        return customer.LoyaltyCard.DiscountPercentage;
    }
}
```

**After - Using Option:**
```csharp
public class CustomerService
{
    public string GetCustomerEmail(int customerId)
    {
        return _repository.GetCustomer(customerId)
            .Bind(c => Option.FromNullable(c.Email))
            .GetValueOr("noreply@example.com");
    }
    
    public Option<decimal> GetCustomerDiscount(int customerId)
    {
        return _repository.GetCustomer(customerId)
            .Bind(c => Option.FromNullable(c.LoyaltyCard))
            .Map(card => card.DiscountPercentage);
    }
}
```

### Example 2: Chained Operations

**Before - Null Propagation:**
```csharp
public class OrderService
{
    public string? GetShippingCity(int orderId)
    {
        var order = _orderRepo.GetOrder(orderId);
        return order?.Customer?.Address?.City?.Name;
    }
    
    public decimal CalculateShippingCost(int orderId)
    {
        var order = _orderRepo.GetOrder(orderId);
        var city = order?.Customer?.Address?.City;
        
        if (city == null)
            return 10.0m; // Default shipping
            
        var rate = _shippingRates.GetRate(city.Id);
        if (rate == null)
            return 10.0m;
            
        return order.Total * rate.Percentage;
    }
}
```

**After - Option Chaining:**
```csharp
public class OrderService
{
    public Option<string> GetShippingCity(int orderId)
    {
        return _orderRepo.GetOrder(orderId)
            .Bind(order => order.Customer)
            .Bind(customer => customer.Address)
            .Bind(address => address.City)
            .Map(city => city.Name);
    }
    
    public decimal CalculateShippingCost(int orderId)
    {
        return _orderRepo.GetOrder(orderId)
            .Bind(order => order.Customer
                .Bind(c => c.Address)
                .Bind(a => a.City)
                .Bind(city => _shippingRates.GetRate(city.Id))
                .Map(rate => order.Total * rate.Percentage))
            .GetValueOr(10.0m);
    }
}
```

### Example 3: Collection Operations

**Before - Nullable in Collections:**
```csharp
public class ProductService
{
    public List<ProductInfo> GetProductInfos(List<int> productIds)
    {
        var infos = new List<ProductInfo>();
        
        foreach (var id in productIds)
        {
            var product = _repository.GetProduct(id);
            if (product != null)
            {
                var category = _categoryRepo.GetCategory(product.CategoryId);
                var info = new ProductInfo
                {
                    Name = product.Name,
                    CategoryName = category?.Name ?? "Uncategorized",
                    Price = product.Price
                };
                infos.Add(info);
            }
        }
        
        return infos;
    }
}
```

**After - Option with Collections:**
```csharp
public class ProductService
{
    public List<ProductInfo> GetProductInfos(List<int> productIds)
    {
        return productIds
            .Select(id => _repository.GetProduct(id))
            .Where(opt => opt.IsSome)
            .Select(opt => opt.Value)
            .Select(product => new ProductInfo
            {
                Name = product.Name,
                CategoryName = _categoryRepo.GetCategory(product.CategoryId)
                    .Map(c => c.Name)
                    .GetValueOr("Uncategorized"),
                Price = product.Price
            })
            .ToList();
    }
    
    // Or using Option.Traverse
    public Option<List<ProductInfo>> GetAllProductInfos(List<int> productIds)
    {
        return Option.Traverse(productIds, id =>
            _repository.GetProduct(id)
                .Bind(product => _categoryRepo.GetCategory(product.CategoryId)
                    .Map(category => new ProductInfo
                    {
                        Name = product.Name,
                        CategoryName = category.Name,
                        Price = product.Price
                    })));
    }
}
```

## Gradual Migration Strategy

### Phase 1: Wrapper Approach

```csharp
// Keep existing nullable interface
public interface IUserRepository
{
    User? GetUser(int id);
}

// Create Option-based wrapper
public class OptionUserRepository
{
    private readonly IUserRepository _inner;
    
    public Option<User> GetUser(int id)
    {
        return Option.FromNullable(_inner.GetUser(id));
    }
}
```

### Phase 2: Dual Interface

```csharp
public interface IUserRepository
{
    // Old method marked obsolete
    [Obsolete("Use GetUserOption instead")]
    User? GetUser(int id);
    
    // New Option-based method
    Option<User> GetUserOption(int id);
}

public class UserRepository : IUserRepository
{
    public User? GetUser(int id)
    {
        return GetUserOption(id).ToNullable();
    }
    
    public Option<User> GetUserOption(int id)
    {
        var user = _context.Users.Find(id);
        return Option.FromNullable(user);
    }
}
```

### Phase 3: Extension Methods for Transition

```csharp
public static class MigrationExtensions
{
    // Convert nullable methods to Option
    public static Option<T> ToOption<T>(this T? value) where T : class
    {
        return Option.FromNullable(value);
    }
    
    // Chain nullable operations
    public static Option<TResult> SelectNotNull<T, TResult>(
        this T? source, 
        Func<T, TResult?> selector) 
        where T : class 
        where TResult : class
    {
        return source == null 
            ? Option.None<TResult>() 
            : Option.FromNullable(selector(source));
    }
    
    // Safe navigation
    public static Option<TResult> Navigate<T, TResult>(
        this Option<T> source,
        Func<T, TResult?> selector) where TResult : class
    {
        return source.Bind(x => Option.FromNullable(selector(x)));
    }
}

// Usage
var email = user.ToOption()
    .Navigate(u => u.Profile)
    .Navigate(p => p.Email)
    .GetValueOr("no-email@example.com");
```

## Common Scenarios

### Configuration and Settings

**Before:**
```csharp
public class AppSettings
{
    public string? ApiKey { get; set; }
    public int? Timeout { get; set; }
    public string? BaseUrl { get; set; }
}

public class ConfigService
{
    public string GetApiKey()
    {
        var settings = LoadSettings();
        if (settings?.ApiKey == null)
            throw new ConfigurationException("API key not configured");
        return settings.ApiKey;
    }
    
    public int GetTimeout()
    {
        var settings = LoadSettings();
        return settings?.Timeout ?? 30; // Default 30 seconds
    }
}
```

**After:**
```csharp
public class AppSettings
{
    public Option<string> ApiKey { get; set; } = Option.None<string>();
    public Option<int> Timeout { get; set; } = Option.None<int>();
    public Option<string> BaseUrl { get; set; } = Option.None<string>();
}

public class ConfigService
{
    public Result<string> GetApiKey()
    {
        return LoadSettings()
            .Bind(s => s.ApiKey)
            .ToResult(new ConfigurationError("API key not configured"));
    }
    
    public int GetTimeout()
    {
        return LoadSettings()
            .Bind(s => s.Timeout)
            .GetValueOr(30); // Default 30 seconds
    }
}
```

### Database Operations

**Before:**
```csharp
public class DatabaseService
{
    public Employee? GetEmployee(int id)
    {
        using var conn = GetConnection();
        return conn.QuerySingleOrDefault<Employee>(
            "SELECT * FROM Employees WHERE Id = @id", 
            new { id });
    }
    
    public Department? GetEmployeeDepartment(int employeeId)
    {
        var employee = GetEmployee(employeeId);
        if (employee?.DepartmentId == null)
            return null;
            
        using var conn = GetConnection();
        return conn.QuerySingleOrDefault<Department>(
            "SELECT * FROM Departments WHERE Id = @id",
            new { id = employee.DepartmentId });
    }
}
```

**After:**
```csharp
public class DatabaseService
{
    public Option<Employee> GetEmployee(int id)
    {
        using var conn = GetConnection();
        var employee = conn.QuerySingleOrDefault<Employee>(
            "SELECT * FROM Employees WHERE Id = @id", 
            new { id });
        return Option.FromNullable(employee);
    }
    
    public Option<Department> GetEmployeeDepartment(int employeeId)
    {
        return GetEmployee(employeeId)
            .Bind(emp => Option.FromNullable(emp.DepartmentId))
            .Bind(deptId =>
            {
                using var conn = GetConnection();
                var dept = conn.QuerySingleOrDefault<Department>(
                    "SELECT * FROM Departments WHERE Id = @id",
                    new { id = deptId });
                return Option.FromNullable(dept);
            });
    }
}
```

### Caching Scenarios

**Before:**
```csharp
public class CacheService
{
    private readonly Dictionary<string, object?> _cache = new();
    
    public T? Get<T>(string key) where T : class
    {
        if (_cache.TryGetValue(key, out var value))
            return value as T;
        return null;
    }
    
    public T GetOrAdd<T>(string key, Func<T> factory) where T : class
    {
        var cached = Get<T>(key);
        if (cached != null)
            return cached;
            
        var value = factory();
        _cache[key] = value;
        return value;
    }
}
```

**After:**
```csharp
public class CacheService
{
    private readonly Dictionary<string, object> _cache = new();
    
    public Option<T> Get<T>(string key) where T : class
    {
        return _cache.TryGetValue(key, out var value)
            ? Option.Some((T)value)
            : Option.None<T>();
    }
    
    public T GetOrAdd<T>(string key, Func<T> factory) where T : class
    {
        return Get<T>(key).GetValueOr(() =>
        {
            var value = factory();
            _cache[key] = value;
            return value;
        });
    }
    
    public Option<T> GetOrCompute<T>(string key, Func<Option<T>> factory) where T : class
    {
        return Get<T>(key).Or(() =>
        {
            var computed = factory();
            computed.Match(
                onSome: value => _cache[key] = value,
                onNone: () => { });
            return computed;
        });
    }
}
```

## Integration with Existing Code

### LINQ Integration

```csharp
// Extension methods for LINQ compatibility
public static class OptionLinqExtensions
{
    public static IEnumerable<T> WhereHasValue<T>(
        this IEnumerable<Option<T>> source)
    {
        return source.Where(x => x.IsSome).Select(x => x.Value);
    }
    
    public static IEnumerable<Option<TResult>> SelectOption<T, TResult>(
        this IEnumerable<T?> source,
        Func<T, TResult> selector) where T : class
    {
        return source.Select(x => 
            x == null 
                ? Option.None<TResult>() 
                : Option.Some(selector(x)));
    }
}

// Usage
var validEmails = users
    .Select(u => Option.FromNullable(u.Email))
    .Where(opt => opt.IsSome)
    .Select(opt => opt.Value)
    .Where(email => email.Contains("@"))
    .ToList();
```

### Entity Framework Integration

```csharp
public class UserRepository
{
    private readonly DbContext _context;
    
    public Option<User> FindById(int id)
    {
        // Handle nullable result from EF
        var user = _context.Users
            .Include(u => u.Profile)
            .FirstOrDefault(u => u.Id == id);
            
        return Option.FromNullable(user);
    }
    
    public async Task<Option<User>> FindByEmailAsync(string email)
    {
        var user = await _context.Users
            .Include(u => u.Profile)
            .FirstOrDefaultAsync(u => u.Email == email);
            
        return Option.FromNullable(user);
    }
    
    // Handle nullable navigation properties
    public Option<UserProfile> GetUserProfile(int userId)
    {
        return FindById(userId)
            .Bind(user => Option.FromNullable(user.Profile));
    }
}
```

### JSON Serialization

```csharp
// Custom JSON converter for Option<T>
public class OptionJsonConverter<T> : JsonConverter<Option<T>>
{
    public override void WriteJson(JsonWriter writer, Option<T> value, JsonSerializer serializer)
    {
        if (value.IsSome)
            serializer.Serialize(writer, value.Value);
        else
            writer.WriteNull();
    }
    
    public override Option<T> ReadJson(JsonReader reader, Type objectType, 
        Option<T> existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null)
            return Option.None<T>();
            
        var value = serializer.Deserialize<T>(reader);
        return Option.FromNullable(value);
    }
}

// Usage
public class ApiResponse
{
    [JsonConverter(typeof(OptionJsonConverter<string>))]
    public Option<string> OptionalField { get; set; }
}
```

## Best Practices

### 1. Use Semantic Names

```csharp
// Avoid generic names
public Option<User> user = GetUser(); // Less clear

// Use descriptive names
public Option<User> currentUser = GetCurrentUser();
public Option<User> maybeUser = FindUserByEmail(email);
```

### 2. Prefer Option Methods Over Manual Checks

```csharp
// Avoid manual IsSome/IsNone checks
if (userOption.IsSome)
{
    var user = userOption.Value;
    ProcessUser(user);
}
else
{
    HandleMissing();
}

// Prefer Match or other combinators
userOption.Match(
    onSome: ProcessUser,
    onNone: HandleMissing);
```

### 3. Don't Overuse Option for Collections

```csharp
// Unnecessary - empty list is sufficient
public Option<List<User>> GetUsers() 
{
    var users = _repository.GetAll();
    return users.Any() 
        ? Option.Some(users) 
        : Option.None<List<User>>();
}

// Better - empty list represents no items
public List<User> GetUsers()
{
    return _repository.GetAll(); // Empty list if none
}

// Use Option for single items
public Option<User> GetFirstActiveUser()
{
    var user = _repository.GetAll().FirstOrDefault(u => u.IsActive);
    return Option.FromNullable(user);
}
```

### 4. Consider Result When You Need Error Information

```csharp
// Option doesn't tell us why
public Option<User> GetUser(int id)
{
    if (id <= 0) return Option.None<User>();
    var user = _repository.Find(id);
    if (user == null) return Option.None<User>();
    if (!user.IsActive) return Option.None<User>();
    return Option.Some(user);
}

// Result provides context
public Result<User> GetUser(int id)
{
    if (id <= 0) 
        return new ValidationError("Invalid user ID");
    
    var user = _repository.Find(id);
    if (user == null) 
        return new NotFoundError("User", id);
        
    if (!user.IsActive) 
        return new ValidationError("User account is inactive");
        
    return Result.Success(user);
}
```

### 5. Migration Checklist

- [ ] Identify all nullable return types in public APIs
- [ ] Create Option-based alternatives
- [ ] Add extension methods for common patterns
- [ ] Update unit tests to use Option assertions
- [ ] Mark old nullable methods as obsolete
- [ ] Update documentation with Option examples
- [ ] Train team on Option pattern usage
- [ ] Monitor for NullReferenceExceptions in production
- [ ] Complete migration and remove nullable methods

## Summary

Migrating from nullable types to Option:

1. **Explicit absence** - Option makes missing values explicit
2. **Composable operations** - Chain operations without null checks
3. **Type safety** - Compiler enforces handling of None case
4. **Better semantics** - Clear distinction between "missing" and "null"
5. **Rich API** - Map, Bind, Filter, and more operations

Key migration strategies:
- Start with new code using Option
- Create adapters for existing nullable APIs
- Use extension methods for gradual migration
- Provide both interfaces during transition
- Update tests to verify Option behavior

Next steps:
- [Option Pattern Basics](../tutorials/option-pattern-basics.md)
- [Combining Results and Options](../guides/combining-results.md)
- [API Reference - Option](../reference/option-api.md)
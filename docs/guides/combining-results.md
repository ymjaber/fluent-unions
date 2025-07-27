# Combining Results Guide

This guide covers patterns and techniques for working with multiple Results and Options in FluentUnions.

## Table of Contents
1. [Introduction](#introduction)
2. [Combining Multiple Results](#combining-multiple-results)
3. [BindAll Operations](#bindall-operations)
4. [Collecting Results](#collecting-results)
5. [Parallel Processing](#parallel-processing)
6. [Aggregating Errors](#aggregating-errors)
7. [Option Combinations](#option-combinations)
8. [Advanced Patterns](#advanced-patterns)
9. [Performance Considerations](#performance-considerations)
10. [Best Practices](#best-practices)

## Introduction

When building applications, you often need to work with multiple operations that return Result or Option types. FluentUnions provides several ways to combine these operations efficiently.

### Common Scenarios

- Validating multiple fields
- Loading data from multiple sources
- Processing collections of items
- Aggregating results from parallel operations

## Combining Multiple Results

### Result.BindAll

The most common way to combine multiple Results is using `BindAll`:

```csharp
public Result<Order> CreateOrder(Guid customerId, List<Guid> productIds, Guid addressId)
{
    return Result.BindAll(
        GetCustomer(customerId),        // Result<Customer>
        GetProducts(productIds),        // Result<List<Product>>
        GetShippingAddress(addressId)   // Result<Address>
    )
    .Map((customer, products, address) => new Order
    {
        Customer = customer,
        Products = products,
        ShippingAddress = address,
        CreatedAt = DateTime.UtcNow
    });
}
```

### Different Arities

BindAll supports combining 2 to 8 Results:

```csharp
// 2 Results
Result<Summary> summary = Result.BindAll(
    CalculateSubtotal(items),
    CalculateTax(items)
).Map((subtotal, tax) => new Summary(subtotal, tax));

// 3 Results
Result<Invoice> invoice = Result.BindAll(
    GetOrder(orderId),
    GetCustomer(customerId),
    GetPaymentInfo(paymentId)
).Map((order, customer, payment) => 
    CreateInvoice(order, customer, payment));

// Up to 8 Results
Result<Dashboard> dashboard = Result.BindAll(
    GetSales(), GetExpenses(), GetInventory(),
    GetCustomers(), GetSuppliers(), GetEmployees(),
    GetRevenue(), GetMetrics()
).Map((s, e, i, c, sup, emp, r, m) => 
    BuildDashboard(s, e, i, c, sup, emp, r, m));
```

### Early Exit on Failure

BindAll returns the first error encountered:

```csharp
var result = Result.BindAll(
    Result.Success(1),
    Result.Failure<int>("Error 1"),
    Result.Failure<int>("Error 2")  // Never evaluated
);

// result is Failure with "Error 1"
```

## BindAll Operations

### Custom BindAll Implementation

For more than 8 Results or custom behavior:

```csharp
public static Result<List<T>> BindAll<T>(IEnumerable<Result<T>> results)
{
    var list = new List<T>();
    
    foreach (var result in results)
    {
        if (result.IsFailure)
            return Result.Failure<List<T>>(result.Error);
            
        list.Add(result.Value);
    }
    
    return Result.Success(list);
}

// Usage
var productResults = productIds.Select(id => GetProduct(id));
var allProducts = BindAll(productResults);
```

### BindAll with Different Types

When combining Results of different types:

```csharp
public static Result<(T1, T2, T3)> BindAll<T1, T2, T3>(
    Result<T1> result1,
    Result<T2> result2,
    Result<T3> result3)
{
    if (result1.IsFailure) return Result.Failure<(T1, T2, T3)>(result1.Error);
    if (result2.IsFailure) return Result.Failure<(T1, T2, T3)>(result2.Error);
    if (result3.IsFailure) return Result.Failure<(T1, T2, T3)>(result3.Error);
    
    return Result.Success((result1.Value, result2.Value, result3.Value));
}
```

## Collecting Results

### Collecting All Results (Including Failures)

Sometimes you want to collect all results, not stop at the first failure:

```csharp
public static (List<T> successes, List<Error> failures) CollectAll<T>(
    IEnumerable<Result<T>> results)
{
    var successes = new List<T>();
    var failures = new List<Error>();
    
    foreach (var result in results)
    {
        result.Match(
            onSuccess: value => successes.Add(value),
            onFailure: error => failures.Add(error)
        );
    }
    
    return (successes, failures);
}

// Usage
var results = items.Select(item => ProcessItem(item));
var (processed, errors) = CollectAll(results);

if (errors.Any())
{
    logger.LogWarning($"Processed {processed.Count} items with {errors.Count} errors");
}
```

### Validation Aggregation

Aggregate all validation errors:

```csharp
public Result<ValidatedOrder> ValidateOrder(OrderRequest request)
{
    var errors = new List<Error>();
    
    // Validate customer
    var customerResult = ValidateCustomer(request.CustomerId);
    if (customerResult.IsFailure)
        errors.Add(customerResult.Error);
    
    // Validate items
    foreach (var item in request.Items)
    {
        var itemResult = ValidateItem(item);
        if (itemResult.IsFailure)
            errors.Add(itemResult.Error);
    }
    
    // Validate shipping
    var shippingResult = ValidateShipping(request.ShippingAddress);
    if (shippingResult.IsFailure)
        errors.Add(shippingResult.Error);
    
    // Return aggregate error or success
    return errors.Any()
        ? new AggregateError("Order validation failed", errors)
        : Result.Success(new ValidatedOrder(request));
}
```

## Parallel Processing

### Processing Collections in Parallel

```csharp
public Result<List<ProcessedItem>> ProcessItemsInParallel(
    List<Item> items)
{
    var results = items
        .AsParallel()
        .Select(item => ProcessItem(item))
        .ToList();
    
    return BindAll(results);
}

// With degree of parallelism
public Result<List<ProcessedData>> ProcessWithControlledParallelism(
    List<Data> dataList,
    int maxDegreeOfParallelism = 4)
{
    var results = dataList
        .AsParallel()
        .WithDegreeOfParallelism(maxDegreeOfParallelism)
        .Select(data => ProcessData(data))
        .ToList();
    
    return BindAll(results);
}
```

### Combining Independent Operations

```csharp
public Result<Report> GenerateCompleteReport(int year)
{
    // Start all operations in parallel
    var salesData = GetSalesData(year);
    var expenseData = GetExpenseData(year);
    var customerData = GetCustomerData(year);
    var inventoryData = GetInventoryData(year);
    
    // Combine results
    return Result.BindAll(
        salesData,
        expenseData,
        customerData,
        inventoryData
    ).Map((sales, expenses, customers, inventory) => 
        new Report
        {
            Year = year,
            Sales = sales,
            Expenses = expenses,
            Customers = customers,
            Inventory = inventory
        });
}
```

## Aggregating Errors

### Creating Aggregate Errors

```csharp
public class ValidationService
{
    public Result<ValidatedData> ValidateComplex(ComplexRequest request)
    {
        var validators = new List<Func<Result>>
        {
            () => ValidateName(request.Name),
            () => ValidateEmail(request.Email),
            () => ValidateAge(request.Age),
            () => ValidateAddress(request.Address)
        };
        
        var errors = validators
            .Select(validator => validator())
            .Where(result => result.IsFailure)
            .Select(result => result.Error)
            .ToList();
        
        return errors.Any()
            ? new AggregateError("Validation failed", errors)
            : Result.Success(new ValidatedData(request));
    }
}
```

### Hierarchical Error Aggregation

```csharp
public Result<ProcessedBatch> ProcessBatch(Batch batch)
{
    var groupErrors = new Dictionary<string, List<Error>>();
    
    foreach (var group in batch.Groups)
    {
        var groupResult = ProcessGroup(group);
        if (groupResult.IsFailure)
        {
            if (!groupErrors.ContainsKey(group.Name))
                groupErrors[group.Name] = new List<Error>();
                
            groupErrors[group.Name].Add(groupResult.Error);
        }
    }
    
    if (groupErrors.Any())
    {
        var aggregatedErrors = groupErrors
            .Select(kvp => new AggregateError($"Group {kvp.Key} failed", kvp.Value))
            .Cast<Error>()
            .ToList();
            
        return new AggregateError("Batch processing failed", aggregatedErrors);
    }
    
    return Result.Success(new ProcessedBatch(batch));
}
```

## Option Combinations

### Combining Options

```csharp
// Combine two Options
public Option<FullName> GetFullName(Option<string> firstName, Option<string> lastName)
{
    return firstName.Bind(first =>
        lastName.Map(last => new FullName(first, last))
    );
}

// Using pattern matching
public Option<Address> CombineAddress(
    Option<string> street,
    Option<string> city,
    Option<string> zipCode)
{
    return (street.IsSome, city.IsSome, zipCode.IsSome) switch
    {
        (true, true, true) => Option.Some(new Address
        {
            Street = street.Value,
            City = city.Value,
            ZipCode = zipCode.Value
        }),
        _ => Option.None<Address>()
    };
}
```

### Option.Zip

```csharp
// Zip multiple Options
var fullAddress = Option.Zip(
    GetStreet(),
    GetCity(),
    GetState(),
    GetZipCode()
).Map(tuple => new Address(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4));

// Custom Zip for collections
public static Option<List<T>> ZipAll<T>(IEnumerable<Option<T>> options)
{
    var list = new List<T>();
    
    foreach (var option in options)
    {
        if (option.IsNone)
            return Option.None<List<T>>();
            
        list.Add(option.Value);
    }
    
    return Option.Some(list);
}
```

## Advanced Patterns

### Transactional Operations

```csharp
public Result<TransferReceipt> TransferFunds(
    Guid fromAccountId,
    Guid toAccountId,
    decimal amount)
{
    return Result.BindAll(
        GetAccount(fromAccountId),
        GetAccount(toAccountId)
    )
    .Bind((from, to) => ValidateTransfer(from, to, amount)
        .Map(_ => (from, to)))
    .Bind((from, to) => 
    {
        // Start transaction
        using var transaction = BeginTransaction();
        
        var debitResult = DebitAccount(from, amount);
        if (debitResult.IsFailure)
        {
            transaction.Rollback();
            return Result.Failure<TransferReceipt>(debitResult.Error);
        }
        
        var creditResult = CreditAccount(to, amount);
        if (creditResult.IsFailure)
        {
            transaction.Rollback();
            return Result.Failure<TransferReceipt>(creditResult.Error);
        }
        
        transaction.Commit();
        return Result.Success(new TransferReceipt(from, to, amount));
    });
}
```

### Pipeline Processing

```csharp
public Result<ProcessedData> ProcessPipeline(RawData data)
{
    var stages = new List<Func<ProcessedData, Result<ProcessedData>>>
    {
        Validate,
        Transform,
        Enrich,
        Optimize,
        Finalize
    };
    
    var result = Result.Success(new ProcessedData(data));
    
    foreach (var stage in stages)
    {
        result = result.Bind(stage);
        if (result.IsFailure)
            break;
    }
    
    return result;
}
```

### Conditional Combinations

```csharp
public Result<Order> CreateConditionalOrder(OrderRequest request)
{
    var baseResults = new List<Result<object>>
    {
        ValidateBasicInfo(request).Map(x => (object)x),
        CheckInventory(request.Items).Map(x => (object)x)
    };
    
    // Add conditional validations
    if (request.RequiresShipping)
    {
        baseResults.Add(
            ValidateShippingAddress(request.ShippingAddress)
                .Map(x => (object)x)
        );
    }
    
    if (request.HasDiscount)
    {
        baseResults.Add(
            ValidateDiscountCode(request.DiscountCode)
                .Map(x => (object)x)
        );
    }
    
    // Combine all results
    var combinedResult = BindAll(baseResults);
    
    return combinedResult.IsSuccess
        ? CreateOrder(request)
        : Result.Failure<Order>(combinedResult.Error);
}
```

## Performance Considerations

### Lazy Evaluation

```csharp
public Result<ExpensiveData> GetExpensiveData(
    bool useCache,
    bool useDatabase,
    bool useApi)
{
    var sources = new List<Func<Result<ExpensiveData>>>();
    
    if (useCache)
        sources.Add(() => GetFromCache());
    
    if (useDatabase)
        sources.Add(() => GetFromDatabase());
        
    if (useApi)
        sources.Add(() => GetFromApi());
    
    // Try each source until one succeeds
    foreach (var source in sources)
    {
        var result = source();
        if (result.IsSuccess)
            return result;
    }
    
    return new Error("NO_DATA", "No data source available");
}
```

### Chunked Processing

```csharp
public Result<List<ProcessedItem>> ProcessLargeDataset(
    List<Item> items,
    int chunkSize = 100)
{
    var allResults = new List<ProcessedItem>();
    var errors = new List<Error>();
    
    var chunks = items
        .Select((item, index) => new { item, index })
        .GroupBy(x => x.index / chunkSize)
        .Select(g => g.Select(x => x.item).ToList());
    
    foreach (var chunk in chunks)
    {
        var chunkResult = ProcessChunk(chunk);
        
        chunkResult.Match(
            onSuccess: processed => allResults.AddRange(processed),
            onFailure: error => errors.Add(error)
        );
    }
    
    return errors.Any()
        ? new AggregateError($"Failed to process {errors.Count} chunks", errors)
        : Result.Success(allResults);
}
```

## Best Practices

### 1. Use BindAll for Independent Operations

```csharp
// Good - operations are independent
Result<Dashboard> dashboard = Result.BindAll(
    GetUserStats(userId),
    GetRecentOrders(userId),
    GetNotifications(userId)
).Map((stats, orders, notifications) => 
    new Dashboard(stats, orders, notifications));

// Less optimal - sequential when could be parallel
Result<Dashboard> dashboard = GetUserStats(userId)
    .Bind(stats => GetRecentOrders(userId)
        .Bind(orders => GetNotifications(userId)
            .Map(notifications => 
                new Dashboard(stats, orders, notifications))));
```

### 2. Aggregate Validation Errors

```csharp
// Good - collect all validation errors
public Result<ValidForm> ValidateForm(FormData data)
{
    var errors = new List<Error>();
    
    if (string.IsNullOrEmpty(data.Name))
        errors.Add(new ValidationError("Name is required"));
        
    if (!IsValidEmail(data.Email))
        errors.Add(new ValidationError("Invalid email"));
        
    if (data.Age < 18)
        errors.Add(new ValidationError("Must be 18 or older"));
    
    return errors.Any()
        ? new AggregateError("Form validation failed", errors)
        : Result.Success(new ValidForm(data));
}
```

### 3. Handle Partial Success

```csharp
public Result<ImportSummary> ImportData(List<DataRow> rows)
{
    var results = rows.Select(row => ImportRow(row)).ToList();
    var (successes, failures) = CollectAll(results);
    
    if (failures.Count == rows.Count)
    {
        // Total failure
        return new AggregateError("Import failed completely", failures);
    }
    
    var summary = new ImportSummary
    {
        TotalRows = rows.Count,
        SuccessCount = successes.Count,
        FailureCount = failures.Count,
        Errors = failures
    };
    
    // Return success with summary even if some failed
    return Result.Success(summary);
}
```

### 4. Use Appropriate Combination Method

```csharp
// For fail-fast behavior
var result = Result.BindAll(op1, op2, op3);

// For collecting all errors
var errors = operations
    .Select(op => op())
    .Where(r => r.IsFailure)
    .Select(r => r.Error)
    .ToList();

// For partial success
var (successes, failures) = CollectAll(operations);
```

## Summary

Combining Results effectively requires understanding:

1. **BindAll** for combining multiple Results that must all succeed
2. **Error aggregation** for validation scenarios
3. **Parallel processing** for independent operations
4. **Partial success** handling for batch operations
5. **Performance implications** of different combination strategies

The key is choosing the right pattern for your specific use case.

Next steps:
- [Functional Operations Guide](functional-operations.md)
- [Error Handling Tutorial](../tutorials/error-handling.md)
- [Performance Best Practices](performance-best-practices.md)
# Entity Framework Integration

This guide demonstrates how to integrate FluentUnions with Entity Framework Core for robust data access with proper error handling.

## Table of Contents
1. [Setup](#setup)
2. [Repository Pattern](#repository-pattern)
3. [Query Specifications](#query-specifications)
4. [Transaction Management](#transaction-management)
5. [Error Handling](#error-handling)
6. [Async Operations](#async-operations)
7. [Performance Optimization](#performance-optimization)
8. [Testing](#testing)
9. [Advanced Patterns](#advanced-patterns)
10. [Best Practices](#best-practices)

## Setup

### Installation

```bash
dotnet add package FluentUnions
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.SqlServer # or your provider
```

### DbContext Configuration

```csharp
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Product> Products { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Apply configurations
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        // Global query filters
        modelBuilder.Entity<User>().HasQueryFilter(u => !u.IsDeleted);
        modelBuilder.Entity<Order>().HasQueryFilter(o => !o.IsDeleted);
    }
    
    // Override SaveChanges to return Result
    public async Task<Result<int>> SaveChangesAsyncResult(CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await SaveChangesAsync(cancellationToken);
            return Result.Success(result);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            return new ConflictError($"The record was modified by another user: {ex.Message}");
        }
        catch (DbUpdateException ex)
        {
            return HandleDbUpdateException(ex);
        }
        catch (Exception ex)
        {
            return new Error("DB_ERROR", $"Database error: {ex.Message}");
        }
    }
    
    private Result<int> HandleDbUpdateException(DbUpdateException ex)
    {
        if (ex.InnerException?.Message.Contains("UNIQUE") == true)
            return new ConflictError("A record with the same key already exists");
            
        if (ex.InnerException?.Message.Contains("FOREIGN KEY") == true)
            return new ValidationError("Related record not found");
            
        return new Error("DB_UPDATE_ERROR", "Failed to save changes to database");
    }
}
```

## Repository Pattern

### Base Repository Interface

```csharp
public interface IRepository<T> where T : class
{
    // Query operations
    Task<Result<T>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Option<T>> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<List<T>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<List<T>>> GetBySpecificationAsync(ISpecification<T> spec, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<T>>> GetPagedAsync(IPagedSpecification<T> spec, CancellationToken cancellationToken = default);
    
    // Command operations
    Task<Result<T>> AddAsync(T entity, CancellationToken cancellationToken = default);
    Task<Result<T>> UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(T entity, CancellationToken cancellationToken = default);
    Task<Result> DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default);
    
    // Bulk operations
    Task<Result<List<T>>> AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
    Task<Result> DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
}
```

### Base Repository Implementation

```csharp
public abstract class RepositoryBase<T> : IRepository<T> where T : Entity
{
    protected readonly ApplicationDbContext Context;
    protected readonly DbSet<T> DbSet;
    protected readonly ILogger<RepositoryBase<T>> Logger;
    
    protected RepositoryBase(
        ApplicationDbContext context,
        ILogger<RepositoryBase<T>> logger)
    {
        Context = context;
        DbSet = context.Set<T>();
        Logger = logger;
    }
    
    public virtual async Task<Result<T>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await DbSet
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
                
            return entity != null
                ? Result.Success(entity)
                : new NotFoundError(typeof(T).Name, id);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error getting {Entity} by ID {Id}", typeof(T).Name, id);
            return new Error("DB_ERROR", $"Failed to get {typeof(T).Name}");
        }
    }
    
    public virtual async Task<Option<T>> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await DbSet
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
                
            return Option.FromNullable(entity);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error finding {Entity} by ID {Id}", typeof(T).Name, id);
            return Option.None<T>();
        }
    }
    
    public virtual async Task<Result<List<T>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var entities = await DbSet.ToListAsync(cancellationToken);
            return Result.Success(entities);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error getting all {Entity}", typeof(T).Name);
            return new Error("DB_ERROR", $"Failed to get {typeof(T).Name} records");
        }
    }
    
    public virtual async Task<Result<T>> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        try
        {
            entity.CreatedAt = DateTime.UtcNow;
            DbSet.Add(entity);
            
            var saveResult = await Context.SaveChangesAsyncResult(cancellationToken);
            
            return saveResult.IsSuccess
                ? Result.Success(entity)
                : Result.Failure<T>(saveResult.Error);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error adding {Entity}", typeof(T).Name);
            return new Error("DB_ERROR", $"Failed to add {typeof(T).Name}");
        }
    }
    
    public virtual async Task<Result<T>> UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        try
        {
            entity.UpdatedAt = DateTime.UtcNow;
            DbSet.Update(entity);
            
            var saveResult = await Context.SaveChangesAsyncResult(cancellationToken);
            
            return saveResult.IsSuccess
                ? Result.Success(entity)
                : Result.Failure<T>(saveResult.Error);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error updating {Entity}", typeof(T).Name);
            return new Error("DB_ERROR", $"Failed to update {typeof(T).Name}");
        }
    }
    
    public virtual async Task<Result> DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        try
        {
            DbSet.Remove(entity);
            return await Context.SaveChangesAsyncResult(cancellationToken)
                .Map(_ => Result.Success())
                .GetValueOr(error => Result.Failure(error));
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error deleting {Entity}", typeof(T).Name);
            return new Error("DB_ERROR", $"Failed to delete {typeof(T).Name}");
        }
    }
    
    public virtual async Task<Result> DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entityResult = await GetByIdAsync(id, cancellationToken);
        
        return await entityResult
            .BindAsync(entity => DeleteAsync(entity, cancellationToken));
    }
}
```

### Specific Repository Implementation

```csharp
public interface IUserRepository : IRepository<User>
{
    Task<Option<User>> FindByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<Result<List<User>>> GetActiveUsersAsync(CancellationToken cancellationToken = default);
    Task<Result<PagedResult<User>>> SearchUsersAsync(UserSearchCriteria criteria, CancellationToken cancellationToken = default);
}

public class UserRepository : RepositoryBase<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context, ILogger<UserRepository> logger)
        : base(context, logger) { }
    
    public async Task<Option<User>> FindByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await DbSet
                .Include(u => u.Profile)
                .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
                
            return Option.FromNullable(user);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error finding user by email {Email}", email);
            return Option.None<User>();
        }
    }
    
    public async Task<Result<List<User>>> GetActiveUsersAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var users = await DbSet
                .Where(u => u.IsActive && u.EmailConfirmed)
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .ToListAsync(cancellationToken);
                
            return Result.Success(users);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error getting active users");
            return new Error("DB_ERROR", "Failed to get active users");
        }
    }
    
    public async Task<Result<PagedResult<User>>> SearchUsersAsync(
        UserSearchCriteria criteria,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = DbSet.AsQueryable();
            
            // Apply filters
            if (!string.IsNullOrWhiteSpace(criteria.SearchTerm))
            {
                query = query.Where(u =>
                    u.FirstName.Contains(criteria.SearchTerm) ||
                    u.LastName.Contains(criteria.SearchTerm) ||
                    u.Email.Contains(criteria.SearchTerm));
            }
            
            if (criteria.IsActive.HasValue)
                query = query.Where(u => u.IsActive == criteria.IsActive.Value);
                
            if (criteria.CreatedAfter.HasValue)
                query = query.Where(u => u.CreatedAt >= criteria.CreatedAfter.Value);
            
            // Get total count
            var totalCount = await query.CountAsync(cancellationToken);
            
            // Apply sorting
            query = criteria.SortBy switch
            {
                "Name" => criteria.SortDescending
                    ? query.OrderByDescending(u => u.LastName).ThenByDescending(u => u.FirstName)
                    : query.OrderBy(u => u.LastName).ThenBy(u => u.FirstName),
                "Email" => criteria.SortDescending
                    ? query.OrderByDescending(u => u.Email)
                    : query.OrderBy(u => u.Email),
                "CreatedAt" => criteria.SortDescending
                    ? query.OrderByDescending(u => u.CreatedAt)
                    : query.OrderBy(u => u.CreatedAt),
                _ => query.OrderBy(u => u.Id)
            };
            
            // Apply pagination
            var items = await query
                .Skip((criteria.Page - 1) * criteria.PageSize)
                .Take(criteria.PageSize)
                .ToListAsync(cancellationToken);
            
            return Result.Success(new PagedResult<User>
            {
                Items = items,
                TotalCount = totalCount,
                Page = criteria.Page,
                PageSize = criteria.PageSize
            });
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error searching users");
            return new Error("DB_ERROR", "Failed to search users");
        }
    }
}
```

## Query Specifications

### Specification Pattern

```csharp
public interface ISpecification<T>
{
    Expression<Func<T, bool>> Criteria { get; }
    List<Expression<Func<T, object>>> Includes { get; }
    List<string> IncludeStrings { get; }
    Expression<Func<T, object>> OrderBy { get; }
    Expression<Func<T, object>> OrderByDescending { get; }
    Expression<Func<T, object>> GroupBy { get; }
    
    int Take { get; }
    int Skip { get; }
    bool IsPagingEnabled { get; }
}

public abstract class BaseSpecification<T> : ISpecification<T>
{
    public Expression<Func<T, bool>> Criteria { get; private set; }
    public List<Expression<Func<T, object>>> Includes { get; } = new();
    public List<string> IncludeStrings { get; } = new();
    public Expression<Func<T, object>> OrderBy { get; private set; }
    public Expression<Func<T, object>> OrderByDescending { get; private set; }
    public Expression<Func<T, object>> GroupBy { get; private set; }
    
    public int Take { get; private set; }
    public int Skip { get; private set; }
    public bool IsPagingEnabled { get; private set; }
    
    protected BaseSpecification() { }
    
    protected BaseSpecification(Expression<Func<T, bool>> criteria)
    {
        Criteria = criteria;
    }
    
    protected void AddInclude(Expression<Func<T, object>> includeExpression)
    {
        Includes.Add(includeExpression);
    }
    
    protected void AddInclude(string includeString)
    {
        IncludeStrings.Add(includeString);
    }
    
    protected void ApplyPaging(int skip, int take)
    {
        Skip = skip;
        Take = take;
        IsPagingEnabled = true;
    }
    
    protected void ApplyOrderBy(Expression<Func<T, object>> orderByExpression)
    {
        OrderBy = orderByExpression;
    }
    
    protected void ApplyOrderByDescending(Expression<Func<T, object>> orderByDescExpression)
    {
        OrderByDescending = orderByDescExpression;
    }
}
```

### Specification Examples

```csharp
public class ActiveUsersSpecification : BaseSpecification<User>
{
    public ActiveUsersSpecification()
        : base(u => u.IsActive && u.EmailConfirmed)
    {
        AddInclude(u => u.Profile);
        ApplyOrderBy(u => u.LastName);
    }
}

public class UserByEmailSpecification : BaseSpecification<User>
{
    public UserByEmailSpecification(string email)
        : base(u => u.Email == email)
    {
        AddInclude(u => u.Profile);
        AddInclude(u => u.Roles);
    }
}

public class RecentOrdersSpecification : BaseSpecification<Order>
{
    public RecentOrdersSpecification(Guid userId, int days = 30)
        : base(o => o.UserId == userId && o.CreatedAt >= DateTime.UtcNow.AddDays(-days))
    {
        AddInclude(o => o.Items);
        AddInclude("Items.Product");
        ApplyOrderByDescending(o => o.CreatedAt);
    }
}
```

### Specification Evaluator

```csharp
public static class SpecificationEvaluator<T> where T : class
{
    public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> specification)
    {
        var query = inputQuery;
        
        // Apply criteria
        if (specification.Criteria != null)
        {
            query = query.Where(specification.Criteria);
        }
        
        // Apply includes
        query = specification.Includes.Aggregate(query,
            (current, include) => current.Include(include));
            
        // Apply string includes
        query = specification.IncludeStrings.Aggregate(query,
            (current, include) => current.Include(include));
        
        // Apply ordering
        if (specification.OrderBy != null)
        {
            query = query.OrderBy(specification.OrderBy);
        }
        else if (specification.OrderByDescending != null)
        {
            query = query.OrderByDescending(specification.OrderByDescending);
        }
        
        // Apply grouping
        if (specification.GroupBy != null)
        {
            query = query.GroupBy(specification.GroupBy).SelectMany(x => x);
        }
        
        // Apply paging
        if (specification.IsPagingEnabled)
        {
            query = query.Skip(specification.Skip).Take(specification.Take);
        }
        
        return query;
    }
}
```

### Using Specifications in Repository

```csharp
public async Task<Result<List<T>>> GetBySpecificationAsync(
    ISpecification<T> spec,
    CancellationToken cancellationToken = default)
{
    try
    {
        var query = SpecificationEvaluator<T>.GetQuery(DbSet.AsQueryable(), spec);
        var entities = await query.ToListAsync(cancellationToken);
        return Result.Success(entities);
    }
    catch (Exception ex)
    {
        Logger.LogError(ex, "Error getting {Entity} by specification", typeof(T).Name);
        return new Error("DB_ERROR", $"Failed to query {typeof(T).Name}");
    }
}
```

## Transaction Management

### Unit of Work Pattern

```csharp
public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    IOrderRepository Orders { get; }
    IProductRepository Products { get; }
    
    Task<Result<int>> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<Result> ExecuteInTransactionAsync(Func<Task<Result>> operation, CancellationToken cancellationToken = default);
    Task<Result<T>> ExecuteInTransactionAsync<T>(Func<Task<Result<T>>> operation, CancellationToken cancellationToken = default);
}

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<UnitOfWork> _logger;
    
    private IUserRepository _users;
    private IOrderRepository _orders;
    private IProductRepository _products;
    
    public UnitOfWork(
        ApplicationDbContext context,
        ILogger<UnitOfWork> logger,
        ILoggerFactory loggerFactory)
    {
        _context = context;
        _logger = logger;
        
        // Initialize repositories lazily
        _users = new UserRepository(context, loggerFactory.CreateLogger<UserRepository>());
        _orders = new OrderRepository(context, loggerFactory.CreateLogger<OrderRepository>());
        _products = new ProductRepository(context, loggerFactory.CreateLogger<ProductRepository>());
    }
    
    public IUserRepository Users => _users;
    public IOrderRepository Orders => _orders;
    public IProductRepository Products => _products;
    
    public async Task<Result<int>> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsyncResult(cancellationToken);
    }
    
    public async Task<Result> ExecuteInTransactionAsync(
        Func<Task<Result>> operation,
        CancellationToken cancellationToken = default)
    {
        // Skip if already in transaction
        if (_context.Database.CurrentTransaction != null)
        {
            return await operation();
        }
        
        var strategy = _context.Database.CreateExecutionStrategy();
        
        return await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            
            try
            {
                var result = await operation();
                
                if (result.IsSuccess)
                {
                    await transaction.CommitAsync(cancellationToken);
                }
                else
                {
                    await transaction.RollbackAsync(cancellationToken);
                }
                
                return result;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                _logger.LogError(ex, "Transaction failed and was rolled back");
                throw;
            }
        });
    }
    
    public async Task<Result<T>> ExecuteInTransactionAsync<T>(
        Func<Task<Result<T>>> operation,
        CancellationToken cancellationToken = default)
    {
        if (_context.Database.CurrentTransaction != null)
        {
            return await operation();
        }
        
        var strategy = _context.Database.CreateExecutionStrategy();
        
        return await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            
            try
            {
                var result = await operation();
                
                if (result.IsSuccess)
                {
                    await transaction.CommitAsync(cancellationToken);
                }
                else
                {
                    await transaction.RollbackAsync(cancellationToken);
                }
                
                return result;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                _logger.LogError(ex, "Transaction failed and was rolled back");
                throw;
            }
        });
    }
    
    public void Dispose()
    {
        _context?.Dispose();
    }
}
```

### Using Transactions

```csharp
public class OrderService
{
    private readonly IUnitOfWork _unitOfWork;
    
    public async Task<Result<Order>> CreateOrderAsync(CreateOrderRequest request)
    {
        return await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            // Get customer
            var customerResult = await _unitOfWork.Users.GetByIdAsync(request.CustomerId);
            if (customerResult.IsFailure)
                return Result.Failure<Order>(customerResult.Error);
                
            var customer = customerResult.Value;
            
            // Check and update inventory
            foreach (var item in request.Items)
            {
                var productResult = await _unitOfWork.Products.GetByIdAsync(item.ProductId);
                if (productResult.IsFailure)
                    return Result.Failure<Order>(productResult.Error);
                    
                var product = productResult.Value;
                if (product.Stock < item.Quantity)
                    return Result.Failure<Order>(new OutOfStockError(product.Name, item.Quantity, product.Stock));
                    
                product.Stock -= item.Quantity;
                var updateResult = await _unitOfWork.Products.UpdateAsync(product);
                if (updateResult.IsFailure)
                    return Result.Failure<Order>(updateResult.Error);
            }
            
            // Create order
            var order = new Order
            {
                CustomerId = customer.Id,
                Items = request.Items.Select(i => new OrderItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList(),
                Total = request.Items.Sum(i => i.Quantity * i.Price),
                Status = OrderStatus.Pending
            };
            
            return await _unitOfWork.Orders.AddAsync(order);
        });
    }
}
```

## Error Handling

### Custom EF Error Handler

```csharp
public static class EFErrorHandler
{
    public static Error ConvertException(Exception exception)
    {
        return exception switch
        {
            DbUpdateConcurrencyException => new ConflictError(
                "The record was modified by another user. Please refresh and try again."),
                
            DbUpdateException dbEx when IsUniqueConstraintViolation(dbEx) =>
                new ConflictError("A record with the same key already exists"),
                
            DbUpdateException dbEx when IsForeignKeyViolation(dbEx) =>
                new ValidationError("Related record not found or cannot be deleted due to existing references"),
                
            InvalidOperationException ioEx when ioEx.Message.Contains("sequence contains") =>
                new NotFoundError("The requested record was not found"),
                
            SqlException sqlEx when sqlEx.Number == 2601 =>
                new ConflictError("Duplicate key violation"),
                
            SqlException sqlEx when sqlEx.Number == 547 =>
                new ValidationError("Foreign key constraint violation"),
                
            SqlException sqlEx when sqlEx.Number == -2 =>
                new Error("TIMEOUT", "Database operation timed out"),
                
            _ => new Error("DB_ERROR", "A database error occurred")
        };
    }
    
    private static bool IsUniqueConstraintViolation(DbUpdateException ex)
    {
        return ex.InnerException?.Message.Contains("UNIQUE") == true ||
               ex.InnerException?.Message.Contains("duplicate key") == true;
    }
    
    private static bool IsForeignKeyViolation(DbUpdateException ex)
    {
        return ex.InnerException?.Message.Contains("FOREIGN KEY") == true ||
               ex.InnerException?.Message.Contains("REFERENCE") == true;
    }
}
```

### Repository Error Handling

```csharp
public class SafeRepository<T> : RepositoryBase<T> where T : Entity
{
    protected override async Task<Result<TResult>> ExecuteAsync<TResult>(
        Func<Task<TResult>> operation,
        string operationName)
    {
        try
        {
            var result = await operation();
            return Result.Success(result);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "{Operation} failed for {Entity}", operationName, typeof(T).Name);
            var error = EFErrorHandler.ConvertException(ex);
            return Result.Failure<TResult>(error);
        }
    }
    
    protected override async Task<Result> ExecuteAsync(
        Func<Task> operation,
        string operationName)
    {
        try
        {
            await operation();
            return Result.Success();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "{Operation} failed for {Entity}", operationName, typeof(T).Name);
            var error = EFErrorHandler.ConvertException(ex);
            return Result.Failure(error);
        }
    }
}
```

## Async Operations

### Async Repository Methods

```csharp
public class AsyncRepository<T> : RepositoryBase<T> where T : Entity
{
    public async Task<Result<List<T>>> GetAllAsync(
        Expression<Func<T, bool>> predicate = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        string includeProperties = "",
        CancellationToken cancellationToken = default)
    {
        try
        {
            IQueryable<T> query = DbSet;
            
            if (predicate != null)
                query = query.Where(predicate);
                
            foreach (var includeProperty in includeProperties.Split(
                new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            
            if (orderBy != null)
                query = orderBy(query);
                
            var entities = await query.ToListAsync(cancellationToken);
            return Result.Success(entities);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error in GetAllAsync");
            return Result.Failure<List<T>>(EFErrorHandler.ConvertException(ex));
        }
    }
    
    public async Task<Option<T>> FirstOrDefaultAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await DbSet.FirstOrDefaultAsync(predicate, cancellationToken);
            return Option.FromNullable(entity);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error in FirstOrDefaultAsync");
            return Option.None<T>();
        }
    }
    
    public async Task<Result<bool>> AnyAsync(
        Expression<Func<T, bool>> predicate = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = predicate != null ? DbSet.Where(predicate) : DbSet;
            var exists = await query.AnyAsync(cancellationToken);
            return Result.Success(exists);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error in AnyAsync");
            return Result.Failure<bool>(EFErrorHandler.ConvertException(ex));
        }
    }
    
    public async Task<Result<int>> CountAsync(
        Expression<Func<T, bool>> predicate = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = predicate != null ? DbSet.Where(predicate) : DbSet;
            var count = await query.CountAsync(cancellationToken);
            return Result.Success(count);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error in CountAsync");
            return Result.Failure<int>(EFErrorHandler.ConvertException(ex));
        }
    }
}
```

### Parallel Async Operations

```csharp
public class ParallelDataService
{
    private readonly IUnitOfWork _unitOfWork;
    
    public async Task<Result<DashboardData>> GetDashboardDataAsync(Guid userId)
    {
        // Execute queries in parallel
        var userTask = _unitOfWork.Users.GetByIdAsync(userId);
        var ordersTask = _unitOfWork.Orders.GetRecentOrdersAsync(userId, 10);
        var statsTask = _unitOfWork.Users.GetUserStatisticsAsync(userId);
        
        // Wait for all
        await Task.WhenAll(userTask, ordersTask, statsTask);
        
        // Combine results
        return Result.BindAll(
            userTask.Result,
            ordersTask.Result,
            statsTask.Result)
            .Map((user, orders, stats) => new DashboardData
            {
                User = user,
                RecentOrders = orders,
                Statistics = stats
            });
    }
}
```

## Performance Optimization

### Projection with Result

```csharp
public class ProjectionRepository<T> : RepositoryBase<T> where T : Entity
{
    public async Task<Result<List<TDto>>> GetProjectedAsync<TDto>(
        Expression<Func<T, TDto>> selector,
        Expression<Func<T, bool>> predicate = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = DbSet.AsQueryable();
            
            if (predicate != null)
                query = query.Where(predicate);
                
            var projections = await query
                .Select(selector)
                .ToListAsync(cancellationToken);
                
            return Result.Success(projections);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error in GetProjectedAsync");
            return Result.Failure<List<TDto>>(EFErrorHandler.ConvertException(ex));
        }
    }
}

// Usage
public async Task<Result<List<UserSummaryDto>>> GetUserSummariesAsync()
{
    return await _repository.GetProjectedAsync<UserSummaryDto>(
        user => new UserSummaryDto
        {
            Id = user.Id,
            Name = $"{user.FirstName} {user.LastName}",
            Email = user.Email,
            IsActive = user.IsActive
        },
        user => user.IsActive);
}
```

### Compiled Queries

```csharp
public class CompiledQueries
{
    private static readonly Func<ApplicationDbContext, Guid, Task<User>> GetUserByIdCompiled =
        EF.CompileAsyncQuery((ApplicationDbContext context, Guid id) =>
            context.Users.FirstOrDefault(u => u.Id == id));
            
    private static readonly Func<ApplicationDbContext, string, Task<User>> GetUserByEmailCompiled =
        EF.CompileAsyncQuery((ApplicationDbContext context, string email) =>
            context.Users.FirstOrDefault(u => u.Email == email));
    
    public static async Task<Option<User>> GetUserByIdAsync(ApplicationDbContext context, Guid id)
    {
        try
        {
            var user = await GetUserByIdCompiled(context, id);
            return Option.FromNullable(user);
        }
        catch (Exception ex)
        {
            // Log error
            return Option.None<User>();
        }
    }
}
```

### Batch Operations

```csharp
public class BatchRepository<T> : RepositoryBase<T> where T : Entity
{
    public async Task<Result<List<T>>> AddBatchAsync(
        IEnumerable<T> entities,
        int batchSize = 100,
        CancellationToken cancellationToken = default)
    {
        var entityList = entities.ToList();
        var results = new List<T>();
        
        try
        {
            for (int i = 0; i < entityList.Count; i += batchSize)
            {
                var batch = entityList.Skip(i).Take(batchSize);
                
                DbSet.AddRange(batch);
                
                if ((i + batchSize) % 1000 == 0 || i + batchSize >= entityList.Count)
                {
                    var saveResult = await Context.SaveChangesAsyncResult(cancellationToken);
                    if (saveResult.IsFailure)
                        return Result.Failure<List<T>>(saveResult.Error);
                }
                
                results.AddRange(batch);
            }
            
            return Result.Success(results);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error in batch add operation");
            return Result.Failure<List<T>>(EFErrorHandler.ConvertException(ex));
        }
    }
}
```

## Testing

### In-Memory Database Tests

```csharp
public class RepositoryTestBase : IDisposable
{
    protected ApplicationDbContext Context { get; }
    protected ILoggerFactory LoggerFactory { get; }
    
    protected RepositoryTestBase()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
            
        Context = new ApplicationDbContext(options);
        LoggerFactory = new LoggerFactory();
        
        SeedTestData();
    }
    
    protected virtual void SeedTestData()
    {
        // Override in derived classes
    }
    
    public void Dispose()
    {
        Context?.Dispose();
    }
}

public class UserRepositoryTests : RepositoryTestBase
{
    private readonly UserRepository _repository;
    
    public UserRepositoryTests()
    {
        _repository = new UserRepository(Context, LoggerFactory.CreateLogger<UserRepository>());
    }
    
    protected override void SeedTestData()
    {
        var users = new[]
        {
            new User { Id = Guid.NewGuid(), Email = "user1@test.com", FirstName = "User", LastName = "One" },
            new User { Id = Guid.NewGuid(), Email = "user2@test.com", FirstName = "User", LastName = "Two" }
        };
        
        Context.Users.AddRange(users);
        Context.SaveChanges();
    }
    
    [Fact]
    public async Task GetByIdAsync_WhenUserExists_ReturnsSuccess()
    {
        // Arrange
        var existingUser = Context.Users.First();
        
        // Act
        var result = await _repository.GetByIdAsync(existingUser.Id);
        
        // Assert
        result.Should().BeSuccess();
        result.Value.Should().NotBeNull();
        result.Value.Email.Should().Be(existingUser.Email);
    }
    
    [Fact]
    public async Task GetByIdAsync_WhenUserNotFound_ReturnsNotFoundError()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();
        
        // Act
        var result = await _repository.GetByIdAsync(nonExistentId);
        
        // Assert
        result.Should().BeFailure();
        result.Error.Should().BeOfType<NotFoundError>();
    }
    
    [Fact]
    public async Task FindByEmailAsync_WhenUserExists_ReturnsSome()
    {
        // Arrange
        var email = "user1@test.com";
        
        // Act
        var option = await _repository.FindByEmailAsync(email);
        
        // Assert
        option.Should().HaveValue();
        option.Value.Email.Should().Be(email);
    }
}
```

### Mocking EF Context

```csharp
public class MockDbContextTests
{
    private readonly Mock<ApplicationDbContext> _mockContext;
    private readonly Mock<DbSet<User>> _mockUserSet;
    
    public MockDbContextTests()
    {
        _mockContext = new Mock<ApplicationDbContext>();
        _mockUserSet = new Mock<DbSet<User>>();
        
        var users = new List<User>
        {
            new User { Id = Guid.NewGuid(), Email = "test@example.com" }
        }.AsQueryable();
        
        _mockUserSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(users.Provider);
        _mockUserSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
        _mockUserSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
        _mockUserSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(users.GetEnumerator());
        
        _mockContext.Setup(c => c.Users).Returns(_mockUserSet.Object);
    }
    
    [Fact]
    public async Task Repository_CanBeMocked()
    {
        // Arrange
        var repository = new UserRepository(
            _mockContext.Object,
            Mock.Of<ILogger<UserRepository>>());
        
        // Act & Assert
        // Test repository methods with mocked context
    }
}
```

## Advanced Patterns

### Domain Events from EF

```csharp
public abstract class Entity
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents;
    
    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
    
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}

public class EventDispatchingContext : ApplicationDbContext
{
    private readonly IMediator _mediator;
    
    public EventDispatchingContext(
        DbContextOptions<ApplicationDbContext> options,
        IMediator mediator) : base(options)
    {
        _mediator = mediator;
    }
    
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entities = ChangeTracker
            .Entries<Entity>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity)
            .ToList();
            
        var domainEvents = entities
            .SelectMany(e => e.DomainEvents)
            .ToList();
            
        entities.ForEach(e => e.ClearDomainEvents());
        
        var result = await base.SaveChangesAsync(cancellationToken);
        
        // Dispatch events after successful save
        foreach (var domainEvent in domainEvents)
        {
            await _mediator.Publish(domainEvent, cancellationToken);
        }
        
        return result;
    }
}
```

### Soft Delete with Global Filters

```csharp
public interface ISoftDeletable
{
    bool IsDeleted { get; set; }
    DateTime? DeletedAt { get; set; }
    string DeletedBy { get; set; }
}

public class SoftDeleteEntity : Entity, ISoftDeletable
{
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string DeletedBy { get; set; }
}

public class SoftDeleteRepository<T> : RepositoryBase<T> where T : SoftDeleteEntity
{
    private readonly ICurrentUserService _currentUser;
    
    public override async Task<Result> DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        try
        {
            entity.IsDeleted = true;
            entity.DeletedAt = DateTime.UtcNow;
            entity.DeletedBy = _currentUser.UserName;
            
            return await UpdateAsync(entity, cancellationToken)
                .Map(_ => Result.Success())
                .GetValueOr(error => Result.Failure(error));
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error soft deleting {Entity}", typeof(T).Name);
            return Result.Failure(EFErrorHandler.ConvertException(ex));
        }
    }
    
    public async Task<Result> RestoreAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await Context.Set<T>()
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(e => e.Id == id && e.IsDeleted, cancellationToken);
                
            if (entity == null)
                return new NotFoundError($"Deleted {typeof(T).Name}", id);
                
            entity.IsDeleted = false;
            entity.DeletedAt = null;
            entity.DeletedBy = null;
            
            return await UpdateAsync(entity, cancellationToken)
                .Map(_ => Result.Success())
                .GetValueOr(error => Result.Failure(error));
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error restoring {Entity}", typeof(T).Name);
            return Result.Failure(EFErrorHandler.ConvertException(ex));
        }
    }
}
```

## Best Practices

### 1. Always Return Result from Data Access

```csharp
// Good - explicit error handling
public async Task<Result<User>> GetUserAsync(Guid id)
{
    try
    {
        var user = await _context.Users.FindAsync(id);
        return user != null
            ? Result.Success(user)
            : new NotFoundError("User", id);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error getting user {Id}", id);
        return Result.Failure<User>(EFErrorHandler.ConvertException(ex));
    }
}

// Avoid - exceptions for control flow
public async Task<User> GetUserAsync(Guid id)
{
    var user = await _context.Users.FindAsync(id);
    if (user == null)
        throw new NotFoundException($"User {id} not found");
    return user;
}
```

### 2. Use Specifications for Complex Queries

```csharp
// Good - reusable specification
public class ActivePremiumUsersSpec : BaseSpecification<User>
{
    public ActivePremiumUsersSpec()
        : base(u => u.IsActive && u.IsPremium && u.SubscriptionExpiry > DateTime.UtcNow)
    {
        AddInclude(u => u.Subscription);
        ApplyOrderBy(u => u.SubscriptionExpiry);
    }
}

// Avoid - inline complex queries
var users = await _context.Users
    .Where(u => u.IsActive && u.IsPremium && u.SubscriptionExpiry > DateTime.UtcNow)
    .Include(u => u.Subscription)
    .OrderBy(u => u.SubscriptionExpiry)
    .ToListAsync();
```

### 3. Handle Concurrency Properly

```csharp
public async Task<Result<Product>> UpdateProductStockAsync(
    Guid productId,
    int quantityChange,
    byte[] rowVersion)
{
    try
    {
        var product = await _context.Products.FindAsync(productId);
        if (product == null)
            return new NotFoundError("Product", productId);
            
        _context.Entry(product).Property("RowVersion").OriginalValue = rowVersion;
        
        product.Stock += quantityChange;
        
        await _context.SaveChangesAsync();
        
        return Result.Success(product);
    }
    catch (DbUpdateConcurrencyException)
    {
        return new ConflictError(
            "The product was modified by another user. Please refresh and try again.");
    }
}
```

### 4. Use Async Methods

```csharp
// Good - async all the way
public async Task<Result<List<Order>>> GetUserOrdersAsync(Guid userId)
{
    return await _context.Orders
        .Where(o => o.UserId == userId)
        .OrderByDescending(o => o.CreatedAt)
        .ToListAsync()
        .ContinueWith(t => t.Exception != null
            ? Result.Failure<List<Order>>(new Error("DB_ERROR", "Failed to get orders"))
            : Result.Success(t.Result));
}

// Avoid - blocking async calls
public Result<List<Order>> GetUserOrders(Guid userId)
{
    var orders = _context.Orders
        .Where(o => o.UserId == userId)
        .OrderByDescending(o => o.CreatedAt)
        .ToListAsync()
        .GetAwaiter()
        .GetResult(); // Blocks!
        
    return Result.Success(orders);
}
```

### 5. Dispose Resources Properly

```csharp
public class DisposableService : IDisposable
{
    private readonly ApplicationDbContext _context;
    private bool _disposed;
    
    public DisposableService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Result<User>> GetUserAsync(Guid id)
    {
        if (_disposed)
            return new Error("DISPOSED", "Service has been disposed");
            
        return await _context.Users
            .FindAsync(id)
            .ContinueWith(t => t.Result != null
                ? Result.Success(t.Result)
                : new NotFoundError("User", id));
    }
    
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _context?.Dispose();
            }
            _disposed = true;
        }
    }
}
```

## Summary

Integrating FluentUnions with Entity Framework provides:

1. **Explicit error handling** - All data operations return Result types
2. **Better testability** - Easy to mock and test error scenarios
3. **Transaction safety** - Automatic rollback on Result failures
4. **Performance optimization** - Projections and compiled queries with Result
5. **Clean architecture** - Repository and Unit of Work patterns with Result

Key integration points:
- Custom SaveChangesAsyncResult method
- Repository pattern returning Result/Option
- Specification pattern for complex queries
- Transaction management with Result
- Error conversion from EF exceptions

Next steps:
- [Dependency Injection](dependency-injection.md)
- [Testing Guide](../guides/testing-guide.md)
- [Performance Best Practices](../guides/performance-best-practices.md)
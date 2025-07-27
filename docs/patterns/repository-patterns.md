# Repository Patterns

This guide demonstrates how to implement repository patterns using FluentUnions for robust data access layers.

## Table of Contents
1. [Introduction](#introduction)
2. [Basic Repository Pattern](#basic-repository-pattern)
3. [Generic Repository](#generic-repository)
4. [Query Specifications](#query-specifications)
5. [Unit of Work Pattern](#unit-of-work-pattern)
6. [Caching Strategies](#caching-strategies)
7. [Event Sourcing](#event-sourcing)
8. [Testing Repositories](#testing-repositories)
9. [Advanced Patterns](#advanced-patterns)
10. [Best Practices](#best-practices)

## Introduction

The repository pattern with FluentUnions provides:
- Explicit error handling for data operations
- Clear separation between domain and data access
- Testable data access layer
- Consistent error types across the application

## Basic Repository Pattern

### Repository Interface

```csharp
public interface IUserRepository
{
    Result<User> GetById(Guid id);
    Result<User> GetByEmail(string email);
    Result<IReadOnlyList<User>> GetAll();
    Result<IReadOnlyList<User>> GetByFilter(UserFilter filter);
    Result<Guid> Create(User user);
    Result Update(User user);
    Result Delete(Guid id);
    Result<bool> Exists(Guid id);
}
```

### Entity Framework Implementation

```csharp
public class EfUserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<EfUserRepository> _logger;
    
    public EfUserRepository(AppDbContext context, ILogger<EfUserRepository> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    public Result<User> GetById(Guid id)
    {
        return Result.Try(() =>
        {
            var user = _context.Users
                .Include(u => u.Profile)
                .Include(u => u.Roles)
                .FirstOrDefault(u => u.Id == id);
                
            return user != null
                ? Result.Success(user)
                : new NotFoundError($"User with ID {id} not found");
        })
        .MapError(ex => HandleDatabaseError(ex, "Failed to get user by ID"));
    }
    
    public Result<User> GetByEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return new ValidationError("Email cannot be empty");
            
        return Result.Try(() =>
        {
            var normalizedEmail = email.ToLowerInvariant();
            var user = _context.Users
                .Include(u => u.Profile)
                .FirstOrDefault(u => u.NormalizedEmail == normalizedEmail);
                
            return user != null
                ? Result.Success(user)
                : new NotFoundError($"User with email {email} not found");
        })
        .MapError(ex => HandleDatabaseError(ex, "Failed to get user by email"));
    }
    
    public Result<Guid> Create(User user)
    {
        return ValidateUser(user)
            .Bind(() => CheckEmailUniqueness(user.Email))
            .Bind(() => Result.Try(() =>
            {
                user.Id = Guid.NewGuid();
                user.CreatedAt = DateTime.UtcNow;
                user.NormalizedEmail = user.Email.ToLowerInvariant();
                
                _context.Users.Add(user);
                _context.SaveChanges();
                
                return Result.Success(user.Id);
            }))
            .MapError(ex => HandleDatabaseError(ex, "Failed to create user"));
    }
    
    public Result Update(User user)
    {
        return ValidateUser(user)
            .Bind(() => Result.Try(() =>
            {
                var existing = _context.Users.Find(user.Id);
                if (existing == null)
                    return new NotFoundError($"User {user.Id} not found");
                    
                // Update tracking
                user.UpdatedAt = DateTime.UtcNow;
                user.Version = existing.Version + 1;
                
                _context.Entry(existing).CurrentValues.SetValues(user);
                
                var affected = _context.SaveChanges();
                return affected > 0
                    ? Result.Success()
                    : new Error("UPDATE_FAILED", "No rows were updated");
            }))
            .MapError(ex => HandleDatabaseError(ex, "Failed to update user"));
    }
    
    public Result Delete(Guid id)
    {
        return Result.Try(() =>
        {
            var user = _context.Users.Find(id);
            if (user == null)
                return new NotFoundError($"User {id} not found");
                
            // Soft delete
            user.IsDeleted = true;
            user.DeletedAt = DateTime.UtcNow;
            
            _context.SaveChanges();
            return Result.Success();
        })
        .MapError(ex => HandleDatabaseError(ex, "Failed to delete user"));
    }
    
    private Result ValidateUser(User user)
    {
        if (user == null)
            return new ValidationError("User cannot be null");
            
        if (string.IsNullOrWhiteSpace(user.Email))
            return new ValidationError("Email is required");
            
        return Result.Success();
    }
    
    private Result CheckEmailUniqueness(string email)
    {
        var exists = _context.Users.Any(u => 
            u.NormalizedEmail == email.ToLowerInvariant());
            
        return exists
            ? new ConflictError($"Email {email} is already registered")
            : Result.Success();
    }
    
    private Error HandleDatabaseError(Exception ex, string message)
    {
        _logger.LogError(ex, message);
        
        return ex switch
        {
            DbUpdateConcurrencyException => new ConflictError(
                "The record was modified by another user"),
            DbUpdateException dbEx when IsUniqueConstraintViolation(dbEx) => 
                new ConflictError("A record with this value already exists"),
            TimeoutException => new Error("TIMEOUT", "Database operation timed out"),
            _ => new Error("DATABASE_ERROR", message)
        };
    }
}
```

## Generic Repository

### Generic Repository Interface

```csharp
public interface IRepository<TEntity> where TEntity : class, IEntity
{
    Result<TEntity> GetById(Guid id);
    Result<IReadOnlyList<TEntity>> GetAll();
    Result<IReadOnlyList<TEntity>> Find(ISpecification<TEntity> specification);
    Result<TEntity> FindSingle(ISpecification<TEntity> specification);
    Result<Guid> Add(TEntity entity);
    Result Update(TEntity entity);
    Result Delete(Guid id);
    Result<int> Count(ISpecification<TEntity> specification = null);
    Result<bool> Exists(ISpecification<TEntity> specification);
}

public interface IEntity
{
    Guid Id { get; }
    DateTime CreatedAt { get; }
    DateTime? UpdatedAt { get; }
    bool IsDeleted { get; }
}
```

### Generic Repository Implementation

```csharp
public class Repository<TEntity> : IRepository<TEntity> 
    where TEntity : class, IEntity
{
    protected readonly DbContext Context;
    protected readonly DbSet<TEntity> DbSet;
    private readonly ILogger<Repository<TEntity>> _logger;
    
    public Repository(DbContext context, ILogger<Repository<TEntity>> logger)
    {
        Context = context;
        DbSet = context.Set<TEntity>();
        _logger = logger;
    }
    
    public virtual Result<TEntity> GetById(Guid id)
    {
        return Result.Try(() =>
        {
            var entity = DbSet.Find(id);
            
            return entity != null && !entity.IsDeleted
                ? Result.Success(entity)
                : new NotFoundError($"{typeof(TEntity).Name} with ID {id} not found");
        })
        .MapError(ex => HandleError(ex, $"Failed to get {typeof(TEntity).Name}"));
    }
    
    public virtual Result<IReadOnlyList<TEntity>> GetAll()
    {
        return Result.Try(() =>
        {
            var entities = DbSet
                .Where(e => !e.IsDeleted)
                .ToList();
                
            return Result.Success<IReadOnlyList<TEntity>>(entities);
        })
        .MapError(ex => HandleError(ex, "Failed to get all entities"));
    }
    
    public virtual Result<IReadOnlyList<TEntity>> Find(ISpecification<TEntity> specification)
    {
        return Result.Try(() =>
        {
            var query = ApplySpecification(specification);
            var entities = query.ToList();
            
            return Result.Success<IReadOnlyList<TEntity>>(entities);
        })
        .MapError(ex => HandleError(ex, "Failed to find entities"));
    }
    
    public virtual Result<TEntity> FindSingle(ISpecification<TEntity> specification)
    {
        return Result.Try(() =>
        {
            var query = ApplySpecification(specification);
            var entity = query.SingleOrDefault();
            
            return entity != null
                ? Result.Success(entity)
                : new NotFoundError($"No {typeof(TEntity).Name} found matching criteria");
        })
        .MapError(ex => HandleError(ex, "Failed to find single entity"));
    }
    
    public virtual Result<Guid> Add(TEntity entity)
    {
        return ValidateEntity(entity)
            .Bind(() => Result.Try(() =>
            {
                if (entity.Id == Guid.Empty)
                    entity.Id = Guid.NewGuid();
                    
                entity.CreatedAt = DateTime.UtcNow;
                entity.IsDeleted = false;
                
                DbSet.Add(entity);
                Context.SaveChanges();
                
                return Result.Success(entity.Id);
            }))
            .MapError(ex => HandleError(ex, "Failed to add entity"));
    }
    
    protected IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> spec)
    {
        var query = DbSet.AsQueryable();
        
        if (spec.Criteria != null)
            query = query.Where(spec.Criteria);
            
        query = spec.Includes.Aggregate(query, 
            (current, include) => current.Include(include));
            
        if (spec.OrderBy != null)
            query = query.OrderBy(spec.OrderBy);
        else if (spec.OrderByDescending != null)
            query = query.OrderByDescending(spec.OrderByDescending);
            
        if (spec.IsPagingEnabled)
            query = query.Skip(spec.Skip).Take(spec.Take);
            
        return query;
    }
    
    protected virtual Result ValidateEntity(TEntity entity)
    {
        if (entity == null)
            return new ValidationError($"{typeof(TEntity).Name} cannot be null");
            
        return Result.Success();
    }
    
    protected Error HandleError(Exception ex, string message)
    {
        _logger.LogError(ex, message);
        
        return ex switch
        {
            InvalidOperationException ioe when ioe.Message.Contains("Sequence contains more than one element") => 
                new Error("MULTIPLE_RESULTS", "Query returned multiple results when one was expected"),
            DbUpdateConcurrencyException => 
                new ConflictError("The record was modified by another user"),
            DbUpdateException dbEx when IsUniqueConstraintViolation(dbEx) => 
                new ConflictError("A record with this value already exists"),
            _ => new Error("REPOSITORY_ERROR", message)
        };
    }
    
    private bool IsUniqueConstraintViolation(DbUpdateException ex)
    {
        // Check for unique constraint violation
        // Implementation depends on database provider
        return ex.InnerException?.Message.Contains("unique") ?? false;
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
    Expression<Func<T, object>> OrderBy { get; }
    Expression<Func<T, object>> OrderByDescending { get; }
    int Take { get; }
    int Skip { get; }
    bool IsPagingEnabled { get; }
}

public abstract class BaseSpecification<T> : ISpecification<T>
{
    public Expression<Func<T, bool>> Criteria { get; private set; }
    public List<Expression<Func<T, object>>> Includes { get; } = new();
    public Expression<Func<T, object>> OrderBy { get; private set; }
    public Expression<Func<T, object>> OrderByDescending { get; private set; }
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

### Concrete Specifications

```csharp
public class ActiveUsersSpecification : BaseSpecification<User>
{
    public ActiveUsersSpecification() 
        : base(u => u.IsActive && !u.IsDeleted)
    {
        AddInclude(u => u.Profile);
        AddInclude(u => u.Roles);
        ApplyOrderBy(u => u.CreatedAt);
    }
}

public class UsersByEmailSpecification : BaseSpecification<User>
{
    public UsersByEmailSpecification(string email) 
        : base(u => u.NormalizedEmail == email.ToLowerInvariant() && !u.IsDeleted)
    {
        AddInclude(u => u.Profile);
    }
}

public class UsersWithPagingSpecification : BaseSpecification<User>
{
    public UsersWithPagingSpecification(int pageNumber, int pageSize)
        : base(u => !u.IsDeleted)
    {
        ApplyPaging((pageNumber - 1) * pageSize, pageSize);
        ApplyOrderByDescending(u => u.CreatedAt);
    }
}

public class UsersWithComplexFilterSpecification : BaseSpecification<User>
{
    public UsersWithComplexFilterSpecification(UserSearchCriteria criteria)
    {
        // Build dynamic criteria
        Expression<Func<User, bool>> filter = u => !u.IsDeleted;
        
        if (!string.IsNullOrWhiteSpace(criteria.SearchTerm))
        {
            var term = criteria.SearchTerm.ToLowerInvariant();
            filter = filter.And(u => 
                u.NormalizedEmail.Contains(term) ||
                u.Profile.FirstName.Contains(term) ||
                u.Profile.LastName.Contains(term));
        }
        
        if (criteria.IsActive.HasValue)
            filter = filter.And(u => u.IsActive == criteria.IsActive.Value);
            
        if (criteria.RoleIds?.Any() == true)
            filter = filter.And(u => u.Roles.Any(r => criteria.RoleIds.Contains(r.Id)));
            
        Criteria = filter;
        
        // Apply sorting
        switch (criteria.SortBy)
        {
            case "email":
                ApplyOrderBy(u => u.Email);
                break;
            case "created":
                ApplyOrderByDescending(u => u.CreatedAt);
                break;
            default:
                ApplyOrderBy(u => u.Profile.LastName);
                break;
        }
        
        // Apply paging
        if (criteria.PageNumber > 0 && criteria.PageSize > 0)
            ApplyPaging((criteria.PageNumber - 1) * criteria.PageSize, criteria.PageSize);
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
        
        if (specification.Criteria != null)
            query = query.Where(specification.Criteria);
            
        query = specification.Includes.Aggregate(query,
            (current, include) => current.Include(include));
            
        if (specification.OrderBy != null)
            query = query.OrderBy(specification.OrderBy);
        else if (specification.OrderByDescending != null)
            query = query.OrderByDescending(specification.OrderByDescending);
            
        if (specification.IsPagingEnabled)
            query = query.Skip(specification.Skip).Take(specification.Take);
            
        return query;
    }
}
```

## Unit of Work Pattern

### Unit of Work Interface

```csharp
public interface IUnitOfWork : IDisposable
{
    IRepository<TEntity> Repository<TEntity>() where TEntity : class, IEntity;
    IUserRepository UserRepository { get; }
    IOrderRepository OrderRepository { get; }
    
    Result<int> SaveChanges();
    Task<Result<int>> SaveChangesAsync(CancellationToken cancellationToken = default);
    Result BeginTransaction();
    Result CommitTransaction();
    Result RollbackTransaction();
}
```

### Unit of Work Implementation

```csharp
public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private readonly ILogger<UnitOfWork> _logger;
    private IDbContextTransaction _currentTransaction;
    private readonly Dictionary<Type, object> _repositories = new();
    
    public UnitOfWork(AppDbContext context, ILogger<UnitOfWork> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    public IUserRepository UserRepository => 
        GetCustomRepository<IUserRepository>(() => new EfUserRepository(_context, _logger));
        
    public IOrderRepository OrderRepository => 
        GetCustomRepository<IOrderRepository>(() => new EfOrderRepository(_context, _logger));
    
    public IRepository<TEntity> Repository<TEntity>() where TEntity : class, IEntity
    {
        var type = typeof(TEntity);
        
        if (!_repositories.ContainsKey(type))
        {
            var repositoryType = typeof(Repository<>).MakeGenericType(type);
            var repository = Activator.CreateInstance(repositoryType, _context, _logger);
            _repositories[type] = repository;
        }
        
        return (IRepository<TEntity>)_repositories[type];
    }
    
    public Result<int> SaveChanges()
    {
        return Result.Try(() =>
        {
            var affected = _context.SaveChanges();
            return Result.Success(affected);
        })
        .MapError(ex => HandleSaveError(ex));
    }
    
    public async Task<Result<int>> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await Result.TryAsync(async () =>
        {
            var affected = await _context.SaveChangesAsync(cancellationToken);
            return Result.Success(affected);
        })
        .MapError(ex => HandleSaveError(ex));
    }
    
    public Result BeginTransaction()
    {
        return Result.Try(() =>
        {
            if (_currentTransaction != null)
                return new Error("TRANSACTION_EXISTS", "A transaction is already in progress");
                
            _currentTransaction = _context.Database.BeginTransaction();
            return Result.Success();
        });
    }
    
    public Result CommitTransaction()
    {
        return Result.Try(() =>
        {
            if (_currentTransaction == null)
                return new Error("NO_TRANSACTION", "No transaction to commit");
                
            _currentTransaction.Commit();
            _currentTransaction.Dispose();
            _currentTransaction = null;
            
            return Result.Success();
        })
        .MapError(ex =>
        {
            RollbackTransaction();
            return new Error("COMMIT_FAILED", $"Failed to commit transaction: {ex.Message}");
        });
    }
    
    public Result RollbackTransaction()
    {
        return Result.Try(() =>
        {
            if (_currentTransaction == null)
                return Result.Success();
                
            _currentTransaction.Rollback();
            _currentTransaction.Dispose();
            _currentTransaction = null;
            
            return Result.Success();
        });
    }
    
    private TRepo GetCustomRepository<TRepo>(Func<TRepo> factory)
    {
        var type = typeof(TRepo);
        
        if (!_repositories.ContainsKey(type))
            _repositories[type] = factory();
            
        return (TRepo)_repositories[type];
    }
    
    private Error HandleSaveError(Exception ex)
    {
        _logger.LogError(ex, "Failed to save changes");
        
        return ex switch
        {
            DbUpdateConcurrencyException => new ConflictError(
                "The record was modified by another user"),
            DbUpdateException dbEx when IsConstraintViolation(dbEx) => 
                ParseConstraintViolation(dbEx),
            _ => new Error("SAVE_FAILED", "Failed to save changes to database")
        };
    }
    
    public void Dispose()
    {
        _currentTransaction?.Dispose();
        _context.Dispose();
    }
}
```

### Using Unit of Work

```csharp
public class OrderService
{
    private readonly IUnitOfWork _unitOfWork;
    
    public async Task<Result<Order>> CreateOrderAsync(CreateOrderRequest request)
    {
        return await _unitOfWork.BeginTransaction()
            .BindAsync(async () => await ValidateCustomer(request.CustomerId))
            .BindAsync(async customer => await CreateOrder(customer, request))
            .BindAsync(async order => await ReserveInventory(order))
            .BindAsync(async order => await ProcessPayment(order))
            .BindAsync(async order => await _unitOfWork.SaveChangesAsync()
                .Map(_ => order))
            .TapAsync(async _ => await _unitOfWork.CommitTransaction())
            .TapErrorAsync(async _ => await _unitOfWork.RollbackTransaction());
    }
    
    private async Task<Result<Order>> CreateOrder(Customer customer, CreateOrderRequest request)
    {
        var order = new Order
        {
            CustomerId = customer.Id,
            Items = request.Items.Select(i => new OrderItem(i)).ToList(),
            Status = OrderStatus.Pending
        };
        
        return await _unitOfWork.OrderRepository.AddAsync(order)
            .Map(_ => order);
    }
}
```

## Caching Strategies

### Cached Repository Decorator

```csharp
public class CachedUserRepository : IUserRepository
{
    private readonly IUserRepository _innerRepository;
    private readonly IMemoryCache _cache;
    private readonly TimeSpan _cacheDuration;
    
    public CachedUserRepository(
        IUserRepository innerRepository,
        IMemoryCache cache,
        TimeSpan? cacheDuration = null)
    {
        _innerRepository = innerRepository;
        _cache = cache;
        _cacheDuration = cacheDuration ?? TimeSpan.FromMinutes(5);
    }
    
    public Result<User> GetById(Guid id)
    {
        var cacheKey = $"user_{id}";
        
        if (_cache.TryGetValue<User>(cacheKey, out var cachedUser))
            return Result.Success(cachedUser);
            
        return _innerRepository.GetById(id)
            .Tap(user => _cache.Set(cacheKey, user, _cacheDuration));
    }
    
    public Result<User> GetByEmail(string email)
    {
        var cacheKey = $"user_email_{email.ToLowerInvariant()}";
        
        return _cache.GetOrCreate(cacheKey, entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = _cacheDuration;
            return _innerRepository.GetByEmail(email);
        });
    }
    
    public Result Update(User user)
    {
        return _innerRepository.Update(user)
            .Tap(() => InvalidateCache(user));
    }
    
    public Result Delete(Guid id)
    {
        return _innerRepository.Delete(id)
            .Tap(() => _cache.Remove($"user_{id}"));
    }
    
    private void InvalidateCache(User user)
    {
        _cache.Remove($"user_{user.Id}");
        _cache.Remove($"user_email_{user.Email.ToLowerInvariant()}");
        
        // Invalidate any list caches
        InvalidateListCaches();
    }
    
    private void InvalidateListCaches()
    {
        // Remove all cached lists
        // In production, use cache tags or more sophisticated invalidation
        _cache.Remove("users_all");
        _cache.Remove("users_active");
    }
}
```

### Redis Cache Repository

```csharp
public class RedisCachedRepository<T> : IRepository<T> where T : class, IEntity
{
    private readonly IRepository<T> _innerRepository;
    private readonly IConnectionMultiplexer _redis;
    private readonly IDatabase _cache;
    private readonly string _keyPrefix;
    private readonly TimeSpan _expiration;
    
    public RedisCachedRepository(
        IRepository<T> innerRepository,
        IConnectionMultiplexer redis,
        TimeSpan? expiration = null)
    {
        _innerRepository = innerRepository;
        _redis = redis;
        _cache = redis.GetDatabase();
        _keyPrefix = $"{typeof(T).Name.ToLower()}:";
        _expiration = expiration ?? TimeSpan.FromMinutes(10);
    }
    
    public Result<T> GetById(Guid id)
    {
        var key = $"{_keyPrefix}{id}";
        
        return GetFromCache(key)
            .OrElse(() => _innerRepository.GetById(id)
                .Tap(entity => SetCache(key, entity)));
    }
    
    public Result<IReadOnlyList<T>> Find(ISpecification<T> specification)
    {
        // For complex queries, compute a cache key from the specification
        var specKey = ComputeSpecificationKey(specification);
        var key = $"{_keyPrefix}spec:{specKey}";
        
        return GetListFromCache(key)
            .OrElse(() => _innerRepository.Find(specification)
                .Tap(list => SetCache(key, list, TimeSpan.FromMinutes(5))));
    }
    
    private Option<T> GetFromCache(string key)
    {
        try
        {
            var json = _cache.StringGet(key);
            if (!json.HasValue)
                return Option<T>.None;
                
            var entity = JsonSerializer.Deserialize<T>(json);
            return Option.From(entity);
        }
        catch
        {
            return Option<T>.None;
        }
    }
    
    private void SetCache(string key, T entity, TimeSpan? expiration = null)
    {
        try
        {
            var json = JsonSerializer.Serialize(entity);
            _cache.StringSet(key, json, expiration ?? _expiration);
        }
        catch
        {
            // Log but don't fail the operation
        }
    }
    
    private string ComputeSpecificationKey(ISpecification<T> spec)
    {
        // Create a unique key based on specification properties
        var parts = new List<string>();
        
        if (spec.Criteria != null)
            parts.Add($"criteria:{spec.Criteria.GetHashCode()}");
            
        if (spec.OrderBy != null)
            parts.Add($"orderby:{spec.OrderBy.GetHashCode()}");
            
        if (spec.IsPagingEnabled)
            parts.Add($"page:{spec.Skip}:{spec.Take}");
            
        return string.Join(":", parts);
    }
}
```

## Event Sourcing

### Event Sourced Repository

```csharp
public interface IEventSourcedRepository<T> where T : AggregateRoot
{
    Result<T> GetById(Guid id);
    Result<T> GetByIdAtVersion(Guid id, int version);
    Result Save(T aggregate);
    Result<IReadOnlyList<DomainEvent>> GetEvents(Guid aggregateId);
}

public abstract class AggregateRoot
{
    private readonly List<DomainEvent> _events = new();
    
    public Guid Id { get; protected set; }
    public int Version { get; private set; }
    
    public IReadOnlyList<DomainEvent> GetUncommittedEvents() => _events;
    
    public void MarkEventsAsCommitted()
    {
        _events.Clear();
    }
    
    protected void RaiseEvent(DomainEvent @event)
    {
        _events.Add(@event);
        Apply(@event);
        Version++;
    }
    
    protected abstract void Apply(DomainEvent @event);
    
    public void LoadFromHistory(IEnumerable<DomainEvent> events)
    {
        foreach (var @event in events)
        {
            Apply(@event);
            Version++;
        }
    }
}

public class EventSourcedRepository<T> : IEventSourcedRepository<T> where T : AggregateRoot, new()
{
    private readonly IEventStore _eventStore;
    private readonly ISnapshotStore _snapshotStore;
    
    public Result<T> GetById(Guid id)
    {
        // Try to load from snapshot
        var snapshotResult = _snapshotStore.GetLatestSnapshot<T>(id);
        
        T aggregate;
        int fromVersion = 0;
        
        if (snapshotResult.IsSuccess)
        {
            aggregate = snapshotResult.Value.Aggregate;
            fromVersion = snapshotResult.Value.Version;
        }
        else
        {
            aggregate = new T { Id = id };
        }
        
        // Load events after snapshot
        return _eventStore.GetEvents(id, fromVersion)
            .Map(events =>
            {
                aggregate.LoadFromHistory(events);
                return aggregate;
            });
    }
    
    public Result Save(T aggregate)
    {
        var events = aggregate.GetUncommittedEvents();
        if (!events.Any())
            return Result.Success();
            
        return _eventStore.SaveEvents(aggregate.Id, events, aggregate.Version)
            .Tap(() => aggregate.MarkEventsAsCommitted())
            .Tap(() => TryCreateSnapshot(aggregate));
    }
    
    private void TryCreateSnapshot(T aggregate)
    {
        // Create snapshot every N events
        if (aggregate.Version % 10 == 0)
        {
            _snapshotStore.SaveSnapshot(new Snapshot<T>
            {
                AggregateId = aggregate.Id,
                Aggregate = aggregate,
                Version = aggregate.Version,
                CreatedAt = DateTime.UtcNow
            });
        }
    }
}
```

## Testing Repositories

### In-Memory Repository for Testing

```csharp
public class InMemoryRepository<T> : IRepository<T> where T : class, IEntity
{
    private readonly Dictionary<Guid, T> _entities = new();
    private readonly List<ISpecification<T>> _appliedSpecifications = new();
    
    public Result<T> GetById(Guid id)
    {
        return _entities.TryGetValue(id, out var entity) && !entity.IsDeleted
            ? Result.Success(entity)
            : new NotFoundError($"Entity with ID {id} not found");
    }
    
    public Result<IReadOnlyList<T>> Find(ISpecification<T> specification)
    {
        _appliedSpecifications.Add(specification);
        
        var query = _entities.Values.AsQueryable();
        
        if (specification.Criteria != null)
            query = query.Where(specification.Criteria);
            
        if (specification.OrderBy != null)
            query = query.OrderBy(specification.OrderBy);
        else if (specification.OrderByDescending != null)
            query = query.OrderByDescending(specification.OrderByDescending);
            
        if (specification.IsPagingEnabled)
            query = query.Skip(specification.Skip).Take(specification.Take);
            
        var results = query.ToList();
        return Result.Success<IReadOnlyList<T>>(results);
    }
    
    public Result<Guid> Add(T entity)
    {
        if (entity.Id == Guid.Empty)
            entity.Id = Guid.NewGuid();
            
        entity.CreatedAt = DateTime.UtcNow;
        _entities[entity.Id] = entity;
        
        return Result.Success(entity.Id);
    }
    
    // Test helpers
    public void VerifySpecificationWasUsed<TSpec>() where TSpec : ISpecification<T>
    {
        if (!_appliedSpecifications.Any(s => s is TSpec))
            throw new InvalidOperationException($"Specification {typeof(TSpec).Name} was not used");
    }
    
    public void Reset()
    {
        _entities.Clear();
        _appliedSpecifications.Clear();
    }
}
```

### Repository Test Base

```csharp
public abstract class RepositoryTestBase<TRepository, TEntity> 
    where TRepository : IRepository<TEntity>
    where TEntity : class, IEntity
{
    protected TRepository Repository { get; private set; }
    protected abstract TRepository CreateRepository();
    protected abstract TEntity CreateValidEntity();
    
    [SetUp]
    public virtual void SetUp()
    {
        Repository = CreateRepository();
    }
    
    [Test]
    public void GetById_WithExistingEntity_ReturnsSuccess()
    {
        // Arrange
        var entity = CreateValidEntity();
        var addResult = Repository.Add(entity);
        addResult.Should().BeSuccess();
        
        // Act
        var result = Repository.GetById(entity.Id);
        
        // Assert
        result.Should().BeSuccessWithValue(entity);
    }
    
    [Test]
    public void GetById_WithNonExistentId_ReturnsNotFound()
    {
        // Act
        var result = Repository.GetById(Guid.NewGuid());
        
        // Assert
        result.Should()
            .BeFailure()
            .WithError<NotFoundError>();
    }
    
    [Test]
    public void Add_WithValidEntity_ReturnsSuccessWithId()
    {
        // Arrange
        var entity = CreateValidEntity();
        
        // Act
        var result = Repository.Add(entity);
        
        // Assert
        result.Should().BeSuccess();
        result.Value.Should().NotBeEmpty();
        entity.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }
    
    [Test]
    public void Update_WithExistingEntity_ReturnsSuccess()
    {
        // Arrange
        var entity = CreateValidEntity();
        Repository.Add(entity);
        
        // Modify entity
        ModifyEntity(entity);
        
        // Act
        var result = Repository.Update(entity);
        
        // Assert
        result.Should().BeSuccess();
        
        var updated = Repository.GetById(entity.Id);
        updated.Should().BeSuccess();
        VerifyEntityWasUpdated(updated.Value);
    }
    
    protected abstract void ModifyEntity(TEntity entity);
    protected abstract void VerifyEntityWasUpdated(TEntity entity);
}
```

## Advanced Patterns

### Repository with Domain Events

```csharp
public interface IDomainEventRepository<T> : IRepository<T> where T : class, IEntity
{
    Result<IReadOnlyList<DomainEvent>> GetPendingEvents();
    Result MarkEventsAsPublished(IEnumerable<DomainEvent> events);
}

public class DomainEventRepository<T> : Repository<T>, IDomainEventRepository<T> 
    where T : class, IEntity, IAggregateRoot
{
    private readonly List<DomainEvent> _pendingEvents = new();
    
    public override Result<Guid> Add(T entity)
    {
        return base.Add(entity)
            .Tap(() => CollectDomainEvents(entity));
    }
    
    public override Result Update(T entity)
    {
        return base.Update(entity)
            .Tap(() => CollectDomainEvents(entity));
    }
    
    private void CollectDomainEvents(IAggregateRoot aggregate)
    {
        var events = aggregate.GetDomainEvents();
        _pendingEvents.AddRange(events);
        aggregate.ClearDomainEvents();
    }
    
    public Result<IReadOnlyList<DomainEvent>> GetPendingEvents()
    {
        return Result.Success<IReadOnlyList<DomainEvent>>(_pendingEvents.ToList());
    }
    
    public Result MarkEventsAsPublished(IEnumerable<DomainEvent> events)
    {
        foreach (var @event in events)
        {
            _pendingEvents.Remove(@event);
        }
        return Result.Success();
    }
}
```

### Repository with Audit Trail

```csharp
public interface IAuditableRepository<T> : IRepository<T> where T : class, IEntity
{
    Result<IReadOnlyList<AuditEntry>> GetAuditTrail(Guid entityId);
}

public class AuditableRepository<T> : Repository<T>, IAuditableRepository<T> 
    where T : class, IEntity
{
    private readonly ICurrentUserService _currentUserService;
    
    public override Result<Guid> Add(T entity)
    {
        return base.Add(entity)
            .Tap(id => CreateAuditEntry(id, "Created", null, entity));
    }
    
    public override Result Update(T entity)
    {
        return GetById(entity.Id)
            .Bind(existing =>
            {
                var changes = GetChanges(existing, entity);
                return base.Update(entity)
                    .Tap(() => CreateAuditEntry(entity.Id, "Updated", existing, entity, changes));
            });
    }
    
    public override Result Delete(Guid id)
    {
        return GetById(id)
            .Bind(existing => base.Delete(id)
                .Tap(() => CreateAuditEntry(id, "Deleted", existing, null)));
    }
    
    private void CreateAuditEntry(
        Guid entityId, 
        string action, 
        T oldValue, 
        T newValue, 
        Dictionary<string, (object Old, object New)> changes = null)
    {
        var entry = new AuditEntry
        {
            EntityId = entityId,
            EntityType = typeof(T).Name,
            Action = action,
            UserId = _currentUserService.UserId,
            Timestamp = DateTime.UtcNow,
            OldValues = oldValue != null ? JsonSerializer.Serialize(oldValue) : null,
            NewValues = newValue != null ? JsonSerializer.Serialize(newValue) : null,
            Changes = changes != null ? JsonSerializer.Serialize(changes) : null
        };
        
        Context.Set<AuditEntry>().Add(entry);
    }
    
    public Result<IReadOnlyList<AuditEntry>> GetAuditTrail(Guid entityId)
    {
        return Result.Try(() =>
        {
            var entries = Context.Set<AuditEntry>()
                .Where(a => a.EntityId == entityId && a.EntityType == typeof(T).Name)
                .OrderByDescending(a => a.Timestamp)
                .ToList();
                
            return Result.Success<IReadOnlyList<AuditEntry>>(entries);
        });
    }
}
```

## Best Practices

### 1. Return Results, Not Exceptions

```csharp
// Bad: Throwing exceptions
public User GetUser(Guid id)
{
    var user = _context.Users.Find(id);
    if (user == null)
        throw new EntityNotFoundException($"User {id} not found");
    return user;
}

// Good: Returning Results
public Result<User> GetUser(Guid id)
{
    var user = _context.Users.Find(id);
    return user != null
        ? Result.Success(user)
        : new NotFoundError($"User {id} not found");
}
```

### 2. Use Specifications for Complex Queries

```csharp
// Bad: Complex queries in repository
public List<User> GetActiveUsersWithOrdersInLastMonth()
{
    return _context.Users
        .Include(u => u.Orders)
        .Where(u => u.IsActive && 
                   u.Orders.Any(o => o.CreatedAt > DateTime.Now.AddMonths(-1)))
        .ToList();
}

// Good: Use specifications
public Result<IReadOnlyList<User>> Find(ISpecification<User> specification)
{
    var query = ApplySpecification(specification);
    return Result.Success<IReadOnlyList<User>>(query.ToList());
}
```

### 3. Handle Concurrency

```csharp
public Result Update(T entity)
{
    return Result.Try(() =>
    {
        Context.Entry(entity).State = EntityState.Modified;
        Context.SaveChanges();
        return Result.Success();
    })
    .MapError(ex => ex is DbUpdateConcurrencyException
        ? new ConflictError("The record was modified by another user")
        : HandleDatabaseError(ex));
}
```

### 4. Implement Proper Transaction Handling

```csharp
public async Task<Result<Order>> CreateOrderWithItemsAsync(Order order)
{
    using var transaction = await Context.Database.BeginTransactionAsync();
    
    return await AddOrderAsync(order)
        .BindAsync(async () => await AddOrderItemsAsync(order.Items))
        .BindAsync(async () => await UpdateInventoryAsync(order.Items))
        .TapAsync(async () => await transaction.CommitAsync())
        .TapErrorAsync(async () => await transaction.RollbackAsync());
}
```

### 5. Keep Repositories Focused

```csharp
// Bad: Business logic in repository
public class UserRepository
{
    public Result<User> RegisterUser(string email, string password)
    {
        // Validation, hashing, sending emails... No!
    }
}

// Good: Repository only handles data access
public class UserRepository
{
    public Result<User> GetByEmail(string email) { }
    public Result<Guid> Add(User user) { }
    public Result Update(User user) { }
}
```

## Summary

Repository patterns with FluentUnions provide:

1. **Explicit error handling** - All data operations return Results
2. **Testable data access** - Easy to mock and test
3. **Consistent patterns** - Uniform API across repositories
4. **Flexible querying** - Specification pattern for complex queries
5. **Advanced features** - Caching, auditing, event sourcing

Key principles:
- Return Results instead of throwing exceptions
- Use specifications for complex queries
- Implement proper transaction handling
- Keep repositories focused on data access
- Handle concurrency and constraints properly

Next steps:
- [Service Patterns](service-patterns.md)
- [API Patterns](api-patterns.md)
- [Testing Guide](../guides/testing-guide.md)
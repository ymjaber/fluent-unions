namespace FluentUnions;

/// <summary>
/// Represents an error that occurs when an operation cannot be completed due to a conflict
/// with the current state of a resource or system.
/// This error type is specifically designed for scenarios where concurrent modifications,
/// state conflicts, or business rule violations prevent an operation from succeeding.
/// </summary>
/// <remarks>
/// <para>
/// ConflictError provides a semantic way to distinguish conflict scenarios from other types
/// of errors. This is particularly important in distributed systems, REST APIs, and concurrent
/// applications where conflicts need special handling strategies like retry logic or user intervention.
/// </para>
/// <para>
/// Common use cases include:
/// - Optimistic concurrency conflicts (version mismatch, ETag conflicts)
/// - Duplicate key violations when creating resources
/// - Business state conflicts (trying to cancel an already-shipped order)
/// - Resource lock conflicts in concurrent operations
/// - Conflicting business rules (scheduling conflicts, resource allocation conflicts)
/// </para>
/// <para>
/// The error type helps in implementing proper conflict resolution strategies, such as:
/// - Returning HTTP 409 (Conflict) responses in web APIs
/// - Triggering retry mechanisms with exponential backoff
/// - Prompting users to resolve conflicts manually
/// - Implementing merge strategies for concurrent updates
/// - Providing detailed information about the conflicting state
/// </para>
/// </remarks>
/// <example>
/// <code>
/// // Simple conflict error
/// var error = new ConflictError("User.DuplicateEmail", "A user with this email already exists");
/// 
/// // Conflict error with metadata
/// var metadata = new Dictionary&lt;string, object&gt;
/// {
///     ["Email"] = email,
///     ["ExistingUserId"] = existingUserId,
///     ["AttemptedOperation"] = "CreateUser"
/// };
/// var detailedError = new ConflictError("User.DuplicateEmail", 
///     $"Cannot create user: email '{email}' is already in use", metadata);
/// 
/// // Optimistic concurrency conflict
/// public Result&lt;Order&gt; UpdateOrder(Order order, int expectedVersion)
/// {
///     var currentVersion = database.GetOrderVersion(order.Id);
///     if (currentVersion != expectedVersion)
///     {
///         var metadata = new Dictionary&lt;string, object&gt;
///         {
///             ["OrderId"] = order.Id,
///             ["ExpectedVersion"] = expectedVersion,
///             ["CurrentVersion"] = currentVersion
///         };
///         return new ConflictError("Order.VersionConflict", 
///             "Order has been modified by another user", metadata);
///     }
///     
///     return database.UpdateOrder(order);
/// }
/// 
/// // Business state conflict
/// public Result CancelOrder(int orderId)
/// {
///     var order = database.GetOrder(orderId);
///     if (order.Status == OrderStatus.Shipped)
///     {
///         return new ConflictError("Order.CannotCancel", 
///             "Cannot cancel order that has already been shipped");
///     }
///     
///     return database.CancelOrder(orderId);
/// }
/// </code>
/// </example>
[Serializable]
public class ConflictError : Error
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConflictError"/> class with the specified code, message, and metadata.
    /// </summary>
    /// <param name="code">
    /// A unique error code that identifies the type of conflict that occurred.
    /// Should follow a hierarchical naming convention (e.g., "User.DuplicateEmail", "Order.VersionConflict", "Resource.Locked").
    /// </param>
    /// <param name="message">
    /// A human-readable error message that describes the conflict and why the operation failed.
    /// Should clearly explain what is conflicting and potentially how to resolve it.
    /// </param>
    /// <param name="metadata">
    /// Additional contextual information about the conflict.
    /// Common metadata includes conflicting values, current state, expected state, timestamps, or version numbers.
    /// </param>
    /// <remarks>
    /// Use this constructor when you need to provide detailed information about the conflict,
    /// such as the current and expected values, which can help in automatic or manual conflict resolution.
    /// </remarks>
    public ConflictError(string code, string message, IDictionary<string, object> metadata) : base(code, message, metadata)
    {
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="ConflictError"/> class with the specified code and message.
    /// </summary>
    /// <param name="code">
    /// A unique error code that identifies the type of conflict that occurred.
    /// Should follow a hierarchical naming convention (e.g., "User.DuplicateEmail", "Order.VersionConflict", "Resource.Locked").
    /// </param>
    /// <param name="message">
    /// A human-readable error message that describes the conflict and why the operation failed.
    /// Should clearly explain what is conflicting and potentially how to resolve it.
    /// </param>
    /// <remarks>
    /// Use this constructor for simple conflict errors where additional metadata is not necessary.
    /// The message should still provide enough information for users to understand and potentially resolve the conflict.
    /// </remarks>
    public ConflictError(string code, string message) : base(code, message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConflictError"/> class with only a message.
    /// The error code will be set to an empty string.
    /// </summary>
    /// <param name="message">
    /// A human-readable error message that describes the conflict and why the operation failed.
    /// Should clearly explain what is conflicting and potentially how to resolve it.
    /// </param>
    /// <remarks>
    /// Use this constructor when you don't need to categorize the conflict error with a specific code.
    /// This is useful for simple conflict scenarios where the message itself provides sufficient context.
    /// </remarks>
    public ConflictError(string message) : base(message)
    {
    }
}
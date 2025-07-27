namespace FluentUnions;

/// <summary>
/// Represents an error that occurs when an operation conflicts with current state.
/// </summary>
/// <remarks>
/// Used for concurrency conflicts, duplicate keys, or business state violations. Maps to HTTP 409 in web contexts.
/// </remarks>
[Serializable]
public class ConflictError : ErrorWithMetadata
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
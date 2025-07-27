namespace FluentUnions;

/// <summary>
/// Represents an error that occurs when a requested resource cannot be found.
/// </summary>
/// <remarks>
/// Used for missing database records, files, API resources, or configuration settings. Maps to HTTP 404 in web contexts.
/// </remarks>
[Serializable]
public class NotFoundError : ErrorWithMetadata
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NotFoundError"/> class with the specified code, message, and metadata.
    /// </summary>
    /// <param name="code">
    /// A unique error code that identifies the type of resource that was not found.
    /// Should follow a hierarchical naming convention (e.g., "User.NotFound", "Order.NotFound", "File.NotFound").
    /// </param>
    /// <param name="message">
    /// A human-readable error message that describes what was not found.
    /// Should include relevant identifiers or search criteria when appropriate.
    /// </param>
    /// <param name="metadata">
    /// Additional contextual information about what was searched for and how.
    /// Common metadata includes IDs, search parameters, paths, or timestamps.
    /// </param>
    /// <remarks>
    /// Use this constructor when you need to provide additional context about the failed lookup,
    /// such as the search criteria used or related entities that might help the user.
    /// </remarks>
    public NotFoundError(string code, string message, IDictionary<string, object> metadata) 
        : base(code, message, metadata)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NotFoundError"/> class with the specified code and message.
    /// </summary>
    /// <param name="code">
    /// A unique error code that identifies the type of resource that was not found.
    /// Should follow a hierarchical naming convention (e.g., "User.NotFound", "Order.NotFound", "File.NotFound").
    /// </param>
    /// <param name="message">
    /// A human-readable error message that describes what was not found.
    /// Should include relevant identifiers or search criteria when appropriate.
    /// </param>
    /// <remarks>
    /// Use this constructor for simple not found errors where additional metadata is not necessary.
    /// The message should still be descriptive enough to help users understand what was not found.
    /// </remarks>
    public NotFoundError(string code, string message) : base(code, message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NotFoundError"/> class with only a message.
    /// The error code will be set to an empty string.
    /// </summary>
    /// <param name="message">
    /// A human-readable error message that describes what was not found.
    /// Should include relevant identifiers or search criteria when appropriate.
    /// </param>
    /// <remarks>
    /// Use this constructor when you don't need to categorize the not found error with a specific code.
    /// This is useful for simple scenarios where the message itself provides sufficient context.
    /// </remarks>
    public NotFoundError(string message) : base(message)
    {
    }
}
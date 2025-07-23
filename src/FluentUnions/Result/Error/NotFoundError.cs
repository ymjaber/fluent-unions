namespace FluentUnions;

/// <summary>
/// Represents an error that occurs when a requested resource, entity, or item cannot be found.
/// This error type is specifically designed for scenarios where lookups, queries, or searches
/// fail to locate the expected data.
/// </summary>
/// <remarks>
/// <para>
/// NotFoundError provides a semantic way to distinguish "not found" scenarios from other types
/// of errors. This is particularly important in REST APIs and domain services where a 404-style
/// error needs to be handled differently from validation errors or system failures.
/// </para>
/// <para>
/// Common use cases include:
/// - Database queries that return no results for a given ID
/// - File system operations where a file or directory doesn't exist
/// - API endpoints trying to access non-existent resources
/// - Cache misses that should be treated as errors
/// - Configuration or settings that are missing
/// </para>
/// <para>
/// The error type helps in implementing proper error handling strategies, such as:
/// - Returning HTTP 404 responses in web APIs
/// - Triggering fallback mechanisms
/// - Differentiating between "not found" and "access denied" scenarios
/// - Providing helpful error messages to guide users
/// </para>
/// </remarks>
/// <example>
/// <code>
/// // Simple not found error
/// var error = new NotFoundError("User.NotFound", "User with ID '123' was not found");
/// 
/// // Not found error with metadata
/// var metadata = new Dictionary&lt;string, object&gt;
/// {
///     ["UserId"] = userId,
///     ["SearchCriteria"] = "ID",
///     ["Timestamp"] = DateTime.UtcNow
/// };
/// var detailedError = new NotFoundError("User.NotFound", $"User with ID '{userId}' was not found", metadata);
/// 
/// // Used in a repository method
/// public Result&lt;User&gt; GetUserById(int userId)
/// {
///     var user = database.Users.FindById(userId);
///     if (user == null)
///         return new NotFoundError("User.NotFound", $"User with ID '{userId}' was not found");
///     
///     return user;
/// }
/// 
/// // Used in a file service
/// public Result&lt;string&gt; ReadFile(string path)
/// {
///     if (!File.Exists(path))
///         return new NotFoundError("File.NotFound", $"File '{path}' does not exist");
///     
///     return File.ReadAllText(path);
/// }
/// </code>
/// </example>
[Serializable]
public class NotFoundError : Error
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
namespace FluentUnions;

/// <summary>
/// Represents an error that occurs when data validation fails.
/// </summary>
/// <remarks>
/// Used for input validation, business rule violations, and domain constraint violations.
/// </remarks>
[Serializable]
public class ValidationError : ErrorWithMetadata
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationError"/> class with the specified code, message, and metadata.
    /// </summary>
    /// <param name="code">
    /// A unique error code that identifies the type of validation failure.
    /// Should follow a hierarchical naming convention (e.g., "User.Email.Invalid", "Order.Quantity.TooLarge").
    /// </param>
    /// <param name="message">
    /// A human-readable error message that describes the validation failure.
    /// Should be clear and actionable for end users or developers.
    /// </param>
    /// <param name="metadata">
    /// Additional contextual information about the validation error.
    /// Common metadata includes field names, invalid values, validation constraints, and limits.
    /// </param>
    /// <remarks>
    /// Use this constructor when you need to provide additional context about the validation failure
    /// through metadata. This is particularly useful for client applications that need to handle
    /// specific validation scenarios or display field-specific error messages.
    /// </remarks>
    public ValidationError(string code, string message, IDictionary<string, object> metadata) : base(code, message,
        metadata)
    {
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationError"/> class with the specified code and message.
    /// </summary>
    /// <param name="code">
    /// A unique error code that identifies the type of validation failure.
    /// Should follow a hierarchical naming convention (e.g., "User.Email.Invalid", "Order.Quantity.TooLarge").
    /// </param>
    /// <param name="message">
    /// A human-readable error message that describes the validation failure.
    /// Should be clear and actionable for end users or developers.
    /// </param>
    /// <remarks>
    /// Use this constructor for simple validation errors that don't require additional metadata.
    /// This is the most commonly used constructor for ValidationError instances.
    /// </remarks>
    public ValidationError(string code, string message) : base(code, message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationError"/> class with only a message.
    /// The error code will be set to an empty string.
    /// </summary>
    /// <param name="message">
    /// A human-readable error message that describes the validation failure.
    /// Should be clear and actionable for end users or developers.
    /// </param>
    /// <remarks>
    /// Use this constructor when you don't need to categorize the validation error with a specific code.
    /// This is useful for simple validation scenarios or when the message itself is sufficient.
    /// </remarks>
    public ValidationError(string message) : base(message)
    {
    }
}
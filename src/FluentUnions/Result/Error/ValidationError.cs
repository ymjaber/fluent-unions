namespace FluentUnions;

/// <summary>
/// Represents an error that occurs when data validation fails.
/// This error type is specifically designed for scenarios where input data, business rules,
/// or domain constraints are violated.
/// </summary>
/// <remarks>
/// <para>
/// ValidationError is the most commonly used error type in FluentUnions and is used extensively
/// throughout the PreDefinedEnsure validation methods. It provides a semantic way to distinguish
/// validation failures from other types of errors like system failures or missing resources.
/// </para>
/// <para>
/// Common use cases include:
/// - Input validation (email format, string length, numeric ranges)
/// - Business rule violations (insufficient balance, expired membership)
/// - Domain constraint violations (duplicate username, invalid state transitions)
/// - Data integrity checks (referential integrity, consistency rules)
/// </para>
/// <para>
/// ValidationError inherits all functionality from the base Error class, including support
/// for error codes, messages, and metadata. The type itself serves as a semantic marker
/// to help distinguish validation failures from other error categories.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// // Simple validation error
/// var error = new ValidationError("User.InvalidEmail", "Email format is invalid");
/// 
/// // Validation error with metadata
/// var metadata = new Dictionary&lt;string, object&gt;
/// {
///     ["Field"] = "email",
///     ["Value"] = "invalid-email",
///     ["Pattern"] = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"
/// };
/// var detailedError = new ValidationError("User.InvalidEmail", "Email format is invalid", metadata);
/// 
/// // Used in a validation method
/// public static Result&lt;string&gt; ValidateEmail(string email)
/// {
///     if (!IsValidEmail(email))
///         return new ValidationError("User.InvalidEmail", $"'{email}' is not a valid email address");
///     
///     return email;
/// }
/// </code>
/// </example>
[Serializable]
public class ValidationError : Error
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
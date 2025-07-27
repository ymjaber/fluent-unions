namespace FluentUnions;

/// <summary>
/// Represents an error that occurs when authentication fails.
/// </summary>
/// <remarks>
/// Used for invalid credentials, expired tokens, or missing authentication. Maps to HTTP 401 in web contexts.
/// </remarks>
[Serializable]
public class AuthenticationError : ErrorWithMetadata
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AuthenticationError"/> class with the specified code, message, and metadata.
    /// </summary>
    /// <param name="code">
    /// A unique error code that identifies the type of authentication failure.
    /// Should follow a hierarchical naming convention (e.g., "Auth.InvalidCredentials", "Auth.TokenExpired", "Auth.MfaRequired").
    /// </param>
    /// <param name="message">
    /// A human-readable error message that describes the authentication failure.
    /// Should be generic enough to avoid leaking security information (e.g., don't reveal if username exists).
    /// </param>
    /// <param name="metadata">
    /// Additional contextual information about the authentication failure.
    /// Common metadata includes authentication method, failed attempts count, or required authentication factors.
    /// Be careful not to include sensitive information like passwords or full tokens.
    /// </param>
    /// <remarks>
    /// Use this constructor when you need to provide additional context about the authentication failure,
    /// such as the number of failed attempts or the authentication method used.
    /// </remarks>
    public AuthenticationError(string code, string message, IDictionary<string, object> metadata) 
        : base(code, message, metadata)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthenticationError"/> class with the specified code and message.
    /// </summary>
    /// <param name="code">
    /// A unique error code that identifies the type of authentication failure.
    /// Should follow a hierarchical naming convention (e.g., "Auth.InvalidCredentials", "Auth.TokenExpired", "Auth.MfaRequired").
    /// </param>
    /// <param name="message">
    /// A human-readable error message that describes the authentication failure.
    /// Should be generic enough to avoid leaking security information (e.g., don't reveal if username exists).
    /// </param>
    /// <remarks>
    /// Use this constructor for simple authentication errors that don't require additional metadata.
    /// This is the most commonly used constructor for AuthenticationError instances.
    /// </remarks>
    public AuthenticationError(string code, string message) : base(code, message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthenticationError"/> class with only a message.
    /// The error code will be set to an empty string.
    /// </summary>
    /// <param name="message">
    /// A human-readable error message that describes the authentication failure.
    /// Should be generic enough to avoid leaking security information (e.g., don't reveal if username exists).
    /// </param>
    /// <remarks>
    /// Use this constructor when you don't need to categorize the authentication error with a specific code.
    /// This is useful for simple authentication scenarios where the message itself is sufficient.
    /// </remarks>
    public AuthenticationError(string message) : base(message)
    {
    }
}
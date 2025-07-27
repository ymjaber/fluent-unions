namespace FluentUnions;

/// <summary>
/// Represents an error that occurs when a user lacks necessary permissions.
/// </summary>
/// <remarks>
/// Used for insufficient roles, resource access denial, or policy violations. Maps to HTTP 403 in web contexts.
/// </remarks>
[Serializable]
public class AuthorizationError : ErrorWithMetadata
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AuthorizationError"/> class with the specified code, message, and metadata.
    /// </summary>
    /// <param name="code">
    /// A unique error code that identifies the type of authorization failure.
    /// Should follow a hierarchical naming convention (e.g., "Auth.InsufficientPermissions", "Auth.RoleRequired", "Auth.ResourceAccessDenied").
    /// </param>
    /// <param name="message">
    /// A human-readable error message that describes why access was denied.
    /// Should clearly indicate what permission is missing without exposing sensitive system details.
    /// </param>
    /// <param name="metadata">
    /// Additional contextual information about the authorization failure.
    /// Common metadata includes required permissions, user's current permissions, resource identifiers, or attempted actions.
    /// Avoid including sensitive information that could aid in privilege escalation attempts.
    /// </param>
    /// <remarks>
    /// Use this constructor when you need to provide detailed context about the authorization failure,
    /// which can help users understand what permissions they need or administrators to debug access issues.
    /// </remarks>
    public AuthorizationError(string code, string message, IDictionary<string, object> metadata) 
        : base(code, message, metadata)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthorizationError"/> class with the specified code and message.
    /// </summary>
    /// <param name="code">
    /// A unique error code that identifies the type of authorization failure.
    /// Should follow a hierarchical naming convention (e.g., "Auth.InsufficientPermissions", "Auth.RoleRequired", "Auth.ResourceAccessDenied").
    /// </param>
    /// <param name="message">
    /// A human-readable error message that describes why access was denied.
    /// Should clearly indicate what permission is missing without exposing sensitive system details.
    /// </param>
    /// <remarks>
    /// Use this constructor for simple authorization errors where additional metadata is not necessary.
    /// The message should still provide enough information for users to understand why access was denied.
    /// </remarks>
    public AuthorizationError(string code, string message) : base(code, message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthorizationError"/> class with only a message.
    /// The error code will be set to an empty string.
    /// </summary>
    /// <param name="message">
    /// A human-readable error message that describes why access was denied.
    /// Should clearly indicate what permission is missing without exposing sensitive system details.
    /// </param>
    /// <remarks>
    /// Use this constructor when you don't need to categorize the authorization error with a specific code.
    /// This is useful for simple authorization scenarios where the message itself provides sufficient context.
    /// </remarks>
    public AuthorizationError(string message) : base(message)
    {
    }
}
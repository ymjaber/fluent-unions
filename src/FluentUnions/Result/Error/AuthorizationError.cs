namespace FluentUnions;

/// <summary>
/// Represents an error that occurs when a user lacks the necessary permissions to perform an operation.
/// This error type is specifically designed for scenarios where user identity is known but they lack
/// the required authorization to access a resource or perform an action.
/// </summary>
/// <remarks>
/// <para>
/// AuthorizationError provides a semantic way to distinguish authorization failures from authentication
/// or other types of errors. This is critical for implementing proper access control and security
/// policies in applications where different users have different levels of access.
/// </para>
/// <para>
/// Common use cases include:
/// - Insufficient role or permissions (user lacks admin role)
/// - Resource-based authorization failures (user can't access another user's data)
/// - Feature-based restrictions (feature not available in user's subscription plan)
/// - Time-based access restrictions (access outside allowed hours)
/// - Geographic restrictions (content not available in user's region)
/// - Policy-based access control violations
/// </para>
/// <para>
/// The error type helps in implementing proper authorization handling strategies, such as:
/// - Returning HTTP 403 (Forbidden) responses in web APIs
/// - Providing guidance on how to obtain necessary permissions
/// - Implementing principle of least privilege
/// - Audit logging for compliance and security monitoring
/// - Distinguishing between "not authenticated" (401) and "not authorized" (403)
/// </para>
/// </remarks>
/// <example>
/// <code>
/// // Simple authorization error
/// var error = new AuthorizationError("Auth.InsufficientPermissions", "User lacks admin privileges");
/// 
/// // Authorization error with metadata
/// var metadata = new Dictionary&lt;string, object&gt;
/// {
///     ["RequiredRole"] = "Admin",
///     ["UserRole"] = "Member",
///     ["Resource"] = "UserManagement",
///     ["Action"] = "Delete"
/// };
/// var detailedError = new AuthorizationError("Auth.RoleRequired", 
///     "Admin role required to delete users", metadata);
/// 
/// // Used in an authorization service
/// public Result&lt;T&gt; AuthorizeAction&lt;T&gt;(User user, string resource, string action, Func&lt;Result&lt;T&gt;&gt; operation)
/// {
///     if (!HasPermission(user, resource, action))
///     {
///         var metadata = new Dictionary&lt;string, object&gt;
///         {
///             ["UserId"] = user.Id,
///             ["Resource"] = resource,
///             ["Action"] = action
///         };
///         return new AuthorizationError("Auth.AccessDenied", 
///             $"Access denied to {action} {resource}", metadata);
///     }
///     
///     return operation();
/// }
/// 
/// // Resource-based authorization
/// public Result&lt;Document&gt; GetDocument(User user, int documentId)
/// {
///     var document = documentRepository.FindById(documentId);
///     if (document == null)
///         return new NotFoundError("Document.NotFound", $"Document {documentId} not found");
///     
///     if (document.OwnerId != user.Id &amp;&amp; !user.IsAdmin)
///         return new AuthorizationError("Auth.ResourceAccessDenied", 
///             "You don't have permission to access this document");
///     
///     return document;
/// }
/// </code>
/// </example>
[Serializable]
public class AuthorizationError : Error
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
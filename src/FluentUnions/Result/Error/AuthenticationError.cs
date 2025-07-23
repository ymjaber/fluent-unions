namespace FluentUnions;

/// <summary>
/// Represents an error that occurs when authentication fails due to invalid or missing credentials.
/// This error type is specifically designed for scenarios where user identity cannot be verified.
/// </summary>
/// <remarks>
/// <para>
/// AuthenticationError provides a semantic way to distinguish authentication failures from other types
/// of errors. This is particularly important in security-conscious applications where authentication
/// failures need special handling, such as audit logging, rate limiting, or account lockout policies.
/// </para>
/// <para>
/// Common use cases include:
/// - Invalid username/password combinations
/// - Expired or invalid authentication tokens (JWT, API keys)
/// - Missing authentication credentials
/// - Failed multi-factor authentication attempts
/// - Expired sessions requiring re-authentication
/// - Invalid OAuth/SAML responses
/// </para>
/// <para>
/// The error type helps in implementing proper authentication handling strategies, such as:
/// - Returning HTTP 401 (Unauthorized) responses in web APIs
/// - Triggering re-authentication flows
/// - Logging security events for audit purposes
/// - Implementing account lockout after repeated failures
/// - Distinguishing between "who you are" (authentication) vs "what you can do" (authorization)
/// </para>
/// </remarks>
/// <example>
/// <code>
/// // Simple authentication error
/// var error = new AuthenticationError("Auth.InvalidCredentials", "Invalid username or password");
/// 
/// // Authentication error with metadata
/// var metadata = new Dictionary&lt;string, object&gt;
/// {
///     ["Username"] = username,
///     ["AuthMethod"] = "password",
///     ["FailedAttempts"] = 3,
///     ["LastAttempt"] = DateTime.UtcNow
/// };
/// var detailedError = new AuthenticationError("Auth.InvalidCredentials", 
///     "Invalid username or password", metadata);
/// 
/// // Used in an authentication service
/// public Result&lt;AuthToken&gt; Authenticate(string username, string password)
/// {
///     var user = userRepository.FindByUsername(username);
///     if (user == null || !VerifyPassword(password, user.PasswordHash))
///         return new AuthenticationError("Auth.InvalidCredentials", "Invalid username or password");
///     
///     return GenerateToken(user);
/// }
/// 
/// // Token validation
/// public Result&lt;ClaimsPrincipal&gt; ValidateToken(string token)
/// {
///     if (IsTokenExpired(token))
///         return new AuthenticationError("Auth.TokenExpired", "Authentication token has expired");
///     
///     if (!IsTokenValid(token))
///         return new AuthenticationError("Auth.InvalidToken", "Authentication token is invalid");
///     
///     return ExtractClaims(token);
/// }
/// </code>
/// </example>
[Serializable]
public class AuthenticationError : Error
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
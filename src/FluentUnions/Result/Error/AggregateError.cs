namespace FluentUnions;

/// <summary>
/// Represents a composite error that contains multiple individual errors.
/// This error type is used when multiple validation failures or other errors occur simultaneously
/// and need to be reported together as a single unit.
/// </summary>
/// <remarks>
/// <para>
/// AggregateError is particularly useful in scenarios where:
/// - Multiple validation rules are checked and several fail
/// - Batch operations encounter errors on multiple items
/// - Complex business rules produce multiple error conditions
/// </para>
/// <para>
/// The error automatically generates a standard code ("Errors.Aggregate") and message ("Multiple errors occurred.")
/// while preserving all individual error details in the Errors collection.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// var errors = new List&lt;Error&gt;
/// {
///     new ValidationError("User.InvalidEmail", "Email format is invalid"),
///     new ValidationError("User.PasswordTooShort", "Password must be at least 8 characters"),
///     new ValidationError("User.UsernameTaken", "Username already exists")
/// };
/// 
/// var aggregateError = new AggregateError(errors);
/// // Returns a Result&lt;T&gt; with all validation errors combined
/// </code>
/// </example>
[Serializable]
public sealed class AggregateError : Error
{
    /// <summary>
    /// Gets the collection of individual errors contained within this aggregate error.
    /// </summary>
    /// <value>
    /// A read-only list of Error instances representing all the errors that occurred.
    /// This collection is never null and contains at least one error.
    /// </value>
    public IReadOnlyList<Error> Errors { get; }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="AggregateError"/> class with the specified collection of errors.
    /// </summary>
    /// <param name="errors">The collection of errors to aggregate. Must not be null or empty.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="errors"/> is null.</exception>
    /// <remarks>
    /// The constructor automatically sets the error code to "Errors.Aggregate" and 
    /// the message to "Multiple errors occurred." to provide a consistent interface
    /// for aggregate errors throughout the application.
    /// </remarks>
    public AggregateError(IReadOnlyList<Error> errors) : base("Errors.Aggregate", "Multiple errors occurred.")
    {
        Errors = errors;
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current aggregate error.
    /// </summary>
    /// <param name="obj">The object to compare with the current aggregate error.</param>
    /// <returns>
    /// <c>true</c> if the specified object is an AggregateError with the same sequence of errors; 
    /// otherwise, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Two AggregateError instances are considered equal if they contain the same errors
    /// in the same order. The comparison uses SequenceEqual to ensure both the content
    /// and order of errors match.
    /// </remarks>
    public override bool Equals(object? obj)
    {
        if (obj is not AggregateError other) return false;
        if (ReferenceEquals(this, other)) return true;

        return GetType() == obj.GetType() && Errors.SequenceEqual(other.Errors);
    }

    /// <summary>
    /// Returns the hash code for this aggregate error.
    /// </summary>
    /// <returns>A 32-bit signed integer hash code calculated from the contained errors.</returns>
    public override int GetHashCode() => HashCode.Combine(Errors);

    /// <summary>
    /// Returns a string representation of the aggregate error, including all contained errors.
    /// </summary>
    /// <returns>
    /// A string in the format: "{Code} - {Message} ( {comma-separated list of all errors} )"
    /// This provides a comprehensive view of all errors that occurred.
    /// </returns>
    /// <example>
    /// Output: "Errors.Aggregate - Multiple errors occurred. ( User.InvalidEmail - Email format is invalid, User.PasswordTooShort - Password must be at least 8 characters )"
    /// </example>
    public override string ToString()
    {
        var errors = string.Join(", ", Errors.Select(e => e.ToString()));
        
        return $"{Code} - {Message} ( {errors} )";
    }
}
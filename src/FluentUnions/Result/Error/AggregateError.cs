namespace FluentUnions;

/// <summary>
/// Represents a composite error containing multiple individual errors.
/// </summary>
/// <remarks>
/// Used when multiple errors occur simultaneously, such as validation failures or batch operation errors.
/// </remarks>
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
    internal AggregateError(IReadOnlyList<Error> errors) : base("Errors.Aggregate", "Multiple errors occurred.")
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

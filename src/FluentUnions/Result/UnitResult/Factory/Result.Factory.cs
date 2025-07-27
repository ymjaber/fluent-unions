namespace FluentUnions;

/// <summary>
/// Provides factory methods for creating <see cref="Result"/> instances.
/// </summary>
public readonly partial struct Result
{
    /// <summary>
    /// Creates a successful result.
    /// </summary>
    /// <returns>A result representing a successful operation.</returns>
    public static Result Success() => new();
    
    /// <summary>
    /// Creates a failed result with the specified error.
    /// </summary>
    /// <param name="error">The error that caused the failure.</param>
    /// <returns>A result representing a failed operation.</returns>
    public static Result Failure(Error error) => new(error);
}
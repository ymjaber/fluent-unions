namespace FluentUnions;

/// <summary>
/// Provides factory methods for creating <see cref="Result{TValue}"/> instances.
/// </summary>
public readonly partial struct Result
{
    /// <summary>
    /// Creates a successful result with the specified value.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="value">The value of the successful result.</param>
    /// <returns>A successful result containing the specified value.</returns>
    public static Result<TValue> For<TValue>(TValue value) => new(value);

    /// <summary>
    /// Creates a successful result with the specified value.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="value">The value of the successful result.</param>
    /// <returns>A successful result containing the specified value.</returns>
    public static Result<TValue> Success<TValue>(TValue value) => new(value);

    /// <summary>
    /// Creates a failed result with the specified error.
    /// </summary>
    /// <typeparam name="TValue">The type of the value that would have been returned on success.</typeparam>
    /// <param name="error">The error that caused the failure.</param>
    /// <returns>A failed result containing the specified error.</returns>
    public static Result<TValue> Failure<TValue>(Error error) => new(error);
}

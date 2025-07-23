namespace FluentUnions;

/// <summary>
/// Provides validation extension methods for <see cref="Result{TValue}"/> that allow adding
/// additional checks on the contained value while preserving the result chain.
/// </summary>
public static partial class ValueResultExtensions
{
    /// <summary>
    /// Creates an <see cref="EnsureBuilder{TValue}"/> that provides a fluent interface for chaining multiple validations.
    /// </summary>
    /// <typeparam name="TValue">The type of value contained in the result.</typeparam>
    /// <param name="result">The result to validate.</param>
    /// <returns>An <see cref="EnsureBuilder{TValue}"/> for building validation chains.</returns>
    /// <remarks>
    /// This method enables a fluent syntax for applying multiple validations to a result value,
    /// with built-in predicates for common validation scenarios.
    /// </remarks>
    public static EnsureBuilder<TValue> EnsureThat<TValue>(in this Result<TValue> result) => new(result);

    /// <summary>
    /// Ensures that an <see cref="Option{TValue}"/> contained in a result has a value (is Some).
    /// </summary>
    /// <typeparam name="TValue">The type of value contained in the option.</typeparam>
    /// <param name="result">The result containing an option to validate.</param>
    /// <param name="error">The error to return if the option is None. If null, a default error will be used.</param>
    /// <returns>
    /// A <see cref="Result{TValue}"/> containing the unwrapped value if the option has a value;
    /// otherwise, a failure result with the specified error.
    /// </returns>
    /// <remarks>
    /// This method is useful when working with results that contain optional values,
    /// allowing you to ensure the presence of a value and unwrap it in a single operation.
    /// </remarks>
    public static Result<TValue> EnsureSome<TValue>(in this Result<Option<TValue>> result, Error error = null!)
        where TValue : notnull
    {
        return result.Match(
            success: option => option.EnsureSome(error),
            failure: error => error
        );
    }

    /// <summary>
    /// Ensures that an <see cref="Option{TValue}"/> contained in a result has no value (is None).
    /// </summary>
    /// <typeparam name="TValue">The type of value that would be contained in the option.</typeparam>
    /// <param name="result">The result containing an option to validate.</param>
    /// <param name="error">The error to return if the option has a value. If null, a default error will be used.</param>
    /// <returns>
    /// A unit <see cref="Result"/> indicating success if the option is None;
    /// otherwise, a failure result with the specified error.
    /// </returns>
    /// <remarks>
    /// This method is useful for validation scenarios where the absence of a value is the expected state,
    /// such as checking for uniqueness or ensuring something doesn't exist.
    /// </remarks>
    public static Result EnsureNone<TValue>(in this Result<Option<TValue>> result, Error error = null!)
        where TValue : notnull
    {
        return result.Match(
            success: option => option.EnsureNone(),
            failure: error => error
        );
    }

    /// <summary>
    /// Ensures that the value in the result satisfies the specified predicate, returning a failure if it doesn't.
    /// </summary>
    /// <typeparam name="TValue">The type of value contained in the result.</typeparam>
    /// <param name="result">The result containing the value to validate.</param>
    /// <param name="predicate">A function that returns true if the value is valid.</param>
    /// <param name="error">The error to return if the predicate returns false.</param>
    /// <returns>
    /// The original result if it's already a failure or if the predicate returns true;
    /// otherwise, a failure result with the specified error.
    /// </returns>
    /// <remarks>
    /// This method is the fundamental validation method for value results.
    /// It only evaluates the predicate if the result is successful,
    /// allowing you to build up a series of validations that short-circuit on the first failure.
    /// </remarks>
    public static Result<TValue> Ensure<TValue>(
        in this Result<TValue> result,
        Func<TValue, bool> predicate,
        Error error)
    {
        if (result.IsFailure) return result;

        return predicate(result.Value) ? result : Result.Failure<TValue>(error);
    }
}
namespace FluentUnions;

/// <summary>
/// Provides validation extension methods for <see cref="Result{TValue}"/> that allow adding
/// additional checks on the contained value while preserving the result chain.
/// </summary>
public static partial class ValueResultExtensions
{
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

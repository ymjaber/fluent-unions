namespace FluentUnions;

/// <summary>
/// Provides validation extension methods for <see cref="Result{TValue}"/> that aggregate all errors
/// instead of short-circuiting on the first failure.
/// </summary>
public static partial class ValueResultExtensions
{
    /// <summary>
    /// Ensures that a condition is met, aggregating errors if the condition fails or the result is already a failure.
    /// </summary>
    /// <typeparam name="TValue">The type of value contained in the result.</typeparam>
    /// <param name="result">The current result.</param>
    /// <param name="condition">The condition that must be true.</param>
    /// <param name="error">The error to add if the condition is false.</param>
    /// <returns>
    /// The original result if it's successful and the condition is true;
    /// otherwise, a failure result containing all errors (both existing and new).
    /// </returns>
    /// <remarks>
    /// Unlike the regular Ensure method which preserves the original error,
    /// EnsureAll aggregates all errors. This is useful for validation scenarios
    /// where you want to check multiple conditions and report all violations at once,
    /// such as form validation where users should see all errors rather than one at a time.
    /// </remarks>
    public static Result<TValue> EnsureAll<TValue>(in this Result<TValue> result, bool condition, Error error)
    {
        if (condition) return result;
        
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .Append(error)
            .Build();
    }
}
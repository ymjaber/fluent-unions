namespace FluentUnions;

/// <summary>
/// Provides validation extension methods for <see cref="Result"/> that aggregate all errors
/// instead of short-circuiting on the first failure.
/// </summary>
public static partial class UnitResultExtensions
{
    /// <summary>
    /// Ensures that a condition is met, aggregating errors if the condition fails or the result is already a failure.
    /// </summary>
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
    /// where you want to check multiple conditions and report all violations at once.
    /// </remarks>
    public static Result EnsureAll(in this Result result, bool condition, Error error)
    {
        if (condition) return result;
        
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .Append(error)
            .Build();
    }
}
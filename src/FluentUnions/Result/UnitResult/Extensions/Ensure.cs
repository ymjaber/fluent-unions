namespace FluentUnions;

/// <summary>
/// Provides validation extension methods for <see cref="Result"/> that allow adding
/// additional checks while preserving the result chain.
/// </summary>
public static partial class UnitResultExtensions
{
    /// <summary>
    /// Ensures that a condition is met, returning a failure result if the predicate returns false.
    /// </summary>
    /// <param name="result">The current result.</param>
    /// <param name="predicate">A function that returns true if the condition is met.</param>
    /// <param name="error">The error to return if the predicate returns false.</param>
    /// <returns>
    /// The original result if it's already a failure or if the predicate returns true;
    /// otherwise, a failure result with the specified error.
    /// </returns>
    /// <remarks>
    /// This method is useful for adding validation steps in a result chain.
    /// It only evaluates the predicate if the current result is successful,
    /// allowing you to build up a series of validations that short-circuit on the first failure.
    /// </remarks>
    public static Result Ensure(in this Result result, Func<bool> predicate, Error error)
    {
        if (result.IsFailure) return result;
        
        return predicate() ? result : Result.Failure(error);
    }
}
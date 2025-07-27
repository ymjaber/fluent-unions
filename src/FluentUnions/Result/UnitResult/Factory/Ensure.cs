namespace FluentUnions;

public readonly partial struct Result
{
    /// <summary>
    /// Creates a success result if the condition is true, otherwise creates a failure result with the specified error.
    /// </summary>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="error">The error to return if the condition is false.</param>
    /// <returns>A success result if the condition is true; otherwise, a failure result with the specified error.</returns>
    public static Result Ensure(bool condition, Error error) => condition ? Success() : Failure(error);

    /// <summary>
    /// Evaluates multiple predicates sequentially and returns a failure result with the error of the first failing predicate.
    /// </summary>
    /// <param name="predicates">A collection of predicate-error pairs to evaluate.</param>
    /// <returns>A success result if all predicates return true; otherwise, a failure result with the error of the first failing predicate.</returns>
    /// <remarks>
    /// This method short-circuits on the first failing predicate, returning immediately without evaluating subsequent predicates.
    /// </remarks>
    public static Result Ensure(params ReadOnlySpan<(Func<bool> Predicate, Error Error)> predicates)
    {
        foreach (var (predicate, error) in predicates)
        {
            if (!predicate()) return error;
        }

        return Result.Success();
    }
}
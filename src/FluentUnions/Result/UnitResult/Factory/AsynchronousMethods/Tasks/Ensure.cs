namespace FluentUnions;

public readonly partial struct Result
{
    /// <summary>
    /// Asynchronously creates a success result if the condition is true, otherwise creates a failure result with the specified error.
    /// </summary>
    /// <param name="condition">The asynchronous condition to evaluate.</param>
    /// <param name="error">The error to return if the condition is false.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a success result if the condition is true; otherwise, a failure result with the specified error.</returns>
    public static async Task<Result> EnsureAsync(Task<bool> condition, Error error) => await condition.ConfigureAwait(false)
        ? Success()
        : Failure(error);

    /// <summary>
    /// Asynchronously evaluates multiple predicates sequentially and returns a failure result with the error of the first failing predicate.
    /// </summary>
    /// <param name="firstPredicate">The first predicate-error pair to evaluate.</param>
    /// <param name="predicates">Additional predicate-error pairs to evaluate.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a success result if all predicates return true; otherwise, a failure result with the error of the first failing predicate.</returns>
    /// <remarks>
    /// This method short-circuits on the first failing predicate, returning immediately without evaluating subsequent predicates.
    /// All tasks are configured with ConfigureAwait(false) for better performance in library code.
    /// </remarks>
    public static async Task<Result> EnsureAsync((Func<Task<bool>> Predicate, Error Error) firstPredicate, params (Func<Task<bool>> Predicate, Error Error)[] predicates)
    {
        if (!await firstPredicate.Predicate().ConfigureAwait(false)) return firstPredicate.Error;
        
        foreach (var (predicate, error) in predicates)
        {
            if (!await predicate().ConfigureAwait(false)) return Failure(error);
        }

        return Success();
    }
}
namespace FluentUnions;

public readonly partial struct Result
{
    /// <summary>
    /// Asynchronously combines multiple results, aggregating all errors from failed results into a single result.
    /// </summary>
    /// <param name="firstResult">The first asynchronous result to combine.</param>
    /// <param name="results">Additional asynchronous results to combine.</param>
    /// <returns>A value task that represents the asynchronous operation. The task result contains a success result if all results are successful; otherwise, a failure result with an aggregate error containing all errors from failed results.</returns>
    /// <remarks>
    /// Unlike BindAsync, this method evaluates all results and collects errors from all failures.
    /// If multiple results fail, the returned result will contain an AggregateError with all the collected errors.
    /// All value tasks are configured with ConfigureAwait(false) for better performance in library code.
    /// </remarks>
    public static async ValueTask<Result> BindAllAsync(ValueTask<Result> firstResult, params ValueTask<Result>[] results)
    {
        ErrorBuilder errorBuilder = new();
        
        errorBuilder.AppendOnFailure(await firstResult.ConfigureAwait(false));
        
        foreach (var res in results) errorBuilder.AppendOnFailure(await res.ConfigureAwait(false));

        return errorBuilder.TryBuild(out var error) ? Failure(error) : Success();
    }
}
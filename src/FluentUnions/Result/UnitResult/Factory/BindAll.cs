namespace FluentUnions;

public readonly partial struct Result
{
    /// <summary>
    /// Combines multiple results, aggregating all errors from failed results into a single result.
    /// </summary>
    /// <param name="results">A collection of Result values to combine.</param>
    /// <returns>A success result if all results are successful; otherwise, a failure result with an aggregate error containing all errors from failed results.</returns>
    /// <remarks>
    /// Unlike Bind, this method evaluates all results and collects errors from all failures.
    /// If multiple results fail, the returned result will contain an AggregateError with all the collected errors.
    /// </remarks>
    public static Result BindAll(params ReadOnlySpan<Result> results)
    {
        ErrorBuilder errorBuilder = new();
        
        foreach (var result in results) errorBuilder.AppendOnFailure(result);

        return errorBuilder.TryBuild(out var error) ? error : Success();
    }
}
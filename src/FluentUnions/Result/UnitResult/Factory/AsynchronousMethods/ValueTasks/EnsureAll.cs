namespace FluentUnions;

public readonly partial struct Result
{
    /// <summary>
    /// Asynchronously evaluates all conditions and aggregates all errors from failing conditions into a single result.
    /// </summary>
    /// <param name="firstPredicate">The first condition-error pair to evaluate.</param>
    /// <param name="conditions">Additional condition-error pairs to evaluate.</param>
    /// <returns>A value task that represents the asynchronous operation. The task result contains a success result if all conditions are true; otherwise, a failure result with an aggregate error containing all errors from failing conditions.</returns>
    /// <remarks>
    /// Unlike EnsureAsync, this method evaluates all conditions regardless of failures and collects all errors.
    /// If multiple conditions fail, the result will contain an AggregateError with all the collected errors.
    /// All value tasks are configured with ConfigureAwait(false) for better performance in library code.
    /// </remarks>
    public static async ValueTask<Result> EnsureAllAsync((ValueTask<bool> Condition, Error Error) firstPredicate, params (ValueTask<bool> Condition, Error Error)[] conditions)
    {
        ErrorBuilder errorBuilder = new();
        
        if (!await firstPredicate.Condition.ConfigureAwait(false)) errorBuilder.Append(firstPredicate.Error);

        foreach (var (condition, error) in conditions)
        {
            if (!await condition.ConfigureAwait(false)) errorBuilder.Append(error);
        }

        return errorBuilder.TryBuild(out var err) ? Failure(err) : Success();
    }
}
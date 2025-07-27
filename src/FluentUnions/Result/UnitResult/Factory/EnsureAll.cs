namespace FluentUnions;

public readonly partial struct Result
{
    /// <summary>
    /// Evaluates all conditions and aggregates all errors from failing conditions into a single result.
    /// </summary>
    /// <param name="predicates">A collection of condition-error pairs to evaluate.</param>
    /// <returns>A success result if all conditions are true; otherwise, a failure result with an aggregate error containing all errors from failing conditions.</returns>
    /// <remarks>
    /// Unlike Ensure, this method evaluates all conditions regardless of failures and collects all errors.
    /// If multiple conditions fail, the result will contain an AggregateError with all the collected errors.
    /// </remarks>
    public static Result EnsureAll(params ReadOnlySpan<(bool Condition, Error Error)> predicates)
    {
        ErrorBuilder errorBuilder = new();

        foreach (var (condition, error) in predicates)
        {
            if (!condition) errorBuilder.Append(error);
        }

        return errorBuilder.TryBuild(out Error? err) ? err : Success();
    }
}

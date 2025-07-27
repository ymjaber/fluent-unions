namespace FluentUnions;

public readonly partial struct Result
{
    /// <summary>
    /// Executes a sequence of result-producing functions sequentially, returning the first failure or success if all succeed.
    /// </summary>
    /// <param name="results">A collection of functions that produce Result values.</param>
    /// <returns>The first failure result encountered; otherwise, a success result if all functions return success.</returns>
    /// <remarks>
    /// This method implements the monadic bind operation for multiple results. It short-circuits on the first failure,
    /// meaning subsequent functions won't be executed once a failure is encountered.
    /// </remarks>
    public static Result Bind(params ReadOnlySpan<Func<Result>> results)
    {
        foreach (var res in results)
        {
            var result = res();
            if (result.IsFailure) return result;
        }

        return Success();
    }
}

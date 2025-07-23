namespace FluentUnions;

public readonly partial struct Result
{
    /// <summary>
    /// Asynchronously executes a sequence of result-producing functions sequentially, returning the first failure or success if all succeed.
    /// </summary>
    /// <param name="firstResult">The first asynchronous result to evaluate.</param>
    /// <param name="results">Additional functions that produce asynchronous Result values.</param>
    /// <returns>A value task that represents the asynchronous operation. The task result contains the first failure result encountered; otherwise, a success result if all functions return success.</returns>
    /// <remarks>
    /// This method implements the asynchronous monadic bind operation for multiple results. It short-circuits on the first failure,
    /// meaning subsequent functions won't be executed once a failure is encountered.
    /// All value tasks are configured with ConfigureAwait(false) for better performance in library code.
    /// </remarks>
    public static async ValueTask<Result> BindAsync(ValueTask<Result> firstResult, params Func<ValueTask<Result>>[] results)
    {
        var result = await firstResult.ConfigureAwait(false);
        if (result.IsFailure) return result;
        
        foreach (var res in results)
        {
            result = await res().ConfigureAwait(false);
            if (result.IsFailure) return result;
        }

        return Result.Success();
    }
}
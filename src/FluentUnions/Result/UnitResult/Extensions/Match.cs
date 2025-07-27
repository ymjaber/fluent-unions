namespace FluentUnions;

/// <summary>
/// Provides pattern matching extension methods for <see cref="Result"/> that allow handling
/// both success and failure cases to produce a value.
/// </summary>
public static partial class UnitResultExtensions
{
    /// <summary>
    /// Performs pattern matching on the result, executing one of two functions based on the result state.
    /// </summary>
    /// <typeparam name="TTarget">The type of value to return from the pattern match.</typeparam>
    /// <param name="result">The result to match against.</param>
    /// <param name="success">The function to execute if the result is successful.</param>
    /// <param name="failure">The function to execute if the result is a failure, receiving the error as a parameter.</param>
    /// <returns>The value returned by either the <paramref name="success"/> or <paramref name="failure"/> function.</returns>
    /// <remarks>
    /// This method provides exhaustive pattern matching for <see cref="Result"/>,
    /// ensuring that both success and failure cases are handled. It's particularly useful
    /// when you need to transform a result into another type regardless of its state.
    /// </remarks>
    public static TTarget Match<TTarget>(in this Result result, Func<TTarget> success, Func<Error, TTarget> failure)
    {
        return result.IsSuccess ? success() : failure(result.Error);
    }
}
namespace FluentUnions;

/// <summary>
/// Provides bind extension methods for <see cref="Result{TValue}"/> that enable chaining operations
/// that may also return a Result, propagating failures automatically.
/// </summary>
public static partial class ValueResultExtensions
{
    /// <summary>
    /// Chains a subsequent operation that returns a unit <see cref="Result"/> while preserving the original value if successful.
    /// </summary>
    /// <typeparam name="TValue">The type of value in the current result.</typeparam>
    /// <param name="result">The current result containing a value.</param>
    /// <param name="next">A function that takes the current value and returns a unit <see cref="Result"/>.</param>
    /// <returns>
    /// The original result if both the current result and the next operation are successful;
    /// otherwise, a failure result with the appropriate error.
    /// </returns>
    /// <remarks>
    /// This method is useful when you need to perform a validation or side-effect operation
    /// that doesn't produce a new value but may fail, while preserving the original value
    /// in the result chain.
    /// </remarks>
    public static Result<TValue> Bind<TValue>(in this Result<TValue> result, Func<TValue, Result> next)
    {
        if (result.IsFailure) return result;

        Result nextResult = next(result.Value);
        if (nextResult.IsFailure) return Result.Failure<TValue>(nextResult.Error);

        return result;
    }

    /// <summary>
    /// Chains a subsequent operation that returns a <see cref="Result{TTarget}"/> based on the current value.
    /// </summary>
    /// <typeparam name="TSource">The type of value in the source result.</typeparam>
    /// <typeparam name="TTarget">The type of value in the target result.</typeparam>
    /// <param name="result">The current result containing a value.</param>
    /// <param name="binder">A function that takes the current value and returns a new <see cref="Result{TTarget}"/>.</param>
    /// <returns>
    /// The result of the <paramref name="binder"/> function if the current result is successful;
    /// otherwise, a failure result containing the current error.
    /// </returns>
    /// <remarks>
    /// This method implements the monadic bind operation for <see cref="Result{TValue}"/>.
    /// It allows for clean chaining of operations that may fail, automatically short-circuiting
    /// on the first failure encountered. The binder function can transform the value type.
    /// </remarks>
    public static Result<TTarget> Bind<TSource, TTarget>(
        in this Result<TSource> result,
        Func<TSource, Result<TTarget>> binder)
    {
        if (result.IsFailure) return Result.Failure<TTarget>(result.Error);
        return binder(result.Value);
    }
}
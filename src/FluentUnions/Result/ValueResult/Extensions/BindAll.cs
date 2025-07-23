namespace FluentUnions;

/// <summary>
/// Provides bind extension methods for <see cref="Result{TValue}"/> that aggregate all errors
/// instead of short-circuiting on the first failure.
/// </summary>
public static partial class ValueResultExtensions
{
    /// <summary>
    /// Combines a value result with a unit result, aggregating all errors if any failures are present.
    /// </summary>
    /// <typeparam name="TValue">The type of value in the current result.</typeparam>
    /// <param name="result">The current value result.</param>
    /// <param name="next">The unit result to combine with.</param>
    /// <returns>
    /// The original value result if both inputs are successful;
    /// otherwise, a failure result containing all errors from both results.
    /// </returns>
    /// <remarks>
    /// Unlike the regular Bind method which short-circuits on the first failure,
    /// BindAll evaluates both results and collects all errors. This is useful
    /// for validation scenarios where you want to perform additional checks
    /// that don't produce values while reporting all problems at once.
    /// </remarks>
    public static Result<TValue> BindAll<TValue>(in this Result<TValue> result, in Result next)
    {
        if (next.IsSuccess) return result;

        return new ErrorBuilder()
            .AppendOnFailure(result)
            .Append(next.Error)
            .Build();
    }

    /// <summary>
    /// Combines two value results, aggregating all errors if any failures are present.
    /// </summary>
    /// <typeparam name="TSource">The type of value in the source result.</typeparam>
    /// <typeparam name="TTarget">The type of value in the target result.</typeparam>
    /// <param name="result">The current value result.</param>
    /// <param name="binder">The target value result to combine with.</param>
    /// <returns>
    /// The target result if the source result is successful;
    /// otherwise, a failure result containing all errors from both results.
    /// </returns>
    /// <remarks>
    /// This overload allows transitioning from one value type to another
    /// while collecting all errors if any operations failed. It's useful when
    /// you want to perform multiple independent operations and collect all
    /// validation errors rather than stopping at the first failure.
    /// </remarks>
    public static Result<TTarget> BindAll<TSource, TTarget>(in this Result<TSource> result, in Result<TTarget> binder)
    {
        if (result.IsSuccess) return binder;

        return new ErrorBuilder()
            .Append(result.Error)
            .AppendOnFailure(binder)
            .Build();
    }
}
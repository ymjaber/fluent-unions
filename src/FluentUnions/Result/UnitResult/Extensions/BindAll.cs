namespace FluentUnions;

/// <summary>
/// Provides bind extension methods for <see cref="Result"/> that aggregate all errors
/// instead of short-circuiting on the first failure.
/// </summary>
public static partial class UnitResultExtensions
{
    /// <summary>
    /// Combines two results, aggregating all errors if any failures are present.
    /// </summary>
    /// <param name="result">The current result.</param>
    /// <param name="next">The next result to combine with.</param>
    /// <returns>
    /// A successful result if both inputs are successful;
    /// otherwise, a failure result containing all errors from both results.
    /// </returns>
    /// <remarks>
    /// Unlike the regular Bind method which short-circuits on the first failure,
    /// BindAll evaluates both results and collects all errors. This is useful
    /// for validation scenarios where you want to report all problems at once.
    /// </remarks>
    public static Result BindAll(in this Result result, in Result next)
    {
        if (next.IsSuccess) return result;

        return new ErrorBuilder()
            .AppendOnFailure(result)
            .Append(next.Error)
            .Build();
    }

    /// <summary>
    /// Combines a unit result with a value result, aggregating all errors if any failures are present.
    /// </summary>
    /// <typeparam name="TTarget">The type of value in the second result.</typeparam>
    /// <param name="result">The current unit result.</param>
    /// <param name="binder">The value result to combine with.</param>
    /// <returns>
    /// The value result if the unit result is successful;
    /// otherwise, a failure result containing all errors from both results.
    /// </returns>
    /// <remarks>
    /// This overload allows transitioning from a unit result to a value result
    /// while collecting all errors if any operations failed. It's useful when
    /// you want to perform validation that doesn't produce a value followed
    /// by an operation that does, while still collecting all errors.
    /// </remarks>
    public static Result<TTarget> BindAll<TTarget>(in this Result result, in Result<TTarget> binder)
    {
        if (result.IsSuccess) return binder;

        return new ErrorBuilder()
            .Append(result.Error)
            .AppendOnFailure(binder)
            .Build();
    }
}
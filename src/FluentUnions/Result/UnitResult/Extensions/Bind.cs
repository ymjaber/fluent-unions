namespace FluentUnions;

/// <summary>
/// Provides bind extension methods for <see cref="Result"/> that enable chaining operations
/// that may also return a Result, propagating failures automatically.
/// </summary>
public static partial class UnitResultExtensions
{
    /// <summary>
    /// Chains a subsequent operation that returns a <see cref="Result"/> if the current result is successful.
    /// </summary>
    /// <param name="result">The current result.</param>
    /// <param name="next">A function that returns the next <see cref="Result"/> in the chain.</param>
    /// <returns>
    /// The result of the <paramref name="next"/> function if the current result is successful;
    /// otherwise, the current failure result.
    /// </returns>
    /// <remarks>
    /// This method implements the monadic bind operation for <see cref="Result"/>.
    /// It allows for clean chaining of operations that may fail, automatically short-circuiting
    /// on the first failure encountered.
    /// </remarks>
    public static Result Bind(in this Result result, Func<Result> next)
    {
        if (result.IsFailure) return result;
        return next();
    }
    
    /// <summary>
    /// Chains a subsequent operation that returns a <see cref="Result{TTarget}"/> if the current result is successful.
    /// </summary>
    /// <typeparam name="TTarget">The type of value in the resulting <see cref="Result{TTarget}"/>.</typeparam>
    /// <param name="result">The current result.</param>
    /// <param name="binder">A function that returns a <see cref="Result{TTarget}"/>.</param>
    /// <returns>
    /// The result of the <paramref name="binder"/> function if the current result is successful;
    /// otherwise, a failure result containing the current error.
    /// </returns>
    /// <remarks>
    /// This overload allows transitioning from a unit result to a value result,
    /// useful when the first operation doesn't produce a value but subsequent operations do.
    /// </remarks>
    public static Result<TTarget> Bind<TTarget>(in this Result result, Func<Result<TTarget>> binder)
    {
        if (result.IsFailure) return result.Error;
        return binder();
    }
}
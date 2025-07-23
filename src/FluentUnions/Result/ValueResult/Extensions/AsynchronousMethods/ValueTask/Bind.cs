namespace FluentUnions;

/// <summary>Provides ValueTask-based asynchronous bind operations for <see cref="Result{T}"/> types.</summary>
public static partial class ValueResultExtensions
{
    /// <summary>
    /// Asynchronously chains a result with an operation that returns a unit result.
    /// </summary>
    /// <typeparam name="TValue">The type of value contained in the result.</typeparam>
    /// <param name="result">The result to bind from.</param>
    /// <param name="next">An asynchronous function that takes the success value and returns a ValueTask&lt;Result&gt;.</param>
    /// <returns>
    /// A ValueTask containing the original result if both operations succeeded;
    /// otherwise, a failed result with the first error encountered.
    /// </returns>
    /// <remarks>
    /// This method allows chaining operations where the subsequent operation doesn't produce a new value.
    /// Uses ValueTask for better performance when operations may complete synchronously.
    /// Note: Fixed bug where failure result wasn't being returned.
    /// </remarks>
    public static async ValueTask<Result<TValue>> BindAsync<TValue>(this Result<TValue> result,
        Func<TValue, ValueTask<Result>> next)
    {
        if (result.IsFailure) return result;

        Result r = await next(result.Value).ConfigureAwait(false);
        if (r.IsFailure) return Result.Failure<TValue>(r.Error); // Fixed: Added return statement

        return result;
    }

    /// <summary>
    /// Chains an asynchronous result with a synchronous operation that returns a unit result.
    /// </summary>
    /// <typeparam name="TValue">The type of value contained in the result.</typeparam>
    /// <param name="result">The ValueTask containing the result to bind from.</param>
    /// <param name="next">A synchronous function that takes the success value and returns a Result.</param>
    /// <returns>
    /// A ValueTask containing the original result if both operations succeeded;
    /// otherwise, a failed result with the first error encountered.
    /// </returns>
    /// <remarks>
    /// This overload is useful when you have an async result but a synchronous bind operation.
    /// Optimized for scenarios where the result might already be available synchronously.
    /// </remarks>
    public static async ValueTask<Result<TValue>> BindAsync<TValue>(this ValueTask<Result<TValue>> result,
        Func<TValue, Result> next)
        => (await result.ConfigureAwait(false)).Bind(next);

    /// <summary>
    /// Chains an asynchronous result with an asynchronous operation that returns a unit result.
    /// </summary>
    /// <typeparam name="TValue">The type of value contained in the result.</typeparam>
    /// <param name="result">The ValueTask containing the result to bind from.</param>
    /// <param name="next">An asynchronous function that takes the success value and returns a ValueTask&lt;Result&gt;.</param>
    /// <returns>
    /// A ValueTask containing the original result if both operations succeeded;
    /// otherwise, a failed result with the first error encountered.
    /// </returns>
    /// <remarks>
    /// This overload handles fully asynchronous pipelines where both the result and bind operation use ValueTask.
    /// Provides optimal performance for operations that may complete synchronously.
    /// </remarks>
    public static async ValueTask<Result<TValue>> BindAsync<TValue>(
        this ValueTask<Result<TValue>> result,
        Func<TValue, ValueTask<Result>> next)
        => await (await result.ConfigureAwait(false)).BindAsync(next).ConfigureAwait(false);

    /// <summary>
    /// Asynchronously transforms a successful result value using a function that returns a new result.
    /// </summary>
    /// <typeparam name="TSource">The type of the source value.</typeparam>
    /// <typeparam name="TTarget">The type of the target value.</typeparam>
    /// <param name="result">The source result to transform.</param>
    /// <param name="binder">An asynchronous function that takes the source value and returns a ValueTask&lt;Result&lt;TTarget&gt;&gt;.</param>
    /// <returns>
    /// A ValueTask containing the result of the binder function if the source was successful;
    /// otherwise, a ValueTask containing a failed result with the original error.
    /// </returns>
    /// <remarks>
    /// This is the ValueTask-based version of the monadic bind operation for Result types.
    /// It enables chaining async operations that might fail, with better performance for
    /// operations that may complete synchronously.
    /// </remarks>
    public static ValueTask<Result<TTarget>> BindAsync<TSource, TTarget>(
        in this Result<TSource> result,
        Func<TSource, ValueTask<Result<TTarget>>> binder)
    {
        if (result.IsFailure) return ValueTask.FromResult(Result.Failure<TTarget>(result.Error));
        return binder(result.Value);
    }

    /// <summary>
    /// Transforms an asynchronous result value using a synchronous function that returns a new result.
    /// </summary>
    /// <typeparam name="TSource">The type of the source value.</typeparam>
    /// <typeparam name="TTarget">The type of the target value.</typeparam>
    /// <param name="result">The ValueTask containing the source result to transform.</param>
    /// <param name="next">A synchronous function that takes the source value and returns a Result&lt;TTarget&gt;.</param>
    /// <returns>
    /// A ValueTask containing the result of the next function if the source was successful;
    /// otherwise, a ValueTask containing a failed result with the original error.
    /// </returns>
    /// <remarks>
    /// This overload is useful when you have an async result but a synchronous transformation.
    /// Optimized for scenarios where the result might already be available synchronously.
    /// </remarks>
    public static async ValueTask<Result<TTarget>> BindAsync<TSource, TTarget>(
        this ValueTask<Result<TSource>> result,
        Func<TSource, Result<TTarget>> next)
        => (await result.ConfigureAwait(false)).Bind(next);

    /// <summary>
    /// Transforms an asynchronous result value using an asynchronous function that returns a new result.
    /// </summary>
    /// <typeparam name="TSource">The type of the source value.</typeparam>
    /// <typeparam name="TTarget">The type of the target value.</typeparam>
    /// <param name="result">The ValueTask containing the source result to transform.</param>
    /// <param name="next">An asynchronous function that takes the source value and returns a ValueTask&lt;Result&lt;TTarget&gt;&gt;.</param>
    /// <returns>
    /// A ValueTask containing the result of the next function if the source was successful;
    /// otherwise, a ValueTask containing a failed result with the original error.
    /// </returns>
    /// <remarks>
    /// This overload handles fully asynchronous pipelines using ValueTask throughout,
    /// ideal for high-performance scenarios where operations may complete synchronously.
    /// </remarks>
    public static async ValueTask<Result<TTarget>> BindAsync<TSource, TTarget>(
        this ValueTask<Result<TSource>> result,
        Func<TSource, ValueTask<Result<TTarget>>> next)
        => await (await result.ConfigureAwait(false)).BindAsync(next).ConfigureAwait(false);
}
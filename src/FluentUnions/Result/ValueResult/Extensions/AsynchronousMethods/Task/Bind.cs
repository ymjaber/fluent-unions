namespace FluentUnions;

/// <summary>Provides Task-based asynchronous bind operations for <see cref="Result{T}"/> types.</summary>
public static partial class ValueResultExtensions
{
    /// <summary>
    /// Asynchronously chains a result with an operation that returns a unit result.
    /// </summary>
    /// <typeparam name="TValue">The type of value contained in the result.</typeparam>
    /// <param name="result">The result to bind from.</param>
    /// <param name="next">An asynchronous function that takes the success value and returns a Task&lt;Result&gt;.</param>
    /// <returns>
    /// A task containing the original result if both operations succeeded;
    /// otherwise, a failed result with the first error encountered.
    /// </returns>
    /// <remarks>
    /// This method allows chaining operations where the subsequent operation doesn't produce a new value.
    /// If the initial result is a failure or the next operation fails, the chain short-circuits.
    /// Note: There appears to be a bug in this implementation - it should return the failure result.
    /// </remarks>
    public static async Task<Result<TValue>> BindAsync<TValue>(this Result<TValue> result, Func<TValue, Task<Result>> next)
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
    /// <param name="result">The task containing the result to bind from.</param>
    /// <param name="next">A synchronous function that takes the success value and returns a Result.</param>
    /// <returns>
    /// A task containing the original result if both operations succeeded;
    /// otherwise, a failed result with the first error encountered.
    /// </returns>
    /// <remarks>
    /// This overload is useful when you have an async result but a synchronous bind operation.
    /// </remarks>
    public static async Task<Result<TValue>> BindAsync<TValue>(this Task<Result<TValue>> result, Func<TValue, Result> next)
        => (await result.ConfigureAwait(false)).Bind(next);

    /// <summary>
    /// Chains an asynchronous result with an asynchronous operation that returns a unit result.
    /// </summary>
    /// <typeparam name="TValue">The type of value contained in the result.</typeparam>
    /// <param name="result">The task containing the result to bind from.</param>
    /// <param name="next">An asynchronous function that takes the success value and returns a Task&lt;Result&gt;.</param>
    /// <returns>
    /// A task containing the original result if both operations succeeded;
    /// otherwise, a failed result with the first error encountered.
    /// </returns>
    /// <remarks>
    /// This overload handles fully asynchronous pipelines where both the result and bind operation are async.
    /// </remarks>
    public static async Task<Result<TValue>> BindAsync<TValue>(
        this Task<Result<TValue>> result,
        Func<TValue, Task<Result>> next)
        => await (await result.ConfigureAwait(false)).BindAsync(next).ConfigureAwait(false);

    /// <summary>
    /// Asynchronously transforms a successful result value using a function that returns a new result.
    /// </summary>
    /// <typeparam name="TSource">The type of the source value.</typeparam>
    /// <typeparam name="TTarget">The type of the target value.</typeparam>
    /// <param name="result">The source result to transform.</param>
    /// <param name="binder">An asynchronous function that takes the source value and returns a Task&lt;Result&lt;TTarget&gt;&gt;.</param>
    /// <returns>
    /// A task containing the result of the binder function if the source was successful;
    /// otherwise, a task containing a failed result with the original error.
    /// </returns>
    /// <remarks>
    /// This is the asynchronous version of the monadic bind operation for Result types.
    /// It enables chaining async operations that might fail, propagating the first failure.
    /// </remarks>
    public static Task<Result<TTarget>> BindAsync<TSource, TTarget>(
        in this Result<TSource> result,
        Func<TSource, Task<Result<TTarget>>> binder)
    {
        if (result.IsFailure) return Task.FromResult(Result.Failure<TTarget>(result.Error));
        return binder(result.Value);
    }
    
    /// <summary>
    /// Transforms an asynchronous result value using a synchronous function that returns a new result.
    /// </summary>
    /// <typeparam name="TSource">The type of the source value.</typeparam>
    /// <typeparam name="TTarget">The type of the target value.</typeparam>
    /// <param name="result">The task containing the source result to transform.</param>
    /// <param name="next">A synchronous function that takes the source value and returns a Result&lt;TTarget&gt;.</param>
    /// <returns>
    /// A task containing the result of the next function if the source was successful;
    /// otherwise, a task containing a failed result with the original error.
    /// </returns>
    /// <remarks>
    /// This overload is useful when you have an async result but a synchronous transformation.
    /// </remarks>
    public static async Task<Result<TTarget>> BindAsync<TSource, TTarget>(
        this Task<Result<TSource>> result,
        Func<TSource, Result<TTarget>> next)
        => (await result.ConfigureAwait(false)).Bind(next);
    
    /// <summary>
    /// Transforms an asynchronous result value using an asynchronous function that returns a new result.
    /// </summary>
    /// <typeparam name="TSource">The type of the source value.</typeparam>
    /// <typeparam name="TTarget">The type of the target value.</typeparam>
    /// <param name="result">The task containing the source result to transform.</param>
    /// <param name="next">An asynchronous function that takes the source value and returns a Task&lt;Result&lt;TTarget&gt;&gt;.</param>
    /// <returns>
    /// A task containing the result of the next function if the source was successful;
    /// otherwise, a task containing a failed result with the original error.
    /// </returns>
    /// <remarks>
    /// This overload handles fully asynchronous pipelines, ideal for chaining multiple async operations
    /// that each might fail independently.
    /// </remarks>
    public static async Task<Result<TTarget>> BindAsync<TSource, TTarget>(
        this Task<Result<TSource>> result,
        Func<TSource, Task<Result<TTarget>>> next)
        => await (await result.ConfigureAwait(false)).BindAsync(next).ConfigureAwait(false);
}
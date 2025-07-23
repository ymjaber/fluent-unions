namespace FluentUnions;

/// <summary>Provides Task-based asynchronous bind operations for <see cref="Result"/> types.</summary>
public static partial class UnitResultExtensions
{
    /// <summary>
    /// Asynchronously chains a unit result with another asynchronous operation that returns a unit result.
    /// </summary>
    /// <param name="result">The result to bind from.</param>
    /// <param name="next">An asynchronous function that returns a Task&lt;Result&gt;.</param>
    /// <returns>
    /// A task containing the result of the next operation if the initial result was successful;
    /// otherwise, a task containing the initial failure.
    /// </returns>
    /// <remarks>
    /// This method implements the monadic bind pattern for asynchronous operations without values.
    /// If the initial result is a failure, the next operation is not executed (short-circuit behavior).
    /// </remarks>
    public static Task<Result> BindAsync(this Result result, Func<Task<Result>> next)
    {
        if (result.IsFailure) return Task.FromResult(result);
        return next();
    }

    /// <summary>
    /// Chains an asynchronous unit result with a synchronous operation that returns a unit result.
    /// </summary>
    /// <param name="result">The task containing the result to bind from.</param>
    /// <param name="next">A synchronous function that returns a Result.</param>
    /// <returns>
    /// A task containing the result of the next operation if the initial result was successful;
    /// otherwise, a task containing the initial failure.
    /// </returns>
    /// <remarks>
    /// This overload is useful when you have an async result but a synchronous bind operation.
    /// It awaits the result first, then applies the synchronous bind.
    /// </remarks>
    public static async Task<Result> BindAsync(this Task<Result> result, Func<Result> next) =>
        (await result.ConfigureAwait(false)).Bind(next);

    /// <summary>
    /// Chains an asynchronous unit result with another asynchronous operation that returns a unit result.
    /// </summary>
    /// <param name="result">The task containing the result to bind from.</param>
    /// <param name="next">An asynchronous function that returns a Task&lt;Result&gt;.</param>
    /// <returns>
    /// A task containing the result of the next operation if the initial result was successful;
    /// otherwise, a task containing the initial failure.
    /// </returns>
    /// <remarks>
    /// This overload handles fully asynchronous pipelines where both the result and bind operation are async.
    /// Perfect for chaining multiple async operations that don't produce values.
    /// </remarks>
    public static async Task<Result> BindAsync(this Task<Result> result, Func<Task<Result>> next) =>
        await (await result.ConfigureAwait(false)).BindAsync(next).ConfigureAwait(false);

    /// <summary>
    /// Asynchronously chains a unit result with an operation that produces a value result.
    /// </summary>
    /// <typeparam name="TTarget">The type of value produced by the bind operation.</typeparam>
    /// <param name="result">The unit result to bind from.</param>
    /// <param name="binder">An asynchronous function that returns a Task&lt;Result&lt;TTarget&gt;&gt;.</param>
    /// <returns>
    /// A task containing the result of the binder function if the initial result was successful;
    /// otherwise, a task containing a failed result with the original error.
    /// </returns>
    /// <remarks>
    /// This method allows transitioning from a unit result to a value result in an async context.
    /// Useful when an operation that doesn't produce a value needs to be followed by one that does.
    /// </remarks>
    public static Task<Result<TTarget>> BindAsync<TTarget>(this Result result, Func<Task<Result<TTarget>>> binder)
    {
        if (result.IsFailure) return Task.FromResult(Result.Failure<TTarget>(result.Error));
        return binder();
    }

    /// <summary>
    /// Chains an asynchronous unit result with a synchronous operation that produces a value result.
    /// </summary>
    /// <typeparam name="TTarget">The type of value produced by the bind operation.</typeparam>
    /// <param name="result">The task containing the unit result to bind from.</param>
    /// <param name="binder">A synchronous function that returns a Result&lt;TTarget&gt;.</param>
    /// <returns>
    /// A task containing the result of the binder function if the initial result was successful;
    /// otherwise, a task containing a failed result with the original error.
    /// </returns>
    /// <remarks>
    /// This overload is useful when transitioning from an async unit result to a synchronous value-producing operation.
    /// </remarks>
    public static async Task<Result<TTarget>> BindAsync<TTarget>(this Task<Result> result, Func<Result<TTarget>> binder)
        => (await result.ConfigureAwait(false)).Bind(binder);

    /// <summary>
    /// Chains an asynchronous unit result with an asynchronous operation that produces a value result.
    /// </summary>
    /// <typeparam name="TTarget">The type of value produced by the bind operation.</typeparam>
    /// <param name="result">The task containing the unit result to bind from.</param>
    /// <param name="binder">An asynchronous function that returns a Task&lt;Result&lt;TTarget&gt;&gt;.</param>
    /// <returns>
    /// A task containing the result of the binder function if the initial result was successful;
    /// otherwise, a task containing a failed result with the original error.
    /// </returns>
    /// <remarks>
    /// This overload handles fully asynchronous pipelines when transitioning from a unit result to a value result.
    /// Ideal for complex async workflows where operations progressively produce values.
    /// </remarks>
    public static async Task<Result<TTarget>> BindAsync<TTarget>(
        this Task<Result> result,
        Func<Task<Result<TTarget>>> binder)
        => await (await result.ConfigureAwait(false)).BindAsync(binder).ConfigureAwait(false);
}
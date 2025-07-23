namespace FluentUnions;

/// <summary>Provides ValueTask-based asynchronous side effect methods for <see cref="Result"/> types.</summary>
public static partial class UnitResultExtensions
{
    /// <summary>
    /// Executes an asynchronous action if the result represents a success state.
    /// </summary>
    /// <param name="result">The result to check.</param>
    /// <param name="action">The asynchronous action to execute if the result is successful.</param>
    /// <returns>A ValueTask that completes with the original result, unmodified.</returns>
    /// <remarks>
    /// This is the ValueTask-based version, providing better performance for async side effects
    /// that may complete synchronously, such as cached operations or fast local I/O.
    /// </remarks>
    public static async ValueTask<Result> OnSuccessAsync(this Result result, Func<ValueTask> action)
    {
        if (result.IsSuccess) await action().ConfigureAwait(false);
        return result;
    }

    /// <summary>
    /// Executes a synchronous action if the ValueTask-wrapped result represents a success state.
    /// </summary>
    /// <param name="result">The ValueTask containing the result to check.</param>
    /// <param name="action">The action to execute if the result is successful.</param>
    /// <returns>A ValueTask that completes with the original result, unmodified.</returns>
    /// <remarks>
    /// This overload allows chaining synchronous actions on ValueTask-wrapped results.
    /// Optimized for scenarios where the result might already be available synchronously.
    /// </remarks>
    public static async ValueTask<Result> OnSuccessAsync(this ValueTask<Result> result, Action action) =>
        (await result.ConfigureAwait(false)).OnSuccess(action);

    /// <summary>
    /// Executes an asynchronous action if the ValueTask-wrapped result represents a success state.
    /// </summary>
    /// <param name="result">The ValueTask containing the result to check.</param>
    /// <param name="action">The asynchronous action to execute if the result is successful.</param>
    /// <returns>A ValueTask that completes with the original result, unmodified.</returns>
    /// <remarks>
    /// This overload enables fully asynchronous chains when working with ValueTask-wrapped results.
    /// Provides optimal performance for operations that may complete synchronously.
    /// </remarks>
    public static async ValueTask<Result> OnSuccessAsync(this ValueTask<Result> result, Func<ValueTask> action) =>
        await (await result.ConfigureAwait(false)).OnSuccessAsync(action).ConfigureAwait(false);

    /// <summary>
    /// Executes an asynchronous action with the error if the result represents a failure state.
    /// </summary>
    /// <param name="result">The result to check.</param>
    /// <param name="action">The asynchronous action to execute if the result is a failure, receiving the error as a parameter.</param>
    /// <returns>A ValueTask that completes with the original result, unmodified.</returns>
    /// <remarks>
    /// This is the ValueTask-based version for async error handling, providing better performance
    /// for operations that may complete synchronously, such as local error logging.
    /// </remarks>
    public static async ValueTask<Result> OnFailureAsync(this Result result, Func<Error, ValueTask> action)
    {
        if (result.IsFailure) await action(result.Error).ConfigureAwait(false);
        return result;
    }

    /// <summary>
    /// Executes a synchronous action with the error if the ValueTask-wrapped result represents a failure state.
    /// </summary>
    /// <param name="result">The ValueTask containing the result to check.</param>
    /// <param name="action">The action to execute if the result is a failure, receiving the error as a parameter.</param>
    /// <returns>A ValueTask that completes with the original result, unmodified.</returns>
    /// <remarks>
    /// This overload allows chaining synchronous error handling on ValueTask-wrapped results.
    /// Optimized for scenarios where the result might already be available synchronously.
    /// </remarks>
    public static async ValueTask<Result> OnFailureAsync(this ValueTask<Result> result, Action<Error> action) =>
        (await result.ConfigureAwait(false)).OnFailure(action);

    /// <summary>
    /// Executes an asynchronous action with the error if the ValueTask-wrapped result represents a failure state.
    /// </summary>
    /// <param name="result">The ValueTask containing the result to check.</param>
    /// <param name="action">The asynchronous action to execute if the result is a failure, receiving the error as a parameter.</param>
    /// <returns>A ValueTask that completes with the original result, unmodified.</returns>
    /// <remarks>
    /// This overload enables fully asynchronous error handling chains when working with ValueTask-wrapped results.
    /// </remarks>
    public static async ValueTask<Result> OnFailureAsync(this ValueTask<Result> result, Func<Error, ValueTask> action) =>
        await (await result.ConfigureAwait(false)).OnFailureAsync(action).ConfigureAwait(false);

    /// <summary>
    /// Executes one of two asynchronous actions based on whether the result represents success or failure.
    /// </summary>
    /// <param name="result">The result to check.</param>
    /// <param name="success">The asynchronous action to execute if the result is successful.</param>
    /// <param name="failure">The asynchronous action to execute if the result is a failure, receiving the error as a parameter.</param>
    /// <returns>A ValueTask that completes with the original result, unmodified.</returns>
    /// <remarks>
    /// This is the ValueTask-based version, providing a way to handle both success and failure cases
    /// asynchronously with better performance for operations that may complete synchronously.
    /// </remarks>
    public static async ValueTask<Result> OnEitherAsync(this Result result, Func<ValueTask> success, Func<Error, ValueTask> failure)
    {
        if (result.IsSuccess) await success().ConfigureAwait(false);
        else await failure(result.Error).ConfigureAwait(false);
        return result;
    }

    /// <summary>
    /// Executes one of two synchronous actions based on whether the ValueTask-wrapped result represents success or failure.
    /// </summary>
    /// <param name="result">The ValueTask containing the result to check.</param>
    /// <param name="success">The action to execute if the result is successful.</param>
    /// <param name="failure">The action to execute if the result is a failure, receiving the error as a parameter.</param>
    /// <returns>A ValueTask that completes with the original result, unmodified.</returns>
    /// <remarks>
    /// This overload allows branching with synchronous actions on ValueTask-wrapped results.
    /// Optimized for scenarios where the result might already be available synchronously.
    /// </remarks>
    public static async ValueTask<Result> OnEitherAsync(this ValueTask<Result> result, Action success, Action<Error> failure) =>
        (await result.ConfigureAwait(false)).OnEither(success, failure);

    /// <summary>
    /// Executes one of two asynchronous actions based on whether the ValueTask-wrapped result represents success or failure.
    /// </summary>
    /// <param name="result">The ValueTask containing the result to check.</param>
    /// <param name="success">The asynchronous action to execute if the result is successful.</param>
    /// <param name="failure">The asynchronous action to execute if the result is a failure, receiving the error as a parameter.</param>
    /// <returns>A ValueTask that completes with the original result, unmodified.</returns>
    /// <remarks>
    /// This overload enables fully asynchronous branching chains when working with ValueTask-wrapped results.
    /// Provides optimal performance for operations that may complete synchronously.
    /// </remarks>
    public static async ValueTask<Result> OnEitherAsync(
        this ValueTask<Result> result,
        Func<ValueTask> success,
        Func<Error, ValueTask> failure) =>
        await (await result.ConfigureAwait(false)).OnEitherAsync(success, failure).ConfigureAwait(false);
}
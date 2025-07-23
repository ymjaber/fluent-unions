namespace FluentUnions;

/// <summary>Provides ValueTask-based asynchronous side effect methods for <see cref="Result{T}"/> types.</summary>
public static partial class ValueResultExtensions
{
    /// <summary>
    /// Executes an asynchronous action with the result value if the result represents a success state.
    /// </summary>
    /// <typeparam name="TValue">The type of value contained in the result.</typeparam>
    /// <param name="result">The result to check.</param>
    /// <param name="action">The asynchronous action to execute if the result is successful, receiving the value as a parameter.</param>
    /// <returns>A ValueTask that represents the asynchronous operation, containing the original result.</returns>
    /// <remarks>
    /// This is the ValueTask-based version of OnSuccess, providing better performance for async side effects
    /// that may complete synchronously, such as cached operations or fast I/O.
    /// </remarks>
    public static async ValueTask<Result<TValue>> OnSuccessAsync<TValue>(this Result<TValue> result, Func<TValue, ValueTask> action)
    {
        if (result.IsSuccess) await action(result.Value).ConfigureAwait(false);
        return result;
    }

    /// <summary>
    /// Executes a synchronous action on the value of an asynchronous result if it represents a success state.
    /// </summary>
    /// <typeparam name="TValue">The type of value contained in the result.</typeparam>
    /// <param name="result">The ValueTask containing the result to check.</param>
    /// <param name="action">The action to execute if the result is successful, receiving the value as a parameter.</param>
    /// <returns>A ValueTask that represents the asynchronous operation, containing the original result.</returns>
    /// <remarks>
    /// This overload awaits the result ValueTask first, then executes a synchronous action on the value.
    /// Optimized for scenarios where the result might already be available synchronously.
    /// </remarks>
    public static async ValueTask<Result<TValue>> OnSuccessAsync<TValue>(this ValueTask<Result<TValue>> result, Action<TValue> action)
        => (await result.ConfigureAwait(false)).OnSuccess(action);

    /// <summary>
    /// Executes an asynchronous action on the value of an asynchronous result if it represents a success state.
    /// </summary>
    /// <typeparam name="TValue">The type of value contained in the result.</typeparam>
    /// <param name="result">The ValueTask containing the result to check.</param>
    /// <param name="action">The asynchronous action to execute if the result is successful, receiving the value as a parameter.</param>
    /// <returns>A ValueTask that represents the asynchronous operation, containing the original result.</returns>
    /// <remarks>
    /// This overload handles fully asynchronous pipelines where both the result and the action use ValueTask.
    /// Provides optimal performance for operations that may complete synchronously.
    /// </remarks>
    public static async ValueTask<Result<TValue>> OnSuccessAsync<TValue>(
        this ValueTask<Result<TValue>> result,
        Func<TValue, ValueTask> action)
        => await (await result.ConfigureAwait(false)).OnSuccessAsync(action).ConfigureAwait(false);

    /// <summary>
    /// Executes an asynchronous action with the error if the result represents a failure state.
    /// </summary>
    /// <typeparam name="TValue">The type of value that would be contained in a successful result.</typeparam>
    /// <param name="result">The result to check.</param>
    /// <param name="action">The asynchronous action to execute if the result is a failure, receiving the error as a parameter.</param>
    /// <returns>A ValueTask that represents the asynchronous operation, containing the original result.</returns>
    /// <remarks>
    /// This is the ValueTask-based version of OnFailure, providing better performance for async error handling
    /// operations that may complete synchronously, such as local logging or cached error responses.
    /// </remarks>
    public static async ValueTask<Result<TValue>> OnFailureAsync<TValue>(this Result<TValue> result, Func<Error, ValueTask> action)
    {
        if (result.IsFailure) await action(result.Error).ConfigureAwait(false);
        return result;
    }

    /// <summary>
    /// Executes a synchronous action on the error of an asynchronous result if it represents a failure state.
    /// </summary>
    /// <typeparam name="TValue">The type of value that would be contained in a successful result.</typeparam>
    /// <param name="result">The ValueTask containing the result to check.</param>
    /// <param name="action">The action to execute if the result is a failure, receiving the error as a parameter.</param>
    /// <returns>A ValueTask that represents the asynchronous operation, containing the original result.</returns>
    /// <remarks>
    /// This overload awaits the result ValueTask first, then executes a synchronous action on the error.
    /// Optimized for scenarios where the result might already be available synchronously.
    /// </remarks>
    public static async ValueTask<Result<TValue>> OnFailureAsync<TValue>(this ValueTask<Result<TValue>> result, Action<Error> action)
        => (await result.ConfigureAwait(false)).OnFailure(action);

    /// <summary>
    /// Executes an asynchronous action on the error of an asynchronous result if it represents a failure state.
    /// </summary>
    /// <typeparam name="TValue">The type of value that would be contained in a successful result.</typeparam>
    /// <param name="result">The ValueTask containing the result to check.</param>
    /// <param name="action">The asynchronous action to execute if the result is a failure, receiving the error as a parameter.</param>
    /// <returns>A ValueTask that represents the asynchronous operation, containing the original result.</returns>
    /// <remarks>
    /// This overload handles fully asynchronous pipelines where both the result and the action use ValueTask.
    /// </remarks>
    public static async ValueTask<Result<TValue>> OnFailureAsync<TValue>(
        this ValueTask<Result<TValue>> result,
        Func<Error, ValueTask> action)
        => await (await result.ConfigureAwait(false)).OnFailureAsync(action).ConfigureAwait(false);

    /// <summary>
    /// Executes one of two asynchronous actions based on whether the result represents success or failure.
    /// </summary>
    /// <typeparam name="TValue">The type of value contained in the result.</typeparam>
    /// <param name="result">The result to check.</param>
    /// <param name="success">The asynchronous action to execute if the result is successful, receiving the value as a parameter.</param>
    /// <param name="failure">The asynchronous action to execute if the result is a failure, receiving the error as a parameter.</param>
    /// <returns>A ValueTask that represents the asynchronous operation, containing the original result.</returns>
    /// <remarks>
    /// This is the ValueTask-based version of OnEither, ensuring exactly one of the two actions is executed
    /// based on the result's state. Provides optimal performance for async branching with side effects
    /// that may complete synchronously.
    /// </remarks>
    public static async ValueTask<Result<TValue>> OnEitherAsync<TValue>(
        this Result<TValue> result,
        Func<TValue, ValueTask> success,
        Func<Error, ValueTask> failure)
    {
        if (result.IsSuccess) await success(result.Value).ConfigureAwait(false);
        else await failure(result.Error).ConfigureAwait(false);
        return result;
    }

    /// <summary>
    /// Executes one of two synchronous actions on an asynchronous result based on its state.
    /// </summary>
    /// <typeparam name="TValue">The type of value contained in the result.</typeparam>
    /// <param name="result">The ValueTask containing the result to check.</param>
    /// <param name="success">The action to execute if the result is successful, receiving the value as a parameter.</param>
    /// <param name="failure">The action to execute if the result is a failure, receiving the error as a parameter.</param>
    /// <returns>A ValueTask that represents the asynchronous operation, containing the original result.</returns>
    /// <remarks>
    /// This overload awaits the result ValueTask first, then executes one of two synchronous actions.
    /// Optimized for scenarios where the result might already be available synchronously.
    /// </remarks>
    public static async ValueTask<Result<TValue>> OnEitherAsync<TValue>(
        this ValueTask<Result<TValue>> result,
        Action<TValue> success,
        Action<Error> failure)
        => (await result.ConfigureAwait(false)).OnEither(success, failure);

    /// <summary>
    /// Executes one of two asynchronous actions on an asynchronous result based on its state.
    /// </summary>
    /// <typeparam name="TValue">The type of value contained in the result.</typeparam>
    /// <param name="result">The ValueTask containing the result to check.</param>
    /// <param name="success">The asynchronous action to execute if the result is successful, receiving the value as a parameter.</param>
    /// <param name="failure">The asynchronous action to execute if the result is a failure, receiving the error as a parameter.</param>
    /// <returns>A ValueTask that represents the asynchronous operation, containing the original result.</returns>
    /// <remarks>
    /// This overload handles fully asynchronous pipelines where the result and both actions use ValueTask.
    /// Provides optimal performance for operations that may complete synchronously.
    /// </remarks>
    public static async ValueTask<Result<TValue>> OnEitherAsync<TValue>(
        this ValueTask<Result<TValue>> result,
        Func<TValue, ValueTask> success,
        Func<Error, ValueTask> failure)
        => await (await result.ConfigureAwait(false)).OnEitherAsync(success, failure).ConfigureAwait(false);
}
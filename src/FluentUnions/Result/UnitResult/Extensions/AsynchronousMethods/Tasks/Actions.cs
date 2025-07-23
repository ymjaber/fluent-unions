namespace FluentUnions;

/// <summary>
/// Provides asynchronous action-based extension methods for <see cref="Result"/> and <see cref="Task{Result}"/>
/// that allow executing async side effects based on the result state without modifying the result itself.
/// </summary>
public static partial class UnitResultExtensions
{
    /// <summary>
    /// Executes an asynchronous action if the result represents a success state.
    /// </summary>
    /// <param name="result">The result to check.</param>
    /// <param name="action">The asynchronous action to execute if the result is successful.</param>
    /// <returns>A task that completes with the original result, unmodified.</returns>
    /// <remarks>
    /// This method is useful for performing asynchronous side effects (like async logging or API calls)
    /// when a result is successful, while maintaining the fluent chain.
    /// </remarks>
    public static async Task<Result> OnSuccessAsync(this Result result, Func<Task> action)
    {
        if (result.IsSuccess) await action().ConfigureAwait(false);
        return result;
    }

    /// <summary>
    /// Executes a synchronous action if the task-wrapped result represents a success state.
    /// </summary>
    /// <param name="result">The task containing the result to check.</param>
    /// <param name="action">The action to execute if the result is successful.</param>
    /// <returns>A task that completes with the original result, unmodified.</returns>
    /// <remarks>
    /// This overload allows chaining synchronous actions on task-wrapped results without additional async overhead.
    /// </remarks>
    public static async Task<Result> OnSuccessAsync(this Task<Result> result, Action action) =>
        (await result.ConfigureAwait(false)).OnSuccess(action);

    /// <summary>
    /// Executes an asynchronous action if the task-wrapped result represents a success state.
    /// </summary>
    /// <param name="result">The task containing the result to check.</param>
    /// <param name="action">The asynchronous action to execute if the result is successful.</param>
    /// <returns>A task that completes with the original result, unmodified.</returns>
    /// <remarks>
    /// This overload enables fully asynchronous chains when working with task-wrapped results.
    /// </remarks>
    public static async Task<Result> OnSuccessAsync(this Task<Result> result, Func<Task> action) =>
        await (await result.ConfigureAwait(false)).OnSuccessAsync(action).ConfigureAwait(false);

    /// <summary>
    /// Executes an asynchronous action with the error if the result represents a failure state.
    /// </summary>
    /// <param name="result">The result to check.</param>
    /// <param name="action">The asynchronous action to execute if the result is a failure, receiving the error as a parameter.</param>
    /// <returns>A task that completes with the original result, unmodified.</returns>
    /// <remarks>
    /// This method is useful for asynchronous error handling (like async logging or error reporting)
    /// when a result is a failure, while maintaining the fluent chain.
    /// </remarks>
    public static async Task<Result> OnFailureAsync(this Result result, Func<Error, Task> action)
    {
        if (result.IsFailure) await action(result.Error).ConfigureAwait(false);
        return result;
    }

    /// <summary>
    /// Executes a synchronous action with the error if the task-wrapped result represents a failure state.
    /// </summary>
    /// <param name="result">The task containing the result to check.</param>
    /// <param name="action">The action to execute if the result is a failure, receiving the error as a parameter.</param>
    /// <returns>A task that completes with the original result, unmodified.</returns>
    /// <remarks>
    /// This overload allows chaining synchronous error handling on task-wrapped results without additional async overhead.
    /// </remarks>
    public static async Task<Result> OnFailureAsync(this Task<Result> result, Action<Error> action) =>
        (await result.ConfigureAwait(false)).OnFailure(action);

    /// <summary>
    /// Executes an asynchronous action with the error if the task-wrapped result represents a failure state.
    /// </summary>
    /// <param name="result">The task containing the result to check.</param>
    /// <param name="action">The asynchronous action to execute if the result is a failure, receiving the error as a parameter.</param>
    /// <returns>A task that completes with the original result, unmodified.</returns>
    /// <remarks>
    /// This overload enables fully asynchronous error handling chains when working with task-wrapped results.
    /// </remarks>
    public static async Task<Result> OnFailureAsync(this Task<Result> result, Func<Error, Task> action) =>
        await (await result.ConfigureAwait(false)).OnFailureAsync(action).ConfigureAwait(false);

    /// <summary>
    /// Executes one of two asynchronous actions based on whether the result represents success or failure.
    /// </summary>
    /// <param name="result">The result to check.</param>
    /// <param name="success">The asynchronous action to execute if the result is successful.</param>
    /// <param name="failure">The asynchronous action to execute if the result is a failure, receiving the error as a parameter.</param>
    /// <returns>A task that completes with the original result, unmodified.</returns>
    /// <remarks>
    /// This method provides a way to handle both success and failure cases asynchronously in a single call.
    /// </remarks>
    public static async Task<Result> OnEitherAsync(this Result result, Func<Task> success, Func<Error, Task> failure)
    {
        if (result.IsSuccess) await success().ConfigureAwait(false);
        else await failure(result.Error).ConfigureAwait(false);
        return result;
    }

    /// <summary>
    /// Executes one of two synchronous actions based on whether the task-wrapped result represents success or failure.
    /// </summary>
    /// <param name="result">The task containing the result to check.</param>
    /// <param name="success">The action to execute if the result is successful.</param>
    /// <param name="failure">The action to execute if the result is a failure, receiving the error as a parameter.</param>
    /// <returns>A task that completes with the original result, unmodified.</returns>
    /// <remarks>
    /// This overload allows branching with synchronous actions on task-wrapped results without additional async overhead.
    /// </remarks>
    public static async Task<Result> OnEitherAsync(this Task<Result> result, Action success, Action<Error> failure) =>
        (await result.ConfigureAwait(false)).OnEither(success, failure);

    /// <summary>
    /// Executes one of two asynchronous actions based on whether the task-wrapped result represents success or failure.
    /// </summary>
    /// <param name="result">The task containing the result to check.</param>
    /// <param name="success">The asynchronous action to execute if the result is successful.</param>
    /// <param name="failure">The asynchronous action to execute if the result is a failure, receiving the error as a parameter.</param>
    /// <returns>A task that completes with the original result, unmodified.</returns>
    /// <remarks>
    /// This overload enables fully asynchronous branching chains when working with task-wrapped results.
    /// </remarks>
    public static async Task<Result> OnEitherAsync(
        this Task<Result> result,
        Func<Task> success,
        Func<Error, Task> failure) =>
        await (await result.ConfigureAwait(false)).OnEitherAsync(success, failure).ConfigureAwait(false);
}
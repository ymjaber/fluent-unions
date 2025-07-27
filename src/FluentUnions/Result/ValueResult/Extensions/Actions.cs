namespace FluentUnions;

/// <summary>
/// Provides action-based extension methods for <see cref="Result{TValue}"/> that allow executing side effects
/// based on the result state without modifying the result itself.
/// </summary>
public static partial class ValueResultExtensions
{
    /// <summary>
    /// Executes the specified action with the result value if the result represents a success state.
    /// </summary>
    /// <typeparam name="TValue">The type of value contained in the result.</typeparam>
    /// <param name="result">The result to check.</param>
    /// <param name="action">The action to execute if the result is successful, receiving the value as a parameter.</param>
    /// <returns>The original result, unmodified.</returns>
    /// <remarks>
    /// This method is useful for performing side effects (like logging, updating UI, or triggering events)
    /// based on the successful value, while maintaining the fluent chain.
    /// </remarks>
    public static Result<TValue> OnSuccess<TValue>(in this Result<TValue> result, Action<TValue> action)
    {
        if (result.IsSuccess) action(result.Value);
        return result;
    }
    
    /// <summary>
    /// Executes the specified action with the error if the result represents a failure state.
    /// </summary>
    /// <typeparam name="TValue">The type of value that would be contained in a successful result.</typeparam>
    /// <param name="result">The result to check.</param>
    /// <param name="action">The action to execute if the result is a failure, receiving the error as a parameter.</param>
    /// <returns>The original result, unmodified.</returns>
    /// <remarks>
    /// This method is useful for handling errors (like logging, error reporting, or cleanup)
    /// when a result is a failure, while maintaining the fluent chain.
    /// </remarks>
    public static Result<TValue> OnFailure<TValue>(in this Result<TValue> result, Action<Error> action)
    {
        if (result.IsFailure) action(result.Error);
        return result;
    }
    
    /// <summary>
    /// Executes one of two actions based on whether the result represents success or failure.
    /// </summary>
    /// <typeparam name="TValue">The type of value contained in the result.</typeparam>
    /// <param name="result">The result to check.</param>
    /// <param name="success">The action to execute if the result is successful, receiving the value as a parameter.</param>
    /// <param name="failure">The action to execute if the result is a failure, receiving the error as a parameter.</param>
    /// <returns>The original result, unmodified.</returns>
    /// <remarks>
    /// This method provides a way to handle both success and failure cases in a single call,
    /// useful for scenarios where different side effects are needed for each case.
    /// </remarks>
    public static Result<TValue> OnEither<TValue>(in this Result<TValue> result, Action<TValue> success, Action<Error> failure)
    {
        if (result.IsSuccess) success(result.Value);
        else failure(result.Error);
        return result;
    }
}
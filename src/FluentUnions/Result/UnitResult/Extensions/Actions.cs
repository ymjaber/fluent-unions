namespace FluentUnions;

/// <summary>
/// Provides action-based extension methods for <see cref="Result"/> that allow executing side effects
/// based on the result state without modifying the result itself.
/// </summary>
public static partial class UnitResultExtensions
{
    /// <summary>
    /// Executes the specified action if the result represents a success state.
    /// </summary>
    /// <param name="result">The result to check.</param>
    /// <param name="action">The action to execute if the result is successful.</param>
    /// <returns>The original result, unmodified.</returns>
    /// <remarks>
    /// This method is useful for performing side effects (like logging or triggering events)
    /// when a result is successful, while maintaining the fluent chain.
    /// </remarks>
    public static Result OnSuccess(in this Result result, Action action)
    {
        if (result.IsSuccess) action();
        return result;
    }
    
    /// <summary>
    /// Executes the specified action with the error if the result represents a failure state.
    /// </summary>
    /// <param name="result">The result to check.</param>
    /// <param name="action">The action to execute if the result is a failure, receiving the error as a parameter.</param>
    /// <returns>The original result, unmodified.</returns>
    /// <remarks>
    /// This method is useful for handling errors (like logging or error reporting)
    /// when a result is a failure, while maintaining the fluent chain.
    /// </remarks>
    public static Result OnFailure(in this Result result, Action<Error> action)
    {
        if (result.IsFailure) action(result.Error);
        return result;
    }
    
    /// <summary>
    /// Executes one of two actions based on whether the result represents success or failure.
    /// </summary>
    /// <param name="result">The result to check.</param>
    /// <param name="success">The action to execute if the result is successful.</param>
    /// <param name="failure">The action to execute if the result is a failure, receiving the error as a parameter.</param>
    /// <returns>The original result, unmodified.</returns>
    /// <remarks>
    /// This method provides a way to handle both success and failure cases in a single call,
    /// useful for scenarios where different side effects are needed for each case.
    /// </remarks>
    public static Result OnEither(in this Result result, Action success, Action<Error> failure)
    {
        if (result.IsSuccess) success();
        else failure(result.Error);
        return result;
    }
}
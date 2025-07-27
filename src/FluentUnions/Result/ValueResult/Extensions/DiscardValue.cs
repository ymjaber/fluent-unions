namespace FluentUnions;

/// <summary>
/// Provides extension methods for <see cref="Result{TValue}"/> to convert a value result
/// into a unit result by discarding the value.
/// </summary>
public static partial class ValueResultExtensions
{
    /// <summary>
    /// Converts a value result to a unit result by discarding the value if successful.
    /// </summary>
    /// <typeparam name="TValue">The type of value to discard from the result.</typeparam>
    /// <param name="result">The value result to convert.</param>
    /// <returns>
    /// A unit <see cref="Result"/> indicating success if the original result was successful;
    /// otherwise, a failure result with the original error.
    /// </returns>
    /// <remarks>
    /// This method is useful when you only care about whether an operation succeeded or failed,
    /// not about the value it produced. Common use cases include operations performed for their
    /// side effects or when transitioning from a value-producing operation to validation-only operations.
    /// </remarks>
    public static Result DiscardValue<TValue>(in this Result<TValue> result) =>
        result.IsSuccess ? Result.Success() : Result.Failure(result.Error);
}
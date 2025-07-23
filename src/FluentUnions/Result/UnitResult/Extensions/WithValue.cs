namespace FluentUnions;

/// <summary>
/// Provides extension methods for <see cref="Result"/> to convert a unit result
/// into a value result by attaching a value.
/// </summary>
public static partial class UnitResultExtensions
{
    /// <summary>
    /// Converts a unit result to a value result by attaching the specified value if successful.
    /// </summary>
    /// <typeparam name="TValue">The type of value to attach to the result.</typeparam>
    /// <param name="result">The unit result to convert.</param>
    /// <param name="value">The value to attach if the result is successful.</param>
    /// <returns>
    /// A <see cref="Result{TValue}"/> containing the specified value if the original result was successful;
    /// otherwise, a failure result with the original error.
    /// </returns>
    /// <remarks>
    /// This method is useful when you have completed an operation that doesn't produce a value
    /// but you need to continue the chain with a value result.
    /// </remarks>
    public static Result<TValue> WithValue<TValue>(in this Result result, TValue value) =>
        result.IsSuccess ? Result.Success(value) : Result.Failure<TValue>(result.Error);

    /// <summary>
    /// Converts a unit result to a value result by invoking a value factory if successful.
    /// </summary>
    /// <typeparam name="TValue">The type of value to attach to the result.</typeparam>
    /// <param name="result">The unit result to convert.</param>
    /// <param name="valueFactory">A function that produces the value to attach if the result is successful.</param>
    /// <returns>
    /// A <see cref="Result{TValue}"/> containing the value produced by the factory if the original result was successful;
    /// otherwise, a failure result with the original error.
    /// </returns>
    /// <remarks>
    /// This overload is useful when the value to attach is expensive to create or should only
    /// be created if the result is actually successful.
    /// </remarks>
    public static Result<TValue> WithValue<TValue>(in this Result result, Func<TValue> valueFactory) =>
        result.IsSuccess ? Result.Success(valueFactory()) : Result.Failure<TValue>(result.Error);
}
using FluentUnions.PreDefinedEnsure;

namespace FluentUnions;

/// <summary>
/// Provides conversion methods from <see cref="Option{T}"/> to <see cref="Result{T}"/> types.
/// </summary>
public static partial class OptionExtensions
{
    /// <summary>
    /// Ensures the option contains a value and converts it to a successful <see cref="Result{TValue}"/>,
    /// or returns a failure result if the option is None.
    /// </summary>
    /// <typeparam name="TValue">The type of the value contained in the option.</typeparam>
    /// <param name="option">The option to convert to a result.</param>
    /// <param name="error">The error to use if the option is None. If null, a default error indicating the option was None is used.</param>
    /// <returns>
    /// A successful <see cref="Result{TValue}"/> containing the option's value if the option is Some;
    /// otherwise, a failure result with the specified error or a default error.
    /// </returns>
    /// <remarks>
    /// This method is useful when you need to convert an optional value to a result type,
    /// treating the absence of a value as an error condition. It allows you to specify
    /// a custom error message for better context in error handling.
    /// </remarks>
    public static Result<TValue> EnsureSome<TValue>(
        in this Option<TValue> option,
        Error? error = null)
        where TValue : notnull
    {
        return option.IsSome
            ? Result.Success(option.Value)
            : Result.Failure<TValue>(error ?? OptionErrors.None);
    }

    /// <summary>
    /// Ensures the option is None and returns a successful <see cref="Result"/>,
    /// or returns a failure result if the option contains a value.
    /// </summary>
    /// <typeparam name="TValue">The type of the value that would be contained in the option if it were Some.</typeparam>
    /// <param name="option">The option to verify is None.</param>
    /// <param name="error">The error to use if the option contains a value. If null, a default error indicating the option was Some is used.</param>
    /// <returns>
    /// A successful <see cref="Result"/> if the option is None;
    /// otherwise, a failure result with the specified error or a default error.
    /// </returns>
    /// <remarks>
    /// This method is useful in scenarios where the presence of a value is considered an error,
    /// such as checking that a resource doesn't already exist before creating it.
    /// </remarks>
    public static Result EnsureNone<TValue>(
        in this Option<TValue> option,
        Error? error = null)
    where TValue : notnull
    {
        return option.IsNone
            ? Result.Success()
            : Result.Failure(error ?? OptionErrors.Some);
    }
}
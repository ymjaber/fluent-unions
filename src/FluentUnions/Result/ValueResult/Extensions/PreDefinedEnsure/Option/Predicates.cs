using FluentUnions.PreDefinedEnsure;

namespace FluentUnions;

/// <summary>Provides pre-defined validation predicates for Option values in <see cref="Result{T}"/> types.</summary>
public static partial class EnsureBuilderExtensions
{

    /// <summary>
    /// Ensures that the option has a value (is Some).
    /// </summary>
    /// <typeparam name="TValue">The type of value contained in the option.</typeparam>
    /// <param name="ensure">The ensure builder for the Guid value.</param>
    /// <param name="error">Optional custom error. If null, a default error message will be used.</param>
    /// <returns>
    /// A result containing the unwrapped value if the option has a value;
    /// otherwise, a failure result with the specified error.
    /// </returns>
    public static Result<TValue> Some<TValue>(
            in this EnsureBuilder<Option<TValue>> ensure,
            Error error = null!)
        where TValue : notnull =>
        ensure.Satisfies(o => o.IsSome, error ?? OptionErrors.None)
            .Map(o => o.Value);

    /// <summary>
    /// Ensures that the option has no value (is None).
    /// </summary>
    /// <typeparam name="TValue">The type of value that would be contained in the option.</typeparam>
    /// <param name="ensure">The ensure builder for the Guid value.</param>
    /// <param name="error">Optional custom error. If null, a default error message will be used.</param>
    /// <returns>
    /// A unit result indicating success if the option is None;
    /// otherwise, a failure result with the specified error.
    /// </returns>
    public static Result None<TValue>(
            in this EnsureBuilder<Option<TValue>> ensure,
            Error error = null!)
        where TValue : notnull =>
        ensure.Satisfies(o => o.IsNone, error ?? OptionErrors.Some)
            .DiscardValue();
}

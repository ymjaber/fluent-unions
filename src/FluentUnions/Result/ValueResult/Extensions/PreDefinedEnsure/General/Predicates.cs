using FluentUnions.PreDefinedEnsure;

namespace FluentUnions;

/// <summary>Provides pre-defined general-purpose validation predicates for <see cref="Result{T}"/> types.</summary>
public static partial class EnsureBuilderExtensions
{
    /// <summary>
    /// Ensures that the value is equal to the specified value.
    /// </summary>
    /// <typeparam name="TValue">The type of the value to compare.</typeparam>
    /// <param name="ensure">The ensure builder for the value.</param>
    /// <param name="value">The value to compare with.</param>
    /// <param name="error">Optional custom error. If null, a default error message will be used.</param>
    /// <returns>A unit result indicating success if the values are equal; otherwise, a failure result.</returns>
    public static Result IsEqualTo<TValue>(
        in this EnsureBuilder<TValue> ensure,
        TValue value,
        Error? error = null)
        where TValue : IEquatable<TValue> =>
        ensure.Satisfies(v => v.Equals(value), error ?? GeneralErrors.NotEqual).Build().DiscardValue();

    /// <summary>
    /// Ensures that the value is not equal to the specified value.
    /// </summary>
    /// <typeparam name="TValue">The type of the value to compare.</typeparam>
    /// <param name="ensure">The ensure builder for the value.</param>
    /// <param name="value">The value to compare with.</param>
    /// <param name="error">Optional custom error. If null, a default error message will be used.</param>
    /// <returns>An ensure builder that validates the values are not equal.</returns>
    public static EnsureBuilder<TValue> IsNotEqualTo<TValue>(
        in this EnsureBuilder<TValue> ensure,
        TValue value,
        Error? error = null)
        where TValue : IEquatable<TValue> =>
        ensure.Satisfies(v => !v.Equals(value), error ?? GeneralErrors.Equal);
}
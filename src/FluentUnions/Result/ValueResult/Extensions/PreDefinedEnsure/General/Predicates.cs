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
        ensure.Satisfies(v => v.Equals(value), error ?? GeneralErrors.NotEqual).DiscardValue();

    /// <summary>
    /// Ensures that the value is not equal to the specified value.
    /// </summary>
    /// <typeparam name="TValue">The type of the value to compare.</typeparam>
    /// <param name="ensure">The ensure builder for the value.</param>
    /// <param name="value">The value to compare with.</param>
    /// <param name="error">Optional custom error. If null, a default error message will be used.</param>
    /// <returns>A result that validates the values are not equal.</returns>
    public static Result<TValue> IsNotEqualTo<TValue>(
        in this EnsureBuilder<TValue> ensure,
        TValue value,
        Error? error = null)
        where TValue : IEquatable<TValue> =>
        ensure.Satisfies(v => !v.Equals(value), error ?? GeneralErrors.Equal);


    /// <summary>
    /// Ensures that the reference type value is not null.
    /// </summary>
    /// <typeparam name="TValue">The reference type of the value to check.</typeparam>
    /// <param name="ensure">The ensure builder for the value.</param>
    /// <param name="error">Optional custom error. If null, a default error message will be used.</param>
    /// <returns>A result that validates the value is not null.</returns>
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
    public static Result<TValue> NotNull<TValue>(
            in this EnsureBuilder<TValue?> ensure,
            Error error = null!)
        where TValue : class =>
        ensure.Satisfies(v => v is not null, error ?? GeneralErrors.Null);
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.

    /// <summary>
    /// Ensures that the nullable value type is not null.
    /// </summary>
    /// <typeparam name="TValue">The value type to check.</typeparam>
    /// <param name="ensure">The ensure builder for the value.</param>
    /// <param name="error">Optional custom error. If null, a default error message will be used.</param>
    /// <returns>A success result containing the unwrapped value if it's not null; otherwise, a failure result with the specified error.</returns>
    public static Result<TValue> NotNull<TValue>(
            in this EnsureBuilder<TValue?> ensure,
            Error error = null!)
        where TValue : struct =>
        ensure.Satisfies(v => v is not null, error ?? GeneralErrors.Null)
        .Map(v => v!.Value);

    /// <summary>
    /// Ensures that the value is null.
    /// </summary>
    /// <typeparam name="TValue">The type of the value to check.</typeparam>
    /// <param name="ensure">The ensure builder for the value.</param>
    /// <param name="error">Optional custom error. If null, a default error message will be used.</param>
    /// <returns>A success result if the value is null; otherwise, a failure result with the specified error.</returns>
    /// <remarks>
    /// This method is useful for validation scenarios where a value must be null, such as ensuring a field is not set.
    /// </remarks>
    public static Result Null<TValue>(
        in this EnsureBuilder<TValue?> ensure,
        Error? error = null) =>
        ensure.Satisfies(v => v is null, error ?? GeneralErrors.Null)
            .DiscardValue();
}

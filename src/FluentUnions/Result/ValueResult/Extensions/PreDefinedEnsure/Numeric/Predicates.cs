using System.Numerics;
using FluentUnions.PreDefinedEnsure;

namespace FluentUnions;

/// <summary>Provides pre-defined validation predicates for numeric values in <see cref="Result{T}"/> types.</summary>
public static partial class EnsureBuilderExtensions
{
    /// <summary>
    /// Ensures that the numeric value is greater than the specified minimum.
    /// </summary>
    /// <typeparam name="TValue">The type of the numeric value.</typeparam>
    /// <param name="ensure">The ensure builder for the numeric value.</param>
    /// <param name="min">The minimum value (exclusive).</param>
    /// <param name="error">Optional custom error. If null, a default error message will be used.</param>
    /// <returns>A result that validates the value is greater than the minimum.</returns>
    public static Result<TValue> GreaterThan<TValue>(
        in this EnsureBuilder<TValue> ensure,
        TValue min,
        Error? error = null) where TValue : IComparable<TValue> =>
        ensure.Satisfies(n => n.CompareTo(min) > 0, error ?? NumericErrors.TooSmall(min, false));

    /// <summary>
    /// Ensures that the numeric value is greater than or equal to the specified minimum.
    /// </summary>
    /// <typeparam name="TValue">The type of the numeric value.</typeparam>
    /// <param name="ensure">The ensure builder for the numeric value.</param>
    /// <param name="min">The minimum value (inclusive).</param>
    /// <param name="error">Optional custom error. If null, a default error message will be used.</param>
    /// <returns>A result that validates the value is greater than or equal to the minimum.</returns>
    public static Result<TValue> GreaterThanOrEqualTo<TValue>(
        in this EnsureBuilder<TValue> ensure,
        TValue min,
        Error? error = null) where TValue : IComparable<TValue> =>
        ensure.Satisfies(n => n.CompareTo(min) >= 0, error ?? NumericErrors.TooSmall(min, true));

    /// <summary>
    /// Ensures that the numeric value is less than the specified maximum.
    /// </summary>
    /// <typeparam name="TValue">The type of the numeric value.</typeparam>
    /// <param name="ensure">The ensure builder for the numeric value.</param>
    /// <param name="max">The maximum value (exclusive).</param>
    /// <param name="error">Optional custom error. If null, a default error message will be used.</param>
    /// <returns>A result that validates the value is less than the maximum.</returns>
    public static Result<TValue> LessThan<TValue>(
        in this EnsureBuilder<TValue> ensure,
        TValue max,
        Error? error = null) where TValue : IComparable<TValue> =>
        ensure.Satisfies(n => n.CompareTo(max) < 0, error ?? NumericErrors.TooLarge(max, false));

    /// <summary>
    /// Ensures that the numeric value is less than or equal to the specified maximum.
    /// </summary>
    /// <typeparam name="TValue">The type of the numeric value.</typeparam>
    /// <param name="ensure">The ensure builder for the numeric value.</param>
    /// <param name="max">The maximum value (inclusive).</param>
    /// <param name="error">Optional custom error. If null, a default error message will be used.</param>
    /// <returns>A result that validates the value is less than or equal to the maximum.</returns>
    public static Result<TValue> LessThanOrEqualTo<TValue>(
        in this EnsureBuilder<TValue> ensure,
        TValue max,
        Error? error = null) where TValue : IComparable<TValue> =>
        ensure.Satisfies(n => n.CompareTo(max) <= 0, error ?? NumericErrors.TooLarge(max, true));

    /// <summary>
    /// Ensures that the numeric value is within the specified range.
    /// </summary>
    /// <typeparam name="TValue">The type of the numeric value.</typeparam>
    /// <param name="ensure">The ensure builder for the numeric value.</param>
    /// <param name="min">The minimum value of the range.</param>
    /// <param name="minInclusive">Whether the minimum value is inclusive.</param>
    /// <param name="max">The maximum value of the range.</param>
    /// <param name="maxInclusive">Whether the maximum value is inclusive.</param>
    /// <param name="error">Optional custom error. If null, a default error message will be used.</param>
    /// <returns>A result that validates the value is within the specified range.</returns>
    public static Result<TValue> InRange<TValue>(
        in this EnsureBuilder<TValue> ensure,
        TValue min,
        bool minInclusive,
        TValue max,
        bool maxInclusive,
        Error? error = null) where TValue : IComparable<TValue> =>
        ensure.Satisfies(
            n =>
            {
                var minComparison = n.CompareTo(min);
                var maxComparison = n.CompareTo(max);

                return (minInclusive ? minComparison >= 0 : minComparison > 0) &&
                       (maxInclusive ? maxComparison <= 0 : maxComparison < 0);
            },
            error ?? NumericErrors.OutOfRange(min, minInclusive, max, maxInclusive));

    /// <summary>
    /// Ensures that the numeric value is positive (greater than zero).
    /// </summary>
    /// <typeparam name="TValue">The type of the numeric value.</typeparam>
    /// <param name="ensure">The ensure builder for the numeric value.</param>
    /// <param name="error">Optional custom error. If null, a default error message will be used.</param>
    /// <returns>A result that validates the value is positive.</returns>
    public static Result<TValue> Positive<TValue>(
        in this EnsureBuilder<TValue> ensure,
        Error? error = null) where TValue : INumberBase<TValue> =>
        ensure.Satisfies(
            TValue.IsPositive,
            error ?? NumericErrors.NotPositive);

    /// <summary>
    /// Ensures that the numeric value is negative (less than zero).
    /// </summary>
    /// <typeparam name="TValue">The type of the numeric value.</typeparam>
    /// <param name="ensure">The ensure builder for the numeric value.</param>
    /// <param name="error">Optional custom error. If null, a default error message will be used.</param>
    /// <returns>A result that validates the value is negative.</returns>
    public static Result<TValue> Negative<TValue>(
        in this EnsureBuilder<TValue> ensure,
        Error? error = null) where TValue : INumberBase<TValue> =>
        ensure.Satisfies(
            TValue.IsNegative,
            error ?? NumericErrors.NotNegative);

    /// <summary>
    /// Ensures that the numeric value is zero.
    /// </summary>
    /// <typeparam name="TValue">The type of the numeric value.</typeparam>
    /// <param name="ensure">The ensure builder for the numeric value.</param>
    /// <param name="error">Optional custom error. If null, a default error message will be used.</param>
    /// <returns>A unit result indicating success if the value is zero; otherwise, a failure result.</returns>
    public static Result Zero<TValue>(
        in this EnsureBuilder<TValue> ensure,
        Error? error = null) where TValue : INumberBase<TValue> =>
        ensure.Satisfies(
            TValue.IsZero,
            error ?? NumericErrors.NotZero).DiscardValue();

    /// <summary>
    /// Ensures that the numeric value is not zero.
    /// </summary>
    /// <typeparam name="TValue">The type of the numeric value.</typeparam>
    /// <param name="ensure">The ensure builder for the numeric value.</param>
    /// <param name="error">Optional custom error. If null, a default error message will be used.</param>
    /// <returns>A result that validates the value is not zero.</returns>
    public static Result<TValue> NonZero<TValue>(
        in this EnsureBuilder<TValue> ensure,
        Error? error = null) where TValue : INumberBase<TValue> =>
        ensure.Satisfies(
            n => !TValue.IsZero(n),
            error ?? NumericErrors.Zero);

    /// <summary>
    /// Ensures that the numeric value is non-positive (zero or negative).
    /// </summary>
    /// <typeparam name="TValue">The type of the numeric value.</typeparam>
    /// <param name="ensure">The ensure builder for the numeric value.</param>
    /// <param name="error">Optional custom error. If null, a default error message will be used.</param>
    /// <returns>A result that validates the value is non-positive.</returns>
    public static Result<TValue> NonPositive<TValue>(
        in this EnsureBuilder<TValue> ensure,
        Error? error = null) where TValue : INumberBase<TValue> =>
        ensure.Satisfies(
            n => !TValue.IsPositive(n),
            error ?? NumericErrors.Positive);

    /// <summary>
    /// Ensures that the numeric value is non-negative (zero or positive).
    /// </summary>
    /// <typeparam name="TValue">The type of the numeric value.</typeparam>
    /// <param name="ensure">The ensure builder for the numeric value.</param>
    /// <param name="error">Optional custom error. If null, a default error message will be used.</param>
    /// <returns>A result that validates the value is non-negative.</returns>
    public static Result<TValue> NonNegative<TValue>(
        in this EnsureBuilder<TValue> ensure,
        Error? error = null) where TValue : INumberBase<TValue> =>
        ensure.Satisfies(
            n => !TValue.IsNegative(n),
            error ?? NumericErrors.Negative);
}

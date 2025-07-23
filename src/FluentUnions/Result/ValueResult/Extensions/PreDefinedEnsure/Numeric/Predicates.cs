using System.Numerics;
using FluentUnions.PreDefinedEnsure;

namespace FluentUnions;

/// <summary>Provides pre-defined validation predicates for numeric values in <see cref="Result{T}"/> types.</summary>
public static partial class EnsureBuilderExtensions
{
    public static EnsureBuilder<TValue> GreaterThan<TValue>(
        in this EnsureBuilder<TValue> ensure,
        TValue min,
        Error? error = null) where TValue : IComparable<TValue> =>
        ensure.Satisfies(n => n.CompareTo(min) > 0, error ?? NumericErrors.TooSmall(min, false));

    public static EnsureBuilder<TValue> GreaterThanOrEqualTo<TValue>(
        in this EnsureBuilder<TValue> ensure,
        TValue min,
        Error? error = null) where TValue : IComparable<TValue> =>
        ensure.Satisfies(n => n.CompareTo(min) >= 0, error ?? NumericErrors.TooSmall(min, true));

    public static EnsureBuilder<TValue> LessThan<TValue>(
        in this EnsureBuilder<TValue> ensure,
        TValue max,
        Error? error = null) where TValue : IComparable<TValue> =>
        ensure.Satisfies(n => n.CompareTo(max) < 0, error ?? NumericErrors.TooLarge(max, false));

    public static EnsureBuilder<TValue> LessThanOrEqualTo<TValue>(
        in this EnsureBuilder<TValue> ensure,
        TValue max,
        Error? error = null) where TValue : IComparable<TValue> =>
        ensure.Satisfies(n => n.CompareTo(max) <= 0, error ?? NumericErrors.TooLarge(max, true));

    public static EnsureBuilder<TValue> InRange<TValue>(
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

    public static EnsureBuilder<TValue> Positive<TValue>(
        in this EnsureBuilder<TValue> ensure,
        Error? error = null) where TValue : INumberBase<TValue> =>
        ensure.Satisfies(
            TValue.IsPositive,
            error ?? NumericErrors.NotPositive);

    public static EnsureBuilder<TValue> Negative<TValue>(
        in this EnsureBuilder<TValue> ensure,
        Error? error = null) where TValue : INumberBase<TValue> =>
        ensure.Satisfies(
            TValue.IsNegative,
            error ?? NumericErrors.NotNegative);

    public static Result Zero<TValue>(
        in this EnsureBuilder<TValue> ensure,
        Error? error = null) where TValue : INumberBase<TValue> =>
        ensure.Satisfies(
            TValue.IsZero,
            error ?? NumericErrors.NotZero).Build().DiscardValue();

    public static EnsureBuilder<TValue> NonZero<TValue>(
        in this EnsureBuilder<TValue> ensure,
        Error? error = null) where TValue : INumberBase<TValue> =>
        ensure.Satisfies(
            n => !TValue.IsZero(n),
            error ?? NumericErrors.Zero);

    public static EnsureBuilder<TValue> NonPositive<TValue>(
        in this EnsureBuilder<TValue> ensure,
        Error? error = null) where TValue : INumberBase<TValue> =>
        ensure.Satisfies(
            n => !TValue.IsPositive(n),
            error ?? NumericErrors.Positive);

    public static EnsureBuilder<TValue> NonNegative<TValue>(
        in this EnsureBuilder<TValue> ensure,
        Error? error = null) where TValue : INumberBase<TValue> =>
        ensure.Satisfies(
            n => !TValue.IsNegative(n),
            error ?? NumericErrors.Negative);
}
using System.Numerics;

namespace FluentUnions;

/// <summary>Provides pre-defined filter methods for numeric values in <see cref="Option{T}"/> types.</summary>
public static partial class FilterBuilderExtensions
{
    /// <summary>
    /// Filters options where the numeric value is greater than the specified minimum.
    /// </summary>
    /// <typeparam name="TValue">The type of the numeric value.</typeparam>
    /// <param name="filter">The filter builder for the numeric value.</param>
    /// <param name="min">The minimum value (exclusive).</param>
    /// <returns>An option that validates the value is greater than the minimum.</returns>
    public static Option<TValue> GreaterThan<TValue>(in this FilterBuilder<TValue> filter, TValue min)
        where TValue : IComparable<TValue> =>
        filter.Satisfies(n => n.CompareTo(min) > 0);

    /// <summary>
    /// Filters options where the numeric value is greater than or equal to the specified minimum.
    /// </summary>
    /// <typeparam name="TValue">The type of the numeric value.</typeparam>
    /// <param name="filter">The filter builder for the numeric value.</param>
    /// <param name="min">The minimum value (inclusive).</param>
    /// <returns>An option that validates the value is greater than or equal to the minimum.</returns>
    public static Option<TValue> GreaterThanOrEqualTo<TValue>(in this FilterBuilder<TValue> filter, TValue min)
        where TValue : IComparable<TValue> =>
        filter.Satisfies(n => n.CompareTo(min) >= 0);

    /// <summary>
    /// Filters options where the numeric value is less than the specified maximum.
    /// </summary>
    /// <typeparam name="TValue">The type of the numeric value.</typeparam>
    /// <param name="filter">The filter builder for the numeric value.</param>
    /// <param name="max">The maximum value (exclusive).</param>
    /// <returns>An option that validates the value is less than the maximum.</returns>
    public static Option<TValue> LessThan<TValue>(in this FilterBuilder<TValue> filter, TValue max)
        where TValue : IComparable<TValue> =>
        filter.Satisfies(n => n.CompareTo(max) < 0);

    /// <summary>
    /// Filters options where the numeric value is less than or equal to the specified maximum.
    /// </summary>
    /// <typeparam name="TValue">The type of the numeric value.</typeparam>
    /// <param name="filter">The filter builder for the numeric value.</param>
    /// <param name="max">The maximum value (inclusive).</param>
    /// <returns>An option that validates the value is less than or equal to the maximum.</returns>
    public static Option<TValue> LessThanOrEqualTo<TValue>(in this FilterBuilder<TValue> filter, TValue max)
        where TValue : IComparable<TValue> =>
        filter.Satisfies(n => n.CompareTo(max) <= 0);

    /// <summary>
    /// Filters options where the numeric value is within the specified range.
    /// </summary>
    /// <typeparam name="TValue">The type of the numeric value.</typeparam>
    /// <param name="filter">The filter builder for the numeric value.</param>
    /// <param name="min">The minimum value of the range.</param>
    /// <param name="minInclusive">Whether the minimum value is inclusive.</param>
    /// <param name="max">The maximum value of the range.</param>
    /// <param name="maxInclusive">Whether the maximum value is inclusive.</param>
    /// <returns>An option that validates the value is within the specified range.</returns>
    public static Option<TValue> InRange<TValue>(
        in this FilterBuilder<TValue> filter,
        TValue min,
        bool minInclusive,
        TValue max,
        bool maxInclusive)
        where TValue : IComparable<TValue> =>
        filter.Satisfies(n =>
        {
            var minComparison = n.CompareTo(min);
            var maxComparison = n.CompareTo(max);

            return (minInclusive ? minComparison >= 0 : minComparison > 0) &&
                   (maxInclusive ? maxComparison <= 0 : maxComparison < 0);
        });

    /// <summary>
    /// Filters options where the numeric value is positive (greater than zero).
    /// </summary>
    /// <typeparam name="TValue">The type of the numeric value.</typeparam>
    /// <param name="filter">The filter builder for the numeric value.</param>
    /// <returns>An option that validates the value is positive.</returns>
    public static Option<TValue> Positive<TValue>(in this FilterBuilder<TValue> filter)
        where TValue : INumberBase<TValue> =>
        filter.Satisfies(TValue.IsPositive);

    /// <summary>
    /// Filters options where the numeric value is negative (less than zero).
    /// </summary>
    /// <typeparam name="TValue">The type of the numeric value.</typeparam>
    /// <param name="filter">The filter builder for the numeric value.</param>
    /// <returns>An option that validates the value is negative.</returns>
    public static Option<TValue> Negative<TValue>(in this FilterBuilder<TValue> filter)
        where TValue : INumberBase<TValue> =>
        filter.Satisfies(TValue.IsNegative);

    /// <summary>
    /// Filters options where the numeric value is zero.
    /// </summary>
    /// <typeparam name="TValue">The type of the numeric value.</typeparam>
    /// <param name="filter">The filter builder for the numeric value.</param>
    /// <returns>True if the value passes the filter (is zero); otherwise, false.</returns>
    public static bool Zero<TValue>(in this FilterBuilder<TValue> filter)
        where TValue : INumberBase<TValue> =>
        filter.Satisfies(TValue.IsZero).IsSome;

    /// <summary>
    /// Filters options where the numeric value is not zero.
    /// </summary>
    /// <typeparam name="TValue">The type of the numeric value.</typeparam>
    /// <param name="filter">The filter builder for the numeric value.</param>
    /// <returns>An option that validates the value is not zero.</returns>
    public static Option<TValue> NonZero<TValue>(in this FilterBuilder<TValue> filter)
        where TValue : INumberBase<TValue> =>
        filter.Satisfies(n => !TValue.IsZero(n));

    /// <summary>
    /// Filters options where the numeric value is non-positive (zero or negative).
    /// </summary>
    /// <typeparam name="TValue">The type of the numeric value.</typeparam>
    /// <param name="filter">The filter builder for the numeric value.</param>
    /// <returns>An option that validates the value is non-positive.</returns>
    public static Option<TValue> NonPositive<TValue>(in this FilterBuilder<TValue> filter)
        where TValue : INumberBase<TValue> =>
        filter.Satisfies(n => !TValue.IsPositive(n));

    /// <summary>
    /// Filters options where the numeric value is non-negative (zero or positive).
    /// </summary>
    /// <typeparam name="TValue">The type of the numeric value.</typeparam>
    /// <param name="filter">The filter builder for the numeric value.</param>
    /// <returns>An option that validates the value is non-negative.</returns>
    public static Option<TValue> NonNegative<TValue>(in this FilterBuilder<TValue> filter)
        where TValue : INumberBase<TValue> =>
        filter.Satisfies(n => !TValue.IsNegative(n));
}

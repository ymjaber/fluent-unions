using System.Numerics;

namespace FluentUnions;

/// <summary>Provides pre-defined filter methods for numeric values in <see cref="Option{T}"/> types.</summary>
public static partial class FilterBuilderExtensions
{
    /// <summary>
    /// Filters values that are greater than the specified minimum value.
    /// </summary>
    /// <typeparam name="TValue">The type of the value, must implement IComparable.</typeparam>
    /// <param name="filter">The filter builder to apply the condition to.</param>
    /// <param name="min">The minimum value (exclusive).</param>
    /// <returns>A filter builder that continues the fluent chain.</returns>
    public static FilterBuilder<TValue> GreaterThan<TValue>(in this FilterBuilder<TValue> filter, TValue min)
        where TValue : IComparable<TValue> =>
        filter.Satisfies(n => n.CompareTo(min) > 0);

    /// <summary>
    /// Filters values that are greater than or equal to the specified minimum value.
    /// </summary>
    /// <typeparam name="TValue">The type of the value, must implement IComparable.</typeparam>
    /// <param name="filter">The filter builder to apply the condition to.</param>
    /// <param name="min">The minimum value (inclusive).</param>
    /// <returns>A filter builder that continues the fluent chain.</returns>
    public static FilterBuilder<TValue> GreaterThanOrEqualTo<TValue>(in this FilterBuilder<TValue> filter, TValue min)
        where TValue : IComparable<TValue> =>
        filter.Satisfies(n => n.CompareTo(min) >= 0);

    /// <summary>
    /// Filters values that are less than the specified maximum value.
    /// </summary>
    /// <typeparam name="TValue">The type of the value, must implement IComparable.</typeparam>
    /// <param name="filter">The filter builder to apply the condition to.</param>
    /// <param name="max">The maximum value (exclusive).</param>
    /// <returns>A filter builder that continues the fluent chain.</returns>
    public static FilterBuilder<TValue> LessThan<TValue>(in this FilterBuilder<TValue> filter, TValue max)
        where TValue : IComparable<TValue> =>
        filter.Satisfies(n => n.CompareTo(max) < 0);

    /// <summary>
    /// Filters values that are less than or equal to the specified maximum value.
    /// </summary>
    /// <typeparam name="TValue">The type of the value, must implement IComparable.</typeparam>
    /// <param name="filter">The filter builder to apply the condition to.</param>
    /// <param name="max">The maximum value (inclusive).</param>
    /// <returns>A filter builder that continues the fluent chain.</returns>
    public static FilterBuilder<TValue> LessThanOrEqualTo<TValue>(in this FilterBuilder<TValue> filter, TValue max)
        where TValue : IComparable<TValue> =>
        filter.Satisfies(n => n.CompareTo(max) <= 0);

    /// <summary>
    /// Filters values that fall within the specified range.
    /// </summary>
    /// <typeparam name="TValue">The type of the value, must implement IComparable.</typeparam>
    /// <param name="filter">The filter builder to apply the condition to.</param>
    /// <param name="min">The minimum value of the range.</param>
    /// <param name="minInclusive">Whether the minimum value is inclusive.</param>
    /// <param name="max">The maximum value of the range.</param>
    /// <param name="maxInclusive">Whether the maximum value is inclusive.</param>
    /// <returns>A filter builder that continues the fluent chain.</returns>
    public static FilterBuilder<TValue> InRange<TValue>(
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
    /// Filters numeric values that are positive (greater than zero).
    /// </summary>
    /// <typeparam name="TValue">The numeric type, must implement INumberBase.</typeparam>
    /// <param name="filter">The filter builder to apply the condition to.</param>
    /// <returns>A filter builder that continues the fluent chain.</returns>
    public static FilterBuilder<TValue> Positive<TValue>(in this FilterBuilder<TValue> filter)
        where TValue : INumberBase<TValue> =>
        filter.Satisfies(TValue.IsPositive);

    /// <summary>
    /// Filters numeric values that are negative (less than zero).
    /// </summary>
    /// <typeparam name="TValue">The numeric type, must implement INumberBase.</typeparam>
    /// <param name="filter">The filter builder to apply the condition to.</param>
    /// <returns>A filter builder that continues the fluent chain.</returns>
    public static FilterBuilder<TValue> Negative<TValue>(in this FilterBuilder<TValue> filter)
        where TValue : INumberBase<TValue> =>
        filter.Satisfies(TValue.IsNegative);

    /// <summary>
    /// Checks if the numeric value is zero.
    /// </summary>
    /// <typeparam name="TValue">The numeric type, must implement INumberBase.</typeparam>
    /// <param name="filter">The filter builder to apply the condition to.</param>
    /// <returns>True if the value is zero; otherwise, false.</returns>
    public static bool Zero<TValue>(in this FilterBuilder<TValue> filter)
        where TValue : INumberBase<TValue> =>
        filter.Satisfies(TValue.IsZero).Build().IsSome;

    /// <summary>
    /// Filters numeric values that are not zero.
    /// </summary>
    /// <typeparam name="TValue">The numeric type, must implement INumberBase.</typeparam>
    /// <param name="filter">The filter builder to apply the condition to.</param>
    /// <returns>A filter builder that continues the fluent chain.</returns>
    public static FilterBuilder<TValue> NonZero<TValue>(in this FilterBuilder<TValue> filter)
        where TValue : INumberBase<TValue> =>
        filter.Satisfies(n => !TValue.IsZero(n));

    /// <summary>
    /// Filters numeric values that are not positive (zero or negative).
    /// </summary>
    /// <typeparam name="TValue">The numeric type, must implement INumberBase.</typeparam>
    /// <param name="filter">The filter builder to apply the condition to.</param>
    /// <returns>A filter builder that continues the fluent chain.</returns>
    public static FilterBuilder<TValue> NonPositive<TValue>(in this FilterBuilder<TValue> filter)
        where TValue : INumberBase<TValue> =>
        filter.Satisfies(n => !TValue.IsPositive(n));

    /// <summary>
    /// Filters numeric values that are not negative (zero or positive).
    /// </summary>
    /// <typeparam name="TValue">The numeric type, must implement INumberBase.</typeparam>
    /// <param name="filter">The filter builder to apply the condition to.</param>
    /// <returns>A filter builder that continues the fluent chain.</returns>
    public static FilterBuilder<TValue> NonNegative<TValue>(in this FilterBuilder<TValue> filter)
        where TValue : INumberBase<TValue> =>
        filter.Satisfies(n => !TValue.IsNegative(n));
}
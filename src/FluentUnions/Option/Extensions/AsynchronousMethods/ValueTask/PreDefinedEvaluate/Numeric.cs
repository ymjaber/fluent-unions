using System.Numerics;

namespace FluentUnions;

/// <summary>Provides pre-defined filter methods for numeric values in <see cref="Option{T}"/> types using ValueTask-based asynchronous evaluation.</summary>
public static partial class FilterBuilderExtensions
{
    public static async ValueTask<FilterBuilder<TValue>> GreaterThan<TValue>(this ValueTask<FilterBuilder<TValue>> filter,
        TValue min)
        where TValue : IComparable<TValue>
        => (await filter.ConfigureAwait(false)).GreaterThan(min);

    public static async ValueTask<FilterBuilder<TValue>> GreaterThanOrEqualTo<TValue>(this ValueTask<FilterBuilder<TValue>> filter,
        TValue min)
        where TValue : IComparable<TValue>
        => (await filter.ConfigureAwait(false)).GreaterThanOrEqualTo(min);

    public static async ValueTask<FilterBuilder<TValue>> LessThan<TValue>(this ValueTask<FilterBuilder<TValue>> filter, TValue max)
        where TValue : IComparable<TValue>
        => (await filter.ConfigureAwait(false)).LessThan(max);

    public static async ValueTask<FilterBuilder<TValue>> LessThanOrEqualTo<TValue>(this ValueTask<FilterBuilder<TValue>> filter,
        TValue max)
        where TValue : IComparable<TValue>
        => (await filter.ConfigureAwait(false)).LessThanOrEqualTo(max);

    public static async ValueTask<FilterBuilder<TValue>> InRange<TValue>(
        this ValueTask<FilterBuilder<TValue>> filter,
        TValue min,
        bool minInclusive,
        TValue max,
        bool maxInclusive)
        where TValue : IComparable<TValue>
        => (await filter.ConfigureAwait(false)).InRange(min, minInclusive, max, maxInclusive);

    public static async ValueTask<FilterBuilder<TValue>> Positive<TValue>(this ValueTask<FilterBuilder<TValue>> filter)
        where TValue : INumberBase<TValue>
        => (await filter.ConfigureAwait(false)).Positive();

    public static async ValueTask<FilterBuilder<TValue>> Negative<TValue>(this ValueTask<FilterBuilder<TValue>> filter)
        where TValue : INumberBase<TValue>
        => (await filter.ConfigureAwait(false)).Negative();

    public static async ValueTask<bool> Zero<TValue>(this ValueTask<FilterBuilder<TValue>> filter)
        where TValue : INumberBase<TValue>
        => (await filter.ConfigureAwait(false)).Zero();

    public static async ValueTask<FilterBuilder<TValue>> NonZero<TValue>(this ValueTask<FilterBuilder<TValue>> filter)
        where TValue : INumberBase<TValue>
        => (await filter.ConfigureAwait(false)).NonZero();

    public static async ValueTask<FilterBuilder<TValue>> NonPositive<TValue>(this ValueTask<FilterBuilder<TValue>> filter)
        where TValue : INumberBase<TValue>
        => (await filter.ConfigureAwait(false)).NonPositive();

    public static async ValueTask<FilterBuilder<TValue>> NonNegative<TValue>(this ValueTask<FilterBuilder<TValue>> filter)
        where TValue : INumberBase<TValue>
        => (await filter.ConfigureAwait(false)).NonNegative();
}
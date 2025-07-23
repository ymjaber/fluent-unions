using System.Numerics;

namespace FluentUnions;

/// <summary>Provides pre-defined filter methods for numeric values in <see cref="Option{T}"/> types using Task-based asynchronous evaluation.</summary>
public static partial class FilterBuilderExtensions
{
    public static async Task<FilterBuilder<TValue>> GreaterThan<TValue>(this Task<FilterBuilder<TValue>> filter,
        TValue min)
        where TValue : IComparable<TValue>
        => (await filter.ConfigureAwait(false)).GreaterThan(min);

    public static async Task<FilterBuilder<TValue>> GreaterThanOrEqualTo<TValue>(this Task<FilterBuilder<TValue>> filter,
        TValue min)
        where TValue : IComparable<TValue>
        => (await filter.ConfigureAwait(false)).GreaterThanOrEqualTo(min);

    public static async Task<FilterBuilder<TValue>> LessThan<TValue>(this Task<FilterBuilder<TValue>> filter, TValue max)
        where TValue : IComparable<TValue>
        => (await filter.ConfigureAwait(false)).LessThan(max);

    public static async Task<FilterBuilder<TValue>> LessThanOrEqualTo<TValue>(this Task<FilterBuilder<TValue>> filter,
        TValue max)
        where TValue : IComparable<TValue>
        => (await filter.ConfigureAwait(false)).LessThanOrEqualTo(max);

    public static async Task<FilterBuilder<TValue>> InRange<TValue>(
        this Task<FilterBuilder<TValue>> filter,
        TValue min,
        bool minInclusive,
        TValue max,
        bool maxInclusive)
        where TValue : IComparable<TValue>
        => (await filter.ConfigureAwait(false)).InRange(min, minInclusive, max, maxInclusive);

    public static async Task<FilterBuilder<TValue>> Positive<TValue>(this Task<FilterBuilder<TValue>> filter)
        where TValue : INumberBase<TValue>
        => (await filter.ConfigureAwait(false)).Positive();

    public static async Task<FilterBuilder<TValue>> Negative<TValue>(this Task<FilterBuilder<TValue>> filter)
        where TValue : INumberBase<TValue>
        => (await filter.ConfigureAwait(false)).Negative();

    public static async Task<bool> Zero<TValue>(this Task<FilterBuilder<TValue>> filter)
        where TValue : INumberBase<TValue>
        => (await filter.ConfigureAwait(false)).Zero();

    public static async Task<FilterBuilder<TValue>> NonZero<TValue>(this Task<FilterBuilder<TValue>> filter)
        where TValue : INumberBase<TValue>
        => (await filter.ConfigureAwait(false)).NonZero();

    public static async Task<FilterBuilder<TValue>> NonPositive<TValue>(this Task<FilterBuilder<TValue>> filter)
        where TValue : INumberBase<TValue>
        => (await filter.ConfigureAwait(false)).NonPositive();

    public static async Task<FilterBuilder<TValue>> NonNegative<TValue>(this Task<FilterBuilder<TValue>> filter)
        where TValue : INumberBase<TValue>
        => (await filter.ConfigureAwait(false)).NonNegative();
}
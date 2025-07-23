namespace FluentUnions;

/// <summary>Provides pre-defined general-purpose filter methods for <see cref="Option{T}"/> types using ValueTask-based asynchronous evaluation.</summary>
public static partial class FilterBuilderExtensions
{
    public static async ValueTask<bool> IsEqualTo<TValue>(
        this ValueTask<FilterBuilder<TValue>> filter,
        TValue value)
        where TValue : IEquatable<TValue>
        => (await filter.ConfigureAwait(false)).IsEqualTo(value);

    public static async ValueTask<FilterBuilder<TValue>> IsNotEqualTo<TValue>(
        this ValueTask<FilterBuilder<TValue>> filter,
        TValue value)
        where TValue : IEquatable<TValue>
        => (await filter.ConfigureAwait(false)).IsNotEqualTo(value);
}
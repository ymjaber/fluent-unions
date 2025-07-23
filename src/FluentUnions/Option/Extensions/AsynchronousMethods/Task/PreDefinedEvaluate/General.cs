namespace FluentUnions;

/// <summary>Provides pre-defined general-purpose filter methods for <see cref="Option{T}"/> types using Task-based asynchronous evaluation.</summary>
public static partial class FilterBuilderExtensions
{
    public static async Task<bool> IsEqualTo<TValue>(
        this Task<FilterBuilder<TValue>> filter,
        TValue value)
        where TValue : IEquatable<TValue>
        => (await filter.ConfigureAwait(false)).IsEqualTo(value);

    public static async Task<FilterBuilder<TValue>> IsNotEqualTo<TValue>(
        this Task<FilterBuilder<TValue>> filter,
        TValue value)
        where TValue : IEquatable<TValue>
        => (await filter.ConfigureAwait(false)).IsNotEqualTo(value);
}
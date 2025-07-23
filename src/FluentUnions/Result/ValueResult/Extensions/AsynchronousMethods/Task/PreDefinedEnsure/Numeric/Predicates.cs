using System.Numerics;

namespace FluentUnions;

/// <summary>Provides pre-defined validation predicates for numeric values in <see cref="Result{T}"/> types using Task-based asynchronous validation.</summary>
public static partial class EnsureBuilderExtensions
{
    public static async Task<EnsureBuilder<TValue>> GreaterThan<TValue>(
        this Task<EnsureBuilder<TValue>> ensure,
        TValue min,
        Error? error = null) where TValue : IComparable<TValue>
        => (await ensure.ConfigureAwait(false)).GreaterThan(min, error);

    public static async Task<EnsureBuilder<TValue>> GreaterThanOrEqualTo<TValue>(
        this Task<EnsureBuilder<TValue>> ensure,
        TValue min,
        Error? error = null) where TValue : IComparable<TValue>
        => (await ensure.ConfigureAwait(false)).GreaterThanOrEqualTo(min, error);

    public static async Task<EnsureBuilder<TValue>> LessThan<TValue>(
        this Task<EnsureBuilder<TValue>> ensure,
        TValue max,
        Error? error = null) where TValue : IComparable<TValue>
        => (await ensure.ConfigureAwait(false)).LessThan(max, error);

    public static async Task<EnsureBuilder<TValue>> LessThanOrEqualTo<TValue>(
        this Task<EnsureBuilder<TValue>> ensure,
        TValue max,
        Error? error = null) where TValue : IComparable<TValue>
        => (await ensure.ConfigureAwait(false)).LessThanOrEqualTo(max, error);

    public static async Task<EnsureBuilder<TValue>> InRange<TValue>(
        this Task<EnsureBuilder<TValue>> ensure,
        TValue min,
        bool minInclusive,
        TValue max,
        bool maxInclusive,
        Error? error = null) where TValue : IComparable<TValue>
        => (await ensure.ConfigureAwait(false)).InRange(min, minInclusive, max, maxInclusive, error);

    public static async Task<EnsureBuilder<TValue>> Positive<TValue>(
        this Task<EnsureBuilder<TValue>> ensure,
        Error? error = null) where TValue : INumberBase<TValue>
        => (await ensure.ConfigureAwait(false)).Positive(error);

    public static async Task<EnsureBuilder<TValue>> Negative<TValue>(
        this Task<EnsureBuilder<TValue>> ensure,
        Error? error = null) where TValue : INumberBase<TValue>
        => (await ensure.ConfigureAwait(false)).Negative(error);

    public static async Task<Result> Zero<TValue>(
        this Task<EnsureBuilder<TValue>> ensure,
        Error? error = null) where TValue : INumberBase<TValue>
        => (await ensure.ConfigureAwait(false)).Zero(error);

    public static async Task<EnsureBuilder<TValue>> NonZero<TValue>(
        this Task<EnsureBuilder<TValue>> ensure,
        Error? error = null) where TValue : INumberBase<TValue>
        => (await ensure.ConfigureAwait(false)).NonZero(error);

    public static async Task<EnsureBuilder<TValue>> NonPositive<TValue>(
        this Task<EnsureBuilder<TValue>> ensure,
        Error? error = null) where TValue : INumberBase<TValue>
        => (await ensure.ConfigureAwait(false)).NonPositive(error);

    public static async Task<EnsureBuilder<TValue>> NonNegative<TValue>(
        this Task<EnsureBuilder<TValue>> ensure,
        Error? error = null) where TValue : INumberBase<TValue>
        => (await ensure.ConfigureAwait(false)).NonNegative(error);
}
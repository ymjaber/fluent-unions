namespace FluentUnions;

/// <summary>Provides pre-defined general-purpose validation predicates for <see cref="Result{T}"/> types using ValueTask-based asynchronous validation.</summary>
public static partial class EnsureBuilderExtensions
{
    public static async ValueTask<Result> IsEqualTo<TValue>(
        this ValueTask<EnsureBuilder<TValue>> ensure,
        TValue value,
        Error? error = null)
        where TValue : IEquatable<TValue>
        => (await ensure.ConfigureAwait(false)).IsEqualTo(value, error);

    public static async ValueTask<EnsureBuilder<TValue>> IsNotEqualTo<TValue>(
        this ValueTask<EnsureBuilder<TValue>> ensure,
        TValue value,
        Error? error = null)
        where TValue : IEquatable<TValue>
        => (await ensure.ConfigureAwait(false)).IsNotEqualTo(value, error);
}
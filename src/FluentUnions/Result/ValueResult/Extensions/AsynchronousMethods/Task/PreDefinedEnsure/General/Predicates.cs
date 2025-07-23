namespace FluentUnions;

/// <summary>Provides pre-defined general-purpose validation predicates for <see cref="Result{T}"/> types using Task-based asynchronous validation.</summary>
public static partial class EnsureBuilderExtensions
{
    public static async Task<Result> IsEqualTo<TValue>(
        this Task<EnsureBuilder<TValue>> ensure,
        TValue value,
        Error? error = null)
        where TValue : IEquatable<TValue>
        => (await ensure.ConfigureAwait(false)).IsEqualTo(value, error);

    public static async Task<EnsureBuilder<TValue>> IsNotEqualTo<TValue>(
        this Task<EnsureBuilder<TValue>> ensure,
        TValue value,
        Error? error = null)
        where TValue : IEquatable<TValue>
        => (await ensure.ConfigureAwait(false)).IsNotEqualTo(value, error);
}
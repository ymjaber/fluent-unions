namespace FluentUnions;

/// <summary>Provides pre-defined validation predicates for enum values in <see cref="Result{T}"/> types using ValueTask-based asynchronous validation.</summary>
public static partial class EnsureBuilderExtensions
{
    public static async ValueTask<EnsureBuilder<TEnum>> Defined<TEnum>(
        this ValueTask<EnsureBuilder<TEnum>> ensure,
        Error? error = null)
        where TEnum : struct, Enum
        => (await ensure.ConfigureAwait(false)).Defined(error);
}
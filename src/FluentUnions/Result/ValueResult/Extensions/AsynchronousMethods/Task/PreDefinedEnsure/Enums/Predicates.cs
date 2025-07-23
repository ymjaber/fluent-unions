namespace FluentUnions;

/// <summary>Provides pre-defined validation predicates for enum values in <see cref="Result{T}"/> types using Task-based asynchronous validation.</summary>
public static partial class EnsureBuilderExtensions
{
    public static async Task<EnsureBuilder<TEnum>> Defined<TEnum>(
        this Task<EnsureBuilder<TEnum>> ensure,
        Error? error = null)
        where TEnum : struct, Enum
        => (await ensure.ConfigureAwait(false)).Defined(error);
}
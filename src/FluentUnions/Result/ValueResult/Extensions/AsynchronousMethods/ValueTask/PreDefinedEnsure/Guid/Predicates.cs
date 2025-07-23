namespace FluentUnions;

/// <summary>Provides pre-defined validation predicates for Guid values in <see cref="Result{T}"/> types using ValueTask-based asynchronous validation.</summary>
public static partial class EnsureBuilderExtensions
{
    public static async ValueTask<Result> Empty(
        this ValueTask<EnsureBuilder<Guid>> ensure,
        Error? error = null)
        => (await ensure.ConfigureAwait(false)).Empty(error);

    public static async ValueTask<EnsureBuilder<Guid>> NotEmpty(
        this ValueTask<EnsureBuilder<Guid>> ensure,
        Error? error = null)
        => (await ensure.ConfigureAwait(false)).NotEmpty(error);
}
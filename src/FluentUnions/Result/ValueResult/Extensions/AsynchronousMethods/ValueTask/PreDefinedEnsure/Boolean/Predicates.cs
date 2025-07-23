namespace FluentUnions;

/// <summary>Provides pre-defined validation predicates for boolean values in <see cref="Result{T}"/> types using ValueTask-based asynchronous validation.</summary>
public static partial class EnsureBuilderExtensions
{
    public static async ValueTask<Result> True(this ValueTask<EnsureBuilder<bool>> ensure, Error? error = null)
        => (await ensure.ConfigureAwait(false)).True(error);

    public static async ValueTask<Result> False(this ValueTask<EnsureBuilder<bool>> ensure, Error? error = null)
        => (await ensure.ConfigureAwait(false)).False(error);
}
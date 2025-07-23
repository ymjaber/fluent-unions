namespace FluentUnions;

/// <summary>Provides pre-defined validation predicates for boolean values in <see cref="Result{T}"/> types using Task-based asynchronous validation.</summary>
public static partial class EnsureBuilderExtensions
{
    public static async Task<Result> True(this Task<EnsureBuilder<bool>> ensure, Error? error = null)
        => (await ensure.ConfigureAwait(false)).True(error);

    public static async Task<Result> False(this Task<EnsureBuilder<bool>> ensure, Error? error = null)
        => (await ensure.ConfigureAwait(false)).False(error);
}
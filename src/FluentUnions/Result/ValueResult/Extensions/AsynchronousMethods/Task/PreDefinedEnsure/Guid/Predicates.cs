namespace FluentUnions;

/// <summary>Provides pre-defined validation predicates for Guid values in <see cref="Result{T}"/> types using Task-based asynchronous validation.</summary>
public static partial class EnsureBuilderExtensions
{
    public static async Task<Result> Empty(
        this Task<EnsureBuilder<Guid>> ensure,
        Error? error = null)
        => (await ensure.ConfigureAwait(false)).Empty(error);

    public static async Task<EnsureBuilder<Guid>> NotEmpty(
        this Task<EnsureBuilder<Guid>> ensure,
        Error? error = null)
        => (await ensure.ConfigureAwait(false)).NotEmpty(error);
}
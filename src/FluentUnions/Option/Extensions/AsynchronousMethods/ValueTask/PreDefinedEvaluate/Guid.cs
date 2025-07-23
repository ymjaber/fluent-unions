namespace FluentUnions;

/// <summary>Provides pre-defined filter methods for Guid values in <see cref="Option{T}"/> types using ValueTask-based asynchronous evaluation.</summary>
public static partial class FilterBuilderExtensions
{
    public static async ValueTask<bool> Empty(this ValueTask<FilterBuilder<Guid>> filter)
        => (await filter.ConfigureAwait(false)).Empty();

    public static async ValueTask<FilterBuilder<Guid>> NotEmpty(this ValueTask<FilterBuilder<Guid>> filter)
        => (await filter.ConfigureAwait(false)).NotEmpty();
}
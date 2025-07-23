namespace FluentUnions;

/// <summary>Provides pre-defined filter methods for boolean values in <see cref="Option{T}"/> types using ValueTask-based asynchronous evaluation.</summary>
public static partial class FilterBuilderExtensions
{
    public static async ValueTask<bool> True(this ValueTask<FilterBuilder<bool>> filter) =>
        (await filter.ConfigureAwait(false)).True();

    public static async ValueTask<bool> False(this ValueTask<FilterBuilder<bool>> filter) =>
        (await filter.ConfigureAwait(false)).False();
}
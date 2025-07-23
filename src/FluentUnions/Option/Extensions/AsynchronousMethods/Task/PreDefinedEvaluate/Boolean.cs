namespace FluentUnions;

/// <summary>Provides pre-defined filter methods for boolean values in <see cref="Option{T}"/> types using Task-based asynchronous evaluation.</summary>
public static partial class FilterBuilderExtensions
{
    public static async Task<bool> True(this Task<FilterBuilder<bool>> filter) =>
        (await filter.ConfigureAwait(false)).True();

    public static async Task<bool> False(this Task<FilterBuilder<bool>> filter) =>
        (await filter.ConfigureAwait(false)).False();
}
namespace FluentUnions;

/// <summary>Provides pre-defined filter methods for Guid values in <see cref="Option{T}"/> types using Task-based asynchronous evaluation.</summary>
public static partial class FilterBuilderExtensions
{
    public static async Task<bool> Empty(this Task<FilterBuilder<Guid>> filter)
        => (await filter.ConfigureAwait(false)).Empty();

    public static async Task<FilterBuilder<Guid>> NotEmpty(this Task<FilterBuilder<Guid>> filter)
        => (await filter.ConfigureAwait(false)).NotEmpty();
}
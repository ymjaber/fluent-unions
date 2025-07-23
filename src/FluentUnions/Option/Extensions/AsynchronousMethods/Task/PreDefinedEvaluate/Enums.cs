namespace FluentUnions;

/// <summary>Provides pre-defined filter methods for enum values in <see cref="Option{T}"/> types using Task-based asynchronous evaluation.</summary>
public static partial class FilterBuilderExtensions
{
    public static async Task<FilterBuilder<TEnum>> Defined<TEnum>(this Task<FilterBuilder<TEnum>> filter)
        where TEnum : struct, Enum
        => (await filter.ConfigureAwait(false)).Defined();
}
namespace FluentUnions;

/// <summary>Provides pre-defined filter methods for enum values in <see cref="Option{T}"/> types using ValueTask-based asynchronous evaluation.</summary>
public static partial class FilterBuilderExtensions
{
    public static async ValueTask<FilterBuilder<TEnum>> Defined<TEnum>(this ValueTask<FilterBuilder<TEnum>> filter)
        where TEnum : struct, Enum
        => (await filter.ConfigureAwait(false)).Defined();
}
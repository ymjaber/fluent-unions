namespace FluentUnions;

/// <summary>Provides pre-defined filter methods for enum values in <see cref="Option{T}"/> types.</summary>
public static partial class FilterBuilderExtensions
{
    /// <summary>
    /// Filters options where the enum value is defined in the enum type.
    /// </summary>
    /// <typeparam name="TEnum">The type of the enum value.</typeparam>
    /// <param name="filter">The filter builder for the enum value.</param>
    /// <returns>An option that validates the enum value is defined.</returns>
    public static Option<TEnum> Defined<TEnum>(in this FilterBuilder<TEnum> filter)
        where TEnum : struct, Enum => filter.Satisfies(Enum.IsDefined);
}

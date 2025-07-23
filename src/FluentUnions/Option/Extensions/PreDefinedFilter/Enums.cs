namespace FluentUnions;

/// <summary>Provides pre-defined filter methods for enum values in <see cref="Option{T}"/> types.</summary>
public static partial class FilterBuilderExtensions
{
    /// <summary>
    /// Filters enum values that are defined in the enum type.
    /// </summary>
    /// <typeparam name="TEnum">The enum type.</typeparam>
    /// <param name="filter">The filter builder to apply the condition to.</param>
    /// <returns>A filter builder that continues the fluent chain.</returns>
    /// <remarks>
    /// This is useful for validating that an enum value is a valid member of the enum type,
    /// excluding invalid cast values.
    /// </remarks>
    public static FilterBuilder<TEnum> Defined<TEnum>(in this FilterBuilder<TEnum> filter)
        where TEnum : struct, Enum => filter.Satisfies(Enum.IsDefined);
}
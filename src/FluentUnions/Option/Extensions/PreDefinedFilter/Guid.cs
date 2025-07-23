namespace FluentUnions;

/// <summary>Provides pre-defined filter methods for Guid values in <see cref="Option{T}"/> types.</summary>
public static partial class FilterBuilderExtensions
{
    /// <summary>
    /// Checks if the GUID value is empty (all zeros).
    /// </summary>
    /// <param name="filter">The filter builder to apply the condition to.</param>
    /// <returns>True if the GUID is empty; otherwise, false.</returns>
    public static bool Empty(in this FilterBuilder<Guid> filter) =>
        filter.Satisfies(v => v == Guid.Empty).Build().IsSome;

    /// <summary>
    /// Filters GUID values that are not empty.
    /// </summary>
    /// <param name="filter">The filter builder to apply the condition to.</param>
    /// <returns>A filter builder that continues the fluent chain.</returns>
    public static FilterBuilder<Guid> NotEmpty(in this FilterBuilder<Guid> filter) =>
        filter.Satisfies(v => v != Guid.Empty);
}
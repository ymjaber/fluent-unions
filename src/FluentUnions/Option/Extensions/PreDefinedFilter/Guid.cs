namespace FluentUnions;

/// <summary>Provides pre-defined filter methods for Guid values in <see cref="Option{T}"/> types.</summary>
public static partial class FilterBuilderExtensions
{
    /// <summary>
    /// Filters options where the Guid value is empty (all zeros).
    /// </summary>
    /// <param name="filter">The filter builder for the Guid value.</param>
    /// <returns>True if the value passes the filter (is empty); otherwise, false.</returns>
    public static bool Empty(in this FilterBuilder<Guid> filter) =>
        filter.Satisfies(v => v == Guid.Empty).IsSome;

    /// <summary>
    /// Filters options where the Guid value is not empty (not all zeros).
    /// </summary>
    /// <param name="filter">The filter builder for the Guid value.</param>
    /// <returns>An option that validates the Guid is not empty.</returns>
    public static Option<Guid> NotEmpty(in this FilterBuilder<Guid> filter) =>
        filter.Satisfies(v => v != Guid.Empty);
}

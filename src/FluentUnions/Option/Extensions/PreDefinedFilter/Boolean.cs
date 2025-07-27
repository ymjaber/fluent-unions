namespace FluentUnions;

/// <summary>Provides pre-defined filter methods for boolean values in <see cref="Option{T}"/> types.</summary>
public static partial class FilterBuilderExtensions
{
    /// <summary>
    /// Filters options where the boolean value is true.
    /// </summary>
    /// <param name="filter">The filter builder for the boolean value.</param>
    /// <returns>True if the value passes the filter (is true); otherwise, false.</returns>
    public static bool True(in this FilterBuilder<bool> filter) => filter.Satisfies(v => v).IsSome;

    /// <summary>
    /// Filters options where the boolean value is false.
    /// </summary>
    /// <param name="filter">The filter builder for the boolean value.</param>
    /// <returns>True if the value passes the filter (is false); otherwise, false.</returns>
    public static bool False(in this FilterBuilder<bool> filter) => filter.Satisfies(v => !v).IsSome;
}

namespace FluentUnions;

/// <summary>Provides pre-defined filter methods for boolean values in <see cref="Option{T}"/> types.</summary>
public static partial class FilterBuilderExtensions
{
    /// <summary>
    /// Checks if the boolean value is true.
    /// </summary>
    /// <param name="filter">The filter builder to apply the condition to.</param>
    /// <returns>True if the value is true; otherwise, false.</returns>
    public static bool True(in this FilterBuilder<bool> filter) => filter.Satisfies(v => v).Build().IsSome;
    
    /// <summary>
    /// Checks if the boolean value is false.
    /// </summary>
    /// <param name="filter">The filter builder to apply the condition to.</param>
    /// <returns>True if the value is false; otherwise, false.</returns>
    public static bool False(in this FilterBuilder<bool> filter) => filter.Satisfies(v => !v).Build().IsSome;
}
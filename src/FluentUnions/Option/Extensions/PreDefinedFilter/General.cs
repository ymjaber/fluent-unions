namespace FluentUnions;

/// <summary>Provides pre-defined general-purpose filter methods for <see cref="Option{T}"/> types.</summary>
public static partial class FilterBuilderExtensions
{
    /// <summary>
    /// Filters options where the value is equal to the specified value.
    /// </summary>
    /// <typeparam name="TValue">The type of the value to compare.</typeparam>
    /// <param name="filter">The filter builder for the value.</param>
    /// <param name="value">The value to compare with.</param>
    /// <returns>True if the value passes the filter (is equal); otherwise, false.</returns>
    public static bool IsEqualTo<TValue>(
        in this FilterBuilder<TValue> filter,
        TValue value)
        where TValue : IEquatable<TValue>
        => filter.Satisfies(v => v.Equals(value)).IsSome;

    /// <summary>
    /// Filters options where the value is not equal to the specified value.
    /// </summary>
    /// <typeparam name="TValue">The type of the value to compare.</typeparam>
    /// <param name="filter">The filter builder for the value.</param>
    /// <param name="value">The value to compare with.</param>
    /// <returns>An option that validates the values are not equal.</returns>
    public static Option<TValue> IsNotEqualTo<TValue>(
        in this FilterBuilder<TValue> filter,
        TValue value)
        where TValue : IEquatable<TValue>
        => filter.Satisfies(v => !v.Equals(value));
}

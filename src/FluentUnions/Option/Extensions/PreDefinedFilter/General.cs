namespace FluentUnions;

/// <summary>Provides pre-defined general-purpose filter methods for <see cref="Option{T}"/> types.</summary>
public static partial class FilterBuilderExtensions
{
    /// <summary>
    /// Checks if the value is equal to the specified value.
    /// </summary>
    /// <typeparam name="TValue">The type of the value, must implement IEquatable.</typeparam>
    /// <param name="filter">The filter builder to apply the condition to.</param>
    /// <param name="value">The value to compare for equality.</param>
    /// <returns>True if the values are equal; otherwise, false.</returns>
    public static bool IsEqualTo<TValue>(
        in this FilterBuilder<TValue> filter,
        TValue value)
        where TValue : IEquatable<TValue>
        => filter.Satisfies(v => v.Equals(value)).Build().IsSome;

    /// <summary>
    /// Filters values that are not equal to the specified value.
    /// </summary>
    /// <typeparam name="TValue">The type of the value, must implement IEquatable.</typeparam>
    /// <param name="filter">The filter builder to apply the condition to.</param>
    /// <param name="value">The value to compare for inequality.</param>
    /// <returns>A filter builder that continues the fluent chain.</returns>
    public static FilterBuilder<TValue> IsNotEqualTo<TValue>(
        in this FilterBuilder<TValue> filter,
        TValue value)
        where TValue : IEquatable<TValue>
        => filter.Satisfies(v => !v.Equals(value));
}
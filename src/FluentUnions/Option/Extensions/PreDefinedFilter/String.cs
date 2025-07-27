using System.Text.RegularExpressions;

namespace FluentUnions;

/// <summary>Provides pre-defined filter methods for string values in <see cref="Option{T}"/> types.</summary>
public static partial class FilterBuilderExtensions
{
    /// <summary>
    /// Filters options where the string is empty.
    /// </summary>
    /// <param name="filter">The filter builder for the string value.</param>
    /// <returns>True if the value passes the filter (is empty); otherwise, false.</returns>
    public static bool Empty(in this FilterBuilder<string> filter) =>
        filter.Satisfies(s => s == string.Empty).IsSome;

    /// <summary>
    /// Filters options where the string is not empty.
    /// </summary>
    /// <param name="filter">The filter builder for the string value.</param>
    /// <returns>An option that validates the string is Empty</returns>
    public static Option<string> NotEmpty(in this FilterBuilder<string> filter) =>
        filter.Satisfies(s => s != string.Empty);

    /// <summary>
    /// Filters options where the string has exactly the specified length.
    /// </summary>
    /// <param name="filter">The filter builder for the string value.</param>
    /// <param name="length">The required length.</param>
    /// <returns>An option that validates the string has the specified value</returns>
    public static Option<string> HasLength(in this FilterBuilder<string> filter, int length) =>
        filter.Satisfies(s => s.Length == length);

    /// <summary>
    /// Filters options where the string is longer than the specified length.
    /// </summary>
    /// <param name="filter">The filter builder for the string value.</param>
    /// <param name="length">The minimum length (exclusive).</param>
    /// <returns>An option that validates the string is longer than the specified length</returns>
    public static Option<string> LongerThan(in this FilterBuilder<string> filter, int length) =>
        filter.Satisfies(s => s.Length > length);

    /// <summary>
    /// Filters options where the string is longer than or equal to the specified length.
    /// </summary>
    /// <param name="filter">The filter builder for the string value.</param>
    /// <param name="length">The minimum length (inclusive).</param>
    /// <returns>An option that validates the string is longer than or equals to the specified length</returns>
    public static Option<string> LongerThanOrEqualTo(in this FilterBuilder<string> filter, int length) =>
        filter.Satisfies(s => s.Length >= length);

    /// <summary>
    /// Filters options where the string is shorter than the specified length.
    /// </summary>
    /// <param name="filter">The filter builder for the string value.</param>
    /// <param name="length">The maximum length (exclusive).</param>
    /// <returns>An option that validates the string is shorter than the specified length</returns>
    public static Option<string> ShorterThan(in this FilterBuilder<string> filter, int length) =>
        filter.Satisfies(s => s.Length < length);

    /// <summary>
    /// Filters options where the string is shorter than or equal to the specified length.
    /// </summary>
    /// <param name="filter">The filter builder for the string value.</param>
    /// <param name="length">The maximum length (inclusive).</param>
    /// <returns>An option that validates the string is shorter than or equals to the specified length</returns>
    public static Option<string> ShorterThanOrEqualTo(in this FilterBuilder<string> filter, int length) =>
        filter.Satisfies(s => s.Length <= length);

    /// <summary>
    /// Filters options where the string matches the specified regular expression pattern.
    /// </summary>
    /// <param name="filter">The filter builder for the string value.</param>
    /// <param name="pattern">The regular expression pattern to match.</param>
    /// <returns>An option that validates the string matches the pattern</returns>
    public static Option<string> Matches(in this FilterBuilder<string> filter, Regex pattern) =>
        filter.Satisfies(pattern.IsMatch);

    /// <summary>
    /// Filters options where the string does not match the specified regular expression pattern.
    /// </summary>
    /// <param name="filter">The filter builder for the string value.</param>
    /// <param name="pattern">The regular expression pattern that should not match.</param>
    /// <returns>An option that validates the string doesn't match the pattern</returns>
    public static Option<string> NotMatch(in this FilterBuilder<string> filter, Regex pattern) =>
        filter.Satisfies(s => !pattern.IsMatch(s));

    /// <summary>
    /// Filters options where the string contains the specified substring.
    /// </summary>
    /// <param name="filter">The filter builder for the string value.</param>
    /// <param name="substring">The substring that must be present.</param>
    /// <returns>An option that validates the string contains the specified substring</returns>
    public static Option<string> Contains(in this FilterBuilder<string> filter, string substring) =>
        filter.Satisfies(s => s.Contains(substring));

    /// <summary>
    /// Filters options where the string does not contain the specified substring.
    /// </summary>
    /// <param name="filter">The filter builder for the string value.</param>
    /// <param name="substring">The substring that must not be present.</param>
    /// <returns>An option that validates the string doesn't contain the specified substring</returns>
    public static Option<string> NotContains(in this FilterBuilder<string> filter, string substring) =>
        filter.Satisfies(s => !s.Contains(substring));

    /// <summary>
    /// Filters options where the string starts with the specified substring.
    /// </summary>
    /// <param name="filter">The filter builder for the string value.</param>
    /// <param name="substring">The substring that must be at the beginning.</param>
    /// <returns>An option that validates the string starts with the specified substring</returns>
    public static Option<string> StartsWith(in this FilterBuilder<string> filter, string substring) =>
        filter.Satisfies(s => s.StartsWith(substring));

    /// <summary>
    /// Filters options where the string does not start with the specified substring.
    /// </summary>
    /// <param name="filter">The filter builder for the string value.</param>
    /// <param name="substring">The substring that must not be at the beginning.</param>
    /// <returns>An option that validates doesn't start with the specified substring</returns>
    public static Option<string> NotStartsWith(in this FilterBuilder<string> filter, string substring) =>
        filter.Satisfies(s => !s.StartsWith(substring));

    /// <summary>
    /// Filters options where the string ends with the specified substring.
    /// </summary>
    /// <param name="filter">The filter builder for the string string.</param>
    /// <param name="substring">The substring that must be at the end.</param>
    /// <returns>An option that validates the value ends with the specified substring</returns>
    public static Option<string> EndsWith(in this FilterBuilder<string> filter, string substring) =>
        filter.Satisfies(s => s.EndsWith(substring));

    /// <summary>
    /// Filters options where the string does not end with the specified substring.
    /// </summary>
    /// <param name="filter">The filter builder for the string value.</param>
    /// <param name="substring">The substring that must not be at the end.</param>
    /// <returns>An option that validates the string doesn't end with the specified substring</returns>
    public static Option<string> NotEndsWith(in this FilterBuilder<string> filter, string substring) =>
        filter.Satisfies(s => !s.EndsWith(substring));
}

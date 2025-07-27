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
        filter.Satisfies(s => s == string.Empty).Build().IsSome;

    /// <summary>
    /// Filters options where the string is not empty.
    /// </summary>
    /// <param name="filter">The filter builder for the string value.</param>
    /// <returns>The filter builder for method chaining.</returns>
    public static FilterBuilder<string> NotEmpty(in this FilterBuilder<string> filter) =>
        filter.Satisfies(s => s != string.Empty);

    /// <summary>
    /// Filters options where the string has exactly the specified length.
    /// </summary>
    /// <param name="filter">The filter builder for the string value.</param>
    /// <param name="length">The required length.</param>
    /// <returns>The filter builder for method chaining.</returns>
    public static FilterBuilder<string> HasLength(in this FilterBuilder<string> filter, int length) =>
        filter.Satisfies(s => s.Length == length);

    /// <summary>
    /// Filters options where the string is longer than the specified length.
    /// </summary>
    /// <param name="filter">The filter builder for the string value.</param>
    /// <param name="length">The minimum length (exclusive).</param>
    /// <returns>The filter builder for method chaining.</returns>
    public static FilterBuilder<string> LongerThan(in this FilterBuilder<string> filter, int length) =>
        filter.Satisfies(s => s.Length > length);

    /// <summary>
    /// Filters options where the string is longer than or equal to the specified length.
    /// </summary>
    /// <param name="filter">The filter builder for the string value.</param>
    /// <param name="length">The minimum length (inclusive).</param>
    /// <returns>The filter builder for method chaining.</returns>
    public static FilterBuilder<string> LongerThanOrEqualTo(in this FilterBuilder<string> filter, int length) =>
        filter.Satisfies(s => s.Length >= length);

    /// <summary>
    /// Filters options where the string is shorter than the specified length.
    /// </summary>
    /// <param name="filter">The filter builder for the string value.</param>
    /// <param name="length">The maximum length (exclusive).</param>
    /// <returns>The filter builder for method chaining.</returns>
    public static FilterBuilder<string> ShorterThan(in this FilterBuilder<string> filter, int length) =>
        filter.Satisfies(s => s.Length < length);

    /// <summary>
    /// Filters options where the string is shorter than or equal to the specified length.
    /// </summary>
    /// <param name="filter">The filter builder for the string value.</param>
    /// <param name="length">The maximum length (inclusive).</param>
    /// <returns>The filter builder for method chaining.</returns>
    public static FilterBuilder<string> ShorterThanOrEqualTo(in this FilterBuilder<string> filter, int length) =>
        filter.Satisfies(s => s.Length <= length);

    /// <summary>
    /// Filters options where the string matches the specified regular expression pattern.
    /// </summary>
    /// <param name="filter">The filter builder for the string value.</param>
    /// <param name="pattern">The regular expression pattern to match.</param>
    /// <returns>The filter builder for method chaining.</returns>
    public static FilterBuilder<string> Matches(in this FilterBuilder<string> filter, Regex pattern) =>
        filter.Satisfies(pattern.IsMatch);

    /// <summary>
    /// Filters options where the string does not match the specified regular expression pattern.
    /// </summary>
    /// <param name="filter">The filter builder for the string value.</param>
    /// <param name="pattern">The regular expression pattern that should not match.</param>
    /// <returns>The filter builder for method chaining.</returns>
    public static FilterBuilder<string> NotMatch(in this FilterBuilder<string> filter, Regex pattern) =>
        filter.Satisfies(s => !pattern.IsMatch(s));

    /// <summary>
    /// Filters options where the string contains the specified substring.
    /// </summary>
    /// <param name="filter">The filter builder for the string value.</param>
    /// <param name="substring">The substring that must be present.</param>
    /// <returns>The filter builder for method chaining.</returns>
    public static FilterBuilder<string> Contains(in this FilterBuilder<string> filter, string substring) =>
        filter.Satisfies(s => s.Contains(substring));

    /// <summary>
    /// Filters options where the string does not contain the specified substring.
    /// </summary>
    /// <param name="filter">The filter builder for the string value.</param>
    /// <param name="substring">The substring that must not be present.</param>
    /// <returns>The filter builder for method chaining.</returns>
    public static FilterBuilder<string> NotContains(in this FilterBuilder<string> filter, string substring) =>
        filter.Satisfies(s => !s.Contains(substring));

    /// <summary>
    /// Filters options where the string starts with the specified substring.
    /// </summary>
    /// <param name="filter">The filter builder for the string value.</param>
    /// <param name="substring">The substring that must be at the beginning.</param>
    /// <returns>The filter builder for method chaining.</returns>
    public static FilterBuilder<string> StartsWith(in this FilterBuilder<string> filter, string substring) =>
        filter.Satisfies(s => s.StartsWith(substring));

    /// <summary>
    /// Filters options where the string does not start with the specified substring.
    /// </summary>
    /// <param name="filter">The filter builder for the string value.</param>
    /// <param name="substring">The substring that must not be at the beginning.</param>
    /// <returns>The filter builder for method chaining.</returns>
    public static FilterBuilder<string> NotStartsWith(in this FilterBuilder<string> filter, string substring) =>
        filter.Satisfies(s => !s.StartsWith(substring));

    /// <summary>
    /// Filters options where the string ends with the specified substring.
    /// </summary>
    /// <param name="filter">The filter builder for the string value.</param>
    /// <param name="substring">The substring that must be at the end.</param>
    /// <returns>The filter builder for method chaining.</returns>
    public static FilterBuilder<string> EndsWith(in this FilterBuilder<string> filter, string substring) =>
        filter.Satisfies(s => s.EndsWith(substring));

    /// <summary>
    /// Filters options where the string does not end with the specified substring.
    /// </summary>
    /// <param name="filter">The filter builder for the string value.</param>
    /// <param name="substring">The substring that must not be at the end.</param>
    /// <returns>The filter builder for method chaining.</returns>
    public static FilterBuilder<string> NotEndsWith(in this FilterBuilder<string> filter, string substring) =>
        filter.Satisfies(s => !s.EndsWith(substring));

    /// <summary>
    /// Filters options where the string is a valid email address format.
    /// </summary>
    /// <param name="filter">The filter builder for the string value.</param>
    /// <returns>The filter builder for method chaining.</returns>
    /// <remarks>
    /// Uses a basic email validation pattern. For production use, consider more comprehensive validation.
    /// </remarks>
    public static FilterBuilder<string> MatchesEmail(in this FilterBuilder<string> filter) =>
        filter.Satisfies(s => RegexPatterns.EmailRegex().IsMatch(s));

    /// <summary>
    /// Filters options where the string is a valid URL format.
    /// </summary>
    /// <param name="filter">The filter builder for the string value.</param>
    /// <returns>The filter builder for method chaining.</returns>
    /// <remarks>
    /// Validates HTTP and HTTPS URLs. For more specific URL validation, use custom patterns.
    /// </remarks>
    public static FilterBuilder<string> MatchesUrl(in this FilterBuilder<string> filter) =>
        filter.Satisfies(s => RegexPatterns.UrlRegex().IsMatch(s));

    /// <summary>
    /// Filters options where the string is a valid phone number format.
    /// </summary>
    /// <param name="filter">The filter builder for the string value.</param>
    /// <returns>The filter builder for method chaining.</returns>
    /// <remarks>
    /// Uses a flexible pattern that accepts various international phone number formats.
    /// </remarks>
    public static FilterBuilder<string> MatchesPhoneNumber(in this FilterBuilder<string> filter) =>
        filter.Satisfies(s => RegexPatterns.PhoneNumberRegex().IsMatch(s));

    /// <summary>
    /// Filters options where the string is a valid IPv4 address format.
    /// </summary>
    /// <param name="filter">The filter builder for the string value.</param>
    /// <returns>The filter builder for method chaining.</returns>
    /// <remarks>
    /// This validates the format only, not whether the IP address is valid or reachable.
    /// </remarks>
    public static FilterBuilder<string> MatchesIpAddress(in this FilterBuilder<string> filter) =>
        filter.Satisfies(s => RegexPatterns.IpAddressRegex().IsMatch(s));

    /// <summary>
    /// Filters options where the string is a valid GUID format.
    /// </summary>
    /// <param name="filter">The filter builder for the string value.</param>
    /// <returns>The filter builder for method chaining.</returns>
    /// <remarks>
    /// Accepts GUIDs with or without braces in standard format.
    /// </remarks>
    public static FilterBuilder<string> MatchesGuid(in this FilterBuilder<string> filter) =>
        filter.Satisfies(s => RegexPatterns.GuidRegex().IsMatch(s));
}
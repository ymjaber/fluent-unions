using System.Text.RegularExpressions;

namespace FluentUnions;

/// <summary>Provides pre-defined filter methods for string values in <see cref="Option{T}"/> types.</summary>
public static partial class FilterBuilderExtensions
{
    /// <summary>
    /// Checks if the string value is empty.
    /// </summary>
    /// <param name="filter">The filter builder to apply the condition to.</param>
    /// <returns>True if the string is empty; otherwise, false.</returns>
    public static bool Empty(in this FilterBuilder<string> filter) =>
        filter.Satisfies(s => s == string.Empty).Build().IsSome;

    /// <summary>
    /// Filters string values that are not empty.
    /// </summary>
    /// <param name="filter">The filter builder to apply the condition to.</param>
    /// <returns>A filter builder that continues the fluent chain.</returns>
    public static FilterBuilder<string> NotEmpty(in this FilterBuilder<string> filter) =>
        filter.Satisfies(s => s != string.Empty);

    /// <summary>
    /// Filters string values that have exactly the specified length.
    /// </summary>
    /// <param name="filter">The filter builder to apply the condition to.</param>
    /// <param name="length">The exact length the string must have.</param>
    /// <returns>A filter builder that continues the fluent chain.</returns>
    public static FilterBuilder<string> HasLength(in this FilterBuilder<string> filter, int length) =>
        filter.Satisfies(s => s.Length == length);

    /// <summary>
    /// Filters string values that are longer than the specified length.
    /// </summary>
    /// <param name="filter">The filter builder to apply the condition to.</param>
    /// <param name="length">The length threshold.</param>
    /// <returns>A filter builder that continues the fluent chain.</returns>
    public static FilterBuilder<string> LongerThan(in this FilterBuilder<string> filter, int length) =>
        filter.Satisfies(s => s.Length > length);

    /// <summary>
    /// Filters string values that are longer than or equal to the specified length.
    /// </summary>
    /// <param name="filter">The filter builder to apply the condition to.</param>
    /// <param name="length">The minimum length threshold.</param>
    /// <returns>A filter builder that continues the fluent chain.</returns>
    public static FilterBuilder<string> LongerThanOrEqualTo(in this FilterBuilder<string> filter, int length) =>
        filter.Satisfies(s => s.Length >= length);

    /// <summary>
    /// Filters string values that are shorter than the specified length.
    /// </summary>
    /// <param name="filter">The filter builder to apply the condition to.</param>
    /// <param name="length">The length threshold.</param>
    /// <returns>A filter builder that continues the fluent chain.</returns>
    public static FilterBuilder<string> ShorterThan(in this FilterBuilder<string> filter, int length) =>
        filter.Satisfies(s => s.Length < length);

    /// <summary>
    /// Filters string values that are shorter than or equal to the specified length.
    /// </summary>
    /// <param name="filter">The filter builder to apply the condition to.</param>
    /// <param name="length">The maximum length threshold.</param>
    /// <returns>A filter builder that continues the fluent chain.</returns>
    public static FilterBuilder<string> ShorterThanOrEqualTo(in this FilterBuilder<string> filter, int length) =>
        filter.Satisfies(s => s.Length <= length);

    /// <summary>
    /// Filters string values that match the specified regular expression pattern.
    /// </summary>
    /// <param name="filter">The filter builder to apply the condition to.</param>
    /// <param name="pattern">The regular expression pattern to match against.</param>
    /// <returns>A filter builder that continues the fluent chain.</returns>
    public static FilterBuilder<string> Matches(in this FilterBuilder<string> filter, Regex pattern) =>
        filter.Satisfies(pattern.IsMatch);

    /// <summary>
    /// Filters string values that do not match the specified regular expression pattern.
    /// </summary>
    /// <param name="filter">The filter builder to apply the condition to.</param>
    /// <param name="pattern">The regular expression pattern to match against.</param>
    /// <returns>A filter builder that continues the fluent chain.</returns>
    public static FilterBuilder<string> NotMatch(in this FilterBuilder<string> filter, Regex pattern) =>
        filter.Satisfies(s => !pattern.IsMatch(s));

    /// <summary>
    /// Filters string values that contain the specified substring.
    /// </summary>
    /// <param name="filter">The filter builder to apply the condition to.</param>
    /// <param name="substring">The substring to search for.</param>
    /// <returns>A filter builder that continues the fluent chain.</returns>
    public static FilterBuilder<string> Contains(in this FilterBuilder<string> filter, string substring) =>
        filter.Satisfies(s => s.Contains(substring));

    /// <summary>
    /// Filters string values that do not contain the specified substring.
    /// </summary>
    /// <param name="filter">The filter builder to apply the condition to.</param>
    /// <param name="substring">The substring to search for.</param>
    /// <returns>A filter builder that continues the fluent chain.</returns>
    public static FilterBuilder<string> NotContains(in this FilterBuilder<string> filter, string substring) =>
        filter.Satisfies(s => !s.Contains(substring));

    /// <summary>
    /// Filters string values that start with the specified substring.
    /// </summary>
    /// <param name="filter">The filter builder to apply the condition to.</param>
    /// <param name="substring">The substring to check at the beginning.</param>
    /// <returns>A filter builder that continues the fluent chain.</returns>
    public static FilterBuilder<string> StartsWith(in this FilterBuilder<string> filter, string substring) =>
        filter.Satisfies(s => s.StartsWith(substring));

    /// <summary>
    /// Filters string values that do not start with the specified substring.
    /// </summary>
    /// <param name="filter">The filter builder to apply the condition to.</param>
    /// <param name="substring">The substring to check at the beginning.</param>
    /// <returns>A filter builder that continues the fluent chain.</returns>
    public static FilterBuilder<string> NotStartsWith(in this FilterBuilder<string> filter, string substring) =>
        filter.Satisfies(s => !s.StartsWith(substring));

    /// <summary>
    /// Filters string values that end with the specified substring.
    /// </summary>
    /// <param name="filter">The filter builder to apply the condition to.</param>
    /// <param name="substring">The substring to check at the end.</param>
    /// <returns>A filter builder that continues the fluent chain.</returns>
    public static FilterBuilder<string> EndsWith(in this FilterBuilder<string> filter, string substring) =>
        filter.Satisfies(s => s.EndsWith(substring));

    /// <summary>
    /// Filters string values that do not end with the specified substring.
    /// </summary>
    /// <param name="filter">The filter builder to apply the condition to.</param>
    /// <param name="substring">The substring to check at the end.</param>
    /// <returns>A filter builder that continues the fluent chain.</returns>
    public static FilterBuilder<string> NotEndsWith(in this FilterBuilder<string> filter, string substring) =>
        filter.Satisfies(s => !s.EndsWith(substring));

    /// <summary>
    /// Filters string values that match a standard email address format.
    /// </summary>
    /// <param name="filter">The filter builder to apply the condition to.</param>
    /// <returns>A filter builder that continues the fluent chain.</returns>
    /// <remarks>
    /// Uses a basic email regex pattern that covers common email formats.
    /// </remarks>
    public static FilterBuilder<string> MatchesEmail(in this FilterBuilder<string> filter) =>
        filter.Satisfies(s =>
            System.Text.RegularExpressions.Regex.IsMatch(s, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"));

    /// <summary>
    /// Filters string values that match a standard URL format.
    /// </summary>
    /// <param name="filter">The filter builder to apply the condition to.</param>
    /// <returns>A filter builder that continues the fluent chain.</returns>
    /// <remarks>
    /// Validates HTTP and HTTPS URLs with common path formats.
    /// </remarks>
    public static FilterBuilder<string> MatchesUrl(in this FilterBuilder<string> filter) =>
        filter.Satisfies(s =>
            System.Text.RegularExpressions.Regex.IsMatch(s, @"^http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$"));

    /// <summary>
    /// Filters string values that match a standard phone number format.
    /// </summary>
    /// <param name="filter">The filter builder to apply the condition to.</param>
    /// <returns>A filter builder that continues the fluent chain.</returns>
    /// <remarks>
    /// Supports international format with optional country code and various separators.
    /// </remarks>
    public static FilterBuilder<string> MatchesPhoneNumber(in this FilterBuilder<string> filter) =>
        filter.Satisfies(s =>
            System.Text.RegularExpressions.Regex.IsMatch(s, @"^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$"));

    /// <summary>
    /// Filters string values that match an IPv4 address format.
    /// </summary>
    /// <param name="filter">The filter builder to apply the condition to.</param>
    /// <returns>A filter builder that continues the fluent chain.</returns>
    /// <remarks>
    /// Validates IPv4 addresses in the format XXX.XXX.XXX.XXX.
    /// </remarks>
    public static FilterBuilder<string> MatchesIpAddress(in this FilterBuilder<string> filter) =>
        filter.Satisfies(s =>
            System.Text.RegularExpressions.Regex.IsMatch(s, @"^(\d{1,3}\.){3}\d{1,3}$"));

    /// <summary>
    /// Filters string values that match a GUID format.
    /// </summary>
    /// <param name="filter">The filter builder to apply the condition to.</param>
    /// <returns>A filter builder that continues the fluent chain.</returns>
    /// <remarks>
    /// Supports GUIDs with or without surrounding braces.
    /// </remarks>
    public static FilterBuilder<string> MatchesGuid(in this FilterBuilder<string> filter) =>
        filter.Satisfies(s =>
            System.Text.RegularExpressions.Regex.IsMatch(s,
                @"^(\{){0,1}[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}(\}){0,1}$"));
}
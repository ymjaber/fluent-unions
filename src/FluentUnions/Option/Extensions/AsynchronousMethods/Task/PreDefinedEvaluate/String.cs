using System.Text.RegularExpressions;

namespace FluentUnions;

/// <summary>Provides pre-defined filter methods for string values in <see cref="Option{T}"/> types using Task-based asynchronous evaluation.</summary>
public static partial class FilterBuilderExtensions
{
    public static async Task<bool> Empty(this Task<FilterBuilder<string>> filter)
        => (await filter.ConfigureAwait(false)).Empty();

    public static async Task<FilterBuilder<string>> NotEmpty(this Task<FilterBuilder<string>> filter)
        => (await filter.ConfigureAwait(false)).NotEmpty();

    public static async Task<FilterBuilder<string>> HasLength(this Task<FilterBuilder<string>> filter, int length)
        => (await filter.ConfigureAwait(false)).HasLength(length);

    public static async Task<FilterBuilder<string>> LongerThan(this Task<FilterBuilder<string>> filter, int length)
        => (await filter.ConfigureAwait(false)).LongerThan(length);

    public static async Task<FilterBuilder<string>> LongerThanOrEqualTo(this Task<FilterBuilder<string>> filter,
        int length)
        => (await filter.ConfigureAwait(false)).LongerThanOrEqualTo(length);

    public static async Task<FilterBuilder<string>> ShorterThan(this Task<FilterBuilder<string>> filter, int length)
        => (await filter.ConfigureAwait(false)).ShorterThan(length);

    public static async Task<FilterBuilder<string>> ShorterThanOrEqualTo(this Task<FilterBuilder<string>> filter,
        int length)
        => (await filter.ConfigureAwait(false)).ShorterThanOrEqualTo(length);

    public static async Task<FilterBuilder<string>> Matches(this Task<FilterBuilder<string>> filter, Regex pattern)
        => (await filter.ConfigureAwait(false)).Matches(pattern);

    public static async Task<FilterBuilder<string>> NotMatch(this Task<FilterBuilder<string>> filter, Regex pattern)
        => (await filter.ConfigureAwait(false)).NotMatch(pattern);

    public static async Task<FilterBuilder<string>> Contains(this Task<FilterBuilder<string>> filter, string substring)
        => (await filter.ConfigureAwait(false)).Contains(substring);

    public static async Task<FilterBuilder<string>> NotContains(this Task<FilterBuilder<string>> filter, string substring)
        => (await filter.ConfigureAwait(false)).NotContains(substring);

    public static async Task<FilterBuilder<string>> StartsWith(this Task<FilterBuilder<string>> filter, string substring)
        => (await filter.ConfigureAwait(false)).StartsWith(substring);

    public static async Task<FilterBuilder<string>> NotStartsWith(
        this Task<FilterBuilder<string>> filter,
        string substring)
        => (await filter.ConfigureAwait(false)).NotStartsWith(substring);

    public static async Task<FilterBuilder<string>> EndsWith(this Task<FilterBuilder<string>> filter, string substring)
        => (await filter.ConfigureAwait(false)).EndsWith(substring);

    public static async Task<FilterBuilder<string>> NotEndsWith(this Task<FilterBuilder<string>> filter, string substring)
        => (await filter.ConfigureAwait(false)).NotEndsWith(substring);

    public static async Task<FilterBuilder<string>> MatchesEmail(this Task<FilterBuilder<string>> filter)
        => (await filter.ConfigureAwait(false)).MatchesEmail();

    public static async Task<FilterBuilder<string>> MatchesUrl(this Task<FilterBuilder<string>> filter)
        => (await filter.ConfigureAwait(false)).MatchesUrl();

    public static async Task<FilterBuilder<string>> MatchesPhoneNumber(this Task<FilterBuilder<string>> filter)
        => (await filter.ConfigureAwait(false)).MatchesPhoneNumber();

    public static async Task<FilterBuilder<string>> MatchesIpAddress(this Task<FilterBuilder<string>> filter)
        => (await filter.ConfigureAwait(false)).MatchesIpAddress();

    public static async Task<FilterBuilder<string>> MatchesGuid(this Task<FilterBuilder<string>> filter)
        => (await filter.ConfigureAwait(false)).MatchesGuid();
}
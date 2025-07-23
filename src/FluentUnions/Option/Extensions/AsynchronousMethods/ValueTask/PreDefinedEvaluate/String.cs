using System.Text.RegularExpressions;

namespace FluentUnions;

/// <summary>Provides pre-defined filter methods for string values in <see cref="Option{T}"/> types using ValueTask-based asynchronous evaluation.</summary>
public static partial class FilterBuilderExtensions
{
    public static async ValueTask<bool> Empty(this ValueTask<FilterBuilder<string>> filter)
        => (await filter.ConfigureAwait(false)).Empty();

    public static async ValueTask<FilterBuilder<string>> NotEmpty(this ValueTask<FilterBuilder<string>> filter)
        => (await filter.ConfigureAwait(false)).NotEmpty();

    public static async ValueTask<FilterBuilder<string>> HasLength(this ValueTask<FilterBuilder<string>> filter, int length)
        => (await filter.ConfigureAwait(false)).HasLength(length);

    public static async ValueTask<FilterBuilder<string>> LongerThan(this ValueTask<FilterBuilder<string>> filter, int length)
        => (await filter.ConfigureAwait(false)).LongerThan(length);

    public static async ValueTask<FilterBuilder<string>> LongerThanOrEqualTo(this ValueTask<FilterBuilder<string>> filter,
        int length)
        => (await filter.ConfigureAwait(false)).LongerThanOrEqualTo(length);

    public static async ValueTask<FilterBuilder<string>> ShorterThan(this ValueTask<FilterBuilder<string>> filter, int length)
        => (await filter.ConfigureAwait(false)).ShorterThan(length);

    public static async ValueTask<FilterBuilder<string>> ShorterThanOrEqualTo(this ValueTask<FilterBuilder<string>> filter,
        int length)
        => (await filter.ConfigureAwait(false)).ShorterThanOrEqualTo(length);

    public static async ValueTask<FilterBuilder<string>> Matches(this ValueTask<FilterBuilder<string>> filter, Regex pattern)
        => (await filter.ConfigureAwait(false)).Matches(pattern);

    public static async ValueTask<FilterBuilder<string>> NotMatch(this ValueTask<FilterBuilder<string>> filter, Regex pattern)
        => (await filter.ConfigureAwait(false)).NotMatch(pattern);

    public static async ValueTask<FilterBuilder<string>> Contains(this ValueTask<FilterBuilder<string>> filter, string substring)
        => (await filter.ConfigureAwait(false)).Contains(substring);

    public static async ValueTask<FilterBuilder<string>> NotContains(this ValueTask<FilterBuilder<string>> filter, string substring)
        => (await filter.ConfigureAwait(false)).NotContains(substring);

    public static async ValueTask<FilterBuilder<string>> StartsWith(this ValueTask<FilterBuilder<string>> filter, string substring)
        => (await filter.ConfigureAwait(false)).StartsWith(substring);

    public static async ValueTask<FilterBuilder<string>> NotStartsWith(
        this ValueTask<FilterBuilder<string>> filter,
        string substring)
        => (await filter.ConfigureAwait(false)).NotStartsWith(substring);

    public static async ValueTask<FilterBuilder<string>> EndsWith(this ValueTask<FilterBuilder<string>> filter, string substring)
        => (await filter.ConfigureAwait(false)).EndsWith(substring);

    public static async ValueTask<FilterBuilder<string>> NotEndsWith(this ValueTask<FilterBuilder<string>> filter, string substring)
        => (await filter.ConfigureAwait(false)).NotEndsWith(substring);

    public static async ValueTask<FilterBuilder<string>> MatchesEmail(this ValueTask<FilterBuilder<string>> filter)
        => (await filter.ConfigureAwait(false)).MatchesEmail();

    public static async ValueTask<FilterBuilder<string>> MatchesUrl(this ValueTask<FilterBuilder<string>> filter)
        => (await filter.ConfigureAwait(false)).MatchesUrl();

    public static async ValueTask<FilterBuilder<string>> MatchesPhoneNumber(this ValueTask<FilterBuilder<string>> filter)
        => (await filter.ConfigureAwait(false)).MatchesPhoneNumber();

    public static async ValueTask<FilterBuilder<string>> MatchesIpAddress(this ValueTask<FilterBuilder<string>> filter)
        => (await filter.ConfigureAwait(false)).MatchesIpAddress();

    public static async ValueTask<FilterBuilder<string>> MatchesGuid(this ValueTask<FilterBuilder<string>> filter)
        => (await filter.ConfigureAwait(false)).MatchesGuid();
}
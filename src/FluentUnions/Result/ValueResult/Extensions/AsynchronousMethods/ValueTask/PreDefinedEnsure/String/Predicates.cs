using System.Text.RegularExpressions;

namespace FluentUnions;

/// <summary>Provides pre-defined validation predicates for string values in <see cref="Result{T}"/> types using ValueTask-based asynchronous validation.</summary>
public static partial class EnsureBuilderExtensions
{
    public static async ValueTask<Result> Empty(
        this ValueTask<EnsureBuilder<string>> ensure,
        Error? error = null)
        => (await ensure.ConfigureAwait(false)).Empty(error);

    public static async ValueTask<EnsureBuilder<string>> NotEmpty(
        this ValueTask<EnsureBuilder<string>> ensure,
        Error? error = null)
        => (await ensure.ConfigureAwait(false)).NotEmpty(error);

    public static async ValueTask<EnsureBuilder<string>> HasLength(
        this ValueTask<EnsureBuilder<string>> ensure,
        int length,
        Error? error = null)
        => (await ensure.ConfigureAwait(false)).HasLength(length, error);

    public static async ValueTask<EnsureBuilder<string>> LongerThan(
        this ValueTask<EnsureBuilder<string>> ensure,
        int length,
        Error? error = null)
        => (await ensure.ConfigureAwait(false)).LongerThan(length, error);

    public static async ValueTask<EnsureBuilder<string>> LongerThanOrEqualTo(
        this ValueTask<EnsureBuilder<string>> ensure,
        int length,
        Error? error = null)
        => (await ensure.ConfigureAwait(false)).LongerThanOrEqualTo(length, error);

    public static async ValueTask<EnsureBuilder<string>> ShorterThan(
        this ValueTask<EnsureBuilder<string>> ensure,
        int length,
        Error? error = null)
        => (await ensure.ConfigureAwait(false)).ShorterThan(length, error);

    public static async ValueTask<EnsureBuilder<string>> ShorterThanOrEqualTo(
        this ValueTask<EnsureBuilder<string>> ensure,
        int length,
        Error? error = null)
        => (await ensure.ConfigureAwait(false)).ShorterThanOrEqualTo(length, error);

    public static async ValueTask<EnsureBuilder<string>> Matches(
        this ValueTask<EnsureBuilder<string>> ensure,
        Regex pattern,
        Error? error = null)
        => (await ensure.ConfigureAwait(false)).Matches(pattern, error);

    public static async ValueTask<EnsureBuilder<string>> NotMatch(
        this ValueTask<EnsureBuilder<string>> ensure,
        Regex pattern,
        Error? error = null)
        => (await ensure.ConfigureAwait(false)).NotMatch(pattern, error);

    public static async ValueTask<EnsureBuilder<string>> Contains(
        this ValueTask<EnsureBuilder<string>> ensure,
        string substring,
        Error? error = null)
        => (await ensure.ConfigureAwait(false)).Contains(substring, error);

    public static async ValueTask<EnsureBuilder<string>> NotContain(
        this ValueTask<EnsureBuilder<string>> ensure,
        string substring,
        Error? error = null)
        => (await ensure.ConfigureAwait(false)).NotContain(substring, error);

    public static async ValueTask<EnsureBuilder<string>> StartsWith(
        this ValueTask<EnsureBuilder<string>> ensure,
        string substring,
        Error? error = null)
        => (await ensure.ConfigureAwait(false)).StartsWith(substring, error);

    public static async ValueTask<EnsureBuilder<string>> NotStartWith(
        this ValueTask<EnsureBuilder<string>> ensure,
        string substring,
        Error? error = null)
        => (await ensure.ConfigureAwait(false)).NotStartWith(substring, error);

    public static async ValueTask<EnsureBuilder<string>> EndsWith(
        this ValueTask<EnsureBuilder<string>> ensure,
        string substring,
        Error? error = null)
        => (await ensure.ConfigureAwait(false)).EndsWith(substring, error);

    public static async ValueTask<EnsureBuilder<string>> NotEndWith(
        this ValueTask<EnsureBuilder<string>> ensure,
        string substring,
        Error? error = null)
        => (await ensure.ConfigureAwait(false)).NotEndWith(substring, error);

    public static async ValueTask<EnsureBuilder<string>> MatchesEmail(
        this ValueTask<EnsureBuilder<string>> ensure,
        Error? error = null)
        => (await ensure.ConfigureAwait(false)).MatchesEmail(error);

    public static async ValueTask<EnsureBuilder<string>> MatchesUrl(
        this ValueTask<EnsureBuilder<string>> ensure,
        Error? error = null)
        => (await ensure.ConfigureAwait(false)).MatchesUrl(error);

    public static async ValueTask<EnsureBuilder<string>> MatchesPhoneNumber(
        this ValueTask<EnsureBuilder<string>> ensure,
        Error? error = null)
        => (await ensure.ConfigureAwait(false)).MatchesPhoneNumber(error);

    public static async ValueTask<EnsureBuilder<string>> MatchesIpAddress(
        this ValueTask<EnsureBuilder<string>> ensure,
        Error? error = null)
        => (await ensure.ConfigureAwait(false)).MatchesIpAddress(error);

    public static async ValueTask<EnsureBuilder<string>> MatchesGuid(
        this ValueTask<EnsureBuilder<string>> ensure,
        Error? error = null)
        => (await ensure.ConfigureAwait(false)).MatchesGuid(error);
}
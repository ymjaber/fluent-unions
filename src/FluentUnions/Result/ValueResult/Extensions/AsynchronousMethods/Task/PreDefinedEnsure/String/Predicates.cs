using System.Text.RegularExpressions;

namespace FluentUnions;

/// <summary>Provides pre-defined validation predicates for string values in <see cref="Result{T}"/> types using Task-based asynchronous validation.</summary>
public static partial class EnsureBuilderExtensions
{
    public static async Task<Result> Empty(
        this Task<EnsureBuilder<string>> ensure,
        Error? error = null)
        => (await ensure.ConfigureAwait(false)).Empty(error);

    public static async Task<EnsureBuilder<string>> NotEmpty(
        this Task<EnsureBuilder<string>> ensure,
        Error? error = null)
        => (await ensure.ConfigureAwait(false)).NotEmpty(error);

    public static async Task<EnsureBuilder<string>> HasLength(
        this Task<EnsureBuilder<string>> ensure,
        int length,
        Error? error = null)
        => (await ensure.ConfigureAwait(false)).HasLength(length, error);

    public static async Task<EnsureBuilder<string>> LongerThan(
        this Task<EnsureBuilder<string>> ensure,
        int length,
        Error? error = null)
        => (await ensure.ConfigureAwait(false)).LongerThan(length, error);

    public static async Task<EnsureBuilder<string>> LongerThanOrEqualTo(
        this Task<EnsureBuilder<string>> ensure,
        int length,
        Error? error = null)
        => (await ensure.ConfigureAwait(false)).LongerThanOrEqualTo(length, error);

    public static async Task<EnsureBuilder<string>> ShorterThan(
        this Task<EnsureBuilder<string>> ensure,
        int length,
        Error? error = null)
        => (await ensure.ConfigureAwait(false)).ShorterThan(length, error);

    public static async Task<EnsureBuilder<string>> ShorterThanOrEqualTo(
        this Task<EnsureBuilder<string>> ensure,
        int length,
        Error? error = null)
        => (await ensure.ConfigureAwait(false)).ShorterThanOrEqualTo(length, error);

    public static async Task<EnsureBuilder<string>> Matches(
        this Task<EnsureBuilder<string>> ensure,
        Regex pattern,
        Error? error = null)
        => (await ensure.ConfigureAwait(false)).Matches(pattern, error);

    public static async Task<EnsureBuilder<string>> NotMatch(
        this Task<EnsureBuilder<string>> ensure,
        Regex pattern,
        Error? error = null)
        => (await ensure.ConfigureAwait(false)).NotMatch(pattern, error);

    public static async Task<EnsureBuilder<string>> Contains(
        this Task<EnsureBuilder<string>> ensure,
        string substring,
        Error? error = null)
        => (await ensure.ConfigureAwait(false)).Contains(substring, error);

    public static async Task<EnsureBuilder<string>> NotContain(
        this Task<EnsureBuilder<string>> ensure,
        string substring,
        Error? error = null)
        => (await ensure.ConfigureAwait(false)).NotContain(substring, error);

    public static async Task<EnsureBuilder<string>> StartsWith(
        this Task<EnsureBuilder<string>> ensure,
        string substring,
        Error? error = null)
        => (await ensure.ConfigureAwait(false)).StartsWith(substring, error);

    public static async Task<EnsureBuilder<string>> NotStartWith(
        this Task<EnsureBuilder<string>> ensure,
        string substring,
        Error? error = null)
        => (await ensure.ConfigureAwait(false)).NotStartWith(substring, error);

    public static async Task<EnsureBuilder<string>> EndsWith(
        this Task<EnsureBuilder<string>> ensure,
        string substring,
        Error? error = null)
        => (await ensure.ConfigureAwait(false)).EndsWith(substring, error);

    public static async Task<EnsureBuilder<string>> NotEndWith(
        this Task<EnsureBuilder<string>> ensure,
        string substring,
        Error? error = null)
        => (await ensure.ConfigureAwait(false)).NotEndWith(substring, error);

    public static async Task<EnsureBuilder<string>> MatchesEmail(
        this Task<EnsureBuilder<string>> ensure,
        Error? error = null)
        => (await ensure.ConfigureAwait(false)).MatchesEmail(error);

    public static async Task<EnsureBuilder<string>> MatchesUrl(
        this Task<EnsureBuilder<string>> ensure,
        Error? error = null)
        => (await ensure.ConfigureAwait(false)).MatchesUrl(error);

    public static async Task<EnsureBuilder<string>> MatchesPhoneNumber(
        this Task<EnsureBuilder<string>> ensure,
        Error? error = null)
        => (await ensure.ConfigureAwait(false)).MatchesPhoneNumber(error);

    public static async Task<EnsureBuilder<string>> MatchesIpAddress(
        this Task<EnsureBuilder<string>> ensure,
        Error? error = null)
        => (await ensure.ConfigureAwait(false)).MatchesIpAddress(error);

    public static async Task<EnsureBuilder<string>> MatchesGuid(
        this Task<EnsureBuilder<string>> ensure,
        Error? error = null)
        => (await ensure.ConfigureAwait(false)).MatchesGuid(error);
}
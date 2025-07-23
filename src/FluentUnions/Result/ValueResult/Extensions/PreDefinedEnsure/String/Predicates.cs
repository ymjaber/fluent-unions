using System.Text.RegularExpressions;
using FluentUnions.PreDefinedEnsure;

namespace FluentUnions;

/// <summary>Provides pre-defined validation predicates for string values in <see cref="Result{T}"/> types.</summary>
public static partial class EnsureBuilderExtensions
{
    /// <summary>
    /// Ensures that the string is empty.
    /// </summary>
    /// <param name="ensure">The ensure builder for the string value.</param>
    /// <param name="error">Optional custom error. If null, a default error message will be used.</param>
    /// <returns>A unit result indicating success if the string is empty; otherwise, a failure result.</returns>
    public static Result Empty(
        in this EnsureBuilder<string> ensure,
        Error? error = null) =>
        ensure.Satisfies(s => s == string.Empty, error ?? StringErrors.NotEmpty).Build().DiscardValue();

    /// <summary>
    /// Ensures that the string is not empty.
    /// </summary>
    /// <param name="ensure">The ensure builder for the string value.</param>
    /// <param name="error">Optional custom error. If null, a default error message will be used.</param>
    /// <returns>The ensure builder for method chaining.</returns>
    public static EnsureBuilder<string> NotEmpty(
        in this EnsureBuilder<string> ensure,
        Error? error = null) =>
        ensure.Satisfies(s => s != string.Empty, error ?? StringErrors.Empty);

    /// <summary>
    /// Ensures that the string has exactly the specified length.
    /// </summary>
    /// <param name="ensure">The ensure builder for the string value.</param>
    /// <param name="length">The required length.</param>
    /// <param name="error">Optional custom error. If null, a default error message will be used.</param>
    /// <returns>The ensure builder for method chaining.</returns>
    public static EnsureBuilder<string> HasLength(
        in this EnsureBuilder<string> ensure,
        int length,
        Error? error = null) =>
        ensure.Satisfies(s => s.Length == length, error ?? StringErrors.InvalidLength(length));

    /// <summary>
    /// Ensures that the string is longer than the specified length.
    /// </summary>
    /// <param name="ensure">The ensure builder for the string value.</param>
    /// <param name="length">The minimum length (exclusive).</param>
    /// <param name="error">Optional custom error. If null, a default error message will be used.</param>
    /// <returns>The ensure builder for method chaining.</returns>
    public static EnsureBuilder<string> LongerThan(
        in this EnsureBuilder<string> ensure,
        int length,
        Error? error = null) =>
        ensure.Satisfies(s => s.Length > length, error ?? StringErrors.TooShort(length, false));

    /// <summary>
    /// Ensures that the string is longer than or equal to the specified length.
    /// </summary>
    /// <param name="ensure">The ensure builder for the string value.</param>
    /// <param name="length">The minimum length (inclusive).</param>
    /// <param name="error">Optional custom error. If null, a default error message will be used.</param>
    /// <returns>The ensure builder for method chaining.</returns>
    public static EnsureBuilder<string> LongerThanOrEqualTo(
        in this EnsureBuilder<string> ensure,
        int length,
        Error? error = null) =>
        ensure.Satisfies(s => s.Length >= length, error ?? StringErrors.TooShort(length, true));

    /// <summary>
    /// Ensures that the string is shorter than the specified length.
    /// </summary>
    /// <param name="ensure">The ensure builder for the string value.</param>
    /// <param name="length">The maximum length (exclusive).</param>
    /// <param name="error">Optional custom error. If null, a default error message will be used.</param>
    /// <returns>The ensure builder for method chaining.</returns>
    public static EnsureBuilder<string> ShorterThan(
        in this EnsureBuilder<string> ensure,
        int length,
        Error? error = null) =>
        ensure.Satisfies(s => s.Length < length, error ?? StringErrors.TooLong(length, false));

    /// <summary>
    /// Ensures that the string is shorter than or equal to the specified length.
    /// </summary>
    /// <param name="ensure">The ensure builder for the string value.</param>
    /// <param name="length">The maximum length (inclusive).</param>
    /// <param name="error">Optional custom error. If null, a default error message will be used.</param>
    /// <returns>The ensure builder for method chaining.</returns>
    public static EnsureBuilder<string> ShorterThanOrEqualTo(
        in this EnsureBuilder<string> ensure,
        int length,
        Error? error = null) =>
        ensure.Satisfies(s => s.Length <= length, error ?? StringErrors.TooLong(length, true));

    /// <summary>
    /// Ensures that the string matches the specified regular expression pattern.
    /// </summary>
    /// <param name="ensure">The ensure builder for the string value.</param>
    /// <param name="pattern">The regular expression pattern to match.</param>
    /// <param name="error">Optional custom error. If null, a default error message will be used.</param>
    /// <returns>The ensure builder for method chaining.</returns>
    public static EnsureBuilder<string> Matches(
        in this EnsureBuilder<string> ensure,
        Regex pattern,
        Error? error = null) =>
        ensure.Satisfies(
            pattern.IsMatch,
            error ?? StringErrors.NotMatch(pattern.ToString()));

    /// <summary>
    /// Ensures that the string does not match the specified regular expression pattern.
    /// </summary>
    /// <param name="ensure">The ensure builder for the string value.</param>
    /// <param name="pattern">The regular expression pattern that should not match.</param>
    /// <param name="error">Optional custom error. If null, a default error message will be used.</param>
    /// <returns>The ensure builder for method chaining.</returns>
    public static EnsureBuilder<string> NotMatch(
        in this EnsureBuilder<string> ensure,
        Regex pattern,
        Error? error = null) =>
        ensure.Satisfies(
            s => !pattern.IsMatch(s),
            error ?? StringErrors.Match(pattern.ToString()));

    /// <summary>
    /// Ensures that the string contains the specified substring.
    /// </summary>
    /// <param name="ensure">The ensure builder for the string value.</param>
    /// <param name="substring">The substring that must be present.</param>
    /// <param name="error">Optional custom error. If null, a default error message will be used.</param>
    /// <returns>The ensure builder for method chaining.</returns>
    public static EnsureBuilder<string> Contains(
        in this EnsureBuilder<string> ensure,
        string substring,
        Error? error = null) =>
        ensure.Satisfies(s => s.Contains(substring), error ?? StringErrors.NotContain(substring));

    /// <summary>
    /// Ensures that the string does not contain the specified substring.
    /// </summary>
    /// <param name="ensure">The ensure builder for the string value.</param>
    /// <param name="substring">The substring that must not be present.</param>
    /// <param name="error">Optional custom error. If null, a default error message will be used.</param>
    /// <returns>The ensure builder for method chaining.</returns>
    public static EnsureBuilder<string> NotContain(
        in this EnsureBuilder<string> ensure,
        string substring,
        Error? error = null) =>
        ensure.Satisfies(s => !s.Contains(substring), error ?? StringErrors.Contain(substring));

    /// <summary>
    /// Ensures that the string starts with the specified substring.
    /// </summary>
    /// <param name="ensure">The ensure builder for the string value.</param>
    /// <param name="substring">The substring that must be at the beginning.</param>
    /// <param name="error">Optional custom error. If null, a default error message will be used.</param>
    /// <returns>The ensure builder for method chaining.</returns>
    public static EnsureBuilder<string> StartsWith(
        in this EnsureBuilder<string> ensure,
        string substring,
        Error? error = null) =>
        ensure.Satisfies(s => s.StartsWith(substring), error ?? StringErrors.NotStartWith(substring));

    /// <summary>
    /// Ensures that the string does not start with the specified substring.
    /// </summary>
    /// <param name="ensure">The ensure builder for the string value.</param>
    /// <param name="substring">The substring that must not be at the beginning.</param>
    /// <param name="error">Optional custom error. If null, a default error message will be used.</param>
    /// <returns>The ensure builder for method chaining.</returns>
    public static EnsureBuilder<string> NotStartWith(
        in this EnsureBuilder<string> ensure,
        string substring,
        Error? error = null) =>
        ensure.Satisfies(s => !s.StartsWith(substring), error ?? StringErrors.StartWith(substring));

    /// <summary>
    /// Ensures that the string ends with the specified substring.
    /// </summary>
    /// <param name="ensure">The ensure builder for the string value.</param>
    /// <param name="substring">The substring that must be at the end.</param>
    /// <param name="error">Optional custom error. If null, a default error message will be used.</param>
    /// <returns>The ensure builder for method chaining.</returns>
    public static EnsureBuilder<string> EndsWith(
        in this EnsureBuilder<string> ensure,
        string substring,
        Error? error = null) =>
        ensure.Satisfies(s => s.EndsWith(substring), error ?? StringErrors.NotEndWith(substring));

    /// <summary>
    /// Ensures that the string does not end with the specified substring.
    /// </summary>
    /// <param name="ensure">The ensure builder for the string value.</param>
    /// <param name="substring">The substring that must not be at the end.</param>
    /// <param name="error">Optional custom error. If null, a default error message will be used.</param>
    /// <returns>The ensure builder for method chaining.</returns>
    public static EnsureBuilder<string> NotEndWith(
        in this EnsureBuilder<string> ensure,
        string substring,
        Error? error = null) =>
        ensure.Satisfies(s => !s.EndsWith(substring), error ?? StringErrors.EndWith(substring));

    /// <summary>
    /// Ensures that the string is a valid email address format.
    /// </summary>
    /// <param name="ensure">The ensure builder for the string value.</param>
    /// <param name="error">Optional custom error. If null, a default error message will be used.</param>
    /// <returns>The ensure builder for method chaining.</returns>
    /// <remarks>
    /// Uses a basic email validation pattern. For production use, consider more comprehensive validation.
    /// </remarks>
    public static EnsureBuilder<string> MatchesEmail(
        in this EnsureBuilder<string> ensure,
        Error? error = null) =>
        ensure.Satisfies(
            s => System.Text.RegularExpressions.Regex.IsMatch(s, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"),
            error ?? StringErrors.NotEmail);

    /// <summary>
    /// Ensures that the string is a valid URL format.
    /// </summary>
    /// <param name="ensure">The ensure builder for the string value.</param>
    /// <param name="error">Optional custom error. If null, a default error message will be used.</param>
    /// <returns>The ensure builder for method chaining.</returns>
    /// <remarks>
    /// Validates HTTP and HTTPS URLs. For more specific URL validation, use custom patterns.
    /// </remarks>
    public static EnsureBuilder<string> MatchesUrl(
        in this EnsureBuilder<string> ensure,
        Error? error = null) =>
        ensure.Satisfies(
            s => System.Text.RegularExpressions.Regex.IsMatch(s, @"^http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$"),
            error ?? StringErrors.NotUrl);

    /// <summary>
    /// Ensures that the string is a valid phone number format.
    /// </summary>
    /// <param name="ensure">The ensure builder for the string value.</param>
    /// <param name="error">Optional custom error. If null, a default error message will be used.</param>
    /// <returns>The ensure builder for method chaining.</returns>
    /// <remarks>
    /// Uses a flexible pattern that accepts various international phone number formats.
    /// </remarks>
    public static EnsureBuilder<string> MatchesPhoneNumber(
        in this EnsureBuilder<string> ensure,
        Error? error = null) =>
        ensure.Satisfies(
            s => System.Text.RegularExpressions.Regex.IsMatch(s, @"^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$"),
            error ?? StringErrors.NotPhoneNumber);

    /// <summary>
    /// Ensures that the string is a valid IPv4 address format.
    /// </summary>
    /// <param name="ensure">The ensure builder for the string value.</param>
    /// <param name="error">Optional custom error. If null, a default error message will be used.</param>
    /// <returns>The ensure builder for method chaining.</returns>
    /// <remarks>
    /// This validates the format only, not whether the IP address is valid or reachable.
    /// </remarks>
    public static EnsureBuilder<string> MatchesIpAddress(
        in this EnsureBuilder<string> ensure,
        Error? error = null) =>
        ensure.Satisfies(
            s => System.Text.RegularExpressions.Regex.IsMatch(s, @"^(\d{1,3}\.){3}\d{1,3}$"),
            error ?? StringErrors.NotIpAddress);

    /// <summary>
    /// Ensures that the string is a valid GUID format.
    /// </summary>
    /// <param name="ensure">The ensure builder for the string value.</param>
    /// <param name="error">Optional custom error. If null, a default error message will be used.</param>
    /// <returns>The ensure builder for method chaining.</returns>
    /// <remarks>
    /// Accepts GUIDs with or without braces in standard format.
    /// </remarks>
    public static EnsureBuilder<string> MatchesGuid(
        in this EnsureBuilder<string> ensure,
        Error? error = null) =>
        ensure.Satisfies(
            s => System.Text.RegularExpressions.Regex.IsMatch(s,
                @"^(\{){0,1}[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}(\}){0,1}$"),
            error ?? StringErrors.NotGuid);
}
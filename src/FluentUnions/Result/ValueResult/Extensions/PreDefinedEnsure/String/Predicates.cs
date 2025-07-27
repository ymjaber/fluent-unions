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
        ensure.Satisfies(s => s == string.Empty, error ?? StringErrors.NotEmpty).DiscardValue();

    /// <summary>
    /// Ensures that the string is not empty.
    /// </summary>
    /// <param name="ensure">The ensure builder for the string value.</param>
    /// <param name="error">Optional custom error. If null, a default error message will be used.</param>
    /// <returns>A result that validates the string is empty.</returns>
    public static Result<string> NotEmpty(
        in this EnsureBuilder<string> ensure,
        Error? error = null) =>
        ensure.Satisfies(s => s != string.Empty, error ?? StringErrors.Empty);

    /// <summary>
    /// Ensures that the string value is not null or empty.
    /// </summary>
    /// <param name="ensure">The ensure builder for the value.</param>
    /// <param name="error">Optional custom error. If null, a default error message will be used.</param>
    /// <returns>A success result containing the string if it's not null or empty; otherwise, a failure result with the specified error.</returns>
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
    public static Result<string> NotNullOrEmpty(
            in this EnsureBuilder<string?> ensure,
            Error error = null!) =>
        ensure.Satisfies(s => !string.IsNullOrEmpty(s), error ?? StringErrors.Empty);
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.

    /// <summary>
    /// Ensures that the string value is not null, empty, or consists only of whitespace characters.
    /// </summary>
    /// <param name="ensure">The ensure builder for the value.</param>
    /// <param name="error">Optional custom error. If null, a default error message will be used.</param>
    /// <returns>A success result containing the string if it's not null, empty, or whitespace; otherwise, a failure result with the specified error.</returns>
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
    public static Result<string> NotNullOrWhiteSpace(
            in this EnsureBuilder<string?> ensure,
            Error error = null!) =>
        ensure.Satisfies(s => !string.IsNullOrWhiteSpace(s), error ?? StringErrors.Empty);
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.


    /// <summary>
    /// Ensures that the string has exactly the specified length.
    /// </summary>
    /// <param name="ensure">The ensure builder for the string value.</param>
    /// <param name="length">The required length.</param>
    /// <param name="error">Optional custom error. If null, a default error message will be used.</param>
    /// <returns>A result that validates the string has the length of the specified value.</returns>
    public static Result<string> HasLength(
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
    /// <returns>A result that validates the string is longer than the specified length.</returns>
    public static Result<string> LongerThan(
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
    /// <returns>A result that validates the string is longer than or equals to the specified length.</returns>
    public static Result<string> LongerThanOrEqualTo(
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
    /// <returns>A result that validates the string is shorter than the specified length.</returns>
    public static Result<string> ShorterThan(
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
    /// <returns>A result that validates the string is shorter than or equals to the specified length.</returns>
    public static Result<string> ShorterThanOrEqualTo(
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
    /// <returns>A result that validates the string mathces the pattern.</returns>
    public static Result<string> Matches(
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
    /// <returns>A result that validates the string doesn't match the pattern.</returns>
    public static Result<string> NotMatch(
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
    /// <returns>A result that validates the string contains the specified substring.</returns>
    public static Result<string> Contains(
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
    /// <returns>A result that validates the string doesn't contain the spacified substring.</returns>
    public static Result<string> NotContain(
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
    /// <returns>A result that validates the string starts wit the specified substring.</returns>
    public static Result<string> StartsWith(
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
    /// <returns>A result that validates the string doesn't start with the specified substring.</returns>
    public static Result<string> NotStartWith(
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
    /// <returns>A result that validates the string ends with the specified substring.</returns>
    public static Result<string> EndsWith(
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
    /// <returns>A result that validates the string doesn't end with the specified substring.</returns>
    public static Result<string> NotEndWith(
        in this EnsureBuilder<string> ensure,
        string substring,
        Error? error = null) =>
        ensure.Satisfies(s => !s.EndsWith(substring), error ?? StringErrors.EndWith(substring));
}

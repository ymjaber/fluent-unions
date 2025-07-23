namespace FluentUnions.PreDefinedEnsure;

/// <summary>
/// Provides pre-defined validation errors for string validation scenarios.
/// </summary>
/// <remarks>
/// This class contains static factory members and methods that create ValidationError instances
/// for comprehensive string validation failures. It covers common string validation scenarios
/// including length validation, pattern matching, content validation, and format validation
/// for specific string types like email addresses, URLs, and GUIDs.
/// </remarks>
public static class StringErrors
{
    /// <summary>
    /// Gets a validation error indicating that a string is not empty when it should be.
    /// </summary>
    /// <returns>
    /// A <see cref="ValidationError"/> with code "StringError.NotEmpty" and message "String must be empty."
    /// </returns>
    /// <remarks>
    /// Use this error when validation requires an empty string but a non-empty string was provided.
    /// </remarks>
    public static readonly ValidationError NotEmpty = new(
        "StringError.NotEmpty",
        "String must be empty.");

    /// <summary>
    /// Gets a validation error indicating that a string is empty when it should not be.
    /// </summary>
    /// <returns>
    /// A <see cref="ValidationError"/> with code "StringError.Empty" and message "String cannot be empty."
    /// </returns>
    /// <remarks>
    /// Use this error when validation requires a non-empty string but an empty string was provided.
    /// This is one of the most common string validation errors.
    /// </remarks>
    public static readonly ValidationError Empty = new(
        "StringError.Empty",
        "String cannot be empty.");

    /// <summary>
    /// Creates a validation error indicating that a string does not have the required exact length.
    /// </summary>
    /// <param name="length">The exact number of characters the string should have.</param>
    /// <returns>
    /// A <see cref="ValidationError"/> with code "StringError.InvalidLength" and a message indicating the required length.
    /// </returns>
    /// <remarks>
    /// Use this error when validation requires a string to have an exact number of characters.
    /// Common use cases include validation of fixed-length codes, postal codes, or identifiers.
    /// </remarks>
    public static ValidationError InvalidLength(int length) => new(
        "StringError.InvalidLength",
        $"String must have exactly {length} characters.");

    /// <summary>
    /// Creates a validation error indicating that a string is shorter than the minimum required length.
    /// </summary>
    /// <param name="length">The minimum length threshold.</param>
    /// <param name="inclusive">If true, the length must be at least the specified value; if false, it must be greater than the specified value.</param>
    /// <returns>
    /// A <see cref="ValidationError"/> with code "StringError.TooShort" and a message indicating the minimum length requirement.
    /// </returns>
    /// <remarks>
    /// Use this error when validation requires a string to meet a minimum length requirement.
    /// The inclusive parameter determines whether the exact length value satisfies the requirement.
    /// </remarks>
    public static ValidationError TooShort(int length, bool inclusive) => new(
        "StringError.TooShort",
        $"String must be {(inclusive ? "at least" : "longer than")} {length} characters.");

    /// <summary>
    /// Creates a validation error indicating that a string exceeds the maximum allowed length.
    /// </summary>
    /// <param name="length">The maximum length threshold.</param>
    /// <param name="inclusive">If true, the length must be at most the specified value; if false, it must be less than the specified value.</param>
    /// <returns>
    /// A <see cref="ValidationError"/> with code "StringError.TooLong" and a message indicating the maximum length requirement.
    /// </returns>
    /// <remarks>
    /// Use this error when validation requires a string to not exceed a maximum length.
    /// Common use cases include database field constraints or UI display limitations.
    /// </remarks>
    public static ValidationError TooLong(int length, bool inclusive) => new(
        "StringError.TooLong",
        $"String must be {(inclusive ? "at most" : "shorter than")} {length} characters.");

    /// <summary>
    /// Creates a validation error indicating that a string does not match the required pattern.
    /// </summary>
    /// <param name="pattern">The regular expression pattern the string should match.</param>
    /// <returns>
    /// A <see cref="ValidationError"/> with code "StringError.NotMatch" and a message indicating the required pattern.
    /// </returns>
    /// <remarks>
    /// Use this error when validation requires a string to match a specific regular expression pattern.
    /// The pattern parameter should be a valid regular expression.
    /// </remarks>
    public static ValidationError NotMatch(string pattern) => new(
        "StringError.NotMatch",
        $"String must match the pattern '{pattern}'.");

    /// <summary>
    /// Creates a validation error indicating that a string matches a forbidden pattern.
    /// </summary>
    /// <param name="pattern">The regular expression pattern the string should not match.</param>
    /// <returns>
    /// A <see cref="ValidationError"/> with code "StringError.Match" and a message indicating the forbidden pattern.
    /// </returns>
    /// <remarks>
    /// Use this error when validation requires a string to NOT match a specific pattern.
    /// Useful for blacklisting certain patterns or formats.
    /// </remarks>
    public static ValidationError Match(string pattern) => new(
        "StringError.Match",
        $"String cannot match the pattern '{pattern}'.");

    /// <summary>
    /// Creates a validation error indicating that a string does not contain the required substring.
    /// </summary>
    /// <param name="value">The substring that should be present in the string.</param>
    /// <returns>
    /// A <see cref="ValidationError"/> with code "StringError.NotContain" and a message indicating the required substring.
    /// </returns>
    /// <remarks>
    /// Use this error when validation requires a string to contain a specific substring.
    /// The search is typically case-sensitive unless otherwise specified.
    /// </remarks>
    public static ValidationError NotContain(string value) => new(
        "StringError.NotContain",
        $"String must contain '{value}'.");

    /// <summary>
    /// Creates a validation error indicating that a string contains a forbidden substring.
    /// </summary>
    /// <param name="value">The substring that should not be present in the string.</param>
    /// <returns>
    /// A <see cref="ValidationError"/> with code "StringError.Contain" and a message indicating the forbidden substring.
    /// </returns>
    /// <remarks>
    /// Use this error when validation requires a string to NOT contain a specific substring.
    /// Useful for preventing certain words, characters, or patterns within strings.
    /// </remarks>
    public static ValidationError Contain(string value) => new(
        "StringError.Contain",
        $"String cannot contain '{value}'.");

    /// <summary>
    /// Creates a validation error indicating that a string does not start with the required prefix.
    /// </summary>
    /// <param name="value">The prefix the string should start with.</param>
    /// <returns>
    /// A <see cref="ValidationError"/> with code "StringError.NotStartWith" and a message indicating the required prefix.
    /// </returns>
    /// <remarks>
    /// Use this error when validation requires a string to have a specific prefix.
    /// Common use cases include URL validation, code prefixes, or namespacing requirements.
    /// </remarks>
    public static ValidationError NotStartWith(string value) => new(
        "StringError.NotStartWith",
        $"String must start with '{value}'.");

    /// <summary>
    /// Creates a validation error indicating that a string starts with a forbidden prefix.
    /// </summary>
    /// <param name="value">The prefix the string should not start with.</param>
    /// <returns>
    /// A <see cref="ValidationError"/> with code "StringError.StartWith" and a message indicating the forbidden prefix.
    /// </returns>
    /// <remarks>
    /// Use this error when validation requires a string to NOT start with a specific prefix.
    /// Useful for security validations or format restrictions.
    /// </remarks>
    public static ValidationError StartWith(string value) => new(
        "StringError.StartWith",
        $"String cannot start with '{value}'.");

    /// <summary>
    /// Creates a validation error indicating that a string does not end with the required suffix.
    /// </summary>
    /// <param name="value">The suffix the string should end with.</param>
    /// <returns>
    /// A <see cref="ValidationError"/> with code "StringError.NotEndWith" and a message indicating the required suffix.
    /// </returns>
    /// <remarks>
    /// Use this error when validation requires a string to have a specific suffix.
    /// Common use cases include file extension validation or domain name requirements.
    /// </remarks>
    public static ValidationError NotEndWith(string value) => new(
        "StringError.NotEndWith",
        $"String must end with '{value}'.");

    /// <summary>
    /// Creates a validation error indicating that a string ends with a forbidden suffix.
    /// </summary>
    /// <param name="value">The suffix the string should not end with.</param>
    /// <returns>
    /// A <see cref="ValidationError"/> with code "StringError.EndWith" and a message indicating the forbidden suffix.
    /// </returns>
    /// <remarks>
    /// Use this error when validation requires a string to NOT end with a specific suffix.
    /// Useful for preventing certain file extensions or format restrictions.
    /// </remarks>
    public static ValidationError EndWith(string value) => new(
        "StringError.EndWith",
        $"String cannot end with '{value}'.");

    /// <summary>
    /// Gets a validation error indicating that a string is not a valid email address.
    /// </summary>
    /// <returns>
    /// A <see cref="ValidationError"/> with code "StringError.NotEmail" and message "String must be a valid email address."
    /// </returns>
    /// <remarks>
    /// Use this error when validation requires a string to be in a valid email format.
    /// The validation typically checks for basic email structure (local@domain.tld).
    /// </remarks>
    public static readonly ValidationError NotEmail = new(
        "StringError.NotEmail",
        "String must be a valid email address.");

    /// <summary>
    /// Gets a validation error indicating that a string is not a valid URL.
    /// </summary>
    /// <returns>
    /// A <see cref="ValidationError"/> with code "StringError.NotUrl" and message "String must be a valid URL."
    /// </returns>
    /// <remarks>
    /// Use this error when validation requires a string to be a properly formatted URL.
    /// The validation typically checks for valid URL structure including protocol, domain, and path.
    /// </remarks>
    public static readonly ValidationError NotUrl = new(
        "StringError.NotUrl",
        "String must be a valid URL.");

    /// <summary>
    /// Gets a validation error indicating that a string is not a valid IP address.
    /// </summary>
    /// <returns>
    /// A <see cref="ValidationError"/> with code "StringError.NotIp" and message "String must be a valid IP address."
    /// </returns>
    /// <remarks>
    /// Use this error when validation requires a string to be a valid IP address.
    /// This typically validates both IPv4 and IPv6 address formats.
    /// </remarks>
    public static readonly ValidationError NotIpAddress = new(
        "StringError.NotIp",
        "String must be a valid IP address.");

    /// <summary>
    /// Gets a validation error indicating that a string is not a valid GUID.
    /// </summary>
    /// <returns>
    /// A <see cref="ValidationError"/> with code "StringError.NotGuid" and message "String must be a valid GUID."
    /// </returns>
    /// <remarks>
    /// Use this error when validation requires a string to be a valid GUID/UUID format.
    /// The validation checks for standard GUID formats (with or without hyphens).
    /// </remarks>
    public static readonly ValidationError NotGuid = new(
        "StringError.NotGuid",
        "String must be a valid GUID.");

    /// <summary>
    /// Gets a validation error indicating that a string is not a valid phone number.
    /// </summary>
    /// <returns>
    /// A <see cref="ValidationError"/> with code "StringError.NotPhoneNumber" and message "String must be a valid phone number."
    /// </returns>
    /// <remarks>
    /// Use this error when validation requires a string to be a valid phone number format.
    /// The validation rules may vary based on regional formats and requirements.
    /// </remarks>
    public static readonly ValidationError NotPhoneNumber = new(
        "StringError.NotPhoneNumber",
        "String must be a valid phone number.");
}
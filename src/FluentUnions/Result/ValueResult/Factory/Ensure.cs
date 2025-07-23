using FluentUnions.PreDefinedEnsure;

namespace FluentUnions;

public readonly partial struct Result
{
    /// <summary>
    /// Creates an EnsureBuilder for fluently validating a value against multiple conditions.
    /// </summary>
    /// <typeparam name="TValue">The type of the value to validate.</typeparam>
    /// <param name="value">The value to validate.</param>
    /// <returns>An EnsureBuilder instance for chaining validation conditions.</returns>
    public static EnsureBuilder<TValue> EnsureThat<TValue>(TValue value) => new(value);

    /// <summary>
    /// Ensures that a reference type value is not null.
    /// </summary>
    /// <typeparam name="TValue">The reference type of the value to check.</typeparam>
    /// <param name="value">The nullable value to check.</param>
    /// <param name="error">The error to return if the value is null. If not provided, a default null error is used.</param>
    /// <returns>A success result containing the value if it's not null; otherwise, a failure result with the specified error.</returns>
    public static Result<TValue> EnsureNotNull<TValue>(TValue? value, Error error = null!)
        where TValue : class
    {
        if (value is null) return error ?? GeneralErrors.Null;

        return value;
    }

    /// <summary>
    /// Ensures that a nullable value type is not null.
    /// </summary>
    /// <typeparam name="TValue">The value type to check.</typeparam>
    /// <param name="value">The nullable value to check.</param>
    /// <param name="error">The error to return if the value is null. If not provided, a default null error is used.</param>
    /// <returns>A success result containing the unwrapped value if it's not null; otherwise, a failure result with the specified error.</returns>
    public static Result<TValue> EnsureNotNull<TValue>(TValue? value, Error error = null!)
        where TValue : struct
    {
        if (value is null) return error ?? GeneralErrors.Null;

        return value.Value;
    }

    /// <summary>
    /// Ensures that a string value is not null or empty.
    /// </summary>
    /// <param name="value">The string value to check.</param>
    /// <param name="error">The error to return if the value is null or empty. If not provided, appropriate default errors are used.</param>
    /// <returns>A success result containing the string if it's not null or empty; otherwise, a failure result with the specified error.</returns>
    public static Result<string> EnsureNotNullOrEmpty(string? value, Error error = null!)
    {
        if (value is null) return error ?? GeneralErrors.Null;
        if (string.IsNullOrEmpty(value)) return error ?? StringErrors.Empty;

        return value;
    }

    /// <summary>
    /// Ensures that a string value is not null, empty, or consists only of whitespace characters.
    /// </summary>
    /// <param name="value">The string value to check.</param>
    /// <param name="error">The error to return if the value is null, empty, or whitespace. If not provided, appropriate default errors are used.</param>
    /// <returns>A success result containing the string if it's not null, empty, or whitespace; otherwise, a failure result with the specified error.</returns>
    public static Result<string> EnsureNotNullOrWhiteSpace(string? value, Error error = null!)
    {
        if (value is null) return error ?? GeneralErrors.Null;
        if (string.IsNullOrWhiteSpace(value)) return error ?? StringErrors.Empty;

        return value;
    }

    /// <summary>
    /// Ensures that a value is null.
    /// </summary>
    /// <typeparam name="TValue">The type of the value to check.</typeparam>
    /// <param name="value">The value to check for null.</param>
    /// <param name="error">The error to return if the value is not null. If not provided, a default null error is used.</param>
    /// <returns>A success result if the value is null; otherwise, a failure result with the specified error.</returns>
    /// <remarks>
    /// This method is useful for validation scenarios where a value must be null, such as ensuring a field is not set.
    /// </remarks>
    public static Result Null<TValue>(
        TValue? value, Error? error = null)
    {
        if (value is not null) return error ?? GeneralErrors.Null;

        return Result.Success();
    }
}
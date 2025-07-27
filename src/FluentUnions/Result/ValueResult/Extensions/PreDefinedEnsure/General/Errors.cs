namespace FluentUnions.PreDefinedEnsure;

/// <summary>
/// Provides pre-defined validation errors for general-purpose validation scenarios.
/// </summary>
/// <remarks>
/// This class contains static factory members that create ValidationError instances
/// for common validation failures such as equality checks and null reference validation.
/// These errors can be used across different data types for basic validation requirements.
/// </remarks>
public class GeneralErrors
{
    /// <summary>
    /// Gets a validation error indicating that a value is not equal to the expected value.
    /// </summary>
    /// <returns>
    /// A <see cref="ValidationError"/> with code "Error.NotEqual" and message "Value must be equal to the expected value."
    /// </returns>
    /// <remarks>
    /// Use this error when validation requires two values to be equal but they are not.
    /// </remarks>
    public static readonly ValidationError NotEqual = new(
        "Error.NotEqual",
        "Value must be equal to the expected value.");

    /// <summary>
    /// Gets a validation error indicating that a value is equal to a forbidden value.
    /// </summary>
    /// <returns>
    /// A <see cref="ValidationError"/> with code "Error.Equal" and message "Value must not be equal to the expected value."
    /// </returns>
    /// <remarks>
    /// Use this error when validation requires two values to be different but they are equal.
    /// </remarks>
    public static readonly ValidationError Equal = new(
        "Error.Equal",
        "Value must not be equal to the expected value.");

    /// <summary>
    /// Gets a validation error indicating that a value is null when it should not be.
    /// </summary>
    /// <returns>
    /// A <see cref="ValidationError"/> with code "Error.Null" and message "Value cannot be null."
    /// </returns>
    /// <remarks>
    /// Use this error when validation requires a non-null value but null was provided.
    /// This is one of the most common validation errors for reference types.
    /// </remarks>
    public static readonly ValidationError Null = new(
        "Error.Null",
        "Value cannot be null.");

    /// <summary>
    /// Gets a validation error indicating that a value is not null when it should be.
    /// </summary>
    /// <returns>
    /// A <see cref="ValidationError"/> with code "Error.NotNull" and message "Value must be null."
    /// </returns>
    /// <remarks>
    /// Use this error when validation specifically requires a null value but a non-null value was provided.
    /// This is less common but useful for certain validation scenarios.
    /// </remarks>
    public static readonly ValidationError NotNull = new(
        "Error.NotNull",
        "Value must be null.");
}
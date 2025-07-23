namespace FluentUnions.PreDefinedEnsure;

/// <summary>
/// Provides pre-defined validation errors for boolean validation scenarios.
/// </summary>
/// <remarks>
/// This class contains static factory members that create ValidationError instances
/// for boolean validation failures. These errors are used when validating boolean values
/// to ensure they meet expected true/false state requirements.
/// </remarks>
public static class BooleanErrors
{
    /// <summary>
    /// Gets a validation error indicating that a boolean value is not true when it should be.
    /// </summary>
    /// <returns>
    /// A <see cref="ValidationError"/> with code "BooleanError.NotTrue" and message "The value must be true."
    /// </returns>
    /// <remarks>
    /// Use this error when validation requires a boolean value to be true but it is false.
    /// Common use cases include agreement confirmations, feature flags, or prerequisite checks.
    /// </remarks>
    public static readonly ValidationError NotTrue = new("BooleanError.NotTrue", "The value must be true.");
    
    /// <summary>
    /// Gets a validation error indicating that a boolean value is not false when it should be.
    /// </summary>
    /// <returns>
    /// A <see cref="ValidationError"/> with code "BooleanError.NotFalse" and message "The value must be false."
    /// </returns>
    /// <remarks>
    /// Use this error when validation requires a boolean value to be false but it is true.
    /// This might be used for ensuring certain conditions are not met or flags are disabled.
    /// </remarks>
    public static readonly ValidationError NotFalse = new("BooleanError.NotFalse", "The value must be false.");
}
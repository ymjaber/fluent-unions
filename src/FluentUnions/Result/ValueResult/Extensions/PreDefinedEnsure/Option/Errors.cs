namespace FluentUnions.PreDefinedEnsure;

/// <summary>
/// Provides pre-defined validation errors for Option type validation scenarios.
/// </summary>
/// <remarks>
/// This class contains static factory members that create ValidationError instances
/// for common Option type validation failures. These errors are used when validating
/// Option types to ensure they meet expected Some/None state requirements.
/// </remarks>
public static class OptionErrors
{
    /// <summary>
    /// Gets a validation error indicating that an Option value cannot be in the Some state.
    /// </summary>
    /// <returns>
    /// A <see cref="ValidationError"/> with code "OptionError.Some" and message "The value cannot be some."
    /// </returns>
    /// <remarks>
    /// Use this error when validation requires an Option to be None but it contains a value.
    /// </remarks>
    public static readonly ValidationError Some = new("OptionError.Some", "The value cannot be some.");
    
    /// <summary>
    /// Gets a validation error indicating that an Option value cannot be in the None state.
    /// </summary>
    /// <returns>
    /// A <see cref="ValidationError"/> with code "OptionError.None" and message "The value cannot be none."
    /// </returns>
    /// <remarks>
    /// Use this error when validation requires an Option to have a value but it is None.
    /// </remarks>
    public static readonly ValidationError None = new("OptionError.None", "The value cannot be none.");
}
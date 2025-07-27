namespace FluentUnions.PreDefinedEnsure;

/// <summary>
/// Provides pre-defined validation errors for enum type validation scenarios.
/// </summary>
/// <remarks>
/// This class contains static factory members that create ValidationError instances
/// for enum validation failures. These errors are used when validating enum values
/// to ensure they represent valid, defined enum members.
/// </remarks>
public static class EnumErrors
{
    /// <summary>
    /// Gets a validation error indicating that an enum value is not defined in the enum type.
    /// </summary>
    /// <returns>
    /// A <see cref="ValidationError"/> with code "EnumError.NotDefined" and message "The value is not defined."
    /// </returns>
    /// <remarks>
    /// Use this error when an enum value falls outside the defined enum members.
    /// This commonly occurs when casting integers to enums without validation or when
    /// dealing with flag enums that have invalid bit combinations.
    /// </remarks>
    public static readonly ValidationError NotDefined = new("EnumError.NotDefined", "The value is not defined.");
}
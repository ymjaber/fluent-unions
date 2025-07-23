namespace FluentUnions.PreDefinedEnsure;

/// <summary>
/// Provides pre-defined validation errors for GUID validation scenarios.
/// </summary>
/// <remarks>
/// This class contains static factory members that create ValidationError instances
/// for GUID (Globally Unique Identifier) validation failures. These errors are used 
/// when validating System.Guid values to ensure they meet empty/non-empty requirements.
/// </remarks>
public static class GuidErrors
{
    /// <summary>
    /// Gets a validation error indicating that a GUID value is not empty when it should be.
    /// </summary>
    /// <returns>
    /// A <see cref="ValidationError"/> with code "GuidError.NotEmpty" and message "Value must be empty."
    /// </returns>
    /// <remarks>
    /// Use this error when validation requires a GUID to be Guid.Empty but it contains a non-empty value.
    /// This might be used when expecting uninitialized or default GUID values.
    /// </remarks>
    public static readonly ValidationError NotEmpty = new("GuidError.NotEmpty", "Value must be empty.");
    
    /// <summary>
    /// Gets a validation error indicating that a GUID value is empty when it should not be.
    /// </summary>
    /// <returns>
    /// A <see cref="ValidationError"/> with code "GuidError.Empty" and message "Value cannot be empty."
    /// </returns>
    /// <remarks>
    /// Use this error when validation requires a non-empty GUID but Guid.Empty was provided.
    /// This is one of the most common GUID validations, ensuring that identifiers have been properly generated
    /// and are not default/uninitialized values.
    /// </remarks>
    public static readonly ValidationError Empty = new("GuidError.Empty", "Value cannot be empty.");
}
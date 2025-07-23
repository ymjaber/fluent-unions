namespace FluentUnions.PreDefinedEnsure;

/// <summary>
/// Provides pre-defined validation errors for numeric validation scenarios.
/// </summary>
/// <remarks>
/// This class contains static factory members and methods that create ValidationError instances
/// for numeric validation failures. These errors support validation of numeric ranges, signs,
/// and specific numeric values across all numeric types (int, long, double, decimal, etc.).
/// </remarks>
public static class NumericErrors
{
    /// <summary>
    /// Creates a validation error indicating that a numeric value is smaller than the minimum allowed value.
    /// </summary>
    /// <typeparam name="T">The numeric type being validated.</typeparam>
    /// <param name="min">The minimum threshold value.</param>
    /// <param name="inclusive">If true, the value must be greater than or equal to min; if false, it must be strictly greater than min.</param>
    /// <returns>
    /// A <see cref="ValidationError"/> with code "NumericError.TooSmall" and a message indicating the minimum value requirement.
    /// </returns>
    /// <remarks>
    /// Use this error when validation requires a numeric value to meet a minimum threshold.
    /// The generic type parameter allows this method to work with any numeric type.
    /// </remarks>
    public static ValidationError TooSmall<T>(T min, bool inclusive) => new(
        "NumericError.TooSmall",
        $"Value must be greater than {(inclusive ? "or equal to" : "")} {min}.");

    /// <summary>
    /// Creates a validation error indicating that a numeric value exceeds the maximum allowed value.
    /// </summary>
    /// <typeparam name="T">The numeric type being validated.</typeparam>
    /// <param name="max">The maximum threshold value.</param>
    /// <param name="inclusive">If true, the value must be less than or equal to max; if false, it must be strictly less than max.</param>
    /// <returns>
    /// A <see cref="ValidationError"/> with code "NumericError.TooLarge" and a message indicating the maximum value requirement.
    /// </returns>
    /// <remarks>
    /// Use this error when validation requires a numeric value to not exceed a maximum threshold.
    /// Common use cases include age limits, quantity restrictions, or maximum allowed values.
    /// </remarks>
    public static ValidationError TooLarge<T>(T max, bool inclusive) => new(
        "NumericError.TooLarge",
        $"Value must be less than {(inclusive ? "or equal to" : "")} {max}.");

    /// <summary>
    /// Creates a validation error indicating that a numeric value is outside the allowed range.
    /// </summary>
    /// <typeparam name="T">The numeric type being validated.</typeparam>
    /// <param name="min">The minimum boundary of the allowed range.</param>
    /// <param name="inclusiveMin">If true, the minimum boundary is inclusive; if false, it is exclusive.</param>
    /// <param name="max">The maximum boundary of the allowed range.</param>
    /// <param name="inclusiveMax">If true, the maximum boundary is inclusive; if false, it is exclusive.</param>
    /// <returns>
    /// A <see cref="ValidationError"/> with code "NumericError.OutOfRange" and a message indicating the allowed range.
    /// </returns>
    /// <remarks>
    /// Use this error when validation requires a numeric value to be within a specific range.
    /// This combines both minimum and maximum validation in a single error message.
    /// </remarks>
    public static ValidationError OutOfRange<T>(T min, bool inclusiveMin, T max, bool inclusiveMax) => new(
        "NumericError.OutOfRange",
        $"Value must be greater than {(inclusiveMin ? "or equal to" : "")} {min} and less than {(inclusiveMax ? "or equal to" : "")} {max}.");
    
    /// <summary>
    /// Gets a validation error indicating that a numeric value is not positive when it should be.
    /// </summary>
    /// <returns>
    /// A <see cref="ValidationError"/> with code "NumericError.NotPositive" and message "Value must be positive."
    /// </returns>
    /// <remarks>
    /// Use this error when validation requires a numeric value to be greater than zero.
    /// This error indicates that zero or negative values are not acceptable.
    /// </remarks>
    public static readonly ValidationError NotPositive = new("NumericError.NotPositive", "Value must be positive.");
    
    /// <summary>
    /// Gets a validation error indicating that a numeric value is not negative when it should be.
    /// </summary>
    /// <returns>
    /// A <see cref="ValidationError"/> with code "NumericError.NotNegative" and message "Value must be negative."
    /// </returns>
    /// <remarks>
    /// Use this error when validation requires a numeric value to be less than zero.
    /// This is less common but useful for specific business logic requirements.
    /// </remarks>
    public static readonly ValidationError NotNegative = new("NumericError.NotNegative", "Value must be negative.");
    
    /// <summary>
    /// Gets a validation error indicating that a numeric value is not zero when it should be.
    /// </summary>
    /// <returns>
    /// A <see cref="ValidationError"/> with code "NumericError.NotZero" and message "Value must be zero."
    /// </returns>
    /// <remarks>
    /// Use this error when validation specifically requires a numeric value to be exactly zero.
    /// This is useful for balance checks or initialization validations.
    /// </remarks>
    public static readonly ValidationError NotZero = new("NumericError.NotZero", "Value must be zero.");
    
    /// <summary>
    /// Gets a validation error indicating that a numeric value is zero when it should not be.
    /// </summary>
    /// <returns>
    /// A <see cref="ValidationError"/> with code "NumericError.Zero" and message "Value cannot be zero."
    /// </returns>
    /// <remarks>
    /// Use this error when validation requires a non-zero numeric value.
    /// Common use cases include division operations, multipliers, or any scenario where zero is invalid.
    /// </remarks>
    public static readonly ValidationError Zero = new("NumericError.Zero", "Value cannot be zero.");
    
    /// <summary>
    /// Gets a validation error indicating that a numeric value is positive when it should not be.
    /// </summary>
    /// <returns>
    /// A <see cref="ValidationError"/> with code "NumericError.Positive" and message "Value cannot be positive."
    /// </returns>
    /// <remarks>
    /// Use this error when validation requires a numeric value to be zero or negative.
    /// This might be used for debt amounts, temperature below freezing, or similar scenarios.
    /// </remarks>
    public static readonly ValidationError Positive = new("NumericError.Positive", "Value cannot be positive.");
    
    /// <summary>
    /// Gets a validation error indicating that a numeric value is negative when it should not be.
    /// </summary>
    /// <returns>
    /// A <see cref="ValidationError"/> with code "NumericError.Negative" and message "Value cannot be negative."
    /// </returns>
    /// <remarks>
    /// Use this error when validation requires a numeric value to be zero or positive.
    /// This is one of the most common numeric validations for quantities, ages, distances, etc.
    /// </remarks>
    public static readonly ValidationError Negative = new("NumericError.Negative", "Value cannot be negative.");
}
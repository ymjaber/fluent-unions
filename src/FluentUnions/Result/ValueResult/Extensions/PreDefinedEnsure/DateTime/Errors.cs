namespace FluentUnions.PreDefinedEnsure;

/// <summary>
/// Provides pre-defined validation errors for DateTime validation scenarios.
/// </summary>
/// <remarks>
/// This class contains static factory members and methods that create ValidationError instances
/// for date and time validation failures. These errors are used when validating DateTime values
/// to ensure they meet temporal requirements such as being in the past or future.
/// </remarks>
public static class DateTimeErrors
{
    /// <summary>
    /// Creates a validation error indicating that a date/time value is not in the past when it should be.
    /// </summary>
    /// <param name="parameterType">The type or name of the parameter being validated (e.g., "Date", "Timestamp", "Creation date").</param>
    /// <returns>
    /// A <see cref="ValidationError"/> with code "DateTimeError.NotInPast" and a message indicating the value should be in the past.
    /// </returns>
    /// <remarks>
    /// Use this error when validation requires a DateTime to be before the current time but it is not.
    /// The parameterType parameter allows customization of the error message for better context.
    /// </remarks>
    public static ValidationError NotInPast(string parameterType) =>
        new("DateTimeError.NotInPast", $"{parameterType} should be in the past.");

    /// <summary>
    /// Creates a validation error indicating that a date/time value is not in the future when it should be.
    /// </summary>
    /// <param name="parameterType">The type or name of the parameter being validated (e.g., "Date", "Expiration date", "Due date").</param>
    /// <returns>
    /// A <see cref="ValidationError"/> with code "DateTimeError.NotInFuture" and a message indicating the value should be in the future.
    /// </returns>
    /// <remarks>
    /// Use this error when validation requires a DateTime to be after the current time but it is not.
    /// The parameterType parameter allows customization of the error message for better context.
    /// </remarks>
    public static ValidationError NotInFuture(string parameterType) =>
        new("DateTimeError.NotInFuture", $"{parameterType} should be in the future.");

    /// <summary>
    /// Gets a validation error indicating that a date is in the past when it should not be.
    /// </summary>
    /// <returns>
    /// A <see cref="ValidationError"/> with code "DateTimeError.InPast" and message "Date cannot be in the past."
    /// </returns>
    /// <remarks>
    /// Use this error when validation requires a DateTime to be current or future but a past date was provided.
    /// Common use cases include preventing backdated entries or ensuring future scheduling.
    /// </remarks>
    public static readonly ValidationError DateInPast =
        new("DateTimeError.InPast", "Date cannot be in the past.");

    /// <summary>
    /// Gets a validation error indicating that a date is in the future when it should not be.
    /// </summary>
    /// <returns>
    /// A <see cref="ValidationError"/> with code "DateTimeError.InFuture" and message "Date cannot be in the future."
    /// </returns>
    /// <remarks>
    /// Use this error when validation requires a DateTime to be current or past but a future date was provided.
    /// Common use cases include birth dates, historical records, or preventing future-dated transactions.
    /// </remarks>
    public static readonly ValidationError DateInFuture =
        new("DateTimeError.InFuture", "Date cannot be in the future.");
}
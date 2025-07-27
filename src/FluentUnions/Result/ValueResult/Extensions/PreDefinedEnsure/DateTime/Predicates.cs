using FluentUnions.PreDefinedEnsure;

namespace FluentUnions;

/// <summary>Provides pre-defined validation predicates for DateTime values in <see cref="Result{T}"/> types.</summary>
public static partial class EnsureBuilderExtensions
{
    /// <summary>
    /// Ensures that the DateTime value is in the future.
    /// </summary>
    /// <param name="ensure">The ensure builder for the DateTime value.</param>
    /// <param name="error">Optional custom error. If null, a default error message will be used.</param>
    /// <returns>A result that validates the DateTime is after the current time.</returns>
    public static Result<DateTime> InFuture(
        in this EnsureBuilder<DateTime> ensure,
        Error? error = null) =>
        ensure.Satisfies(v => v > DateTime.Now, error ?? DateTimeErrors.NotInFuture("DateTime"));

    /// <summary>
    /// Ensures that the DateOnly value is in the future.
    /// </summary>
    /// <param name="ensure">The ensure builder for the DateOnly value.</param>
    /// <param name="error">Optional custom error. If null, a default error message will be used.</param>
    /// <returns>A result that validates the DateOnly is after the current date.</returns>
    public static Result<DateOnly> InFuture(
        in this EnsureBuilder<DateOnly> ensure,
        Error? error = null) =>
        ensure.Satisfies(v => v > DateOnly.FromDateTime(DateTime.Now),
            error ?? DateTimeErrors.NotInFuture("Date"));

    /// <summary>
    /// Ensures that the TimeOnly value is in the future.
    /// </summary>
    /// <param name="ensure">The ensure builder for the TimeOnly value.</param>
    /// <param name="error">Optional custom error. If null, a default error message will be used.</param>
    /// <returns>A result that validates the TimeOnly is after the current time.</returns>
    public static Result<TimeOnly> InFuture(
        in this EnsureBuilder<TimeOnly> ensure,
        Error? error = null) =>
        ensure.Satisfies(v => v > TimeOnly.FromDateTime(DateTime.Now),
            error ?? DateTimeErrors.NotInFuture("Time"));

    /// <summary>
    /// Ensures that the DateTime value is in the past.
    /// </summary>
    /// <param name="ensure">The ensure builder for the DateTime value.</param>
    /// <param name="error">Optional custom error. If null, a default error message will be used.</param>
    /// <returns>A result that validates the DateTime is before the current time.</returns>
    public static Result<DateTime> InPast(
        in this EnsureBuilder<DateTime> ensure,
        Error? error = null) =>
        ensure.Satisfies(v => v < DateTime.Now, error ?? DateTimeErrors.NotInPast("DateTime"));

    /// <summary>
    /// Ensures that the DateOnly value is in the past.
    /// </summary>
    /// <param name="ensure">The ensure builder for the DateOnly value.</param>
    /// <param name="error">Optional custom error. If null, a default error message will be used.</param>
    /// <returns>A result that validates the DateOnly is before the current date.</returns>
    public static Result<DateOnly> InPast(
        in this EnsureBuilder<DateOnly> ensure,
        Error? error = null) =>
        ensure.Satisfies(v => v < DateOnly.FromDateTime(DateTime.Now),
            error ?? DateTimeErrors.NotInPast("Date"));

    /// <summary>
    /// Ensures that the TimeOnly value is in the past.
    /// </summary>
    /// <param name="ensure">The ensure builder for the TimeOnly value.</param>
    /// <param name="error">Optional custom error. If null, a default error message will be used.</param>
    /// <returns>A result that validates the TimeOnly is before the current time.</returns>
    public static Result<TimeOnly> InPast(
        in this EnsureBuilder<TimeOnly> ensure,
        Error? error = null) =>
        ensure.Satisfies(v => v < TimeOnly.FromDateTime(DateTime.Now),
            error ?? DateTimeErrors.NotInPast("Time"));

    /// <summary>
    /// Ensures that the DateOnly value is in the future or present.
    /// </summary>
    /// <param name="ensure">The ensure builder for the DateOnly value.</param>
    /// <param name="error">Optional custom error. If null, a default error message will be used.</param>
    /// <returns>A result that validates the DateOnly is on or after the current date.</returns>
    public static Result<DateOnly> InFutureOrPresent(
        in this EnsureBuilder<DateOnly> ensure,
        Error? error = null) =>
        ensure.Satisfies(v => v >= DateOnly.FromDateTime(DateTime.Now),
            error ?? DateTimeErrors.DateInPast);

    /// <summary>
    /// Ensures that the DateOnly value is in the past or present.
    /// </summary>
    /// <param name="ensure">The ensure builder for the DateOnly value.</param>
    /// <param name="error">Optional custom error. If null, a default error message will be used.</param>
    /// <returns>A result that validates the DateOnly is on or before the current date.</returns>
    public static Result<DateOnly> InPastOrPresent(
        in this EnsureBuilder<DateOnly> ensure,
        Error? error = null) =>
        ensure.Satisfies(v => v <= DateOnly.FromDateTime(DateTime.Now),
            error ?? DateTimeErrors.DateInFuture);
}

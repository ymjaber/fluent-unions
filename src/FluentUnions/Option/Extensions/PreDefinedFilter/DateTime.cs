namespace FluentUnions;

/// <summary>Provides pre-defined filter methods for DateTime values in <see cref="Option{T}"/> types.</summary>
public static partial class FilterBuilderExtensions
{
    /// <summary>
    /// Filters options where the DateTime value is in the future.
    /// </summary>
    /// <param name="filter">The filter builder for the DateTime value.</param>
    /// <returns>An option that validates the DateTime is after the current time.</returns>
    public static Option<DateTime> InFuture(in this FilterBuilder<DateTime> filter)
        => filter.Satisfies(v => v > DateTime.Now);

    /// <summary>
    /// Filters options where the DateOnly value is in the future.
    /// </summary>
    /// <param name="filter">The filter builder for the DateOnly value.</param>
    /// <returns>An option that validates the DateOnly is after the current date.</returns>
    public static Option<DateOnly> InFuture(in this FilterBuilder<DateOnly> filter)
        => filter.Satisfies(v => v > DateOnly.FromDateTime(DateTime.Now));

    /// <summary>
    /// Filters options where the TimeOnly value is in the future.
    /// </summary>
    /// <param name="filter">The filter builder for the TimeOnly value.</param>
    /// <returns>An option that validates the TimeOnly is after the current time.</returns>
    public static Option<TimeOnly> InFuture(in this FilterBuilder<TimeOnly> filter)
        => filter.Satisfies(v => v > TimeOnly.FromDateTime(DateTime.Now));

    /// <summary>
    /// Filters options where the DateTime value is in the past.
    /// </summary>
    /// <param name="filter">The filter builder for the DateTime value.</param>
    /// <returns>An option that validates the DateTime is before the current time.</returns>
    public static Option<DateTime> InPast(in this FilterBuilder<DateTime> filter)
        => filter.Satisfies(v => v < DateTime.Now);

    /// <summary>
    /// Filters options where the DateOnly value is in the past.
    /// </summary>
    /// <param name="filter">The filter builder for the DateOnly value.</param>
    /// <returns>An option that validates the DateOnly is before the current date.</returns>
    public static Option<DateOnly> InPast(in this FilterBuilder<DateOnly> filter)
        => filter.Satisfies(v => v < DateOnly.FromDateTime(DateTime.Now));

    /// <summary>
    /// Filters options where the TimeOnly value is in the past.
    /// </summary>
    /// <param name="filter">The filter builder for the TimeOnly value.</param>
    /// <returns>An option that validates the TimeOnly is before the current time.</returns>
    public static Option<TimeOnly> InPast(in this FilterBuilder<TimeOnly> filter)
        => filter.Satisfies(v => v < TimeOnly.FromDateTime(DateTime.Now));

    /// <summary>
    /// Filters options where the DateOnly value is in the future or present.
    /// </summary>
    /// <param name="filter">The filter builder for the DateOnly value.</param>
    /// <returns>An option that validates the DateOnly is on or after the current date.</returns>
    public static Option<DateOnly> InFutureOrPresent(in this FilterBuilder<DateOnly> filter)
        => filter.Satisfies(v => v >= DateOnly.FromDateTime(DateTime.Now));

    /// <summary>
    /// Filters options where the DateOnly value is in the past or present.
    /// </summary>
    /// <param name="filter">The filter builder for the DateOnly value.</param>
    /// <returns>An option that validates the DateOnly is on or before the current date.</returns>
    public static Option<DateOnly> InPastOrPresent(in this FilterBuilder<DateOnly> filter)
        => filter.Satisfies(v => v <= DateOnly.FromDateTime(DateTime.Now));
}

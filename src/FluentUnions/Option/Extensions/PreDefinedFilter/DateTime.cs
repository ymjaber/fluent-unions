namespace FluentUnions;

/// <summary>Provides pre-defined filter methods for DateTime values in <see cref="Option{T}"/> types.</summary>
public static partial class FilterBuilderExtensions
{
    /// <summary>
    /// Filters DateTime values that are in the future.
    /// </summary>
    /// <param name="filter">The filter builder to apply the condition to.</param>
    /// <returns>A filter builder that continues the fluent chain.</returns>
    public static FilterBuilder<DateTime> InFuture(in this FilterBuilder<DateTime> filter)
        => filter.Satisfies(v => v > DateTime.Now);

    /// <summary>
    /// Filters DateOnly values that are in the future.
    /// </summary>
    /// <param name="filter">The filter builder to apply the condition to.</param>
    /// <returns>A filter builder that continues the fluent chain.</returns>
    public static FilterBuilder<DateOnly> InFuture(in this FilterBuilder<DateOnly> filter)
        => filter.Satisfies(v => v > DateOnly.FromDateTime(DateTime.Now));

    /// <summary>
    /// Filters TimeOnly values that are later than the current time.
    /// </summary>
    /// <param name="filter">The filter builder to apply the condition to.</param>
    /// <returns>A filter builder that continues the fluent chain.</returns>
    public static FilterBuilder<TimeOnly> InFuture(in this FilterBuilder<TimeOnly> filter)
        => filter.Satisfies(v => v > TimeOnly.FromDateTime(DateTime.Now));

    /// <summary>
    /// Filters DateTime values that are in the past.
    /// </summary>
    /// <param name="filter">The filter builder to apply the condition to.</param>
    /// <returns>A filter builder that continues the fluent chain.</returns>
    public static FilterBuilder<DateTime> InPast(in this FilterBuilder<DateTime> filter)
        => filter.Satisfies(v => v < DateTime.Now);

    /// <summary>
    /// Filters DateOnly values that are in the past.
    /// </summary>
    /// <param name="filter">The filter builder to apply the condition to.</param>
    /// <returns>A filter builder that continues the fluent chain.</returns>
    public static FilterBuilder<DateOnly> InPast(in this FilterBuilder<DateOnly> filter)
        => filter.Satisfies(v => v < DateOnly.FromDateTime(DateTime.Now));

    /// <summary>
    /// Filters TimeOnly values that are earlier than the current time.
    /// </summary>
    /// <param name="filter">The filter builder to apply the condition to.</param>
    /// <returns>A filter builder that continues the fluent chain.</returns>
    public static FilterBuilder<TimeOnly> InPast(in this FilterBuilder<TimeOnly> filter)
        => filter.Satisfies(v => v < TimeOnly.FromDateTime(DateTime.Now));

    /// <summary>
    /// Filters DateOnly values that are today or in the future.
    /// </summary>
    /// <param name="filter">The filter builder to apply the condition to.</param>
    /// <returns>A filter builder that continues the fluent chain.</returns>
    public static FilterBuilder<DateOnly> InFutureOrPresent(in this FilterBuilder<DateOnly> filter)
        => filter.Satisfies(v => v >= DateOnly.FromDateTime(DateTime.Now));

    /// <summary>
    /// Filters DateOnly values that are today or in the past.
    /// </summary>
    /// <param name="filter">The filter builder to apply the condition to.</param>
    /// <returns>A filter builder that continues the fluent chain.</returns>
    public static FilterBuilder<DateOnly> InPastOrPresent(in this FilterBuilder<DateOnly> filter)
        => filter.Satisfies(v => v <= DateOnly.FromDateTime(DateTime.Now));
}
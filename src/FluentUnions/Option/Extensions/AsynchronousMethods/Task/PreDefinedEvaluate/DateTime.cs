namespace FluentUnions;

/// <summary>Provides pre-defined filter methods for DateTime values in <see cref="Option{T}"/> types using Task-based asynchronous evaluation.</summary>
public static partial class FilterBuilderExtensions
{
    public static async Task<FilterBuilder<DateTime>> InFuture(this Task<FilterBuilder<DateTime>> filter)
        => (await filter.ConfigureAwait(false)).InFuture();

    public static async Task<FilterBuilder<DateOnly>> InFuture(this Task<FilterBuilder<DateOnly>> filter)
        => (await filter.ConfigureAwait(false)).InFuture();

    public static async Task<FilterBuilder<TimeOnly>> InFuture(this Task<FilterBuilder<TimeOnly>> filter)
        => (await filter.ConfigureAwait(false)).InFuture();

    public static async Task<FilterBuilder<DateTime>> InPast(this Task<FilterBuilder<DateTime>> filter)
        => (await filter.ConfigureAwait(false)).InPast();

    public static async Task<FilterBuilder<DateOnly>> InPast(this Task<FilterBuilder<DateOnly>> filter)
        => (await filter.ConfigureAwait(false)).InPast();

    public static async Task<FilterBuilder<TimeOnly>> InPast(this Task<FilterBuilder<TimeOnly>> filter)
        => (await filter.ConfigureAwait(false)).InPast();

    public static async Task<FilterBuilder<DateOnly>> InFutureOrPresent(this Task<FilterBuilder<DateOnly>> filter)
        => (await filter.ConfigureAwait(false)).InFutureOrPresent();

    public static async Task<FilterBuilder<DateOnly>> InPastOrPresent(this Task<FilterBuilder<DateOnly>> filter)
        => (await filter.ConfigureAwait(false)).InPastOrPresent();
}
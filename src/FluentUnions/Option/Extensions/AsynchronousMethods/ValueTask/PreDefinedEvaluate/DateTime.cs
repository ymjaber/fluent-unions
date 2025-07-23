namespace FluentUnions;

/// <summary>Provides pre-defined filter methods for DateTime values in <see cref="Option{T}"/> types using ValueTask-based asynchronous evaluation.</summary>
public static partial class FilterBuilderExtensions
{
    public static async ValueTask<FilterBuilder<DateTime>> InFuture(this ValueTask<FilterBuilder<DateTime>> filter)
        => (await filter.ConfigureAwait(false)).InFuture();

    public static async ValueTask<FilterBuilder<DateOnly>> InFuture(this ValueTask<FilterBuilder<DateOnly>> filter)
        => (await filter.ConfigureAwait(false)).InFuture();

    public static async ValueTask<FilterBuilder<TimeOnly>> InFuture(this ValueTask<FilterBuilder<TimeOnly>> filter)
        => (await filter.ConfigureAwait(false)).InFuture();

    public static async ValueTask<FilterBuilder<DateTime>> InPast(this ValueTask<FilterBuilder<DateTime>> filter)
        => (await filter.ConfigureAwait(false)).InPast();

    public static async ValueTask<FilterBuilder<DateOnly>> InPast(this ValueTask<FilterBuilder<DateOnly>> filter)
        => (await filter.ConfigureAwait(false)).InPast();

    public static async ValueTask<FilterBuilder<TimeOnly>> InPast(this ValueTask<FilterBuilder<TimeOnly>> filter)
        => (await filter.ConfigureAwait(false)).InPast();

    public static async ValueTask<FilterBuilder<DateOnly>> InFutureOrPresent(this ValueTask<FilterBuilder<DateOnly>> filter)
        => (await filter.ConfigureAwait(false)).InFutureOrPresent();

    public static async ValueTask<FilterBuilder<DateOnly>> InPastOrPresent(this ValueTask<FilterBuilder<DateOnly>> filter)
        => (await filter.ConfigureAwait(false)).InPastOrPresent();
}
namespace FluentUnions;

/// <summary>Provides pre-defined validation predicates for DateTime values in <see cref="Result{T}"/> types using ValueTask-based asynchronous validation.</summary>
public static partial class EnsureBuilderExtensions
{
    public static async ValueTask<EnsureBuilder<DateTime>> InFuture(
        this ValueTask<EnsureBuilder<DateTime>> ensure,
        Error? error = null)
        => (await ensure.ConfigureAwait(false)).InFuture(error);

    public static async ValueTask<EnsureBuilder<DateOnly>> InFuture(
        this ValueTask<EnsureBuilder<DateOnly>> ensure,
        Error? error = null)
        => (await ensure.ConfigureAwait(false)).InFuture(error);

    public static async ValueTask<EnsureBuilder<TimeOnly>> InFuture(
        this ValueTask<EnsureBuilder<TimeOnly>> ensure,
        Error? error = null)
        => (await ensure.ConfigureAwait(false)).InFuture(error);

    public static async ValueTask<EnsureBuilder<DateTime>> InPast(
        this ValueTask<EnsureBuilder<DateTime>> ensure,
        Error? error = null)
        => (await ensure.ConfigureAwait(false)).InPast(error);

    public static async ValueTask<EnsureBuilder<DateOnly>> InPast(
        this ValueTask<EnsureBuilder<DateOnly>> ensure,
        Error? error = null)
        => (await ensure.ConfigureAwait(false)).InPast(error);

    public static async ValueTask<EnsureBuilder<TimeOnly>> InPast(
        this ValueTask<EnsureBuilder<TimeOnly>> ensure,
        Error? error = null)
        => (await ensure.ConfigureAwait(false)).InPast(error);

    public static async ValueTask<EnsureBuilder<DateOnly>> InFutureOrPresent(
        this ValueTask<EnsureBuilder<DateOnly>> ensure,
        Error? error = null)
        => (await ensure.ConfigureAwait(false)).InFutureOrPresent(error);

    public static async ValueTask<EnsureBuilder<DateOnly>> InPastOrPresent(
        this ValueTask<EnsureBuilder<DateOnly>> ensure,
        Error? error = null)
        => (await ensure.ConfigureAwait(false)).InPastOrPresent(error);
}
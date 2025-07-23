namespace FluentUnions;

/// <summary>Provides pre-defined validation predicates for DateTime values in <see cref="Result{T}"/> types using Task-based asynchronous validation.</summary>
public static partial class EnsureBuilderExtensions
{
    public static async Task<EnsureBuilder<DateTime>> InFuture(
        this Task<EnsureBuilder<DateTime>> ensure,
        Error? error = null)
        => (await ensure.ConfigureAwait(false)).InFuture(error);

    public static async Task<EnsureBuilder<DateOnly>> InFuture(
        this Task<EnsureBuilder<DateOnly>> ensure,
        Error? error = null)
        => (await ensure.ConfigureAwait(false)).InFuture(error);

    public static async Task<EnsureBuilder<TimeOnly>> InFuture(
        this Task<EnsureBuilder<TimeOnly>> ensure,
        Error? error = null)
        => (await ensure.ConfigureAwait(false)).InFuture(error);

    public static async Task<EnsureBuilder<DateTime>> InPast(
        this Task<EnsureBuilder<DateTime>> ensure,
        Error? error = null)
        => (await ensure.ConfigureAwait(false)).InPast(error);

    public static async Task<EnsureBuilder<DateOnly>> InPast(
        this Task<EnsureBuilder<DateOnly>> ensure,
        Error? error = null)
        => (await ensure.ConfigureAwait(false)).InPast(error);

    public static async Task<EnsureBuilder<TimeOnly>> InPast(
        this Task<EnsureBuilder<TimeOnly>> ensure,
        Error? error = null)
        => (await ensure.ConfigureAwait(false)).InPast(error);

    public static async Task<EnsureBuilder<DateOnly>> InFutureOrPresent(
        this Task<EnsureBuilder<DateOnly>> ensure,
        Error? error = null)
        => (await ensure.ConfigureAwait(false)).InFutureOrPresent(error);

    public static async Task<EnsureBuilder<DateOnly>> InPastOrPresent(
        this Task<EnsureBuilder<DateOnly>> ensure,
        Error? error = null)
        => (await ensure.ConfigureAwait(false)).InPastOrPresent(error);
}
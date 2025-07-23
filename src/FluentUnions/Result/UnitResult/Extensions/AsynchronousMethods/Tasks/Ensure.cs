namespace FluentUnions;

/// <summary>Provides Task-based asynchronous validation methods for <see cref="Result"/> types.</summary>
public static partial class UnitResultExtensions
{
    public static async Task<Result> EnsureAsync(this Result result, Func<Task<bool>> predicate, Error error)
    {
        if (!await predicate().ConfigureAwait(false)) return error;
        return result;
    }

    public static async Task<Result> EnsureAsync(this Task<Result> result, Func<bool> predicate, Error error) =>
        (await result.ConfigureAwait(false)).Ensure(predicate, error);

    public static async Task<Result> EnsureAsync(this Task<Result> result, Func<Task<bool>> predicate, Error error) =>
        await (await result.ConfigureAwait(false)).EnsureAsync(predicate, error).ConfigureAwait(false);
}
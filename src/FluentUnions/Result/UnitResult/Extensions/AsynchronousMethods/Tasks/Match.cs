namespace FluentUnions;

/// <summary>Provides Task-based asynchronous pattern matching for <see cref="Result"/> types.</summary>
public static partial class UnitResultExtensions
{
    public static Task<TTarget> MatchAsync<TTarget>(
        this Result result,
        Func<Task<TTarget>> success,
        Func<Error, Task<TTarget>> failure)
    {
        return result.IsSuccess
            ? success()
            : failure(result.Error);
    }

    public static async Task<TTarget> MatchAsync<TTarget>(
        this Task<Result> result,
        Func<TTarget> success,
        Func<Error, TTarget> failure) => (await result.ConfigureAwait(false)).Match(success, failure);

    public static async Task<TTarget> MatchAsync<TTarget>(
        this Task<Result> result,
        Func<Task<TTarget>> success,
        Func<Error, Task<TTarget>> failure) =>
        await (await result.ConfigureAwait(false)).MatchAsync(success, failure).ConfigureAwait(false);
}
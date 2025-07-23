namespace FluentUnions;

/// <summary>Provides ValueTask-based asynchronous pattern matching for <see cref="Result"/> types.</summary>
public static partial class UnitResultExtensions
{
    public static ValueTask<TTarget> MatchAsync<TTarget>(
        this Result result,
        Func<ValueTask<TTarget>> success,
        Func<Error, ValueTask<TTarget>> failure)
    {
        return result.IsSuccess
            ? success()
            : failure(result.Error);
    }

    public static async ValueTask<TTarget> MatchAsync<TTarget>(
        this ValueTask<Result> result,
        Func<TTarget> success,
        Func<Error, TTarget> failure) => (await result.ConfigureAwait(false)).Match(success, failure);

    public static async ValueTask<TTarget> MatchAsync<TTarget>(
        this ValueTask<Result> result,
        Func<ValueTask<TTarget>> success,
        Func<Error, ValueTask<TTarget>> failure) =>
        await (await result.ConfigureAwait(false)).MatchAsync(success, failure).ConfigureAwait(false);
}
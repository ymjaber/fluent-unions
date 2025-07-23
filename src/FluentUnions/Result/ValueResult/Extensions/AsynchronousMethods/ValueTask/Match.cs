namespace FluentUnions;

/// <summary>Provides ValueTask-based asynchronous pattern matching for <see cref="Result{T}"/> types.</summary>
public static partial class ValueResultExtensions
{
    public static ValueTask<TTarget> MatchAsync<TSource, TTarget>(
        in this Result<TSource> result,
        Func<TSource, ValueTask<TTarget>> success,
        Func<Error, ValueTask<TTarget>> failure)
    {
        return result.IsSuccess
            ? success(result.Value)
            : failure(result.Error);
    }

    public static async ValueTask<TTarget> MatchAsync<TSource, TTarget>(
        this ValueTask<Result<TSource>> result,
        Func<TSource, TTarget> success,
        Func<Error, TTarget> failure)
        => (await result.ConfigureAwait(false)).Match(success, failure);

    public static async ValueTask<TTarget> MatchAsync<TSource, TTarget>(
        this ValueTask<Result<TSource>> result,
        Func<TSource, ValueTask<TTarget>> success,
        Func<Error, ValueTask<TTarget>> failure)
        => await (await result.ConfigureAwait(false)).MatchAsync(success, failure).ConfigureAwait(false);
}
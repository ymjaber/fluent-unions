namespace FluentUnions;

/// <summary>Provides Task-based asynchronous pattern matching for <see cref="Result{T}"/> types.</summary>
public static partial class ValueResultExtensions
{
    public static Task<TTarget> MatchAsync<TSource, TTarget>(
        in this Result<TSource> result,
        Func<TSource, Task<TTarget>> success,
        Func<Error, Task<TTarget>> failure)
    {
        return result.IsSuccess
            ? success(result.Value)
            : failure(result.Error);
    }

    public static async Task<TTarget> MatchAsync<TSource, TTarget>(
        this Task<Result<TSource>> result,
        Func<TSource, TTarget> success,
        Func<Error, TTarget> failure)
        => (await result.ConfigureAwait(false)).Match(success, failure);

    public static async Task<TTarget> MatchAsync<TSource, TTarget>(
        this Task<Result<TSource>> result,
        Func<TSource, Task<TTarget>> success,
        Func<Error, Task<TTarget>> failure)
        => await (await result.ConfigureAwait(false)).MatchAsync(success, failure).ConfigureAwait(false);
}
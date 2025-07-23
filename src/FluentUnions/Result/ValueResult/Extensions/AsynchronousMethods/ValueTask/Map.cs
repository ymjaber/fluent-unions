namespace FluentUnions;

/// <summary>Provides ValueTask-based asynchronous transformation methods for <see cref="Result{T}"/> types.</summary>
public static partial class ValueResultExtensions
{
    public static async ValueTask<Result<TTarget>> MapAsync<TSource, TTarget>(
        this Result<TSource> result,
        Func<TSource, ValueTask<TTarget>> mapper)
    {
        if (result.IsFailure) return Result.Failure<TTarget>(result.Error);
        return Result.Success(await mapper(result.Value).ConfigureAwait(false));
    }

    public static async ValueTask<Result<TTarget>> MapAsync<TSource, TTarget>(
        this ValueTask<Result<TSource>> result,
        Func<TSource, TTarget> mapper)
        => (await result.ConfigureAwait(false)).Map(mapper);

    public static async ValueTask<Result<TTarget>> MapAsync<TSource, TTarget>(
        this ValueTask<Result<TSource>> result,
        Func<TSource, ValueTask<TTarget>> mapper)
        => await (await result.ConfigureAwait(false)).MapAsync(mapper).ConfigureAwait(false);
}
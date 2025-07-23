namespace FluentUnions;

/// <summary>Provides Task-based asynchronous bind operations that aggregate all errors for <see cref="Result{T}"/> types.</summary>
public static partial class ValueResultExtensions
{
    public static async Task<Result<TValue>> BindAllAsync<TValue>(this Result<TValue> result, Task<Result> next)
    {
        var nextResult = await next.ConfigureAwait(false);
        if (nextResult.IsSuccess) return result;

        return new ErrorBuilder()
            .AppendOnFailure(result)
            .Append(nextResult.Error)
            .Build();
    }

    public static async Task<Result<TValue>> BindAllAsync<TValue>(this Task<Result<TValue>> result, Result next)
        => (await result.ConfigureAwait(false)).BindAll(next);

    public static async Task<Result<TValue>> BindAllAsync<TValue>(this Task<Result<TValue>> result, Task<Result> next)
        => await (await result.ConfigureAwait(false)).BindAllAsync(next).ConfigureAwait(false);

    public static async Task<Result<TTarget>> BindAllAsync<TSource, TTarget>(
        this Result<TSource> result,
        Task<Result<TTarget>> binder)
    {
        var binderResult = await binder.ConfigureAwait(false);
        if (result.IsSuccess) return binderResult;

        return new ErrorBuilder()
            .Append(result.Error)
            .AppendOnFailure(binderResult)
            .Build();
    }

    public static async Task<Result<TTarget>> BindAllAsync<TSource, TTarget>(
        this Task<Result<TSource>> result,
        Result<TTarget> binder)
        => (await result.ConfigureAwait(false)).BindAll(binder);

    public static async Task<Result<TTarget>> BindAllAsync<TSource, TTarget>(
        this Task<Result<TSource>> result,
        Task<Result<TTarget>> binder)
        => await (await result.ConfigureAwait(false)).BindAllAsync(binder).ConfigureAwait(false);
}
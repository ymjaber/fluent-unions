namespace FluentUnions;

/// <summary>Provides ValueTask-based asynchronous bind operations that aggregate all errors for <see cref="Result"/> types.</summary>
public static partial class UnitResultExtensions
{
    public static async ValueTask<Result> BindAllAsync(this Result result, ValueTask<Result> next)
    {
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(await next.ConfigureAwait(false))
            .TryBuild(out var error)
            ? Result.Failure(error)
            : Result.Success();
    }

    public static async ValueTask<Result> BindAllAsync(this ValueTask<Result> result, Result next) =>
        (await result.ConfigureAwait(false)).BindAll(next);

    public static async ValueTask<Result> BindAllAsync(this ValueTask<Result> result, ValueTask<Result> next) =>
        await (await result.ConfigureAwait(false)).BindAllAsync(next).ConfigureAwait(false);

    public static async ValueTask<Result<TTarget>> BindAllAsync<TTarget>(this Result result, ValueTask<Result<TTarget>> binder)
    {
        var targetResult = await binder.ConfigureAwait(false);

        if (result.IsSuccess) return targetResult;

        return new ErrorBuilder()
            .Append(result.Error)
            .AppendOnFailure(targetResult)
            .Build();
    }

    public static async ValueTask<Result<TTarget>> BindAllAsync<TTarget>(this ValueTask<Result> result, Result<TTarget> binder)
        => (await result.ConfigureAwait(false)).BindAll(binder);

    public static async ValueTask<Result<TTarget>> BindAllAsync<TTarget>(this ValueTask<Result> result, ValueTask<Result<TTarget>> binder)
        => await (await result.ConfigureAwait(false)).BindAllAsync(binder).ConfigureAwait(false);
}
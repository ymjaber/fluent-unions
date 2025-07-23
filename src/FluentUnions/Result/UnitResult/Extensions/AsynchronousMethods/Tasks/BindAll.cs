namespace FluentUnions;

/// <summary>Provides Task-based asynchronous bind operations that aggregate all errors for <see cref="Result"/> types.</summary>
public static partial class UnitResultExtensions
{
    public static async Task<Result> BindAllAsync(this Result result, Task<Result> next)
    {
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(await next.ConfigureAwait(false))
            .TryBuild(out var error)
            ? Result.Failure(error)
            : Result.Success();
    }

    public static async Task<Result> BindAllAsync(this Task<Result> result, Result next) =>
        (await result.ConfigureAwait(false)).BindAll(next);

    public static async Task<Result> BindAllAsync(this Task<Result> result, Task<Result> next) =>
        await (await result.ConfigureAwait(false)).BindAllAsync(next).ConfigureAwait(false);

    public static async Task<Result<TTarget>> BindAllAsync<TTarget>(this Result result, Task<Result<TTarget>> binder)
    {
        var targetResult = await binder.ConfigureAwait(false);

        if (result.IsSuccess) return targetResult;

        return new ErrorBuilder()
            .Append(result.Error)
            .AppendOnFailure(targetResult)
            .Build();
    }

    public static async Task<Result<TTarget>> BindAllAsync<TTarget>(this Task<Result> result, Result<TTarget> binder)
        => (await result.ConfigureAwait(false)).BindAll(binder);

    public static async Task<Result<TTarget>> BindAllAsync<TTarget>(this Task<Result> result, Task<Result<TTarget>> binder)
        => await (await result.ConfigureAwait(false)).BindAllAsync(binder).ConfigureAwait(false);
}
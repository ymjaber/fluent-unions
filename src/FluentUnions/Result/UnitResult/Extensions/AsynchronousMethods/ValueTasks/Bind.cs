namespace FluentUnions;

/// <summary>Provides ValueTask-based asynchronous bind operations for <see cref="Result"/> types.</summary>
public static partial class UnitResultExtensions
{
    public static ValueTask<Result> BindAsync(this Result result, Func<ValueTask<Result>> next)
    {
        if (result.IsFailure) return ValueTask.FromResult(result);
        return next();
    }

    public static async ValueTask<Result> BindAsync(this ValueTask<Result> result, Func<Result> next) =>
        (await result.ConfigureAwait(false)).Bind(next);

    public static async ValueTask<Result> BindAsync(this ValueTask<Result> result, Func<ValueTask<Result>> next) =>
        await (await result.ConfigureAwait(false)).BindAsync(next).ConfigureAwait(false);

    public static ValueTask<Result<TTarget>> BindAsync<TTarget>(this Result result, Func<ValueTask<Result<TTarget>>> binder)
    {
        if (result.IsFailure) return ValueTask.FromResult(Result.Failure<TTarget>(result.Error));
        return binder();
    }

    public static async ValueTask<Result<TTarget>> BindAsync<TTarget>(this ValueTask<Result> result, Func<Result<TTarget>> binder)
        => (await result.ConfigureAwait(false)).Bind(binder);

    public static async ValueTask<Result<TTarget>> BindAsync<TTarget>(
        this ValueTask<Result> result,
        Func<ValueTask<Result<TTarget>>> binder)
        => await (await result.ConfigureAwait(false)).BindAsync(binder).ConfigureAwait(false);
}
namespace FluentUnions;

/// <summary>Provides ValueTask-based asynchronous methods to add a value to unit results.</summary>
public static partial class UnitResultExtensions
{
    public static async ValueTask<Result<TValue>> WithValueAsync<TValue>(this Result result, ValueTask<TValue> value) =>
        result.IsSuccess ? Result.Success(await value.ConfigureAwait(false)) : Result.Failure<TValue>(result.Error);

    public static async ValueTask<Result<TValue>> WithValueAsync<TValue>(this ValueTask<Result> result, TValue value) =>
        (await result.ConfigureAwait(false)).WithValue(value);

    public static async ValueTask<Result<TValue>> WithValueAsync<TValue>(this ValueTask<Result> result, ValueTask<TValue> value) =>
        await (await result.ConfigureAwait(false)).WithValueAsync(value);

    public static async ValueTask<Result<TValue>> WithValueAsync<TValue>(this Result result, Func<ValueTask<TValue>> valueFactory) =>
        result.IsSuccess
            ? Result.Success(await valueFactory().ConfigureAwait(false))
            : Result.Failure<TValue>(result.Error);

    public static async ValueTask<Result<TValue>> WithValueAsync<TValue>(this ValueTask<Result> result, Func<TValue> valueFactory) =>
        (await result.ConfigureAwait(false)).WithValue(valueFactory);

    public static async ValueTask<Result<TValue>> WithValueAsync<TValue>(
        this ValueTask<Result> result,
        Func<ValueTask<TValue>> valueFactory) =>
        await (await result.ConfigureAwait(false)).WithValueAsync(valueFactory);
}
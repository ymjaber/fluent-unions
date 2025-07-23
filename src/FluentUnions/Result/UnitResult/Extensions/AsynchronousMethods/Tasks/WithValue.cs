namespace FluentUnions;

/// <summary>Provides Task-based asynchronous methods to add a value to unit results.</summary>
public static partial class UnitResultExtensions
{
    public static async Task<Result<TValue>> WithValueAsync<TValue>(this Result result, Task<TValue> value) =>
        result.IsSuccess ? Result.Success(await value.ConfigureAwait(false)) : Result.Failure<TValue>(result.Error);

    public static async Task<Result<TValue>> WithValueAsync<TValue>(this Task<Result> result, TValue value) =>
        (await result.ConfigureAwait(false)).WithValue(value);

    public static async Task<Result<TValue>> WithValueAsync<TValue>(this Task<Result> result, Task<TValue> value) =>
        await (await result.ConfigureAwait(false)).WithValueAsync(value);

    public static async Task<Result<TValue>> WithValueAsync<TValue>(this Result result, Func<Task<TValue>> valueFactory) =>
        result.IsSuccess
            ? Result.Success(await valueFactory().ConfigureAwait(false))
            : Result.Failure<TValue>(result.Error);

    public static async Task<Result<TValue>> WithValueAsync<TValue>(this Task<Result> result, Func<TValue> valueFactory) =>
        (await result.ConfigureAwait(false)).WithValue(valueFactory);

    public static async Task<Result<TValue>> WithValueAsync<TValue>(
        this Task<Result> result,
        Func<Task<TValue>> valueFactory) =>
        await (await result.ConfigureAwait(false)).WithValueAsync(valueFactory);
}
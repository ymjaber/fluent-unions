namespace FluentUnions;

/// <summary>Provides ValueTask-based asynchronous validation methods for <see cref="Result{T}"/> types.</summary>
public static partial class ValueResultExtensions
{
    public static async ValueTask<EnsureBuilder<TValue>> EnsureThatAsync<TValue>(this ValueTask<Result<TValue>> result) =>
        new(await result.ConfigureAwait(false));
    
    public static async ValueTask<Result<TValue>> EnsureSomeAsync<TValue>(this ValueTask<Result<Option<TValue>>> result,
        Error error = null!)
        where TValue : notnull
        => (await result.ConfigureAwait(false)).EnsureSome(error);

    public static async ValueTask<Result> EnsureNoneAsync<TValue>(this ValueTask<Result<Option<TValue>>> result, Error error = null!)
        where TValue : notnull
        => (await result.ConfigureAwait(false)).EnsureNone(error);

    public static async ValueTask<Result<TValue>> EnsureAsync<TValue>(
        this Result<TValue> result,
        Func<TValue, ValueTask<bool>> predicate,
        Error error)
    {
        if (result.IsFailure) return result;

        return await predicate(result.Value).ConfigureAwait(false) ? result : Result.Failure<TValue>(error);
    }

    public static async ValueTask<Result<TValue>> EnsureAsync<TValue>(
        this ValueTask<Result<TValue>> result,
        Func<TValue, bool> predicate,
        Error error)
        => (await result.ConfigureAwait(false)).Ensure(predicate, error);

    public static async ValueTask<Result<TValue>> EnsureAsync<TValue>(
        this ValueTask<Result<TValue>> result,
        Func<TValue, ValueTask<bool>> predicate,
        Error error)
        => await (await result.ConfigureAwait(false)).EnsureAsync(predicate, error).ConfigureAwait(false);
}
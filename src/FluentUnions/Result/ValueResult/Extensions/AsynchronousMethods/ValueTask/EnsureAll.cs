namespace FluentUnions;

/// <summary>Provides ValueTask-based asynchronous validation methods that aggregate all errors for <see cref="Result{T}"/> types.</summary>
public static partial class ValueResultExtensions
{
    public static async ValueTask<Result<TValue>> EnsureAllAsync<TValue>(
        this Result<TValue> result,
        ValueTask<bool> condition,
        Error error)
    {
        if (await condition.ConfigureAwait(false)) return result;

        return new ErrorBuilder()
            .AppendOnFailure(result)
            .Append(error)
            .Build();
    }

    public static async ValueTask<Result<TValue>> EnsureAllAsync<TValue>(
        this ValueTask<Result<TValue>> result,
        bool condition,
        Error error)
        => (await result.ConfigureAwait(false)).EnsureAll(condition, error);

    public static async ValueTask<Result<TValue>> EnsureAllAsync<TValue>(
        this ValueTask<Result<TValue>> result,
        ValueTask<bool> condition,
        Error error)
        => await (await result.ConfigureAwait(false)).EnsureAllAsync(condition, error).ConfigureAwait(false);
}
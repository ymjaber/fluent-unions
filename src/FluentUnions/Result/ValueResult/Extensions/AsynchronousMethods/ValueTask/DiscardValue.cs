namespace FluentUnions;

/// <summary>Provides ValueTask-based asynchronous methods to discard the value from <see cref="Result{T}"/> types.</summary>
public static partial class ValueResultExtensions
{
    public static async ValueTask<Result> DiscardValueAsync<TValue>(this ValueTask<Result<TValue>> result) =>
        (await result.ConfigureAwait(false)).DiscardValue();
}
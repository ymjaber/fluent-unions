namespace FluentUnions;

/// <summary>Provides Task-based asynchronous methods to discard the value from <see cref="Result{T}"/> types.</summary>
public static partial class ValueResultExtensions
{
    public static async Task<Result> DiscardValueAsync<TValue>(this Task<Result<TValue>> result) =>
        (await result.ConfigureAwait(false)).DiscardValue();
}
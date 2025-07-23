namespace FluentUnions;

/// <summary>
/// Provides Task-based asynchronous extension methods for converting values to <see cref="Option{T}"/> instances.
/// </summary>
public static partial class AsOptionExtensions
{
    public static async Task<Option<TValue>> AsOptionAsync<TValue>(this Task<TValue?> value)
        where TValue : class
    {
        return Option.From(await value.ConfigureAwait(false));
    }

    public static async Task<Option<TValue>> AsOptionAsync<TValue>(this Task<TValue?> value)
        where TValue : struct
    {
        return Option.From(await value.ConfigureAwait(false));
    }

    public static Task<Result<Option<TValue>>> AsOptionAsync<TValue>(
        this Task<Result<TValue?>> result)
        where TValue : class
    {
        return result.MapAsync(Option.From);
    }
    
    public static Task<Result<Option<TValue>>> AsOptionAsync<TValue>(
        this Task<Result<TValue?>> result)
        where TValue : struct
    {
        return result.MapAsync(Option.From);
    }
}
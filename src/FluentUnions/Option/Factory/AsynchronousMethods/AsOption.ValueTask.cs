namespace FluentUnions;

/// <summary>
/// Provides ValueTask-based asynchronous extension methods for converting values to <see cref="Option{T}"/> instances.
/// </summary>
public static partial class AsOptionExtensions
{
    public static async ValueTask<Option<TValue>> AsOptionAsync<TValue>(this ValueTask<TValue?> value)
        where TValue : class
    {
        return Option.From(await value.ConfigureAwait(false));
    }

    public static async ValueTask<Option<TValue>> AsOptionAsync<TValue>(this ValueTask<TValue?> value)
        where TValue : struct
    {
        return Option.From(await value.ConfigureAwait(false));
    }

    public static ValueTask<Result<Option<TValue>>> AsOptionAsync<TValue>(
        this ValueTask<Result<TValue?>> result)
        where TValue : class
    {
        return result.MapAsync(Option.From);
    }
    
    public static ValueTask<Result<Option<TValue>>> AsOptionAsync<TValue>(
        this ValueTask<Result<TValue?>> result)
        where TValue : struct
    {
        return result.MapAsync(Option.From);
    }
}
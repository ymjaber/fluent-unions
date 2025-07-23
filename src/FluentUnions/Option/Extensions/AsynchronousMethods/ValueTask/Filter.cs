namespace FluentUnions;

/// <summary>Provides asynchronous filtering operations for <see cref="Option{T}"/> types using ValueTask-based operations.</summary>
public static partial class OptionExtensions
{
    public static async ValueTask<FilterBuilder<TValue>> FilterAsync<TValue>(this ValueTask<Option<TValue>> option)
        where TValue : notnull
        => new(await option.ConfigureAwait(false));

    public static async ValueTask<Option<TValue>> FilterAsync<TValue>(
        this Option<TValue> option,
        Func<TValue, ValueTask<bool>> predicate)
        where TValue : notnull
    {
        return option.IsNone || await predicate(option.Value).ConfigureAwait(false)
            ? option
            : Option<TValue>.None;
    }

    public static async ValueTask<Option<TValue>> FilterAsync<TValue>(
        this ValueTask<Option<TValue>> option,
        Func<TValue, bool> predicate)
        where TValue : notnull
        => (await option.ConfigureAwait(false)).Filter(predicate);

    public static async ValueTask<Option<TValue>> FilterAsync<TValue>(
        this ValueTask<Option<TValue>> option,
        Func<TValue, ValueTask<bool>> predicate)
        where TValue : notnull
        => await (await option.ConfigureAwait(false)).FilterAsync(predicate).ConfigureAwait(false);
}
namespace FluentUnions;

/// <summary>Provides asynchronous pattern matching operations for <see cref="Option{T}"/> types using ValueTask-based operations.</summary>
public static partial class OptionExtensions
{
    public static ValueTask<TTarget> MatchAsync<TSource, TTarget>(
        in this Option<TSource> option,
        Func<TSource, ValueTask<TTarget>> some,
        Func<ValueTask<TTarget>> none)
        where TSource : notnull
    {
        return option.IsSome ? some(option.Value) : none();
    }

    public static async ValueTask<TTarget> MatchAsync<TSource, TTarget>(
        this ValueTask<Option<TSource>> option,
        Func<TSource, TTarget> some,
        Func<TTarget> none)
        where TSource : notnull
        => (await option.ConfigureAwait(false)).Match(some, none);

    public static async ValueTask<TTarget> MatchAsync<TSource, TTarget>(
        this ValueTask<Option<TSource>> option,
        Func<TSource, ValueTask<TTarget>> some,
        Func<ValueTask<TTarget>> none)
        where TSource : notnull
        => await (await option.ConfigureAwait(false)).MatchAsync(some, none).ConfigureAwait(false);
}
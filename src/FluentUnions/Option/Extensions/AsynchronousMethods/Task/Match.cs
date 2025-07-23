namespace FluentUnions;

/// <summary>Provides asynchronous pattern matching operations for <see cref="Option{T}"/> types using Task-based operations.</summary>
public static partial class OptionExtensions
{
    public static Task<TTarget> MatchAsync<TSource, TTarget>(
        in this Option<TSource> option,
        Func<TSource, Task<TTarget>> some,
        Func<Task<TTarget>> none)
        where TSource : notnull
    {
        return option.IsSome ? some(option.Value) : none();
    }

    public static async Task<TTarget> MatchAsync<TSource, TTarget>(
        this Task<Option<TSource>> option,
        Func<TSource, TTarget> some,
        Func<TTarget> none)
        where TSource : notnull
        => (await option.ConfigureAwait(false)).Match(some, none);

    public static async Task<TTarget> MatchAsync<TSource, TTarget>(
        this Task<Option<TSource>> option,
        Func<TSource, Task<TTarget>> some,
        Func<Task<TTarget>> none)
        where TSource : notnull
        => await (await option.ConfigureAwait(false)).MatchAsync(some, none).ConfigureAwait(false);
}
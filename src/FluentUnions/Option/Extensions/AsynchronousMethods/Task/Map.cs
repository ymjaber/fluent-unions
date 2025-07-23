namespace FluentUnions;

/// <summary>Provides asynchronous transformation operations for <see cref="Option{T}"/> types using Task-based operations.</summary>
public static partial class OptionExtensions
{
    public static async Task<Option<TTarget>> MapAsync<TSource, TTarget>(
        this Option<TSource> option,
        Func<TSource, Task<TTarget>> mapper)
        where TSource : notnull
        where TTarget : notnull
    {
        if (option.IsNone) return Option<TTarget>.None;
        return Option.Some(await mapper(option.Value).ConfigureAwait(false));
    }

    public static async Task<Option<TTarget>> MapAsync<TSource, TTarget>(
        this Task<Option<TSource>> option,
        Func<TSource, TTarget> mapper)
        where TSource : notnull
        where TTarget : notnull
        => (await option.ConfigureAwait(false)).Map(mapper);

    public static async Task<Option<TTarget>> MapAsync<TSource, TTarget>(
        this Task<Option<TSource>> option,
        Func<TSource, Task<TTarget>> mapper)
        where TSource : notnull
        where TTarget : notnull
        => await (await option.ConfigureAwait(false)).MapAsync(mapper).ConfigureAwait(false);
}
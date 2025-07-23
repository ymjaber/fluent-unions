namespace FluentUnions;

public static partial class OptionExtensions
{
    public static ValueTask<TTarget> MatchAsync<TSource1, TSource2, TTarget>(
        in this Option<(TSource1, TSource2)> option,
        Func<TSource1, TSource2, ValueTask<TTarget>> some,
        Func<ValueTask<TTarget>> none)
    {
        return option.IsSome ? some(option.Value.Item1, option.Value.Item2) : none();
    }

    public static async ValueTask<TTarget> MatchAsync<TSource1, TSource2, TTarget>(
        this ValueTask<Option<(TSource1, TSource2)>> option,
        Func<TSource1, TSource2, TTarget> some,
        Func<TTarget> none)
        => (await option.ConfigureAwait(false)).Match(some, none);

    public static async ValueTask<TTarget> MatchAsync<TSource1, TSource2, TTarget>(
        this ValueTask<Option<(TSource1, TSource2)>> option,
        Func<TSource1, TSource2, ValueTask<TTarget>> some,
        Func<ValueTask<TTarget>> none)
        => await (await option.ConfigureAwait(false)).MatchAsync(some, none).ConfigureAwait(false);
        
    public static ValueTask<TTarget> MatchAsync<TSource1, TSource2, TSource3, TTarget>(
        in this Option<(TSource1, TSource2, TSource3)> option,
        Func<TSource1, TSource2, TSource3, ValueTask<TTarget>> some,
        Func<ValueTask<TTarget>> none)
    {
        return option.IsSome ? some(option.Value.Item1, option.Value.Item2, option.Value.Item3) : none();
    }

    public static async ValueTask<TTarget> MatchAsync<TSource1, TSource2, TSource3, TTarget>(
        this ValueTask<Option<(TSource1, TSource2, TSource3)>> option,
        Func<TSource1, TSource2, TSource3, TTarget> some,
        Func<TTarget> none)
        => (await option.ConfigureAwait(false)).Match(some, none);

    public static async ValueTask<TTarget> MatchAsync<TSource1, TSource2, TSource3, TTarget>(
        this ValueTask<Option<(TSource1, TSource2, TSource3)>> option,
        Func<TSource1, TSource2, TSource3, ValueTask<TTarget>> some,
        Func<ValueTask<TTarget>> none)
        => await (await option.ConfigureAwait(false)).MatchAsync(some, none).ConfigureAwait(false);
        
    public static ValueTask<TTarget> MatchAsync<TSource1, TSource2, TSource3, TSource4, TTarget>(
        in this Option<(TSource1, TSource2, TSource3, TSource4)> option,
        Func<TSource1, TSource2, TSource3, TSource4, ValueTask<TTarget>> some,
        Func<ValueTask<TTarget>> none)
    {
        return option.IsSome ? some(option.Value.Item1, option.Value.Item2, option.Value.Item3, option.Value.Item4) : none();
    }

    public static async ValueTask<TTarget> MatchAsync<TSource1, TSource2, TSource3, TSource4, TTarget>(
        this ValueTask<Option<(TSource1, TSource2, TSource3, TSource4)>> option,
        Func<TSource1, TSource2, TSource3, TSource4, TTarget> some,
        Func<TTarget> none)
        => (await option.ConfigureAwait(false)).Match(some, none);

    public static async ValueTask<TTarget> MatchAsync<TSource1, TSource2, TSource3, TSource4, TTarget>(
        this ValueTask<Option<(TSource1, TSource2, TSource3, TSource4)>> option,
        Func<TSource1, TSource2, TSource3, TSource4, ValueTask<TTarget>> some,
        Func<ValueTask<TTarget>> none)
        => await (await option.ConfigureAwait(false)).MatchAsync(some, none).ConfigureAwait(false);
        
    public static ValueTask<TTarget> MatchAsync<TSource1, TSource2, TSource3, TSource4, TSource5, TTarget>(
        in this Option<(TSource1, TSource2, TSource3, TSource4, TSource5)> option,
        Func<TSource1, TSource2, TSource3, TSource4, TSource5, ValueTask<TTarget>> some,
        Func<ValueTask<TTarget>> none)
    {
        return option.IsSome ? some(option.Value.Item1, option.Value.Item2, option.Value.Item3, option.Value.Item4, option.Value.Item5) : none();
    }

    public static async ValueTask<TTarget> MatchAsync<TSource1, TSource2, TSource3, TSource4, TSource5, TTarget>(
        this ValueTask<Option<(TSource1, TSource2, TSource3, TSource4, TSource5)>> option,
        Func<TSource1, TSource2, TSource3, TSource4, TSource5, TTarget> some,
        Func<TTarget> none)
        => (await option.ConfigureAwait(false)).Match(some, none);

    public static async ValueTask<TTarget> MatchAsync<TSource1, TSource2, TSource3, TSource4, TSource5, TTarget>(
        this ValueTask<Option<(TSource1, TSource2, TSource3, TSource4, TSource5)>> option,
        Func<TSource1, TSource2, TSource3, TSource4, TSource5, ValueTask<TTarget>> some,
        Func<ValueTask<TTarget>> none)
        => await (await option.ConfigureAwait(false)).MatchAsync(some, none).ConfigureAwait(false);
        
    public static ValueTask<TTarget> MatchAsync<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TTarget>(
        in this Option<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6)> option,
        Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, ValueTask<TTarget>> some,
        Func<ValueTask<TTarget>> none)
    {
        return option.IsSome ? some(option.Value.Item1, option.Value.Item2, option.Value.Item3, option.Value.Item4, option.Value.Item5, option.Value.Item6) : none();
    }

    public static async ValueTask<TTarget> MatchAsync<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TTarget>(
        this ValueTask<Option<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6)>> option,
        Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TTarget> some,
        Func<TTarget> none)
        => (await option.ConfigureAwait(false)).Match(some, none);

    public static async ValueTask<TTarget> MatchAsync<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TTarget>(
        this ValueTask<Option<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6)>> option,
        Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, ValueTask<TTarget>> some,
        Func<ValueTask<TTarget>> none)
        => await (await option.ConfigureAwait(false)).MatchAsync(some, none).ConfigureAwait(false);
        
    public static ValueTask<TTarget> MatchAsync<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TTarget>(
        in this Option<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7)> option,
        Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, ValueTask<TTarget>> some,
        Func<ValueTask<TTarget>> none)
    {
        return option.IsSome ? some(option.Value.Item1, option.Value.Item2, option.Value.Item3, option.Value.Item4, option.Value.Item5, option.Value.Item6, option.Value.Item7) : none();
    }

    public static async ValueTask<TTarget> MatchAsync<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TTarget>(
        this ValueTask<Option<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7)>> option,
        Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TTarget> some,
        Func<TTarget> none)
        => (await option.ConfigureAwait(false)).Match(some, none);

    public static async ValueTask<TTarget> MatchAsync<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TTarget>(
        this ValueTask<Option<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7)>> option,
        Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, ValueTask<TTarget>> some,
        Func<ValueTask<TTarget>> none)
        => await (await option.ConfigureAwait(false)).MatchAsync(some, none).ConfigureAwait(false);
        
    public static ValueTask<TTarget> MatchAsync<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TTarget>(
        in this Option<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8)> option,
        Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, ValueTask<TTarget>> some,
        Func<ValueTask<TTarget>> none)
    {
        return option.IsSome ? some(option.Value.Item1, option.Value.Item2, option.Value.Item3, option.Value.Item4, option.Value.Item5, option.Value.Item6, option.Value.Item7, option.Value.Item8) : none();
    }

    public static async ValueTask<TTarget> MatchAsync<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TTarget>(
        this ValueTask<Option<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8)>> option,
        Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TTarget> some,
        Func<TTarget> none)
        => (await option.ConfigureAwait(false)).Match(some, none);

    public static async ValueTask<TTarget> MatchAsync<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TTarget>(
        this ValueTask<Option<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8)>> option,
        Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, ValueTask<TTarget>> some,
        Func<ValueTask<TTarget>> none)
        => await (await option.ConfigureAwait(false)).MatchAsync(some, none).ConfigureAwait(false);
        
}
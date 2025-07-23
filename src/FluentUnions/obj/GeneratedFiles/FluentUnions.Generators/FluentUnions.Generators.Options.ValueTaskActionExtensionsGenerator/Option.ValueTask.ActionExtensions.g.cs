namespace FluentUnions;

public static partial class OptionExtensions
{
    public static async ValueTask<Option<(TValue1, TValue2)>> OnSomeAsync<TValue1, TValue2>(this Option<(TValue1, TValue2)> option, Func<TValue1, TValue2, ValueTask> action)
    {
        if (option.IsSome) await action(option.Value.Item1, option.Value.Item2).ConfigureAwait(false);
        return option;
    }
    
    public static async ValueTask<Option<(TValue1, TValue2)>> OnSomeAsync<TValue1, TValue2>(this ValueTask<Option<(TValue1, TValue2)>> option, Action<TValue1, TValue2> action)
    => (await option.ConfigureAwait(false)).OnSome(action);
    
    public static async ValueTask<Option<(TValue1, TValue2)>> OnSomeAsync<TValue1, TValue2>(this ValueTask<Option<(TValue1, TValue2)>> option, Func<TValue1, TValue2, ValueTask> action)
    => await (await option.ConfigureAwait(false)).OnSomeAsync(action).ConfigureAwait(false);
    
    public static async ValueTask<Option<(TValue1, TValue2)>> OnEitherAsync<TValue1, TValue2>(this Option<(TValue1, TValue2)> option, Func<TValue1, TValue2, ValueTask> some, Func<ValueTask> none)
    {
        if (option.IsSome) await some(option.Value.Item1, option.Value.Item2).ConfigureAwait(false);
        else await none().ConfigureAwait(false);
        return option;
    }
    
    public static async ValueTask<Option<(TValue1, TValue2)>> OnEitherAsync<TValue1, TValue2>(this ValueTask<Option<(TValue1, TValue2)>> option, Action<TValue1, TValue2> some, Action none)
    => (await option.ConfigureAwait(false)).OnEither(some, none);
    
    public static async ValueTask<Option<(TValue1, TValue2)>> OnEitherAsync<TValue1, TValue2>(this ValueTask<Option<(TValue1, TValue2)>> option, Func<TValue1, TValue2, ValueTask> some, Func<ValueTask> none)
    => await (await option.ConfigureAwait(false)).OnEitherAsync(some, none).ConfigureAwait(false);

    public static async ValueTask<Option<(TValue1, TValue2, TValue3)>> OnSomeAsync<TValue1, TValue2, TValue3>(this Option<(TValue1, TValue2, TValue3)> option, Func<TValue1, TValue2, TValue3, ValueTask> action)
    {
        if (option.IsSome) await action(option.Value.Item1, option.Value.Item2, option.Value.Item3).ConfigureAwait(false);
        return option;
    }
    
    public static async ValueTask<Option<(TValue1, TValue2, TValue3)>> OnSomeAsync<TValue1, TValue2, TValue3>(this ValueTask<Option<(TValue1, TValue2, TValue3)>> option, Action<TValue1, TValue2, TValue3> action)
    => (await option.ConfigureAwait(false)).OnSome(action);
    
    public static async ValueTask<Option<(TValue1, TValue2, TValue3)>> OnSomeAsync<TValue1, TValue2, TValue3>(this ValueTask<Option<(TValue1, TValue2, TValue3)>> option, Func<TValue1, TValue2, TValue3, ValueTask> action)
    => await (await option.ConfigureAwait(false)).OnSomeAsync(action).ConfigureAwait(false);
    
    public static async ValueTask<Option<(TValue1, TValue2, TValue3)>> OnEitherAsync<TValue1, TValue2, TValue3>(this Option<(TValue1, TValue2, TValue3)> option, Func<TValue1, TValue2, TValue3, ValueTask> some, Func<ValueTask> none)
    {
        if (option.IsSome) await some(option.Value.Item1, option.Value.Item2, option.Value.Item3).ConfigureAwait(false);
        else await none().ConfigureAwait(false);
        return option;
    }
    
    public static async ValueTask<Option<(TValue1, TValue2, TValue3)>> OnEitherAsync<TValue1, TValue2, TValue3>(this ValueTask<Option<(TValue1, TValue2, TValue3)>> option, Action<TValue1, TValue2, TValue3> some, Action none)
    => (await option.ConfigureAwait(false)).OnEither(some, none);
    
    public static async ValueTask<Option<(TValue1, TValue2, TValue3)>> OnEitherAsync<TValue1, TValue2, TValue3>(this ValueTask<Option<(TValue1, TValue2, TValue3)>> option, Func<TValue1, TValue2, TValue3, ValueTask> some, Func<ValueTask> none)
    => await (await option.ConfigureAwait(false)).OnEitherAsync(some, none).ConfigureAwait(false);

    public static async ValueTask<Option<(TValue1, TValue2, TValue3, TValue4)>> OnSomeAsync<TValue1, TValue2, TValue3, TValue4>(this Option<(TValue1, TValue2, TValue3, TValue4)> option, Func<TValue1, TValue2, TValue3, TValue4, ValueTask> action)
    {
        if (option.IsSome) await action(option.Value.Item1, option.Value.Item2, option.Value.Item3, option.Value.Item4).ConfigureAwait(false);
        return option;
    }
    
    public static async ValueTask<Option<(TValue1, TValue2, TValue3, TValue4)>> OnSomeAsync<TValue1, TValue2, TValue3, TValue4>(this ValueTask<Option<(TValue1, TValue2, TValue3, TValue4)>> option, Action<TValue1, TValue2, TValue3, TValue4> action)
    => (await option.ConfigureAwait(false)).OnSome(action);
    
    public static async ValueTask<Option<(TValue1, TValue2, TValue3, TValue4)>> OnSomeAsync<TValue1, TValue2, TValue3, TValue4>(this ValueTask<Option<(TValue1, TValue2, TValue3, TValue4)>> option, Func<TValue1, TValue2, TValue3, TValue4, ValueTask> action)
    => await (await option.ConfigureAwait(false)).OnSomeAsync(action).ConfigureAwait(false);
    
    public static async ValueTask<Option<(TValue1, TValue2, TValue3, TValue4)>> OnEitherAsync<TValue1, TValue2, TValue3, TValue4>(this Option<(TValue1, TValue2, TValue3, TValue4)> option, Func<TValue1, TValue2, TValue3, TValue4, ValueTask> some, Func<ValueTask> none)
    {
        if (option.IsSome) await some(option.Value.Item1, option.Value.Item2, option.Value.Item3, option.Value.Item4).ConfigureAwait(false);
        else await none().ConfigureAwait(false);
        return option;
    }
    
    public static async ValueTask<Option<(TValue1, TValue2, TValue3, TValue4)>> OnEitherAsync<TValue1, TValue2, TValue3, TValue4>(this ValueTask<Option<(TValue1, TValue2, TValue3, TValue4)>> option, Action<TValue1, TValue2, TValue3, TValue4> some, Action none)
    => (await option.ConfigureAwait(false)).OnEither(some, none);
    
    public static async ValueTask<Option<(TValue1, TValue2, TValue3, TValue4)>> OnEitherAsync<TValue1, TValue2, TValue3, TValue4>(this ValueTask<Option<(TValue1, TValue2, TValue3, TValue4)>> option, Func<TValue1, TValue2, TValue3, TValue4, ValueTask> some, Func<ValueTask> none)
    => await (await option.ConfigureAwait(false)).OnEitherAsync(some, none).ConfigureAwait(false);

    public static async ValueTask<Option<(TValue1, TValue2, TValue3, TValue4, TValue5)>> OnSomeAsync<TValue1, TValue2, TValue3, TValue4, TValue5>(this Option<(TValue1, TValue2, TValue3, TValue4, TValue5)> option, Func<TValue1, TValue2, TValue3, TValue4, TValue5, ValueTask> action)
    {
        if (option.IsSome) await action(option.Value.Item1, option.Value.Item2, option.Value.Item3, option.Value.Item4, option.Value.Item5).ConfigureAwait(false);
        return option;
    }
    
    public static async ValueTask<Option<(TValue1, TValue2, TValue3, TValue4, TValue5)>> OnSomeAsync<TValue1, TValue2, TValue3, TValue4, TValue5>(this ValueTask<Option<(TValue1, TValue2, TValue3, TValue4, TValue5)>> option, Action<TValue1, TValue2, TValue3, TValue4, TValue5> action)
    => (await option.ConfigureAwait(false)).OnSome(action);
    
    public static async ValueTask<Option<(TValue1, TValue2, TValue3, TValue4, TValue5)>> OnSomeAsync<TValue1, TValue2, TValue3, TValue4, TValue5>(this ValueTask<Option<(TValue1, TValue2, TValue3, TValue4, TValue5)>> option, Func<TValue1, TValue2, TValue3, TValue4, TValue5, ValueTask> action)
    => await (await option.ConfigureAwait(false)).OnSomeAsync(action).ConfigureAwait(false);
    
    public static async ValueTask<Option<(TValue1, TValue2, TValue3, TValue4, TValue5)>> OnEitherAsync<TValue1, TValue2, TValue3, TValue4, TValue5>(this Option<(TValue1, TValue2, TValue3, TValue4, TValue5)> option, Func<TValue1, TValue2, TValue3, TValue4, TValue5, ValueTask> some, Func<ValueTask> none)
    {
        if (option.IsSome) await some(option.Value.Item1, option.Value.Item2, option.Value.Item3, option.Value.Item4, option.Value.Item5).ConfigureAwait(false);
        else await none().ConfigureAwait(false);
        return option;
    }
    
    public static async ValueTask<Option<(TValue1, TValue2, TValue3, TValue4, TValue5)>> OnEitherAsync<TValue1, TValue2, TValue3, TValue4, TValue5>(this ValueTask<Option<(TValue1, TValue2, TValue3, TValue4, TValue5)>> option, Action<TValue1, TValue2, TValue3, TValue4, TValue5> some, Action none)
    => (await option.ConfigureAwait(false)).OnEither(some, none);
    
    public static async ValueTask<Option<(TValue1, TValue2, TValue3, TValue4, TValue5)>> OnEitherAsync<TValue1, TValue2, TValue3, TValue4, TValue5>(this ValueTask<Option<(TValue1, TValue2, TValue3, TValue4, TValue5)>> option, Func<TValue1, TValue2, TValue3, TValue4, TValue5, ValueTask> some, Func<ValueTask> none)
    => await (await option.ConfigureAwait(false)).OnEitherAsync(some, none).ConfigureAwait(false);

    public static async ValueTask<Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)>> OnSomeAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(this Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)> option, Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, ValueTask> action)
    {
        if (option.IsSome) await action(option.Value.Item1, option.Value.Item2, option.Value.Item3, option.Value.Item4, option.Value.Item5, option.Value.Item6).ConfigureAwait(false);
        return option;
    }
    
    public static async ValueTask<Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)>> OnSomeAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(this ValueTask<Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)>> option, Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> action)
    => (await option.ConfigureAwait(false)).OnSome(action);
    
    public static async ValueTask<Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)>> OnSomeAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(this ValueTask<Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)>> option, Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, ValueTask> action)
    => await (await option.ConfigureAwait(false)).OnSomeAsync(action).ConfigureAwait(false);
    
    public static async ValueTask<Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)>> OnEitherAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(this Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)> option, Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, ValueTask> some, Func<ValueTask> none)
    {
        if (option.IsSome) await some(option.Value.Item1, option.Value.Item2, option.Value.Item3, option.Value.Item4, option.Value.Item5, option.Value.Item6).ConfigureAwait(false);
        else await none().ConfigureAwait(false);
        return option;
    }
    
    public static async ValueTask<Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)>> OnEitherAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(this ValueTask<Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)>> option, Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> some, Action none)
    => (await option.ConfigureAwait(false)).OnEither(some, none);
    
    public static async ValueTask<Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)>> OnEitherAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(this ValueTask<Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)>> option, Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, ValueTask> some, Func<ValueTask> none)
    => await (await option.ConfigureAwait(false)).OnEitherAsync(some, none).ConfigureAwait(false);

    public static async ValueTask<Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)>> OnSomeAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(this Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)> option, Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, ValueTask> action)
    {
        if (option.IsSome) await action(option.Value.Item1, option.Value.Item2, option.Value.Item3, option.Value.Item4, option.Value.Item5, option.Value.Item6, option.Value.Item7).ConfigureAwait(false);
        return option;
    }
    
    public static async ValueTask<Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)>> OnSomeAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(this ValueTask<Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)>> option, Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> action)
    => (await option.ConfigureAwait(false)).OnSome(action);
    
    public static async ValueTask<Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)>> OnSomeAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(this ValueTask<Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)>> option, Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, ValueTask> action)
    => await (await option.ConfigureAwait(false)).OnSomeAsync(action).ConfigureAwait(false);
    
    public static async ValueTask<Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)>> OnEitherAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(this Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)> option, Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, ValueTask> some, Func<ValueTask> none)
    {
        if (option.IsSome) await some(option.Value.Item1, option.Value.Item2, option.Value.Item3, option.Value.Item4, option.Value.Item5, option.Value.Item6, option.Value.Item7).ConfigureAwait(false);
        else await none().ConfigureAwait(false);
        return option;
    }
    
    public static async ValueTask<Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)>> OnEitherAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(this ValueTask<Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)>> option, Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> some, Action none)
    => (await option.ConfigureAwait(false)).OnEither(some, none);
    
    public static async ValueTask<Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)>> OnEitherAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(this ValueTask<Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)>> option, Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, ValueTask> some, Func<ValueTask> none)
    => await (await option.ConfigureAwait(false)).OnEitherAsync(some, none).ConfigureAwait(false);

    public static async ValueTask<Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)>> OnSomeAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(this Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)> option, Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, ValueTask> action)
    {
        if (option.IsSome) await action(option.Value.Item1, option.Value.Item2, option.Value.Item3, option.Value.Item4, option.Value.Item5, option.Value.Item6, option.Value.Item7, option.Value.Item8).ConfigureAwait(false);
        return option;
    }
    
    public static async ValueTask<Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)>> OnSomeAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(this ValueTask<Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)>> option, Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> action)
    => (await option.ConfigureAwait(false)).OnSome(action);
    
    public static async ValueTask<Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)>> OnSomeAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(this ValueTask<Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)>> option, Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, ValueTask> action)
    => await (await option.ConfigureAwait(false)).OnSomeAsync(action).ConfigureAwait(false);
    
    public static async ValueTask<Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)>> OnEitherAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(this Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)> option, Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, ValueTask> some, Func<ValueTask> none)
    {
        if (option.IsSome) await some(option.Value.Item1, option.Value.Item2, option.Value.Item3, option.Value.Item4, option.Value.Item5, option.Value.Item6, option.Value.Item7, option.Value.Item8).ConfigureAwait(false);
        else await none().ConfigureAwait(false);
        return option;
    }
    
    public static async ValueTask<Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)>> OnEitherAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(this ValueTask<Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)>> option, Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> some, Action none)
    => (await option.ConfigureAwait(false)).OnEither(some, none);
    
    public static async ValueTask<Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)>> OnEitherAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(this ValueTask<Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)>> option, Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, ValueTask> some, Func<ValueTask> none)
    => await (await option.ConfigureAwait(false)).OnEitherAsync(some, none).ConfigureAwait(false);

}
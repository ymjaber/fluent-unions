namespace FluentUnions;

public static partial class OptionExtensions
{
    public static async Task<Option<(TValue1, TValue2)>> OnSomeAsync<TValue1, TValue2>(this Option<(TValue1, TValue2)> option, Func<TValue1, TValue2, Task> action)
    {
        if (option.IsSome) await action(option.Value.Item1, option.Value.Item2).ConfigureAwait(false);
        return option;
    }
    
    public static async Task<Option<(TValue1, TValue2)>> OnSomeAsync<TValue1, TValue2>(this Task<Option<(TValue1, TValue2)>> option, Action<TValue1, TValue2> action)
    => (await option.ConfigureAwait(false)).OnSome(action);
    
    public static async Task<Option<(TValue1, TValue2)>> OnSomeAsync<TValue1, TValue2>(this Task<Option<(TValue1, TValue2)>> option, Func<TValue1, TValue2, Task> action)
    => await (await option.ConfigureAwait(false)).OnSomeAsync(action).ConfigureAwait(false);
    
    public static async Task<Option<(TValue1, TValue2)>> OnEitherAsync<TValue1, TValue2>(this Option<(TValue1, TValue2)> option, Func<TValue1, TValue2, Task> some, Func<Task> none)
    {
        if (option.IsSome) await some(option.Value.Item1, option.Value.Item2).ConfigureAwait(false);
        else await none().ConfigureAwait(false);
        return option;
    }
    
    public static async Task<Option<(TValue1, TValue2)>> OnEitherAsync<TValue1, TValue2>(this Task<Option<(TValue1, TValue2)>> option, Action<TValue1, TValue2> some, Action none)
    => (await option.ConfigureAwait(false)).OnEither(some, none);
    
    public static async Task<Option<(TValue1, TValue2)>> OnEitherAsync<TValue1, TValue2>(this Task<Option<(TValue1, TValue2)>> option, Func<TValue1, TValue2, Task> some, Func<Task> none)
    => await (await option.ConfigureAwait(false)).OnEitherAsync(some, none).ConfigureAwait(false);

    public static async Task<Option<(TValue1, TValue2, TValue3)>> OnSomeAsync<TValue1, TValue2, TValue3>(this Option<(TValue1, TValue2, TValue3)> option, Func<TValue1, TValue2, TValue3, Task> action)
    {
        if (option.IsSome) await action(option.Value.Item1, option.Value.Item2, option.Value.Item3).ConfigureAwait(false);
        return option;
    }
    
    public static async Task<Option<(TValue1, TValue2, TValue3)>> OnSomeAsync<TValue1, TValue2, TValue3>(this Task<Option<(TValue1, TValue2, TValue3)>> option, Action<TValue1, TValue2, TValue3> action)
    => (await option.ConfigureAwait(false)).OnSome(action);
    
    public static async Task<Option<(TValue1, TValue2, TValue3)>> OnSomeAsync<TValue1, TValue2, TValue3>(this Task<Option<(TValue1, TValue2, TValue3)>> option, Func<TValue1, TValue2, TValue3, Task> action)
    => await (await option.ConfigureAwait(false)).OnSomeAsync(action).ConfigureAwait(false);
    
    public static async Task<Option<(TValue1, TValue2, TValue3)>> OnEitherAsync<TValue1, TValue2, TValue3>(this Option<(TValue1, TValue2, TValue3)> option, Func<TValue1, TValue2, TValue3, Task> some, Func<Task> none)
    {
        if (option.IsSome) await some(option.Value.Item1, option.Value.Item2, option.Value.Item3).ConfigureAwait(false);
        else await none().ConfigureAwait(false);
        return option;
    }
    
    public static async Task<Option<(TValue1, TValue2, TValue3)>> OnEitherAsync<TValue1, TValue2, TValue3>(this Task<Option<(TValue1, TValue2, TValue3)>> option, Action<TValue1, TValue2, TValue3> some, Action none)
    => (await option.ConfigureAwait(false)).OnEither(some, none);
    
    public static async Task<Option<(TValue1, TValue2, TValue3)>> OnEitherAsync<TValue1, TValue2, TValue3>(this Task<Option<(TValue1, TValue2, TValue3)>> option, Func<TValue1, TValue2, TValue3, Task> some, Func<Task> none)
    => await (await option.ConfigureAwait(false)).OnEitherAsync(some, none).ConfigureAwait(false);

    public static async Task<Option<(TValue1, TValue2, TValue3, TValue4)>> OnSomeAsync<TValue1, TValue2, TValue3, TValue4>(this Option<(TValue1, TValue2, TValue3, TValue4)> option, Func<TValue1, TValue2, TValue3, TValue4, Task> action)
    {
        if (option.IsSome) await action(option.Value.Item1, option.Value.Item2, option.Value.Item3, option.Value.Item4).ConfigureAwait(false);
        return option;
    }
    
    public static async Task<Option<(TValue1, TValue2, TValue3, TValue4)>> OnSomeAsync<TValue1, TValue2, TValue3, TValue4>(this Task<Option<(TValue1, TValue2, TValue3, TValue4)>> option, Action<TValue1, TValue2, TValue3, TValue4> action)
    => (await option.ConfigureAwait(false)).OnSome(action);
    
    public static async Task<Option<(TValue1, TValue2, TValue3, TValue4)>> OnSomeAsync<TValue1, TValue2, TValue3, TValue4>(this Task<Option<(TValue1, TValue2, TValue3, TValue4)>> option, Func<TValue1, TValue2, TValue3, TValue4, Task> action)
    => await (await option.ConfigureAwait(false)).OnSomeAsync(action).ConfigureAwait(false);
    
    public static async Task<Option<(TValue1, TValue2, TValue3, TValue4)>> OnEitherAsync<TValue1, TValue2, TValue3, TValue4>(this Option<(TValue1, TValue2, TValue3, TValue4)> option, Func<TValue1, TValue2, TValue3, TValue4, Task> some, Func<Task> none)
    {
        if (option.IsSome) await some(option.Value.Item1, option.Value.Item2, option.Value.Item3, option.Value.Item4).ConfigureAwait(false);
        else await none().ConfigureAwait(false);
        return option;
    }
    
    public static async Task<Option<(TValue1, TValue2, TValue3, TValue4)>> OnEitherAsync<TValue1, TValue2, TValue3, TValue4>(this Task<Option<(TValue1, TValue2, TValue3, TValue4)>> option, Action<TValue1, TValue2, TValue3, TValue4> some, Action none)
    => (await option.ConfigureAwait(false)).OnEither(some, none);
    
    public static async Task<Option<(TValue1, TValue2, TValue3, TValue4)>> OnEitherAsync<TValue1, TValue2, TValue3, TValue4>(this Task<Option<(TValue1, TValue2, TValue3, TValue4)>> option, Func<TValue1, TValue2, TValue3, TValue4, Task> some, Func<Task> none)
    => await (await option.ConfigureAwait(false)).OnEitherAsync(some, none).ConfigureAwait(false);

    public static async Task<Option<(TValue1, TValue2, TValue3, TValue4, TValue5)>> OnSomeAsync<TValue1, TValue2, TValue3, TValue4, TValue5>(this Option<(TValue1, TValue2, TValue3, TValue4, TValue5)> option, Func<TValue1, TValue2, TValue3, TValue4, TValue5, Task> action)
    {
        if (option.IsSome) await action(option.Value.Item1, option.Value.Item2, option.Value.Item3, option.Value.Item4, option.Value.Item5).ConfigureAwait(false);
        return option;
    }
    
    public static async Task<Option<(TValue1, TValue2, TValue3, TValue4, TValue5)>> OnSomeAsync<TValue1, TValue2, TValue3, TValue4, TValue5>(this Task<Option<(TValue1, TValue2, TValue3, TValue4, TValue5)>> option, Action<TValue1, TValue2, TValue3, TValue4, TValue5> action)
    => (await option.ConfigureAwait(false)).OnSome(action);
    
    public static async Task<Option<(TValue1, TValue2, TValue3, TValue4, TValue5)>> OnSomeAsync<TValue1, TValue2, TValue3, TValue4, TValue5>(this Task<Option<(TValue1, TValue2, TValue3, TValue4, TValue5)>> option, Func<TValue1, TValue2, TValue3, TValue4, TValue5, Task> action)
    => await (await option.ConfigureAwait(false)).OnSomeAsync(action).ConfigureAwait(false);
    
    public static async Task<Option<(TValue1, TValue2, TValue3, TValue4, TValue5)>> OnEitherAsync<TValue1, TValue2, TValue3, TValue4, TValue5>(this Option<(TValue1, TValue2, TValue3, TValue4, TValue5)> option, Func<TValue1, TValue2, TValue3, TValue4, TValue5, Task> some, Func<Task> none)
    {
        if (option.IsSome) await some(option.Value.Item1, option.Value.Item2, option.Value.Item3, option.Value.Item4, option.Value.Item5).ConfigureAwait(false);
        else await none().ConfigureAwait(false);
        return option;
    }
    
    public static async Task<Option<(TValue1, TValue2, TValue3, TValue4, TValue5)>> OnEitherAsync<TValue1, TValue2, TValue3, TValue4, TValue5>(this Task<Option<(TValue1, TValue2, TValue3, TValue4, TValue5)>> option, Action<TValue1, TValue2, TValue3, TValue4, TValue5> some, Action none)
    => (await option.ConfigureAwait(false)).OnEither(some, none);
    
    public static async Task<Option<(TValue1, TValue2, TValue3, TValue4, TValue5)>> OnEitherAsync<TValue1, TValue2, TValue3, TValue4, TValue5>(this Task<Option<(TValue1, TValue2, TValue3, TValue4, TValue5)>> option, Func<TValue1, TValue2, TValue3, TValue4, TValue5, Task> some, Func<Task> none)
    => await (await option.ConfigureAwait(false)).OnEitherAsync(some, none).ConfigureAwait(false);

    public static async Task<Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)>> OnSomeAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(this Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)> option, Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, Task> action)
    {
        if (option.IsSome) await action(option.Value.Item1, option.Value.Item2, option.Value.Item3, option.Value.Item4, option.Value.Item5, option.Value.Item6).ConfigureAwait(false);
        return option;
    }
    
    public static async Task<Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)>> OnSomeAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(this Task<Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)>> option, Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> action)
    => (await option.ConfigureAwait(false)).OnSome(action);
    
    public static async Task<Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)>> OnSomeAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(this Task<Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)>> option, Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, Task> action)
    => await (await option.ConfigureAwait(false)).OnSomeAsync(action).ConfigureAwait(false);
    
    public static async Task<Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)>> OnEitherAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(this Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)> option, Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, Task> some, Func<Task> none)
    {
        if (option.IsSome) await some(option.Value.Item1, option.Value.Item2, option.Value.Item3, option.Value.Item4, option.Value.Item5, option.Value.Item6).ConfigureAwait(false);
        else await none().ConfigureAwait(false);
        return option;
    }
    
    public static async Task<Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)>> OnEitherAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(this Task<Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)>> option, Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> some, Action none)
    => (await option.ConfigureAwait(false)).OnEither(some, none);
    
    public static async Task<Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)>> OnEitherAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(this Task<Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)>> option, Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, Task> some, Func<Task> none)
    => await (await option.ConfigureAwait(false)).OnEitherAsync(some, none).ConfigureAwait(false);

    public static async Task<Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)>> OnSomeAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(this Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)> option, Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, Task> action)
    {
        if (option.IsSome) await action(option.Value.Item1, option.Value.Item2, option.Value.Item3, option.Value.Item4, option.Value.Item5, option.Value.Item6, option.Value.Item7).ConfigureAwait(false);
        return option;
    }
    
    public static async Task<Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)>> OnSomeAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(this Task<Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)>> option, Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> action)
    => (await option.ConfigureAwait(false)).OnSome(action);
    
    public static async Task<Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)>> OnSomeAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(this Task<Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)>> option, Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, Task> action)
    => await (await option.ConfigureAwait(false)).OnSomeAsync(action).ConfigureAwait(false);
    
    public static async Task<Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)>> OnEitherAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(this Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)> option, Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, Task> some, Func<Task> none)
    {
        if (option.IsSome) await some(option.Value.Item1, option.Value.Item2, option.Value.Item3, option.Value.Item4, option.Value.Item5, option.Value.Item6, option.Value.Item7).ConfigureAwait(false);
        else await none().ConfigureAwait(false);
        return option;
    }
    
    public static async Task<Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)>> OnEitherAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(this Task<Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)>> option, Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> some, Action none)
    => (await option.ConfigureAwait(false)).OnEither(some, none);
    
    public static async Task<Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)>> OnEitherAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(this Task<Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)>> option, Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, Task> some, Func<Task> none)
    => await (await option.ConfigureAwait(false)).OnEitherAsync(some, none).ConfigureAwait(false);

    public static async Task<Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)>> OnSomeAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(this Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)> option, Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, Task> action)
    {
        if (option.IsSome) await action(option.Value.Item1, option.Value.Item2, option.Value.Item3, option.Value.Item4, option.Value.Item5, option.Value.Item6, option.Value.Item7, option.Value.Item8).ConfigureAwait(false);
        return option;
    }
    
    public static async Task<Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)>> OnSomeAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(this Task<Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)>> option, Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> action)
    => (await option.ConfigureAwait(false)).OnSome(action);
    
    public static async Task<Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)>> OnSomeAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(this Task<Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)>> option, Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, Task> action)
    => await (await option.ConfigureAwait(false)).OnSomeAsync(action).ConfigureAwait(false);
    
    public static async Task<Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)>> OnEitherAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(this Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)> option, Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, Task> some, Func<Task> none)
    {
        if (option.IsSome) await some(option.Value.Item1, option.Value.Item2, option.Value.Item3, option.Value.Item4, option.Value.Item5, option.Value.Item6, option.Value.Item7, option.Value.Item8).ConfigureAwait(false);
        else await none().ConfigureAwait(false);
        return option;
    }
    
    public static async Task<Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)>> OnEitherAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(this Task<Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)>> option, Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> some, Action none)
    => (await option.ConfigureAwait(false)).OnEither(some, none);
    
    public static async Task<Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)>> OnEitherAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(this Task<Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)>> option, Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, Task> some, Func<Task> none)
    => await (await option.ConfigureAwait(false)).OnEitherAsync(some, none).ConfigureAwait(false);

}
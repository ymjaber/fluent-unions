namespace FluentUnions;

public static partial class OptionExtensions
{
    public static Option<(TValue1, TValue2)> OnSome<TValue1, TValue2>(in this Option<(TValue1, TValue2)> option, Action<TValue1, TValue2> action)
    {
        if (option.IsSome) action(option.Value.Item1, option.Value.Item2);
        return option;
    }
    
    public static Option<(TValue1, TValue2)> OnEither<TValue1, TValue2>(in this Option<(TValue1, TValue2)> option, Action<TValue1, TValue2> some, Action none)
    {
        if (option.IsSome) some(option.Value.Item1, option.Value.Item2);
        else none();
        return option;
    }

    public static Option<(TValue1, TValue2, TValue3)> OnSome<TValue1, TValue2, TValue3>(in this Option<(TValue1, TValue2, TValue3)> option, Action<TValue1, TValue2, TValue3> action)
    {
        if (option.IsSome) action(option.Value.Item1, option.Value.Item2, option.Value.Item3);
        return option;
    }
    
    public static Option<(TValue1, TValue2, TValue3)> OnEither<TValue1, TValue2, TValue3>(in this Option<(TValue1, TValue2, TValue3)> option, Action<TValue1, TValue2, TValue3> some, Action none)
    {
        if (option.IsSome) some(option.Value.Item1, option.Value.Item2, option.Value.Item3);
        else none();
        return option;
    }

    public static Option<(TValue1, TValue2, TValue3, TValue4)> OnSome<TValue1, TValue2, TValue3, TValue4>(in this Option<(TValue1, TValue2, TValue3, TValue4)> option, Action<TValue1, TValue2, TValue3, TValue4> action)
    {
        if (option.IsSome) action(option.Value.Item1, option.Value.Item2, option.Value.Item3, option.Value.Item4);
        return option;
    }
    
    public static Option<(TValue1, TValue2, TValue3, TValue4)> OnEither<TValue1, TValue2, TValue3, TValue4>(in this Option<(TValue1, TValue2, TValue3, TValue4)> option, Action<TValue1, TValue2, TValue3, TValue4> some, Action none)
    {
        if (option.IsSome) some(option.Value.Item1, option.Value.Item2, option.Value.Item3, option.Value.Item4);
        else none();
        return option;
    }

    public static Option<(TValue1, TValue2, TValue3, TValue4, TValue5)> OnSome<TValue1, TValue2, TValue3, TValue4, TValue5>(in this Option<(TValue1, TValue2, TValue3, TValue4, TValue5)> option, Action<TValue1, TValue2, TValue3, TValue4, TValue5> action)
    {
        if (option.IsSome) action(option.Value.Item1, option.Value.Item2, option.Value.Item3, option.Value.Item4, option.Value.Item5);
        return option;
    }
    
    public static Option<(TValue1, TValue2, TValue3, TValue4, TValue5)> OnEither<TValue1, TValue2, TValue3, TValue4, TValue5>(in this Option<(TValue1, TValue2, TValue3, TValue4, TValue5)> option, Action<TValue1, TValue2, TValue3, TValue4, TValue5> some, Action none)
    {
        if (option.IsSome) some(option.Value.Item1, option.Value.Item2, option.Value.Item3, option.Value.Item4, option.Value.Item5);
        else none();
        return option;
    }

    public static Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)> OnSome<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(in this Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)> option, Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> action)
    {
        if (option.IsSome) action(option.Value.Item1, option.Value.Item2, option.Value.Item3, option.Value.Item4, option.Value.Item5, option.Value.Item6);
        return option;
    }
    
    public static Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)> OnEither<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(in this Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)> option, Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> some, Action none)
    {
        if (option.IsSome) some(option.Value.Item1, option.Value.Item2, option.Value.Item3, option.Value.Item4, option.Value.Item5, option.Value.Item6);
        else none();
        return option;
    }

    public static Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)> OnSome<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(in this Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)> option, Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> action)
    {
        if (option.IsSome) action(option.Value.Item1, option.Value.Item2, option.Value.Item3, option.Value.Item4, option.Value.Item5, option.Value.Item6, option.Value.Item7);
        return option;
    }
    
    public static Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)> OnEither<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(in this Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)> option, Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> some, Action none)
    {
        if (option.IsSome) some(option.Value.Item1, option.Value.Item2, option.Value.Item3, option.Value.Item4, option.Value.Item5, option.Value.Item6, option.Value.Item7);
        else none();
        return option;
    }

    public static Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)> OnSome<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(in this Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)> option, Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> action)
    {
        if (option.IsSome) action(option.Value.Item1, option.Value.Item2, option.Value.Item3, option.Value.Item4, option.Value.Item5, option.Value.Item6, option.Value.Item7, option.Value.Item8);
        return option;
    }
    
    public static Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)> OnEither<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(in this Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)> option, Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> some, Action none)
    {
        if (option.IsSome) some(option.Value.Item1, option.Value.Item2, option.Value.Item3, option.Value.Item4, option.Value.Item5, option.Value.Item6, option.Value.Item7, option.Value.Item8);
        else none();
        return option;
    }

}
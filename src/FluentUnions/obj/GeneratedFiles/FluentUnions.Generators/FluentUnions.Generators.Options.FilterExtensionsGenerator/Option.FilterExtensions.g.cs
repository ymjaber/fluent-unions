namespace FluentUnions;

public static partial class OptionExtensions
{
    public static Option<(TValue1, TValue2)> Filter<TValue1, TValue2>(
        in this Option<(TValue1, TValue2)> option,
        Func<TValue1, TValue2, bool> predicate)
    {
        if (option.IsNone) return option;
    
        return predicate(option.Value.Item1, option.Value.Item2) ? option : Option<(TValue1, TValue2)>.None;
    }

    public static Option<(TValue1, TValue2, TValue3)> Filter<TValue1, TValue2, TValue3>(
        in this Option<(TValue1, TValue2, TValue3)> option,
        Func<TValue1, TValue2, TValue3, bool> predicate)
    {
        if (option.IsNone) return option;
    
        return predicate(option.Value.Item1, option.Value.Item2, option.Value.Item3) ? option : Option<(TValue1, TValue2, TValue3)>.None;
    }

    public static Option<(TValue1, TValue2, TValue3, TValue4)> Filter<TValue1, TValue2, TValue3, TValue4>(
        in this Option<(TValue1, TValue2, TValue3, TValue4)> option,
        Func<TValue1, TValue2, TValue3, TValue4, bool> predicate)
    {
        if (option.IsNone) return option;
    
        return predicate(option.Value.Item1, option.Value.Item2, option.Value.Item3, option.Value.Item4) ? option : Option<(TValue1, TValue2, TValue3, TValue4)>.None;
    }

    public static Option<(TValue1, TValue2, TValue3, TValue4, TValue5)> Filter<TValue1, TValue2, TValue3, TValue4, TValue5>(
        in this Option<(TValue1, TValue2, TValue3, TValue4, TValue5)> option,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, bool> predicate)
    {
        if (option.IsNone) return option;
    
        return predicate(option.Value.Item1, option.Value.Item2, option.Value.Item3, option.Value.Item4, option.Value.Item5) ? option : Option<(TValue1, TValue2, TValue3, TValue4, TValue5)>.None;
    }

    public static Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)> Filter<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(
        in this Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)> option,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, bool> predicate)
    {
        if (option.IsNone) return option;
    
        return predicate(option.Value.Item1, option.Value.Item2, option.Value.Item3, option.Value.Item4, option.Value.Item5, option.Value.Item6) ? option : Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)>.None;
    }

    public static Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)> Filter<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(
        in this Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)> option,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, bool> predicate)
    {
        if (option.IsNone) return option;
    
        return predicate(option.Value.Item1, option.Value.Item2, option.Value.Item3, option.Value.Item4, option.Value.Item5, option.Value.Item6, option.Value.Item7) ? option : Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)>.None;
    }

    public static Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)> Filter<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(
        in this Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)> option,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, bool> predicate)
    {
        if (option.IsNone) return option;
    
        return predicate(option.Value.Item1, option.Value.Item2, option.Value.Item3, option.Value.Item4, option.Value.Item5, option.Value.Item6, option.Value.Item7, option.Value.Item8) ? option : Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)>.None;
    }

}
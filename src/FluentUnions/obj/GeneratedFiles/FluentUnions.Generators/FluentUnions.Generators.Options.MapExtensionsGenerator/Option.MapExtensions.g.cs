namespace FluentUnions;

public static partial class OptionExtensions
{
    public static Option<TTarget> Map<TValue1, TValue2, TTarget>(
        in this Option<(TValue1, TValue2)> source,
        Func<TValue1, TValue2, TTarget> mapper)
    {
        if (source.IsNone) return Option<TTarget>.None;
        return mapper(source.Value.Item1, source.Value.Item2);
    }
    
    public static Option<TTarget> Map<TValue1, TValue2, TValue3, TTarget>(
        in this Option<(TValue1, TValue2, TValue3)> source,
        Func<TValue1, TValue2, TValue3, TTarget> mapper)
    {
        if (source.IsNone) return Option<TTarget>.None;
        return mapper(source.Value.Item1, source.Value.Item2, source.Value.Item3);
    }
    
    public static Option<TTarget> Map<TValue1, TValue2, TValue3, TValue4, TTarget>(
        in this Option<(TValue1, TValue2, TValue3, TValue4)> source,
        Func<TValue1, TValue2, TValue3, TValue4, TTarget> mapper)
    {
        if (source.IsNone) return Option<TTarget>.None;
        return mapper(source.Value.Item1, source.Value.Item2, source.Value.Item3, source.Value.Item4);
    }
    
    public static Option<TTarget> Map<TValue1, TValue2, TValue3, TValue4, TValue5, TTarget>(
        in this Option<(TValue1, TValue2, TValue3, TValue4, TValue5)> source,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TTarget> mapper)
    {
        if (source.IsNone) return Option<TTarget>.None;
        return mapper(source.Value.Item1, source.Value.Item2, source.Value.Item3, source.Value.Item4, source.Value.Item5);
    }
    
    public static Option<TTarget> Map<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TTarget>(
        in this Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)> source,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TTarget> mapper)
    {
        if (source.IsNone) return Option<TTarget>.None;
        return mapper(source.Value.Item1, source.Value.Item2, source.Value.Item3, source.Value.Item4, source.Value.Item5, source.Value.Item6);
    }
    
    public static Option<TTarget> Map<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TTarget>(
        in this Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)> source,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TTarget> mapper)
    {
        if (source.IsNone) return Option<TTarget>.None;
        return mapper(source.Value.Item1, source.Value.Item2, source.Value.Item3, source.Value.Item4, source.Value.Item5, source.Value.Item6, source.Value.Item7);
    }
    
    public static Option<TTarget> Map<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, TTarget>(
        in this Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)> source,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, TTarget> mapper)
    {
        if (source.IsNone) return Option<TTarget>.None;
        return mapper(source.Value.Item1, source.Value.Item2, source.Value.Item3, source.Value.Item4, source.Value.Item5, source.Value.Item6, source.Value.Item7, source.Value.Item8);
    }
    
}
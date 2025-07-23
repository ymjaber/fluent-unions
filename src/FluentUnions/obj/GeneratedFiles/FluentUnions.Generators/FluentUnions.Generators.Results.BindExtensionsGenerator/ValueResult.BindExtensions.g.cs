namespace FluentUnions;

public static partial class ValueResultExtensions
{
    public static Result<TTarget> Bind<TValue1, TValue2, TTarget>(
        in this Result<(TValue1, TValue2)> source,
        Func<TValue1, TValue2, Result<TTarget>> binder)
    {
        if (source.IsFailure) return source.Error;
        return binder(source.Value.Item1, source.Value.Item2);
    }
    
    public static Result<TTarget> Bind<TValue1, TValue2, TValue3, TTarget>(
        in this Result<(TValue1, TValue2, TValue3)> source,
        Func<TValue1, TValue2, TValue3, Result<TTarget>> binder)
    {
        if (source.IsFailure) return source.Error;
        return binder(source.Value.Item1, source.Value.Item2, source.Value.Item3);
    }
    
    public static Result<TTarget> Bind<TValue1, TValue2, TValue3, TValue4, TTarget>(
        in this Result<(TValue1, TValue2, TValue3, TValue4)> source,
        Func<TValue1, TValue2, TValue3, TValue4, Result<TTarget>> binder)
    {
        if (source.IsFailure) return source.Error;
        return binder(source.Value.Item1, source.Value.Item2, source.Value.Item3, source.Value.Item4);
    }
    
    public static Result<TTarget> Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TTarget>(
        in this Result<(TValue1, TValue2, TValue3, TValue4, TValue5)> source,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, Result<TTarget>> binder)
    {
        if (source.IsFailure) return source.Error;
        return binder(source.Value.Item1, source.Value.Item2, source.Value.Item3, source.Value.Item4, source.Value.Item5);
    }
    
    public static Result<TTarget> Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TTarget>(
        in this Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)> source,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, Result<TTarget>> binder)
    {
        if (source.IsFailure) return source.Error;
        return binder(source.Value.Item1, source.Value.Item2, source.Value.Item3, source.Value.Item4, source.Value.Item5, source.Value.Item6);
    }
    
    public static Result<TTarget> Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TTarget>(
        in this Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)> source,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, Result<TTarget>> binder)
    {
        if (source.IsFailure) return source.Error;
        return binder(source.Value.Item1, source.Value.Item2, source.Value.Item3, source.Value.Item4, source.Value.Item5, source.Value.Item6, source.Value.Item7);
    }
    
    public static Result<TTarget> Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, TTarget>(
        in this Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)> source,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, Result<TTarget>> binder)
    {
        if (source.IsFailure) return source.Error;
        return binder(source.Value.Item1, source.Value.Item2, source.Value.Item3, source.Value.Item4, source.Value.Item5, source.Value.Item6, source.Value.Item7, source.Value.Item8);
    }
    
}
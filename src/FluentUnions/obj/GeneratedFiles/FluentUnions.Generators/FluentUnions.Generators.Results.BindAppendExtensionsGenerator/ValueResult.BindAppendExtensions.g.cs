namespace FluentUnions;

public static partial class ValueResultExtensions
{
    public static Result<(TSource, TTarget)> BindAppend<TSource, TTarget>(
        in this Result<TSource> result,
        Func<TSource, Result<TTarget>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = binder(result.Value);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value, target);
    }

    public static Result<(TSource, TTarget1, TTarget2)> BindAppend<TSource, TTarget1, TTarget2>(
        in this Result<TSource> result,
        Func<TSource, Result<(TTarget1, TTarget2)>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = binder(result.Value);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value, target.Item1, target.Item2);
    }

    public static Result<(TSource1, TSource2, TTarget)> BindAppend<TSource1, TSource2, TTarget>(
        in this Result<(TSource1, TSource2)> result,
        Func<TSource1, TSource2, Result<TTarget>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = binder(result.Value.Item1, result.Value.Item2);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value.Item1, result.Value.Item2, target);
    }

    public static Result<(TSource, TTarget1, TTarget2, TTarget3)> BindAppend<TSource, TTarget1, TTarget2, TTarget3>(
        in this Result<TSource> result,
        Func<TSource, Result<(TTarget1, TTarget2, TTarget3)>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = binder(result.Value);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value, target.Item1, target.Item2, target.Item3);
    }

    public static Result<(TSource1, TSource2, TTarget1, TTarget2)> BindAppend<TSource1, TSource2, TTarget1, TTarget2>(
        in this Result<(TSource1, TSource2)> result,
        Func<TSource1, TSource2, Result<(TTarget1, TTarget2)>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = binder(result.Value.Item1, result.Value.Item2);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value.Item1, result.Value.Item2, target.Item1, target.Item2);
    }

    public static Result<(TSource1, TSource2, TSource3, TTarget)> BindAppend<TSource1, TSource2, TSource3, TTarget>(
        in this Result<(TSource1, TSource2, TSource3)> result,
        Func<TSource1, TSource2, TSource3, Result<TTarget>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = binder(result.Value.Item1, result.Value.Item2, result.Value.Item3);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value.Item1, result.Value.Item2, result.Value.Item3, target);
    }

    public static Result<(TSource, TTarget1, TTarget2, TTarget3, TTarget4)> BindAppend<TSource, TTarget1, TTarget2, TTarget3, TTarget4>(
        in this Result<TSource> result,
        Func<TSource, Result<(TTarget1, TTarget2, TTarget3, TTarget4)>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = binder(result.Value);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value, target.Item1, target.Item2, target.Item3, target.Item4);
    }

    public static Result<(TSource1, TSource2, TTarget1, TTarget2, TTarget3)> BindAppend<TSource1, TSource2, TTarget1, TTarget2, TTarget3>(
        in this Result<(TSource1, TSource2)> result,
        Func<TSource1, TSource2, Result<(TTarget1, TTarget2, TTarget3)>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = binder(result.Value.Item1, result.Value.Item2);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value.Item1, result.Value.Item2, target.Item1, target.Item2, target.Item3);
    }

    public static Result<(TSource1, TSource2, TSource3, TTarget1, TTarget2)> BindAppend<TSource1, TSource2, TSource3, TTarget1, TTarget2>(
        in this Result<(TSource1, TSource2, TSource3)> result,
        Func<TSource1, TSource2, TSource3, Result<(TTarget1, TTarget2)>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = binder(result.Value.Item1, result.Value.Item2, result.Value.Item3);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value.Item1, result.Value.Item2, result.Value.Item3, target.Item1, target.Item2);
    }

    public static Result<(TSource1, TSource2, TSource3, TSource4, TTarget)> BindAppend<TSource1, TSource2, TSource3, TSource4, TTarget>(
        in this Result<(TSource1, TSource2, TSource3, TSource4)> result,
        Func<TSource1, TSource2, TSource3, TSource4, Result<TTarget>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = binder(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, target);
    }

    public static Result<(TSource, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5)> BindAppend<TSource, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5>(
        in this Result<TSource> result,
        Func<TSource, Result<(TTarget1, TTarget2, TTarget3, TTarget4, TTarget5)>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = binder(result.Value);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value, target.Item1, target.Item2, target.Item3, target.Item4, target.Item5);
    }

    public static Result<(TSource1, TSource2, TTarget1, TTarget2, TTarget3, TTarget4)> BindAppend<TSource1, TSource2, TTarget1, TTarget2, TTarget3, TTarget4>(
        in this Result<(TSource1, TSource2)> result,
        Func<TSource1, TSource2, Result<(TTarget1, TTarget2, TTarget3, TTarget4)>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = binder(result.Value.Item1, result.Value.Item2);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value.Item1, result.Value.Item2, target.Item1, target.Item2, target.Item3, target.Item4);
    }

    public static Result<(TSource1, TSource2, TSource3, TTarget1, TTarget2, TTarget3)> BindAppend<TSource1, TSource2, TSource3, TTarget1, TTarget2, TTarget3>(
        in this Result<(TSource1, TSource2, TSource3)> result,
        Func<TSource1, TSource2, TSource3, Result<(TTarget1, TTarget2, TTarget3)>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = binder(result.Value.Item1, result.Value.Item2, result.Value.Item3);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value.Item1, result.Value.Item2, result.Value.Item3, target.Item1, target.Item2, target.Item3);
    }

    public static Result<(TSource1, TSource2, TSource3, TSource4, TTarget1, TTarget2)> BindAppend<TSource1, TSource2, TSource3, TSource4, TTarget1, TTarget2>(
        in this Result<(TSource1, TSource2, TSource3, TSource4)> result,
        Func<TSource1, TSource2, TSource3, TSource4, Result<(TTarget1, TTarget2)>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = binder(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, target.Item1, target.Item2);
    }

    public static Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TTarget)> BindAppend<TSource1, TSource2, TSource3, TSource4, TSource5, TTarget>(
        in this Result<(TSource1, TSource2, TSource3, TSource4, TSource5)> result,
        Func<TSource1, TSource2, TSource3, TSource4, TSource5, Result<TTarget>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = binder(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, target);
    }

    public static Result<(TSource, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6)> BindAppend<TSource, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6>(
        in this Result<TSource> result,
        Func<TSource, Result<(TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6)>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = binder(result.Value);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value, target.Item1, target.Item2, target.Item3, target.Item4, target.Item5, target.Item6);
    }

    public static Result<(TSource1, TSource2, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5)> BindAppend<TSource1, TSource2, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5>(
        in this Result<(TSource1, TSource2)> result,
        Func<TSource1, TSource2, Result<(TTarget1, TTarget2, TTarget3, TTarget4, TTarget5)>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = binder(result.Value.Item1, result.Value.Item2);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value.Item1, result.Value.Item2, target.Item1, target.Item2, target.Item3, target.Item4, target.Item5);
    }

    public static Result<(TSource1, TSource2, TSource3, TTarget1, TTarget2, TTarget3, TTarget4)> BindAppend<TSource1, TSource2, TSource3, TTarget1, TTarget2, TTarget3, TTarget4>(
        in this Result<(TSource1, TSource2, TSource3)> result,
        Func<TSource1, TSource2, TSource3, Result<(TTarget1, TTarget2, TTarget3, TTarget4)>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = binder(result.Value.Item1, result.Value.Item2, result.Value.Item3);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value.Item1, result.Value.Item2, result.Value.Item3, target.Item1, target.Item2, target.Item3, target.Item4);
    }

    public static Result<(TSource1, TSource2, TSource3, TSource4, TTarget1, TTarget2, TTarget3)> BindAppend<TSource1, TSource2, TSource3, TSource4, TTarget1, TTarget2, TTarget3>(
        in this Result<(TSource1, TSource2, TSource3, TSource4)> result,
        Func<TSource1, TSource2, TSource3, TSource4, Result<(TTarget1, TTarget2, TTarget3)>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = binder(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, target.Item1, target.Item2, target.Item3);
    }

    public static Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TTarget1, TTarget2)> BindAppend<TSource1, TSource2, TSource3, TSource4, TSource5, TTarget1, TTarget2>(
        in this Result<(TSource1, TSource2, TSource3, TSource4, TSource5)> result,
        Func<TSource1, TSource2, TSource3, TSource4, TSource5, Result<(TTarget1, TTarget2)>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = binder(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, target.Item1, target.Item2);
    }

    public static Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TTarget)> BindAppend<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TTarget>(
        in this Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6)> result,
        Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, Result<TTarget>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = binder(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, result.Value.Item6);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, result.Value.Item6, target);
    }

    public static Result<(TSource, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6, TTarget7)> BindAppend<TSource, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6, TTarget7>(
        in this Result<TSource> result,
        Func<TSource, Result<(TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6, TTarget7)>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = binder(result.Value);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value, target.Item1, target.Item2, target.Item3, target.Item4, target.Item5, target.Item6, target.Item7);
    }

    public static Result<(TSource1, TSource2, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6)> BindAppend<TSource1, TSource2, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6>(
        in this Result<(TSource1, TSource2)> result,
        Func<TSource1, TSource2, Result<(TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6)>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = binder(result.Value.Item1, result.Value.Item2);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value.Item1, result.Value.Item2, target.Item1, target.Item2, target.Item3, target.Item4, target.Item5, target.Item6);
    }

    public static Result<(TSource1, TSource2, TSource3, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5)> BindAppend<TSource1, TSource2, TSource3, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5>(
        in this Result<(TSource1, TSource2, TSource3)> result,
        Func<TSource1, TSource2, TSource3, Result<(TTarget1, TTarget2, TTarget3, TTarget4, TTarget5)>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = binder(result.Value.Item1, result.Value.Item2, result.Value.Item3);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value.Item1, result.Value.Item2, result.Value.Item3, target.Item1, target.Item2, target.Item3, target.Item4, target.Item5);
    }

    public static Result<(TSource1, TSource2, TSource3, TSource4, TTarget1, TTarget2, TTarget3, TTarget4)> BindAppend<TSource1, TSource2, TSource3, TSource4, TTarget1, TTarget2, TTarget3, TTarget4>(
        in this Result<(TSource1, TSource2, TSource3, TSource4)> result,
        Func<TSource1, TSource2, TSource3, TSource4, Result<(TTarget1, TTarget2, TTarget3, TTarget4)>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = binder(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, target.Item1, target.Item2, target.Item3, target.Item4);
    }

    public static Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TTarget1, TTarget2, TTarget3)> BindAppend<TSource1, TSource2, TSource3, TSource4, TSource5, TTarget1, TTarget2, TTarget3>(
        in this Result<(TSource1, TSource2, TSource3, TSource4, TSource5)> result,
        Func<TSource1, TSource2, TSource3, TSource4, TSource5, Result<(TTarget1, TTarget2, TTarget3)>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = binder(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, target.Item1, target.Item2, target.Item3);
    }

    public static Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TTarget1, TTarget2)> BindAppend<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TTarget1, TTarget2>(
        in this Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6)> result,
        Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, Result<(TTarget1, TTarget2)>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = binder(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, result.Value.Item6);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, result.Value.Item6, target.Item1, target.Item2);
    }

    public static Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TTarget)> BindAppend<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TTarget>(
        in this Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7)> result,
        Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, Result<TTarget>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = binder(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, result.Value.Item6, result.Value.Item7);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, result.Value.Item6, result.Value.Item7, target);
    }

}
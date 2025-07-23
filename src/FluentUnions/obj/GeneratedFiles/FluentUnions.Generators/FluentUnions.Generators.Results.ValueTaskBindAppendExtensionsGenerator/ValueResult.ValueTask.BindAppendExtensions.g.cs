namespace FluentUnions;

public static partial class ValueResultExtensions
{
    public static async ValueTask<Result<(TSource, TTarget)>> BindAppendAsync<TSource, TTarget>(
        this Result<TSource> result,
        Func<TSource, ValueTask<Result<TTarget>>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = await binder(result.Value).ConfigureAwait(false);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value, target);
    }

    public static async ValueTask<Result<(TSource, TTarget)>> BindAppendAsync<TSource, TTarget>(
        this ValueTask<Result<TSource>> result,
        Func<TSource, Result<TTarget>> binder)
        => (await result.ConfigureAwait(false)).BindAppend(binder);
        

    public static async ValueTask<Result<(TSource, TTarget)>> BindAppendAsync<TSource, TTarget>(
        this ValueTask<Result<TSource>> result,
        Func<TSource, ValueTask<Result<TTarget>>> binder)
        => await (await result.ConfigureAwait(false)).BindAppendAsync(binder).ConfigureAwait(false);
    
    public static async ValueTask<Result<(TSource, TTarget1, TTarget2)>> BindAppendAsync<TSource, TTarget1, TTarget2>(
        this Result<TSource> result,
        Func<TSource, ValueTask<Result<(TTarget1, TTarget2)>>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = await binder(result.Value).ConfigureAwait(false);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value, target.Item1, target.Item2);
    }

    public static async ValueTask<Result<(TSource, TTarget1, TTarget2)>> BindAppendAsync<TSource, TTarget1, TTarget2>(
        this ValueTask<Result<TSource>> result,
        Func<TSource, Result<(TTarget1, TTarget2)>> binder)
        => (await result.ConfigureAwait(false)).BindAppend(binder);
        

    public static async ValueTask<Result<(TSource, TTarget1, TTarget2)>> BindAppendAsync<TSource, TTarget1, TTarget2>(
        this ValueTask<Result<TSource>> result,
        Func<TSource, ValueTask<Result<(TTarget1, TTarget2)>>> binder)
        => await (await result.ConfigureAwait(false)).BindAppendAsync(binder).ConfigureAwait(false);
    
    public static async ValueTask<Result<(TSource1, TSource2, TTarget)>> BindAppendAsync<TSource1, TSource2, TTarget>(
        this Result<(TSource1, TSource2)> result,
        Func<TSource1, TSource2, ValueTask<Result<TTarget>>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = await binder(result.Value.Item1, result.Value.Item2).ConfigureAwait(false);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value.Item1, result.Value.Item2, target);
    }

    public static async ValueTask<Result<(TSource1, TSource2, TTarget)>> BindAppendAsync<TSource1, TSource2, TTarget>(
        this ValueTask<Result<(TSource1, TSource2)>> result,
        Func<TSource1, TSource2, Result<TTarget>> binder)
        => (await result.ConfigureAwait(false)).BindAppend(binder);
        

    public static async ValueTask<Result<(TSource1, TSource2, TTarget)>> BindAppendAsync<TSource1, TSource2, TTarget>(
        this ValueTask<Result<(TSource1, TSource2)>> result,
        Func<TSource1, TSource2, ValueTask<Result<TTarget>>> binder)
        => await (await result.ConfigureAwait(false)).BindAppendAsync(binder).ConfigureAwait(false);
    
    public static async ValueTask<Result<(TSource, TTarget1, TTarget2, TTarget3)>> BindAppendAsync<TSource, TTarget1, TTarget2, TTarget3>(
        this Result<TSource> result,
        Func<TSource, ValueTask<Result<(TTarget1, TTarget2, TTarget3)>>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = await binder(result.Value).ConfigureAwait(false);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value, target.Item1, target.Item2, target.Item3);
    }

    public static async ValueTask<Result<(TSource, TTarget1, TTarget2, TTarget3)>> BindAppendAsync<TSource, TTarget1, TTarget2, TTarget3>(
        this ValueTask<Result<TSource>> result,
        Func<TSource, Result<(TTarget1, TTarget2, TTarget3)>> binder)
        => (await result.ConfigureAwait(false)).BindAppend(binder);
        

    public static async ValueTask<Result<(TSource, TTarget1, TTarget2, TTarget3)>> BindAppendAsync<TSource, TTarget1, TTarget2, TTarget3>(
        this ValueTask<Result<TSource>> result,
        Func<TSource, ValueTask<Result<(TTarget1, TTarget2, TTarget3)>>> binder)
        => await (await result.ConfigureAwait(false)).BindAppendAsync(binder).ConfigureAwait(false);
    
    public static async ValueTask<Result<(TSource1, TSource2, TTarget1, TTarget2)>> BindAppendAsync<TSource1, TSource2, TTarget1, TTarget2>(
        this Result<(TSource1, TSource2)> result,
        Func<TSource1, TSource2, ValueTask<Result<(TTarget1, TTarget2)>>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = await binder(result.Value.Item1, result.Value.Item2).ConfigureAwait(false);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value.Item1, result.Value.Item2, target.Item1, target.Item2);
    }

    public static async ValueTask<Result<(TSource1, TSource2, TTarget1, TTarget2)>> BindAppendAsync<TSource1, TSource2, TTarget1, TTarget2>(
        this ValueTask<Result<(TSource1, TSource2)>> result,
        Func<TSource1, TSource2, Result<(TTarget1, TTarget2)>> binder)
        => (await result.ConfigureAwait(false)).BindAppend(binder);
        

    public static async ValueTask<Result<(TSource1, TSource2, TTarget1, TTarget2)>> BindAppendAsync<TSource1, TSource2, TTarget1, TTarget2>(
        this ValueTask<Result<(TSource1, TSource2)>> result,
        Func<TSource1, TSource2, ValueTask<Result<(TTarget1, TTarget2)>>> binder)
        => await (await result.ConfigureAwait(false)).BindAppendAsync(binder).ConfigureAwait(false);
    
    public static async ValueTask<Result<(TSource1, TSource2, TSource3, TTarget)>> BindAppendAsync<TSource1, TSource2, TSource3, TTarget>(
        this Result<(TSource1, TSource2, TSource3)> result,
        Func<TSource1, TSource2, TSource3, ValueTask<Result<TTarget>>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = await binder(result.Value.Item1, result.Value.Item2, result.Value.Item3).ConfigureAwait(false);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value.Item1, result.Value.Item2, result.Value.Item3, target);
    }

    public static async ValueTask<Result<(TSource1, TSource2, TSource3, TTarget)>> BindAppendAsync<TSource1, TSource2, TSource3, TTarget>(
        this ValueTask<Result<(TSource1, TSource2, TSource3)>> result,
        Func<TSource1, TSource2, TSource3, Result<TTarget>> binder)
        => (await result.ConfigureAwait(false)).BindAppend(binder);
        

    public static async ValueTask<Result<(TSource1, TSource2, TSource3, TTarget)>> BindAppendAsync<TSource1, TSource2, TSource3, TTarget>(
        this ValueTask<Result<(TSource1, TSource2, TSource3)>> result,
        Func<TSource1, TSource2, TSource3, ValueTask<Result<TTarget>>> binder)
        => await (await result.ConfigureAwait(false)).BindAppendAsync(binder).ConfigureAwait(false);
    
    public static async ValueTask<Result<(TSource, TTarget1, TTarget2, TTarget3, TTarget4)>> BindAppendAsync<TSource, TTarget1, TTarget2, TTarget3, TTarget4>(
        this Result<TSource> result,
        Func<TSource, ValueTask<Result<(TTarget1, TTarget2, TTarget3, TTarget4)>>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = await binder(result.Value).ConfigureAwait(false);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value, target.Item1, target.Item2, target.Item3, target.Item4);
    }

    public static async ValueTask<Result<(TSource, TTarget1, TTarget2, TTarget3, TTarget4)>> BindAppendAsync<TSource, TTarget1, TTarget2, TTarget3, TTarget4>(
        this ValueTask<Result<TSource>> result,
        Func<TSource, Result<(TTarget1, TTarget2, TTarget3, TTarget4)>> binder)
        => (await result.ConfigureAwait(false)).BindAppend(binder);
        

    public static async ValueTask<Result<(TSource, TTarget1, TTarget2, TTarget3, TTarget4)>> BindAppendAsync<TSource, TTarget1, TTarget2, TTarget3, TTarget4>(
        this ValueTask<Result<TSource>> result,
        Func<TSource, ValueTask<Result<(TTarget1, TTarget2, TTarget3, TTarget4)>>> binder)
        => await (await result.ConfigureAwait(false)).BindAppendAsync(binder).ConfigureAwait(false);
    
    public static async ValueTask<Result<(TSource1, TSource2, TTarget1, TTarget2, TTarget3)>> BindAppendAsync<TSource1, TSource2, TTarget1, TTarget2, TTarget3>(
        this Result<(TSource1, TSource2)> result,
        Func<TSource1, TSource2, ValueTask<Result<(TTarget1, TTarget2, TTarget3)>>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = await binder(result.Value.Item1, result.Value.Item2).ConfigureAwait(false);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value.Item1, result.Value.Item2, target.Item1, target.Item2, target.Item3);
    }

    public static async ValueTask<Result<(TSource1, TSource2, TTarget1, TTarget2, TTarget3)>> BindAppendAsync<TSource1, TSource2, TTarget1, TTarget2, TTarget3>(
        this ValueTask<Result<(TSource1, TSource2)>> result,
        Func<TSource1, TSource2, Result<(TTarget1, TTarget2, TTarget3)>> binder)
        => (await result.ConfigureAwait(false)).BindAppend(binder);
        

    public static async ValueTask<Result<(TSource1, TSource2, TTarget1, TTarget2, TTarget3)>> BindAppendAsync<TSource1, TSource2, TTarget1, TTarget2, TTarget3>(
        this ValueTask<Result<(TSource1, TSource2)>> result,
        Func<TSource1, TSource2, ValueTask<Result<(TTarget1, TTarget2, TTarget3)>>> binder)
        => await (await result.ConfigureAwait(false)).BindAppendAsync(binder).ConfigureAwait(false);
    
    public static async ValueTask<Result<(TSource1, TSource2, TSource3, TTarget1, TTarget2)>> BindAppendAsync<TSource1, TSource2, TSource3, TTarget1, TTarget2>(
        this Result<(TSource1, TSource2, TSource3)> result,
        Func<TSource1, TSource2, TSource3, ValueTask<Result<(TTarget1, TTarget2)>>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = await binder(result.Value.Item1, result.Value.Item2, result.Value.Item3).ConfigureAwait(false);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value.Item1, result.Value.Item2, result.Value.Item3, target.Item1, target.Item2);
    }

    public static async ValueTask<Result<(TSource1, TSource2, TSource3, TTarget1, TTarget2)>> BindAppendAsync<TSource1, TSource2, TSource3, TTarget1, TTarget2>(
        this ValueTask<Result<(TSource1, TSource2, TSource3)>> result,
        Func<TSource1, TSource2, TSource3, Result<(TTarget1, TTarget2)>> binder)
        => (await result.ConfigureAwait(false)).BindAppend(binder);
        

    public static async ValueTask<Result<(TSource1, TSource2, TSource3, TTarget1, TTarget2)>> BindAppendAsync<TSource1, TSource2, TSource3, TTarget1, TTarget2>(
        this ValueTask<Result<(TSource1, TSource2, TSource3)>> result,
        Func<TSource1, TSource2, TSource3, ValueTask<Result<(TTarget1, TTarget2)>>> binder)
        => await (await result.ConfigureAwait(false)).BindAppendAsync(binder).ConfigureAwait(false);
    
    public static async ValueTask<Result<(TSource1, TSource2, TSource3, TSource4, TTarget)>> BindAppendAsync<TSource1, TSource2, TSource3, TSource4, TTarget>(
        this Result<(TSource1, TSource2, TSource3, TSource4)> result,
        Func<TSource1, TSource2, TSource3, TSource4, ValueTask<Result<TTarget>>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = await binder(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4).ConfigureAwait(false);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, target);
    }

    public static async ValueTask<Result<(TSource1, TSource2, TSource3, TSource4, TTarget)>> BindAppendAsync<TSource1, TSource2, TSource3, TSource4, TTarget>(
        this ValueTask<Result<(TSource1, TSource2, TSource3, TSource4)>> result,
        Func<TSource1, TSource2, TSource3, TSource4, Result<TTarget>> binder)
        => (await result.ConfigureAwait(false)).BindAppend(binder);
        

    public static async ValueTask<Result<(TSource1, TSource2, TSource3, TSource4, TTarget)>> BindAppendAsync<TSource1, TSource2, TSource3, TSource4, TTarget>(
        this ValueTask<Result<(TSource1, TSource2, TSource3, TSource4)>> result,
        Func<TSource1, TSource2, TSource3, TSource4, ValueTask<Result<TTarget>>> binder)
        => await (await result.ConfigureAwait(false)).BindAppendAsync(binder).ConfigureAwait(false);
    
    public static async ValueTask<Result<(TSource, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5)>> BindAppendAsync<TSource, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5>(
        this Result<TSource> result,
        Func<TSource, ValueTask<Result<(TTarget1, TTarget2, TTarget3, TTarget4, TTarget5)>>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = await binder(result.Value).ConfigureAwait(false);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value, target.Item1, target.Item2, target.Item3, target.Item4, target.Item5);
    }

    public static async ValueTask<Result<(TSource, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5)>> BindAppendAsync<TSource, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5>(
        this ValueTask<Result<TSource>> result,
        Func<TSource, Result<(TTarget1, TTarget2, TTarget3, TTarget4, TTarget5)>> binder)
        => (await result.ConfigureAwait(false)).BindAppend(binder);
        

    public static async ValueTask<Result<(TSource, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5)>> BindAppendAsync<TSource, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5>(
        this ValueTask<Result<TSource>> result,
        Func<TSource, ValueTask<Result<(TTarget1, TTarget2, TTarget3, TTarget4, TTarget5)>>> binder)
        => await (await result.ConfigureAwait(false)).BindAppendAsync(binder).ConfigureAwait(false);
    
    public static async ValueTask<Result<(TSource1, TSource2, TTarget1, TTarget2, TTarget3, TTarget4)>> BindAppendAsync<TSource1, TSource2, TTarget1, TTarget2, TTarget3, TTarget4>(
        this Result<(TSource1, TSource2)> result,
        Func<TSource1, TSource2, ValueTask<Result<(TTarget1, TTarget2, TTarget3, TTarget4)>>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = await binder(result.Value.Item1, result.Value.Item2).ConfigureAwait(false);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value.Item1, result.Value.Item2, target.Item1, target.Item2, target.Item3, target.Item4);
    }

    public static async ValueTask<Result<(TSource1, TSource2, TTarget1, TTarget2, TTarget3, TTarget4)>> BindAppendAsync<TSource1, TSource2, TTarget1, TTarget2, TTarget3, TTarget4>(
        this ValueTask<Result<(TSource1, TSource2)>> result,
        Func<TSource1, TSource2, Result<(TTarget1, TTarget2, TTarget3, TTarget4)>> binder)
        => (await result.ConfigureAwait(false)).BindAppend(binder);
        

    public static async ValueTask<Result<(TSource1, TSource2, TTarget1, TTarget2, TTarget3, TTarget4)>> BindAppendAsync<TSource1, TSource2, TTarget1, TTarget2, TTarget3, TTarget4>(
        this ValueTask<Result<(TSource1, TSource2)>> result,
        Func<TSource1, TSource2, ValueTask<Result<(TTarget1, TTarget2, TTarget3, TTarget4)>>> binder)
        => await (await result.ConfigureAwait(false)).BindAppendAsync(binder).ConfigureAwait(false);
    
    public static async ValueTask<Result<(TSource1, TSource2, TSource3, TTarget1, TTarget2, TTarget3)>> BindAppendAsync<TSource1, TSource2, TSource3, TTarget1, TTarget2, TTarget3>(
        this Result<(TSource1, TSource2, TSource3)> result,
        Func<TSource1, TSource2, TSource3, ValueTask<Result<(TTarget1, TTarget2, TTarget3)>>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = await binder(result.Value.Item1, result.Value.Item2, result.Value.Item3).ConfigureAwait(false);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value.Item1, result.Value.Item2, result.Value.Item3, target.Item1, target.Item2, target.Item3);
    }

    public static async ValueTask<Result<(TSource1, TSource2, TSource3, TTarget1, TTarget2, TTarget3)>> BindAppendAsync<TSource1, TSource2, TSource3, TTarget1, TTarget2, TTarget3>(
        this ValueTask<Result<(TSource1, TSource2, TSource3)>> result,
        Func<TSource1, TSource2, TSource3, Result<(TTarget1, TTarget2, TTarget3)>> binder)
        => (await result.ConfigureAwait(false)).BindAppend(binder);
        

    public static async ValueTask<Result<(TSource1, TSource2, TSource3, TTarget1, TTarget2, TTarget3)>> BindAppendAsync<TSource1, TSource2, TSource3, TTarget1, TTarget2, TTarget3>(
        this ValueTask<Result<(TSource1, TSource2, TSource3)>> result,
        Func<TSource1, TSource2, TSource3, ValueTask<Result<(TTarget1, TTarget2, TTarget3)>>> binder)
        => await (await result.ConfigureAwait(false)).BindAppendAsync(binder).ConfigureAwait(false);
    
    public static async ValueTask<Result<(TSource1, TSource2, TSource3, TSource4, TTarget1, TTarget2)>> BindAppendAsync<TSource1, TSource2, TSource3, TSource4, TTarget1, TTarget2>(
        this Result<(TSource1, TSource2, TSource3, TSource4)> result,
        Func<TSource1, TSource2, TSource3, TSource4, ValueTask<Result<(TTarget1, TTarget2)>>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = await binder(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4).ConfigureAwait(false);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, target.Item1, target.Item2);
    }

    public static async ValueTask<Result<(TSource1, TSource2, TSource3, TSource4, TTarget1, TTarget2)>> BindAppendAsync<TSource1, TSource2, TSource3, TSource4, TTarget1, TTarget2>(
        this ValueTask<Result<(TSource1, TSource2, TSource3, TSource4)>> result,
        Func<TSource1, TSource2, TSource3, TSource4, Result<(TTarget1, TTarget2)>> binder)
        => (await result.ConfigureAwait(false)).BindAppend(binder);
        

    public static async ValueTask<Result<(TSource1, TSource2, TSource3, TSource4, TTarget1, TTarget2)>> BindAppendAsync<TSource1, TSource2, TSource3, TSource4, TTarget1, TTarget2>(
        this ValueTask<Result<(TSource1, TSource2, TSource3, TSource4)>> result,
        Func<TSource1, TSource2, TSource3, TSource4, ValueTask<Result<(TTarget1, TTarget2)>>> binder)
        => await (await result.ConfigureAwait(false)).BindAppendAsync(binder).ConfigureAwait(false);
    
    public static async ValueTask<Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TTarget)>> BindAppendAsync<TSource1, TSource2, TSource3, TSource4, TSource5, TTarget>(
        this Result<(TSource1, TSource2, TSource3, TSource4, TSource5)> result,
        Func<TSource1, TSource2, TSource3, TSource4, TSource5, ValueTask<Result<TTarget>>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = await binder(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5).ConfigureAwait(false);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, target);
    }

    public static async ValueTask<Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TTarget)>> BindAppendAsync<TSource1, TSource2, TSource3, TSource4, TSource5, TTarget>(
        this ValueTask<Result<(TSource1, TSource2, TSource3, TSource4, TSource5)>> result,
        Func<TSource1, TSource2, TSource3, TSource4, TSource5, Result<TTarget>> binder)
        => (await result.ConfigureAwait(false)).BindAppend(binder);
        

    public static async ValueTask<Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TTarget)>> BindAppendAsync<TSource1, TSource2, TSource3, TSource4, TSource5, TTarget>(
        this ValueTask<Result<(TSource1, TSource2, TSource3, TSource4, TSource5)>> result,
        Func<TSource1, TSource2, TSource3, TSource4, TSource5, ValueTask<Result<TTarget>>> binder)
        => await (await result.ConfigureAwait(false)).BindAppendAsync(binder).ConfigureAwait(false);
    
    public static async ValueTask<Result<(TSource, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6)>> BindAppendAsync<TSource, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6>(
        this Result<TSource> result,
        Func<TSource, ValueTask<Result<(TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6)>>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = await binder(result.Value).ConfigureAwait(false);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value, target.Item1, target.Item2, target.Item3, target.Item4, target.Item5, target.Item6);
    }

    public static async ValueTask<Result<(TSource, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6)>> BindAppendAsync<TSource, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6>(
        this ValueTask<Result<TSource>> result,
        Func<TSource, Result<(TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6)>> binder)
        => (await result.ConfigureAwait(false)).BindAppend(binder);
        

    public static async ValueTask<Result<(TSource, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6)>> BindAppendAsync<TSource, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6>(
        this ValueTask<Result<TSource>> result,
        Func<TSource, ValueTask<Result<(TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6)>>> binder)
        => await (await result.ConfigureAwait(false)).BindAppendAsync(binder).ConfigureAwait(false);
    
    public static async ValueTask<Result<(TSource1, TSource2, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5)>> BindAppendAsync<TSource1, TSource2, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5>(
        this Result<(TSource1, TSource2)> result,
        Func<TSource1, TSource2, ValueTask<Result<(TTarget1, TTarget2, TTarget3, TTarget4, TTarget5)>>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = await binder(result.Value.Item1, result.Value.Item2).ConfigureAwait(false);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value.Item1, result.Value.Item2, target.Item1, target.Item2, target.Item3, target.Item4, target.Item5);
    }

    public static async ValueTask<Result<(TSource1, TSource2, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5)>> BindAppendAsync<TSource1, TSource2, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5>(
        this ValueTask<Result<(TSource1, TSource2)>> result,
        Func<TSource1, TSource2, Result<(TTarget1, TTarget2, TTarget3, TTarget4, TTarget5)>> binder)
        => (await result.ConfigureAwait(false)).BindAppend(binder);
        

    public static async ValueTask<Result<(TSource1, TSource2, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5)>> BindAppendAsync<TSource1, TSource2, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5>(
        this ValueTask<Result<(TSource1, TSource2)>> result,
        Func<TSource1, TSource2, ValueTask<Result<(TTarget1, TTarget2, TTarget3, TTarget4, TTarget5)>>> binder)
        => await (await result.ConfigureAwait(false)).BindAppendAsync(binder).ConfigureAwait(false);
    
    public static async ValueTask<Result<(TSource1, TSource2, TSource3, TTarget1, TTarget2, TTarget3, TTarget4)>> BindAppendAsync<TSource1, TSource2, TSource3, TTarget1, TTarget2, TTarget3, TTarget4>(
        this Result<(TSource1, TSource2, TSource3)> result,
        Func<TSource1, TSource2, TSource3, ValueTask<Result<(TTarget1, TTarget2, TTarget3, TTarget4)>>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = await binder(result.Value.Item1, result.Value.Item2, result.Value.Item3).ConfigureAwait(false);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value.Item1, result.Value.Item2, result.Value.Item3, target.Item1, target.Item2, target.Item3, target.Item4);
    }

    public static async ValueTask<Result<(TSource1, TSource2, TSource3, TTarget1, TTarget2, TTarget3, TTarget4)>> BindAppendAsync<TSource1, TSource2, TSource3, TTarget1, TTarget2, TTarget3, TTarget4>(
        this ValueTask<Result<(TSource1, TSource2, TSource3)>> result,
        Func<TSource1, TSource2, TSource3, Result<(TTarget1, TTarget2, TTarget3, TTarget4)>> binder)
        => (await result.ConfigureAwait(false)).BindAppend(binder);
        

    public static async ValueTask<Result<(TSource1, TSource2, TSource3, TTarget1, TTarget2, TTarget3, TTarget4)>> BindAppendAsync<TSource1, TSource2, TSource3, TTarget1, TTarget2, TTarget3, TTarget4>(
        this ValueTask<Result<(TSource1, TSource2, TSource3)>> result,
        Func<TSource1, TSource2, TSource3, ValueTask<Result<(TTarget1, TTarget2, TTarget3, TTarget4)>>> binder)
        => await (await result.ConfigureAwait(false)).BindAppendAsync(binder).ConfigureAwait(false);
    
    public static async ValueTask<Result<(TSource1, TSource2, TSource3, TSource4, TTarget1, TTarget2, TTarget3)>> BindAppendAsync<TSource1, TSource2, TSource3, TSource4, TTarget1, TTarget2, TTarget3>(
        this Result<(TSource1, TSource2, TSource3, TSource4)> result,
        Func<TSource1, TSource2, TSource3, TSource4, ValueTask<Result<(TTarget1, TTarget2, TTarget3)>>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = await binder(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4).ConfigureAwait(false);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, target.Item1, target.Item2, target.Item3);
    }

    public static async ValueTask<Result<(TSource1, TSource2, TSource3, TSource4, TTarget1, TTarget2, TTarget3)>> BindAppendAsync<TSource1, TSource2, TSource3, TSource4, TTarget1, TTarget2, TTarget3>(
        this ValueTask<Result<(TSource1, TSource2, TSource3, TSource4)>> result,
        Func<TSource1, TSource2, TSource3, TSource4, Result<(TTarget1, TTarget2, TTarget3)>> binder)
        => (await result.ConfigureAwait(false)).BindAppend(binder);
        

    public static async ValueTask<Result<(TSource1, TSource2, TSource3, TSource4, TTarget1, TTarget2, TTarget3)>> BindAppendAsync<TSource1, TSource2, TSource3, TSource4, TTarget1, TTarget2, TTarget3>(
        this ValueTask<Result<(TSource1, TSource2, TSource3, TSource4)>> result,
        Func<TSource1, TSource2, TSource3, TSource4, ValueTask<Result<(TTarget1, TTarget2, TTarget3)>>> binder)
        => await (await result.ConfigureAwait(false)).BindAppendAsync(binder).ConfigureAwait(false);
    
    public static async ValueTask<Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TTarget1, TTarget2)>> BindAppendAsync<TSource1, TSource2, TSource3, TSource4, TSource5, TTarget1, TTarget2>(
        this Result<(TSource1, TSource2, TSource3, TSource4, TSource5)> result,
        Func<TSource1, TSource2, TSource3, TSource4, TSource5, ValueTask<Result<(TTarget1, TTarget2)>>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = await binder(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5).ConfigureAwait(false);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, target.Item1, target.Item2);
    }

    public static async ValueTask<Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TTarget1, TTarget2)>> BindAppendAsync<TSource1, TSource2, TSource3, TSource4, TSource5, TTarget1, TTarget2>(
        this ValueTask<Result<(TSource1, TSource2, TSource3, TSource4, TSource5)>> result,
        Func<TSource1, TSource2, TSource3, TSource4, TSource5, Result<(TTarget1, TTarget2)>> binder)
        => (await result.ConfigureAwait(false)).BindAppend(binder);
        

    public static async ValueTask<Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TTarget1, TTarget2)>> BindAppendAsync<TSource1, TSource2, TSource3, TSource4, TSource5, TTarget1, TTarget2>(
        this ValueTask<Result<(TSource1, TSource2, TSource3, TSource4, TSource5)>> result,
        Func<TSource1, TSource2, TSource3, TSource4, TSource5, ValueTask<Result<(TTarget1, TTarget2)>>> binder)
        => await (await result.ConfigureAwait(false)).BindAppendAsync(binder).ConfigureAwait(false);
    
    public static async ValueTask<Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TTarget)>> BindAppendAsync<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TTarget>(
        this Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6)> result,
        Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, ValueTask<Result<TTarget>>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = await binder(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, result.Value.Item6).ConfigureAwait(false);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, result.Value.Item6, target);
    }

    public static async ValueTask<Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TTarget)>> BindAppendAsync<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TTarget>(
        this ValueTask<Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6)>> result,
        Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, Result<TTarget>> binder)
        => (await result.ConfigureAwait(false)).BindAppend(binder);
        

    public static async ValueTask<Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TTarget)>> BindAppendAsync<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TTarget>(
        this ValueTask<Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6)>> result,
        Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, ValueTask<Result<TTarget>>> binder)
        => await (await result.ConfigureAwait(false)).BindAppendAsync(binder).ConfigureAwait(false);
    
    public static async ValueTask<Result<(TSource, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6, TTarget7)>> BindAppendAsync<TSource, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6, TTarget7>(
        this Result<TSource> result,
        Func<TSource, ValueTask<Result<(TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6, TTarget7)>>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = await binder(result.Value).ConfigureAwait(false);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value, target.Item1, target.Item2, target.Item3, target.Item4, target.Item5, target.Item6, target.Item7);
    }

    public static async ValueTask<Result<(TSource, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6, TTarget7)>> BindAppendAsync<TSource, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6, TTarget7>(
        this ValueTask<Result<TSource>> result,
        Func<TSource, Result<(TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6, TTarget7)>> binder)
        => (await result.ConfigureAwait(false)).BindAppend(binder);
        

    public static async ValueTask<Result<(TSource, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6, TTarget7)>> BindAppendAsync<TSource, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6, TTarget7>(
        this ValueTask<Result<TSource>> result,
        Func<TSource, ValueTask<Result<(TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6, TTarget7)>>> binder)
        => await (await result.ConfigureAwait(false)).BindAppendAsync(binder).ConfigureAwait(false);
    
    public static async ValueTask<Result<(TSource1, TSource2, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6)>> BindAppendAsync<TSource1, TSource2, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6>(
        this Result<(TSource1, TSource2)> result,
        Func<TSource1, TSource2, ValueTask<Result<(TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6)>>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = await binder(result.Value.Item1, result.Value.Item2).ConfigureAwait(false);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value.Item1, result.Value.Item2, target.Item1, target.Item2, target.Item3, target.Item4, target.Item5, target.Item6);
    }

    public static async ValueTask<Result<(TSource1, TSource2, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6)>> BindAppendAsync<TSource1, TSource2, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6>(
        this ValueTask<Result<(TSource1, TSource2)>> result,
        Func<TSource1, TSource2, Result<(TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6)>> binder)
        => (await result.ConfigureAwait(false)).BindAppend(binder);
        

    public static async ValueTask<Result<(TSource1, TSource2, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6)>> BindAppendAsync<TSource1, TSource2, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6>(
        this ValueTask<Result<(TSource1, TSource2)>> result,
        Func<TSource1, TSource2, ValueTask<Result<(TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6)>>> binder)
        => await (await result.ConfigureAwait(false)).BindAppendAsync(binder).ConfigureAwait(false);
    
    public static async ValueTask<Result<(TSource1, TSource2, TSource3, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5)>> BindAppendAsync<TSource1, TSource2, TSource3, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5>(
        this Result<(TSource1, TSource2, TSource3)> result,
        Func<TSource1, TSource2, TSource3, ValueTask<Result<(TTarget1, TTarget2, TTarget3, TTarget4, TTarget5)>>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = await binder(result.Value.Item1, result.Value.Item2, result.Value.Item3).ConfigureAwait(false);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value.Item1, result.Value.Item2, result.Value.Item3, target.Item1, target.Item2, target.Item3, target.Item4, target.Item5);
    }

    public static async ValueTask<Result<(TSource1, TSource2, TSource3, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5)>> BindAppendAsync<TSource1, TSource2, TSource3, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5>(
        this ValueTask<Result<(TSource1, TSource2, TSource3)>> result,
        Func<TSource1, TSource2, TSource3, Result<(TTarget1, TTarget2, TTarget3, TTarget4, TTarget5)>> binder)
        => (await result.ConfigureAwait(false)).BindAppend(binder);
        

    public static async ValueTask<Result<(TSource1, TSource2, TSource3, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5)>> BindAppendAsync<TSource1, TSource2, TSource3, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5>(
        this ValueTask<Result<(TSource1, TSource2, TSource3)>> result,
        Func<TSource1, TSource2, TSource3, ValueTask<Result<(TTarget1, TTarget2, TTarget3, TTarget4, TTarget5)>>> binder)
        => await (await result.ConfigureAwait(false)).BindAppendAsync(binder).ConfigureAwait(false);
    
    public static async ValueTask<Result<(TSource1, TSource2, TSource3, TSource4, TTarget1, TTarget2, TTarget3, TTarget4)>> BindAppendAsync<TSource1, TSource2, TSource3, TSource4, TTarget1, TTarget2, TTarget3, TTarget4>(
        this Result<(TSource1, TSource2, TSource3, TSource4)> result,
        Func<TSource1, TSource2, TSource3, TSource4, ValueTask<Result<(TTarget1, TTarget2, TTarget3, TTarget4)>>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = await binder(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4).ConfigureAwait(false);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, target.Item1, target.Item2, target.Item3, target.Item4);
    }

    public static async ValueTask<Result<(TSource1, TSource2, TSource3, TSource4, TTarget1, TTarget2, TTarget3, TTarget4)>> BindAppendAsync<TSource1, TSource2, TSource3, TSource4, TTarget1, TTarget2, TTarget3, TTarget4>(
        this ValueTask<Result<(TSource1, TSource2, TSource3, TSource4)>> result,
        Func<TSource1, TSource2, TSource3, TSource4, Result<(TTarget1, TTarget2, TTarget3, TTarget4)>> binder)
        => (await result.ConfigureAwait(false)).BindAppend(binder);
        

    public static async ValueTask<Result<(TSource1, TSource2, TSource3, TSource4, TTarget1, TTarget2, TTarget3, TTarget4)>> BindAppendAsync<TSource1, TSource2, TSource3, TSource4, TTarget1, TTarget2, TTarget3, TTarget4>(
        this ValueTask<Result<(TSource1, TSource2, TSource3, TSource4)>> result,
        Func<TSource1, TSource2, TSource3, TSource4, ValueTask<Result<(TTarget1, TTarget2, TTarget3, TTarget4)>>> binder)
        => await (await result.ConfigureAwait(false)).BindAppendAsync(binder).ConfigureAwait(false);
    
    public static async ValueTask<Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TTarget1, TTarget2, TTarget3)>> BindAppendAsync<TSource1, TSource2, TSource3, TSource4, TSource5, TTarget1, TTarget2, TTarget3>(
        this Result<(TSource1, TSource2, TSource3, TSource4, TSource5)> result,
        Func<TSource1, TSource2, TSource3, TSource4, TSource5, ValueTask<Result<(TTarget1, TTarget2, TTarget3)>>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = await binder(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5).ConfigureAwait(false);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, target.Item1, target.Item2, target.Item3);
    }

    public static async ValueTask<Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TTarget1, TTarget2, TTarget3)>> BindAppendAsync<TSource1, TSource2, TSource3, TSource4, TSource5, TTarget1, TTarget2, TTarget3>(
        this ValueTask<Result<(TSource1, TSource2, TSource3, TSource4, TSource5)>> result,
        Func<TSource1, TSource2, TSource3, TSource4, TSource5, Result<(TTarget1, TTarget2, TTarget3)>> binder)
        => (await result.ConfigureAwait(false)).BindAppend(binder);
        

    public static async ValueTask<Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TTarget1, TTarget2, TTarget3)>> BindAppendAsync<TSource1, TSource2, TSource3, TSource4, TSource5, TTarget1, TTarget2, TTarget3>(
        this ValueTask<Result<(TSource1, TSource2, TSource3, TSource4, TSource5)>> result,
        Func<TSource1, TSource2, TSource3, TSource4, TSource5, ValueTask<Result<(TTarget1, TTarget2, TTarget3)>>> binder)
        => await (await result.ConfigureAwait(false)).BindAppendAsync(binder).ConfigureAwait(false);
    
    public static async ValueTask<Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TTarget1, TTarget2)>> BindAppendAsync<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TTarget1, TTarget2>(
        this Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6)> result,
        Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, ValueTask<Result<(TTarget1, TTarget2)>>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = await binder(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, result.Value.Item6).ConfigureAwait(false);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, result.Value.Item6, target.Item1, target.Item2);
    }

    public static async ValueTask<Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TTarget1, TTarget2)>> BindAppendAsync<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TTarget1, TTarget2>(
        this ValueTask<Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6)>> result,
        Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, Result<(TTarget1, TTarget2)>> binder)
        => (await result.ConfigureAwait(false)).BindAppend(binder);
        

    public static async ValueTask<Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TTarget1, TTarget2)>> BindAppendAsync<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TTarget1, TTarget2>(
        this ValueTask<Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6)>> result,
        Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, ValueTask<Result<(TTarget1, TTarget2)>>> binder)
        => await (await result.ConfigureAwait(false)).BindAppendAsync(binder).ConfigureAwait(false);
    
    public static async ValueTask<Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TTarget)>> BindAppendAsync<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TTarget>(
        this Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7)> result,
        Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, ValueTask<Result<TTarget>>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = await binder(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, result.Value.Item6, result.Value.Item7).ConfigureAwait(false);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, result.Value.Item6, result.Value.Item7, target);
    }

    public static async ValueTask<Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TTarget)>> BindAppendAsync<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TTarget>(
        this ValueTask<Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7)>> result,
        Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, Result<TTarget>> binder)
        => (await result.ConfigureAwait(false)).BindAppend(binder);
        

    public static async ValueTask<Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TTarget)>> BindAppendAsync<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TTarget>(
        this ValueTask<Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7)>> result,
        Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, ValueTask<Result<TTarget>>> binder)
        => await (await result.ConfigureAwait(false)).BindAppendAsync(binder).ConfigureAwait(false);
    
}
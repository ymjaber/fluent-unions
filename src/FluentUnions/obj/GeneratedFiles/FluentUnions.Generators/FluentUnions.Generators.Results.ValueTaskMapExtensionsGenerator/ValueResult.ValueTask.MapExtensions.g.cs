namespace FluentUnions;

public static partial class ValueResultExtensions
{
    public static async ValueTask<Result<TTarget>> MapAsync<TValue1, TValue2, TTarget>(
        this Result<(TValue1, TValue2)> source,
        Func<TValue1, TValue2, ValueTask<TTarget>> mapper)
    {
        if (source.IsFailure) return source.Error;
        return await mapper(source.Value.Item1, source.Value.Item2).ConfigureAwait(false);
    }
    
    public static async ValueTask<Result<TTarget>> MapAsync<TValue1, TValue2, TTarget>(
        this ValueTask<Result<(TValue1, TValue2)>> source,
        Func<TValue1, TValue2, TTarget> mapper)
        => (await source.ConfigureAwait(false)).Map(mapper);
         
    public static async ValueTask<Result<TTarget>> MapAsync<TValue1, TValue2, TTarget>(
        this ValueTask<Result<(TValue1, TValue2)>> source,
        Func<TValue1, TValue2, ValueTask<TTarget>> mapper)
        => await (await source.ConfigureAwait(false)).MapAsync(mapper).ConfigureAwait(false);
        
    public static async ValueTask<Result<TTarget>> MapAsync<TValue1, TValue2, TValue3, TTarget>(
        this Result<(TValue1, TValue2, TValue3)> source,
        Func<TValue1, TValue2, TValue3, ValueTask<TTarget>> mapper)
    {
        if (source.IsFailure) return source.Error;
        return await mapper(source.Value.Item1, source.Value.Item2, source.Value.Item3).ConfigureAwait(false);
    }
    
    public static async ValueTask<Result<TTarget>> MapAsync<TValue1, TValue2, TValue3, TTarget>(
        this ValueTask<Result<(TValue1, TValue2, TValue3)>> source,
        Func<TValue1, TValue2, TValue3, TTarget> mapper)
        => (await source.ConfigureAwait(false)).Map(mapper);
         
    public static async ValueTask<Result<TTarget>> MapAsync<TValue1, TValue2, TValue3, TTarget>(
        this ValueTask<Result<(TValue1, TValue2, TValue3)>> source,
        Func<TValue1, TValue2, TValue3, ValueTask<TTarget>> mapper)
        => await (await source.ConfigureAwait(false)).MapAsync(mapper).ConfigureAwait(false);
        
    public static async ValueTask<Result<TTarget>> MapAsync<TValue1, TValue2, TValue3, TValue4, TTarget>(
        this Result<(TValue1, TValue2, TValue3, TValue4)> source,
        Func<TValue1, TValue2, TValue3, TValue4, ValueTask<TTarget>> mapper)
    {
        if (source.IsFailure) return source.Error;
        return await mapper(source.Value.Item1, source.Value.Item2, source.Value.Item3, source.Value.Item4).ConfigureAwait(false);
    }
    
    public static async ValueTask<Result<TTarget>> MapAsync<TValue1, TValue2, TValue3, TValue4, TTarget>(
        this ValueTask<Result<(TValue1, TValue2, TValue3, TValue4)>> source,
        Func<TValue1, TValue2, TValue3, TValue4, TTarget> mapper)
        => (await source.ConfigureAwait(false)).Map(mapper);
         
    public static async ValueTask<Result<TTarget>> MapAsync<TValue1, TValue2, TValue3, TValue4, TTarget>(
        this ValueTask<Result<(TValue1, TValue2, TValue3, TValue4)>> source,
        Func<TValue1, TValue2, TValue3, TValue4, ValueTask<TTarget>> mapper)
        => await (await source.ConfigureAwait(false)).MapAsync(mapper).ConfigureAwait(false);
        
    public static async ValueTask<Result<TTarget>> MapAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TTarget>(
        this Result<(TValue1, TValue2, TValue3, TValue4, TValue5)> source,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, ValueTask<TTarget>> mapper)
    {
        if (source.IsFailure) return source.Error;
        return await mapper(source.Value.Item1, source.Value.Item2, source.Value.Item3, source.Value.Item4, source.Value.Item5).ConfigureAwait(false);
    }
    
    public static async ValueTask<Result<TTarget>> MapAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TTarget>(
        this ValueTask<Result<(TValue1, TValue2, TValue3, TValue4, TValue5)>> source,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TTarget> mapper)
        => (await source.ConfigureAwait(false)).Map(mapper);
         
    public static async ValueTask<Result<TTarget>> MapAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TTarget>(
        this ValueTask<Result<(TValue1, TValue2, TValue3, TValue4, TValue5)>> source,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, ValueTask<TTarget>> mapper)
        => await (await source.ConfigureAwait(false)).MapAsync(mapper).ConfigureAwait(false);
        
    public static async ValueTask<Result<TTarget>> MapAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TTarget>(
        this Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)> source,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, ValueTask<TTarget>> mapper)
    {
        if (source.IsFailure) return source.Error;
        return await mapper(source.Value.Item1, source.Value.Item2, source.Value.Item3, source.Value.Item4, source.Value.Item5, source.Value.Item6).ConfigureAwait(false);
    }
    
    public static async ValueTask<Result<TTarget>> MapAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TTarget>(
        this ValueTask<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)>> source,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TTarget> mapper)
        => (await source.ConfigureAwait(false)).Map(mapper);
         
    public static async ValueTask<Result<TTarget>> MapAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TTarget>(
        this ValueTask<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)>> source,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, ValueTask<TTarget>> mapper)
        => await (await source.ConfigureAwait(false)).MapAsync(mapper).ConfigureAwait(false);
        
    public static async ValueTask<Result<TTarget>> MapAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TTarget>(
        this Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)> source,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, ValueTask<TTarget>> mapper)
    {
        if (source.IsFailure) return source.Error;
        return await mapper(source.Value.Item1, source.Value.Item2, source.Value.Item3, source.Value.Item4, source.Value.Item5, source.Value.Item6, source.Value.Item7).ConfigureAwait(false);
    }
    
    public static async ValueTask<Result<TTarget>> MapAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TTarget>(
        this ValueTask<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)>> source,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TTarget> mapper)
        => (await source.ConfigureAwait(false)).Map(mapper);
         
    public static async ValueTask<Result<TTarget>> MapAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TTarget>(
        this ValueTask<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)>> source,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, ValueTask<TTarget>> mapper)
        => await (await source.ConfigureAwait(false)).MapAsync(mapper).ConfigureAwait(false);
        
    public static async ValueTask<Result<TTarget>> MapAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, TTarget>(
        this Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)> source,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, ValueTask<TTarget>> mapper)
    {
        if (source.IsFailure) return source.Error;
        return await mapper(source.Value.Item1, source.Value.Item2, source.Value.Item3, source.Value.Item4, source.Value.Item5, source.Value.Item6, source.Value.Item7, source.Value.Item8).ConfigureAwait(false);
    }
    
    public static async ValueTask<Result<TTarget>> MapAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, TTarget>(
        this ValueTask<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)>> source,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, TTarget> mapper)
        => (await source.ConfigureAwait(false)).Map(mapper);
         
    public static async ValueTask<Result<TTarget>> MapAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, TTarget>(
        this ValueTask<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)>> source,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, ValueTask<TTarget>> mapper)
        => await (await source.ConfigureAwait(false)).MapAsync(mapper).ConfigureAwait(false);
        
}
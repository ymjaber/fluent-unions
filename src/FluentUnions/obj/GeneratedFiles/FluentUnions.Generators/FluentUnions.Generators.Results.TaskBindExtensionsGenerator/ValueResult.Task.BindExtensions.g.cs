namespace FluentUnions;

public static partial class ValueResultExtensions
{
    public static Task<Result<TTarget>> BindAsync<TValue1, TValue2, TTarget>(
        in this Result<(TValue1, TValue2)> source,
        Func<TValue1, TValue2, Task<Result<TTarget>>> binder)
    {
        if (source.IsFailure) return Task.FromResult(Result.Failure<TTarget>(source.Error));
        return binder(source.Value.Item1, source.Value.Item2);
    }
    
    public static async Task<Result<TTarget>> BindAsync<TValue1, TValue2, TTarget>(
        this Task<Result<(TValue1, TValue2)>> source,
        Func<TValue1, TValue2, Result<TTarget>> binder)
        => (await source.ConfigureAwait(false)).Bind(binder);

    public static async Task<Result<TTarget>> BindAsync<TValue1, TValue2, TTarget>(
        this Task<Result<(TValue1, TValue2)>> source,
        Func<TValue1, TValue2, Task<Result<TTarget>>> binder)
        => await (await source.ConfigureAwait(false)).BindAsync(binder).ConfigureAwait(false);
        
    public static Task<Result<TTarget>> BindAsync<TValue1, TValue2, TValue3, TTarget>(
        in this Result<(TValue1, TValue2, TValue3)> source,
        Func<TValue1, TValue2, TValue3, Task<Result<TTarget>>> binder)
    {
        if (source.IsFailure) return Task.FromResult(Result.Failure<TTarget>(source.Error));
        return binder(source.Value.Item1, source.Value.Item2, source.Value.Item3);
    }
    
    public static async Task<Result<TTarget>> BindAsync<TValue1, TValue2, TValue3, TTarget>(
        this Task<Result<(TValue1, TValue2, TValue3)>> source,
        Func<TValue1, TValue2, TValue3, Result<TTarget>> binder)
        => (await source.ConfigureAwait(false)).Bind(binder);

    public static async Task<Result<TTarget>> BindAsync<TValue1, TValue2, TValue3, TTarget>(
        this Task<Result<(TValue1, TValue2, TValue3)>> source,
        Func<TValue1, TValue2, TValue3, Task<Result<TTarget>>> binder)
        => await (await source.ConfigureAwait(false)).BindAsync(binder).ConfigureAwait(false);
        
    public static Task<Result<TTarget>> BindAsync<TValue1, TValue2, TValue3, TValue4, TTarget>(
        in this Result<(TValue1, TValue2, TValue3, TValue4)> source,
        Func<TValue1, TValue2, TValue3, TValue4, Task<Result<TTarget>>> binder)
    {
        if (source.IsFailure) return Task.FromResult(Result.Failure<TTarget>(source.Error));
        return binder(source.Value.Item1, source.Value.Item2, source.Value.Item3, source.Value.Item4);
    }
    
    public static async Task<Result<TTarget>> BindAsync<TValue1, TValue2, TValue3, TValue4, TTarget>(
        this Task<Result<(TValue1, TValue2, TValue3, TValue4)>> source,
        Func<TValue1, TValue2, TValue3, TValue4, Result<TTarget>> binder)
        => (await source.ConfigureAwait(false)).Bind(binder);

    public static async Task<Result<TTarget>> BindAsync<TValue1, TValue2, TValue3, TValue4, TTarget>(
        this Task<Result<(TValue1, TValue2, TValue3, TValue4)>> source,
        Func<TValue1, TValue2, TValue3, TValue4, Task<Result<TTarget>>> binder)
        => await (await source.ConfigureAwait(false)).BindAsync(binder).ConfigureAwait(false);
        
    public static Task<Result<TTarget>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TTarget>(
        in this Result<(TValue1, TValue2, TValue3, TValue4, TValue5)> source,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, Task<Result<TTarget>>> binder)
    {
        if (source.IsFailure) return Task.FromResult(Result.Failure<TTarget>(source.Error));
        return binder(source.Value.Item1, source.Value.Item2, source.Value.Item3, source.Value.Item4, source.Value.Item5);
    }
    
    public static async Task<Result<TTarget>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TTarget>(
        this Task<Result<(TValue1, TValue2, TValue3, TValue4, TValue5)>> source,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, Result<TTarget>> binder)
        => (await source.ConfigureAwait(false)).Bind(binder);

    public static async Task<Result<TTarget>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TTarget>(
        this Task<Result<(TValue1, TValue2, TValue3, TValue4, TValue5)>> source,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, Task<Result<TTarget>>> binder)
        => await (await source.ConfigureAwait(false)).BindAsync(binder).ConfigureAwait(false);
        
    public static Task<Result<TTarget>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TTarget>(
        in this Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)> source,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, Task<Result<TTarget>>> binder)
    {
        if (source.IsFailure) return Task.FromResult(Result.Failure<TTarget>(source.Error));
        return binder(source.Value.Item1, source.Value.Item2, source.Value.Item3, source.Value.Item4, source.Value.Item5, source.Value.Item6);
    }
    
    public static async Task<Result<TTarget>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TTarget>(
        this Task<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)>> source,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, Result<TTarget>> binder)
        => (await source.ConfigureAwait(false)).Bind(binder);

    public static async Task<Result<TTarget>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TTarget>(
        this Task<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)>> source,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, Task<Result<TTarget>>> binder)
        => await (await source.ConfigureAwait(false)).BindAsync(binder).ConfigureAwait(false);
        
    public static Task<Result<TTarget>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TTarget>(
        in this Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)> source,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, Task<Result<TTarget>>> binder)
    {
        if (source.IsFailure) return Task.FromResult(Result.Failure<TTarget>(source.Error));
        return binder(source.Value.Item1, source.Value.Item2, source.Value.Item3, source.Value.Item4, source.Value.Item5, source.Value.Item6, source.Value.Item7);
    }
    
    public static async Task<Result<TTarget>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TTarget>(
        this Task<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)>> source,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, Result<TTarget>> binder)
        => (await source.ConfigureAwait(false)).Bind(binder);

    public static async Task<Result<TTarget>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TTarget>(
        this Task<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)>> source,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, Task<Result<TTarget>>> binder)
        => await (await source.ConfigureAwait(false)).BindAsync(binder).ConfigureAwait(false);
        
    public static Task<Result<TTarget>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, TTarget>(
        in this Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)> source,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, Task<Result<TTarget>>> binder)
    {
        if (source.IsFailure) return Task.FromResult(Result.Failure<TTarget>(source.Error));
        return binder(source.Value.Item1, source.Value.Item2, source.Value.Item3, source.Value.Item4, source.Value.Item5, source.Value.Item6, source.Value.Item7, source.Value.Item8);
    }
    
    public static async Task<Result<TTarget>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, TTarget>(
        this Task<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)>> source,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, Result<TTarget>> binder)
        => (await source.ConfigureAwait(false)).Bind(binder);

    public static async Task<Result<TTarget>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, TTarget>(
        this Task<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)>> source,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, Task<Result<TTarget>>> binder)
        => await (await source.ConfigureAwait(false)).BindAsync(binder).ConfigureAwait(false);
        
}
namespace FluentUnions;

public static partial class ValueResultExtensions
{
    public static async Task<Result<(TValue1, TValue2)>> EnsureAsync<TValue1, TValue2>(
        this Result<(TValue1, TValue2)> result,
        Func<TValue1, TValue2, Task<bool>> predicate,
        Error error)
    {
        if (result.IsFailure) return result;
    
        return await predicate(result.Value.Item1, result.Value.Item2).ConfigureAwait(false) ? result : error;
    }

    public static async Task<Result<(TValue1, TValue2)>> EnsureAsync<TValue1, TValue2>(
        this Task<Result<(TValue1, TValue2)>> result,
        Func<TValue1, TValue2, bool> predicate,
        Error error)
        => (await result.ConfigureAwait(false)).Ensure(predicate, error);

    public static async Task<Result<(TValue1, TValue2)>> EnsureAsync<TValue1, TValue2>(
        this Task<Result<(TValue1, TValue2)>> result,
        Func<TValue1, TValue2, Task<bool>> predicate,
        Error error)
        => await (await result.ConfigureAwait(false)).EnsureAsync(predicate, error).ConfigureAwait(false);
        
    public static async Task<Result<(TValue1, TValue2, TValue3)>> EnsureAsync<TValue1, TValue2, TValue3>(
        this Result<(TValue1, TValue2, TValue3)> result,
        Func<TValue1, TValue2, TValue3, Task<bool>> predicate,
        Error error)
    {
        if (result.IsFailure) return result;
    
        return await predicate(result.Value.Item1, result.Value.Item2, result.Value.Item3).ConfigureAwait(false) ? result : error;
    }

    public static async Task<Result<(TValue1, TValue2, TValue3)>> EnsureAsync<TValue1, TValue2, TValue3>(
        this Task<Result<(TValue1, TValue2, TValue3)>> result,
        Func<TValue1, TValue2, TValue3, bool> predicate,
        Error error)
        => (await result.ConfigureAwait(false)).Ensure(predicate, error);

    public static async Task<Result<(TValue1, TValue2, TValue3)>> EnsureAsync<TValue1, TValue2, TValue3>(
        this Task<Result<(TValue1, TValue2, TValue3)>> result,
        Func<TValue1, TValue2, TValue3, Task<bool>> predicate,
        Error error)
        => await (await result.ConfigureAwait(false)).EnsureAsync(predicate, error).ConfigureAwait(false);
        
    public static async Task<Result<(TValue1, TValue2, TValue3, TValue4)>> EnsureAsync<TValue1, TValue2, TValue3, TValue4>(
        this Result<(TValue1, TValue2, TValue3, TValue4)> result,
        Func<TValue1, TValue2, TValue3, TValue4, Task<bool>> predicate,
        Error error)
    {
        if (result.IsFailure) return result;
    
        return await predicate(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4).ConfigureAwait(false) ? result : error;
    }

    public static async Task<Result<(TValue1, TValue2, TValue3, TValue4)>> EnsureAsync<TValue1, TValue2, TValue3, TValue4>(
        this Task<Result<(TValue1, TValue2, TValue3, TValue4)>> result,
        Func<TValue1, TValue2, TValue3, TValue4, bool> predicate,
        Error error)
        => (await result.ConfigureAwait(false)).Ensure(predicate, error);

    public static async Task<Result<(TValue1, TValue2, TValue3, TValue4)>> EnsureAsync<TValue1, TValue2, TValue3, TValue4>(
        this Task<Result<(TValue1, TValue2, TValue3, TValue4)>> result,
        Func<TValue1, TValue2, TValue3, TValue4, Task<bool>> predicate,
        Error error)
        => await (await result.ConfigureAwait(false)).EnsureAsync(predicate, error).ConfigureAwait(false);
        
    public static async Task<Result<(TValue1, TValue2, TValue3, TValue4, TValue5)>> EnsureAsync<TValue1, TValue2, TValue3, TValue4, TValue5>(
        this Result<(TValue1, TValue2, TValue3, TValue4, TValue5)> result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, Task<bool>> predicate,
        Error error)
    {
        if (result.IsFailure) return result;
    
        return await predicate(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5).ConfigureAwait(false) ? result : error;
    }

    public static async Task<Result<(TValue1, TValue2, TValue3, TValue4, TValue5)>> EnsureAsync<TValue1, TValue2, TValue3, TValue4, TValue5>(
        this Task<Result<(TValue1, TValue2, TValue3, TValue4, TValue5)>> result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, bool> predicate,
        Error error)
        => (await result.ConfigureAwait(false)).Ensure(predicate, error);

    public static async Task<Result<(TValue1, TValue2, TValue3, TValue4, TValue5)>> EnsureAsync<TValue1, TValue2, TValue3, TValue4, TValue5>(
        this Task<Result<(TValue1, TValue2, TValue3, TValue4, TValue5)>> result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, Task<bool>> predicate,
        Error error)
        => await (await result.ConfigureAwait(false)).EnsureAsync(predicate, error).ConfigureAwait(false);
        
    public static async Task<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)>> EnsureAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(
        this Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)> result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, Task<bool>> predicate,
        Error error)
    {
        if (result.IsFailure) return result;
    
        return await predicate(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, result.Value.Item6).ConfigureAwait(false) ? result : error;
    }

    public static async Task<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)>> EnsureAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(
        this Task<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)>> result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, bool> predicate,
        Error error)
        => (await result.ConfigureAwait(false)).Ensure(predicate, error);

    public static async Task<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)>> EnsureAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(
        this Task<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)>> result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, Task<bool>> predicate,
        Error error)
        => await (await result.ConfigureAwait(false)).EnsureAsync(predicate, error).ConfigureAwait(false);
        
    public static async Task<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)>> EnsureAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(
        this Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)> result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, Task<bool>> predicate,
        Error error)
    {
        if (result.IsFailure) return result;
    
        return await predicate(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, result.Value.Item6, result.Value.Item7).ConfigureAwait(false) ? result : error;
    }

    public static async Task<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)>> EnsureAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(
        this Task<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)>> result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, bool> predicate,
        Error error)
        => (await result.ConfigureAwait(false)).Ensure(predicate, error);

    public static async Task<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)>> EnsureAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(
        this Task<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)>> result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, Task<bool>> predicate,
        Error error)
        => await (await result.ConfigureAwait(false)).EnsureAsync(predicate, error).ConfigureAwait(false);
        
    public static async Task<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)>> EnsureAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(
        this Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)> result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, Task<bool>> predicate,
        Error error)
    {
        if (result.IsFailure) return result;
    
        return await predicate(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, result.Value.Item6, result.Value.Item7, result.Value.Item8).ConfigureAwait(false) ? result : error;
    }

    public static async Task<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)>> EnsureAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(
        this Task<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)>> result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, bool> predicate,
        Error error)
        => (await result.ConfigureAwait(false)).Ensure(predicate, error);

    public static async Task<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)>> EnsureAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(
        this Task<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)>> result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, Task<bool>> predicate,
        Error error)
        => await (await result.ConfigureAwait(false)).EnsureAsync(predicate, error).ConfigureAwait(false);
        
}
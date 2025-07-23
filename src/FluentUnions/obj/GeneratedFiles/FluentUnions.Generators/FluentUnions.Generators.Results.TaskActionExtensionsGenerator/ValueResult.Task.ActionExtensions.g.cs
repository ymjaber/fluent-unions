namespace FluentUnions;

public static partial class ValueResultExtensions
{
public static async Task<Result<(TValue1, TValue2)>> OnSuccessAsync<TValue1, TValue2>(this Result<(TValue1, TValue2)> result, Func<TValue1, TValue2, Task> action)
{
    if (result.IsSuccess) await action(result.Value.Item1, result.Value.Item2).ConfigureAwait(false);
    return result;
}

public static async Task<Result<(TValue1, TValue2)>> OnSuccessAsync<TValue1, TValue2>(this Task<Result<(TValue1, TValue2)>> result, Action<TValue1, TValue2> action)
    => (await result.ConfigureAwait(false)).OnSuccess(action);
    
public static async Task<Result<(TValue1, TValue2)>> OnSuccessAsync<TValue1, TValue2>(this Task<Result<(TValue1, TValue2)>> result, Func<TValue1, TValue2, Task> action)
    => await (await result.ConfigureAwait(false)).OnSuccessAsync(action).ConfigureAwait(false);

public static async Task<Result<(TValue1, TValue2)>> OnEitherAsync<TValue1, TValue2>(this Result<(TValue1, TValue2)> result, Func<TValue1, TValue2, Task> success, Func<Error, Task> failure)
{
    if (result.IsSuccess) await success(result.Value.Item1, result.Value.Item2).ConfigureAwait(false);
    else await failure(result.Error).ConfigureAwait(false);
    return result;
}

public static async Task<Result<(TValue1, TValue2)>> OnEitherAsync<TValue1, TValue2>(this Task<Result<(TValue1, TValue2)>> result, Action<TValue1, TValue2> success, Action<Error> failure)
    => (await result.ConfigureAwait(false)).OnEither(success, failure);
    
public static async Task<Result<(TValue1, TValue2)>> OnEitherAsync<TValue1, TValue2>(this Task<Result<(TValue1, TValue2)>> result, Func<TValue1, TValue2, Task> success, Func<Error, Task> failure)
    => await (await result.ConfigureAwait(false)).OnEitherAsync(success, failure).ConfigureAwait(false);
    
    public static async Task<Result<(TValue1, TValue2, TValue3)>> OnSuccessAsync<TValue1, TValue2, TValue3>(this Result<(TValue1, TValue2, TValue3)> result, Func<TValue1, TValue2, TValue3, Task> action)
{
    if (result.IsSuccess) await action(result.Value.Item1, result.Value.Item2, result.Value.Item3).ConfigureAwait(false);
    return result;
}

public static async Task<Result<(TValue1, TValue2, TValue3)>> OnSuccessAsync<TValue1, TValue2, TValue3>(this Task<Result<(TValue1, TValue2, TValue3)>> result, Action<TValue1, TValue2, TValue3> action)
    => (await result.ConfigureAwait(false)).OnSuccess(action);
    
public static async Task<Result<(TValue1, TValue2, TValue3)>> OnSuccessAsync<TValue1, TValue2, TValue3>(this Task<Result<(TValue1, TValue2, TValue3)>> result, Func<TValue1, TValue2, TValue3, Task> action)
    => await (await result.ConfigureAwait(false)).OnSuccessAsync(action).ConfigureAwait(false);

public static async Task<Result<(TValue1, TValue2, TValue3)>> OnEitherAsync<TValue1, TValue2, TValue3>(this Result<(TValue1, TValue2, TValue3)> result, Func<TValue1, TValue2, TValue3, Task> success, Func<Error, Task> failure)
{
    if (result.IsSuccess) await success(result.Value.Item1, result.Value.Item2, result.Value.Item3).ConfigureAwait(false);
    else await failure(result.Error).ConfigureAwait(false);
    return result;
}

public static async Task<Result<(TValue1, TValue2, TValue3)>> OnEitherAsync<TValue1, TValue2, TValue3>(this Task<Result<(TValue1, TValue2, TValue3)>> result, Action<TValue1, TValue2, TValue3> success, Action<Error> failure)
    => (await result.ConfigureAwait(false)).OnEither(success, failure);
    
public static async Task<Result<(TValue1, TValue2, TValue3)>> OnEitherAsync<TValue1, TValue2, TValue3>(this Task<Result<(TValue1, TValue2, TValue3)>> result, Func<TValue1, TValue2, TValue3, Task> success, Func<Error, Task> failure)
    => await (await result.ConfigureAwait(false)).OnEitherAsync(success, failure).ConfigureAwait(false);
    
    public static async Task<Result<(TValue1, TValue2, TValue3, TValue4)>> OnSuccessAsync<TValue1, TValue2, TValue3, TValue4>(this Result<(TValue1, TValue2, TValue3, TValue4)> result, Func<TValue1, TValue2, TValue3, TValue4, Task> action)
{
    if (result.IsSuccess) await action(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4).ConfigureAwait(false);
    return result;
}

public static async Task<Result<(TValue1, TValue2, TValue3, TValue4)>> OnSuccessAsync<TValue1, TValue2, TValue3, TValue4>(this Task<Result<(TValue1, TValue2, TValue3, TValue4)>> result, Action<TValue1, TValue2, TValue3, TValue4> action)
    => (await result.ConfigureAwait(false)).OnSuccess(action);
    
public static async Task<Result<(TValue1, TValue2, TValue3, TValue4)>> OnSuccessAsync<TValue1, TValue2, TValue3, TValue4>(this Task<Result<(TValue1, TValue2, TValue3, TValue4)>> result, Func<TValue1, TValue2, TValue3, TValue4, Task> action)
    => await (await result.ConfigureAwait(false)).OnSuccessAsync(action).ConfigureAwait(false);

public static async Task<Result<(TValue1, TValue2, TValue3, TValue4)>> OnEitherAsync<TValue1, TValue2, TValue3, TValue4>(this Result<(TValue1, TValue2, TValue3, TValue4)> result, Func<TValue1, TValue2, TValue3, TValue4, Task> success, Func<Error, Task> failure)
{
    if (result.IsSuccess) await success(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4).ConfigureAwait(false);
    else await failure(result.Error).ConfigureAwait(false);
    return result;
}

public static async Task<Result<(TValue1, TValue2, TValue3, TValue4)>> OnEitherAsync<TValue1, TValue2, TValue3, TValue4>(this Task<Result<(TValue1, TValue2, TValue3, TValue4)>> result, Action<TValue1, TValue2, TValue3, TValue4> success, Action<Error> failure)
    => (await result.ConfigureAwait(false)).OnEither(success, failure);
    
public static async Task<Result<(TValue1, TValue2, TValue3, TValue4)>> OnEitherAsync<TValue1, TValue2, TValue3, TValue4>(this Task<Result<(TValue1, TValue2, TValue3, TValue4)>> result, Func<TValue1, TValue2, TValue3, TValue4, Task> success, Func<Error, Task> failure)
    => await (await result.ConfigureAwait(false)).OnEitherAsync(success, failure).ConfigureAwait(false);
    
    public static async Task<Result<(TValue1, TValue2, TValue3, TValue4, TValue5)>> OnSuccessAsync<TValue1, TValue2, TValue3, TValue4, TValue5>(this Result<(TValue1, TValue2, TValue3, TValue4, TValue5)> result, Func<TValue1, TValue2, TValue3, TValue4, TValue5, Task> action)
{
    if (result.IsSuccess) await action(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5).ConfigureAwait(false);
    return result;
}

public static async Task<Result<(TValue1, TValue2, TValue3, TValue4, TValue5)>> OnSuccessAsync<TValue1, TValue2, TValue3, TValue4, TValue5>(this Task<Result<(TValue1, TValue2, TValue3, TValue4, TValue5)>> result, Action<TValue1, TValue2, TValue3, TValue4, TValue5> action)
    => (await result.ConfigureAwait(false)).OnSuccess(action);
    
public static async Task<Result<(TValue1, TValue2, TValue3, TValue4, TValue5)>> OnSuccessAsync<TValue1, TValue2, TValue3, TValue4, TValue5>(this Task<Result<(TValue1, TValue2, TValue3, TValue4, TValue5)>> result, Func<TValue1, TValue2, TValue3, TValue4, TValue5, Task> action)
    => await (await result.ConfigureAwait(false)).OnSuccessAsync(action).ConfigureAwait(false);

public static async Task<Result<(TValue1, TValue2, TValue3, TValue4, TValue5)>> OnEitherAsync<TValue1, TValue2, TValue3, TValue4, TValue5>(this Result<(TValue1, TValue2, TValue3, TValue4, TValue5)> result, Func<TValue1, TValue2, TValue3, TValue4, TValue5, Task> success, Func<Error, Task> failure)
{
    if (result.IsSuccess) await success(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5).ConfigureAwait(false);
    else await failure(result.Error).ConfigureAwait(false);
    return result;
}

public static async Task<Result<(TValue1, TValue2, TValue3, TValue4, TValue5)>> OnEitherAsync<TValue1, TValue2, TValue3, TValue4, TValue5>(this Task<Result<(TValue1, TValue2, TValue3, TValue4, TValue5)>> result, Action<TValue1, TValue2, TValue3, TValue4, TValue5> success, Action<Error> failure)
    => (await result.ConfigureAwait(false)).OnEither(success, failure);
    
public static async Task<Result<(TValue1, TValue2, TValue3, TValue4, TValue5)>> OnEitherAsync<TValue1, TValue2, TValue3, TValue4, TValue5>(this Task<Result<(TValue1, TValue2, TValue3, TValue4, TValue5)>> result, Func<TValue1, TValue2, TValue3, TValue4, TValue5, Task> success, Func<Error, Task> failure)
    => await (await result.ConfigureAwait(false)).OnEitherAsync(success, failure).ConfigureAwait(false);
    
    public static async Task<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)>> OnSuccessAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(this Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)> result, Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, Task> action)
{
    if (result.IsSuccess) await action(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, result.Value.Item6).ConfigureAwait(false);
    return result;
}

public static async Task<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)>> OnSuccessAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(this Task<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)>> result, Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> action)
    => (await result.ConfigureAwait(false)).OnSuccess(action);
    
public static async Task<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)>> OnSuccessAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(this Task<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)>> result, Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, Task> action)
    => await (await result.ConfigureAwait(false)).OnSuccessAsync(action).ConfigureAwait(false);

public static async Task<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)>> OnEitherAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(this Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)> result, Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, Task> success, Func<Error, Task> failure)
{
    if (result.IsSuccess) await success(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, result.Value.Item6).ConfigureAwait(false);
    else await failure(result.Error).ConfigureAwait(false);
    return result;
}

public static async Task<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)>> OnEitherAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(this Task<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)>> result, Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> success, Action<Error> failure)
    => (await result.ConfigureAwait(false)).OnEither(success, failure);
    
public static async Task<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)>> OnEitherAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(this Task<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)>> result, Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, Task> success, Func<Error, Task> failure)
    => await (await result.ConfigureAwait(false)).OnEitherAsync(success, failure).ConfigureAwait(false);
    
    public static async Task<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)>> OnSuccessAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(this Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)> result, Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, Task> action)
{
    if (result.IsSuccess) await action(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, result.Value.Item6, result.Value.Item7).ConfigureAwait(false);
    return result;
}

public static async Task<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)>> OnSuccessAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(this Task<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)>> result, Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> action)
    => (await result.ConfigureAwait(false)).OnSuccess(action);
    
public static async Task<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)>> OnSuccessAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(this Task<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)>> result, Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, Task> action)
    => await (await result.ConfigureAwait(false)).OnSuccessAsync(action).ConfigureAwait(false);

public static async Task<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)>> OnEitherAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(this Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)> result, Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, Task> success, Func<Error, Task> failure)
{
    if (result.IsSuccess) await success(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, result.Value.Item6, result.Value.Item7).ConfigureAwait(false);
    else await failure(result.Error).ConfigureAwait(false);
    return result;
}

public static async Task<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)>> OnEitherAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(this Task<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)>> result, Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> success, Action<Error> failure)
    => (await result.ConfigureAwait(false)).OnEither(success, failure);
    
public static async Task<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)>> OnEitherAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(this Task<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)>> result, Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, Task> success, Func<Error, Task> failure)
    => await (await result.ConfigureAwait(false)).OnEitherAsync(success, failure).ConfigureAwait(false);
    
    public static async Task<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)>> OnSuccessAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(this Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)> result, Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, Task> action)
{
    if (result.IsSuccess) await action(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, result.Value.Item6, result.Value.Item7, result.Value.Item8).ConfigureAwait(false);
    return result;
}

public static async Task<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)>> OnSuccessAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(this Task<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)>> result, Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> action)
    => (await result.ConfigureAwait(false)).OnSuccess(action);
    
public static async Task<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)>> OnSuccessAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(this Task<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)>> result, Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, Task> action)
    => await (await result.ConfigureAwait(false)).OnSuccessAsync(action).ConfigureAwait(false);

public static async Task<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)>> OnEitherAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(this Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)> result, Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, Task> success, Func<Error, Task> failure)
{
    if (result.IsSuccess) await success(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, result.Value.Item6, result.Value.Item7, result.Value.Item8).ConfigureAwait(false);
    else await failure(result.Error).ConfigureAwait(false);
    return result;
}

public static async Task<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)>> OnEitherAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(this Task<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)>> result, Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> success, Action<Error> failure)
    => (await result.ConfigureAwait(false)).OnEither(success, failure);
    
public static async Task<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)>> OnEitherAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(this Task<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)>> result, Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, Task> success, Func<Error, Task> failure)
    => await (await result.ConfigureAwait(false)).OnEitherAsync(success, failure).ConfigureAwait(false);
    
    }
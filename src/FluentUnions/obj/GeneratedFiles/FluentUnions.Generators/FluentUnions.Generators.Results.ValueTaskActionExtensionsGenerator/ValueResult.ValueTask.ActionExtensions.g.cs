namespace FluentUnions;

public static partial class ValueResultExtensions
{
public static async ValueTask<Result<(TValue1, TValue2)>> OnSuccessAsync<TValue1, TValue2>(this Result<(TValue1, TValue2)> result, Func<TValue1, TValue2, ValueTask> action)
{
    if (result.IsSuccess) await action(result.Value.Item1, result.Value.Item2).ConfigureAwait(false);
    return result;
}

public static async ValueTask<Result<(TValue1, TValue2)>> OnSuccessAsync<TValue1, TValue2>(this ValueTask<Result<(TValue1, TValue2)>> result, Action<TValue1, TValue2> action)
    => (await result.ConfigureAwait(false)).OnSuccess(action);
    
public static async ValueTask<Result<(TValue1, TValue2)>> OnSuccessAsync<TValue1, TValue2>(this ValueTask<Result<(TValue1, TValue2)>> result, Func<TValue1, TValue2, ValueTask> action)
    => await (await result.ConfigureAwait(false)).OnSuccessAsync(action).ConfigureAwait(false);

public static async ValueTask<Result<(TValue1, TValue2)>> OnEitherAsync<TValue1, TValue2>(this Result<(TValue1, TValue2)> result, Func<TValue1, TValue2, ValueTask> success, Func<Error, ValueTask> failure)
{
    if (result.IsSuccess) await success(result.Value.Item1, result.Value.Item2).ConfigureAwait(false);
    else await failure(result.Error).ConfigureAwait(false);
    return result;
}

public static async ValueTask<Result<(TValue1, TValue2)>> OnEitherAsync<TValue1, TValue2>(this ValueTask<Result<(TValue1, TValue2)>> result, Action<TValue1, TValue2> success, Action<Error> failure)
    => (await result.ConfigureAwait(false)).OnEither(success, failure);
    
public static async ValueTask<Result<(TValue1, TValue2)>> OnEitherAsync<TValue1, TValue2>(this ValueTask<Result<(TValue1, TValue2)>> result, Func<TValue1, TValue2, ValueTask> success, Func<Error, ValueTask> failure)
    => await (await result.ConfigureAwait(false)).OnEitherAsync(success, failure).ConfigureAwait(false);
    
    public static async ValueTask<Result<(TValue1, TValue2, TValue3)>> OnSuccessAsync<TValue1, TValue2, TValue3>(this Result<(TValue1, TValue2, TValue3)> result, Func<TValue1, TValue2, TValue3, ValueTask> action)
{
    if (result.IsSuccess) await action(result.Value.Item1, result.Value.Item2, result.Value.Item3).ConfigureAwait(false);
    return result;
}

public static async ValueTask<Result<(TValue1, TValue2, TValue3)>> OnSuccessAsync<TValue1, TValue2, TValue3>(this ValueTask<Result<(TValue1, TValue2, TValue3)>> result, Action<TValue1, TValue2, TValue3> action)
    => (await result.ConfigureAwait(false)).OnSuccess(action);
    
public static async ValueTask<Result<(TValue1, TValue2, TValue3)>> OnSuccessAsync<TValue1, TValue2, TValue3>(this ValueTask<Result<(TValue1, TValue2, TValue3)>> result, Func<TValue1, TValue2, TValue3, ValueTask> action)
    => await (await result.ConfigureAwait(false)).OnSuccessAsync(action).ConfigureAwait(false);

public static async ValueTask<Result<(TValue1, TValue2, TValue3)>> OnEitherAsync<TValue1, TValue2, TValue3>(this Result<(TValue1, TValue2, TValue3)> result, Func<TValue1, TValue2, TValue3, ValueTask> success, Func<Error, ValueTask> failure)
{
    if (result.IsSuccess) await success(result.Value.Item1, result.Value.Item2, result.Value.Item3).ConfigureAwait(false);
    else await failure(result.Error).ConfigureAwait(false);
    return result;
}

public static async ValueTask<Result<(TValue1, TValue2, TValue3)>> OnEitherAsync<TValue1, TValue2, TValue3>(this ValueTask<Result<(TValue1, TValue2, TValue3)>> result, Action<TValue1, TValue2, TValue3> success, Action<Error> failure)
    => (await result.ConfigureAwait(false)).OnEither(success, failure);
    
public static async ValueTask<Result<(TValue1, TValue2, TValue3)>> OnEitherAsync<TValue1, TValue2, TValue3>(this ValueTask<Result<(TValue1, TValue2, TValue3)>> result, Func<TValue1, TValue2, TValue3, ValueTask> success, Func<Error, ValueTask> failure)
    => await (await result.ConfigureAwait(false)).OnEitherAsync(success, failure).ConfigureAwait(false);
    
    public static async ValueTask<Result<(TValue1, TValue2, TValue3, TValue4)>> OnSuccessAsync<TValue1, TValue2, TValue3, TValue4>(this Result<(TValue1, TValue2, TValue3, TValue4)> result, Func<TValue1, TValue2, TValue3, TValue4, ValueTask> action)
{
    if (result.IsSuccess) await action(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4).ConfigureAwait(false);
    return result;
}

public static async ValueTask<Result<(TValue1, TValue2, TValue3, TValue4)>> OnSuccessAsync<TValue1, TValue2, TValue3, TValue4>(this ValueTask<Result<(TValue1, TValue2, TValue3, TValue4)>> result, Action<TValue1, TValue2, TValue3, TValue4> action)
    => (await result.ConfigureAwait(false)).OnSuccess(action);
    
public static async ValueTask<Result<(TValue1, TValue2, TValue3, TValue4)>> OnSuccessAsync<TValue1, TValue2, TValue3, TValue4>(this ValueTask<Result<(TValue1, TValue2, TValue3, TValue4)>> result, Func<TValue1, TValue2, TValue3, TValue4, ValueTask> action)
    => await (await result.ConfigureAwait(false)).OnSuccessAsync(action).ConfigureAwait(false);

public static async ValueTask<Result<(TValue1, TValue2, TValue3, TValue4)>> OnEitherAsync<TValue1, TValue2, TValue3, TValue4>(this Result<(TValue1, TValue2, TValue3, TValue4)> result, Func<TValue1, TValue2, TValue3, TValue4, ValueTask> success, Func<Error, ValueTask> failure)
{
    if (result.IsSuccess) await success(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4).ConfigureAwait(false);
    else await failure(result.Error).ConfigureAwait(false);
    return result;
}

public static async ValueTask<Result<(TValue1, TValue2, TValue3, TValue4)>> OnEitherAsync<TValue1, TValue2, TValue3, TValue4>(this ValueTask<Result<(TValue1, TValue2, TValue3, TValue4)>> result, Action<TValue1, TValue2, TValue3, TValue4> success, Action<Error> failure)
    => (await result.ConfigureAwait(false)).OnEither(success, failure);
    
public static async ValueTask<Result<(TValue1, TValue2, TValue3, TValue4)>> OnEitherAsync<TValue1, TValue2, TValue3, TValue4>(this ValueTask<Result<(TValue1, TValue2, TValue3, TValue4)>> result, Func<TValue1, TValue2, TValue3, TValue4, ValueTask> success, Func<Error, ValueTask> failure)
    => await (await result.ConfigureAwait(false)).OnEitherAsync(success, failure).ConfigureAwait(false);
    
    public static async ValueTask<Result<(TValue1, TValue2, TValue3, TValue4, TValue5)>> OnSuccessAsync<TValue1, TValue2, TValue3, TValue4, TValue5>(this Result<(TValue1, TValue2, TValue3, TValue4, TValue5)> result, Func<TValue1, TValue2, TValue3, TValue4, TValue5, ValueTask> action)
{
    if (result.IsSuccess) await action(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5).ConfigureAwait(false);
    return result;
}

public static async ValueTask<Result<(TValue1, TValue2, TValue3, TValue4, TValue5)>> OnSuccessAsync<TValue1, TValue2, TValue3, TValue4, TValue5>(this ValueTask<Result<(TValue1, TValue2, TValue3, TValue4, TValue5)>> result, Action<TValue1, TValue2, TValue3, TValue4, TValue5> action)
    => (await result.ConfigureAwait(false)).OnSuccess(action);
    
public static async ValueTask<Result<(TValue1, TValue2, TValue3, TValue4, TValue5)>> OnSuccessAsync<TValue1, TValue2, TValue3, TValue4, TValue5>(this ValueTask<Result<(TValue1, TValue2, TValue3, TValue4, TValue5)>> result, Func<TValue1, TValue2, TValue3, TValue4, TValue5, ValueTask> action)
    => await (await result.ConfigureAwait(false)).OnSuccessAsync(action).ConfigureAwait(false);

public static async ValueTask<Result<(TValue1, TValue2, TValue3, TValue4, TValue5)>> OnEitherAsync<TValue1, TValue2, TValue3, TValue4, TValue5>(this Result<(TValue1, TValue2, TValue3, TValue4, TValue5)> result, Func<TValue1, TValue2, TValue3, TValue4, TValue5, ValueTask> success, Func<Error, ValueTask> failure)
{
    if (result.IsSuccess) await success(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5).ConfigureAwait(false);
    else await failure(result.Error).ConfigureAwait(false);
    return result;
}

public static async ValueTask<Result<(TValue1, TValue2, TValue3, TValue4, TValue5)>> OnEitherAsync<TValue1, TValue2, TValue3, TValue4, TValue5>(this ValueTask<Result<(TValue1, TValue2, TValue3, TValue4, TValue5)>> result, Action<TValue1, TValue2, TValue3, TValue4, TValue5> success, Action<Error> failure)
    => (await result.ConfigureAwait(false)).OnEither(success, failure);
    
public static async ValueTask<Result<(TValue1, TValue2, TValue3, TValue4, TValue5)>> OnEitherAsync<TValue1, TValue2, TValue3, TValue4, TValue5>(this ValueTask<Result<(TValue1, TValue2, TValue3, TValue4, TValue5)>> result, Func<TValue1, TValue2, TValue3, TValue4, TValue5, ValueTask> success, Func<Error, ValueTask> failure)
    => await (await result.ConfigureAwait(false)).OnEitherAsync(success, failure).ConfigureAwait(false);
    
    public static async ValueTask<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)>> OnSuccessAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(this Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)> result, Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, ValueTask> action)
{
    if (result.IsSuccess) await action(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, result.Value.Item6).ConfigureAwait(false);
    return result;
}

public static async ValueTask<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)>> OnSuccessAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(this ValueTask<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)>> result, Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> action)
    => (await result.ConfigureAwait(false)).OnSuccess(action);
    
public static async ValueTask<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)>> OnSuccessAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(this ValueTask<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)>> result, Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, ValueTask> action)
    => await (await result.ConfigureAwait(false)).OnSuccessAsync(action).ConfigureAwait(false);

public static async ValueTask<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)>> OnEitherAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(this Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)> result, Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, ValueTask> success, Func<Error, ValueTask> failure)
{
    if (result.IsSuccess) await success(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, result.Value.Item6).ConfigureAwait(false);
    else await failure(result.Error).ConfigureAwait(false);
    return result;
}

public static async ValueTask<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)>> OnEitherAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(this ValueTask<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)>> result, Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> success, Action<Error> failure)
    => (await result.ConfigureAwait(false)).OnEither(success, failure);
    
public static async ValueTask<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)>> OnEitherAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(this ValueTask<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)>> result, Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, ValueTask> success, Func<Error, ValueTask> failure)
    => await (await result.ConfigureAwait(false)).OnEitherAsync(success, failure).ConfigureAwait(false);
    
    public static async ValueTask<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)>> OnSuccessAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(this Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)> result, Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, ValueTask> action)
{
    if (result.IsSuccess) await action(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, result.Value.Item6, result.Value.Item7).ConfigureAwait(false);
    return result;
}

public static async ValueTask<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)>> OnSuccessAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(this ValueTask<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)>> result, Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> action)
    => (await result.ConfigureAwait(false)).OnSuccess(action);
    
public static async ValueTask<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)>> OnSuccessAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(this ValueTask<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)>> result, Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, ValueTask> action)
    => await (await result.ConfigureAwait(false)).OnSuccessAsync(action).ConfigureAwait(false);

public static async ValueTask<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)>> OnEitherAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(this Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)> result, Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, ValueTask> success, Func<Error, ValueTask> failure)
{
    if (result.IsSuccess) await success(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, result.Value.Item6, result.Value.Item7).ConfigureAwait(false);
    else await failure(result.Error).ConfigureAwait(false);
    return result;
}

public static async ValueTask<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)>> OnEitherAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(this ValueTask<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)>> result, Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> success, Action<Error> failure)
    => (await result.ConfigureAwait(false)).OnEither(success, failure);
    
public static async ValueTask<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)>> OnEitherAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(this ValueTask<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)>> result, Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, ValueTask> success, Func<Error, ValueTask> failure)
    => await (await result.ConfigureAwait(false)).OnEitherAsync(success, failure).ConfigureAwait(false);
    
    public static async ValueTask<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)>> OnSuccessAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(this Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)> result, Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, ValueTask> action)
{
    if (result.IsSuccess) await action(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, result.Value.Item6, result.Value.Item7, result.Value.Item8).ConfigureAwait(false);
    return result;
}

public static async ValueTask<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)>> OnSuccessAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(this ValueTask<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)>> result, Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> action)
    => (await result.ConfigureAwait(false)).OnSuccess(action);
    
public static async ValueTask<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)>> OnSuccessAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(this ValueTask<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)>> result, Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, ValueTask> action)
    => await (await result.ConfigureAwait(false)).OnSuccessAsync(action).ConfigureAwait(false);

public static async ValueTask<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)>> OnEitherAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(this Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)> result, Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, ValueTask> success, Func<Error, ValueTask> failure)
{
    if (result.IsSuccess) await success(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, result.Value.Item6, result.Value.Item7, result.Value.Item8).ConfigureAwait(false);
    else await failure(result.Error).ConfigureAwait(false);
    return result;
}

public static async ValueTask<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)>> OnEitherAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(this ValueTask<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)>> result, Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> success, Action<Error> failure)
    => (await result.ConfigureAwait(false)).OnEither(success, failure);
    
public static async ValueTask<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)>> OnEitherAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(this ValueTask<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)>> result, Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, ValueTask> success, Func<Error, ValueTask> failure)
    => await (await result.ConfigureAwait(false)).OnEitherAsync(success, failure).ConfigureAwait(false);
    
    }
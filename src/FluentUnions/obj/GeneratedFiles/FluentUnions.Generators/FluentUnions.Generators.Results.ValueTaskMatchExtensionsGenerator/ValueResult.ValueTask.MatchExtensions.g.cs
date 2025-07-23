namespace FluentUnions;

public static partial class ValueResultExtensions
{
    public static ValueTask<TTarget> MatchAsync<TSource1, TSource2, TTarget>(
        in this Result<(TSource1, TSource2)> result,
        Func<TSource1, TSource2, ValueTask<TTarget>> success,
        Func<Error, ValueTask<TTarget>> failure)
    {
        return result.IsSuccess
            ? success(result.Value.Item1, result.Value.Item2)
            : failure(result.Error);
    }

    public static async ValueTask<TTarget> MatchAsync<TSource1, TSource2, TTarget>(
        this ValueTask<Result<(TSource1, TSource2)>> result,
        Func<TSource1, TSource2, TTarget> success,
        Func<Error, TTarget> failure)
        => (await result.ConfigureAwait(false)).Match(success, failure);
         
    public static async ValueTask<TTarget> MatchAsync<TSource1, TSource2, TTarget>(
        this ValueTask<Result<(TSource1, TSource2)>> result,
        Func<TSource1, TSource2, ValueTask<TTarget>> success,
        Func<Error, ValueTask<TTarget>> failure)
        => await (await result.ConfigureAwait(false)).MatchAsync(success, failure).ConfigureAwait(false);
        
    public static ValueTask<TTarget> MatchAsync<TSource1, TSource2, TSource3, TTarget>(
        in this Result<(TSource1, TSource2, TSource3)> result,
        Func<TSource1, TSource2, TSource3, ValueTask<TTarget>> success,
        Func<Error, ValueTask<TTarget>> failure)
    {
        return result.IsSuccess
            ? success(result.Value.Item1, result.Value.Item2, result.Value.Item3)
            : failure(result.Error);
    }

    public static async ValueTask<TTarget> MatchAsync<TSource1, TSource2, TSource3, TTarget>(
        this ValueTask<Result<(TSource1, TSource2, TSource3)>> result,
        Func<TSource1, TSource2, TSource3, TTarget> success,
        Func<Error, TTarget> failure)
        => (await result.ConfigureAwait(false)).Match(success, failure);
         
    public static async ValueTask<TTarget> MatchAsync<TSource1, TSource2, TSource3, TTarget>(
        this ValueTask<Result<(TSource1, TSource2, TSource3)>> result,
        Func<TSource1, TSource2, TSource3, ValueTask<TTarget>> success,
        Func<Error, ValueTask<TTarget>> failure)
        => await (await result.ConfigureAwait(false)).MatchAsync(success, failure).ConfigureAwait(false);
        
    public static ValueTask<TTarget> MatchAsync<TSource1, TSource2, TSource3, TSource4, TTarget>(
        in this Result<(TSource1, TSource2, TSource3, TSource4)> result,
        Func<TSource1, TSource2, TSource3, TSource4, ValueTask<TTarget>> success,
        Func<Error, ValueTask<TTarget>> failure)
    {
        return result.IsSuccess
            ? success(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4)
            : failure(result.Error);
    }

    public static async ValueTask<TTarget> MatchAsync<TSource1, TSource2, TSource3, TSource4, TTarget>(
        this ValueTask<Result<(TSource1, TSource2, TSource3, TSource4)>> result,
        Func<TSource1, TSource2, TSource3, TSource4, TTarget> success,
        Func<Error, TTarget> failure)
        => (await result.ConfigureAwait(false)).Match(success, failure);
         
    public static async ValueTask<TTarget> MatchAsync<TSource1, TSource2, TSource3, TSource4, TTarget>(
        this ValueTask<Result<(TSource1, TSource2, TSource3, TSource4)>> result,
        Func<TSource1, TSource2, TSource3, TSource4, ValueTask<TTarget>> success,
        Func<Error, ValueTask<TTarget>> failure)
        => await (await result.ConfigureAwait(false)).MatchAsync(success, failure).ConfigureAwait(false);
        
    public static ValueTask<TTarget> MatchAsync<TSource1, TSource2, TSource3, TSource4, TSource5, TTarget>(
        in this Result<(TSource1, TSource2, TSource3, TSource4, TSource5)> result,
        Func<TSource1, TSource2, TSource3, TSource4, TSource5, ValueTask<TTarget>> success,
        Func<Error, ValueTask<TTarget>> failure)
    {
        return result.IsSuccess
            ? success(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5)
            : failure(result.Error);
    }

    public static async ValueTask<TTarget> MatchAsync<TSource1, TSource2, TSource3, TSource4, TSource5, TTarget>(
        this ValueTask<Result<(TSource1, TSource2, TSource3, TSource4, TSource5)>> result,
        Func<TSource1, TSource2, TSource3, TSource4, TSource5, TTarget> success,
        Func<Error, TTarget> failure)
        => (await result.ConfigureAwait(false)).Match(success, failure);
         
    public static async ValueTask<TTarget> MatchAsync<TSource1, TSource2, TSource3, TSource4, TSource5, TTarget>(
        this ValueTask<Result<(TSource1, TSource2, TSource3, TSource4, TSource5)>> result,
        Func<TSource1, TSource2, TSource3, TSource4, TSource5, ValueTask<TTarget>> success,
        Func<Error, ValueTask<TTarget>> failure)
        => await (await result.ConfigureAwait(false)).MatchAsync(success, failure).ConfigureAwait(false);
        
    public static ValueTask<TTarget> MatchAsync<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TTarget>(
        in this Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6)> result,
        Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, ValueTask<TTarget>> success,
        Func<Error, ValueTask<TTarget>> failure)
    {
        return result.IsSuccess
            ? success(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, result.Value.Item6)
            : failure(result.Error);
    }

    public static async ValueTask<TTarget> MatchAsync<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TTarget>(
        this ValueTask<Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6)>> result,
        Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TTarget> success,
        Func<Error, TTarget> failure)
        => (await result.ConfigureAwait(false)).Match(success, failure);
         
    public static async ValueTask<TTarget> MatchAsync<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TTarget>(
        this ValueTask<Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6)>> result,
        Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, ValueTask<TTarget>> success,
        Func<Error, ValueTask<TTarget>> failure)
        => await (await result.ConfigureAwait(false)).MatchAsync(success, failure).ConfigureAwait(false);
        
    public static ValueTask<TTarget> MatchAsync<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TTarget>(
        in this Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7)> result,
        Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, ValueTask<TTarget>> success,
        Func<Error, ValueTask<TTarget>> failure)
    {
        return result.IsSuccess
            ? success(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, result.Value.Item6, result.Value.Item7)
            : failure(result.Error);
    }

    public static async ValueTask<TTarget> MatchAsync<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TTarget>(
        this ValueTask<Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7)>> result,
        Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TTarget> success,
        Func<Error, TTarget> failure)
        => (await result.ConfigureAwait(false)).Match(success, failure);
         
    public static async ValueTask<TTarget> MatchAsync<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TTarget>(
        this ValueTask<Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7)>> result,
        Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, ValueTask<TTarget>> success,
        Func<Error, ValueTask<TTarget>> failure)
        => await (await result.ConfigureAwait(false)).MatchAsync(success, failure).ConfigureAwait(false);
        
    public static ValueTask<TTarget> MatchAsync<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TTarget>(
        in this Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8)> result,
        Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, ValueTask<TTarget>> success,
        Func<Error, ValueTask<TTarget>> failure)
    {
        return result.IsSuccess
            ? success(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, result.Value.Item6, result.Value.Item7, result.Value.Item8)
            : failure(result.Error);
    }

    public static async ValueTask<TTarget> MatchAsync<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TTarget>(
        this ValueTask<Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8)>> result,
        Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TTarget> success,
        Func<Error, TTarget> failure)
        => (await result.ConfigureAwait(false)).Match(success, failure);
         
    public static async ValueTask<TTarget> MatchAsync<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TTarget>(
        this ValueTask<Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8)>> result,
        Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, ValueTask<TTarget>> success,
        Func<Error, ValueTask<TTarget>> failure)
        => await (await result.ConfigureAwait(false)).MatchAsync(success, failure).ConfigureAwait(false);
        
}
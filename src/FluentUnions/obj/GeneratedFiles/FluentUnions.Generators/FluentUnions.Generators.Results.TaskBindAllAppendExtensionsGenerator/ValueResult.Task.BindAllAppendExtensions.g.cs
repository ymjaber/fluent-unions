namespace FluentUnions;

public static partial class ValueResultExtensions
{
    public static async Task<Result<(TSource, TTarget)>> BindAllAppendAsync<TSource, TTarget>(
        this Result<TSource> result,
        Task<Result<TTarget>> binder)
    {
        var binderResult = await binder.ConfigureAwait(false);
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binderResult)
            .TryBuild(out Error error)
                ? error
                : (result.Value, binderResult.Value);
    }

    public static async Task<Result<(TSource, TTarget)>> BindAllAppendAsync<TSource, TTarget>(
        this Task<Result<TSource>> result,
        Result<TTarget> binder)
        => (await result.ConfigureAwait(false)).BindAllAppend(binder);

    public static async Task<Result<(TSource, TTarget)>> BindAllAppendAsync<TSource, TTarget>(
        this Task<Result<TSource>> result,
        Task<Result<TTarget>> binder)
        => await (await result.ConfigureAwait(false)).BindAllAppendAsync(binder).ConfigureAwait(false);
        
    public static async Task<Result<(TSource, TTarget1, TTarget2)>> BindAllAppendAsync<TSource, TTarget1, TTarget2>(
        this Result<TSource> result,
        Task<Result<(TTarget1, TTarget2)>> binder)
    {
        var binderResult = await binder.ConfigureAwait(false);
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binderResult)
            .TryBuild(out Error error)
                ? error
                : (result.Value, binderResult.Value.Item1, binderResult.Value.Item2);
    }

    public static async Task<Result<(TSource, TTarget1, TTarget2)>> BindAllAppendAsync<TSource, TTarget1, TTarget2>(
        this Task<Result<TSource>> result,
        Result<(TTarget1, TTarget2)> binder)
        => (await result.ConfigureAwait(false)).BindAllAppend(binder);

    public static async Task<Result<(TSource, TTarget1, TTarget2)>> BindAllAppendAsync<TSource, TTarget1, TTarget2>(
        this Task<Result<TSource>> result,
        Task<Result<(TTarget1, TTarget2)>> binder)
        => await (await result.ConfigureAwait(false)).BindAllAppendAsync(binder).ConfigureAwait(false);
        
    public static async Task<Result<(TSource1, TSource2, TTarget)>> BindAllAppendAsync<TSource1, TSource2, TTarget>(
        this Result<(TSource1, TSource2)> result,
        Task<Result<TTarget>> binder)
    {
        var binderResult = await binder.ConfigureAwait(false);
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binderResult)
            .TryBuild(out Error error)
                ? error
                : (result.Value.Item1, result.Value.Item2, binderResult.Value);
    }

    public static async Task<Result<(TSource1, TSource2, TTarget)>> BindAllAppendAsync<TSource1, TSource2, TTarget>(
        this Task<Result<(TSource1, TSource2)>> result,
        Result<TTarget> binder)
        => (await result.ConfigureAwait(false)).BindAllAppend(binder);

    public static async Task<Result<(TSource1, TSource2, TTarget)>> BindAllAppendAsync<TSource1, TSource2, TTarget>(
        this Task<Result<(TSource1, TSource2)>> result,
        Task<Result<TTarget>> binder)
        => await (await result.ConfigureAwait(false)).BindAllAppendAsync(binder).ConfigureAwait(false);
        
    public static async Task<Result<(TSource, TTarget1, TTarget2, TTarget3)>> BindAllAppendAsync<TSource, TTarget1, TTarget2, TTarget3>(
        this Result<TSource> result,
        Task<Result<(TTarget1, TTarget2, TTarget3)>> binder)
    {
        var binderResult = await binder.ConfigureAwait(false);
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binderResult)
            .TryBuild(out Error error)
                ? error
                : (result.Value, binderResult.Value.Item1, binderResult.Value.Item2, binderResult.Value.Item3);
    }

    public static async Task<Result<(TSource, TTarget1, TTarget2, TTarget3)>> BindAllAppendAsync<TSource, TTarget1, TTarget2, TTarget3>(
        this Task<Result<TSource>> result,
        Result<(TTarget1, TTarget2, TTarget3)> binder)
        => (await result.ConfigureAwait(false)).BindAllAppend(binder);

    public static async Task<Result<(TSource, TTarget1, TTarget2, TTarget3)>> BindAllAppendAsync<TSource, TTarget1, TTarget2, TTarget3>(
        this Task<Result<TSource>> result,
        Task<Result<(TTarget1, TTarget2, TTarget3)>> binder)
        => await (await result.ConfigureAwait(false)).BindAllAppendAsync(binder).ConfigureAwait(false);
        
    public static async Task<Result<(TSource1, TSource2, TTarget1, TTarget2)>> BindAllAppendAsync<TSource1, TSource2, TTarget1, TTarget2>(
        this Result<(TSource1, TSource2)> result,
        Task<Result<(TTarget1, TTarget2)>> binder)
    {
        var binderResult = await binder.ConfigureAwait(false);
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binderResult)
            .TryBuild(out Error error)
                ? error
                : (result.Value.Item1, result.Value.Item2, binderResult.Value.Item1, binderResult.Value.Item2);
    }

    public static async Task<Result<(TSource1, TSource2, TTarget1, TTarget2)>> BindAllAppendAsync<TSource1, TSource2, TTarget1, TTarget2>(
        this Task<Result<(TSource1, TSource2)>> result,
        Result<(TTarget1, TTarget2)> binder)
        => (await result.ConfigureAwait(false)).BindAllAppend(binder);

    public static async Task<Result<(TSource1, TSource2, TTarget1, TTarget2)>> BindAllAppendAsync<TSource1, TSource2, TTarget1, TTarget2>(
        this Task<Result<(TSource1, TSource2)>> result,
        Task<Result<(TTarget1, TTarget2)>> binder)
        => await (await result.ConfigureAwait(false)).BindAllAppendAsync(binder).ConfigureAwait(false);
        
    public static async Task<Result<(TSource1, TSource2, TSource3, TTarget)>> BindAllAppendAsync<TSource1, TSource2, TSource3, TTarget>(
        this Result<(TSource1, TSource2, TSource3)> result,
        Task<Result<TTarget>> binder)
    {
        var binderResult = await binder.ConfigureAwait(false);
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binderResult)
            .TryBuild(out Error error)
                ? error
                : (result.Value.Item1, result.Value.Item2, result.Value.Item3, binderResult.Value);
    }

    public static async Task<Result<(TSource1, TSource2, TSource3, TTarget)>> BindAllAppendAsync<TSource1, TSource2, TSource3, TTarget>(
        this Task<Result<(TSource1, TSource2, TSource3)>> result,
        Result<TTarget> binder)
        => (await result.ConfigureAwait(false)).BindAllAppend(binder);

    public static async Task<Result<(TSource1, TSource2, TSource3, TTarget)>> BindAllAppendAsync<TSource1, TSource2, TSource3, TTarget>(
        this Task<Result<(TSource1, TSource2, TSource3)>> result,
        Task<Result<TTarget>> binder)
        => await (await result.ConfigureAwait(false)).BindAllAppendAsync(binder).ConfigureAwait(false);
        
    public static async Task<Result<(TSource, TTarget1, TTarget2, TTarget3, TTarget4)>> BindAllAppendAsync<TSource, TTarget1, TTarget2, TTarget3, TTarget4>(
        this Result<TSource> result,
        Task<Result<(TTarget1, TTarget2, TTarget3, TTarget4)>> binder)
    {
        var binderResult = await binder.ConfigureAwait(false);
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binderResult)
            .TryBuild(out Error error)
                ? error
                : (result.Value, binderResult.Value.Item1, binderResult.Value.Item2, binderResult.Value.Item3, binderResult.Value.Item4);
    }

    public static async Task<Result<(TSource, TTarget1, TTarget2, TTarget3, TTarget4)>> BindAllAppendAsync<TSource, TTarget1, TTarget2, TTarget3, TTarget4>(
        this Task<Result<TSource>> result,
        Result<(TTarget1, TTarget2, TTarget3, TTarget4)> binder)
        => (await result.ConfigureAwait(false)).BindAllAppend(binder);

    public static async Task<Result<(TSource, TTarget1, TTarget2, TTarget3, TTarget4)>> BindAllAppendAsync<TSource, TTarget1, TTarget2, TTarget3, TTarget4>(
        this Task<Result<TSource>> result,
        Task<Result<(TTarget1, TTarget2, TTarget3, TTarget4)>> binder)
        => await (await result.ConfigureAwait(false)).BindAllAppendAsync(binder).ConfigureAwait(false);
        
    public static async Task<Result<(TSource1, TSource2, TTarget1, TTarget2, TTarget3)>> BindAllAppendAsync<TSource1, TSource2, TTarget1, TTarget2, TTarget3>(
        this Result<(TSource1, TSource2)> result,
        Task<Result<(TTarget1, TTarget2, TTarget3)>> binder)
    {
        var binderResult = await binder.ConfigureAwait(false);
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binderResult)
            .TryBuild(out Error error)
                ? error
                : (result.Value.Item1, result.Value.Item2, binderResult.Value.Item1, binderResult.Value.Item2, binderResult.Value.Item3);
    }

    public static async Task<Result<(TSource1, TSource2, TTarget1, TTarget2, TTarget3)>> BindAllAppendAsync<TSource1, TSource2, TTarget1, TTarget2, TTarget3>(
        this Task<Result<(TSource1, TSource2)>> result,
        Result<(TTarget1, TTarget2, TTarget3)> binder)
        => (await result.ConfigureAwait(false)).BindAllAppend(binder);

    public static async Task<Result<(TSource1, TSource2, TTarget1, TTarget2, TTarget3)>> BindAllAppendAsync<TSource1, TSource2, TTarget1, TTarget2, TTarget3>(
        this Task<Result<(TSource1, TSource2)>> result,
        Task<Result<(TTarget1, TTarget2, TTarget3)>> binder)
        => await (await result.ConfigureAwait(false)).BindAllAppendAsync(binder).ConfigureAwait(false);
        
    public static async Task<Result<(TSource1, TSource2, TSource3, TTarget1, TTarget2)>> BindAllAppendAsync<TSource1, TSource2, TSource3, TTarget1, TTarget2>(
        this Result<(TSource1, TSource2, TSource3)> result,
        Task<Result<(TTarget1, TTarget2)>> binder)
    {
        var binderResult = await binder.ConfigureAwait(false);
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binderResult)
            .TryBuild(out Error error)
                ? error
                : (result.Value.Item1, result.Value.Item2, result.Value.Item3, binderResult.Value.Item1, binderResult.Value.Item2);
    }

    public static async Task<Result<(TSource1, TSource2, TSource3, TTarget1, TTarget2)>> BindAllAppendAsync<TSource1, TSource2, TSource3, TTarget1, TTarget2>(
        this Task<Result<(TSource1, TSource2, TSource3)>> result,
        Result<(TTarget1, TTarget2)> binder)
        => (await result.ConfigureAwait(false)).BindAllAppend(binder);

    public static async Task<Result<(TSource1, TSource2, TSource3, TTarget1, TTarget2)>> BindAllAppendAsync<TSource1, TSource2, TSource3, TTarget1, TTarget2>(
        this Task<Result<(TSource1, TSource2, TSource3)>> result,
        Task<Result<(TTarget1, TTarget2)>> binder)
        => await (await result.ConfigureAwait(false)).BindAllAppendAsync(binder).ConfigureAwait(false);
        
    public static async Task<Result<(TSource1, TSource2, TSource3, TSource4, TTarget)>> BindAllAppendAsync<TSource1, TSource2, TSource3, TSource4, TTarget>(
        this Result<(TSource1, TSource2, TSource3, TSource4)> result,
        Task<Result<TTarget>> binder)
    {
        var binderResult = await binder.ConfigureAwait(false);
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binderResult)
            .TryBuild(out Error error)
                ? error
                : (result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, binderResult.Value);
    }

    public static async Task<Result<(TSource1, TSource2, TSource3, TSource4, TTarget)>> BindAllAppendAsync<TSource1, TSource2, TSource3, TSource4, TTarget>(
        this Task<Result<(TSource1, TSource2, TSource3, TSource4)>> result,
        Result<TTarget> binder)
        => (await result.ConfigureAwait(false)).BindAllAppend(binder);

    public static async Task<Result<(TSource1, TSource2, TSource3, TSource4, TTarget)>> BindAllAppendAsync<TSource1, TSource2, TSource3, TSource4, TTarget>(
        this Task<Result<(TSource1, TSource2, TSource3, TSource4)>> result,
        Task<Result<TTarget>> binder)
        => await (await result.ConfigureAwait(false)).BindAllAppendAsync(binder).ConfigureAwait(false);
        
    public static async Task<Result<(TSource, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5)>> BindAllAppendAsync<TSource, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5>(
        this Result<TSource> result,
        Task<Result<(TTarget1, TTarget2, TTarget3, TTarget4, TTarget5)>> binder)
    {
        var binderResult = await binder.ConfigureAwait(false);
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binderResult)
            .TryBuild(out Error error)
                ? error
                : (result.Value, binderResult.Value.Item1, binderResult.Value.Item2, binderResult.Value.Item3, binderResult.Value.Item4, binderResult.Value.Item5);
    }

    public static async Task<Result<(TSource, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5)>> BindAllAppendAsync<TSource, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5>(
        this Task<Result<TSource>> result,
        Result<(TTarget1, TTarget2, TTarget3, TTarget4, TTarget5)> binder)
        => (await result.ConfigureAwait(false)).BindAllAppend(binder);

    public static async Task<Result<(TSource, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5)>> BindAllAppendAsync<TSource, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5>(
        this Task<Result<TSource>> result,
        Task<Result<(TTarget1, TTarget2, TTarget3, TTarget4, TTarget5)>> binder)
        => await (await result.ConfigureAwait(false)).BindAllAppendAsync(binder).ConfigureAwait(false);
        
    public static async Task<Result<(TSource1, TSource2, TTarget1, TTarget2, TTarget3, TTarget4)>> BindAllAppendAsync<TSource1, TSource2, TTarget1, TTarget2, TTarget3, TTarget4>(
        this Result<(TSource1, TSource2)> result,
        Task<Result<(TTarget1, TTarget2, TTarget3, TTarget4)>> binder)
    {
        var binderResult = await binder.ConfigureAwait(false);
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binderResult)
            .TryBuild(out Error error)
                ? error
                : (result.Value.Item1, result.Value.Item2, binderResult.Value.Item1, binderResult.Value.Item2, binderResult.Value.Item3, binderResult.Value.Item4);
    }

    public static async Task<Result<(TSource1, TSource2, TTarget1, TTarget2, TTarget3, TTarget4)>> BindAllAppendAsync<TSource1, TSource2, TTarget1, TTarget2, TTarget3, TTarget4>(
        this Task<Result<(TSource1, TSource2)>> result,
        Result<(TTarget1, TTarget2, TTarget3, TTarget4)> binder)
        => (await result.ConfigureAwait(false)).BindAllAppend(binder);

    public static async Task<Result<(TSource1, TSource2, TTarget1, TTarget2, TTarget3, TTarget4)>> BindAllAppendAsync<TSource1, TSource2, TTarget1, TTarget2, TTarget3, TTarget4>(
        this Task<Result<(TSource1, TSource2)>> result,
        Task<Result<(TTarget1, TTarget2, TTarget3, TTarget4)>> binder)
        => await (await result.ConfigureAwait(false)).BindAllAppendAsync(binder).ConfigureAwait(false);
        
    public static async Task<Result<(TSource1, TSource2, TSource3, TTarget1, TTarget2, TTarget3)>> BindAllAppendAsync<TSource1, TSource2, TSource3, TTarget1, TTarget2, TTarget3>(
        this Result<(TSource1, TSource2, TSource3)> result,
        Task<Result<(TTarget1, TTarget2, TTarget3)>> binder)
    {
        var binderResult = await binder.ConfigureAwait(false);
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binderResult)
            .TryBuild(out Error error)
                ? error
                : (result.Value.Item1, result.Value.Item2, result.Value.Item3, binderResult.Value.Item1, binderResult.Value.Item2, binderResult.Value.Item3);
    }

    public static async Task<Result<(TSource1, TSource2, TSource3, TTarget1, TTarget2, TTarget3)>> BindAllAppendAsync<TSource1, TSource2, TSource3, TTarget1, TTarget2, TTarget3>(
        this Task<Result<(TSource1, TSource2, TSource3)>> result,
        Result<(TTarget1, TTarget2, TTarget3)> binder)
        => (await result.ConfigureAwait(false)).BindAllAppend(binder);

    public static async Task<Result<(TSource1, TSource2, TSource3, TTarget1, TTarget2, TTarget3)>> BindAllAppendAsync<TSource1, TSource2, TSource3, TTarget1, TTarget2, TTarget3>(
        this Task<Result<(TSource1, TSource2, TSource3)>> result,
        Task<Result<(TTarget1, TTarget2, TTarget3)>> binder)
        => await (await result.ConfigureAwait(false)).BindAllAppendAsync(binder).ConfigureAwait(false);
        
    public static async Task<Result<(TSource1, TSource2, TSource3, TSource4, TTarget1, TTarget2)>> BindAllAppendAsync<TSource1, TSource2, TSource3, TSource4, TTarget1, TTarget2>(
        this Result<(TSource1, TSource2, TSource3, TSource4)> result,
        Task<Result<(TTarget1, TTarget2)>> binder)
    {
        var binderResult = await binder.ConfigureAwait(false);
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binderResult)
            .TryBuild(out Error error)
                ? error
                : (result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, binderResult.Value.Item1, binderResult.Value.Item2);
    }

    public static async Task<Result<(TSource1, TSource2, TSource3, TSource4, TTarget1, TTarget2)>> BindAllAppendAsync<TSource1, TSource2, TSource3, TSource4, TTarget1, TTarget2>(
        this Task<Result<(TSource1, TSource2, TSource3, TSource4)>> result,
        Result<(TTarget1, TTarget2)> binder)
        => (await result.ConfigureAwait(false)).BindAllAppend(binder);

    public static async Task<Result<(TSource1, TSource2, TSource3, TSource4, TTarget1, TTarget2)>> BindAllAppendAsync<TSource1, TSource2, TSource3, TSource4, TTarget1, TTarget2>(
        this Task<Result<(TSource1, TSource2, TSource3, TSource4)>> result,
        Task<Result<(TTarget1, TTarget2)>> binder)
        => await (await result.ConfigureAwait(false)).BindAllAppendAsync(binder).ConfigureAwait(false);
        
    public static async Task<Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TTarget)>> BindAllAppendAsync<TSource1, TSource2, TSource3, TSource4, TSource5, TTarget>(
        this Result<(TSource1, TSource2, TSource3, TSource4, TSource5)> result,
        Task<Result<TTarget>> binder)
    {
        var binderResult = await binder.ConfigureAwait(false);
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binderResult)
            .TryBuild(out Error error)
                ? error
                : (result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, binderResult.Value);
    }

    public static async Task<Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TTarget)>> BindAllAppendAsync<TSource1, TSource2, TSource3, TSource4, TSource5, TTarget>(
        this Task<Result<(TSource1, TSource2, TSource3, TSource4, TSource5)>> result,
        Result<TTarget> binder)
        => (await result.ConfigureAwait(false)).BindAllAppend(binder);

    public static async Task<Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TTarget)>> BindAllAppendAsync<TSource1, TSource2, TSource3, TSource4, TSource5, TTarget>(
        this Task<Result<(TSource1, TSource2, TSource3, TSource4, TSource5)>> result,
        Task<Result<TTarget>> binder)
        => await (await result.ConfigureAwait(false)).BindAllAppendAsync(binder).ConfigureAwait(false);
        
    public static async Task<Result<(TSource, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6)>> BindAllAppendAsync<TSource, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6>(
        this Result<TSource> result,
        Task<Result<(TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6)>> binder)
    {
        var binderResult = await binder.ConfigureAwait(false);
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binderResult)
            .TryBuild(out Error error)
                ? error
                : (result.Value, binderResult.Value.Item1, binderResult.Value.Item2, binderResult.Value.Item3, binderResult.Value.Item4, binderResult.Value.Item5, binderResult.Value.Item6);
    }

    public static async Task<Result<(TSource, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6)>> BindAllAppendAsync<TSource, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6>(
        this Task<Result<TSource>> result,
        Result<(TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6)> binder)
        => (await result.ConfigureAwait(false)).BindAllAppend(binder);

    public static async Task<Result<(TSource, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6)>> BindAllAppendAsync<TSource, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6>(
        this Task<Result<TSource>> result,
        Task<Result<(TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6)>> binder)
        => await (await result.ConfigureAwait(false)).BindAllAppendAsync(binder).ConfigureAwait(false);
        
    public static async Task<Result<(TSource1, TSource2, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5)>> BindAllAppendAsync<TSource1, TSource2, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5>(
        this Result<(TSource1, TSource2)> result,
        Task<Result<(TTarget1, TTarget2, TTarget3, TTarget4, TTarget5)>> binder)
    {
        var binderResult = await binder.ConfigureAwait(false);
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binderResult)
            .TryBuild(out Error error)
                ? error
                : (result.Value.Item1, result.Value.Item2, binderResult.Value.Item1, binderResult.Value.Item2, binderResult.Value.Item3, binderResult.Value.Item4, binderResult.Value.Item5);
    }

    public static async Task<Result<(TSource1, TSource2, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5)>> BindAllAppendAsync<TSource1, TSource2, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5>(
        this Task<Result<(TSource1, TSource2)>> result,
        Result<(TTarget1, TTarget2, TTarget3, TTarget4, TTarget5)> binder)
        => (await result.ConfigureAwait(false)).BindAllAppend(binder);

    public static async Task<Result<(TSource1, TSource2, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5)>> BindAllAppendAsync<TSource1, TSource2, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5>(
        this Task<Result<(TSource1, TSource2)>> result,
        Task<Result<(TTarget1, TTarget2, TTarget3, TTarget4, TTarget5)>> binder)
        => await (await result.ConfigureAwait(false)).BindAllAppendAsync(binder).ConfigureAwait(false);
        
    public static async Task<Result<(TSource1, TSource2, TSource3, TTarget1, TTarget2, TTarget3, TTarget4)>> BindAllAppendAsync<TSource1, TSource2, TSource3, TTarget1, TTarget2, TTarget3, TTarget4>(
        this Result<(TSource1, TSource2, TSource3)> result,
        Task<Result<(TTarget1, TTarget2, TTarget3, TTarget4)>> binder)
    {
        var binderResult = await binder.ConfigureAwait(false);
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binderResult)
            .TryBuild(out Error error)
                ? error
                : (result.Value.Item1, result.Value.Item2, result.Value.Item3, binderResult.Value.Item1, binderResult.Value.Item2, binderResult.Value.Item3, binderResult.Value.Item4);
    }

    public static async Task<Result<(TSource1, TSource2, TSource3, TTarget1, TTarget2, TTarget3, TTarget4)>> BindAllAppendAsync<TSource1, TSource2, TSource3, TTarget1, TTarget2, TTarget3, TTarget4>(
        this Task<Result<(TSource1, TSource2, TSource3)>> result,
        Result<(TTarget1, TTarget2, TTarget3, TTarget4)> binder)
        => (await result.ConfigureAwait(false)).BindAllAppend(binder);

    public static async Task<Result<(TSource1, TSource2, TSource3, TTarget1, TTarget2, TTarget3, TTarget4)>> BindAllAppendAsync<TSource1, TSource2, TSource3, TTarget1, TTarget2, TTarget3, TTarget4>(
        this Task<Result<(TSource1, TSource2, TSource3)>> result,
        Task<Result<(TTarget1, TTarget2, TTarget3, TTarget4)>> binder)
        => await (await result.ConfigureAwait(false)).BindAllAppendAsync(binder).ConfigureAwait(false);
        
    public static async Task<Result<(TSource1, TSource2, TSource3, TSource4, TTarget1, TTarget2, TTarget3)>> BindAllAppendAsync<TSource1, TSource2, TSource3, TSource4, TTarget1, TTarget2, TTarget3>(
        this Result<(TSource1, TSource2, TSource3, TSource4)> result,
        Task<Result<(TTarget1, TTarget2, TTarget3)>> binder)
    {
        var binderResult = await binder.ConfigureAwait(false);
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binderResult)
            .TryBuild(out Error error)
                ? error
                : (result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, binderResult.Value.Item1, binderResult.Value.Item2, binderResult.Value.Item3);
    }

    public static async Task<Result<(TSource1, TSource2, TSource3, TSource4, TTarget1, TTarget2, TTarget3)>> BindAllAppendAsync<TSource1, TSource2, TSource3, TSource4, TTarget1, TTarget2, TTarget3>(
        this Task<Result<(TSource1, TSource2, TSource3, TSource4)>> result,
        Result<(TTarget1, TTarget2, TTarget3)> binder)
        => (await result.ConfigureAwait(false)).BindAllAppend(binder);

    public static async Task<Result<(TSource1, TSource2, TSource3, TSource4, TTarget1, TTarget2, TTarget3)>> BindAllAppendAsync<TSource1, TSource2, TSource3, TSource4, TTarget1, TTarget2, TTarget3>(
        this Task<Result<(TSource1, TSource2, TSource3, TSource4)>> result,
        Task<Result<(TTarget1, TTarget2, TTarget3)>> binder)
        => await (await result.ConfigureAwait(false)).BindAllAppendAsync(binder).ConfigureAwait(false);
        
    public static async Task<Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TTarget1, TTarget2)>> BindAllAppendAsync<TSource1, TSource2, TSource3, TSource4, TSource5, TTarget1, TTarget2>(
        this Result<(TSource1, TSource2, TSource3, TSource4, TSource5)> result,
        Task<Result<(TTarget1, TTarget2)>> binder)
    {
        var binderResult = await binder.ConfigureAwait(false);
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binderResult)
            .TryBuild(out Error error)
                ? error
                : (result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, binderResult.Value.Item1, binderResult.Value.Item2);
    }

    public static async Task<Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TTarget1, TTarget2)>> BindAllAppendAsync<TSource1, TSource2, TSource3, TSource4, TSource5, TTarget1, TTarget2>(
        this Task<Result<(TSource1, TSource2, TSource3, TSource4, TSource5)>> result,
        Result<(TTarget1, TTarget2)> binder)
        => (await result.ConfigureAwait(false)).BindAllAppend(binder);

    public static async Task<Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TTarget1, TTarget2)>> BindAllAppendAsync<TSource1, TSource2, TSource3, TSource4, TSource5, TTarget1, TTarget2>(
        this Task<Result<(TSource1, TSource2, TSource3, TSource4, TSource5)>> result,
        Task<Result<(TTarget1, TTarget2)>> binder)
        => await (await result.ConfigureAwait(false)).BindAllAppendAsync(binder).ConfigureAwait(false);
        
    public static async Task<Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TTarget)>> BindAllAppendAsync<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TTarget>(
        this Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6)> result,
        Task<Result<TTarget>> binder)
    {
        var binderResult = await binder.ConfigureAwait(false);
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binderResult)
            .TryBuild(out Error error)
                ? error
                : (result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, result.Value.Item6, binderResult.Value);
    }

    public static async Task<Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TTarget)>> BindAllAppendAsync<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TTarget>(
        this Task<Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6)>> result,
        Result<TTarget> binder)
        => (await result.ConfigureAwait(false)).BindAllAppend(binder);

    public static async Task<Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TTarget)>> BindAllAppendAsync<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TTarget>(
        this Task<Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6)>> result,
        Task<Result<TTarget>> binder)
        => await (await result.ConfigureAwait(false)).BindAllAppendAsync(binder).ConfigureAwait(false);
        
    public static async Task<Result<(TSource, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6, TTarget7)>> BindAllAppendAsync<TSource, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6, TTarget7>(
        this Result<TSource> result,
        Task<Result<(TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6, TTarget7)>> binder)
    {
        var binderResult = await binder.ConfigureAwait(false);
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binderResult)
            .TryBuild(out Error error)
                ? error
                : (result.Value, binderResult.Value.Item1, binderResult.Value.Item2, binderResult.Value.Item3, binderResult.Value.Item4, binderResult.Value.Item5, binderResult.Value.Item6, binderResult.Value.Item7);
    }

    public static async Task<Result<(TSource, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6, TTarget7)>> BindAllAppendAsync<TSource, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6, TTarget7>(
        this Task<Result<TSource>> result,
        Result<(TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6, TTarget7)> binder)
        => (await result.ConfigureAwait(false)).BindAllAppend(binder);

    public static async Task<Result<(TSource, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6, TTarget7)>> BindAllAppendAsync<TSource, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6, TTarget7>(
        this Task<Result<TSource>> result,
        Task<Result<(TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6, TTarget7)>> binder)
        => await (await result.ConfigureAwait(false)).BindAllAppendAsync(binder).ConfigureAwait(false);
        
    public static async Task<Result<(TSource1, TSource2, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6)>> BindAllAppendAsync<TSource1, TSource2, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6>(
        this Result<(TSource1, TSource2)> result,
        Task<Result<(TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6)>> binder)
    {
        var binderResult = await binder.ConfigureAwait(false);
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binderResult)
            .TryBuild(out Error error)
                ? error
                : (result.Value.Item1, result.Value.Item2, binderResult.Value.Item1, binderResult.Value.Item2, binderResult.Value.Item3, binderResult.Value.Item4, binderResult.Value.Item5, binderResult.Value.Item6);
    }

    public static async Task<Result<(TSource1, TSource2, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6)>> BindAllAppendAsync<TSource1, TSource2, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6>(
        this Task<Result<(TSource1, TSource2)>> result,
        Result<(TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6)> binder)
        => (await result.ConfigureAwait(false)).BindAllAppend(binder);

    public static async Task<Result<(TSource1, TSource2, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6)>> BindAllAppendAsync<TSource1, TSource2, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6>(
        this Task<Result<(TSource1, TSource2)>> result,
        Task<Result<(TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6)>> binder)
        => await (await result.ConfigureAwait(false)).BindAllAppendAsync(binder).ConfigureAwait(false);
        
    public static async Task<Result<(TSource1, TSource2, TSource3, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5)>> BindAllAppendAsync<TSource1, TSource2, TSource3, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5>(
        this Result<(TSource1, TSource2, TSource3)> result,
        Task<Result<(TTarget1, TTarget2, TTarget3, TTarget4, TTarget5)>> binder)
    {
        var binderResult = await binder.ConfigureAwait(false);
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binderResult)
            .TryBuild(out Error error)
                ? error
                : (result.Value.Item1, result.Value.Item2, result.Value.Item3, binderResult.Value.Item1, binderResult.Value.Item2, binderResult.Value.Item3, binderResult.Value.Item4, binderResult.Value.Item5);
    }

    public static async Task<Result<(TSource1, TSource2, TSource3, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5)>> BindAllAppendAsync<TSource1, TSource2, TSource3, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5>(
        this Task<Result<(TSource1, TSource2, TSource3)>> result,
        Result<(TTarget1, TTarget2, TTarget3, TTarget4, TTarget5)> binder)
        => (await result.ConfigureAwait(false)).BindAllAppend(binder);

    public static async Task<Result<(TSource1, TSource2, TSource3, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5)>> BindAllAppendAsync<TSource1, TSource2, TSource3, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5>(
        this Task<Result<(TSource1, TSource2, TSource3)>> result,
        Task<Result<(TTarget1, TTarget2, TTarget3, TTarget4, TTarget5)>> binder)
        => await (await result.ConfigureAwait(false)).BindAllAppendAsync(binder).ConfigureAwait(false);
        
    public static async Task<Result<(TSource1, TSource2, TSource3, TSource4, TTarget1, TTarget2, TTarget3, TTarget4)>> BindAllAppendAsync<TSource1, TSource2, TSource3, TSource4, TTarget1, TTarget2, TTarget3, TTarget4>(
        this Result<(TSource1, TSource2, TSource3, TSource4)> result,
        Task<Result<(TTarget1, TTarget2, TTarget3, TTarget4)>> binder)
    {
        var binderResult = await binder.ConfigureAwait(false);
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binderResult)
            .TryBuild(out Error error)
                ? error
                : (result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, binderResult.Value.Item1, binderResult.Value.Item2, binderResult.Value.Item3, binderResult.Value.Item4);
    }

    public static async Task<Result<(TSource1, TSource2, TSource3, TSource4, TTarget1, TTarget2, TTarget3, TTarget4)>> BindAllAppendAsync<TSource1, TSource2, TSource3, TSource4, TTarget1, TTarget2, TTarget3, TTarget4>(
        this Task<Result<(TSource1, TSource2, TSource3, TSource4)>> result,
        Result<(TTarget1, TTarget2, TTarget3, TTarget4)> binder)
        => (await result.ConfigureAwait(false)).BindAllAppend(binder);

    public static async Task<Result<(TSource1, TSource2, TSource3, TSource4, TTarget1, TTarget2, TTarget3, TTarget4)>> BindAllAppendAsync<TSource1, TSource2, TSource3, TSource4, TTarget1, TTarget2, TTarget3, TTarget4>(
        this Task<Result<(TSource1, TSource2, TSource3, TSource4)>> result,
        Task<Result<(TTarget1, TTarget2, TTarget3, TTarget4)>> binder)
        => await (await result.ConfigureAwait(false)).BindAllAppendAsync(binder).ConfigureAwait(false);
        
    public static async Task<Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TTarget1, TTarget2, TTarget3)>> BindAllAppendAsync<TSource1, TSource2, TSource3, TSource4, TSource5, TTarget1, TTarget2, TTarget3>(
        this Result<(TSource1, TSource2, TSource3, TSource4, TSource5)> result,
        Task<Result<(TTarget1, TTarget2, TTarget3)>> binder)
    {
        var binderResult = await binder.ConfigureAwait(false);
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binderResult)
            .TryBuild(out Error error)
                ? error
                : (result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, binderResult.Value.Item1, binderResult.Value.Item2, binderResult.Value.Item3);
    }

    public static async Task<Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TTarget1, TTarget2, TTarget3)>> BindAllAppendAsync<TSource1, TSource2, TSource3, TSource4, TSource5, TTarget1, TTarget2, TTarget3>(
        this Task<Result<(TSource1, TSource2, TSource3, TSource4, TSource5)>> result,
        Result<(TTarget1, TTarget2, TTarget3)> binder)
        => (await result.ConfigureAwait(false)).BindAllAppend(binder);

    public static async Task<Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TTarget1, TTarget2, TTarget3)>> BindAllAppendAsync<TSource1, TSource2, TSource3, TSource4, TSource5, TTarget1, TTarget2, TTarget3>(
        this Task<Result<(TSource1, TSource2, TSource3, TSource4, TSource5)>> result,
        Task<Result<(TTarget1, TTarget2, TTarget3)>> binder)
        => await (await result.ConfigureAwait(false)).BindAllAppendAsync(binder).ConfigureAwait(false);
        
    public static async Task<Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TTarget1, TTarget2)>> BindAllAppendAsync<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TTarget1, TTarget2>(
        this Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6)> result,
        Task<Result<(TTarget1, TTarget2)>> binder)
    {
        var binderResult = await binder.ConfigureAwait(false);
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binderResult)
            .TryBuild(out Error error)
                ? error
                : (result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, result.Value.Item6, binderResult.Value.Item1, binderResult.Value.Item2);
    }

    public static async Task<Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TTarget1, TTarget2)>> BindAllAppendAsync<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TTarget1, TTarget2>(
        this Task<Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6)>> result,
        Result<(TTarget1, TTarget2)> binder)
        => (await result.ConfigureAwait(false)).BindAllAppend(binder);

    public static async Task<Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TTarget1, TTarget2)>> BindAllAppendAsync<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TTarget1, TTarget2>(
        this Task<Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6)>> result,
        Task<Result<(TTarget1, TTarget2)>> binder)
        => await (await result.ConfigureAwait(false)).BindAllAppendAsync(binder).ConfigureAwait(false);
        
    public static async Task<Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TTarget)>> BindAllAppendAsync<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TTarget>(
        this Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7)> result,
        Task<Result<TTarget>> binder)
    {
        var binderResult = await binder.ConfigureAwait(false);
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binderResult)
            .TryBuild(out Error error)
                ? error
                : (result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, result.Value.Item6, result.Value.Item7, binderResult.Value);
    }

    public static async Task<Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TTarget)>> BindAllAppendAsync<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TTarget>(
        this Task<Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7)>> result,
        Result<TTarget> binder)
        => (await result.ConfigureAwait(false)).BindAllAppend(binder);

    public static async Task<Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TTarget)>> BindAllAppendAsync<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TTarget>(
        this Task<Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7)>> result,
        Task<Result<TTarget>> binder)
        => await (await result.ConfigureAwait(false)).BindAllAppendAsync(binder).ConfigureAwait(false);
        
}
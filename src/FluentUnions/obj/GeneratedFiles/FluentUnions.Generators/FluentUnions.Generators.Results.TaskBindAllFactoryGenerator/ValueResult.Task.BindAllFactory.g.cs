namespace FluentUnions;

public readonly partial struct Result
{
    public static async Task<Result<(TValue1, TValue2)>> BindAllAsync<TValue1, TValue2>(
        Task<Result<TValue1>> result1,
        Task<Result<TValue2>> result2)
    {
        var r1 = await result1.ConfigureAwait(false);
        var r2 = await result2.ConfigureAwait(false);
        return new ErrorBuilder()
            .AppendOnFailure(r1)
            .AppendOnFailure(r2)
            .TryBuild(out Error error)
                ? error
                : (r1.Value, r2.Value);
    }

    public static async Task<Result<(TValue1, TValue2, TValue3)>> BindAllAsync<TValue1, TValue2, TValue3>(
        Task<Result<TValue1>> result1,
        Task<Result<TValue2>> result2,
        Task<Result<TValue3>> result3)
    {
        var r1 = await result1.ConfigureAwait(false);
        var r2 = await result2.ConfigureAwait(false);
        var r3 = await result3.ConfigureAwait(false);
        return new ErrorBuilder()
            .AppendOnFailure(r1)
            .AppendOnFailure(r2)
            .AppendOnFailure(r3)
            .TryBuild(out Error error)
                ? error
                : (r1.Value, r2.Value, r3.Value);
    }

    public static async Task<Result<(TValue1, TValue2, TValue3, TValue4)>> BindAllAsync<TValue1, TValue2, TValue3, TValue4>(
        Task<Result<TValue1>> result1,
        Task<Result<TValue2>> result2,
        Task<Result<TValue3>> result3,
        Task<Result<TValue4>> result4)
    {
        var r1 = await result1.ConfigureAwait(false);
        var r2 = await result2.ConfigureAwait(false);
        var r3 = await result3.ConfigureAwait(false);
        var r4 = await result4.ConfigureAwait(false);
        return new ErrorBuilder()
            .AppendOnFailure(r1)
            .AppendOnFailure(r2)
            .AppendOnFailure(r3)
            .AppendOnFailure(r4)
            .TryBuild(out Error error)
                ? error
                : (r1.Value, r2.Value, r3.Value, r4.Value);
    }

    public static async Task<Result<(TValue1, TValue2, TValue3, TValue4, TValue5)>> BindAllAsync<TValue1, TValue2, TValue3, TValue4, TValue5>(
        Task<Result<TValue1>> result1,
        Task<Result<TValue2>> result2,
        Task<Result<TValue3>> result3,
        Task<Result<TValue4>> result4,
        Task<Result<TValue5>> result5)
    {
        var r1 = await result1.ConfigureAwait(false);
        var r2 = await result2.ConfigureAwait(false);
        var r3 = await result3.ConfigureAwait(false);
        var r4 = await result4.ConfigureAwait(false);
        var r5 = await result5.ConfigureAwait(false);
        return new ErrorBuilder()
            .AppendOnFailure(r1)
            .AppendOnFailure(r2)
            .AppendOnFailure(r3)
            .AppendOnFailure(r4)
            .AppendOnFailure(r5)
            .TryBuild(out Error error)
                ? error
                : (r1.Value, r2.Value, r3.Value, r4.Value, r5.Value);
    }

    public static async Task<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)>> BindAllAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(
        Task<Result<TValue1>> result1,
        Task<Result<TValue2>> result2,
        Task<Result<TValue3>> result3,
        Task<Result<TValue4>> result4,
        Task<Result<TValue5>> result5,
        Task<Result<TValue6>> result6)
    {
        var r1 = await result1.ConfigureAwait(false);
        var r2 = await result2.ConfigureAwait(false);
        var r3 = await result3.ConfigureAwait(false);
        var r4 = await result4.ConfigureAwait(false);
        var r5 = await result5.ConfigureAwait(false);
        var r6 = await result6.ConfigureAwait(false);
        return new ErrorBuilder()
            .AppendOnFailure(r1)
            .AppendOnFailure(r2)
            .AppendOnFailure(r3)
            .AppendOnFailure(r4)
            .AppendOnFailure(r5)
            .AppendOnFailure(r6)
            .TryBuild(out Error error)
                ? error
                : (r1.Value, r2.Value, r3.Value, r4.Value, r5.Value, r6.Value);
    }

    public static async Task<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)>> BindAllAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(
        Task<Result<TValue1>> result1,
        Task<Result<TValue2>> result2,
        Task<Result<TValue3>> result3,
        Task<Result<TValue4>> result4,
        Task<Result<TValue5>> result5,
        Task<Result<TValue6>> result6,
        Task<Result<TValue7>> result7)
    {
        var r1 = await result1.ConfigureAwait(false);
        var r2 = await result2.ConfigureAwait(false);
        var r3 = await result3.ConfigureAwait(false);
        var r4 = await result4.ConfigureAwait(false);
        var r5 = await result5.ConfigureAwait(false);
        var r6 = await result6.ConfigureAwait(false);
        var r7 = await result7.ConfigureAwait(false);
        return new ErrorBuilder()
            .AppendOnFailure(r1)
            .AppendOnFailure(r2)
            .AppendOnFailure(r3)
            .AppendOnFailure(r4)
            .AppendOnFailure(r5)
            .AppendOnFailure(r6)
            .AppendOnFailure(r7)
            .TryBuild(out Error error)
                ? error
                : (r1.Value, r2.Value, r3.Value, r4.Value, r5.Value, r6.Value, r7.Value);
    }

    public static async Task<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)>> BindAllAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(
        Task<Result<TValue1>> result1,
        Task<Result<TValue2>> result2,
        Task<Result<TValue3>> result3,
        Task<Result<TValue4>> result4,
        Task<Result<TValue5>> result5,
        Task<Result<TValue6>> result6,
        Task<Result<TValue7>> result7,
        Task<Result<TValue8>> result8)
    {
        var r1 = await result1.ConfigureAwait(false);
        var r2 = await result2.ConfigureAwait(false);
        var r3 = await result3.ConfigureAwait(false);
        var r4 = await result4.ConfigureAwait(false);
        var r5 = await result5.ConfigureAwait(false);
        var r6 = await result6.ConfigureAwait(false);
        var r7 = await result7.ConfigureAwait(false);
        var r8 = await result8.ConfigureAwait(false);
        return new ErrorBuilder()
            .AppendOnFailure(r1)
            .AppendOnFailure(r2)
            .AppendOnFailure(r3)
            .AppendOnFailure(r4)
            .AppendOnFailure(r5)
            .AppendOnFailure(r6)
            .AppendOnFailure(r7)
            .AppendOnFailure(r8)
            .TryBuild(out Error error)
                ? error
                : (r1.Value, r2.Value, r3.Value, r4.Value, r5.Value, r6.Value, r7.Value, r8.Value);
    }

}
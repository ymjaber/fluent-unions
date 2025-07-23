namespace FluentUnions;

public readonly partial struct Result
{
    public static async ValueTask<Result<(TValue1, TValue2)>> BindAsync<TValue1, TValue2>(
        Func<ValueTask<Result<TValue1>>> result1,
        Func<ValueTask<Result<TValue2>>> result2)
    {
        Result<TValue1> r1 = await result1().ConfigureAwait(false);
        if(r1.IsFailure) return r1.Error;

        Result<TValue2> r2 = await result2().ConfigureAwait(false);
        if(r2.IsFailure) return r2.Error;

        return (r1.Value, r2.Value);
    }

    public static async ValueTask<Result<(TValue1, TValue2, TValue3)>> BindAsync<TValue1, TValue2, TValue3>(
        Func<ValueTask<Result<TValue1>>> result1,
        Func<ValueTask<Result<TValue2>>> result2,
        Func<ValueTask<Result<TValue3>>> result3)
    {
        Result<TValue1> r1 = await result1().ConfigureAwait(false);
        if(r1.IsFailure) return r1.Error;

        Result<TValue2> r2 = await result2().ConfigureAwait(false);
        if(r2.IsFailure) return r2.Error;

        Result<TValue3> r3 = await result3().ConfigureAwait(false);
        if(r3.IsFailure) return r3.Error;

        return (r1.Value, r2.Value, r3.Value);
    }

    public static async ValueTask<Result<(TValue1, TValue2, TValue3, TValue4)>> BindAsync<TValue1, TValue2, TValue3, TValue4>(
        Func<ValueTask<Result<TValue1>>> result1,
        Func<ValueTask<Result<TValue2>>> result2,
        Func<ValueTask<Result<TValue3>>> result3,
        Func<ValueTask<Result<TValue4>>> result4)
    {
        Result<TValue1> r1 = await result1().ConfigureAwait(false);
        if(r1.IsFailure) return r1.Error;

        Result<TValue2> r2 = await result2().ConfigureAwait(false);
        if(r2.IsFailure) return r2.Error;

        Result<TValue3> r3 = await result3().ConfigureAwait(false);
        if(r3.IsFailure) return r3.Error;

        Result<TValue4> r4 = await result4().ConfigureAwait(false);
        if(r4.IsFailure) return r4.Error;

        return (r1.Value, r2.Value, r3.Value, r4.Value);
    }

    public static async ValueTask<Result<(TValue1, TValue2, TValue3, TValue4, TValue5)>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5>(
        Func<ValueTask<Result<TValue1>>> result1,
        Func<ValueTask<Result<TValue2>>> result2,
        Func<ValueTask<Result<TValue3>>> result3,
        Func<ValueTask<Result<TValue4>>> result4,
        Func<ValueTask<Result<TValue5>>> result5)
    {
        Result<TValue1> r1 = await result1().ConfigureAwait(false);
        if(r1.IsFailure) return r1.Error;

        Result<TValue2> r2 = await result2().ConfigureAwait(false);
        if(r2.IsFailure) return r2.Error;

        Result<TValue3> r3 = await result3().ConfigureAwait(false);
        if(r3.IsFailure) return r3.Error;

        Result<TValue4> r4 = await result4().ConfigureAwait(false);
        if(r4.IsFailure) return r4.Error;

        Result<TValue5> r5 = await result5().ConfigureAwait(false);
        if(r5.IsFailure) return r5.Error;

        return (r1.Value, r2.Value, r3.Value, r4.Value, r5.Value);
    }

    public static async ValueTask<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(
        Func<ValueTask<Result<TValue1>>> result1,
        Func<ValueTask<Result<TValue2>>> result2,
        Func<ValueTask<Result<TValue3>>> result3,
        Func<ValueTask<Result<TValue4>>> result4,
        Func<ValueTask<Result<TValue5>>> result5,
        Func<ValueTask<Result<TValue6>>> result6)
    {
        Result<TValue1> r1 = await result1().ConfigureAwait(false);
        if(r1.IsFailure) return r1.Error;

        Result<TValue2> r2 = await result2().ConfigureAwait(false);
        if(r2.IsFailure) return r2.Error;

        Result<TValue3> r3 = await result3().ConfigureAwait(false);
        if(r3.IsFailure) return r3.Error;

        Result<TValue4> r4 = await result4().ConfigureAwait(false);
        if(r4.IsFailure) return r4.Error;

        Result<TValue5> r5 = await result5().ConfigureAwait(false);
        if(r5.IsFailure) return r5.Error;

        Result<TValue6> r6 = await result6().ConfigureAwait(false);
        if(r6.IsFailure) return r6.Error;

        return (r1.Value, r2.Value, r3.Value, r4.Value, r5.Value, r6.Value);
    }

    public static async ValueTask<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(
        Func<ValueTask<Result<TValue1>>> result1,
        Func<ValueTask<Result<TValue2>>> result2,
        Func<ValueTask<Result<TValue3>>> result3,
        Func<ValueTask<Result<TValue4>>> result4,
        Func<ValueTask<Result<TValue5>>> result5,
        Func<ValueTask<Result<TValue6>>> result6,
        Func<ValueTask<Result<TValue7>>> result7)
    {
        Result<TValue1> r1 = await result1().ConfigureAwait(false);
        if(r1.IsFailure) return r1.Error;

        Result<TValue2> r2 = await result2().ConfigureAwait(false);
        if(r2.IsFailure) return r2.Error;

        Result<TValue3> r3 = await result3().ConfigureAwait(false);
        if(r3.IsFailure) return r3.Error;

        Result<TValue4> r4 = await result4().ConfigureAwait(false);
        if(r4.IsFailure) return r4.Error;

        Result<TValue5> r5 = await result5().ConfigureAwait(false);
        if(r5.IsFailure) return r5.Error;

        Result<TValue6> r6 = await result6().ConfigureAwait(false);
        if(r6.IsFailure) return r6.Error;

        Result<TValue7> r7 = await result7().ConfigureAwait(false);
        if(r7.IsFailure) return r7.Error;

        return (r1.Value, r2.Value, r3.Value, r4.Value, r5.Value, r6.Value, r7.Value);
    }

    public static async ValueTask<Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(
        Func<ValueTask<Result<TValue1>>> result1,
        Func<ValueTask<Result<TValue2>>> result2,
        Func<ValueTask<Result<TValue3>>> result3,
        Func<ValueTask<Result<TValue4>>> result4,
        Func<ValueTask<Result<TValue5>>> result5,
        Func<ValueTask<Result<TValue6>>> result6,
        Func<ValueTask<Result<TValue7>>> result7,
        Func<ValueTask<Result<TValue8>>> result8)
    {
        Result<TValue1> r1 = await result1().ConfigureAwait(false);
        if(r1.IsFailure) return r1.Error;

        Result<TValue2> r2 = await result2().ConfigureAwait(false);
        if(r2.IsFailure) return r2.Error;

        Result<TValue3> r3 = await result3().ConfigureAwait(false);
        if(r3.IsFailure) return r3.Error;

        Result<TValue4> r4 = await result4().ConfigureAwait(false);
        if(r4.IsFailure) return r4.Error;

        Result<TValue5> r5 = await result5().ConfigureAwait(false);
        if(r5.IsFailure) return r5.Error;

        Result<TValue6> r6 = await result6().ConfigureAwait(false);
        if(r6.IsFailure) return r6.Error;

        Result<TValue7> r7 = await result7().ConfigureAwait(false);
        if(r7.IsFailure) return r7.Error;

        Result<TValue8> r8 = await result8().ConfigureAwait(false);
        if(r8.IsFailure) return r8.Error;

        return (r1.Value, r2.Value, r3.Value, r4.Value, r5.Value, r6.Value, r7.Value, r8.Value);
    }

}
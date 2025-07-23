namespace FluentUnions;

public readonly partial struct Result
{
    public static Result<(TValue1, TValue2)> BindAppend<TValue1, TValue2>(
        Func<Result<TValue1>> result1,
        Func<Result<TValue2>> result2)
    {
        Result<TValue1> r1 = result1();
        if(r1.IsFailure) return r1.Error;

        Result<TValue2> r2 = result2();
        if(r2.IsFailure) return r2.Error;

        return (r1.Value, r2.Value);
    }

    public static Result<(TValue1, TValue2, TValue3)> BindAppend<TValue1, TValue2, TValue3>(
        Func<Result<TValue1>> result1,
        Func<Result<TValue2>> result2,
        Func<Result<TValue3>> result3)
    {
        Result<TValue1> r1 = result1();
        if(r1.IsFailure) return r1.Error;

        Result<TValue2> r2 = result2();
        if(r2.IsFailure) return r2.Error;

        Result<TValue3> r3 = result3();
        if(r3.IsFailure) return r3.Error;

        return (r1.Value, r2.Value, r3.Value);
    }

    public static Result<(TValue1, TValue2, TValue3, TValue4)> BindAppend<TValue1, TValue2, TValue3, TValue4>(
        Func<Result<TValue1>> result1,
        Func<Result<TValue2>> result2,
        Func<Result<TValue3>> result3,
        Func<Result<TValue4>> result4)
    {
        Result<TValue1> r1 = result1();
        if(r1.IsFailure) return r1.Error;

        Result<TValue2> r2 = result2();
        if(r2.IsFailure) return r2.Error;

        Result<TValue3> r3 = result3();
        if(r3.IsFailure) return r3.Error;

        Result<TValue4> r4 = result4();
        if(r4.IsFailure) return r4.Error;

        return (r1.Value, r2.Value, r3.Value, r4.Value);
    }

    public static Result<(TValue1, TValue2, TValue3, TValue4, TValue5)> BindAppend<TValue1, TValue2, TValue3, TValue4, TValue5>(
        Func<Result<TValue1>> result1,
        Func<Result<TValue2>> result2,
        Func<Result<TValue3>> result3,
        Func<Result<TValue4>> result4,
        Func<Result<TValue5>> result5)
    {
        Result<TValue1> r1 = result1();
        if(r1.IsFailure) return r1.Error;

        Result<TValue2> r2 = result2();
        if(r2.IsFailure) return r2.Error;

        Result<TValue3> r3 = result3();
        if(r3.IsFailure) return r3.Error;

        Result<TValue4> r4 = result4();
        if(r4.IsFailure) return r4.Error;

        Result<TValue5> r5 = result5();
        if(r5.IsFailure) return r5.Error;

        return (r1.Value, r2.Value, r3.Value, r4.Value, r5.Value);
    }

    public static Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)> BindAppend<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(
        Func<Result<TValue1>> result1,
        Func<Result<TValue2>> result2,
        Func<Result<TValue3>> result3,
        Func<Result<TValue4>> result4,
        Func<Result<TValue5>> result5,
        Func<Result<TValue6>> result6)
    {
        Result<TValue1> r1 = result1();
        if(r1.IsFailure) return r1.Error;

        Result<TValue2> r2 = result2();
        if(r2.IsFailure) return r2.Error;

        Result<TValue3> r3 = result3();
        if(r3.IsFailure) return r3.Error;

        Result<TValue4> r4 = result4();
        if(r4.IsFailure) return r4.Error;

        Result<TValue5> r5 = result5();
        if(r5.IsFailure) return r5.Error;

        Result<TValue6> r6 = result6();
        if(r6.IsFailure) return r6.Error;

        return (r1.Value, r2.Value, r3.Value, r4.Value, r5.Value, r6.Value);
    }

    public static Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)> BindAppend<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(
        Func<Result<TValue1>> result1,
        Func<Result<TValue2>> result2,
        Func<Result<TValue3>> result3,
        Func<Result<TValue4>> result4,
        Func<Result<TValue5>> result5,
        Func<Result<TValue6>> result6,
        Func<Result<TValue7>> result7)
    {
        Result<TValue1> r1 = result1();
        if(r1.IsFailure) return r1.Error;

        Result<TValue2> r2 = result2();
        if(r2.IsFailure) return r2.Error;

        Result<TValue3> r3 = result3();
        if(r3.IsFailure) return r3.Error;

        Result<TValue4> r4 = result4();
        if(r4.IsFailure) return r4.Error;

        Result<TValue5> r5 = result5();
        if(r5.IsFailure) return r5.Error;

        Result<TValue6> r6 = result6();
        if(r6.IsFailure) return r6.Error;

        Result<TValue7> r7 = result7();
        if(r7.IsFailure) return r7.Error;

        return (r1.Value, r2.Value, r3.Value, r4.Value, r5.Value, r6.Value, r7.Value);
    }

    public static Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)> BindAppend<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(
        Func<Result<TValue1>> result1,
        Func<Result<TValue2>> result2,
        Func<Result<TValue3>> result3,
        Func<Result<TValue4>> result4,
        Func<Result<TValue5>> result5,
        Func<Result<TValue6>> result6,
        Func<Result<TValue7>> result7,
        Func<Result<TValue8>> result8)
    {
        Result<TValue1> r1 = result1();
        if(r1.IsFailure) return r1.Error;

        Result<TValue2> r2 = result2();
        if(r2.IsFailure) return r2.Error;

        Result<TValue3> r3 = result3();
        if(r3.IsFailure) return r3.Error;

        Result<TValue4> r4 = result4();
        if(r4.IsFailure) return r4.Error;

        Result<TValue5> r5 = result5();
        if(r5.IsFailure) return r5.Error;

        Result<TValue6> r6 = result6();
        if(r6.IsFailure) return r6.Error;

        Result<TValue7> r7 = result7();
        if(r7.IsFailure) return r7.Error;

        Result<TValue8> r8 = result8();
        if(r8.IsFailure) return r8.Error;

        return (r1.Value, r2.Value, r3.Value, r4.Value, r5.Value, r6.Value, r7.Value, r8.Value);
    }

}
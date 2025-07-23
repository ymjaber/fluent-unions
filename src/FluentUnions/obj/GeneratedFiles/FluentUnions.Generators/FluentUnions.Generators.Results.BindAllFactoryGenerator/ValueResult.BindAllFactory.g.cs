namespace FluentUnions;

public readonly partial struct Result
{
    public static Result<(TValue1, TValue2)> BindAllAppend<TValue1, TValue2>(
        in Result<TValue1> result1,
        in Result<TValue2> result2)
    {
        return new ErrorBuilder()
            .AppendOnFailure(result1)
            .AppendOnFailure(result2)
            .TryBuild(out Error error)
                ? error
                : (result1.Value, result2.Value);
    }

    public static Result<(TValue1, TValue2, TValue3)> BindAllAppend<TValue1, TValue2, TValue3>(
        in Result<TValue1> result1,
        in Result<TValue2> result2,
        in Result<TValue3> result3)
    {
        return new ErrorBuilder()
            .AppendOnFailure(result1)
            .AppendOnFailure(result2)
            .AppendOnFailure(result3)
            .TryBuild(out Error error)
                ? error
                : (result1.Value, result2.Value, result3.Value);
    }

    public static Result<(TValue1, TValue2, TValue3, TValue4)> BindAllAppend<TValue1, TValue2, TValue3, TValue4>(
        in Result<TValue1> result1,
        in Result<TValue2> result2,
        in Result<TValue3> result3,
        in Result<TValue4> result4)
    {
        return new ErrorBuilder()
            .AppendOnFailure(result1)
            .AppendOnFailure(result2)
            .AppendOnFailure(result3)
            .AppendOnFailure(result4)
            .TryBuild(out Error error)
                ? error
                : (result1.Value, result2.Value, result3.Value, result4.Value);
    }

    public static Result<(TValue1, TValue2, TValue3, TValue4, TValue5)> BindAllAppend<TValue1, TValue2, TValue3, TValue4, TValue5>(
        in Result<TValue1> result1,
        in Result<TValue2> result2,
        in Result<TValue3> result3,
        in Result<TValue4> result4,
        in Result<TValue5> result5)
    {
        return new ErrorBuilder()
            .AppendOnFailure(result1)
            .AppendOnFailure(result2)
            .AppendOnFailure(result3)
            .AppendOnFailure(result4)
            .AppendOnFailure(result5)
            .TryBuild(out Error error)
                ? error
                : (result1.Value, result2.Value, result3.Value, result4.Value, result5.Value);
    }

    public static Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)> BindAllAppend<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(
        in Result<TValue1> result1,
        in Result<TValue2> result2,
        in Result<TValue3> result3,
        in Result<TValue4> result4,
        in Result<TValue5> result5,
        in Result<TValue6> result6)
    {
        return new ErrorBuilder()
            .AppendOnFailure(result1)
            .AppendOnFailure(result2)
            .AppendOnFailure(result3)
            .AppendOnFailure(result4)
            .AppendOnFailure(result5)
            .AppendOnFailure(result6)
            .TryBuild(out Error error)
                ? error
                : (result1.Value, result2.Value, result3.Value, result4.Value, result5.Value, result6.Value);
    }

    public static Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)> BindAllAppend<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(
        in Result<TValue1> result1,
        in Result<TValue2> result2,
        in Result<TValue3> result3,
        in Result<TValue4> result4,
        in Result<TValue5> result5,
        in Result<TValue6> result6,
        in Result<TValue7> result7)
    {
        return new ErrorBuilder()
            .AppendOnFailure(result1)
            .AppendOnFailure(result2)
            .AppendOnFailure(result3)
            .AppendOnFailure(result4)
            .AppendOnFailure(result5)
            .AppendOnFailure(result6)
            .AppendOnFailure(result7)
            .TryBuild(out Error error)
                ? error
                : (result1.Value, result2.Value, result3.Value, result4.Value, result5.Value, result6.Value, result7.Value);
    }

    public static Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)> BindAllAppend<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(
        in Result<TValue1> result1,
        in Result<TValue2> result2,
        in Result<TValue3> result3,
        in Result<TValue4> result4,
        in Result<TValue5> result5,
        in Result<TValue6> result6,
        in Result<TValue7> result7,
        in Result<TValue8> result8)
    {
        return new ErrorBuilder()
            .AppendOnFailure(result1)
            .AppendOnFailure(result2)
            .AppendOnFailure(result3)
            .AppendOnFailure(result4)
            .AppendOnFailure(result5)
            .AppendOnFailure(result6)
            .AppendOnFailure(result7)
            .AppendOnFailure(result8)
            .TryBuild(out Error error)
                ? error
                : (result1.Value, result2.Value, result3.Value, result4.Value, result5.Value, result6.Value, result7.Value, result8.Value);
    }

}
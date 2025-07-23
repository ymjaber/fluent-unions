namespace FluentUnions;

public static partial class ValueResultExtensions
{
public static Result<(TValue1, TValue2)> OnSuccess<TValue1, TValue2>(in this Result<(TValue1, TValue2)> result, Action<TValue1, TValue2> action)
{
    if (result.IsSuccess) action(result.Value.Item1, result.Value.Item2);
    return result;
}

public static Result<(TValue1, TValue2)> OnEither<TValue1, TValue2>(in this Result<(TValue1, TValue2)> result, Action<TValue1, TValue2> success, Action<Error> failure)
{
    if (result.IsSuccess) success(result.Value.Item1, result.Value.Item2);
    else failure(result.Error);
    return result;
}public static Result<(TValue1, TValue2, TValue3)> OnSuccess<TValue1, TValue2, TValue3>(in this Result<(TValue1, TValue2, TValue3)> result, Action<TValue1, TValue2, TValue3> action)
{
    if (result.IsSuccess) action(result.Value.Item1, result.Value.Item2, result.Value.Item3);
    return result;
}

public static Result<(TValue1, TValue2, TValue3)> OnEither<TValue1, TValue2, TValue3>(in this Result<(TValue1, TValue2, TValue3)> result, Action<TValue1, TValue2, TValue3> success, Action<Error> failure)
{
    if (result.IsSuccess) success(result.Value.Item1, result.Value.Item2, result.Value.Item3);
    else failure(result.Error);
    return result;
}public static Result<(TValue1, TValue2, TValue3, TValue4)> OnSuccess<TValue1, TValue2, TValue3, TValue4>(in this Result<(TValue1, TValue2, TValue3, TValue4)> result, Action<TValue1, TValue2, TValue3, TValue4> action)
{
    if (result.IsSuccess) action(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4);
    return result;
}

public static Result<(TValue1, TValue2, TValue3, TValue4)> OnEither<TValue1, TValue2, TValue3, TValue4>(in this Result<(TValue1, TValue2, TValue3, TValue4)> result, Action<TValue1, TValue2, TValue3, TValue4> success, Action<Error> failure)
{
    if (result.IsSuccess) success(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4);
    else failure(result.Error);
    return result;
}public static Result<(TValue1, TValue2, TValue3, TValue4, TValue5)> OnSuccess<TValue1, TValue2, TValue3, TValue4, TValue5>(in this Result<(TValue1, TValue2, TValue3, TValue4, TValue5)> result, Action<TValue1, TValue2, TValue3, TValue4, TValue5> action)
{
    if (result.IsSuccess) action(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5);
    return result;
}

public static Result<(TValue1, TValue2, TValue3, TValue4, TValue5)> OnEither<TValue1, TValue2, TValue3, TValue4, TValue5>(in this Result<(TValue1, TValue2, TValue3, TValue4, TValue5)> result, Action<TValue1, TValue2, TValue3, TValue4, TValue5> success, Action<Error> failure)
{
    if (result.IsSuccess) success(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5);
    else failure(result.Error);
    return result;
}public static Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)> OnSuccess<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(in this Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)> result, Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> action)
{
    if (result.IsSuccess) action(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, result.Value.Item6);
    return result;
}

public static Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)> OnEither<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(in this Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)> result, Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> success, Action<Error> failure)
{
    if (result.IsSuccess) success(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, result.Value.Item6);
    else failure(result.Error);
    return result;
}public static Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)> OnSuccess<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(in this Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)> result, Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> action)
{
    if (result.IsSuccess) action(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, result.Value.Item6, result.Value.Item7);
    return result;
}

public static Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)> OnEither<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(in this Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)> result, Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> success, Action<Error> failure)
{
    if (result.IsSuccess) success(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, result.Value.Item6, result.Value.Item7);
    else failure(result.Error);
    return result;
}public static Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)> OnSuccess<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(in this Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)> result, Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> action)
{
    if (result.IsSuccess) action(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, result.Value.Item6, result.Value.Item7, result.Value.Item8);
    return result;
}

public static Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)> OnEither<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(in this Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)> result, Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> success, Action<Error> failure)
{
    if (result.IsSuccess) success(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, result.Value.Item6, result.Value.Item7, result.Value.Item8);
    else failure(result.Error);
    return result;
}}
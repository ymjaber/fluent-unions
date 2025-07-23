namespace FluentUnions;

public static partial class ValueResultExtensions
{
    public static Result<(TValue1, TValue2)> Ensure<TValue1, TValue2>(
        in this Result<(TValue1, TValue2)> result,
        Func<TValue1, TValue2, bool> predicate,
        Error error)
    {
        if (result.IsFailure) return result;
    
        return predicate(result.Value.Item1, result.Value.Item2) ? result : error;
    }

    public static Result<(TValue1, TValue2, TValue3)> Ensure<TValue1, TValue2, TValue3>(
        in this Result<(TValue1, TValue2, TValue3)> result,
        Func<TValue1, TValue2, TValue3, bool> predicate,
        Error error)
    {
        if (result.IsFailure) return result;
    
        return predicate(result.Value.Item1, result.Value.Item2, result.Value.Item3) ? result : error;
    }

    public static Result<(TValue1, TValue2, TValue3, TValue4)> Ensure<TValue1, TValue2, TValue3, TValue4>(
        in this Result<(TValue1, TValue2, TValue3, TValue4)> result,
        Func<TValue1, TValue2, TValue3, TValue4, bool> predicate,
        Error error)
    {
        if (result.IsFailure) return result;
    
        return predicate(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4) ? result : error;
    }

    public static Result<(TValue1, TValue2, TValue3, TValue4, TValue5)> Ensure<TValue1, TValue2, TValue3, TValue4, TValue5>(
        in this Result<(TValue1, TValue2, TValue3, TValue4, TValue5)> result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, bool> predicate,
        Error error)
    {
        if (result.IsFailure) return result;
    
        return predicate(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5) ? result : error;
    }

    public static Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)> Ensure<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(
        in this Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)> result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, bool> predicate,
        Error error)
    {
        if (result.IsFailure) return result;
    
        return predicate(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, result.Value.Item6) ? result : error;
    }

    public static Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)> Ensure<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(
        in this Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)> result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, bool> predicate,
        Error error)
    {
        if (result.IsFailure) return result;
    
        return predicate(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, result.Value.Item6, result.Value.Item7) ? result : error;
    }

    public static Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)> Ensure<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(
        in this Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)> result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, bool> predicate,
        Error error)
    {
        if (result.IsFailure) return result;
    
        return predicate(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, result.Value.Item6, result.Value.Item7, result.Value.Item8) ? result : error;
    }

}
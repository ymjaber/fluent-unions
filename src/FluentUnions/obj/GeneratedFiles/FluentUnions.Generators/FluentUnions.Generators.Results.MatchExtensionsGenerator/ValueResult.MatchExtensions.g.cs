namespace FluentUnions;

public static partial class ValueResultExtensions
{
    public static TTarget Match<TSource1, TSource2, TTarget>(
        in this Result<(TSource1, TSource2)> result,
        Func<TSource1, TSource2, TTarget> success,
        Func<Error, TTarget> failure)
    {
        return result.IsSuccess ? success(result.Value.Item1, result.Value.Item2) : failure(result.Error);
    }

    public static TTarget Match<TSource1, TSource2, TSource3, TTarget>(
        in this Result<(TSource1, TSource2, TSource3)> result,
        Func<TSource1, TSource2, TSource3, TTarget> success,
        Func<Error, TTarget> failure)
    {
        return result.IsSuccess ? success(result.Value.Item1, result.Value.Item2, result.Value.Item3) : failure(result.Error);
    }

    public static TTarget Match<TSource1, TSource2, TSource3, TSource4, TTarget>(
        in this Result<(TSource1, TSource2, TSource3, TSource4)> result,
        Func<TSource1, TSource2, TSource3, TSource4, TTarget> success,
        Func<Error, TTarget> failure)
    {
        return result.IsSuccess ? success(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4) : failure(result.Error);
    }

    public static TTarget Match<TSource1, TSource2, TSource3, TSource4, TSource5, TTarget>(
        in this Result<(TSource1, TSource2, TSource3, TSource4, TSource5)> result,
        Func<TSource1, TSource2, TSource3, TSource4, TSource5, TTarget> success,
        Func<Error, TTarget> failure)
    {
        return result.IsSuccess ? success(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5) : failure(result.Error);
    }

    public static TTarget Match<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TTarget>(
        in this Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6)> result,
        Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TTarget> success,
        Func<Error, TTarget> failure)
    {
        return result.IsSuccess ? success(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, result.Value.Item6) : failure(result.Error);
    }

    public static TTarget Match<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TTarget>(
        in this Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7)> result,
        Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TTarget> success,
        Func<Error, TTarget> failure)
    {
        return result.IsSuccess ? success(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, result.Value.Item6, result.Value.Item7) : failure(result.Error);
    }

    public static TTarget Match<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TTarget>(
        in this Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8)> result,
        Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TTarget> success,
        Func<Error, TTarget> failure)
    {
        return result.IsSuccess ? success(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, result.Value.Item6, result.Value.Item7, result.Value.Item8) : failure(result.Error);
    }

}
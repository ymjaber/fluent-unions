namespace FluentUnions;

public static partial class ValueResultExtensions
{
    public static Result<(TSource, TTarget)> BindAllAppend<TSource, TTarget>(
        in this Result<TSource> result,
        in Result<TTarget> binder)
    {
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binder)
            .TryBuild(out Error error)
                ? error
                : (result.Value, binder.Value);
    }

    public static Result<(TSource, TTarget1, TTarget2)> BindAllAppend<TSource, TTarget1, TTarget2>(
        in this Result<TSource> result,
        in Result<(TTarget1, TTarget2)> binder)
    {
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binder)
            .TryBuild(out Error error)
                ? error
                : (result.Value, binder.Value.Item1, binder.Value.Item2);
    }

    public static Result<(TSource1, TSource2, TTarget)> BindAllAppend<TSource1, TSource2, TTarget>(
        in this Result<(TSource1, TSource2)> result,
        in Result<TTarget> binder)
    {
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binder)
            .TryBuild(out Error error)
                ? error
                : (result.Value.Item1, result.Value.Item2, binder.Value);
    }

    public static Result<(TSource, TTarget1, TTarget2, TTarget3)> BindAllAppend<TSource, TTarget1, TTarget2, TTarget3>(
        in this Result<TSource> result,
        in Result<(TTarget1, TTarget2, TTarget3)> binder)
    {
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binder)
            .TryBuild(out Error error)
                ? error
                : (result.Value, binder.Value.Item1, binder.Value.Item2, binder.Value.Item3);
    }

    public static Result<(TSource1, TSource2, TTarget1, TTarget2)> BindAllAppend<TSource1, TSource2, TTarget1, TTarget2>(
        in this Result<(TSource1, TSource2)> result,
        in Result<(TTarget1, TTarget2)> binder)
    {
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binder)
            .TryBuild(out Error error)
                ? error
                : (result.Value.Item1, result.Value.Item2, binder.Value.Item1, binder.Value.Item2);
    }

    public static Result<(TSource1, TSource2, TSource3, TTarget)> BindAllAppend<TSource1, TSource2, TSource3, TTarget>(
        in this Result<(TSource1, TSource2, TSource3)> result,
        in Result<TTarget> binder)
    {
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binder)
            .TryBuild(out Error error)
                ? error
                : (result.Value.Item1, result.Value.Item2, result.Value.Item3, binder.Value);
    }

    public static Result<(TSource, TTarget1, TTarget2, TTarget3, TTarget4)> BindAllAppend<TSource, TTarget1, TTarget2, TTarget3, TTarget4>(
        in this Result<TSource> result,
        in Result<(TTarget1, TTarget2, TTarget3, TTarget4)> binder)
    {
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binder)
            .TryBuild(out Error error)
                ? error
                : (result.Value, binder.Value.Item1, binder.Value.Item2, binder.Value.Item3, binder.Value.Item4);
    }

    public static Result<(TSource1, TSource2, TTarget1, TTarget2, TTarget3)> BindAllAppend<TSource1, TSource2, TTarget1, TTarget2, TTarget3>(
        in this Result<(TSource1, TSource2)> result,
        in Result<(TTarget1, TTarget2, TTarget3)> binder)
    {
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binder)
            .TryBuild(out Error error)
                ? error
                : (result.Value.Item1, result.Value.Item2, binder.Value.Item1, binder.Value.Item2, binder.Value.Item3);
    }

    public static Result<(TSource1, TSource2, TSource3, TTarget1, TTarget2)> BindAllAppend<TSource1, TSource2, TSource3, TTarget1, TTarget2>(
        in this Result<(TSource1, TSource2, TSource3)> result,
        in Result<(TTarget1, TTarget2)> binder)
    {
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binder)
            .TryBuild(out Error error)
                ? error
                : (result.Value.Item1, result.Value.Item2, result.Value.Item3, binder.Value.Item1, binder.Value.Item2);
    }

    public static Result<(TSource1, TSource2, TSource3, TSource4, TTarget)> BindAllAppend<TSource1, TSource2, TSource3, TSource4, TTarget>(
        in this Result<(TSource1, TSource2, TSource3, TSource4)> result,
        in Result<TTarget> binder)
    {
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binder)
            .TryBuild(out Error error)
                ? error
                : (result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, binder.Value);
    }

    public static Result<(TSource, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5)> BindAllAppend<TSource, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5>(
        in this Result<TSource> result,
        in Result<(TTarget1, TTarget2, TTarget3, TTarget4, TTarget5)> binder)
    {
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binder)
            .TryBuild(out Error error)
                ? error
                : (result.Value, binder.Value.Item1, binder.Value.Item2, binder.Value.Item3, binder.Value.Item4, binder.Value.Item5);
    }

    public static Result<(TSource1, TSource2, TTarget1, TTarget2, TTarget3, TTarget4)> BindAllAppend<TSource1, TSource2, TTarget1, TTarget2, TTarget3, TTarget4>(
        in this Result<(TSource1, TSource2)> result,
        in Result<(TTarget1, TTarget2, TTarget3, TTarget4)> binder)
    {
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binder)
            .TryBuild(out Error error)
                ? error
                : (result.Value.Item1, result.Value.Item2, binder.Value.Item1, binder.Value.Item2, binder.Value.Item3, binder.Value.Item4);
    }

    public static Result<(TSource1, TSource2, TSource3, TTarget1, TTarget2, TTarget3)> BindAllAppend<TSource1, TSource2, TSource3, TTarget1, TTarget2, TTarget3>(
        in this Result<(TSource1, TSource2, TSource3)> result,
        in Result<(TTarget1, TTarget2, TTarget3)> binder)
    {
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binder)
            .TryBuild(out Error error)
                ? error
                : (result.Value.Item1, result.Value.Item2, result.Value.Item3, binder.Value.Item1, binder.Value.Item2, binder.Value.Item3);
    }

    public static Result<(TSource1, TSource2, TSource3, TSource4, TTarget1, TTarget2)> BindAllAppend<TSource1, TSource2, TSource3, TSource4, TTarget1, TTarget2>(
        in this Result<(TSource1, TSource2, TSource3, TSource4)> result,
        in Result<(TTarget1, TTarget2)> binder)
    {
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binder)
            .TryBuild(out Error error)
                ? error
                : (result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, binder.Value.Item1, binder.Value.Item2);
    }

    public static Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TTarget)> BindAllAppend<TSource1, TSource2, TSource3, TSource4, TSource5, TTarget>(
        in this Result<(TSource1, TSource2, TSource3, TSource4, TSource5)> result,
        in Result<TTarget> binder)
    {
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binder)
            .TryBuild(out Error error)
                ? error
                : (result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, binder.Value);
    }

    public static Result<(TSource, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6)> BindAllAppend<TSource, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6>(
        in this Result<TSource> result,
        in Result<(TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6)> binder)
    {
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binder)
            .TryBuild(out Error error)
                ? error
                : (result.Value, binder.Value.Item1, binder.Value.Item2, binder.Value.Item3, binder.Value.Item4, binder.Value.Item5, binder.Value.Item6);
    }

    public static Result<(TSource1, TSource2, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5)> BindAllAppend<TSource1, TSource2, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5>(
        in this Result<(TSource1, TSource2)> result,
        in Result<(TTarget1, TTarget2, TTarget3, TTarget4, TTarget5)> binder)
    {
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binder)
            .TryBuild(out Error error)
                ? error
                : (result.Value.Item1, result.Value.Item2, binder.Value.Item1, binder.Value.Item2, binder.Value.Item3, binder.Value.Item4, binder.Value.Item5);
    }

    public static Result<(TSource1, TSource2, TSource3, TTarget1, TTarget2, TTarget3, TTarget4)> BindAllAppend<TSource1, TSource2, TSource3, TTarget1, TTarget2, TTarget3, TTarget4>(
        in this Result<(TSource1, TSource2, TSource3)> result,
        in Result<(TTarget1, TTarget2, TTarget3, TTarget4)> binder)
    {
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binder)
            .TryBuild(out Error error)
                ? error
                : (result.Value.Item1, result.Value.Item2, result.Value.Item3, binder.Value.Item1, binder.Value.Item2, binder.Value.Item3, binder.Value.Item4);
    }

    public static Result<(TSource1, TSource2, TSource3, TSource4, TTarget1, TTarget2, TTarget3)> BindAllAppend<TSource1, TSource2, TSource3, TSource4, TTarget1, TTarget2, TTarget3>(
        in this Result<(TSource1, TSource2, TSource3, TSource4)> result,
        in Result<(TTarget1, TTarget2, TTarget3)> binder)
    {
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binder)
            .TryBuild(out Error error)
                ? error
                : (result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, binder.Value.Item1, binder.Value.Item2, binder.Value.Item3);
    }

    public static Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TTarget1, TTarget2)> BindAllAppend<TSource1, TSource2, TSource3, TSource4, TSource5, TTarget1, TTarget2>(
        in this Result<(TSource1, TSource2, TSource3, TSource4, TSource5)> result,
        in Result<(TTarget1, TTarget2)> binder)
    {
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binder)
            .TryBuild(out Error error)
                ? error
                : (result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, binder.Value.Item1, binder.Value.Item2);
    }

    public static Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TTarget)> BindAllAppend<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TTarget>(
        in this Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6)> result,
        in Result<TTarget> binder)
    {
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binder)
            .TryBuild(out Error error)
                ? error
                : (result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, result.Value.Item6, binder.Value);
    }

    public static Result<(TSource, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6, TTarget7)> BindAllAppend<TSource, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6, TTarget7>(
        in this Result<TSource> result,
        in Result<(TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6, TTarget7)> binder)
    {
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binder)
            .TryBuild(out Error error)
                ? error
                : (result.Value, binder.Value.Item1, binder.Value.Item2, binder.Value.Item3, binder.Value.Item4, binder.Value.Item5, binder.Value.Item6, binder.Value.Item7);
    }

    public static Result<(TSource1, TSource2, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6)> BindAllAppend<TSource1, TSource2, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6>(
        in this Result<(TSource1, TSource2)> result,
        in Result<(TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6)> binder)
    {
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binder)
            .TryBuild(out Error error)
                ? error
                : (result.Value.Item1, result.Value.Item2, binder.Value.Item1, binder.Value.Item2, binder.Value.Item3, binder.Value.Item4, binder.Value.Item5, binder.Value.Item6);
    }

    public static Result<(TSource1, TSource2, TSource3, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5)> BindAllAppend<TSource1, TSource2, TSource3, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5>(
        in this Result<(TSource1, TSource2, TSource3)> result,
        in Result<(TTarget1, TTarget2, TTarget3, TTarget4, TTarget5)> binder)
    {
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binder)
            .TryBuild(out Error error)
                ? error
                : (result.Value.Item1, result.Value.Item2, result.Value.Item3, binder.Value.Item1, binder.Value.Item2, binder.Value.Item3, binder.Value.Item4, binder.Value.Item5);
    }

    public static Result<(TSource1, TSource2, TSource3, TSource4, TTarget1, TTarget2, TTarget3, TTarget4)> BindAllAppend<TSource1, TSource2, TSource3, TSource4, TTarget1, TTarget2, TTarget3, TTarget4>(
        in this Result<(TSource1, TSource2, TSource3, TSource4)> result,
        in Result<(TTarget1, TTarget2, TTarget3, TTarget4)> binder)
    {
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binder)
            .TryBuild(out Error error)
                ? error
                : (result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, binder.Value.Item1, binder.Value.Item2, binder.Value.Item3, binder.Value.Item4);
    }

    public static Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TTarget1, TTarget2, TTarget3)> BindAllAppend<TSource1, TSource2, TSource3, TSource4, TSource5, TTarget1, TTarget2, TTarget3>(
        in this Result<(TSource1, TSource2, TSource3, TSource4, TSource5)> result,
        in Result<(TTarget1, TTarget2, TTarget3)> binder)
    {
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binder)
            .TryBuild(out Error error)
                ? error
                : (result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, binder.Value.Item1, binder.Value.Item2, binder.Value.Item3);
    }

    public static Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TTarget1, TTarget2)> BindAllAppend<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TTarget1, TTarget2>(
        in this Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6)> result,
        in Result<(TTarget1, TTarget2)> binder)
    {
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binder)
            .TryBuild(out Error error)
                ? error
                : (result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, result.Value.Item6, binder.Value.Item1, binder.Value.Item2);
    }

    public static Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TTarget)> BindAllAppend<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TTarget>(
        in this Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7)> result,
        in Result<TTarget> binder)
    {
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binder)
            .TryBuild(out Error error)
                ? error
                : (result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, result.Value.Item6, result.Value.Item7, binder.Value);
    }

}
namespace FluentUnions;

/// <summary>Provides ValueTask-based asynchronous extension methods for <see cref="EnsureBuilder{T}"/> types.</summary>
public static partial class EnsureBuilderExtensions
{
    public static async ValueTask<Result<TValue>> BuildAsync<TValue>(this ValueTask<EnsureBuilder<TValue>> builder) =>
        (await builder.ConfigureAwait(false)).Build();

    public static async ValueTask<Result<TTarget>> MapAsync<TSource, TTarget>(
        this EnsureBuilder<TSource> builder, Func<TSource, ValueTask<TTarget>> mapper)
    {
        var result = builder.Build();

        if (result.IsFailure) return Result.Failure<TTarget>(result.Error);
        return Result.Success(await mapper(result.Value).ConfigureAwait(false));
    }

    public static async ValueTask<Result<TTarget>> MapAsync<TSource, TTarget>(
        this ValueTask<EnsureBuilder<TSource>> builder, Func<TSource, TTarget> mapper) =>
        (await builder.ConfigureAwait(false)).Map(mapper);

    public static async ValueTask<Result<TTarget>> MapAsync<TSource, TTarget>(
        this ValueTask<EnsureBuilder<TSource>> builder, Func<TSource, ValueTask<TTarget>> mapper) =>
        await (await builder.ConfigureAwait(false)).MapAsync(mapper).ConfigureAwait(false);

    public static ValueTask<Result<TTarget>> BindAsync<TSource, TTarget>(
        in this EnsureBuilder<TSource> builder,
        Func<TSource, ValueTask<Result<TTarget>>> binder)
    {
        var result = builder.Build();

        if (result.IsFailure) return ValueTask.FromResult(Result.Failure<TTarget>(result.Error));
        return binder(result.Value);
    }

    public static async ValueTask<Result<TTarget>> BindAsync<TSource, TTarget>(
        this ValueTask<EnsureBuilder<TSource>> builder,
        Func<TSource, Result<TTarget>> binder) =>
        (await builder.ConfigureAwait(false)).Bind(binder);

    public static async ValueTask<Result<TTarget>> BindAsync<TSource, TTarget>(
        this ValueTask<EnsureBuilder<TSource>> builder,
        Func<TSource, ValueTask<Result<TTarget>>> binder)
        => await (await builder.ConfigureAwait(false)).BindAsync(binder).ConfigureAwait(false);

    public static async ValueTask<Result<TTarget>> BindAllAsync<TSource, TTarget>(
        this EnsureBuilder<TSource> builder,
        ValueTask<Result<TTarget>> binder)
    {
        var result = builder.Build();

        var binderResult = await binder.ConfigureAwait(false);
        if (result.IsSuccess) return binderResult;

        return new ErrorBuilder()
            .Append(result.Error)
            .AppendOnFailure(binderResult)
            .Build();
    }

    public static async ValueTask<Result<TTarget>> BindAllAsync<TSource, TTarget>(
        this ValueTask<EnsureBuilder<TSource>> builder,
        Result<TTarget> binder) =>
        (await builder.ConfigureAwait(false)).BindAll(binder);

    public static async ValueTask<Result<TTarget>> BindAllAsync<TSource, TTarget>(
        this ValueTask<EnsureBuilder<TSource>> builder,
        ValueTask<Result<TTarget>> binder) =>
        await (await builder.ConfigureAwait(false)).BindAllAsync(binder).ConfigureAwait(false);
}
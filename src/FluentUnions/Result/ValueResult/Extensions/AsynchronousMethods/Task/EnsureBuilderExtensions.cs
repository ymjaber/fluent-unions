namespace FluentUnions;

/// <summary>Provides Task-based asynchronous extension methods for <see cref="EnsureBuilder{T}"/> types.</summary>
public static partial class EnsureBuilderExtensions
{
    public static async Task<Result<TValue>> BuildAsync<TValue>(this Task<EnsureBuilder<TValue>> builder) =>
        (await builder.ConfigureAwait(false)).Build();

    public static async Task<Result<TTarget>> MapAsync<TSource, TTarget>(
        this EnsureBuilder<TSource> builder, Func<TSource, Task<TTarget>> mapper)
    {
        var result = builder.Build();

        if (result.IsFailure) return Result.Failure<TTarget>(result.Error);
        return Result.Success(await mapper(result.Value).ConfigureAwait(false));
    }

    public static async Task<Result<TTarget>> MapAsync<TSource, TTarget>(
        this Task<EnsureBuilder<TSource>> builder, Func<TSource, TTarget> mapper) =>
        (await builder.ConfigureAwait(false)).Map(mapper);

    public static async Task<Result<TTarget>> MapAsync<TSource, TTarget>(
        this Task<EnsureBuilder<TSource>> builder, Func<TSource, Task<TTarget>> mapper) =>
        await (await builder.ConfigureAwait(false)).MapAsync(mapper).ConfigureAwait(false);

    public static Task<Result<TTarget>> BindAsync<TSource, TTarget>(
        in this EnsureBuilder<TSource> builder,
        Func<TSource, Task<Result<TTarget>>> binder)
    {
        var result = builder.Build();

        if (result.IsFailure) return Task.FromResult(Result.Failure<TTarget>(result.Error));
        return binder(result.Value);
    }

    public static async Task<Result<TTarget>> BindAsync<TSource, TTarget>(
        this Task<EnsureBuilder<TSource>> builder,
        Func<TSource, Result<TTarget>> binder) =>
        (await builder.ConfigureAwait(false)).Bind(binder);

    public static async Task<Result<TTarget>> BindAsync<TSource, TTarget>(
        this Task<EnsureBuilder<TSource>> builder,
        Func<TSource, Task<Result<TTarget>>> binder)
        => await (await builder.ConfigureAwait(false)).BindAsync(binder).ConfigureAwait(false);

    public static async Task<Result<TTarget>> BindAllAsync<TSource, TTarget>(
        this EnsureBuilder<TSource> builder,
        Task<Result<TTarget>> binder)
    {
        var result = builder.Build();

        var binderResult = await binder.ConfigureAwait(false);
        if (result.IsSuccess) return binderResult;

        return new ErrorBuilder()
            .Append(result.Error)
            .AppendOnFailure(binderResult)
            .Build();
    }

    public static async Task<Result<TTarget>> BindAllAsync<TSource, TTarget>(
        this Task<EnsureBuilder<TSource>> builder,
        Result<TTarget> binder) =>
        (await builder.ConfigureAwait(false)).BindAll(binder);

    public static async Task<Result<TTarget>> BindAllAsync<TSource, TTarget>(
        this Task<EnsureBuilder<TSource>> builder,
        Task<Result<TTarget>> binder) =>
        await (await builder.ConfigureAwait(false)).BindAllAsync(binder).ConfigureAwait(false);
}
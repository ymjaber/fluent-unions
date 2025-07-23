namespace FluentUnions;

/// <summary>Provides asynchronous filter builder extension methods using ValueTask-based operations.</summary>
public static partial class FilterBuilderExtensions
{
    public static async ValueTask<Option<TValue>> BuildAsync<TValue>(this ValueTask<FilterBuilder<TValue>> builder)
        where TValue : notnull
        => (await builder.ConfigureAwait(false)).Build();

    public static async ValueTask<Option<TTarget>> MapAsync<TSource, TTarget>(
        this FilterBuilder<TSource> builder,
        Func<TSource, ValueTask<TTarget>> mapper)
        where TSource : notnull
        where TTarget : notnull
    {
        var option = builder.Build();

        if (option.IsNone) return Option<TTarget>.None;
        return Option.Some(await mapper(option.Value).ConfigureAwait(false));
    }

    public static async ValueTask<Option<TTarget>> MapAsync<TSource, TTarget>(
        this ValueTask<FilterBuilder<TSource>> builder,
        Func<TSource, TTarget> mapper)
        where TSource : notnull
        where TTarget : notnull
        => (await builder.ConfigureAwait(false)).Map(mapper);

    public static async ValueTask<Option<TTarget>> MapAsync<TSource, TTarget>(
        this ValueTask<FilterBuilder<TSource>> builder,
        Func<TSource, ValueTask<TTarget>> mapper)
        where TSource : notnull
        where TTarget : notnull
        => await (await builder.ConfigureAwait(false)).MapAsync(mapper).ConfigureAwait(false);

    public static ValueTask<Option<TTarget>> BindAsync<TSource, TTarget>(
        in this FilterBuilder<TSource> builder,
        Func<TSource, ValueTask<Option<TTarget>>> binder)
        where TSource : notnull
        where TTarget : notnull
    {
        var option = builder.Build();

        if (option.IsNone) return ValueTask.FromResult(Option<TTarget>.None);
        return binder(option.Value);
    }

    public static async ValueTask<Option<TTarget>> BindAsync<TSource, TTarget>(
        this ValueTask<FilterBuilder<TSource>> builder,
        Func<TSource, Option<TTarget>> binder)
        where TSource : notnull
        where TTarget : notnull
        => (await builder.ConfigureAwait(false)).Bind(binder);

    public static async ValueTask<Option<TTarget>> BindAsync<TSource, TTarget>(
        this ValueTask<FilterBuilder<TSource>> builder,
        Func<TSource, ValueTask<Option<TTarget>>> binder)
        where TSource : notnull
        where TTarget : notnull
        => await (await builder.ConfigureAwait(false)).BindAsync(binder).ConfigureAwait(false);
}
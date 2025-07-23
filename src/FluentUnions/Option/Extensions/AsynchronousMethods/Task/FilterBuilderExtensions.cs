namespace FluentUnions;

/// <summary>Provides asynchronous filter builder extension methods using Task-based operations.</summary>
public static partial class FilterBuilderExtensions
{
    public static async Task<Option<TValue>> BuildAsync<TValue>(this Task<FilterBuilder<TValue>> builder)
        where TValue : notnull
        => (await builder.ConfigureAwait(false)).Build();

    public static async Task<Option<TTarget>> MapAsync<TSource, TTarget>(
        this FilterBuilder<TSource> builder,
        Func<TSource, Task<TTarget>> mapper)
        where TSource : notnull
        where TTarget : notnull
    {
        var option = builder.Build();

        if (option.IsNone) return Option<TTarget>.None;
        return Option.Some(await mapper(option.Value).ConfigureAwait(false));
    }

    public static async Task<Option<TTarget>> MapAsync<TSource, TTarget>(
        this Task<FilterBuilder<TSource>> builder,
        Func<TSource, TTarget> mapper)
        where TSource : notnull
        where TTarget : notnull
        => (await builder.ConfigureAwait(false)).Map(mapper);

    public static async Task<Option<TTarget>> MapAsync<TSource, TTarget>(
        this Task<FilterBuilder<TSource>> builder,
        Func<TSource, Task<TTarget>> mapper)
        where TSource : notnull
        where TTarget : notnull
        => await (await builder.ConfigureAwait(false)).MapAsync(mapper).ConfigureAwait(false);

    public static Task<Option<TTarget>> BindAsync<TSource, TTarget>(
        in this FilterBuilder<TSource> builder,
        Func<TSource, Task<Option<TTarget>>> binder)
        where TSource : notnull
        where TTarget : notnull
    {
        var option = builder.Build();

        if (option.IsNone) return Task.FromResult(Option<TTarget>.None);
        return binder(option.Value);
    }

    public static async Task<Option<TTarget>> BindAsync<TSource, TTarget>(
        this Task<FilterBuilder<TSource>> builder,
        Func<TSource, Option<TTarget>> binder)
        where TSource : notnull
        where TTarget : notnull
        => (await builder.ConfigureAwait(false)).Bind(binder);

    public static async Task<Option<TTarget>> BindAsync<TSource, TTarget>(
        this Task<FilterBuilder<TSource>> builder,
        Func<TSource, Task<Option<TTarget>>> binder)
        where TSource : notnull
        where TTarget : notnull
        => await (await builder.ConfigureAwait(false)).BindAsync(binder).ConfigureAwait(false);
}
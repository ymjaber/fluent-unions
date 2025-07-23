namespace FluentUnions;

/// <summary>Provides asynchronous monadic bind operations for <see cref="Option{T}"/> types using ValueTask-based operations.</summary>
public static partial class OptionExtensions
{
    /// <summary>
    /// Applies an asynchronous function that returns an <see cref="Option{TTarget}"/> to the value inside an <see cref="Option{TSource}"/>.
    /// </summary>
    /// <typeparam name="TSource">The type of the value contained in the source option.</typeparam>
    /// <typeparam name="TTarget">The type of the value in the resulting option.</typeparam>
    /// <param name="option">The option containing the value to transform.</param>
    /// <param name="binder">An asynchronous function that takes the value from the source option and returns a ValueTask&lt;Option&lt;TTarget&gt;&gt;.</param>
    /// <returns>
    /// A ValueTask containing the result of applying <paramref name="binder"/> to the value if the source option contains a value;
    /// otherwise, a ValueTask containing <see cref="Option{TTarget}.None"/>.
    /// </returns>
    /// <remarks>
    /// This is the ValueTask-based version of the monadic bind operation, providing better performance
    /// for async operations that may complete synchronously. If the source is None, the operation
    /// short-circuits without invoking the binder function.
    /// </remarks>
    public static ValueTask<Option<TTarget>> BindAsync<TSource, TTarget>(
        in this Option<TSource> option,
        Func<TSource, ValueTask<Option<TTarget>>> binder)
        where TSource : notnull
        where TTarget : notnull
    {
        if (option.IsNone) return ValueTask.FromResult(Option<TTarget>.None);
        return binder(option.Value);
    }

    /// <summary>
    /// Applies a synchronous bind function to the value of an asynchronous option.
    /// </summary>
    /// <typeparam name="TSource">The type of the value contained in the source option.</typeparam>
    /// <typeparam name="TTarget">The type of the value in the resulting option.</typeparam>
    /// <param name="option">The ValueTask containing the option to transform.</param>
    /// <param name="binder">A synchronous function that takes the value from the source option and returns an Option&lt;TTarget&gt;.</param>
    /// <returns>
    /// A ValueTask containing the result of applying <paramref name="binder"/> to the value if the source option contains a value;
    /// otherwise, a ValueTask containing <see cref="Option{TTarget}.None"/>.
    /// </returns>
    /// <remarks>
    /// This overload is useful when you have an async option but a synchronous transformation function.
    /// It awaits the option first, then applies the synchronous bind operation. Optimized for scenarios
    /// where the option might be available synchronously.
    /// </remarks>
    public static async ValueTask<Option<TTarget>> BindAsync<TSource, TTarget>(
        this ValueTask<Option<TSource>> option,
        Func<TSource, Option<TTarget>> binder)
        where TSource : notnull
        where TTarget : notnull
        => (await option.ConfigureAwait(false)).Bind(binder);

    /// <summary>
    /// Applies an asynchronous bind function to the value of an asynchronous option.
    /// </summary>
    /// <typeparam name="TSource">The type of the value contained in the source option.</typeparam>
    /// <typeparam name="TTarget">The type of the value in the resulting option.</typeparam>
    /// <param name="option">The ValueTask containing the option to transform.</param>
    /// <param name="binder">An asynchronous function that takes the value from the source option and returns a ValueTask&lt;Option&lt;TTarget&gt;&gt;.</param>
    /// <returns>
    /// A ValueTask containing the result of applying <paramref name="binder"/> to the value if the source option contains a value;
    /// otherwise, a ValueTask containing <see cref="Option{TTarget}.None"/>.
    /// </returns>
    /// <remarks>
    /// This overload handles fully asynchronous pipelines where both the option and the bind function use ValueTask.
    /// Provides optimal performance for chaining multiple async operations that may complete synchronously.
    /// </remarks>
    public static async ValueTask<Option<TTarget>> BindAsync<TSource, TTarget>(
        this ValueTask<Option<TSource>> option,
        Func<TSource, ValueTask<Option<TTarget>>> binder)
        where TSource : notnull
        where TTarget : notnull
        => await (await option.ConfigureAwait(false)).BindAsync(binder).ConfigureAwait(false);
}
namespace FluentUnions;

/// <summary>Provides asynchronous monadic bind operations for <see cref="Option{T}"/> types using Task-based operations.</summary>
public static partial class OptionExtensions
{
    /// <summary>
    /// Applies an asynchronous function that returns an <see cref="Option{TTarget}"/> to the value inside an <see cref="Option{TSource}"/>.
    /// </summary>
    /// <typeparam name="TSource">The type of the value contained in the source option.</typeparam>
    /// <typeparam name="TTarget">The type of the value in the resulting option.</typeparam>
    /// <param name="option">The option containing the value to transform.</param>
    /// <param name="binder">An asynchronous function that takes the value from the source option and returns a Task&lt;Option&lt;TTarget&gt;&gt;.</param>
    /// <returns>
    /// A task containing the result of applying <paramref name="binder"/> to the value if the source option contains a value;
    /// otherwise, a task containing <see cref="Option{TTarget}.None"/>.
    /// </returns>
    /// <remarks>
    /// This is the asynchronous version of the monadic bind operation. It allows chaining async operations
    /// that might fail, where each operation returns a Task&lt;Option&gt;. If the source is None, the operation
    /// short-circuits without invoking the binder function.
    /// </remarks>
    public static Task<Option<TTarget>> BindAsync<TSource, TTarget>(
        in this Option<TSource> option,
        Func<TSource, Task<Option<TTarget>>> binder)
        where TSource : notnull
        where TTarget : notnull
    {
        if (option.IsNone) return Task.FromResult(Option<TTarget>.None);
        return binder(option.Value);
    }

    /// <summary>
    /// Applies a synchronous bind function to the value of an asynchronous option.
    /// </summary>
    /// <typeparam name="TSource">The type of the value contained in the source option.</typeparam>
    /// <typeparam name="TTarget">The type of the value in the resulting option.</typeparam>
    /// <param name="option">The task containing the option to transform.</param>
    /// <param name="binder">A synchronous function that takes the value from the source option and returns an Option&lt;TTarget&gt;.</param>
    /// <returns>
    /// A task containing the result of applying <paramref name="binder"/> to the value if the source option contains a value;
    /// otherwise, a task containing <see cref="Option{TTarget}.None"/>.
    /// </returns>
    /// <remarks>
    /// This overload is useful when you have an async option but a synchronous transformation function.
    /// It awaits the option first, then applies the synchronous bind operation.
    /// </remarks>
    public static async Task<Option<TTarget>> BindAsync<TSource, TTarget>(
        this Task<Option<TSource>> option,
        Func<TSource, Option<TTarget>> binder)
        where TSource : notnull
        where TTarget : notnull
        => (await option.ConfigureAwait(false)).Bind(binder);

    /// <summary>
    /// Applies an asynchronous bind function to the value of an asynchronous option.
    /// </summary>
    /// <typeparam name="TSource">The type of the value contained in the source option.</typeparam>
    /// <typeparam name="TTarget">The type of the value in the resulting option.</typeparam>
    /// <param name="option">The task containing the option to transform.</param>
    /// <param name="binder">An asynchronous function that takes the value from the source option and returns a Task&lt;Option&lt;TTarget&gt;&gt;.</param>
    /// <returns>
    /// A task containing the result of applying <paramref name="binder"/> to the value if the source option contains a value;
    /// otherwise, a task containing <see cref="Option{TTarget}.None"/>.
    /// </returns>
    /// <remarks>
    /// This overload handles fully asynchronous pipelines where both the option and the bind function are async.
    /// It's ideal for chaining multiple async operations that each might return None.
    /// </remarks>
    public static async Task<Option<TTarget>> BindAsync<TSource, TTarget>(
        this Task<Option<TSource>> option,
        Func<TSource, Task<Option<TTarget>>> binder)
        where TSource : notnull
        where TTarget : notnull
        => await (await option.ConfigureAwait(false)).BindAsync(binder).ConfigureAwait(false);
}
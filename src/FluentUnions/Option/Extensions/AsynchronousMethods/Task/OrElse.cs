namespace FluentUnions;

/// <summary>Provides asynchronous default value methods for <see cref="Option{T}"/> types using Task-based operations.</summary>
public static partial class OptionExtensions
{
    /// <summary>
    /// Returns the task's option if it has a value, otherwise returns the specified fallback option.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="optionTask">The task containing the option.</param>
    /// <param name="fallback">The fallback option to return if the task's option is None.</param>
    /// <returns>The task's option if it has a value; otherwise, the fallback option.</returns>
    public static async Task<Option<TValue>> OrElseAsync<TValue>(
        this Task<Option<TValue>> optionTask,
        Option<TValue> fallback)
        where TValue : notnull
        => (await optionTask.ConfigureAwait(false)).OrElse(fallback);

    /// <summary>
    /// Returns this option if it has a value, otherwise awaits the specified async function and returns its result.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="option">The option.</param>
    /// <param name="fallbackFactory">An async function that returns the fallback option when this option is None.</param>
    /// <returns>This option if it has a value; otherwise, the result of awaiting the fallback factory.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="fallbackFactory"/> is null.</exception>
    public static async Task<Option<TValue>> OrElseAsync<TValue>(
        this Option<TValue> option,
        Func<Task<Option<TValue>>> fallbackFactory)
        where TValue : notnull
    {
        if (fallbackFactory is null) throw new ArgumentNullException(nameof(fallbackFactory));
        return option.IsSome ? option : await fallbackFactory().ConfigureAwait(false);
    }

    /// <summary>
    /// Returns the task's option if it has a value, otherwise invokes the specified function and returns its result.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="optionTask">The task containing the option.</param>
    /// <param name="fallbackFactory">A function that returns the fallback option when the task's option is None.</param>
    /// <returns>The task's option if it has a value; otherwise, the result of invoking the fallback factory.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="fallbackFactory"/> is null.</exception>
    public static async Task<Option<TValue>> OrElseAsync<TValue>(
        this Task<Option<TValue>> optionTask,
        Func<Option<TValue>> fallbackFactory)
        where TValue : notnull
        => (await optionTask.ConfigureAwait(false)).OrElse(fallbackFactory);

    /// <summary>
    /// Returns the task's option if it has a value, otherwise awaits the specified async function and returns its result.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="optionTask">The task containing the option.</param>
    /// <param name="fallbackFactory">An async function that returns the fallback option when the task's option is None.</param>
    /// <returns>The task's option if it has a value; otherwise, the result of awaiting the fallback factory.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="fallbackFactory"/> is null.</exception>
    public static async Task<Option<TValue>> OrElseAsync<TValue>(
        this Task<Option<TValue>> optionTask,
        Func<Task<Option<TValue>>> fallbackFactory)
        where TValue : notnull
        => await (await optionTask.ConfigureAwait(false)).OrElseAsync(fallbackFactory).ConfigureAwait(false);
}
namespace FluentUnions;

/// <summary>Provides asynchronous side effect methods for <see cref="Option{T}"/> types using Task-based operations.</summary>
public static partial class OptionExtensions
{
    /// <summary>
    /// Executes an asynchronous action if the option contains a value, then returns the original option.
    /// </summary>
    /// <typeparam name="TValue">The type of the value contained in the option.</typeparam>
    /// <param name="option">The option to check.</param>
    /// <param name="action">The asynchronous action to execute with the contained value if the option is Some.</param>
    /// <returns>A task that represents the asynchronous operation, containing the original option.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="action"/> is null.</exception>
    /// <remarks>
    /// This is the asynchronous version of OnSome, useful for performing async side effects
    /// like database updates or API calls based on the option's value.
    /// </remarks>
    public static async Task<Option<TValue>> OnSomeAsync<TValue>(
        this Option<TValue> option,
        Func<TValue, Task> action)
        where TValue : notnull
    {
        if (option.IsSome) await action(option.Value).ConfigureAwait(false);
        return option;
    }

    /// <summary>
    /// Executes a synchronous action on the value of an asynchronous option if it contains a value.
    /// </summary>
    /// <typeparam name="TValue">The type of the value contained in the option.</typeparam>
    /// <param name="option">The task containing the option to check.</param>
    /// <param name="action">The action to execute with the contained value if the option is Some.</param>
    /// <returns>A task that represents the asynchronous operation, containing the original option.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="action"/> is null.</exception>
    /// <remarks>
    /// This overload awaits the option task first, then executes a synchronous action on the value.
    /// Useful when the option is already wrapped in a Task from a previous async operation.
    /// </remarks>
    public static async Task<Option<TValue>> OnSomeAsync<TValue>(
        this Task<Option<TValue>> option,
        Action<TValue> action)
        where TValue : notnull
        => (await option.ConfigureAwait(false)).OnSome(action);

    /// <summary>
    /// Executes an asynchronous action on the value of an asynchronous option if it contains a value.
    /// </summary>
    /// <typeparam name="TValue">The type of the value contained in the option.</typeparam>
    /// <param name="option">The task containing the option to check.</param>
    /// <param name="action">The asynchronous action to execute with the contained value if the option is Some.</param>
    /// <returns>A task that represents the asynchronous operation, containing the original option.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="action"/> is null.</exception>
    /// <remarks>
    /// This overload handles fully asynchronous pipelines where both the option and the action are async.
    /// </remarks>
    public static async Task<Option<TValue>> OnSomeAsync<TValue>(
        this Task<Option<TValue>> option,
        Func<TValue, Task> action)
        where TValue : notnull
        => await (await option.ConfigureAwait(false)).OnSomeAsync(action);

    /// <summary>
    /// Executes an asynchronous action if the option is empty (None), then returns the original option.
    /// </summary>
    /// <typeparam name="TValue">The type of the value that would be contained in the option if it were Some.</typeparam>
    /// <param name="option">The option to check.</param>
    /// <param name="action">The asynchronous action to execute if the option is None.</param>
    /// <returns>A task that represents the asynchronous operation, containing the original option.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="action"/> is null.</exception>
    /// <remarks>
    /// This is the asynchronous version of OnNone, useful for handling the absence of a value
    /// with async operations like logging to a remote service or fetching default values.
    /// </remarks>
    public static async Task<Option<TValue>> OnNoneAsync<TValue>(
        this Option<TValue> option,
        Func<Task> action)
        where TValue : notnull
    {
        if (option.IsNone) await action().ConfigureAwait(false);
        return option;
    }

    /// <summary>
    /// Executes a synchronous action on an asynchronous option if it is empty (None).
    /// </summary>
    /// <typeparam name="TValue">The type of the value that would be contained in the option if it were Some.</typeparam>
    /// <param name="option">The task containing the option to check.</param>
    /// <param name="action">The action to execute if the option is None.</param>
    /// <returns>A task that represents the asynchronous operation, containing the original option.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="action"/> is null.</exception>
    /// <remarks>
    /// This overload awaits the option task first, then executes a synchronous action if None.
    /// </remarks>
    public static async Task<Option<TValue>> OnNoneAsync<TValue>(
        this Task<Option<TValue>> option,
        Action action)
        where TValue : notnull
        => (await option.ConfigureAwait(false)).OnNone(action);

    /// <summary>
    /// Executes an asynchronous action on an asynchronous option if it is empty (None).
    /// </summary>
    /// <typeparam name="TValue">The type of the value that would be contained in the option if it were Some.</typeparam>
    /// <param name="option">The task containing the option to check.</param>
    /// <param name="action">The asynchronous action to execute if the option is None.</param>
    /// <returns>A task that represents the asynchronous operation, containing the original option.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="action"/> is null.</exception>
    /// <remarks>
    /// This overload handles fully asynchronous pipelines where both the option and the action are async.
    /// </remarks>
    public static async Task<Option<TValue>> OnNoneAsync<TValue>(
        this Task<Option<TValue>> option,
        Func<Task> action)
        where TValue : notnull
        => await (await option.ConfigureAwait(false)).OnNoneAsync(action);

    /// <summary>
    /// Executes one of two asynchronous actions depending on whether the option contains a value or is empty.
    /// </summary>
    /// <typeparam name="TValue">The type of the value contained in the option.</typeparam>
    /// <param name="option">The option to check.</param>
    /// <param name="some">The asynchronous action to execute with the contained value if the option is Some.</param>
    /// <param name="none">The asynchronous action to execute if the option is None.</param>
    /// <returns>A task that represents the asynchronous operation, containing the original option.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="some"/> or <paramref name="none"/> is null.</exception>
    /// <remarks>
    /// This is the asynchronous version of OnEither, ensuring exactly one of the two actions is executed
    /// based on the option's state. Useful for async branching logic with side effects.
    /// </remarks>
    public static async Task<Option<TValue>> OnEitherAsync<TValue>(
        this Option<TValue> option,
        Func<TValue, Task> some,
        Func<Task> none)
        where TValue : notnull
    {
        if (option.IsSome) await some(option.Value).ConfigureAwait(false);
        else await none().ConfigureAwait(false);

        return option;
    }

    /// <summary>
    /// Executes one of two synchronous actions on an asynchronous option based on its state.
    /// </summary>
    /// <typeparam name="TValue">The type of the value contained in the option.</typeparam>
    /// <param name="option">The task containing the option to check.</param>
    /// <param name="some">The action to execute with the contained value if the option is Some.</param>
    /// <param name="none">The action to execute if the option is None.</param>
    /// <returns>A task that represents the asynchronous operation, containing the original option.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="some"/> or <paramref name="none"/> is null.</exception>
    /// <remarks>
    /// This overload awaits the option task first, then executes one of two synchronous actions.
    /// </remarks>
    public static async Task<Option<TValue>> OnEitherAsync<TValue>(
        this Task<Option<TValue>> option,
        Action<TValue> some,
        Action none)
        where TValue : notnull
        => (await option.ConfigureAwait(false)).OnEither(some, none);

    /// <summary>
    /// Executes one of two asynchronous actions on an asynchronous option based on its state.
    /// </summary>
    /// <typeparam name="TValue">The type of the value contained in the option.</typeparam>
    /// <param name="option">The task containing the option to check.</param>
    /// <param name="some">The asynchronous action to execute with the contained value if the option is Some.</param>
    /// <param name="none">The asynchronous action to execute if the option is None.</param>
    /// <returns>A task that represents the asynchronous operation, containing the original option.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="some"/> or <paramref name="none"/> is null.</exception>
    /// <remarks>
    /// This overload handles fully asynchronous pipelines where the option and both actions are async.
    /// </remarks>
    public static async Task<Option<TValue>> OnEitherAsync<TValue>(
        this Task<Option<TValue>> option,
        Func<TValue, Task> some,
        Func<Task> none)
        where TValue : notnull
        => await (await option.ConfigureAwait(false)).OnEitherAsync(some, none);
}
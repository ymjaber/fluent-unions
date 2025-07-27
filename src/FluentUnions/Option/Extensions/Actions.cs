namespace FluentUnions;

/// <summary>
/// Provides side effect methods for <see cref="Option{T}"/> types that allow executing actions based on the option's state.
/// </summary>
public static partial class OptionExtensions
{
    /// <summary>
    /// Executes the specified action if the option contains a value, then returns the original option.
    /// </summary>
    /// <typeparam name="TValue">The type of the value contained in the option.</typeparam>
    /// <param name="option">The option to check.</param>
    /// <param name="action">The action to execute with the contained value if the option is Some.</param>
    /// <returns>The original option, allowing for method chaining.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="action"/> is null.</exception>
    /// <remarks>
    /// This method is useful for performing side effects (like logging or updating external state)
    /// based on the value in the option without transforming the option itself.
    /// The option is returned unchanged to allow for fluent chaining of operations.
    /// </remarks>
    public static Option<TValue> OnSome<TValue>(
        in this Option<TValue> option,
        Action<TValue> action)
        where TValue : notnull
    {
        if (option.IsSome) action(option.Value);
        return option;
    }
    
    /// <summary>
    /// Executes the specified action if the option is empty (None), then returns the original option.
    /// </summary>
    /// <typeparam name="TValue">The type of the value that would be contained in the option if it were Some.</typeparam>
    /// <param name="option">The option to check.</param>
    /// <param name="action">The action to execute if the option is None.</param>
    /// <returns>The original option, allowing for method chaining.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="action"/> is null.</exception>
    /// <remarks>
    /// This method is useful for handling the absence of a value, such as logging warnings
    /// or providing default behavior when an expected value is not present.
    /// The option is returned unchanged to allow for fluent chaining of operations.
    /// </remarks>
    public static Option<TValue> OnNone<TValue>(
        in this Option<TValue> option,
        Action action)
        where TValue : notnull
    {
        if (option.IsNone) action();
        return option;
    }

    /// <summary>
    /// Executes one of two actions depending on whether the option contains a value or is empty, then returns the original option.
    /// </summary>
    /// <typeparam name="TValue">The type of the value contained in the option.</typeparam>
    /// <param name="option">The option to check.</param>
    /// <param name="some">The action to execute with the contained value if the option is Some.</param>
    /// <param name="none">The action to execute if the option is None.</param>
    /// <returns>The original option, allowing for method chaining.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="some"/> or <paramref name="none"/> is null.</exception>
    /// <remarks>
    /// This method provides a way to handle both states of an option with side effects,
    /// similar to pattern matching but for actions rather than transformations.
    /// It ensures that exactly one of the two actions is executed based on the option's state.
    /// The option is returned unchanged to allow for fluent chaining of operations.
    /// </remarks>
    public static Option<TValue> OnEither<TValue>(
        in this Option<TValue> option,
        Action<TValue> some,
        Action none)
        where TValue : notnull
    {
        if (option.IsSome) some(option.Value);
        else none();
        
        return option;
    }
}
namespace FluentUnions;

public static partial class OptionExtensions
{
    /// <summary>
    /// Executes an action on the value inside an <see cref="Option{T}"/> containing a tuple with 2 elements if it has a value.
    /// </summary>
    /// <typeparam name="TValue1">The type of the first tuple element.</typeparam>
    /// <typeparam name="TValue2">The type of the second tuple element.</typeparam>
    /// <param name="option">The source <see cref="Option{T}"/> containing a tuple.</param>
    /// <param name="action">An action to execute on the tuple elements if the Option has a value.</param>
    /// <returns>The original Option to enable fluent chaining.</returns>
    /// <remarks>
    /// OnSome is useful for performing side effects (such as logging, debugging, or updating external state)
    /// when an Option has a value. The action is only executed if the Option is Some.
    /// The original Option is always returned unchanged, allowing for method chaining.
    /// </remarks>
    public static Option<(TValue1, TValue2)> OnSome<TValue1, TValue2>(in this Option<(TValue1, TValue2)> option, Action<TValue1, TValue2> action)
    {
        if (option.IsSome) action(option.Value.Item1, option.Value.Item2);
        return option;
    }
    
    /// <summary>
    /// Executes one of two actions based on whether an <see cref="Option{T}"/> containing a tuple with 2 elements has a value.
    /// </summary>
    /// <typeparam name="TValue1">The type of the first tuple element.</typeparam>
    /// <typeparam name="TValue2">The type of the second tuple element.</typeparam>
    /// <param name="option">The source <see cref="Option{T}"/> containing a tuple.</param>
    /// <param name="some">An action to execute on the tuple elements if the Option has a value.</param>
    /// <param name="none">An action to execute if the Option is None.</param>
    /// <returns>The original Option to enable fluent chaining.</returns>
    /// <remarks>
    /// OnEither ensures that exactly one action is executed based on the Option's state.
    /// This is useful for handling both Some and None cases with side effects, such as logging
    /// different messages or updating UI based on presence or absence of a value.
    /// The original Option is always returned unchanged.
    /// </remarks>
    public static Option<(TValue1, TValue2)> OnEither<TValue1, TValue2>(in this Option<(TValue1, TValue2)> option, Action<TValue1, TValue2> some, Action none)
    {
        if (option.IsSome) some(option.Value.Item1, option.Value.Item2);
        else none();
        return option;
    }

    /// <summary>
    /// Executes an action on the value inside an <see cref="Option{T}"/> containing a tuple with 3 elements if it has a value.
    /// </summary>
    /// <typeparam name="TValue1">The type of the first tuple element.</typeparam>
    /// <typeparam name="TValue2">The type of the second tuple element.</typeparam>
    /// <typeparam name="TValue3">The type of the third tuple element.</typeparam>
    /// <param name="option">The source <see cref="Option{T}"/> containing a tuple.</param>
    /// <param name="action">An action to execute on the tuple elements if the Option has a value.</param>
    /// <returns>The original Option to enable fluent chaining.</returns>
    /// <remarks>
    /// OnSome is useful for performing side effects (such as logging, debugging, or updating external state)
    /// when an Option has a value. The action is only executed if the Option is Some.
    /// The original Option is always returned unchanged, allowing for method chaining.
    /// </remarks>
    public static Option<(TValue1, TValue2, TValue3)> OnSome<TValue1, TValue2, TValue3>(in this Option<(TValue1, TValue2, TValue3)> option, Action<TValue1, TValue2, TValue3> action)
    {
        if (option.IsSome) action(option.Value.Item1, option.Value.Item2, option.Value.Item3);
        return option;
    }
    
    /// <summary>
    /// Executes one of two actions based on whether an <see cref="Option{T}"/> containing a tuple with 3 elements has a value.
    /// </summary>
    /// <typeparam name="TValue1">The type of the first tuple element.</typeparam>
    /// <typeparam name="TValue2">The type of the second tuple element.</typeparam>
    /// <typeparam name="TValue3">The type of the third tuple element.</typeparam>
    /// <param name="option">The source <see cref="Option{T}"/> containing a tuple.</param>
    /// <param name="some">An action to execute on the tuple elements if the Option has a value.</param>
    /// <param name="none">An action to execute if the Option is None.</param>
    /// <returns>The original Option to enable fluent chaining.</returns>
    /// <remarks>
    /// OnEither ensures that exactly one action is executed based on the Option's state.
    /// This is useful for handling both Some and None cases with side effects, such as logging
    /// different messages or updating UI based on presence or absence of a value.
    /// The original Option is always returned unchanged.
    /// </remarks>
    public static Option<(TValue1, TValue2, TValue3)> OnEither<TValue1, TValue2, TValue3>(in this Option<(TValue1, TValue2, TValue3)> option, Action<TValue1, TValue2, TValue3> some, Action none)
    {
        if (option.IsSome) some(option.Value.Item1, option.Value.Item2, option.Value.Item3);
        else none();
        return option;
    }

    /// <summary>
    /// Executes an action on the value inside an <see cref="Option{T}"/> containing a tuple with 4 elements if it has a value.
    /// </summary>
    /// <typeparam name="TValue1">The type of the first tuple element.</typeparam>
    /// <typeparam name="TValue2">The type of the second tuple element.</typeparam>
    /// <typeparam name="TValue3">The type of the third tuple element.</typeparam>
    /// <typeparam name="TValue4">The type of the fourth tuple element.</typeparam>
    /// <param name="option">The source <see cref="Option{T}"/> containing a tuple.</param>
    /// <param name="action">An action to execute on the tuple elements if the Option has a value.</param>
    /// <returns>The original Option to enable fluent chaining.</returns>
    /// <remarks>
    /// OnSome is useful for performing side effects (such as logging, debugging, or updating external state)
    /// when an Option has a value. The action is only executed if the Option is Some.
    /// The original Option is always returned unchanged, allowing for method chaining.
    /// </remarks>
    public static Option<(TValue1, TValue2, TValue3, TValue4)> OnSome<TValue1, TValue2, TValue3, TValue4>(in this Option<(TValue1, TValue2, TValue3, TValue4)> option, Action<TValue1, TValue2, TValue3, TValue4> action)
    {
        if (option.IsSome) action(option.Value.Item1, option.Value.Item2, option.Value.Item3, option.Value.Item4);
        return option;
    }
    
    /// <summary>
    /// Executes one of two actions based on whether an <see cref="Option{T}"/> containing a tuple with 4 elements has a value.
    /// </summary>
    /// <typeparam name="TValue1">The type of the first tuple element.</typeparam>
    /// <typeparam name="TValue2">The type of the second tuple element.</typeparam>
    /// <typeparam name="TValue3">The type of the third tuple element.</typeparam>
    /// <typeparam name="TValue4">The type of the fourth tuple element.</typeparam>
    /// <param name="option">The source <see cref="Option{T}"/> containing a tuple.</param>
    /// <param name="some">An action to execute on the tuple elements if the Option has a value.</param>
    /// <param name="none">An action to execute if the Option is None.</param>
    /// <returns>The original Option to enable fluent chaining.</returns>
    /// <remarks>
    /// OnEither ensures that exactly one action is executed based on the Option's state.
    /// This is useful for handling both Some and None cases with side effects, such as logging
    /// different messages or updating UI based on presence or absence of a value.
    /// The original Option is always returned unchanged.
    /// </remarks>
    public static Option<(TValue1, TValue2, TValue3, TValue4)> OnEither<TValue1, TValue2, TValue3, TValue4>(in this Option<(TValue1, TValue2, TValue3, TValue4)> option, Action<TValue1, TValue2, TValue3, TValue4> some, Action none)
    {
        if (option.IsSome) some(option.Value.Item1, option.Value.Item2, option.Value.Item3, option.Value.Item4);
        else none();
        return option;
    }

    /// <summary>
    /// Executes an action on the value inside an <see cref="Option{T}"/> containing a tuple with 5 elements if it has a value.
    /// </summary>
    /// <typeparam name="TValue1">The type of the first tuple element.</typeparam>
    /// <typeparam name="TValue2">The type of the second tuple element.</typeparam>
    /// <typeparam name="TValue3">The type of the third tuple element.</typeparam>
    /// <typeparam name="TValue4">The type of the fourth tuple element.</typeparam>
    /// <typeparam name="TValue5">The type of the fifth tuple element.</typeparam>
    /// <param name="option">The source <see cref="Option{T}"/> containing a tuple.</param>
    /// <param name="action">An action to execute on the tuple elements if the Option has a value.</param>
    /// <returns>The original Option to enable fluent chaining.</returns>
    /// <remarks>
    /// OnSome is useful for performing side effects (such as logging, debugging, or updating external state)
    /// when an Option has a value. The action is only executed if the Option is Some.
    /// The original Option is always returned unchanged, allowing for method chaining.
    /// </remarks>
    public static Option<(TValue1, TValue2, TValue3, TValue4, TValue5)> OnSome<TValue1, TValue2, TValue3, TValue4, TValue5>(in this Option<(TValue1, TValue2, TValue3, TValue4, TValue5)> option, Action<TValue1, TValue2, TValue3, TValue4, TValue5> action)
    {
        if (option.IsSome) action(option.Value.Item1, option.Value.Item2, option.Value.Item3, option.Value.Item4, option.Value.Item5);
        return option;
    }
    
    /// <summary>
    /// Executes one of two actions based on whether an <see cref="Option{T}"/> containing a tuple with 5 elements has a value.
    /// </summary>
    /// <typeparam name="TValue1">The type of the first tuple element.</typeparam>
    /// <typeparam name="TValue2">The type of the second tuple element.</typeparam>
    /// <typeparam name="TValue3">The type of the third tuple element.</typeparam>
    /// <typeparam name="TValue4">The type of the fourth tuple element.</typeparam>
    /// <typeparam name="TValue5">The type of the fifth tuple element.</typeparam>
    /// <param name="option">The source <see cref="Option{T}"/> containing a tuple.</param>
    /// <param name="some">An action to execute on the tuple elements if the Option has a value.</param>
    /// <param name="none">An action to execute if the Option is None.</param>
    /// <returns>The original Option to enable fluent chaining.</returns>
    /// <remarks>
    /// OnEither ensures that exactly one action is executed based on the Option's state.
    /// This is useful for handling both Some and None cases with side effects, such as logging
    /// different messages or updating UI based on presence or absence of a value.
    /// The original Option is always returned unchanged.
    /// </remarks>
    public static Option<(TValue1, TValue2, TValue3, TValue4, TValue5)> OnEither<TValue1, TValue2, TValue3, TValue4, TValue5>(in this Option<(TValue1, TValue2, TValue3, TValue4, TValue5)> option, Action<TValue1, TValue2, TValue3, TValue4, TValue5> some, Action none)
    {
        if (option.IsSome) some(option.Value.Item1, option.Value.Item2, option.Value.Item3, option.Value.Item4, option.Value.Item5);
        else none();
        return option;
    }

    /// <summary>
    /// Executes an action on the value inside an <see cref="Option{T}"/> containing a tuple with 6 elements if it has a value.
    /// </summary>
    /// <typeparam name="TValue1">The type of the first tuple element.</typeparam>
    /// <typeparam name="TValue2">The type of the second tuple element.</typeparam>
    /// <typeparam name="TValue3">The type of the third tuple element.</typeparam>
    /// <typeparam name="TValue4">The type of the fourth tuple element.</typeparam>
    /// <typeparam name="TValue5">The type of the fifth tuple element.</typeparam>
    /// <typeparam name="TValue6">The type of the sixth tuple element.</typeparam>
    /// <param name="option">The source <see cref="Option{T}"/> containing a tuple.</param>
    /// <param name="action">An action to execute on the tuple elements if the Option has a value.</param>
    /// <returns>The original Option to enable fluent chaining.</returns>
    /// <remarks>
    /// OnSome is useful for performing side effects (such as logging, debugging, or updating external state)
    /// when an Option has a value. The action is only executed if the Option is Some.
    /// The original Option is always returned unchanged, allowing for method chaining.
    /// </remarks>
    public static Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)> OnSome<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(in this Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)> option, Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> action)
    {
        if (option.IsSome) action(option.Value.Item1, option.Value.Item2, option.Value.Item3, option.Value.Item4, option.Value.Item5, option.Value.Item6);
        return option;
    }
    
    /// <summary>
    /// Executes one of two actions based on whether an <see cref="Option{T}"/> containing a tuple with 6 elements has a value.
    /// </summary>
    /// <typeparam name="TValue1">The type of the first tuple element.</typeparam>
    /// <typeparam name="TValue2">The type of the second tuple element.</typeparam>
    /// <typeparam name="TValue3">The type of the third tuple element.</typeparam>
    /// <typeparam name="TValue4">The type of the fourth tuple element.</typeparam>
    /// <typeparam name="TValue5">The type of the fifth tuple element.</typeparam>
    /// <typeparam name="TValue6">The type of the sixth tuple element.</typeparam>
    /// <param name="option">The source <see cref="Option{T}"/> containing a tuple.</param>
    /// <param name="some">An action to execute on the tuple elements if the Option has a value.</param>
    /// <param name="none">An action to execute if the Option is None.</param>
    /// <returns>The original Option to enable fluent chaining.</returns>
    /// <remarks>
    /// OnEither ensures that exactly one action is executed based on the Option's state.
    /// This is useful for handling both Some and None cases with side effects, such as logging
    /// different messages or updating UI based on presence or absence of a value.
    /// The original Option is always returned unchanged.
    /// </remarks>
    public static Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)> OnEither<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(in this Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)> option, Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> some, Action none)
    {
        if (option.IsSome) some(option.Value.Item1, option.Value.Item2, option.Value.Item3, option.Value.Item4, option.Value.Item5, option.Value.Item6);
        else none();
        return option;
    }

    /// <summary>
    /// Executes an action on the value inside an <see cref="Option{T}"/> containing a tuple with 7 elements if it has a value.
    /// </summary>
    /// <typeparam name="TValue1">The type of the first tuple element.</typeparam>
    /// <typeparam name="TValue2">The type of the second tuple element.</typeparam>
    /// <typeparam name="TValue3">The type of the third tuple element.</typeparam>
    /// <typeparam name="TValue4">The type of the fourth tuple element.</typeparam>
    /// <typeparam name="TValue5">The type of the fifth tuple element.</typeparam>
    /// <typeparam name="TValue6">The type of the sixth tuple element.</typeparam>
    /// <typeparam name="TValue7">The type of the seventh tuple element.</typeparam>
    /// <param name="option">The source <see cref="Option{T}"/> containing a tuple.</param>
    /// <param name="action">An action to execute on the tuple elements if the Option has a value.</param>
    /// <returns>The original Option to enable fluent chaining.</returns>
    /// <remarks>
    /// OnSome is useful for performing side effects (such as logging, debugging, or updating external state)
    /// when an Option has a value. The action is only executed if the Option is Some.
    /// The original Option is always returned unchanged, allowing for method chaining.
    /// </remarks>
    public static Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)> OnSome<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(in this Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)> option, Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> action)
    {
        if (option.IsSome) action(option.Value.Item1, option.Value.Item2, option.Value.Item3, option.Value.Item4, option.Value.Item5, option.Value.Item6, option.Value.Item7);
        return option;
    }
    
    /// <summary>
    /// Executes one of two actions based on whether an <see cref="Option{T}"/> containing a tuple with 7 elements has a value.
    /// </summary>
    /// <typeparam name="TValue1">The type of the first tuple element.</typeparam>
    /// <typeparam name="TValue2">The type of the second tuple element.</typeparam>
    /// <typeparam name="TValue3">The type of the third tuple element.</typeparam>
    /// <typeparam name="TValue4">The type of the fourth tuple element.</typeparam>
    /// <typeparam name="TValue5">The type of the fifth tuple element.</typeparam>
    /// <typeparam name="TValue6">The type of the sixth tuple element.</typeparam>
    /// <typeparam name="TValue7">The type of the seventh tuple element.</typeparam>
    /// <param name="option">The source <see cref="Option{T}"/> containing a tuple.</param>
    /// <param name="some">An action to execute on the tuple elements if the Option has a value.</param>
    /// <param name="none">An action to execute if the Option is None.</param>
    /// <returns>The original Option to enable fluent chaining.</returns>
    /// <remarks>
    /// OnEither ensures that exactly one action is executed based on the Option's state.
    /// This is useful for handling both Some and None cases with side effects, such as logging
    /// different messages or updating UI based on presence or absence of a value.
    /// The original Option is always returned unchanged.
    /// </remarks>
    public static Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)> OnEither<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(in this Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)> option, Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> some, Action none)
    {
        if (option.IsSome) some(option.Value.Item1, option.Value.Item2, option.Value.Item3, option.Value.Item4, option.Value.Item5, option.Value.Item6, option.Value.Item7);
        else none();
        return option;
    }

    /// <summary>
    /// Executes an action on the value inside an <see cref="Option{T}"/> containing a tuple with 8 elements if it has a value.
    /// </summary>
    /// <typeparam name="TValue1">The type of the first tuple element.</typeparam>
    /// <typeparam name="TValue2">The type of the second tuple element.</typeparam>
    /// <typeparam name="TValue3">The type of the third tuple element.</typeparam>
    /// <typeparam name="TValue4">The type of the fourth tuple element.</typeparam>
    /// <typeparam name="TValue5">The type of the fifth tuple element.</typeparam>
    /// <typeparam name="TValue6">The type of the sixth tuple element.</typeparam>
    /// <typeparam name="TValue7">The type of the seventh tuple element.</typeparam>
    /// <typeparam name="TValue8">The type of the eighth tuple element.</typeparam>
    /// <param name="option">The source <see cref="Option{T}"/> containing a tuple.</param>
    /// <param name="action">An action to execute on the tuple elements if the Option has a value.</param>
    /// <returns>The original Option to enable fluent chaining.</returns>
    /// <remarks>
    /// OnSome is useful for performing side effects (such as logging, debugging, or updating external state)
    /// when an Option has a value. The action is only executed if the Option is Some.
    /// The original Option is always returned unchanged, allowing for method chaining.
    /// </remarks>
    public static Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)> OnSome<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(in this Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)> option, Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> action)
    {
        if (option.IsSome) action(option.Value.Item1, option.Value.Item2, option.Value.Item3, option.Value.Item4, option.Value.Item5, option.Value.Item6, option.Value.Item7, option.Value.Item8);
        return option;
    }
    
    /// <summary>
    /// Executes one of two actions based on whether an <see cref="Option{T}"/> containing a tuple with 8 elements has a value.
    /// </summary>
    /// <typeparam name="TValue1">The type of the first tuple element.</typeparam>
    /// <typeparam name="TValue2">The type of the second tuple element.</typeparam>
    /// <typeparam name="TValue3">The type of the third tuple element.</typeparam>
    /// <typeparam name="TValue4">The type of the fourth tuple element.</typeparam>
    /// <typeparam name="TValue5">The type of the fifth tuple element.</typeparam>
    /// <typeparam name="TValue6">The type of the sixth tuple element.</typeparam>
    /// <typeparam name="TValue7">The type of the seventh tuple element.</typeparam>
    /// <typeparam name="TValue8">The type of the eighth tuple element.</typeparam>
    /// <param name="option">The source <see cref="Option{T}"/> containing a tuple.</param>
    /// <param name="some">An action to execute on the tuple elements if the Option has a value.</param>
    /// <param name="none">An action to execute if the Option is None.</param>
    /// <returns>The original Option to enable fluent chaining.</returns>
    /// <remarks>
    /// OnEither ensures that exactly one action is executed based on the Option's state.
    /// This is useful for handling both Some and None cases with side effects, such as logging
    /// different messages or updating UI based on presence or absence of a value.
    /// The original Option is always returned unchanged.
    /// </remarks>
    public static Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)> OnEither<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(in this Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)> option, Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> some, Action none)
    {
        if (option.IsSome) some(option.Value.Item1, option.Value.Item2, option.Value.Item3, option.Value.Item4, option.Value.Item5, option.Value.Item6, option.Value.Item7, option.Value.Item8);
        else none();
        return option;
    }

}
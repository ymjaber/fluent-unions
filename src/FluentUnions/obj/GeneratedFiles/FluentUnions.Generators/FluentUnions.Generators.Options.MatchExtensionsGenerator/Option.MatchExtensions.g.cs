namespace FluentUnions;

public static partial class OptionExtensions
{
    /// <summary>
    /// Matches on an <see cref="Option{T}"/> containing a tuple with 2 elements, executing one of two functions based on its state.
    /// </summary>
    /// <typeparam name="TSource1">The type of the first tuple element.</typeparam>
    /// <typeparam name="TSource2">The type of the second tuple element.</typeparam>
    /// <typeparam name="TTarget">The type of the value returned by the match functions.</typeparam>
    /// <param name="option">The source <see cref="Option{T}"/> containing a tuple.</param>
    /// <param name="some">A function to execute on the tuple elements if the Option has a value.</param>
    /// <param name="none">A function to execute if the Option is None.</param>
    /// <returns>The value returned by either the some or none function.</returns>
    /// <remarks>
    /// Match provides exhaustive pattern matching for Option types, ensuring both Some and None cases
    /// are handled. This is a fundamental functional programming pattern that guarantees a value
    /// is always produced, making it impossible to forget handling the None case.
    /// The tuple elements are automatically destructured when passed to the some function.
    /// </remarks>
    public static TTarget Match<TSource1, TSource2, TTarget>(
        in this Option<(TSource1, TSource2)> option,
        Func<TSource1, TSource2, TTarget> some,
        Func<TTarget> none)
    {
        return option.IsSome ? some(option.Value.Item1, option.Value.Item2) : none();
    }

    /// <summary>
    /// Matches on an <see cref="Option{T}"/> containing a tuple with 3 elements, executing one of two functions based on its state.
    /// </summary>
    /// <typeparam name="TSource1">The type of the first tuple element.</typeparam>
    /// <typeparam name="TSource2">The type of the second tuple element.</typeparam>
    /// <typeparam name="TSource3">The type of the third tuple element.</typeparam>
    /// <typeparam name="TTarget">The type of the value returned by the match functions.</typeparam>
    /// <param name="option">The source <see cref="Option{T}"/> containing a tuple.</param>
    /// <param name="some">A function to execute on the tuple elements if the Option has a value.</param>
    /// <param name="none">A function to execute if the Option is None.</param>
    /// <returns>The value returned by either the some or none function.</returns>
    /// <remarks>
    /// Match provides exhaustive pattern matching for Option types, ensuring both Some and None cases
    /// are handled. This is a fundamental functional programming pattern that guarantees a value
    /// is always produced, making it impossible to forget handling the None case.
    /// The tuple elements are automatically destructured when passed to the some function.
    /// </remarks>
    public static TTarget Match<TSource1, TSource2, TSource3, TTarget>(
        in this Option<(TSource1, TSource2, TSource3)> option,
        Func<TSource1, TSource2, TSource3, TTarget> some,
        Func<TTarget> none)
    {
        return option.IsSome ? some(option.Value.Item1, option.Value.Item2, option.Value.Item3) : none();
    }

    /// <summary>
    /// Matches on an <see cref="Option{T}"/> containing a tuple with 4 elements, executing one of two functions based on its state.
    /// </summary>
    /// <typeparam name="TSource1">The type of the first tuple element.</typeparam>
    /// <typeparam name="TSource2">The type of the second tuple element.</typeparam>
    /// <typeparam name="TSource3">The type of the third tuple element.</typeparam>
    /// <typeparam name="TSource4">The type of the fourth tuple element.</typeparam>
    /// <typeparam name="TTarget">The type of the value returned by the match functions.</typeparam>
    /// <param name="option">The source <see cref="Option{T}"/> containing a tuple.</param>
    /// <param name="some">A function to execute on the tuple elements if the Option has a value.</param>
    /// <param name="none">A function to execute if the Option is None.</param>
    /// <returns>The value returned by either the some or none function.</returns>
    /// <remarks>
    /// Match provides exhaustive pattern matching for Option types, ensuring both Some and None cases
    /// are handled. This is a fundamental functional programming pattern that guarantees a value
    /// is always produced, making it impossible to forget handling the None case.
    /// The tuple elements are automatically destructured when passed to the some function.
    /// </remarks>
    public static TTarget Match<TSource1, TSource2, TSource3, TSource4, TTarget>(
        in this Option<(TSource1, TSource2, TSource3, TSource4)> option,
        Func<TSource1, TSource2, TSource3, TSource4, TTarget> some,
        Func<TTarget> none)
    {
        return option.IsSome ? some(option.Value.Item1, option.Value.Item2, option.Value.Item3, option.Value.Item4) : none();
    }

    /// <summary>
    /// Matches on an <see cref="Option{T}"/> containing a tuple with 5 elements, executing one of two functions based on its state.
    /// </summary>
    /// <typeparam name="TSource1">The type of the first tuple element.</typeparam>
    /// <typeparam name="TSource2">The type of the second tuple element.</typeparam>
    /// <typeparam name="TSource3">The type of the third tuple element.</typeparam>
    /// <typeparam name="TSource4">The type of the fourth tuple element.</typeparam>
    /// <typeparam name="TSource5">The type of the fifth tuple element.</typeparam>
    /// <typeparam name="TTarget">The type of the value returned by the match functions.</typeparam>
    /// <param name="option">The source <see cref="Option{T}"/> containing a tuple.</param>
    /// <param name="some">A function to execute on the tuple elements if the Option has a value.</param>
    /// <param name="none">A function to execute if the Option is None.</param>
    /// <returns>The value returned by either the some or none function.</returns>
    /// <remarks>
    /// Match provides exhaustive pattern matching for Option types, ensuring both Some and None cases
    /// are handled. This is a fundamental functional programming pattern that guarantees a value
    /// is always produced, making it impossible to forget handling the None case.
    /// The tuple elements are automatically destructured when passed to the some function.
    /// </remarks>
    public static TTarget Match<TSource1, TSource2, TSource3, TSource4, TSource5, TTarget>(
        in this Option<(TSource1, TSource2, TSource3, TSource4, TSource5)> option,
        Func<TSource1, TSource2, TSource3, TSource4, TSource5, TTarget> some,
        Func<TTarget> none)
    {
        return option.IsSome ? some(option.Value.Item1, option.Value.Item2, option.Value.Item3, option.Value.Item4, option.Value.Item5) : none();
    }

    /// <summary>
    /// Matches on an <see cref="Option{T}"/> containing a tuple with 6 elements, executing one of two functions based on its state.
    /// </summary>
    /// <typeparam name="TSource1">The type of the first tuple element.</typeparam>
    /// <typeparam name="TSource2">The type of the second tuple element.</typeparam>
    /// <typeparam name="TSource3">The type of the third tuple element.</typeparam>
    /// <typeparam name="TSource4">The type of the fourth tuple element.</typeparam>
    /// <typeparam name="TSource5">The type of the fifth tuple element.</typeparam>
    /// <typeparam name="TSource6">The type of the sixth tuple element.</typeparam>
    /// <typeparam name="TTarget">The type of the value returned by the match functions.</typeparam>
    /// <param name="option">The source <see cref="Option{T}"/> containing a tuple.</param>
    /// <param name="some">A function to execute on the tuple elements if the Option has a value.</param>
    /// <param name="none">A function to execute if the Option is None.</param>
    /// <returns>The value returned by either the some or none function.</returns>
    /// <remarks>
    /// Match provides exhaustive pattern matching for Option types, ensuring both Some and None cases
    /// are handled. This is a fundamental functional programming pattern that guarantees a value
    /// is always produced, making it impossible to forget handling the None case.
    /// The tuple elements are automatically destructured when passed to the some function.
    /// </remarks>
    public static TTarget Match<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TTarget>(
        in this Option<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6)> option,
        Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TTarget> some,
        Func<TTarget> none)
    {
        return option.IsSome ? some(option.Value.Item1, option.Value.Item2, option.Value.Item3, option.Value.Item4, option.Value.Item5, option.Value.Item6) : none();
    }

    /// <summary>
    /// Matches on an <see cref="Option{T}"/> containing a tuple with 7 elements, executing one of two functions based on its state.
    /// </summary>
    /// <typeparam name="TSource1">The type of the first tuple element.</typeparam>
    /// <typeparam name="TSource2">The type of the second tuple element.</typeparam>
    /// <typeparam name="TSource3">The type of the third tuple element.</typeparam>
    /// <typeparam name="TSource4">The type of the fourth tuple element.</typeparam>
    /// <typeparam name="TSource5">The type of the fifth tuple element.</typeparam>
    /// <typeparam name="TSource6">The type of the sixth tuple element.</typeparam>
    /// <typeparam name="TSource7">The type of the seventh tuple element.</typeparam>
    /// <typeparam name="TTarget">The type of the value returned by the match functions.</typeparam>
    /// <param name="option">The source <see cref="Option{T}"/> containing a tuple.</param>
    /// <param name="some">A function to execute on the tuple elements if the Option has a value.</param>
    /// <param name="none">A function to execute if the Option is None.</param>
    /// <returns>The value returned by either the some or none function.</returns>
    /// <remarks>
    /// Match provides exhaustive pattern matching for Option types, ensuring both Some and None cases
    /// are handled. This is a fundamental functional programming pattern that guarantees a value
    /// is always produced, making it impossible to forget handling the None case.
    /// The tuple elements are automatically destructured when passed to the some function.
    /// </remarks>
    public static TTarget Match<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TTarget>(
        in this Option<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7)> option,
        Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TTarget> some,
        Func<TTarget> none)
    {
        return option.IsSome ? some(option.Value.Item1, option.Value.Item2, option.Value.Item3, option.Value.Item4, option.Value.Item5, option.Value.Item6, option.Value.Item7) : none();
    }

    /// <summary>
    /// Matches on an <see cref="Option{T}"/> containing a tuple with 8 elements, executing one of two functions based on its state.
    /// </summary>
    /// <typeparam name="TSource1">The type of the first tuple element.</typeparam>
    /// <typeparam name="TSource2">The type of the second tuple element.</typeparam>
    /// <typeparam name="TSource3">The type of the third tuple element.</typeparam>
    /// <typeparam name="TSource4">The type of the fourth tuple element.</typeparam>
    /// <typeparam name="TSource5">The type of the fifth tuple element.</typeparam>
    /// <typeparam name="TSource6">The type of the sixth tuple element.</typeparam>
    /// <typeparam name="TSource7">The type of the seventh tuple element.</typeparam>
    /// <typeparam name="TSource8">The type of the eighth tuple element.</typeparam>
    /// <typeparam name="TTarget">The type of the value returned by the match functions.</typeparam>
    /// <param name="option">The source <see cref="Option{T}"/> containing a tuple.</param>
    /// <param name="some">A function to execute on the tuple elements if the Option has a value.</param>
    /// <param name="none">A function to execute if the Option is None.</param>
    /// <returns>The value returned by either the some or none function.</returns>
    /// <remarks>
    /// Match provides exhaustive pattern matching for Option types, ensuring both Some and None cases
    /// are handled. This is a fundamental functional programming pattern that guarantees a value
    /// is always produced, making it impossible to forget handling the None case.
    /// The tuple elements are automatically destructured when passed to the some function.
    /// </remarks>
    public static TTarget Match<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TTarget>(
        in this Option<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8)> option,
        Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TTarget> some,
        Func<TTarget> none)
    {
        return option.IsSome ? some(option.Value.Item1, option.Value.Item2, option.Value.Item3, option.Value.Item4, option.Value.Item5, option.Value.Item6, option.Value.Item7, option.Value.Item8) : none();
    }

}
namespace FluentUnions;

public static partial class OptionExtensions
{
    /// <summary>
    /// Applies a binder function to the value inside an <see cref="Option{T}"/> containing a tuple with 2 elements.
    /// </summary>
    /// <typeparam name="TValue1">The type of the first tuple element.</typeparam>
    /// <typeparam name="TValue2">The type of the second tuple element.</typeparam>
    /// <typeparam name="TTarget">The type of the value in the resulting Option.</typeparam>
    /// <param name="source">The source <see cref="Option{T}"/> containing a tuple.</param>
    /// <param name="binder">A function that takes the tuple elements and returns a new Option.</param>
    /// <returns>The Option returned by the binder if the source was Some; otherwise, None.</returns>
    /// <remarks>
    /// Bind (also known as flatMap or >>=) is the fundamental monadic composition operation.
    /// It allows chaining operations that may fail or produce no value. If the source Option is None,
    /// the binder function is not executed and None is returned. This enables safe composition
    /// of operations without explicit null checking.
    /// </remarks>
    public static Option<TTarget> Bind<TValue1, TValue2, TTarget>(
        in this Option<(TValue1, TValue2)> source,
        Func<TValue1, TValue2, Option<TTarget>> binder)
    {
        if (source.IsNone) return Option<TTarget>.None;
        return binder(source.Value.Item1, source.Value.Item2);
    }
    
    /// <summary>
    /// Applies a binder function to the value inside an <see cref="Option{T}"/> containing a tuple with 3 elements.
    /// </summary>
    /// <typeparam name="TValue1">The type of the first tuple element.</typeparam>
    /// <typeparam name="TValue2">The type of the second tuple element.</typeparam>
    /// <typeparam name="TValue3">The type of the third tuple element.</typeparam>
    /// <typeparam name="TTarget">The type of the value in the resulting Option.</typeparam>
    /// <param name="source">The source <see cref="Option{T}"/> containing a tuple.</param>
    /// <param name="binder">A function that takes the tuple elements and returns a new Option.</param>
    /// <returns>The Option returned by the binder if the source was Some; otherwise, None.</returns>
    /// <remarks>
    /// Bind (also known as flatMap or >>=) is the fundamental monadic composition operation.
    /// It allows chaining operations that may fail or produce no value. If the source Option is None,
    /// the binder function is not executed and None is returned. This enables safe composition
    /// of operations without explicit null checking.
    /// </remarks>
    public static Option<TTarget> Bind<TValue1, TValue2, TValue3, TTarget>(
        in this Option<(TValue1, TValue2, TValue3)> source,
        Func<TValue1, TValue2, TValue3, Option<TTarget>> binder)
    {
        if (source.IsNone) return Option<TTarget>.None;
        return binder(source.Value.Item1, source.Value.Item2, source.Value.Item3);
    }
    
    /// <summary>
    /// Applies a binder function to the value inside an <see cref="Option{T}"/> containing a tuple with 4 elements.
    /// </summary>
    /// <typeparam name="TValue1">The type of the first tuple element.</typeparam>
    /// <typeparam name="TValue2">The type of the second tuple element.</typeparam>
    /// <typeparam name="TValue3">The type of the third tuple element.</typeparam>
    /// <typeparam name="TValue4">The type of the fourth tuple element.</typeparam>
    /// <typeparam name="TTarget">The type of the value in the resulting Option.</typeparam>
    /// <param name="source">The source <see cref="Option{T}"/> containing a tuple.</param>
    /// <param name="binder">A function that takes the tuple elements and returns a new Option.</param>
    /// <returns>The Option returned by the binder if the source was Some; otherwise, None.</returns>
    /// <remarks>
    /// Bind (also known as flatMap or >>=) is the fundamental monadic composition operation.
    /// It allows chaining operations that may fail or produce no value. If the source Option is None,
    /// the binder function is not executed and None is returned. This enables safe composition
    /// of operations without explicit null checking.
    /// </remarks>
    public static Option<TTarget> Bind<TValue1, TValue2, TValue3, TValue4, TTarget>(
        in this Option<(TValue1, TValue2, TValue3, TValue4)> source,
        Func<TValue1, TValue2, TValue3, TValue4, Option<TTarget>> binder)
    {
        if (source.IsNone) return Option<TTarget>.None;
        return binder(source.Value.Item1, source.Value.Item2, source.Value.Item3, source.Value.Item4);
    }
    
    /// <summary>
    /// Applies a binder function to the value inside an <see cref="Option{T}"/> containing a tuple with 5 elements.
    /// </summary>
    /// <typeparam name="TValue1">The type of the first tuple element.</typeparam>
    /// <typeparam name="TValue2">The type of the second tuple element.</typeparam>
    /// <typeparam name="TValue3">The type of the third tuple element.</typeparam>
    /// <typeparam name="TValue4">The type of the fourth tuple element.</typeparam>
    /// <typeparam name="TValue5">The type of the fifth tuple element.</typeparam>
    /// <typeparam name="TTarget">The type of the value in the resulting Option.</typeparam>
    /// <param name="source">The source <see cref="Option{T}"/> containing a tuple.</param>
    /// <param name="binder">A function that takes the tuple elements and returns a new Option.</param>
    /// <returns>The Option returned by the binder if the source was Some; otherwise, None.</returns>
    /// <remarks>
    /// Bind (also known as flatMap or >>=) is the fundamental monadic composition operation.
    /// It allows chaining operations that may fail or produce no value. If the source Option is None,
    /// the binder function is not executed and None is returned. This enables safe composition
    /// of operations without explicit null checking.
    /// </remarks>
    public static Option<TTarget> Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TTarget>(
        in this Option<(TValue1, TValue2, TValue3, TValue4, TValue5)> source,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, Option<TTarget>> binder)
    {
        if (source.IsNone) return Option<TTarget>.None;
        return binder(source.Value.Item1, source.Value.Item2, source.Value.Item3, source.Value.Item4, source.Value.Item5);
    }
    
    /// <summary>
    /// Applies a binder function to the value inside an <see cref="Option{T}"/> containing a tuple with 6 elements.
    /// </summary>
    /// <typeparam name="TValue1">The type of the first tuple element.</typeparam>
    /// <typeparam name="TValue2">The type of the second tuple element.</typeparam>
    /// <typeparam name="TValue3">The type of the third tuple element.</typeparam>
    /// <typeparam name="TValue4">The type of the fourth tuple element.</typeparam>
    /// <typeparam name="TValue5">The type of the fifth tuple element.</typeparam>
    /// <typeparam name="TValue6">The type of the sixth tuple element.</typeparam>
    /// <typeparam name="TTarget">The type of the value in the resulting Option.</typeparam>
    /// <param name="source">The source <see cref="Option{T}"/> containing a tuple.</param>
    /// <param name="binder">A function that takes the tuple elements and returns a new Option.</param>
    /// <returns>The Option returned by the binder if the source was Some; otherwise, None.</returns>
    /// <remarks>
    /// Bind (also known as flatMap or >>=) is the fundamental monadic composition operation.
    /// It allows chaining operations that may fail or produce no value. If the source Option is None,
    /// the binder function is not executed and None is returned. This enables safe composition
    /// of operations without explicit null checking.
    /// </remarks>
    public static Option<TTarget> Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TTarget>(
        in this Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)> source,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, Option<TTarget>> binder)
    {
        if (source.IsNone) return Option<TTarget>.None;
        return binder(source.Value.Item1, source.Value.Item2, source.Value.Item3, source.Value.Item4, source.Value.Item5, source.Value.Item6);
    }
    
    /// <summary>
    /// Applies a binder function to the value inside an <see cref="Option{T}"/> containing a tuple with 7 elements.
    /// </summary>
    /// <typeparam name="TValue1">The type of the first tuple element.</typeparam>
    /// <typeparam name="TValue2">The type of the second tuple element.</typeparam>
    /// <typeparam name="TValue3">The type of the third tuple element.</typeparam>
    /// <typeparam name="TValue4">The type of the fourth tuple element.</typeparam>
    /// <typeparam name="TValue5">The type of the fifth tuple element.</typeparam>
    /// <typeparam name="TValue6">The type of the sixth tuple element.</typeparam>
    /// <typeparam name="TValue7">The type of the seventh tuple element.</typeparam>
    /// <typeparam name="TTarget">The type of the value in the resulting Option.</typeparam>
    /// <param name="source">The source <see cref="Option{T}"/> containing a tuple.</param>
    /// <param name="binder">A function that takes the tuple elements and returns a new Option.</param>
    /// <returns>The Option returned by the binder if the source was Some; otherwise, None.</returns>
    /// <remarks>
    /// Bind (also known as flatMap or >>=) is the fundamental monadic composition operation.
    /// It allows chaining operations that may fail or produce no value. If the source Option is None,
    /// the binder function is not executed and None is returned. This enables safe composition
    /// of operations without explicit null checking.
    /// </remarks>
    public static Option<TTarget> Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TTarget>(
        in this Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)> source,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, Option<TTarget>> binder)
    {
        if (source.IsNone) return Option<TTarget>.None;
        return binder(source.Value.Item1, source.Value.Item2, source.Value.Item3, source.Value.Item4, source.Value.Item5, source.Value.Item6, source.Value.Item7);
    }
    
    /// <summary>
    /// Applies a binder function to the value inside an <see cref="Option{T}"/> containing a tuple with 8 elements.
    /// </summary>
    /// <typeparam name="TValue1">The type of the first tuple element.</typeparam>
    /// <typeparam name="TValue2">The type of the second tuple element.</typeparam>
    /// <typeparam name="TValue3">The type of the third tuple element.</typeparam>
    /// <typeparam name="TValue4">The type of the fourth tuple element.</typeparam>
    /// <typeparam name="TValue5">The type of the fifth tuple element.</typeparam>
    /// <typeparam name="TValue6">The type of the sixth tuple element.</typeparam>
    /// <typeparam name="TValue7">The type of the seventh tuple element.</typeparam>
    /// <typeparam name="TValue8">The type of the eighth tuple element.</typeparam>
    /// <typeparam name="TTarget">The type of the value in the resulting Option.</typeparam>
    /// <param name="source">The source <see cref="Option{T}"/> containing a tuple.</param>
    /// <param name="binder">A function that takes the tuple elements and returns a new Option.</param>
    /// <returns>The Option returned by the binder if the source was Some; otherwise, None.</returns>
    /// <remarks>
    /// Bind (also known as flatMap or >>=) is the fundamental monadic composition operation.
    /// It allows chaining operations that may fail or produce no value. If the source Option is None,
    /// the binder function is not executed and None is returned. This enables safe composition
    /// of operations without explicit null checking.
    /// </remarks>
    public static Option<TTarget> Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, TTarget>(
        in this Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)> source,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, Option<TTarget>> binder)
    {
        if (source.IsNone) return Option<TTarget>.None;
        return binder(source.Value.Item1, source.Value.Item2, source.Value.Item3, source.Value.Item4, source.Value.Item5, source.Value.Item6, source.Value.Item7, source.Value.Item8);
    }
    
}
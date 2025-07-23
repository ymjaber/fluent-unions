namespace FluentUnions;

/// <summary>
/// Provides monadic bind operations for <see cref="Option{T}"/> types.
/// </summary>
public static partial class OptionExtensions
{
    /// <summary>
    /// Applies a function that returns an <see cref="Option{TTarget}"/> to the value inside an <see cref="Option{TSource}"/>.
    /// </summary>
    /// <typeparam name="TSource">The type of the value contained in the source option.</typeparam>
    /// <typeparam name="TTarget">The type of the value in the resulting option.</typeparam>
    /// <param name="option">The option containing the value to transform.</param>
    /// <param name="binder">A function that takes the value from the source option and returns an <see cref="Option{TTarget}"/>.</param>
    /// <returns>
    /// The result of applying <paramref name="binder"/> to the value if the source option contains a value;
    /// otherwise, <see cref="Option{TTarget}.None"/>.
    /// </returns>
    /// <remarks>
    /// This method implements the monadic bind operation (also known as flatMap or SelectMany in other libraries).
    /// It allows for chaining operations that might fail, where each operation returns an Option.
    /// If any operation in the chain returns None, the entire chain short-circuits and returns None.
    /// This is useful for composing multiple operations that might not produce a value.
    /// </remarks>
    public static Option<TTarget> Bind<TSource, TTarget>(
        in this Option<TSource> option,
        Func<TSource, Option<TTarget>> binder)
    where TSource : notnull
    where TTarget : notnull
    {
        if (option.IsNone) return Option<TTarget>.None;
        return binder(option.Value);
    }
}
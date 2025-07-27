namespace FluentUnions;

/// <summary>
/// Provides pattern matching extension methods for <see cref="Option{T}"/> types.
/// </summary>
public static partial class OptionExtensions
{
    /// <summary>
    /// Matches on the <see cref="Option{TSource}"/> and returns a value of type <typeparamref name="TTarget"/>.
    /// </summary>
    /// <typeparam name="TSource">The type of the value contained in the option.</typeparam>
    /// <typeparam name="TTarget">The type of the result returned by the match operation.</typeparam>
    /// <param name="option">The option to match on.</param>
    /// <param name="some">The function to execute when the option contains a value. Receives the contained value and returns a result.</param>
    /// <param name="none">The function to execute when the option is empty. Returns a result.</param>
    /// <returns>The result of executing <paramref name="some"/> if the option contains a value; otherwise, the result of executing <paramref name="none"/>.</returns>
    /// <remarks>
    /// This method provides exhaustive pattern matching for option types, ensuring that both the Some and None cases are handled.
    /// It is useful for transforming an option into another type while handling both possible states.
    /// </remarks>
    public static TTarget Match<TSource, TTarget>(
        in this Option<TSource> option,
        Func<TSource, TTarget> some,
        Func<TTarget> none)
        where TSource : notnull
    {
        return option.IsSome ? some(option.Value) : none();
    }
}
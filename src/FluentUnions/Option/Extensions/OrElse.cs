namespace FluentUnions;

/// <summary>
/// Provides default value methods for <see cref="Option{T}"/> types.
/// </summary>
public static partial class OptionExtensions
{
    /// <summary>
    /// Returns this option if it has a value, otherwise returns the specified fallback option.
    /// </summary>
    /// <typeparam name="TValue">The type of the value contained in the option.</typeparam>
    /// <param name="option">The primary option to check for a value.</param>
    /// <param name="fallback">The fallback option to return if the primary option is None.</param>
    /// <returns>
    /// The primary option if it contains a value (is Some); 
    /// otherwise, the fallback option (which may itself be Some or None).
    /// </returns>
    /// <remarks>
    /// This method provides a way to chain multiple options as fallbacks. It's useful when you have
    /// multiple potential sources for a value and want to try them in order. The fallback is eagerly
    /// evaluated, so if you need lazy evaluation, use the overload that takes a factory function.
    /// </remarks>
    public static Option<TValue> OrElse<TValue>(
        in this Option<TValue> option,
        Option<TValue> fallback)
        where TValue : notnull
    {
        return option.IsSome ? option : fallback;
    }

    /// <summary>
    /// Returns this option if it has a value, otherwise invokes the specified function and returns its result.
    /// </summary>
    /// <typeparam name="TValue">The type of the value contained in the option.</typeparam>
    /// <param name="option">The primary option to check for a value.</param>
    /// <param name="fallbackFactory">A function that produces the fallback option when the primary option is None.</param>
    /// <returns>
    /// The primary option if it contains a value (is Some);
    /// otherwise, the result of invoking the fallback factory (which may return Some or None).
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="fallbackFactory"/> is null.</exception>
    /// <remarks>
    /// This method provides lazy evaluation of the fallback option. The factory function is only
    /// invoked if the primary option is None, making it more efficient when the fallback computation
    /// is expensive or has side effects. This is particularly useful for scenarios like database
    /// lookups or API calls that should only happen if the primary value is not available.
    /// </remarks>
    public static Option<TValue> OrElse<TValue>(
        in this Option<TValue> option,
        Func<Option<TValue>> fallbackFactory)
        where TValue : notnull
    {
        if (fallbackFactory is null) throw new ArgumentNullException(nameof(fallbackFactory));
        return option.IsSome ? option : fallbackFactory();
    }
}
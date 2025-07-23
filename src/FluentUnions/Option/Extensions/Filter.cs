namespace FluentUnions;

/// <summary>
/// Provides filtering operations for <see cref="Option{T}"/> types.
/// </summary>
public static partial class OptionExtensions
{
    /// <summary>
    /// Creates a <see cref="FilterBuilder{TValue}"/> for building complex filter operations on the option.
    /// </summary>
    /// <typeparam name="TValue">The type of the value contained in the option.</typeparam>
    /// <param name="option">The option to create a filter builder for.</param>
    /// <returns>A new <see cref="FilterBuilder{TValue}"/> instance for chaining filter operations.</returns>
    /// <remarks>
    /// The filter builder pattern allows for creating complex filtering logic with a fluent API.
    /// This is useful when you need to apply multiple conditions or transformations to an option
    /// in a readable, chainable manner.
    /// </remarks>
    public static FilterBuilder<TValue> Filter<TValue>(in this Option<TValue> option)
        where TValue : notnull => new(option);

    /// <summary>
    /// Filters the option based on a predicate, returning None if the predicate is not satisfied.
    /// </summary>
    /// <typeparam name="TValue">The type of the value contained in the option.</typeparam>
    /// <param name="option">The option to filter.</param>
    /// <param name="predicate">A function to test the value against a condition.</param>
    /// <returns>
    /// The original option if it is None or if the value satisfies the predicate;
    /// otherwise, <see cref="Option{TValue}.None"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="predicate"/> is null.</exception>
    /// <remarks>
    /// This method allows you to conditionally keep or discard the value in an option based on
    /// a predicate. If the option is already None, it remains None. If it contains a value,
    /// the predicate is applied, and the option becomes None if the predicate returns false.
    /// This is useful for adding validation or constraints to optional values.
    /// </remarks>
    public static Option<TValue> Filter<TValue>(
        in this Option<TValue> option,
        Func<TValue, bool> predicate)
        where TValue : notnull
    {
        return option.IsNone || predicate(option.Value)
            ? option 
            : Option<TValue>.None;
    }
}
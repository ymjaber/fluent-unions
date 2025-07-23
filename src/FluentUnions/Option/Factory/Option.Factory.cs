namespace FluentUnions;

/// <summary>
/// Provides factory methods for creating <see cref="Option{TValue}"/> instances.
/// </summary>
public readonly struct Option
{
    /// <summary>
    /// Gets the global None value that can be implicitly converted to any <see cref="Option{TValue}"/>.
    /// </summary>
    public static Option None = default;

    /// <summary>
    /// Creates an <see cref="Option{TValue}"/> containing the specified value.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="value">The value to wrap in an option.</param>
    /// <returns>An option containing the specified value.</returns>
    /// <exception cref="ArgumentNullException">The value is null.</exception>
    public static Option<TValue> Some<TValue>(TValue value)
        where TValue : notnull
    {
        ArgumentNullException.ThrowIfNull(value, nameof(value));
        return new Option<TValue>(value);
    }

    /// <summary>
    /// Creates an <see cref="Option{TValue}"/> from a nullable reference type.
    /// </summary>
    /// <typeparam name="TValue">The reference type of the value.</typeparam>
    /// <param name="value">The nullable value to convert to an option.</param>
    /// <returns>An option containing the value if it's not null; otherwise, None.</returns>
    public static Option<TValue> From<TValue>(TValue? value)
        where TValue : class
    {
        return value is null ? Option<TValue>.None : new Option<TValue>(value);
    }

    /// <summary>
    /// Creates an <see cref="Option{TValue}"/> from a nullable value type.
    /// </summary>
    /// <typeparam name="TValue">The value type.</typeparam>
    /// <param name="value">The nullable value to convert to an option.</param>
    /// <returns>An option containing the value if it has a value; otherwise, None.</returns>
    public static Option<TValue> From<TValue>(TValue? value)
        where TValue : struct
    {
        return value.HasValue ? new Option<TValue>(value.Value) : Option<TValue>.None;
    }

    /// <summary>
    /// Returns a string representation of the None value.
    /// </summary>
    /// <returns>The string "None".</returns>
    public override string ToString() => "None";
}
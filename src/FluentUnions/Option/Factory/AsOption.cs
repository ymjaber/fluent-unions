namespace FluentUnions;

/// <summary>
/// Provides extension methods for converting nullable values to <see cref="Option{TValue}"/> instances.
/// </summary>
public static partial class AsOptionExtensions
{
    /// <summary>
    /// Converts a nullable reference type to an <see cref="Option{TValue}"/>.
    /// </summary>
    /// <typeparam name="TValue">The reference type of the value.</typeparam>
    /// <param name="value">The nullable value to convert.</param>
    /// <returns>Some containing the value if it's not null; otherwise, None.</returns>
    public static Option<TValue> AsOption<TValue>(this TValue? value)
        where TValue : class
    {
        return Option.From(value);
    }

    /// <summary>
    /// Converts a nullable value type to an <see cref="Option{TValue}"/>.
    /// </summary>
    /// <typeparam name="TValue">The value type.</typeparam>
    /// <param name="value">The nullable value to convert.</param>
    /// <returns>Some containing the value if it has a value; otherwise, None.</returns>
    public static Option<TValue> AsOption<TValue>(in this TValue? value)
        where TValue : struct
    {
        return Option.From(value);
    }

    /// <summary>
    /// Converts a result containing a nullable reference type to a result containing an option.
    /// </summary>
    /// <typeparam name="TValue">The reference type of the value.</typeparam>
    /// <param name="result">The result containing a nullable value.</param>
    /// <returns>A result containing Some if the original result was successful with a non-null value; 
    /// a result containing None if the original result was successful with a null value; 
    /// or a failed result if the original result was failed.</returns>
    public static Result<Option<TValue>> AsOption<TValue>(
        in this Result<TValue?> result)
        where TValue : class
    {
        return result.Map(Option.From);
    }
    
    /// <summary>
    /// Converts a result containing a nullable value type to a result containing an option.
    /// </summary>
    /// <typeparam name="TValue">The value type.</typeparam>
    /// <param name="result">The result containing a nullable value.</param>
    /// <returns>A result containing Some if the original result was successful with a value; 
    /// a result containing None if the original result was successful with null; 
    /// or a failed result if the original result was failed.</returns>
    public static Result<Option<TValue>> AsOption<TValue>(
        in this Result<TValue?> result)
        where TValue : struct
    {
        return result.Map(Option.From);
    }
}
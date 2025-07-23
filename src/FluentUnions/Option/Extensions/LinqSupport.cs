namespace FluentUnions;

/// <summary>
/// Provides LINQ query syntax support for <see cref="Option{T}"/> by implementing
/// Select and SelectMany extension methods.
/// </summary>
public static partial class OptionExtensions
{
    /// <summary>
    /// Projects the value of a Some option into a new form using LINQ query syntax.
    /// </summary>
    /// <typeparam name="TSource">The type of the value in the source option.</typeparam>
    /// <typeparam name="TResult">The type of the value in the resulting option.</typeparam>
    /// <param name="option">The option to project.</param>
    /// <param name="selector">A transform function to apply to the value.</param>
    /// <returns>
    /// An <see cref="Option{TResult}"/> whose value is the result of invoking the transform function
    /// on the source option's value if it was Some; otherwise, None.
    /// </returns>
    /// <remarks>
    /// This method enables LINQ query syntax for Option types. It is an alias for the Map method
    /// and allows writing queries like:
    /// <code>
    /// var result = from x in GetOption()
    ///              select x * 2;
    /// </code>
    /// </remarks>
    public static Option<TResult> Select<TSource, TResult>(
        in this Option<TSource> option,
        Func<TSource, TResult> selector)
        where TSource : notnull
        where TResult : notnull
    {
        return option.Map(selector);
    }

    /// <summary>
    /// Projects the value of a Some option to a new option and flattens the result using LINQ query syntax.
    /// </summary>
    /// <typeparam name="TSource">The type of the value in the source option.</typeparam>
    /// <typeparam name="TResult">The type of the value in the resulting option.</typeparam>
    /// <param name="option">The option to project.</param>
    /// <param name="selector">A transform function to apply to the value that returns a new option.</param>
    /// <returns>
    /// The result of invoking the transform function on the source option's value if it was Some;
    /// otherwise, None.
    /// </returns>
    /// <remarks>
    /// This method is an alias for the Bind method to support LINQ query syntax.
    /// It enables writing queries with multiple from clauses.
    /// </remarks>
    public static Option<TResult> SelectMany<TSource, TResult>(
        in this Option<TSource> option,
        Func<TSource, Option<TResult>> selector)
        where TSource : notnull
        where TResult : notnull
    {
        return option.Bind(selector);
    }

    /// <summary>
    /// Projects the value of a Some option to a new option, flattens the result,
    /// and invokes a result selector function on the pair of values.
    /// </summary>
    /// <typeparam name="TSource">The type of the value in the source option.</typeparam>
    /// <typeparam name="TCollection">The type of the intermediate value.</typeparam>
    /// <typeparam name="TResult">The type of the value in the resulting option.</typeparam>
    /// <param name="option">The option to project.</param>
    /// <param name="collectionSelector">A transform function to apply to the value that returns an intermediate option.</param>
    /// <param name="resultSelector">A transform function to apply to the pair of values.</param>
    /// <returns>
    /// An <see cref="Option{TResult}"/> that is the result of invoking the transform functions
    /// if all options were Some; otherwise, None.
    /// </returns>
    /// <remarks>
    /// This overload enables full LINQ query syntax support including multiple from clauses
    /// with projections:
    /// <code>
    /// var result = from x in GetX()
    ///              from y in GetY(x)
    ///              select x + y;
    /// </code>
    /// </remarks>
    public static Option<TResult> SelectMany<TSource, TCollection, TResult>(
        in this Option<TSource> option,
        Func<TSource, Option<TCollection>> collectionSelector,
        Func<TSource, TCollection, TResult> resultSelector)
        where TSource : notnull
        where TCollection : notnull
        where TResult : notnull
    {
        if (option.IsNone) 
            return Option<TResult>.None;

        var collectionOption = collectionSelector(option.Value);
        if (collectionOption.IsNone) 
            return Option<TResult>.None;

        return Option.Some(resultSelector(option.Value, collectionOption.Value));
    }

    /// <summary>
    /// Filters an option based on a predicate.
    /// </summary>
    /// <typeparam name="TValue">The type of the value in the option.</typeparam>
    /// <param name="option">The option to filter.</param>
    /// <param name="predicate">A function to test the value.</param>
    /// <returns>
    /// The original option if it was None or if the predicate returns true;
    /// otherwise, None.
    /// </returns>
    /// <remarks>
    /// This method enables where clauses in LINQ queries and is an alias for the Filter method:
    /// <code>
    /// var result = from x in GetOption()
    ///              where x > 0
    ///              select x;
    /// </code>
    /// </remarks>
    public static Option<TValue> Where<TValue>(
        in this Option<TValue> option,
        Func<TValue, bool> predicate)
        where TValue : notnull
    {
        return option.Filter(predicate);
    }
}
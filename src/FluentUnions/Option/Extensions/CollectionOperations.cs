namespace FluentUnions;

/// <summary>
/// Provides functional collection operations for working with collections of <see cref="Option{T}"/> instances.
/// </summary>
public static partial class OptionExtensions
{
    /// <summary>
    /// Transforms a collection of options into an option containing a collection of values.
    /// </summary>
    /// <typeparam name="TValue">The type of value in each option.</typeparam>
    /// <param name="options">The collection of options to sequence.</param>
    /// <returns>
    /// A Some option containing all values if all options were Some;
    /// otherwise, None.
    /// </returns>
    /// <remarks>
    /// This method short-circuits on the first None, returning immediately without
    /// evaluating the remaining options. This is useful for validating a collection
    /// where all items must have values.
    /// </remarks>
    /// <example>
    /// <code>
    /// var options = new[] { Option.Some(1), Option.Some(2), Option.Some(3) };
    /// var sequenced = options.Sequence(); // Some([1, 2, 3])
    /// </code>
    /// </example>
    public static Option<IEnumerable<TValue>> Sequence<TValue>(
        this IEnumerable<Option<TValue>> options)
        where TValue : notnull
    {
        var values = new List<TValue>();
        
        foreach (var option in options)
        {
            if (option.IsNone)
                return Option<IEnumerable<TValue>>.None;
            
            values.Add(option.Value);
        }
        
        return Option.Some<IEnumerable<TValue>>(values);
    }

    /// <summary>
    /// Applies a function that returns an option to each element of a collection and sequences the results.
    /// </summary>
    /// <typeparam name="TSource">The type of elements in the source collection.</typeparam>
    /// <typeparam name="TResult">The type of value in the resulting options.</typeparam>
    /// <param name="source">The source collection to traverse.</param>
    /// <param name="selector">A function to apply to each element that returns an option.</param>
    /// <returns>
    /// A Some option containing all transformed values if all operations returned Some;
    /// otherwise, None.
    /// </returns>
    /// <remarks>
    /// This method combines mapping and sequencing in a single operation. It's equivalent to
    /// calling Select followed by Sequence but more efficient.
    /// </remarks>
    /// <example>
    /// <code>
    /// var numbers = new[] { "1", "2", "3" };
    /// var parsed = numbers.Traverse(s => TryParseInt(s)); // Option&lt;IEnumerable&lt;int&gt;&gt;
    /// </code>
    /// </example>
    public static Option<IEnumerable<TResult>> Traverse<TSource, TResult>(
        this IEnumerable<TSource> source,
        Func<TSource, Option<TResult>> selector)
        where TResult : notnull
    {
        var results = new List<TResult>();
        
        foreach (var item in source)
        {
            var option = selector(item);
            if (option.IsNone)
                return Option<IEnumerable<TResult>>.None;
            
            results.Add(option.Value);
        }
        
        return Option.Some<IEnumerable<TResult>>(results);
    }

    /// <summary>
    /// Partitions a collection of options into values and Nones.
    /// </summary>
    /// <typeparam name="TValue">The type of value in each option.</typeparam>
    /// <param name="options">The collection of options to partition.</param>
    /// <returns>
    /// A tuple containing the collection of values from Some options and the count of None options.
    /// </returns>
    /// <remarks>
    /// This method is useful when you want to process both Some and None cases
    /// differently, without short-circuiting on None values.
    /// </remarks>
    /// <example>
    /// <code>
    /// var options = GetOptions();
    /// var (values, noneCount) = options.Partition();
    /// Console.WriteLine($"Found {values.Count()} values and {noneCount} empty results");
    /// </code>
    /// </example>
    public static (IEnumerable<TValue> Values, int NoneCount) Partition<TValue>(
        this IEnumerable<Option<TValue>> options)
        where TValue : notnull
    {
        var values = new List<TValue>();
        var noneCount = 0;
        
        foreach (var option in options)
        {
            if (option.IsSome)
                values.Add(option.Value);
            else
                noneCount++;
        }
        
        return (values, noneCount);
    }

    /// <summary>
    /// Filters a collection to only the Some options, extracting their values.
    /// </summary>
    /// <typeparam name="TValue">The type of value in each option.</typeparam>
    /// <param name="options">The collection of options to filter.</param>
    /// <returns>
    /// A collection containing only the values from Some options.
    /// </returns>
    /// <remarks>
    /// This method silently ignores None options. Use Partition if you need
    /// to track how many None values were encountered.
    /// </remarks>
    public static IEnumerable<TValue> Choose<TValue>(
        this IEnumerable<Option<TValue>> options)
        where TValue : notnull
    {
        foreach (var option in options)
        {
            if (option.IsSome)
                yield return option.Value;
        }
    }

    /// <summary>
    /// Applies a function that returns an option to each element and returns only the Some values.
    /// </summary>
    /// <typeparam name="TSource">The type of elements in the source collection.</typeparam>
    /// <typeparam name="TResult">The type of value in the resulting options.</typeparam>
    /// <param name="source">The source collection.</param>
    /// <param name="selector">A function to apply to each element that returns an option.</param>
    /// <returns>
    /// A collection containing only the values from elements where the selector returned Some.
    /// </returns>
    /// <remarks>
    /// This method combines mapping with filtering, returning only successful transformations.
    /// It's useful for partial functions that may not produce a value for every input.
    /// </remarks>
    /// <example>
    /// <code>
    /// var strings = new[] { "1", "abc", "2", "def", "3" };
    /// var numbers = strings.ChooseMap(s => TryParseInt(s)); // [1, 2, 3]
    /// </code>
    /// </example>
    public static IEnumerable<TResult> ChooseMap<TSource, TResult>(
        this IEnumerable<TSource> source,
        Func<TSource, Option<TResult>> selector)
        where TResult : notnull
    {
        foreach (var item in source)
        {
            var option = selector(item);
            if (option.IsSome)
                yield return option.Value;
        }
    }

    /// <summary>
    /// Combines two options into a single option containing a tuple of both values.
    /// </summary>
    /// <typeparam name="T1">The type of the first value.</typeparam>
    /// <typeparam name="T2">The type of the second value.</typeparam>
    /// <param name="option1">The first option.</param>
    /// <param name="option2">The second option.</param>
    /// <returns>
    /// A Some option containing both values if both options were Some;
    /// otherwise, None.
    /// </returns>
    /// <remarks>
    /// This method short-circuits on the first None, similar to applicative behavior
    /// in functional programming.
    /// </remarks>
    /// <example>
    /// <code>
    /// var nameOption = GetName();
    /// var ageOption = GetAge();
    /// var combined = nameOption.Zip(ageOption);
    /// </code>
    /// </example>
    public static Option<(T1, T2)> Zip<T1, T2>(
        in this Option<T1> option1,
        in Option<T2> option2)
        where T1 : notnull
        where T2 : notnull
    {
        if (option1.IsNone || option2.IsNone)
            return Option<(T1, T2)>.None;
        
        return Option.Some((option1.Value, option2.Value));
    }

    /// <summary>
    /// Combines three options into a single option containing a tuple of all values.
    /// </summary>
    /// <typeparam name="T1">The type of the first value.</typeparam>
    /// <typeparam name="T2">The type of the second value.</typeparam>
    /// <typeparam name="T3">The type of the third value.</typeparam>
    /// <param name="option1">The first option.</param>
    /// <param name="option2">The second option.</param>
    /// <param name="option3">The third option.</param>
    /// <returns>
    /// A Some option containing all values if all options were Some;
    /// otherwise, None.
    /// </returns>
    public static Option<(T1, T2, T3)> Zip<T1, T2, T3>(
        in this Option<T1> option1,
        in Option<T2> option2,
        in Option<T3> option3)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
    {
        if (option1.IsNone || option2.IsNone || option3.IsNone)
            return Option<(T1, T2, T3)>.None;
        
        return Option.Some((option1.Value, option2.Value, option3.Value));
    }

    /// <summary>
    /// Flattens a nested option into a single option.
    /// </summary>
    /// <typeparam name="TValue">The type of the value in the inner option.</typeparam>
    /// <param name="option">The nested option to flatten.</param>
    /// <returns>
    /// The inner option if the outer option was Some;
    /// otherwise, None.
    /// </returns>
    /// <remarks>
    /// This method is useful when you have operations that return Option&lt;Option&lt;T&gt;&gt;
    /// and need to flatten them to Option&lt;T&gt;.
    /// </remarks>
    public static Option<TValue> Flatten<TValue>(
        in this Option<Option<TValue>> option)
        where TValue : notnull
    {
        if (option.IsNone)
            return Option<TValue>.None;
        
        return option.Value;
    }
}
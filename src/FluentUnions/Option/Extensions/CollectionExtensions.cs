namespace FluentUnions;

/// <summary>
/// Provides collection-related extension methods for working with <see cref="Option{T}"/> types.
/// </summary>
public static partial class OptionExtensions
{
    /// <summary>
    /// Returns an <see cref="Option{TValue}"/> containing the first element of the sequence,
    /// or <see cref="Option{TValue}.None"/> if the sequence contains no elements.
    /// </summary>
    /// <typeparam name="TValue">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">The sequence to return the first element from.</param>
    /// <returns>
    /// An <see cref="Option{TValue}"/> containing the first element if the sequence is not empty;
    /// otherwise, <see cref="Option{TValue}.None"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
    /// <remarks>
    /// This method is optimized for different collection types (arrays, lists, etc.) to provide
    /// the best performance. It avoids enumeration when possible by checking collection counts
    /// and accessing elements by index.
    /// </remarks>
    public static Option<TValue> FirstOrNone<TValue>(this IEnumerable<TValue> source)
        where TValue : notnull
    {
        if (source is IReadOnlyCollection<TValue> readOnlyCollection)
        {
            if (readOnlyCollection.Count == 0)
                return Option<TValue>.None;

            if (source is IReadOnlyList<TValue> readOnlyList)
                return Option.Some(readOnlyList[0]);
        }
        else if (source is ICollection<TValue> collection)
        {
            if (collection.Count == 0)
                return Option<TValue>.None;

            if (source is IList<TValue> list)
                return Option.Some(list[0]);
        }

        using var enumerator = source.GetEnumerator();
        return enumerator.MoveNext() 
            ? Option.Some(enumerator.Current) 
            : Option<TValue>.None;
    }
    
    /// <summary>
    /// Returns an <see cref="Option{TValue}"/> containing the first element of the sequence that satisfies a specified condition,
    /// or <see cref="Option{TValue}.None"/> if no such element is found.
    /// </summary>
    /// <typeparam name="TValue">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">The sequence to search for a matching element.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <returns>
    /// An <see cref="Option{TValue}"/> containing the first element that satisfies the condition;
    /// otherwise, <see cref="Option{TValue}.None"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="predicate"/> is null.</exception>
    /// <remarks>
    /// This method is optimized for different collection types. For empty collections, it returns None immediately.
    /// For lists and arrays, it uses indexed access for better performance than enumeration.
    /// </remarks>
    public static Option<TValue> FirstOrNone<TValue>(this IEnumerable<TValue> source, Func<TValue, bool> predicate)
        where TValue : notnull
    {
        // Fast path for empty collections
        if (source is IReadOnlyCollection<TValue> readOnlyCollection && readOnlyCollection.Count == 0)
        {
                return Option<TValue>.None;
        }
        else if (source is ICollection<TValue> collection && collection.Count == 0)
        {
            return Option<TValue>.None;
        }

        // Fast path for arrays and lists with predicate
        if (source is IReadOnlyList<TValue> readOnlyList)
        {
            for (var i = 0; i < readOnlyList.Count; i++)
            {
                if (predicate(readOnlyList[i])) return Option.Some(readOnlyList[i]);
            }

            return Option<TValue>.None;
        }

        if (source is IList<TValue> list)
        {
            for (var i = 0; i < list.Count; i++)
            {
                if (predicate(list[i])) return Option.Some(list[i]);
            }

            return Option<TValue>.None;
        }

        foreach (var item in source)
        {
            if (predicate(item)) return Option.Some(item);
        }

        return Option<TValue>.None;
    }
    
    /// <summary>
    /// Returns an <see cref="Option{TValue}"/> containing the last element of the sequence,
    /// or <see cref="Option{TValue}.None"/> if the sequence contains no elements.
    /// </summary>
    /// <typeparam name="TValue">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">The sequence to return the last element from.</param>
    /// <returns>
    /// An <see cref="Option{TValue}"/> containing the last element if the sequence is not empty;
    /// otherwise, <see cref="Option{TValue}.None"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
    /// <remarks>
    /// This method is optimized for collections that support indexed access (arrays, lists) by directly
    /// accessing the last element. For other sequences, it enumerates through all elements to find the last one.
    /// </remarks>
    public static Option<TValue> LastOrNone<TValue>(this IEnumerable<TValue> source)
        where TValue : notnull
    {
        if (source is IReadOnlyCollection<TValue> readOnlyCollection)
        {
            if (readOnlyCollection.Count == 0) return Option<TValue>.None;

            if (source is IReadOnlyList<TValue> readOnlyList) return Option.Some(readOnlyList[^1]);
        }
        else if (source is ICollection<TValue> collection)
        {
            if (collection.Count == 0) return Option<TValue>.None;

            if (source is IList<TValue> list) return Option.Some(list[^1]);
        }

        using var enumerator = source.GetEnumerator();
        Option<TValue> last = Option<TValue>.None;
        
        while (enumerator.MoveNext()) last = enumerator.Current;
        return last;
    }
    
    /// <summary>
    /// Returns an <see cref="Option{TValue}"/> containing the last element of the sequence that satisfies a specified condition,
    /// or <see cref="Option{TValue}.None"/> if no such element is found.
    /// </summary>
    /// <typeparam name="TValue">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">The sequence to search for a matching element.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <returns>
    /// An <see cref="Option{TValue}"/> containing the last element that satisfies the condition;
    /// otherwise, <see cref="Option{TValue}.None"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="predicate"/> is null.</exception>
    /// <remarks>
    /// This method is optimized for lists and arrays by iterating backwards through the collection.
    /// For other sequences, it must enumerate through all elements, keeping track of the last matching element.
    /// </remarks>
    public static Option<TValue> LastOrNone<TValue>(this IEnumerable<TValue> source, Func<TValue, bool> predicate)
        where TValue : notnull
    {
        // Fast path for empty collections
        if (source is IReadOnlyCollection<TValue> readOnlyCollection && readOnlyCollection.Count == 0)
        {
                return Option<TValue>.None;
        }
        else if (source is ICollection<TValue> collection && collection.Count == 0)
        {
            return Option<TValue>.None;
        }

        // Fast path for arrays and lists with predicate
        if (source is IReadOnlyList<TValue> readOnlyList)
        {
            for (var i = readOnlyList.Count - 1; i >= 0; i--)
            {
                if (predicate(readOnlyList[i])) return Option.Some(readOnlyList[i]);
            }

            return Option<TValue>.None;
        }

        if (source is IList<TValue> list)
        {
            for (var i = list.Count - 1; i >= 0; i--)
            {
                if (predicate(list[i])) return Option.Some(list[i]);
            }

            return Option<TValue>.None;
        }

        using var enumerator = source.GetEnumerator();
        Option<TValue> last = Option<TValue>.None;
        
        while (enumerator.MoveNext())
        {
            if (predicate(enumerator.Current)) last = enumerator.Current;
        }
        
        return last;
    }
}
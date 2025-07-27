namespace FluentUnions;

/// <summary>
/// Provides extension methods for working with collections of <see cref="Result{TValue}"/> instances.
/// </summary>
public static partial class ValueResultExtensions
{
    /// <summary>
    /// Transforms a collection of results into a result containing a collection of values.
    /// </summary>
    /// <typeparam name="TValue">The type of value in each result.</typeparam>
    /// <param name="results">The collection of results to sequence.</param>
    /// <returns>
    /// A successful result containing all values if all results were successful;
    /// otherwise, a failure result with the first error encountered.
    /// </returns>
    /// <remarks>
    /// This method short-circuits on the first failure, returning immediately without
    /// evaluating the remaining results. This is useful for validating a collection
    /// where all items must succeed.
    /// </remarks>
    /// <example>
    /// <code>
    /// var results = new[] { Result.Success(1), Result.Success(2), Result.Success(3) };
    /// var sequenced = results.Sequence(); // Result&lt;IEnumerable&lt;int&gt;&gt; containing [1, 2, 3]
    /// </code>
    /// </example>
    public static Result<IEnumerable<TValue>> Sequence<TValue>(
        this IEnumerable<Result<TValue>> results)
    {
        var values = new List<TValue>();
        
        foreach (var result in results)
        {
            if (result.IsFailure)
                return Result.Failure<IEnumerable<TValue>>(result.Error);
            
            values.Add(result.Value);
        }
        
        return Result.Success<IEnumerable<TValue>>(values);
    }

    /// <summary>
    /// Applies a function that returns a result to each element of a collection and sequences the results.
    /// </summary>
    /// <typeparam name="TSource">The type of elements in the source collection.</typeparam>
    /// <typeparam name="TResult">The type of value in the resulting results.</typeparam>
    /// <param name="source">The source collection to traverse.</param>
    /// <param name="selector">A function to apply to each element that returns a result.</param>
    /// <returns>
    /// A successful result containing all transformed values if all operations were successful;
    /// otherwise, a failure result with the first error encountered.
    /// </returns>
    /// <remarks>
    /// This method combines mapping and sequencing in a single operation. It's equivalent to
    /// calling Select followed by Sequence but more efficient.
    /// </remarks>
    /// <example>
    /// <code>
    /// var numbers = new[] { "1", "2", "3" };
    /// var parsed = numbers.Traverse(s => ParseInt(s)); // Result&lt;IEnumerable&lt;int&gt;&gt;
    /// </code>
    /// </example>
    public static Result<IEnumerable<TResult>> Traverse<TSource, TResult>(
        this IEnumerable<TSource> source,
        Func<TSource, Result<TResult>> selector)
    {
        var results = new List<TResult>();
        
        foreach (var item in source)
        {
            var result = selector(item);
            if (result.IsFailure)
                return Result.Failure<IEnumerable<TResult>>(result.Error);
            
            results.Add(result.Value);
        }
        
        return Result.Success<IEnumerable<TResult>>(results);
    }

    /// <summary>
    /// Collects all results from a collection, aggregating all errors if any failures occur.
    /// </summary>
    /// <typeparam name="TValue">The type of value in each result.</typeparam>
    /// <param name="results">The collection of results to collect.</param>
    /// <returns>
    /// A successful result containing all values if all results were successful;
    /// otherwise, a failure result with an AggregateError containing all errors encountered.
    /// </returns>
    /// <remarks>
    /// Unlike Sequence, this method evaluates all results and collects all errors
    /// before returning a failure. This is useful when you want to report all
    /// validation errors at once rather than stopping at the first error.
    /// </remarks>
    /// <example>
    /// <code>
    /// var results = new[] { 
    ///     Result.Failure&lt;int&gt;(error1), 
    ///     Result.Success(2), 
    ///     Result.Failure&lt;int&gt;(error2) 
    /// };
    /// var collected = results.CollectAll(); // Failure with AggregateError containing [error1, error2]
    /// </code>
    /// </example>
    public static Result<IEnumerable<TValue>> CollectAll<TValue>(
        this IEnumerable<Result<TValue>> results)
    {
        var values = new List<TValue>();
        var errors = new List<Error>();
        
        foreach (var result in results)
        {
            if (result.IsFailure)
                errors.Add(result.Error);
            else
                values.Add(result.Value);
        }
        
        if (errors.Count > 0)
        {
            var aggregateError = errors.Count == 1 
                ? errors[0] 
                : new AggregateError(errors.ToArray());
            return Result.Failure<IEnumerable<TValue>>(aggregateError);
        }
        
        return Result.Success<IEnumerable<TValue>>(values);
    }

    /// <summary>
    /// Partitions a collection of results into successful values and errors.
    /// </summary>
    /// <typeparam name="TValue">The type of value in each result.</typeparam>
    /// <param name="results">The collection of results to partition.</param>
    /// <returns>
    /// A tuple containing two collections: successful values and errors from failed results.
    /// </returns>
    /// <remarks>
    /// This method is useful when you want to process both successful and failed results
    /// differently, without short-circuiting on failures.
    /// </remarks>
    /// <example>
    /// <code>
    /// var results = GetResults();
    /// var (successes, failures) = results.Partition();
    /// Console.WriteLine($"Processed {successes.Count()} items successfully");
    /// Console.WriteLine($"Failed to process {failures.Count()} items");
    /// </code>
    /// </example>
    public static (IEnumerable<TValue> Successes, IEnumerable<Error> Failures) Partition<TValue>(
        this IEnumerable<Result<TValue>> results)
    {
        var successes = new List<TValue>();
        var failures = new List<Error>();
        
        foreach (var result in results)
        {
            if (result.IsSuccess)
                successes.Add(result.Value);
            else
                failures.Add(result.Error);
        }
        
        return (successes, failures);
    }

    /// <summary>
    /// Filters a collection to only the successful results, extracting their values.
    /// </summary>
    /// <typeparam name="TValue">The type of value in each result.</typeparam>
    /// <param name="results">The collection of results to filter.</param>
    /// <returns>
    /// A collection containing only the values from successful results.
    /// </returns>
    /// <remarks>
    /// This method silently ignores failed results. Use Partition if you need
    /// to track which results failed.
    /// </remarks>
    public static IEnumerable<TValue> ChooseSuccesses<TValue>(
        this IEnumerable<Result<TValue>> results)
    {
        foreach (var result in results)
        {
            if (result.IsSuccess)
                yield return result.Value;
        }
    }

    /// <summary>
    /// Filters a collection to only the failed results, extracting their errors.
    /// </summary>
    /// <typeparam name="TValue">The type of value in each result.</typeparam>
    /// <param name="results">The collection of results to filter.</param>
    /// <returns>
    /// A collection containing only the errors from failed results.
    /// </returns>
    /// <remarks>
    /// This method is useful for error reporting and logging scenarios where
    /// you need to collect all errors from a batch operation.
    /// </remarks>
    public static IEnumerable<Error> ChooseFailures<TValue>(
        this IEnumerable<Result<TValue>> results)
    {
        foreach (var result in results)
        {
            if (result.IsFailure)
                yield return result.Error;
        }
    }
}
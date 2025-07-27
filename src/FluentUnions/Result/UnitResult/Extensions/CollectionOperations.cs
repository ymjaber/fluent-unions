namespace FluentUnions;

/// <summary>
/// Provides collection operations for working with collections of <see cref="Result"/> instances.
/// </summary>
public static partial class UnitResultExtensions
{
    /// <summary>
    /// Combines multiple unit results into a single result.
    /// </summary>
    /// <param name="results">The collection of results to sequence.</param>
    /// <returns>
    /// A successful result if all results were successful;
    /// otherwise, a failure result with the first error encountered.
    /// </returns>
    /// <remarks>
    /// This method short-circuits on the first failure, returning immediately without
    /// evaluating the remaining results. This is useful for ensuring all operations
    /// in a sequence have succeeded.
    /// </remarks>
    /// <example>
    /// <code>
    /// var results = new[] { ValidateA(), ValidateB(), ValidateC() };
    /// var combined = results.Sequence(); // Success if all validations pass
    /// </code>
    /// </example>
    public static Result Sequence(this IEnumerable<Result> results)
    {
        foreach (var result in results)
        {
            if (result.IsFailure)
                return result;
        }

        return Result.Success();
    }

    /// <summary>
    /// Applies a function that returns a result to each element of a collection and sequences the results.
    /// </summary>
    /// <typeparam name="TSource">The type of elements in the source collection.</typeparam>
    /// <param name="source">The source collection to traverse.</param>
    /// <param name="operation">A function to apply to each element that returns a result.</param>
    /// <returns>
    /// A successful result if all operations were successful;
    /// otherwise, a failure result with the first error encountered.
    /// </returns>
    /// <remarks>
    /// This method combines mapping and sequencing in a single operation. It's useful
    /// for applying the same validation or operation to multiple items.
    /// </remarks>
    /// <example>
    /// <code>
    /// var files = new[] { "file1.txt", "file2.txt", "file3.txt" };
    /// var result = files.Traverse(file => ValidateFile(file)); // Unit Result
    /// </code>
    /// </example>
    public static Result Traverse<TSource>(
        this IEnumerable<TSource> source,
        Func<TSource, Result> operation)
    {
        foreach (var item in source)
        {
            var result = operation(item);
            if (result.IsFailure)
                return result;
        }

        return Result.Success();
    }

    /// <summary>
    /// Collects all results from a collection, aggregating all errors if any failures occur.
    /// </summary>
    /// <param name="results">The collection of results to collect.</param>
    /// <returns>
    /// A successful result if all results were successful;
    /// otherwise, a failure result with an AggregateError containing all errors encountered.
    /// </returns>
    /// <remarks>
    /// Unlike Sequence, this method evaluates all results and collects all errors
    /// before returning a failure. This is useful when you want to report all
    /// validation errors at once rather than stopping at the first error.
    /// </remarks>
    /// <example>
    /// <code>
    /// var validations = new[] { 
    ///     ValidateField1(), 
    ///     ValidateField2(), 
    ///     ValidateField3() 
    /// };
    /// var result = validations.CollectAll(); // Collects all validation errors
    /// </code>
    /// </example>
    public static Result CollectAll(this IEnumerable<Result> results)
    {
        var errors = new List<Error>();

        foreach (var result in results)
        {
            if (result.IsFailure)
                errors.Add(result.Error);
        }

        if (errors.Count > 0)
        {
            var aggregateError = errors.Count == 1
                ? errors[0]
                : new AggregateError(errors.ToArray());
            return Result.Failure(aggregateError);
        }

        return Result.Success();
    }

    /// <summary>
    /// Partitions a collection of results into successful and failed counts.
    /// </summary>
    /// <param name="results">The collection of results to partition.</param>
    /// <returns>
    /// A tuple containing the count of successful results and a collection of errors from failed results.
    /// </returns>
    /// <remarks>
    /// This method is useful when you want to get statistics about a batch operation
    /// and handle failures separately.
    /// </remarks>
    /// <example>
    /// <code>
    /// var results = ProcessBatch(items);
    /// var (successCount, errors) = results.Partition();
    /// Console.WriteLine($"Processed {successCount} items successfully");
    /// Console.WriteLine($"Failed to process {errors.Count()} items");
    /// </code>
    /// </example>
    public static (int SuccessCount, IEnumerable<Error> Errors) Partition(
        this IEnumerable<Result> results)
    {
        var successCount = 0;
        var errors = new List<Error>();

        foreach (var result in results)
        {
            if (result.IsSuccess)
                successCount++;
            else
                errors.Add(result.Error);
        }

        return (successCount, errors);
    }

    /// <summary>
    /// Counts the number of successful results in a collection.
    /// </summary>
    /// <param name="results">The collection of results to count.</param>
    /// <returns>The number of successful results.</returns>
    /// <remarks>
    /// This is a convenience method for quickly determining how many operations succeeded.
    /// </remarks>
    public static int CountSuccesses(this IEnumerable<Result> results)
    {
        return results.Count(r => r.IsSuccess);
    }

    /// <summary>
    /// Counts the number of failed results in a collection.
    /// </summary>
    /// <param name="results">The collection of results to count.</param>
    /// <returns>The number of failed results.</returns>
    /// <remarks>
    /// This is a convenience method for quickly determining how many operations failed.
    /// </remarks>
    public static int CountFailures(this IEnumerable<Result> results)
    {
        return results.Count(r => r.IsFailure);
    }

    /// <summary>
    /// Extracts all errors from a collection of results.
    /// </summary>
    /// <param name="results">The collection of results.</param>
    /// <returns>A collection containing only the errors from failed results.</returns>
    /// <remarks>
    /// This method is useful for error reporting and logging scenarios where
    /// you need to collect all errors from a batch operation.
    /// </remarks>
    public static IEnumerable<Error> ExtractErrors(this IEnumerable<Result> results)
    {
        foreach (var result in results)
        {
            if (result.IsFailure)
                yield return result.Error;
        }
    }

    /// <summary>
    /// Combines two unit results into a single result.
    /// </summary>
    /// <param name="result1">The first result.</param>
    /// <param name="result2">The second result.</param>
    /// <returns>
    /// A successful result if both results were successful;
    /// otherwise, a failure result with the first error encountered.
    /// </returns>
    /// <remarks>
    /// This method short-circuits on the first failure.
    /// </remarks>
    /// <example>
    /// <code>
    /// var permissionCheck = CheckPermissions();
    /// var quotaCheck = CheckQuota();
    /// var combined = permissionCheck.Combine(quotaCheck);
    /// </code>
    /// </example>
    public static Result Combine(
        in this Result result1,
        in Result result2)
    {
        if (result1.IsFailure)
            return result1;

        if (result2.IsFailure)
            return result2;

        return Result.Success();
    }

    /// <summary>
    /// Combines three unit results into a single result.
    /// </summary>
    /// <param name="result1">The first result.</param>
    /// <param name="result2">The second result.</param>
    /// <param name="result3">The third result.</param>
    /// <returns>
    /// A successful result if all results were successful;
    /// otherwise, a failure result with the first error encountered.
    /// </returns>
    public static Result Combine(
        in this Result result1,
        in Result result2,
        in Result result3)
    {
        if (result1.IsFailure)
            return result1;

        if (result2.IsFailure)
            return result2;

        if (result3.IsFailure)
            return result3;

        return Result.Success();
    }

    /// <summary>
    /// Combines multiple unit results into a single result.
    /// </summary>
    /// <param name="results">The results to combine.</param>
    /// <returns>
    /// A successful result if all results were successful;
    /// otherwise, a failure result with the first error encountered.
    /// </returns>
    /// <remarks>
    /// This is a convenience method for combining many results at once.
    /// </remarks>
    public static Result Combine(params Result[] results)
    {
        return results.Sequence();
    }
}

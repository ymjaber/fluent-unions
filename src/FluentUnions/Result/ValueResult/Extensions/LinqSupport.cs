namespace FluentUnions;

/// <summary>
/// Provides LINQ query syntax support for <see cref="Result{TValue}"/> by implementing
/// Select and SelectMany extension methods.
/// </summary>
public static partial class ValueResultExtensions
{
    /// <summary>
    /// Projects the value of a successful result into a new form using LINQ query syntax.
    /// </summary>
    /// <typeparam name="TSource">The type of the value in the source result.</typeparam>
    /// <typeparam name="TResult">The type of the value in the resulting result.</typeparam>
    /// <param name="result">The result to project.</param>
    /// <param name="selector">A transform function to apply to the value.</param>
    /// <returns>
    /// A <see cref="Result{TResult}"/> whose value is the result of invoking the transform function
    /// on the source result's value if it was successful; otherwise, a failure result with the original error.
    /// </returns>
    /// <remarks>
    /// This method enables LINQ query syntax for Result types. It is an alias for the Map method
    /// and allows writing queries like:
    /// <code>
    /// var result = from x in GetResult()
    ///              select x * 2;
    /// </code>
    /// </remarks>
    public static Result<TResult> Select<TSource, TResult>(
        in this Result<TSource> result,
        Func<TSource, TResult> selector)
    {
        return result.Map(selector);
    }

    /// <summary>
    /// Projects the value of a successful result to a new result and flattens the result using LINQ query syntax.
    /// </summary>
    /// <typeparam name="TSource">The type of the value in the source result.</typeparam>
    /// <typeparam name="TResult">The type of the value in the resulting result.</typeparam>
    /// <param name="result">The result to project.</param>
    /// <param name="selector">A transform function to apply to the value that returns a new result.</param>
    /// <returns>
    /// The result of invoking the transform function on the source result's value if it was successful;
    /// otherwise, a failure result with the original error.
    /// </returns>
    /// <remarks>
    /// This method is an alias for the Bind method to support LINQ query syntax.
    /// It enables writing queries with multiple from clauses.
    /// </remarks>
    public static Result<TResult> SelectMany<TSource, TResult>(
        in this Result<TSource> result,
        Func<TSource, Result<TResult>> selector)
    {
        return result.Bind(selector);
    }

    /// <summary>
    /// Projects the value of a successful result to a new result, flattens the result,
    /// and invokes a result selector function on the pair of values.
    /// </summary>
    /// <typeparam name="TSource">The type of the value in the source result.</typeparam>
    /// <typeparam name="TCollection">The type of the intermediate value.</typeparam>
    /// <typeparam name="TResult">The type of the value in the resulting result.</typeparam>
    /// <param name="result">The result to project.</param>
    /// <param name="collectionSelector">A transform function to apply to the value that returns an intermediate result.</param>
    /// <param name="resultSelector">A transform function to apply to the pair of values.</param>
    /// <returns>
    /// A <see cref="Result{TResult}"/> that is the result of invoking the transform functions
    /// if all results were successful; otherwise, a failure result with the first error encountered.
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
    public static Result<TResult> SelectMany<TSource, TCollection, TResult>(
        in this Result<TSource> result,
        Func<TSource, Result<TCollection>> collectionSelector,
        Func<TSource, TCollection, TResult> resultSelector)
    {
        if (result.IsFailure) 
            return Result.Failure<TResult>(result.Error);

        var collectionResult = collectionSelector(result.Value);
        if (collectionResult.IsFailure) 
            return Result.Failure<TResult>(collectionResult.Error);

        return Result.Success(resultSelector(result.Value, collectionResult.Value));
    }

    /// <summary>
    /// Filters a result based on a predicate.
    /// </summary>
    /// <typeparam name="TValue">The type of the value in the result.</typeparam>
    /// <param name="result">The result to filter.</param>
    /// <param name="predicate">A function to test the value.</param>
    /// <param name="error">The error to return if the predicate fails. If null, a default validation error is used.</param>
    /// <returns>
    /// The original result if it was already a failure or if the predicate returns true;
    /// otherwise, a failure result with the specified error.
    /// </returns>
    /// <remarks>
    /// This method enables where clauses in LINQ queries:
    /// <code>
    /// var result = from x in GetResult()
    ///              where x > 0
    ///              select x;
    /// </code>
    /// Note: The error parameter is not available in LINQ query syntax and will use a default error.
    /// </remarks>
    public static Result<TValue> Where<TValue>(
        in this Result<TValue> result,
        Func<TValue, bool> predicate,
        Error? error = null)
    {
        if (result.IsFailure) 
            return result;

        if (!predicate(result.Value))
        {
            var filterError = error ?? new ValidationError(
                "Error.PredicateFailed",
                "The value did not satisfy the specified condition.");
            return Result.Failure<TValue>(filterError);
        }

        return result;
    }
}
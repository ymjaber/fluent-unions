namespace FluentUnions;

/// <summary>
/// Provides LINQ query syntax support for <see cref="Result"/> by implementing
/// SelectMany extension methods.
/// </summary>
public static partial class UnitResultExtensions
{
    /// <summary>
    /// Enables LINQ query syntax for chaining unit result operations.
    /// </summary>
    /// <param name="result">The result to chain from.</param>
    /// <param name="selector">A function that returns another unit result.</param>
    /// <returns>
    /// The result of the selector if the source result was successful;
    /// otherwise, the original failure result.
    /// </returns>
    /// <remarks>
    /// This method is an alias for the Bind method to support LINQ query syntax.
    /// It enables writing queries with multiple from clauses for unit results.
    /// Note that Select is not implemented for unit Result as there's no value to transform.
    /// </remarks>
    public static Result SelectMany(
        in this Result result,
        Func<Result> selector)
    {
        return result.Bind(selector);
    }

    /// <summary>
    /// Enables LINQ query syntax for chaining from unit result to value result operations.
    /// </summary>
    /// <typeparam name="TResult">The type of value in the resulting result.</typeparam>
    /// <param name="result">The unit result to chain from.</param>
    /// <param name="selector">A function that returns a value result.</param>
    /// <returns>
    /// The result of the selector if the source result was successful;
    /// otherwise, a failure result with the original error.
    /// </returns>
    /// <remarks>
    /// This overload allows transitioning from unit results to value results in LINQ queries.
    /// </remarks>
    public static Result<TResult> SelectMany<TResult>(
        in this Result result,
        Func<Result<TResult>> selector)
    {
        return result.Bind(selector);
    }

    /// <summary>
    /// Enables LINQ query syntax for chaining unit result operations with projection.
    /// </summary>
    /// <typeparam name="TResult">The type of the final result.</typeparam>
    /// <param name="result">The unit result to chain from.</param>
    /// <param name="collectionSelector">A function that returns another unit result.</param>
    /// <param name="resultSelector">A function to create the final result.</param>
    /// <returns>
    /// A value result containing the result of the resultSelector if both operations were successful;
    /// otherwise, a failure result with the first error encountered.
    /// </returns>
    /// <remarks>
    /// This overload enables full LINQ query syntax with select clauses.
    /// Since unit results have no value, the resultSelector takes no parameters.
    /// <code>
    /// var result = from _ in ValidatePermissions()
    ///              from __ in CheckQuota()
    ///              select "Operations completed successfully";
    /// </code>
    /// </remarks>
    public static Result<TResult> SelectMany<TResult>(
        in this Result result,
        Func<Result> collectionSelector,
        Func<TResult> resultSelector)
    {
        if (result.IsFailure) 
            return Result.Failure<TResult>(result.Error);

        var collectionResult = collectionSelector();
        if (collectionResult.IsFailure) 
            return Result.Failure<TResult>(collectionResult.Error);

        return Result.Success(resultSelector());
    }

    /// <summary>
    /// Enables LINQ query syntax for transitioning from unit result to value result with projection.
    /// </summary>
    /// <typeparam name="TCollection">The type of value in the intermediate result.</typeparam>
    /// <typeparam name="TResult">The type of the final result.</typeparam>
    /// <param name="result">The unit result to chain from.</param>
    /// <param name="collectionSelector">A function that returns a value result.</param>
    /// <param name="resultSelector">A function to create the final result from the intermediate value.</param>
    /// <returns>
    /// A value result containing the result of the resultSelector if both operations were successful;
    /// otherwise, a failure result with the first error encountered.
    /// </returns>
    /// <remarks>
    /// This overload is useful when you need to transition from a unit result to a value result
    /// and then transform that value:
    /// <code>
    /// var result = from _ in ValidateInput()
    ///              from data in LoadData()
    ///              select data.Transform();
    /// </code>
    /// </remarks>
    public static Result<TResult> SelectMany<TCollection, TResult>(
        in this Result result,
        Func<Result<TCollection>> collectionSelector,
        Func<TCollection, TResult> resultSelector)
    {
        if (result.IsFailure) 
            return Result.Failure<TResult>(result.Error);

        var collectionResult = collectionSelector();
        if (collectionResult.IsFailure) 
            return Result.Failure<TResult>(collectionResult.Error);

        return Result.Success(resultSelector(collectionResult.Value));
    }

    /// <summary>
    /// Filters a unit result based on a predicate.
    /// </summary>
    /// <param name="result">The result to filter.</param>
    /// <param name="predicate">A function to test whether the operation should continue.</param>
    /// <param name="error">The error to return if the predicate fails. If null, a default validation error is used.</param>
    /// <returns>
    /// The original result if it was already a failure or if the predicate returns true;
    /// otherwise, a failure result with the specified error.
    /// </returns>
    /// <remarks>
    /// This method enables where clauses in LINQ queries for unit results:
    /// <code>
    /// var result = from _ in CheckPermissions()
    ///              where IsBusinessHours()
    ///              from __ in PerformOperation()
    ///              select Unit.Value;
    /// </code>
    /// Note: The error parameter is not available in LINQ query syntax and will use a default error.
    /// </remarks>
    public static Result Where(
        in this Result result,
        Func<bool> predicate,
        Error? error = null)
    {
        if (result.IsFailure) 
            return result;

        if (!predicate())
        {
            var filterError = error ?? new ValidationError(
                "Error.PredicateFailed",
                "The condition was not satisfied.");
            return Result.Failure(filterError);
        }

        return result;
    }
}
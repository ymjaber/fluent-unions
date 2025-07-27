namespace FluentUnions;

public static partial class ValueResultExtensions
{
    /// <summary>
    /// Validates that a successful <see cref="Result{T}"/> containing a tuple with 2 elements satisfies a predicate, converting to failure if not.
    /// </summary>
    /// <typeparam name="TValue1">The type of the first tuple element.</typeparam>
    /// <typeparam name="TValue2">The type of the second tuple element.</typeparam>
    /// <param name="result">The source <see cref="Result{T}"/> containing a tuple to validate.</param>
    /// <param name="predicate">A function that validates the tuple elements, returning true if valid.</param>
    /// <param name="error">The error to return if the predicate returns false.</param>
    /// <returns>The original Result if already failed or if the predicate returns true; otherwise, a failure Result with the specified error.</returns>
    /// <remarks>
    /// Ensure is a validation operation in the railway-oriented programming pattern. It adds a guard clause
    /// to the success track, allowing you to validate business rules and constraints on successful values.
    /// If the Result is already a failure, the validation is skipped and the existing error is propagated.
    /// This differs from Option's Filter operation as it explicitly provides error information when validation fails,
    /// maintaining the rich error context that Result types provide.
    /// </remarks>
    public static Result<(TValue1, TValue2)> Ensure<TValue1, TValue2>(
        in this Result<(TValue1, TValue2)> result,
        Func<TValue1, TValue2, bool> predicate,
        Error error)
    {
        if (result.IsFailure) return result;
    
        return predicate(result.Value.Item1, result.Value.Item2) ? result : error;
    }

    /// <summary>
    /// Validates that a successful <see cref="Result{T}"/> containing a tuple with 3 elements satisfies a predicate, converting to failure if not.
    /// </summary>
    /// <typeparam name="TValue1">The type of the first tuple element.</typeparam>
    /// <typeparam name="TValue2">The type of the second tuple element.</typeparam>
    /// <typeparam name="TValue3">The type of the third tuple element.</typeparam>
    /// <param name="result">The source <see cref="Result{T}"/> containing a tuple to validate.</param>
    /// <param name="predicate">A function that validates the tuple elements, returning true if valid.</param>
    /// <param name="error">The error to return if the predicate returns false.</param>
    /// <returns>The original Result if already failed or if the predicate returns true; otherwise, a failure Result with the specified error.</returns>
    /// <remarks>
    /// Ensure is a validation operation in the railway-oriented programming pattern. It adds a guard clause
    /// to the success track, allowing you to validate business rules and constraints on successful values.
    /// If the Result is already a failure, the validation is skipped and the existing error is propagated.
    /// This differs from Option's Filter operation as it explicitly provides error information when validation fails,
    /// maintaining the rich error context that Result types provide.
    /// </remarks>
    public static Result<(TValue1, TValue2, TValue3)> Ensure<TValue1, TValue2, TValue3>(
        in this Result<(TValue1, TValue2, TValue3)> result,
        Func<TValue1, TValue2, TValue3, bool> predicate,
        Error error)
    {
        if (result.IsFailure) return result;
    
        return predicate(result.Value.Item1, result.Value.Item2, result.Value.Item3) ? result : error;
    }

    /// <summary>
    /// Validates that a successful <see cref="Result{T}"/> containing a tuple with 4 elements satisfies a predicate, converting to failure if not.
    /// </summary>
    /// <typeparam name="TValue1">The type of the first tuple element.</typeparam>
    /// <typeparam name="TValue2">The type of the second tuple element.</typeparam>
    /// <typeparam name="TValue3">The type of the third tuple element.</typeparam>
    /// <typeparam name="TValue4">The type of the fourth tuple element.</typeparam>
    /// <param name="result">The source <see cref="Result{T}"/> containing a tuple to validate.</param>
    /// <param name="predicate">A function that validates the tuple elements, returning true if valid.</param>
    /// <param name="error">The error to return if the predicate returns false.</param>
    /// <returns>The original Result if already failed or if the predicate returns true; otherwise, a failure Result with the specified error.</returns>
    /// <remarks>
    /// Ensure is a validation operation in the railway-oriented programming pattern. It adds a guard clause
    /// to the success track, allowing you to validate business rules and constraints on successful values.
    /// If the Result is already a failure, the validation is skipped and the existing error is propagated.
    /// This differs from Option's Filter operation as it explicitly provides error information when validation fails,
    /// maintaining the rich error context that Result types provide.
    /// </remarks>
    public static Result<(TValue1, TValue2, TValue3, TValue4)> Ensure<TValue1, TValue2, TValue3, TValue4>(
        in this Result<(TValue1, TValue2, TValue3, TValue4)> result,
        Func<TValue1, TValue2, TValue3, TValue4, bool> predicate,
        Error error)
    {
        if (result.IsFailure) return result;
    
        return predicate(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4) ? result : error;
    }

    /// <summary>
    /// Validates that a successful <see cref="Result{T}"/> containing a tuple with 5 elements satisfies a predicate, converting to failure if not.
    /// </summary>
    /// <typeparam name="TValue1">The type of the first tuple element.</typeparam>
    /// <typeparam name="TValue2">The type of the second tuple element.</typeparam>
    /// <typeparam name="TValue3">The type of the third tuple element.</typeparam>
    /// <typeparam name="TValue4">The type of the fourth tuple element.</typeparam>
    /// <typeparam name="TValue5">The type of the fifth tuple element.</typeparam>
    /// <param name="result">The source <see cref="Result{T}"/> containing a tuple to validate.</param>
    /// <param name="predicate">A function that validates the tuple elements, returning true if valid.</param>
    /// <param name="error">The error to return if the predicate returns false.</param>
    /// <returns>The original Result if already failed or if the predicate returns true; otherwise, a failure Result with the specified error.</returns>
    /// <remarks>
    /// Ensure is a validation operation in the railway-oriented programming pattern. It adds a guard clause
    /// to the success track, allowing you to validate business rules and constraints on successful values.
    /// If the Result is already a failure, the validation is skipped and the existing error is propagated.
    /// This differs from Option's Filter operation as it explicitly provides error information when validation fails,
    /// maintaining the rich error context that Result types provide.
    /// </remarks>
    public static Result<(TValue1, TValue2, TValue3, TValue4, TValue5)> Ensure<TValue1, TValue2, TValue3, TValue4, TValue5>(
        in this Result<(TValue1, TValue2, TValue3, TValue4, TValue5)> result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, bool> predicate,
        Error error)
    {
        if (result.IsFailure) return result;
    
        return predicate(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5) ? result : error;
    }

    /// <summary>
    /// Validates that a successful <see cref="Result{T}"/> containing a tuple with 6 elements satisfies a predicate, converting to failure if not.
    /// </summary>
    /// <typeparam name="TValue1">The type of the first tuple element.</typeparam>
    /// <typeparam name="TValue2">The type of the second tuple element.</typeparam>
    /// <typeparam name="TValue3">The type of the third tuple element.</typeparam>
    /// <typeparam name="TValue4">The type of the fourth tuple element.</typeparam>
    /// <typeparam name="TValue5">The type of the fifth tuple element.</typeparam>
    /// <typeparam name="TValue6">The type of the sixth tuple element.</typeparam>
    /// <param name="result">The source <see cref="Result{T}"/> containing a tuple to validate.</param>
    /// <param name="predicate">A function that validates the tuple elements, returning true if valid.</param>
    /// <param name="error">The error to return if the predicate returns false.</param>
    /// <returns>The original Result if already failed or if the predicate returns true; otherwise, a failure Result with the specified error.</returns>
    /// <remarks>
    /// Ensure is a validation operation in the railway-oriented programming pattern. It adds a guard clause
    /// to the success track, allowing you to validate business rules and constraints on successful values.
    /// If the Result is already a failure, the validation is skipped and the existing error is propagated.
    /// This differs from Option's Filter operation as it explicitly provides error information when validation fails,
    /// maintaining the rich error context that Result types provide.
    /// </remarks>
    public static Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)> Ensure<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(
        in this Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)> result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, bool> predicate,
        Error error)
    {
        if (result.IsFailure) return result;
    
        return predicate(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, result.Value.Item6) ? result : error;
    }

    /// <summary>
    /// Validates that a successful <see cref="Result{T}"/> containing a tuple with 7 elements satisfies a predicate, converting to failure if not.
    /// </summary>
    /// <typeparam name="TValue1">The type of the first tuple element.</typeparam>
    /// <typeparam name="TValue2">The type of the second tuple element.</typeparam>
    /// <typeparam name="TValue3">The type of the third tuple element.</typeparam>
    /// <typeparam name="TValue4">The type of the fourth tuple element.</typeparam>
    /// <typeparam name="TValue5">The type of the fifth tuple element.</typeparam>
    /// <typeparam name="TValue6">The type of the sixth tuple element.</typeparam>
    /// <typeparam name="TValue7">The type of the seventh tuple element.</typeparam>
    /// <param name="result">The source <see cref="Result{T}"/> containing a tuple to validate.</param>
    /// <param name="predicate">A function that validates the tuple elements, returning true if valid.</param>
    /// <param name="error">The error to return if the predicate returns false.</param>
    /// <returns>The original Result if already failed or if the predicate returns true; otherwise, a failure Result with the specified error.</returns>
    /// <remarks>
    /// Ensure is a validation operation in the railway-oriented programming pattern. It adds a guard clause
    /// to the success track, allowing you to validate business rules and constraints on successful values.
    /// If the Result is already a failure, the validation is skipped and the existing error is propagated.
    /// This differs from Option's Filter operation as it explicitly provides error information when validation fails,
    /// maintaining the rich error context that Result types provide.
    /// </remarks>
    public static Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)> Ensure<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(
        in this Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)> result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, bool> predicate,
        Error error)
    {
        if (result.IsFailure) return result;
    
        return predicate(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, result.Value.Item6, result.Value.Item7) ? result : error;
    }

    /// <summary>
    /// Validates that a successful <see cref="Result{T}"/> containing a tuple with 8 elements satisfies a predicate, converting to failure if not.
    /// </summary>
    /// <typeparam name="TValue1">The type of the first tuple element.</typeparam>
    /// <typeparam name="TValue2">The type of the second tuple element.</typeparam>
    /// <typeparam name="TValue3">The type of the third tuple element.</typeparam>
    /// <typeparam name="TValue4">The type of the fourth tuple element.</typeparam>
    /// <typeparam name="TValue5">The type of the fifth tuple element.</typeparam>
    /// <typeparam name="TValue6">The type of the sixth tuple element.</typeparam>
    /// <typeparam name="TValue7">The type of the seventh tuple element.</typeparam>
    /// <typeparam name="TValue8">The type of the eighth tuple element.</typeparam>
    /// <param name="result">The source <see cref="Result{T}"/> containing a tuple to validate.</param>
    /// <param name="predicate">A function that validates the tuple elements, returning true if valid.</param>
    /// <param name="error">The error to return if the predicate returns false.</param>
    /// <returns>The original Result if already failed or if the predicate returns true; otherwise, a failure Result with the specified error.</returns>
    /// <remarks>
    /// Ensure is a validation operation in the railway-oriented programming pattern. It adds a guard clause
    /// to the success track, allowing you to validate business rules and constraints on successful values.
    /// If the Result is already a failure, the validation is skipped and the existing error is propagated.
    /// This differs from Option's Filter operation as it explicitly provides error information when validation fails,
    /// maintaining the rich error context that Result types provide.
    /// </remarks>
    public static Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)> Ensure<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(
        in this Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)> result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, bool> predicate,
        Error error)
    {
        if (result.IsFailure) return result;
    
        return predicate(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, result.Value.Item6, result.Value.Item7, result.Value.Item8) ? result : error;
    }

}
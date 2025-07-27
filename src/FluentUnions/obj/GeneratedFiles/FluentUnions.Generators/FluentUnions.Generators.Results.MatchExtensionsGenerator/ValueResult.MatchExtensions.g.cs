namespace FluentUnions;

public static partial class ValueResultExtensions
{
    /// <summary>
    /// Pattern matches on a <see cref="Result{T}"/> containing a tuple with 2 elements, executing different functions based on success or failure.
    /// </summary>
    /// <typeparam name="TSource1">The type of the first tuple element in the source Result.</typeparam>
    /// <typeparam name="TSource2">The type of the second tuple element in the source Result.</typeparam>
    /// <typeparam name="TTarget">The type of the value returned by the match operation.</typeparam>
    /// <param name="result">The source <see cref="Result{T}"/> containing a tuple to match on.</param>
    /// <param name="success">The function to execute if the Result is successful, receiving the tuple elements as separate parameters.</param>
    /// <param name="failure">The function to execute if the Result is a failure, receiving the error.</param>
    /// <returns>The value returned by either the success or failure function.</returns>
    /// <remarks>
    /// The Match operation provides exhaustive pattern matching for Result types, ensuring both success and failure
    /// cases are handled. This is a key pattern in railway-oriented programming, forcing explicit handling of both
    /// the happy path (success) and error cases. Unlike Option's Match which handles Some/None, Result's Match
    /// handles Success/Failure with explicit error information in the failure case.
    /// </remarks>
    public static TTarget Match<TSource1, TSource2, TTarget>(
        in this Result<(TSource1, TSource2)> result,
        Func<TSource1, TSource2, TTarget> success,
        Func<Error, TTarget> failure)
    {
        return result.IsSuccess ? success(result.Value.Item1, result.Value.Item2) : failure(result.Error);
    }

    /// <summary>
    /// Pattern matches on a <see cref="Result{T}"/> containing a tuple with 3 elements, executing different functions based on success or failure.
    /// </summary>
    /// <typeparam name="TSource1">The type of the first tuple element in the source Result.</typeparam>
    /// <typeparam name="TSource2">The type of the second tuple element in the source Result.</typeparam>
    /// <typeparam name="TSource3">The type of the third tuple element in the source Result.</typeparam>
    /// <typeparam name="TTarget">The type of the value returned by the match operation.</typeparam>
    /// <param name="result">The source <see cref="Result{T}"/> containing a tuple to match on.</param>
    /// <param name="success">The function to execute if the Result is successful, receiving the tuple elements as separate parameters.</param>
    /// <param name="failure">The function to execute if the Result is a failure, receiving the error.</param>
    /// <returns>The value returned by either the success or failure function.</returns>
    /// <remarks>
    /// The Match operation provides exhaustive pattern matching for Result types, ensuring both success and failure
    /// cases are handled. This is a key pattern in railway-oriented programming, forcing explicit handling of both
    /// the happy path (success) and error cases. Unlike Option's Match which handles Some/None, Result's Match
    /// handles Success/Failure with explicit error information in the failure case.
    /// </remarks>
    public static TTarget Match<TSource1, TSource2, TSource3, TTarget>(
        in this Result<(TSource1, TSource2, TSource3)> result,
        Func<TSource1, TSource2, TSource3, TTarget> success,
        Func<Error, TTarget> failure)
    {
        return result.IsSuccess ? success(result.Value.Item1, result.Value.Item2, result.Value.Item3) : failure(result.Error);
    }

    /// <summary>
    /// Pattern matches on a <see cref="Result{T}"/> containing a tuple with 4 elements, executing different functions based on success or failure.
    /// </summary>
    /// <typeparam name="TSource1">The type of the first tuple element in the source Result.</typeparam>
    /// <typeparam name="TSource2">The type of the second tuple element in the source Result.</typeparam>
    /// <typeparam name="TSource3">The type of the third tuple element in the source Result.</typeparam>
    /// <typeparam name="TSource4">The type of the fourth tuple element in the source Result.</typeparam>
    /// <typeparam name="TTarget">The type of the value returned by the match operation.</typeparam>
    /// <param name="result">The source <see cref="Result{T}"/> containing a tuple to match on.</param>
    /// <param name="success">The function to execute if the Result is successful, receiving the tuple elements as separate parameters.</param>
    /// <param name="failure">The function to execute if the Result is a failure, receiving the error.</param>
    /// <returns>The value returned by either the success or failure function.</returns>
    /// <remarks>
    /// The Match operation provides exhaustive pattern matching for Result types, ensuring both success and failure
    /// cases are handled. This is a key pattern in railway-oriented programming, forcing explicit handling of both
    /// the happy path (success) and error cases. Unlike Option's Match which handles Some/None, Result's Match
    /// handles Success/Failure with explicit error information in the failure case.
    /// </remarks>
    public static TTarget Match<TSource1, TSource2, TSource3, TSource4, TTarget>(
        in this Result<(TSource1, TSource2, TSource3, TSource4)> result,
        Func<TSource1, TSource2, TSource3, TSource4, TTarget> success,
        Func<Error, TTarget> failure)
    {
        return result.IsSuccess ? success(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4) : failure(result.Error);
    }

    /// <summary>
    /// Pattern matches on a <see cref="Result{T}"/> containing a tuple with 5 elements, executing different functions based on success or failure.
    /// </summary>
    /// <typeparam name="TSource1">The type of the first tuple element in the source Result.</typeparam>
    /// <typeparam name="TSource2">The type of the second tuple element in the source Result.</typeparam>
    /// <typeparam name="TSource3">The type of the third tuple element in the source Result.</typeparam>
    /// <typeparam name="TSource4">The type of the fourth tuple element in the source Result.</typeparam>
    /// <typeparam name="TSource5">The type of the fifth tuple element in the source Result.</typeparam>
    /// <typeparam name="TTarget">The type of the value returned by the match operation.</typeparam>
    /// <param name="result">The source <see cref="Result{T}"/> containing a tuple to match on.</param>
    /// <param name="success">The function to execute if the Result is successful, receiving the tuple elements as separate parameters.</param>
    /// <param name="failure">The function to execute if the Result is a failure, receiving the error.</param>
    /// <returns>The value returned by either the success or failure function.</returns>
    /// <remarks>
    /// The Match operation provides exhaustive pattern matching for Result types, ensuring both success and failure
    /// cases are handled. This is a key pattern in railway-oriented programming, forcing explicit handling of both
    /// the happy path (success) and error cases. Unlike Option's Match which handles Some/None, Result's Match
    /// handles Success/Failure with explicit error information in the failure case.
    /// </remarks>
    public static TTarget Match<TSource1, TSource2, TSource3, TSource4, TSource5, TTarget>(
        in this Result<(TSource1, TSource2, TSource3, TSource4, TSource5)> result,
        Func<TSource1, TSource2, TSource3, TSource4, TSource5, TTarget> success,
        Func<Error, TTarget> failure)
    {
        return result.IsSuccess ? success(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5) : failure(result.Error);
    }

    /// <summary>
    /// Pattern matches on a <see cref="Result{T}"/> containing a tuple with 6 elements, executing different functions based on success or failure.
    /// </summary>
    /// <typeparam name="TSource1">The type of the first tuple element in the source Result.</typeparam>
    /// <typeparam name="TSource2">The type of the second tuple element in the source Result.</typeparam>
    /// <typeparam name="TSource3">The type of the third tuple element in the source Result.</typeparam>
    /// <typeparam name="TSource4">The type of the fourth tuple element in the source Result.</typeparam>
    /// <typeparam name="TSource5">The type of the fifth tuple element in the source Result.</typeparam>
    /// <typeparam name="TSource6">The type of the sixth tuple element in the source Result.</typeparam>
    /// <typeparam name="TTarget">The type of the value returned by the match operation.</typeparam>
    /// <param name="result">The source <see cref="Result{T}"/> containing a tuple to match on.</param>
    /// <param name="success">The function to execute if the Result is successful, receiving the tuple elements as separate parameters.</param>
    /// <param name="failure">The function to execute if the Result is a failure, receiving the error.</param>
    /// <returns>The value returned by either the success or failure function.</returns>
    /// <remarks>
    /// The Match operation provides exhaustive pattern matching for Result types, ensuring both success and failure
    /// cases are handled. This is a key pattern in railway-oriented programming, forcing explicit handling of both
    /// the happy path (success) and error cases. Unlike Option's Match which handles Some/None, Result's Match
    /// handles Success/Failure with explicit error information in the failure case.
    /// </remarks>
    public static TTarget Match<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TTarget>(
        in this Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6)> result,
        Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TTarget> success,
        Func<Error, TTarget> failure)
    {
        return result.IsSuccess ? success(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, result.Value.Item6) : failure(result.Error);
    }

    /// <summary>
    /// Pattern matches on a <see cref="Result{T}"/> containing a tuple with 7 elements, executing different functions based on success or failure.
    /// </summary>
    /// <typeparam name="TSource1">The type of the first tuple element in the source Result.</typeparam>
    /// <typeparam name="TSource2">The type of the second tuple element in the source Result.</typeparam>
    /// <typeparam name="TSource3">The type of the third tuple element in the source Result.</typeparam>
    /// <typeparam name="TSource4">The type of the fourth tuple element in the source Result.</typeparam>
    /// <typeparam name="TSource5">The type of the fifth tuple element in the source Result.</typeparam>
    /// <typeparam name="TSource6">The type of the sixth tuple element in the source Result.</typeparam>
    /// <typeparam name="TSource7">The type of the seventh tuple element in the source Result.</typeparam>
    /// <typeparam name="TTarget">The type of the value returned by the match operation.</typeparam>
    /// <param name="result">The source <see cref="Result{T}"/> containing a tuple to match on.</param>
    /// <param name="success">The function to execute if the Result is successful, receiving the tuple elements as separate parameters.</param>
    /// <param name="failure">The function to execute if the Result is a failure, receiving the error.</param>
    /// <returns>The value returned by either the success or failure function.</returns>
    /// <remarks>
    /// The Match operation provides exhaustive pattern matching for Result types, ensuring both success and failure
    /// cases are handled. This is a key pattern in railway-oriented programming, forcing explicit handling of both
    /// the happy path (success) and error cases. Unlike Option's Match which handles Some/None, Result's Match
    /// handles Success/Failure with explicit error information in the failure case.
    /// </remarks>
    public static TTarget Match<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TTarget>(
        in this Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7)> result,
        Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TTarget> success,
        Func<Error, TTarget> failure)
    {
        return result.IsSuccess ? success(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, result.Value.Item6, result.Value.Item7) : failure(result.Error);
    }

    /// <summary>
    /// Pattern matches on a <see cref="Result{T}"/> containing a tuple with 8 elements, executing different functions based on success or failure.
    /// </summary>
    /// <typeparam name="TSource1">The type of the first tuple element in the source Result.</typeparam>
    /// <typeparam name="TSource2">The type of the second tuple element in the source Result.</typeparam>
    /// <typeparam name="TSource3">The type of the third tuple element in the source Result.</typeparam>
    /// <typeparam name="TSource4">The type of the fourth tuple element in the source Result.</typeparam>
    /// <typeparam name="TSource5">The type of the fifth tuple element in the source Result.</typeparam>
    /// <typeparam name="TSource6">The type of the sixth tuple element in the source Result.</typeparam>
    /// <typeparam name="TSource7">The type of the seventh tuple element in the source Result.</typeparam>
    /// <typeparam name="TSource8">The type of the eighth tuple element in the source Result.</typeparam>
    /// <typeparam name="TTarget">The type of the value returned by the match operation.</typeparam>
    /// <param name="result">The source <see cref="Result{T}"/> containing a tuple to match on.</param>
    /// <param name="success">The function to execute if the Result is successful, receiving the tuple elements as separate parameters.</param>
    /// <param name="failure">The function to execute if the Result is a failure, receiving the error.</param>
    /// <returns>The value returned by either the success or failure function.</returns>
    /// <remarks>
    /// The Match operation provides exhaustive pattern matching for Result types, ensuring both success and failure
    /// cases are handled. This is a key pattern in railway-oriented programming, forcing explicit handling of both
    /// the happy path (success) and error cases. Unlike Option's Match which handles Some/None, Result's Match
    /// handles Success/Failure with explicit error information in the failure case.
    /// </remarks>
    public static TTarget Match<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TTarget>(
        in this Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8)> result,
        Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TTarget> success,
        Func<Error, TTarget> failure)
    {
        return result.IsSuccess ? success(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, result.Value.Item6, result.Value.Item7, result.Value.Item8) : failure(result.Error);
    }

}
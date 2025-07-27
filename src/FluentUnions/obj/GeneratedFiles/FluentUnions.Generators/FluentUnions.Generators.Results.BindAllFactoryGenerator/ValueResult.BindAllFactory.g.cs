namespace FluentUnions;

public readonly partial struct Result
{
    /// <summary>
    /// Combines 2 Result values into a single Result containing a tuple, accumulating all errors if any fail.
    /// </summary>
    /// <typeparam name="TValue1">The type of the first value in the resulting tuple.</typeparam>
    /// <typeparam name="TValue2">The type of the second value in the resulting tuple.</typeparam>
    /// <param name="result1">The first Result value to combine.</param>
    /// <param name="result2">The second Result value to combine.</param>
    /// <returns>A <see cref="Result{T}"/> containing a tuple with all values if all Results succeed; otherwise, a failure with all accumulated errors.</returns>
    /// <remarks>
    /// The BindAll factory method combines multiple Result values with comprehensive error accumulation:
    /// - All Result values are evaluated (no short-circuiting)
    /// - If any Results fail, ALL errors are collected using ErrorBuilder
    /// - Returns a composite error containing all failure information
    /// - Only if ALL Results succeed are their values combined into a tuple
    /// 
    /// This differs from Bind which uses short-circuit evaluation. Use BindAll when you need
    /// comprehensive error reporting, such as in validation scenarios where users benefit from
    /// seeing all errors at once rather than fixing them one at a time.
    /// 
    /// The 'in' parameter modifier provides performance optimization for value types.
    /// </remarks>
    public static Result<(TValue1, TValue2)> BindAllAppend<TValue1, TValue2>(
        in Result<TValue1> result1,
        in Result<TValue2> result2)
    {
        return new ErrorBuilder()
            .AppendOnFailure(result1)
            .AppendOnFailure(result2)
            .TryBuild(out Error error)
                ? error
                : (result1.Value, result2.Value);
    }

    /// <summary>
    /// Combines 3 Result values into a single Result containing a tuple, accumulating all errors if any fail.
    /// </summary>
    /// <typeparam name="TValue1">The type of the first value in the resulting tuple.</typeparam>
    /// <typeparam name="TValue2">The type of the second value in the resulting tuple.</typeparam>
    /// <typeparam name="TValue3">The type of the third value in the resulting tuple.</typeparam>
    /// <param name="result1">The first Result value to combine.</param>
    /// <param name="result2">The second Result value to combine.</param>
    /// <param name="result3">The third Result value to combine.</param>
    /// <returns>A <see cref="Result{T}"/> containing a tuple with all values if all Results succeed; otherwise, a failure with all accumulated errors.</returns>
    /// <remarks>
    /// The BindAll factory method combines multiple Result values with comprehensive error accumulation:
    /// - All Result values are evaluated (no short-circuiting)
    /// - If any Results fail, ALL errors are collected using ErrorBuilder
    /// - Returns a composite error containing all failure information
    /// - Only if ALL Results succeed are their values combined into a tuple
    /// 
    /// This differs from Bind which uses short-circuit evaluation. Use BindAll when you need
    /// comprehensive error reporting, such as in validation scenarios where users benefit from
    /// seeing all errors at once rather than fixing them one at a time.
    /// 
    /// The 'in' parameter modifier provides performance optimization for value types.
    /// </remarks>
    public static Result<(TValue1, TValue2, TValue3)> BindAllAppend<TValue1, TValue2, TValue3>(
        in Result<TValue1> result1,
        in Result<TValue2> result2,
        in Result<TValue3> result3)
    {
        return new ErrorBuilder()
            .AppendOnFailure(result1)
            .AppendOnFailure(result2)
            .AppendOnFailure(result3)
            .TryBuild(out Error error)
                ? error
                : (result1.Value, result2.Value, result3.Value);
    }

    /// <summary>
    /// Combines 4 Result values into a single Result containing a tuple, accumulating all errors if any fail.
    /// </summary>
    /// <typeparam name="TValue1">The type of the first value in the resulting tuple.</typeparam>
    /// <typeparam name="TValue2">The type of the second value in the resulting tuple.</typeparam>
    /// <typeparam name="TValue3">The type of the third value in the resulting tuple.</typeparam>
    /// <typeparam name="TValue4">The type of the fourth value in the resulting tuple.</typeparam>
    /// <param name="result1">The first Result value to combine.</param>
    /// <param name="result2">The second Result value to combine.</param>
    /// <param name="result3">The third Result value to combine.</param>
    /// <param name="result4">The fourth Result value to combine.</param>
    /// <returns>A <see cref="Result{T}"/> containing a tuple with all values if all Results succeed; otherwise, a failure with all accumulated errors.</returns>
    /// <remarks>
    /// The BindAll factory method combines multiple Result values with comprehensive error accumulation:
    /// - All Result values are evaluated (no short-circuiting)
    /// - If any Results fail, ALL errors are collected using ErrorBuilder
    /// - Returns a composite error containing all failure information
    /// - Only if ALL Results succeed are their values combined into a tuple
    /// 
    /// This differs from Bind which uses short-circuit evaluation. Use BindAll when you need
    /// comprehensive error reporting, such as in validation scenarios where users benefit from
    /// seeing all errors at once rather than fixing them one at a time.
    /// 
    /// The 'in' parameter modifier provides performance optimization for value types.
    /// </remarks>
    public static Result<(TValue1, TValue2, TValue3, TValue4)> BindAllAppend<TValue1, TValue2, TValue3, TValue4>(
        in Result<TValue1> result1,
        in Result<TValue2> result2,
        in Result<TValue3> result3,
        in Result<TValue4> result4)
    {
        return new ErrorBuilder()
            .AppendOnFailure(result1)
            .AppendOnFailure(result2)
            .AppendOnFailure(result3)
            .AppendOnFailure(result4)
            .TryBuild(out Error error)
                ? error
                : (result1.Value, result2.Value, result3.Value, result4.Value);
    }

    /// <summary>
    /// Combines 5 Result values into a single Result containing a tuple, accumulating all errors if any fail.
    /// </summary>
    /// <typeparam name="TValue1">The type of the first value in the resulting tuple.</typeparam>
    /// <typeparam name="TValue2">The type of the second value in the resulting tuple.</typeparam>
    /// <typeparam name="TValue3">The type of the third value in the resulting tuple.</typeparam>
    /// <typeparam name="TValue4">The type of the fourth value in the resulting tuple.</typeparam>
    /// <typeparam name="TValue5">The type of the fifth value in the resulting tuple.</typeparam>
    /// <param name="result1">The first Result value to combine.</param>
    /// <param name="result2">The second Result value to combine.</param>
    /// <param name="result3">The third Result value to combine.</param>
    /// <param name="result4">The fourth Result value to combine.</param>
    /// <param name="result5">The fifth Result value to combine.</param>
    /// <returns>A <see cref="Result{T}"/> containing a tuple with all values if all Results succeed; otherwise, a failure with all accumulated errors.</returns>
    /// <remarks>
    /// The BindAll factory method combines multiple Result values with comprehensive error accumulation:
    /// - All Result values are evaluated (no short-circuiting)
    /// - If any Results fail, ALL errors are collected using ErrorBuilder
    /// - Returns a composite error containing all failure information
    /// - Only if ALL Results succeed are their values combined into a tuple
    /// 
    /// This differs from Bind which uses short-circuit evaluation. Use BindAll when you need
    /// comprehensive error reporting, such as in validation scenarios where users benefit from
    /// seeing all errors at once rather than fixing them one at a time.
    /// 
    /// The 'in' parameter modifier provides performance optimization for value types.
    /// </remarks>
    public static Result<(TValue1, TValue2, TValue3, TValue4, TValue5)> BindAllAppend<TValue1, TValue2, TValue3, TValue4, TValue5>(
        in Result<TValue1> result1,
        in Result<TValue2> result2,
        in Result<TValue3> result3,
        in Result<TValue4> result4,
        in Result<TValue5> result5)
    {
        return new ErrorBuilder()
            .AppendOnFailure(result1)
            .AppendOnFailure(result2)
            .AppendOnFailure(result3)
            .AppendOnFailure(result4)
            .AppendOnFailure(result5)
            .TryBuild(out Error error)
                ? error
                : (result1.Value, result2.Value, result3.Value, result4.Value, result5.Value);
    }

    /// <summary>
    /// Combines 6 Result values into a single Result containing a tuple, accumulating all errors if any fail.
    /// </summary>
    /// <typeparam name="TValue1">The type of the first value in the resulting tuple.</typeparam>
    /// <typeparam name="TValue2">The type of the second value in the resulting tuple.</typeparam>
    /// <typeparam name="TValue3">The type of the third value in the resulting tuple.</typeparam>
    /// <typeparam name="TValue4">The type of the fourth value in the resulting tuple.</typeparam>
    /// <typeparam name="TValue5">The type of the fifth value in the resulting tuple.</typeparam>
    /// <typeparam name="TValue6">The type of the sixth value in the resulting tuple.</typeparam>
    /// <param name="result1">The first Result value to combine.</param>
    /// <param name="result2">The second Result value to combine.</param>
    /// <param name="result3">The third Result value to combine.</param>
    /// <param name="result4">The fourth Result value to combine.</param>
    /// <param name="result5">The fifth Result value to combine.</param>
    /// <param name="result6">The sixth Result value to combine.</param>
    /// <returns>A <see cref="Result{T}"/> containing a tuple with all values if all Results succeed; otherwise, a failure with all accumulated errors.</returns>
    /// <remarks>
    /// The BindAll factory method combines multiple Result values with comprehensive error accumulation:
    /// - All Result values are evaluated (no short-circuiting)
    /// - If any Results fail, ALL errors are collected using ErrorBuilder
    /// - Returns a composite error containing all failure information
    /// - Only if ALL Results succeed are their values combined into a tuple
    /// 
    /// This differs from Bind which uses short-circuit evaluation. Use BindAll when you need
    /// comprehensive error reporting, such as in validation scenarios where users benefit from
    /// seeing all errors at once rather than fixing them one at a time.
    /// 
    /// The 'in' parameter modifier provides performance optimization for value types.
    /// </remarks>
    public static Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)> BindAllAppend<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(
        in Result<TValue1> result1,
        in Result<TValue2> result2,
        in Result<TValue3> result3,
        in Result<TValue4> result4,
        in Result<TValue5> result5,
        in Result<TValue6> result6)
    {
        return new ErrorBuilder()
            .AppendOnFailure(result1)
            .AppendOnFailure(result2)
            .AppendOnFailure(result3)
            .AppendOnFailure(result4)
            .AppendOnFailure(result5)
            .AppendOnFailure(result6)
            .TryBuild(out Error error)
                ? error
                : (result1.Value, result2.Value, result3.Value, result4.Value, result5.Value, result6.Value);
    }

    /// <summary>
    /// Combines 7 Result values into a single Result containing a tuple, accumulating all errors if any fail.
    /// </summary>
    /// <typeparam name="TValue1">The type of the first value in the resulting tuple.</typeparam>
    /// <typeparam name="TValue2">The type of the second value in the resulting tuple.</typeparam>
    /// <typeparam name="TValue3">The type of the third value in the resulting tuple.</typeparam>
    /// <typeparam name="TValue4">The type of the fourth value in the resulting tuple.</typeparam>
    /// <typeparam name="TValue5">The type of the fifth value in the resulting tuple.</typeparam>
    /// <typeparam name="TValue6">The type of the sixth value in the resulting tuple.</typeparam>
    /// <typeparam name="TValue7">The type of the seventh value in the resulting tuple.</typeparam>
    /// <param name="result1">The first Result value to combine.</param>
    /// <param name="result2">The second Result value to combine.</param>
    /// <param name="result3">The third Result value to combine.</param>
    /// <param name="result4">The fourth Result value to combine.</param>
    /// <param name="result5">The fifth Result value to combine.</param>
    /// <param name="result6">The sixth Result value to combine.</param>
    /// <param name="result7">The seventh Result value to combine.</param>
    /// <returns>A <see cref="Result{T}"/> containing a tuple with all values if all Results succeed; otherwise, a failure with all accumulated errors.</returns>
    /// <remarks>
    /// The BindAll factory method combines multiple Result values with comprehensive error accumulation:
    /// - All Result values are evaluated (no short-circuiting)
    /// - If any Results fail, ALL errors are collected using ErrorBuilder
    /// - Returns a composite error containing all failure information
    /// - Only if ALL Results succeed are their values combined into a tuple
    /// 
    /// This differs from Bind which uses short-circuit evaluation. Use BindAll when you need
    /// comprehensive error reporting, such as in validation scenarios where users benefit from
    /// seeing all errors at once rather than fixing them one at a time.
    /// 
    /// The 'in' parameter modifier provides performance optimization for value types.
    /// </remarks>
    public static Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)> BindAllAppend<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(
        in Result<TValue1> result1,
        in Result<TValue2> result2,
        in Result<TValue3> result3,
        in Result<TValue4> result4,
        in Result<TValue5> result5,
        in Result<TValue6> result6,
        in Result<TValue7> result7)
    {
        return new ErrorBuilder()
            .AppendOnFailure(result1)
            .AppendOnFailure(result2)
            .AppendOnFailure(result3)
            .AppendOnFailure(result4)
            .AppendOnFailure(result5)
            .AppendOnFailure(result6)
            .AppendOnFailure(result7)
            .TryBuild(out Error error)
                ? error
                : (result1.Value, result2.Value, result3.Value, result4.Value, result5.Value, result6.Value, result7.Value);
    }

    /// <summary>
    /// Combines 8 Result values into a single Result containing a tuple, accumulating all errors if any fail.
    /// </summary>
    /// <typeparam name="TValue1">The type of the first value in the resulting tuple.</typeparam>
    /// <typeparam name="TValue2">The type of the second value in the resulting tuple.</typeparam>
    /// <typeparam name="TValue3">The type of the third value in the resulting tuple.</typeparam>
    /// <typeparam name="TValue4">The type of the fourth value in the resulting tuple.</typeparam>
    /// <typeparam name="TValue5">The type of the fifth value in the resulting tuple.</typeparam>
    /// <typeparam name="TValue6">The type of the sixth value in the resulting tuple.</typeparam>
    /// <typeparam name="TValue7">The type of the seventh value in the resulting tuple.</typeparam>
    /// <typeparam name="TValue8">The type of the eighth value in the resulting tuple.</typeparam>
    /// <param name="result1">The first Result value to combine.</param>
    /// <param name="result2">The second Result value to combine.</param>
    /// <param name="result3">The third Result value to combine.</param>
    /// <param name="result4">The fourth Result value to combine.</param>
    /// <param name="result5">The fifth Result value to combine.</param>
    /// <param name="result6">The sixth Result value to combine.</param>
    /// <param name="result7">The seventh Result value to combine.</param>
    /// <param name="result8">The eighth Result value to combine.</param>
    /// <returns>A <see cref="Result{T}"/> containing a tuple with all values if all Results succeed; otherwise, a failure with all accumulated errors.</returns>
    /// <remarks>
    /// The BindAll factory method combines multiple Result values with comprehensive error accumulation:
    /// - All Result values are evaluated (no short-circuiting)
    /// - If any Results fail, ALL errors are collected using ErrorBuilder
    /// - Returns a composite error containing all failure information
    /// - Only if ALL Results succeed are their values combined into a tuple
    /// 
    /// This differs from Bind which uses short-circuit evaluation. Use BindAll when you need
    /// comprehensive error reporting, such as in validation scenarios where users benefit from
    /// seeing all errors at once rather than fixing them one at a time.
    /// 
    /// The 'in' parameter modifier provides performance optimization for value types.
    /// </remarks>
    public static Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)> BindAllAppend<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(
        in Result<TValue1> result1,
        in Result<TValue2> result2,
        in Result<TValue3> result3,
        in Result<TValue4> result4,
        in Result<TValue5> result5,
        in Result<TValue6> result6,
        in Result<TValue7> result7,
        in Result<TValue8> result8)
    {
        return new ErrorBuilder()
            .AppendOnFailure(result1)
            .AppendOnFailure(result2)
            .AppendOnFailure(result3)
            .AppendOnFailure(result4)
            .AppendOnFailure(result5)
            .AppendOnFailure(result6)
            .AppendOnFailure(result7)
            .AppendOnFailure(result8)
            .TryBuild(out Error error)
                ? error
                : (result1.Value, result2.Value, result3.Value, result4.Value, result5.Value, result6.Value, result7.Value, result8.Value);
    }

}
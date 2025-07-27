namespace FluentUnions;

public readonly partial struct Result
{
    /// <summary>
    /// Combines 2 Result-returning operations into a single Result containing a tuple, using short-circuit evaluation.
    /// </summary>
    /// <typeparam name="TValue1">The type of the first value in the resulting tuple.</typeparam>
    /// <typeparam name="TValue2">The type of the second value in the resulting tuple.</typeparam>
    /// <param name="result1">A function that returns the first Result value.</param>
    /// <param name="result2">A function that returns the second Result value.</param>
    /// <returns>A <see cref="Result{T}"/> containing a tuple with all values if all operations succeed; otherwise, the first error encountered.</returns>
    /// <remarks>
    /// The Bind factory method executes Result-returning operations sequentially with short-circuit evaluation:
    /// - Each function is executed in order
    /// - If any function returns a failure, execution stops and that error is returned immediately
    /// - Only if all functions succeed are their values combined into a tuple Result
    /// 
    /// This differs from BindAll which accumulates all errors. Use Bind when you want fail-fast behavior
    /// and don't need to collect all possible errors. The sequential execution means later operations
    /// won't run if earlier ones fail, which can be more efficient for expensive operations.
    /// </remarks>
    public static Result<(TValue1, TValue2)> BindAppend<TValue1, TValue2>(
        Func<Result<TValue1>> result1,
        Func<Result<TValue2>> result2)
    {
        Result<TValue1> r1 = result1();
        if(r1.IsFailure) return r1.Error;

        Result<TValue2> r2 = result2();
        if(r2.IsFailure) return r2.Error;

        return (r1.Value, r2.Value);
    }

    /// <summary>
    /// Combines 3 Result-returning operations into a single Result containing a tuple, using short-circuit evaluation.
    /// </summary>
    /// <typeparam name="TValue1">The type of the first value in the resulting tuple.</typeparam>
    /// <typeparam name="TValue2">The type of the second value in the resulting tuple.</typeparam>
    /// <typeparam name="TValue3">The type of the third value in the resulting tuple.</typeparam>
    /// <param name="result1">A function that returns the first Result value.</param>
    /// <param name="result2">A function that returns the second Result value.</param>
    /// <param name="result3">A function that returns the third Result value.</param>
    /// <returns>A <see cref="Result{T}"/> containing a tuple with all values if all operations succeed; otherwise, the first error encountered.</returns>
    /// <remarks>
    /// The Bind factory method executes Result-returning operations sequentially with short-circuit evaluation:
    /// - Each function is executed in order
    /// - If any function returns a failure, execution stops and that error is returned immediately
    /// - Only if all functions succeed are their values combined into a tuple Result
    /// 
    /// This differs from BindAll which accumulates all errors. Use Bind when you want fail-fast behavior
    /// and don't need to collect all possible errors. The sequential execution means later operations
    /// won't run if earlier ones fail, which can be more efficient for expensive operations.
    /// </remarks>
    public static Result<(TValue1, TValue2, TValue3)> BindAppend<TValue1, TValue2, TValue3>(
        Func<Result<TValue1>> result1,
        Func<Result<TValue2>> result2,
        Func<Result<TValue3>> result3)
    {
        Result<TValue1> r1 = result1();
        if(r1.IsFailure) return r1.Error;

        Result<TValue2> r2 = result2();
        if(r2.IsFailure) return r2.Error;

        Result<TValue3> r3 = result3();
        if(r3.IsFailure) return r3.Error;

        return (r1.Value, r2.Value, r3.Value);
    }

    /// <summary>
    /// Combines 4 Result-returning operations into a single Result containing a tuple, using short-circuit evaluation.
    /// </summary>
    /// <typeparam name="TValue1">The type of the first value in the resulting tuple.</typeparam>
    /// <typeparam name="TValue2">The type of the second value in the resulting tuple.</typeparam>
    /// <typeparam name="TValue3">The type of the third value in the resulting tuple.</typeparam>
    /// <typeparam name="TValue4">The type of the fourth value in the resulting tuple.</typeparam>
    /// <param name="result1">A function that returns the first Result value.</param>
    /// <param name="result2">A function that returns the second Result value.</param>
    /// <param name="result3">A function that returns the third Result value.</param>
    /// <param name="result4">A function that returns the fourth Result value.</param>
    /// <returns>A <see cref="Result{T}"/> containing a tuple with all values if all operations succeed; otherwise, the first error encountered.</returns>
    /// <remarks>
    /// The Bind factory method executes Result-returning operations sequentially with short-circuit evaluation:
    /// - Each function is executed in order
    /// - If any function returns a failure, execution stops and that error is returned immediately
    /// - Only if all functions succeed are their values combined into a tuple Result
    /// 
    /// This differs from BindAll which accumulates all errors. Use Bind when you want fail-fast behavior
    /// and don't need to collect all possible errors. The sequential execution means later operations
    /// won't run if earlier ones fail, which can be more efficient for expensive operations.
    /// </remarks>
    public static Result<(TValue1, TValue2, TValue3, TValue4)> BindAppend<TValue1, TValue2, TValue3, TValue4>(
        Func<Result<TValue1>> result1,
        Func<Result<TValue2>> result2,
        Func<Result<TValue3>> result3,
        Func<Result<TValue4>> result4)
    {
        Result<TValue1> r1 = result1();
        if(r1.IsFailure) return r1.Error;

        Result<TValue2> r2 = result2();
        if(r2.IsFailure) return r2.Error;

        Result<TValue3> r3 = result3();
        if(r3.IsFailure) return r3.Error;

        Result<TValue4> r4 = result4();
        if(r4.IsFailure) return r4.Error;

        return (r1.Value, r2.Value, r3.Value, r4.Value);
    }

    /// <summary>
    /// Combines 5 Result-returning operations into a single Result containing a tuple, using short-circuit evaluation.
    /// </summary>
    /// <typeparam name="TValue1">The type of the first value in the resulting tuple.</typeparam>
    /// <typeparam name="TValue2">The type of the second value in the resulting tuple.</typeparam>
    /// <typeparam name="TValue3">The type of the third value in the resulting tuple.</typeparam>
    /// <typeparam name="TValue4">The type of the fourth value in the resulting tuple.</typeparam>
    /// <typeparam name="TValue5">The type of the fifth value in the resulting tuple.</typeparam>
    /// <param name="result1">A function that returns the first Result value.</param>
    /// <param name="result2">A function that returns the second Result value.</param>
    /// <param name="result3">A function that returns the third Result value.</param>
    /// <param name="result4">A function that returns the fourth Result value.</param>
    /// <param name="result5">A function that returns the fifth Result value.</param>
    /// <returns>A <see cref="Result{T}"/> containing a tuple with all values if all operations succeed; otherwise, the first error encountered.</returns>
    /// <remarks>
    /// The Bind factory method executes Result-returning operations sequentially with short-circuit evaluation:
    /// - Each function is executed in order
    /// - If any function returns a failure, execution stops and that error is returned immediately
    /// - Only if all functions succeed are their values combined into a tuple Result
    /// 
    /// This differs from BindAll which accumulates all errors. Use Bind when you want fail-fast behavior
    /// and don't need to collect all possible errors. The sequential execution means later operations
    /// won't run if earlier ones fail, which can be more efficient for expensive operations.
    /// </remarks>
    public static Result<(TValue1, TValue2, TValue3, TValue4, TValue5)> BindAppend<TValue1, TValue2, TValue3, TValue4, TValue5>(
        Func<Result<TValue1>> result1,
        Func<Result<TValue2>> result2,
        Func<Result<TValue3>> result3,
        Func<Result<TValue4>> result4,
        Func<Result<TValue5>> result5)
    {
        Result<TValue1> r1 = result1();
        if(r1.IsFailure) return r1.Error;

        Result<TValue2> r2 = result2();
        if(r2.IsFailure) return r2.Error;

        Result<TValue3> r3 = result3();
        if(r3.IsFailure) return r3.Error;

        Result<TValue4> r4 = result4();
        if(r4.IsFailure) return r4.Error;

        Result<TValue5> r5 = result5();
        if(r5.IsFailure) return r5.Error;

        return (r1.Value, r2.Value, r3.Value, r4.Value, r5.Value);
    }

    /// <summary>
    /// Combines 6 Result-returning operations into a single Result containing a tuple, using short-circuit evaluation.
    /// </summary>
    /// <typeparam name="TValue1">The type of the first value in the resulting tuple.</typeparam>
    /// <typeparam name="TValue2">The type of the second value in the resulting tuple.</typeparam>
    /// <typeparam name="TValue3">The type of the third value in the resulting tuple.</typeparam>
    /// <typeparam name="TValue4">The type of the fourth value in the resulting tuple.</typeparam>
    /// <typeparam name="TValue5">The type of the fifth value in the resulting tuple.</typeparam>
    /// <typeparam name="TValue6">The type of the sixth value in the resulting tuple.</typeparam>
    /// <param name="result1">A function that returns the first Result value.</param>
    /// <param name="result2">A function that returns the second Result value.</param>
    /// <param name="result3">A function that returns the third Result value.</param>
    /// <param name="result4">A function that returns the fourth Result value.</param>
    /// <param name="result5">A function that returns the fifth Result value.</param>
    /// <param name="result6">A function that returns the sixth Result value.</param>
    /// <returns>A <see cref="Result{T}"/> containing a tuple with all values if all operations succeed; otherwise, the first error encountered.</returns>
    /// <remarks>
    /// The Bind factory method executes Result-returning operations sequentially with short-circuit evaluation:
    /// - Each function is executed in order
    /// - If any function returns a failure, execution stops and that error is returned immediately
    /// - Only if all functions succeed are their values combined into a tuple Result
    /// 
    /// This differs from BindAll which accumulates all errors. Use Bind when you want fail-fast behavior
    /// and don't need to collect all possible errors. The sequential execution means later operations
    /// won't run if earlier ones fail, which can be more efficient for expensive operations.
    /// </remarks>
    public static Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)> BindAppend<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(
        Func<Result<TValue1>> result1,
        Func<Result<TValue2>> result2,
        Func<Result<TValue3>> result3,
        Func<Result<TValue4>> result4,
        Func<Result<TValue5>> result5,
        Func<Result<TValue6>> result6)
    {
        Result<TValue1> r1 = result1();
        if(r1.IsFailure) return r1.Error;

        Result<TValue2> r2 = result2();
        if(r2.IsFailure) return r2.Error;

        Result<TValue3> r3 = result3();
        if(r3.IsFailure) return r3.Error;

        Result<TValue4> r4 = result4();
        if(r4.IsFailure) return r4.Error;

        Result<TValue5> r5 = result5();
        if(r5.IsFailure) return r5.Error;

        Result<TValue6> r6 = result6();
        if(r6.IsFailure) return r6.Error;

        return (r1.Value, r2.Value, r3.Value, r4.Value, r5.Value, r6.Value);
    }

    /// <summary>
    /// Combines 7 Result-returning operations into a single Result containing a tuple, using short-circuit evaluation.
    /// </summary>
    /// <typeparam name="TValue1">The type of the first value in the resulting tuple.</typeparam>
    /// <typeparam name="TValue2">The type of the second value in the resulting tuple.</typeparam>
    /// <typeparam name="TValue3">The type of the third value in the resulting tuple.</typeparam>
    /// <typeparam name="TValue4">The type of the fourth value in the resulting tuple.</typeparam>
    /// <typeparam name="TValue5">The type of the fifth value in the resulting tuple.</typeparam>
    /// <typeparam name="TValue6">The type of the sixth value in the resulting tuple.</typeparam>
    /// <typeparam name="TValue7">The type of the seventh value in the resulting tuple.</typeparam>
    /// <param name="result1">A function that returns the first Result value.</param>
    /// <param name="result2">A function that returns the second Result value.</param>
    /// <param name="result3">A function that returns the third Result value.</param>
    /// <param name="result4">A function that returns the fourth Result value.</param>
    /// <param name="result5">A function that returns the fifth Result value.</param>
    /// <param name="result6">A function that returns the sixth Result value.</param>
    /// <param name="result7">A function that returns the seventh Result value.</param>
    /// <returns>A <see cref="Result{T}"/> containing a tuple with all values if all operations succeed; otherwise, the first error encountered.</returns>
    /// <remarks>
    /// The Bind factory method executes Result-returning operations sequentially with short-circuit evaluation:
    /// - Each function is executed in order
    /// - If any function returns a failure, execution stops and that error is returned immediately
    /// - Only if all functions succeed are their values combined into a tuple Result
    /// 
    /// This differs from BindAll which accumulates all errors. Use Bind when you want fail-fast behavior
    /// and don't need to collect all possible errors. The sequential execution means later operations
    /// won't run if earlier ones fail, which can be more efficient for expensive operations.
    /// </remarks>
    public static Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)> BindAppend<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(
        Func<Result<TValue1>> result1,
        Func<Result<TValue2>> result2,
        Func<Result<TValue3>> result3,
        Func<Result<TValue4>> result4,
        Func<Result<TValue5>> result5,
        Func<Result<TValue6>> result6,
        Func<Result<TValue7>> result7)
    {
        Result<TValue1> r1 = result1();
        if(r1.IsFailure) return r1.Error;

        Result<TValue2> r2 = result2();
        if(r2.IsFailure) return r2.Error;

        Result<TValue3> r3 = result3();
        if(r3.IsFailure) return r3.Error;

        Result<TValue4> r4 = result4();
        if(r4.IsFailure) return r4.Error;

        Result<TValue5> r5 = result5();
        if(r5.IsFailure) return r5.Error;

        Result<TValue6> r6 = result6();
        if(r6.IsFailure) return r6.Error;

        Result<TValue7> r7 = result7();
        if(r7.IsFailure) return r7.Error;

        return (r1.Value, r2.Value, r3.Value, r4.Value, r5.Value, r6.Value, r7.Value);
    }

    /// <summary>
    /// Combines 8 Result-returning operations into a single Result containing a tuple, using short-circuit evaluation.
    /// </summary>
    /// <typeparam name="TValue1">The type of the first value in the resulting tuple.</typeparam>
    /// <typeparam name="TValue2">The type of the second value in the resulting tuple.</typeparam>
    /// <typeparam name="TValue3">The type of the third value in the resulting tuple.</typeparam>
    /// <typeparam name="TValue4">The type of the fourth value in the resulting tuple.</typeparam>
    /// <typeparam name="TValue5">The type of the fifth value in the resulting tuple.</typeparam>
    /// <typeparam name="TValue6">The type of the sixth value in the resulting tuple.</typeparam>
    /// <typeparam name="TValue7">The type of the seventh value in the resulting tuple.</typeparam>
    /// <typeparam name="TValue8">The type of the eighth value in the resulting tuple.</typeparam>
    /// <param name="result1">A function that returns the first Result value.</param>
    /// <param name="result2">A function that returns the second Result value.</param>
    /// <param name="result3">A function that returns the third Result value.</param>
    /// <param name="result4">A function that returns the fourth Result value.</param>
    /// <param name="result5">A function that returns the fifth Result value.</param>
    /// <param name="result6">A function that returns the sixth Result value.</param>
    /// <param name="result7">A function that returns the seventh Result value.</param>
    /// <param name="result8">A function that returns the eighth Result value.</param>
    /// <returns>A <see cref="Result{T}"/> containing a tuple with all values if all operations succeed; otherwise, the first error encountered.</returns>
    /// <remarks>
    /// The Bind factory method executes Result-returning operations sequentially with short-circuit evaluation:
    /// - Each function is executed in order
    /// - If any function returns a failure, execution stops and that error is returned immediately
    /// - Only if all functions succeed are their values combined into a tuple Result
    /// 
    /// This differs from BindAll which accumulates all errors. Use Bind when you want fail-fast behavior
    /// and don't need to collect all possible errors. The sequential execution means later operations
    /// won't run if earlier ones fail, which can be more efficient for expensive operations.
    /// </remarks>
    public static Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)> BindAppend<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(
        Func<Result<TValue1>> result1,
        Func<Result<TValue2>> result2,
        Func<Result<TValue3>> result3,
        Func<Result<TValue4>> result4,
        Func<Result<TValue5>> result5,
        Func<Result<TValue6>> result6,
        Func<Result<TValue7>> result7,
        Func<Result<TValue8>> result8)
    {
        Result<TValue1> r1 = result1();
        if(r1.IsFailure) return r1.Error;

        Result<TValue2> r2 = result2();
        if(r2.IsFailure) return r2.Error;

        Result<TValue3> r3 = result3();
        if(r3.IsFailure) return r3.Error;

        Result<TValue4> r4 = result4();
        if(r4.IsFailure) return r4.Error;

        Result<TValue5> r5 = result5();
        if(r5.IsFailure) return r5.Error;

        Result<TValue6> r6 = result6();
        if(r6.IsFailure) return r6.Error;

        Result<TValue7> r7 = result7();
        if(r7.IsFailure) return r7.Error;

        Result<TValue8> r8 = result8();
        if(r8.IsFailure) return r8.Error;

        return (r1.Value, r2.Value, r3.Value, r4.Value, r5.Value, r6.Value, r7.Value, r8.Value);
    }

}
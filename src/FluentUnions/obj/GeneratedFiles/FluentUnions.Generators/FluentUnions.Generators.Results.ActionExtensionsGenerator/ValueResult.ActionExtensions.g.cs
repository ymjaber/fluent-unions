namespace FluentUnions;

public static partial class ValueResultExtensions
{
/// <summary>
/// Executes a side effect on the value of a successful <see cref="Result{T}"/> containing a tuple with 2 elements.
/// </summary>
/// <typeparam name="TValue1">The type of the first tuple element.</typeparam>
    /// <typeparam name="TValue2">The type of the second tuple element.</typeparam>
/// <param name="result">The source <see cref="Result{T}"/> containing a tuple.</param>
/// <param name="action">The action to execute if the Result is successful, receiving the tuple elements as separate parameters.</param>
/// <returns>The original Result unchanged, allowing for method chaining.</returns>
/// <remarks>
/// OnSuccess is a side-effect operation that executes only when the Result is successful. Unlike Map, it doesn't
/// transform the value but allows you to perform actions like logging, notifications, or state updates.
/// If the Result is a failure, the action is not executed and the error is propagated unchanged.
/// This maintains the railway-oriented programming flow while enabling observable side effects.
/// </remarks>
public static Result<(TValue1, TValue2)> OnSuccess<TValue1, TValue2>(in this Result<(TValue1, TValue2)> result, Action<TValue1, TValue2> action)
{
    if (result.IsSuccess) action(result.Value.Item1, result.Value.Item2);
    return result;
}

/// <summary>
/// Executes different side effects based on whether a <see cref="Result{T}"/> containing a tuple with 2 elements is successful or failed.
/// </summary>
/// <typeparam name="TValue1">The type of the first tuple element.</typeparam>
    /// <typeparam name="TValue2">The type of the second tuple element.</typeparam>
/// <param name="result">The source <see cref="Result{T}"/> containing a tuple.</param>
/// <param name="success">The action to execute if the Result is successful, receiving the tuple elements as separate parameters.</param>
/// <param name="failure">The action to execute if the Result is a failure, receiving the error.</param>
/// <returns>The original Result unchanged, allowing for method chaining.</returns>
/// <remarks>
/// OnEither allows you to execute different side effects for both success and failure cases without
/// transforming the Result. This is useful for comprehensive logging, debugging, or triggering
/// different workflows based on the Result state. The original Result is returned unchanged,
/// preserving the error handling chain while allowing observation of both paths.
/// </remarks>
public static Result<(TValue1, TValue2)> OnEither<TValue1, TValue2>(in this Result<(TValue1, TValue2)> result, Action<TValue1, TValue2> success, Action<Error> failure)
{
    if (result.IsSuccess) success(result.Value.Item1, result.Value.Item2);
    else failure(result.Error);
    return result;
}
/// <summary>
/// Executes a side effect on the value of a successful <see cref="Result{T}"/> containing a tuple with 3 elements.
/// </summary>
/// <typeparam name="TValue1">The type of the first tuple element.</typeparam>
    /// <typeparam name="TValue2">The type of the second tuple element.</typeparam>
    /// <typeparam name="TValue3">The type of the third tuple element.</typeparam>
/// <param name="result">The source <see cref="Result{T}"/> containing a tuple.</param>
/// <param name="action">The action to execute if the Result is successful, receiving the tuple elements as separate parameters.</param>
/// <returns>The original Result unchanged, allowing for method chaining.</returns>
/// <remarks>
/// OnSuccess is a side-effect operation that executes only when the Result is successful. Unlike Map, it doesn't
/// transform the value but allows you to perform actions like logging, notifications, or state updates.
/// If the Result is a failure, the action is not executed and the error is propagated unchanged.
/// This maintains the railway-oriented programming flow while enabling observable side effects.
/// </remarks>
public static Result<(TValue1, TValue2, TValue3)> OnSuccess<TValue1, TValue2, TValue3>(in this Result<(TValue1, TValue2, TValue3)> result, Action<TValue1, TValue2, TValue3> action)
{
    if (result.IsSuccess) action(result.Value.Item1, result.Value.Item2, result.Value.Item3);
    return result;
}

/// <summary>
/// Executes different side effects based on whether a <see cref="Result{T}"/> containing a tuple with 3 elements is successful or failed.
/// </summary>
/// <typeparam name="TValue1">The type of the first tuple element.</typeparam>
    /// <typeparam name="TValue2">The type of the second tuple element.</typeparam>
    /// <typeparam name="TValue3">The type of the third tuple element.</typeparam>
/// <param name="result">The source <see cref="Result{T}"/> containing a tuple.</param>
/// <param name="success">The action to execute if the Result is successful, receiving the tuple elements as separate parameters.</param>
/// <param name="failure">The action to execute if the Result is a failure, receiving the error.</param>
/// <returns>The original Result unchanged, allowing for method chaining.</returns>
/// <remarks>
/// OnEither allows you to execute different side effects for both success and failure cases without
/// transforming the Result. This is useful for comprehensive logging, debugging, or triggering
/// different workflows based on the Result state. The original Result is returned unchanged,
/// preserving the error handling chain while allowing observation of both paths.
/// </remarks>
public static Result<(TValue1, TValue2, TValue3)> OnEither<TValue1, TValue2, TValue3>(in this Result<(TValue1, TValue2, TValue3)> result, Action<TValue1, TValue2, TValue3> success, Action<Error> failure)
{
    if (result.IsSuccess) success(result.Value.Item1, result.Value.Item2, result.Value.Item3);
    else failure(result.Error);
    return result;
}
/// <summary>
/// Executes a side effect on the value of a successful <see cref="Result{T}"/> containing a tuple with 4 elements.
/// </summary>
/// <typeparam name="TValue1">The type of the first tuple element.</typeparam>
    /// <typeparam name="TValue2">The type of the second tuple element.</typeparam>
    /// <typeparam name="TValue3">The type of the third tuple element.</typeparam>
    /// <typeparam name="TValue4">The type of the fourth tuple element.</typeparam>
/// <param name="result">The source <see cref="Result{T}"/> containing a tuple.</param>
/// <param name="action">The action to execute if the Result is successful, receiving the tuple elements as separate parameters.</param>
/// <returns>The original Result unchanged, allowing for method chaining.</returns>
/// <remarks>
/// OnSuccess is a side-effect operation that executes only when the Result is successful. Unlike Map, it doesn't
/// transform the value but allows you to perform actions like logging, notifications, or state updates.
/// If the Result is a failure, the action is not executed and the error is propagated unchanged.
/// This maintains the railway-oriented programming flow while enabling observable side effects.
/// </remarks>
public static Result<(TValue1, TValue2, TValue3, TValue4)> OnSuccess<TValue1, TValue2, TValue3, TValue4>(in this Result<(TValue1, TValue2, TValue3, TValue4)> result, Action<TValue1, TValue2, TValue3, TValue4> action)
{
    if (result.IsSuccess) action(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4);
    return result;
}

/// <summary>
/// Executes different side effects based on whether a <see cref="Result{T}"/> containing a tuple with 4 elements is successful or failed.
/// </summary>
/// <typeparam name="TValue1">The type of the first tuple element.</typeparam>
    /// <typeparam name="TValue2">The type of the second tuple element.</typeparam>
    /// <typeparam name="TValue3">The type of the third tuple element.</typeparam>
    /// <typeparam name="TValue4">The type of the fourth tuple element.</typeparam>
/// <param name="result">The source <see cref="Result{T}"/> containing a tuple.</param>
/// <param name="success">The action to execute if the Result is successful, receiving the tuple elements as separate parameters.</param>
/// <param name="failure">The action to execute if the Result is a failure, receiving the error.</param>
/// <returns>The original Result unchanged, allowing for method chaining.</returns>
/// <remarks>
/// OnEither allows you to execute different side effects for both success and failure cases without
/// transforming the Result. This is useful for comprehensive logging, debugging, or triggering
/// different workflows based on the Result state. The original Result is returned unchanged,
/// preserving the error handling chain while allowing observation of both paths.
/// </remarks>
public static Result<(TValue1, TValue2, TValue3, TValue4)> OnEither<TValue1, TValue2, TValue3, TValue4>(in this Result<(TValue1, TValue2, TValue3, TValue4)> result, Action<TValue1, TValue2, TValue3, TValue4> success, Action<Error> failure)
{
    if (result.IsSuccess) success(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4);
    else failure(result.Error);
    return result;
}
/// <summary>
/// Executes a side effect on the value of a successful <see cref="Result{T}"/> containing a tuple with 5 elements.
/// </summary>
/// <typeparam name="TValue1">The type of the first tuple element.</typeparam>
    /// <typeparam name="TValue2">The type of the second tuple element.</typeparam>
    /// <typeparam name="TValue3">The type of the third tuple element.</typeparam>
    /// <typeparam name="TValue4">The type of the fourth tuple element.</typeparam>
    /// <typeparam name="TValue5">The type of the fifth tuple element.</typeparam>
/// <param name="result">The source <see cref="Result{T}"/> containing a tuple.</param>
/// <param name="action">The action to execute if the Result is successful, receiving the tuple elements as separate parameters.</param>
/// <returns>The original Result unchanged, allowing for method chaining.</returns>
/// <remarks>
/// OnSuccess is a side-effect operation that executes only when the Result is successful. Unlike Map, it doesn't
/// transform the value but allows you to perform actions like logging, notifications, or state updates.
/// If the Result is a failure, the action is not executed and the error is propagated unchanged.
/// This maintains the railway-oriented programming flow while enabling observable side effects.
/// </remarks>
public static Result<(TValue1, TValue2, TValue3, TValue4, TValue5)> OnSuccess<TValue1, TValue2, TValue3, TValue4, TValue5>(in this Result<(TValue1, TValue2, TValue3, TValue4, TValue5)> result, Action<TValue1, TValue2, TValue3, TValue4, TValue5> action)
{
    if (result.IsSuccess) action(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5);
    return result;
}

/// <summary>
/// Executes different side effects based on whether a <see cref="Result{T}"/> containing a tuple with 5 elements is successful or failed.
/// </summary>
/// <typeparam name="TValue1">The type of the first tuple element.</typeparam>
    /// <typeparam name="TValue2">The type of the second tuple element.</typeparam>
    /// <typeparam name="TValue3">The type of the third tuple element.</typeparam>
    /// <typeparam name="TValue4">The type of the fourth tuple element.</typeparam>
    /// <typeparam name="TValue5">The type of the fifth tuple element.</typeparam>
/// <param name="result">The source <see cref="Result{T}"/> containing a tuple.</param>
/// <param name="success">The action to execute if the Result is successful, receiving the tuple elements as separate parameters.</param>
/// <param name="failure">The action to execute if the Result is a failure, receiving the error.</param>
/// <returns>The original Result unchanged, allowing for method chaining.</returns>
/// <remarks>
/// OnEither allows you to execute different side effects for both success and failure cases without
/// transforming the Result. This is useful for comprehensive logging, debugging, or triggering
/// different workflows based on the Result state. The original Result is returned unchanged,
/// preserving the error handling chain while allowing observation of both paths.
/// </remarks>
public static Result<(TValue1, TValue2, TValue3, TValue4, TValue5)> OnEither<TValue1, TValue2, TValue3, TValue4, TValue5>(in this Result<(TValue1, TValue2, TValue3, TValue4, TValue5)> result, Action<TValue1, TValue2, TValue3, TValue4, TValue5> success, Action<Error> failure)
{
    if (result.IsSuccess) success(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5);
    else failure(result.Error);
    return result;
}
/// <summary>
/// Executes a side effect on the value of a successful <see cref="Result{T}"/> containing a tuple with 6 elements.
/// </summary>
/// <typeparam name="TValue1">The type of the first tuple element.</typeparam>
    /// <typeparam name="TValue2">The type of the second tuple element.</typeparam>
    /// <typeparam name="TValue3">The type of the third tuple element.</typeparam>
    /// <typeparam name="TValue4">The type of the fourth tuple element.</typeparam>
    /// <typeparam name="TValue5">The type of the fifth tuple element.</typeparam>
    /// <typeparam name="TValue6">The type of the sixth tuple element.</typeparam>
/// <param name="result">The source <see cref="Result{T}"/> containing a tuple.</param>
/// <param name="action">The action to execute if the Result is successful, receiving the tuple elements as separate parameters.</param>
/// <returns>The original Result unchanged, allowing for method chaining.</returns>
/// <remarks>
/// OnSuccess is a side-effect operation that executes only when the Result is successful. Unlike Map, it doesn't
/// transform the value but allows you to perform actions like logging, notifications, or state updates.
/// If the Result is a failure, the action is not executed and the error is propagated unchanged.
/// This maintains the railway-oriented programming flow while enabling observable side effects.
/// </remarks>
public static Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)> OnSuccess<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(in this Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)> result, Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> action)
{
    if (result.IsSuccess) action(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, result.Value.Item6);
    return result;
}

/// <summary>
/// Executes different side effects based on whether a <see cref="Result{T}"/> containing a tuple with 6 elements is successful or failed.
/// </summary>
/// <typeparam name="TValue1">The type of the first tuple element.</typeparam>
    /// <typeparam name="TValue2">The type of the second tuple element.</typeparam>
    /// <typeparam name="TValue3">The type of the third tuple element.</typeparam>
    /// <typeparam name="TValue4">The type of the fourth tuple element.</typeparam>
    /// <typeparam name="TValue5">The type of the fifth tuple element.</typeparam>
    /// <typeparam name="TValue6">The type of the sixth tuple element.</typeparam>
/// <param name="result">The source <see cref="Result{T}"/> containing a tuple.</param>
/// <param name="success">The action to execute if the Result is successful, receiving the tuple elements as separate parameters.</param>
/// <param name="failure">The action to execute if the Result is a failure, receiving the error.</param>
/// <returns>The original Result unchanged, allowing for method chaining.</returns>
/// <remarks>
/// OnEither allows you to execute different side effects for both success and failure cases without
/// transforming the Result. This is useful for comprehensive logging, debugging, or triggering
/// different workflows based on the Result state. The original Result is returned unchanged,
/// preserving the error handling chain while allowing observation of both paths.
/// </remarks>
public static Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)> OnEither<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(in this Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)> result, Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> success, Action<Error> failure)
{
    if (result.IsSuccess) success(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, result.Value.Item6);
    else failure(result.Error);
    return result;
}
/// <summary>
/// Executes a side effect on the value of a successful <see cref="Result{T}"/> containing a tuple with 7 elements.
/// </summary>
/// <typeparam name="TValue1">The type of the first tuple element.</typeparam>
    /// <typeparam name="TValue2">The type of the second tuple element.</typeparam>
    /// <typeparam name="TValue3">The type of the third tuple element.</typeparam>
    /// <typeparam name="TValue4">The type of the fourth tuple element.</typeparam>
    /// <typeparam name="TValue5">The type of the fifth tuple element.</typeparam>
    /// <typeparam name="TValue6">The type of the sixth tuple element.</typeparam>
    /// <typeparam name="TValue7">The type of the seventh tuple element.</typeparam>
/// <param name="result">The source <see cref="Result{T}"/> containing a tuple.</param>
/// <param name="action">The action to execute if the Result is successful, receiving the tuple elements as separate parameters.</param>
/// <returns>The original Result unchanged, allowing for method chaining.</returns>
/// <remarks>
/// OnSuccess is a side-effect operation that executes only when the Result is successful. Unlike Map, it doesn't
/// transform the value but allows you to perform actions like logging, notifications, or state updates.
/// If the Result is a failure, the action is not executed and the error is propagated unchanged.
/// This maintains the railway-oriented programming flow while enabling observable side effects.
/// </remarks>
public static Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)> OnSuccess<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(in this Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)> result, Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> action)
{
    if (result.IsSuccess) action(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, result.Value.Item6, result.Value.Item7);
    return result;
}

/// <summary>
/// Executes different side effects based on whether a <see cref="Result{T}"/> containing a tuple with 7 elements is successful or failed.
/// </summary>
/// <typeparam name="TValue1">The type of the first tuple element.</typeparam>
    /// <typeparam name="TValue2">The type of the second tuple element.</typeparam>
    /// <typeparam name="TValue3">The type of the third tuple element.</typeparam>
    /// <typeparam name="TValue4">The type of the fourth tuple element.</typeparam>
    /// <typeparam name="TValue5">The type of the fifth tuple element.</typeparam>
    /// <typeparam name="TValue6">The type of the sixth tuple element.</typeparam>
    /// <typeparam name="TValue7">The type of the seventh tuple element.</typeparam>
/// <param name="result">The source <see cref="Result{T}"/> containing a tuple.</param>
/// <param name="success">The action to execute if the Result is successful, receiving the tuple elements as separate parameters.</param>
/// <param name="failure">The action to execute if the Result is a failure, receiving the error.</param>
/// <returns>The original Result unchanged, allowing for method chaining.</returns>
/// <remarks>
/// OnEither allows you to execute different side effects for both success and failure cases without
/// transforming the Result. This is useful for comprehensive logging, debugging, or triggering
/// different workflows based on the Result state. The original Result is returned unchanged,
/// preserving the error handling chain while allowing observation of both paths.
/// </remarks>
public static Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)> OnEither<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(in this Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)> result, Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> success, Action<Error> failure)
{
    if (result.IsSuccess) success(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, result.Value.Item6, result.Value.Item7);
    else failure(result.Error);
    return result;
}
/// <summary>
/// Executes a side effect on the value of a successful <see cref="Result{T}"/> containing a tuple with 8 elements.
/// </summary>
/// <typeparam name="TValue1">The type of the first tuple element.</typeparam>
    /// <typeparam name="TValue2">The type of the second tuple element.</typeparam>
    /// <typeparam name="TValue3">The type of the third tuple element.</typeparam>
    /// <typeparam name="TValue4">The type of the fourth tuple element.</typeparam>
    /// <typeparam name="TValue5">The type of the fifth tuple element.</typeparam>
    /// <typeparam name="TValue6">The type of the sixth tuple element.</typeparam>
    /// <typeparam name="TValue7">The type of the seventh tuple element.</typeparam>
    /// <typeparam name="TValue8">The type of the eighth tuple element.</typeparam>
/// <param name="result">The source <see cref="Result{T}"/> containing a tuple.</param>
/// <param name="action">The action to execute if the Result is successful, receiving the tuple elements as separate parameters.</param>
/// <returns>The original Result unchanged, allowing for method chaining.</returns>
/// <remarks>
/// OnSuccess is a side-effect operation that executes only when the Result is successful. Unlike Map, it doesn't
/// transform the value but allows you to perform actions like logging, notifications, or state updates.
/// If the Result is a failure, the action is not executed and the error is propagated unchanged.
/// This maintains the railway-oriented programming flow while enabling observable side effects.
/// </remarks>
public static Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)> OnSuccess<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(in this Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)> result, Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> action)
{
    if (result.IsSuccess) action(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, result.Value.Item6, result.Value.Item7, result.Value.Item8);
    return result;
}

/// <summary>
/// Executes different side effects based on whether a <see cref="Result{T}"/> containing a tuple with 8 elements is successful or failed.
/// </summary>
/// <typeparam name="TValue1">The type of the first tuple element.</typeparam>
    /// <typeparam name="TValue2">The type of the second tuple element.</typeparam>
    /// <typeparam name="TValue3">The type of the third tuple element.</typeparam>
    /// <typeparam name="TValue4">The type of the fourth tuple element.</typeparam>
    /// <typeparam name="TValue5">The type of the fifth tuple element.</typeparam>
    /// <typeparam name="TValue6">The type of the sixth tuple element.</typeparam>
    /// <typeparam name="TValue7">The type of the seventh tuple element.</typeparam>
    /// <typeparam name="TValue8">The type of the eighth tuple element.</typeparam>
/// <param name="result">The source <see cref="Result{T}"/> containing a tuple.</param>
/// <param name="success">The action to execute if the Result is successful, receiving the tuple elements as separate parameters.</param>
/// <param name="failure">The action to execute if the Result is a failure, receiving the error.</param>
/// <returns>The original Result unchanged, allowing for method chaining.</returns>
/// <remarks>
/// OnEither allows you to execute different side effects for both success and failure cases without
/// transforming the Result. This is useful for comprehensive logging, debugging, or triggering
/// different workflows based on the Result state. The original Result is returned unchanged,
/// preserving the error handling chain while allowing observation of both paths.
/// </remarks>
public static Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)> OnEither<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(in this Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)> result, Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> success, Action<Error> failure)
{
    if (result.IsSuccess) success(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, result.Value.Item6, result.Value.Item7, result.Value.Item8);
    else failure(result.Error);
    return result;
}
}
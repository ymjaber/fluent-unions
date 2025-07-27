namespace FluentUnions;

public static partial class ValueResultExtensions
{
    /// <summary>
    /// Combines two Result values by appending their values into a larger tuple, accumulating all errors if any fail.
    /// </summary>
        /// <typeparam name="TSource">The type of the source value.</typeparam>
        /// <typeparam name="TTarget">The type of the target value to append.</typeparam>
    /// <param name="result">The source <see cref="Result{T}"/> containing a value.</param>
    /// <param name="binder">The target <see cref="Result{T}"/> containing a value to append.</param>
    /// <returns>A <see cref="Result{T}"/> containing a tuple with all values if both Results succeed; otherwise, a failure with accumulated errors.</returns>
    /// <remarks>
    /// BindAllAppend differs from BindAppend in its error handling strategy:
    /// - It takes a Result value directly rather than a function returning a Result
    /// - It uses ErrorBuilder to accumulate ALL errors from both Results
    /// - If both Results fail, it returns a composite error containing all error information
    /// - If either Result succeeds while the other fails, it returns the error(s)
    /// - Only if both Results succeed does it combine values into a tuple
    /// 
    /// This is particularly useful in validation scenarios where you want comprehensive error reporting
    /// rather than fail-fast behavior. It enables collecting all validation errors in a single pass
    /// through your data processing pipeline.
    /// </remarks>
    public static Result<(TSource, TTarget)> BindAllAppend<TSource, TTarget>(
        in this Result<TSource> result,
        in Result<TTarget> binder)
    {
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binder)
            .TryBuild(out Error error)
                ? error
                : (result.Value, binder.Value);
    }

    /// <summary>
    /// Combines two Result values by appending their values into a larger tuple, accumulating all errors if any fail.
    /// </summary>
        /// <typeparam name="TSource">The type of the source value.</typeparam>
        /// <typeparam name="TTarget1">The type of the first target tuple element to append.</typeparam>
    /// <typeparam name="TTarget2">The type of the second target tuple element to append.</typeparam>
    /// <param name="result">The source <see cref="Result{T}"/> containing a value.</param>
    /// <param name="binder">The target <see cref="Result{T}"/> containing a tuple with 2 elements to append.</param>
    /// <returns>A <see cref="Result{T}"/> containing a tuple with all values if both Results succeed; otherwise, a failure with accumulated errors.</returns>
    /// <remarks>
    /// BindAllAppend differs from BindAppend in its error handling strategy:
    /// - It takes a Result value directly rather than a function returning a Result
    /// - It uses ErrorBuilder to accumulate ALL errors from both Results
    /// - If both Results fail, it returns a composite error containing all error information
    /// - If either Result succeeds while the other fails, it returns the error(s)
    /// - Only if both Results succeed does it combine values into a tuple
    /// 
    /// This is particularly useful in validation scenarios where you want comprehensive error reporting
    /// rather than fail-fast behavior. It enables collecting all validation errors in a single pass
    /// through your data processing pipeline.
    /// </remarks>
    public static Result<(TSource, TTarget1, TTarget2)> BindAllAppend<TSource, TTarget1, TTarget2>(
        in this Result<TSource> result,
        in Result<(TTarget1, TTarget2)> binder)
    {
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binder)
            .TryBuild(out Error error)
                ? error
                : (result.Value, binder.Value.Item1, binder.Value.Item2);
    }

    /// <summary>
    /// Combines two Result values by appending their values into a larger tuple, accumulating all errors if any fail.
    /// </summary>
        /// <typeparam name="TSource1">The type of the first source tuple element.</typeparam>
    /// <typeparam name="TSource2">The type of the second source tuple element.</typeparam>
        /// <typeparam name="TTarget">The type of the target value to append.</typeparam>
    /// <param name="result">The source <see cref="Result{T}"/> containing a tuple with 2 elements.</param>
    /// <param name="binder">The target <see cref="Result{T}"/> containing a value to append.</param>
    /// <returns>A <see cref="Result{T}"/> containing a tuple with all values if both Results succeed; otherwise, a failure with accumulated errors.</returns>
    /// <remarks>
    /// BindAllAppend differs from BindAppend in its error handling strategy:
    /// - It takes a Result value directly rather than a function returning a Result
    /// - It uses ErrorBuilder to accumulate ALL errors from both Results
    /// - If both Results fail, it returns a composite error containing all error information
    /// - If either Result succeeds while the other fails, it returns the error(s)
    /// - Only if both Results succeed does it combine values into a tuple
    /// 
    /// This is particularly useful in validation scenarios where you want comprehensive error reporting
    /// rather than fail-fast behavior. It enables collecting all validation errors in a single pass
    /// through your data processing pipeline.
    /// </remarks>
    public static Result<(TSource1, TSource2, TTarget)> BindAllAppend<TSource1, TSource2, TTarget>(
        in this Result<(TSource1, TSource2)> result,
        in Result<TTarget> binder)
    {
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binder)
            .TryBuild(out Error error)
                ? error
                : (result.Value.Item1, result.Value.Item2, binder.Value);
    }

    /// <summary>
    /// Combines two Result values by appending their values into a larger tuple, accumulating all errors if any fail.
    /// </summary>
        /// <typeparam name="TSource">The type of the source value.</typeparam>
        /// <typeparam name="TTarget1">The type of the first target tuple element to append.</typeparam>
    /// <typeparam name="TTarget2">The type of the second target tuple element to append.</typeparam>
    /// <typeparam name="TTarget3">The type of the third target tuple element to append.</typeparam>
    /// <param name="result">The source <see cref="Result{T}"/> containing a value.</param>
    /// <param name="binder">The target <see cref="Result{T}"/> containing a tuple with 3 elements to append.</param>
    /// <returns>A <see cref="Result{T}"/> containing a tuple with all values if both Results succeed; otherwise, a failure with accumulated errors.</returns>
    /// <remarks>
    /// BindAllAppend differs from BindAppend in its error handling strategy:
    /// - It takes a Result value directly rather than a function returning a Result
    /// - It uses ErrorBuilder to accumulate ALL errors from both Results
    /// - If both Results fail, it returns a composite error containing all error information
    /// - If either Result succeeds while the other fails, it returns the error(s)
    /// - Only if both Results succeed does it combine values into a tuple
    /// 
    /// This is particularly useful in validation scenarios where you want comprehensive error reporting
    /// rather than fail-fast behavior. It enables collecting all validation errors in a single pass
    /// through your data processing pipeline.
    /// </remarks>
    public static Result<(TSource, TTarget1, TTarget2, TTarget3)> BindAllAppend<TSource, TTarget1, TTarget2, TTarget3>(
        in this Result<TSource> result,
        in Result<(TTarget1, TTarget2, TTarget3)> binder)
    {
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binder)
            .TryBuild(out Error error)
                ? error
                : (result.Value, binder.Value.Item1, binder.Value.Item2, binder.Value.Item3);
    }

    /// <summary>
    /// Combines two Result values by appending their values into a larger tuple, accumulating all errors if any fail.
    /// </summary>
        /// <typeparam name="TSource1">The type of the first source tuple element.</typeparam>
    /// <typeparam name="TSource2">The type of the second source tuple element.</typeparam>
        /// <typeparam name="TTarget1">The type of the first target tuple element to append.</typeparam>
    /// <typeparam name="TTarget2">The type of the second target tuple element to append.</typeparam>
    /// <param name="result">The source <see cref="Result{T}"/> containing a tuple with 2 elements.</param>
    /// <param name="binder">The target <see cref="Result{T}"/> containing a tuple with 2 elements to append.</param>
    /// <returns>A <see cref="Result{T}"/> containing a tuple with all values if both Results succeed; otherwise, a failure with accumulated errors.</returns>
    /// <remarks>
    /// BindAllAppend differs from BindAppend in its error handling strategy:
    /// - It takes a Result value directly rather than a function returning a Result
    /// - It uses ErrorBuilder to accumulate ALL errors from both Results
    /// - If both Results fail, it returns a composite error containing all error information
    /// - If either Result succeeds while the other fails, it returns the error(s)
    /// - Only if both Results succeed does it combine values into a tuple
    /// 
    /// This is particularly useful in validation scenarios where you want comprehensive error reporting
    /// rather than fail-fast behavior. It enables collecting all validation errors in a single pass
    /// through your data processing pipeline.
    /// </remarks>
    public static Result<(TSource1, TSource2, TTarget1, TTarget2)> BindAllAppend<TSource1, TSource2, TTarget1, TTarget2>(
        in this Result<(TSource1, TSource2)> result,
        in Result<(TTarget1, TTarget2)> binder)
    {
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binder)
            .TryBuild(out Error error)
                ? error
                : (result.Value.Item1, result.Value.Item2, binder.Value.Item1, binder.Value.Item2);
    }

    /// <summary>
    /// Combines two Result values by appending their values into a larger tuple, accumulating all errors if any fail.
    /// </summary>
        /// <typeparam name="TSource1">The type of the first source tuple element.</typeparam>
    /// <typeparam name="TSource2">The type of the second source tuple element.</typeparam>
    /// <typeparam name="TSource3">The type of the third source tuple element.</typeparam>
        /// <typeparam name="TTarget">The type of the target value to append.</typeparam>
    /// <param name="result">The source <see cref="Result{T}"/> containing a tuple with 3 elements.</param>
    /// <param name="binder">The target <see cref="Result{T}"/> containing a value to append.</param>
    /// <returns>A <see cref="Result{T}"/> containing a tuple with all values if both Results succeed; otherwise, a failure with accumulated errors.</returns>
    /// <remarks>
    /// BindAllAppend differs from BindAppend in its error handling strategy:
    /// - It takes a Result value directly rather than a function returning a Result
    /// - It uses ErrorBuilder to accumulate ALL errors from both Results
    /// - If both Results fail, it returns a composite error containing all error information
    /// - If either Result succeeds while the other fails, it returns the error(s)
    /// - Only if both Results succeed does it combine values into a tuple
    /// 
    /// This is particularly useful in validation scenarios where you want comprehensive error reporting
    /// rather than fail-fast behavior. It enables collecting all validation errors in a single pass
    /// through your data processing pipeline.
    /// </remarks>
    public static Result<(TSource1, TSource2, TSource3, TTarget)> BindAllAppend<TSource1, TSource2, TSource3, TTarget>(
        in this Result<(TSource1, TSource2, TSource3)> result,
        in Result<TTarget> binder)
    {
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binder)
            .TryBuild(out Error error)
                ? error
                : (result.Value.Item1, result.Value.Item2, result.Value.Item3, binder.Value);
    }

    /// <summary>
    /// Combines two Result values by appending their values into a larger tuple, accumulating all errors if any fail.
    /// </summary>
        /// <typeparam name="TSource">The type of the source value.</typeparam>
        /// <typeparam name="TTarget1">The type of the first target tuple element to append.</typeparam>
    /// <typeparam name="TTarget2">The type of the second target tuple element to append.</typeparam>
    /// <typeparam name="TTarget3">The type of the third target tuple element to append.</typeparam>
    /// <typeparam name="TTarget4">The type of the fourth target tuple element to append.</typeparam>
    /// <param name="result">The source <see cref="Result{T}"/> containing a value.</param>
    /// <param name="binder">The target <see cref="Result{T}"/> containing a tuple with 4 elements to append.</param>
    /// <returns>A <see cref="Result{T}"/> containing a tuple with all values if both Results succeed; otherwise, a failure with accumulated errors.</returns>
    /// <remarks>
    /// BindAllAppend differs from BindAppend in its error handling strategy:
    /// - It takes a Result value directly rather than a function returning a Result
    /// - It uses ErrorBuilder to accumulate ALL errors from both Results
    /// - If both Results fail, it returns a composite error containing all error information
    /// - If either Result succeeds while the other fails, it returns the error(s)
    /// - Only if both Results succeed does it combine values into a tuple
    /// 
    /// This is particularly useful in validation scenarios where you want comprehensive error reporting
    /// rather than fail-fast behavior. It enables collecting all validation errors in a single pass
    /// through your data processing pipeline.
    /// </remarks>
    public static Result<(TSource, TTarget1, TTarget2, TTarget3, TTarget4)> BindAllAppend<TSource, TTarget1, TTarget2, TTarget3, TTarget4>(
        in this Result<TSource> result,
        in Result<(TTarget1, TTarget2, TTarget3, TTarget4)> binder)
    {
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binder)
            .TryBuild(out Error error)
                ? error
                : (result.Value, binder.Value.Item1, binder.Value.Item2, binder.Value.Item3, binder.Value.Item4);
    }

    /// <summary>
    /// Combines two Result values by appending their values into a larger tuple, accumulating all errors if any fail.
    /// </summary>
        /// <typeparam name="TSource1">The type of the first source tuple element.</typeparam>
    /// <typeparam name="TSource2">The type of the second source tuple element.</typeparam>
        /// <typeparam name="TTarget1">The type of the first target tuple element to append.</typeparam>
    /// <typeparam name="TTarget2">The type of the second target tuple element to append.</typeparam>
    /// <typeparam name="TTarget3">The type of the third target tuple element to append.</typeparam>
    /// <param name="result">The source <see cref="Result{T}"/> containing a tuple with 2 elements.</param>
    /// <param name="binder">The target <see cref="Result{T}"/> containing a tuple with 3 elements to append.</param>
    /// <returns>A <see cref="Result{T}"/> containing a tuple with all values if both Results succeed; otherwise, a failure with accumulated errors.</returns>
    /// <remarks>
    /// BindAllAppend differs from BindAppend in its error handling strategy:
    /// - It takes a Result value directly rather than a function returning a Result
    /// - It uses ErrorBuilder to accumulate ALL errors from both Results
    /// - If both Results fail, it returns a composite error containing all error information
    /// - If either Result succeeds while the other fails, it returns the error(s)
    /// - Only if both Results succeed does it combine values into a tuple
    /// 
    /// This is particularly useful in validation scenarios where you want comprehensive error reporting
    /// rather than fail-fast behavior. It enables collecting all validation errors in a single pass
    /// through your data processing pipeline.
    /// </remarks>
    public static Result<(TSource1, TSource2, TTarget1, TTarget2, TTarget3)> BindAllAppend<TSource1, TSource2, TTarget1, TTarget2, TTarget3>(
        in this Result<(TSource1, TSource2)> result,
        in Result<(TTarget1, TTarget2, TTarget3)> binder)
    {
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binder)
            .TryBuild(out Error error)
                ? error
                : (result.Value.Item1, result.Value.Item2, binder.Value.Item1, binder.Value.Item2, binder.Value.Item3);
    }

    /// <summary>
    /// Combines two Result values by appending their values into a larger tuple, accumulating all errors if any fail.
    /// </summary>
        /// <typeparam name="TSource1">The type of the first source tuple element.</typeparam>
    /// <typeparam name="TSource2">The type of the second source tuple element.</typeparam>
    /// <typeparam name="TSource3">The type of the third source tuple element.</typeparam>
        /// <typeparam name="TTarget1">The type of the first target tuple element to append.</typeparam>
    /// <typeparam name="TTarget2">The type of the second target tuple element to append.</typeparam>
    /// <param name="result">The source <see cref="Result{T}"/> containing a tuple with 3 elements.</param>
    /// <param name="binder">The target <see cref="Result{T}"/> containing a tuple with 2 elements to append.</param>
    /// <returns>A <see cref="Result{T}"/> containing a tuple with all values if both Results succeed; otherwise, a failure with accumulated errors.</returns>
    /// <remarks>
    /// BindAllAppend differs from BindAppend in its error handling strategy:
    /// - It takes a Result value directly rather than a function returning a Result
    /// - It uses ErrorBuilder to accumulate ALL errors from both Results
    /// - If both Results fail, it returns a composite error containing all error information
    /// - If either Result succeeds while the other fails, it returns the error(s)
    /// - Only if both Results succeed does it combine values into a tuple
    /// 
    /// This is particularly useful in validation scenarios where you want comprehensive error reporting
    /// rather than fail-fast behavior. It enables collecting all validation errors in a single pass
    /// through your data processing pipeline.
    /// </remarks>
    public static Result<(TSource1, TSource2, TSource3, TTarget1, TTarget2)> BindAllAppend<TSource1, TSource2, TSource3, TTarget1, TTarget2>(
        in this Result<(TSource1, TSource2, TSource3)> result,
        in Result<(TTarget1, TTarget2)> binder)
    {
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binder)
            .TryBuild(out Error error)
                ? error
                : (result.Value.Item1, result.Value.Item2, result.Value.Item3, binder.Value.Item1, binder.Value.Item2);
    }

    /// <summary>
    /// Combines two Result values by appending their values into a larger tuple, accumulating all errors if any fail.
    /// </summary>
        /// <typeparam name="TSource1">The type of the first source tuple element.</typeparam>
    /// <typeparam name="TSource2">The type of the second source tuple element.</typeparam>
    /// <typeparam name="TSource3">The type of the third source tuple element.</typeparam>
    /// <typeparam name="TSource4">The type of the fourth source tuple element.</typeparam>
        /// <typeparam name="TTarget">The type of the target value to append.</typeparam>
    /// <param name="result">The source <see cref="Result{T}"/> containing a tuple with 4 elements.</param>
    /// <param name="binder">The target <see cref="Result{T}"/> containing a value to append.</param>
    /// <returns>A <see cref="Result{T}"/> containing a tuple with all values if both Results succeed; otherwise, a failure with accumulated errors.</returns>
    /// <remarks>
    /// BindAllAppend differs from BindAppend in its error handling strategy:
    /// - It takes a Result value directly rather than a function returning a Result
    /// - It uses ErrorBuilder to accumulate ALL errors from both Results
    /// - If both Results fail, it returns a composite error containing all error information
    /// - If either Result succeeds while the other fails, it returns the error(s)
    /// - Only if both Results succeed does it combine values into a tuple
    /// 
    /// This is particularly useful in validation scenarios where you want comprehensive error reporting
    /// rather than fail-fast behavior. It enables collecting all validation errors in a single pass
    /// through your data processing pipeline.
    /// </remarks>
    public static Result<(TSource1, TSource2, TSource3, TSource4, TTarget)> BindAllAppend<TSource1, TSource2, TSource3, TSource4, TTarget>(
        in this Result<(TSource1, TSource2, TSource3, TSource4)> result,
        in Result<TTarget> binder)
    {
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binder)
            .TryBuild(out Error error)
                ? error
                : (result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, binder.Value);
    }

    /// <summary>
    /// Combines two Result values by appending their values into a larger tuple, accumulating all errors if any fail.
    /// </summary>
        /// <typeparam name="TSource">The type of the source value.</typeparam>
        /// <typeparam name="TTarget1">The type of the first target tuple element to append.</typeparam>
    /// <typeparam name="TTarget2">The type of the second target tuple element to append.</typeparam>
    /// <typeparam name="TTarget3">The type of the third target tuple element to append.</typeparam>
    /// <typeparam name="TTarget4">The type of the fourth target tuple element to append.</typeparam>
    /// <typeparam name="TTarget5">The type of the fifth target tuple element to append.</typeparam>
    /// <param name="result">The source <see cref="Result{T}"/> containing a value.</param>
    /// <param name="binder">The target <see cref="Result{T}"/> containing a tuple with 5 elements to append.</param>
    /// <returns>A <see cref="Result{T}"/> containing a tuple with all values if both Results succeed; otherwise, a failure with accumulated errors.</returns>
    /// <remarks>
    /// BindAllAppend differs from BindAppend in its error handling strategy:
    /// - It takes a Result value directly rather than a function returning a Result
    /// - It uses ErrorBuilder to accumulate ALL errors from both Results
    /// - If both Results fail, it returns a composite error containing all error information
    /// - If either Result succeeds while the other fails, it returns the error(s)
    /// - Only if both Results succeed does it combine values into a tuple
    /// 
    /// This is particularly useful in validation scenarios where you want comprehensive error reporting
    /// rather than fail-fast behavior. It enables collecting all validation errors in a single pass
    /// through your data processing pipeline.
    /// </remarks>
    public static Result<(TSource, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5)> BindAllAppend<TSource, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5>(
        in this Result<TSource> result,
        in Result<(TTarget1, TTarget2, TTarget3, TTarget4, TTarget5)> binder)
    {
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binder)
            .TryBuild(out Error error)
                ? error
                : (result.Value, binder.Value.Item1, binder.Value.Item2, binder.Value.Item3, binder.Value.Item4, binder.Value.Item5);
    }

    /// <summary>
    /// Combines two Result values by appending their values into a larger tuple, accumulating all errors if any fail.
    /// </summary>
        /// <typeparam name="TSource1">The type of the first source tuple element.</typeparam>
    /// <typeparam name="TSource2">The type of the second source tuple element.</typeparam>
        /// <typeparam name="TTarget1">The type of the first target tuple element to append.</typeparam>
    /// <typeparam name="TTarget2">The type of the second target tuple element to append.</typeparam>
    /// <typeparam name="TTarget3">The type of the third target tuple element to append.</typeparam>
    /// <typeparam name="TTarget4">The type of the fourth target tuple element to append.</typeparam>
    /// <param name="result">The source <see cref="Result{T}"/> containing a tuple with 2 elements.</param>
    /// <param name="binder">The target <see cref="Result{T}"/> containing a tuple with 4 elements to append.</param>
    /// <returns>A <see cref="Result{T}"/> containing a tuple with all values if both Results succeed; otherwise, a failure with accumulated errors.</returns>
    /// <remarks>
    /// BindAllAppend differs from BindAppend in its error handling strategy:
    /// - It takes a Result value directly rather than a function returning a Result
    /// - It uses ErrorBuilder to accumulate ALL errors from both Results
    /// - If both Results fail, it returns a composite error containing all error information
    /// - If either Result succeeds while the other fails, it returns the error(s)
    /// - Only if both Results succeed does it combine values into a tuple
    /// 
    /// This is particularly useful in validation scenarios where you want comprehensive error reporting
    /// rather than fail-fast behavior. It enables collecting all validation errors in a single pass
    /// through your data processing pipeline.
    /// </remarks>
    public static Result<(TSource1, TSource2, TTarget1, TTarget2, TTarget3, TTarget4)> BindAllAppend<TSource1, TSource2, TTarget1, TTarget2, TTarget3, TTarget4>(
        in this Result<(TSource1, TSource2)> result,
        in Result<(TTarget1, TTarget2, TTarget3, TTarget4)> binder)
    {
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binder)
            .TryBuild(out Error error)
                ? error
                : (result.Value.Item1, result.Value.Item2, binder.Value.Item1, binder.Value.Item2, binder.Value.Item3, binder.Value.Item4);
    }

    /// <summary>
    /// Combines two Result values by appending their values into a larger tuple, accumulating all errors if any fail.
    /// </summary>
        /// <typeparam name="TSource1">The type of the first source tuple element.</typeparam>
    /// <typeparam name="TSource2">The type of the second source tuple element.</typeparam>
    /// <typeparam name="TSource3">The type of the third source tuple element.</typeparam>
        /// <typeparam name="TTarget1">The type of the first target tuple element to append.</typeparam>
    /// <typeparam name="TTarget2">The type of the second target tuple element to append.</typeparam>
    /// <typeparam name="TTarget3">The type of the third target tuple element to append.</typeparam>
    /// <param name="result">The source <see cref="Result{T}"/> containing a tuple with 3 elements.</param>
    /// <param name="binder">The target <see cref="Result{T}"/> containing a tuple with 3 elements to append.</param>
    /// <returns>A <see cref="Result{T}"/> containing a tuple with all values if both Results succeed; otherwise, a failure with accumulated errors.</returns>
    /// <remarks>
    /// BindAllAppend differs from BindAppend in its error handling strategy:
    /// - It takes a Result value directly rather than a function returning a Result
    /// - It uses ErrorBuilder to accumulate ALL errors from both Results
    /// - If both Results fail, it returns a composite error containing all error information
    /// - If either Result succeeds while the other fails, it returns the error(s)
    /// - Only if both Results succeed does it combine values into a tuple
    /// 
    /// This is particularly useful in validation scenarios where you want comprehensive error reporting
    /// rather than fail-fast behavior. It enables collecting all validation errors in a single pass
    /// through your data processing pipeline.
    /// </remarks>
    public static Result<(TSource1, TSource2, TSource3, TTarget1, TTarget2, TTarget3)> BindAllAppend<TSource1, TSource2, TSource3, TTarget1, TTarget2, TTarget3>(
        in this Result<(TSource1, TSource2, TSource3)> result,
        in Result<(TTarget1, TTarget2, TTarget3)> binder)
    {
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binder)
            .TryBuild(out Error error)
                ? error
                : (result.Value.Item1, result.Value.Item2, result.Value.Item3, binder.Value.Item1, binder.Value.Item2, binder.Value.Item3);
    }

    /// <summary>
    /// Combines two Result values by appending their values into a larger tuple, accumulating all errors if any fail.
    /// </summary>
        /// <typeparam name="TSource1">The type of the first source tuple element.</typeparam>
    /// <typeparam name="TSource2">The type of the second source tuple element.</typeparam>
    /// <typeparam name="TSource3">The type of the third source tuple element.</typeparam>
    /// <typeparam name="TSource4">The type of the fourth source tuple element.</typeparam>
        /// <typeparam name="TTarget1">The type of the first target tuple element to append.</typeparam>
    /// <typeparam name="TTarget2">The type of the second target tuple element to append.</typeparam>
    /// <param name="result">The source <see cref="Result{T}"/> containing a tuple with 4 elements.</param>
    /// <param name="binder">The target <see cref="Result{T}"/> containing a tuple with 2 elements to append.</param>
    /// <returns>A <see cref="Result{T}"/> containing a tuple with all values if both Results succeed; otherwise, a failure with accumulated errors.</returns>
    /// <remarks>
    /// BindAllAppend differs from BindAppend in its error handling strategy:
    /// - It takes a Result value directly rather than a function returning a Result
    /// - It uses ErrorBuilder to accumulate ALL errors from both Results
    /// - If both Results fail, it returns a composite error containing all error information
    /// - If either Result succeeds while the other fails, it returns the error(s)
    /// - Only if both Results succeed does it combine values into a tuple
    /// 
    /// This is particularly useful in validation scenarios where you want comprehensive error reporting
    /// rather than fail-fast behavior. It enables collecting all validation errors in a single pass
    /// through your data processing pipeline.
    /// </remarks>
    public static Result<(TSource1, TSource2, TSource3, TSource4, TTarget1, TTarget2)> BindAllAppend<TSource1, TSource2, TSource3, TSource4, TTarget1, TTarget2>(
        in this Result<(TSource1, TSource2, TSource3, TSource4)> result,
        in Result<(TTarget1, TTarget2)> binder)
    {
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binder)
            .TryBuild(out Error error)
                ? error
                : (result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, binder.Value.Item1, binder.Value.Item2);
    }

    /// <summary>
    /// Combines two Result values by appending their values into a larger tuple, accumulating all errors if any fail.
    /// </summary>
        /// <typeparam name="TSource1">The type of the first source tuple element.</typeparam>
    /// <typeparam name="TSource2">The type of the second source tuple element.</typeparam>
    /// <typeparam name="TSource3">The type of the third source tuple element.</typeparam>
    /// <typeparam name="TSource4">The type of the fourth source tuple element.</typeparam>
    /// <typeparam name="TSource5">The type of the fifth source tuple element.</typeparam>
        /// <typeparam name="TTarget">The type of the target value to append.</typeparam>
    /// <param name="result">The source <see cref="Result{T}"/> containing a tuple with 5 elements.</param>
    /// <param name="binder">The target <see cref="Result{T}"/> containing a value to append.</param>
    /// <returns>A <see cref="Result{T}"/> containing a tuple with all values if both Results succeed; otherwise, a failure with accumulated errors.</returns>
    /// <remarks>
    /// BindAllAppend differs from BindAppend in its error handling strategy:
    /// - It takes a Result value directly rather than a function returning a Result
    /// - It uses ErrorBuilder to accumulate ALL errors from both Results
    /// - If both Results fail, it returns a composite error containing all error information
    /// - If either Result succeeds while the other fails, it returns the error(s)
    /// - Only if both Results succeed does it combine values into a tuple
    /// 
    /// This is particularly useful in validation scenarios where you want comprehensive error reporting
    /// rather than fail-fast behavior. It enables collecting all validation errors in a single pass
    /// through your data processing pipeline.
    /// </remarks>
    public static Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TTarget)> BindAllAppend<TSource1, TSource2, TSource3, TSource4, TSource5, TTarget>(
        in this Result<(TSource1, TSource2, TSource3, TSource4, TSource5)> result,
        in Result<TTarget> binder)
    {
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binder)
            .TryBuild(out Error error)
                ? error
                : (result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, binder.Value);
    }

    /// <summary>
    /// Combines two Result values by appending their values into a larger tuple, accumulating all errors if any fail.
    /// </summary>
        /// <typeparam name="TSource">The type of the source value.</typeparam>
        /// <typeparam name="TTarget1">The type of the first target tuple element to append.</typeparam>
    /// <typeparam name="TTarget2">The type of the second target tuple element to append.</typeparam>
    /// <typeparam name="TTarget3">The type of the third target tuple element to append.</typeparam>
    /// <typeparam name="TTarget4">The type of the fourth target tuple element to append.</typeparam>
    /// <typeparam name="TTarget5">The type of the fifth target tuple element to append.</typeparam>
    /// <typeparam name="TTarget6">The type of the sixth target tuple element to append.</typeparam>
    /// <param name="result">The source <see cref="Result{T}"/> containing a value.</param>
    /// <param name="binder">The target <see cref="Result{T}"/> containing a tuple with 6 elements to append.</param>
    /// <returns>A <see cref="Result{T}"/> containing a tuple with all values if both Results succeed; otherwise, a failure with accumulated errors.</returns>
    /// <remarks>
    /// BindAllAppend differs from BindAppend in its error handling strategy:
    /// - It takes a Result value directly rather than a function returning a Result
    /// - It uses ErrorBuilder to accumulate ALL errors from both Results
    /// - If both Results fail, it returns a composite error containing all error information
    /// - If either Result succeeds while the other fails, it returns the error(s)
    /// - Only if both Results succeed does it combine values into a tuple
    /// 
    /// This is particularly useful in validation scenarios where you want comprehensive error reporting
    /// rather than fail-fast behavior. It enables collecting all validation errors in a single pass
    /// through your data processing pipeline.
    /// </remarks>
    public static Result<(TSource, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6)> BindAllAppend<TSource, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6>(
        in this Result<TSource> result,
        in Result<(TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6)> binder)
    {
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binder)
            .TryBuild(out Error error)
                ? error
                : (result.Value, binder.Value.Item1, binder.Value.Item2, binder.Value.Item3, binder.Value.Item4, binder.Value.Item5, binder.Value.Item6);
    }

    /// <summary>
    /// Combines two Result values by appending their values into a larger tuple, accumulating all errors if any fail.
    /// </summary>
        /// <typeparam name="TSource1">The type of the first source tuple element.</typeparam>
    /// <typeparam name="TSource2">The type of the second source tuple element.</typeparam>
        /// <typeparam name="TTarget1">The type of the first target tuple element to append.</typeparam>
    /// <typeparam name="TTarget2">The type of the second target tuple element to append.</typeparam>
    /// <typeparam name="TTarget3">The type of the third target tuple element to append.</typeparam>
    /// <typeparam name="TTarget4">The type of the fourth target tuple element to append.</typeparam>
    /// <typeparam name="TTarget5">The type of the fifth target tuple element to append.</typeparam>
    /// <param name="result">The source <see cref="Result{T}"/> containing a tuple with 2 elements.</param>
    /// <param name="binder">The target <see cref="Result{T}"/> containing a tuple with 5 elements to append.</param>
    /// <returns>A <see cref="Result{T}"/> containing a tuple with all values if both Results succeed; otherwise, a failure with accumulated errors.</returns>
    /// <remarks>
    /// BindAllAppend differs from BindAppend in its error handling strategy:
    /// - It takes a Result value directly rather than a function returning a Result
    /// - It uses ErrorBuilder to accumulate ALL errors from both Results
    /// - If both Results fail, it returns a composite error containing all error information
    /// - If either Result succeeds while the other fails, it returns the error(s)
    /// - Only if both Results succeed does it combine values into a tuple
    /// 
    /// This is particularly useful in validation scenarios where you want comprehensive error reporting
    /// rather than fail-fast behavior. It enables collecting all validation errors in a single pass
    /// through your data processing pipeline.
    /// </remarks>
    public static Result<(TSource1, TSource2, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5)> BindAllAppend<TSource1, TSource2, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5>(
        in this Result<(TSource1, TSource2)> result,
        in Result<(TTarget1, TTarget2, TTarget3, TTarget4, TTarget5)> binder)
    {
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binder)
            .TryBuild(out Error error)
                ? error
                : (result.Value.Item1, result.Value.Item2, binder.Value.Item1, binder.Value.Item2, binder.Value.Item3, binder.Value.Item4, binder.Value.Item5);
    }

    /// <summary>
    /// Combines two Result values by appending their values into a larger tuple, accumulating all errors if any fail.
    /// </summary>
        /// <typeparam name="TSource1">The type of the first source tuple element.</typeparam>
    /// <typeparam name="TSource2">The type of the second source tuple element.</typeparam>
    /// <typeparam name="TSource3">The type of the third source tuple element.</typeparam>
        /// <typeparam name="TTarget1">The type of the first target tuple element to append.</typeparam>
    /// <typeparam name="TTarget2">The type of the second target tuple element to append.</typeparam>
    /// <typeparam name="TTarget3">The type of the third target tuple element to append.</typeparam>
    /// <typeparam name="TTarget4">The type of the fourth target tuple element to append.</typeparam>
    /// <param name="result">The source <see cref="Result{T}"/> containing a tuple with 3 elements.</param>
    /// <param name="binder">The target <see cref="Result{T}"/> containing a tuple with 4 elements to append.</param>
    /// <returns>A <see cref="Result{T}"/> containing a tuple with all values if both Results succeed; otherwise, a failure with accumulated errors.</returns>
    /// <remarks>
    /// BindAllAppend differs from BindAppend in its error handling strategy:
    /// - It takes a Result value directly rather than a function returning a Result
    /// - It uses ErrorBuilder to accumulate ALL errors from both Results
    /// - If both Results fail, it returns a composite error containing all error information
    /// - If either Result succeeds while the other fails, it returns the error(s)
    /// - Only if both Results succeed does it combine values into a tuple
    /// 
    /// This is particularly useful in validation scenarios where you want comprehensive error reporting
    /// rather than fail-fast behavior. It enables collecting all validation errors in a single pass
    /// through your data processing pipeline.
    /// </remarks>
    public static Result<(TSource1, TSource2, TSource3, TTarget1, TTarget2, TTarget3, TTarget4)> BindAllAppend<TSource1, TSource2, TSource3, TTarget1, TTarget2, TTarget3, TTarget4>(
        in this Result<(TSource1, TSource2, TSource3)> result,
        in Result<(TTarget1, TTarget2, TTarget3, TTarget4)> binder)
    {
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binder)
            .TryBuild(out Error error)
                ? error
                : (result.Value.Item1, result.Value.Item2, result.Value.Item3, binder.Value.Item1, binder.Value.Item2, binder.Value.Item3, binder.Value.Item4);
    }

    /// <summary>
    /// Combines two Result values by appending their values into a larger tuple, accumulating all errors if any fail.
    /// </summary>
        /// <typeparam name="TSource1">The type of the first source tuple element.</typeparam>
    /// <typeparam name="TSource2">The type of the second source tuple element.</typeparam>
    /// <typeparam name="TSource3">The type of the third source tuple element.</typeparam>
    /// <typeparam name="TSource4">The type of the fourth source tuple element.</typeparam>
        /// <typeparam name="TTarget1">The type of the first target tuple element to append.</typeparam>
    /// <typeparam name="TTarget2">The type of the second target tuple element to append.</typeparam>
    /// <typeparam name="TTarget3">The type of the third target tuple element to append.</typeparam>
    /// <param name="result">The source <see cref="Result{T}"/> containing a tuple with 4 elements.</param>
    /// <param name="binder">The target <see cref="Result{T}"/> containing a tuple with 3 elements to append.</param>
    /// <returns>A <see cref="Result{T}"/> containing a tuple with all values if both Results succeed; otherwise, a failure with accumulated errors.</returns>
    /// <remarks>
    /// BindAllAppend differs from BindAppend in its error handling strategy:
    /// - It takes a Result value directly rather than a function returning a Result
    /// - It uses ErrorBuilder to accumulate ALL errors from both Results
    /// - If both Results fail, it returns a composite error containing all error information
    /// - If either Result succeeds while the other fails, it returns the error(s)
    /// - Only if both Results succeed does it combine values into a tuple
    /// 
    /// This is particularly useful in validation scenarios where you want comprehensive error reporting
    /// rather than fail-fast behavior. It enables collecting all validation errors in a single pass
    /// through your data processing pipeline.
    /// </remarks>
    public static Result<(TSource1, TSource2, TSource3, TSource4, TTarget1, TTarget2, TTarget3)> BindAllAppend<TSource1, TSource2, TSource3, TSource4, TTarget1, TTarget2, TTarget3>(
        in this Result<(TSource1, TSource2, TSource3, TSource4)> result,
        in Result<(TTarget1, TTarget2, TTarget3)> binder)
    {
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binder)
            .TryBuild(out Error error)
                ? error
                : (result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, binder.Value.Item1, binder.Value.Item2, binder.Value.Item3);
    }

    /// <summary>
    /// Combines two Result values by appending their values into a larger tuple, accumulating all errors if any fail.
    /// </summary>
        /// <typeparam name="TSource1">The type of the first source tuple element.</typeparam>
    /// <typeparam name="TSource2">The type of the second source tuple element.</typeparam>
    /// <typeparam name="TSource3">The type of the third source tuple element.</typeparam>
    /// <typeparam name="TSource4">The type of the fourth source tuple element.</typeparam>
    /// <typeparam name="TSource5">The type of the fifth source tuple element.</typeparam>
        /// <typeparam name="TTarget1">The type of the first target tuple element to append.</typeparam>
    /// <typeparam name="TTarget2">The type of the second target tuple element to append.</typeparam>
    /// <param name="result">The source <see cref="Result{T}"/> containing a tuple with 5 elements.</param>
    /// <param name="binder">The target <see cref="Result{T}"/> containing a tuple with 2 elements to append.</param>
    /// <returns>A <see cref="Result{T}"/> containing a tuple with all values if both Results succeed; otherwise, a failure with accumulated errors.</returns>
    /// <remarks>
    /// BindAllAppend differs from BindAppend in its error handling strategy:
    /// - It takes a Result value directly rather than a function returning a Result
    /// - It uses ErrorBuilder to accumulate ALL errors from both Results
    /// - If both Results fail, it returns a composite error containing all error information
    /// - If either Result succeeds while the other fails, it returns the error(s)
    /// - Only if both Results succeed does it combine values into a tuple
    /// 
    /// This is particularly useful in validation scenarios where you want comprehensive error reporting
    /// rather than fail-fast behavior. It enables collecting all validation errors in a single pass
    /// through your data processing pipeline.
    /// </remarks>
    public static Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TTarget1, TTarget2)> BindAllAppend<TSource1, TSource2, TSource3, TSource4, TSource5, TTarget1, TTarget2>(
        in this Result<(TSource1, TSource2, TSource3, TSource4, TSource5)> result,
        in Result<(TTarget1, TTarget2)> binder)
    {
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binder)
            .TryBuild(out Error error)
                ? error
                : (result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, binder.Value.Item1, binder.Value.Item2);
    }

    /// <summary>
    /// Combines two Result values by appending their values into a larger tuple, accumulating all errors if any fail.
    /// </summary>
        /// <typeparam name="TSource1">The type of the first source tuple element.</typeparam>
    /// <typeparam name="TSource2">The type of the second source tuple element.</typeparam>
    /// <typeparam name="TSource3">The type of the third source tuple element.</typeparam>
    /// <typeparam name="TSource4">The type of the fourth source tuple element.</typeparam>
    /// <typeparam name="TSource5">The type of the fifth source tuple element.</typeparam>
    /// <typeparam name="TSource6">The type of the sixth source tuple element.</typeparam>
        /// <typeparam name="TTarget">The type of the target value to append.</typeparam>
    /// <param name="result">The source <see cref="Result{T}"/> containing a tuple with 6 elements.</param>
    /// <param name="binder">The target <see cref="Result{T}"/> containing a value to append.</param>
    /// <returns>A <see cref="Result{T}"/> containing a tuple with all values if both Results succeed; otherwise, a failure with accumulated errors.</returns>
    /// <remarks>
    /// BindAllAppend differs from BindAppend in its error handling strategy:
    /// - It takes a Result value directly rather than a function returning a Result
    /// - It uses ErrorBuilder to accumulate ALL errors from both Results
    /// - If both Results fail, it returns a composite error containing all error information
    /// - If either Result succeeds while the other fails, it returns the error(s)
    /// - Only if both Results succeed does it combine values into a tuple
    /// 
    /// This is particularly useful in validation scenarios where you want comprehensive error reporting
    /// rather than fail-fast behavior. It enables collecting all validation errors in a single pass
    /// through your data processing pipeline.
    /// </remarks>
    public static Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TTarget)> BindAllAppend<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TTarget>(
        in this Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6)> result,
        in Result<TTarget> binder)
    {
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binder)
            .TryBuild(out Error error)
                ? error
                : (result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, result.Value.Item6, binder.Value);
    }

    /// <summary>
    /// Combines two Result values by appending their values into a larger tuple, accumulating all errors if any fail.
    /// </summary>
        /// <typeparam name="TSource">The type of the source value.</typeparam>
        /// <typeparam name="TTarget1">The type of the first target tuple element to append.</typeparam>
    /// <typeparam name="TTarget2">The type of the second target tuple element to append.</typeparam>
    /// <typeparam name="TTarget3">The type of the third target tuple element to append.</typeparam>
    /// <typeparam name="TTarget4">The type of the fourth target tuple element to append.</typeparam>
    /// <typeparam name="TTarget5">The type of the fifth target tuple element to append.</typeparam>
    /// <typeparam name="TTarget6">The type of the sixth target tuple element to append.</typeparam>
    /// <typeparam name="TTarget7">The type of the seventh target tuple element to append.</typeparam>
    /// <param name="result">The source <see cref="Result{T}"/> containing a value.</param>
    /// <param name="binder">The target <see cref="Result{T}"/> containing a tuple with 7 elements to append.</param>
    /// <returns>A <see cref="Result{T}"/> containing a tuple with all values if both Results succeed; otherwise, a failure with accumulated errors.</returns>
    /// <remarks>
    /// BindAllAppend differs from BindAppend in its error handling strategy:
    /// - It takes a Result value directly rather than a function returning a Result
    /// - It uses ErrorBuilder to accumulate ALL errors from both Results
    /// - If both Results fail, it returns a composite error containing all error information
    /// - If either Result succeeds while the other fails, it returns the error(s)
    /// - Only if both Results succeed does it combine values into a tuple
    /// 
    /// This is particularly useful in validation scenarios where you want comprehensive error reporting
    /// rather than fail-fast behavior. It enables collecting all validation errors in a single pass
    /// through your data processing pipeline.
    /// </remarks>
    public static Result<(TSource, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6, TTarget7)> BindAllAppend<TSource, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6, TTarget7>(
        in this Result<TSource> result,
        in Result<(TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6, TTarget7)> binder)
    {
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binder)
            .TryBuild(out Error error)
                ? error
                : (result.Value, binder.Value.Item1, binder.Value.Item2, binder.Value.Item3, binder.Value.Item4, binder.Value.Item5, binder.Value.Item6, binder.Value.Item7);
    }

    /// <summary>
    /// Combines two Result values by appending their values into a larger tuple, accumulating all errors if any fail.
    /// </summary>
        /// <typeparam name="TSource1">The type of the first source tuple element.</typeparam>
    /// <typeparam name="TSource2">The type of the second source tuple element.</typeparam>
        /// <typeparam name="TTarget1">The type of the first target tuple element to append.</typeparam>
    /// <typeparam name="TTarget2">The type of the second target tuple element to append.</typeparam>
    /// <typeparam name="TTarget3">The type of the third target tuple element to append.</typeparam>
    /// <typeparam name="TTarget4">The type of the fourth target tuple element to append.</typeparam>
    /// <typeparam name="TTarget5">The type of the fifth target tuple element to append.</typeparam>
    /// <typeparam name="TTarget6">The type of the sixth target tuple element to append.</typeparam>
    /// <param name="result">The source <see cref="Result{T}"/> containing a tuple with 2 elements.</param>
    /// <param name="binder">The target <see cref="Result{T}"/> containing a tuple with 6 elements to append.</param>
    /// <returns>A <see cref="Result{T}"/> containing a tuple with all values if both Results succeed; otherwise, a failure with accumulated errors.</returns>
    /// <remarks>
    /// BindAllAppend differs from BindAppend in its error handling strategy:
    /// - It takes a Result value directly rather than a function returning a Result
    /// - It uses ErrorBuilder to accumulate ALL errors from both Results
    /// - If both Results fail, it returns a composite error containing all error information
    /// - If either Result succeeds while the other fails, it returns the error(s)
    /// - Only if both Results succeed does it combine values into a tuple
    /// 
    /// This is particularly useful in validation scenarios where you want comprehensive error reporting
    /// rather than fail-fast behavior. It enables collecting all validation errors in a single pass
    /// through your data processing pipeline.
    /// </remarks>
    public static Result<(TSource1, TSource2, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6)> BindAllAppend<TSource1, TSource2, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6>(
        in this Result<(TSource1, TSource2)> result,
        in Result<(TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6)> binder)
    {
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binder)
            .TryBuild(out Error error)
                ? error
                : (result.Value.Item1, result.Value.Item2, binder.Value.Item1, binder.Value.Item2, binder.Value.Item3, binder.Value.Item4, binder.Value.Item5, binder.Value.Item6);
    }

    /// <summary>
    /// Combines two Result values by appending their values into a larger tuple, accumulating all errors if any fail.
    /// </summary>
        /// <typeparam name="TSource1">The type of the first source tuple element.</typeparam>
    /// <typeparam name="TSource2">The type of the second source tuple element.</typeparam>
    /// <typeparam name="TSource3">The type of the third source tuple element.</typeparam>
        /// <typeparam name="TTarget1">The type of the first target tuple element to append.</typeparam>
    /// <typeparam name="TTarget2">The type of the second target tuple element to append.</typeparam>
    /// <typeparam name="TTarget3">The type of the third target tuple element to append.</typeparam>
    /// <typeparam name="TTarget4">The type of the fourth target tuple element to append.</typeparam>
    /// <typeparam name="TTarget5">The type of the fifth target tuple element to append.</typeparam>
    /// <param name="result">The source <see cref="Result{T}"/> containing a tuple with 3 elements.</param>
    /// <param name="binder">The target <see cref="Result{T}"/> containing a tuple with 5 elements to append.</param>
    /// <returns>A <see cref="Result{T}"/> containing a tuple with all values if both Results succeed; otherwise, a failure with accumulated errors.</returns>
    /// <remarks>
    /// BindAllAppend differs from BindAppend in its error handling strategy:
    /// - It takes a Result value directly rather than a function returning a Result
    /// - It uses ErrorBuilder to accumulate ALL errors from both Results
    /// - If both Results fail, it returns a composite error containing all error information
    /// - If either Result succeeds while the other fails, it returns the error(s)
    /// - Only if both Results succeed does it combine values into a tuple
    /// 
    /// This is particularly useful in validation scenarios where you want comprehensive error reporting
    /// rather than fail-fast behavior. It enables collecting all validation errors in a single pass
    /// through your data processing pipeline.
    /// </remarks>
    public static Result<(TSource1, TSource2, TSource3, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5)> BindAllAppend<TSource1, TSource2, TSource3, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5>(
        in this Result<(TSource1, TSource2, TSource3)> result,
        in Result<(TTarget1, TTarget2, TTarget3, TTarget4, TTarget5)> binder)
    {
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binder)
            .TryBuild(out Error error)
                ? error
                : (result.Value.Item1, result.Value.Item2, result.Value.Item3, binder.Value.Item1, binder.Value.Item2, binder.Value.Item3, binder.Value.Item4, binder.Value.Item5);
    }

    /// <summary>
    /// Combines two Result values by appending their values into a larger tuple, accumulating all errors if any fail.
    /// </summary>
        /// <typeparam name="TSource1">The type of the first source tuple element.</typeparam>
    /// <typeparam name="TSource2">The type of the second source tuple element.</typeparam>
    /// <typeparam name="TSource3">The type of the third source tuple element.</typeparam>
    /// <typeparam name="TSource4">The type of the fourth source tuple element.</typeparam>
        /// <typeparam name="TTarget1">The type of the first target tuple element to append.</typeparam>
    /// <typeparam name="TTarget2">The type of the second target tuple element to append.</typeparam>
    /// <typeparam name="TTarget3">The type of the third target tuple element to append.</typeparam>
    /// <typeparam name="TTarget4">The type of the fourth target tuple element to append.</typeparam>
    /// <param name="result">The source <see cref="Result{T}"/> containing a tuple with 4 elements.</param>
    /// <param name="binder">The target <see cref="Result{T}"/> containing a tuple with 4 elements to append.</param>
    /// <returns>A <see cref="Result{T}"/> containing a tuple with all values if both Results succeed; otherwise, a failure with accumulated errors.</returns>
    /// <remarks>
    /// BindAllAppend differs from BindAppend in its error handling strategy:
    /// - It takes a Result value directly rather than a function returning a Result
    /// - It uses ErrorBuilder to accumulate ALL errors from both Results
    /// - If both Results fail, it returns a composite error containing all error information
    /// - If either Result succeeds while the other fails, it returns the error(s)
    /// - Only if both Results succeed does it combine values into a tuple
    /// 
    /// This is particularly useful in validation scenarios where you want comprehensive error reporting
    /// rather than fail-fast behavior. It enables collecting all validation errors in a single pass
    /// through your data processing pipeline.
    /// </remarks>
    public static Result<(TSource1, TSource2, TSource3, TSource4, TTarget1, TTarget2, TTarget3, TTarget4)> BindAllAppend<TSource1, TSource2, TSource3, TSource4, TTarget1, TTarget2, TTarget3, TTarget4>(
        in this Result<(TSource1, TSource2, TSource3, TSource4)> result,
        in Result<(TTarget1, TTarget2, TTarget3, TTarget4)> binder)
    {
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binder)
            .TryBuild(out Error error)
                ? error
                : (result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, binder.Value.Item1, binder.Value.Item2, binder.Value.Item3, binder.Value.Item4);
    }

    /// <summary>
    /// Combines two Result values by appending their values into a larger tuple, accumulating all errors if any fail.
    /// </summary>
        /// <typeparam name="TSource1">The type of the first source tuple element.</typeparam>
    /// <typeparam name="TSource2">The type of the second source tuple element.</typeparam>
    /// <typeparam name="TSource3">The type of the third source tuple element.</typeparam>
    /// <typeparam name="TSource4">The type of the fourth source tuple element.</typeparam>
    /// <typeparam name="TSource5">The type of the fifth source tuple element.</typeparam>
        /// <typeparam name="TTarget1">The type of the first target tuple element to append.</typeparam>
    /// <typeparam name="TTarget2">The type of the second target tuple element to append.</typeparam>
    /// <typeparam name="TTarget3">The type of the third target tuple element to append.</typeparam>
    /// <param name="result">The source <see cref="Result{T}"/> containing a tuple with 5 elements.</param>
    /// <param name="binder">The target <see cref="Result{T}"/> containing a tuple with 3 elements to append.</param>
    /// <returns>A <see cref="Result{T}"/> containing a tuple with all values if both Results succeed; otherwise, a failure with accumulated errors.</returns>
    /// <remarks>
    /// BindAllAppend differs from BindAppend in its error handling strategy:
    /// - It takes a Result value directly rather than a function returning a Result
    /// - It uses ErrorBuilder to accumulate ALL errors from both Results
    /// - If both Results fail, it returns a composite error containing all error information
    /// - If either Result succeeds while the other fails, it returns the error(s)
    /// - Only if both Results succeed does it combine values into a tuple
    /// 
    /// This is particularly useful in validation scenarios where you want comprehensive error reporting
    /// rather than fail-fast behavior. It enables collecting all validation errors in a single pass
    /// through your data processing pipeline.
    /// </remarks>
    public static Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TTarget1, TTarget2, TTarget3)> BindAllAppend<TSource1, TSource2, TSource3, TSource4, TSource5, TTarget1, TTarget2, TTarget3>(
        in this Result<(TSource1, TSource2, TSource3, TSource4, TSource5)> result,
        in Result<(TTarget1, TTarget2, TTarget3)> binder)
    {
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binder)
            .TryBuild(out Error error)
                ? error
                : (result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, binder.Value.Item1, binder.Value.Item2, binder.Value.Item3);
    }

    /// <summary>
    /// Combines two Result values by appending their values into a larger tuple, accumulating all errors if any fail.
    /// </summary>
        /// <typeparam name="TSource1">The type of the first source tuple element.</typeparam>
    /// <typeparam name="TSource2">The type of the second source tuple element.</typeparam>
    /// <typeparam name="TSource3">The type of the third source tuple element.</typeparam>
    /// <typeparam name="TSource4">The type of the fourth source tuple element.</typeparam>
    /// <typeparam name="TSource5">The type of the fifth source tuple element.</typeparam>
    /// <typeparam name="TSource6">The type of the sixth source tuple element.</typeparam>
        /// <typeparam name="TTarget1">The type of the first target tuple element to append.</typeparam>
    /// <typeparam name="TTarget2">The type of the second target tuple element to append.</typeparam>
    /// <param name="result">The source <see cref="Result{T}"/> containing a tuple with 6 elements.</param>
    /// <param name="binder">The target <see cref="Result{T}"/> containing a tuple with 2 elements to append.</param>
    /// <returns>A <see cref="Result{T}"/> containing a tuple with all values if both Results succeed; otherwise, a failure with accumulated errors.</returns>
    /// <remarks>
    /// BindAllAppend differs from BindAppend in its error handling strategy:
    /// - It takes a Result value directly rather than a function returning a Result
    /// - It uses ErrorBuilder to accumulate ALL errors from both Results
    /// - If both Results fail, it returns a composite error containing all error information
    /// - If either Result succeeds while the other fails, it returns the error(s)
    /// - Only if both Results succeed does it combine values into a tuple
    /// 
    /// This is particularly useful in validation scenarios where you want comprehensive error reporting
    /// rather than fail-fast behavior. It enables collecting all validation errors in a single pass
    /// through your data processing pipeline.
    /// </remarks>
    public static Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TTarget1, TTarget2)> BindAllAppend<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TTarget1, TTarget2>(
        in this Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6)> result,
        in Result<(TTarget1, TTarget2)> binder)
    {
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binder)
            .TryBuild(out Error error)
                ? error
                : (result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, result.Value.Item6, binder.Value.Item1, binder.Value.Item2);
    }

    /// <summary>
    /// Combines two Result values by appending their values into a larger tuple, accumulating all errors if any fail.
    /// </summary>
        /// <typeparam name="TSource1">The type of the first source tuple element.</typeparam>
    /// <typeparam name="TSource2">The type of the second source tuple element.</typeparam>
    /// <typeparam name="TSource3">The type of the third source tuple element.</typeparam>
    /// <typeparam name="TSource4">The type of the fourth source tuple element.</typeparam>
    /// <typeparam name="TSource5">The type of the fifth source tuple element.</typeparam>
    /// <typeparam name="TSource6">The type of the sixth source tuple element.</typeparam>
    /// <typeparam name="TSource7">The type of the seventh source tuple element.</typeparam>
        /// <typeparam name="TTarget">The type of the target value to append.</typeparam>
    /// <param name="result">The source <see cref="Result{T}"/> containing a tuple with 7 elements.</param>
    /// <param name="binder">The target <see cref="Result{T}"/> containing a value to append.</param>
    /// <returns>A <see cref="Result{T}"/> containing a tuple with all values if both Results succeed; otherwise, a failure with accumulated errors.</returns>
    /// <remarks>
    /// BindAllAppend differs from BindAppend in its error handling strategy:
    /// - It takes a Result value directly rather than a function returning a Result
    /// - It uses ErrorBuilder to accumulate ALL errors from both Results
    /// - If both Results fail, it returns a composite error containing all error information
    /// - If either Result succeeds while the other fails, it returns the error(s)
    /// - Only if both Results succeed does it combine values into a tuple
    /// 
    /// This is particularly useful in validation scenarios where you want comprehensive error reporting
    /// rather than fail-fast behavior. It enables collecting all validation errors in a single pass
    /// through your data processing pipeline.
    /// </remarks>
    public static Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TTarget)> BindAllAppend<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TTarget>(
        in this Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7)> result,
        in Result<TTarget> binder)
    {
        return new ErrorBuilder()
            .AppendOnFailure(result)
            .AppendOnFailure(binder)
            .TryBuild(out Error error)
                ? error
                : (result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, result.Value.Item6, result.Value.Item7, binder.Value);
    }

}
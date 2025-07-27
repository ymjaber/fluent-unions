namespace FluentUnions;

public static partial class ValueResultExtensions
{
    /// <summary>
    /// Chains a Result-returning operation and appends its value(s) to form a larger tuple Result.
    /// </summary>
        /// <typeparam name="TSource">The type of the source value.</typeparam>
        /// <typeparam name="TTarget">The type of the target value to append.</typeparam>
    /// <param name="result">The source <see cref="Result{T}"/> containing a value.</param>
    /// <param name="binder">A function that takes the source value(s) and returns a Result containing a value to append.</param>
    /// <returns>A <see cref="Result{T}"/> containing a tuple with all source and target values if both operations succeed; otherwise, the first error encountered.</returns>
    /// <remarks>
    /// BindAppend is a monadic bind operation that accumulates values into larger tuples. It's part of the
    /// railway-oriented programming pattern where:
    /// - If the source Result is a failure, that error is propagated immediately
    /// - If the source is successful, the binder function is called with the value(s)
    /// - If the binder returns a failure, that error is propagated
    /// - If both succeed, all values are combined into a larger tuple
    /// This enables building up complex data structures through a series of Result-returning operations
    /// while maintaining proper error handling throughout the chain.
    /// </remarks>
    public static Result<(TSource, TTarget)> BindAppend<TSource, TTarget>(
        in this Result<TSource> result,
        Func<TSource, Result<TTarget>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = binder(result.Value);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value, target);
    }

    /// <summary>
    /// Chains a Result-returning operation and appends its value(s) to form a larger tuple Result.
    /// </summary>
        /// <typeparam name="TSource">The type of the source value.</typeparam>
        /// <typeparam name="TTarget1">The type of the first target tuple element to append.</typeparam>
    /// <typeparam name="TTarget2">The type of the second target tuple element to append.</typeparam>
    /// <param name="result">The source <see cref="Result{T}"/> containing a value.</param>
    /// <param name="binder">A function that takes the source value(s) and returns a Result containing a tuple with 2 elements to append.</param>
    /// <returns>A <see cref="Result{T}"/> containing a tuple with all source and target values if both operations succeed; otherwise, the first error encountered.</returns>
    /// <remarks>
    /// BindAppend is a monadic bind operation that accumulates values into larger tuples. It's part of the
    /// railway-oriented programming pattern where:
    /// - If the source Result is a failure, that error is propagated immediately
    /// - If the source is successful, the binder function is called with the value(s)
    /// - If the binder returns a failure, that error is propagated
    /// - If both succeed, all values are combined into a larger tuple
    /// This enables building up complex data structures through a series of Result-returning operations
    /// while maintaining proper error handling throughout the chain.
    /// </remarks>
    public static Result<(TSource, TTarget1, TTarget2)> BindAppend<TSource, TTarget1, TTarget2>(
        in this Result<TSource> result,
        Func<TSource, Result<(TTarget1, TTarget2)>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = binder(result.Value);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value, target.Item1, target.Item2);
    }

    /// <summary>
    /// Chains a Result-returning operation and appends its value(s) to form a larger tuple Result.
    /// </summary>
        /// <typeparam name="TSource1">The type of the first source tuple element.</typeparam>
    /// <typeparam name="TSource2">The type of the second source tuple element.</typeparam>
        /// <typeparam name="TTarget">The type of the target value to append.</typeparam>
    /// <param name="result">The source <see cref="Result{T}"/> containing a tuple with 2 elements.</param>
    /// <param name="binder">A function that takes the source value(s) and returns a Result containing a value to append.</param>
    /// <returns>A <see cref="Result{T}"/> containing a tuple with all source and target values if both operations succeed; otherwise, the first error encountered.</returns>
    /// <remarks>
    /// BindAppend is a monadic bind operation that accumulates values into larger tuples. It's part of the
    /// railway-oriented programming pattern where:
    /// - If the source Result is a failure, that error is propagated immediately
    /// - If the source is successful, the binder function is called with the value(s)
    /// - If the binder returns a failure, that error is propagated
    /// - If both succeed, all values are combined into a larger tuple
    /// This enables building up complex data structures through a series of Result-returning operations
    /// while maintaining proper error handling throughout the chain.
    /// </remarks>
    public static Result<(TSource1, TSource2, TTarget)> BindAppend<TSource1, TSource2, TTarget>(
        in this Result<(TSource1, TSource2)> result,
        Func<TSource1, TSource2, Result<TTarget>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = binder(result.Value.Item1, result.Value.Item2);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value.Item1, result.Value.Item2, target);
    }

    /// <summary>
    /// Chains a Result-returning operation and appends its value(s) to form a larger tuple Result.
    /// </summary>
        /// <typeparam name="TSource">The type of the source value.</typeparam>
        /// <typeparam name="TTarget1">The type of the first target tuple element to append.</typeparam>
    /// <typeparam name="TTarget2">The type of the second target tuple element to append.</typeparam>
    /// <typeparam name="TTarget3">The type of the third target tuple element to append.</typeparam>
    /// <param name="result">The source <see cref="Result{T}"/> containing a value.</param>
    /// <param name="binder">A function that takes the source value(s) and returns a Result containing a tuple with 3 elements to append.</param>
    /// <returns>A <see cref="Result{T}"/> containing a tuple with all source and target values if both operations succeed; otherwise, the first error encountered.</returns>
    /// <remarks>
    /// BindAppend is a monadic bind operation that accumulates values into larger tuples. It's part of the
    /// railway-oriented programming pattern where:
    /// - If the source Result is a failure, that error is propagated immediately
    /// - If the source is successful, the binder function is called with the value(s)
    /// - If the binder returns a failure, that error is propagated
    /// - If both succeed, all values are combined into a larger tuple
    /// This enables building up complex data structures through a series of Result-returning operations
    /// while maintaining proper error handling throughout the chain.
    /// </remarks>
    public static Result<(TSource, TTarget1, TTarget2, TTarget3)> BindAppend<TSource, TTarget1, TTarget2, TTarget3>(
        in this Result<TSource> result,
        Func<TSource, Result<(TTarget1, TTarget2, TTarget3)>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = binder(result.Value);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value, target.Item1, target.Item2, target.Item3);
    }

    /// <summary>
    /// Chains a Result-returning operation and appends its value(s) to form a larger tuple Result.
    /// </summary>
        /// <typeparam name="TSource1">The type of the first source tuple element.</typeparam>
    /// <typeparam name="TSource2">The type of the second source tuple element.</typeparam>
        /// <typeparam name="TTarget1">The type of the first target tuple element to append.</typeparam>
    /// <typeparam name="TTarget2">The type of the second target tuple element to append.</typeparam>
    /// <param name="result">The source <see cref="Result{T}"/> containing a tuple with 2 elements.</param>
    /// <param name="binder">A function that takes the source value(s) and returns a Result containing a tuple with 2 elements to append.</param>
    /// <returns>A <see cref="Result{T}"/> containing a tuple with all source and target values if both operations succeed; otherwise, the first error encountered.</returns>
    /// <remarks>
    /// BindAppend is a monadic bind operation that accumulates values into larger tuples. It's part of the
    /// railway-oriented programming pattern where:
    /// - If the source Result is a failure, that error is propagated immediately
    /// - If the source is successful, the binder function is called with the value(s)
    /// - If the binder returns a failure, that error is propagated
    /// - If both succeed, all values are combined into a larger tuple
    /// This enables building up complex data structures through a series of Result-returning operations
    /// while maintaining proper error handling throughout the chain.
    /// </remarks>
    public static Result<(TSource1, TSource2, TTarget1, TTarget2)> BindAppend<TSource1, TSource2, TTarget1, TTarget2>(
        in this Result<(TSource1, TSource2)> result,
        Func<TSource1, TSource2, Result<(TTarget1, TTarget2)>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = binder(result.Value.Item1, result.Value.Item2);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value.Item1, result.Value.Item2, target.Item1, target.Item2);
    }

    /// <summary>
    /// Chains a Result-returning operation and appends its value(s) to form a larger tuple Result.
    /// </summary>
        /// <typeparam name="TSource1">The type of the first source tuple element.</typeparam>
    /// <typeparam name="TSource2">The type of the second source tuple element.</typeparam>
    /// <typeparam name="TSource3">The type of the third source tuple element.</typeparam>
        /// <typeparam name="TTarget">The type of the target value to append.</typeparam>
    /// <param name="result">The source <see cref="Result{T}"/> containing a tuple with 3 elements.</param>
    /// <param name="binder">A function that takes the source value(s) and returns a Result containing a value to append.</param>
    /// <returns>A <see cref="Result{T}"/> containing a tuple with all source and target values if both operations succeed; otherwise, the first error encountered.</returns>
    /// <remarks>
    /// BindAppend is a monadic bind operation that accumulates values into larger tuples. It's part of the
    /// railway-oriented programming pattern where:
    /// - If the source Result is a failure, that error is propagated immediately
    /// - If the source is successful, the binder function is called with the value(s)
    /// - If the binder returns a failure, that error is propagated
    /// - If both succeed, all values are combined into a larger tuple
    /// This enables building up complex data structures through a series of Result-returning operations
    /// while maintaining proper error handling throughout the chain.
    /// </remarks>
    public static Result<(TSource1, TSource2, TSource3, TTarget)> BindAppend<TSource1, TSource2, TSource3, TTarget>(
        in this Result<(TSource1, TSource2, TSource3)> result,
        Func<TSource1, TSource2, TSource3, Result<TTarget>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = binder(result.Value.Item1, result.Value.Item2, result.Value.Item3);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value.Item1, result.Value.Item2, result.Value.Item3, target);
    }

    /// <summary>
    /// Chains a Result-returning operation and appends its value(s) to form a larger tuple Result.
    /// </summary>
        /// <typeparam name="TSource">The type of the source value.</typeparam>
        /// <typeparam name="TTarget1">The type of the first target tuple element to append.</typeparam>
    /// <typeparam name="TTarget2">The type of the second target tuple element to append.</typeparam>
    /// <typeparam name="TTarget3">The type of the third target tuple element to append.</typeparam>
    /// <typeparam name="TTarget4">The type of the fourth target tuple element to append.</typeparam>
    /// <param name="result">The source <see cref="Result{T}"/> containing a value.</param>
    /// <param name="binder">A function that takes the source value(s) and returns a Result containing a tuple with 4 elements to append.</param>
    /// <returns>A <see cref="Result{T}"/> containing a tuple with all source and target values if both operations succeed; otherwise, the first error encountered.</returns>
    /// <remarks>
    /// BindAppend is a monadic bind operation that accumulates values into larger tuples. It's part of the
    /// railway-oriented programming pattern where:
    /// - If the source Result is a failure, that error is propagated immediately
    /// - If the source is successful, the binder function is called with the value(s)
    /// - If the binder returns a failure, that error is propagated
    /// - If both succeed, all values are combined into a larger tuple
    /// This enables building up complex data structures through a series of Result-returning operations
    /// while maintaining proper error handling throughout the chain.
    /// </remarks>
    public static Result<(TSource, TTarget1, TTarget2, TTarget3, TTarget4)> BindAppend<TSource, TTarget1, TTarget2, TTarget3, TTarget4>(
        in this Result<TSource> result,
        Func<TSource, Result<(TTarget1, TTarget2, TTarget3, TTarget4)>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = binder(result.Value);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value, target.Item1, target.Item2, target.Item3, target.Item4);
    }

    /// <summary>
    /// Chains a Result-returning operation and appends its value(s) to form a larger tuple Result.
    /// </summary>
        /// <typeparam name="TSource1">The type of the first source tuple element.</typeparam>
    /// <typeparam name="TSource2">The type of the second source tuple element.</typeparam>
        /// <typeparam name="TTarget1">The type of the first target tuple element to append.</typeparam>
    /// <typeparam name="TTarget2">The type of the second target tuple element to append.</typeparam>
    /// <typeparam name="TTarget3">The type of the third target tuple element to append.</typeparam>
    /// <param name="result">The source <see cref="Result{T}"/> containing a tuple with 2 elements.</param>
    /// <param name="binder">A function that takes the source value(s) and returns a Result containing a tuple with 3 elements to append.</param>
    /// <returns>A <see cref="Result{T}"/> containing a tuple with all source and target values if both operations succeed; otherwise, the first error encountered.</returns>
    /// <remarks>
    /// BindAppend is a monadic bind operation that accumulates values into larger tuples. It's part of the
    /// railway-oriented programming pattern where:
    /// - If the source Result is a failure, that error is propagated immediately
    /// - If the source is successful, the binder function is called with the value(s)
    /// - If the binder returns a failure, that error is propagated
    /// - If both succeed, all values are combined into a larger tuple
    /// This enables building up complex data structures through a series of Result-returning operations
    /// while maintaining proper error handling throughout the chain.
    /// </remarks>
    public static Result<(TSource1, TSource2, TTarget1, TTarget2, TTarget3)> BindAppend<TSource1, TSource2, TTarget1, TTarget2, TTarget3>(
        in this Result<(TSource1, TSource2)> result,
        Func<TSource1, TSource2, Result<(TTarget1, TTarget2, TTarget3)>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = binder(result.Value.Item1, result.Value.Item2);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value.Item1, result.Value.Item2, target.Item1, target.Item2, target.Item3);
    }

    /// <summary>
    /// Chains a Result-returning operation and appends its value(s) to form a larger tuple Result.
    /// </summary>
        /// <typeparam name="TSource1">The type of the first source tuple element.</typeparam>
    /// <typeparam name="TSource2">The type of the second source tuple element.</typeparam>
    /// <typeparam name="TSource3">The type of the third source tuple element.</typeparam>
        /// <typeparam name="TTarget1">The type of the first target tuple element to append.</typeparam>
    /// <typeparam name="TTarget2">The type of the second target tuple element to append.</typeparam>
    /// <param name="result">The source <see cref="Result{T}"/> containing a tuple with 3 elements.</param>
    /// <param name="binder">A function that takes the source value(s) and returns a Result containing a tuple with 2 elements to append.</param>
    /// <returns>A <see cref="Result{T}"/> containing a tuple with all source and target values if both operations succeed; otherwise, the first error encountered.</returns>
    /// <remarks>
    /// BindAppend is a monadic bind operation that accumulates values into larger tuples. It's part of the
    /// railway-oriented programming pattern where:
    /// - If the source Result is a failure, that error is propagated immediately
    /// - If the source is successful, the binder function is called with the value(s)
    /// - If the binder returns a failure, that error is propagated
    /// - If both succeed, all values are combined into a larger tuple
    /// This enables building up complex data structures through a series of Result-returning operations
    /// while maintaining proper error handling throughout the chain.
    /// </remarks>
    public static Result<(TSource1, TSource2, TSource3, TTarget1, TTarget2)> BindAppend<TSource1, TSource2, TSource3, TTarget1, TTarget2>(
        in this Result<(TSource1, TSource2, TSource3)> result,
        Func<TSource1, TSource2, TSource3, Result<(TTarget1, TTarget2)>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = binder(result.Value.Item1, result.Value.Item2, result.Value.Item3);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value.Item1, result.Value.Item2, result.Value.Item3, target.Item1, target.Item2);
    }

    /// <summary>
    /// Chains a Result-returning operation and appends its value(s) to form a larger tuple Result.
    /// </summary>
        /// <typeparam name="TSource1">The type of the first source tuple element.</typeparam>
    /// <typeparam name="TSource2">The type of the second source tuple element.</typeparam>
    /// <typeparam name="TSource3">The type of the third source tuple element.</typeparam>
    /// <typeparam name="TSource4">The type of the fourth source tuple element.</typeparam>
        /// <typeparam name="TTarget">The type of the target value to append.</typeparam>
    /// <param name="result">The source <see cref="Result{T}"/> containing a tuple with 4 elements.</param>
    /// <param name="binder">A function that takes the source value(s) and returns a Result containing a value to append.</param>
    /// <returns>A <see cref="Result{T}"/> containing a tuple with all source and target values if both operations succeed; otherwise, the first error encountered.</returns>
    /// <remarks>
    /// BindAppend is a monadic bind operation that accumulates values into larger tuples. It's part of the
    /// railway-oriented programming pattern where:
    /// - If the source Result is a failure, that error is propagated immediately
    /// - If the source is successful, the binder function is called with the value(s)
    /// - If the binder returns a failure, that error is propagated
    /// - If both succeed, all values are combined into a larger tuple
    /// This enables building up complex data structures through a series of Result-returning operations
    /// while maintaining proper error handling throughout the chain.
    /// </remarks>
    public static Result<(TSource1, TSource2, TSource3, TSource4, TTarget)> BindAppend<TSource1, TSource2, TSource3, TSource4, TTarget>(
        in this Result<(TSource1, TSource2, TSource3, TSource4)> result,
        Func<TSource1, TSource2, TSource3, TSource4, Result<TTarget>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = binder(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, target);
    }

    /// <summary>
    /// Chains a Result-returning operation and appends its value(s) to form a larger tuple Result.
    /// </summary>
        /// <typeparam name="TSource">The type of the source value.</typeparam>
        /// <typeparam name="TTarget1">The type of the first target tuple element to append.</typeparam>
    /// <typeparam name="TTarget2">The type of the second target tuple element to append.</typeparam>
    /// <typeparam name="TTarget3">The type of the third target tuple element to append.</typeparam>
    /// <typeparam name="TTarget4">The type of the fourth target tuple element to append.</typeparam>
    /// <typeparam name="TTarget5">The type of the fifth target tuple element to append.</typeparam>
    /// <param name="result">The source <see cref="Result{T}"/> containing a value.</param>
    /// <param name="binder">A function that takes the source value(s) and returns a Result containing a tuple with 5 elements to append.</param>
    /// <returns>A <see cref="Result{T}"/> containing a tuple with all source and target values if both operations succeed; otherwise, the first error encountered.</returns>
    /// <remarks>
    /// BindAppend is a monadic bind operation that accumulates values into larger tuples. It's part of the
    /// railway-oriented programming pattern where:
    /// - If the source Result is a failure, that error is propagated immediately
    /// - If the source is successful, the binder function is called with the value(s)
    /// - If the binder returns a failure, that error is propagated
    /// - If both succeed, all values are combined into a larger tuple
    /// This enables building up complex data structures through a series of Result-returning operations
    /// while maintaining proper error handling throughout the chain.
    /// </remarks>
    public static Result<(TSource, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5)> BindAppend<TSource, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5>(
        in this Result<TSource> result,
        Func<TSource, Result<(TTarget1, TTarget2, TTarget3, TTarget4, TTarget5)>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = binder(result.Value);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value, target.Item1, target.Item2, target.Item3, target.Item4, target.Item5);
    }

    /// <summary>
    /// Chains a Result-returning operation and appends its value(s) to form a larger tuple Result.
    /// </summary>
        /// <typeparam name="TSource1">The type of the first source tuple element.</typeparam>
    /// <typeparam name="TSource2">The type of the second source tuple element.</typeparam>
        /// <typeparam name="TTarget1">The type of the first target tuple element to append.</typeparam>
    /// <typeparam name="TTarget2">The type of the second target tuple element to append.</typeparam>
    /// <typeparam name="TTarget3">The type of the third target tuple element to append.</typeparam>
    /// <typeparam name="TTarget4">The type of the fourth target tuple element to append.</typeparam>
    /// <param name="result">The source <see cref="Result{T}"/> containing a tuple with 2 elements.</param>
    /// <param name="binder">A function that takes the source value(s) and returns a Result containing a tuple with 4 elements to append.</param>
    /// <returns>A <see cref="Result{T}"/> containing a tuple with all source and target values if both operations succeed; otherwise, the first error encountered.</returns>
    /// <remarks>
    /// BindAppend is a monadic bind operation that accumulates values into larger tuples. It's part of the
    /// railway-oriented programming pattern where:
    /// - If the source Result is a failure, that error is propagated immediately
    /// - If the source is successful, the binder function is called with the value(s)
    /// - If the binder returns a failure, that error is propagated
    /// - If both succeed, all values are combined into a larger tuple
    /// This enables building up complex data structures through a series of Result-returning operations
    /// while maintaining proper error handling throughout the chain.
    /// </remarks>
    public static Result<(TSource1, TSource2, TTarget1, TTarget2, TTarget3, TTarget4)> BindAppend<TSource1, TSource2, TTarget1, TTarget2, TTarget3, TTarget4>(
        in this Result<(TSource1, TSource2)> result,
        Func<TSource1, TSource2, Result<(TTarget1, TTarget2, TTarget3, TTarget4)>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = binder(result.Value.Item1, result.Value.Item2);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value.Item1, result.Value.Item2, target.Item1, target.Item2, target.Item3, target.Item4);
    }

    /// <summary>
    /// Chains a Result-returning operation and appends its value(s) to form a larger tuple Result.
    /// </summary>
        /// <typeparam name="TSource1">The type of the first source tuple element.</typeparam>
    /// <typeparam name="TSource2">The type of the second source tuple element.</typeparam>
    /// <typeparam name="TSource3">The type of the third source tuple element.</typeparam>
        /// <typeparam name="TTarget1">The type of the first target tuple element to append.</typeparam>
    /// <typeparam name="TTarget2">The type of the second target tuple element to append.</typeparam>
    /// <typeparam name="TTarget3">The type of the third target tuple element to append.</typeparam>
    /// <param name="result">The source <see cref="Result{T}"/> containing a tuple with 3 elements.</param>
    /// <param name="binder">A function that takes the source value(s) and returns a Result containing a tuple with 3 elements to append.</param>
    /// <returns>A <see cref="Result{T}"/> containing a tuple with all source and target values if both operations succeed; otherwise, the first error encountered.</returns>
    /// <remarks>
    /// BindAppend is a monadic bind operation that accumulates values into larger tuples. It's part of the
    /// railway-oriented programming pattern where:
    /// - If the source Result is a failure, that error is propagated immediately
    /// - If the source is successful, the binder function is called with the value(s)
    /// - If the binder returns a failure, that error is propagated
    /// - If both succeed, all values are combined into a larger tuple
    /// This enables building up complex data structures through a series of Result-returning operations
    /// while maintaining proper error handling throughout the chain.
    /// </remarks>
    public static Result<(TSource1, TSource2, TSource3, TTarget1, TTarget2, TTarget3)> BindAppend<TSource1, TSource2, TSource3, TTarget1, TTarget2, TTarget3>(
        in this Result<(TSource1, TSource2, TSource3)> result,
        Func<TSource1, TSource2, TSource3, Result<(TTarget1, TTarget2, TTarget3)>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = binder(result.Value.Item1, result.Value.Item2, result.Value.Item3);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value.Item1, result.Value.Item2, result.Value.Item3, target.Item1, target.Item2, target.Item3);
    }

    /// <summary>
    /// Chains a Result-returning operation and appends its value(s) to form a larger tuple Result.
    /// </summary>
        /// <typeparam name="TSource1">The type of the first source tuple element.</typeparam>
    /// <typeparam name="TSource2">The type of the second source tuple element.</typeparam>
    /// <typeparam name="TSource3">The type of the third source tuple element.</typeparam>
    /// <typeparam name="TSource4">The type of the fourth source tuple element.</typeparam>
        /// <typeparam name="TTarget1">The type of the first target tuple element to append.</typeparam>
    /// <typeparam name="TTarget2">The type of the second target tuple element to append.</typeparam>
    /// <param name="result">The source <see cref="Result{T}"/> containing a tuple with 4 elements.</param>
    /// <param name="binder">A function that takes the source value(s) and returns a Result containing a tuple with 2 elements to append.</param>
    /// <returns>A <see cref="Result{T}"/> containing a tuple with all source and target values if both operations succeed; otherwise, the first error encountered.</returns>
    /// <remarks>
    /// BindAppend is a monadic bind operation that accumulates values into larger tuples. It's part of the
    /// railway-oriented programming pattern where:
    /// - If the source Result is a failure, that error is propagated immediately
    /// - If the source is successful, the binder function is called with the value(s)
    /// - If the binder returns a failure, that error is propagated
    /// - If both succeed, all values are combined into a larger tuple
    /// This enables building up complex data structures through a series of Result-returning operations
    /// while maintaining proper error handling throughout the chain.
    /// </remarks>
    public static Result<(TSource1, TSource2, TSource3, TSource4, TTarget1, TTarget2)> BindAppend<TSource1, TSource2, TSource3, TSource4, TTarget1, TTarget2>(
        in this Result<(TSource1, TSource2, TSource3, TSource4)> result,
        Func<TSource1, TSource2, TSource3, TSource4, Result<(TTarget1, TTarget2)>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = binder(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, target.Item1, target.Item2);
    }

    /// <summary>
    /// Chains a Result-returning operation and appends its value(s) to form a larger tuple Result.
    /// </summary>
        /// <typeparam name="TSource1">The type of the first source tuple element.</typeparam>
    /// <typeparam name="TSource2">The type of the second source tuple element.</typeparam>
    /// <typeparam name="TSource3">The type of the third source tuple element.</typeparam>
    /// <typeparam name="TSource4">The type of the fourth source tuple element.</typeparam>
    /// <typeparam name="TSource5">The type of the fifth source tuple element.</typeparam>
        /// <typeparam name="TTarget">The type of the target value to append.</typeparam>
    /// <param name="result">The source <see cref="Result{T}"/> containing a tuple with 5 elements.</param>
    /// <param name="binder">A function that takes the source value(s) and returns a Result containing a value to append.</param>
    /// <returns>A <see cref="Result{T}"/> containing a tuple with all source and target values if both operations succeed; otherwise, the first error encountered.</returns>
    /// <remarks>
    /// BindAppend is a monadic bind operation that accumulates values into larger tuples. It's part of the
    /// railway-oriented programming pattern where:
    /// - If the source Result is a failure, that error is propagated immediately
    /// - If the source is successful, the binder function is called with the value(s)
    /// - If the binder returns a failure, that error is propagated
    /// - If both succeed, all values are combined into a larger tuple
    /// This enables building up complex data structures through a series of Result-returning operations
    /// while maintaining proper error handling throughout the chain.
    /// </remarks>
    public static Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TTarget)> BindAppend<TSource1, TSource2, TSource3, TSource4, TSource5, TTarget>(
        in this Result<(TSource1, TSource2, TSource3, TSource4, TSource5)> result,
        Func<TSource1, TSource2, TSource3, TSource4, TSource5, Result<TTarget>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = binder(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, target);
    }

    /// <summary>
    /// Chains a Result-returning operation and appends its value(s) to form a larger tuple Result.
    /// </summary>
        /// <typeparam name="TSource">The type of the source value.</typeparam>
        /// <typeparam name="TTarget1">The type of the first target tuple element to append.</typeparam>
    /// <typeparam name="TTarget2">The type of the second target tuple element to append.</typeparam>
    /// <typeparam name="TTarget3">The type of the third target tuple element to append.</typeparam>
    /// <typeparam name="TTarget4">The type of the fourth target tuple element to append.</typeparam>
    /// <typeparam name="TTarget5">The type of the fifth target tuple element to append.</typeparam>
    /// <typeparam name="TTarget6">The type of the sixth target tuple element to append.</typeparam>
    /// <param name="result">The source <see cref="Result{T}"/> containing a value.</param>
    /// <param name="binder">A function that takes the source value(s) and returns a Result containing a tuple with 6 elements to append.</param>
    /// <returns>A <see cref="Result{T}"/> containing a tuple with all source and target values if both operations succeed; otherwise, the first error encountered.</returns>
    /// <remarks>
    /// BindAppend is a monadic bind operation that accumulates values into larger tuples. It's part of the
    /// railway-oriented programming pattern where:
    /// - If the source Result is a failure, that error is propagated immediately
    /// - If the source is successful, the binder function is called with the value(s)
    /// - If the binder returns a failure, that error is propagated
    /// - If both succeed, all values are combined into a larger tuple
    /// This enables building up complex data structures through a series of Result-returning operations
    /// while maintaining proper error handling throughout the chain.
    /// </remarks>
    public static Result<(TSource, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6)> BindAppend<TSource, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6>(
        in this Result<TSource> result,
        Func<TSource, Result<(TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6)>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = binder(result.Value);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value, target.Item1, target.Item2, target.Item3, target.Item4, target.Item5, target.Item6);
    }

    /// <summary>
    /// Chains a Result-returning operation and appends its value(s) to form a larger tuple Result.
    /// </summary>
        /// <typeparam name="TSource1">The type of the first source tuple element.</typeparam>
    /// <typeparam name="TSource2">The type of the second source tuple element.</typeparam>
        /// <typeparam name="TTarget1">The type of the first target tuple element to append.</typeparam>
    /// <typeparam name="TTarget2">The type of the second target tuple element to append.</typeparam>
    /// <typeparam name="TTarget3">The type of the third target tuple element to append.</typeparam>
    /// <typeparam name="TTarget4">The type of the fourth target tuple element to append.</typeparam>
    /// <typeparam name="TTarget5">The type of the fifth target tuple element to append.</typeparam>
    /// <param name="result">The source <see cref="Result{T}"/> containing a tuple with 2 elements.</param>
    /// <param name="binder">A function that takes the source value(s) and returns a Result containing a tuple with 5 elements to append.</param>
    /// <returns>A <see cref="Result{T}"/> containing a tuple with all source and target values if both operations succeed; otherwise, the first error encountered.</returns>
    /// <remarks>
    /// BindAppend is a monadic bind operation that accumulates values into larger tuples. It's part of the
    /// railway-oriented programming pattern where:
    /// - If the source Result is a failure, that error is propagated immediately
    /// - If the source is successful, the binder function is called with the value(s)
    /// - If the binder returns a failure, that error is propagated
    /// - If both succeed, all values are combined into a larger tuple
    /// This enables building up complex data structures through a series of Result-returning operations
    /// while maintaining proper error handling throughout the chain.
    /// </remarks>
    public static Result<(TSource1, TSource2, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5)> BindAppend<TSource1, TSource2, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5>(
        in this Result<(TSource1, TSource2)> result,
        Func<TSource1, TSource2, Result<(TTarget1, TTarget2, TTarget3, TTarget4, TTarget5)>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = binder(result.Value.Item1, result.Value.Item2);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value.Item1, result.Value.Item2, target.Item1, target.Item2, target.Item3, target.Item4, target.Item5);
    }

    /// <summary>
    /// Chains a Result-returning operation and appends its value(s) to form a larger tuple Result.
    /// </summary>
        /// <typeparam name="TSource1">The type of the first source tuple element.</typeparam>
    /// <typeparam name="TSource2">The type of the second source tuple element.</typeparam>
    /// <typeparam name="TSource3">The type of the third source tuple element.</typeparam>
        /// <typeparam name="TTarget1">The type of the first target tuple element to append.</typeparam>
    /// <typeparam name="TTarget2">The type of the second target tuple element to append.</typeparam>
    /// <typeparam name="TTarget3">The type of the third target tuple element to append.</typeparam>
    /// <typeparam name="TTarget4">The type of the fourth target tuple element to append.</typeparam>
    /// <param name="result">The source <see cref="Result{T}"/> containing a tuple with 3 elements.</param>
    /// <param name="binder">A function that takes the source value(s) and returns a Result containing a tuple with 4 elements to append.</param>
    /// <returns>A <see cref="Result{T}"/> containing a tuple with all source and target values if both operations succeed; otherwise, the first error encountered.</returns>
    /// <remarks>
    /// BindAppend is a monadic bind operation that accumulates values into larger tuples. It's part of the
    /// railway-oriented programming pattern where:
    /// - If the source Result is a failure, that error is propagated immediately
    /// - If the source is successful, the binder function is called with the value(s)
    /// - If the binder returns a failure, that error is propagated
    /// - If both succeed, all values are combined into a larger tuple
    /// This enables building up complex data structures through a series of Result-returning operations
    /// while maintaining proper error handling throughout the chain.
    /// </remarks>
    public static Result<(TSource1, TSource2, TSource3, TTarget1, TTarget2, TTarget3, TTarget4)> BindAppend<TSource1, TSource2, TSource3, TTarget1, TTarget2, TTarget3, TTarget4>(
        in this Result<(TSource1, TSource2, TSource3)> result,
        Func<TSource1, TSource2, TSource3, Result<(TTarget1, TTarget2, TTarget3, TTarget4)>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = binder(result.Value.Item1, result.Value.Item2, result.Value.Item3);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value.Item1, result.Value.Item2, result.Value.Item3, target.Item1, target.Item2, target.Item3, target.Item4);
    }

    /// <summary>
    /// Chains a Result-returning operation and appends its value(s) to form a larger tuple Result.
    /// </summary>
        /// <typeparam name="TSource1">The type of the first source tuple element.</typeparam>
    /// <typeparam name="TSource2">The type of the second source tuple element.</typeparam>
    /// <typeparam name="TSource3">The type of the third source tuple element.</typeparam>
    /// <typeparam name="TSource4">The type of the fourth source tuple element.</typeparam>
        /// <typeparam name="TTarget1">The type of the first target tuple element to append.</typeparam>
    /// <typeparam name="TTarget2">The type of the second target tuple element to append.</typeparam>
    /// <typeparam name="TTarget3">The type of the third target tuple element to append.</typeparam>
    /// <param name="result">The source <see cref="Result{T}"/> containing a tuple with 4 elements.</param>
    /// <param name="binder">A function that takes the source value(s) and returns a Result containing a tuple with 3 elements to append.</param>
    /// <returns>A <see cref="Result{T}"/> containing a tuple with all source and target values if both operations succeed; otherwise, the first error encountered.</returns>
    /// <remarks>
    /// BindAppend is a monadic bind operation that accumulates values into larger tuples. It's part of the
    /// railway-oriented programming pattern where:
    /// - If the source Result is a failure, that error is propagated immediately
    /// - If the source is successful, the binder function is called with the value(s)
    /// - If the binder returns a failure, that error is propagated
    /// - If both succeed, all values are combined into a larger tuple
    /// This enables building up complex data structures through a series of Result-returning operations
    /// while maintaining proper error handling throughout the chain.
    /// </remarks>
    public static Result<(TSource1, TSource2, TSource3, TSource4, TTarget1, TTarget2, TTarget3)> BindAppend<TSource1, TSource2, TSource3, TSource4, TTarget1, TTarget2, TTarget3>(
        in this Result<(TSource1, TSource2, TSource3, TSource4)> result,
        Func<TSource1, TSource2, TSource3, TSource4, Result<(TTarget1, TTarget2, TTarget3)>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = binder(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, target.Item1, target.Item2, target.Item3);
    }

    /// <summary>
    /// Chains a Result-returning operation and appends its value(s) to form a larger tuple Result.
    /// </summary>
        /// <typeparam name="TSource1">The type of the first source tuple element.</typeparam>
    /// <typeparam name="TSource2">The type of the second source tuple element.</typeparam>
    /// <typeparam name="TSource3">The type of the third source tuple element.</typeparam>
    /// <typeparam name="TSource4">The type of the fourth source tuple element.</typeparam>
    /// <typeparam name="TSource5">The type of the fifth source tuple element.</typeparam>
        /// <typeparam name="TTarget1">The type of the first target tuple element to append.</typeparam>
    /// <typeparam name="TTarget2">The type of the second target tuple element to append.</typeparam>
    /// <param name="result">The source <see cref="Result{T}"/> containing a tuple with 5 elements.</param>
    /// <param name="binder">A function that takes the source value(s) and returns a Result containing a tuple with 2 elements to append.</param>
    /// <returns>A <see cref="Result{T}"/> containing a tuple with all source and target values if both operations succeed; otherwise, the first error encountered.</returns>
    /// <remarks>
    /// BindAppend is a monadic bind operation that accumulates values into larger tuples. It's part of the
    /// railway-oriented programming pattern where:
    /// - If the source Result is a failure, that error is propagated immediately
    /// - If the source is successful, the binder function is called with the value(s)
    /// - If the binder returns a failure, that error is propagated
    /// - If both succeed, all values are combined into a larger tuple
    /// This enables building up complex data structures through a series of Result-returning operations
    /// while maintaining proper error handling throughout the chain.
    /// </remarks>
    public static Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TTarget1, TTarget2)> BindAppend<TSource1, TSource2, TSource3, TSource4, TSource5, TTarget1, TTarget2>(
        in this Result<(TSource1, TSource2, TSource3, TSource4, TSource5)> result,
        Func<TSource1, TSource2, TSource3, TSource4, TSource5, Result<(TTarget1, TTarget2)>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = binder(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, target.Item1, target.Item2);
    }

    /// <summary>
    /// Chains a Result-returning operation and appends its value(s) to form a larger tuple Result.
    /// </summary>
        /// <typeparam name="TSource1">The type of the first source tuple element.</typeparam>
    /// <typeparam name="TSource2">The type of the second source tuple element.</typeparam>
    /// <typeparam name="TSource3">The type of the third source tuple element.</typeparam>
    /// <typeparam name="TSource4">The type of the fourth source tuple element.</typeparam>
    /// <typeparam name="TSource5">The type of the fifth source tuple element.</typeparam>
    /// <typeparam name="TSource6">The type of the sixth source tuple element.</typeparam>
        /// <typeparam name="TTarget">The type of the target value to append.</typeparam>
    /// <param name="result">The source <see cref="Result{T}"/> containing a tuple with 6 elements.</param>
    /// <param name="binder">A function that takes the source value(s) and returns a Result containing a value to append.</param>
    /// <returns>A <see cref="Result{T}"/> containing a tuple with all source and target values if both operations succeed; otherwise, the first error encountered.</returns>
    /// <remarks>
    /// BindAppend is a monadic bind operation that accumulates values into larger tuples. It's part of the
    /// railway-oriented programming pattern where:
    /// - If the source Result is a failure, that error is propagated immediately
    /// - If the source is successful, the binder function is called with the value(s)
    /// - If the binder returns a failure, that error is propagated
    /// - If both succeed, all values are combined into a larger tuple
    /// This enables building up complex data structures through a series of Result-returning operations
    /// while maintaining proper error handling throughout the chain.
    /// </remarks>
    public static Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TTarget)> BindAppend<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TTarget>(
        in this Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6)> result,
        Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, Result<TTarget>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = binder(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, result.Value.Item6);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, result.Value.Item6, target);
    }

    /// <summary>
    /// Chains a Result-returning operation and appends its value(s) to form a larger tuple Result.
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
    /// <param name="binder">A function that takes the source value(s) and returns a Result containing a tuple with 7 elements to append.</param>
    /// <returns>A <see cref="Result{T}"/> containing a tuple with all source and target values if both operations succeed; otherwise, the first error encountered.</returns>
    /// <remarks>
    /// BindAppend is a monadic bind operation that accumulates values into larger tuples. It's part of the
    /// railway-oriented programming pattern where:
    /// - If the source Result is a failure, that error is propagated immediately
    /// - If the source is successful, the binder function is called with the value(s)
    /// - If the binder returns a failure, that error is propagated
    /// - If both succeed, all values are combined into a larger tuple
    /// This enables building up complex data structures through a series of Result-returning operations
    /// while maintaining proper error handling throughout the chain.
    /// </remarks>
    public static Result<(TSource, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6, TTarget7)> BindAppend<TSource, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6, TTarget7>(
        in this Result<TSource> result,
        Func<TSource, Result<(TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6, TTarget7)>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = binder(result.Value);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value, target.Item1, target.Item2, target.Item3, target.Item4, target.Item5, target.Item6, target.Item7);
    }

    /// <summary>
    /// Chains a Result-returning operation and appends its value(s) to form a larger tuple Result.
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
    /// <param name="binder">A function that takes the source value(s) and returns a Result containing a tuple with 6 elements to append.</param>
    /// <returns>A <see cref="Result{T}"/> containing a tuple with all source and target values if both operations succeed; otherwise, the first error encountered.</returns>
    /// <remarks>
    /// BindAppend is a monadic bind operation that accumulates values into larger tuples. It's part of the
    /// railway-oriented programming pattern where:
    /// - If the source Result is a failure, that error is propagated immediately
    /// - If the source is successful, the binder function is called with the value(s)
    /// - If the binder returns a failure, that error is propagated
    /// - If both succeed, all values are combined into a larger tuple
    /// This enables building up complex data structures through a series of Result-returning operations
    /// while maintaining proper error handling throughout the chain.
    /// </remarks>
    public static Result<(TSource1, TSource2, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6)> BindAppend<TSource1, TSource2, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6>(
        in this Result<(TSource1, TSource2)> result,
        Func<TSource1, TSource2, Result<(TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6)>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = binder(result.Value.Item1, result.Value.Item2);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value.Item1, result.Value.Item2, target.Item1, target.Item2, target.Item3, target.Item4, target.Item5, target.Item6);
    }

    /// <summary>
    /// Chains a Result-returning operation and appends its value(s) to form a larger tuple Result.
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
    /// <param name="binder">A function that takes the source value(s) and returns a Result containing a tuple with 5 elements to append.</param>
    /// <returns>A <see cref="Result{T}"/> containing a tuple with all source and target values if both operations succeed; otherwise, the first error encountered.</returns>
    /// <remarks>
    /// BindAppend is a monadic bind operation that accumulates values into larger tuples. It's part of the
    /// railway-oriented programming pattern where:
    /// - If the source Result is a failure, that error is propagated immediately
    /// - If the source is successful, the binder function is called with the value(s)
    /// - If the binder returns a failure, that error is propagated
    /// - If both succeed, all values are combined into a larger tuple
    /// This enables building up complex data structures through a series of Result-returning operations
    /// while maintaining proper error handling throughout the chain.
    /// </remarks>
    public static Result<(TSource1, TSource2, TSource3, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5)> BindAppend<TSource1, TSource2, TSource3, TTarget1, TTarget2, TTarget3, TTarget4, TTarget5>(
        in this Result<(TSource1, TSource2, TSource3)> result,
        Func<TSource1, TSource2, TSource3, Result<(TTarget1, TTarget2, TTarget3, TTarget4, TTarget5)>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = binder(result.Value.Item1, result.Value.Item2, result.Value.Item3);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value.Item1, result.Value.Item2, result.Value.Item3, target.Item1, target.Item2, target.Item3, target.Item4, target.Item5);
    }

    /// <summary>
    /// Chains a Result-returning operation and appends its value(s) to form a larger tuple Result.
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
    /// <param name="binder">A function that takes the source value(s) and returns a Result containing a tuple with 4 elements to append.</param>
    /// <returns>A <see cref="Result{T}"/> containing a tuple with all source and target values if both operations succeed; otherwise, the first error encountered.</returns>
    /// <remarks>
    /// BindAppend is a monadic bind operation that accumulates values into larger tuples. It's part of the
    /// railway-oriented programming pattern where:
    /// - If the source Result is a failure, that error is propagated immediately
    /// - If the source is successful, the binder function is called with the value(s)
    /// - If the binder returns a failure, that error is propagated
    /// - If both succeed, all values are combined into a larger tuple
    /// This enables building up complex data structures through a series of Result-returning operations
    /// while maintaining proper error handling throughout the chain.
    /// </remarks>
    public static Result<(TSource1, TSource2, TSource3, TSource4, TTarget1, TTarget2, TTarget3, TTarget4)> BindAppend<TSource1, TSource2, TSource3, TSource4, TTarget1, TTarget2, TTarget3, TTarget4>(
        in this Result<(TSource1, TSource2, TSource3, TSource4)> result,
        Func<TSource1, TSource2, TSource3, TSource4, Result<(TTarget1, TTarget2, TTarget3, TTarget4)>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = binder(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, target.Item1, target.Item2, target.Item3, target.Item4);
    }

    /// <summary>
    /// Chains a Result-returning operation and appends its value(s) to form a larger tuple Result.
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
    /// <param name="binder">A function that takes the source value(s) and returns a Result containing a tuple with 3 elements to append.</param>
    /// <returns>A <see cref="Result{T}"/> containing a tuple with all source and target values if both operations succeed; otherwise, the first error encountered.</returns>
    /// <remarks>
    /// BindAppend is a monadic bind operation that accumulates values into larger tuples. It's part of the
    /// railway-oriented programming pattern where:
    /// - If the source Result is a failure, that error is propagated immediately
    /// - If the source is successful, the binder function is called with the value(s)
    /// - If the binder returns a failure, that error is propagated
    /// - If both succeed, all values are combined into a larger tuple
    /// This enables building up complex data structures through a series of Result-returning operations
    /// while maintaining proper error handling throughout the chain.
    /// </remarks>
    public static Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TTarget1, TTarget2, TTarget3)> BindAppend<TSource1, TSource2, TSource3, TSource4, TSource5, TTarget1, TTarget2, TTarget3>(
        in this Result<(TSource1, TSource2, TSource3, TSource4, TSource5)> result,
        Func<TSource1, TSource2, TSource3, TSource4, TSource5, Result<(TTarget1, TTarget2, TTarget3)>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = binder(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, target.Item1, target.Item2, target.Item3);
    }

    /// <summary>
    /// Chains a Result-returning operation and appends its value(s) to form a larger tuple Result.
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
    /// <param name="binder">A function that takes the source value(s) and returns a Result containing a tuple with 2 elements to append.</param>
    /// <returns>A <see cref="Result{T}"/> containing a tuple with all source and target values if both operations succeed; otherwise, the first error encountered.</returns>
    /// <remarks>
    /// BindAppend is a monadic bind operation that accumulates values into larger tuples. It's part of the
    /// railway-oriented programming pattern where:
    /// - If the source Result is a failure, that error is propagated immediately
    /// - If the source is successful, the binder function is called with the value(s)
    /// - If the binder returns a failure, that error is propagated
    /// - If both succeed, all values are combined into a larger tuple
    /// This enables building up complex data structures through a series of Result-returning operations
    /// while maintaining proper error handling throughout the chain.
    /// </remarks>
    public static Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TTarget1, TTarget2)> BindAppend<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TTarget1, TTarget2>(
        in this Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6)> result,
        Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, Result<(TTarget1, TTarget2)>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = binder(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, result.Value.Item6);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, result.Value.Item6, target.Item1, target.Item2);
    }

    /// <summary>
    /// Chains a Result-returning operation and appends its value(s) to form a larger tuple Result.
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
    /// <param name="binder">A function that takes the source value(s) and returns a Result containing a value to append.</param>
    /// <returns>A <see cref="Result{T}"/> containing a tuple with all source and target values if both operations succeed; otherwise, the first error encountered.</returns>
    /// <remarks>
    /// BindAppend is a monadic bind operation that accumulates values into larger tuples. It's part of the
    /// railway-oriented programming pattern where:
    /// - If the source Result is a failure, that error is propagated immediately
    /// - If the source is successful, the binder function is called with the value(s)
    /// - If the binder returns a failure, that error is propagated
    /// - If both succeed, all values are combined into a larger tuple
    /// This enables building up complex data structures through a series of Result-returning operations
    /// while maintaining proper error handling throughout the chain.
    /// </remarks>
    public static Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TTarget)> BindAppend<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TTarget>(
        in this Result<(TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7)> result,
        Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, Result<TTarget>> binder)
    {
        if (result.IsFailure) return result.Error;
    
        var targetResult = binder(result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, result.Value.Item6, result.Value.Item7);
        if (targetResult.IsFailure) return targetResult.Error;
        
        var target = targetResult.Value;
        return (result.Value.Item1, result.Value.Item2, result.Value.Item3, result.Value.Item4, result.Value.Item5, result.Value.Item6, result.Value.Item7, target);
    }

}
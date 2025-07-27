namespace FluentUnions;

public static partial class ValueResultExtensions
{
    /// <summary>
    /// Applies a binder function to the value inside a successful <see cref="Result{T}"/> containing a tuple with 2 elements.
    /// </summary>
    /// <typeparam name="TValue1">The type of the first tuple element.</typeparam>
    /// <typeparam name="TValue2">The type of the second tuple element.</typeparam>
    /// <typeparam name="TTarget">The type of the value in the resulting Result.</typeparam>
    /// <param name="source">The source <see cref="Result{T}"/> containing a tuple.</param>
    /// <param name="binder">A function that takes the tuple elements and returns a new Result that may succeed or fail.</param>
    /// <returns>The Result returned by the binder if the source was successful; otherwise, the original error.</returns>
    /// <remarks>
    /// Bind (also known as flatMap or >>=) is the fundamental operation for railway-oriented programming.
    /// It allows chaining operations that may fail, automatically short-circuiting on the first error.
    /// If the source Result is a failure, the binder is not executed and the error is propagated.
    /// This enables clean error handling without nested conditionals.
    /// </remarks>
    public static Result<TTarget> Bind<TValue1, TValue2, TTarget>(
        in this Result<(TValue1, TValue2)> source,
        Func<TValue1, TValue2, Result<TTarget>> binder)
    {
        if (source.IsFailure) return source.Error;
        return binder(source.Value.Item1, source.Value.Item2);
    }
    
    /// <summary>
    /// Applies a binder function to the value inside a successful <see cref="Result{T}"/> containing a tuple with 3 elements.
    /// </summary>
    /// <typeparam name="TValue1">The type of the first tuple element.</typeparam>
    /// <typeparam name="TValue2">The type of the second tuple element.</typeparam>
    /// <typeparam name="TValue3">The type of the third tuple element.</typeparam>
    /// <typeparam name="TTarget">The type of the value in the resulting Result.</typeparam>
    /// <param name="source">The source <see cref="Result{T}"/> containing a tuple.</param>
    /// <param name="binder">A function that takes the tuple elements and returns a new Result that may succeed or fail.</param>
    /// <returns>The Result returned by the binder if the source was successful; otherwise, the original error.</returns>
    /// <remarks>
    /// Bind (also known as flatMap or >>=) is the fundamental operation for railway-oriented programming.
    /// It allows chaining operations that may fail, automatically short-circuiting on the first error.
    /// If the source Result is a failure, the binder is not executed and the error is propagated.
    /// This enables clean error handling without nested conditionals.
    /// </remarks>
    public static Result<TTarget> Bind<TValue1, TValue2, TValue3, TTarget>(
        in this Result<(TValue1, TValue2, TValue3)> source,
        Func<TValue1, TValue2, TValue3, Result<TTarget>> binder)
    {
        if (source.IsFailure) return source.Error;
        return binder(source.Value.Item1, source.Value.Item2, source.Value.Item3);
    }
    
    /// <summary>
    /// Applies a binder function to the value inside a successful <see cref="Result{T}"/> containing a tuple with 4 elements.
    /// </summary>
    /// <typeparam name="TValue1">The type of the first tuple element.</typeparam>
    /// <typeparam name="TValue2">The type of the second tuple element.</typeparam>
    /// <typeparam name="TValue3">The type of the third tuple element.</typeparam>
    /// <typeparam name="TValue4">The type of the fourth tuple element.</typeparam>
    /// <typeparam name="TTarget">The type of the value in the resulting Result.</typeparam>
    /// <param name="source">The source <see cref="Result{T}"/> containing a tuple.</param>
    /// <param name="binder">A function that takes the tuple elements and returns a new Result that may succeed or fail.</param>
    /// <returns>The Result returned by the binder if the source was successful; otherwise, the original error.</returns>
    /// <remarks>
    /// Bind (also known as flatMap or >>=) is the fundamental operation for railway-oriented programming.
    /// It allows chaining operations that may fail, automatically short-circuiting on the first error.
    /// If the source Result is a failure, the binder is not executed and the error is propagated.
    /// This enables clean error handling without nested conditionals.
    /// </remarks>
    public static Result<TTarget> Bind<TValue1, TValue2, TValue3, TValue4, TTarget>(
        in this Result<(TValue1, TValue2, TValue3, TValue4)> source,
        Func<TValue1, TValue2, TValue3, TValue4, Result<TTarget>> binder)
    {
        if (source.IsFailure) return source.Error;
        return binder(source.Value.Item1, source.Value.Item2, source.Value.Item3, source.Value.Item4);
    }
    
    /// <summary>
    /// Applies a binder function to the value inside a successful <see cref="Result{T}"/> containing a tuple with 5 elements.
    /// </summary>
    /// <typeparam name="TValue1">The type of the first tuple element.</typeparam>
    /// <typeparam name="TValue2">The type of the second tuple element.</typeparam>
    /// <typeparam name="TValue3">The type of the third tuple element.</typeparam>
    /// <typeparam name="TValue4">The type of the fourth tuple element.</typeparam>
    /// <typeparam name="TValue5">The type of the fifth tuple element.</typeparam>
    /// <typeparam name="TTarget">The type of the value in the resulting Result.</typeparam>
    /// <param name="source">The source <see cref="Result{T}"/> containing a tuple.</param>
    /// <param name="binder">A function that takes the tuple elements and returns a new Result that may succeed or fail.</param>
    /// <returns>The Result returned by the binder if the source was successful; otherwise, the original error.</returns>
    /// <remarks>
    /// Bind (also known as flatMap or >>=) is the fundamental operation for railway-oriented programming.
    /// It allows chaining operations that may fail, automatically short-circuiting on the first error.
    /// If the source Result is a failure, the binder is not executed and the error is propagated.
    /// This enables clean error handling without nested conditionals.
    /// </remarks>
    public static Result<TTarget> Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TTarget>(
        in this Result<(TValue1, TValue2, TValue3, TValue4, TValue5)> source,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, Result<TTarget>> binder)
    {
        if (source.IsFailure) return source.Error;
        return binder(source.Value.Item1, source.Value.Item2, source.Value.Item3, source.Value.Item4, source.Value.Item5);
    }
    
    /// <summary>
    /// Applies a binder function to the value inside a successful <see cref="Result{T}"/> containing a tuple with 6 elements.
    /// </summary>
    /// <typeparam name="TValue1">The type of the first tuple element.</typeparam>
    /// <typeparam name="TValue2">The type of the second tuple element.</typeparam>
    /// <typeparam name="TValue3">The type of the third tuple element.</typeparam>
    /// <typeparam name="TValue4">The type of the fourth tuple element.</typeparam>
    /// <typeparam name="TValue5">The type of the fifth tuple element.</typeparam>
    /// <typeparam name="TValue6">The type of the sixth tuple element.</typeparam>
    /// <typeparam name="TTarget">The type of the value in the resulting Result.</typeparam>
    /// <param name="source">The source <see cref="Result{T}"/> containing a tuple.</param>
    /// <param name="binder">A function that takes the tuple elements and returns a new Result that may succeed or fail.</param>
    /// <returns>The Result returned by the binder if the source was successful; otherwise, the original error.</returns>
    /// <remarks>
    /// Bind (also known as flatMap or >>=) is the fundamental operation for railway-oriented programming.
    /// It allows chaining operations that may fail, automatically short-circuiting on the first error.
    /// If the source Result is a failure, the binder is not executed and the error is propagated.
    /// This enables clean error handling without nested conditionals.
    /// </remarks>
    public static Result<TTarget> Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TTarget>(
        in this Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)> source,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, Result<TTarget>> binder)
    {
        if (source.IsFailure) return source.Error;
        return binder(source.Value.Item1, source.Value.Item2, source.Value.Item3, source.Value.Item4, source.Value.Item5, source.Value.Item6);
    }
    
    /// <summary>
    /// Applies a binder function to the value inside a successful <see cref="Result{T}"/> containing a tuple with 7 elements.
    /// </summary>
    /// <typeparam name="TValue1">The type of the first tuple element.</typeparam>
    /// <typeparam name="TValue2">The type of the second tuple element.</typeparam>
    /// <typeparam name="TValue3">The type of the third tuple element.</typeparam>
    /// <typeparam name="TValue4">The type of the fourth tuple element.</typeparam>
    /// <typeparam name="TValue5">The type of the fifth tuple element.</typeparam>
    /// <typeparam name="TValue6">The type of the sixth tuple element.</typeparam>
    /// <typeparam name="TValue7">The type of the seventh tuple element.</typeparam>
    /// <typeparam name="TTarget">The type of the value in the resulting Result.</typeparam>
    /// <param name="source">The source <see cref="Result{T}"/> containing a tuple.</param>
    /// <param name="binder">A function that takes the tuple elements and returns a new Result that may succeed or fail.</param>
    /// <returns>The Result returned by the binder if the source was successful; otherwise, the original error.</returns>
    /// <remarks>
    /// Bind (also known as flatMap or >>=) is the fundamental operation for railway-oriented programming.
    /// It allows chaining operations that may fail, automatically short-circuiting on the first error.
    /// If the source Result is a failure, the binder is not executed and the error is propagated.
    /// This enables clean error handling without nested conditionals.
    /// </remarks>
    public static Result<TTarget> Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TTarget>(
        in this Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)> source,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, Result<TTarget>> binder)
    {
        if (source.IsFailure) return source.Error;
        return binder(source.Value.Item1, source.Value.Item2, source.Value.Item3, source.Value.Item4, source.Value.Item5, source.Value.Item6, source.Value.Item7);
    }
    
    /// <summary>
    /// Applies a binder function to the value inside a successful <see cref="Result{T}"/> containing a tuple with 8 elements.
    /// </summary>
    /// <typeparam name="TValue1">The type of the first tuple element.</typeparam>
    /// <typeparam name="TValue2">The type of the second tuple element.</typeparam>
    /// <typeparam name="TValue3">The type of the third tuple element.</typeparam>
    /// <typeparam name="TValue4">The type of the fourth tuple element.</typeparam>
    /// <typeparam name="TValue5">The type of the fifth tuple element.</typeparam>
    /// <typeparam name="TValue6">The type of the sixth tuple element.</typeparam>
    /// <typeparam name="TValue7">The type of the seventh tuple element.</typeparam>
    /// <typeparam name="TValue8">The type of the eighth tuple element.</typeparam>
    /// <typeparam name="TTarget">The type of the value in the resulting Result.</typeparam>
    /// <param name="source">The source <see cref="Result{T}"/> containing a tuple.</param>
    /// <param name="binder">A function that takes the tuple elements and returns a new Result that may succeed or fail.</param>
    /// <returns>The Result returned by the binder if the source was successful; otherwise, the original error.</returns>
    /// <remarks>
    /// Bind (also known as flatMap or >>=) is the fundamental operation for railway-oriented programming.
    /// It allows chaining operations that may fail, automatically short-circuiting on the first error.
    /// If the source Result is a failure, the binder is not executed and the error is propagated.
    /// This enables clean error handling without nested conditionals.
    /// </remarks>
    public static Result<TTarget> Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, TTarget>(
        in this Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)> source,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, Result<TTarget>> binder)
    {
        if (source.IsFailure) return source.Error;
        return binder(source.Value.Item1, source.Value.Item2, source.Value.Item3, source.Value.Item4, source.Value.Item5, source.Value.Item6, source.Value.Item7, source.Value.Item8);
    }
    
}
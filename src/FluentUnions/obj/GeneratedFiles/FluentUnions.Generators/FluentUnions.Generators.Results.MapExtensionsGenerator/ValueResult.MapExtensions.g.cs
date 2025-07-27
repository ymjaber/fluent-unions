namespace FluentUnions;

public static partial class ValueResultExtensions
{
    /// <summary>
    /// Transforms the value inside a successful <see cref="Result{T}"/> containing a tuple with 2 elements.
    /// </summary>
    /// <typeparam name="TValue1">The type of the first tuple element.</typeparam>
    /// <typeparam name="TValue2">The type of the second tuple element.</typeparam>
    /// <typeparam name="TTarget">The type of the transformed value.</typeparam>
    /// <param name="source">The source <see cref="Result{T}"/> containing a tuple.</param>
    /// <param name="mapper">A function that transforms the tuple elements into a new value.</param>
    /// <returns>A <see cref="Result{T}"/> containing the transformed value if the source was successful; otherwise, the original error.</returns>
    /// <remarks>
    /// The Map operation is a fundamental railway-oriented programming concept. It applies the transformation
    /// only to successful Results, automatically propagating any errors without executing the mapper.
    /// This maintains the error-handling chain through functional transformations.
    /// </remarks>
    public static Result<TTarget> Map<TValue1, TValue2, TTarget>(
        in this Result<(TValue1, TValue2)> source,
        Func<TValue1, TValue2, TTarget> mapper)
    {
        if (source.IsFailure) return source.Error;
        return mapper(source.Value.Item1, source.Value.Item2);
    }
    
    /// <summary>
    /// Transforms the value inside a successful <see cref="Result{T}"/> containing a tuple with 3 elements.
    /// </summary>
    /// <typeparam name="TValue1">The type of the first tuple element.</typeparam>
    /// <typeparam name="TValue2">The type of the second tuple element.</typeparam>
    /// <typeparam name="TValue3">The type of the third tuple element.</typeparam>
    /// <typeparam name="TTarget">The type of the transformed value.</typeparam>
    /// <param name="source">The source <see cref="Result{T}"/> containing a tuple.</param>
    /// <param name="mapper">A function that transforms the tuple elements into a new value.</param>
    /// <returns>A <see cref="Result{T}"/> containing the transformed value if the source was successful; otherwise, the original error.</returns>
    /// <remarks>
    /// The Map operation is a fundamental railway-oriented programming concept. It applies the transformation
    /// only to successful Results, automatically propagating any errors without executing the mapper.
    /// This maintains the error-handling chain through functional transformations.
    /// </remarks>
    public static Result<TTarget> Map<TValue1, TValue2, TValue3, TTarget>(
        in this Result<(TValue1, TValue2, TValue3)> source,
        Func<TValue1, TValue2, TValue3, TTarget> mapper)
    {
        if (source.IsFailure) return source.Error;
        return mapper(source.Value.Item1, source.Value.Item2, source.Value.Item3);
    }
    
    /// <summary>
    /// Transforms the value inside a successful <see cref="Result{T}"/> containing a tuple with 4 elements.
    /// </summary>
    /// <typeparam name="TValue1">The type of the first tuple element.</typeparam>
    /// <typeparam name="TValue2">The type of the second tuple element.</typeparam>
    /// <typeparam name="TValue3">The type of the third tuple element.</typeparam>
    /// <typeparam name="TValue4">The type of the fourth tuple element.</typeparam>
    /// <typeparam name="TTarget">The type of the transformed value.</typeparam>
    /// <param name="source">The source <see cref="Result{T}"/> containing a tuple.</param>
    /// <param name="mapper">A function that transforms the tuple elements into a new value.</param>
    /// <returns>A <see cref="Result{T}"/> containing the transformed value if the source was successful; otherwise, the original error.</returns>
    /// <remarks>
    /// The Map operation is a fundamental railway-oriented programming concept. It applies the transformation
    /// only to successful Results, automatically propagating any errors without executing the mapper.
    /// This maintains the error-handling chain through functional transformations.
    /// </remarks>
    public static Result<TTarget> Map<TValue1, TValue2, TValue3, TValue4, TTarget>(
        in this Result<(TValue1, TValue2, TValue3, TValue4)> source,
        Func<TValue1, TValue2, TValue3, TValue4, TTarget> mapper)
    {
        if (source.IsFailure) return source.Error;
        return mapper(source.Value.Item1, source.Value.Item2, source.Value.Item3, source.Value.Item4);
    }
    
    /// <summary>
    /// Transforms the value inside a successful <see cref="Result{T}"/> containing a tuple with 5 elements.
    /// </summary>
    /// <typeparam name="TValue1">The type of the first tuple element.</typeparam>
    /// <typeparam name="TValue2">The type of the second tuple element.</typeparam>
    /// <typeparam name="TValue3">The type of the third tuple element.</typeparam>
    /// <typeparam name="TValue4">The type of the fourth tuple element.</typeparam>
    /// <typeparam name="TValue5">The type of the fifth tuple element.</typeparam>
    /// <typeparam name="TTarget">The type of the transformed value.</typeparam>
    /// <param name="source">The source <see cref="Result{T}"/> containing a tuple.</param>
    /// <param name="mapper">A function that transforms the tuple elements into a new value.</param>
    /// <returns>A <see cref="Result{T}"/> containing the transformed value if the source was successful; otherwise, the original error.</returns>
    /// <remarks>
    /// The Map operation is a fundamental railway-oriented programming concept. It applies the transformation
    /// only to successful Results, automatically propagating any errors without executing the mapper.
    /// This maintains the error-handling chain through functional transformations.
    /// </remarks>
    public static Result<TTarget> Map<TValue1, TValue2, TValue3, TValue4, TValue5, TTarget>(
        in this Result<(TValue1, TValue2, TValue3, TValue4, TValue5)> source,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TTarget> mapper)
    {
        if (source.IsFailure) return source.Error;
        return mapper(source.Value.Item1, source.Value.Item2, source.Value.Item3, source.Value.Item4, source.Value.Item5);
    }
    
    /// <summary>
    /// Transforms the value inside a successful <see cref="Result{T}"/> containing a tuple with 6 elements.
    /// </summary>
    /// <typeparam name="TValue1">The type of the first tuple element.</typeparam>
    /// <typeparam name="TValue2">The type of the second tuple element.</typeparam>
    /// <typeparam name="TValue3">The type of the third tuple element.</typeparam>
    /// <typeparam name="TValue4">The type of the fourth tuple element.</typeparam>
    /// <typeparam name="TValue5">The type of the fifth tuple element.</typeparam>
    /// <typeparam name="TValue6">The type of the sixth tuple element.</typeparam>
    /// <typeparam name="TTarget">The type of the transformed value.</typeparam>
    /// <param name="source">The source <see cref="Result{T}"/> containing a tuple.</param>
    /// <param name="mapper">A function that transforms the tuple elements into a new value.</param>
    /// <returns>A <see cref="Result{T}"/> containing the transformed value if the source was successful; otherwise, the original error.</returns>
    /// <remarks>
    /// The Map operation is a fundamental railway-oriented programming concept. It applies the transformation
    /// only to successful Results, automatically propagating any errors without executing the mapper.
    /// This maintains the error-handling chain through functional transformations.
    /// </remarks>
    public static Result<TTarget> Map<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TTarget>(
        in this Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)> source,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TTarget> mapper)
    {
        if (source.IsFailure) return source.Error;
        return mapper(source.Value.Item1, source.Value.Item2, source.Value.Item3, source.Value.Item4, source.Value.Item5, source.Value.Item6);
    }
    
    /// <summary>
    /// Transforms the value inside a successful <see cref="Result{T}"/> containing a tuple with 7 elements.
    /// </summary>
    /// <typeparam name="TValue1">The type of the first tuple element.</typeparam>
    /// <typeparam name="TValue2">The type of the second tuple element.</typeparam>
    /// <typeparam name="TValue3">The type of the third tuple element.</typeparam>
    /// <typeparam name="TValue4">The type of the fourth tuple element.</typeparam>
    /// <typeparam name="TValue5">The type of the fifth tuple element.</typeparam>
    /// <typeparam name="TValue6">The type of the sixth tuple element.</typeparam>
    /// <typeparam name="TValue7">The type of the seventh tuple element.</typeparam>
    /// <typeparam name="TTarget">The type of the transformed value.</typeparam>
    /// <param name="source">The source <see cref="Result{T}"/> containing a tuple.</param>
    /// <param name="mapper">A function that transforms the tuple elements into a new value.</param>
    /// <returns>A <see cref="Result{T}"/> containing the transformed value if the source was successful; otherwise, the original error.</returns>
    /// <remarks>
    /// The Map operation is a fundamental railway-oriented programming concept. It applies the transformation
    /// only to successful Results, automatically propagating any errors without executing the mapper.
    /// This maintains the error-handling chain through functional transformations.
    /// </remarks>
    public static Result<TTarget> Map<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TTarget>(
        in this Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)> source,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TTarget> mapper)
    {
        if (source.IsFailure) return source.Error;
        return mapper(source.Value.Item1, source.Value.Item2, source.Value.Item3, source.Value.Item4, source.Value.Item5, source.Value.Item6, source.Value.Item7);
    }
    
    /// <summary>
    /// Transforms the value inside a successful <see cref="Result{T}"/> containing a tuple with 8 elements.
    /// </summary>
    /// <typeparam name="TValue1">The type of the first tuple element.</typeparam>
    /// <typeparam name="TValue2">The type of the second tuple element.</typeparam>
    /// <typeparam name="TValue3">The type of the third tuple element.</typeparam>
    /// <typeparam name="TValue4">The type of the fourth tuple element.</typeparam>
    /// <typeparam name="TValue5">The type of the fifth tuple element.</typeparam>
    /// <typeparam name="TValue6">The type of the sixth tuple element.</typeparam>
    /// <typeparam name="TValue7">The type of the seventh tuple element.</typeparam>
    /// <typeparam name="TValue8">The type of the eighth tuple element.</typeparam>
    /// <typeparam name="TTarget">The type of the transformed value.</typeparam>
    /// <param name="source">The source <see cref="Result{T}"/> containing a tuple.</param>
    /// <param name="mapper">A function that transforms the tuple elements into a new value.</param>
    /// <returns>A <see cref="Result{T}"/> containing the transformed value if the source was successful; otherwise, the original error.</returns>
    /// <remarks>
    /// The Map operation is a fundamental railway-oriented programming concept. It applies the transformation
    /// only to successful Results, automatically propagating any errors without executing the mapper.
    /// This maintains the error-handling chain through functional transformations.
    /// </remarks>
    public static Result<TTarget> Map<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, TTarget>(
        in this Result<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)> source,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, TTarget> mapper)
    {
        if (source.IsFailure) return source.Error;
        return mapper(source.Value.Item1, source.Value.Item2, source.Value.Item3, source.Value.Item4, source.Value.Item5, source.Value.Item6, source.Value.Item7, source.Value.Item8);
    }
    
}
using System.Runtime.CompilerServices;

namespace FluentUnions;

/// <summary>
/// Provides mapping extension methods for <see cref="Result{TValue}"/> that allow transforming
/// the contained value while preserving the result structure.
/// </summary>
public static partial class ValueResultExtensions
{
    /// <summary>
    /// Transforms the value contained in a successful result using the specified mapping function.
    /// </summary>
    /// <typeparam name="TSource">The type of the value in the source result.</typeparam>
    /// <typeparam name="TTarget">The type of the value in the target result.</typeparam>
    /// <param name="result">The result containing the value to transform.</param>
    /// <param name="mapper">A function that transforms the source value to the target value.</param>
    /// <returns>
    /// A new <see cref="Result{TTarget}"/> containing the transformed value if the original result was successful;
    /// otherwise, a failure result with the original error.
    /// </returns>
    /// <remarks>
    /// This method implements the functor map operation for <see cref="Result{TValue}"/>.
    /// The mapping function is only called if the result is successful, allowing safe transformation
    /// of values without explicit null or error checking.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TTarget> Map<TSource, TTarget>(
        in this Result<TSource> result,
        Func<TSource, TTarget> mapper)
    {
        if (result.IsFailure) return Result.Failure<TTarget>(result.Error);
        return Result.Success(mapper(result.Value));
    }
}
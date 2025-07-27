using System.Runtime.CompilerServices;

namespace FluentUnions;

/// <summary>
/// Provides extension methods for <see cref="EnsureBuilder{T}"/> that enable mapping and binding operations
/// after validation chains are built.
/// </summary>
public static partial class EnsureBuilderExtensions
{
    /// <summary>
    /// Builds the validation chain and maps the result value using the specified mapping function.
    /// </summary>
    /// <typeparam name="TSource">The type of the value being validated.</typeparam>
    /// <typeparam name="TTarget">The type of the mapped value.</typeparam>
    /// <param name="builder">The ensure builder containing the validation chain.</param>
    /// <param name="mapper">A function that transforms the validated value to the target type.</param>
    /// <returns>
    /// A <see cref="Result{TTarget}"/> containing the mapped value if all validations passed;
    /// otherwise, a failure result with the validation errors.
    /// </returns>
    /// <remarks>
    /// This method is useful when you want to transform a validated value immediately after
    /// ensuring it meets all criteria, combining validation and transformation in a single fluent chain.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TTarget> Map<TSource, TTarget>(
        in this EnsureBuilder<TSource> builder,
        Func<TSource, TTarget> mapper) =>
        builder.Build().Map(mapper);

    /// <summary>
    /// Builds the validation chain and binds the result to another operation that may also fail.
    /// </summary>
    /// <typeparam name="TSource">The type of the value being validated.</typeparam>
    /// <typeparam name="TTarget">The type of value produced by the binding operation.</typeparam>
    /// <param name="builder">The ensure builder containing the validation chain.</param>
    /// <param name="binder">A function that takes the validated value and returns a new result.</param>
    /// <returns>
    /// The result of the binding operation if all validations passed;
    /// otherwise, a failure result with the validation errors.
    /// </returns>
    /// <remarks>
    /// This method allows chaining operations that may fail after validation,
    /// short-circuiting if any validation fails.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TTarget> Bind<TSource, TTarget>(in this EnsureBuilder<TSource> builder,
        Func<TSource, Result<TTarget>> binder) =>
        builder.Build().Bind(binder);

    /// <summary>
    /// Builds the validation chain and combines it with another result, aggregating all errors.
    /// </summary>
    /// <typeparam name="TSource">The type of the value being validated.</typeparam>
    /// <typeparam name="TTarget">The type of value in the second result.</typeparam>
    /// <param name="builder">The ensure builder containing the validation chain.</param>
    /// <param name="binder">The result to combine with the validation result.</param>
    /// <returns>
    /// The second result if validations passed; otherwise, a failure result containing
    /// all errors from both the validation and the second result.
    /// </returns>
    /// <remarks>
    /// Unlike Bind, this method collects all errors rather than short-circuiting,
    /// useful for comprehensive validation scenarios.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TTarget> BindAll<TSource, TTarget>(in this EnsureBuilder<TSource> builder,
        in Result<TTarget> binder)
        => builder.Build().BindAll(binder);
}
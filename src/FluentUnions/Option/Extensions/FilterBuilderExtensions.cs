using System.Runtime.CompilerServices;

namespace FluentUnions;

/// <summary>
/// Provides extension methods for the <see cref="FilterBuilder{T}"/> type to support filter builder pattern operations.
/// </summary>
public static partial class FilterBuilderExtensions
{
    /// <summary>
    /// Builds the filtered option and applies a transformation to the value if it passes all filters.
    /// </summary>
    /// <typeparam name="TSource">The type of the value in the source filter builder.</typeparam>
    /// <typeparam name="TTarget">The type of the value in the resulting option after transformation.</typeparam>
    /// <param name="builder">The filter builder containing the filters to apply.</param>
    /// <param name="mapper">The function that transforms the value from <typeparamref name="TSource"/> to <typeparamref name="TTarget"/>.</param>
    /// <returns>
    /// An <see cref="Option{TTarget}"/> containing the transformed value if all filters pass;
    /// otherwise, <see cref="Option{TTarget}.None"/>.
    /// </returns>
    /// <remarks>
    /// This method combines filtering and mapping in a single operation. It first applies all filters
    /// configured in the builder, and if the value passes all filters, it applies the mapping function.
    /// This is more efficient than calling Build() followed by Map() separately.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<TTarget> Map<TSource, TTarget>(
        in this FilterBuilder<TSource> builder,
        Func<TSource, TTarget> mapper)
        where TSource : notnull
        where TTarget : notnull
        => builder.Build().Map(mapper);

    /// <summary>
    /// Builds the filtered option and applies a function that returns an option, flattening the result.
    /// </summary>
    /// <typeparam name="TSource">The type of the value in the source filter builder.</typeparam>
    /// <typeparam name="TTarget">The type of the value in the resulting option.</typeparam>
    /// <param name="builder">The filter builder containing the filters to apply.</param>
    /// <param name="binder">A function that takes the filtered value and returns an <see cref="Option{TTarget}"/>.</param>
    /// <returns>
    /// The result of applying <paramref name="binder"/> to the value if all filters pass;
    /// otherwise, <see cref="Option{TTarget}.None"/>.
    /// </returns>
    /// <remarks>
    /// This method combines filtering and monadic bind in a single operation. It first applies all filters
    /// configured in the builder, and if the value passes all filters, it applies the binder function.
    /// This is useful when the transformation itself might fail and return None.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<TTarget> Bind<TSource, TTarget>(in this FilterBuilder<TSource> builder,
        Func<TSource, Option<TTarget>> binder)
        where TSource : notnull
        where TTarget : notnull
        => builder.Build().Bind(binder);
}
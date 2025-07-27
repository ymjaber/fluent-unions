using System.Runtime.CompilerServices;

namespace FluentUnions;

/// <summary>
/// Provides transformation extension methods for <see cref="Option{T}"/> types.
/// </summary>
public static partial class OptionExtensions
{
    /// <summary>
    /// Transforms the value inside an <see cref="Option{TSource}"/> using the provided mapping function.
    /// </summary>
    /// <typeparam name="TSource">The type of the value contained in the source option.</typeparam>
    /// <typeparam name="TTarget">The type of the value in the resulting option after transformation.</typeparam>
    /// <param name="option">The option containing the value to transform.</param>
    /// <param name="mapper">The function that transforms the value from <typeparamref name="TSource"/> to <typeparamref name="TTarget"/>.</param>
    /// <returns>
    /// An <see cref="Option{TTarget}"/> containing the transformed value if the source option contains a value;
    /// otherwise, <see cref="Option{TTarget}.None"/>.
    /// </returns>
    /// <remarks>
    /// This method applies the functor pattern to options. If the option is None, the mapping function is not called
    /// and None is returned. This allows for chaining transformations on optional values without explicit null checking.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<TTarget> Map<TSource, TTarget>(
        in this Option<TSource> option,
        Func<TSource, TTarget> mapper)
        where TSource : notnull
        where TTarget : notnull
    {
        if (option.IsNone) return Option<TTarget>.None;
        return Option.Some(mapper(option.Value));
    }
}
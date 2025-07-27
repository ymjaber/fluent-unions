using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FluentUnions;

/// <summary>
/// Provides a fluent builder pattern for applying filter conditions to optional values.
/// </summary>
/// <typeparam name="TValue">The type of the value being filtered.</typeparam>
/// <remarks>
/// The FilterBuilder allows chaining multiple filter conditions. If any condition fails,
/// the result becomes None. This provides a more readable way to apply multiple validations.
/// </remarks>
[StructLayout(LayoutKind.Auto)]
public readonly struct FilterBuilder<TValue>
    where TValue : notnull
{
    private readonly bool _isSome;
    private readonly TValue? _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="FilterBuilder{TValue}"/> struct from an option.
    /// </summary>
    /// <param name="option">The option to start filtering from.</param>
    internal FilterBuilder(Option<TValue> option) => (_isSome, _value) = option.IsSome
        ? (true, option.Value)
        : (false, default);

    /// <summary>
    /// Initializes a new instance of the <see cref="FilterBuilder{TValue}"/> struct from a value.
    /// </summary>
    /// <param name="value">The value to start filtering from.</param>
    /// <exception cref="ArgumentNullException">The value is null.</exception>
    internal FilterBuilder(TValue value) =>
        (_isSome, _value) = (true, value ?? throw new ArgumentNullException(nameof(value)));
    
    /// <summary>
    /// Implicitly converts a <see cref="FilterBuilder{TValue}"/> to an <see cref="Option{TValue}"/>.
    /// </summary>
    /// <param name="builder">The filter builder to convert.</param>
    /// <returns>An option containing the filtered value or None if any filter failed.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Option<TValue>(FilterBuilder<TValue> builder) => builder.Build();

    /// <summary>
    /// Applies a filter condition to the value.
    /// </summary>
    /// <param name="predicate">The condition that the value must satisfy.</param>
    /// <returns>The current builder if the condition is satisfied; otherwise, an empty builder that will produce None.</returns>
    /// <remarks>
    /// Multiple calls to Satisfies can be chained. If any condition fails, all subsequent
    /// conditions are skipped and the final result will be None.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public FilterBuilder<TValue> Satisfies(Func<TValue, bool> predicate)
    {
        if (!_isSome) return this;
        
        if (predicate(_value!)) return this;

        return default;
    }

    /// <summary>
    /// Builds the final <see cref="Option{TValue}"/> result.
    /// </summary>
    /// <returns>Some containing the value if all filter conditions were satisfied; otherwise, None.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Option<TValue> Build() => _isSome ? Option.Some(_value!) : Option.None;
}
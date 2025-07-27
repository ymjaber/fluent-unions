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
/// 
/// WARNING: This struct is binary-compatible with <see cref="Option{TValue}"/> for performance reasons.
/// The following invariants MUST be maintained:
/// - LayoutKind must remain Sequential
/// - Field order must be: _isSome (bool), _value (TValue?)
/// - Field types must not change
/// Any modification to these aspects will break unsafe transformations.
/// </remarks>
[StructLayout(LayoutKind.Sequential)]
public readonly struct FilterBuilder<TValue>
    where TValue : notnull
{
    private readonly bool _isSome;
    private readonly TValue? _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="FilterBuilder{TValue}"/> struct from an option.
    /// </summary>
    /// <param name="option">The option to start filtering from.</param>
    /// <remarks>
    /// This constructor performs an unsafe transformation, treating Option{TValue} and FilterBuilder{TValue} as binary compatible.
    /// </remarks>
    internal unsafe FilterBuilder(Option<TValue> option)
    {
        this = Unsafe.As<Option<TValue>, FilterBuilder<TValue>>(ref option);
    }

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
    public Option<TValue> Satisfies(Func<TValue, bool> predicate)
    {
        if (!_isSome) return new(this);

        if (!predicate(_value!)) return default;

        return new(this);
    }
}

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FluentUnions;

/// <summary>
/// Represents a wrapper that behaves as an optional value.
/// </summary>
/// <typeparam name="TValue">The type of the value.</typeparam>
/// <remarks>
/// WARNING: This struct is binary-compatible with <see cref="FilterBuilder{TValue}"/> for performance reasons.
/// The following invariants MUST be maintained:
/// - LayoutKind must remain Sequential
/// - Field order must be: _isSome (bool), _value (TValue?)
/// - Field types must not change
/// Any modification to these aspects will break unsafe transformations.
/// </remarks>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
public readonly struct Option<TValue> : IEquatable<Option<TValue>>, IEquatable<TValue>
    where TValue : notnull
{
    /// <summary>
    /// Represents an option with no value.
    /// </summary>
    public static Option<TValue> None = default;

    private readonly bool _isSome;
    private readonly TValue? _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="Option{TValue}"/> struct with a value.
    /// </summary>
    /// <param name="value">The value to wrap in the option.</param>
    internal Option(TValue value)
    {
        _isSome = true;
        _value = value;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Option{TValue}"/> struct from a <see cref="FilterBuilder{TValue}"/> using unsafe transformation.
    /// </summary>
    /// <param name="filterBuilder">The filter builder to transform.</param>
    /// <remarks>
    /// This constructor performs an unsafe transformation, treating FilterBuilder{TValue} and Option{TValue} as binary compatible.
    /// </remarks>
    internal unsafe Option(FilterBuilder<TValue> filterBuilder)
    {
        this = Unsafe.As<FilterBuilder<TValue>, Option<TValue>>(ref filterBuilder);
    }

    /// <summary>
    /// Gets a value indicating whether the option has a value.
    /// </summary>
    public bool IsSome
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _isSome;
    }

    /// <summary>
    /// Gets a value indicating whether the option has no value.
    /// </summary>
    public bool IsNone
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => !_isSome;
    }

    /// <summary>
    /// Gets the value of the option if it has one; otherwise, throws an <see cref="InvalidOperationException"/>.
    /// </summary>
    /// <exception cref="InvalidOperationException">The option is None.</exception>
    public TValue Value
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _isSome ? _value! : throw new InvalidOperationException("Option is None");
    }

    /// <summary>
    /// Returns a <see cref="FilterBuilder{TValue}"/> for validating built-in predicates.
    /// </summary>
    /// <returns>A new <see cref="FilterBuilder{TValue}"/> instance for build-in predicates.</returns>
    public FilterBuilder<TValue> Filter => new(this);


    /// <summary>
    /// Implicitly converts a value to an <see cref="Option{TValue}"/> containing that value.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>An option containing the specified value.</returns>
    public static implicit operator Option<TValue>(TValue value) => Option.Some(value);

    /// <summary>
    /// Explicitly converts an <see cref="Option{TValue}"/> to its underlying value.
    /// </summary>
    /// <param name="option">The option to convert.</param>
    /// <returns>The underlying value.</returns>
    /// <exception cref="InvalidOperationException">The option is None.</exception>
    public static explicit operator TValue(Option<TValue> option) => option.Value;

    /// <summary>
    /// Implicitly converts the global None to a typed <see cref="Option{TValue}"/>.
    /// </summary>
    /// <param name="_">The global None value.</param>
    /// <returns>A None option of the appropriate type.</returns>
    public static implicit operator Option<TValue>(Option _) => None;

    /// <summary>
    /// Determines whether two options are equal.
    /// </summary>
    /// <param name="left">The first option to compare.</param>
    /// <param name="right">The second option to compare.</param>
    /// <returns><see langword="true"/> if the options are equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(Option<TValue> left, Option<TValue> right) => left.Equals(right);

    /// <summary>
    /// Determines whether two options are not equal.
    /// </summary>
    /// <param name="left">The first option to compare.</param>
    /// <param name="right">The second option to compare.</param>
    /// <returns><see langword="true"/> if the options are not equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(Option<TValue> left, Option<TValue> right) => !(left == right);

    /// <summary>
    /// Determines whether an option is equal to a value.
    /// </summary>
    /// <param name="left">The option to compare.</param>
    /// <param name="right">The value to compare.</param>
    /// <returns><see langword="true"/> if the option contains the specified value; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(Option<TValue> left, TValue right) => left.Equals(right);

    /// <summary>
    /// Determines whether an option is not equal to a value.
    /// </summary>
    /// <param name="left">The option to compare.</param>
    /// <param name="right">The value to compare.</param>
    /// <returns><see langword="true"/> if the option does not contain the specified value; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(Option<TValue> left, TValue right) => !(left == right);

    /// <summary>
    /// Determines whether a value is equal to an option.
    /// </summary>
    /// <param name="left">The value to compare.</param>
    /// <param name="right">The option to compare.</param>
    /// <returns><see langword="true"/> if the option contains the specified value; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(TValue left, Option<TValue> right) => right.Equals(left);

    /// <summary>
    /// Determines whether a value is not equal to an option.
    /// </summary>
    /// <param name="left">The value to compare.</param>
    /// <param name="right">The option to compare.</param>
    /// <returns><see langword="true"/> if the option does not contain the specified value; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(TValue left, Option<TValue> right) => !(left == right);

    /// <inheritdoc/>
    public override bool Equals([NotNullWhen(true)] object? obj) => obj switch
    {
        Option<TValue> other => Equals(other),
        TValue other => Equals(other),
        _ => false
    };

    /// <summary>
    /// Determines whether the specified option is equal to the current option.
    /// </summary>
    /// <param name="other">The option to compare with the current option.</param>
    /// <returns><see langword="true"/> if the specified option is equal to the current option; otherwise, <see langword="false"/>.</returns>
    public bool Equals(Option<TValue> other)
    {
        if (IsSome && other.IsSome) return EqualityComparer<TValue>.Default.Equals(Value, other.Value);

        return IsNone && other.IsNone;
    }

    /// <summary>
    /// Determines whether the specified value is equal to the current option.
    /// </summary>
    /// <param name="other">The value to compare with the current option.</param>
    /// <returns><see langword="true"/> if the specified value is equal to the current option; otherwise, <see langword="false"/>.</returns>
    public bool Equals(TValue? other)
    {
        if (IsSome && other is not null) return EqualityComparer<TValue>.Default.Equals(Value, other);

        return IsNone && other is null;
    }

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(_isSome, _value);

    /// <summary>
    /// Returns the option's state including its value if it has one.
    /// </summary>
    /// <returns>A string representation of the option.</returns>
    public override string? ToString() => _isSome ? $"Some({_value})" : "None";

    /// <summary>
    /// Attempts to get the value of the option.
    /// </summary>
    /// <param name="value">The value of the option if it has one; otherwise, the default value of <typeparamref name="TValue"/>.</param>
    /// <returns><see langword="true"/> if the option has a value; otherwise, <see langword="false"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool TryGetValue([MaybeNullWhen(false)] out TValue value)
    {
        value = _value!;
        return _isSome;
    }
}

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FluentUnions;

/// <summary>
/// Provides a fluent builder pattern for validating values and building results based on multiple conditions.
/// </summary>
/// <typeparam name="TValue">The type of the value being validated.</typeparam>
/// <remarks>
/// The EnsureBuilder allows chaining multiple validation conditions. If any validation fails,
/// the first error is captured and subsequent validations are skipped (short-circuit evaluation).
/// 
/// WARNING: This struct is binary-compatible with <see cref="Result{TValue}"/> for performance reasons.
/// The following invariants MUST be maintained:
/// - LayoutKind must remain Sequential
/// - Field order must be: _value (TValue?), _error (Error?)
/// - Field types must not change
/// Any modification to these aspects will break unsafe transformations.
/// </remarks>
[StructLayout(LayoutKind.Sequential)]
public readonly struct EnsureBuilder<TValue>
{
    private readonly TValue? _value;
    private readonly Error? _error;

    /// <summary>
    /// Initializes a new instance of the <see cref="EnsureBuilder{TValue}"/> struct from a result.
    /// </summary>
    /// <param name="result">The result to start validation from.</param>
    /// <remarks>
    /// This constructor performs an unsafe transformation, treating Result{TValue} and EnsureBuilder{TValue} as binary compatible.
    /// </remarks>
    internal unsafe EnsureBuilder(Result<TValue> result)
    {
        this = Unsafe.As<Result<TValue>, EnsureBuilder<TValue>>(ref result);
    }

    /// <summary>
    /// Validates that the value satisfies the specified condition.
    /// </summary>
    /// <param name="predicate">The condition that the value must satisfy.</param>
    /// <param name="error">The error to return if the condition is not satisfied.</param>
    /// <returns>The current builder if the condition is satisfied; otherwise, a builder containing the error.</returns>
    /// <remarks>
    /// If a previous validation has already failed, this method returns immediately without
    /// evaluating the predicate (short-circuit evaluation).
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Result<TValue> Satisfies(Func<TValue, bool> predicate, Error error)
    {
        if (_error is not null) return new(this);

        if (!predicate(_value!)) return new(error);

        return new(this);
    }
}

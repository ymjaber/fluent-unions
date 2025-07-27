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
/// </remarks>
[StructLayout(LayoutKind.Auto)]
public readonly struct EnsureBuilder<TValue>
{
    private readonly TValue? _value;
    private readonly Error? _error;

    /// <summary>
    /// Initializes a new instance of the <see cref="EnsureBuilder{TValue}"/> struct from a result.
    /// </summary>
    /// <param name="result">The result to start validation from.</param>
    internal EnsureBuilder(Result<TValue> result) => (_value, _error) = result.IsSuccess
        ? (result.Value, null)
        : (default(TValue), result.Error);

    /// <summary>
    /// Initializes a new instance of the <see cref="EnsureBuilder{TValue}"/> struct from a value.
    /// </summary>
    /// <param name="value">The value to start validation from.</param>
    internal EnsureBuilder(TValue value) => _value = value;

    /// <summary>
    /// Initializes a new instance of the <see cref="EnsureBuilder{TValue}"/> struct with an error.
    /// </summary>
    /// <param name="error">The error that caused validation to fail.</param>
    private EnsureBuilder(Error error) => _error = error;

    /// <summary>
    /// Implicitly converts an <see cref="EnsureBuilder{TValue}"/> to a <see cref="Result{TValue}"/>.
    /// </summary>
    /// <param name="builder">The ensure builder to convert.</param>
    /// <returns>A result containing either the validated value or the first validation error.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Result<TValue>(EnsureBuilder<TValue> builder) => builder.Build();

    /// <summary>
    /// Implicitly converts an <see cref="EnsureBuilder{TValue}"/> to a unit <see cref="Result"/>.
    /// </summary>
    /// <param name="builder">The ensure builder to convert.</param>
    /// <returns>A unit result preserving the success/failure state but discarding any value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Result(EnsureBuilder<TValue> builder) =>
        builder._error ?? Result.Success();

    /// <summary>
    /// Determines whether the builder is in a success state.
    /// </summary>
    /// <returns><see langword="true"/> if no validation has failed; otherwise, <see langword="false"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool IsSuccess() => _error is null;

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
    public EnsureBuilder<TValue> Satisfies(Func<TValue, bool> predicate, Error error)
    {
        if (_error is not null) return new(_error);

        if (!predicate(_value!)) return new(error);
        return this;
    }

    /// <summary>
    /// Builds the final <see cref="Result{TValue}"/> from the validation chain.
    /// </summary>
    /// <returns>A successful result containing the value if all validations passed; otherwise, a failed result with the first error.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Result<TValue> Build() => IsSuccess() ? _value! : _error!;
}
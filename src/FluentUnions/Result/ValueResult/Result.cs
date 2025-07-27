using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FluentUnions;

/// <summary>
/// Represents the result of an operation that can either succeed with a value or fail with an error.
/// </summary>
/// <typeparam name="TValue">The type of the value returned on success.</typeparam>
/// <remarks>
/// WARNING: This struct is binary-compatible with <see cref="EnsureBuilder{TValue}"/> for performance reasons.
/// The following invariants MUST be maintained:
/// - LayoutKind must remain Sequential
/// - Field order must be: _value (TValue?), _error (Error?)
/// - Field types must not change
/// Any modification to these aspects will break unsafe transformations.
/// </remarks>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
public readonly struct Result<TValue>
{
    private readonly TValue? _value;
    private readonly Error? _error;

    /// <summary>
    /// Initializes a new instance of the <see cref="Result{TValue}"/> struct with a successful value.
    /// </summary>
    /// <param name="value">The value of the successful result.</param>
    internal Result(TValue value) => _value = value;

    /// <summary>
    /// Initializes a new instance of the <see cref="Result{TValue}"/> struct with a failure error.
    /// </summary>
    /// <param name="error">The error that caused the failure.</param>
    internal Result(Error error) => _error = error;

    /// <summary>
    /// Initializes a new instance of the <see cref="Result{TValue}"/> struct from an <see cref="EnsureBuilder{TValue}"/> using unsafe transformation.
    /// </summary>
    /// <param name="ensureBuilder">The ensure builder to transform.</param>
    /// <remarks>
    /// This constructor performs an unsafe transformation, treating EnsureBuilder{TValue} and Result{TValue} as binary compatible.
    /// </remarks>
    internal unsafe Result(EnsureBuilder<TValue> ensureBuilder)
    {
        this = Unsafe.As<EnsureBuilder<TValue>, Result<TValue>>(ref ensureBuilder);
    }

    /// <summary>
    /// Gets a value indicating whether the result represents a successful operation.
    /// </summary>
    public bool IsSuccess
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _error is null;
    }

    /// <summary>
    /// Gets a value indicating whether the result represents a failed operation.
    /// </summary>
    public bool IsFailure
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => !IsSuccess;
    }

    /// <summary>
    /// Gets the error associated with a failed result.
    /// </summary>
    /// <exception cref="InvalidOperationException">The result is in a success state.</exception>
    public Error Error
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _error ?? throw new InvalidOperationException("Result is not in a failure state.");
    }

    /// <summary>
    /// Gets the value of a successful result.
    /// </summary>
    /// <exception cref="InvalidOperationException">The result is in a failure state.</exception>
    public TValue Value
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => IsSuccess
            ? _value!
            : throw new InvalidOperationException("Result is not in a success state.");
    }

    /// <summary>
    /// Returns an <see cref="EnsureBuilder{TValue}"/> that provides a fluent interface for validating built-in perdicates.
    /// </summary>
    /// <returns>An <see cref="EnsureBuilder{TValue}"/> for applying a built-in predicate.</returns>
    public EnsureBuilder<TValue> Ensure => new(this);

    /// <summary>
    /// Implicitly converts a value to a successful <see cref="Result{TValue}"/>.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>A successful result containing the specified value.</returns>
    public static implicit operator Result<TValue>(TValue value) => new(value);

    /// <summary>
    /// Implicitly converts an <see cref="Error"/> to a failed <see cref="Result{TValue}"/>.
    /// </summary>
    /// <param name="error">The error to convert.</param>
    /// <returns>A failed result containing the specified error.</returns>
    public static implicit operator Result<TValue>(Error error) => new(error);

    /// <summary>
    /// Implicitly converts a <see cref="Result{TValue}"/> to a unit <see cref="Result"/>.
    /// </summary>
    /// <param name="result">The result to convert.</param>
    /// <returns>A unit result preserving the success/failure state but discarding any value.</returns>
    public static implicit operator Result(Result<TValue> result) =>
        result.IsSuccess ? Result.Success() : Result.Failure(result.Error);

    /// <summary>
    /// Attempts to get the value from the result.
    /// </summary>
    /// <param name="value">When this method returns, contains the value if the result is in a success state; otherwise, the default value.</param>
    /// <returns><see langword="true"/> if the result is in a success state; otherwise, <see langword="false"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool TryGetValue([NotNullWhen(true)] out TValue? value)
    {
        value = _value;
        return IsSuccess;
    }

    /// <summary>
    /// Attempts to get the error from the result.
    /// </summary>
    /// <param name="error">When this method returns, contains the error if the result is in a failure state; otherwise, null.</param>
    /// <returns><see langword="true"/> if the result is in a failure state; otherwise, <see langword="false"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool TryGetError([NotNullWhen(true)] out Error? error)
    {
        error = _error;
        return IsFailure;
    }

    /// <summary>
    /// Returns a string representation of the result.
    /// </summary>
    /// <returns>"Success: " followed by the value if successful; otherwise, "Failure: " followed by the error details.</returns>
    public override string ToString() =>
        IsSuccess
            ? $"Success: {Value}"
            : $"Failure: {Error}";
}

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace FluentUnions;

/// <summary>
/// Represents the result of an operation that can either succeed with no value or fail with an error.
/// This is the unit result type, used when the operation doesn't return a value on success.
/// </summary>
[Serializable]
public readonly partial struct Result
{
    private readonly Error? _error;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="Result"/> struct with a failure state.
    /// </summary>
    /// <param name="error">The error that caused the failure.</param>
    private Result(Error error) => _error = error;
    
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
    public Error Error => _error ?? throw new InvalidOperationException("Result is not in a failure state.");
    
    /// <summary>
    /// Implicitly converts an <see cref="Error"/> to a failed <see cref="Result"/>.
    /// </summary>
    /// <param name="error">The error to convert.</param>
    /// <returns>A failed result containing the specified error.</returns>
    public static implicit operator Result(Error error) => new(error);

    /// <summary>
    /// Attempts to get the error from the result.
    /// </summary>
    /// <param name="error">When this method returns, contains the error if the result is in a failure state; otherwise, null.</param>
    /// <returns><see langword="true"/> if the result is in a failure state; otherwise, <see langword="false"/>.</returns>
    public bool TryGetError([NotNullWhen(true)] out Error? error)
    {
        error = _error;
        return IsFailure;
    }

    /// <summary>
    /// Returns a string representation of the result.
    /// </summary>
    /// <returns>"Success" if the result is successful; otherwise, "Failure: " followed by the error details.</returns>
    public override string ToString() =>
        IsSuccess
            ? "Success"
            : $"Failure: {Error}";
}
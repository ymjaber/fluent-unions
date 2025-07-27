using System.Diagnostics.CodeAnalysis;

namespace FluentUnions;

/// <summary>
/// Provides a builder pattern for constructing errors, including the ability to aggregate multiple errors.
/// </summary>
public sealed class ErrorBuilder
{
    private readonly List<Error> _errors = [];

    /// <summary>
    /// Gets a value indicating whether any errors have been added to the builder.
    /// </summary>
    public bool HasErrors => _errors.Count > 0;

    /// <summary>
    /// Appends an error to the builder.
    /// </summary>
    /// <param name="error">The error to append. If this is an <see cref="AggregateError"/>, its individual errors will be added.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public ErrorBuilder Append(Error error)
    {
        if (error is AggregateError aggregateError) _errors.AddRange(aggregateError.Errors);
        else _errors.Add(error);

        return this;
    }

    /// <summary>
    /// Appends the error from a failed result to the builder.
    /// </summary>
    /// <param name="result">The result to check. If it's in a failure state, its error will be appended.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public ErrorBuilder AppendOnFailure(Result result)
    {
        if (result.IsFailure) Append(result.Error);
        return this;
    }

    /// <summary>
    /// Appends the error from a failed result to the builder.
    /// </summary>
    /// <typeparam name="T">The type of the value in the result.</typeparam>
    /// <param name="result">The result to check. If it's in a failure state, its error will be appended.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public ErrorBuilder AppendOnFailure<T>(Result<T> result)
    {
        if (result.IsFailure) Append(result.Error);
        return this;
    }

    /// <summary>
    /// Attempts to build an error from the accumulated errors.
    /// </summary>
    /// <param name="error">When this method returns, contains the built error if any errors were added; otherwise, null.</param>
    /// <returns><see langword="true"/> if an error was built; otherwise, <see langword="false"/>.</returns>
    /// <remarks>
    /// If no errors were added, returns null. If one error was added, returns that error.
    /// If multiple errors were added, returns an <see cref="AggregateError"/> containing all errors.
    /// </remarks>
    public bool TryBuild([NotNullWhen(true)] out Error? error)
    {
        error = _errors.Count switch
        {
            0 => null,
            1 => _errors[0],
            _ => new AggregateError(_errors)
        };

        return error is not null;
    }

    /// <summary>
    /// Builds an error from the accumulated errors.
    /// </summary>
    /// <returns>The built error. If one error was added, returns that error. If multiple errors were added, returns an <see cref="AggregateError"/>.</returns>
    /// <exception cref="InvalidOperationException">No errors have been added to the builder.</exception>
    public Error Build() => TryBuild(out Error? error)
        ? error
        : throw new InvalidOperationException("No errors to build.");
}

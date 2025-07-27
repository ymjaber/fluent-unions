using FluentUnions.PreDefinedEnsure;

namespace FluentUnions;

/// <summary>Provides pre-defined validation predicates for boolean values in <see cref="Result{T}"/> types.</summary>
public static partial class EnsureBuilderExtensions
{
    /// <summary>
    /// Ensures that the boolean value is true.
    /// </summary>
    /// <param name="ensure">The ensure builder for the boolean value.</param>
    /// <param name="error">Optional custom error. If null, a default error message will be used.</param>
    /// <returns>A unit result indicating success if the value is true; otherwise, a failure result.</returns>
    public static Result True(in this EnsureBuilder<bool> ensure, Error? error = null) =>
        ensure.Satisfies(v => v, error ?? BooleanErrors.NotTrue).DiscardValue();

    /// <summary>
    /// Ensures that the boolean value is false.
    /// </summary>
    /// <param name="ensure">The ensure builder for the boolean value.</param>
    /// <param name="error">Optional custom error. If null, a default error message will be used.</param>
    /// <returns>A unit result indicating success if the value is false; otherwise, a failure result.</returns>
    public static Result False(in this EnsureBuilder<bool> ensure, Error? error = null) =>
        ensure.Satisfies(v => !v, error ?? BooleanErrors.NotFalse).DiscardValue();
}

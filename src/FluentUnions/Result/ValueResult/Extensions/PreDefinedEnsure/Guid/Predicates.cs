using FluentUnions.PreDefinedEnsure;

namespace FluentUnions;

/// <summary>Provides pre-defined validation predicates for Guid values in <see cref="Result{T}"/> types.</summary>
public static partial class EnsureBuilderExtensions
{
    /// <summary>
    /// Ensures that the Guid value is empty (all zeros).
    /// </summary>
    /// <param name="ensure">The ensure builder for the Guid value.</param>
    /// <param name="error">Optional custom error. If null, a default error message will be used.</param>
    /// <returns>A unit result indicating success if the Guid is empty; otherwise, a failure result.</returns>
    public static Result Empty(
        in this EnsureBuilder<Guid> ensure,
        Error? error = null) =>
        ensure.Satisfies(v => v == Guid.Empty, error ?? GuidErrors.NotEmpty).DiscardValue();

    /// <summary>
    /// Ensures that the Guid value is not empty (not all zeros).
    /// </summary>
    /// <param name="ensure">The ensure builder for the Guid value.</param>
    /// <param name="error">Optional custom error. If null, a default error message will be used.</param>
    /// <returns>A result that validates the Guid is not empty.</returns>
    public static Result<Guid> NotEmpty(
        in this EnsureBuilder<Guid> ensure,
        Error? error = null) =>
        ensure.Satisfies(v => v != Guid.Empty, error ?? GuidErrors.Empty);
}

using FluentUnions.PreDefinedEnsure;

namespace FluentUnions;

/// <summary>Provides pre-defined validation predicates for Guid values in <see cref="Result{T}"/> types.</summary>
public static partial class EnsureBuilderExtensions
{
    public static Result Empty(
        in this EnsureBuilder<Guid> ensure,
        Error? error = null) =>
        ensure.Satisfies(v => v == Guid.Empty, error ?? GuidErrors.NotEmpty).Build().DiscardValue();

    public static EnsureBuilder<Guid> NotEmpty(
        in this EnsureBuilder<Guid> ensure,
        Error? error = null) =>
        ensure.Satisfies(v => v != Guid.Empty, error ?? GuidErrors.Empty);
}
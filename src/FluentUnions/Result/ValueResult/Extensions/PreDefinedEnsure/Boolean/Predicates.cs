using FluentUnions.PreDefinedEnsure;

namespace FluentUnions;

/// <summary>Provides pre-defined validation predicates for boolean values in <see cref="Result{T}"/> types.</summary>
public static partial class EnsureBuilderExtensions
{
    public static Result True(in this EnsureBuilder<bool> ensure, Error? error = null) =>
        ensure.Satisfies(v => v, error ?? BooleanErrors.NotTrue).Build().DiscardValue();

    public static Result False(in this EnsureBuilder<bool> ensure, Error? error = null) =>
        ensure.Satisfies(v => !v, error ?? BooleanErrors.NotFalse).Build().DiscardValue();
}
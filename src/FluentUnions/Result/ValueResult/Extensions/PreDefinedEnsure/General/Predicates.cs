using FluentUnions.PreDefinedEnsure;

namespace FluentUnions;

/// <summary>Provides pre-defined general-purpose validation predicates for <see cref="Result{T}"/> types.</summary>
public static partial class EnsureBuilderExtensions
{
    public static Result IsEqualTo<TValue>(
        in this EnsureBuilder<TValue> ensure,
        TValue value,
        Error? error = null)
        where TValue : IEquatable<TValue> =>
        ensure.Satisfies(v => v.Equals(value), error ?? GeneralErrors.NotEqual).Build().DiscardValue();

    public static EnsureBuilder<TValue> IsNotEqualTo<TValue>(
        in this EnsureBuilder<TValue> ensure,
        TValue value,
        Error? error = null)
        where TValue : IEquatable<TValue> =>
        ensure.Satisfies(v => !v.Equals(value), error ?? GeneralErrors.Equal);
}
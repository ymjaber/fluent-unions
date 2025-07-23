using FluentUnions.PreDefinedEnsure;

namespace FluentUnions;

/// <summary>Provides pre-defined validation predicates for enum values in <see cref="Result{T}"/> types.</summary>
public static partial class EnsureBuilderExtensions
{
    public static EnsureBuilder<TEnum> Defined<TEnum>(
        in this EnsureBuilder<TEnum> ensure,
        Error? error = null)
        where TEnum : struct, Enum =>
        ensure.Satisfies(Enum.IsDefined, error ?? EnumErrors.NotDefined);
}
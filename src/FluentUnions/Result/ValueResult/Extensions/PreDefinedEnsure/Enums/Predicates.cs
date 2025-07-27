using FluentUnions.PreDefinedEnsure;

namespace FluentUnions;

/// <summary>Provides pre-defined validation predicates for enum values in <see cref="Result{T}"/> types.</summary>
public static partial class EnsureBuilderExtensions
{
    /// <summary>
    /// Ensures that the enum value is defined in the enum type.
    /// </summary>
    /// <typeparam name="TEnum">The type of the enum value.</typeparam>
    /// <param name="ensure">The ensure builder for the enum value.</param>
    /// <param name="error">Optional custom error. If null, a default error message will be used.</param>
    /// <returns>A result that validates the enum value is defined.</returns>
    public static Result<TEnum> Defined<TEnum>(
        in this EnsureBuilder<TEnum> ensure,
        Error? error = null)
        where TEnum : struct, Enum =>
        ensure.Satisfies(Enum.IsDefined, error ?? EnumErrors.NotDefined);
}

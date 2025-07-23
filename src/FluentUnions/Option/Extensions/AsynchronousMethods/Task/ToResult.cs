using FluentUnions.PreDefinedEnsure;

namespace FluentUnions;

/// <summary>Provides asynchronous conversion methods from <see cref="Option{T}"/> to <see cref="Result{T}"/> types using Task-based operations.</summary>
public static partial class OptionExtensions
{
    public static async Task<Result<TValue>> EnsureSomeAsync<TValue>(this Task<Option<TValue>> option, Error? error = null)
        where TValue : notnull
        => (await option.ConfigureAwait(false)).EnsureSome(error);

    public static async Task<Result> EnsureNoneAsync<TValue>(this Task<Option<TValue>> option, Error? error = null)
        where TValue : notnull
        => (await option.ConfigureAwait(false)).EnsureNone(error);
}
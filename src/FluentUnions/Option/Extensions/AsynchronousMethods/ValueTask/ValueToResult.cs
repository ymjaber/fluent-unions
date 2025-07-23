using FluentUnions.PreDefinedEnsure;

namespace FluentUnions;

/// <summary>Provides asynchronous conversion methods from <see cref="Option{T}"/> to <see cref="Result{T}"/> types using ValueTask-based operations.</summary>
public static partial class OptionExtensions
{
    public static async ValueTask<Result<TValue>> EnsureSomeAsync<TValue>(this ValueTask<Option<TValue>> option, Error? error = null)
        where TValue : notnull
        => (await option.ConfigureAwait(false)).EnsureSome(error);

    public static async ValueTask<Result> EnsureNoneAsync<TValue>(this ValueTask<Option<TValue>> option, Error? error = null)
        where TValue : notnull
        => (await option.ConfigureAwait(false)).EnsureNone(error);
}
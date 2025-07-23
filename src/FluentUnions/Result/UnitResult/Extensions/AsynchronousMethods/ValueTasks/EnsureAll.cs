namespace FluentUnions;

/// <summary>Provides ValueTask-based asynchronous validation methods that aggregate all errors for <see cref="Result"/> types.</summary>
public static partial class UnitResultExtensions
{
    public static async ValueTask<Result> EnsureAllAsync(this Result result, ValueTask<bool> condition, Error error)
    {
        var errorBuilder = new ErrorBuilder().AppendOnFailure(result);

        if (!await condition.ConfigureAwait(false)) errorBuilder.Append(error);

        return errorBuilder.TryBuild(out var err) ? Result.Failure(err) : Result.Success();
    }

    public static async ValueTask<Result> EnsureAllAsync(this ValueTask<Result> result, bool condition, Error error) =>
        (await result.ConfigureAwait(false)).EnsureAll(condition, error);

    public static async ValueTask<Result> EnsureAllAsync(this ValueTask<Result> result, ValueTask<bool> condition, Error error) =>
        await (await result.ConfigureAwait(false)).EnsureAllAsync(condition, error).ConfigureAwait(false);
}
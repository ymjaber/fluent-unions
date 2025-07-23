namespace FluentUnions;

/// <summary>Provides Task-based asynchronous validation methods that aggregate all errors for <see cref="Result"/> types.</summary>
public static partial class UnitResultExtensions
{
    public static async Task<Result> EnsureAllAsync(this Result result, Task<bool> condition, Error error)
    {
        var errorBuilder = new ErrorBuilder().AppendOnFailure(result);

        if (!await condition.ConfigureAwait(false)) errorBuilder.Append(error);

        return errorBuilder.TryBuild(out var err) ? Result.Failure(err) : Result.Success();
    }

    public static async Task<Result> EnsureAllAsync(this Task<Result> result, bool condition, Error error) =>
        (await result.ConfigureAwait(false)).EnsureAll(condition, error);

    public static async Task<Result> EnsureAllAsync(this Task<Result> result, Task<bool> condition, Error error) =>
        await (await result.ConfigureAwait(false)).EnsureAllAsync(condition, error).ConfigureAwait(false);
}
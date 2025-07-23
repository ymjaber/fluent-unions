using FluentUnions.PreDefinedEnsure;

namespace FluentUnions;

public readonly partial struct Result
{
    /// <summary>
    /// Asynchronously creates an EnsureBuilder for fluently validating a value against multiple conditions.
    /// </summary>
    /// <typeparam name="TValue">The type of the value to validate.</typeparam>
    /// <param name="value">The task that will produce the value to validate.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an EnsureBuilder instance for chaining validation conditions.</returns>
    public static async Task<EnsureBuilder<TValue>> EnsureThatAsync<TValue>(Task<TValue> value) =>
        new(await value.ConfigureAwait(false));

    /// <summary>
    /// Asynchronously ensures that a reference type value is not null.
    /// </summary>
    /// <typeparam name="TValue">The reference type of the value to check.</typeparam>
    /// <param name="value">The task that will produce the nullable value to check.</param>
    /// <param name="error">The error to return if the value is null. If not provided, a default null error is used.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a success result with the value if it's not null; otherwise, a failure result with the specified error.</returns>
    public static async Task<Result<TValue>> EnsureNotNullAsync<TValue>(Task<TValue?> value, Error error = null!)
        where TValue : class
        => EnsureNotNull(await value.ConfigureAwait(false), error);

    /// <summary>
    /// Asynchronously ensures that a nullable value type is not null.
    /// </summary>
    /// <typeparam name="TValue">The value type to check.</typeparam>
    /// <param name="value">The task that will produce the nullable value to check.</param>
    /// <param name="error">The error to return if the value is null. If not provided, a default null error is used.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a success result with the unwrapped value if it's not null; otherwise, a failure result with the specified error.</returns>
    public static async Task<Result<TValue>> EnsureNotNullAsync<TValue>(Task<TValue?> value, Error error = null!)
        where TValue : struct
        => EnsureNotNull(await value.ConfigureAwait(false), error);
}
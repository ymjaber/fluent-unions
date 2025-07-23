namespace FluentUnions;

public static partial class ResultExtensions
{
    /// <summary>
    /// Throws an exception if the task result is a failure.
    /// </summary>
    /// <param name="resultTask">The task containing the result to check.</param>
    /// <param name="exceptionSelector">A function that creates an exception from the error. If null, throws an InvalidOperationException with the error message.</param>
    /// <exception cref="Exception">The exception created by the selector if the result is a failure.</exception>
    public static async Task ThrowIfFailureAsync(
        this Task<Result> resultTask,
        Func<Error, Exception>? exceptionSelector = null)
    {
        var result = await resultTask;
        result.ThrowIfFailure(exceptionSelector);
    }
}
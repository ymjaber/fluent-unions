namespace FluentUnions;

public static partial class ResultExtensions
{
    /// <summary>
    /// Throws an exception if the result is a failure.
    /// </summary>
    /// <param name="result">The result to check.</param>
    /// <param name="exceptionSelector">A function that creates an exception from the error. If null, throws an InvalidOperationException with the error message.</param>
    /// <exception cref="Exception">The exception created by the selector if the result is a failure.</exception>
    public static void ThrowIfFailure(
        this Result result,
        Func<Error, Exception>? exceptionSelector = null)
    {
        if (result.IsSuccess) return;
        
        var selector = exceptionSelector ?? (error => new InvalidOperationException(error.Message));
        throw selector(result.Error);
    }
}
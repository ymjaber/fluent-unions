namespace FluentUnions;

public static partial class ResultExtensions
{
    /// <summary>
    /// Gets the value of the result if it's successful; otherwise, throws an exception created by the specified selector.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="result">The result to get the value from.</param>
    /// <param name="exceptionSelector">A function that creates an exception from the error. If null, throws an InvalidOperationException with the error message.</param>
    /// <returns>The value if the result is successful.</returns>
    /// <exception cref="Exception">The exception created by the selector if the result is a failure.</exception>
    public static TValue GetValueOrThrow<TValue>(
        this Result<TValue> result,
        Func<Error, Exception>? exceptionSelector = null)
        where TValue : notnull
    {
        if (result.IsSuccess) return result.Value;
        
        var selector = exceptionSelector ?? (error => new InvalidOperationException(error.Message));
        throw selector(result.Error);
    }
}
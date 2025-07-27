using FluentUnions.Tests.Errors;

namespace FluentUnions.Tests.ResultTests.Extensions;

public class ThrowIfFailureTests
{
    [Fact]
    public void ThrowIfFailure_WhenSuccess_DoesNotThrow()
    {
        // Arrange
        var result = Result.Success();

        // Act & Assert
        var exception = Record.Exception(() => result.ThrowIfFailure());
        Assert.Null(exception);
    }

    [Fact]
    public void ThrowIfFailure_WhenFailure_ThrowsException()
    {
        // Arrange
        var error = new Error("E001", "Something went wrong");
        var result = Result.Failure(error);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => result.ThrowIfFailure());
        Assert.Equal("Something went wrong", exception.Message);
    }

    [Fact]
    public void ThrowIfFailure_WithCustomExceptionFactory()
    {
        // Arrange
        var error = new Error("E001", "Custom error");
        var result = Result.Failure(error);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() =>
            result.ThrowIfFailure(err => new InvalidOperationException($"Custom: {err.Code}")));
        Assert.Equal("Custom: E001", exception.Message);
    }

    [Fact]
    public void ThrowIfFailure_PreservesErrorMetadata()
    {
        // Arrange
        var metadata = new Dictionary<string, object> 
        { 
            ["UserId"] = "123", 
            ["Timestamp"] = "2023-01-01" 
        };
        var error = new TestError("E001", "Error with metadata", metadata);
        var result = Result.Failure(error);

        // Act & Assert
        var exception = Assert.Throws<Exception>(() =>
            result.ThrowIfFailure(err =>
            {
                var testError = err as TestError;
                var userId = testError?.Metadata.GetValueOrDefault("UserId");
                return new Exception($"Error for user {userId}");
            }));
        Assert.Equal("Error for user 123", exception.Message);
    }

    [Fact]
    public void ThrowIfFailure_WithValidationError()
    {
        // Arrange
        var error = new ValidationError("email", "Invalid email format");
        var result = Result.Failure(error);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => result.ThrowIfFailure());
        Assert.Equal("Invalid email format", exception.Message);
    }

    [Fact]
    public void ThrowIfFailure_ChainedAfterOperations()
    {
        // Arrange
        var result = Result.Success()
            .Ensure(() => false, new Error("CHECK_FAILED", "Condition not met"));

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => result.ThrowIfFailure());
        Assert.Equal("Condition not met", exception.Message);
    }
}
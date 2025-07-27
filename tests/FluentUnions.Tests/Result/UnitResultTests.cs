namespace FluentUnions.Tests.ResultTests
{

public class UnitResultTests
{
    [Fact]
    public void Success_CreatesSuccessResult()
    {
        // Arrange & Act
        var result = Result.Success();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
    }

    [Fact]
    public void Failure_CreatesFailureResult()
    {
        // Arrange
        var error = new Error("E001", "Something went wrong");

        // Act
        var result = Result.Failure(error);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal(error, result.Error);
    }

    [Fact]
    public void Error_WhenSuccess_ThrowsInvalidOperationException()
    {
        // Arrange
        var result = Result.Success();

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => result.Error);
        Assert.Equal("Result is not in a failure state.", exception.Message);
    }

    [Fact]
    public void ImplicitOperator_FromError_CreatesFailure()
    {
        // Arrange
        var error = new Error("E001", "Test error");

        // Act
        Result result = error;

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(error, result.Error);
    }

    [Fact]
    public void TryGetError_WhenFailure_ReturnsTrueAndError()
    {
        // Arrange
        var error = new Error("E001", "Test error");
        var result = Result.Failure(error);

        // Act
        var success = result.TryGetError(out var retrievedError);

        // Assert
        Assert.True(success);
        Assert.Equal(error, retrievedError);
    }

    [Fact]
    public void TryGetError_WhenSuccess_ReturnsFalseAndNull()
    {
        // Arrange
        var result = Result.Success();

        // Act
        var success = result.TryGetError(out var error);

        // Assert
        Assert.False(success);
        Assert.Null(error);
    }

    [Fact]
    public void Equals_BothSuccess_ReturnsTrue()
    {
        // Arrange
        var result1 = Result.Success();
        var result2 = Result.Success();

        // Act & Assert
        Assert.True(result1.Equals(result2));
        Assert.True(result1.IsSuccess && result2.IsSuccess);
        Assert.Equal(result1.IsSuccess, result2.IsSuccess);
    }

    [Fact]
    public void Equals_BothFailureWithSameError_ReturnsTrue()
    {
        // Arrange
        var error = new Error("E001", "Test error");
        var result1 = Result.Failure(error);
        var result2 = Result.Failure(error);

        // Act & Assert
        Assert.True(result1.Equals(result2));
        Assert.True(result1.IsFailure && result2.IsFailure);
        Assert.Equal(result1.Error, result2.Error);
    }

    [Fact]
    public void Equals_BothFailureWithDifferentError_ReturnsFalse()
    {
        // Arrange
        var result1 = Result.Failure(new Error("E001", "Error 1"));
        var result2 = Result.Failure(new Error("E002", "Error 2"));

        // Act & Assert
        Assert.False(result1.Equals(result2));
        Assert.True(result1.IsFailure && result2.IsFailure);
        Assert.NotEqual(result1.Error, result2.Error);
    }

    [Fact]
    public void Equals_SuccessAndFailure_ReturnsFalse()
    {
        // Arrange
        var success = Result.Success();
        var failure = Result.Failure(new Error("E001", "Test error"));

        // Act & Assert
        Assert.False(success.Equals(failure));
        Assert.False(failure.Equals(success));
    }

    [Fact]
    public void GetHashCode_SameResults_ReturnSameHash()
    {
        // Arrange
        var result1 = Result.Success();
        var result2 = Result.Success();

        // Act & Assert
        Assert.Equal(result1.GetHashCode(), result2.GetHashCode());
    }

    [Fact]
    public void GetHashCode_SameError_ReturnSameHash()
    {
        // Arrange
        var error = new Error("E001", "Test error");
        var result1 = Result.Failure(error);
        var result2 = Result.Failure(error);

        // Act & Assert
        Assert.Equal(result1.GetHashCode(), result2.GetHashCode());
    }

    [Fact]
    public void ToString_Success_ReturnsSuccess()
    {
        // Arrange
        var result = Result.Success();

        // Act
        var str = result.ToString();

        // Assert
        Assert.Equal("Success", str);
    }

    [Fact]
    public void ToString_Failure_ReturnsFormattedError()
    {
        // Arrange
        var error = new Error("E001", "Test error");
        var result = Result.Failure(error);

        // Act
        var str = result.ToString();

        // Assert
        Assert.Equal("Failure: Error: E001 - Test error", str);
    }

    [Fact]
    public void Failure_WithValidationError_WorksCorrectly()
    {
        // Arrange
        var error = new ValidationError("email", "Invalid email format");

        // Act
        var result = Result.Failure(error);

        // Assert
        Assert.True(result.IsFailure);
        Assert.IsType<ValidationError>(result.Error);
    }

    [Fact]
    public void Failure_WithNotFoundError_WorksCorrectly()
    {
        // Arrange
        var error = new NotFoundError("User", "123");

        // Act
        var result = Result.Failure(error);

        // Assert
        Assert.True(result.IsFailure);
        Assert.IsType<NotFoundError>(result.Error);
    }

    [Fact]
    public void DefaultValue_IsSuccess()
    {
        // Arrange & Act
        var result = default(Result);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
    }
}
}

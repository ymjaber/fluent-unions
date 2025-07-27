namespace FluentUnions.Tests.ResultTests.Extensions;

public class GetValueOrThrowTests
{
    [Fact]
    public void GetValueOrThrow_WhenSuccess_ReturnsValue()
    {
        // Arrange
        var result = Result.Success(42);

        // Act
        var value = result.GetValueOrThrow();

        // Assert
        Assert.Equal(42, value);
    }

    [Fact]
    public void GetValueOrThrow_WhenFailure_ThrowsException()
    {
        // Arrange
        var error = new Error("E001", "Something went wrong");
        var result = Result.Failure<int>(error);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => result.GetValueOrThrow());
        Assert.Equal("Something went wrong", exception.Message);
    }

    [Fact]
    public void GetValueOrThrow_WithCustomExceptionFactory()
    {
        // Arrange
        var error = new Error("NOT_FOUND", "User not found");
        var result = Result.Failure<string>(error);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() =>
            result.GetValueOrThrow(err => new InvalidOperationException($"Failed to get value: {err.Message}")));
        Assert.Equal("Failed to get value: User not found", exception.Message);
    }

    [Fact]
    public void GetValueOrThrow_AfterSuccessfulOperations()
    {
        // Arrange
        var result = Result.Success(10)
            .Map(x => x * 2)
            .Ensure(x => x > 0, new Error("NEGATIVE", "Value is negative"));

        // Act
        var value = result.GetValueOrThrow();

        // Assert
        Assert.Equal(20, value);
    }

    [Fact]
    public void GetValueOrThrow_AfterFailedOperations()
    {
        // Arrange
        var result = Result.Success(10)
            .Map(x => x * 2)
            .Ensure(x => x > 100, new Error("TOO_SMALL", "Value too small"));

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => result.GetValueOrThrow());
        Assert.Equal("Value too small", exception.Message);
    }

    [Fact]
    public void GetValueOrThrow_WithValidationError()
    {
        // Arrange
        var error = new ValidationError("age", "Must be 18 or older");
        var result = Result.Failure<int>(error);

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            result.GetValueOrThrow(err => new ArgumentException($"Validation failed: {err.Message}")));
        Assert.Contains("Must be 18 or older", exception.Message);
    }

    [Fact]
    public void GetValueOrThrow_WithComplexType()
    {
        // Arrange
        var user = new User { Id = 1, Name = "John" };
        var result = Result.Success(user);

        // Act
        var retrievedUser = result.GetValueOrThrow();

        // Assert
        Assert.Same(user, retrievedUser);
        Assert.Equal("John", retrievedUser.Name);
    }

    private class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
    }
}
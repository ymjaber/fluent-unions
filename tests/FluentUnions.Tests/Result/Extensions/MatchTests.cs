namespace FluentUnions.Tests.ResultTests.Extensions;

public class MatchTests
{
    [Fact]
    public void Match_WhenSuccess_ExecutesSuccessFunction()
    {
        // Arrange
        var result = Result.Success(42);

        // Act
        var output = result.Match(
            success: value => $"Success: {value}",
            failure: error => $"Failure: {error.Message}"
        );

        // Assert
        Assert.Equal("Success: 42", output);
    }

    [Fact]
    public void Match_WhenFailure_ExecutesFailureFunction()
    {
        // Arrange
        var error = new Error("E001", "Something went wrong");
        var result = Result.Failure<int>(error);

        // Act
        var output = result.Match(
            success: value => $"Success: {value}",
            failure: err => $"Failure: {err.Message}"
        );

        // Assert
        Assert.Equal("Failure: Something went wrong", output);
    }

    [Fact]
    public void Match_DifferentReturnType()
    {
        // Arrange
        var result = Result.Success(42);

        // Act
        var isEven = result.Match(
            success: value => value % 2 == 0,
            failure: _ => false
        );

        // Assert
        Assert.True(isEven);
    }

    [Fact]
    public void Match_WithSideEffects()
    {
        // Arrange
        var result = Result.Success(42);
        var successCalled = false;
        var failureCalled = false;

        // Act
        result.Match(
            success: value =>
            {
                successCalled = true;
                return value;
            },
            failure: error =>
            {
                failureCalled = true;
                return 0;
            }
        );

        // Assert
        Assert.True(successCalled);
        Assert.False(failureCalled);
    }

    [Fact]
    public void Match_Failure_WithSideEffects()
    {
        // Arrange
        var result = Result.Failure<int>(new Error("E001", "Error"));
        var successCalled = false;
        var failureCalled = false;

        // Act
        result.Match(
            success: value =>
            {
                successCalled = true;
                return value;
            },
            failure: error =>
            {
                failureCalled = true;
                return 0;
            }
        );

        // Assert
        Assert.False(successCalled);
        Assert.True(failureCalled);
    }

    [Fact]
    public void Match_UnitResult_Success()
    {
        // Arrange
        var result = Result.Success();

        // Act
        var output = result.Match(
            success: () => "Success",
            failure: error => $"Failure: {error.Code}"
        );

        // Assert
        Assert.Equal("Success", output);
    }

    [Fact]
    public void Match_UnitResult_Failure()
    {
        // Arrange
        var error = new Error("E001", "Error");
        var result = Result.Failure(error);

        // Act
        var output = result.Match(
            success: () => "Success",
            failure: err => $"Failure: {err.Code}"
        );

        // Assert
        Assert.Equal("Failure: E001", output);
    }

    [Fact]
    public void Match_InLinqExpression()
    {
        // Arrange
        var results = new[]
        {
            Result.Success(1),
            Result.Failure<int>(new Error("E001", "Error")),
            Result.Success(3),
            Result.Failure<int>(new Error("E002", "Error")),
            Result.Success(5)
        };

        // Act
        var sum = results.Sum(r => r.Match(
            success: value => value,
            failure: _ => 0
        ));

        // Assert
        Assert.Equal(9, sum); // 1 + 0 + 3 + 0 + 5
    }

    [Fact]
    public void Match_WithComplexType()
    {
        // Arrange
        var user = new User { Id = 1, Name = "John", IsActive = true };
        var result = Result.Success(user);

        // Act
        var description = result.Match(
            success: u => u.IsActive ? $"Active user: {u.Name}" : $"Inactive user: {u.Name}",
            failure: _ => "User not found"
        );

        // Assert
        Assert.Equal("Active user: John", description);
    }

    [Fact]
    public void Match_ReturnsResult()
    {
        // Arrange
        var result = Result.Success(10);

        // Act
        var newResult = result.Match(
            success: value => value > 5
                ? Result.Success(value * 2)
                : Result.Failure<int>(new Error("TOO_SMALL", "Value too small")),
            failure: error => Result.Failure<int>(error)
        );

        // Assert
        Assert.True(newResult.IsSuccess);
        Assert.Equal(20, newResult.Value);
    }

    [Fact]
    public void Match_HandlesDifferentErrorTypes()
    {
        // Arrange
        var validationError = new ValidationError("email", "Invalid format");
        var result = Result.Failure<string>(validationError);

        // Act
        var message = result.Match(
            success: value => $"Email: {value}",
            failure: error => error switch
            {
                ValidationError ve => $"Validation failed: {ve.Message}",
                NotFoundError nfe => $"Not found: {nfe.Message}",
                _ => $"Error: {error.Message}"
            }
        );

        // Assert
        Assert.Equal("Validation failed: Invalid format", message);
    }

    private class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public bool IsActive { get; set; }
    }
}
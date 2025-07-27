namespace FluentUnions.AwesomeAssertions.Tests.ResultTests;

public class UnitResultAssertionsTests
{
    #region Succeed Tests

    [Fact]
    public void Succeed_WhenResultIsSuccess_ShouldPass()
    {
        // Arrange
        var result = Result.Success();

        // Act & Assert
        result.Should().Succeed();
    }

    [Fact]
    public void Succeed_WhenResultIsFailure_ShouldFail()
    {
        // Arrange
        var result = Result.Failure(new Error("Failed"));

        // Act & Assert
        var act = () => result.Should().Succeed();
        
        act.Should().Throw<XunitException>()
            .WithMessage("Expected Result to succeed, but it failed.");
    }

    #endregion

    #region Fail Tests

    [Fact]
    public void Fail_WhenResultIsFailure_ShouldPass()
    {
        // Arrange
        var result = Result.Failure(new Error("Failed"));

        // Act & Assert
        result.Should().Fail();
    }

    [Fact]
    public void Fail_WhenResultIsSuccess_ShouldFail()
    {
        // Arrange
        var result = Result.Success();

        // Act & Assert
        var act = () => result.Should().Fail();
        
        act.Should().Throw<XunitException>()
            .WithMessage("Expected Result to fail, but it succeeded.");
    }

    [Fact]
    public void Fail_ShouldReturnFailedResultAssertions()
    {
        // Arrange
        var result = Result.Failure(new ValidationError("VAL001", "Invalid input"));

        // Act & Assert
        result.Should().Fail()
            .WithErrorType<ValidationError>()
            .WithErrorCode("VAL001")
            .WithErrorMessage("Invalid input");
    }

    #endregion

    #region FailWith Tests

    [Fact]
    public void FailWith_ExactError_WhenErrorMatches_ShouldPass()
    {
        // Arrange
        var expectedError = new ValidationError("Input is required");
        var result = Result.Failure(expectedError);

        // Act & Assert
        result.Should().FailWith(expectedError);
    }

    [Fact]
    public void FailWith_ExactError_WhenErrorDoesNotMatch_ShouldFail()
    {
        // Arrange
        var expectedError = new ValidationError("Input is required");
        var actualError = new ValidationError("Input is invalid");
        var result = Result.Failure(actualError);

        // Act & Assert
        var act = () => result.Should().FailWith(expectedError);
        
        act.Should().Throw<XunitException>();
    }

    [Fact]
    public void FailWith_ErrorType_WhenTypeMatches_ShouldPass()
    {
        // Arrange
        var result = Result.Failure(new AuthenticationError("Invalid credentials"));

        // Act & Assert
        result.Should().FailWith<AuthenticationError>()
            .WithErrorMessage("Invalid credentials");
    }

    [Fact]
    public void FailWith_ErrorType_WhenTypeDoesNotMatch_ShouldFail()
    {
        // Arrange
        var result = Result.Failure(new ValidationError("Invalid"));

        // Act & Assert
        var act = () => result.Should().FailWith<AuthenticationError>();
        
        act.Should().Throw<XunitException>()
            .WithMessage("*Expected*AuthenticationError*ValidationError*");
    }

    [Fact]
    public void FailWith_ErrorPredicate_WhenPredicateMatches_ShouldPass()
    {
        // Arrange
        var result = Result.Failure(new Error("TIMEOUT", "Operation timed out after 30 seconds"));

        // Act & Assert
        result.Should().FailWith(error => 
            error.Code == "TIMEOUT" && 
            error.Message.Contains("timed out"));
    }

    [Fact]
    public void FailWith_ErrorPredicate_WhenPredicateDoesNotMatch_ShouldFail()
    {
        // Arrange
        var result = Result.Failure(new Error("TIMEOUT", "Operation timed out"));

        // Act & Assert
        var act = () => result.Should().FailWith(error => error.Code == "ERROR");
        
        act.Should().Throw<XunitException>()
            .WithMessage("Expected Result error to match the predicate, but it did not.");
    }

    [Fact]
    public void FailWith_TypedErrorPredicate_WhenPredicateMatches_ShouldPass()
    {
        // Arrange
        var result = Result.Failure(new AuthorizationError("FORBIDDEN", "Insufficient permissions for admin panel"));

        // Act & Assert
        result.Should().FailWith<AuthorizationError>(error => 
            error.Code == "FORBIDDEN" && 
            error.Message.Contains("Insufficient permissions"));
    }

    [Fact]
    public void FailWith_TypedErrorPredicate_WhenPredicateDoesNotMatch_ShouldFail()
    {
        // Arrange
        var result = Result.Failure(new AuthorizationError("FORBIDDEN", "Access denied"));

        // Act & Assert
        var act = () => result.Should().FailWith<AuthorizationError>(error => error.Code == "UNAUTHORIZED");
        
        act.Should().Throw<XunitException>()
            .WithMessage("Expected Result error to match the predicate, but it did not.");
    }

    #endregion
}
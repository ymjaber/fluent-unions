namespace FluentUnions.AwesomeAssertions.Tests.ResultTests;

public class FailedResultAssertionsTests
{
    #region WithErrorType Tests

    [Fact]
    public void WithErrorType_WhenTypeMatches_ShouldPass()
    {
        // Arrange
        var result = Failure(new ValidationError("Invalid"));

        // Act & Assert
        result.Should().Fail()
            .WithErrorType<ValidationError>();
    }

    [Fact]
    public void WithErrorType_WhenTypeDoesNotMatch_ShouldFail()
    {
        // Arrange
        var result = Failure(new ValidationError("Invalid"));

        // Act & Assert
        var act = () => result.Should().Fail()
            .WithErrorType<NotFoundError>();
        
        act.Should().Throw<XunitException>()
            .WithMessage("*Expected*NotFoundError*ValidationError*");
    }

    #endregion

    #region WithErrorCode Tests

    [Fact]
    public void WithErrorCode_WhenCodeMatches_ShouldPass()
    {
        // Arrange
        var result = Failure(new Error("ERR_NETWORK", "Network timeout"));

        // Act & Assert
        result.Should().Fail()
            .WithErrorCode("ERR_NETWORK");
    }

    [Fact]
    public void WithErrorCode_WhenCodeDoesNotMatch_ShouldFail()
    {
        // Arrange
        var result = Failure(new Error("ERR_NETWORK", "Network timeout"));

        // Act & Assert
        var act = () => result.Should().Fail()
            .WithErrorCode("ERR_DATABASE");
        
        act.Should().Throw<XunitException>()
            .WithMessage("*Expected*ERR_DATABASE*ERR_NETWORK*");
    }

    #endregion

    #region WithErrorMessage Tests

    [Fact]
    public void WithErrorMessage_WhenMessageMatches_ShouldPass()
    {
        // Arrange
        var result = Failure(new Error("Operation failed"));

        // Act & Assert
        result.Should().Fail()
            .WithErrorMessage("Operation failed");
    }

    [Fact]
    public void WithErrorMessage_WhenMessageDoesNotMatch_ShouldFail()
    {
        // Arrange
        var result = Failure(new Error("Operation failed"));

        // Act & Assert
        var act = () => result.Should().Fail()
            .WithErrorMessage("Operation succeeded");
        
        act.Should().Throw<XunitException>()
            .WithMessage("*Expected*Operation succeeded*Operation failed*");
    }

    #endregion

    #region WithError Tests

    [Fact]
    public void WithError_WhenErrorMatches_ShouldPass()
    {
        // Arrange
        var expectedError = new ValidationError("VAL001", "Invalid email format");
        var result = Failure(expectedError);

        // Act & Assert
        result.Should().Fail()
            .WithError(expectedError);
    }

    [Fact]
    public void WithError_WhenErrorDoesNotMatch_ShouldFail()
    {
        // Arrange
        var expectedError = new ValidationError("VAL001", "Invalid email format");
        var actualError = new ValidationError("VAL002", "Invalid password");
        var result = Failure(actualError);

        // Act & Assert
        var act = () => result.Should().Fail()
            .WithError(expectedError);
        
        act.Should().Throw<XunitException>();
    }

    #endregion

    #region WithErrorMessageContaining Tests

    [Fact]
    public void WithErrorMessageContaining_WhenMessageContainsSubstring_ShouldPass()
    {
        // Arrange
        var result = Failure(new Error("Connection to database failed"));

        // Act & Assert
        result.Should().Fail()
            .WithErrorMessageContaining("database");
    }

    [Fact]
    public void WithErrorMessageContaining_WhenMessageDoesNotContainSubstring_ShouldFail()
    {
        // Arrange
        var result = Failure(new Error("Connection to server failed"));

        // Act & Assert
        var act = () => result.Should().Fail()
            .WithErrorMessageContaining("database");
        
        act.Should().Throw<XunitException>()
            .WithMessage("*Expected*contain*database*");
    }

    #endregion

    #region WithErrorMessageStartingWith Tests

    [Fact]
    public void WithErrorMessageStartingWith_WhenMessageStartsWithPrefix_ShouldPass()
    {
        // Arrange
        var result = Failure(new Error("Validation failed: Invalid email"));

        // Act & Assert
        result.Should().Fail()
            .WithErrorMessageStartingWith("Validation failed");
    }

    [Fact]
    public void WithErrorMessageStartingWith_WhenMessageDoesNotStartWithPrefix_ShouldFail()
    {
        // Arrange
        var result = Failure(new Error("Error: Validation failed"));

        // Act & Assert
        var act = () => result.Should().Fail()
            .WithErrorMessageStartingWith("Validation failed");
        
        act.Should().Throw<XunitException>()
            .WithMessage("*Expected*start with*Validation failed*");
    }

    #endregion

    #region WithErrorMessageEndingWith Tests

    [Fact]
    public void WithErrorMessageEndingWith_WhenMessageEndsWithSuffix_ShouldPass()
    {
        // Arrange
        var result = Failure(new Error("Operation timed out after 30 seconds"));

        // Act & Assert
        result.Should().Fail()
            .WithErrorMessageEndingWith("30 seconds");
    }

    [Fact]
    public void WithErrorMessageEndingWith_WhenMessageDoesNotEndWithSuffix_ShouldFail()
    {
        // Arrange
        var result = Failure(new Error("Operation timed out after 30 seconds"));

        // Act & Assert
        var act = () => result.Should().Fail()
            .WithErrorMessageEndingWith("60 seconds");
        
        act.Should().Throw<XunitException>()
            .WithMessage("*Expected*end with*60 seconds*");
    }

    #endregion

    #region WithErrorMessageMatching Tests

    [Fact]
    public void WithErrorMessageMatching_WhenPredicateMatches_ShouldPass()
    {
        // Arrange
        var result = Failure(new Error("Failed after 3 retries"));

        // Act & Assert
        result.Should().Fail()
            .WithErrorMessageMatching(msg => msg.Contains("retries") && msg.Any(char.IsDigit));
    }

    [Fact]
    public void WithErrorMessageMatching_WhenPredicateDoesNotMatch_ShouldFail()
    {
        // Arrange
        var result = Failure(new Error("Failed immediately"));

        // Act & Assert
        var act = () => result.Should().Fail()
            .WithErrorMessageMatching(msg => msg.Contains("retries"));
        
        act.Should().Throw<XunitException>()
            .WithMessage("*Expected*match the predicate*");
    }

    #endregion

    #region AggregateError Tests

    [Fact]
    public void WithAggregateError_WhenErrorIsAggregateError_ShouldPass()
    {
        // Arrange
        var errors = new[] { new Error("Error1"), new Error("Error2") };
        var builder = new ErrorBuilder();
        foreach (var error in errors)
            builder.Append(error);
        var aggregateError = builder.Build();
        var result = Failure(aggregateError);

        // Act & Assert
        result.Should().Fail()
            .WithAggregateError()
            .Which.Errors.Should().HaveCount(2);
    }

    [Fact]
    public void WithAggregateError_WhenErrorIsNotAggregateError_ShouldFail()
    {
        // Arrange
        var result = Result.Failure(new ValidationError("Single error"));

        // Act & Assert
        var act = () => result.Should().Fail()
            .WithAggregateError();
        
        act.Should().Throw<XunitException>()
            .WithMessage("*Expected*AggregateError*ValidationError*");
    }

    [Fact]
    public void WithAggregateErrorCount_WhenCountMatches_ShouldPass()
    {
        // Arrange
        var errors = new[] { new Error("Error1"), new Error("Error2"), new Error("Error3") };
        var builder = new ErrorBuilder();
        foreach (var error in errors)
            builder.Append(error);
        var aggregateError = builder.Build();
        var result = Failure(aggregateError);

        // Act & Assert
        result.Should().Fail()
            .WithAggregateErrorCount(3);
    }

    [Fact]
    public void WithAggregateErrorCount_WhenCountDoesNotMatch_ShouldFail()
    {
        // Arrange
        var errors = new[] { new Error("Error1"), new Error("Error2") };
        var builder = new ErrorBuilder();
        foreach (var error in errors)
            builder.Append(error);
        var aggregateError = builder.Build();
        var result = Failure(aggregateError);

        // Act & Assert
        var act = () => result.Should().Fail()
            .WithAggregateErrorCount(3);
        
        act.Should().Throw<XunitException>()
            .WithMessage("*Expected*3*error(s)*found*2*");
    }

    [Fact]
    public void WithAggregateErrorContaining_ErrorType_WhenContainsType_ShouldPass()
    {
        // Arrange
        var errors = new Error[] { new ValidationError("Error1"), new NotFoundError("Error2") };
        var builder = new ErrorBuilder();
        foreach (var error in errors)
            builder.Append(error);
        var aggregateError = builder.Build();
        var result = Failure(aggregateError);

        // Act & Assert
        result.Should().Fail()
            .WithAggregateErrorContaining<ValidationError>();
    }

    [Fact]
    public void WithAggregateErrorContaining_ErrorType_WhenDoesNotContainType_ShouldFail()
    {
        // Arrange
        var errors = new Error[] { new ValidationError("Error1"), new ValidationError("Error2") };
        var builder = new ErrorBuilder();
        foreach (var error in errors)
            builder.Append(error);
        var aggregateError = builder.Build();
        var result = Failure(aggregateError);

        // Act & Assert
        var act = () => result.Should().Fail()
            .WithAggregateErrorContaining<NotFoundError>();
        
        act.Should().Throw<XunitException>()
            .WithMessage("*Expected*contain*NotFoundError*");
    }

    [Fact]
    public void WithAggregateErrorContaining_SpecificError_WhenContainsError_ShouldPass()
    {
        // Arrange
        var specificError = new ValidationError("Email is required");
        var errors = new Error[] { specificError, new ValidationError("Name is required") };
        var builder = new ErrorBuilder();
        foreach (var error in errors)
            builder.Append(error);
        var aggregateError = builder.Build();
        var result = Failure(aggregateError);

        // Act & Assert
        result.Should().Fail()
            .WithAggregateErrorContaining(specificError);
    }

    [Fact]
    public void WithAggregateErrorContainingCode_WhenContainsCode_ShouldPass()
    {
        // Arrange
        var errors = new Error[] 
        { 
            new Error("VAL001", "Validation error"), 
            new Error("ERR002", "Another error") 
        };
        var builder = new ErrorBuilder();
        foreach (var error in errors)
            builder.Append(error);
        var aggregateError = builder.Build();
        var result = Failure(aggregateError);

        // Act & Assert
        result.Should().Fail()
            .WithAggregateErrorContainingCode("VAL001");
    }

    [Fact]
    public void WithAggregateErrorContainingMessage_WhenContainsMessage_ShouldPass()
    {
        // Arrange
        var errors = new Error[] 
        { 
            new ValidationError("Email is required"), 
            new ValidationError("Password is too short") 
        };
        var builder = new ErrorBuilder();
        foreach (var error in errors)
            builder.Append(error);
        var aggregateError = builder.Build();
        var result = Failure(aggregateError);

        // Act & Assert
        result.Should().Fail()
            .WithAggregateErrorContainingMessage("Email is required");
    }

    [Fact]
    public void WithAggregateErrorMatching_WhenContainsMatchingError_ShouldPass()
    {
        // Arrange
        var errors = new Error[] 
        { 
            new Error("TIMEOUT", "Database connection timeout"), 
            new Error("ERROR", "Generic error") 
        };
        var builder = new ErrorBuilder();
        foreach (var error in errors)
            builder.Append(error);
        var aggregateError = builder.Build();
        var result = Failure(aggregateError);

        // Act & Assert
        result.Should().Fail()
            .WithAggregateErrorMatching(error => 
                error.Code == "TIMEOUT" && error.Message.Contains("Database"));
    }

    [Fact]
    public void WithAggregateErrorMatching_Typed_WhenContainsMatchingError_ShouldPass()
    {
        // Arrange
        var errors = new Error[] 
        { 
            new ValidationError("INVALID_EMAIL", "Email format is invalid"), 
            new NotFoundError("User not found") 
        };
        var builder = new ErrorBuilder();
        foreach (var error in errors)
            builder.Append(error);
        var aggregateError = builder.Build();
        var result = Failure(aggregateError);

        // Act & Assert
        result.Should().Fail()
            .WithAggregateErrorMatching<ValidationError>(error => 
                error.Code == "INVALID_EMAIL" && error.Message.StartsWith("Email"));
    }

    #endregion

    #region Chaining Tests

    [Fact]
    public void MultipleAssertions_CanBeChained()
    {
        // Arrange
        var result = Failure(new ValidationError("VAL001", "Invalid input"));

        // Act & Assert
        result.Should().Fail()
            .WithErrorType<ValidationError>()
            .WithErrorCode("VAL001")
            .WithErrorMessage("Invalid input")
            .WithErrorMessageContaining("Invalid")
            .WithErrorMessageStartingWith("Invalid")
            .WithErrorMessageEndingWith("input");
    }

    #endregion
}
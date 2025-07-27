namespace FluentUnions.AwesomeAssertions.Tests.ResultTests;

public class ValueResultAssertionsTests
{
    #region Succeed Tests

    [Fact]
    public void Succeed_WhenResultIsSuccess_ShouldPass()
    {
        // Arrange
        var result = Result.Success(42);

        // Act & Assert
        result.Should().Succeed();
    }

    [Fact]
    public void Succeed_WhenResultIsFailure_ShouldFail()
    {
        // Arrange
        var result = Result.Failure<int>(new Error("Failed"));

        // Act & Assert
        var act = () => result.Should().Succeed();
        
        act.Should().Throw<XunitException>()
            .WithMessage("Expected Result to succeed, but it failed.");
    }

    [Fact]
    public void Succeed_ShouldReturnAndWhichConstraint()
    {
        // Arrange
        var result = Result.Success(42);

        // Act & Assert
        result.Should().Succeed()
            .Which.Should().Be(42);
    }

    #endregion

    #region SucceedWithValue Tests

    [Fact]
    public void SucceedWithValue_WhenValueMatches_ShouldPass()
    {
        // Arrange
        var result = Result.Success("Hello");

        // Act & Assert
        result.Should().SucceedWithValue("Hello");
    }

    [Fact]
    public void SucceedWithValue_WhenValueDoesNotMatch_ShouldFail()
    {
        // Arrange
        var result = Result.Success("Hello");

        // Act & Assert
        var act = () => result.Should().SucceedWithValue("World");
        
        act.Should().Throw<XunitException>()
            .WithMessage("*Expected*World*but*Hello*");
    }

    [Fact]
    public void SucceedWithValue_WhenResultIsFailure_ShouldFail()
    {
        // Arrange
        var result = Result.Failure<string>(new Error("Failed"));

        // Act & Assert
        var act = () => result.Should().SucceedWithValue("Hello");
        
        act.Should().Throw<XunitException>()
            .WithMessage("Expected Result to succeed, but it failed.");
    }

    #endregion

    #region SucceedMatching Tests

    [Fact]
    public void SucceedMatching_WhenPredicateMatches_ShouldPass()
    {
        // Arrange
        var result = Result.Success(42);

        // Act & Assert
        result.Should().SucceedMatching(x => x > 40 && x < 50);
    }

    [Fact]
    public void SucceedMatching_WhenPredicateDoesNotMatch_ShouldFail()
    {
        // Arrange
        var result = Result.Success(42);

        // Act & Assert
        var act = () => result.Should().SucceedMatching(x => x < 40);
        
        act.Should().Throw<XunitException>()
            .WithMessage("Expected Result value to match the predicate, but it did not.");
    }

    #endregion

    #region SucceedSatisfying Tests

    [Fact]
    public void SucceedSatisfying_WhenAssertionsPass_ShouldPass()
    {
        // Arrange
        var result = Result.Success(new Product { Name = "Widget", Price = 9.99m });

        // Act & Assert
        result.Should().SucceedSatisfying(product =>
        {
            product.Name.Should().Be("Widget");
            product.Price.Should().Be(9.99m);
        });
    }

    [Fact]
    public void SucceedSatisfying_WhenAssertionsFail_ShouldFail()
    {
        // Arrange
        var result = Result.Success(new Product { Name = "Widget", Price = 9.99m });

        // Act & Assert
        var act = () => result.Should().SucceedSatisfying(product =>
        {
            product.Name.Should().Be("Gadget");
        });
        
        act.Should().Throw<XunitException>();
    }

    #endregion

    #region Fail Tests

    [Fact]
    public void Fail_WhenResultIsFailure_ShouldPass()
    {
        // Arrange
        var result = Result.Failure<int>(new Error("Failed"));

        // Act & Assert
        result.Should().Fail();
    }

    [Fact]
    public void Fail_WhenResultIsSuccess_ShouldFail()
    {
        // Arrange
        var result = Result.Success(42);

        // Act & Assert
        var act = () => result.Should().Fail();
        
        act.Should().Throw<XunitException>()
            .WithMessage("Expected Result to fail, but it succeeded.");
    }

    [Fact]
    public void Fail_ShouldReturnFailedResultAssertions()
    {
        // Arrange
        var result = Result.Failure<int>(new ValidationError("Invalid"));

        // Act & Assert
        result.Should().Fail()
            .WithErrorType<ValidationError>()
            .WithErrorMessage("Invalid");
    }

    #endregion

    #region FailWith Tests

    [Fact]
    public void FailWith_ExactError_WhenErrorMatches_ShouldPass()
    {
        // Arrange
        var expectedError = new ValidationError("Email is required");
        var result = Result.Failure<string>(expectedError);

        // Act & Assert
        result.Should().FailWith(expectedError);
    }

    [Fact]
    public void FailWith_ExactError_WhenErrorDoesNotMatch_ShouldFail()
    {
        // Arrange
        var expectedError = new ValidationError("Email is required");
        var actualError = new ValidationError("Name is required");
        var result = Result.Failure<string>(actualError);

        // Act & Assert
        var act = () => result.Should().FailWith(expectedError);
        
        act.Should().Throw<XunitException>();
    }

    [Fact]
    public void FailWith_ErrorType_WhenTypeMatches_ShouldPass()
    {
        // Arrange
        var result = Result.Failure<string>(new NotFoundError("User not found"));

        // Act & Assert
        result.Should().FailWith<NotFoundError>()
            .WithErrorMessage("User not found");
    }

    [Fact]
    public void FailWith_ErrorType_WhenTypeDoesNotMatch_ShouldFail()
    {
        // Arrange
        var result = Result.Failure<string>(new ValidationError("Invalid"));

        // Act & Assert
        var act = () => result.Should().FailWith<NotFoundError>();
        
        act.Should().Throw<XunitException>()
            .WithMessage("*Expected*NotFoundError*ValidationError*");
    }

    [Fact]
    public void FailWith_ErrorPredicate_WhenPredicateMatches_ShouldPass()
    {
        // Arrange
        var result = Result.Failure<string>(new Error("USER_404", "User not found"));

        // Act & Assert
        result.Should().FailWith(error => 
            error.Code == "USER_404" && 
            error.Message.Contains("not found"));
    }

    [Fact]
    public void FailWith_ErrorPredicate_WhenPredicateDoesNotMatch_ShouldFail()
    {
        // Arrange
        var result = Result.Failure<string>(new Error("USER_404", "User not found"));

        // Act & Assert
        var act = () => result.Should().FailWith(error => error.Code == "USER_500");
        
        act.Should().Throw<XunitException>()
            .WithMessage("Expected Result error to match the predicate, but it did not.");
    }

    [Fact]
    public void FailWith_TypedErrorPredicate_WhenPredicateMatches_ShouldPass()
    {
        // Arrange
        var result = Result.Failure<string>(new ValidationError("INVALID_EMAIL", "Email format is invalid"));

        // Act & Assert
        result.Should().FailWith<ValidationError>(error => 
            error.Code == "INVALID_EMAIL" && 
            error.Message.StartsWith("Email"));
    }

    [Fact]
    public void FailWith_TypedErrorPredicate_WhenPredicateDoesNotMatch_ShouldFail()
    {
        // Arrange
        var result = Result.Failure<string>(new ValidationError("INVALID_EMAIL", "Email format is invalid"));

        // Act & Assert
        var act = () => result.Should().FailWith<ValidationError>(error => error.Code == "INVALID_NAME");
        
        act.Should().Throw<XunitException>()
            .WithMessage("Expected Result error to match the predicate, but it did not.");
    }

    #endregion

    private class Product
    {
        public string Name { get; init; } = string.Empty;
        public decimal Price { get; init; }
    }
}
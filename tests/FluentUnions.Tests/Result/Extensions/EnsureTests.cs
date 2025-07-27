namespace FluentUnions.Tests.ResultTests.Extensions;

public class EnsureTests
{
    [Fact]
    public void Ensure_WhenSuccess_AndPredicateTrue_ReturnsSuccess()
    {
        // Arrange
        var result = Result.Success(10);

        // Act
        var ensured = result.Ensure(
            x => x > 5,
            new Error("TOO_SMALL", "Value must be greater than 5"));

        // Assert
        Assert.True(ensured.IsSuccess);
        Assert.Equal(10, ensured.Value);
    }

    [Fact]
    public void Ensure_WhenSuccess_AndPredicateFalse_ReturnsFailure()
    {
        // Arrange
        var result = Result.Success(3);
        var error = new Error("TOO_SMALL", "Value must be greater than 5");

        // Act
        var ensured = result.Ensure(x => x > 5, error);

        // Assert
        Assert.True(ensured.IsFailure);
        Assert.Equal(error, ensured.Error);
    }

    [Fact]
    public void Ensure_WhenFailure_PropagatesError()
    {
        // Arrange
        var originalError = new Error("E001", "Original error");
        var result = Result.Failure<int>(originalError);

        // Act
        var ensured = result.Ensure(
            x => x > 5,
            new Error("TOO_SMALL", "Value must be greater than 5"));

        // Assert
        Assert.True(ensured.IsFailure);
        Assert.Equal(originalError, ensured.Error);
    }

    [Fact]
    public void Ensure_ChainedValidations_AllPass()
    {
        // Arrange
        var result = Result.Success(10);

        // Act
        var validated = result
            .Ensure(x => x > 0, new Error("NOT_POSITIVE", "Must be positive"))
            .Ensure(x => x < 100, new Error("TOO_LARGE", "Must be less than 100"))
            .Ensure(x => x % 2 == 0, new Error("NOT_EVEN", "Must be even"));

        // Assert
        Assert.True(validated.IsSuccess);
        Assert.Equal(10, validated.Value);
    }

    [Fact]
    public void Ensure_ChainedValidations_StopsAtFirstFailure()
    {
        // Arrange
        var result = Result.Success(101);

        // Act
        var validated = result
            .Ensure(x => x > 0, new Error("NOT_POSITIVE", "Must be positive"))
            .Ensure(x => x < 100, new Error("TOO_LARGE", "Must be less than 100"))
            .Ensure(x => x % 2 == 0, new Error("NOT_EVEN", "Must be even"));

        // Assert
        Assert.True(validated.IsFailure);
        Assert.Equal("TOO_LARGE", validated.Error.Code);
    }

    [Fact]
    public void Ensure_WithErrorFactory()
    {
        // Arrange
        var result = Result.Success("test@example");

        // Act
        var validated = result.Ensure
            .Satisfies(email => email.Contains("@") && email.Contains("."), 
                new ValidationError("email", "'test@example' is not a valid email"));

        // Assert
        Assert.True(validated.IsFailure);
        Assert.IsType<ValidationError>(validated.Error);
        Assert.Contains("'test@example' is not a valid email", validated.Error.Message);
    }

    [Fact]
    public void Ensure_UnitResult_Success()
    {
        // Arrange
        var result = Result.Success();
        var flag = true;

        // Act
        var ensured = result.Ensure(
            () => flag,
            new Error("FLAG_FALSE", "Flag must be true"));

        // Assert
        Assert.True(ensured.IsSuccess);
    }

    [Fact]
    public void Ensure_UnitResult_Failure()
    {
        // Arrange
        var result = Result.Success();
        var flag = false;
        var error = new Error("FLAG_FALSE", "Flag must be true");

        // Act
        var ensured = result.Ensure(() => flag, error);

        // Assert
        Assert.True(ensured.IsFailure);
        Assert.Equal(error, ensured.Error);
    }

    [Fact]
    public void EnsureAll_WithSingleCondition_Success()
    {
        // Arrange
        var result = Result.Success(10);

        // Act
        var validated = result.EnsureAll(10 > 0, new Error("NOT_POSITIVE", "Must be positive"));

        // Assert
        Assert.True(validated.IsSuccess);
        Assert.Equal(10, validated.Value);
    }

    [Fact]
    public void EnsureAll_WithSingleCondition_Failure()
    {
        // Arrange
        var result = Result.Success(15);

        // Act
        var validated = result.EnsureAll(15 % 2 == 0, new Error("NOT_EVEN", "Must be even"));

        // Assert
        Assert.True(validated.IsFailure);
        Assert.Equal("NOT_EVEN", validated.Error.Code);
    }

    [Fact]
    public void Ensure_ComplexValidation()
    {
        // Arrange
        var user = new User { Name = "John", Age = 25, Email = "john@example.com" };
        var result = Result.Success(user);

        // Act
        var validated = result
            .Ensure(u => !string.IsNullOrWhiteSpace(u.Name),
                new ValidationError("name", "Name is required"))
            .Ensure(u => u.Age >= 18,
                new ValidationError("age", "Must be 18 or older"))
            .Ensure(u => u.Email.Contains("@"),
                new ValidationError("email", "Invalid email format"));

        // Assert
        Assert.True(validated.IsSuccess);
    }

    [Fact]
    public void Ensure_PreDefinedValidations()
    {
        // Arrange
        var result = Result.Success("test");

        // Act
        var validated = result
            .Ensure(s => !string.IsNullOrEmpty(s),
                new ValidationError("value", "Cannot be empty"))
            .Ensure(s => s.Length >= 3,
                new ValidationError("value", "Too short"));

        // Assert
        Assert.True(validated.IsSuccess);
    }

    private class User
    {
        public string Name { get; set; } = "";
        public int Age { get; set; }
        public string Email { get; set; } = "";
    }
}
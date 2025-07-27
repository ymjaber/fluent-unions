namespace FluentUnions.Tests.ResultTests.Extensions;

public class BindTests
{
    [Fact]
    public void Bind_WhenSuccess_AppliesFunction()
    {
        // Arrange
        var result = Result.Success(5);

        // Act
        var bound = result.Bind(x => Result.Success(x * 2));

        // Assert
        Assert.True(bound.IsSuccess);
        Assert.Equal(10, bound.Value);
    }

    [Fact]
    public void Bind_WhenFailure_PropagatesError()
    {
        // Arrange
        var error = new Error("E001", "Original error");
        var result = Result.Failure<int>(error);

        // Act
        var bound = result.Bind(x => Result.Success(x * 2));

        // Assert
        Assert.True(bound.IsFailure);
        Assert.Equal(error, bound.Error);
    }

    [Fact]
    public void Bind_WhenFunctionReturnsFailure_ReturnsFailure()
    {
        // Arrange
        var result = Result.Success(5);
        var error = new Error("E002", "Bind error");

        // Act
        var bound = result.Bind(x => Result.Failure<int>(error));

        // Assert
        Assert.True(bound.IsFailure);
        Assert.Equal(error, bound.Error);
    }

    [Fact]
    public void Bind_ChainedOperations_AllSuccess()
    {
        // Arrange
        var result = Result.Success(10);

        // Act
        var final = result
            .Bind(x => x > 5 ? Result.Success(x) : Result.Failure<int>(new Error("E001", "Too small")))
            .Bind(x => Result.Success(x * 2))
            .Bind(x => Result.Success(x.ToString()));

        // Assert
        Assert.True(final.IsSuccess);
        Assert.Equal("20", final.Value);
    }

    [Fact]
    public void Bind_ChainedOperations_StopsAtFirstFailure()
    {
        // Arrange
        var result = Result.Success(3);

        // Act
        var final = result
            .Bind(x => x > 5 ? Result.Success(x) : Result.Failure<int>(new Error("E001", "Too small")))
            .Bind(x => Result.Success(x * 2)) // Not executed
            .Bind(x => Result.Success(x.ToString())); // Not executed

        // Assert
        Assert.True(final.IsFailure);
        Assert.Equal("E001", final.Error.Code);
    }

    [Fact]
    public void Bind_ChangesType()
    {
        // Arrange
        var result = Result.Success("42");

        // Act
        var bound = result.Bind(str =>
            int.TryParse(str, out var num)
                ? Result.Success(num)
                : Result.Failure<int>(new Error("PARSE_ERROR", "Invalid number")));

        // Assert
        Assert.True(bound.IsSuccess);
        Assert.Equal(42, bound.Value);
    }

    [Fact]
    public void Bind_ParseFailure()
    {
        // Arrange
        var result = Result.Success("not a number");

        // Act
        var bound = result.Bind(str =>
            int.TryParse(str, out var num)
                ? Result.Success(num)
                : Result.Failure<int>(new Error("PARSE_ERROR", "Invalid number")));

        // Assert
        Assert.True(bound.IsFailure);
        Assert.Equal("PARSE_ERROR", bound.Error.Code);
    }

    [Fact]
    public void Bind_SimulatesValidation()
    {
        // Arrange
        static Result<int> ValidatePositive(int value) =>
            value > 0
                ? Result.Success(value)
                : Result.Failure<int>(new ValidationError("value", "Must be positive"));

        static Result<int> ValidateEven(int value) =>
            value % 2 == 0
                ? Result.Success(value)
                : Result.Failure<int>(new ValidationError("value", "Must be even"));

        // Act
        var result = Result.Success(6)
            .Bind(ValidatePositive)
            .Bind(ValidateEven);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(6, result.Value);
    }

    [Fact]
    public void Bind_SimulatesValidation_Failure()
    {
        // Arrange
        static Result<int> ValidatePositive(int value) =>
            value > 0
                ? Result.Success(value)
                : Result.Failure<int>(new ValidationError("value", "Must be positive"));

        static Result<int> ValidateEven(int value) =>
            value % 2 == 0
                ? Result.Success(value)
                : Result.Failure<int>(new ValidationError("value", "Must be even"));

        // Act
        var result = Result.Success(5)
            .Bind(ValidatePositive)
            .Bind(ValidateEven);

        // Assert
        Assert.True(result.IsFailure);
        Assert.IsType<ValidationError>(result.Error);
        Assert.Contains("Must be even", result.Error.Message);
    }

    [Fact]
    public void Bind_UnitResult_Success()
    {
        // Arrange
        var result = Result.Success();

        // Act
        var bound = result.Bind(() => Result.Success());

        // Assert
        Assert.True(bound.IsSuccess);
    }

    [Fact]
    public void Bind_UnitResult_Failure()
    {
        // Arrange
        var error = new Error("E001", "Error");
        var result = Result.Failure(error);

        // Act
        var bound = result.Bind(() => Result.Success());

        // Assert
        Assert.True(bound.IsFailure);
        Assert.Equal(error, bound.Error);
    }

    [Fact]
    public void BindAll_WithUnitResult_Success()
    {
        // Arrange
        var result = Result.Success(42);
        var unitResult = Result.Success();

        // Act
        var combined = result.BindAll(unitResult);

        // Assert
        Assert.True(combined.IsSuccess);
        Assert.Equal(42, combined.Value);
    }

    [Fact]
    public void BindAll_WithUnitResult_Failure()
    {
        // Arrange
        var result = Result.Success(42);
        var error = new Error("E001", "Error");
        var unitResult = Result.Failure(error);

        // Act
        var combined = result.BindAll(unitResult);

        // Assert
        Assert.True(combined.IsFailure);
        Assert.Equal(error, combined.Error);
    }
}
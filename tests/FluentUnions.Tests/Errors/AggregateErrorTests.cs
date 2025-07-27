namespace FluentUnions.Tests.Errors
{

public class AggregateErrorTests
{
    [Fact]
    public void Constructor_SetsPropertiesCorrectly()
    {
        // Arrange
        var errors = new List<Error>
        {
            new Error("E001", "First error"),
            new Error("E002", "Second error"),
            new ValidationError("field", "Invalid value")
        };

        // Act
        var aggregateError = new AggregateError(errors);

        // Assert
        Assert.Equal("Errors.Aggregate", aggregateError.Code);
        Assert.Equal("Multiple errors occurred.", aggregateError.Message);
        Assert.Equal(3, aggregateError.Errors.Count);
        Assert.Same(errors[0], aggregateError.Errors[0]);
        Assert.Same(errors[1], aggregateError.Errors[1]);
        Assert.Same(errors[2], aggregateError.Errors[2]);
    }

    [Fact]
    public void Errors_ReturnsReadOnlyCollection()
    {
        // Arrange
        var errors = new List<Error> { new Error("E001", "Error") };
        var aggregateError = new AggregateError(errors);

        // Act & Assert
        Assert.IsAssignableFrom<IReadOnlyList<Error>>(aggregateError.Errors);
    }

    [Fact]
    public void Equals_SameErrors_ReturnsTrue()
    {
        // Arrange
        var error1 = new Error("E001", "Error 1");
        var error2 = new Error("E002", "Error 2");
        var aggregate1 = new AggregateError(new[] { error1, error2 });
        var aggregate2 = new AggregateError(new[] { error1, error2 });

        // Act & Assert
        Assert.True(aggregate1.Equals(aggregate2));
        Assert.True(aggregate1 == aggregate2);
        Assert.False(aggregate1 != aggregate2);
    }

    [Fact]
    public void Equals_DifferentErrorOrder_ReturnsFalse()
    {
        // Arrange
        var error1 = new Error("E001", "Error 1");
        var error2 = new Error("E002", "Error 2");
        var aggregate1 = new AggregateError(new[] { error1, error2 });
        var aggregate2 = new AggregateError(new[] { error2, error1 });

        // Act & Assert
        Assert.False(aggregate1.Equals(aggregate2));
    }

    [Fact]
    public void Equals_DifferentErrors_ReturnsFalse()
    {
        // Arrange
        var aggregate1 = new AggregateError(new[] { new Error("E001", "Error 1") });
        var aggregate2 = new AggregateError(new[] { new Error("E002", "Error 2") });

        // Act & Assert
        Assert.False(aggregate1.Equals(aggregate2));
        Assert.False(aggregate1 == aggregate2);
        Assert.True(aggregate1 != aggregate2);
    }

    [Fact]
    public void Equals_DifferentNumberOfErrors_ReturnsFalse()
    {
        // Arrange
        var error1 = new Error("E001", "Error 1");
        var error2 = new Error("E002", "Error 2");
        var aggregate1 = new AggregateError(new[] { error1 });
        var aggregate2 = new AggregateError(new[] { error1, error2 });

        // Act & Assert
        Assert.False(aggregate1.Equals(aggregate2));
    }

    [Fact]
    public void Equals_SameReference_ReturnsTrue()
    {
        // Arrange
        var aggregate = new AggregateError(new[] { new Error("E001", "Error") });

        // Act & Assert
        Assert.True(aggregate.Equals(aggregate));
    }

    [Fact]
    public void Equals_NotAggregateError_ReturnsFalse()
    {
        // Arrange
        var aggregate = new AggregateError(new[] { new Error("E001", "Error") });
        var regularError = new Error("E001", "Error");

        // Act & Assert
        Assert.False(aggregate.Equals(regularError));
    }

    [Fact]
    public void GetHashCode_SameReference_ReturnsSameHash()
    {
        // Arrange
        var error1 = new Error("E001", "Error 1");
        var error2 = new Error("E002", "Error 2");
        var errors = new[] { error1, error2 };
        var aggregate1 = new AggregateError(errors);
        var aggregate2 = new AggregateError(errors);

        // Act & Assert
        Assert.Equal(aggregate1.GetHashCode(), aggregate2.GetHashCode());
    }

    [Fact]
    public void ToString_FormatsCorrectly()
    {
        // Arrange
        var errors = new[]
        {
            new Error("E001", "First error"),
            new Error("E002", "Second error"),
            new ValidationError("email", "Invalid format")
        };
        var aggregate = new AggregateError(errors);

        // Act
        var result = aggregate.ToString();

        // Assert
        Assert.Contains("Errors.Aggregate", result);
        Assert.Contains("Multiple errors occurred.", result);
        Assert.Contains("Error: E001 - First error", result);
        Assert.Contains("Error: E002 - Second error", result);
        Assert.Contains("ValidationError: email - Invalid format", result);
    }

    [Fact]
    public void CanBeUsedInResult()
    {
        // Arrange
        var errors = new[]
        {
            new ValidationError("name", "Name is required"),
            new ValidationError("email", "Email is invalid")
        };
        var aggregateError = new AggregateError(errors);

        // Act
        var result = Result.Failure<User>(aggregateError);

        // Assert
        Assert.True(result.IsFailure);
        Assert.IsType<AggregateError>(result.Error);
        var returnedAggregate = (AggregateError)result.Error;
        Assert.Equal(2, returnedAggregate.Errors.Count);
    }

    [Fact]
    public void NestedAggregateErrors_WorkCorrectly()
    {
        // Arrange
        var innerErrors = new[]
        {
            new Error("E001", "Inner error 1"),
            new Error("E002", "Inner error 2")
        };
        var innerAggregate = new AggregateError(innerErrors);
        var outerErrors = new[]
        {
            new Error("E003", "Outer error"),
            innerAggregate
        };
        var outerAggregate = new AggregateError(outerErrors);

        // Act & Assert
        Assert.Equal(2, outerAggregate.Errors.Count);
        Assert.IsType<Error>(outerAggregate.Errors[0]);
        Assert.IsType<AggregateError>(outerAggregate.Errors[1]);
    }

    [Fact]
    public void EmptyErrors_StillWorks()
    {
        // Arrange & Act
        var aggregate = new AggregateError(new List<Error>());

        // Assert
        Assert.Equal("Errors.Aggregate", aggregate.Code);
        Assert.Equal("Multiple errors occurred.", aggregate.Message);
        Assert.Empty(aggregate.Errors);
    }

    [Fact]
    public void MixedErrorTypes_WorkCorrectly()
    {
        // Arrange
        var errors = new Error[]
        {
            new ValidationError("field1", "Validation failed"),
            new NotFoundError("NOT_FOUND", "User with ID '123' was not found"),
            new ConflictError("CONFLICT", "Resource with id '456' already exists"),
            new Error("CUSTOM", "Custom error")
        };

        // Act
        var aggregate = new AggregateError(errors);

        // Assert
        Assert.Equal(4, aggregate.Errors.Count);
        Assert.IsType<ValidationError>(aggregate.Errors[0]);
        Assert.IsType<NotFoundError>(aggregate.Errors[1]);
        Assert.IsType<ConflictError>(aggregate.Errors[2]);
        Assert.IsType<Error>(aggregate.Errors[3]);
    }

    private class User
    {
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
    }
}
}

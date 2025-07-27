namespace FluentUnions.Tests.Errors
{

public class ErrorBuilderTests
{
    [Fact]
    public void NewBuilder_HasNoErrors()
    {
        // Arrange & Act
        var builder = new ErrorBuilder();

        // Assert
        Assert.False(builder.HasErrors);
    }

    [Fact]
    public void Append_SingleError_HasErrors()
    {
        // Arrange
        var builder = new ErrorBuilder();
        var error = new Error("E001", "Test error");

        // Act
        builder.Append(error);

        // Assert
        Assert.True(builder.HasErrors);
    }

    [Fact]
    public void Append_ReturnsBuilder_ForChaining()
    {
        // Arrange
        var builder = new ErrorBuilder();
        var error1 = new Error("E001", "Error 1");
        var error2 = new Error("E002", "Error 2");

        // Act
        var result = builder
            .Append(error1)
            .Append(error2);

        // Assert
        Assert.Same(builder, result);
        Assert.True(builder.HasErrors);
    }

    [Fact]
    public void Append_AggregateError_FlattensList()
    {
        // Arrange
        var builder = new ErrorBuilder();
        var innerErrors = new[]
        {
            new Error("E001", "Error 1"),
            new Error("E002", "Error 2"),
            new Error("E003", "Error 3")
        };
        var aggregateError = new AggregateError(innerErrors);

        // Act
        builder.Append(aggregateError);
        var result = builder.Build();

        // Assert
        Assert.IsType<AggregateError>(result);
        var builtAggregate = (AggregateError)result;
        Assert.Equal(3, builtAggregate.Errors.Count);
        Assert.Equal("E001", builtAggregate.Errors[0].Code);
        Assert.Equal("E002", builtAggregate.Errors[1].Code);
        Assert.Equal("E003", builtAggregate.Errors[2].Code);
    }

    [Fact]
    public void AppendOnFailure_WithFailedResult_AppendsError()
    {
        // Arrange
        var builder = new ErrorBuilder();
        var error = new Error("E001", "Operation failed");
        var result = Result.Failure(error);

        // Act
        builder.AppendOnFailure(result);

        // Assert
        Assert.True(builder.HasErrors);
        var builtError = builder.Build();
        Assert.Same(error, builtError);
    }

    [Fact]
    public void AppendOnFailure_WithSuccessResult_DoesNotAppend()
    {
        // Arrange
        var builder = new ErrorBuilder();
        var result = Result.Success();

        // Act
        builder.AppendOnFailure(result);

        // Assert
        Assert.False(builder.HasErrors);
    }

    [Fact]
    public void AppendOnFailure_Generic_WithFailedResult_AppendsError()
    {
        // Arrange
        var builder = new ErrorBuilder();
        var error = new ValidationError("value", "Invalid value");
        var result = Result.Failure<int>(error);

        // Act
        builder.AppendOnFailure(result);

        // Assert
        Assert.True(builder.HasErrors);
        var builtError = builder.Build();
        Assert.Same(error, builtError);
    }

    [Fact]
    public void AppendOnFailure_Generic_WithSuccessResult_DoesNotAppend()
    {
        // Arrange
        var builder = new ErrorBuilder();
        var result = Result.Success(42);

        // Act
        builder.AppendOnFailure(result);

        // Assert
        Assert.False(builder.HasErrors);
    }

    [Fact]
    public void Build_NoErrors_ThrowsInvalidOperationException()
    {
        // Arrange
        var builder = new ErrorBuilder();

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => builder.Build());
        Assert.Equal("No errors to build.", exception.Message);
    }

    [Fact]
    public void Build_SingleError_ReturnsSingleError()
    {
        // Arrange
        var builder = new ErrorBuilder();
        var error = new Error("E001", "Single error");
        builder.Append(error);

        // Act
        var result = builder.Build();

        // Assert
        Assert.Same(error, result);
        Assert.IsNotType<AggregateError>(result);
    }

    [Fact]
    public void Build_MultipleErrors_ReturnsAggregateError()
    {
        // Arrange
        var builder = new ErrorBuilder();
        var error1 = new Error("E001", "Error 1");
        var error2 = new Error("E002", "Error 2");
        builder
            .Append(error1)
            .Append(error2);

        // Act
        var result = builder.Build();

        // Assert
        Assert.IsType<AggregateError>(result);
        var aggregate = (AggregateError)result;
        Assert.Equal(2, aggregate.Errors.Count);
        Assert.Same(error1, aggregate.Errors[0]);
        Assert.Same(error2, aggregate.Errors[1]);
    }

    [Fact]
    public void TryBuild_NoErrors_ReturnsFalse()
    {
        // Arrange
        var builder = new ErrorBuilder();

        // Act
        var success = builder.TryBuild(out var error);

        // Assert
        Assert.False(success);
        Assert.Null(error);
    }

    [Fact]
    public void TryBuild_WithErrors_ReturnsTrue()
    {
        // Arrange
        var builder = new ErrorBuilder();
        builder.Append(new Error("E001", "Error"));

        // Act
        var success = builder.TryBuild(out var error);

        // Assert
        Assert.True(success);
        Assert.NotNull(error);
        Assert.Equal("E001", error.Code);
    }

    [Fact]
    public void ComplexScenario_ValidatingMultipleFields()
    {
        // Arrange
        var builder = new ErrorBuilder();
        var nameResult = string.IsNullOrEmpty("") 
            ? Result.Failure<string>(new ValidationError("name", "Name is required"))
            : Result.Success("");
        var emailResult = !IsValidEmail("invalid-email")
            ? Result.Failure<string>(new ValidationError("email", "Invalid email format"))
            : Result.Success("invalid-email");
        var ageResult = -5 < 0
            ? Result.Failure<int>(new ValidationError("age", "Age must be positive"))
            : Result.Success(-5);

        // Act
        builder
            .AppendOnFailure(nameResult)
            .AppendOnFailure(emailResult)
            .AppendOnFailure(ageResult);

        // Assert
        Assert.True(builder.HasErrors);
        var error = builder.Build();
        Assert.IsType<AggregateError>(error);
        var aggregate = (AggregateError)error;
        Assert.Equal(3, aggregate.Errors.Count);
        Assert.All(aggregate.Errors, e => Assert.IsType<ValidationError>(e));
    }

    [Fact]
    public void MixedScenario_DifferentErrorTypes()
    {
        // Arrange
        var builder = new ErrorBuilder();
        
        // Simulate various operations
        var validationError = new ValidationError("field", "Invalid value");
        var notFoundError = new NotFoundError("NOT_FOUND", "Resource with ID '123' was not found");
        var conflictError = new ConflictError("CONFLICT", "User with email 'test@example.com' already exists");
        var customError = new Error("CUSTOM", "Custom error");

        // Act
        builder
            .Append(validationError)
            .Append(notFoundError)
            .Append(conflictError)
            .Append(customError);

        // Assert
        var result = builder.Build();
        Assert.IsType<AggregateError>(result);
        var aggregate = (AggregateError)result;
        Assert.Equal(4, aggregate.Errors.Count);
        Assert.Contains(aggregate.Errors, e => e is ValidationError);
        Assert.Contains(aggregate.Errors, e => e is NotFoundError);
        Assert.Contains(aggregate.Errors, e => e is ConflictError);
        Assert.Contains(aggregate.Errors, e => e.Code == "CUSTOM");
    }

    [Fact]
    public void ChainedOperations_BuildComplexError()
    {
        // Arrange
        var builder = new ErrorBuilder();
        var userNotFound = Result.Failure<User>(new NotFoundError("NOT_FOUND", "User with ID '123' was not found"));
        var validationErrors = new AggregateError(new[]
        {
            new ValidationError("password", "Too short"),
            new ValidationError("confirmPassword", "Does not match")
        });

        // Act
        var finalError = builder
            .AppendOnFailure(userNotFound)
            .Append(validationErrors)
            .Append(new Error("SYSTEM", "Unexpected error"))
            .Build();

        // Assert
        Assert.IsType<AggregateError>(finalError);
        var aggregate = (AggregateError)finalError;
        Assert.Equal(4, aggregate.Errors.Count); // 1 NotFound + 2 Validation + 1 System
    }

    private static bool IsValidEmail(string email)
    {
        return email.Contains('@') && email.Contains('.');
    }

    private class User
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
    }
}
}

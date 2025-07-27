namespace FluentUnions.Tests.Errors
{

public class ValidationErrorTests
{
    [Fact]
    public void Constructor_WithCodeAndMessage_SetsPropertiesCorrectly()
    {
        // Arrange & Act
        var error = new ValidationError("VALIDATION_ERROR", "Invalid email format");

        // Assert
        Assert.Equal("VALIDATION_ERROR", error.Code);
        Assert.Equal("Invalid email format", error.Message);
        Assert.Empty(error.Metadata);
    }

    [Fact]
    public void Constructor_WithCodeMessageAndMetadata_SetsAllProperties()
    {
        // Arrange
        var metadata = new Dictionary<string, object>
        {
            { "field", "email" },
            { "value", "invalid-email" }
        };

        // Act
        var error = new ValidationError("VALIDATION_ERROR", "Invalid email format", metadata);

        // Assert
        Assert.Equal("VALIDATION_ERROR", error.Code);
        Assert.Equal("Invalid email format", error.Message);
        Assert.Equal(2, error.Metadata.Count);
        Assert.Equal("email", error.Metadata["field"]);
        Assert.Equal("invalid-email", error.Metadata["value"]);
    }

    [Fact]
    public void Equals_SameCodeAndMessage_ReturnsTrue()
    {
        // Arrange
        var error1 = new ValidationError("VALIDATION_ERROR", "Invalid format");
        var error2 = new ValidationError("VALIDATION_ERROR", "Invalid format");

        // Act & Assert
        Assert.True(error1.Equals(error2));
        Assert.True(error1 == error2);
        Assert.False(error1 != error2);
    }

    [Fact]
    public void Equals_DifferentCode_ReturnsFalse()
    {
        // Arrange
        var error1 = new ValidationError("VALIDATION_ERROR", "Invalid format");
        var error2 = new ValidationError("FIELD_ERROR", "Invalid format");

        // Act & Assert
        Assert.False(error1.Equals(error2));
        Assert.False(error1 == error2);
        Assert.True(error1 != error2);
    }

    [Fact]
    public void Equals_DifferentMessage_ReturnsFalse()
    {
        // Arrange
        var error1 = new ValidationError("VALIDATION_ERROR", "Invalid format");
        var error2 = new ValidationError("VALIDATION_ERROR", "Required field");

        // Act & Assert
        Assert.False(error1.Equals(error2));
    }

    [Fact]
    public void GetHashCode_SameValues_ReturnsSameHash()
    {
        // Arrange
        var error1 = new ValidationError("VALIDATION_ERROR", "Invalid format");
        var error2 = new ValidationError("VALIDATION_ERROR", "Invalid format");

        // Act & Assert
        Assert.Equal(error1.GetHashCode(), error2.GetHashCode());
    }

    [Fact]
    public void ToString_ReturnsFormattedString()
    {
        // Arrange
        var error = new ValidationError("VALIDATION_ERROR", "Username already exists");

        // Act
        var result = error.ToString();

        // Assert
        Assert.Contains("VALIDATION_ERROR", result);
        Assert.Contains("Username already exists", result);
    }

    [Fact]
    public void InheritanceFromError_WorksCorrectly()
    {
        // Arrange
        var validationError = new ValidationError("VALIDATION_ERROR", "Error message");

        // Act
        Error baseError = validationError;

        // Assert
        Assert.Equal("VALIDATION_ERROR", baseError.Code);
        Assert.Equal("Error message", baseError.Message);
    }

    [Fact]
    public void CanBeUsedInResult()
    {
        // Arrange
        var error = new ValidationError("VALIDATION_ERROR", "Password must be at least 8 characters");

        // Act
        var result = Result.Failure<string>(error);

        // Assert
        Assert.True(result.IsFailure);
        Assert.IsType<ValidationError>(result.Error);
        Assert.Equal("VALIDATION_ERROR", result.Error.Code);
        Assert.Equal("Password must be at least 8 characters", result.Error.Message);
    }

    [Theory]
    [InlineData("REQUIRED", "First name is required")]
    [InlineData("INVALID_FORMAT", "Email format is invalid")]
    [InlineData("OUT_OF_RANGE", "Age must be positive")]
    public void VariousValidationScenarios(string code, string message)
    {
        // Arrange & Act
        var error = new ValidationError(code, message);

        // Assert
        Assert.Equal(code, error.Code);
        Assert.Equal(message, error.Message);
    }

    [Fact]
    public void WithPropertyMetadata_StoresFieldInfo()
    {
        // Arrange
        var metadata = new Dictionary<string, object>
        {
            { "field", "email" },
            { "attempted_value", "not-an-email" },
            { "validation_rule", "email_format" }
        };

        // Act
        var error = new ValidationError("INVALID_EMAIL", "Email format is invalid", metadata);

        // Assert
        Assert.Equal("email", error.Metadata["field"]);
        Assert.Equal("not-an-email", error.Metadata["attempted_value"]);
        Assert.Equal("email_format", error.Metadata["validation_rule"]);
    }
}
}

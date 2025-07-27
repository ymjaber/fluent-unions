namespace FluentUnions.Tests.Errors
{

public class ConflictErrorTests
{
    [Fact]
    public void Constructor_WithCodeAndMessage_SetsPropertiesCorrectly()
    {
        // Arrange & Act
        var error = new ConflictError("CONFLICT", "User with email 'user@example.com' already exists.");

        // Assert
        Assert.Equal("CONFLICT", error.Code);
        Assert.Equal("User with email 'user@example.com' already exists.", error.Message);
        Assert.Empty(error.Metadata);
    }

    [Fact]
    public void Constructor_WithCodeMessageAndMetadata_SetsAllProperties()
    {
        // Arrange
        var metadata = new Dictionary<string, object>
        {
            { "resource_type", "Product" },
            { "conflicting_property", "sku" },
            { "conflicting_value", "PROD-123" }
        };

        // Act
        var error = new ConflictError("CONFLICT", "This SKU is reserved", metadata);

        // Assert
        Assert.Equal("CONFLICT", error.Code);
        Assert.Equal("This SKU is reserved", error.Message);
        Assert.Equal(3, error.Metadata.Count);
        Assert.Equal("Product", error.Metadata["resource_type"]);
        Assert.Equal("sku", error.Metadata["conflicting_property"]);
        Assert.Equal("PROD-123", error.Metadata["conflicting_value"]);
    }

    [Fact]
    public void Equals_SameCodeAndMessage_ReturnsTrue()
    {
        // Arrange
        var error1 = new ConflictError("CONFLICT", "Username already exists");
        var error2 = new ConflictError("CONFLICT", "Username already exists");

        // Act & Assert
        Assert.True(error1.Equals(error2));
        Assert.True(error1 == error2);
        Assert.False(error1 != error2);
    }

    [Fact]
    public void Equals_DifferentCode_ReturnsFalse()
    {
        // Arrange
        var error1 = new ConflictError("CONFLICT", "Email already exists");
        var error2 = new ConflictError("DUPLICATE", "Email already exists");

        // Act & Assert
        Assert.False(error1.Equals(error2));
    }

    [Fact]
    public void Equals_DifferentMessage_ReturnsFalse()
    {
        // Arrange
        var error1 = new ConflictError("CONFLICT", "Email already exists");
        var error2 = new ConflictError("CONFLICT", "Username already exists");

        // Act & Assert
        Assert.False(error1.Equals(error2));
    }

    [Fact]
    public void Equals_SameCodeDifferentMetadata_ReturnsFalse()
    {
        // Arrange
        var metadata1 = new Dictionary<string, object> { { "value", "user1@example.com" } };
        var metadata2 = new Dictionary<string, object> { { "value", "user2@example.com" } };
        var error1 = new ConflictError("CONFLICT", "Email already exists", metadata1);
        var error2 = new ConflictError("CONFLICT", "Email already exists", metadata2);

        // Act & Assert
        Assert.False(error1.Equals(error2));
    }

    [Fact]
    public void GetHashCode_SameValues_ReturnsSameHash()
    {
        // Arrange
        var error1 = new ConflictError("CONFLICT", "Order number already exists");
        var error2 = new ConflictError("CONFLICT", "Order number already exists");

        // Act & Assert
        Assert.Equal(error1.GetHashCode(), error2.GetHashCode());
    }

    [Fact]
    public void ToString_ReturnsFormattedString()
    {
        // Arrange
        var error = new ConflictError("CONFLICT", "Product with barcode '1234567890' already exists.");

        // Act
        var result = error.ToString();

        // Assert
        Assert.Contains("CONFLICT", result);
        Assert.Contains("Product with barcode '1234567890' already exists.", result);
    }

    [Theory]
    [InlineData("CONFLICT", "Resource already exists")]
    [InlineData("DUPLICATE_KEY", "Unique constraint violation")]
    [InlineData("ALREADY_EXISTS", "Entity with this identifier already exists")]
    public void VariousConflictScenarios(string code, string message)
    {
        // Arrange & Act
        var error = new ConflictError(code, message);

        // Assert
        Assert.Equal(code, error.Code);
        Assert.Equal(message, error.Message);
    }

    [Fact]
    public void CanBeUsedInResult()
    {
        // Arrange
        var error = new ConflictError("CONFLICT", "Account number already exists");

        // Act
        var result = Result.Failure<Account>(error);

        // Assert
        Assert.True(result.IsFailure);
        Assert.IsType<ConflictError>(result.Error);
        Assert.Equal("CONFLICT", result.Error.Code);
        Assert.Equal("Account number already exists", result.Error.Message);
    }

    [Fact]
    public void UniqueConstraintViolation_Scenario()
    {
        // Arrange
        var existingUsers = new[] { "john@example.com", "jane@example.com" };
        var newEmail = "john@example.com";

        // Act
        var result = existingUsers.Contains(newEmail)
            ? Result.Failure<string>(new ConflictError("CONFLICT", $"User with email '{newEmail}' already exists"))
            : Result.Success(newEmail);

        // Assert
        Assert.True(result.IsFailure);
        Assert.IsType<ConflictError>(result.Error);
    }

    [Fact]
    public void InheritanceFromError_WorksCorrectly()
    {
        // Arrange
        var conflictError = new ConflictError("CONFLICT", "Resource with key 'value123' already exists");

        // Act
        Error baseError = conflictError;

        // Assert
        Assert.Equal("CONFLICT", baseError.Code);
        Assert.Equal("Resource with key 'value123' already exists", baseError.Message);
    }

    [Fact]
    public void WithConflictMetadata_StoresConflictDetails()
    {
        // Arrange
        var metadata = new Dictionary<string, object>
        {
            { "resource_type", "Account" },
            { "conflicting_field", "accountNumber" },
            { "existing_value", "ACC-12345" },
            { "attempted_value", "ACC-12345" },
            { "timestamp", "2023-01-01T10:00:00Z" }
        };

        // Act
        var error = new ConflictError("CONFLICT", "Duplicate account number", metadata);

        // Assert
        Assert.Equal("Account", error.Metadata["resource_type"]);
        Assert.Equal("accountNumber", error.Metadata["conflicting_field"]);
        Assert.Equal("ACC-12345", error.Metadata["existing_value"]);
    }

    private class Account
    {
        public string AccountNumber { get; set; } = "";
        public string Name { get; set; } = "";
    }
}
}

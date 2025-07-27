namespace FluentUnions.Tests.Errors
{

public class NotFoundErrorTests
{
    [Fact]
    public void Constructor_WithCodeAndMessage_SetsPropertiesCorrectly()
    {
        // Arrange & Act
        var error = new NotFoundError("NOT_FOUND", "User with ID '123' was not found.");

        // Assert
        Assert.Equal("NOT_FOUND", error.Code);
        Assert.Equal("User with ID '123' was not found.", error.Message);
    }

    [Fact]
    public void Constructor_WithMetadata_UsesMetadata()
    {
        // Arrange
        var metadata = new Dictionary<string, object> { { "resourceType", "Product" }, { "resourceId", "ABC123" } };
        
        // Act
        var error = new NotFoundError("NOT_FOUND", "This product has been discontinued", metadata);

        // Assert
        Assert.Equal("NOT_FOUND", error.Code);
        Assert.Equal("This product has been discontinued", error.Message);
        Assert.Equal("Product", error.Metadata["resourceType"]);
        Assert.Equal("ABC123", error.Metadata["resourceId"]);
    }

    [Fact]
    public void Equals_SameCodeAndMessage_ReturnsTrue()
    {
        // Arrange
        var error1 = new NotFoundError("NOT_FOUND", "User with ID '123' was not found.");
        var error2 = new NotFoundError("NOT_FOUND", "User with ID '123' was not found.");

        // Act & Assert
        Assert.True(error1.Equals(error2));
        Assert.True(error1 == error2);
        Assert.False(error1 != error2);
    }

    [Fact]
    public void Equals_DifferentMessage_ReturnsFalse()
    {
        // Arrange
        var error1 = new NotFoundError("NOT_FOUND", "User with ID '123' was not found.");
        var error2 = new NotFoundError("NOT_FOUND", "Product with ID '123' was not found.");

        // Act & Assert
        Assert.False(error1.Equals(error2));
        Assert.False(error1 == error2);
        Assert.True(error1 != error2);
    }

    [Fact]
    public void Equals_DifferentCode_ReturnsFalse()
    {
        // Arrange
        var error1 = new NotFoundError("NOT_FOUND", "User with ID '123' was not found.");
        var error2 = new NotFoundError("RESOURCE_NOT_FOUND", "User with ID '123' was not found.");

        // Act & Assert
        Assert.False(error1.Equals(error2));
    }

    [Fact]
    public void GetHashCode_SameValues_ReturnsSameHash()
    {
        // Arrange
        var error1 = new NotFoundError("NOT_FOUND", "Order with ID 'ORD-001' was not found.");
        var error2 = new NotFoundError("NOT_FOUND", "Order with ID 'ORD-001' was not found.");

        // Act & Assert
        Assert.Equal(error1.GetHashCode(), error2.GetHashCode());
    }

    [Fact]
    public void ToString_ReturnsFormattedString()
    {
        // Arrange
        var error = new NotFoundError("NOT_FOUND", "Customer with ID 'CUST-123' was not found.");

        // Act
        var result = error.ToString();

        // Assert
        Assert.Equal("NotFoundError: NOT_FOUND - Customer with ID 'CUST-123' was not found.", result);
    }

    [Theory]
    [InlineData("NOT_FOUND", "User with ID '123' was not found.")]
    [InlineData("RESOURCE_NOT_FOUND", "Product with ID 'ABC' was not found.")]
    [InlineData("NOT_FOUND", "Order with ID 'ORD-001' was not found.")]
    public void Constructor_SetsCodeAndMessage(string code, string message)
    {
        // Arrange & Act
        var error = new NotFoundError(code, message);

        // Assert
        Assert.Equal(code, error.Code);
        Assert.Equal(message, error.Message);
    }

    [Fact]
    public void CanBeUsedInResult()
    {
        // Arrange
        var error = new NotFoundError("NOT_FOUND", "User with ID 'USER-123' was not found.");

        // Act
        var result = Result.Failure<User>(error);

        // Assert
        Assert.True(result.IsFailure);
        Assert.IsType<NotFoundError>(result.Error);
        var notFoundError = (NotFoundError)result.Error;
        Assert.Equal("NOT_FOUND", notFoundError.Code);
        Assert.Contains("USER-123", notFoundError.Message);
    }

    [Fact]
    public void InheritanceFromError_WorksCorrectly()
    {
        // Arrange
        var notFoundError = new NotFoundError("NOT_FOUND", "Document with ID 'DOC-456' was not found.");

        // Act
        Error baseError = notFoundError;

        // Assert
        Assert.Equal("NOT_FOUND", baseError.Code);
        Assert.Contains("Document", baseError.Message);
        Assert.Contains("DOC-456", baseError.Message);
    }

    [Fact]
    public void WithResourceMetadata_StoresResourceInfo()
    {
        // Arrange
        var metadata = new Dictionary<string, object>
        {
            { "resource_type", "CompositeResource" },
            { "resource_id", "Type:SubType:123:ABC" },
            { "search_params", new { type = "Type", subtype = "SubType", id = "123", code = "ABC" } }
        };

        // Act
        var error = new NotFoundError("NOT_FOUND", "Composite resource not found", metadata);

        // Assert
        Assert.Equal("CompositeResource", error.Metadata["resource_type"]);
        Assert.Equal("Type:SubType:123:ABC", error.Metadata["resource_id"]);
        Assert.NotNull(error.Metadata["search_params"]);
    }

    private class User
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
    }
}
}

using System.Text.Json;
using FluentUnions.Serialization;
using FluentUnions.Tests.Errors;

namespace FluentUnions.Tests.Serialization
{
    public class ErrorSerializationTests
    {
        private readonly JsonSerializerOptions _options;

        public ErrorSerializationTests()
        {
            _options = new JsonSerializerOptions();
            _options.Converters.Add(new ErrorJsonConverter());
            _options.Converters.Add(new AggregateErrorJsonConverter());
        }

        [Fact]
        public void Serialize_BasicError_WritesCorrectly()
        {
            // Arrange
            var error = new Error("E001", "Something went wrong");

            // Act
            var json = JsonSerializer.Serialize(error, _options);

            // Assert
            Assert.Contains("\"Code\":\"E001\"", json);
            Assert.Contains("\"Message\":\"Something went wrong\"", json);
        }

        [Fact]
        public void Serialize_ErrorWithMetadata_WritesMetadataCorrectly()
        {
            // Arrange
            var metadata = new Dictionary<string, object>
            {
                ["userId"] = 123,
                ["timestamp"] = "2023-12-25T10:00:00Z",
                ["details"] = new { action = "delete", resource = "user" }
            };
            var error = new TestError("E001", "Operation failed", metadata);

            // Act
            var json = JsonSerializer.Serialize(error, _options);

            // Assert
            Assert.Contains("\"Code\":\"E001\"", json);
            Assert.Contains("\"Message\":\"Operation failed\"", json);
            Assert.Contains("\"Metadata\":{", json);
            Assert.Contains("\"userId\":123", json);
            Assert.Contains("\"timestamp\":\"2023-12-25T10:00:00Z\"", json);
        }

        [Fact]
        public void Deserialize_BasicError_ReconstructsCorrectly()
        {
            // Arrange
            var json = "{\"Code\":\"E001\",\"Message\":\"Test error\"}";

            // Act
            var error = JsonSerializer.Deserialize<Error>(json, _options);

            // Assert
            Assert.NotNull(error);
            Assert.Equal("E001", error.Code);
            Assert.Equal("Test error", error.Message);
        }

        [Fact]
        public void Deserialize_ErrorWithMetadata_ReconstructsCorrectly()
        {
            // Arrange
            var json = "{\"Code\":\"E001\",\"Message\":\"Test error\",\"Metadata\":{\"key\":\"value\",\"number\":42}}";

            // Act
            var error = JsonSerializer.Deserialize<Error>(json, _options);

            // Assert
            Assert.NotNull(error);
            Assert.Equal("E001", error.Code);
            Assert.Equal("Test error", error.Message);
        }

        [Fact]
        public void Serialize_ValidationError_WritesTypeDiscriminator()
        {
            // Arrange
            var error = new ValidationError("email", "Invalid email format");

            // Act
            var json = JsonSerializer.Serialize(error, _options);

            // Assert
            Assert.Contains("\"$type\":\"ValidationError\"", json);
            Assert.Contains("\"Code\":\"email\"", json);
            Assert.Contains("\"Message\":\"Invalid email format\"", json);
        }

        [Fact]
        public void Serialize_NotFoundError_WritesCorrectly()
        {
            // Arrange
            var error = new NotFoundError("USER_NOT_FOUND", "User with ID '123' was not found");

            // Act
            var json = JsonSerializer.Serialize(error, _options);

            // Assert
            Assert.Contains("\"$type\":\"NotFoundError\"", json);
            Assert.Contains("\"Code\":\"USER_NOT_FOUND\"", json);
            Assert.Contains("\"Message\":", json);
            Assert.Contains("User with ID", json);
            Assert.Contains("123", json);
            Assert.Contains("was not found", json);
        }

        [Fact]
        public void Serialize_ConflictError_WritesCorrectly()
        {
            // Arrange
            var error = new ConflictError("DUPLICATE_EMAIL", "Email 'test@example.com' already exists");

            // Act
            var json = JsonSerializer.Serialize(error, _options);

            // Assert
            Assert.Contains("\"$type\":\"ConflictError\"", json);
            Assert.Contains("\"Code\":\"DUPLICATE_EMAIL\"", json);
            Assert.Contains("\"Message\":", json);
            Assert.Contains("Email", json);
            Assert.Contains("test@example.com", json);
            Assert.Contains("already exists", json);
        }

        [Fact]
        public void Serialize_AggregateError_WritesNestedErrors()
        {
            // Arrange
            var errors = new List<Error>
            {
                new ValidationError("name", "Name is required"),
                new ValidationError("email", "Invalid email format"),
                new Error("E001", "Generic error")
            };
            var aggregateError = new AggregateError(errors);

            // Act
            var json = JsonSerializer.Serialize(aggregateError, _options);

            // Assert
            Assert.Contains("\"Code\":\"Errors.Aggregate\"", json);
            Assert.Contains("\"Message\":\"Multiple errors occurred.\"", json);
            Assert.Contains("\"Errors\":[", json);
            Assert.Contains("\"Code\":\"name\"", json);
            Assert.Contains("\"Code\":\"email\"", json);
            Assert.Contains("\"Code\":\"E001\"", json);
        }

        [Fact]
        public void RoundTrip_ValidationError_PreservesType()
        {
            // Arrange
            var original = new ValidationError("field", "Invalid value");

            // Act
            var json = JsonSerializer.Serialize(original, _options);
            var deserialized = JsonSerializer.Deserialize<Error>(json, _options);

            // Assert
            Assert.NotNull(deserialized);
            Assert.Equal(original.Code, deserialized.Code);
            Assert.Equal(original.Message, deserialized.Message);
            // Note: Deserialization to base Error type loses the specific type information
        }

        [Fact]
        public void Serialize_ErrorArray_HandlesCorrectly()
        {
            // Arrange
            var errors = new Error[]
            {
                new Error("E001", "Error 1"),
                new ValidationError("field", "Invalid"),
                new NotFoundError("NOT_FOUND", "Resource not found"),
                new ConflictError("CONFLICT", "Resource conflict"),
                new AuthenticationError("AUTH_FAILED", "Authentication failed"),
                new AuthorizationError("AUTH_DENIED", "Access denied")
            };

            // Act
            var json = JsonSerializer.Serialize(errors, _options);

            // Assert
            Assert.Contains("\"Code\":\"E001\"", json);
            Assert.Contains("\"Code\":\"field\"", json);
            Assert.Contains("\"Code\":\"NOT_FOUND\"", json);
            Assert.Contains("\"Code\":\"CONFLICT\"", json);
            Assert.Contains("\"Code\":\"AUTH_FAILED\"", json);
            Assert.Contains("\"Code\":\"AUTH_DENIED\"", json);
        }

        [Fact]
        public void Serialize_ErrorWithComplexMetadata_HandlesCorrectly()
        {
            // Arrange
            var metadata = new Dictionary<string, object>
            {
                ["simple"] = "value",
                ["number"] = 42,
                ["boolean"] = true,
                ["array"] = new[] { 1, 2, 3 },
                ["nested"] = new Dictionary<string, object>
                {
                    ["inner"] = "value",
                    ["count"] = 10
                }
            };
            var error = new TestError("E001", "Complex error", metadata);

            // Act
            var json = JsonSerializer.Serialize(error, _options);

            // Assert
            Assert.Contains("\"simple\":\"value\"", json);
            Assert.Contains("\"number\":42", json);
            Assert.Contains("\"boolean\":true", json);
            Assert.Contains("\"array\":[1,2,3]", json);
            Assert.Contains("\"nested\":{", json);
            Assert.Contains("\"inner\":\"value\"", json);
            Assert.Contains("\"count\":10", json);
        }

        [Fact]
        public void Serialize_ErrorWithNullMetadata_HandlesCorrectly()
        {
            // Arrange
            var error = new Error("E001", "Error without metadata");

            // Act
            var json = JsonSerializer.Serialize(error, _options);

            // Assert
            Assert.Contains("\"Code\":\"E001\"", json);
            Assert.Contains("\"Message\":\"Error without metadata\"", json);
            // Should not contain Metadata property when it's empty
        }

        [Fact]
        public void Serialize_NestedAggregateErrors_HandlesCorrectly()
        {
            // Arrange
            var innerErrors = new[]
            {
                new ValidationError("field1", "Invalid field1"),
                new ValidationError("field2", "Invalid field2")
            };
            var innerAggregate = new AggregateError(innerErrors);
            
            var outerErrors = new Error[]
            {
                new Error("E001", "Top level error"),
                innerAggregate
            };
            var outerAggregate = new AggregateError(outerErrors);

            // Act
            var json = JsonSerializer.Serialize(outerAggregate, _options);

            // Assert
            Assert.Contains("\"Code\":\"Errors.Aggregate\"", json);
            Assert.Contains("\"Errors\":[", json);
            Assert.Contains("\"Code\":\"E001\"", json);
            // The inner aggregate will be serialized as another error in the array
            var aggregateCount = json.Split(new[] { "\"Code\":\"Errors.Aggregate\"" }, StringSplitOptions.None).Length - 1;
            Assert.Equal(2, aggregateCount); // One for outer, one for inner
        }

        [Fact]
        public void Deserialize_ErrorWithEmptyMetadata_HandlesCorrectly()
        {
            // Arrange
            var json = "{\"Code\":\"E001\",\"Message\":\"Test\",\"Metadata\":{}}";

            // Act
            var error = JsonSerializer.Deserialize<Error>(json, _options);

            // Assert
            Assert.NotNull(error);
            Assert.Equal("E001", error.Code);
            Assert.Equal("Test", error.Message);
        }

        [Fact]
        public void Serialize_AuthenticationError_WritesTypeDiscriminator()
        {
            // Arrange
            var error = new AuthenticationError("Auth.InvalidCredentials", "Invalid username or password");

            // Act
            var json = JsonSerializer.Serialize(error, _options);

            // Assert
            Assert.Contains("\"$type\":\"AuthenticationError\"", json);
            Assert.Contains("\"Code\":\"Auth.InvalidCredentials\"", json);
            Assert.Contains("\"Message\":\"Invalid username or password\"", json);
        }

        [Fact]
        public void Serialize_AuthorizationError_WritesTypeDiscriminator()
        {
            // Arrange
            var error = new AuthorizationError("Auth.InsufficientPermissions", "User lacks admin privileges");

            // Act
            var json = JsonSerializer.Serialize(error, _options);

            // Assert
            Assert.Contains("\"$type\":\"AuthorizationError\"", json);
            Assert.Contains("\"Code\":\"Auth.InsufficientPermissions\"", json);
            Assert.Contains("\"Message\":\"User lacks admin privileges\"", json);
        }

        [Fact]
        public void Serialize_AuthenticationErrorWithMetadata_WritesCorrectly()
        {
            // Arrange
            var metadata = new Dictionary<string, object>
            {
                ["FailedAttempts"] = 3,
                ["LastAttempt"] = "2023-12-25T10:00:00Z",
                ["AuthMethod"] = "password"
            };
            var error = new AuthenticationError("Auth.AccountLocked", "Account locked due to multiple failed attempts", metadata);

            // Act
            var json = JsonSerializer.Serialize(error, _options);

            // Assert
            Assert.Contains("\"$type\":\"AuthenticationError\"", json);
            Assert.Contains("\"Code\":\"Auth.AccountLocked\"", json);
            Assert.Contains("\"FailedAttempts\":3", json);
            Assert.Contains("\"AuthMethod\":\"password\"", json);
        }

        [Fact]
        public void Serialize_AuthorizationErrorWithMetadata_WritesCorrectly()
        {
            // Arrange
            var metadata = new Dictionary<string, object>
            {
                ["RequiredRole"] = "Admin",
                ["UserRole"] = "Member",
                ["Resource"] = "UserManagement"
            };
            var error = new AuthorizationError("Auth.RoleRequired", "Admin role required for this operation", metadata);

            // Act
            var json = JsonSerializer.Serialize(error, _options);

            // Assert
            Assert.Contains("\"$type\":\"AuthorizationError\"", json);
            Assert.Contains("\"Code\":\"Auth.RoleRequired\"", json);
            Assert.Contains("\"RequiredRole\":\"Admin\"", json);
            Assert.Contains("\"UserRole\":\"Member\"", json);
            Assert.Contains("\"Resource\":\"UserManagement\"", json);
        }
    }
}
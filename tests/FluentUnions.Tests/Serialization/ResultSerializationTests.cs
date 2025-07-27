using System.Text.Json;
using FluentUnions.Serialization;
using FluentUnions.Tests.Errors;

namespace FluentUnions.Tests.Serialization
{
    public class ResultSerializationTests
    {
        private readonly JsonSerializerOptions _options;

        public ResultSerializationTests()
        {
            _options = new JsonSerializerOptions();
            _options.Converters.Add(new ResultJsonConverter<int>());
            _options.Converters.Add(new ResultJsonConverter<string>());
            _options.Converters.Add(new ResultJsonConverter<Product>());
            _options.Converters.Add(new ResultJsonConverter<List<string>>());
            _options.Converters.Add(new ErrorJsonConverter());
        }

        [Fact]
        public void Serialize_Success_WritesSuccessObject()
        {
            // Arrange
            var result = Result.Success(42);

            // Act
            var json = JsonSerializer.Serialize(result, _options);

            // Assert
            var expected = "{\"isSuccess\":true,\"value\":42}";
            Assert.Equal(expected, json);
        }

        [Fact]
        public void Serialize_Failure_WritesFailureObject()
        {
            // Arrange
            var error = new Error("E001", "Something went wrong");
            var result = Result.Failure<int>(error);

            // Act
            var json = JsonSerializer.Serialize(result, _options);

            // Assert
            Assert.Contains("\"isSuccess\":false", json);
            Assert.Contains("\"error\":{", json);
            Assert.Contains("\"Code\":\"E001\"", json);
            Assert.Contains("\"Message\":\"Something went wrong\"", json);
        }

        [Fact]
        public void Serialize_SuccessWithString_WritesCorrectly()
        {
            // Arrange
            var result = Result.Success("Hello World");

            // Act
            var json = JsonSerializer.Serialize(result, _options);

            // Assert
            var expected = "{\"isSuccess\":true,\"value\":\"Hello World\"}";
            Assert.Equal(expected, json);
        }

        [Fact]
        public void Serialize_SuccessWithComplexType_WritesCorrectly()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Laptop", Price = 999.99m };
            var result = Result.Success(product);

            // Act
            var json = JsonSerializer.Serialize(result, _options);

            // Assert
            Assert.Contains("\"isSuccess\":true", json);
            Assert.Contains("\"value\":{", json);
            Assert.Contains("\"Id\":1", json);
            Assert.Contains("\"Name\":\"Laptop\"", json);
            Assert.Contains("\"Price\":999.99", json);
        }

        [Fact]
        public void Serialize_FailureWithValidationError_WritesCorrectly()
        {
            // Arrange
            var error = new ValidationError("email", "Invalid email format");
            var result = Result.Failure<string>(error);

            // Act
            var json = JsonSerializer.Serialize(result, _options);

            // Assert
            Assert.Contains("\"isSuccess\":false", json);
            Assert.Contains("\"Code\":\"email\"", json);
            Assert.Contains("\"Message\":\"Invalid email format\"", json);
        }

        [Fact]
        public void Deserialize_SuccessObject_ReturnsSuccess()
        {
            // Arrange
            var json = "{\"isSuccess\":true,\"value\":42}";

            // Act
            var result = JsonSerializer.Deserialize<Result<int>>(json, _options);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(42, result.Value);
        }

        [Fact]
        public void Deserialize_FailureObject_ReturnsFailure()
        {
            // Arrange
            var json = "{\"isSuccess\":false,\"error\":{\"Code\":\"E001\",\"Message\":\"Test error\"}}";

            // Act
            var result = JsonSerializer.Deserialize<Result<int>>(json, _options);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal("E001", result.Error.Code);
            Assert.Equal("Test error", result.Error.Message);
        }

        [Fact]
        public void RoundTrip_Success_PreservesValue()
        {
            // Arrange
            var original = Result.Success(42);

            // Act
            var json = JsonSerializer.Serialize(original, _options);
            var deserialized = JsonSerializer.Deserialize<Result<int>>(json, _options);

            // Assert
            Assert.True(deserialized.IsSuccess);
            Assert.Equal(original.Value, deserialized.Value);
        }

        [Fact]
        public void RoundTrip_Failure_PreservesError()
        {
            // Arrange
            var error = new Error("E001", "Original error");
            var original = Result.Failure<int>(error);

            // Act
            var json = JsonSerializer.Serialize(original, _options);
            var deserialized = JsonSerializer.Deserialize<Result<int>>(json, _options);

            // Assert
            Assert.True(deserialized.IsFailure);
            Assert.Equal(original.Error.Code, deserialized.Error.Code);
            Assert.Equal(original.Error.Message, deserialized.Error.Message);
        }

        [Fact]
        public void Serialize_NestedResult_WritesCorrectly()
        {
            // Arrange
            var operation = new Operation
            {
                Id = 1,
                Name = "Test Operation",
                Result = Result.Success("Operation completed")
            };

            // Act
            var json = JsonSerializer.Serialize(operation, _options);

            // Assert
            Assert.Contains("\"Id\":1", json);
            Assert.Contains("\"Name\":\"Test Operation\"", json);
            Assert.Contains("\"Result\":{\"isSuccess\":true,\"value\":\"Operation completed\"}", json);
        }

        [Fact]
        public void Deserialize_NestedResult_ReconstructsCorrectly()
        {
            // Arrange
            var json = "{\"Id\":1,\"Name\":\"Test\",\"Result\":{\"isSuccess\":false,\"error\":{\"Code\":\"OP_FAIL\",\"Message\":\"Operation failed\"}}}";

            // Act
            var operation = JsonSerializer.Deserialize<Operation>(json, _options);

            // Assert
            Assert.NotNull(operation);
            Assert.Equal(1, operation.Id);
            Assert.Equal("Test", operation.Name);
            Assert.True(operation.Result.IsFailure);
            Assert.Equal("OP_FAIL", operation.Result.Error.Code);
        }

        [Fact]
        public void Serialize_ArrayOfResults_HandlesCorrectly()
        {
            // Arrange
            var results = new[]
            {
                Result.Success(1),
                Result.Failure<int>(new Error("E001", "Error 1")),
                Result.Success(3)
            };

            // Act
            var json = JsonSerializer.Serialize(results, _options);

            // Assert
            Assert.Contains("{\"isSuccess\":true,\"value\":1}", json);
            Assert.Contains("{\"isSuccess\":false,\"error\":", json);
            Assert.Contains("{\"isSuccess\":true,\"value\":3}", json);
        }

        [Fact]
        public void Deserialize_ArrayOfResults_HandlesCorrectly()
        {
            // Arrange
            var json = "[{\"isSuccess\":true,\"value\":1},{\"isSuccess\":false,\"error\":{\"Code\":\"E001\",\"Message\":\"Error\"}},{\"isSuccess\":true,\"value\":3}]";

            // Act
            var results = JsonSerializer.Deserialize<Result<int>[]>(json, _options);

            // Assert
            Assert.NotNull(results);
            Assert.Equal(3, results.Length);
            Assert.True(results[0].IsSuccess);
            Assert.Equal(1, results[0].Value);
            Assert.True(results[1].IsFailure);
            Assert.Equal("E001", results[1].Error.Code);
            Assert.True(results[2].IsSuccess);
            Assert.Equal(3, results[2].Value);
        }

        [Fact]
        public void Serialize_CollectionResult_HandlesCorrectly()
        {
            // Arrange
            var items = new List<string> { "item1", "item2", "item3" };
            var result = Result.Success(items);

            // Act
            var json = JsonSerializer.Serialize(result, _options);

            // Assert
            Assert.Contains("\"isSuccess\":true", json);
            Assert.Contains("\"value\":[\"item1\",\"item2\",\"item3\"]", json);
        }

        [Fact]
        public void Deserialize_CollectionResult_HandlesCorrectly()
        {
            // Arrange
            var json = "{\"isSuccess\":true,\"value\":[\"item1\",\"item2\",\"item3\"]}";

            // Act
            var result = JsonSerializer.Deserialize<Result<List<string>>>(json, _options);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(3, result.Value.Count);
            Assert.Equal("item1", result.Value[0]);
            Assert.Equal("item2", result.Value[1]);
            Assert.Equal("item3", result.Value[2]);
        }

        [Fact]
        public void Deserialize_CaseInsensitive_WorksCorrectly()
        {
            // Arrange
            var json = "{\"IsSuccess\":true,\"Value\":42}"; // Different casing

            // Act
            var result = JsonSerializer.Deserialize<Result<int>>(json, _options);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(42, result.Value);
        }

        [Fact]
        public void Serialize_ErrorWithMetadata_PreservesMetadata()
        {
            // Arrange
            var metadata = new Dictionary<string, object>
            {
                ["userId"] = 123,
                ["timestamp"] = "2023-12-25T10:00:00Z"
            };
            var error = new TestError("E001", "Error with metadata", metadata);
            var result = Result.Failure<int>(error);

            // Act
            var json = JsonSerializer.Serialize(result, _options);

            // Assert
            Assert.Contains("\"Metadata\":{", json);
            Assert.Contains("\"userId\":123", json);
            Assert.Contains("\"timestamp\":\"2023-12-25T10:00:00Z\"", json);
        }

        [Fact]
        public void Deserialize_MalformedJson_ThrowsJsonException()
        {
            // Arrange
            var json = "not valid json";

            // Act & Assert
            Assert.Throws<JsonException>(() => 
                JsonSerializer.Deserialize<Result<int>>(json, _options));
        }

        [Fact]
        public void Deserialize_MissingIsSuccess_DefaultsToFailure()
        {
            // Arrange
            var json = "{\"value\":42}"; // Missing isSuccess

            // Act & Assert
            var result = JsonSerializer.Deserialize<Result<int>>(json, _options);
            // Should default to failure since isSuccess defaults to false
            Assert.True(result.IsFailure);
        }

        // Test models
        private class Product
        {
            public int Id { get; set; }
            public string Name { get; set; } = "";
            public decimal Price { get; set; }
        }

        private class Operation
        {
            public int Id { get; set; }
            public string Name { get; set; } = "";
            public Result<string> Result { get; set; }
        }
    }
}
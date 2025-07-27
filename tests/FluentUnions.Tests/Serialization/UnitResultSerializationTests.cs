using System.Text.Json;
using System.Text.Json.Serialization;
using FluentUnions.Serialization;

namespace FluentUnions.Tests.Serialization
{
    public class UnitResultSerializationTests
    {
        private readonly JsonSerializerOptions _options;

        public UnitResultSerializationTests()
        {
            _options = new JsonSerializerOptions();
            _options.Converters.Add(new UnitResultJsonConverter());
            _options.Converters.Add(new ErrorJsonConverter());
        }

        [Fact]
        public void Serialize_Success_WritesSuccessObject()
        {
            // Arrange
            var result = Result.Success();

            // Act
            var json = JsonSerializer.Serialize(result, _options);

            // Assert
            var expected = "{\"isSuccess\":true}";
            Assert.Equal(expected, json);
        }

        [Fact]
        public void Serialize_Failure_WritesFailureObject()
        {
            // Arrange
            var error = new Error("E001", "Operation failed");
            var result = Result.Failure(error);

            // Act
            var json = JsonSerializer.Serialize(result, _options);

            // Assert
            Assert.Contains("\"isSuccess\":false", json);
            Assert.Contains("\"error\":{", json);
            Assert.Contains("\"Code\":\"E001\"", json);
            Assert.Contains("\"Message\":\"Operation failed\"", json);
        }

        [Fact]
        public void Deserialize_SuccessObject_ReturnsSuccess()
        {
            // Arrange
            var json = "{\"isSuccess\":true}";

            // Act
            var result = JsonSerializer.Deserialize<Result>(json, _options);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public void Deserialize_FailureObject_ReturnsFailure()
        {
            // Arrange
            var json = "{\"isSuccess\":false,\"error\":{\"Code\":\"E001\",\"Message\":\"Test error\"}}";

            // Act
            var result = JsonSerializer.Deserialize<Result>(json, _options);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal("E001", result.Error.Code);
            Assert.Equal("Test error", result.Error.Message);
        }

        [Fact]
        public void RoundTrip_Success_PreservesState()
        {
            // Arrange
            var original = Result.Success();

            // Act
            var json = JsonSerializer.Serialize(original, _options);
            var deserialized = JsonSerializer.Deserialize<Result>(json, _options);

            // Assert
            Assert.True(deserialized.IsSuccess);
        }

        [Fact]
        public void RoundTrip_Failure_PreservesError()
        {
            // Arrange
            var error = new ValidationError("field", "Invalid value");
            var original = Result.Failure(error);

            // Act
            var json = JsonSerializer.Serialize(original, _options);
            var deserialized = JsonSerializer.Deserialize<Result>(json, _options);

            // Assert
            Assert.True(deserialized.IsFailure);
            Assert.Equal(original.Error.Code, deserialized.Error.Code);
            Assert.Equal(original.Error.Message, deserialized.Error.Message);
        }

        [Fact]
        public void Serialize_NestedUnitResult_WritesCorrectly()
        {
            // Arrange
            var response = new ApiResponse
            {
                Id = 1,
                Operation = "Delete",
                Result = Result.Success()
            };

            // Act
            var json = JsonSerializer.Serialize(response, _options);

            // Assert
            Assert.Contains("\"Id\":1", json);
            Assert.Contains("\"Operation\":\"Delete\"", json);
            Assert.Contains("\"Result\":{\"isSuccess\":true}", json);
        }

        [Fact]
        public void Deserialize_NestedUnitResult_ReconstructsCorrectly()
        {
            // Arrange
            var json = "{\"Id\":2,\"Operation\":\"Update\",\"Result\":{\"isSuccess\":false,\"error\":{\"Code\":\"UPDATE_FAIL\",\"Message\":\"Update failed\"}}}";

            // Act
            var response = JsonSerializer.Deserialize<ApiResponse>(json, _options);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(2, response.Id);
            Assert.Equal("Update", response.Operation);
            Assert.True(response.Result.IsFailure);
            Assert.Equal("UPDATE_FAIL", response.Result.Error.Code);
        }

        [Fact]
        public void Serialize_ArrayOfUnitResults_HandlesCorrectly()
        {
            // Arrange
            var results = new[]
            {
                Result.Success(),
                Result.Failure(new Error("E001", "Error")),
                Result.Success()
            };

            // Act
            var json = JsonSerializer.Serialize(results, _options);

            // Assert
            Assert.Contains("{\"isSuccess\":true}", json);
            Assert.Contains("{\"isSuccess\":false,\"error\":", json);
            Assert.Equal(2, json.Split(new[] { "{\"isSuccess\":true}" }, StringSplitOptions.None).Length - 1);
        }

        // Test model
        private class ApiResponse
        {
            public int Id { get; set; }
            public string Operation { get; set; } = "";
            public Result Result { get; set; }
        }

        // Custom converter for unit Result
        private class UnitResultJsonConverter : JsonConverter<Result>
        {
            public override Result Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType != JsonTokenType.StartObject)
                    throw new JsonException();

                bool isSuccess = false;
                Error? error = null;

                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndObject)
                        break;

                    if (reader.TokenType != JsonTokenType.PropertyName)
                        throw new JsonException();

                    string propertyName = reader.GetString()!;
                    reader.Read();

                    switch (propertyName.ToLower())
                    {
                        case "issuccess":
                            isSuccess = reader.GetBoolean();
                            break;
                        case "error":
                            error = JsonSerializer.Deserialize<Error>(ref reader, options);
                            break;
                    }
                }

                return isSuccess ? Result.Success() : Result.Failure(error!);
            }

            public override void Write(Utf8JsonWriter writer, Result value, JsonSerializerOptions options)
            {
                writer.WriteStartObject();
                writer.WriteBoolean("isSuccess", value.IsSuccess);
                
                if (value.IsFailure)
                {
                    writer.WritePropertyName("error");
                    JsonSerializer.Serialize(writer, value.Error, options);
                }
                
                writer.WriteEndObject();
            }
        }
    }
}
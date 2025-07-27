using System.Text.Json;
using FluentUnions.Serialization;

namespace FluentUnions.Tests.Serialization
{
    public class IntegrationSerializationTests
    {
        private readonly JsonSerializerOptions _options;

        public IntegrationSerializationTests()
        {
            _options = new JsonSerializerOptions();
            _options.Converters.Add(new OptionJsonConverter<string>());
            _options.Converters.Add(new OptionJsonConverter<int>());
            _options.Converters.Add(new OptionJsonConverter<UserProfile>());
            _options.Converters.Add(new OptionJsonConverter<DateTime>());
            _options.Converters.Add(new OptionJsonConverter<Dictionary<string, object>>());
            _options.Converters.Add(new ResultJsonConverter<UserProfile>());
            _options.Converters.Add(new ResultJsonConverter<List<OrderItem>>());
            _options.Converters.Add(new ResultJsonConverter<SearchResults>());
            _options.Converters.Add(new ErrorJsonConverter());
            _options.Converters.Add(new AggregateErrorJsonConverter());
        }

        [Fact]
        public void Serialize_UserProfileWithOptions_HandlesCorrectly()
        {
            // Arrange
            var profile = new UserProfile
            {
                Id = 1,
                Name = "John Doe",
                Email = Option.Some("john@example.com"),
                Phone = Option<string>.None,
                Age = Option.Some(30),
                LastLogin = Option.Some(new DateTime(2023, 12, 25, 10, 0, 0, DateTimeKind.Utc))
            };

            // Act
            var json = JsonSerializer.Serialize(profile, _options);

            // Assert
            Assert.Contains("\"Id\":1", json);
            Assert.Contains("\"Name\":\"John Doe\"", json);
            Assert.Contains("\"Email\":\"john@example.com\"", json);
            Assert.Contains("\"Phone\":null", json);
            Assert.Contains("\"Age\":30", json);
            Assert.Contains("2023-12-25T10:00:00", json);
        }

        [Fact]
        public void Deserialize_UserProfileWithOptions_ReconstructsCorrectly()
        {
            // Arrange
            var json = "{\"Id\":2,\"Name\":\"Jane Smith\",\"Email\":null,\"Phone\":\"+1234567890\",\"Age\":null,\"LastLogin\":\"2023-12-26T15:30:00Z\"}";

            // Act
            var profile = JsonSerializer.Deserialize<UserProfile>(json, _options);

            // Assert
            Assert.NotNull(profile);
            Assert.Equal(2, profile.Id);
            Assert.Equal("Jane Smith", profile.Name);
            Assert.True(profile.Email.IsNone);
            Assert.True(profile.Phone.IsSome);
            Assert.Equal("+1234567890", profile.Phone.Value);
            Assert.True(profile.Age.IsNone);
            Assert.True(profile.LastLogin.IsSome);
        }

        [Fact]
        public void Serialize_ApiResponseWithResult_HandlesSuccess()
        {
            // Arrange
            var response = new ApiResponse<UserProfile>
            {
                RequestId = "req-123",
                Timestamp = new DateTime(2023, 12, 25, 12, 0, 0, DateTimeKind.Utc),
                Result = Result.Success(new UserProfile 
                { 
                    Id = 1, 
                    Name = "Test User",
                    Email = Option.Some("test@example.com")
                })
            };

            // Act
            var json = JsonSerializer.Serialize(response, _options);

            // Assert
            Assert.Contains("\"RequestId\":\"req-123\"", json);
            Assert.Contains("\"Result\":{\"isSuccess\":true", json);
            Assert.Contains("\"value\":{", json);
            Assert.Contains("\"Name\":\"Test User\"", json);
        }

        [Fact]
        public void Serialize_ApiResponseWithResult_HandlesFailure()
        {
            // Arrange
            var response = new ApiResponse<UserProfile>
            {
                RequestId = "req-456",
                Timestamp = new DateTime(2023, 12, 25, 12, 0, 0, DateTimeKind.Utc),
                Result = Result.Failure<UserProfile>(new ValidationError("email", "Invalid email format"))
            };

            // Act
            var json = JsonSerializer.Serialize(response, _options);

            // Assert
            Assert.Contains("\"RequestId\":\"req-456\"", json);
            Assert.Contains("\"Result\":{\"isSuccess\":false", json);
            Assert.Contains("\"error\":{", json);
            Assert.Contains("\"Code\":\"email\"", json);
            Assert.Contains("\"Message\":\"Invalid email format\"", json);
        }

        [Fact]
        public void RoundTrip_ComplexNestedStructure_PreservesAllData()
        {
            // Arrange
            var original = new SearchResults
            {
                Query = "test query",
                TotalCount = 100,
                Page = 1,
                Items = new List<SearchItem>
                {
                    new SearchItem
                    {
                        Id = 1,
                        Title = "First Item",
                        Description = Option.Some("This is the first item"),
                        Tags = new List<string> { "tag1", "tag2" },
                        Metadata = Option.Some(new Dictionary<string, object>
                        {
                            ["score"] = 0.95,
                            ["source"] = "database"
                        })
                    },
                    new SearchItem
                    {
                        Id = 2,
                        Title = "Second Item",
                        Description = Option<string>.None,
                        Tags = new List<string> { "tag3" },
                        Metadata = Option<Dictionary<string, object>>.None
                    }
                }
            };

            // Act
            var json = JsonSerializer.Serialize(original, _options);
            var deserialized = JsonSerializer.Deserialize<SearchResults>(json, _options);

            // Assert
            Assert.NotNull(deserialized);
            Assert.Equal(original.Query, deserialized.Query);
            Assert.Equal(original.TotalCount, deserialized.TotalCount);
            Assert.Equal(original.Items.Count, deserialized.Items.Count);
            
            var firstItem = deserialized.Items[0];
            Assert.Equal(1, firstItem.Id);
            Assert.True(firstItem.Description.IsSome);
            Assert.Equal("This is the first item", firstItem.Description.Value);
            Assert.Equal(2, firstItem.Tags.Count);
            
            var secondItem = deserialized.Items[1];
            Assert.True(secondItem.Description.IsNone);
            Assert.True(secondItem.Metadata.IsNone);
        }

        [Fact]
        public void Serialize_OrderProcessingResult_WithAggregateError()
        {
            // Arrange
            var errors = new List<Error>
            {
                new ValidationError("quantity", "Quantity must be positive"),
                new ValidationError("productId", "Product ID is required"),
                new NotFoundError("PRODUCT_NOT_FOUND", "Product with ID 'ABC123' was not found")
            };
            var aggregateError = new AggregateError(errors);
            
            var result = Result.Failure<List<OrderItem>>(aggregateError);

            // Act
            var json = JsonSerializer.Serialize(result, _options);

            // Assert
            Assert.Contains("\"isSuccess\":false", json);
            Assert.Contains("\"$type\":\"AggregateError\"", json);
            Assert.Contains("\"Code\":\"Errors.Aggregate\"", json);
            Assert.Contains("\"Errors\":[", json);
            Assert.Contains("\"$type\":\"ValidationError\"", json);
            Assert.Contains("\"Code\":\"quantity\"", json);
            Assert.Contains("\"Code\":\"productId\"", json);
            Assert.Contains("\"$type\":\"NotFoundError\"", json);
            Assert.Contains("\"Code\":\"PRODUCT_NOT_FOUND\"", json);
        }

        [Fact]
        public void Deserialize_BatchOperationResults_HandlesCorrectly()
        {
            // Arrange
            var json = @"{
                ""Operations"": [
                    {""isSuccess"":true,""value"":{""Id"":1,""Name"":""User1"",""Email"":""user1@example.com"",""Phone"":null,""Age"":25,""LastLogin"":null}},
                    {""isSuccess"":false,""error"":{""Code"":""NOT_FOUND"",""Message"":""User not found""}},
                    {""isSuccess"":true,""value"":{""Id"":3,""Name"":""User3"",""Email"":null,""Phone"":""+123456"",""Age"":null,""LastLogin"":""2023-12-25T10:00:00Z""}}
                ],
                ""ProcessedAt"": ""2023-12-25T12:00:00Z""
            }";

            var options = new JsonSerializerOptions();
            options.Converters.Add(new ResultJsonConverter<UserProfile>());
            options.Converters.Add(new OptionJsonConverter<string>());
            options.Converters.Add(new OptionJsonConverter<int>());
            options.Converters.Add(new OptionJsonConverter<DateTime>());
            options.Converters.Add(new ErrorJsonConverter());

            // Act
            var batch = JsonSerializer.Deserialize<BatchOperationResult>(json, options);

            // Assert
            Assert.NotNull(batch);
            Assert.Equal(3, batch.Operations.Count);
            
            Assert.True(batch.Operations[0].IsSuccess);
            Assert.Equal("User1", batch.Operations[0].Value.Name);
            
            Assert.True(batch.Operations[1].IsFailure);
            Assert.Equal("NOT_FOUND", batch.Operations[1].Error.Code);
            
            Assert.True(batch.Operations[2].IsSuccess);
            Assert.True(batch.Operations[2].Value.Phone.IsSome);
            Assert.Equal("+123456", batch.Operations[2].Value.Phone.Value);
        }

        [Fact]
        public void Serialize_ChainedOperationResult_HandlesComplexScenario()
        {
            // Arrange
            var operation = new ChainedOperation
            {
                Step1 = Result.Success("Step 1 completed"),
                Step2 = Result.Failure<int>(new Error("STEP2_FAIL", "Step 2 failed")),
                Step3 = Option.Some(Result.Success(new List<string> { "result1", "result2" })),
                Step4 = Option<Result<Dictionary<string, object>>>.None
            };

            var options = new JsonSerializerOptions();
            options.Converters.Add(new ResultJsonConverter<string>());
            options.Converters.Add(new ResultJsonConverter<int>());
            options.Converters.Add(new ResultJsonConverter<List<string>>());
            options.Converters.Add(new ResultJsonConverter<Dictionary<string, object>>());
            options.Converters.Add(new OptionJsonConverter<Result<List<string>>>());
            options.Converters.Add(new OptionJsonConverter<Result<Dictionary<string, object>>>());

            // Act
            var json = JsonSerializer.Serialize(operation, options);

            // Assert
            Assert.Contains("\"Step1\":{\"isSuccess\":true,\"value\":\"Step 1 completed\"}", json);
            Assert.Contains("\"Step2\":{\"isSuccess\":false,\"error\":", json);
            Assert.Contains("\"Step3\":{\"isSuccess\":true,\"value\":[\"result1\",\"result2\"]}", json);
            Assert.Contains("\"Step4\":null", json);
        }

        // Test models
        private class UserProfile
        {
            public int Id { get; set; }
            public string Name { get; set; } = "";
            public Option<string> Email { get; set; }
            public Option<string> Phone { get; set; }
            public Option<int> Age { get; set; }
            public Option<DateTime> LastLogin { get; set; }
        }

        private class ApiResponse<T>
        {
            public string RequestId { get; set; } = "";
            public DateTime Timestamp { get; set; }
            public Result<T> Result { get; set; }
        }

        private class SearchItem
        {
            public int Id { get; set; }
            public string Title { get; set; } = "";
            public Option<string> Description { get; set; }
            public List<string> Tags { get; set; } = new();
            public Option<Dictionary<string, object>> Metadata { get; set; }
        }

        private class SearchResults
        {
            public string Query { get; set; } = "";
            public int TotalCount { get; set; }
            public int Page { get; set; }
            public List<SearchItem> Items { get; set; } = new();
        }

        private class OrderItem
        {
            public string ProductId { get; set; } = "";
            public int Quantity { get; set; }
            public decimal Price { get; set; }
        }

        private class BatchOperationResult
        {
            public List<Result<UserProfile>> Operations { get; set; } = new();
            public DateTime ProcessedAt { get; set; }
        }

        private class ChainedOperation
        {
            public Result<string> Step1 { get; set; }
            public Result<int> Step2 { get; set; }
            public Option<Result<List<string>>> Step3 { get; set; }
            public Option<Result<Dictionary<string, object>>> Step4 { get; set; }
        }
    }
}
using FluentUnions.Tests.Errors;

namespace FluentUnions.Tests
{
    public class ErrorTests
    {
        // Tests for ErrorWithMetadata behavior using TestError
        [Fact]
        public void TestError_WithCodeMessageAndMetadata_SetsPropertiesCorrectly()
        {
            // Arrange
            var code = "TEST_CODE";
            var message = "Test message";
            var metadata = new Dictionary<string, object> { { "key1", "value1" }, { "key2", 42 } };

            // Act
            var error = new TestError(code, message, metadata);

            // Assert
            Assert.Equal(code, error.Code);
            Assert.Equal(message, error.Message);
            Assert.Equal(2, error.Metadata.Count);
            Assert.Equal("value1", error.Metadata["key1"]);
            Assert.Equal(42, error.Metadata["key2"]);
        }

        [Fact]
        public void Error_WithCodeAndMessage_SetsPropertiesCorrectly()
        {
            // Arrange
            var code = "TEST_CODE";
            var message = "Test message";

            // Act
            var error = new Error(code, message);

            // Assert
            Assert.Equal(code, error.Code);
            Assert.Equal(message, error.Message);
        }

        [Fact]
        public void TestError_WithCodeAndMessage_HasEmptyMetadata()
        {
            // Arrange
            var code = "TEST_CODE";
            var message = "Test message";

            // Act
            var error = new TestError(code, message);

            // Assert
            Assert.Equal(code, error.Code);
            Assert.Equal(message, error.Message);
            Assert.Empty(error.Metadata);
        }

        [Fact]
        public void Error_WithMessageOnly_SetsPropertiesCorrectly()
        {
            // Arrange
            var message = "Test message";

            // Act
            var error = new Error(message);

            // Assert
            Assert.Equal(string.Empty, error.Code);
            Assert.Equal(message, error.Message);
        }

        [Fact]
        public void TestError_Metadata_IsReadOnly()
        {
            // Arrange
            var metadata = new Dictionary<string, object> { { "key1", "value1" } };
            var error = new TestError("CODE", "message", metadata);

            // Act & Assert
            Assert.IsAssignableFrom<IReadOnlyDictionary<string, object>>(error.Metadata);
        }

        [Fact]
        public void Equals_WithSameError_ReturnsTrue()
        {
            // Arrange
            var error1 = new Error("CODE", "message");
            var error2 = new Error("CODE", "message");

            // Act & Assert
            Assert.True(error1.Equals(error2));
        }

        [Fact]
        public void TestError_Equals_WithSameErrorAndMetadata_ReturnsTrue()
        {
            // Arrange
            var metadata1 = new Dictionary<string, object> { { "key1", "value1" }, { "key2", 42 } };
            var metadata2 = new Dictionary<string, object> { { "key1", "value1" }, { "key2", 42 } };
            var error1 = new TestError("CODE", "message", metadata1);
            var error2 = new TestError("CODE", "message", metadata2);

            // Act & Assert
            Assert.True(error1.Equals(error2));
        }

        [Fact]
        public void Equals_WithDifferentCode_ReturnsFalse()
        {
            // Arrange
            var error1 = new Error("CODE1", "message");
            var error2 = new Error("CODE2", "message");

            // Act & Assert
            Assert.False(error1.Equals(error2));
        }

        [Fact]
        public void Equals_WithDifferentMessage_ReturnsFalse()
        {
            // Arrange
            var error1 = new Error("CODE", "message1");
            var error2 = new Error("CODE", "message2");

            // Act & Assert
            Assert.False(error1.Equals(error2));
        }

        [Fact]
        public void TestError_Equals_WithDifferentMetadata_ReturnsFalse()
        {
            // Arrange
            var metadata1 = new Dictionary<string, object> { { "key1", "value1" } };
            var metadata2 = new Dictionary<string, object> { { "key1", "value2" } };
            var error1 = new TestError("CODE", "message", metadata1);
            var error2 = new TestError("CODE", "message", metadata2);

            // Act & Assert
            Assert.False(error1.Equals(error2));
        }

        [Fact]
        public void TestError_Equals_WithDifferentMetadataKeys_ReturnsFalse()
        {
            // Arrange
            var metadata1 = new Dictionary<string, object> { { "key1", "value1" } };
            var metadata2 = new Dictionary<string, object> { { "key2", "value1" } };
            var error1 = new TestError("CODE", "message", metadata1);
            var error2 = new TestError("CODE", "message", metadata2);

            // Act & Assert
            Assert.False(error1.Equals(error2));
        }

        [Fact]
        public void TestError_Equals_WithDifferentMetadataCount_ReturnsFalse()
        {
            // Arrange
            var metadata1 = new Dictionary<string, object> { { "key1", "value1" } };
            var metadata2 = new Dictionary<string, object> { { "key1", "value1" }, { "key2", "value2" } };
            var error1 = new TestError("CODE", "message", metadata1);
            var error2 = new TestError("CODE", "message", metadata2);

            // Act & Assert
            Assert.False(error1.Equals(error2));
        }

        [Fact]
        public void Equals_WithNull_ReturnsFalse()
        {
            // Arrange
            var error = new Error("CODE", "message");

            // Act & Assert
            Assert.False(error.Equals(null));
        }

        [Fact]
        public void Equals_WithSameReference_ReturnsTrue()
        {
            // Arrange
            var error = new Error("CODE", "message");

            // Act & Assert
            // ReSharper disable once EqualExpressionComparison
            Assert.True(error.Equals(error));
        }

        [Fact]
        public void Equals_WithDifferentType_ReturnsFalse()
        {
            // Arrange
            var error = new Error("CODE", "message");
            var notFoundError = new NotFoundError("CODE", "message");

            // Act & Assert
            Assert.False(error.Equals(notFoundError));
        }

        [Fact]
        public void GetHashCode_WithSameValues_ReturnsSameHashCode()
        {
            // Arrange
            var error1 = new Error("CODE", "message");
            var error2 = new Error("CODE", "message");

            // Act
            var hash1 = error1.GetHashCode();
            var hash2 = error2.GetHashCode();

            // Assert
            Assert.Equal(hash1, hash2);
        }

        [Fact]
        public void TestError_GetHashCode_WithSameValuesAndMetadata_ReturnsSameHashCode()
        {
            // Arrange
            var metadata1 = new Dictionary<string, object> { { "key1", "value1" }, { "key2", 42 } };
            var metadata2 = new Dictionary<string, object> { { "key1", "value1" }, { "key2", 42 } };
            var error1 = new TestError("CODE", "message", metadata1);
            var error2 = new TestError("CODE", "message", metadata2);

            // Act
            var hash1 = error1.GetHashCode();
            var hash2 = error2.GetHashCode();

            // Assert
            Assert.Equal(hash1, hash2);
        }

        [Fact]
        public void GetHashCode_WithDifferentValues_ReturnsDifferentHashCode()
        {
            // Arrange
            var error1 = new Error("CODE1", "message");
            var error2 = new Error("CODE2", "message");

            // Act
            var hash1 = error1.GetHashCode();
            var hash2 = error2.GetHashCode();

            // Assert
            Assert.NotEqual(hash1, hash2);
        }

        [Fact]
        public void ToString_WithEmptyCode_ReturnsCorrectFormat()
        {
            // Arrange
            var error = new Error("", "Test message");

            // Act
            var result = error.ToString();

            // Assert
            Assert.Equal("Error: Test message", result);
        }

        [Fact]
        public void ToString_WithCode_ReturnsCorrectFormat()
        {
            // Arrange
            var error = new Error("TEST_CODE", "Test message");

            // Act
            var result = error.ToString();

            // Assert
            Assert.Equal("Error: TEST_CODE - Test message", result);
        }

        [Fact]
        public void TestError_ToString_WithMetadata_ReturnsCorrectFormat()
        {
            // Arrange
            var metadata = new Dictionary<string, object> { { "key1", "value1" }, { "key2", 42 } };
            var error = new TestError("TEST_CODE", "Test message", metadata);

            // Act
            var result = error.ToString();

            // Assert
            Assert.Contains("TestError: TEST_CODE - Test message", result);
            Assert.Contains("Metadata:", result);
            Assert.Contains("key1: value1", result);
            Assert.Contains("key2: 42", result);
        }

        [Fact]
        public void TestError_ToString_WithEmptyCodeAndMetadata_ReturnsCorrectFormat()
        {
            // Arrange
            var metadata = new Dictionary<string, object> { { "key1", "value1" } };
            var error = new TestError("", "Test message", metadata);

            // Act
            var result = error.ToString();

            // Assert
            Assert.Contains("TestError: Test message", result);
            Assert.Contains("Metadata:", result);
            Assert.Contains("key1: value1", result);
        }

        [Fact]
        public void Error_IsSerializable()
        {
            // Arrange
            var error = new Error("TEST_CODE", "Test message");

            // Act & Assert
#pragma warning disable SYSLIB0050
            Assert.True(error.GetType().IsSerializable);
#pragma warning restore SYSLIB0050
        }

        [Fact]
        public void TestError_Constructor_WithNullMetadata_ThrowsException()
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentNullException>(() => new TestError("CODE", "message", null!));
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("CODE", "")]
        [InlineData("", "message")]
        [InlineData("CODE", "message")]
        public void Error_WithVariousCodeAndMessageCombinations_CreatesError(string code, string message)
        {
            // Act
            var error = new Error(code, message);

            // Assert
            Assert.Equal(code, error.Code);
            Assert.Equal(message, error.Message);
        }
        
        // TODO: Solve adding values problem
        
        // [Fact]
        // public void Metadata_ModifyingOriginalDictionary_DoesNotAffectError()
        // {
        //     // Arrange
        //     var metadata = new Dictionary<string, object> { { "key1", "value1" } };
        //     var error = new Error("CODE", "message", metadata);
        //
        //     // Act
        //     metadata["key2"] = "value2";
        //
        //     // Assert
        //     Assert.Single(error.Metadata);
        //     Assert.False(error.Metadata.ContainsKey("key2"));
        // }
    }
}
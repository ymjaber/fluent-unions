namespace FluentUnions.Tests.ResultTests
{

    public class ValueResultTests
    {
        [Fact]
        public void Success_WithValue_CreatesSuccessResult()
        {
            // Arrange & Act
            var result = Result.Success(42);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.False(result.IsFailure);
            Assert.Equal(42, result.Value);
        }

        [Fact]
        public void Failure_WithError_CreatesFailureResult()
        {
            // Arrange
            var error = new Error("E001", "Something went wrong");

            // Act
            var result = Result.Failure<int>(error);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.True(result.IsFailure);
            Assert.Equal(error, result.Error);
        }

        [Fact]
        public void Value_WhenFailure_ThrowsInvalidOperationException()
        {
            // Arrange
            var result = Result.Failure<int>(new Error("E001", "Error"));

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => result.Value);
            Assert.Equal("Result is not in a success state.", exception.Message);
        }

        [Fact]
        public void Error_WhenSuccess_ThrowsInvalidOperationException()
        {
            // Arrange
            var result = Result.Success(42);

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => result.Error);
            Assert.Equal("Result is not in a failure state.", exception.Message);
        }

        [Fact]
        public void ImplicitOperator_FromValue_CreatesSuccess()
        {
            // Arrange & Act
            Result<int> result = 42;

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(42, result.Value);
        }

        [Fact]
        public void ImplicitOperator_FromError_CreatesFailure()
        {
            // Arrange
            var error = new Error("E001", "Test error");

            // Act
            Result<int> result = error;

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(error, result.Error);
        }

        [Fact]
        public void ExplicitOperator_ToUnitResult_Success()
        {
            // Arrange
            var valueResult = Result.Success(42);

            // Act
            Result unitResult = (Result)valueResult;

            // Assert
            Assert.True(unitResult.IsSuccess);
        }

        [Fact]
        public void ExplicitOperator_ToUnitResult_Failure()
        {
            // Arrange
            var error = new Error("E001", "Test error");
            var valueResult = Result.Failure<int>(error);

            // Act
            Result unitResult = (Result)valueResult;

            // Assert
            Assert.True(unitResult.IsFailure);
            Assert.Equal(error, unitResult.Error);
        }

        [Fact]
        public void TryGetValue_WhenSuccess_ReturnsTrueAndValue()
        {
            // Arrange
            var result = Result.Success(42);

            // Act
            var success = result.TryGetValue(out var value);

            // Assert
            Assert.True(success);
            Assert.Equal(42, value);
        }

        [Fact]
        public void TryGetValue_WhenFailure_ReturnsFalseAndDefault()
        {
            // Arrange
            var result = Result.Failure<int>(new Error("E001", "Error"));

            // Act
            var success = result.TryGetValue(out var value);

            // Assert
            Assert.False(success);
            Assert.Equal(default(int), value);
        }

        [Fact]
        public void TryGetError_WhenFailure_ReturnsTrueAndError()
        {
            // Arrange
            var error = new Error("E001", "Test error");
            var result = Result.Failure<int>(error);

            // Act
            var success = result.TryGetError(out var retrievedError);

            // Assert
            Assert.True(success);
            Assert.Equal(error, retrievedError);
        }

        [Fact]
        public void TryGetError_WhenSuccess_ReturnsFalseAndNull()
        {
            // Arrange
            var result = Result.Success(42);

            // Act
            var success = result.TryGetError(out var error);

            // Assert
            Assert.False(success);
            Assert.Null(error);
        }

        [Fact]
        public void Equals_BothSuccessWithSameValue_ReturnsTrue()
        {
            // Arrange
            var result1 = Result.Success(42);
            var result2 = Result.Success(42);

            // Act & Assert
            Assert.True(result1.Equals(result2));
            Assert.True(result1.IsSuccess && result2.IsSuccess);
            Assert.Equal(result1.Value, result2.Value);
        }

        [Fact]
        public void Equals_BothSuccessWithDifferentValue_ReturnsFalse()
        {
            // Arrange
            var result1 = Result.Success(42);
            var result2 = Result.Success(43);

            // Act & Assert
            Assert.False(result1.Equals(result2));
            Assert.True(result1.IsSuccess && result2.IsSuccess);
            Assert.NotEqual(result1.Value, result2.Value);
        }

        [Fact]
        public void Equals_BothFailureWithSameError_ReturnsTrue()
        {
            // Arrange
            var error = new Error("E001", "Test error");
            var result1 = Result.Failure<int>(error);
            var result2 = Result.Failure<int>(error);

            // Act & Assert
            Assert.True(result1.Equals(result2));
            Assert.True(result1.IsFailure && result2.IsFailure);
            Assert.Equal(result1.Error, result2.Error);
        }

        [Fact]
        public void Equals_SuccessAndFailure_ReturnsFalse()
        {
            // Arrange
            var success = Result.Success(42);
            var failure = Result.Failure<int>(new Error("E001", "Error"));

            // Act & Assert
            Assert.False(success.Equals(failure));
            Assert.False(failure.Equals(success));
        }

        [Fact]
        public void GetHashCode_SameValue_ReturnsSameHash()
        {
            // Arrange
            var result1 = Result.Success(42);
            var result2 = Result.Success(42);

            // Act & Assert
            Assert.Equal(result1.GetHashCode(), result2.GetHashCode());
        }

        [Fact]
        public void ToString_Success_ReturnsFormattedValue()
        {
            // Arrange
            var result = Result.Success(42);

            // Act
            var str = result.ToString();

            // Assert
            Assert.Equal("Success: 42", str);
        }

        [Fact]
        public void ToString_Failure_ReturnsFormattedError()
        {
            // Arrange
            var error = new Error("E001", "Test error");
            var result = Result.Failure<int>(error);

            // Act
            var str = result.ToString();

            // Assert
            Assert.Equal("Failure: Error: E001 - Test error", str);
        }

        [Fact]
        public void Success_WithString_WorksCorrectly()
        {
            // Arrange & Act
            var result = Result.Success("Hello");

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Hello", result.Value);
        }

        [Fact]
        public void Success_WithComplexType_WorksCorrectly()
        {
            // Arrange
            var user = new User { Id = 1, Name = "John" };

            // Act
            var result = Result.Success(user);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Same(user, result.Value);
        }

        [Fact]
        public void Failure_WithDifferentTypes_WorksCorrectly()
        {
            // Arrange
            var error = new ValidationError("age", "Must be positive");

            // Act
            var intResult = Result.Failure<int>(error);
            var stringResult = Result.Failure<string>(error);
            var userResult = Result.Failure<User>(error);

            // Assert
            Assert.True(intResult.IsFailure);
            Assert.True(stringResult.IsFailure);
            Assert.True(userResult.IsFailure);
            Assert.Equal(error, intResult.Error);
            Assert.Equal(error, stringResult.Error);
            Assert.Equal(error, userResult.Error);
        }

        [Fact]
        public void DefaultValue_IsSuccess()
        {
            // Arrange & Act
            var result = default(Result<int>);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.False(result.IsFailure);
            Assert.Equal(0, result.Value);
        }

        private class User
        {
            public int Id { get; set; }
            public string Name { get; set; } = "";
        }
    }
}
